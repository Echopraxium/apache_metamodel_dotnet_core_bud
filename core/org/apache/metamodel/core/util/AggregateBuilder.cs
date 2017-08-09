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
// https://github.com/apache/metamodel/blob/master/core/src/main/java/org/apache/metamodel/util/AggregateBuilder.java
namespace org.apache.metamodel.util
{
    /**
     * Interface for aggregate builders which allows for an iterative approach to
     * evaluating aggregates.
     * 
     * @param <E>
     *            the aggregate result type
     */
    public interface AggregateBuilder<E>
    {
        void add(object o);

        E getAggregate();
    } // AggregateBuilder class
} // org.apache.metamodel.util namespace
