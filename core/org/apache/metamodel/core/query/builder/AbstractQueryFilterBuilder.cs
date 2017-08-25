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
// https://github.com/apache/metamodel/blob/6f9e09449c14b6f6e5256ad724191667a7189d7a/core/src/main/java/org/apache/metamodel/query/builder/AbstractQueryFilterBuilder.java
using org.apache.metamodel.core.query.builder;
using org.apache.metamodel.j2n.data.date_time;
using org.apache.metamodel.j2n.data.numbers;
using org.apache.metamodel.query;
using org.apache.metamodel.schema;
using System;
using System.Collections.Generic;
using System.Text;
using org.apache.metamodel.query.builder;

namespace org.apache.metamodel.core.query.builder
{
    public abstract class AbstractQueryFilterBuilder : GroupedQueryBuilderCallback, FilterBuilder
    {
        protected AbstractFilterBuilder _filterBuilder;

        public abstract GroupedQueryBuilder applyFilter(FilterItem filter);

        // @Override
        public GroupedQueryBuilder In(IList<object> values)
        {
            return _filterBuilder.In(values);
        }

        // @Override
        public GroupedQueryBuilder In(params NNumber[] numbers)
        {
            return _filterBuilder.In(numbers);
        }

        // @Override
        public GroupedQueryBuilder In(params String[] strings)
        {
            return _filterBuilder.In(strings);
        }

        // @Override
        public GroupedQueryBuilder notIn(IList<object> values)
        {
            return _filterBuilder.notIn(values);
        }

        // @Override
        public GroupedQueryBuilder notIn(params NNumber[] numbers)
        {
            return _filterBuilder.notIn(numbers);
        }

        // @Override
        public GroupedQueryBuilder notIn(params String[] strings)
        {
            return _filterBuilder.notIn(strings);
        }

        // @Override
        public GroupedQueryBuilder isNull()
        {
            return _filterBuilder.isNull();
        }

        // @Override
        public GroupedQueryBuilder isNotNull()
        {
            return _filterBuilder.isNotNull();
        }

        // @Override
        public GroupedQueryBuilder isEquals(Column column)
        {
            return _filterBuilder.isEquals(column);
        }

        // @Override
        public GroupedQueryBuilder isEquals(NDate date)
        {
            return _filterBuilder.isEquals(date);
        }

        // @Override
        public GroupedQueryBuilder isEquals(NNumber number)
        {
            return _filterBuilder.isEquals(number);
        }

        // @Override
        public GroupedQueryBuilder isEquals(String string_arg)
        {
            return _filterBuilder.isEquals(string_arg);
        }

        // Override
        public GroupedQueryBuilder isEquals(Boolean bool_arg)
        {
            return _filterBuilder.isEquals(bool_arg);
        }

        // @Override
        public GroupedQueryBuilder isEquals(Object obj)
        {
            return _filterBuilder.isEquals(obj);
        }

        // @Override
        public GroupedQueryBuilder differentFrom(Column column)
        {
            return _filterBuilder.differentFrom(column);
        }

        // @Override
        public GroupedQueryBuilder differentFrom(NDate date)
        {
            return _filterBuilder.differentFrom(date);
        }

        // @Override
        public GroupedQueryBuilder differentFrom(NNumber number)
        {
            return _filterBuilder.differentFrom(number);
        }

        // @Override
        public GroupedQueryBuilder differentFrom(String string_arg)
        {
            return _filterBuilder.differentFrom(string_arg);
        }

        // @Override
        public GroupedQueryBuilder differentFrom(Boolean bool_arg)
        {
            return _filterBuilder.differentFrom(bool_arg);
        }

        // @Override
        public GroupedQueryBuilder differentFrom(Object obj)
        {
            return _filterBuilder.differentFrom(obj);
        }

        public GroupedQueryBuilder greaterThan(Column column)
        {
            return _filterBuilder.greaterThan(column);
        }

