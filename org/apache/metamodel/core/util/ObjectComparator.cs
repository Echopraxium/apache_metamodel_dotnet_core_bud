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
// https://github.com/apache/metamodel/blob/master/core/src/main/java/org/apache/metamodel/util/ObjectComparator.java

using org.apache.metamodel.j2cs.data.numbers;
using org.apache.metamodel.j2cs.slf4j;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace org.apache.metamodel.util
{
    /**
     * General purpose comparator to use for objects of various kinds. Prevents
     * NullPointerExceptions and tries to use comparable interface if available and
     * appropriate on incoming objects.
     */
    public sealed class ObjectComparator : IComparer<Object>
    {
        private static readonly Logger logger = LoggerFactory.getLogger(typeof(ObjectComparator).Name);

        private static readonly IComparer<Object> _instance = new ObjectComparator();

        public static IComparer<Object> getComparator()
        {
            return _instance;
        }

        private ObjectComparator()
        {
        }

        private class _Object_Comparable_Impl_: IComparable<Object>
        {
            private Object _o_;

            public _Object_Comparable_Impl_(Object o_arg)
            {
                _o_ = o_arg;
            }

            public bool equals(Object obj)
            {
                return _instance.Equals(obj);
            }

            public int CompareTo(Object o2)
            {
                return _instance.Compare(_o_, o2);
            }

            public String toString()
            {
                return "ObjectComparable[object=" + _o_ + "]";
            }
        } // _Object_Comparable_Impl_

        public static IComparable<Object> getComparable(Object o)
        {
            return new _Object_Comparable_Impl_(o);
        }

        public int Compare(object x, object y)
        {
            throw new NotImplementedException();
        }

        // @SuppressWarnings("unchecked")
        public int compare(object o1, object o2)
        {
            logger.debug("compare({},{})", o1, o2);
            if (o1 == null && o2 == null)
            {
                return 0;
            }
            if (o1 == null)
            {
                return -1;
            }
            if (o2 == null)
            {
                return 1;
            }
            if (o1 is CsNumber && o1 is CsNumber)
            {
                return NumberComparator.getComparator().Compare(o1, o2);
            }
            if (TimeComparator.isTimeBased(o1) && TimeComparator.isTimeBased(o2))
            {
                return TimeComparator.getComparator().Compare(o1, o2);
            }
            if (BooleanComparator.isBoolean(o1) && BooleanComparator.isBoolean(o2))
            {
                return BooleanComparator.getComparator().Compare(o1, o2);
            }
            if (o1 is IComparable && o2 is IComparable)
            {
                    //@SuppressWarnings("rawtypes")

                    IComparable c1 = (IComparable)o1;
                    //@SuppressWarnings("rawtypes")

                    IComparable c2 = (IComparable)o2;
                // We can only count on using the comparable interface if o1 and o2
                // are within of the same class or if one is a subclass of the other
                if (c1.GetType().IsAssignableFrom(c2.GetType()))
                {
                    return c1.CompareTo(o2);
                }
                if (o2.GetType().IsAssignableFrom(c1.GetType()))
                {
                    return -1 * c2.CompareTo(o1);
                }
            }
            logger.debug("Using ToStringComparator because no apparent better comparison method could be found");
            return ToStringComparator.getComparator().Compare(o1, o2);
        } // compare()
    }
}
