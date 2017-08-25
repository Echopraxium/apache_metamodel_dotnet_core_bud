
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/data/MaxRowsDocumentSource.java

using System.Diagnostics;
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
namespace org.apache.metamodel.core.data
{
    /**
     * A {@link DocumentSource} that has a max rows condition on it, that will make
     * it stop serving documents after a certain limit.
     */
    public class MaxRowsDocumentSource : DocumentSource
    {
        private DocumentSource _delegate;
        private volatile int   _rowsLeft;

        public MaxRowsDocumentSource(DocumentSource delegate_arg, int maxRows) 
        {
            Debug.WriteLine("new MaxRowsDocumentSource()");
            _delegate = delegate_arg;
            _rowsLeft = maxRows;
        } // constructor

        // @Override
        public Document next()
        {
            if (_rowsLeft > 0)
            {
                Document next = _delegate.next();
                _rowsLeft--;
                return next;
            }
            return null;
        } // next()

        // @Override
        public void close()
        {
            _delegate.close();
        } // close()
    } // MaxRowsDocumentSource class
} // org.apache.metamodel.core.data
