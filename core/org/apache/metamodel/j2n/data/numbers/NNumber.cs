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
// https://github.com/shouldly/shouldly/blob/master/src/Shouldly/Internals/FloatingPointNumerics.cs
using System.Runtime.InteropServices;

namespace org.apache.metamodel.j2n.data.numbers
{
    public class NNumber
    {
        public NNumberValue Value;

        public NNumber(long value_arg)
        {
            Value.Long = value_arg;
        } // constructor

        public NNumber(double value_arg)
        {
            Value.Double = value_arg;
        } // constructor

        public NNumber(bool value_arg)
        {
            Value.Bool = value_arg;
        } // constructor

        public int asInt()
        {
            return Value.Int;
        } // asInt()

        public long asLong()
        {
            return Value.Long;
        } // asLong()

        public double asDouble()
        {
            return Value.Double;
        } // asDouble()

        public long asBigInteger()
        {
            return Value.BigInt;
        } // asBigInteger()

        public static long ValueOf(NNumber value_arg)
        {
            return value_arg.Value.Long;
        } // ValueOf()

        //public long fromLong(long value)
        //{
        //    return long.Parse(value);
        //} // fromLong()

        public static NNumber ParseLong(string value_arg)
        {
            return new NNumber(long.Parse(value_arg));
        } // ParseLong()

        public static int ParseInt(string value_arg)
        {
            return NNumber.ParseInt(value_arg);
        } // ParseLong()

        public static long ParseDouble(string value_arg)
        {
            return NNumber.ParseDouble(value_arg);
        } // ParseDouble()
    } // NNumber class
} // org.apache.metamodel.j2n.data.numbers namespace
