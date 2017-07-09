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
// https://github.com/apache/metamodel/blob/master/core/src/main/java/org/apache/metamodel/schema/TableType.java
namespace org.apache.metamodel.schema
{
    /**
     * Represents the various types of tables
     */
    public struct TableType
    {
        // TABLE, VIEW, SYSTEM_TABLE, GLOBAL_TEMPORARY, LOCAL_TEMPORARY, ALIAS, SYNONYM, OTHER;
        public static readonly TableType TABLE            = new TableType("TABLE");
        public static readonly TableType VIEW             = new TableType("VIEW");
        public static readonly TableType SYSTEM_TABLE     = new TableType("SYSTEM_TABLE");
        public static readonly TableType GLOBAL_TEMPORARY = new TableType("GLOBAL_TEMPORARY");
        public static readonly TableType LOCAL_TEMPORARY  = new TableType("LOCAL_TEMPORARY");
        public static readonly TableType ALIAS            = new TableType("ALIAS");
        public static readonly TableType SYNONYM          = new TableType("SYNONYM");
        public static readonly TableType OTHER            = new TableType("OTHER");

        private string _value;

        public TableType(string v)
        {
            this._value = v;
        } // constructor

        /*
        public static final TableType[] DEFAULT_TABLE_TYPES = new TableType[] {
			TableType.TABLE, TableType.VIEW
        };*/
        public bool isMaterialized()
        {
            if (this.Equals(TableType.TABLE) || this.Equals(TableType.SYSTEM_TABLE))
                return true;

            return false;
        } // isMaterialized()

        /**
         * Tries to resolve a TableType based on an incoming string/literal. If no
         * fitting TableType is found, OTHER will be returned.
         */
        public static TableType getTableType(string literalType)
        {
            literalType = literalType.ToUpper();
            if ("TABLE".Equals(literalType))
            {
                return TABLE;
            }
            if ("VIEW".Equals(literalType))
            {
                return VIEW;
            }
            if ("SYSTEM_TABLE".Equals(literalType))
            {
                return SYSTEM_TABLE;
            }
            if ("GLOBAL_TEMPORARY".Equals(literalType))
            {
                return GLOBAL_TEMPORARY;
            }
            if ("LOCAL_TEMPORARY".Equals(literalType))
            {
                return LOCAL_TEMPORARY;
            }
            if ("ALIAS".Equals(literalType))
            {
                return ALIAS;
            }
            if ("SYNONYM".Equals(literalType))
            {
                return SYNONYM;
            }
            return OTHER;
        } // getTableType()
    } // TableType struct
} // org.apache.metamodel.schema namespace
