
// https://github.com/apache/metamodel/blob/master/core/src/main/java/org/apache/metamodel/util/FileResource.java
//import java.io.File;
//import java.io.FileFilter;
//import java.io.IOException;
//import java.io.InputStream;
//import java.io.OutputStream;
//import java.io.Serializable;
//import java.util.Arrays;

using org.apache.metamodel.core.util;
using org.apache.metamodel.j2n.exceptions;
using org.apache.metamodel.j2n.io;
using org.apache.metamodel.util;
using System;
using System.IO;
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
namespace org.apache.metamodel.util
{
    /**
     * {@link File} based {@link Resource} implementation.
     */
    public class FileResource : AbstractResource // Serializable
    {
        private static readonly long serialVersionUID = 1L;

        private NFile _file;

        private class DirectoryInputStream : AbstractDirectoryInputStream<NFile> 
        {
            public DirectoryInputStream(string path, FileMode mode, FileResource res): base(path, mode)
            {
                NFile[] unsortedFiles = res.getChildren();

                if (unsortedFiles == null)
                {
                    _files = new NFile[0];
                }
                else
                {
                    Array.Sort(unsortedFiles);
                    _files = unsortedFiles;
                }
            }

            // @Override
            public override NInputStream openStream(int index) // throws IOException
            {
               return FileHelper.getInputStream((NInputStream)_files[index]);
            }
        } // DirectoryInputStream class

        public FileResource(String filename)
        {
            _file = new NFile(filename, FileMode.Open);
        }

        public FileResource(NFile file)
        {
            _file = file;
        } // FileResource()


        // @Override
        public string toString()
        {
            return "FileResource[" + _file.getPath() + "]";
        } // toString()

        // @Override        
        public override String getName()
        {
            return _file.getBaseName();
            //return _file.Name;
        } // getName()

        // @Override
        public override String getQualifiedPath()
        {
            try
            {
                return _file.getCanonicalPath();
            }
            catch (IOException e)
            {
                return _file.getAbsolutePath();
            }
        } // getQualifiedPath()

        // @Override
        public bool isReadOnly()
        {
            if (!isExists())
            {
                return false;
            }
            if (_file.isDirectory())
            {
                return true;
            }
            bool canWrite = _file.CanWrite;
            return !canWrite;
        } // isReadOnly()

        // @Override
        public NOutputStream write() // throws ResourceException
        {
            if (_file.isDirectory())
            {
                throw new NResourceException("Cannot write to directory: " + _file);
            }
            return FileHelper.getOutputStream((NOutputStream)_file);
        } // write()

        // @Override
        public NOutputStream append() // throws ResourceException
        {
            return FileHelper.getOutputStream((NOutputStream)_file, true);
        } // append()

        public NFile getFile()
        {
            return _file;
        } // getFile()()

        // @Override
        public override bool isExists()
        {
            return _file.exists();
        } // isExists()

        // @Override
        public long getSize()
        {
            if (_file.isDirectory())
            {
                long size = 0;
                NFile[] children = getChildren();
                foreach (NFile file in children)
                {
                    long length = file.Length;
                    if (length == -1)
                    {
                        return -1;
                    }
                    size += length;
                }
                return size;
            }
            return _file.Length;
        } // getSize()

        // @Override
        public long getLastModified()
        {
            long lastModified = 0;
            if (_file.isDirectory())
            {
                lastModified = -1;
                NFile[] children     = getChildren();
                foreach (NFile file in children)
                {
                    long l = file.lastModified();
                    if (l != 0)
                    {
                        lastModified = Math.Max(lastModified, l);
                    }
                }
                return lastModified;
            } 

            lastModified = _file.lastModified();
            if (lastModified == 0)
            {
                return -1;
            }
            return lastModified;
        } // getLastModified()

        // @Override
        public override NInputStream read() // throws ResourceException
        {
            if (_file.isDirectory())
            {
                return new DirectoryInputStream(_file.Path, _file.Mode, this);
            }
            NInputStream in_stream = FileHelper.getInputStream((NInputStream)_file);
            return in_stream;
        } // read()

        private class _FileFilter_impl_ : NFileFilter
        {          
            public bool accept(NFile file_arg)
            {
                return file_arg.isFile();
            } // accept()
        } // _FileFilter_impl_ class

        private NFile[] getChildren()
        {
            return _file.listFiles(); // new _FileFilter_impl_());
        } // getChildren()
    } // FileResource class
} // org.apache.metamodel.util
