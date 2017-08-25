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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/schema/builder/SimpleTableDefSchemaBuilder.java
using org.apache.metamodel.core.convert;
using org.apache.metamodel.core.util;
using org.apache.metamodel.schema;
using System;

namespace org.apache.metamodel.core.schema.builder
{
    /**
     * A {@link SchemaBuilder} that builds a schema according to instructions in the
     * form of {@link SimpleTableDef} objects.
     */
    public class SimpleTableDefSchemaBuilder : SchemaBuilder
    {
        private String           _schemaName;
        private SimpleTableDef[] _simpleTableDefs;

        public SimpleTableDefSchemaBuilder(String schemaName, params SimpleTableDef[] simpleTableDefs)
        {
            _schemaName      = schemaName;
            _simpleTableDefs = simpleTableDefs;
        } // constructor

        // @Override
        public void offerSources(DocumentSourceProvider documentSource)
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
            foreach (SimpleTableDef simpleTableDef in _simpleTableDefs)
            {
                MutableTable table = simpleTableDef.toTable();
                schema.addTable(table);
                table.setSchema(schema);
            }
            return schema;
        }

        // @Override
        public DocumentConverter getDocumentConverter(Table table)
        {
            return new ColumnNameAsKeysRowConverter();
        }
    } // SimpleTableDefSchemaBuilder class
} // org.apache.metamodel.core.schema.builder
