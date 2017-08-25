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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/query/CompiledQuery.java
using org.apache.metamodel.j2n.io;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.apache.metamodel.core.query
{
    /**
     * A {@link CompiledQuery} is a {@link Query} which has been compiled, typically
     * by the data source itself, to provide optimized execution speed. Compiled
     * queries are produced using the {@link DataContext#compileQuery(Query)} method.
     * 
     * Typically the compilation itself takes a bit of time, but firing the compiled
     * query is faster than regular queries. This means that for repeated executions
     * of the same query, it is usually faster to use compiled queries.
     * 
     * To make {@link CompiledQuery} useful for more than just one specific query,
     * variations of the query can be fired, as long as the variations can be
     * expressed as a {@link QueryParameter} for instance in the WHERE clause of the
     * query.
     * 
     * @see DataContext#compileQuery(Query)
     * @see QueryParameter
     */
    public interface CompiledQuery : NCloseable
    {
        /**
         * Gets the {@link QueryParameter}s associated with the compiled query.
         * Values for these parameters are expected when the query is executed.
         * 
         * @return a list of query parameters
         */
        List<QueryParameter> getParameters();

        /**
         * A representation of the query as SQL.
         * 
         * @return a SQL string.
         */
        String toSql();

        /**
         * Closes any resources related to the compiled query.
         */
        //@Override
         void close();
    } // CompiledQuery interface
}
