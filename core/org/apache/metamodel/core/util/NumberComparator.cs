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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/util/NumberComparator.java
using org.apache.metamodel.j2cs.data;
using org.apache.metamodel.j2cs.data.numbers;
using org.apache.metamodel.j2cs.slf4j;
using org.apache.metamodel.j2cs.types;
using System;
using System.Collections.Generic;

namespace org.apache.metamodel.util
{
    /**
     * Comparator that can compare numbers of various kinds (short, integer, float,
     * double etc)
     */
    public sealed class NumberComparator : IComparer<object>
    {

        private static readonly Logger logger = LoggerFactory.getLogger(typeof(NumberComparator).Name);

        private static readonly IComparer<object> _instance = new NumberComparator();

        public static IComparer<object> getComparator()
        {
            return _instance;
        } // getComparator()

        private NumberComparator()
        {
        }

        //[J2Cs] Implementation helper class which replaces the anonymous interface implementation class in Java
        private class _Number_Comparable_Impl_ : IComparable<object>
        {
            private CsNumber _value_;
            private IComparer<object> _instance_;

            public _Number_Comparable_Impl_(IComparer<object> instance_arg, CsNumber value_arg)
            {
                _value_ = value_arg;
                _instance_ = instance_arg;
            } // constructor

            public bool equals(object obj)
            {
                return _instance.Equals(obj);
            }

            public int CompareTo(object o)
            {
                return _instance.Compare(_value_, o);
            }
            public String toString()
            {
                return "NumberComparable[number=" + _value_ + "]";
            }
        } // _Number_Comparable_Impl_


        public static IComparable<object> getComparable(object o)
        {
            CsNumber n = toNumber(o);
            return new _Number_Comparable_Impl_(_instance, n);
        } // getComparable()


        public int Compare(object o1, object o2)
        {
            CsNumber n1 = toNumber(o1);
            CsNumber n2 = toNumber(o2);

            if (n1 == null && n2 == null)
            {
                return 0;
            }
            if (n1 == null)
            {
                return -1;
            }
            if (n2 == null)
            {
                return 1;
            }

            if (n1 is CsNumber && n2 is CsNumber)
            {
                return (n1.asBigInteger()).CompareTo(n2.asBigInteger());
            }

            //if (n1 is BigDecimal && n2 is BigDecimal) {
            //    return ((BigDecimal)n1).compareTo((BigDecimal)n2);
            //}

            if (NumberComparator.IsIntegerType(n1) && NumberComparator.IsIntegerType(n2))
            {
                return CsInteger.ValueOf(n1).CompareTo(n2.asLong());
            }

            return CsNumber.ValueOf(n1).CompareTo(n2.asLong());
        } // Compare()

        /**
         * Determines if a particular number is an integer-type number such as
         * {@link Byte}, {@link Short}, {@link Integer}, {@link Long},
         * {@link AtomicInteger} or {@link AtomicLong}.
         * 
         * Note that {@link BigInteger} is not included in this set of number
         * classes since treatment of {@link BigInteger} requires different logic.
         * 
         * @param n
         * @return
         */
        public static bool IsIntegerType(CsNumber n)
        {
            return n is CsInteger || n is CsAtomicInteger || n is CsAtomicLong;
        } // isIntegerType()

        public static CsNumber toNumber(object value)
        {
            String string_value;

            if (value == null)
            {
                return null;
            }
            else if (value is CsNumber)
            {
                return (CsNumber)value;
            }
            else if (value is CsBool)
            {
                if (bool.TrueString.Equals(value.ToString()))
                {
                    return CsInteger.ONE;
                }
                else
                {
                    return CsInteger.ZERO;
                }
            }
            else
            {
                string_value = value.ToString().Trim();
                if (string_value.IsEmpty())
                {
                    return null;
                }

                try
                {
                    return new CsNumber(CsNumber.ParseInt(string_value));
                }
                catch (FormatException e)
                {
                }
                try
                {
                    return CsNumber.ParseLong(string_value);
                }
                catch (FormatException e)
                {
                }
                try
                {
                    return new CsNumber(CsNumber.ParseDouble(string_value));
                }
                catch (FormatException e)
                {
                }
            }

            //[J2Cs: Weird syntax block referring a variable from the previous if/else block]
            // note: Boolean.parseBoolean does not throw NumberFormatException -
            // it just returns false in case of invalid values.
            {
                if ("true".Equals(string_value, StringComparison.CurrentCultureIgnoreCase))
                {
                    return CsInteger.ONE;
                }
                if ("false".Equals(string_value, StringComparison.CurrentCultureIgnoreCase))
                {
                    return CsInteger.ZERO;
                }
            }
            logger.warn("Could not convert '{}' to number, returning null", value);
            return null;
        } // toNumber()
    } // NumberComparator class
} // org.apache.metamodel.util naspace

