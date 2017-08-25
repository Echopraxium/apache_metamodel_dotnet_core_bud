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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/schema/builder/SingleMapColumnSchemaBuilder.java
using org.apache.metamodel.core.convert;
using org.apache.metamodel.core.data;
using org.apache.metamodel.core.util;
using org.apache.metamodel.data;
using org.apache.metamodel.schema;
using org.apache.metamodel.util;
using System;
using System.Diagnostics;

namespace org.apache.metamodel.core.schema.builder
{
    /**
     * A very simple {@link SchemaBuilder} that builds a schema consisting of 1
     * table with 1 column, of type {@link Map}.
     */
    public class SingleMapColumnSchemaBuilder : SchemaBuilder, DocumentConverter
    {
        private String _schemaName;
        private String _tableName;
        private String _columnName;

        public SingleMapColumnSchemaBuilder(Resource resource, String columnName) :
                this(ResourceUtils.getParentName(resource), resource.getName(), columnName)
        {           
        }

        public SingleMapColumnSchemaBuilder(String schemaName, String tableName, String columnName)
        {
            _schemaName = schemaName;
            _tableName = tableName;
            _columnName = columnName;
        } // constructor

        // @Override
        public void offerSources(DocumentSourceProvider documentSourceProvider)
        {
            // do nothing
        }

        // @Override
        public String getSchemaName()
        {
            return _schemaName;
        }

        // @Override
        public MutableSchema build()
        {
            MutableSchema schema = new MutableSchema(_schemaName);
            MutableTable table = new MutableTable(_tableName, schema);
            table.addColumn(new MutableColumn(_columnName, ColumnTypeConstants.MAP, table, 1, null, null, false, null, false, null));
            schema.addTable(table);
            return schema;
        }

        // @Override
        public DocumentConverter getDocumentConverter(Table table)
        {
            return this;
        }

        // @Override
        public Row convert(Document document, DataSetHeader header)
        {
            Debug.Assert(header.size() == 1);
            Object[] values = new Object[1];
            values[0] = document.getValues();
            return new DefaultRow(header, values);
        }
    } // SingleMapColumnSchemaBuilder class
} // org.apache.metamodel.core.schema.builder
