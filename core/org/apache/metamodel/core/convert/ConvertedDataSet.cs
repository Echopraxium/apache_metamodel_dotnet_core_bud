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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/convert/ConvertedDataSet.java
using org.apache.metamodel.core.data;
using org.apache.metamodel.data;
using org.apache.metamodel.j2n.slf4j;
using System;

namespace org.apache.metamodel.core.convert
{
    /**
     * A {@link DataSet} wrapper/decorator which converts values using
     * {@link TypeConverter}s before returning them to the user.
     */
    public sealed class ConvertedDataSet : AbstractDataSet
    {

        private static readonly NLogger logger = NLoggerFactory.getLogger(typeof(ConvertedDataSet).Name);

        private DataSet                         _dataSet;
        private TypeConverter<object, object>[] _converters;

        public ConvertedDataSet(DataSet dataSet, TypeConverter<object, object>[] converters) : base(dataSet.getSelectItems())
        {       
            _dataSet    = dataSet;
            _converters = converters;
        } // constructor

        // @Override
        public override bool next()
        {
            return _dataSet.next();
        } // next()

        // @Override
        public override Row getRow()
        {
            Row sourceRow = _dataSet.getRow();
            Object[] values = new Object[_converters.Length];
            for (int i = 0; i < values.Length; i++)
            {
                Object value = sourceRow.getValue(i);

                // @SuppressWarnings("unchecked")
                TypeConverter<object, object> converter = (TypeConverter< object, object>) _converters[i];

                if (converter != null)
                {
                    Object virtualValue = converter.toVirtualValue(value);
                    logger.debug("Converted physical value {} to {}", value, virtualValue);
                    value = virtualValue;
                }
                values[i] = value;
            }
            return new DefaultRow(getHeader(), values);
        } // getRow()

        // @Override
        public override void close()
        {
            _dataSet.close();
        } // close()
    } // ConvertedDataSet
} // org.apache.metamodel.core.convert
