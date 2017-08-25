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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/data/AbstractDataSet.java
using org.apache.metamodel.data;
using org.apache.metamodel.j2n.collections;
using org.apache.metamodel.query;
using org.apache.metamodel.schema;
using org.apache.metamodel.util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using org.apache.metamodel.j2n.data;
using System.Diagnostics;

//import java.util.ArrayList;
//import java.util.Arrays;
//import java.util.Iterator;
//import java.util.List;

//import javax.swing.table.TableModel;

//import org.apache.metamodel.MetaModelHelper;
//import org.apache.metamodel.query.SelectItem;
//import org.apache.metamodel.schema.Column;
//import org.apache.metamodel.util.BaseObject;

namespace org.apache.metamodel.core.data
{
    /**
     * Abstract DataSet implementation. Provides convenient implementations of
     * trivial method and reusable parts of non-trivial methods of a DataSet.
     */
    public abstract class AbstractDataSet : BaseObject , DataSet
    {

        private DataSetHeader _header;

        #region Constructors
        /**
         * @deprecated use one of the other constructors, to provide header
         *             information.
         */
        // @Deprecated
        public AbstractDataSet()
        {
            Debug.WriteLine("new AbstractDataSet");
            _header = null;
        } // constructor

        public AbstractDataSet(SelectItem[] selectItems) : this(NArrays.AsList<SelectItem>(selectItems))
        {
            Debug.WriteLine("new AbstractDataSet(SelectItem[])");
        } // constructor

        public AbstractDataSet(List<SelectItem> selectItems) : this(new CachingDataSetHeader(selectItems))
        {
            Debug.WriteLine("new AbstractDataSet(List<SelectItem>)");
        } // constructor

        /**
         * Constructor appropriate for dataset implementations that wrap other
         * datasets, such as the {@link MaxRowsDataSet}, {@link FilteredDataSet} and
         * more.
         * 
         * @param dataSet
         */
        public AbstractDataSet(DataSet dataSet)
        {
            if (dataSet is AbstractDataSet)
            {
                _header = ((AbstractDataSet)dataSet).getHeader();
            } else {
                _header = new CachingDataSetHeader(NArrays.AsList<SelectItem>(dataSet.getSelectItems()));
            }
        } // constructor

        public AbstractDataSet(DataSetHeader header)
        {
            _header = header;
        } // constructor

        public AbstractDataSet(Column[] columns) : this(MetaModelHelper.createSelectItems(columns))
        {
        } // constructor
        #endregion Constructors


        /**
         * {@inheritDoc}
         */
        // @Override
        public SelectItem[] getSelectItems()
        {
            return getHeader().getSelectItems();
        } // getSelectItems()

        public virtual DataSetHeader getHeader()
        {
            return _header;
        }

        /**
         * {@inheritDoc}
         */
        // @Override
        public int indexOf(SelectItem item)
        {
            return getHeader().indexOf(item);
        } // indexOf()

        /**
         * {@inheritDoc}
         */
        // @Override
        public virtual void close()
        {
            // do nothing
        } // close()

        /**
         * {@inheritDoc}
         */
        // @Override
        public NTableModel toTableModel()
        {
            NTableModel tableModel = (NTableModel) new DataSetTableModel(this);
            return tableModel;
        } // toTableModel()

        /**
         * {@inheritDoc}
         */
        // @Override
        public List<Object[]> toObjectArrays() 
        {
            try
            {
                List<Object[]> objects = new List<Object[]>();
                while (next())
                {
                    Row row = getRow();
                    objects.Add(row.getValues());
                }
                return objects;
            }
            finally
            {
                close();
            }
        } // toObjectArrays()

        /**
         * {@inheritDoc}
         */
        // @Override
        public String toString()
        {
            return "DataSet[selectItems=" + getSelectItems().ToString() + "]";
        }

        // @Override
        public override void decorateIdentity(NList<Object> identifiers)
        {
            identifiers.Add(GetType());
            identifiers.Add(getSelectItems());
        } // decorateIdentity()

        // @Override
        public List<Row> toRows()
        {
            try
            {
                List<Row> result = new List<Row>();
                while (next())
                {
                    result.Add(getRow());
                }
                return result;
            }
            finally
            {
                close();
            }
        } // toRows()

        /**
         * {@inheritDoc}
         */
        // @Override
        public IEnumerator<Row> iterator()
        {
            return new DataSetIterator(this);
        } // iterator()

        public virtual bool next()
        {
            throw new NotImplementedException();
        }

        public virtual Row getRow()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<Row> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    } // AbstractDataSet class
} // org.apache.metamodel.core.data namespace
