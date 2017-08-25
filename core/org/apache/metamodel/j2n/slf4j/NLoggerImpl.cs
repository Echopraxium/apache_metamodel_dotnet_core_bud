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
using System;
using System.Diagnostics;

namespace org.apache.metamodel.j2n.slf4j
{
    public class NLoggerImpl : NLogger
    {
        private bool _is_debug_enabled = true;
        private bool _is_info_enabled  = true;

        public void debug(string msg)
        {
            Debug.WriteLine(msg);
        } // debug()

        public void debug(string format, params object[] args)
        {
            string output = String.Format(format, args);                
            Debug.WriteLine(output);
        } // debug()

        public void warn(string msg)
        {
            Debug.WriteLine(msg);
        } // warn()

        public void warn(string format, params object[] args)
        {
            debug(format, args);
        } // warn()

        public void error(string format, params object[] args)
        {
            debug(format, args);
        } // error()

        public void info(string format, params object[] args)
        {
            debug(format, args);
        } // info()

        public bool isDebugEnabled()
        {
            return _is_debug_enabled;
        } // isDebugEnabled()

        public void trace(string msg)
        {
            Debug.WriteLine(msg);
        } // trace()

        public bool isInfoEnabled()
        {
            return _is_info_enabled;
        } // isInfoEnabled()
    } // NLoggerImpl class
} // org.apache.metamodel.j2n.slf4j namespace
