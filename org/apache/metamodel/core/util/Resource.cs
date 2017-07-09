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
// https://github.com/apache/metamodel/blob/master/core/src/main/java/org/apache/metamodel/util/Resource.java
using org.apache.metamodel.util;
using System;
using J2CsMM = org.apache.metamodel;

namespace org.apache.metamodel.util
{
    public interface Resource<E> : HasName
    {
        /**
         * Gets the name of the resource, typically a filename or other identifying
         * string
         */
        // public string getName();

        /**
         * Gets the qualified path of the resource, which typically includes slash
         * or backslash separated nodes in a hierarical tree structure.
         *
         * @return
         */
        string getQualifiedPath();

        /**
         * Determines if the file is read only, or if writes are also possible.
         * 
         * @return
         */
        bool isReadOnly();

        /**
         * Determines if the resource referenced by this object exists or not.
         * 
         * @return
         */
        bool isExists();

        /**
         * Gets the size (in number of bytes) of this resource's data. An
         * approximated number is allowed.
         * 
         * If the size is not determinable without actually reading through the
         * whole contents of the resource, -1 is returned.
         * 
         * @return
         */
        long getSize();

        /**
         * Gets the last modified timestamp value (measured in milliseconds since
         * the epoch (00:00:00 GMT, January 1, 1970)) of the resource, if available.
         * If the last modified date is not available, -1 is returned.
         * 
         * @return
         */
        long getLastModified();

        /**
         * Opens up an {@link OutputStream} to write to the resource, and allows a
         * callback to perform writing actions on it.
         * 
         * @param writeCallback
         *            a callback which should define what to write to the resource.
         * 
         * @throws ResourceException
         *             if an error occurs while writing
         */
        void write(Action<System.IO.Stream> writeCallback); // throws ResourceException;

        /**
         * Opens up an {@link OutputStream} to write to the resource. Consumers of
         * this method are expected to invoke the {@link OutputStream#close()}
         * method manually.
         * 
         * If possible, the other write(...) method is preferred over this one,
         * since it guarantees proper closing of the resource's handles.
         * 
         * @return
         * @throws ResourceException
         */
        System.IO.Stream write(); //throws ResourceException;

        /**
         * Opens up an {@link InputStream} to append (write at the end of the
         * existing stream) to the resource.
         * 
         * @param appendCallback
         *            a callback which should define what to append to the resource.
         * @throws ResourceException
         *             if an error occurs while appending
         */
        void append(Action<System.IO.Stream> appendCallback); // throws ResourceException;

        /**
         * Opens up an {@link OutputStream} to append to the resource. Consumers of
         * this method are expected to invoke the {@link OutputStream#close()}
         * method manually.
         * 
         * If possible, the other append(...) method is preferred over this one,
         * since it guarantees proper closing of the resource's handles.
         * 
         * @return
         * @throws ResourceException
         */
        System.IO.Stream append(); // throws ResourceException;

        /**
         * Opens up an {@link InputStream} to read from the resource. Consumers of
         * this method are expected to invoke the {@link InputStream#close()} method
         * manually.
         * 
         * If possible, the other read(...) methods are preferred over this one,
         * since they guarantee proper closing of the resource's handles.
         * 
         * @return
         * @throws ResourceException
         */
        System.IO.Stream read(); // throws ResourceException;

        /**
         * Opens up an {@link InputStream} to read from the resource, and allows a
         * callback to perform writing actions on it.
         * 
         * @param readCallback
         * 
         * @throws ResourceException
         *             if an error occurs while reading
         */
        void read(Action<System.IO.Stream> readCallback); // throws ResourceException;

        /**
         * Opens up an {@link InputStream} to read from the resource, and allows a
         * callback function to perform writing actions on it and return the
         * function's result.
         * 
         * @param readCallback
         * @return the result of the function
         * 
         * @throws ResourceException
         *             if an error occurs while reading
         */
         E read(J2CsMM.util.Func<System.IO.Stream, E> readCallback); // throws ResourceException;
    } // Resource interface
} // org.apache.metamodel.util namespace