        // @Override
        public GroupedQueryBuilder greaterThan(Object obj)
        {
            return _filterBuilder.greaterThan(obj);
        }

        // @Override
        public GroupedQueryBuilder greaterThan(NDate date)
        {
            return _filterBuilder.greaterThan(date);
        }

        // @Override
        public GroupedQueryBuilder greaterThan(NNumber number)
        {
            return _filterBuilder.greaterThan(number);
        }

        // @Override
        public GroupedQueryBuilder greaterThan(String string_arg)
        {
            return _filterBuilder.greaterThan(string_arg);
        }

        // @Override
        public GroupedQueryBuilder lessThan(Column column)
        {
            return _filterBuilder.lessThan(column);
        }

        // @Override
        public GroupedQueryBuilder lessThan(NDate date)
        {
            return _filterBuilder.lessThan(date);
        }

        // @Override
        public GroupedQueryBuilder lessThan(NNumber number)
        {
            return _filterBuilder.lessThan(number);
        }

        // @Override
        public GroupedQueryBuilder lessThan(String string_arg)
        {
            return _filterBuilder.lessThan(string_arg);
        }

        // @Override
        public GroupedQueryBuilder lessThan(Object obj)
        {
            return _filterBuilder.lessThan(obj);
        }

        // @Override
        public GroupedQueryBuilder greaterThanOrEquals(Column column)
        {
            return _filterBuilder.greaterThanOrEquals(column);
        }

        // @Override
        public GroupedQueryBuilder greaterThanOrEquals(NDate date)
        {
            return _filterBuilder.greaterThanOrEquals(date);
        }

        // @Override
        public GroupedQueryBuilder greaterThanOrEquals(NNumber number)
        {
            return _filterBuilder.greaterThanOrEquals(number);
        }

        // @Override
        public GroupedQueryBuilder greaterThanOrEquals(String string_arg)
        {
            return _filterBuilder.greaterThanOrEquals(string_arg);
        }

        // @Override
        public GroupedQueryBuilder greaterThanOrEquals(Object obj)
        {
            return _filterBuilder.greaterThanOrEquals(obj);
        }

        // @Override
        public GroupedQueryBuilder gte(Column column)
        {
            return _filterBuilder.greaterThanOrEquals(column);
        }

        // @Override
        public GroupedQueryBuilder gte(NDate date)
        {
            return _filterBuilder.greaterThanOrEquals(date);
        }

        // @Override
        public GroupedQueryBuilder gte(NNumber number)
        {
            return _filterBuilder.greaterThanOrEquals(number);
        }

        // @Override
        public GroupedQueryBuilder gte(String string_arg)
        {
            return _filterBuilder.greaterThanOrEquals(string_arg);
        }

        // @Override
        public GroupedQueryBuilder gte(Object obj)
        {
            return _filterBuilder.greaterThanOrEquals(obj);
        }

        // @Override
        public GroupedQueryBuilder lessThanOrEquals(Column column)
        {
            return _filterBuilder.lessThanOrEquals(column);
        }

        // @Override
        public GroupedQueryBuilder lessThanOrEquals(NDate date)
        {
            return _filterBuilder.lessThanOrEquals(date);
        }

        // @Override
        public GroupedQueryBuilder lessThanOrEquals(NNumber number)
        {
            return _filterBuilder.lessThanOrEquals(number);
        }

        // @Override
        public GroupedQueryBuilder lessThanOrEquals(String string_arg)
        {
            return _filterBuilder.lessThanOrEquals(string_arg);
        }

        // @Override
        public GroupedQueryBuilder lessThanOrEquals(Object obj)
        {
            return _filterBuilder.lessThanOrEquals(obj);
        }

        // @Override
        public GroupedQueryBuilder lte(Column column)
        {
            return _filterBuilder.lessThanOrEquals(column);
        }

        // @Override
        public GroupedQueryBuilder lte(NDate date)
        {
            return _filterBuilder.lessThanOrEquals(date);
        }

