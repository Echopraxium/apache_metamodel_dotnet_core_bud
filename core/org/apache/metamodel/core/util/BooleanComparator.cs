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
// https://github.com/apache/metamodel/blob/master/core/src/main/java/org/apache/metamodel/util/BooleanComparator.java
using org.apache.metamodel.j2n.data.numbers;
using org.apache.metamodel.j2n.slf4j;
using System;
using System.Collections.Generic;

namespace org.apache.metamodel.util
{
    /**
     * Comparator of booleans
     */
    public sealed class BooleanComparator : IComparer<object>
    {
        private static readonly NLogger logger = NLoggerFactory.getLogger(typeof(BooleanComparator).Name);

	    private static BooleanComparator _instance = new BooleanComparator();

        private BooleanComparator()
        {
        }

        public static IComparer<Object> getComparator()
        {
            return _instance;
        }

        public static bool? toBoolean(Object o)
        {
            if (o == null)
            {
                return null;
            }

            if (o is Boolean)
            {
                return (Boolean)o;
            }
            if (o is String)
            {
                try
                {
                    return parseBoolean((String)o);
                }
                catch (ArgumentException e)
                {
                    logger.warn("Could not convert String '{0}' to boolean, returning false", o);
                    return false;
                }
            }
            if (o is NNumber)
            {
                int i = ((NNumber)o).asInt();
                return i >= 1;
            }

            logger.warn("Could not convert '{0}' to boolean, returning false", o);
            return false;
        } // toBoolean()

        private class Boolean_Comparable_Impl_ : IComparable<object>
        {
            object _o_;
            public Boolean_Comparable_Impl_(object object_arg)
            {
                _o_ = object_arg;
            } // constructor

            public bool equals(Object obj)
            {
                return _instance.Equals(obj);
            } // equals();

            public int CompareTo(Object o)
            {
               return _instance.Compare(_o_, o);
            }

            public String toString()
            {
               return "BooleanComparable[boolean=" + _o_ + "]";
            }
        } // Boolean_Comparable_Impl_ class

        public static IComparable<object> getComparable(Object object_arg)
        {
            return new Boolean_Comparable_Impl_(object_arg);
        } // getComparable()

        //public static IComparable<Object> getComparable(Object object)
        //{
        //    bool b = toBoolean(object);
        //    return new Comparable<Object>()
        //    {
        //        public boolean equals(Object obj)
        //        {
        //            return _instance.equals(obj);
        //        }

        //        public int compareTo(Object o)
        //        {
        //            return _instance.compare(b, o);
        //        }

        //        public String toString()
        //        {
        //            return "BooleanComparable[boolean=" + b + "]";
        //        }
        //    };
        //} // getComparable()


        public int Compare(Object o1, Object o2)
        {
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

            bool? b1 = toBoolean(o1);
            bool? b2 = toBoolean(o2);

            // https://stackoverflow.com/questions/2341660/how-to-compare-nullable-types 
            // https://www.tutorialspoint.com/java/lang/boolean_compareto.htm
            if (b1 == null && b2 == null)
            {
                return 0;
            }
            if (b1 == null)
            {
                return -1;
            }
            if (b2 == null)
            {
                return 1;
            }
            if ((b1.Value) && (b2.Value))
            {
                return 0;
            }
            if ((b1.Value) && !(b2.Value))
            {
                return 1;
            }
            if (!(b1.Value) && (b2.Value))
            {
                return -1;
            }
            return -1;
            // b1.HasValue && b1.Value != b2.Value;
            //return b1.CompareTo(b2);
        } // Compare()

        /**
         * Parses a string and returns a boolean representation of it. To parse the
         * string the following values will be accepted, irrespective of case.
         * <ul>
         * <li>true</li>
         * <li>false</li>
         * <li>1</li>
         * <li>0</li>
         * <li>yes</li>
         * <li>no</li>
         * <li>y</li>
         * <li>n</li>
         * </ul>
         * 
         * @param string
         *            the string to parse
         * @return a boolean
         * @throws IllegalArgumentException
         *             if the string provided is null or cannot be parsed as a
         *             boolean
         */
        public static bool parseBoolean(String string_arg) // throws IllegalArgumentException
        {
		         if (string_arg == null)
                 {
                    throw new ArgumentException("string cannot be null");
                 }

                 string_arg = string_arg.Trim();
                 if (    "true".Equals(string_arg, StringComparison.CurrentCultureIgnoreCase) || "1".Equals(string_arg)
				      || "y".Equals(string_arg, StringComparison.CurrentCultureIgnoreCase)
                      || "yes".Equals(string_arg, StringComparison.CurrentCultureIgnoreCase)) {
                 return true;

                }
                else if (    "false".Equals(string_arg, StringComparison.CurrentCultureIgnoreCase) || "0".Equals(string_arg)
				          || "n".Equals(string_arg, StringComparison.CurrentCultureIgnoreCase)
                          || "no".Equals(string_arg, StringComparison.CurrentCultureIgnoreCase) 
                        )
                {
                    return false;
                }
                else
                {
                    throw new ArgumentException("Could not get boolean value of string: " + string_arg);
                }
        } // parseBoolean()

        public static bool isBoolean(Object o)
        {
            if (o is bool)
            {
                return true;
            }
            if (o is String)
            {
                if (     "true".Equals ((string)o, StringComparison.CurrentCultureIgnoreCase)
                      || "false".Equals((string)o, StringComparison.CurrentCultureIgnoreCase)
                    )
                {
                    return true;
                }
            }
            return false;
        } // isBoolean()
    } // BooleanComparator class
} // org.apache.metamodel.util namespace
