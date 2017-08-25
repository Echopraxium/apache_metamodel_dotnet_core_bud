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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/schema/builder/ColumnNameAsKeysRowConverter.java
using org.apache.metamodel.core.convert;
using org.apache.metamodel.core.data;
using org.apache.metamodel.data;
using org.apache.metamodel.util;
using System;
using System.Collections.Generic;
using amm_data = org.apache.metamodel.core.data;

namespace org.apache.metamodel.core.schema.builder
{
    /**
     * Converter that assumes that keys in the documents are represented as columns
     * in a table.
     */
    public class ColumnNameAsKeysRowConverter : DocumentConverter
    {
        // @Override
        public Row convert(amm_data.Document document, DataSetHeader header)
        {
            Object[] values = new Object[header.size()];
            for (int i = 0; i < values.Length; i++)
            {
                String columnName = header.getSelectItem(i).getColumn().getName();
                values[i] = get(document, columnName);
            }
            return new DefaultRow(header, values);
        }

        protected Object get(Document document, String columnName)
        {
            Dictionary<string, object> values = document.getValues();
            Dictionary<object, object> map    = new Dictionary<object, object>();
            foreach (string k in values.Keys)
            {
                map.Add(k, values[k]);
            }
            Object value = CollectionUtils.find(map, columnName);
            return value;
        }
    }
}
