﻿/**
* Licensed to the Apache Software Foundation (ASF) under one
* or more contributor license agreements. See the NOTICE file
* distributed with this work for additional information
* regarding copyright ownership. The ASF licenses this file
* to you under the Apache License, Version 2.0 (the
* "License"); you may not use this file except in compliance
* with the License. You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing,
* software distributed under the License is distributed on an
* "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
* KIND, either express or implied. See the License for the
* specific language governing permissions and limitations
* under the License.
*/
// https://github.com/apache/metamodel/blob/master/core/src/main/java/org/apache/metamodel/query/AggregateFunction.java
using org.apache.metamodel.util;

namespace org.apache.metamodel.query
{
    /**
     * Interface that contains the aggregation specific methods
     * related to the AggregateBuilder.
     *
     */
    public interface AggregateFunction : FunctionType
    {
        /**
         * Creates a specific aggregate builder.
         *
         * @return an AggregateBuilder instance
         */
        AggregateBuilder createAggregateBuilder();

        /**
         * Shorthand for creating an aggregate builder, adding all
         * the values and then calculating the value.
         *
         * @param values
         * @return the aggregated value
         */
        object evaluate(params object[] values);

        /**
         * Returns the function ColumnType.
         *
         * @param type
         * @return the ColumnType
         */
        // public abstract ColumnType getExpectedColumnType(ColumnType type);
    } // AggregateFunction class
} // org.apache.metamodel.query namespace
