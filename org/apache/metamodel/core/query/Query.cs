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
using org.apache.metamodel.j2cs.data;
using org.apache.metamodel.schema;
using System;
using System.Collections.Generic;
using System.Text;
using org.apache.metamodel.j2cs.data.numbers;

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
    public sealed class Query 
        //: BaseObject, ICloneable, ISerializable
    {
        private static readonly long serialVersionUID = -5976325207498574216L;

        //private SelectClause               _selectClause;
        //private FromClause                 _fromClause;
        //private FilterClause               _whereClause;
        //private /*Version*/ GroupByClause  _groupByClause;
        //private FilterClause               _havingClause;
        //private OrderByClause              _orderByClause;

        private CsInteger _maxRows;
        private CsInteger _firstRow;

        //public Query()
        //{
        //    _selectClause   = new SelectClause(this);
        //    _fromClause     = new FromClause(this);
        //    _whereClause    = new FilterClause(this, AbstractQueryClause.PREFIX_WHERE);
        //    _groupByClause  = new GroupByClause(this);
        //    _havingClause   = new FilterClause(this, AbstractQueryClause.PREFIX_HAVING);
        //    _orderByClause  = new OrderByClause(this);
        //}

        //public override string ToString()
        //{
        //    return toSql();
        //} // ToString()

        //public Query select(Column column, FromItem fromItem)
        //{
        //    SelectItem selectItem = new SelectItem(column, fromItem);
        //    return select(selectItem);
        //}

        //public Query select(params Column[] columns)
        //{
        //    foreach (Column column in columns)
        //    {
        //        SelectItem selectItem = new SelectItem(column);
        //        selectItem.setQuery(this);
        //        _selectClause.addItem(selectItem);
        //    }
        //    return this;
        //}

        //public Query select(params SelectItem[] items)
        //{
        //    _selectClause.addItems(items);
        //    return this;
        //}

        //public Query select(FunctionType functionType, Column column)
        //{
        //    _selectClause.addItem(new SelectItem(functionType, column));
        //    return this;
        //}

        //public Query select(string expression, string alias)
        //{
        //    return select(new SelectItem(expression, alias));
        //}

        ///**
        // * Adds a selection to this query.
        // * 
        // * @param expression
        // * @return
        // */
        //public Query select(string expression)
        //{
        //    return select(expression, false);
        //}

        //public Query where(SelectItem selectItem, OperatorType operatorType, object operand)
        //{
        //    return where(new FilterItem(selectItem, operatorType, operand));
        //} // where()

        //public Query where(Column column, OperatorType operatorType, object operand)
        //{
        //    SelectItem selectItem = _selectClause.getSelectItem(column);
        //    if (selectItem == null)
        //    {
        //        selectItem = new SelectItem(column);
        //    }
        //    return where(selectItem, operatorType, operand);
        //} // where()

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
        //public Query select(string expression, bool allowExpressionBasedSelectItem)
        //{
        //    QueryPartParser clauseParser = new QueryPartParser(
        //                                       new SelectItemParser(this, allowExpressionBasedSelectItem), expression, ",");
        //    clauseParser.parse();
        //    return this;
        //}

        //private SelectItem findSelectItem(string expression, bool allowExpressionBasedSelectItem)
        //{
        //    SelectItemParser parser = new SelectItemParser(this, allowExpressionBasedSelectItem);
        //    return parser.findSelectItem(expression);
        //}

        ///**
        // * Select all available select items from all currently available FROM
        // * items. Equivalent of the expression "SELECT * FROM ..." in SQL.
        // * 
        // * @return
        // */
        //public Query selectAll()
        //{
        //    List<FromItem> items = getFromClause().getItems();
        //    foreach (FromItem fromItem in items)
        //    {
        //        selectAll(fromItem);
        //    }
        //    return this;
        //}

        //public Query selectAll(FromItem fromItem)
        //{
        //    if (fromItem.getTable() != null)
        //    {
        //        Column[] columns = fromItem.getTable().getColumns();
        //        foreach (Column column in columns)
        //        {
        //            select(column, fromItem);
        //        }
        //    }
        //    else if (fromItem.getJoin() != null)
        //    {
        //        selectAll(fromItem.getLeftSide());
        //        selectAll(fromItem.getRightSide());
        //    }
        //    else if (fromItem.getSubQuery() != null)
        //    {
        //        List<SelectItem> items = fromItem.getSubQuery().getSelectClause().getItems();
        //        foreach (SelectItem subQuerySelectItem in items)
        //        {
        //            select(new SelectItem(subQuerySelectItem, fromItem));
        //        }
        //    }
        //    else
        //    {
        //        throw new MetaModelException("All select items ('*') not determinable with from item: " + fromItem);
        //    }
        //    return this;
        //}

        //public Query selectDistinct()
        //{
        //    _selectClause.setDistinct(true);
        //    return this;
        //}

        //public Query selectCount()
        //{
        //    return select(SelectItem.getCountAllItem());
        //}

        //public Query from(params FromItem[] items)
        //{
        //    _fromClause.addItems(items);
        //    return this;
        //}

        //public Query from(Table table)
        //{
        //    return from(new FromItem(table));
        //}

        //public Query from(string expression)
        //{
        //    return from(new FromItem(expression));
        //}

        //public Query from(Table table, string alias)
        //{
        //    return from(new FromItem(table).setAlias(alias));
        //}

        //public Query from(Table leftTable, Table rightTable, JoinType joinType, Column leftOnColumn, Column rightOnColumn)
        //{
        //    SelectItem[] leftOn   = new SelectItem[] { new SelectItem(leftOnColumn) };
        //    SelectItem[] rightOn  = new SelectItem[] { new SelectItem(rightOnColumn) };
        //    FromItem     fromItem = new FromItem(joinType, new FromItem(leftTable), new FromItem(rightTable), leftOn, rightOn);
        //    return from(fromItem);
        //}

        //public Query groupBy(params String[] groupByTokens)
        //{
        //    foreach (string groupByToken in groupByTokens)
        //    {
        //        SelectItem selectItem = findSelectItem(groupByToken, true);
        //        groupBy(new GroupByItem(selectItem));
        //    }
        //    return this;
        //}

        //public Query groupBy(params GroupByItem[] items)
        //{
        //    foreach (GroupByItem item in items)
        //    {
        //        SelectItem selectItem = item.getSelectItem();
        //        if (selectItem != null && selectItem.getQuery() == null)
        //        {
        //            selectItem.setQuery(this);
        //        }
        //    }
        //    _groupByClause.addItems(items);
        //    return this;
        //}

        //public Query groupBy(params Column [] columns)
        //{
        //    foreach (Column column in columns)
        //    {
        //        SelectItem selectItem = new SelectItem(column).setQuery(this);
        //        _groupByClause.addItem(new GroupByItem(selectItem));
        //    }
        //    return this;
        //}

        //public Query orderBy(params OrderByItem[] items)
        //{
        //    _orderByClause.addItems(items);
        //    return this;
        //} // orderBy()

        //public Query orderBy(params String[] orderByTokens)
        //{
        //    foreach (string orderByToken in orderByTokens)
        //    {
        //        orderByToken = orderByToken.Trim();
        //        Direction direction;
        //        if (orderByToken.ToUpper().EndsWith("DESC"))
        //        {
        //            direction = Direction.DESC;
        //            orderByToken = orderByToken.Substring(0, orderByToken.Length - 4).Trim();
        //        }
        //        else if (orderByToken.ToUpper().EndsWith("ASC"))
        //        {
        //            direction = Direction.ASC;
        //            orderByToken = orderByTokenSsubstring(0, orderByToken.Length - 3).trim();
        //        }
        //        else
        //        {
        //            direction = Direction.ASC;
        //        }

        //        OrderByItem orderByItem = new OrderByItem(findSelectItem(orderByToken, true), direction);
        //        orderBy(orderByItem);
        //    }
        //    return this;
        //} // orderBy()

        //public Query orderBy(Column column)
        //{
        //    return orderBy(column, Direction.ASC);
        //} // orderBy()

        ///**
        // * @deprecated use orderBy(Column, Direction) instead
        // */
        //[System.Obsolete]
        //public Query orderBy(Column column, bool ascending)
        //{
        //    if (ascending)
        //    {
        //        return orderBy(column, Direction.ASC);
        //    }
        //    else
        //    {
        //        return orderBy(column, Direction.DESC);
        //    }
        //} // orderBy()

        //public Query orderBy(Column column, Direction direction)
        //{
        //    SelectItem selectItem = _selectClause.getSelectItem(column);
        //    if (selectItem == null)
        //    {
        //        selectItem = new SelectItem(column);
        //    }
        //    return orderBy(new OrderByItem(selectItem, direction));
        //} // orderBy()

        //public Query where(params FilterItem[] items)
        //{
        //    _whereClause.addItems(items);
        //    return this;
        //} // where()

        //public Query where(IEnumerable<FilterItem> items)
        //{ 
        //    _whereClause.addItems(items);
        //    return this;
        //} // where()

        //public Query where(params String[] whereItemTokens)
        //{
        //    foreach (string whereItemToken in whereItemTokens)
        //    {
        //        FilterItem filterItem = findFilterItem(whereItemToken);
        //        where(filterItem);
        //    }
        //    return this;
        //} // where()

        //    private FilterItem findFilterItem(string expression)
        //    {
        //        string upperExpression = expression.ToUpper();

        //        QueryPartCollectionProcessor collectionProcessor = new QueryPartCollectionProcessor();
        //        new QueryPartParser(collectionProcessor, expression, " AND ", " OR ").parse();

        //        List<String> tokens = collectionProcessor.getTokens();
        //        List<String> delims = collectionProcessor.getDelims();
        //        if (tokens.Count == 1)
        //        {
        //            expression      = tokens[0];
        //            upperExpression = expression.ToUpper();
        //        }
        //        else
        //        {
        //            //[J2Cs]  LogicalOperator.valueOf() <=> Enum.Parse(typeof(LogicalOperator), value)
        //            LogicalOperator logicalOperator = (LogicalOperator)Enum.Parse(typeof(LogicalOperator), delims[1].Trim() );

        //            List<FilterItem> filterItems = new List<FilterItem>();
        //            for (int i = 0; i < tokens.Count; i++)
        //            {
        //                string token = tokens[i];
        //                FilterItem filterItem = findFilterItem(token);
        //                filterItems.Add(filterItem);
        //            }
        //            return new FilterItem(logicalOperator, filterItems);
        //        }

        //        //[J2Cc] operator identifier (a keyword in C#) => operator_var
        //        OperatorType operator_var = null;
        //        string       leftSide     = null;
        //        string       rightSide;
        //        {
        //            string         rightSideCandidate = null;
        //            OperatorType[] operators          = OperatorType.BUILT_IN_OPERATORS;
        //            foreach (OperatorType operatorCandidate in operators)
        //            {
        //                string searchStr;
        //                if (operatorCandidate.isSpaceDelimited())
        //                {
        //                    searchStr = ' ' + operatorCandidate.toSql() + ' ';
        //                }
        //                else
        //                {
        //                    searchStr = operatorCandidate.toSql();
        //                }
        //                int operatorIndex = upperExpression.IndexOf(searchStr);
        //                if (operatorIndex > 0)
        //                {
        //                    operator_var       = operatorCandidate;
        //                    leftSide           = expression.Substring(0, operatorIndex).Trim();
        //                    rightSideCandidate = expression.Substring(operatorIndex + searchStr.Length).Trim();
        //                    break;
        //                }
        //            }

        //            if (operator_var == null) 
        //            {
        //                // check special cases for IS NULL and IS NOT NULL
        //                if (expression.EndsWith(" IS NOT NULL"))
        //                {
        //                    operator_var = OperatorType.DIFFERENT_FROM;
        //                    leftSide = expression.Substring(0, expression.LastIndexOf(" IS NOT NULL")).trim();
        //                    rightSideCandidate = "NULL";
        //                }
        //                else if (expression.EndsWith(" IS NULL"))
        //                {
        //                    operator_var = OperatorType.EQUALS_TO;
        //                    leftSide = expression.Substring(0, expression.LastIndexOf(" IS NULL")).Trim();
        //                    rightSideCandidate = "NULL";
        //                }
        //            }

        //            rightSide = rightSideCandidate;
        //        }

        //        if (operator_var == null) {
        //            return new FilterItem(expression);
        //        }

        //        SelectItem selectItem = findSelectItem(leftSide, false);
        //        if (selectItem == null)
        //        {
        //            return new FilterItem(expression);
        //        }

        //        object operand;
        //        if (operator_var == OperatorType.IN)
        //        {
        //            List<Object> list = new List<Object>();

        //            Func<int, int, int> add = delegate (int x, int y)
        //            {
        //                return x + y;
        //            };

        //            Func<int, int, int> parse_delegate = delegate (int x, int y)
        //            {
        //                object operand_var = createOperand(itemToken, selectItem, false);
        //                list.Add(operand_var);
        //                return 0;
        //            };

        //                new QueryPartParser(new QueryPartProcessor()
        //            {
        //                void parse(string delim, string itemToken)
        //                {
        //                    object operand = createOperand(itemToken, selectItem, false);
        //                    list.Add(operand);
        //                }
        //            }, rightSide, ",").parse();
        //            operand = list;
        //        } else 
        //        {
        //            operand = createOperand(rightSide, selectItem, true);
        //                return new FilterItem(selectItem, operator_var, operand);
        //        }
        //} // findFilterItem()

        //public void GetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    throw new NotImplementedException();
        //}

        public object Clone()
        {
            throw new NotImplementedException();
        }

        //protected override void decorateIdentity(List<object> identifiers)
        //{
        //    throw new NotImplementedException();
        //}

        //private object createOperand(string token, SelectItem leftSelectItem, bool searchSelectItems)
        //{
        //    if (token.equalsIgnoreCase("NULL"))
        //    {
        //        return null;
        //    }

        //    if (token.StartsWith("'") && token.EndsWith("'") && token.Length > 2)
        //    {
        //        string stringOperand = token.Substring(1, token.Length - 1);
        //        //[J2Cs] replaceAll <=>  Replace
        //        stringOperand = stringOperand.Replace("\\\\'", "'");
        //        return stringOperand;
        //    }

        //    if (searchSelectItems)
        //    {
        //        SelectItem selectItem = findSelectItem(token, false);
        //        if (selectItem != null)
        //        {
        //            return selectItem;
        //        }
        //    }

        //    ColumnType expectedColumnType = leftSelectItem.getExpectedColumnType();
        //    object result;
        //    if (expectedColumnType == null)
        //    {
        //        // We're assuming number here, but it could also be boolean or a
        //        // time based type. But anyways, this should not happen since
        //        // expected column type should be available.
        //        result = NumberComparator.toNumber(token);
        //    }
        //    else if (expectedColumnType.isBoolean())
        //    {
        //        result = BooleanComparator.toBoolean(token);
        //    }
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

        //    return result;
        //}

        //public Query having(params FilterItem[] items)
        //{
        //    _havingClause.addItems(items);
        //    return this;
        //}

        //public Query having(FunctionType function, Column column, OperatorType operatorType, object operand)
        //{
        //    SelectItem selectItem = new SelectItem(function, column);
        //    return having(new FilterItem(selectItem, operatorType, operand));
        //}

        //public Query having(Column column, OperatorType operatorType, object operand)
        //{
        //    SelectItem selectItem = _selectClause.getSelectItem(column);
        //    if (selectItem == null)
        //    {
        //        selectItem = new SelectItem(column);
        //    }
        //    return having(new FilterItem(selectItem, operatorType, operand));
        //}

        //public Query having(params string [] havingItemTokens)
        //{
        //    foreach (string havingItemToken in havingItemTokens)
        //    {
        //        FilterItem filterItem = findFilterItem(havingItemToken);
        //        having(filterItem);
        //    }
        //    return this;
        //}

        /*
         * A string representation of this query. This representation will be SQL 99
         * compatible and can thus be used for database queries on databases that
         * meet SQL standards.
         */
        //public string toSql()
        //{
        //    return toSql(false);
        //}

        //protected string toSql(bool includeSchemaInColumnPaths)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(_selectClause.toSql(includeSchemaInColumnPaths));
        //    sb.Append(_fromClause.toSql(includeSchemaInColumnPaths));
        //    sb.Append(_whereClause.toSql(includeSchemaInColumnPaths));
        //    sb.Append(_groupByClause.toSql(includeSchemaInColumnPaths));
        //    sb.Append(_havingClause.toSql(includeSchemaInColumnPaths));
        //    sb.Append(_orderByClause.toSql(includeSchemaInColumnPaths));
        //    return sb.ToString();
        //}

        //public SelectClause getSelectClause()
        //{
        //    return _selectClause;
        //}

        //public FromClause getFromClause()
        //{
        //    return _fromClause;
        //}

        //public FilterClause getWhereClause()
        //{
        //    return _whereClause;
        //}

        //public GroupByClause getGroupByClause()
        //{
        //    return _groupByClause;
        //}

        //public FilterClause getHavingClause()
        //{
        //    return _havingClause;
        //}

        //public OrderByClause getOrderByClause()
        //{
        //    return _orderByClause;
        //}

        /**
         * Sets the maximum number of rows to be queried. If the result of the query
         * yields more rows they should be discarded.
         * 
         * @param maxRows
         *            the number of desired maximum rows. Can be null (default) for
         *            no limits
         * @return this query
         */
        //public Query setMaxRows(Integer maxRows)
        //{
        //    if (maxRows != null)
        //    {
        //        int maxRowsValue = maxRows;
        //        if (maxRowsValue < 0)
        //        {
        //            throw new ArgumentException("Max rows cannot be negative");
        //        }
        //    }
        //    _maxRows = maxRows;
        //    return this;
        //}

        /**
         * @return the number of maximum rows to yield from executing this query or
         *         null if no maximum/limit is set.
         */
        //public int getMaxRows()
        //{
        //    return _maxRows;
        //}

        /**
         * Sets the first row (aka offset) of the query's result. The row number is
         * 1-based, so setting a first row value of 1 is equivalent to not setting
         * it at all..
         * 
         * @param firstRow
         *            the first row, where 1 is the first row.
         * @return this query
         */
        //public Query setFirstRow(Integer firstRow)
        //{
        //    if (firstRow != null && firstRow < 1)
        //    {
        //        //[J2Cs] IllegalArgumentException <=> ArgumentException
        //        throw new ArgumentException("First row cannot be negative or zero");
        //    }
        //    _firstRow = firstRow;
        //    return this;
        //}

        /**
         * Gets the first row (aka offset) of the query's result, or null if none is
         * specified. The row number is 1-based, so setting a first row value of 1
         * is equivalent to not setting it at all..
         * 
         * @return the first row (aka offset) of the query's result, or null if no
         *         offset is specified.
         */
        //public int getFirstRow()
        //{
        //    return _firstRow;
        //}

        //protected void decorateIdentity(List<Object> identifiers)
        //{
        //    identifiers.Add(_maxRows);
        //    identifiers.Add(_selectClause);
        //    identifiers.Add(_fromClause);
        //    identifiers.Add(_whereClause);
        //    identifiers.Add(_groupByClause);
        //    identifiers.Add(_havingClause);
        //    identifiers.Add(_orderByClause);
        //}

        //public Query clone()
        //{
        //    Query q = new Query();
        //    q.setMaxRows(_maxRows);
        //    q.setFirstRow(_firstRow);
        //    q.getSelectClause().setDistinct(_selectClause.isDistinct());
        //    foreach (FromItem item in _fromClause.getItems())
        //    {
        //        q.from(item.clone());
        //    }
        //    foreach (SelectItem item in _selectClause.getItems())
        //    {
        //        q.select(item.clone(q));
        //    }
        //    foreach (FilterItem item in _whereClause.getItems())
        //    {
        //        q.where(item.clone());
        //    }
        //    foreach (GroupByItem item in _groupByClause.getItems())
        //    {
        //        q.groupBy(item.clone());
        //    }
        //    foreach (FilterItem item in _havingClause.getItems())
        //    {
        //        q.having(item.clone());
        //    }
        //    foreach (OrderByItem item in _orderByClause.getItems())
        //    {
        //        q.orderBy(item.clone());
        //    }
        //    return q;
        //}   // clone()
    } // Query class
} // org.apache.metamodel.data namespace
