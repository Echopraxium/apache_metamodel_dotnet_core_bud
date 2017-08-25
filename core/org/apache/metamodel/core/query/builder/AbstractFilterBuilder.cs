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
using org.apache.metamodel.j2n.data.date_time;
using org.apache.metamodel.j2n.data.numbers;
using org.apache.metamodel.j2n.exceptions;
using org.apache.metamodel.query;
using org.apache.metamodel.schema;
using System;
using System.Collections.Generic;
using System.Text;
using org.apache.metamodel.core.data;
using org.apache.metamodel.query.builder;

namespace org.apache.metamodel.core.query.builder
{
    /**
     * Abstract implementation of {@link FilterBuilder} interface. All built filters
     * are channeled to the {@link #applyFilter(FilterItem)} method which needs to
     * be implemented by concrete implementations.
     */
    public abstract class AbstractFilterBuilder : FilterBuilder
    {
            private SelectItem _selectItem;

            public AbstractFilterBuilder(SelectItem selectItem)
            {
                this._selectItem = selectItem;
            }

            protected abstract GroupedQueryBuilder applyFilter(FilterItem filter);

            /**
             * Provides a way to
             */
            public GroupedQueryBuilder applyFilter(OperatorType operator_arg, Object operand)
            {
                return applyFilter(new FilterItem(_selectItem, operator_arg, operand));
            }

            // @Override
            public GroupedQueryBuilder In(IList<object> values) 
            {
                return applyFilter(new FilterItem(_selectItem, OperatorType.IN, values));
            }

            // @Override
            public GroupedQueryBuilder In(params NNumber[] numbers)
            {
                return applyFilter(new FilterItem(_selectItem, OperatorType.IN, numbers));
            }

            // @Override
            public GroupedQueryBuilder In(params String[] strings) 
            {
                return applyFilter(new FilterItem(_selectItem, OperatorType.IN, strings));
            }

            // @Override
            public GroupedQueryBuilder notIn(IList<object> values)
            {
                return applyFilter(new FilterItem(_selectItem, OperatorType.NOT_IN, values));
            }

            // @Override
            public GroupedQueryBuilder notIn(params NNumber[] numbers)
            {
                return applyFilter(new FilterItem(_selectItem, OperatorType.NOT_IN, numbers));
            }

            // @Override
            public GroupedQueryBuilder notIn(params String[] strings)
            {
                return applyFilter(new FilterItem(_selectItem, OperatorType.NOT_IN, strings));
            }

            // @Override
            public GroupedQueryBuilder isNull()
            {
                return applyFilter(new FilterItem(_selectItem, OperatorType.EQUALS_TO, null));
            }

            // @Override
            public GroupedQueryBuilder isNotNull()
            {
                return applyFilter(new FilterItem(_selectItem, OperatorType.DIFFERENT_FROM, null));
            }

            // @Override
            public GroupedQueryBuilder isEquals(Column column)
            {
                if (column == null)
                {
                    throw new ArgumentException("column cannot be null");
                }
                return applyFilter(new FilterItem(_selectItem, OperatorType.EQUALS_TO, new SelectItem(column)));
            }

            // @Override
            public GroupedQueryBuilder isEquals(NDate date)
            {
                if (date == null)
                {
                    throw new ArgumentException("date cannot be null");
                }
                return applyFilter(new FilterItem(_selectItem, OperatorType.EQUALS_TO, date));
            }

            // @Override
            public GroupedQueryBuilder isEquals(NNumber number)
            {
                if (number == null)
                {
                    throw new ArgumentException("number cannot be null");
                }
                return applyFilter(new FilterItem(_selectItem, OperatorType.EQUALS_TO, number));
            }

            // @Override
            public GroupedQueryBuilder isEquals(String string_arg)
            {
                if (string_arg == null)
                {
                    throw new ArgumentException("string cannot be null");
                }
                return applyFilter(new FilterItem(_selectItem, OperatorType.EQUALS_TO, string_arg));
            }

            // @Override
            public GroupedQueryBuilder isEquals(Boolean bool_arg)
            {
                if (bool_arg == null)
                {
                    throw new ArgumentException("bool cannot be null");
                }
                return applyFilter(new FilterItem(_selectItem, OperatorType.EQUALS_TO, bool_arg));
            }

            // @Override
            public GroupedQueryBuilder isEquals(Object obj)
            {
                if (obj == null)
                {
                    return isNull();
                }
                if (obj is Boolean) {
                    return isEquals((Boolean)obj);
                }
                if (obj is NNumber) {
                    return isEquals((NNumber)obj);
                }
                if (obj is NDate) {
                    return isEquals((NDate)obj);
                }
                if (obj is String) {
                    return isEquals((String)obj);
                }
                throw new NUnsupportedOperationException("Argument must be a Boolean, Number, Date or String. Found: " + obj);
            }

