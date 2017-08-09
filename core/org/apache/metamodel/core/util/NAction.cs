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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/util/Action.java

namespace org.apache.metamodel.util
{
    /**
     * Represents an abstract action, which is an executable piece of functionality
     * that takes an argument. An {@link Action} has no return type, unlike a
     * {@link Func}.
     * 
     * @param <E>
     *            the argument type of the action
     */
    public interface NAction<E>
    {
        void run(E arg); // throws Exception;
    } // NAction class
} // org.apache.metamodel.util namespace
