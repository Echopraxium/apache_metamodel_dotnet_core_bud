// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/query/builder/SatisfiedOrderByBuilderImpl.java
using org.apache.metamodel.query;
using org.apache.metamodel.schema;
using System;
using System.Collections.Generic;
using System.Text;
using static org.apache.metamodel.core.query.OrderByItem;

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
namespace org.apache.metamodel.core.query.builder
{
    public sealed class SatisfiedOrderByBuilderImpl : GroupedQueryBuilderCallback, SatisfiedOrderByBuilder
    {
        private OrderByItem orderByitem;

        public SatisfiedOrderByBuilderImpl(Column column, Query query,
                GroupedQueryBuilder queryBuilder) : base(queryBuilder)
        {           
            orderByitem = new OrderByItem(new SelectItem(column));
            query.orderBy(orderByitem);
        }

        public SatisfiedOrderByBuilderImpl(FunctionType function, Column column,
                Query query, GroupedQueryBuilder queryBuilder) : base(queryBuilder)
        {
            orderByitem = new OrderByItem(new SelectItem(function, column));
            query.orderBy(orderByitem);
        }

        // @Override
        public GroupedQueryBuilder asc()
        {
            orderByitem.setDirection(Direction.ASC);
            return getQueryBuilder();
        }

        // @Override
        public GroupedQueryBuilder desc()
        {
            orderByitem.setDirection(Direction.DESC);
            return getQueryBuilder();
        }

        // @Override
        public SatisfiedOrderByBuilder And(Column column)
        {
            return getQueryBuilder().orderBy(column);
        }
    } // SatisfiedOrderByBuilderImpl class
} // org.apache.metamodel.core.query.builder
