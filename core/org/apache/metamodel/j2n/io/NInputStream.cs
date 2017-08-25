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
using Microsoft.Win32.SafeHandles;
using System.IO;
using org.apache.metamodel.j2n.data;
using System.Diagnostics;

namespace org.apache.metamodel.j2n.io
{
    public class NInputStream : NFile
    {
        #region ---------- Contructors ----------
        public NInputStream(string path, FileMode mode) : base(path, mode)
        {
            Debug.WriteLine("NInputStream(String, FileMode) path='" + path + "'");
            _path = path;
            _mode = FileMode.Open;
        } // constructor

        public NInputStream(SafeFileHandle fh, FileAccess access): base(fh, access)
        {
            Debug.WriteLine("NInputStream(SafeFileHandle, FileAccess)");
            _mode = FileMode.Open;
        } // constructor

        public NInputStream(SafeFileHandle fh, FileAccess access, int length) : base(fh, access, length)
        {
            Debug.WriteLine("NInputStream(SafeFileHandle, FileAccess, int)");
            _mode = FileMode.Open;
        } // constructor
        #endregion Contructors


        public override bool CanRead  => true;

        public override bool CanWrite => false;


        public byte[] readFile()
        {
            Debug.WriteLine("NInputStream.readFile()  _path='" + _path + "'");
            byte[] buffer = new byte[0];

            if (_path.IsEmpty())
                return buffer;

            FileStream fileStream = this; // new FileStream(_path, _mode, FileAccess.Read);
            //FileStream fileStream = new FileStream(_path, _mode, FileAccess.Read);
            try
            {
                int length = (int) fileStream.Length;  // get file length
                buffer     = new byte[length];            // create buffer
                int count;                            // actual number of bytes read
                int sum = 0;                          // total number of bytes read

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    sum += count;  // sum is a buffer offset for next reading
            }
            finally
            {
                //fileStream.close();
                this.Position = 0;
            }
            return buffer;
        } // readFile()
    } // CsInputStream class
} // org.apache.metamodel.j2n.io namespace
