﻿/**
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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/query/builder/HavingBuilderImpl.java
using org.apache.metamodel.j2n.collections;
using org.apache.metamodel.query;
using org.apache.metamodel.schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.apache.metamodel.core.query.builder
{
    public sealed class HavingBuilderImpl : AbstractQueryFilterBuilder, HavingBuilder, SatisfiedHavingBuilder 
    {
        private Query             _query;
        private NList<FilterItem> _orFilters;
        private FilterItem        _parentOrFilter;
       // 
        public HavingBuilderImpl(SelectItem selectItem, Query query, GroupedQueryBuilder queryBuilder) : base(selectItem, queryBuilder)
        {           
            _query = query;
            _orFilters = new NList<FilterItem>();
        } // constructor

        public HavingBuilderImpl(FunctionType function, Column column, Query query, GroupedQueryBuilder queryBuilder) :
             this(new SelectItem(function, column), query, queryBuilder)
        {
           
        } // constructor

        public HavingBuilderImpl(FunctionType function, Column column, Query query, FilterItem parentOrFilter,
                List<FilterItem> orFilters, GroupedQueryBuilder queryBuilder) : this(function, column, query, queryBuilder)
        {            
        } // constructor


        // @Override
        public override GroupedQueryBuilder applyFilter(FilterItem filter)
        {
            if (_parentOrFilter == null)
            {
                _query.having(filter);
            }
            else
            {
                if (_parentOrFilter.getChildItemCount() == 1)
                {
                    _query.getHavingClause().removeItem(_orFilters[0]);
                    _query.getHavingClause().addItem(_parentOrFilter);
                }
            }
            _orFilters.Add(filter);
            return this;
        } // applyFilter()

        // @Override
        public HavingBuilder or(FunctionType function, Column column)
        {
            if (function == null)
            {
                throw new ArgumentException("function cannot be null");
            }
            if (column == null)
            {
                throw new ArgumentException("column cannot be null");
            }
            if (_parentOrFilter == null)
            {
                _parentOrFilter = new FilterItem(_orFilters);
            }
            return new HavingBuilderImpl(function, column, _query, _parentOrFilter, _orFilters, getQueryBuilder());
        }

        // @Override
        public HavingBuilder and(FunctionType function, Column column)
        {
            return getQueryBuilder().having(function, column);
        }
    }
}
