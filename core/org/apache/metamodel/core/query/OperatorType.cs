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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/query/OperatorType.java
using org.apache.metamodel.j2cs.attributes;
using System;

namespace org.apache.metamodel.query
{
    /**
     * Defines the types of operators that can be used in filters.
     *
     * @see FilterItem
     */
    [CsSerializableAttribute]
    public abstract class OperatorType //: ISerializable
    {

        public static readonly OperatorType EQUALS_TO             = new OperatorTypeImpl("=", false);
        public static readonly OperatorType DIFFERENT_FROM        = new OperatorTypeImpl("<>", false);
        public static readonly OperatorType LIKE                  = new OperatorTypeImpl("LIKE", true);
        public static readonly OperatorType NOT_LIKE              = new OperatorTypeImpl("NOT LIKE", true);
        public static readonly OperatorType GREATER_THAN          = new OperatorTypeImpl(">", false);
        public static readonly OperatorType GREATER_THAN_OR_EQUAL = new OperatorTypeImpl(">=", false);
        public static readonly OperatorType LESS_THAN             = new OperatorTypeImpl("<", false);
        public static readonly OperatorType LESS_THAN_OR_EQUAL    = new OperatorTypeImpl("<=", false);
        public static readonly OperatorType IN                    = new OperatorTypeImpl("IN", true);

        public static readonly OperatorType NOT_IN                = new OperatorTypeImpl("NOT IN", true);

        //public void GetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    throw new NotImplementedException();
        //}

        public static readonly OperatorType[] BUILT_IN_OPERATORS = new OperatorType[]
        {
            EQUALS_TO, DIFFERENT_FROM, LIKE,
            NOT_LIKE, GREATER_THAN, GREATER_THAN_OR_EQUAL,
            LESS_THAN, LESS_THAN_OR_EQUAL, IN, NOT_IN
        };

        /**
         * Determines if this operator requires a space delimitor. Operators that are written using letters usually require
         * space delimitation whereas sign-based operators such as "=" and "&lt;" can be applied even without any delimitaton.
         * 
         * @return
         */
        public abstract bool isSpaceDelimited();

        public abstract string toSql();
    } // OperatorType interface
} //  org.apache.metamodel.query namespace
