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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/data/FirstRowDataSet.java
using org.apache.metamodel.data;
using System;

namespace org.apache.metamodel.core.data
{
    /**
     * Wraps another DataSet and enforces a first row offset.
     */
    public sealed class FirstRowDataSet : AbstractDataSet, WrappingDataSet
    {
        private DataSet      _dataSet;
        private volatile int _rowsLeftToSkip;

        /**
         * Constructs a {@link FirstRowDataSet}.
         * 
         * @param dataSet
         *            the dataset to wrap
         * @param firstRow
         *            the first row number (1-based).
         */
        public FirstRowDataSet(DataSet dataSet, int firstRow): base(dataSet)
        {          
            _dataSet = dataSet;
            if (firstRow < 1)
            {
                throw new ArgumentException("First row cannot be negative or zero");
            }
            _rowsLeftToSkip = firstRow - 1;
        } // constructor

        // @Override
        public override void close()
        {
            _dataSet.close();
        }

        // @Override
        public override Row getRow()
        {
            return _dataSet.getRow();
        }

        // @Override
        public DataSet getWrappedDataSet()
        {
            return _dataSet;
        }

        // @Override
        public override bool next()
        {
            bool next = true;
            if (_rowsLeftToSkip > 0)
            {
                while (_rowsLeftToSkip > 0)
                {
                    next = _dataSet.next();
                    if (next)
                    {
                        _rowsLeftToSkip--;
                    }
                    else
                    {
                        // no more rows at all - exit loop
                        _rowsLeftToSkip = 0;
                        return false;
                    }
                }
            }
            return _dataSet.next();
        } // next()
    } // FirstRowDataSet class
} // org.apache.metamodel.core.data namespace
