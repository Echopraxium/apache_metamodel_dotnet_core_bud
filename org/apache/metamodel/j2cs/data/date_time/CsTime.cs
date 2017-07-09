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
// https://stackoverflow.com/questions/2037283/how-do-i-represent-a-time-only-value-in-net
using System;

namespace org.apache.metamodel.j2cs.data.date_time
{
    public class CsTime
    {
        public int Hours   { get; private set; }
        public int Minutes { get; private set; }
        public int Seconds { get; private set; }

        public CsTime(uint h, uint m, uint s)
        {
            if (h > 23 || m > 59 || s > 59)
            {
                throw new ArgumentException("Invalid time specified");
            }
            Hours = (int)h; Minutes = (int)m; Seconds = (int)s;
        } // constructor

        public CsTime(DateTime dt)
        {
            Hours = dt.Hour;
            Minutes = dt.Minute;
            Seconds = dt.Second;
        } // constructor

        public override string ToString()
        {
            return String.Format(
                "{0:00}:{1:00}:{2:00}",
                this.Hours, this.Minutes, this.Seconds);
        } // ToString()
    } // CsTime class
} // org.apache.metamodel.j2cs.data.date_time
