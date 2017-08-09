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
// https://github.com/apache/metamodel/blob/master/json/src/test/java/org/apache/metamodel/json/JsonDataContextTest.java
using org.apache.metamodel.core.data;
using org.apache.metamodel.j2n.collections;
using org.apache.metamodel.query;
using org.apache.metamodel.schema;
using org.apache.metamodel.util;
using System.Diagnostics;
using System.IO;
//import junit.framework.TestCase;
//import org.apache.metamodel.data.DataSet;
//import org.apache.metamodel.schema.Table;
//import org.apache.metamodel.schema.builder.SchemaBuilder;
//import org.apache.metamodel.schema.builder.SimpleTableDefSchemaBuilder;
//import org.apache.metamodel.schema.builder.SingleMapColumnSchemaBuilder;
//import org.apache.metamodel.util.FileResource;
//import org.apache.metamodel.util.Resource;
//import org.apache.metamodel.util.SimpleTableDef;

namespace org.apache.metamodel.json
{
    public class JsonDataContextTest //: TestCase
    {
        //public void testReadArrayWithDocumentsFile() // throws Exception
        //{
        //    string          path = "src\\test\\resources\\array_with_documents.json";
        //    JsonDataContext dc   = new JsonDataContext(new NInputStream(path, FileMode.Open));
        //    runParseabilityTest(dc);
        //} // testReadArrayWithDocumentsFile()

        //public void testDocumentsOnEveryLineFile() //throws Exception
        //{
        //    string path = "src\\test\\resources\\documents_on_every_line.json";
        //    JsonDataContext dc = new JsonDataContext(new NInputStream(path, FileMode.Open));
        //    runParseabilityTest(dc);
        //} // testDocumentsOnEveryLineFile()

        //public void testUseAnotherSchemaBuilder() // throws Exception
        //{
        //    SchemaBuilder schemaBuilder = new SingleMapColumnSchemaBuilder("sch", "tbl", "cl");
        //    JsonDataContext dc = new JsonDataContext
        //                                        (new FileResource(
        //                                         "src/test/resources/documents_on_every_line.json"),
        //                                         schemaBuilder);

        //    Table table = dc.getDefaultSchema().getTable(0);
        //    Debug.Assert("tbl".Equals(table.getName()));

        //    Debug.Assert("[cl]".Equals(Arrays.AsString(table.getColumnNames())));

        //    DataSet dataSet = dc.query().from("tbl").select("cl").execute();
        //    Debug.Assert(dataSet.next());
        //    Debug.Assert("Row[values=[{id=1234, name=John Doe, country=US}]]".Equals(dataSet.getRow().ToString()));
        //    Debug.Assert(! dataSet.next());
        //    Debug.Assert("Row[values=[{id=1235, name=Jane Doe, country=USA, gender=F}]]".Equals(dataSet.getRow().ToString()));
        //    Debug.Assert(! dataSet.next());
        //    dataSet.close();
        //} // testUseAnotherSchemaBuilder()

        //private void runParseabilityTest(JsonDataContext dc)
        //{
        //    Table table = dc.getDefaultSchema().getTable(0);
        //    Debug.Assert("[country, gender, id, name]".Equals(Arrays.AsString(table.getColumnNames())));
        //    Column[] columns = table.getColumns();

        //    DataSet dataSet = dc.materializeMainSchemaTable(table, columns, 100000);
        //    Debug.Assert(dataSet.next());
        //    Debug.Assert("Row[values=[US, null, 1234, John Doe]]".Equals(dataSet.getRow().ToString()));
        //    Debug.Assert(! dataSet.next());
        //    Debug.Assert("Row[values=[USA, F, 1235, Jane Doe]]".Equals(dataSet.getRow().ToString()));
        //    Debug.Assert(! dataSet.next());
        //    dataSet.close();
        //} // runParseabilityTest()

        //public void testUseCustomTableDefWithNestedColumnDefinition() // throws Exception
        //{
        //    SimpleTableDef tableDef = new SimpleTableDef("mytable",
        //                                    new string[]
        //                                    { "name.first", "name.last",
        //                                      "gender", "interests[0]", "interests[0].type",
        //                                      "interests[0].name" });

        //    Resource resource = new FileResource("src/test/resources/nested_fields.json");
        //    SchemaBuilder schemaBuilder = new SimpleTableDefSchemaBuilder("myschema", tableDef);
        //    JsonDataContext dataContext = new JsonDataContext(resource, schemaBuilder);

        //    DataSet ds = dataContext.query().from("mytable").selectAll().execute();
        //    try
        //    {
        //        Debug.Assert(ds.next());
        //        Debug.Assert("Row[values=[John, Doe, MALE, football, null, null]]".Equals(ds.getRow().ToString()));
        //        Debug.Assert(ds.next());
        //        Debug.Assert("Row[values=[John, Doe, MALE, {type=sport, name=soccer}, sport, soccer]]"
        //                     .Equals(ds.getRow().ToString()));
        //        Debug.Assert(! ds.next());
        //    }
        //    finally
        //    {
        //        ds.close();
        //    }
        //} // testUseCustomTableDefWithNestedColumnDefinition()

        public void testUseMapValueFunctionToGetFromNestedMap() // throws Exception
        {
            Resource        resource    = new FileResource("src/test/resources/nested_fields.json");
            JsonDataContext dataContext = new JsonDataContext(resource);

            Schema schema = dataContext.getDefaultSchema();
            Debug.Assert("[nested_fields.json]".Equals(NArrays.AsString(schema.getTableNames())));

            DataSet ds = dataContext
                          .query()
                          .from(schema.getTable(0))
                          .select(FunctionTypeConstants.MAP_VALUE, "name", new object[] { "first" })
                          .execute();
            try
            {
                Debug.Assert(ds.next());
                Debug.Assert("Row[values=[John]]".Equals(ds.getRow().ToString()));
                Debug.Assert(ds.next());
                Debug.Assert("Row[values=[John]]".Equals(ds.getRow().ToString()));
                Debug.Assert(! ds.next());
            }
            finally
            {
                ds.close();
            }
        } // testUseMapValueFunctionToGetFromNestedMap()

        public void testUseDotNotationToGetFromNestedMap() // throws Exception
        {
            Resource resource = new FileResource("src/test/resources/nested_fields.json");
            JsonDataContext dataContext = new JsonDataContext(resource);

            Schema schema = dataContext.getDefaultSchema();
            Debug.Assert("[nested_fields.json]".Equals(NArrays.AsString(schema.getTableNames())));

            DataSet ds = dataContext.query().from(schema.getTable(0)).select("name.first").execute();
            try
            {
                Debug.Assert(ds.next());
                Debug.Assert("Row[values=[John]]".Equals(ds.getRow().ToString()));
                Debug.Assert(ds.next());
                Debug.Assert("Row[values=[John]]".Equals(ds.getRow().ToString()));
                Debug.Assert(! ds.next());
            }
            finally
            {
                ds.close();
            }
        } // testUseDotNotationToGetFromNestedMap()
    } // JsonDataContextTest class
} // org.apache.metamodel.json
