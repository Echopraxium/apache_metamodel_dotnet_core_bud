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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/query/GroupByClause.java
using org.apache.metamodel.query;
using System;
using System.Collections.Generic;
using System.Text;
using org.apache.metamodel.j2n.collections;

namespace org.apache.metamodel.core.query
{
    /**
     * Represents the GROUP BY clause of a query that contains GroupByItem's.
     * 
     * @see GroupByItem
     */
    public class GroupByClause : AbstractQueryClause<GroupByItem> 
    {
        private static readonly long serialVersionUID = -3824934110331202101L;

        public GroupByClause(Query query): base(query, AbstractQueryClause<object>.PREFIX_GROUP_BY,
                                                AbstractQueryClause<object>.DELIM_COMMA)
        {        
        }

        public override void decorateIdentity(NList<object> identifiers)
        {
            throw new NotImplementedException();
        }

        public List<SelectItem> getEvaluatedSelectItems()
        {
            List<SelectItem>  result = new List<SelectItem>();
            List<GroupByItem> items  = getItems();
            foreach (GroupByItem item in items)
            {
                result.Add(item.getSelectItem());
            }
            return result;
        } // getEvaluatedSelectItems()
    } // GroupByClause class
} // org.apache.metamodel.core.query
