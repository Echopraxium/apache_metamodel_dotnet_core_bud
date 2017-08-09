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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/factory/SimpleResourceProperties.java
using System;
using System.Collections.Generic;
using System.Text;

namespace org.apache.metamodel.core.factory
{
    public class SimpleResourceProperties : ResourceProperties
    {
        private static readonly long serialVersionUID = 1L;
        private Uri uri;

        public SimpleResourceProperties(Uri uri)
        {
            this.uri = uri;
        }

        public SimpleResourceProperties(String uri)
        {
            Uri uri_output;
            Uri.TryCreate(uri, UriKind.RelativeOrAbsolute, out uri_output);
            this.uri = uri_output;
        }

        // @Override
        public Uri getUri()
        {
            return uri;
        }

        // @Override
        public Dictionary<String, Object> toMap()
        {
            Dictionary<String, Object> map = new Dictionary<String, Object> ();
            map.Add("uri", uri);
            return map;
        }

        // @Override
        public String getUsername()
        {
            return null;
        }

        // @Override
        public String getPassword()
        {
            return null;
        }
    }
}
