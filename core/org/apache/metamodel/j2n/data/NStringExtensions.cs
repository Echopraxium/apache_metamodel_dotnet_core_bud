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
// https://stackoverflow.com/questions/4910108/how-to-extend-c-sharp-built-in-types-like-string
namespace org.apache.metamodel.j2n.data
{
    //Extension methods must be defined in a static class
    public static class NStringExtensions
    {
        // This is the extension method.
        // The first parameter takes the "this" modifier
        // and specifies the type for which the method is defined.
        public static bool IsEmpty(this string value)
        {
            return value == null || value == "";
        } // IsEmpty()

        public static int CompareTo_(this string value, string value2)
        {
            return value.CompareTo(value2);
        } // CompareTo()
    } // NStringExtensions class
} // org.apache.metamodel.j2n.data namespace
