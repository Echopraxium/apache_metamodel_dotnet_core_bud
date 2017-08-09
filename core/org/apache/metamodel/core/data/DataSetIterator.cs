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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/data/DataSetIterator.java
using org.apache.metamodel.data;
using org.apache.metamodel.j2n.exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
//import java.util.Iterator;

namespace org.apache.metamodel.core.data
{
    /**
     * Iterator implementation that iterates through a DataSet.
     */
    public sealed class DataSetIterator : IEnumerator<Row>
    {
        private DataSet        _dataSet;
        private volatile short _iterationState;
        private volatile Row   _row;


        public DataSetIterator(DataSet dataSet)
        {
            _dataSet = dataSet;
            // 0 = uninitialized, 1=row not read yet, 2=row read, 3=finished
            _iterationState = 0;
        } // constructor

        public Row Current
        {
            get { return _row; }
        }

        object IEnumerator.Current
        {
            get { return _row; }
        }

        // @Override
        public bool hasNext()
        {
            if (_iterationState == 0 || _iterationState == 2)
            {
                if (_dataSet.next())
                {
                    _iterationState = 1;
                    _row = _dataSet.getRow();
                }
                else
                {
                    _iterationState = 3;
                    _row = null;
                    _dataSet.close();
                }
            }
            return _iterationState == 1;
        } // hasNext()

        public bool MoveNext()
        {
            next();
            return true;
        } // MoveNext()

        // @Override
        public Row next()
        {
            if (_iterationState == 1)
            {
                _iterationState = 2;
            }
            return _row;
        } // next()

        // @Override
        public void remove()
        {
            throw new NUnsupportedOperationException("DataSet is read-only, remove() is not supported.");
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    } // DataSetIterator class
} // org.apache.metamodel.core.data
