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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/query/parser/QueryPartCollectionProcessor.java
using System;
using System.Collections.Generic;

namespace org.apache.metamodel.core.query.parser
{
    /**
     * Simple implementation of {@link QueryPartProcessor} which simply adds all
     * elements to a collection. Use {@link #getTokens()} to retrieve the 'processed'
     * tokens and {@link #getDelims()} for the corresponding delimitors.
     */
    public class QueryPartCollectionProcessor : QueryPartProcessor
    {
        private List<String> _delims;
        private List<String> _tokens;

        public QueryPartCollectionProcessor()
        {
            _tokens = new List<String>();
            _delims = new List<String>();
        } // constructor

        // @Override
        public void parse(String delim, String token)
        {
            _delims.Add(delim);
            _tokens.Add(token);
        }

        public List<String> getDelims()
        {
            return _delims;
        }

        public List<String> getTokens()
        {
            return _tokens;
        }
    } // QueryPartCollectionProcessor class
} // org.apache.metamodel.core.query.parse
