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
using org.apache.metamodel.core.schema;
using org.apache.metamodel.j2n.collections;
using org.apache.metamodel.schema;
using org.apache.metamodel.util;
using System;
using System.Collections.Generic;
using System.Text;

//import java.io.Serializable;
//import java.util.Arrays;

//import org.apache.metamodel.DataContext;
//import org.apache.metamodel.schema.Column;
//import org.apache.metamodel.schema.ColumnType;
//import org.apache.metamodel.schema.MutableColumn;
//import org.apache.metamodel.schema.MutableTable;
//import org.apache.metamodel.schema.Table;
//import org.apache.metamodel.schema.TableType;

namespace org.apache.metamodel.core.util
{
    /**
     * Represents a table definition to be used in scenarios where a
     * {@link DataContext} is unable to detect/discover the table structure and
     * needs some basic input around expected table structures.
     */
    public class SimpleTableDef : HasName // Serializable, 
    {
        private static readonly long serialVersionUID = 1L;

        private String _name;
        private String[] _columnNames;
        private ColumnType[] _columnTypes;

        #region constructor
        /**
         * Constructs a {@link SimpleTableDef} using a {@link Table} as a prototype.
         * 
         * @param table
         */
        public SimpleTableDef(Table table)
        {
            _name = table.getName();
            _columnNames = new String[table.getColumnCount()];
            _columnTypes = new ColumnType[table.getColumnCount()];
            for (int i = 0; i < table.getColumnCount(); i++)
            {
                Column column = table.getColumn(i);
                _columnNames[i] = column.getName();
                _columnTypes[i] = column.getType();
            }
        } // constructor

        /**
         * Constructs a {@link SimpleTableDef}.
         * @param name
         *            the name of the table
         * @param columnNames
         *            the names of the columns to include in the table
         */
        public SimpleTableDef(String name, String[] columnNames) : this(name, columnNames, null)
        {
        } // constructor

        /**
         * Constructs a {@link SimpleTableDef}.
         * 
         * @param name
         *            the name of table
         * @param columnNames
         *            the names of the columns to include in the table
         * @param columnTypes
         *            the column types of the columns specified.
         */
        public SimpleTableDef(String name, String[] columnNames, ColumnType[] columnTypes)
        {
            if (name == null)
            {
                throw new NullReferenceException("Table name cannot be null");
            }
            _name = name;
            _columnNames = columnNames;
            if (columnTypes == null)
            {
                columnTypes = new ColumnType[columnNames.Length];
                for (int i = 0; i < columnTypes.Length; i++)
                {
                    columnTypes[i] = ColumnTypeConstants.VARCHAR;
                }
            }
            else
            {
                if (columnNames.Length != columnTypes.Length)
                {
                    throw new ArgumentException(
                            "Property names and column types cannot have different lengths (found " + columnNames.Length
                                    + " and " + columnTypes.Length + ")");
                }
            }
            _columnTypes = columnTypes;
        } // constructor
        #endregion constructor


        /**
         * Gets the name of the table
         * 
         * @return the name of the table
         */
        public String getName()
        {
            return _name;
        }

        /**
         * Gets the names of the columns in the table
         * 
         * @return the names of the columns in the table
         */
        public String[] getColumnNames()
        {
            return _columnNames;
        } // getColumnNames()

        /**
         * Gets the types of the columns in the table
         * 
         * @return the types of the columns in the table
         */
        public ColumnType[] getColumnTypes()
        {
            return _columnTypes;
        } // getColumnTypes()

        // @Override
        public int hashCode()
        {
            int prime = 31;
            int result = 1;
            result = prime * result + ((_name == null) ? 0 : _name.GetHashCode());
            result = prime * result + _columnTypes.GetHashCode();
            result = prime * result + _columnNames.GetHashCode();
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
            SimpleTableDef other = (SimpleTableDef)obj;
            if (_name == null)
            {
                if (other._name != null)
                    return false;
            }
            else if (!_name.Equals(other._name))
                return false;
            if (!_columnTypes.Equals(other._columnTypes))
                return false;
            if (!_columnNames.Equals(other._columnNames))
                return false;
            return true;
        }

        // @Override
        public String toString()
        {
            return "SimpleTableDef[name=" + _name + ",columnNames=" + _columnNames.ToString() + ",columnTypes="
                    + _columnTypes.ToString() + "]";
        } // toString()()

        /**
         * Creates a {@link MutableTable} based on this {@link SimpleTableDef}. Note
         * that the created table will not have any schema set.
         * 
         * @return a table representation of this table definition.
         */
        public MutableTable toTable()
        {
            String       name        = getName();
            String[]     columnNames = getColumnNames();
            ColumnType[] columnTypes = getColumnTypes();

            MutableTable table = new MutableTable(name, TableType.TABLE);

            for (int i = 0; i < columnNames.Length; i++)
            {
                table.addColumn(new MutableColumn(columnNames[i], columnTypes[i], table, i, true));
            }
            return table;
        } // toTable()

        /**
         * Gets the index of a column name, or -1 if the column name does not exist
         * 
         * @param columnName
         * @return
         */
        public int indexOf(String columnName)
        {
            if (columnName == null)
            {
                throw new ArgumentException("Column name cannot be null");
            }
            for (int i = 0; i < _columnNames.Length; i++)
            {
                if (columnName.Equals(_columnNames[i]))
                {
                    return i;
                }
            }
            return -1;
        } // indexOf()
    } // SimpleTableDef class
} // org.apache.metamodel.core.util namespace