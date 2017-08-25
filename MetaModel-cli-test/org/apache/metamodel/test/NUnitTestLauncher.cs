/**
 * Licensed to the Apache Software Foundation(ASF) under one
 * or more contributor license agreements.See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */
using org.apache.metamodel.json;
using System.Diagnostics;

namespace org.apache.metamodel.test
{
    public class NUnitTestLauncher
    {
        public static int Run()
        {
            Debug.WriteLine(">> NUnitTestLauncher.Run()");
            JsonDataContextTest json_test = new JsonDataContextTest();

            json_test.testDocumentsOnEveryLineFile();
            //json_test.testUseAnotherSchemaBuilder();
            return 0;
        } // Run()
    } // NUnitTestLauncher class
} // org.apache.metamodel.test
