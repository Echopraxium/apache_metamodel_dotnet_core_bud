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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/convert/DocumentConverter.java
using org.apache.metamodel.data;
using amm_data = org.apache.metamodel.core.data;

namespace org.apache.metamodel.core.convert
{
    /**
     * Object responsible for converting a document ( {@link Map}) into a
     * {@link Row} for a {@link DataSet} that is based on a {@link SchemaBuilder}.
     */
    public interface DocumentConverter
    {
        /**
         * Converts a {@link Document} into a row with the given
         * {@link DataSetHeader}.
         * 
         * @param document
         * @param header
         * @return
         */
        Row convert(amm_data.Document document, DataSetHeader header);
    } // DocumentConverter interface
} // org.apache.metamodel.core.convert
