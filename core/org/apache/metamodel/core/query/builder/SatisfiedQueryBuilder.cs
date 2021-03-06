﻿
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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/query/builder/SatisfiedQueryBuilder.java
using org.apache.metamodel.core.data;
using org.apache.metamodel.core.query;
using org.apache.metamodel.core.query.builder;
using org.apache.metamodel.schema;
using System.Collections.Generic;

namespace org.apache.metamodel.query.builder
{
    /**
     * Represents a built query that is satisfied and ready for querying or further
     * building.
     * 
     * @param <B>
     */
    public interface SatisfiedQueryBuilder
    {

        //ColumnSelectBuilder<B> select(Column column);

        SatisfiedSelectBuilder select(params Column[] columns);

        /**
         * Sets the offset (number of rows to skip) of the query that is being
         * built.
         * 
         * Note that this number is a 0-based variant of invoking
         * {@link #firstRow(int)}.
         * 
         * @param offset
         *            the number of rows to skip
         * @return
         */
        SatisfiedQueryBuilder offset(int offset);

        /**
         * Sets the first row of the query that is being built.
         * 
         * Note that this is a 1-based variant of invoking {@link #limit(int)}.
         * 
         * @param firstRow
         * @return
         */
        SatisfiedQueryBuilder firstRow(int firstRow);

        /**
         * Sets the limit (aka. max rows) of the query that is being built.
         * 
         * @param limit
         * @return
         */
        SatisfiedQueryBuilder limit(int limit);

        /**
         * Sets the max rows (aka. limit) of the query that is being built.
         * 
         * @param maxRows
         * @return
         */
        SatisfiedQueryBuilder maxRows(int maxRows);

        FunctionSelectBuilder select(FunctionType function, Column column);

        SatisfiedQueryBuilder select(FunctionType function, string columnName);

        SatisfiedSelectBuilder selectCount();

        ColumnSelectBuilder select(string columnName);

        WhereBuilder where(Column column);

        WhereBuilder where(string columnName);

        WhereBuilder where(ScalarFunction function, Column column);

        WhereBuilder where(ScalarFunction function, string columnName);

        SatisfiedQueryBuilder where(params FilterItem[] filters);

        SatisfiedQueryBuilder where(IEnumerable<FilterItem> filters);

        SatisfiedOrderByBuilder orderBy(string columnName);

        SatisfiedOrderByBuilder orderBy(Column column);

        GroupedQueryBuilder groupBy(string columnName);

        GroupedQueryBuilder groupBy(params string[] columnNames);

        GroupedQueryBuilder groupBy(Column column);

        GroupedQueryBuilder groupBy(params Column[] columns);

        /**
         * Gets the built query as a {@link Query} object. Typically the returned
         * query will be a clone of the built query to prevent conflicting
         * mutations.
         * 
         * @return a {@link Query} object representing the built query.
         */
        Query toQuery();

        CompiledQuery compile();

        /**
         * Executes the built query. This call is similar to calling
         * {@link #toQuery()} and then {@link DataContext#executeQuery(Query)}.
         * 
         * @return the {@link DataSet} that is returned by executing the query.
         */
        DataSet execute();

        /**
         * Finds a column by name within the already defined FROM items
         * 
         * @param columnName
         * @return
         * @throws IllegalArgumentException
         */
        Column findColumn(string columnName); // throws IllegalArgumentException;
    } // SatisfiedQueryBuilder interface
} //  org.apache.metamodel.query.builder namespace
