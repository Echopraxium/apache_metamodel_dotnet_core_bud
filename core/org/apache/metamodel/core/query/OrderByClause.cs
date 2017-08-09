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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/query/OrderByClause.java
using org.apache.metamodel.query;
using System.Collections.Generic;

namespace org.apache.metamodel.core.query
{
    /**
     * Represents the ORDER BY clause of a query containing OrderByItem's. The order
     * and direction of the OrderItems define the way that the result of a query
     * will be sorted.
     * 
     * @see OrderByItem
     */
    public class OrderByClause : AbstractQueryClause<OrderByItem>
    {
        private static readonly long serialVersionUID = 2441926135870143715L;

        public OrderByClause(Query query) : base(query, PREFIX_ORDER_BY, DELIM_COMMA)
        {          
        } // constructor

        public List<SelectItem> getEvaluatedSelectItems()
        {
            List<SelectItem>  result = new List<SelectItem>();
            List<OrderByItem> items  = getItems();
            foreach (OrderByItem item in items)
            {
                result.Add(item.getSelectItem());
            }
            return result;
        } // getEvaluatedSelectItems()
    }
}
