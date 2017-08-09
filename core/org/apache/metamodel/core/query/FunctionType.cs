/**
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
// https://github.com/apache/metamodel/blob/master/core/src/main/java/org/apache/metamodel/query/FunctionType.java
using System;
using org.apache.metamodel.j2n.attributes;
using org.apache.metamodel.schema;

namespace org.apache.metamodel.query
{
    /**
     * Represents a generic function to use in a SelectItem.
     *
     * @see SelectItem
     */
    [NSerializableAttribute]
    public interface FunctionType // : ISerializable
    {
        ColumnType getExpectedColumnType(ColumnType type);
        string     getFunctionName();
        //public void GetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    throw new NotImplementedException();
        //}
    } // FunctionType class

    public abstract class FunctionTypeConstants : FunctionType
    {
        //[J2Cs: missing dependency; comment out]
        public static readonly AggregateFunction COUNT;      // = new CountAggregateFunction();
        public static readonly AggregateFunction AVG;        // = new AverageAggregateFunction();
        public static readonly AggregateFunction SUM;        // = new SumAggregateFunction();
        public static readonly AggregateFunction MAX;        // =     = new MaxAggregateFunction();
        public static readonly AggregateFunction MIN;        // =   = new MinAggregateFunction();
        public static readonly AggregateFunction RANDOM;     // =   = new RandomAggregateFunction();
        public static readonly AggregateFunction FIRST;      // =   = new FirstAggregateFunction();
        public static readonly AggregateFunction LAST;       // =   = new LastAggregateFunction();
        public static readonly ScalarFunction    TO_STRING;  // == new ToStringFunction();
        public static readonly ScalarFunction    TO_NUMBER;  // = = new ToNumberFunction();
        public static readonly ScalarFunction    TO_DATE;    // = = new ToDateFunction();
        public static readonly ScalarFunction    TO_BOOLEAN; // = = new ToBooleanFunction();
        public static readonly ScalarFunction    MAP_VALUE;  // = = new MapValueFunction();

        public abstract ColumnType getExpectedColumnType(ColumnType type);
        public abstract string getFunctionName();
    } // FunctionTypeConstants class
} // org.apache.metamodel.query namespace
