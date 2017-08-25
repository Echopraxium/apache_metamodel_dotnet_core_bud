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
using Newtonsoft.Json.Linq;
using org.apache.metamodel.j2n.io;
using System;
using System.Diagnostics;

namespace org.apache.metamodel.j2n.json
{
    // http://tutorials.jenkov.com/java-json/jackson-jsonparser.html#creating-a-json-parser
    public class NJsonParser : NCloseable
    {
        JToken  _root            = null;
        JToken  _current_token   = null;
        private string _data_str = "";

        public NJsonParser(NInputStream input_stream)
        {
            Debug.WriteLine("new NJsonParser()");

            byte[] data = input_stream.readFile();
            _data_str = "";
            foreach(byte b in data)
            {
                _data_str += (char)b;
            }
            //parse(_data_str);
        } // constructor     

        public void parse()
        {
            Debug.WriteLine("NJsonParser.parse() data = '" + _data_str + "'");

            // https://stackoverflow.com/questions/34690581/error-reading-jobject-from-jsonreader-current-jsonreader-item-is-not-an-object
            try
            {
                ///_root = JObject.Parse(_data_str);
                JArray jarray  = JArray.Parse(_data_str);
                _root          = (JToken) jarray;
                _current_token = _root[0];
            }
            catch (Exception e)
            {
                throw new MetaModelException("NJsonParser.parse() \n" + e.Message);
            }
        } // parse()

        public JToken nextToken()
        {
            Debug.WriteLine("NJsonParser.nextToken() _current_token = '" + _current_token + "'");
            JToken result  = _current_token;
            if (_current_token != null)
              _current_token = _current_token.Next;
            return result;
        } // nextToken()

        public E readValueAs<E>()
        {
            return _current_token.ToObject<E>();
        } // readValueAs()

        public void close()
        {
        }
    } // NJsonParser class
} // org.apache.metamodel.j2n.json
