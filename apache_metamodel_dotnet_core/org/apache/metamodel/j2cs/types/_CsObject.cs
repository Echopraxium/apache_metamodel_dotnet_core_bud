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
using System.Runtime.InteropServices;

namespace org.apache.metamodel.j2cs.types
{
    // https://stackoverflow.com/questions/4994277/memory-address-of-an-object-in-c-sharp
    public class _CsObject : Object
    {
        private object _value = null;

        public _CsObject()
        {
        } // constructor

        public _CsObject(string string_value)
        {
            _value = string_value;
        } // constructor

        public virtual int hashCode()
        {
            return this.GetHashCode();
        } // hashCode()

        public virtual _CsObject clone()
        {
            return this.clone();
        } // clone()

        //public static int GetHashCode(object o)
        //{
        //    int hash_code = (int)DateTime.Now.Ticks;
        //    GCHandle handle = GCHandle.Alloc(o, GCHandleType.Pinned);
        //    try
        //    {
        //        IntPtr pointer = GCHandle.ToIntPtr(handle);
        //        hash_code = unchecked((int)pointer);
        //    }
        //    finally
        //    {
        //        handle.Free();
        //    }
        //    return hash_code;
        //} // GetHashCode()

        public static implicit operator _CsObject(string value_arg)  // implicit digit to object conversion operator
        {
            return new _CsObject(value_arg);
        } // implicit cast operator string > CsObject
    } // _object class
} // org.apache.metamodel.j2cs.types namespace