            // @Override
            public GroupedQueryBuilder differentFrom(Column column)
            {
                if (column == null)
                {
                    throw new ArgumentException("column cannot be null");
                }
                return applyFilter(new FilterItem(_selectItem, OperatorType.DIFFERENT_FROM, new SelectItem(column)));
            }

            // @Override
            public GroupedQueryBuilder differentFrom(NDate date)
            {
                if (date == null)
                {
                    throw new ArgumentException("date cannot be null");
                }
                return applyFilter(new FilterItem(_selectItem, OperatorType.DIFFERENT_FROM, date));
            }

            // @Override
            public GroupedQueryBuilder differentFrom(NNumber number)
            {
                if (number == null)
                {
                    throw new ArgumentException("number cannot be null");
                }
                return applyFilter(new FilterItem(_selectItem, OperatorType.DIFFERENT_FROM, number));
            }

            // @Override
            public GroupedQueryBuilder differentFrom(String string_arg)
            {
                if (string_arg == null)
                {
                    throw new ArgumentException("string cannot be null");
                }
                return applyFilter(new FilterItem(_selectItem, OperatorType.DIFFERENT_FROM, string_arg));
            }

            // @Override
            public GroupedQueryBuilder differentFrom(Boolean bool_arg)
            {
                if (bool_arg == null)
                {
                    throw new ArgumentException("bool cannot be null");
                }
                return applyFilter(new FilterItem(_selectItem, OperatorType.DIFFERENT_FROM, bool_arg));
            }

            // @Override
            public GroupedQueryBuilder differentFrom(Object obj)
            {
                if (obj == null)
                {
                    return isNotNull();
                }
                if (obj is Boolean) {
                    return differentFrom((Boolean)obj);
                }
                if (obj is NNumber) {
                    return differentFrom((NNumber)obj);
                }
                if (obj is NDate) {
                    return differentFrom((NDate)obj);
                }
                if (obj is String) {
                    return differentFrom((String)obj);
                }
                throw new NUnsupportedOperationException("Argument must be a Boolean, Number, Date or String. Found: " + obj);
            }

            // @Override
            public GroupedQueryBuilder greaterThan(Column column)
            {
                if (column == null)
                {
                    throw new ArgumentException("column cannot be null");
                }
                return applyFilter(new FilterItem(_selectItem, OperatorType.GREATER_THAN, new SelectItem(column)));
            }

            // @Override
            public GroupedQueryBuilder greaterThan(NDate date)
            {
                if (date == null)
                {
                    throw new ArgumentException("date cannot be null");
                }
                return applyFilter(new FilterItem(_selectItem, OperatorType.GREATER_THAN, date));
            }

            // @Override
            public GroupedQueryBuilder greaterThan(NNumber number)
            {
                if (number == null)
                {
                    throw new ArgumentException("number cannot be null");
                }
                return applyFilter(new FilterItem(_selectItem, OperatorType.GREATER_THAN, number));
            }

            // @Override
            public GroupedQueryBuilder greaterThan(String string_arg)
            {
                if (string_arg == null)
                {
                    throw new ArgumentException("string cannot be null");
                }
                return applyFilter(new FilterItem(_selectItem, OperatorType.GREATER_THAN, string_arg));
            }

        // @Override
        public GroupedQueryBuilder lessThan(Column column)
        {
            if (column == null)
            {
                throw new ArgumentException("column cannot be null");
            }
            return applyFilter(new FilterItem(_selectItem, OperatorType.LESS_THAN, new SelectItem(column)));
        }

        // @Override
        public GroupedQueryBuilder lessThan(NDate date)
        {
            if (date == null)
            {
                throw new ArgumentException("date cannot be null");
            }
            return applyFilter(new FilterItem(_selectItem, OperatorType.LESS_THAN, date));
        }

        // @Override
        public GroupedQueryBuilder lessThan(NNumber number)
        {
            if (number == null)
            {
                throw new ArgumentException("number cannot be null");
            }
            return applyFilter(new FilterItem(_selectItem, OperatorType.LESS_THAN, number));
        }

        // @Override
        public GroupedQueryBuilder lessThan(String string_arg)
        {
            if (string_arg == null)
            {
                throw new ArgumentException("string cannot be null");
            }
            return applyFilter(new FilterItem(_selectItem, OperatorType.LESS_THAN, string_arg));
        }

