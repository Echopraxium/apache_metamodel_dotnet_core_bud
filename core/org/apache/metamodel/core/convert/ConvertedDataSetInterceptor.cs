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
// https://github.com/apache/metamodel/blob/57e48be23844537690bdd6191ceb78adb4266e92/core/src/main/java/org/apache/metamodel/convert/ConvertedDataSetInterceptor.java
using org.apache.metamodel.core.data;
using org.apache.metamodel.query;
using org.apache.metamodel.schema;
using System.Collections.Generic;
using org.apache.metamodel.j2n.collections;
using org.apache.metamodel.core.intercept;

namespace org.apache.metamodel.core.convert
{
    /**
     * A {@link DataSetInterceptor} used for intercepting values in {@link DataSet}s
     * that need to be converted, according to a set of {@link TypeConverter}s.
     * 
     * @see TypeConverter
     * @see Converters
     */
    public class ConvertedDataSetInterceptor : DataSetInterceptor, HasReadTypeConverters
    {
	    private Dictionary<Column, TypeConverter<object, object>> _converters;

        public ConvertedDataSetInterceptor(): this(new Dictionary<Column, TypeConverter<object, object>>())
        {     
        }

        public ConvertedDataSetInterceptor(Dictionary<Column, TypeConverter<object, object>> converters)
        {
            _converters = converters;
        }

        //@Override
        public void addConverter(Column column, TypeConverter<object, object> converter)
        {
            if (converter == null)
            {
                _converters.Remove(column);
            }
            else
            {
                _converters.Add(column, converter);
            }
        }

        protected Dictionary<Column, TypeConverter<object, object>> getConverters(DataSet dataSet)
        {
            return _converters;
        } // getConverters()

        // @Override
        public DataSet intercept(DataSet dataSet)
        {
            Dictionary<Column, TypeConverter<object, object>> converters = getConverters(dataSet);
            if (converters.IsEmpty())
            {
                return dataSet;
            }

            bool             hasConverter = false;
            List<SelectItem> selectItems  = NArrays.AsList<SelectItem>(dataSet.getSelectItems());
            TypeConverter<object, object>[] converterArray = new TypeConverter<object, object>[selectItems.Count];
            for (int i = 0; i < selectItems.Count; i++)
            {
                SelectItem selectItem = selectItems[i];
                Column     column     = selectItem.getColumn();
                if (column != null && selectItem.getAggregateFunction() == null)
                {
                    TypeConverter<object, object> converter = converters[column];
                    if (converter != null)
                    {
                        hasConverter = true;
                        converterArray[i] = converter;
                    }
                }
            }

            if (! hasConverter)
            {
                return dataSet;
            }

            return new ConvertedDataSet(dataSet, converterArray);
        } // intercept()
    } // ConvertedDataSetInterceptor class
} // org.apach e.metamodel.core.convert