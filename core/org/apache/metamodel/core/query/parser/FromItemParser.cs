
/**
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */
using org.apache.metamodel.j2n.data;
using org.apache.metamodel.j2n.slf4j;
using org.apache.metamodel.query;
using org.apache.metamodel.schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.apache.metamodel.core.query.parser
{
    public sealed class FromItemParser : QueryPartProcessor
    {

        private static readonly NLogger logger = NLoggerFactory.getLogger(typeof(FromItemParser).Name);

        private Query       _query;
        private DataContext _dataContext;

        /**
         * This field will hold start and end character for delimiter that can be
         * used
         */
        private static readonly Dictionary<Char, Char> delimiterMap = delimiterMapInit();

        private static Dictionary<Char, Char>  delimiterMapInit()
        {
            Dictionary<Char, Char> values = new Dictionary<Char, Char>();
            values.Add('\"', '\"');
            values.Add('[', ']');
            return values;
        }

        public FromItemParser(DataContext dataContext, Query query)
        {
            _dataContext = dataContext;
            _query = query;
        } // constructor

        // @Override
        public void parse(String delim, String itemToken)
        {
            FromItem fromItem;

            int parenthesisStart = itemToken.IndexOf('(');
            if (parenthesisStart != -1)
            {
                if (parenthesisStart != 0)
                {
                    throw new QueryParserException("Not capable of parsing FROM token: " + itemToken
                            + ". Expected parenthesis to start at first character.");
                }
                int parenthesisEnd = itemToken.IndexOf(')', parenthesisStart);
                if (parenthesisEnd == -1)
                {
                    throw new QueryParserException("Not capable of parsing FROM token: " + itemToken
                            + ". Expected end parenthesis.");
                }

                String subQueryString = itemToken.Substring(parenthesisStart + 1, parenthesisEnd);
                logger.debug("Parsing sub-query: {}", subQueryString);

                Query subQuery = new QueryParser(_dataContext, subQueryString).parse();
                fromItem = new FromItem(subQuery);

                string alias = itemToken.Substring(parenthesisEnd + 1).Trim();
                if (! alias.IsEmpty())
                {
                    fromItem.setAlias(alias);
                }
            }
            else if (itemToken.ToUpper().IndexOf(" JOIN ") != -1)
            {
                fromItem = parseAllJoinItems(itemToken);
            }
            else
            {
                fromItem = parseTableItem(itemToken);
            }

            _query.from(fromItem);
        } // parse()

        private FromItem parseTableItem(String itemToken)
        {
            // From token can be starting with [
            String tableNameToken;
            String aliasToken;

            char startDelimiter = itemToken.Trim()[0];
            if (delimiterMap.ContainsKey(startDelimiter))
            {
                char endDelimiter = delimiterMap[startDelimiter];
                int  endIndex     = itemToken.Trim().LastIndexOf(endDelimiter, itemToken.Trim().Length);
                if (endIndex <= 0)
                {
                    throw new QueryParserException("Not capable of parsing FROM token: " + itemToken + ". Expected end "
                            + endDelimiter);
                }
                tableNameToken = itemToken.Trim().Substring(1, endIndex).Trim();

                if (itemToken.Trim().Substring(1 + endIndex).Trim().Equals("", StringComparison.CurrentCultureIgnoreCase))
                {
                    /*
                     * As per code in FromClause Method: getItemByReference(FromItem
                     * item, String reference) if (alias == null && table != null &&
                     * reference.equals(table.getName())) { Either we have to change
                     * the code to add alias.equals("") there or return null here.
                     */
                    aliasToken = null;
                }
                else
                {
                    aliasToken = itemToken.Trim().Substring(1 + endIndex).Trim();
                }

            }
            else
            {
                // Default assumption is space being delimiter for tablename and
                // alias.. If otherwise use DoubleQuotes or [] around tableName
                String[] tokens = itemToken.Split(' ');
                tableNameToken = tokens[0];
                if (tokens.Length == 2)
                {
                    aliasToken = tokens[1];
                }
                else if (tokens.Length == 1)
                {
                    aliasToken = null;
                }
                else
                {
                    throw new QueryParserException("Not capable of parsing FROM token: " + itemToken);
                }
            }

            Table table = _dataContext.getTableByQualifiedLabel(tableNameToken);
            if (table == null)
            {
                throw new QueryParserException("Not capable of parsing FROM token: " + itemToken);
            }

            FromItem result = new FromItem(table);
            result.setAlias(aliasToken);
            result.setQuery(_query);
            return result;
        } // parseTableItem()

        private FromItem parseAllJoinItems(String itemToken)
        {
            String[]     separators = { "(?i) JOIN " };
            String[]     joinSplit = itemToken.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            List<String> joinsList = new List<String>();
            for (int i = 0; i < joinSplit.Length - 1; i++)
            {
                joinSplit[i] = joinSplit[i].Trim();
                joinSplit[i + 1] = joinSplit[i + 1].Trim();
                String leftPart = joinSplit[i].Substring(0, joinSplit[i].LastIndexOf(" "));
                String joinType = joinSplit[i].Substring(joinSplit[i].LastIndexOf(" "));
                String rightPart = (i + 1 == joinSplit.Length - 1) ? 
                                   joinSplit[i + 1] : 
                                   joinSplit[i + 1].Substring(0, joinSplit[i + 1].LastIndexOf(" "));
                joinsList.Add((leftPart + " " + joinType + " JOIN " + rightPart).Replace(" +", " "));
                String rightTable = rightPart.Substring(0, rightPart.ToUpper().LastIndexOf(" ON "));
                String nextJoinType = joinSplit[i + 1].Substring(joinSplit[i + 1].LastIndexOf(" "));
                joinSplit[i + 1] = rightTable + " " + nextJoinType;
            }
            HashSet<FromItem> fromItems = new HashSet<FromItem>();
            FromItem leftFromItem = null;
            foreach (String token in joinsList)
            {
                leftFromItem = parseJoinItem(leftFromItem, token, fromItems);
            }
            return leftFromItem;
        } // parseAllJoinItems()

        // this method will be documented based on this example itemToken: FOO f
        // INNER JOIN BAR b ON f.id = b.id
        private FromItem parseJoinItem(FromItem leftFromItem, String itemToken, HashSet<FromItem> fromItems)
        {
            int indexOfJoin = itemToken.ToUpper().IndexOf(" JOIN ");

            // firstPart = "FOO f INNER"
            String firstPart = itemToken.Substring(0, indexOfJoin).Trim();

            // secondPart = "BAR b ON f.id = b.id"
            String secondPart = itemToken.Substring(indexOfJoin + " JOIN ".Length).Trim();

            int indexOfJoinType = firstPart.LastIndexOf(" ");

            // joinTypeString = "INNER"
            String   joinTypeString = firstPart.Substring(indexOfJoinType).Trim().ToUpper();
            JoinType joinType       = (JoinType)Enum.Parse(typeof(JoinType), joinTypeString);

            // firstTableToken = "FOO f"
            String firstTableToken = firstPart.Substring(0, indexOfJoinType).Trim();

            int indexOfOn = secondPart.ToUpper().IndexOf(" ON ");

            // secondTableToken = "BAR b"
            String secondTableToken = secondPart.Substring(0, indexOfOn).Trim();

            FromItem leftSide  = parseTableItem(firstTableToken);
            FromItem rightSide = parseTableItem(secondTableToken);

            fromItems.Add(leftSide);
            fromItems.Add(rightSide);

            // onClausess = ["f.id = b.id"]
            String[]     separators = { " AND " };
            String[]     onClauses  = secondPart.Substring(indexOfOn + " ON ".Length).Split(separators, StringSplitOptions.RemoveEmptyEntries);
            SelectItem[] leftOn     = new SelectItem[onClauses.Length];
            SelectItem[] rightOn    = new SelectItem[onClauses.Length];
            for (int i = 0; i < onClauses.Length; i++)
            {
                String onClause      = onClauses[i];
                int    indexOfEquals = onClause.IndexOf("=");
                // leftPart = "f.id"
                String leftPart = onClause.Substring(0, indexOfEquals).Trim();
                // rightPart = "b.id"
                String rightPart = onClause.Substring(indexOfEquals + 1).Trim();

                FromItem[] items = new FromItem[fromItems.Count];
                fromItems.CopyTo(items);
                leftOn[i]  = findSelectItem(leftPart, items);
                rightOn[i] = findSelectItem(rightPart, items); 
            }

            FromItem leftItem = (leftFromItem != null) ? leftFromItem : leftSide;
            FromItem result = new FromItem(joinType, leftItem, rightSide, leftOn, rightOn);
            result.setQuery(_query);
            return result;
        } // parseJoinItem()

        private SelectItem findSelectItem(String token, FromItem[] joinTables)
        {
            // first look in the original query
            SelectItemParser selectItemParser = new SelectItemParser(_query, false);
            SelectItem       result           = selectItemParser.findSelectItem(token);

            if (result == null)
            {
                // fail over and try with the from items available in the join that
                // is being built.
                Query temporaryQuery = new Query().from(joinTables);
                selectItemParser = new SelectItemParser(temporaryQuery, false);
                result = selectItemParser.findSelectItem(token);
                if (result == null)
                {
                    throw new QueryParserException("Not capable of parsing ON token: " + token);
                }

                // set the query on the involved query parts (since they have been
                // temporarily moved to the searched query).
                result.setQuery(_query);
            }
            return result;
        } // findSelectItem()
}
} // org.apache.metamodel.core.query.parser namespace
