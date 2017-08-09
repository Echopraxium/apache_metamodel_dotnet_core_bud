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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/data/SimpleDataSetHeader.java
using org.apache.metamodel.data;
using org.apache.metamodel.j2n.collections;
using org.apache.metamodel.query;
using org.apache.metamodel.schema;
using System;
using System.Collections.Generic;
//package org.apache.metamodel.data;

//import java.util.Arrays;
//import java.util.List;

//import org.apache.metamodel.MetaModelHelper;
//import org.apache.metamodel.query.SelectItem;
//import org.apache.metamodel.schema.Column;

namespace org.apache.metamodel.core.data
{
    /**
     * Simple implementation of {@link DataSetHeader} which does no magic to improve
     * performance.
     * 
     * Note that except for datasets with very few records, the
     * {@link CachingDataSetHeader} is preferred.
     */
    public class SimpleDataSetHeader : DataSetHeader
    {
        private static readonly long serialVersionUID = 1L;

        private List<SelectItem> _items;

        #region Constructors
        public SimpleDataSetHeader(List<SelectItem> items)
        {
            _items = items;
        } // constructor

        public SimpleDataSetHeader(SelectItem[] selectItems) : this(NArrays.AsList<SelectItem>(selectItems))
        {
        } // constructor

        public SimpleDataSetHeader(Column[] columns) : this(MetaModelHelper.createSelectItems(columns))
        {   
        } // constructor
        #endregion Constructors


        // @Override
        public SelectItem[] getSelectItems()
        {
            return _items.ToArray(); // new SelectItem[_items.Count]);
        } // getSelectItems()

        // @Override
        public int size()
        {
            return _items.Count;
        } // size()

        // @Override
        public SelectItem getSelectItem(int index)
        {
            return _items[index];
        }

        // @Override
        public virtual int indexOf(Column column)
        {
            if (column == null)
            {
                return -1;
            }
            return indexOf(new SelectItem(column));
        }

        // @Override
        public virtual int indexOf(SelectItem item)
        {
            if (item == null)
            {
                return -1;
            }
            int i = 0;
            foreach (SelectItem selectItem in _items)
            {
                if (item == selectItem)
                {
                    return i;
                }
                i++;
            }

            i = 0;
            foreach (SelectItem selectItem in _items)
            {
                if (item.equalsIgnoreAlias(selectItem, true))
                {
                    return i;
                }
                i++;
            }

            i = 0;
            foreach (SelectItem selectItem in _items)
            {
                if (item.equalsIgnoreAlias(selectItem))
                {
                    return i;
                }
                i++;
            }

            bool scalarFunctionQueried = item.getScalarFunction() != null;
            if (scalarFunctionQueried)
            {
                SelectItem itemWithoutFunction = item.replaceFunction(null);
                return indexOf(itemWithoutFunction);
            }

            return -1;
        } // indexOf()

        // @Override
        public int hashCode()
        {
            int prime = 31;
            int result = 1;
            result = prime * result + ((_items == null) ? 0 : _items.GetHashCode());
            return result;
        } // hashCode()

        // @Override
        public bool equals(Object obj)
        {
            if (this == obj)
                return true;
            if (obj == null)
                return false;
            if (GetType() != obj.GetType())
                return false;

            SimpleDataSetHeader other = (SimpleDataSetHeader)obj;
            if (_items == null)
            {
                if (other._items != null)
                    return false;
            }
            else if (!_items.Equals(other._items))
                return false;
            return true;
        } // equals()

        // @Override
        public String toString()
        {
            return "DataSetHeader" + _items.ToString();
        }
    } // SimpleDataSetHeader class
} // org.apache.metamodel.core.data namespace