        // @Override
        public GroupedQueryBuilder lessThan(Object obj)
        {
            if (obj is NNumber) {
                return lessThan((NNumber)obj);
            }
            if (obj is NDate) {
                return lessThan((NDate)obj);
            }
            if (obj is String) {
                return lessThan((String)obj);
            }
            throw new NUnsupportedOperationException("Argument must be a Number, Date or String. Found: " + obj);
        }

        // @Override
        public GroupedQueryBuilder greaterThan(Object obj)
        {
            if (obj is NNumber) {
                return greaterThan((NNumber)obj);
            }
            if (obj is NDate) {
                return greaterThan((NDate)obj);
            }
            if (obj is String) {
                return greaterThan((String)obj);
            }
            throw new NUnsupportedOperationException("Argument must be a Number, Date or String. Found: " + obj);
        }

        // Greater than or equals

        // @Override
        public GroupedQueryBuilder greaterThanOrEquals(Column column)
        {
            if (column == null)
            {
                throw new ArgumentException("column cannot be null");
            }
            return applyFilter(new FilterItem(_selectItem, OperatorType.GREATER_THAN_OR_EQUAL, new SelectItem(column)));
        }

        // @Override
        public GroupedQueryBuilder gte(Column column)
        {
            return greaterThanOrEquals(column);
        }

        // @Override
        public GroupedQueryBuilder greaterThanOrEquals(NDate date)
        {
            if (date == null)
            {
                throw new ArgumentException("date cannot be null");
            }
            return applyFilter(new FilterItem(_selectItem, OperatorType.GREATER_THAN_OR_EQUAL, date));
        }

        // @Override
        public GroupedQueryBuilder gte(NDate date)
        {
            return greaterThanOrEquals(date);
        }

        // @Override
        public GroupedQueryBuilder greaterThanOrEquals(NNumber number)
        {
            if (number == null)
            {
                throw new ArgumentException("number cannot be null");
            }
            return applyFilter(new FilterItem(_selectItem, OperatorType.GREATER_THAN_OR_EQUAL, number));
        }

        // @Override
        public GroupedQueryBuilder gte(NNumber number)
        {
            return greaterThanOrEquals(number);
        }

        // @Override
        public GroupedQueryBuilder greaterThanOrEquals(String string_arg)
        {
            if (string_arg == null)
            {
                throw new ArgumentException("string cannot be null");
            }
            return applyFilter(new FilterItem(_selectItem, OperatorType.GREATER_THAN_OR_EQUAL, string_arg));
        }

        public GroupedQueryBuilder gte(String string_arg)
        {
            return greaterThanOrEquals(string_arg);
        }

        // @Override
        public GroupedQueryBuilder greaterThanOrEquals(Object obj)
        {
            if (obj is NNumber) {
                return greaterThanOrEquals((NNumber)obj);
            }
            if (obj is NDate) {
                return greaterThanOrEquals((NDate)obj);
            }
            if (obj is String) {
                return greaterThanOrEquals((String)obj);
            }
            throw new NUnsupportedOperationException("Argument must be a Number, Date or String. Found: " + obj);
        }

        public GroupedQueryBuilder gte(Object obj)
        {
            return greaterThanOrEquals(obj);
        }

        // Less than or equals

        // @Override
        public GroupedQueryBuilder lessThanOrEquals(Column column)
        {
            if (column == null)
            {
                throw new ArgumentException("column cannot be null");
            }
            return applyFilter(new FilterItem(_selectItem, OperatorType.LESS_THAN_OR_EQUAL, new SelectItem(column)));
        }

        // @Override
        public GroupedQueryBuilder lte(Column column)
        {
            return lessThanOrEquals(column);
        }

        // @Override
        public GroupedQueryBuilder lessThanOrEquals(NDate date)
        {
            if (date == null)
            {
                throw new ArgumentException("date cannot be null");
            }
            return applyFilter(new FilterItem(_selectItem, OperatorType.LESS_THAN_OR_EQUAL, date));
        }

        // @Override
        public GroupedQueryBuilder lte(NDate date)
        {
            return lessThanOrEquals(date);
        }

        // @Override
        public GroupedQueryBuilder lessThanOrEquals(NNumber number)
        {
            if (number == null)
            {
                throw new ArgumentException("number cannot be null");
            }
            return applyFilter(new FilterItem(_selectItem, OperatorType.LESS_THAN_OR_EQUAL, number));
        }

        // @Override
        public GroupedQueryBuilder lte(NNumber number)
        {
            return lessThanOrEquals(number);
        }

