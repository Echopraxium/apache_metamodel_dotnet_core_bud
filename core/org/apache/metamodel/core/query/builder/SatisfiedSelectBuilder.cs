﻿
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/query/builder/SatisfiedSelectBuilder.java
//import org.apache.metamodel.query.FunctionType;
//import org.apache.metamodel.schema.Column;

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
    public interface SatisfiedSelectBuilder<B> : SatisfiedQueryBuilder<B> // where B : SatisfiedQueryBuilder 
    {
        //ColumnSelectBuilder<B>    And(Column column);
        SatisfiedSelectBuilder<B>   And(params Column[] columns);
        //FunctionSelectBuilder<B>  And(FunctionType function, Column column);
        SatisfiedSelectBuilder<B>   And(string columnName);
    } // SatisfiedSelectBuilder interface
} // org.apache.metamodel.query.builder namespace