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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/data/AbstractRow.java
using org.apache.metamodel.data;
using org.apache.metamodel.j2n;
using org.apache.metamodel.query;
using org.apache.metamodel.schema;
using System;

namespace org.apache.metamodel.core.data
{
    /**
     * An abstract row that decorates another row. Useful for virtual data that may
     * e.g. be converting physical data etc.
     */
    public abstract class AbstractRow : Row, NCloneable
    {
        private static readonly long serialVersionUID = 1L;

        public abstract DataSetHeader getHeader();
        //{
        //    throw new NotImplementedException("AbstractRow.getHeader()");
        //}

        // @Override
        public virtual int hashCode()
        {
            int prime = 31;
            int result = 1;
            result = prime * result + getValues().GetHashCode();
            return result;
        }

        // @Override
        public bool equals(Object obj)
        {
            if (this == obj)
                return true;
            if (obj == null)
                return false;
            if (GetType() != obj.GetType())
                return false;
            Row other = (Row)obj;
            if (! getValues().Equals(other.getValues()))
                return false;
            return true;
        } // equals()

        // @Override
        public String toString()
        {
            return "Row[values=" + getValues().ToString() + "]";
        }

        // @Override
        public virtual Object getValue(SelectItem item)
        {
            int index = indexOf(item);
            if (index == -1)
            {
                return null;
            }
            return getValue(index);
        } // getValue()

        // @Override
        public virtual Style getStyle(SelectItem item)
        {
            int index = indexOf(item);
            if (index == -1)
            {
                return StyleConstants.NO_STYLE;
            }
            return getStyle(index);
        } // getStyle()

        // @Override
        public Style getStyle(Column column)
        {
            int index = indexOf(column);
            if (index == -1)
            {
                return StyleConstants.NO_STYLE;
            }
            return getStyle(index);
        } // getStyle()

        // @Override
        public virtual Style[] getStyles()
        {
            Style[] styles = new Style[size()];
            for (int i = 0; i < styles.Length; i++)
            {
                styles[i] = getStyle(i);
            }
            return styles;
        } //  getStyles()

        // @Override
        public virtual Object[] getValues()
        {
            Object[] values = new Object[size()];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = getValue(i);
            }
            return values;
        } // getValues()

        // @Override
        public virtual Object getValue(Column column)
        {
            int index = indexOf(column);
            if (index == -1)
            {
                return null;
            }
            return getValue(index);
        } // getValue()

        // @Override
        public int indexOf(SelectItem item)
        {
            if (item == null)
            {
                return -1;
            }
            return getHeader().indexOf(item);
        } // indexOf()

        // @Override
        public int indexOf(Column column)
        {
            if (column == null)
            {
                return -1;
            }
            return getHeader().indexOf(column);
        } // indexOf()

        // @Override
        public Row getSubSelection(SelectItem[] selectItems)
        {
            DataSetHeader header = new SimpleDataSetHeader(selectItems);
            return getSubSelection(header);
        }

        // @Override
        public SelectItem[] getSelectItems()
        {
            return getHeader().getSelectItems();
        }

        // @Override
        public virtual int size()
        {
            return getHeader().size();
        } // size()

        // @Override
        protected Row clone()
        {
            return new DefaultRow(getHeader(), getValues(), getStyles());
        }

        // @Override
        public Row getSubSelection(DataSetHeader header)
        {
            int      size   = header.size();
            Object[] values = new Object[size];
            Style[]  styles = new Style[size];
            for (int i = 0; i < size; i++)
            {
                SelectItem selectItem = header.getSelectItem(i);

                if (selectItem.getSubQuerySelectItem() != null)
                {
                    values[i] = getValue(selectItem.getSubQuerySelectItem());
                    styles[i] = getStyle(selectItem.getSubQuerySelectItem());
                    if (values[i] == null)
                    {
                        values[i] = getValue(selectItem);
                        styles[i] = getStyle(selectItem);
                    }
                }
                else
                {
                    values[i] = getValue(selectItem);
                    styles[i] = getStyle(selectItem);
                }
            }
            return new DefaultRow(header, values, styles);
        }

        public abstract object getValue(int index);

        public abstract Style getStyle(int index);
    } // AbstractRow class
} // org.apache.metamodel.core.data namespace
