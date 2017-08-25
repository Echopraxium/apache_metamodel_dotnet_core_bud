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
using System;
using System.Collections.Generic;

namespace org.apache.metamodel.j2n.collections
{
    public static class NArrays
    {
        public static List<T> AsList<T>(params T[] a)
        {
            return new List<T>(a);
        } // asList()

        public static string AsString<T>(IEnumerable<T> list)
        {
            return "[" + string.Join(",", list) + "]";
        } // AsString()

        public static string ArrayAsString<T>(T[] items)
        {
            if (items == null || items.Length <= 0)
                return "[]";

            T[] sorted_items = new T[items.Length]; 
            Array.Copy(items, sorted_items, items.Length);
            Array.Sort(sorted_items);
            return "[" + string.Join(", ", sorted_items) + "]";
        } // ArrayAsString()

        // https://www.tutorialspoint.com/java/util/arrays_copyof_int.htm
        public static string[] CopyOf(string[] source, int length)
        {
            string[] output = new string[length];
            for (int i = 0; i < length; i++)
            {
                if (i > source.Length - 1)
                    output[i] = "";
                else
                    output[i] = source[i];
            }
            return output;
        } // CopyOf()

} // NArrays class
} // org.apache.metamodel.j2n.collections namespace
