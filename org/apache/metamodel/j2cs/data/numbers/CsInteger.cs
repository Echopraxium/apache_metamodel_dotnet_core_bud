﻿/**
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
// https://social.msdn.microsoft.com/Forums/en-US/5bd74f79-4c7b-40c4-9623-082b66240ee6/is-there-a-integer-class-or-some-other-like-this-in-c?forum=csharplanguage
namespace org.apache.metamodel.j2cs.data.numbers
{
    public class CsInteger : CsNumber
    {
            public static readonly CsInteger ONE  = new CsInteger(1);
            public static readonly CsInteger ZERO = new CsInteger(0);

            public CsInteger(int value_arg) : base(value_arg)
	        { 
	        } // constructor

            public static int ValueOf(CsInteger value_arg)
            {
                return value_arg.asInt();
            } // ValueOf()

            public static string ToHexString(CsInteger value_arg)
            {
                return CsInteger.ToHexString(value_arg);
            } // ToHexString()

            public static implicit operator CsInteger(int value_arg)
	        { 
	            return new CsInteger(value_arg); 
	        } 
	 
	        public static implicit operator int(CsInteger int_value)
	        { 
	            return int_value.asInt(); 
	        } 
	 
	        public static int operator + (CsInteger one, CsInteger two)
	        { 
	            return one + two;
            } // operator +

            public static CsInteger operator + (int one, CsInteger two)
	        { 
	            return new CsInteger(one + two);
            } // operator +

            public static int operator - (CsInteger one, CsInteger two)
	        { 
	            return one - two;
            } // operator -

            public static CsInteger operator - (int one, CsInteger two)
	        { 
	            return new CsInteger(one - two);
            } // operator -
    } // CsInteger class
} // org.apache.metamodel.j2cs.data.numbers namespace
