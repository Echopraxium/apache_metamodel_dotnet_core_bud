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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/query/FilterClause.java
using org.apache.metamodel.query;
using org.apache.metamodel.schema;
using System;
using System.Collections.Generic;
using org.apache.metamodel.j2n.collections;

namespace org.apache.metamodel.core.query
{
    /**
     * Represents a clause of filters in the query. This type of clause is used for
     * the WHERE and HAVING parts of an SQL query.
     * 
     * Each provided FilterItem will be evaluated with the logical AND operator,
     * which requires that all filters are applied. Alternatively, if you wan't to
     * use an OR operator, then use the appropriate constructor of FilterItem to
     * create a composite filter.
     * 
     * @see FilterItem
     */
    public class FilterClause : AbstractQueryClause<FilterItem>
    {
        private static readonly long serialVersionUID = -9077342278766808934L;

        public FilterClause(Query query, String prefix) : base(query, prefix, DELIM_AND)
        {  
        }

        public List<SelectItem> getEvaluatedSelectItems()
        {
            List<FilterItem> items = getItems();
            return MetaModelHelper.getEvaluatedSelectItems(items);
        }

        /**
         * Traverses the items and evaluates whether or not the given column is
         * referenced in either of them.
         * 
         * @param column
         * @return true if the column is referenced in the clause or false if not
         */
        public bool isColumnReferenced(Column column)
        {
            foreach (FilterItem item in getItems())
            {
                if (item.isReferenced(column))
                {
                    return true;
                }
            }
            return false;
        }

        public override void decorateIdentity(NList<object> identifiers)
        {
            throw new NotImplementedException();
        }
    } // FilterClause class
} // org.apache.metamodel.core.query
