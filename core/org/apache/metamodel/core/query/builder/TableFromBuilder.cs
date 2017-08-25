﻿
// https://github.com/apache/metamodel/blob/master/core/src/main/java/org/apache/metamodel/query/builder/TableFromBuilder.java
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
    // https://github.com/apache/metamodel/blob/master/core/src/main/java/org/apache/metamodel/query/builder/TableFromBuilder.java
    public interface TableFromBuilder : SatisfiedFromBuilder
    {
        JoinFromBuilder  innerJoin(Table table);
        JoinFromBuilder  innerJoin(string tableName);
        JoinFromBuilder  leftJoin(Table table);
        JoinFromBuilder  leftJoin(string tableName);
        JoinFromBuilder  rightJoin(Table table);
        JoinFromBuilder  rightJoin(string tableName);
        TableFromBuilder As(string alias);
    } // TableFromBuilder interface
} // org.apache.metamodel.query.builder namespace