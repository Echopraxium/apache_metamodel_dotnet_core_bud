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
// https://github.com/apache/metamodel/blob/master/core/src/main/java/org/apache/metamodel/query/QueryClause.java
using System;
using System.Collections.Generic;

namespace org.apache.metamodel.core.query
{
    public interface QueryClause<E> // : Serializable
    {
        QueryClause<E> setItems(/*@SuppressWarnings("unchecked")*/ params E[] items);
        QueryClause<E> addItems(/*@SuppressWarnings("unchecked")*/ params E[] items);
        QueryClause<E> addItems(IEnumerable<E> items);
        QueryClause<E> addItem(int index, E item);
        QueryClause<E> addItem(E item);
        bool isEmpty();
        int getItemCount();
        E getItem(int index);
        List<E> getItems();
        QueryClause<E> removeItem(int index);
        QueryClause<E> removeItem(E item);
        QueryClause<E> removeItems();
        String toSql(bool includeSchemaInColumnPaths);
        String toSql();
        int indexOf(E item);
    } // QueryClause interface
} // org.apache.metamodel.core.query namespace
