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
//https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/schema/builder/InferentialTableBuilder.java
using org.apache.metamodel.core.data;
using org.apache.metamodel.j2n.data.numbers;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.apache.metamodel.core.schema.builder
{
    /**
     * Implementation of {@link TableBuilder} that
     */
    public class InferentialTableBuilder : TableBuilder
    {
        private static int MAX_SAMPLE_SIZE = 1000;

        private  String                                       _tableName;
        private  Dictionary<String, InferentialColumnBuilder> _columnBuilders;
        private  NAtomicInteger                               _observationCounter;

        public InferentialTableBuilder(String tableName)
        {
            _tableName          = tableName;
            _columnBuilders     = new Dictionary<String, InferentialColumnBuilder>();
            _observationCounter = new NAtomicInteger(0);
        } // constructor

        public void addObservation(Document document)
        {
            _observationCounter.incrementAndGet();
            Dictionary<String, object>            values  = document.getValues();
            HashSet<KeyValuePair<object, object>> entries = new HashSet<KeyValuePair<object, object>>();
            foreach (String k in values.Keys)
            {
                entries.Add(new KeyValuePair<object, object>(k, values[k]));
            }
            foreach (KeyValuePair<object, object> entry in entries)
            {
                Object key = entry.Key;
                if (key != null)
                {
                    String column = key.ToString();
                    Object value  = entry.Value;
                    InferentialColumnBuilder columnBuilder = (InferentialColumnBuilder) getColumnBuilder(column);
                    columnBuilder.addObservation(value);
                }
            }
        } // addObservation()

        /**
         * Gets the number of observations that this table builder is basing it's
         * inference on.
         * 
         * @return
         */
        public int getObservationCount()
        {
            return _observationCounter.asInt();
        }

        // @Override
        public MutableTable buildTable()
        {
            int tableObservations = getObservationCount();

            // sort column names by copying them to a TreeSet
            HashSet<String> columnNames = new HashSet<String>(_columnBuilders.Keys);  // new TreeSet<String>(_columnBuilders.keySet());

            MutableTable table = new MutableTable(_tableName);
            int columnNumber = 0;
            foreach (String columnName in columnNames)
            {
                InferentialColumnBuilder columnBuilder = (InferentialColumnBuilder) getColumnBuilder(columnName);
                MutableColumn column = columnBuilder.build();
                column.setTable(table);
                column.setColumnNumber(columnNumber);

                int columnObservations = columnBuilder.getObservationCount();
                if (tableObservations > columnObservations)
                {
                    // there may be nulls - some records does not even contain the
                    // column
                    column.setNullable(true);
                }

                table.addColumn(column);

                columnNumber++;
            }

            return table;
        }

        // @Override
        public ColumnBuilder getColumnBuilder(String column_name)
        {
            InferentialColumnBuilder column_builder = null;
            if (_columnBuilders.ContainsKey(column_name))
                column_builder = _columnBuilders[column_name];

            if (column_builder == null)
            {
                column_builder = new InferentialColumnBuilder(column_name);
                _columnBuilders.Add(column_name, column_builder);
            }
            return column_builder;
        } // getColumnBuilder()

        // @Override
        public void offerSource(DocumentSource documentSource)
        {
            while (getObservationCount() < MAX_SAMPLE_SIZE)
            {
                Document map = documentSource.next();
                if (map == null)
                {
                    return;
                }
                addObservation(map);
            }
        }
    } // InferentialTableBuilder class
}
