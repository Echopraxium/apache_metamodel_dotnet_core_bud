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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/query/parser/SelectItemParser.java
using org.apache.metamodel.query;
using org.apache.metamodel.schema;
using System;

namespace org.apache.metamodel.core.query.parser
{
    public sealed class SelectItemParser : QueryPartProcessor
    {
        public class MultipleSelectItemsParsedException : ArgumentException
        {
            private static readonly long serialVersionUID = 1L;

            private FromItem _fromItem;

            public MultipleSelectItemsParsedException(FromItem fromItem)
            {
                _fromItem = fromItem;
            }

            public FromItem getFromItem()
            {
                return _fromItem;
            }
        } // MultipleSelectItemsParsedException class

        private Query _query;
        private bool  _allowExpressionBasedSelectItems;

        public SelectItemParser(Query query, bool allowExpressionBasedSelectItems)
        {
            _query = query;
            _allowExpressionBasedSelectItems = allowExpressionBasedSelectItems;
        } // constructor

        // Override
        public void parse(String delim, String itemToken) // throws MetaModelException
        {
            if ("*".Equals(itemToken))
            {
              _query.selectAll();
              return;
            }

            String alias = null;
            int indexOfAlias = itemToken.ToUpper().LastIndexOf(" AS ");
            if (indexOfAlias != -1)
            {
                alias = itemToken.Substring(indexOfAlias + " AS ".Length);
                itemToken = itemToken.Substring(0, indexOfAlias).Trim();
            }

            try
            {
                 SelectItem selectItem = findSelectItem(itemToken);
                if (selectItem == null)
                {
                    throw new QueryParserException("Not capable of parsing SELECT token: " + itemToken);
                }

                if (alias != null)
                {
                    selectItem.setAlias(alias);
                }

                _query.select(selectItem);
            }
            catch (MultipleSelectItemsParsedException e)
            {
                FromItem fromItem = e.getFromItem();
                if (fromItem != null)
                {
                    _query.selectAll(fromItem);
                }
                else
                {
                    throw e;
                }
            }
        } // parse()

        /**
         * Finds/creates a SelectItem based on the given expression. Unlike the
         * {@link #parse(String, String)} method, this method will not actually add
         * the selectitem to the query.
         * 
         * @param expression
         * @return
         * 
         * @throws MultipleSelectItemsParsedException
         *             if an expression yielding multiple select-items (such as "*")
         *             was passed in the expression
         */
        public SelectItem findSelectItem(String expression) // throws MultipleSelectItemsParsedException
        {
            if ("*".Equals(expression))
            {
                throw new MultipleSelectItemsParsedException(null);
            }

            if ("COUNT(*)".Equals(expression, StringComparison.CurrentCultureIgnoreCase))
            {
                return SelectItem.getCountAllItem();
            }

            String unmodifiedExpression = expression;

            bool         functionApproximation;
            FunctionType function;

            int startParenthesis = expression.IndexOf('(');
            if (startParenthesis > 0 && expression.EndsWith(")"))
            {
                functionApproximation = (expression.StartsWith(SelectItem.FUNCTION_APPROXIMATION_PREFIX));
                String functionName = expression.Substring(
                        (functionApproximation ? SelectItem.FUNCTION_APPROXIMATION_PREFIX.Length : 0), startParenthesis);
                function = FunctionTypeFactory.get(functionName.ToUpper());
                if (function != null)
                {
                    expression = expression.Substring(startParenthesis + 1, expression.Length - 1).Trim();
                    if (function is CountAggregateFunction && "*".Equals(expression)) 
                    {
                        SelectItem select_item = SelectItem.getCountAllItem();
                        select_item.setFunctionApproximationAllowed(functionApproximation);
                        return select_item;
                    }
                }
            }
            else
            {
                function              = null;
                functionApproximation = false;
            }

            String   columnName = null;
            FromItem fromItem   = null;

            // attempt to find from item by cutting up the string in prefix and
            // suffix around dot.
            {
                int splitIndex = expression.LastIndexOf('.');
                while (fromItem == null && splitIndex != -1)
                {
                    String prefix = expression.Substring(0, splitIndex);
                    columnName = expression.Substring(splitIndex + 1);
                    fromItem = _query.getFromClause().getItemByReference(prefix);

                    splitIndex = expression.LastIndexOf('.', splitIndex - 1);
                }
            }

            if (fromItem == null)
            {
                if (_query.getFromClause().getItemCount() == 1)
                {
                    fromItem = _query.getFromClause().getItem(0);
                    columnName = expression;
                }
                else
                {
                    fromItem = null;
                    columnName = null;
                }
            }

            if (fromItem != null)
            {
                if ("*".Equals(columnName))
                {
                    throw new MultipleSelectItemsParsedException(fromItem);
                }
                else if (fromItem.getTable() != null)
                {
                    Column column = fromItem.getTable().getColumnByName(columnName);
                    int offset = -1;
                    while (function == null && column == null)
                    {
                        // check for MAP_VALUE shortcut syntax
                        offset = columnName.IndexOf('.', offset + 1);
                        if (offset == -1)
                        {
                            break;
                        }

                        String part1 = columnName.Substring(0, offset);
                        column = fromItem.getTable().getColumnByName(part1);
                        if (column != null)
                        {
                            String part2 = columnName.Substring(offset + 1);
                            return new SelectItem(new MapValueFunction(), new Object[] { part2 }, column, fromItem);
                        }
                    }

                    if (column != null)
                    {
                        SelectItem select_item = new SelectItem(function, column, fromItem);
                        select_item.setFunctionApproximationAllowed(functionApproximation);
                        return select_item;
                    }
                }
                else if (fromItem.getSubQuery() != null)
                {
                    Query subQuery = fromItem.getSubQuery();
                    SelectItem subQuerySelectItem = new SelectItemParser(subQuery, _allowExpressionBasedSelectItems)
                                                                         .findSelectItem(columnName);
                    if (subQuerySelectItem == null)
                    {
                        return null;
                    }
                    return new SelectItem(subQuerySelectItem, fromItem);
                }
            }

            // if the expression is alias of some select item defined return that
            // select item
            SelectItem aliasSelectItem = MetaModelHelper.getSelectItemByAlias(_query, unmodifiedExpression);
            if (aliasSelectItem != null)
            {
                return aliasSelectItem;
            }

            if (_allowExpressionBasedSelectItems)
            {
                SelectItem select_item = new SelectItem(function, expression, null);
                select_item.setFunctionApproximationAllowed(functionApproximation);
                return select_item;
            }
            return null;
        } // findSelectItem()
    }
}
