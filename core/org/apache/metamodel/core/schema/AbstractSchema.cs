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
// https://github.com/apache/metamodel/blob/master/core/src/main/java/org/apache/metamodel/schema/AbstractSchema.java
using org.apache.metamodel.core.util;
using org.apache.metamodel.j2n.collections;
using org.apache.metamodel.j2n.slf4j;
using org.apache.metamodel.schema;
using org.apache.metamodel.j2n.collections;
using System;
using System.Collections.Generic;
using System.Linq;
using org.apache.metamodel.j2n.exceptions;
using System.Diagnostics;

namespace org.apache.metamodel.core.schema
{
    /**
     * Abstract implementation of the {@link Schema} interface. Implements most
     * common and trivial methods.
     */
    public abstract class AbstractSchema : Schema
    {
        private static readonly long serialVersionUID = 1L;

        private static readonly NLogger logger = NLoggerFactory.getLogger(typeof(AbstractSchema).Name);

        public AbstractSchema()
        {
            Debug.WriteLine("new AbstractSchema");
        } // constructor

        // @Override
        public String getQuotedName()
        {
            String quote = getQuote();
            if (quote == null)
            {
                return getName();
            }
            return quote + getName() + quote;
        } // getQuotedName()

        // @Override
        public int getRelationshipCount()
        {
            return getRelationships().Count;
        } // getRelationshipCount()

        // @Override
        public IList<Relationship> getRelationships()
        {
            Table[] tables = getTables(false);
            List<Relationship> result = new List<Relationship>();
            foreach (Table t in tables)
            {
                Relationship[] relations = t.getRelationships();
                result.AddRange(NArrays.AsList<Relationship>(relations));
            }
            //return getTables().stream()
            //        .flatMap(tab->tab.getRelationships().stream())
            //        .collect(new HashSet<Relationship>());
            return result;
        } // getRelationships()


        // @Override
        public Table getTable(int index) //throws IndexOutOfBoundsException
        {
            Table[] tables = getTables(false);
            if (tables.Length == 0 || index > tables.Length)
            {
                throw new NIndexOutOfBoundsException("AbstractSchema.getTable(int): Index Out of Range index: " + index);
            }
            return tables[index];
        } // getTable()

        public abstract Table[] getTables(bool dummy_arg_for_covariance_emulation);

        // Emulate return type covariance
        //[J2N] return type covariance issue (conflict with with Table[] getTables(TableType type))
        // https://stackoverflow.com/questions/1048884/c-overriding-return-types
        public virtual Table[] getTables(TableType type, bool dummy_arg_for_covariance_emulation)
        {
            throw new NotImplementedException();
        } // Table[] getTables()

        public virtual IList<Table> getTables(TableType type)
        {
            Table[] tables = getTables(false);
            List<Table> result = new List<Table>();
            foreach (Table t in tables)
            {
                if (t.GetType().Equals(typeof(TableType)))
                {
                    result.Add(t);
                }
            }
            return result;
            //return getTables().stream()
            //        .filter(table->table.getType().equals(type))
            //        .collect(Collectors.toList());
        } // IList<Table> getTables()

        // @Override
        public String getQualifiedLabel()
        {
            return getName();
        } // getQualifiedLabel()

        // @Override
        public int getTableCount(TableType type)
        {
            return getTables(type).Count;
        } // getTableCount()

        // @Override
        public int getTableCount()
        {
            return getTables(false).Length;
        } // getTableCount()

        // @Override
        public virtual Table getTableByName(String tableName)
        {
            if (tableName == null)
            {
                return null;
            }

            List<string> found_table_names = new List<string>(1);
            List<Table>  foundTables       = new List<Table>(1);
            // Search for table matches, case insensitive.
            foreach (Table table in getTables(false))
            {
                if (tableName.Equals(table.getName(), StringComparison.CurrentCultureIgnoreCase))
                {
                    foundTables.Add(table);
                    found_table_names.Add(table.getName());
                }
            }

            int numTables = foundTables.Count;
            if (logger.isDebugEnabled())
            {
                logger.debug("Found {0} tables(s) matching '{1}': {2}", numTables, tableName, found_table_names);
            }

            if (numTables == 0)
            {
                return null;
            }
            else if (numTables == 1)
            {
                return foundTables[0];
            }

            // If more matches are found, search case sensitive
            foreach (Table table in foundTables)
            {
                if (tableName.Equals(table.getName()))
                {
                    return table;
                }
            }

            // if none matches case sensitive, pick the first one.
            return foundTables[0];
        } // getTableByName()

        public string[] getTableNames(bool dummy_arg_for_covariance_emulation)
        {
            List<string> tables = getTableNames().AsList<string>();
            return tables.ToArray();
        } // getTableNames()

        // @Override
        public IList<String> getTableNames()
        {
            Table[] tables = getTables(false);
            List<string> result = new List<string>();
            foreach (Table t in tables)
            {
               result.Add(t.getName());
            }
            return result;
            //return getTables().stream()
            //        .map(table->table.getName())
            //        .collect(Collectors.toList());
        } // getTableNames()

        // @Override
        public String toString()
        {
            return "Schema[name=" + getName() + "]";
        } // toString();

        // @Override
        public bool equals(Object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj == this)
            {
                return true;
            }
            if (obj is Schema)
            {
                Schema other = (Schema)obj;
                EqualsBuilder eb = new EqualsBuilder();
                eb.append(getName(), other.getName());
                eb.append(getQuote(), other.getQuote());
                if (eb.isEquals())
                {
                    try
                    {
                        int tableCount1 = getTableCount();
                        int tableCount2 = other.getTableCount();
                        eb.append(tableCount1, tableCount2);
                    }
                    catch (Exception e)
                    {
                        // might occur when schemas are disconnected. Omit this
                        // check then.
                    }
                }
                return eb.isEquals();
            }
            return false;
        } // equals()

        // @Override
        public int hashCode()
        {
            String name = getName();
            if (name == null)
            {
                return -1;
            }
            return name.GetHashCode();
        } // hashCode()

        // @Override
        public int compareTo(Schema that)
        {
            int diff = getQualifiedLabel().CompareTo(that.getQualifiedLabel());
            if (diff == 0)
            {
                diff = toString().CompareTo(that.ToString());
            }
            return diff;
        } // compareTo()

        public abstract string getQuote();

        public abstract string getName();
    } // AbstractSchema class
} // org.apache.metamodel.core.schema
