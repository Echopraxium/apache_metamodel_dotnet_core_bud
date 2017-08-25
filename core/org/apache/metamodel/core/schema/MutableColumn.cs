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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/schema/MutableColumn.java
using org.apache.metamodel.j2n.data.numbers;
using org.apache.metamodel.schema;
using System;

namespace org.apache.metamodel.core.schema
{
    /**
     * Represents a column and it's metadata description. Columns reside within a
     * Table and can be used as keys for relationships between tables.
     * 
     * @see MutableTable
     * @see Relationship
     */
    public class MutableColumn : AbstractColumn // Serializable
    {
        private static readonly long serialVersionUID = -353183696233890927L;

        private int        _columnNumber;
        private String     _name;
        private ColumnType _type;
        private Table      _table;
        private bool?      _nullable = null;
        private String     _remarks;
        private bool       _indexed = false;
        private bool       _primaryKey = false;
        private NInteger   _columnSize = null;
        private String     _nativeType = null;
        private String     _quoteString = null;

        public MutableColumn() : base()
        {         
        } // constructor

        public MutableColumn(String name) : this()
        {
            setName(name);
        } // constructor

        public MutableColumn(String name, ColumnType type) : this(name)
        {
            setType(type);
        } // constructor

        public MutableColumn(String name, ColumnType type, Table table, int columnNumber, Boolean nullable) : this(name, type)
        {
            setColumnNumber(columnNumber);
            setTable(table);
            setNullable(nullable);
        } // constructor

        public MutableColumn(String name, ColumnType type, Table table, int columnNumber, NInteger columnSize,
                             String nativeType, Boolean nullable, String remarks, bool indexed, String quote) :
                                    this(name, type, table, columnNumber, nullable)
        {
            setColumnSize(columnSize);
            setNativeType(nativeType);
            setRemarks(remarks);
            setIndexed(indexed);
            setQuote(quote);
        } // constructor

        public MutableColumn(String name, Table table) : this(name)
        {
            setTable(table);
        } // constructor


        // @Override
        public override int getColumnNumber()
        {
            return _columnNumber;
        }

        public MutableColumn setColumnNumber(int columnNumber)
        {
            _columnNumber = columnNumber;
            return this;
        }

        // @Override
        public override String getName()
        {
            return _name;
        }

        public MutableColumn setName(String name)
        {
            _name = name;
            return this;
        }

        // @Override
        public override ColumnType getType()
        {
            return _type;
        }

        public MutableColumn setType(ColumnType type)
        {
            _type = type;
            return this;
        }

        // @Override
        public override Table getTable()
        {
            return _table;
        }

        public MutableColumn setTable(Table table)
        {
            _table = table;
            return this;
        }

        // @Override
        public override bool? isNullable()
        {
            return _nullable;
        }

        public MutableColumn setNullable(Boolean nullable)
        {
            _nullable = nullable;
            return this;
        }

        // @Override
        public override String getRemarks()
        {
            return _remarks;
        }

        public MutableColumn setRemarks(String remarks)
        {
            _remarks = remarks;
            return this;
        }

        // @Override
        public override NInteger getColumnSize()
        {
            return _columnSize;
        }

        public MutableColumn setColumnSize(NInteger columnSize)
        {
            _columnSize = columnSize;
            return this;
        }

        // @Override
        public override String getNativeType()
        {
            return _nativeType;
        }

        public MutableColumn setNativeType(String nativeType)
        {
            _nativeType = nativeType;
            return this;
        }

        // @Override
        public override bool isIndexed()
        {
            return _indexed;
        }

        public MutableColumn setIndexed(bool indexed)
        {
            _indexed = indexed;
            return this;
        }

        // @Override
        public override String getQuote()
        {
            return _quoteString;
        }

        public MutableColumn setQuote(String quoteString)
        {
            _quoteString = quoteString;
            return this;
        }

        // @Override
        public override bool isPrimaryKey()
        {
            return _primaryKey;
        }

        public MutableColumn setPrimaryKey(bool primaryKey)
        {
            _primaryKey = primaryKey;
            return this;
        }
    } // MutableColumn class
} // org.apache.metamodel.core.schema
