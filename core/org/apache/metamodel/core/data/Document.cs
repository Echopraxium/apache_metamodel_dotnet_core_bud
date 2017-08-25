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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/data/Document.java
using System;
using System.Collections.Generic;

namespace org.apache.metamodel.core.data
{
    /**
     * Represents a document, ie. an object to be turned into a {@link Row} using a
     * {@link DocumentConverter} and to be sourced by a {@link DocumentSource}.
     * 
     * A document does not require any schema. A document will hold key/value pairs
     * where keys are always strings, but values may be arbitrary values.
     */
    public class Document
    {
        private Dictionary<String, object> _values;
        private Object                     _sourceObject;
        private String                     _sourceCollectionName;

        public Document(Dictionary<String, object> values, Object sourceObject) : this(null, values, sourceObject)
        {           
        } // constructor

        public Document(String sourceCollectionName, Dictionary<String, object> values, Object sourceObject)
        {
            _sourceCollectionName = sourceCollectionName;
            _values               = values;
            _sourceObject         = sourceObject;
        } // constructor

        /**
         * Gets the values of the document.
         * 
         * @return
         */
        public Dictionary<String, object> getValues()
        {
            return _values;
        }

        /**
         * Gets the source representation of the document, if any.
         * 
         * @return
         */
        public Object getSourceObject()
        {
            return _sourceObject;
        }

        /**
         * Gets the collection/table name as defined in the source, or a hint about
         * a table name of this document. This method may return null if the
         * {@link DocumentSource} does not have any knowledge about the originating
         * collection name, or if there is no logical way to determine such a name.
         * 
         * @return
         */
        public String getSourceCollectionName()
        {
            return _sourceCollectionName;
        }
    } // Document class
} // org.apache.metamodel.core.data
