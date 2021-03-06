﻿
// https://docs.oracle.com/javase/7/docs/api/java/util/concurrent/atomic/AtomicInteger.html
using System.Threading;
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
namespace org.apache.metamodel.j2n.data.numbers
{
    public class NAtomicInteger : NInteger
    {
        public NAtomicInteger(int value_arg) : base(value_arg)
	    {
        } // constructor

        public int getAndIncrement()
        {
           return Interlocked.Increment(ref this.Value.Int);
        } // getAndIncrement()

        public int incrementAndGet()
        {
            Interlocked.Increment(ref this.Value.Int);
            return this.Value.Int;
        } // incrementAndGet()
    } // NAtomicInteger class
} // org.apache.metamodel.j2n.data.numbers namespace
