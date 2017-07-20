
//import org.apache.metamodel.schema.Schema;
//import org.apache.metamodel.schema.Table;
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/query/builder/InitFromBuilder.java

using org.apache.metamodel.schema;
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
namespace org.apache.metamodel.query.builder
{
    /**
     * The initial interface used when building a query. A query starts by stating
     * the FROM clause.
     */
    public interface InitFromBuilder
    {
        TableFromBuilder from(Table table);
        TableFromBuilder from(Schema schema, string tableName);
        TableFromBuilder from(string schemaName, string tableName);
        TableFromBuilder from(string tableName);
    } // InitFromBuilder class
} // org.apache.metamodel.query.builder namespace
