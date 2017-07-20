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
using org.apache.metamodel.j2n.data;
using org.apache.metamodel.j2n.data.numbers;
using org.apache.metamodel.j2n.slf4j;
using org.apache.metamodel.j2n.types;
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

        private static readonly NLogger logger = NLoggerFactory.getLogger(typeof(NumberComparator).Name);

        private static readonly IComparer<object> _instance = new NumberComparator();

        public static IComparer<object> getComparator()
        {
            return _instance;
        } // getComparator()

        private NumberComparator()
        {
        }

        //[J2N] Implementation helper class which replaces the anonymous interface implementation class in Java
        private class _Number_Comparable_Impl_ : IComparable<object>
        {
            private NNumber _value_;
            private IComparer<object> _instance_;

            public _Number_Comparable_Impl_(IComparer<object> instance_arg, NNumber value_arg)
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
            NNumber n = toNumber(o);
            return new _Number_Comparable_Impl_(_instance, n);
        } // getComparable()


        public int Compare(object o1, object o2)
        {
            NNumber n1 = toNumber(o1);
            NNumber n2 = toNumber(o2);

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

            if (n1 is NNumber && n2 is NNumber)
            {
                return (n1.asBigInteger()).CompareTo(n2.asBigInteger());
            }

            //if (n1 is BigDecimal && n2 is BigDecimal) {
            //    return ((BigDecimal)n1).compareTo((BigDecimal)n2);
            //}

            if (NumberComparator.IsIntegerType(n1) && NumberComparator.IsIntegerType(n2))
            {
                return NInteger.ValueOf(n1).CompareTo(n2.asLong());
            }

            return NNumber.ValueOf(n1).CompareTo(n2.asLong());
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
        public static bool IsIntegerType(NNumber n)
        {
            return n is NInteger || n is NAtomicInteger || n is NAtomicLong;
        } // isIntegerType()

        public static NNumber toNumber(object value)
        {
            String string_value;

            if (value == null)
            {
                return null;
            }
            else if (value is NNumber)
            {
                return (NNumber)value;
            }
            else if (value is NBool)
            {
                if (bool.TrueString.Equals(value.ToString()))
                {
                    return NInteger.ONE;
                }
                else
                {
                    return NInteger.ZERO;
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
                    return new NNumber(NNumber.ParseInt(string_value));
                }
                catch (FormatException e)
                {
                }
                try
                {
                    return NNumber.ParseLong(string_value);
                }
                catch (FormatException e)
                {
                }
                try
                {
                    return new NNumber(NNumber.ParseDouble(string_value));
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
                    return NInteger.ONE;
                }
                if ("false".Equals(string_value, StringComparison.CurrentCultureIgnoreCase))
                {
                    return NInteger.ZERO;
                }
            }
            logger.warn("Could not convert '{}' to number, returning null", value);
            return null;
        } // toNumber()
    } // NumberComparator class
} // org.apache.metamodel.util naspace

