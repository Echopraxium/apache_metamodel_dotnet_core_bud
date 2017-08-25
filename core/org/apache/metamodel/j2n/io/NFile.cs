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
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using org.apache.metamodel.j2n.data;
using org.apache.metamodel.j2n.collections;
using Microsoft.Win32.SafeHandles;
using System.Diagnostics;

namespace org.apache.metamodel.j2n.io
{
    public class NFile : FileStream
    {
        protected   string          _path      = "";
        protected   string          _base_name = "";
        protected   FileInfo        _fi        = null;
        protected   FileMode        _mode      = default(FileMode);
        protected   FileAttributes  _attr      = default(FileAttributes);

        public NFile(string path, FileMode mode) : base(path, mode)
        {
            _path = path;
            if (!_path.IsEmpty())
            {
                try
                {
                    _attr = File.GetAttributes(_path);
                    _fi   = new FileInfo(_path);

                    string[] items = _path.Split('\\');
                    _base_name = items[items.Length-1];
                    Debug.WriteLine("new NFile() _base_name: '" + _base_name + "'");
                }
                catch (Exception e)
                {
                    throw new IOException("new NFile path: " + path + " mode:" + mode);
                }
            }
        } // constructor

        public NFile(SafeFileHandle fh, FileAccess access): base(fh, access)
        {
        } // constructor

        public NFile(SafeFileHandle fh, FileAccess access, int length) : base(fh, access, length)
        {
        } // constructor

        public string   Path => _path;
        public FileMode Mode => _mode;

        
        public string getBaseName()
        {
            return _base_name;
        } // getBaseName()

        public int read(byte[] b, int off, int len)
        {
            return base.Read(b, off, len);
        } // read()

        public static bool Available(NFile file)
        {
            FileStream stream = null;
            try
            {
                stream = file._fi.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    file.Dispose();
            }

            //file is not locked
            return false;
        } // Available()

        public bool exists()
        {
            if (_path.IsEmpty() || _attr == null)
                return false;
            else
                return File.Exists(_path);
        } // isDirectory()

        // https://stackoverflow.com/questions/1395205/better-way-to-check-if-a-path-is-a-file-or-a-directory
        public bool isDirectory()
        {
            if (_path.IsEmpty() || _attr == null)
                return false;
            else
            {
                bool output = _attr.HasFlag(FileAttributes.Directory);
                return output;
            }                
        } // isDirectory()

        public NFile[] listFiles()
        {
            NFile[] output = new NFile[0];
            if (isDirectory())
            {
                List<NFile> file_list = new List<NFile>();
                IEnumerable<string> items = Directory.EnumerateFileSystemEntries(_path);
                foreach (string item in items)
                {
                    file_list.Add(new NFile(item, FileMode.Open));
                }
                output = file_list.ToArray();
            }
            return output;
        } // listFiles()

        public string getPath()
        {
            return _path;
        } // getPath()

        public string getAbsolutePath()
        {
            return _path;
        } // getAbsolutePath()

        public string getCanonicalPath()
        {
            return _path;
        } // getCanonicalPath()

        public bool isFile()
        {
            if (_path.IsEmpty() || _attr == null)
                return false;
            else
                return File.Exists(_path);
        } // isFile()

        // https://stackoverflow.com/questions/20674435/how-to-get-last-modified-date-of-file-while-moving-it-using-c-sharp
        public long lastModified()
        {
            if (_path.IsEmpty() || _attr == default(FileAttributes))
                return 0;

            return _fi.LastAccessTime.Ticks;
        } // lastModified()
    } // NFile class
} // org.apache.metamodel.j2n.io
