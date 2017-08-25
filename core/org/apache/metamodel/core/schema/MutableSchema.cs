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
// https://github.com/apache/metamodel/blob/57e48be23844537690bdd6191ceb78adb4266e92/core/src/main/java/org/apache/metamodel/schema/MutableSchema.java
using org.apache.metamodel.schema;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using org.apache.metamodel.j2n.collections;
using System.Diagnostics;

namespace org.apache.metamodel.core.schema
{
    /**
     * Represents a schema and it's metadata. Schemas represent a collection of
     * tables.
     * 
     * @see Table
     */
    public class MutableSchema : AbstractSchema, Schema // Serializable,
    {
	    private static readonly long serialVersionUID = 4465197783868238863L;

        private String             _name;
        private List<MutableTable> _tables;

        #region -------- Constructors --------
        public MutableSchema() : base()
        {
            Debug.WriteLine("new MutableSchema()");
            _tables = new List<MutableTable>();
        } // constructor

        public MutableSchema(String name) : this()
        {
            Debug.WriteLine("new MutableSchema(string)  name: '" + name + "'");
            _name = name;
        } // constructor

        public MutableSchema(String name, params MutableTable[] tables) : this(name)
        {
            Debug.WriteLine("new MutableSchema(string,  MutableTable[])");
            setTables(tables);
        } // constructor
        #endregion Constructors


        // @Override
        public override String getName()
        {
            return _name;
        } // getName()

        public MutableSchema setName(String name)
        {
            _name = name;
            return this;
        } // setName()

        public override Table[] getTables(bool dummy_arg_for_covariance_emulation)
        {
            List<Table> tables = getTables().AsList<Table>();
            return tables.ToArray();
        } // getTables()

        // Emulate return type covariance
        //[J2N] return type covariance issue (conflict with with Table[] getTables(TableType type))
        // https://stackoverflow.com/questions/1048884/c-overriding-return-types
        public override Table[] getTables(TableType type, bool dummy_arg_for_covariance_emulation)
        {
            List<Table> tables = getTables().AsList<Table>();
            return tables.ToArray();
        } // getTables()

        // @Override
        // List<Table>
        public IList<Table> getTables()
        {
            List<Table> table_items = new List<Table>();
            foreach (Table t in _tables)
            {
                table_items.Add(t);
            }
            ReadOnlyCollection<Table> result = new ReadOnlyCollection<Table>(table_items);
            return result; // Collections.unmodifiableList(_tables);
        } // getTables()


        public MutableSchema setTables(IList<MutableTable> tables)
        {
            Debug.WriteLine("MutableSchema.setTables()");
            clearTables();
            foreach (MutableTable table in tables)
            {
                _tables.Add(table);
            }
            return this;
        } // setTables()

        public MutableSchema setTables(params MutableTable[] tables)
        {
            clearTables();
            foreach (MutableTable table in tables)
            {
                _tables.Add(table);
            }
            return this;
        } // setTables()

        public MutableSchema clearTables()
        {
            _tables.Clear();
            return this;
        } // clearTables()

        public MutableSchema addTable(MutableTable table)
        {
            _tables.Add(table);
            return this;
        } // addTable()

        public MutableSchema removeTable(Table table)
        {
            _tables.Remove((MutableTable) table);
            return this;
        } // removeTable()

        // @Override
        public override String getQuote()
        {
            return null;
        } //getQuote()
    } // MutableSchema class
} // org.apache.metamodel.core.schema
