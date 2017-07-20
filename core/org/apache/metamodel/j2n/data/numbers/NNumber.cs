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
    public class NNumber : object
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


    // https://msdn.microsoft.com/en-us/library/cc953fe1.aspx
    // bool, char, unsigned char, signed char, __int8	         1 byte
    // __int16, short, unsigned short, wchar_t, __wchar_t	     2 bytes
    // float, __int32, int, unsigned int, long, unsigned long	 4 bytes
    // double, __int64, long double, long long	                 8 bytes
    [StructLayout(LayoutKind.Explicit)]
    public struct NNumberValue
    {
        /// <summary>The union's value as a short variable</summary>
        [FieldOffset(0)] public bool         Bool;

        /// <summary>The union's value as a short variable</summary>
        [FieldOffset(1)]  public short       Short;

        /// <summary>The union's value as a int variable</summary>
        [FieldOffset(3)]  public int         Int;

        /// <summary>The union's value as a long</summary>
        [FieldOffset(7)]  public long        Long;

        /// <summary>The union's value as an unsigned long</summary>
        [FieldOffset(11)] public ulong       ULong;

        /// <summary>The union's value as a double precision floating point variable</summary>
        [FieldOffset(15)]  public double     Double;

        /// <summary>The union's value as a BigInteger</summary>
        [FieldOffset(19)] public NInteger   BigInt;
    } // NNumberValue struct
} // org.apache.metamodel.j2n.data.numbers namespace
