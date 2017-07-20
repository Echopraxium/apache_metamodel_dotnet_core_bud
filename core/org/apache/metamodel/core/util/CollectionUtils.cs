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
// https://github.com/apache/metamodel/blob/master/core/src/main/java/org/apache/metamodel/util/CollectionUtils.java
using org.apache.metamodel.j2n.collections;
using System.Collections.Generic;

namespace org.apache.metamodel.util
{
    public class CollectionUtils
    {
        private CollectionUtils()
        {
            // prevent instantiation
        }

        public static List<E> filter <E, SuperE> (E[] items, Predicate<SuperE> predicate) where E: SuperE
        {
            return filter(NArrays.asList(items), predicate);
        } // filter()

        public static List<E> filter<E, SuperE>(IEnumerable<E> items, Predicate<SuperE> predicate) where E : SuperE
        {
            List<E> result = new List<E>();
            foreach (E e in items)
            {
                // if (predicate.eval(e).booleanValue())
                if (predicate.eval(e))
                {
                    result.Add(e);
                }
            }
            return result;
        } // filter()

        public static List<O> map <I, SuperI, O> (I[] items, Func<SuperI, O> func) where I : SuperI
        {
            return map(NArrays.asList(items), func);
        } // map()

        public static List<O> map <I, SuperI, O> (IEnumerable<I> items, Func<SuperI, O> func) where I : SuperI
        {
            List<O> result = new List<O>();
            foreach (I item in items)
            {
                O output = func.eval((SuperI) item);
                result.Add(output);
            }
            return result;
        } // map()
    } // CollectionUtils class
} // org.apache.metamodel.util namespace
