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
// https://github.com/apache/metamodel/blob/57e48be23844537690bdd6191ceb78adb4266e92/core/src/main/java/org/apache/metamodel/query/builder/InitFromBuilderImpl.java
using org.apache.metamodel.j2n.collections;
using org.apache.metamodel.query;
using org.apache.metamodel.query.builder;
using org.apache.metamodel.schema;
using org.apache.metamodel.util;
using org.apache.metamodel.core.query.builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.apache.metamodel.core.schema.builder
{
    public sealed class InitFromBuilderImpl : BaseObject, InitFromBuilder
    {

        private DataContext dataContext;
        private Query query;

        public InitFromBuilderImpl(DataContext dataContext)
        {
            this.dataContext = dataContext;
            this.query = new Query();
        } // constructor

        // @Override
        public TableFromBuilder from(Table table)
        {
            if (table == null)
            {
                throw new ArgumentException("table cannot be null");
            }
            return new TableFromBuilderImpl(table, query, dataContext);
        }

        // @Override
        public TableFromBuilder from(String schemaName, String tableName)
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
            return from(schema, tableName);
        }

        // @Override
        public TableFromBuilder from(Schema schema, String tableName)
        {
            Table table = schema.getTableByName(tableName);
            if (table == null)
            {
                throw new ArgumentException("Nu such table '" + tableName + "' found in schema: " + schema
                        + ". Available tables are: " + NArrays.ArrayAsString(schema.getTableNames(false)));
            }
            return from(table);
        }

        // @Override
        public TableFromBuilder from(String tableName)
        {
            if (tableName == null)
            {
                throw new ArgumentException("tableName cannot be null");
            }
            Table table = dataContext.getTableByQualifiedLabel(tableName);
            if (table == null)
            {
                throw new ArgumentException("No such table: " + tableName);
            }
            return from(table);
        }

        // @Override
        protected void decorateIdentity(List<Object> identifiers)
        {
            identifiers.Add(query);
        }
    } // InitFromBuilderImpl class
} // org.apache.metamodel.core.schema.builder
