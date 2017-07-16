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
// https://github.com/apache/metamodel/blob/master/core/src/main/java/org/apache/metamodel/schema/SuperColumnType.java
using org.apache.metamodel.j2cs.data;
using org.apache.metamodel.j2cs.data.numbers;
using System;

namespace org.apache.metamodel.schema
{
    /**
     * Represents an abstract, generalized type of column
     */
    public class SuperColumnType
    {
        // private Class<?> _javaEquivalentClass;
        private Type _dot_net_equivalent_class;

        public static readonly SuperColumnType BOOLEAN_TYPE = new SuperColumnType(typeof(bool));
        public static readonly SuperColumnType LITERAL_TYPE = new SuperColumnType(typeof(string));
        public static readonly SuperColumnType NUMBER_TYPE  = new SuperColumnType(typeof(CsNumber));
        public static readonly SuperColumnType TIME_TYPE    = new SuperColumnType(typeof(DateTime));
        public static readonly SuperColumnType BINARY_TYPE  = new SuperColumnType(typeof(byte[]));
        public static readonly SuperColumnType OTHER_TYPE   = new SuperColumnType(typeof(object));

        private SuperColumnType(Type _dot_net_equivalent_class_arg)
        {
            _dot_net_equivalent_class = _dot_net_equivalent_class_arg;
        } // constructor

        /*
         * @return a java class that is appropriate for handling column values of
         *         this column super type
         */
        public Type getJavaEquivalentClass()
        {
             return _dot_net_equivalent_class;
        }
        public Type getDotNetEquivalentClass()
        {
            return _dot_net_equivalent_class;
        } // getDotNetEquivalentClass()
    } // SuperColumnType class
} // org.apache.metamodel.schema namespace
