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
// https://social.msdn.microsoft.com/Forums/en-US/5bd74f79-4c7b-40c4-9623-082b66240ee6/is-there-a-integer-class-or-some-other-like-this-in-c?forum=csharplanguage
namespace org.apache.metamodel.j2n.data.numbers
{
    public class NInteger : NNumber
    {
            public static readonly NInteger ONE  = new NInteger(1);
            public static readonly NInteger ZERO = new NInteger(0);

            public NInteger(int value_arg) : base(value_arg)
	        { 
	        } // constructor

            public static int ValueOf(NInteger value_arg)
            {
                return value_arg.asInt();
            } // ValueOf()

            public static string ToHexString(NInteger value_arg)
            {
                return NInteger.ToHexString(value_arg);
            } // ToHexString()

            public static implicit operator NInteger(int value_arg)
	        { 
	            return new NInteger(value_arg); 
	        } 
	 
	        public static implicit operator int(NInteger int_value)
	        { 
	            return int_value.asInt(); 
	        } 
	 
	        public static int operator + (NInteger one, NInteger two)
	        { 
	            return one + two;
            } // operator +

            public static NInteger operator + (int one, NInteger two)
	        { 
	            return new NInteger(one + two);
            } // operator +

            public static int operator - (NInteger one, NInteger two)
	        { 
	            return one - two;
            } // operator -

            public static NInteger operator - (int one, NInteger two)
	        { 
	            return new NInteger(one - two);
            } // operator -
    } // NInteger class
} // org.apache.metamodel.j2n.data.numbers namespace
