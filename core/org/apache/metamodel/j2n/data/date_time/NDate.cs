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
// https://stackoverflow.com/questions/798121/date-vs-datetime
using System;

namespace org.apache.metamodel.j2n.data.date_time
{
    public class NDate : IEquatable<NDate>, IEquatable<DateTime>
    {
        private DateTime _value;

        public NDate(DateTime date)
        {
            _value = date.Date;
        } // constructor

        public NDate(long value) : this(DateTime.Now)
        {
            _value = new DateTime(1900, 1, 1).AddMilliseconds(value);
        } // constructor

        public NDate(string value, string format, IFormatProvider dtfi)
        {
            _value = DateTime.ParseExact(value, format, dtfi);
        } // constructor

        public static int CompareTo(NDate d1, NDate d2)
        {
            if (d1 == null && d2 == null)
            {
                return -1;
            }
            if (d1 == null)
            {
                return -1;
            }
            if (d2 == null)
            {
                return 1;
            }

            if (d1._value == d2._value)
                return 0;
            else if (d1._value > d2._value)
                return -1;
            else if (d1._value < d2._value)
                return 1;

            return -1;
        } // CompareTo()

        public bool Equals(NDate other)
        {
            return other != null && _value.Equals(other._value);
        }

        public bool Equals(DateTime other)
        {
            return _value.Equals(other);
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        public static implicit operator DateTime(NDate date)
        {
            return date._value;
        }

        public static explicit operator NDate(DateTime dateTime)
        {
            return new NDate(dateTime);
        }
    } // NDate class
} // org.apache.metamodel.j2n.data.date_time namespace