        // @Override
        public GroupedQueryBuilder lte(NNumber number)
        {
            return _filterBuilder.lessThanOrEquals(number);
        }

        // @Override
        public GroupedQueryBuilder lte(String string_arg)
        {
            return _filterBuilder.lessThanOrEquals(string_arg);
        }

        // @Override
        public GroupedQueryBuilder lte(Object obj)
        {
            return _filterBuilder.lessThanOrEquals(obj);
        }

        // @Override
        public GroupedQueryBuilder like(String string_arg)
        {
            return _filterBuilder.like(string_arg);
        }

        // @Override
        public GroupedQueryBuilder notLike(String string_arg)
        {
            return _filterBuilder.notLike(string_arg);
        }

        // @Override
        public GroupedQueryBuilder gt(Column column)
        {
            return greaterThan(column);
        }

        // @Override
        public GroupedQueryBuilder gt(NDate date)
        {
            return greaterThan(date);
        }

        // @Override
        public GroupedQueryBuilder gt(NNumber number)
        {
            return greaterThan(number);
        }

        // @Override
        public GroupedQueryBuilder gt(String string_arg)
        {
            return greaterThan(string_arg);
        }

        // @Override
        public GroupedQueryBuilder lt(Column column)
        {
            return lessThan(column);
        }

        public GroupedQueryBuilder lt(NDate date)
        {
            return lessThan(date);
        }

        public GroupedQueryBuilder lt(NNumber number)
        {
            return lessThan(number);
        }

        public GroupedQueryBuilder lt(String string_arg)
        {
            return lessThan(string_arg);
        }

        // @Override
        public GroupedQueryBuilder eq(Boolean bool_arg)
        {
            return isEquals(bool_arg);
        }

        // @Override
        public GroupedQueryBuilder eq(Column column)
        {
            return isEquals(column);
        }

        // @Override
        public GroupedQueryBuilder eq(NDate date)
        {
            return isEquals(date);
        }

        // @Override
        public GroupedQueryBuilder eq(NNumber number)
        {
            return isEquals(number);
        }

        // @Override
        public GroupedQueryBuilder eq(String string_arg)
        {
            return isEquals(string_arg);
        }

        // @Override
        public GroupedQueryBuilder eq(Object obj)
        {
            return isEquals(obj);
        }

        // @Override
        public GroupedQueryBuilder ne(Boolean bool_arg)
        {
            return differentFrom(bool_arg);
        }

        // @Override
        public GroupedQueryBuilder ne(Column column)
        {
            return differentFrom(column);
        }

        // @Override
        public GroupedQueryBuilder ne(NDate date)
        {
            return differentFrom(date);
        }

        // @Override
        public GroupedQueryBuilder ne(NNumber number)
        {
            return differentFrom(number);
        }

        // @Override
        public GroupedQueryBuilder ne(String string_arg)
        {
            return differentFrom(string_arg);
        }

        // @Override
        public GroupedQueryBuilder ne(Object obj)
        {
            return differentFrom(obj);
        }

        // @Override
        public GroupedQueryBuilder lt(Object obj)
        {
            return lessThan(obj);
        }

        // @Override
        public GroupedQueryBuilder gt(Object obj)
        {
            return greaterThan(obj);
        }

        public GroupedQueryBuilder equals(Column column)
        {
            throw new NotImplementedException();
        }

        public GroupedQueryBuilder equals(NDate date)
        {
            throw new NotImplementedException();
        }

        public GroupedQueryBuilder equals(NNumber number)
        {
            throw new NotImplementedException();
        }

        public GroupedQueryBuilder equals(string string_arg)
        {
            throw new NotImplementedException();
        }

        public GroupedQueryBuilder equals(bool bool_arg)
        {
            throw new NotImplementedException();
        }

        public GroupedQueryBuilder higherThan(Column column)
        {
            throw new NotImplementedException();
        }

        public GroupedQueryBuilder higherThan(NDate date)
        {
            throw new NotImplementedException();
        }

