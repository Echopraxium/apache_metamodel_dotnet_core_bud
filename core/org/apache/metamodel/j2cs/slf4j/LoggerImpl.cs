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
using System.Diagnostics;

namespace org.apache.metamodel.j2cs.slf4j
{
    public class LoggerImpl : Logger
    {
        private bool _is_debug_enabled = true;

        public void debug(string msg)
        {
            Debug.WriteLine(msg);
        } // debug()

        public void debug(string format, object arg, object arg2 = null)
        {
            if (arg2 == null)
                Debug.WriteLine(format, arg);
            else
                Debug.WriteLine(format, arg, arg2);
        } // debug()

        public void warn(string format, object arg)
        {
            Debug.WriteLine(format, arg);
        }

        public void error(string format, object arg, object arg2 = null)
        {
            if (arg2 == null)
              Debug.WriteLine(format, arg);
            else
              Debug.WriteLine(format, arg, arg2);
        }

        public bool isDebugEnabled()
        {
            return _is_debug_enabled;
        } // isDebugEnabled()

        public void trace(string msg)
        {
            Debug.WriteLine(msg);
        } // trace()
    } // LoggerImpl class
} // org.apache.metamodel.j2cs.slf4j namespace
