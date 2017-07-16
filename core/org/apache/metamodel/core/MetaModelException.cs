/**
* Licensed to the Apache Software Foundation (ASF) under one
* or more contributor license agreements. See the NOTICE file
* distributed with this work for additional information
* regarding copyright ownership. The ASF licenses this file
* to you under the Apache License, Version 2.0 (the
* "License"); you may not use this file except in compliance
* with the License. You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing,
* software distributed under the License is distributed on an
* "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
* KIND, either express or implied. See the License for the
* specific language governing permissions and limitations
* under the License.
*/
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/MetaModelException.java
using System;

namespace org.apache.metamodel
{
    /**
    * Unchecked exception used to signal errors occuring in MetaModel.
    *
    * All MetaModelExceptions represent errors discovered withing the MetaModel
    * framework. Typically these will occur if you have put together a query that
    * is not meaningful or if there is a structural problem in a schema.
    */
    //[J2Cs]  RuntimeException <=>  SystemException
    public class MetaModelException : Exception  // RuntimeException
    {
        // Disable warnings for unused or unassigned fields
        #pragma warning disable 0414
        private static readonly long serialVersionUID = 5455738384633428319L;
        #pragma warning restore 0414

        public MetaModelException(string message, Exception cause) : base(message, cause)
        {
        } // constructor

        public MetaModelException(string message) : base(message)
        {
        } // constructor

        public MetaModelException(Exception cause) : base(cause.Message)
        {
        } // constructor

        public MetaModelException() : base()
        {
        } // constructor
    } // MetaModelException class
} // org.apache.metamodel