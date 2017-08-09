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
// https://github.com/apache/metamodel/blob/master/core/src/main/java/org/apache/metamodel/query/FromClause.java
using org.apache.metamodel.query;
using org.apache.metamodel.schema;
using System;
using org.apache.metamodel.j2n.collections;

namespace org.apache.metamodel.core.query
{
    /**
     * Represents the FROM clause of a query containing FromItem's.
     * 
     * @see FromItem
     */
    public class FromClause : AbstractQueryClause<FromItem>
    {

        private static readonly long serialVersionUID = -8227310702249122115L;

        public FromClause(Query query) : base(query, PREFIX_FROM, DELIM_COMMA)
        {
            
        }

        /**
         * Gets the alias of a table, if it is registered (and visible, ie. not part
         * of a sub-query) in the FromClause
         * 
         * @param table
         *            the table to get the alias for
         * @return the alias or null if none is found
         */
        public String getAlias(Table table)
        {
            if (table != null)
            {
                foreach (FromItem item in getItems())
                {
                    String alias = item.getAlias(table);
                    if (alias != null)
                    {
                        return alias;
                    }
                }
            }
            return null;
        }

        /**
         * Retrieves a table by it's reference which may be it's alias or it's
         * qualified table name. Typically, this method is used to resolve a
         * SelectItem with a reference like "foo.bar", where "foo" may either be an
         * alias or a table name
         * 
         * @param reference
         * @return a FromItem which matches the provided reference string
         */
        public FromItem getItemByReference(String reference)
        {
            if (reference == null)
            {
                return null;
            }
            foreach (FromItem item in getItems())
            {
                FromItem result = getItemByReference(item, reference);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        public override void decorateIdentity(NList<object> identifiers)
        {
            throw new NotImplementedException();
        }

        private FromItem getItemByReference(FromItem item, String reference)
        {
            if (reference.Equals(item.toStringNoAlias(false)))
            {
                return item;
            }
            if (reference.Equals(item.toStringNoAlias(true)))
            {
                return item;
            }
            String alias = item.getAlias();
            if (reference.Equals(alias))
            {
                return item;
            }

            Table table = item.getTable();
            if (alias == null && table != null && reference.Equals(table.getName()))
            {
                return item;
            }

            JoinType join = item.getJoin();
            if (join != JoinType.None)
            {
                FromItem leftResult = getItemByReference(item.getLeftSide(), reference);
                if (leftResult != null)
                {
                    return leftResult;
                }
                FromItem rightResult = getItemByReference(item.getRightSide(), reference);
                if (rightResult != null)
                {
                    return rightResult;
                }
            }

            return null;
        }
    }
}
