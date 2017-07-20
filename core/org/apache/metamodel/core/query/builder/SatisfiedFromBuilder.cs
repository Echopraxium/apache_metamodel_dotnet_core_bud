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
// https://github.com/apache/metamodel/blob/master/core/src/main/java/org/apache/metamodel/query/builder/SatisfiedFromBuilder.java
using org.apache.metamodel.schema;

namespace org.apache.metamodel.query.builder
{
    /**
     * Represents a builder where the FROM part is satisfied, ie. a SELECT clause is
     * now buildable.
     */
    public interface SatisfiedFromBuilder
    {
        TableFromBuilder And(Table table);
        TableFromBuilder and(string schemaName, string tableName);
        TableFromBuilder and(string tableName);
        //ColumnSelectBuilder<object>   select(Column column);
        //FunctionSelectBuilder<object> select(FunctionType function, string columnName);
        //FunctionSelectBuilder<object> select(FunctionType function, Column column);
        FunctionSelectBuilder<object> select(FunctionType function, string columnName, object[] functionParameters);
        //FunctionSelectBuilder<object> select(FunctionType function, Column column, object[] functionParameters);
        //CountSelectBuilder<object> selectCount();
        SatisfiedSelectBuilder<object> select(params Column[] columns);
        SatisfiedSelectBuilder<object> selectAll();
        SatisfiedSelectBuilder<object> select(string selectExpression);
        SatisfiedSelectBuilder<object> select(string selectExpression, bool allowExpressionBasedSelectItem);
        SatisfiedSelectBuilder<object> select(params string[] columnNames);
    } // SatisfiedFromBuilder interface
}
