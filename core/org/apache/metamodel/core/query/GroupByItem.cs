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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/query/GroupByItem.java
using org.apache.metamodel.j2n;
using org.apache.metamodel.j2n.collections;
using org.apache.metamodel.query;
using org.apache.metamodel.util;
using System;
using System.Collections.Generic;
using System.Text;


namespace org.apache.metamodel.core.query
{
    /**
     * Represents a GROUP BY item. GroupByItems always use a select item (that may
     * or not be a part of the query already) for grouping.
     * 
     * @see GroupByClause
     */
    public class GroupByItem : BaseObject, QueryItem, NCloneable 
    {
        private static readonly long serialVersionUID = 5218878395877852919L;

        private SelectItem _selectItem;
        private Query _query;

        /**
         * Constructs a GROUP BY item based on a select item that should be grouped.
         * 
         * @param selectItem
         */
        public GroupByItem(SelectItem selectItem)
        {
            if (selectItem == null)
            {
                throw new ArgumentException("SelectItem cannot be null");
            }
            _selectItem = selectItem;
        }

        public SelectItem getSelectItem()
        {
            return _selectItem;
        }

        // @Override
        public String toSql()
        {
            return toSql(false);
        }

        // @Override
        public String toSql(bool? includeSchemaInColumnPaths)
        {
            String sameQueryAlias = _selectItem.getSameQueryAlias(includeSchemaInColumnPaths);
            return sameQueryAlias;
        }

        // @Override
        public String toString()
        {
            return toSql();
        }

        public Query getQuery()
        {
            return _query;
        }

        public QueryItem setQuery(Query query)
        {
            _query = query;
            if (_selectItem != null)
            {
                _selectItem.setQuery(query);
            }
            return this;
        }

        // @Override
        public GroupByItem clone()
        {
            GroupByItem g = new GroupByItem(_selectItem.clone());
            return g;
        }

        // @Override
        public override void decorateIdentity(NList<Object> identifiers)
        {
            identifiers.Add(_selectItem);
        }
    } // GroupByItem class
} // org.apache.metamodel.core.query namespace
