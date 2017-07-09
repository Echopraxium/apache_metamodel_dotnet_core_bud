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
// http://www.primaryobjects.com/2008/01/29/using-the-iterator-pattern-in-c-asp-net/
using System;
using System.Collections.Generic;

namespace org.apache.metamodel.j2cs.collections
{
    public class CsList<E> : List<E> //, IcsIterator<E>
    {
        private int  _position = 0;

        public void set(object key, E value)
        {
            if (! this.Contains(value))
                this.Add(value);
        }  // set()

        public int size()
        {
            return Count;
        }  // size()

        /*
        public E[] toArray()
        {
            return this.ToArray();
        }  // toArray()
        */

        public E[] toArray()
        {
            E[] destination_array = new E[this.Count];
            Array.Copy(this.ToArray(), destination_array, this.Count);
            return destination_array;
        }  // toArray()

        public E[] toArray(E[] destination_array)
        {
            Array.Copy(this.ToArray(), destination_array, this.Count);
            return destination_array;
        }  // toArray()

        public void add(E value)
        {
            this.Add(value);
        }  // add()

        public bool isEmpty()
        {
            return EnumerableUtils.IsEmpty<E>(this);
        }  // isEmpty()

        public void addAll(IEnumerable<E> source_list)
        {
            this.AddRange(source_list);
        }  // addAll()

        public bool HasNext()
        {
            if (_position < this.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        } // HasNext()

        public E Next()
        {
            E itemContents = (E) this[_position];
            _position++;
            return itemContents;
        } // Next()
    } // CsList class
} // org.apache.metamodel.j2cs.collections namespace
