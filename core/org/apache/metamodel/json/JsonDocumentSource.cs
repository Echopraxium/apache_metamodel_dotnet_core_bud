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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/json/src/main/java/org/apache/metamodel/json/JsonDocumentSource.java
using Newtonsoft.Json.Linq;
using org.apache.metamodel.core.data;
using org.apache.metamodel.j2n.json;
using org.apache.metamodel.j2n.slf4j;
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
//import com.fasterxml.jackson.core.JsonParser;
//import com.fasterxml.jackson.core.JsonToken;

namespace org.apache.metamodel.json
{
    /**
     * Utility class that provides an easy way of iterating documents in a JSON file
     */
    public sealed class JsonDocumentSource : DocumentSource
    {
        private static readonly NLogger logger = NLoggerFactory.getLogger(typeof(JsonDocumentSource).Name);

        private NJsonParser _parser;
        private String      _sourceCollectionName;

        public JsonDocumentSource(NJsonParser parser, String sourceCollectionName)
        {
            Debug.WriteLine("new JsonDocumentSource()  sourceCollectionName: '" + sourceCollectionName + "'");
            _parser               = parser;
            _sourceCollectionName = sourceCollectionName;

            _parser.parse();
        } // constructor

        public Document next()
        {
            Debug.WriteLine("JsonDocumentSource.next()");
            while (true)
            {
                JToken token = getNextToken();
                if (token == null)
                {
                    return null;
                }

                Debug.WriteLine(  "JsonDocumentSource.next() \n"
                                + "  token           : '" + token + "'\n"
                                + "  token.Root      : '" + token.Root + "'\n"
                                + "  token.Root.First: '" + token.Root.First + "'");

                if (token.Equals(token.Root.First))
                {
                    Dictionary<String, object> value = readValue();
                    Document doc = new Document(_sourceCollectionName, value, value);
                    return doc;
                }
            }
        } //  next()

        private JToken getNextToken()
        {
            Debug.WriteLine("JsonDocumentSource.getNextToken()");
            try
            {
                return _parser.nextToken();
            }
            catch (Exception e)
            {
                throw new MetaModelException(e);
            }
        } // getNextToken()

        // @SuppressWarnings("unchecked")
        private Dictionary<String, object> readValue()
        {
            Debug.WriteLine("JsonDocumentSource.readValue()");
            try
            {
                return _parser.readValueAs<Dictionary<String, object>>();
            } 
            catch (Exception e) 
            {
                throw new MetaModelException(e);
            }
        } // readValue()

        public void close()
        {
            try
            {
                _parser.close();
            }
            catch (IOException e)
            {
                logger.warn("Failed to ");
            }
            //throw new NotImplementedException();
        } // close()
    } // JsonDocumentSource class
} // org.apache.metamodel.json
