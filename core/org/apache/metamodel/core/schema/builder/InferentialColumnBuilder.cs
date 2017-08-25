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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/schema/builder/InferentialColumnBuilder.java
using org.apache.metamodel.j2n.data.numbers;
using org.apache.metamodel.schema;
using org.apache.metamodel.j2n.collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.apache.metamodel.core.schema.builder
{
    public class InferentialColumnBuilder : ColumnBuilder
    {

        private String              _name;
        private HashSet<ColumnType> _columnTypes;
        private NAtomicInteger      _observationCounter;
        private bool                _nulls;

        public InferentialColumnBuilder(String columnName)
        {
            _name               = columnName;
            _columnTypes        = new HashSet<ColumnType>();
            _observationCounter = new NAtomicInteger(0);
            _nulls              = false;
        }

        public void addObservation(Object value)
        {
            _observationCounter.incrementAndGet();
            if (value == null)
            {
                _nulls = true;
                return;
            }
            Type valueType = value.GetType();
            addObservation(valueType);
        }

        public void addObservation(ColumnType columnType)
        {
            _observationCounter.incrementAndGet();
            if (columnType == null)
            {
                columnType = ColumnTypeConstants.OTHER;
            }
            _columnTypes.Add(columnType);
        } // addObservation()

        public void addObservation(Type valueType)
        {
            ColumnType columnType = ColumnTypeImpl.convertColumnType(valueType);
            addObservation(columnType);
        }

        /**
         * Gets the number of observations that this column builder is basing it's
         * inference on.
         * 
         * @return
         */
        public int getObservationCount()
        {
            return _observationCounter.asInt();
        }

        // @Override
        public MutableColumn build()
        {
            MutableColumn column = new MutableColumn(_name);
            column.setType(detectType());
            if (_nulls)
            {
                column.setNullable(true);
            }
            return column;
        }

        private ColumnType detectType()
        {
            if (_columnTypes.IsEmpty())
            {
                return ColumnTypeConstants.OTHER;
            }

            if (_columnTypes.Count == 1)
            {
                _columnTypes.GetEnumerator().MoveNext();
                return _columnTypes.GetEnumerator().Current;
            }

            bool allStrings = true;
            bool allNumbers = true;

            foreach (ColumnType type in _columnTypes)
            {
                if (allStrings && !type.isLiteral())
                {
                    allStrings = false;
                }
                else if (allNumbers && !type.isNumber())
                {
                    allNumbers = false;
                }
            }

            if (allStrings)
            {
                return ColumnTypeConstants.STRING;
            }

            if (allNumbers)
            {
                return ColumnTypeConstants.NUMBER;
            }

            return ColumnTypeConstants.OTHER;
        }
    } // InferentialColumnBuilder class
}
