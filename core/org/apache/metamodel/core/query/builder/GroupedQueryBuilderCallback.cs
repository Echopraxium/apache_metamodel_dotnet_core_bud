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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/query/builder/GroupedQueryBuilderCallback.java
using org.apache.metamodel.core.data;
using org.apache.metamodel.query;
using org.apache.metamodel.query.builder;
using org.apache.metamodel.schema;
using org.apache.metamodel.util;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.apache.metamodel.core.query.builder
{
    public abstract class GroupedQueryBuilderCallback : BaseObject, GroupedQueryBuilder
    {
        private GroupedQueryBuilder queryBuilder;

        public GroupedQueryBuilderCallback(GroupedQueryBuilder queryBuilder)
        {
            this.queryBuilder = queryBuilder;
        } // constructor

        protected GroupedQueryBuilder getQueryBuilder()
        {
            return queryBuilder;
        } // constructor

        // @Override
        public SatisfiedQueryBuilder firstRow(int firstRow)
        {
            GroupedQueryBuilder query_builder = getQueryBuilder();
            return query_builder.firstRow(firstRow);
        } // constructor

        // @Override
        public SatisfiedQueryBuilder limit(int maxRows)
        {
            GroupedQueryBuilder query_builder = getQueryBuilder();
            return query_builder.limit(maxRows);
        }

        // @Override
        public SatisfiedQueryBuilder offset(int offset)
        {
            GroupedQueryBuilder query_builder = getQueryBuilder();
            return query_builder.offset(offset);
        }

        // @Override
        public SatisfiedQueryBuilder maxRows(int maxRows)
        {
            GroupedQueryBuilder query_builder = getQueryBuilder();
            return query_builder.maxRows(maxRows);
        }

        // @Override
        public SatisfiedSelectBuilder select(params Column[] columns)
        {
            GroupedQueryBuilder query_builder = getQueryBuilder();
            return query_builder.select(columns);
        }

        // @Override
        public Column findColumn(String columnName) // throws IllegalArgumentException
        {
            GroupedQueryBuilder query_builder = getQueryBuilder();
            return query_builder.findColumn(columnName);
        }

        // @Override
        public ColumnSelectBuilder select(Column column)
        {
            GroupedQueryBuilder query_builder = getQueryBuilder();
            return (ColumnSelectBuilder) query_builder.select(column);
        }

        // @Override
        public SatisfiedQueryBuilder select(FunctionType function, String columnName)
        {
            GroupedQueryBuilder query_builder = getQueryBuilder();
            return query_builder.select(function, columnName);
        }

        // @Override
        public FunctionSelectBuilder select(FunctionType functionType, Column column)
        {
            GroupedQueryBuilder query_builder = getQueryBuilder();
            return query_builder.select(functionType, column);
        }

        // @Override
        public ColumnSelectBuilder select(String column_name)
        {
            GroupedQueryBuilder query_builder = getQueryBuilder();
            return query_builder.select(column_name);
        }

        public SatisfiedSelectBuilder selectCount()
        {
            return getQueryBuilder().selectCount();
            //throw new NotImplementedException();
        }

        // @Override
        public WhereBuilder where(Column column)
        {
            GroupedQueryBuilder query_builder = getQueryBuilder();
            return (WhereBuilder) query_builder.where((FilterItem)column);
        }

        // @Override
        public WhereBuilder where(ScalarFunction function, Column column)
        {
            GroupedQueryBuilder query_builder = getQueryBuilder();
            return (WhereBuilder) query_builder.where((FilterItem)function, (FilterItem)column);
        }

        // @Override
        public WhereBuilder where(ScalarFunction function, String column_name)
        {
            GroupedQueryBuilder query_builder = getQueryBuilder();
            return (WhereBuilder) query_builder.where(function, column_name);
        }

        // @Override
        public SatisfiedOrderByBuilder orderBy(Column column)
        {
            GroupedQueryBuilder query_builder = getQueryBuilder();
            return query_builder.orderBy(column);
        }

        // @Override
        public GroupedQueryBuilder groupBy(String column_name)
        {
            GroupedQueryBuilder query_builder = getQueryBuilder();
            return query_builder.groupBy(column_name);
        }

        // @Override
        public GroupedQueryBuilder groupBy(Column column)
        {
            GroupedQueryBuilder query_builder = getQueryBuilder();
            return query_builder.groupBy(column.getName());
        }

        // @Override
        public Query toQuery()
        {
            GroupedQueryBuilder query_builder = getQueryBuilder();
            return query_builder.toQuery();
        }

        // @Override
        public CompiledQuery compile()
        {
            return getQueryBuilder().compile();
        }

        // @Override
        public HavingBuilder having(FunctionType functionType, Column column)
        {
            GroupedQueryBuilder query_builder = getQueryBuilder();
            return query_builder.having(functionType, column);
        }

        // @Override
        public HavingBuilder having(String columnExpression)
        {
            GroupedQueryBuilder query_builder = getQueryBuilder();
            return query_builder.having(columnExpression);
        }

        // @Override
        public HavingBuilder having(SelectItem selectItem)
        {
            GroupedQueryBuilder query_builder = getQueryBuilder();
            return query_builder.having(selectItem);
        }

        // @Override
        public GroupedQueryBuilder groupBy(params String[] column_names)
        {
            GroupedQueryBuilder query_builder = getQueryBuilder();
            return query_builder.groupBy(column_names);
        }

        // @Override
        public GroupedQueryBuilder groupBy(params Column[] columns)
        {
            GroupedQueryBuilder query_builder = getQueryBuilder();
            query_builder.groupBy(columns);
            return this;
        }

        // @Override
        protected void decorateIdentity(List<Object> identifiers)
        {
            identifiers.Add(queryBuilder);
        }

        // @Override
        public DataSet execute()
        {
            return queryBuilder.execute();
        }

        // @Override
        public WhereBuilder where(String columnName)
        {
            GroupedQueryBuilder query_builder = getQueryBuilder();
            return query_builder.where(columnName);
        }

        // @Override
        public SatisfiedQueryBuilder where(params FilterItem[] filters)
        {
            GroupedQueryBuilder query_builder = getQueryBuilder();
            return query_builder.where(filters);
        }

        // @Override
        public SatisfiedQueryBuilder where(IEnumerable<FilterItem> filters)
        {
            return getQueryBuilder().where(filters);
        }

        // @Override
        public SatisfiedOrderByBuilder orderBy(String column_name)
        {
            GroupedQueryBuilder query_builder = getQueryBuilder();
            return query_builder.orderBy(column_name);
        }

        public SatisfiedQueryBuilder orderBy(FunctionType function, Column column)
        {
            return getQueryBuilder().orderBy(function, column);
            //throw new NotImplementedException();
        }
    } // GroupedQueryBuilderCallback class
} // org.apache.metamodel.core.query.builder
