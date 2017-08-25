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
// https://github.com/apache/metamodel/blob/57e48be23844537690bdd6191ceb78adb4266e92/core/src/main/java/org/apache/metamodel/schema/MutableRelationship.java
using org.apache.metamodel.j2n.exceptions;
using org.apache.metamodel.j2n.slf4j;
using org.apache.metamodel.schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.apache.metamodel.core.schema
{
    /**
     * Immutable implementation of the Relationship interface.
     * 
     * The immutability help ensure integrity of object-relationships. To create
     * relationsips use the <code>createRelationship</code> method.
     */
    public class MutableRelationship : AbstractRelationship, Relationship // Serializable
    {
	    private static readonly long serialVersionUID = 238786848828528822L;

        private static readonly NLogger logger = NLoggerFactory.getLogger(typeof(MutableRelationship).Name);

	    private List<Column> _primaryColumns;
        private List<Column> _foreignColumns;

        /**
	     * Factory method to create relations between two tables by specifying which
	     * columns from the tables that enforce the relationship.
	     * 
	     * @param primaryColumns
	     *            the columns from the primary key table
	     * @param foreignColumns
	     *            the columns from the foreign key table
	     * @return the relation created
	     */
        public static Relationship createRelationship(List<Column> primaryColumns,
                                                      List<Column> foreignColumns)
        {
            Table primaryTable = checkSameTable(primaryColumns);
            Table foreignTable = checkSameTable(foreignColumns);
            MutableRelationship relation = new MutableRelationship(primaryColumns,
                    foreignColumns);

            if (primaryTable is MutableTable) {
                try
                {
                    ((MutableTable)primaryTable).addRelationship(relation);
                }
                catch (NUnsupportedOperationException e)
                {
                    // this is an allowed behaviour - not all tables need to support
                    // this method.
                    logger.debug(
                            "primary table ({}) threw exception when adding relationship",
                            primaryTable);
                }

                // Ticket #144: Some tables have relations with them selves and then
                // the
                // relationship should only be added once.
                if (foreignTable != primaryTable  &&  foreignTable is MutableTable) {
                    try
                    {
                        ((MutableTable)foreignTable).addRelationship(relation);
                    }
                    catch (NUnsupportedOperationException e)
                    {
                        // this is an allowed behaviour - not all tables need to
                        // support this method.
                        logger.debug(
                                "foreign table ({}) threw exception when adding relationship",
                                foreignTable);
                    }
                }
            }
            return relation;
        }

        public void remove()
        {
            Table primaryTable = getPrimaryTable();
            if (primaryTable is MutableTable) {
                ((MutableTable)primaryTable).removeRelationship(this);
            }
            Table foreignTable = getForeignTable();
            if (foreignTable is MutableTable) {
                ((MutableTable)foreignTable).removeRelationship(this);
            }
        }

        // @Override
        protected void finalize() // throws Throwable
        {
            // base.finalize();
            remove();
        }

        public static Relationship createRelationship(Column primaryColumn,
                                                      Column foreignColumn)
        {
            List<Column> pcols = new List<Column>();
            pcols.Add(primaryColumn);
            List<Column> fcols = new List<Column>();
            fcols.Add(foreignColumn);

            return createRelationship(pcols, fcols);
        }

        /**
	     * Prevent external instantiation
	     */
        private MutableRelationship(List<Column> primaryColumns, List<Column> foreignColumns)
        {
            _primaryColumns = primaryColumns;
            _foreignColumns = foreignColumns;
        }

        // @Override
        public List<Column> getPrimaryColumns()
        {
            return _primaryColumns;
        }

        // @Override
        public List<Column> getForeignColumns()
        {
            return _foreignColumns;
        }
    } // MutableRelationship class
} // org.apache.metamodel.core.schema