        public GroupedQueryBuilder higherThan(NNumber number)
        {
            throw new NotImplementedException();
        }

        public GroupedQueryBuilder higherThan(string string_arg)
        {
            throw new NotImplementedException();
        }

        private class _AbstractFilterBuilder_Impl : AbstractFilterBuilder
        {
            AbstractQueryFilterBuilder _this;
            public _AbstractFilterBuilder_Impl(AbstractQueryFilterBuilder this_arg, SelectItem select_item): base(select_item)
            {
                _this = this_arg;
            }

            public override GroupedQueryBuilder equals(Column column)
            {
                throw new NotImplementedException();
            }

            public override GroupedQueryBuilder equals(NDate date)
            {
                throw new NotImplementedException();
            }

            public override GroupedQueryBuilder equals(NNumber number)
            {
                throw new NotImplementedException();
            }

            public override GroupedQueryBuilder equals(string string_arg)
            {
                throw new NotImplementedException();
            }

            public override GroupedQueryBuilder equals(bool bool_arg)
            {
                throw new NotImplementedException();
            }

            public override SatisfiedQueryBuilder firstRow(int firstRow)
            {
                throw new NotImplementedException();
            }

            public override GroupedQueryBuilder groupBy(string columnName)
            {
                throw new NotImplementedException();
            }

            public override GroupedQueryBuilder groupBy(params string[] columnNames)
            {
                throw new NotImplementedException();
            }

            public override GroupedQueryBuilder groupBy(Column column)
            {
                throw new NotImplementedException();
            }

            public override GroupedQueryBuilder groupBy(params Column[] columns)
            {
                throw new NotImplementedException();
            }

            public override HavingBuilder having(FunctionType functionType, Column column)
            {
                throw new NotImplementedException();
            }

            public override HavingBuilder having(SelectItem selectItem)
            {
                throw new NotImplementedException();
            }

            public override HavingBuilder having(string columnExpression)
            {
                throw new NotImplementedException();
            }

            public override GroupedQueryBuilder higherThan(Column column)
            {
                throw new NotImplementedException();
            }

            public override GroupedQueryBuilder higherThan(NDate date)
            {
                throw new NotImplementedException();
            }

            public override GroupedQueryBuilder higherThan(NNumber number)
            {
                throw new NotImplementedException();
            }

            public override GroupedQueryBuilder higherThan(string string_arg)
            {
                throw new NotImplementedException();
            }

            public override SatisfiedQueryBuilder limit(int limit)
            {
                throw new NotImplementedException();
            }

            public override SatisfiedQueryBuilder maxRows(int maxRows)
            {
                throw new NotImplementedException();
            }

            public override SatisfiedQueryBuilder offset(int offset)
            {
                throw new NotImplementedException();
            }

            public override SatisfiedQueryBuilder orderBy(FunctionType function, Column column)
            {
                throw new NotImplementedException();
            }

            public override SatisfiedSelectBuilder select(params Column[] columns)
            {
                throw new NotImplementedException();
            }

            public override FunctionSelectBuilder select(FunctionType function, Column column)
            {
                throw new NotImplementedException();
            }

            public override SatisfiedQueryBuilder select(FunctionType function, string columnName)
            {
                throw new NotImplementedException();
            }

            public override ColumnSelectBuilder select(string columnName)
            {
                throw new NotImplementedException();
            }

            public override SatisfiedSelectBuilder selectCount()
            {
                throw new NotImplementedException();
            }

            public override WhereBuilder where(Column column)
            {
                throw new NotImplementedException();
            }

            protected override GroupedQueryBuilder applyFilter(FilterItem filter)
            {
                return _this.applyFilter(filter);
                //throw new NotImplementedException();
            }
        } // _AbstractFilterBuilder_Implclass

        public AbstractQueryFilterBuilder(SelectItem selectItem, GroupedQueryBuilder queryBuilder) : base(queryBuilder)
        {
            _filterBuilder = new _AbstractFilterBuilder_Impl(this, selectItem);
        }
    }
}
