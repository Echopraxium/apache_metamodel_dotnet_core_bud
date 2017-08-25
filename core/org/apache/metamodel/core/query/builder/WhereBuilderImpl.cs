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
// https://github.com/apache/metamodel/blob/6f9e09449c14b6f6e5256ad724191667a7189d7a/core/src/main/java/org/apache/metamodel/query/builder/WhereBuilderImpl.java
using org.apache.metamodel.j2n.collections;
using org.apache.metamodel.query;
using org.apache.metamodel.schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.apache.metamodel.core.query.builder
{
    public sealed class WhereBuilderImpl : AbstractQueryFilterBuilder,
                                           WhereBuilder, SatisfiedWhereBuilder
    {
        private Query             _query;
        private NList<FilterItem> _orFilters;
        private FilterItem        _parentOrFilter;

        public WhereBuilderImpl(Column column, Query query, GroupedQueryBuilder queryBuilder) :
                                this(new SelectItem(column), query, queryBuilder)
        {           
        }

        public WhereBuilderImpl(SelectItem selectItem, Query query, GroupedQueryBuilder queryBuilder) : base(selectItem, queryBuilder)
        {           
            _query     = query;
            _orFilters = new NList<FilterItem>();
        }

        public WhereBuilderImpl(Column column, Query query, FilterItem parentOrFilter, NList<FilterItem> orFilters,
                GroupedQueryBuilder queryBuilder) :  base(new SelectItem(column), queryBuilder)
        {          
            _query          = query;
            _parentOrFilter = parentOrFilter;
            _orFilters      = orFilters;
        }

        // @Override
        public override GroupedQueryBuilder applyFilter(FilterItem filter)
        {
            if (_parentOrFilter == null)
            {
                _query.where(filter);
            }
            else
            {
                if (_parentOrFilter.getChildItemCount() == 1)
                {
                    _query.getWhereClause().removeItem(_orFilters[0]);
                    _query.getWhereClause().addItem(_parentOrFilter);
                }
            }
            _orFilters.add(filter);
            return this;
        }

        // @Override
        public WhereBuilder or(String columnName)
        {
            Column column = findColumn(columnName);
            return or(column);
        }

        // @Override
        public WhereBuilder or(Column column)
        {
            if (_parentOrFilter == null)
            {
                _parentOrFilter = new FilterItem(_orFilters);
            }
            return new WhereBuilderImpl(column, _query, _parentOrFilter, _orFilters, getQueryBuilder());
        }

        // @Override
        public WhereBuilder And(String columnName)
        {
            Column column = findColumn(columnName);
            return And(column);
        }

        // @Override
        public WhereBuilder And(Column column)
        {
            return getQueryBuilder().where(column);
        }

        // @Override
        public SatisfiedWhereBuilder eq(QueryParameter queryParameter)
        {
            return isEquals(queryParameter);
        }

        // @Override
        public SatisfiedWhereBuilder isEquals(QueryParameter queryParameter)
        {
            if (queryParameter == null)
            {
                throw new ArgumentException("query parameter cannot be null");
            }
            return (SatisfiedWhereBuilder) _filterBuilder.applyFilter(OperatorType.EQUALS_TO, queryParameter);
        }

        // @Override
        public SatisfiedWhereBuilder differentFrom(QueryParameter queryParameter)
        {
            return ne(queryParameter);
        }

        // @Override
        public SatisfiedWhereBuilder ne(QueryParameter queryParameter)
        {
            if (queryParameter == null)
            {
                throw new ArgumentException("query parameter cannot be null");
            }
            return (SatisfiedWhereBuilder) _filterBuilder.applyFilter(OperatorType.DIFFERENT_FROM, queryParameter);
        }

        // @Override
        public SatisfiedWhereBuilder greaterThan(QueryParameter queryParameter)
        {
            return gt(queryParameter);
        }

        // @Override
        public SatisfiedWhereBuilder gt(QueryParameter queryParameter)
        {
            if (queryParameter == null)
            {
                throw new ArgumentException("query parameter cannot be null");
            }
            return (SatisfiedWhereBuilder) _filterBuilder.applyFilter(OperatorType.GREATER_THAN, queryParameter);
        }

        // @Override
        public SatisfiedWhereBuilder lessThan(QueryParameter queryParameter)
        {
            return lt(queryParameter);
        }

        // @Override
        public SatisfiedWhereBuilder lt(QueryParameter queryParameter)
        {
            if (queryParameter == null)
            {
                throw new ArgumentException("query parameter cannot be null");
            }
            return (SatisfiedWhereBuilder) _filterBuilder.applyFilter(OperatorType.LESS_THAN, queryParameter);
        }

        // @Override
        public SatisfiedWhereBuilder like(QueryParameter queryParameter)
        {
            if (queryParameter == null)
            {
                throw new ArgumentException("query parameter cannot be null");
            }
            return (SatisfiedWhereBuilder) _filterBuilder.applyFilter(OperatorType.LIKE, queryParameter);
        }
    } // WhereBuilderImpl class
} // org.apache.metamodel.core.query.builder
