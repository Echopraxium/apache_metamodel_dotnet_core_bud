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
// https://github.com/apache/metamodel/blob/57e48be23844537690bdd6191ceb78adb4266e92/core/src/main/java/org/apache/metamodel/query/builder/GroupedQueryBuilderImpl.java
using org.apache.metamodel.core.data;
using org.apache.metamodel.core.query.parser;
using org.apache.metamodel.j2n.collections;
using org.apache.metamodel.j2n.slf4j;
using org.apache.metamodel.query;
using org.apache.metamodel.query.builder;
using org.apache.metamodel.schema;
using org.apache.metamodel.util;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.apache.metamodel.core.query.builder
{
    /**
     * Main implementation of the {@link GroupedQueryBuilder} interface.
     */
    public sealed class GroupedQueryBuilderImpl : BaseObject, GroupedQueryBuilder
    {
        private static NLogger logger = NLoggerFactory.getLogger(typeof(GroupedQueryBuilderImpl).Name);

        private Query       _query;
        private DataContext _dataContext;

        public GroupedQueryBuilderImpl(DataContext dataContext, Query query)
        {
            if (query == null)
            {
                throw new ArgumentException("query cannot be null");
            }
            _dataContext = dataContext;
            _query = query;
        }

        // @Override
        public ColumnSelectBuilder select(Column column)
        {
            if (column == null)
            {
                throw new ArgumentException("column cannot be null");
            }
            return new ColumnSelectBuilderImpl(column, _query, this);
        }

        // @Override
        public SatisfiedQueryBuilder select(FunctionType function, String columnName)
        {
            if (function == null)
            {
                throw new ArgumentException("function cannot be null");
            }
            Column column = findColumn(columnName);
            return new FunctionSelectBuilderImpl(function, column, null, _query, this);
        }

        // @Override
        public FunctionSelectBuilder select(FunctionType function, Column column)
        {
            if (function == null)
            {
                throw new ArgumentException("function cannot be null");
            }
            if (column == null)
            {
                throw new ArgumentException("column cannot be null");
            }
            return new FunctionSelectBuilderImpl(function, column, null, _query, this);
        }

        // @Override
        public SatisfiedQueryBuilder where(params FilterItem[] filters)
        {
            _query.where(filters);
            return this;
        }

        // @Override
        public SatisfiedQueryBuilder where(IEnumerable<FilterItem> filters)
        {
            _query.where(filters);
            return this;
        }

        // @Override
        public ColumnSelectBuilder select(String columnName)
        {
            Column column = findColumn(columnName);
            return select(column);
        }

        // @Override
        public CountSelectBuilder selectCount()
        {
            return new CountSelectBuilderImpl(_query, this);
        }

        // @Override
        public SatisfiedSelectBuilder select(params Column[] columns)
        {
            if (columns == null)
            {
                throw new ArgumentException("column cannot be null");
            }
            _query.select(columns);
            return new SatisfiedSelectBuilderImpl(this);
        }

        // @Override
        public WhereBuilder where(Column column)
        {
            if (column == null)
            {
                throw new ArgumentException("column cannot be null");
            }
            return new WhereBuilderImpl(column, _query, this);
        }

        // @Override
        public WhereBuilder where(String columnName)
        {
            Column column = findColumn(columnName);
            return where(column);
        }

        // @Override
        public WhereBuilder where(ScalarFunction function, Column column)
        {
            SelectItem selectItem = new SelectItem(function, column);
            return new WhereBuilderImpl(selectItem, _query, this);
        }

        // @Override
        public WhereBuilder where(ScalarFunction function, String columnName)
        {
            Column column = findColumn(columnName);
            return where(function, column);
        }

        // @Override
        public Column findColumn(String columnName) // throws IllegalArgumentException
        {
            if (columnName == null) 
            {
                throw new ArgumentException("columnName cannot be null");
            }

            List<FromItem> fromItems = _query.getFromClause().getItems();
            List<SelectItem> selectItems = _query.getSelectClause().getItems();

            Column column = null;

            int dotIndex = columnName.IndexOf('.');
            if (dotIndex != -1) 
            {
                // check aliases of from items
                String aliasPart  = columnName.Substring(0, dotIndex);
                String columnPart = columnName.Substring(dotIndex + 1);

                foreach (FromItem fromItem in fromItems)
                {
                    column = null;
                    column = findColumnInAliasedTable(column, fromItem, aliasPart, columnPart);
                    if (column != null)
                    {
                        return column;
                    }
                }
            }

            // check columns already in select clause
            foreach (SelectItem item in selectItems)
            {
                column = item.getColumn();
                if (column != null)
                {
                    if (columnName.Equals(column.getName()))
                    {
                        return column;
                    }
                }
            }

            foreach (FromItem fromItem in fromItems)
            {
                Table table = fromItem.getTable();
                if (table != null)
                {
                    column = table.getColumnByName(columnName);
                    if (column != null)
                    {
                        return column;
                    }
                }
            }

            column = _dataContext.getColumnByQualifiedLabel(columnName);
            if (column != null) {
                return column;
            }

            ArgumentException exception = new ArgumentException("Could not find column: " + columnName);

            if (logger.isDebugEnabled()) {
                logger.debug("findColumn('" + columnName + "') could not resolve a column", exception);
                foreach (FromItem fromItem in fromItems)
                {
                    Table table = fromItem.getTable();
                    if (table != null)
                    {
                        logger.debug("Table available in FROM item: {}. Column names: {}", table, 
                                     NArrays.ArrayAsString(table.getColumnNames()));
                    }
                }
            }

            throw exception;
        }

        private Column findColumnInAliasedTable(Column column, FromItem fromItem, String aliasPart, String columnPart)
        {
            if (column != null)
            {
                // ensure that if the column has already been found, return it
                return column;
            }

            Table table = fromItem.getTable();
            if (table != null)
            {
                String alias = fromItem.getAlias();
                if (alias != null && alias.Equals(aliasPart))
                {
                    column = table.getColumnByName(columnPart);
                }
            }
            else
            {
                FromItem leftSide = fromItem.getLeftSide();
                column = findColumnInAliasedTable(column, leftSide, aliasPart, columnPart);
                FromItem rightSide = fromItem.getRightSide();
                column = findColumnInAliasedTable(column, rightSide, aliasPart, columnPart);
                if (column != null)
                {
                    Query subQuery = fromItem.getSubQuery();
                    if (subQuery != null)
                    {
                        List<FromItem> items = subQuery.getFromClause().getItems();
                        foreach (FromItem subQueryFromItem in items)
                        {
                            column = findColumnInAliasedTable(column, subQueryFromItem, aliasPart, columnPart);
                        }
                    }
                }
            }

            return column;
        }

        // @Override
        public SatisfiedOrderByBuilder orderBy(String columnName)
        {
            return orderBy(findColumn(columnName));
        }

        // @Override
        public SatisfiedOrderByBuilder orderBy(Column column)
        {
            if (column == null)
            {
                throw new ArgumentException("column cannot be null");
            }
            return (SatisfiedOrderByBuilder) new SatisfiedOrderByBuilderImpl(column, _query, this);
        }

        // @Override
        public SatisfiedOrderByBuilder orderBy(FunctionType function, Column column)
        {
            if (function == null)
            {
                throw new ArgumentException("function cannot be null");
            }
            if (column == null)
            {
                throw new ArgumentException("column cannot be null");
            }
            return new SatisfiedOrderByBuilderImpl(function, column, _query, this);
        }

        // @Override
        public GroupedQueryBuilder groupBy(Column column)
        {
            if (column == null)
            {
                throw new ArgumentException("column cannot be null");
            }
            _query.groupBy(column);
            return this;
        }

        // @Override
        public GroupedQueryBuilder groupBy(String columnName)
        {
            Column column = findColumn(columnName);
            return groupBy(column);
        }

        // @Override
        public GroupedQueryBuilder groupBy(params String[] columnNames)
        {
            _query.groupBy(columnNames);
            return this;
        }

        // @Override
        public GroupedQueryBuilder groupBy(params Column[] columns)
        {
            if (columns == null)
            {
                throw new ArgumentException("columns cannot be null");
            }
            _query.groupBy(columns);
            return this;
        }

        // @Override
        public HavingBuilder having(String columnExpression)
        {
            SelectItemParser parser = new SelectItemParser(_query, false);
            SelectItem selectItem = parser.findSelectItem(columnExpression);
            return having(selectItem);
        }

        // @Override
        public HavingBuilder having(SelectItem selectItem)
        {
            if (selectItem == null)
            {
                throw new ArgumentException("selectItem cannot be null");
            }
            return new HavingBuilderImpl(selectItem, _query, this);
        }

        //  @Override
        public HavingBuilder having(FunctionType function, Column column)
        {
            if (function == null)
            {
                throw new ArgumentException("function cannot be null");
            }
            if (column == null)
            {
                throw new ArgumentException("column cannot be null");
            }
            return new HavingBuilderImpl(function, column, _query, this);
        }

        // @Override
        public SatisfiedQueryBuilder limit(int maxRows)
        {
            _query.setMaxRows(maxRows);
            return this;
        }

        // @Override
        public SatisfiedQueryBuilder maxRows(int maxRows)
        {
            _query.setMaxRows(maxRows);
            return this;
        }

        // @Override
        public SatisfiedQueryBuilder firstRow(int firstRow)
        {
            if (firstRow >= 0)
            {
                _query.setFirstRow(firstRow);
            }
            else
            {
                _query.setFirstRow(null);
            }
            return this;
        }

        // @Override
        public SatisfiedQueryBuilder offset(int offset)
        {
            if (offset >= 0)
            {
                _query.setFirstRow(offset + 1);
            }
            else
            {
                _query.setFirstRow(null);
            }
            return this;
        }

        // @Override
        public String toString()
        {
            return _query.toSql();
        }

        // @Override
        public Query toQuery()
        {
            return _query.clone();
        }

        // @Override
        public CompiledQuery compile()
        {
            return _dataContext.compileQuery(_query);
        }

        // @Override
        public DataSet execute()
        {
            return _dataContext.executeQuery(_query);
        }

        // @Override
        public void decorateIdentity(List<Object> identifiers)
        {
            identifiers.Add(_query);
        }

        SatisfiedQueryBuilder GroupedQueryBuilder.orderBy(FunctionType function, Column column)
        {
            throw new NotImplementedException();
        }

        SatisfiedSelectBuilder SatisfiedQueryBuilder.selectCount()
        {
            throw new NotImplementedException();
        }
    } // GroupedQueryBuilderImpl class
} // org.apache.metamodel.core.query.builder
