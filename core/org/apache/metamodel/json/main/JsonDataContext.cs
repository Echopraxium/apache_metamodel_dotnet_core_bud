
// https://github.com/apache/metamodel/blob/master/json/src/main/java/org/apache/metamodel/json/JsonDataContext.java

using org.apache.metamodel.j2n.slf4j;
using org.apache.metamodel.schema;
using System;
using System.IO;
using org.apache.metamodel.core.data;
using org.apache.metamodel.util;
using org.apache.metamodel.j2n.io;
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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/json/src/main/java/org/apache/metamodel/json/JsonDataContext.java
namespace org.apache.metamodel.json
{
    /**
    * {@link DataContext} implementation that works on JSON files or
    * {@link Resource}s.
    */
    public class JsonDataContext : QueryPostprocessDataContext// extends QueryPostprocessDataContext implements DocumentSourceProvider
    {
        private static readonly NLogger        logger = NLoggerFactory.getLogger(typeof(JsonDataContext).Name);
        //private readonly        Resource      _resource;
        //private readonly        SchemaBuilder _schemaBuilder;

        public JsonDataContext(NInputStream input_stream)
        {
        } // constructor

        //public JsonDataContext(File file)
        //{
        //    this(new FileResource(file));
        //}

        public JsonDataContext(Resource resource)
        {
          // this(resource, new SingleTableInferentialSchemaBuilder(resource));
        } // constructor

        //public JsonDataContext(Resource resource, SchemaBuilder schemaBuilder)
        //{
        //    _resource = resource;
        //    _schemaBuilder = schemaBuilder;
        //}

        protected override Schema getMainSchema() // throws MetaModelException
        {
            throw new NotImplementedException();
            //_schemaBuilder.offerSources(this);
            //return _schemaBuilder.build();
        } // getMainSchema()

        protected override String getMainSchemaName() // throws MetaModelException
        {
           //  return _schemaBuilder.getSchemaName();
            throw new NotImplementedException();
        } // getMainSchemaName()

        protected override DataSet materializeMainSchemaTable(Table table, Column[] columns, int maxRows)
        {
            throw new NotImplementedException();
        } // materializeMainSchemaTable()

        //protected DataSet materializeMainSchemaTable(Table table, Column[] columns, int maxRows)
        //{
        //    final DocumentConverter documentConverter = _schemaBuilder.getDocumentConverter(table);
        //    final SelectItem[] selectItems = MetaModelHelper.createSelectItems(columns);
        //    final DataSetHeader header = new CachingDataSetHeader(selectItems);
        //    final DocumentSource documentSource = getDocumentSourceForTable(table.getName());

        //    DataSet dataSet = new DocumentSourceDataSet(header, documentSource, documentConverter);

        //    if (maxRows > 0)
        //    {
        //        dataSet = new MaxRowsDataSet(dataSet, maxRows);
        //    }

        //    return dataSet;
        //} // materializeMainSchemaTable()

        //private DocumentSource createDocumentSource()
        //{
        //    CsStream inputCsStream = _resource.read();
        //    try
        //    {
        //        MappingJsonFactory jsonFactory = new MappingJsonFactory();
        //        JsonParser parser = jsonFactory.createParser(inputStream);
        //        logger.debug("Created JSON parser for resource: {}", _resource);

        //        return new JsonDocumentSource(parser, _resource.getName());
        //    }
        //    catch (Exception e)
        //    {
        //        FileHelper.safeClose(inputStream);
        //        throw new MetaModelException("Unexpected error while creating JSON parser", e);
        //    }
        //} // createDocumentSource()

        //public DocumentSource getMixedDocumentSourceForSampling()
        //{
        //    return new MaxRowsDocumentSource(createDocumentSource(), 1000);
        //}

        //public DocumentSource getDocumentSourceForTable(String sourceCollectionName)
        //{
        //    // only a single source collection - returning that
        //    return createDocumentSource();
        //}
    } // JsonDataContext class
} // org.apache.metamodel.json
