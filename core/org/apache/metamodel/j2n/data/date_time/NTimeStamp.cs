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
// https://stackoverflow.com/questions/17632584/how-to-get-the-unix-timestamp-in-c-sharp
using System;

namespace org.apache.metamodel.j2n.data.date_time
{
    public class NTimeStamp
    {
        public static Int32 UnixTimeStampUTC()
        {
            Int32    unixTimeStamp;
            DateTime currentTime = DateTime.Now;
            DateTime zuluTime    = currentTime.ToUniversalTime();
            DateTime unixEpoch    = new DateTime(1970, 1, 1);
            unixTimeStamp = (Int32)(zuluTime.Subtract(unixEpoch)).TotalSeconds;
            return unixTimeStamp;
        } // UnixTimeStampUTC()
    } // NTimeStamp class
} // org.apache.metamodel.j2n.data.date_time namespace
