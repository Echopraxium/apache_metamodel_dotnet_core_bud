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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/query/builder/TableFromBuilderImpl.java
using org.apache.metamodel.query;
using org.apache.metamodel.query.builder;
using org.apache.metamodel.schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.apache.metamodel.core.query.builder
{

    public sealed class TableFromBuilderImpl : SatisfiedFromBuilderCallback, TableFromBuilder //  implements TableFromBuilder
    {
        private FromItem _from_item;

        public TableFromBuilderImpl(Table table, Query query, DataContext dataContext) : base(query, dataContext)
        {
            _from_item = new FromItem(table);
            query.from(_from_item);
        }

        // @Override
        public JoinFromBuilder innerJoin(String tableName)
        {
            return innerJoin(findTable(tableName));
        }

        // @Override
        public JoinFromBuilder innerJoin(Table table)
        {
            if (table == null)
            {
                throw new ArgumentException("table cannot be null");
            }
            return new JoinFromBuilderImpl(getQuery(), _from_item, table, JoinType.INNER, getDataContext());
        }

        // @Override
        public JoinFromBuilder leftJoin(String tableName)
        {
            return leftJoin(findTable(tableName));
        }

        // @Override
        public JoinFromBuilder leftJoin(Table table)
        {
            if (table == null)
            {
                throw new ArgumentException("table cannot be null");
            }
            return new JoinFromBuilderImpl(getQuery(), _from_item, table, JoinType.LEFT, getDataContext());
        }

        // @Override
        public JoinFromBuilder rightJoin(String tableName)
        {
            return rightJoin(findTable(tableName));
        }

        // @Override
        public JoinFromBuilder rightJoin(Table table)
        {
            if (table == null)
            {
                throw new ArgumentException("table cannot be null");
            }
            return new JoinFromBuilderImpl(getQuery(), _from_item, table, JoinType.RIGHT, getDataContext());
        }

        // @Override
        public TableFromBuilder As(String alias) 
        {
            if (alias == null) 
            {
                throw new ArgumentException("alias cannot be null");
            }
            _from_item.setAlias(alias);
            return this;
        } // As

        // @Override
        protected void decorateIdentity(List<Object> identifiers)
        {
            base.decorateIdentity(identifiers);
            identifiers.Add(_from_item);
        }

        private Table findTable(String tableName)
        {
            if (tableName == null)
            {
                throw new ArgumentException("tableName cannot be null");
            }
            Table table = getDataContext().getTableByQualifiedLabel(tableName);
            if (table == null)
            {
                throw new ArgumentException("No such table: " + tableName);
            }
            return table;
        }
    } // TableFromBuilderImpl class
} // org.apache.metamodel.core.query.builder
