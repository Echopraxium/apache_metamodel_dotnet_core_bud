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
using org.apache.metamodel.core.schema.builder;
using org.apache.metamodel.core.util;
using org.apache.metamodel.j2n;
using org.apache.metamodel.j2n.collections;
using org.apache.metamodel.j2n.exceptions;
using org.apache.metamodel.j2n.io;
using org.apache.metamodel.query;
using org.apache.metamodel.schema;
using org.apache.metamodel.util;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace org.apache.metamodel.json
{
    //using NUnit.Framework;

    // https://xunit.github.io/docs/getting-started-desktop.html
    public class JsonDataContextTest : NTestCase
    {
        private string _resources_root_path = "";

        public JsonDataContextTest() : base()
        {
            _resources_root_path = _assembly_path + "json\\test\\resources\\";
        } // constructor

        public void testReadArrayWithDocumentsFile() // throws Exception
        {
            test("testReadArrayWithDocumentsFile", "array_with_documents.json"); 
        } // testReadArrayWithDocumentsFile()

        public void testDocumentsOnEveryLineFile() //throws Exception
        {
            test("testDocumentsOnEveryLineFile", "documents_on_every_line.json");
        } // testDocumentsOnEveryLineFile()

        public void testUseAnotherSchemaBuilder() // throws Exception
        {
            SchemaBuilder schemaBuilder = new SingleMapColumnSchemaBuilder("sch", "tbl", "cl");
            string        json_path     = _resources_root_path + "documents_on_every_line.json";

            if (! checkIfJsonFileExists(json_path))
            {
                throw new NResourceException("JsonDataContextTest.testDocumentsOnEveryLineFile()\n   File Not Found: '" + json_path + "'");
            }
            JsonDataContext dc = new JsonDataContext(new FileResource(json_path), schemaBuilder);

            Table table = dc.getDefaultSchema().getTable(0);
            Debug.Assert("tbl".Equals(table.getName()));

            //Debug.Assert("[cl]".Equals(table.getColumnNames().ToString()));
            Debug.Assert("[cl]".Equals(table.getColumnNames().ToString()));

            DataSet dataSet = dc.query().from("tbl").select("cl").execute();
            Debug.Assert (dataSet.next());

            // Debug.Assert("Row[values=[{id=1234, name=John Doe, country=US}]]".Equals(dataSet.getRow().ToString()));
            Debug.Assert("Row[values=[{id=1234, name=John Doe, country=US}]]".Equals(dataSet.getRow().ToString()));

            Debug.Assert(! dataSet.next());
            Debug.Assert("Row[values=[{id=1235, name=Jane Doe, country=USA, gender=F}]]".Equals(dataSet.getRow().ToString()));
            Debug.Assert(! dataSet.next());
            dataSet.close();
        } // testUseAnotherSchemaBuilder()

        private void runParseabilityTest(JsonDataContext dc)
        {
            Schema  default_schema = dc.getDefaultSchema();
            Debug.WriteLine("JsonDataContextTest.runParseabilityTest() default_schema:'" + default_schema.getName() + "'");
            int     table_count    = default_schema.getTableCount();
            Table[] tables         = default_schema.getTables(false);

            string expected_value = "";
            string value_returned = "";

            string[] column_names = null;
            string   tables_str   = "";
            foreach (Table t in tables)
            {
                column_names = t.getColumnNames();
                tables_str   = NArrays.ArrayAsString(column_names);
            }

            Table table    = default_schema.getTable(0);
            column_names   = table.getColumnNames();

            //string tables_str = string.Join(", ", column_names);  // column_names.ToString();
            tables_str = NArrays.ArrayAsString(column_names);

            
            expected_value = "[country, gender, id, name]";
            value_returned = tables_str;
            Debug.WriteLine("JsonDataContextTest.runParseabilityTest()  tables_str: '" + tables_str);
            Debug.Assert(expected_value.Equals(tables_str),
                          "JsonDataContextTest.runParseabilityTest()\n"
                        + "    *Assertion Failed* expected_value: '" + expected_value + "'\n"
                        + "                       test_value:     '" + value_returned + "'"
                        );
            Column[] columns = table.getColumns();

            DataSet dataSet = dc.materializeMainSchemaTable(table, columns, 100000);
            Debug.Assert(dataSet.next());

            expected_value = "Row[values=[US, null, 1234, John Doe]]";
            value_returned = dataSet.getRow().ToString();
            Debug.Assert("Row[values=[US, null, 1234, John Doe]]".Equals(value_returned),
                           "JsonDataContextTest.runParseabilityTest()\n"
                         + "    *Assertion Failed* expected_value: '" + expected_value + "'\n"
                         + "                       test_value:     '" + value_returned + "'");

            Debug.Assert(! dataSet.next());
            Debug.Assert("Row[values=[USA, F, 1235, Jane Doe]]".Equals(dataSet.getRow().ToString()));
            Debug.Assert(! dataSet.next());
            dataSet.close();
        } // runParseabilityTest()

        public void testUseCustomTableDefWithNestedColumnDefinition() // throws Exception
        {
            SimpleTableDef tableDef = new SimpleTableDef("mytable",
                                            new string[]
                                            { "name.first", "name.last",
                                              "gender", "interests[0]", "interests[0].type",
                                              "interests[0].name" });

            Resource        resource      = new FileResource("src/test/resources/nested_fields.json");
            SchemaBuilder   schemaBuilder = new SimpleTableDefSchemaBuilder("myschema", tableDef);
            JsonDataContext dataContext   = new JsonDataContext(resource, schemaBuilder);

            DataSet ds = dataContext.query().from("mytable").selectAll().execute();
            try
            {
                Debug.Assert(ds.next());
                Debug.Assert("Row[values=[John, Doe, MALE, football, null, null]]".Equals(ds.getRow().ToString()));
                Debug.Assert(ds.next());
                Debug.Assert("Row[values=[John, Doe, MALE, {type=sport, name=soccer}, sport, soccer]]"
                             .Equals(ds.getRow().ToString()));
                Debug.Assert(! ds.next());
            }
            finally
            {
                ds.close();
            }
        } // testUseCustomTableDefWithNestedColumnDefinition()

        public void testUseMapValueFunctionToGetFromNestedMap() // throws Exception
        {
            Resource        resource    = new FileResource("src/test/resources/nested_fields.json");
            JsonDataContext dataContext = new JsonDataContext(resource);

            Schema schema = dataContext.getDefaultSchema();
            Debug.Assert("[nested_fields.json]".Equals(NArrays.AsString(schema.getTableNames(false))));

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
            Debug.Assert("[nested_fields.json]".Equals(NArrays.AsString(schema.getTableNames(false))));

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


        public void test(string test_name, string resource_filename) //throws Exception
        {
            Debug.WriteLine("---------- " + test_name + " ----------");
            string json_path = _resources_root_path + resource_filename;
            if (! checkIfJsonFileExists(json_path))
            {
                throw new NResourceException("JsonDataContextTest.testDocumentsOnEveryLineFile()\n"
                                             + "    File Not Found: '" + json_path + "'");
            }

            Debug.WriteLine("> OK FOUND: " + json_path);
            JsonDataContext dc = new JsonDataContext(new NInputStream(json_path, FileMode.Open));
            runParseabilityTest(dc);
        } // test()


        public bool checkIfJsonFileExists(string json_path)
        {
            if (!File.Exists(json_path))
            {
                Debug.WriteLine("*Error* FILE NOT FOUND json_path: " + json_path);
                return false;
            }
            return true;
        } // checkIfJsonFileExists()

    } // JsonDataContextTest class
} // org.apache.metamodel.json
