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
using System.Linq;

namespace org.apache.metamodel.j2n.collections
{
    public static class NEnumerableUtils
    {
        // List<string> data = List<string>();
        // if (EnumerableUtils.IsEmpty<Row>(data)
        public static Boolean IsEmpty<T>(this IEnumerable<T> source)
        {
            if (source == null)
                return true; // or throw an exception
            return !source.Any();
        } // IsEmpty<T>

        public static List<T> toList<T>(this IEnumerable<T> source)
        {
            return source.toList();
        } // toList<T>

        // https://stackoverflow.com/questions/20923418/collection-containsall-implementation-in-c-sharp
        public static bool ContainsAll<T>(this IEnumerable<T> source, IEnumerable<T> inner)
        {
            return inner.All(source.Contains);
        }

        public static bool Contains<T>(this IEnumerable<T> source, IEnumerable<T> inner, IEqualityComparer<T> comparer)
        {
            return inner.All(element => source.Contains(element, comparer));
        }
    } // NEnumerableUtils
} // org.apache.metamodel.j2n.collections namespace
