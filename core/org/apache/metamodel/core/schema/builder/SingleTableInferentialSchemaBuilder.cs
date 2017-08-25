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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/schema/builder/SingleTableInferentialSchemaBuilder.java
using org.apache.metamodel.core.convert;
using org.apache.metamodel.core.data;
using org.apache.metamodel.core.util;
using org.apache.metamodel.schema;
using org.apache.metamodel.util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace org.apache.metamodel.core.schema.builder
{
    /**
     * {@link InferentialSchemaBuilder} that produces a single table.
     */
    public class SingleTableInferentialSchemaBuilder : InferentialSchemaBuilder
    {
        private String _tableName;

        #region -------- Constructors --------
        public SingleTableInferentialSchemaBuilder(Resource resource) :
               this(ResourceUtils.getParentName(resource), resource.getName())
        {
            Debug.WriteLine("new SingleTableInferentialSchemaBuilder(Resource)");
        } // constructor

        public SingleTableInferentialSchemaBuilder(String schemaName, String tableName) : base(schemaName)
        {
            Debug.WriteLine("new SingleTableInferentialSchemaBuilder(String, String)");
            _tableName = tableName;
        } // constructor
        #endregion Constructors


        // @Override
        public void offerSources(DocumentSourceProvider documentSourceProvider)
        {
            DocumentSource documentSource = documentSourceProvider.getMixedDocumentSourceForSampling();
            getTableBuilder(_tableName).offerSource(documentSource);
        } // offerSources()

        // @Override
        public override String determineTable(Document document)
        {
            return _tableName;
        } // determineTable()

        // @Override
        public override DocumentConverter getDocumentConverter(Table table)
        {
            return new ColumnNameAsKeysRowConverter();
        } // getDocumentConverter()
    } // SingleTableInferentialSchemaBuilder class
} // org.apache.metamodel.core.schema.builder
