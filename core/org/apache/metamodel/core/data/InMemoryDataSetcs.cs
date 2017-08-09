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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/data/InMemoryDataSet.java
using org.apache.metamodel.data;
using org.apache.metamodel.j2n.collections;
using org.apache.metamodel.query;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

//import java.util.Arrays;
//import java.util.List;
//import org.apache.metamodel.query.SelectItem;

namespace org.apache.metamodel.core.data
{
    /**
     * DataSet implementation based on in-memory data.
     */
    public sealed class InMemoryDataSet : AbstractDataSet
    {
        private List<Row> _rows;
        private int       _rowNumber = -1;

        public InMemoryDataSet(params Row[] rows) : this(NArrays.AsList(rows))
        {  
        } // constructor

        public InMemoryDataSet(List<Row> rows) : this(getHeader(rows), rows)
        {
        } // constructor

        public InMemoryDataSet(DataSetHeader header, params Row[] rows) :  base(header)
        {  
            _rows = NArrays.AsList(rows);
        } // constructor

        public InMemoryDataSet(DataSetHeader header, List<Row> rows) :  base(header)
        {  
            _rows = rows;
        } // constructor

        private static DataSetHeader getHeader(List<Row> rows)
        {
            if (rows.IsEmpty())
            {
                throw new ArgumentException("Cannot hold an empty list of rows, use " + typeof(EmptyDataSet)
                                            + " for this");
            }

           SelectItem[] selectItems = rows[0].getSelectItems();

            if (rows.Count > 3)
            {
                // not that many records - caching will not have body to scale
                return new SimpleDataSetHeader(selectItems);
            }
            return new CachingDataSetHeader(selectItems);
        }

        // @Override
        public override bool next()
        {
            _rowNumber++;
            if (_rowNumber < _rows.Count)
            {
                return true;
            }
            return false;
        } // next()

        // @Override
        public override Row getRow()
        {
            if (_rowNumber < 0 || _rowNumber >= _rows.Count)
            {
                return null;
            }
            Row row = _rows[_rowNumber];
            Debug.Assert(row.size() == getHeader().size());
            return row;
        } // getRow()()

        public List<Row> getRows()
        {
            return _rows;
        } // getRows()

        public int size()
        {
            return _rows.Count;
        }
    } // InMemoryDataSet class
} // org.apache.metamodel.core.data namespace