        // @Override
        public GroupedQueryBuilder lessThanOrEquals(String string_arg)
        {
            if (string_arg == null)
            {
                throw new ArgumentException("string cannot be null");
            }
            return applyFilter(new FilterItem(_selectItem, OperatorType.LESS_THAN_OR_EQUAL, string_arg));
        }

        public GroupedQueryBuilder lte(String string_arg)
        {
            return lessThanOrEquals(string_arg);
        }

        // @Override
        public GroupedQueryBuilder lessThanOrEquals(Object obj)
        {
            if (obj is NNumber) {
                return lessThanOrEquals((NNumber)obj);
            }
            if (obj is NDate) {
                return lessThanOrEquals((NDate)obj);
            }
            if (obj is String) {
                return lessThanOrEquals((String)obj);
            }
            throw new NUnsupportedOperationException("Argument must be a Number, Date or String. Found: " + obj);
        }

        public GroupedQueryBuilder lte(Object obj)
        {
            return lessThanOrEquals(obj);
        }

        // @Override
        public GroupedQueryBuilder like(String string_arg)
        {
            if (string_arg == null)
            {
                throw new ArgumentException("string cannot be null");
            }
            return applyFilter(new FilterItem(_selectItem, OperatorType.LIKE, string_arg));
        }

        // @Override
        public GroupedQueryBuilder notLike(String string_arg)
        {
            if (string_arg == null)
            {
                throw new ArgumentException("string cannot be null");
            }
            return applyFilter(new FilterItem(_selectItem, OperatorType.NOT_LIKE, string_arg));
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

        public abstract GroupedQueryBuilder groupBy(string columnName);

        public abstract GroupedQueryBuilder groupBy(params string[] columnNames);

        public abstract GroupedQueryBuilder groupBy(Column column);

        public abstract GroupedQueryBuilder groupBy(params Column[] columns);

        public abstract GroupedQueryBuilder equals(Column column);

        public abstract GroupedQueryBuilder equals(NDate date);

        public abstract GroupedQueryBuilder equals(NNumber number);

        public abstract GroupedQueryBuilder equals(string string_arg);

        public abstract GroupedQueryBuilder equals(bool bool_arg);

        public abstract GroupedQueryBuilder higherThan(Column column);

        public abstract GroupedQueryBuilder higherThan(NDate date);

        public abstract GroupedQueryBuilder higherThan(NNumber number);

        public abstract GroupedQueryBuilder higherThan(string string_arg);

        public abstract HavingBuilder having(FunctionType functionType, Column column);

        public abstract HavingBuilder having(SelectItem selectItem);

        public abstract HavingBuilder having(string columnExpression);

        public abstract SatisfiedQueryBuilder orderBy(FunctionType function, Column column);

        public abstract SatisfiedSelectBuilder select(params Column[] columns);

        public abstract SatisfiedQueryBuilder offset(int offset);

        public abstract SatisfiedQueryBuilder firstRow(int firstRow);

        public abstract SatisfiedQueryBuilder limit(int limit);

        public abstract SatisfiedQueryBuilder maxRows(int maxRows);

        public abstract FunctionSelectBuilder select(FunctionType function, Column column);

        public abstract SatisfiedQueryBuilder select(FunctionType function, string columnName);

        public abstract SatisfiedSelectBuilder selectCount();

        public abstract ColumnSelectBuilder select(string columnName);

        public abstract WhereBuilder where(Column column);

        public WhereBuilder where(string columnName)
        {
            throw new NotImplementedException();
        }

        public WhereBuilder where(ScalarFunction function, Column column)
        {
            throw new NotImplementedException();
        }

        public WhereBuilder where(ScalarFunction function, string columnName)
        {
            throw new NotImplementedException();
        }

        public SatisfiedQueryBuilder where(params FilterItem[] filters)
        {
            throw new NotImplementedException();
        }

        public SatisfiedQueryBuilder where(IEnumerable<FilterItem> filters)
        {
            throw new NotImplementedException();
        }

        public SatisfiedOrderByBuilder orderBy(string columnName)
        {
            throw new NotImplementedException();
        }

        public SatisfiedOrderByBuilder orderBy(Column column)
        {
            throw new NotImplementedException();
        }



        public Query toQuery()
        {
            throw new NotImplementedException();
        }

        public CompiledQuery compile()
        {
            throw new NotImplementedException();
        }

        public DataSet execute()
        {
            throw new NotImplementedException();
        }

        public Column findColumn(string columnName)
        {
            throw new NotImplementedException();
        }
    } // AbstractFilterBuilder class
} // org.apache.metamodel.core.query.builder
