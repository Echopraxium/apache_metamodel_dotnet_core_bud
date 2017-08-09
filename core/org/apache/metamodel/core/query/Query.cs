/**
* Licensed to the Apache Software Foundation (ASF) under one
* or more contributor license agreements. See the NOTICE file
* distributed with this work for additional information
* regarding copyright ownership. The ASF licenses this file
* to you under the Apache License, Version 2.0 (the
* "License"); you may not use this file except in compliance
* with the License. You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing,
* software distributed under the License is distributed on an
* "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
* KIND, either express or implied. See the License for the
* specific language governing permissions and limitations
* under the License.
*/
// https://github.com/apache/metamodel/blob/master/core/src/main/java/org/apache/metamodel/query/Query.java
using org.apache.metamodel.query;
using org.apache.metamodel.util;
using org.apache.metamodel.j2n.data;
using org.apache.metamodel.schema;
using System;
using System.Collections.Generic;
using org.apache.metamodel.j2n.data.numbers;
using org.apache.metamodel.j2n.collections;
using org.apache.metamodel.core.query;
using org.apache.metamodel.j2n;
using System.Text;
using org.apache.metamodel.core.query.parser;

namespace org.apache.metamodel.query
{
    /**
     * Represents a query to retrieve data by. A query is made up of six clauses,
     * equivalent to the SQL standard:
     * <ul>
     * <li>the SELECT clause, which define the wanted columns of the resulting
     * DataSet</li>
     * <li>the FROM clause, which define where to retrieve the data from</li>
     * <li>the WHERE clause, which define filters on the retrieved data</li>
     * <li>the GROUP BY clause, which define if the result should be grouped and
     * aggregated according to some columns acting as categories</li>
     * <li>the HAVING clause, which define filters on the grouped data</li>
     * <li>the ORDER BY clause, which define sorting of the resulting dataset</li>
     * </ul>
     * 
     * In addition two properties are applied to queries to limit the resulting
     * dataset:
     * <ul>
     * <li>First row: The first row (aka. offset) of the result of the query.</li>
     * <li>Max rows: The maximum amount of rows to return when executing the query.</li>
     * </ul>
     * 
     * Queries are executed using the DataContext.executeQuery method or can
     * alternatively be used directly in JDBC by using the toString() method.
     * 
     * @see DataContext
     */
    public class Query : BaseObject, NCloneable // ISerializable
    {
        private static readonly long serialVersionUID = -5976325207498574216L;

        private SelectClause               _selectClause;
        private FromClause                 _fromClause;
        private FilterClause               _whereClause;
        private /*Version*/ GroupByClause  _groupByClause;
        private FilterClause               _havingClause;
        private OrderByClause              _orderByClause;

        private NInteger _maxRows;
        private NInteger _firstRow;

        public Query()
        {
            _selectClause   = new SelectClause(this);
            _fromClause     = new FromClause(this);
            _whereClause    = new FilterClause(this, AbstractQueryClause<FilterItem>.PREFIX_WHERE);
            _groupByClause  = new GroupByClause(this);
            _havingClause   = new FilterClause(this, AbstractQueryClause<FilterItem>.PREFIX_HAVING);
            _orderByClause  = new OrderByClause(this);
        } // constructor

        public Query having(params string[] havingItemTokens)
        {
            foreach (string havingItemToken in havingItemTokens)
            {
                FilterItem filterItem = findFilterItem(havingItemToken);
                having(filterItem);
            }
            return this;
        } // having()

        /**
         * Gets the first row (aka offset) of the query's result, or null if none is
         * specified. The row number is 1-based, so setting a first row value of 1
         * is equivalent to not setting it at all..
         * 
         * @return the first row (aka offset) of the query's result, or null if no
         *         offset is specified.
         */
        public int getFirstRow()
        {
            return _firstRow;
        } // getFirstRow()()

        /**
         * Sets the first row (aka offset) of the query's result. The row number is
         * 1-based, so setting a first row value of 1 is equivalent to not setting
         * it at all..
         * 
         * @param firstRow
         *            the first row, where 1 is the first row.
         * @return this query
         */
        public Query setFirstRow(NInteger firstRow)
        {
            if (firstRow != null && firstRow < 1)
            {
                //[J2N] IllegalArgumentException <=> ArgumentException
                throw new ArgumentException("First row cannot be negative or zero");
            }
            _firstRow = firstRow;
            return this;
        }

