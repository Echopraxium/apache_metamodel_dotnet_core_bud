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
using org.apache.metamodel.j2cs.types;
using System;
using System.Collections.Generic;

namespace org.apache.metamodel.util
{
    /**
     * Uses the toString method for comparison of objects
     */
    public sealed class ToStringComparator : IComparer<object>
    {
        private static IComparer<object> _instance = new ToStringComparator();

        public static IComparer<object> getComparator()
        {
            return _instance;
        } // getComparator()

        private ToStringComparator()
        {
        } // ToStringComparator()

        public int Compare(object o1, object o2)
        {
            if (o1 == null && o2 == null)
            {
                return -1;
            }
            if (o1 == null)
            {
                return -1;
            }
            if (o2 == null)
            {
                return 1;
            }
            return o1.ToString().CompareTo(o2.ToString());
        } // Compare()

        //[J2Cs] Implementation helper class which replaces the anonymous interface implementation class in Java
        private class _String_Comparer_Impl_ : IComparable<object>
        {
            private String              _s_;
            private IComparer<object>   _instance_;

            public _String_Comparer_Impl_(String s_arg, IComparer<object> instance_arg)
            {
                _s_        = s_arg;
                _instance_ = instance_arg;
            } // constructor

            public bool equals(Object obj)
            {
                return _instance.Equals(obj);
            }

            public String toString()
            {
                return "ToStringComparable[string=" + _s_ + "]";
            }

            public int CompareTo(object other)
            {
                return _instance.Compare(_s_, other);
            }
        } // _String_Comparer_Impl_

        public static IComparable<object> getComparable(object o)
        {
            String s = o.ToString();
            return new _String_Comparer_Impl_(s, _instance);
        } // getComparable()
    } // ToStringComparator class
} // org.apache.metamodel.util namespace
