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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/query/parser/QueryPartParser.java
using System;
using org.apache.metamodel.j2n.data;

namespace org.apache.metamodel.core.query.parser
{
    /**
     * Parser of query parts. This parser is aware of parenthesis symbols '(' and
     * ')' and only yields tokens that have balanced parentheses. Delimitors are
     * configurable.
     */
    public sealed class QueryPartParser
    {

        private QueryPartProcessor _processor;
        private String             _clause;
        private String[]           _ItemDelims;

        public QueryPartParser(QueryPartProcessor processor, String clause, params String[] itemDelims)
        {
            if (clause == null)
            {
                throw new ArgumentException("Clause cannot be null");
            }
            if (itemDelims == null || itemDelims.Length == 0)
            {
                throw new ArgumentException("Item delimitors cannot be null or empty");
            }
            _processor  = processor;
            _clause     = clause;
            _ItemDelims = itemDelims;
        } // constructor

        public void parse()
        {
            if (_clause.IsEmpty())
            {
                return;
            }

            int parenthesisCount = 0;
            int offset = 0;
            bool singleOuterParenthesis = _clause[0] == '(' && _clause[_clause.Length - 1] == ')';

            String          previousDelim       = null;
            DelimOccurrence nextDelimOccurrence = getNextDelim(0);

            if (nextDelimOccurrence != null)
            {
                for (int i = 0; i < _clause.Length; i++)
                {
                    char c = _clause[i];
                    if (c == '(')
                    {
                        parenthesisCount++;
                    }
                    else if (c == ')')
                    {
                        parenthesisCount--;
                        if (singleOuterParenthesis && parenthesisCount == 0 && i != _clause.Length - 1)
                        {
                            singleOuterParenthesis = false;
                        }
                    }
                    if (i == nextDelimOccurrence.index)
                    {
                        if (parenthesisCount == 0)
                        {
                            // token bounds has been identified
                            String itemToken = _clause.Substring(offset, i);
                            parseItem(previousDelim, itemToken);
                            offset = i + nextDelimOccurrence.delim.Length;
                            previousDelim = nextDelimOccurrence.delim;
                        }
                        nextDelimOccurrence = getNextDelim(nextDelimOccurrence.index + 1);
                        if (nextDelimOccurrence == null)
                        {
                            break;
                        }
                    }
                }
            }

            if (singleOuterParenthesis)
            {
                String newClause = _clause.Substring(1, _clause.Length - 1);
                // re-run based on new clause
                QueryPartParser newParser = new QueryPartParser(_processor, newClause, _ItemDelims);
                newParser.parse();
                return;
            }

            // last token will occur outside loop
            if (offset != _clause.Length)
            {
                String token = _clause.Substring(offset);
                parseItem(previousDelim, token);
            }
        }

        private /*static*/ class DelimOccurrence
        {
            public int    index;
            public String delim;
        }

        private DelimOccurrence getNextDelim(int offset)
        {
            DelimOccurrence result = null;
            for (int i = 0; i < _ItemDelims.Length; i++)
            {
                String delim = _ItemDelims[i];
                int index    = _clause.ToUpper().IndexOf(delim, offset);
                if (index != -1)
                {
                    if (result == null || index == Math.Min(result.index, index))
                    {
                        result = new DelimOccurrence();
                        result.index = index;
                        result.delim = delim;
                    }
                }
            }
            return result;
        } // getNextDelim()

        private void parseItem(string delim, string token)
        {
            if (token != null)
            {
                token = token.Trim();
                if (! token.IsEmpty())
                {
                    _processor.parse(delim, token);
                }
            }
        }
    } // QueryPartParser class
} // org.apache.metamodel.core.query.parser namespace