        public Query clone()
        {
            Query q = new Query();
            q.setMaxRows(_maxRows);
            q.setFirstRow(_firstRow);
            q.getSelectClause().setDistinct(_selectClause.isDistinct());
            foreach (FromItem item in _fromClause.getItems())
            {
                q.from(item.clone());
            }
            foreach (SelectItem item in _selectClause.getItems())
            {
                q.select(item.clone(q));
            }
            foreach (FilterItem item in _whereClause.getItems())
            {
                q.where(item.clone());
            }
            foreach (GroupByItem item in _groupByClause.getItems())
            {
                q.groupBy(item.clone());
            }
            foreach (FilterItem item in _havingClause.getItems())
            {
                q.having(item.clone());
            }
            foreach (OrderByItem item in _orderByClause.getItems())
            {
                q.orderBy(item.clone());
            }
            return q;
        } // clone()

        /*
         * A string representation of this query. This representation will be SQL 99
         * compatible and can thus be used for database queries on databases that
         * meet SQL standards.
         */
        public string toSql()
        {
            return toSql(false);
        } // toSql()

        public override void decorateIdentity(NList<Object> identifiers)
        {
            identifiers.Add(_maxRows);
            identifiers.Add(_selectClause);
            identifiers.Add(_fromClause);
            identifiers.Add(_whereClause);
            identifiers.Add(_groupByClause);
            identifiers.Add(_havingClause);
            identifiers.Add(_orderByClause);
        } // decorateIdentity()

        /**
         * @return the number of maximum rows to yield from executing this query or
         *         null if no maximum/limit is set.
         */
        public int getMaxRows()
        {
            return _maxRows;
        }


        /**
         * Sets the maximum number of rows to be queried. If the result of the query
         * yields more rows they should be discarded.
         * 
         * @param maxRows
         *            the number of desired maximum rows. Can be null (default) for
         *            no limits
         * @return this query
         */
        public Query setMaxRows(NInteger maxRows)
        {
            if (maxRows != null)
            {
                int maxRowsValue = maxRows;
                if (maxRowsValue < 0)
                {
                    throw new ArgumentException("Max rows cannot be negative");
                }
            }
            _maxRows = maxRows;
            return this;
        } // setMaxRows()

