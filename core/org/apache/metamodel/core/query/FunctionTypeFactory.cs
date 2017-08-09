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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/query/FunctionTypeFactory.java
using org.apache.metamodel.query;
using org.apache.metamodel.j2n.data;

namespace org.apache.metamodel.core.query
{
    /**
     * Factory to create AggregateFunctions through its function name.
     *
     */
    public class FunctionTypeFactory
    {
        public static FunctionType get(string functionName)
        {
            if (functionName == null || functionName.IsEmpty())
            {
                return null;
            }

            functionName = functionName.ToUpper();

            switch (functionName)
            {
                case "COUNT":
                    return FunctionTypeConstants.COUNT;
                case "AVG":
                    return FunctionTypeConstants.AVG;
                case "SUM":
                    return FunctionTypeConstants.SUM;
                case "MAX":
                    return FunctionTypeConstants.MAX;
                case "MIN":
                    return FunctionTypeConstants.MIN;
                case "RANDOM":
                case "RAND":
                    return FunctionTypeConstants.RANDOM;
                case "FIRST":
                    return FunctionTypeConstants.FIRST;
                case "LAST":
                    return FunctionTypeConstants.LAST;
                case "TO_NUMBER":
                case "NUMBER":
                case "TO_NUM":
                case "NUM":
                    return FunctionTypeConstants.TO_NUMBER;
                case "TO_STRING":
                case "STRING":
                case "TO_STR":
                case "STR":
                    return FunctionTypeConstants.TO_STRING;
                case "TO_BOOLEAN":
                case "BOOLEAN":
                case "TO_BOOL":
                case "BOOL":
                    return FunctionTypeConstants.TO_BOOLEAN;
                case "TO_DATE":
                case "DATE":
                    return FunctionTypeConstants.TO_DATE;
                case "MAP_VALUE":
                    return FunctionTypeConstants.MAP_VALUE;
                default:
                    return null;
            }
        } // get()
    } // FunctionTypeFactory class
} //  org.apache.metamodel.core.query
