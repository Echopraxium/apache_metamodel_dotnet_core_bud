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
// https://github.com/apache/metamodel/blob/master/json/src/main/java/org/apache/metamodel/json/JsonDataContext.java

using org.apache.metamodel.j2n.slf4j;
using org.apache.metamodel.schema;
using System;
using System.IO;
using org.apache.metamodel.core.data;
using org.apache.metamodel.util;
using org.apache.metamodel.j2n.io;
using org.apache.metamodel.core.schema.builder;
using org.apache.metamodel.core.convert;
using org.apache.metamodel.query;
using org.apache.metamodel.data;
using org.apache.metamodel.core;
using org.apache.metamodel.j2n.json;
using System.Diagnostics;

namespace org.apache.metamodel.json
{
    /**
    * {@link DataContext} implementation that works on JSON files or
    * {@link Resource}s.
    */
    public class JsonDataContext : QueryPostprocessDataContext, DocumentSourceProvider // extends QueryPostprocessDataContext implements DocumentSourceProvider
    {
        private static readonly NLogger        logger = NLoggerFactory.getLogger(typeof(JsonDataContext).Name);
        private readonly        Resource      _resource;
        private readonly        SchemaBuilder _schemaBuilder;

        #region ---- Constructors ----
        public JsonDataContext(NInputStream input_stream) : this(new FileResource(input_stream))
        {
            Debug.WriteLine("new JsonDataContext(NInputStream)");
        } // constructor

        public JsonDataContext(FileStream file_stream) : this(new FileResource(new NInputStream(file_stream.SafeFileHandle, FileAccess.Read)))
        {
            Debug.WriteLine("new JsonDataContext(FileStream)");
        } // constructor

        public JsonDataContext(Resource resource) : this(resource, new SingleTableInferentialSchemaBuilder(resource))
        {
            Debug.WriteLine("new JsonDataContext(Resource)");
        } // constructor

        public JsonDataContext(Resource resource, SchemaBuilder schemaBuilder)
        {
            Debug.WriteLine("new JsonDataContext(Resource, SchemaBuilder)");
            _resource      = resource;
            _schemaBuilder = schemaBuilder;
        } // constructor
        #endregion Constructors 

        public override Schema getMainSchema() // throws MetaModelException
        {
            _schemaBuilder.offerSources((DocumentSourceProvider) this);
            return _schemaBuilder.build();
        } // getMainSchema()

        public override String getMainSchemaName() // throws MetaModelException
        {
            return _schemaBuilder.getSchemaName();
        } // getMainSchemaName()

        public override DataSet materializeMainSchemaTable(Table table, Column[] columns, int maxRows)
        {
            DocumentConverter documentConverter = _schemaBuilder.getDocumentConverter(table);
            SelectItem[]      selectItems       = MetaModelHelper.createSelectItems(columns);
            DataSetHeader     header            = new CachingDataSetHeader(selectItems);
            DocumentSource    documentSource    = getDocumentSourceForTable(table.getName());

            DataSet dataSet = new DocumentSourceDataSet(header, documentSource, documentConverter);

            if (maxRows > 0)
            {
                dataSet = new MaxRowsDataSet(dataSet, maxRows);
            }

            return dataSet;
        } // materializeMainSchemaTable()

        private DocumentSource createDocumentSource()
        {
            Debug.WriteLine("JsonDataContext.createDocumentSource()");

            NInputStream input_stream = _resource.read();
            try 
            {
                // MappingJsonFactory jsonFactory = new MappingJsonFactory();
                NJsonParser parser = new NJsonParser(input_stream); // jsonFactory.createParser(inputStream);
                logger.debug("Created JSON parser for resource: {0}", _resource);

                return new JsonDocumentSource(parser, _resource.getName());
            }
            catch (Exception e)
            {
                Debug.WriteLine("Unexpected error while creating JSON parser  \n    " + e.Message);
                try
                {
                    Debug.WriteLine("Trying to close input_stream");
                    FileHelper.safeClose(input_stream);
                }
                catch (Exception e1)
                {
                    throw new MetaModelException("Tried to close input_stream\n    " + e1.Message);
                }
                throw new MetaModelException("... Unexpected error while creating JSON parser  \n    " + e.Message);
            }
        } // createDocumentSource()

        public DocumentSource getMixedDocumentSourceForSampling()
        {
            Debug.WriteLine("JsonDataContext.getMixedDocumentSourceForSampling()");
            return new MaxRowsDocumentSource(createDocumentSource(), 1000);
        } // getMixedDocumentSourceForSampling()

        public DocumentSource getDocumentSourceForTable(String sourceCollectionName)
        {
            // only a single source collection - returning that
            return createDocumentSource();
        } // getDocumentSourceForTable()

        public Schema getSchemaByName(string name)
        {
            Debug.WriteLine("JsonDataContext.getSchemaByName('" + name + "')");
            if (name == "")
                return getMainSchema();
            return getMainSchema();
        } // getSchemaByName()
    } // JsonDataContext class
} // org.apache.metamodel.json
