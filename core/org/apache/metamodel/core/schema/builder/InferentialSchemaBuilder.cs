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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/schema/builder/InferentialSchemaBuilder.java
using org.apache.metamodel.core.data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using org.apache.metamodel.core.convert;
using org.apache.metamodel.schema;
using System.Diagnostics;

namespace org.apache.metamodel.core.schema.builder
{
    public abstract class InferentialSchemaBuilder : SchemaBuilder
    {
        private ConcurrentDictionary<String, InferentialTableBuilder> _tableBuilders;
        private String                                                _schemaName;

        public InferentialSchemaBuilder(String schemaName)
        {
            Debug.WriteLine("new InferentialSchemaBuilder()");
            _schemaName    = schemaName;
            _tableBuilders = new ConcurrentDictionary<String, InferentialTableBuilder>();
        } // constructor


        // @Override
        public void offerSources(DocumentSourceProvider documentSourceProvider)
        {
            DocumentSource document_source = documentSourceProvider.getMixedDocumentSourceForSampling();
            try
            {
                while (true)
                {
                    Document document = document_source.next();
                    if (document == null)
                    {
                        break;
                    }
                    string tableName = determineTable(document);
                    addObservation(tableName, document);
                }
            }
            finally
            {
                document_source.close();
            }
        } // offerSources()

        protected void offerDocumentSource(DocumentSource documentSource)
        {
            try
            {
                while (true)
                {
                    Document document = documentSource.next();
                    if (document == null)
                    {
                        break;
                    }
                    String tableName = determineTable(document);
                    addObservation(tableName, document);
                }
            }
            finally
            {
                documentSource.close();
            }
        } // offerDocumentSource()

        // @Override
        public String getSchemaName()
        {
            return _schemaName;
        } // getSchemaName()

        /**
         * Determines which table a particular document should be mapped to.
         * 
         * @param document
         * @return
         */
        public abstract String determineTable(Document document);

        public void addObservation(String table, Document document)
        {
            InferentialTableBuilder table_nuilder = getTableBuilder(table);
            table_nuilder.addObservation(document);
        } // addObservation()

        public InferentialTableBuilder getTableBuilder(String table_name)
        {
            InferentialTableBuilder table_builder = null;
            if (_tableBuilders.ContainsKey(table_name))
                table_builder = _tableBuilders[table_name];

            if (table_builder == null)
            {
                table_builder = new InferentialTableBuilder(table_name);
                _tableBuilders[table_name] = table_builder;
                //InferentialTableBuilder existingTableBuilder = null;

                //if (_tableBuilders.ContainsKey(table_name))
                //    existingTableBuilder = _tableBuilders[table_name];
                //else
                //if (_tableBuilders.ContainsKey(table))
                //    existingTableBuilder = _tableBuilders[table] = table_builder;
                //if (existingTableBuilder != null)
                //{
                //    table_builder = existingTableBuilder;
                //}
            }
            return table_builder;
        } // getTableBuilder()

        // @Override
        public MutableSchema build()
        {
            MutableSchema schema = new MutableSchema(_schemaName);

            // Sort table names by moving them to a treeset
            HashSet<String> table_names = new HashSet<String>(_tableBuilders.Keys);  //new TreeSet<String>(_tableBuilders.keySet());

            foreach (String table_name in table_names)
            {
                MutableTable table = buildTable(getTableBuilder(table_name));
                table.setSchema(schema);
                schema.addTable(table);
            }

            return schema;
        } // build()

        protected MutableTable buildTable(InferentialTableBuilder tableBuilder)
        {
            return tableBuilder.buildTable();
        }

        public abstract DocumentConverter getDocumentConverter(Table table);
    } // InferentialSchemaBuilder class
} // org.apache.metamodel.core.schema.builder
