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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/data/CachingDataSetHeader.java
using org.apache.metamodel.data;
using org.apache.metamodel.j2n;
using org.apache.metamodel.j2n.collections;
using org.apache.metamodel.j2n.data.numbers;
using org.apache.metamodel.query;
using org.apache.metamodel.schema;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

//import java.util.Arrays;
//import java.util.List;
//import java.util.Map;
//import java.util.concurrent.ConcurrentHashMap;

//import org.apache.metamodel.query.SelectItem;
//import org.apache.metamodel.schema.Column;

namespace org.apache.metamodel.core.data
{
    /**
     * Most common implementation of {@link DataSetHeader}. This implementation is
     * 'caching' in the sense that index values of selectitems are cached in a map
     * to provide quick access when looking up by {@link SelectItem} or
     * {@link Column}.
     */
    public sealed class CachingDataSetHeader : SimpleDataSetHeader, DataSetHeader
    {

        private static readonly long serialVersionUID = 1L;

        // map of select item identity codes and indexes in the dataset
        private /*transient*/ ConcurrentDictionary<NInteger, NInteger>  _selectItemIndexCache;

        // map of column identity codes and indexes in the dataset
        private /*transient*/ ConcurrentDictionary<NInteger, NInteger>  _columnIndexCache;

        #region Constructors
        public CachingDataSetHeader(List<SelectItem> items) : base(items)
        {    
        } // constructor

        public CachingDataSetHeader(SelectItem[] items) : this(NArrays.AsList(items))
        {
        } // constructor
        #endregion Constructors


        // @Override
        public override int indexOf(Column column)
        {
            if (column == null)
            {
                return -1;
            }

            if (_columnIndexCache == null)
            {
                _columnIndexCache = new ConcurrentDictionary<NInteger, NInteger>(10, base.size());
            }

            int identityCode = column.GetHashCode();
            NInteger index   = _columnIndexCache[identityCode];
            if (index == null)
            {
                index = base.indexOf(column);

                if (index != -1)
                {
                    _columnIndexCache[identityCode] = index;
                }
            }
            return index;
        }

        // @Override
        public override int indexOf(SelectItem item)
        {
            if (item == null)
            {
                return -1;
            }

            if (_selectItemIndexCache == null)
            {
                int concurrency_level = 10;
                int capacity          = base.size();
                _selectItemIndexCache = new ConcurrentDictionary<NInteger, NInteger>(concurrency_level, capacity);
            }

            int      identityCode = item.GetHashCode();
            NInteger index        = _selectItemIndexCache[identityCode];
            if (index == null)
            {
                index = base.indexOf(item);

                if (index != -1)
                {
                    _selectItemIndexCache[identityCode] = index;
                }
            }
            return index;
        }
    } // CachingDataSetHeader class
} // org.apache.metamodel.core.data namespace
