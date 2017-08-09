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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/data/DefaultRow.java
using org.apache.metamodel.data;
using org.apache.metamodel.j2n.collections;
using org.apache.metamodel.j2n.data;
using org.apache.metamodel.j2n.io;
using org.apache.metamodel.j2n.reflection;
using org.apache.metamodel.query;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace org.apache.metamodel.core.data
{
    /**
     * Default Row implementation. Holds values in memory.
     */
    public sealed class DefaultRow : AbstractRow, Row
    {
        private static readonly long serialVersionUID = 1L;

        private DataSetHeader _header;
        private Object[]      _values;
        private Style[]       _styles;

        /**
         * Constructs a row.
         * 
         * @param header
         * @param values
         * @param styles
         */
        public DefaultRow(DataSetHeader header, Object[] values, Style[] styles)
        {
            if (header == null)
            {
                throw new ArgumentException("DataSet header cannot be null");
            }
            if (values == null)
            {
                throw new ArgumentException("Values cannot be null");
            }
            if (header.size() != values.Length)
            {
                throw new ArgumentException("Header size and values length must be equal. " + header.size()
                                             + " select items present in header and encountered these values: " 
                                             + values.ToString());
            }
            if (styles != null)
            {
                if (values.Length != styles.Length)
                {
                    throw new ArgumentException("Values length and styles length must be equal. " + values.Length
                                                + " values present and encountered these styles: " + styles.ToString());
                }
                bool entirelyNoStyle = true;
                for (int i = 0; i < styles.Length; i++)
                {
                    if (styles[i] == null)
                    {
                        throw new ArgumentException("Elements in the style array cannot be null");
                    }
                    if (entirelyNoStyle && ! StyleConstants.NO_STYLE.Equals(styles[i]))
                    {
                        entirelyNoStyle = false;
                    }
                }

                if (entirelyNoStyle)
                {
                    // no need to reference any styles
                    styles = null;
                }
            }
            _header = header;
            _values = values;
            _styles = styles;
        }

        /**
         * Constructs a row.
         * 
         * @param header
         * @param values
         */
        public DefaultRow(DataSetHeader header, Object[] values) : this(header, values, null)
        {
        } // constructor

        /**
         * Constructs a row from an array of SelectItems and an array of
         * corresponding values
         * 
         * @param items
         *            the array of SelectItems
         * @param values
         *            the array of values
         * 
         * @deprecated use {@link #DefaultRow(DataSetHeader, Object[])} or
         *             {@link #DefaultRow(DataSetHeader, Object[], Style[])}
         *             instead.
         */
        // @Deprecated 
        public DefaultRow(SelectItem[] items, Object[] values) : this(NArrays.AsList(items), values, null)
        {  
        } // constructor

        /**
         * Constructs a row from an array of SelectItems and an array of
         * corresponding values
         * 
         * @param items
         *            the array of SelectItems
         * @param values
         *            the array of values
         * @param styles
         *            an optional array of styles
         * @deprecated use {@link #DefaultRow(DataSetHeader, Object[])} or
         *             {@link #DefaultRow(DataSetHeader, Object[], Style[])}
         *             instead.
         */
        // @Deprecated
        public DefaultRow(SelectItem[] items, Object[] values, Style[] styles) : this(NArrays.AsList(items), values, styles)
        {
        } // constructor

        /**
         * Constructs a row from a list of SelectItems and an array of corresponding
         * values
         * 
         * @param items
         *            the list of SelectItems
         * @param values
         *            the array of values
         * @deprecated use {@link #DefaultRow(DataSetHeader, Object[])} or
         *             {@link #DefaultRow(DataSetHeader, Object[], Style[])}
         *             instead.
         */
        // @Deprecated
        public DefaultRow(List<SelectItem> items, Object[] values) : this(items, values, null)
        {   
        } // constructor

        /**
         * Constructs a row from a list of SelectItems and an array of corresponding
         * values
         * 
         * @param items
         *            the list of SelectItems
         * @param values
         *            the array of values
         * @param styles
         *            an optional array of styles
         * @deprecated use {@link #DefaultRow(DataSetHeader, Object[])} or
         *             {@link #DefaultRow(DataSetHeader, Object[], Style[])}
         *             instead.
         */
        // @Deprecated
        public DefaultRow(List<SelectItem> items, Object[] values, Style[] styles) : this(new SimpleDataSetHeader(items), values, styles)
        {   
        } // constructor

        // @Override
        public override Object getValue(int index) // throws ArrayIndexOutOfBoundsException
        {
            return _values [index];
        }

        // @Override
        public override Object[] getValues()
        {
            return _values;
        }

        // @Override
        public override Style getStyle(int index) // throws IndexOutOfBoundsException
        {
            if (_styles == null) {
                return StyleConstants.NO_STYLE;
            }
            return _styles [index];
        }

        // @Override
        public override Style[] getStyles()
        {
            return _styles;
        }

        // @Override
        public override DataSetHeader getHeader()
        {
            return _header;
        }

        /**
         * Method invoked by the Java serialization framework while deserializing
         * Row instances. Since previous versions of MetaModel did not use a
         * DataSetHeader, but had a reference to a List&lt;SelectItem&gt;, this
         * deserialization is particularly tricky. We check if the items variable is
         * there, and if it is, we convert it to a header instead.
         * 
         * @param stream
         * @throws Exception
         */
        private void readObject(Object o, NInputStream stream) // throws Exception
        {
            XmlSerializer serializer = new XmlSerializer(o.GetType());
            object      read_data   = serializer.Deserialize(stream); //.readFields();
            FieldInfo[] fields      = read_data.GetType().GetFields();
            FieldInfo   items_field = NTypeUtils.getField(read_data.GetType(), "_items");

            NGetField get_field = new NGetField(read_data, fields);

            try
            {
                // backwards compatible deserialization, convert items to header
                Object items = items_field.GetValue(read_data);
                //@SuppressWarnings("unchecked")
                List<SelectItem>    itemsList = (List<SelectItem>) items;
                SimpleDataSetHeader header    = new SimpleDataSetHeader(itemsList);
                FieldInfo           field     = NTypeUtils.getField(GetType(), "_header");
                // field.setAccessible(true);
                field.SetValue(this, header);
            }
            catch (ArgumentException e)
            {
                // no backwards compatible deserialization needed.
                setWhileDeserializing(get_field, "_header");
            }
            setWhileDeserializing(get_field, "_values");
            setWhileDeserializing(get_field, "_styles");
        } // readObject()

        private void setWhileDeserializing(NGetField fields, String fieldName) // throws Exception
        {
            object    value  = fields.get(fieldName, null);
            FieldInfo field  = NTypeUtils.getField(GetType(), fieldName);
            //field.setAccessible(true);
            field.SetValue(this, value);
        } // setWhileDeserializing()
    } // DefaultRow
} // org.apache.metamodel.core.data namespace
