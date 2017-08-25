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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/query/builder/WhereBuilder.java
using org.apache.metamodel.query.builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.apache.metamodel.core.query.builder
{
    /**
     * Builder interface for WHERE items.
     * 
     * In addition to the {@link FilterBuilder}, the WHERE builder allows using
     * {@link QueryParameter}s as operands in the generated filters.
     * 
     * @param <B>
     */
    public interface WhereBuilder : FilterBuilder
    {
        /**
         * Equals to a query parameter. Can be used with {@link CompiledQuery}
         * objects.
         */
        SatisfiedWhereBuilder eq(QueryParameter queryParameter);

        /**
         * Equals to a query parameter. Can be used with {@link CompiledQuery}
         * objects.
         */
        SatisfiedWhereBuilder isEquals(QueryParameter queryParameter);

        /**
         * Not equals to a query parameter. Can be used with {@link CompiledQuery}
         * objects.
         */
        SatisfiedWhereBuilder differentFrom(QueryParameter queryParameter);

        /**
         * Not equals to a query parameter. Can be used with {@link CompiledQuery}
         * objects.
         */
        SatisfiedWhereBuilder ne(QueryParameter queryParameter);

        /**
         * Greater than a query parameter. Can be used with {@link CompiledQuery}
         * objects.
         */
        SatisfiedWhereBuilder greaterThan(QueryParameter queryParameter);

        /**
         * Greater than a query parameter. Can be used with {@link CompiledQuery}
         * objects.
         */
        SatisfiedWhereBuilder gt(QueryParameter queryParameter);

        /**
         * Less than a query parameter. Can be used with {@link CompiledQuery}
         * objects.
         */
        SatisfiedWhereBuilder lessThan(QueryParameter queryParameter);

        /**
         * Less than a query parameter. Can be used with {@link CompiledQuery}
         * objects.
         */
        SatisfiedWhereBuilder lt(QueryParameter queryParameter);

        /**
         * Like a query parameter. Can be used with {@link CompiledQuery} objects.
         */
        SatisfiedWhereBuilder like(QueryParameter queryParameter);
    }
}
