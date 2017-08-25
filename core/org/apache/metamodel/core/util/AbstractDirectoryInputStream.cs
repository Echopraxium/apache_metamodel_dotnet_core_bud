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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/util/AbstractDirectoryInputStream.java
using org.apache.metamodel.j2n.io;
using org.apache.metamodel.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace org.apache.metamodel.core.util
{
    public abstract class AbstractDirectoryInputStream<T> : NInputStream
    {
        protected T[]        _files;
        private int          _currentFileIndex = -1;
        private NInputStream _currentInputStream;

        public AbstractDirectoryInputStream(string path, FileMode mode) : base(path, mode)
        {
        } // constructor

        // @Override
        public int read(byte[] b, int off, int len) // throws IOException
        {
            if (_currentInputStream != null)
            {
                int byteCount = ((NFile)_currentInputStream).read(b, off, len);
                if (byteCount > 0)
                {
                    return byteCount;
                }
            }

            if (! openNextFile()) {
                return -1; // No more files.
            }

            return read(b, off, len);
        } // read()

        // @Override
        public int read(byte[] b) //throws IOException
        {
            return read(b, 0, b.Length);
        } // read()

        // @Override
        public int read() /// throws IOException
        {
            byte[] b     = new byte[1];
            int    count = read(b, 0, 1);
            if (count< 0)
            {
                return -1;
            }
            return (int) b[0];
        } // read()

        // @Override
        public bool available() // throws IOException
        {
            if (_currentInputStream != null) 
            {
                return NFile.Available(_currentInputStream);
            }
            else
            {
                return false;
            }
        } // available() 

        private bool openNextFile() // throws IOException
        {
            if (_currentInputStream != null)
            {
                FileHelper.safeClose(_currentInputStream);
                _currentInputStream = null;
            }
            _currentFileIndex++;
            if (_currentFileIndex >= _files.Length)
            {
                return false;
            }

            _currentInputStream = openStream(_currentFileIndex);
            return true;
        } // openNextFile()

        public abstract NInputStream openStream(int index); //throws IOException;

        // @Override
        public bool markSupported()
        {
            return false;
        }

        // @Override
        public void close() // throws IOException
        {
            if (_currentInputStream != null) 
            {
                FileHelper.safeClose(_currentInputStream);
            }
        } //  close()
    } // AbstractDirectoryInputStream class
} // org.apache.metamodel.core.util