        public string toSql(bool? includeSchemaInColumnPaths)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_selectClause.toSql(includeSchemaInColumnPaths));
            sb.Append(_fromClause.toSql(includeSchemaInColumnPaths));
            sb.Append(_whereClause.toSql(includeSchemaInColumnPaths));
            sb.Append(_groupByClause.toSql(includeSchemaInColumnPaths));
            sb.Append(_havingClause.toSql(includeSchemaInColumnPaths));
            sb.Append(_orderByClause.toSql(includeSchemaInColumnPaths));
            return sb.ToString();
        } // toSql()

        public SelectClause getSelectClause()
        {
            return _selectClause;
        } // getSelectClause()

        public FromClause getFromClause()
        {
            return _fromClause;
        }

        public FilterClause getWhereClause()
        {
            return _whereClause;
        }

        public GroupByClause getGroupByClause()
        {
            return _groupByClause;
        }

        public FilterClause getHavingClause()
        {
            return _havingClause;
        }

        public OrderByClause getOrderByClause()
        {
            return _orderByClause;
        }

        public override string ToString()
        {
            return toSql();
        } // ToString()

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public Query having(params FilterItem[] items)
        {
            _havingClause.addItems(items);
            return this;
        } // having()

        public Query having(FunctionType function, Column column, OperatorType operatorType, object operand)
        {
            SelectItem selectItem = new SelectItem(function, column);
            return having(new FilterItem(selectItem, operatorType, operand));
        }  // having()

        public Query having(Column column, OperatorType operatorType, object operand)
        {
            SelectItem selectItem = _selectClause.getSelectItem(column);
            if (selectItem == null)
            {
                selectItem = new SelectItem(column);
            }
            return having(new FilterItem(selectItem, operatorType, operand));
        } // having()

        public Query select(Column column, FromItem fromItem)
        {
            SelectItem selectItem = new SelectItem(column, fromItem);
            return select(selectItem);
        } // select()

        public Query select(params Column[] columns)
        {
            foreach (Column column in columns)
            {
                SelectItem selectItem = new SelectItem(column);
                selectItem.setQuery(this);
                _selectClause.addItem(selectItem);
            }
            return this;
        } // select()

        public Query select(params SelectItem[] items)
        {
            _selectClause.addItems(items);
            return this;
        } // select()

        public Query select(FunctionType functionType, Column column)
        {
            _selectClause.addItem(new SelectItem(functionType, column));
            return this;
        } // select()

        public Query select(string expression, string alias)
        {
            return select(new SelectItem(expression, alias));
        } // select()

        ///**
        // * Adds a selection to this query.
        // * 
        // * @param expression
        // * @return
        // */
        public Query select(string expression)
        {
            return select(expression, false);
        } // select()

        public Query where(SelectItem selectItem, OperatorType operatorType, object operand)
        {
            return where(new FilterItem(selectItem, operatorType, operand));
        } // where()

        public Query where(Column column, OperatorType operatorType, object operand)
        {
            SelectItem selectItem = _selectClause.getSelectItem(column);
            if (selectItem == null)
            {
                selectItem = new SelectItem(column);
            }
            return where(selectItem, operatorType, operand);
        } // where()

        public Query where(params FilterItem[] items)
        {
            _whereClause.addItems(items);
            return this;
        } // where()

        public Query where(IEnumerable<FilterItem> items)
        {
            _whereClause.addItems(items);
            return this;
        } // where()

        public Query where(params String[] whereItemTokens)
        {
            foreach (string whereItemToken in whereItemTokens)
            {
                FilterItem filterItem = findFilterItem(whereItemToken);
                where(filterItem);
            }
            return this;
        } // where()

        ///**
        // * Adds a selection to this query.
        // * 
        // * @param expression
        // *            a textual representation of the select item, e.g. "MAX(foo)"
        // *            or just "foo", where "foo" is a column name.
        // * @param allowExpressionBasedSelectItem
        // *            whether or not expression-based select items are allowed or
        // *            not (see {@link SelectItem#getExpression()}.
        // * @return
        // */
        public Query select(string expression, bool allowExpressionBasedSelectItem)
        {
            QueryPartParser clauseParser = new QueryPartParser(
                                               new SelectItemParser(this, allowExpressionBasedSelectItem), expression, ",");
            clauseParser.parse();
            return this;
        } // select()

        protected object createOperand(string token, SelectItem leftSelectItem, bool searchSelectItems)
        {
            if (token.Equals("NULL", StringComparison.CurrentCultureIgnoreCase))
            {
                return null;
            }

            if (token.StartsWith("'") && token.EndsWith("'") && token.Length > 2)
            {
                string stringOperand = token.Substring(1, token.Length - 1);
                //[J2N] replaceAll <=>  Replace
                stringOperand = stringOperand.Replace("\\\\'", "'");
                return stringOperand;
            }

            if (searchSelectItems)
            {
                SelectItem selectItem = findSelectItem(token, false);
                if (selectItem != null)
                {
                    return selectItem;
                }
            }

            ColumnType expectedColumnType = leftSelectItem.getExpectedColumnType();
            object result = null;
            if (expectedColumnType == null)
            {
                // We're assuming number here, but it could also be boolean or a
                // time based type. But anyways, this should not happen since
                // expected column type should be available.
                result = NumberComparator.toNumber(token);
            }
            else if (expectedColumnType.isBoolean())
            {
                result = BooleanComparator.toBoolean(token);
            }
            //    else if (expectedColumnType.isTimeBased())
            //    {
            //        result = FormatHelper.parseSqlTime(expectedColumnType, token);
            //    }
            //    else
            //    {
            //        result = NumberComparator.toNumber(token);
            //    }

            //    if (result == null)
            //    {
            //        // shouldn't happen since only "NULL" is parsed as null.
            //        throw new QueryParserException("Could not parse operand: " + token);
            //    }

            return result;
        } // createOperand()

        private SelectItem findSelectItem(string expression, bool allowExpressionBasedSelectItem)
        {
            SelectItemParser parser = new SelectItemParser(this, allowExpressionBasedSelectItem);
            return parser.findSelectItem(expression);
        } // findSelectItem()

        ///**
        // * Select all available select items from all currently available FROM
        // * items. Equivalent of the expression "SELECT * FROM ..." in SQL.
        // * 
        // * @return
        // */
        public Query selectAll()
        {
            List<FromItem> items = getFromClause().getItems();
            foreach (FromItem fromItem in items)
            {
                selectAll(fromItem);
            }
            return this;
        } // selectAll()

        public Query selectAll(FromItem fromItem)
        {
            if (fromItem.getTable() != null)
            {
                Column[] columns = fromItem.getTable().getColumns();
                foreach (Column column in columns)
                {
                    select(column, fromItem);
                }
            }
            else if (fromItem.getJoin() != JoinType.None)
            {
                selectAll(fromItem.getLeftSide());
                selectAll(fromItem.getRightSide());
            }
            else if (fromItem.getSubQuery() != null)
            {
                List<SelectItem> items = fromItem.getSubQuery().getSelectClause().getItems();
                foreach (SelectItem subQuerySelectItem in items)
              {
                    select(new SelectItem(subQuerySelectItem, fromItem));
                }
            }
            else
            {
                throw new MetaModelException("All select items ('*') not determinable with from item: " + fromItem);
            }
            return this;
        } // selectAll()

        public Query selectDistinct()
        {
            _selectClause.setDistinct(true);
            return this;
        }

        public Query selectCount()
        {
            return select(SelectItem.getCountAllItem());
        }

        public Query from(params FromItem[] items)
        {
            _fromClause.addItems(items);
            return this;
        } // from()

        public Query from(Table table)
        {
            return from(new FromItem(table));
        } // from()

        public Query from(string expression)
        {
            return from(new FromItem(expression));
        } // from()

        public Query from(Table table, string alias)
        {
            return from(new FromItem(table).setAlias(alias));
        } // from()

        public Query from(Table leftTable, Table rightTable, JoinType joinType, Column leftOnColumn, Column rightOnColumn)
        {
            SelectItem[] leftOn   = new SelectItem[] { new SelectItem(leftOnColumn) };
            SelectItem[] rightOn  = new SelectItem[] { new SelectItem(rightOnColumn) };
            FromItem     fromItem = new FromItem(joinType, new FromItem(leftTable), new FromItem(rightTable), leftOn, rightOn);
            return from(fromItem);
        } // from()

        public Query groupBy(params String[] groupByTokens)
        {
            foreach (string groupByToken in groupByTokens)
            {
                SelectItem selectItem = findSelectItem(groupByToken, true);
                groupBy(new GroupByItem(selectItem));
            }
            return this;
        } // groupBy()

        public Query groupBy(params GroupByItem[] items)
        {
            foreach (GroupByItem item in items)
            {
                SelectItem selectItem = item.getSelectItem();
                if (selectItem != null && selectItem.getQuery() == null)
                {
                    selectItem.setQuery(this);
                }
            }
            _groupByClause.addItems(items);
            return this;
        } // groupBy()

        public Query groupBy(params Column [] columns)
        {
            foreach (Column column in columns)
            {
                SelectItem selectItem = (SelectItem) new SelectItem(column).setQuery(this);
                _groupByClause.addItem(new GroupByItem(selectItem));
            }
            return this;
        } // groupBy()

        public Query orderBy(params OrderByItem[] items)
        {
            _orderByClause.addItems(items);
            return this;
        } // orderBy()

        public Query orderBy(params String[] orderByTokens)
        {
            foreach (string orderByToken_item in orderByTokens)
            {
                String orderByToken = orderByToken_item.Trim();
                OrderByItem.Direction direction;
                if (orderByToken.ToUpper().EndsWith("DESC"))
                {
                    direction = OrderByItem.Direction.DESC;
                    orderByToken = orderByToken.Substring(0, orderByToken.Length - 4).Trim();
                }
                else if (orderByToken.ToUpper().EndsWith("ASC"))
                {
                    direction = OrderByItem.Direction.ASC;
                    orderByToken = orderByToken.Substring(0, orderByToken.Length - 3).Trim();
                }
                else
                {
                    direction = OrderByItem.Direction.ASC;
                }

                OrderByItem orderByItem = new OrderByItem(findSelectItem(orderByToken, true), direction);
                orderBy(orderByItem);
            }
            return this;
        } // orderBy()

        public Query orderBy(Column column)
        {
            return orderBy(column, OrderByItem.Direction.ASC);
        } // orderBy()

        ///**
        // * @deprecated use orderBy(Column, Direction) instead
        // */
        //[System.Obsolete]
        public Query orderBy(Column column, bool ascending)
        {
            if (ascending)
            {
               return orderBy(column, OrderByItem.Direction.ASC);
            }
            else
            {
                return orderBy(column, OrderByItem.Direction.DESC);
            }
        } // orderBy()

        public Query orderBy(Column column, OrderByItem.Direction direction)
        {
            SelectItem selectItem = _selectClause.getSelectItem(column);
            if (selectItem == null)
            {
                selectItem = new SelectItem(column);
            }
            return orderBy(new OrderByItem(selectItem, direction));
        } // orderBy()

        private class _QueryPartProcessor_FindFilterItem_impl_ : QueryPartProcessor
        {
            private Query        _query;
            private List<Object> _list;
            private Object       _operand;
            private SelectItem   _select_item;

            public _QueryPartProcessor_FindFilterItem_impl_(Query query_arg, List<Object> list_arg, object operand_arg, SelectItem select_item_arg)
            {
                _query       = query_arg;
                _list        = list_arg;
                _operand     = operand_arg;
                _select_item = select_item_arg;
            } // constructor

            public void parse(string delim_arg, string itemToken)
            {
                var operand_var = _query.createOperand(itemToken, _select_item, false);
                _list.Add(_operand);
            }
        } // _QueryPartProcessor_FindFilterItem_impl_ class

        private FilterItem findFilterItem(string expression)
        {
            string upperExpression = expression.ToUpper();

            QueryPartCollectionProcessor collectionProcessor = new QueryPartCollectionProcessor();
            new QueryPartParser(collectionProcessor, expression, " AND ", " OR ").parse();

            List<String> tokens = collectionProcessor.getTokens();
            List<String> delims = collectionProcessor.getDelims();
            if (tokens.Count == 1)
            {
               expression      = tokens[0];
               upperExpression = expression.ToUpper();
            }
            else
            {
               //[J2N]  LogicalOperator.valueOf() <=> Enum.Parse(typeof(LogicalOperator), value)
               LogicalOperator logicalOperator = (LogicalOperator)Enum.Parse(typeof(LogicalOperator), delims[1].Trim() );

               NList<FilterItem> filterItems = new NList<FilterItem>();
               for (int i = 0; i < tokens.Count; i++)
               {
                  string token = tokens[i];
                  FilterItem filterItem = findFilterItem(token);
                  filterItems.Add(filterItem);
               }
               return new FilterItem(logicalOperator, filterItems);
            }

            //[J2Cc] operator identifier (a keyword in C#) => operator_var
            OperatorType operator_var = null;
            string       leftSide     = null;
            string       rightSide;
            {
                    string         rightSideCandidate = null;
                    OperatorType[] operators          = OperatorType.BUILT_IN_OPERATORS;
                    foreach (OperatorType operatorCandidate in operators)
                    {
                        string searchStr;
                        if (operatorCandidate.isSpaceDelimited())
                        {
                            searchStr = ' ' + operatorCandidate.toSql() + ' ';
                        }
                        else
                        {
                            searchStr = operatorCandidate.toSql();
                        }
                        int operatorIndex = upperExpression.IndexOf(searchStr);
                        if (operatorIndex > 0)
                        {
                            operator_var       = operatorCandidate;
                            leftSide           = expression.Substring(0, operatorIndex).Trim();
                            rightSideCandidate = expression.Substring(operatorIndex + searchStr.Length).Trim();
                            break;
                         }
                    }

                    if (operator_var == null) 
                    {
                        // check special cases for IS NULL and IS NOT NULL
                        if (expression.EndsWith(" IS NOT NULL"))
                        {
                           operator_var = OperatorType.DIFFERENT_FROM;
                           leftSide = expression.Substring(0, expression.LastIndexOf(" IS NOT NULL")).Trim();
                           rightSideCandidate = "NULL";
                        }
                        else if (expression.EndsWith(" IS NULL"))
                        {
                               operator_var = OperatorType.EQUALS_TO;
                               leftSide = expression.Substring(0, expression.LastIndexOf(" IS NULL")).Trim();
                               rightSideCandidate = "NULL";
                        }
                    }

                    rightSide = rightSideCandidate;
            }

            if (operator_var == null) {
                return new FilterItem(expression);
            }

            SelectItem selectItem = findSelectItem(leftSide, false);
            if (selectItem == null)
            {
                return new FilterItem(expression);
            }

            Object operand = null;
            if (operator_var == OperatorType.IN)
            {
                List<Object> list = new List<Object>();
                new QueryPartParser(new _QueryPartProcessor_FindFilterItem_impl_(this, list, operand, selectItem), rightSide, ",").parse();
                operand = list;
            } 
            else 
            {
                operand = createOperand(rightSide, selectItem, true);
            }

            return new FilterItem(selectItem, operator_var, operand);
        } // findFilterItem()

        //public void GetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    throw new NotImplementedException();
        //}
    } // Query class
} // org.apache.metamodel.data namespace
