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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/data/FilteredDataSet.java
using org.apache.metamodel.data;

namespace org.apache.metamodel.core.data
{
    /**
     * Wraps another DataSet and transparently applies a set of filters to it.
     */
    public sealed class FilteredDataSet : AbstractDataSet, WrappingDataSet
    {
        private DataSet       _dataSet;
	    private IRowFilter[]  _filters;
	    private Row           _row;

        public FilteredDataSet(DataSet dataSet, params IRowFilter[] filters) : base(dataSet)
        {      
            _dataSet = dataSet;
            _filters = filters;
        } // FilteredDataSet

        // @Override
        public override void close()
        {
            base.close();
            _dataSet.close();
        } // close()

        // @Override
        public DataSet getWrappedDataSet()
        {
            return _dataSet;
        } //  getWrappedDataSet()

        // @Override
        public override bool next()
        {
            bool next = false;
            while (_dataSet.next())
            {
                Row row = _dataSet.getRow();
                foreach (IRowFilter filter in _filters)
                {
                    next = filter.accept(row);
                    if (!next)
                    {
                        break;
                    }
                }
                if (next)
                {
                    _row = row;
                    break;
                }
            }
            return next;
        } // next()

        // @Override
        public override Row getRow()
        {
            return _row;
        } // getRow()
    }
}
