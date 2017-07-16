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

namespace org.apache.metamodel.j2cs.exceptions
{
    public class CsSystemException : Exception
    {
        public CsSystemException()
        {
        } // constructor

        public CsSystemException(string msg) : base(msg)
        {
        } // constructor

        public CsSystemException(string msg, Exception inner) : base(msg, inner)
        {
        } // constructor

        public bool isWrapper()
        {
            return base.InnerException != null;
        } // isWrapper()

        public string getStackTrace()
        {
            if (base.InnerException != null)
            {
                return base.InnerException.StackTrace;
            }
            else
                return this.StackTrace;
        } // isWrapper()
    } // CsSystemException class
} // org.apache.metamodel.j2cs.exceptions namespace
