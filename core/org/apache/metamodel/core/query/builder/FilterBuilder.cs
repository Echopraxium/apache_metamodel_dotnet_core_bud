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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/query/builder/FilterBuilder.java
using org.apache.metamodel.j2n.data.date_time;
using org.apache.metamodel.j2n.data.numbers;
using org.apache.metamodel.schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.apache.metamodel.core.query.builder
{
    /**
     * Interface for builder callbacks that "respond" to filter condition building.
     *
     * @param <B>
     *            the builder type to return once filter has been created.
     */
    public interface FilterBuilder : GroupedQueryBuilder
    {
        /**
         * Not null
         */
        GroupedQueryBuilder isNull();

        /**
         * Is not null
         */
        GroupedQueryBuilder isNotNull();

        /**
         * In ...
         */
        GroupedQueryBuilder In(IList<object> values);

        /**
         * In ...
         */
        GroupedQueryBuilder In(params NNumber[] numbers);

        /**
         * In ...
         */
        GroupedQueryBuilder In(params String[] strings_args);

        /**
         * Not in ...
         */
        GroupedQueryBuilder notIn(IList<object> values);

        /**
         * Not in ...
         */
        GroupedQueryBuilder notIn(params NNumber[] numbers);

        /**
         * Not in ...
         */
        GroupedQueryBuilder notIn(params String[] strings);

        /**
         * Like ...
         *
         * (use '%' as wildcard).
         */
        GroupedQueryBuilder like(String string_arg);

        /**
         * Not like ...
         *
         * (use '%' as wildcard).
         */
        GroupedQueryBuilder notLike(String string_arg);

        /**
         * Equal to ...
         */
        GroupedQueryBuilder eq(Column column);

        /**
         * Equal to ...
         */
        GroupedQueryBuilder eq(NDate date);

        /**
         * Equal to ...
         */
        GroupedQueryBuilder eq(NNumber number);

        /**
         * Equal to ...
         */
        GroupedQueryBuilder eq(String string_arg);

        /**
         * Equal to ...
         */
        GroupedQueryBuilder eq(Boolean bool_arg);

        /**
         * Equal to ...
         */
        GroupedQueryBuilder eq(Object obj);

        /**
         * Equal to ...
         */
        GroupedQueryBuilder isEquals(Column column);

        /**
         * Equal to ...
         */
        GroupedQueryBuilder isEquals(NDate date);

        /**
         * Equal to ...
         */
        GroupedQueryBuilder isEquals(NNumber number);

        /**
         * Equal to ...
         */
        GroupedQueryBuilder isEquals(String string_arg);

        /**
         * Equal to ...
         */
        GroupedQueryBuilder isEquals(Boolean bool_arg);

        /**
         * Equal to ...
         */
        GroupedQueryBuilder isEquals(Object obj);

        /**
         * Equal to ...
         *
         * @deprecated use 'eq' or 'isEquals' instead.
         */
        // @Deprecated
        GroupedQueryBuilder equals(Column column);

        /**
         * Equal to ...
         *
         * @deprecated use 'eq' or 'isEquals' instead.
         */
        // @Deprecated
        GroupedQueryBuilder equals(NDate date);

        /**
         * Equal to ...
         *
         * @deprecated use 'eq' or 'isEquals' instead.
         */
        // @Deprecated
        GroupedQueryBuilder equals(NNumber number);

        /**
         * Equal to ...
         *
         * @deprecated use 'eq' or 'isEquals' instead.
         */
        // @Deprecated
        GroupedQueryBuilder equals(String string_arg);

        /**
         * Equal to ...
         *
         * @deprecated use 'eq' or 'isEquals' instead.
         */
        // @Deprecated
        GroupedQueryBuilder equals(Boolean bool_arg);

        /**
         * Not equal to ...
         */
        GroupedQueryBuilder differentFrom(Column column);

        /**
         * Not equal to ...
         */
        GroupedQueryBuilder differentFrom(NDate date);

        /**
         * Not equal to ...
         */
        GroupedQueryBuilder differentFrom(NNumber number);

        /**
         * Not equal to ...
         */
        GroupedQueryBuilder differentFrom(String string_arg);

        /**
         * Not equal to ...
         */
        GroupedQueryBuilder differentFrom(Boolean bool_arg);

        /**
         * Not equal to ...
         */
        GroupedQueryBuilder differentFrom(Object obj);

        /**
         * Not equal to ...
         */
        GroupedQueryBuilder ne(Column column);

        /**
         * Not equal to ...
         */
        GroupedQueryBuilder ne(NDate date);

        /**
         * Not equal to ...
         */
        GroupedQueryBuilder ne(NNumber number);

        /**
         * Not equal to ...
         */
        GroupedQueryBuilder ne(String string_arg);

        /**
         * Not equal to ...
         */
        GroupedQueryBuilder ne(Boolean bool_arg);

        /**
         * Not equal to ...
         */
        GroupedQueryBuilder ne(Object obj);

        /**
         * Greater than ...
         *
         * @deprecated use {@link #greaterThan(Column)} instead
         */
        // @Deprecated
        GroupedQueryBuilder higherThan(Column column);

        /**
         * Greater than ...
         */
        GroupedQueryBuilder greaterThan(Column column);

        /**
         * Greater than ...
         */
        GroupedQueryBuilder gt(Column column);

        /**
         * Greater than ...
         */
        GroupedQueryBuilder greaterThan(Object obj);

        /**
         * Greater than ...
         */
        GroupedQueryBuilder gt(Object obj);

        /**
         * Greater than ...
         *
         * @deprecated use {@link #greaterThan(Date)} instead
         */
        // @Deprecated
        GroupedQueryBuilder higherThan(NDate date);

        /**
         * Greater than ...
         */
        GroupedQueryBuilder greaterThan(NDate date);

        /**
         * Greater than ...
         */
        GroupedQueryBuilder gt(NDate date);

        /**
         * @deprecated use {@link #greaterThan(Number)} instead
         */
        // @Deprecated
        GroupedQueryBuilder higherThan(NNumber number);

        /**
         * Greater than ...
         */
        GroupedQueryBuilder greaterThan(NNumber number);

        /**
         * Greater than ...
         */
        GroupedQueryBuilder gt(NNumber number);

        /**
         * Greater than ...
         *
         * @deprecated use {@link #greaterThan(String)} instead
         */
        //@Deprecated
        GroupedQueryBuilder higherThan(String string_arg);

        /**
         * Greater than ...
         */
        GroupedQueryBuilder greaterThan(String string_arg);

        /**
         * Greater than ...
         */
        GroupedQueryBuilder gt(String string_arg);

        /**
         * Less than ...
         */
        GroupedQueryBuilder lessThan(Column column);

        /**
         * Less than ...
         */
        GroupedQueryBuilder lt(Column column);

        /**
         * Less than ...
         */
        GroupedQueryBuilder lessThan(NDate date);

        /**
         * Less than ...
         */
        GroupedQueryBuilder lessThan(NNumber number);

        /**
         * Less than ...
         */
        GroupedQueryBuilder lessThan(String string_arg);

        /**
         * Less than ...
         */
        GroupedQueryBuilder lessThan(Object obj);

        /**
         * Less than ...
         */
        GroupedQueryBuilder lt(Object obj);

        /**
         * Less than ...
         */
        GroupedQueryBuilder lt(NDate date);

        /**
         * Less than ...
         */
        GroupedQueryBuilder lt(NNumber number);

        /**
         * Less than ...
         */
        GroupedQueryBuilder lt(String string_arg);

        /**
         * Greater than or equals...
         */
        GroupedQueryBuilder greaterThanOrEquals(Column column);

        /**
         * Greater than or equals...
         */
        GroupedQueryBuilder gte(Column column);

        /**
         * Greater than or equals...
         */
        GroupedQueryBuilder greaterThanOrEquals(NDate date);

        /**
         * Greater than or equals...
         */
        GroupedQueryBuilder gte(NDate date);

        /**
         * Greater than or equals...
         */
        GroupedQueryBuilder greaterThanOrEquals(NNumber number);

        /**
         * Greater than or equals...
         */
        GroupedQueryBuilder gte(NNumber number);

        /**
         * Greater than or equals...
         */
        GroupedQueryBuilder greaterThanOrEquals(String string_arg);

        /**
         * Greater than or equals...
         */
        GroupedQueryBuilder gte(String string_arg);

        /**
         * Greater than or equals...
         */
        GroupedQueryBuilder greaterThanOrEquals(Object obj);

        /**
         * Greater than or equals...
         */
        GroupedQueryBuilder gte(Object obj);

        /**
         * Less than or equals...
         */
        GroupedQueryBuilder lessThanOrEquals(Column column);

        /**
         * Less than or equals...
         */
        GroupedQueryBuilder lte(Column column);

        /**
         * Less than or equals...
         */
        GroupedQueryBuilder lessThanOrEquals(NDate date);

        /**
         * Less than or equals...
         */
        GroupedQueryBuilder lte(NDate date);

        /**
         * Less than or equals...
         */
        GroupedQueryBuilder lessThanOrEquals(NNumber number);

        /**
         * Less than or equals...
         */
        GroupedQueryBuilder lte(NNumber number);

        /**
         * Less than or equals...
         */
        GroupedQueryBuilder lessThanOrEquals(String string_arg);

        /**
         * Less than or equals...
         */
        GroupedQueryBuilder lte(String string_arg);

        /**
         * Less than or equals...
         */
        GroupedQueryBuilder lessThanOrEquals(Object obj);

        /**
         * Less than or equals...
         */
        GroupedQueryBuilder lte(Object obj);
    } // FilterBuilder interface
}
