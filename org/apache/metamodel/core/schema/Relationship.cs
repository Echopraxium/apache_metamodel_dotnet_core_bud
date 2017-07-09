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
// https://github.com/apache/metamodel/blob/master/core/src/main/java/org/apache/metamodel/schema/Relationship.java
namespace org.apache.metamodel.schema
{
    public interface Relationship
    {
        /**
         * Gets the table of the primary key column(s).
         * 
         * @return the table of the primary key column(s).
         */
        Table getPrimaryTable();

        /**
         * Gets the primary key columns of this relationship.
         * 
         * @return an array of primary key columns.
         */
        Column[] getPrimaryColumns();

        /**
         * Gets the table of the foreign key column(s).
         * 
         * @return the table of the foreign key column(s).
         */
        Table getForeignTable();

        /**
         * Gets the foreign key columns of this relationship.
         * 
         * @return an array of foreign key columns.
         */
        Column[] getForeignColumns();

        /**
         * Determines whether this relationship contains a specific pair of columns
         * 
         * @param pkColumn
         *            primary key column
         * @param fkColumn
         *            foreign key column
         * @return true if this relation contains the specified primary and foreign
         *         columns as a part of the relation
         */
        bool containsColumnPair(Column pkColumn, Column fkColumn);
    } // Relationship interface
} // org.apache.metamodel.schema namespace
