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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/query/MapValueFunction.java
using org.apache.metamodel.data;
using org.apache.metamodel.query;
using org.apache.metamodel.schema;
using org.apache.metamodel.util;
using System;
using System.Collections.Generic;

namespace org.apache.metamodel.core.query
{
    /**
     * Represents a function that retrieves a value from within a column of type
     * {@link ColumnType#MAP} or similar.
     */
    public sealed class MapValueFunction : DefaultScalarFunction
    {
        private static readonly long serialVersionUID = 1L;

        // @Override
        public override Object evaluate(Row row, Object[] parameters, SelectItem operandItem)
        {
            if (parameters.Length == 0)
            {
                throw new ArgumentException("Expecting path parameter to MAP_VALUE function");
            }
            Object value = row.getValue(operandItem);
            if (value is Dictionary<object, object>)
            {
                Dictionary<object, object> map = (Dictionary<object, object>) value;
                return CollectionUtils.find(map, (String)parameters[0]);
            }
            return null;
        } // evaluate()

        // @Override
        public override ColumnType getExpectedColumnType(ColumnType type)
        {
            // the column type cannot be inferred so null is returned
            return null;
        }

        // @Override
        public override String getFunctionName()
        {
            return "MAP_VALUE";
        }
    }
}
