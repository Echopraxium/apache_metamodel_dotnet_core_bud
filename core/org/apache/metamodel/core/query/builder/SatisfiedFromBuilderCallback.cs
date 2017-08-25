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
// https://github.com/apache/metamodel/blob/57e48be23844537690bdd6191ceb78adb4266e92/core/src/main/java/org/apache/metamodel/query/builder/SatisfiedFromBuilderCallback.java
using org.apache.metamodel.query;
using org.apache.metamodel.query.builder;
using org.apache.metamodel.schema;
using org.apache.metamodel.util;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.apache.metamodel.core.query.builder
{
    public abstract class SatisfiedFromBuilderCallback : BaseObject, SatisfiedFromBuilder
    {
        private Query       query;
        private DataContext dataContext;

        public SatisfiedFromBuilderCallback(Query query, DataContext dataContext)
        {
            this.query = query;
            this.dataContext = dataContext;
        }

        protected Query getQuery()
        {
            return query;
        }

        protected DataContext getDataContext()
        {
            return dataContext;
        }

        // @Override
        public TableFromBuilder And(Table table)
        {
            if (table == null)
            {
                throw new ArgumentException("table cannot be null");
            }

            return new TableFromBuilderImpl(table, query, dataContext);
        }

        // @Override
        public ColumnSelectBuilder select(Column column)
        {
            if (column == null)
            {
                throw new ArgumentException("column cannot be null");
            }

            GroupedQueryBuilder queryBuilder = new GroupedQueryBuilderImpl(dataContext, query);
            return new ColumnSelectBuilderImpl(column, query, queryBuilder);
        }

        // @Override
        public SatisfiedSelectBuilder selectAll()
        {
            getQuery().selectAll();
            GroupedQueryBuilder queryBuilder = new GroupedQueryBuilderImpl(dataContext, query);
            return new SatisfiedSelectBuilderImpl(queryBuilder);
        }

        // @Override
        public FunctionSelectBuilder select(FunctionType function, String columnName)
        {
            GroupedQueryBuilderImpl queryBuilder = new GroupedQueryBuilderImpl(dataContext, query);
            Column column = queryBuilder.findColumn(columnName);
            return select(function, column);
        }

        // @Override
        public FunctionSelectBuilder select(FunctionType function, String columnName, Object[] functionParameters)
        {
            GroupedQueryBuilderImpl queryBuilder = new GroupedQueryBuilderImpl(dataContext, query);
            Column column = queryBuilder.findColumn(columnName);
            return (FunctionSelectBuilder) select(function, column, functionParameters);
        }

        // @Override    
        public GroupedQueryBuilder select(FunctionType function, Column column, Object[] functionParameters)
        {
            if (function == null)
            {
                throw new ArgumentException("functionType cannot be null");
            }
            if (column == null)
            {
                throw new ArgumentException("column cannot be null");
            }

            GroupedQueryBuilder queryBuilder = new GroupedQueryBuilderImpl(dataContext, query);
            return new FunctionSelectBuilderImpl(function, column, functionParameters, query, queryBuilder);
        }

        // @Override
        //public CountSelectBuilder<SatisfiedQueryBuilder<GroupedQueryBuilder>> selectCount()
        public CountSelectBuilder selectCount()
        {
            GroupedQueryBuilder queryBuilder = new GroupedQueryBuilderImpl(dataContext, query);
            return new CountSelectBuilderImpl(query, queryBuilder);
        }

        // @Override
        public TableFromBuilder And(String schemaName, String tableName)
        {
            if (schemaName == null)
            {
                throw new ArgumentException("schemaName cannot be null");
            }
            if (tableName == null)
            {
                throw new ArgumentException("tableName cannot be null");
            }

            Schema schema = dataContext.getSchemaByName(schemaName);
            if (schema == null)
            {
                schema = dataContext.getDefaultSchema();
            }
            return And(schema, tableName);
        }

        private TableFromBuilder And(Schema schema, String tableName)
        {
            Table table = schema.getTableByName(tableName);
            return And(table.getName());
        }

        // @Override
        public TableFromBuilder And(String tableName)
        {
            if (tableName == null)
            {
                throw new ArgumentException("tableName cannot be null");
            }
            return And(dataContext.getDefaultSchema(), tableName);
        }

        // @Override
        public SatisfiedSelectBuilder select(params Column[] columns)
        {
            if (columns == null)
            {
                throw new ArgumentException("columns cannot be null");
            }
            query.select(columns);
            GroupedQueryBuilder queryBuilder = new GroupedQueryBuilderImpl(dataContext, query);
            return new SatisfiedSelectBuilderImpl(queryBuilder);
        }

        // @Override
        public SatisfiedSelectBuilder select(List<Column> columns)
        {
            return select(columns.ToArray()); // new Column[columns.size()]));
        }

        //v@Override
        public SatisfiedSelectBuilder select(params String[] columnNames)
        {
            if (columnNames == null)
            {
                throw new ArgumentException("columnNames cannot be null");
            }
            foreach (String columnName in columnNames)
            {
                select(columnName);
            }
            GroupedQueryBuilder queryBuilder = new GroupedQueryBuilderImpl(dataContext, query);
            return new SatisfiedSelectBuilderImpl(queryBuilder);
        }

        // @Override
        public SatisfiedSelectBuilder select(String selectExpression, bool allowExpressionBasedSelectItem)
        {
            if (selectExpression == null)
            {
                throw new ArgumentException("selectExpression cannot be null");
            }

            query.select(selectExpression, allowExpressionBasedSelectItem);

            GroupedQueryBuilder queryBuilder = new GroupedQueryBuilderImpl(dataContext, query);
            return new SatisfiedSelectBuilderImpl(queryBuilder);
        }

        // @Override
        public SatisfiedSelectBuilder select(String selectExpression)
        {
            return select(selectExpression, false);
        }

        // @Override
        public FunctionSelectBuilder select(FunctionType function, Column column)
        {
            return (FunctionSelectBuilder)select(function, column, new Object[0]);
        }

        // @Override
        protected void decorateIdentity(List<Object> identifiers)
        {
            identifiers.Add(query);
        }

        FunctionSelectBuilder SatisfiedFromBuilder.select(FunctionType function, Column column, object[] functionParameters)
        {
            throw new NotImplementedException();
        }
    } // SatisfiedFromBuilderCallback class
} // org.apache.metamodel.core.query.builder
