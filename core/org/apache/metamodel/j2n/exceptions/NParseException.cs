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
// https://books.google.fr/books?id=IoevNnXjOVkC&pg=PA268&lpg=PA268&dq=c%23+ParseException&source=bl&ots=oqGoHVp0SY&sig=iKq99scjX9xFuyae9V7Di3mrlw0&hl=fr&sa=X&ved=0ahUKEwiJ5MbDzvfUAhXLKlAKHbWEBvEQ6AEITDAF#v=onepage&q=c%23%20ParseException&f=false
using System;

namespace org.apache.metamodel.j2n.exceptions
{
    public class NParseException : NSystemException
    {
        public NParseException()
        {
        } // constructor

        public NParseException(string msg) : base(msg)
        {
        } // constructor

        public NParseException(string msg, Exception inner) : base(msg, inner)
        {
        } // constructor
    } // NParseException class
} // org.apache.metamodel.j2n.exceptions namespace
