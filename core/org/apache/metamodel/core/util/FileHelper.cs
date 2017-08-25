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
// https://github.com/apache/metamodel/blob/master/core/src/main/java/org/apache/metamodel/util/FileHelper.java
using org.apache.metamodel.j2n.io;
using org.apache.metamodel.j2n.slf4j;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
//import java.io.BufferedInputStream;
//import java.io.BufferedOutputStream;
//import java.io.BufferedReader;
//import java.io.BufferedWriter;
//import java.io.ByteArrayOutputStream;
//import java.io.File;
//import java.io.FileInputStream;
//import java.io.FileNotFoundException;
//import java.io.FileOutputStream;
//import java.io.Flushable;
//import java.io.IOException;
//import java.io.InputStream;
//import java.io.InputStreamReader;
//import java.io.OutputStream;
//import java.io.OutputStreamWriter;
//import java.io.PushbackInputStream;
//import java.io.Reader;
//import java.io.Writer;
//import java.lang.reflect.InvocationTargetException;
//import java.lang.reflect.Method;

namespace org.apache.metamodel.util
{
    /**
     * Various helper methods for handling files
     */
    public sealed class FileHelper
    {

        private readonly static NLogger logger = NLoggerFactory.getLogger(typeof(FileHelper).Name);

        public static readonly string UTF_8_ENCODING      = "UTF-8";
        public static readonly string UTF_16_ENCODING     = "UTF-16";
        public static readonly string US_ASCII_ENCODING   = "US-ASCII";
        public static readonly string ISO_8859_1_ENCODING = "ISO_8859_1";
        public static readonly string DEFAULT_ENCODING    = UTF_8_ENCODING;

        private FileHelper()
        {
            // prevent instantiation
        }

        public static FileStream createTempFile(string prefix, string suffix)
        {
            try
            {
                return File.Open(prefix + '.' + suffix, FileMode.Create);  //File.createTempFile(prefix, suffix);
            }
            catch (IOException e)
            {
                logger.error("Could not create tempFile", e);
                string tempDir = getTempDir();
                return File.Open(tempDir + "\\" + prefix + '.' + suffix, FileMode.Create);
            }
        } // createTempFile()

        public static string getTempDir()
        {
            string result = "";
            FileStream fs = null;
            // String tmpDirPath = System.getProperty("java.io.tmpdir");
            string tmp_file_path = Path.GetTempPath();
            if (tmp_file_path != null && !"".Equals(tmp_file_path))
            {
                fs = File.Open(tmp_file_path, FileMode.Create);
            }
            else
            {
                logger.debug("Could not determine tmpdir by using environment variable.");
                try
                {
                    // https://www.tutorialspoint.com/java/io/file_createtempfile_directory.htm
                    //File.createTempFile("foo", "bar");
                    tmp_file_path = ".foo.bar";
                    fs = File.Open(tmp_file_path, FileMode.Create);
                    DirectoryInfo di = Directory.GetParent(tmp_file_path);
                    string tmp_file_full_path = di.FullName + "\\" + tmp_file_path;
                    result = tmp_file_full_path;
                    try
                    {
                        File.Delete(tmp_file_full_path);
                    }
                    catch (Exception e)
                    {
                        logger.warn("Could not delete temp file '{}'", tmp_file_full_path);
                    }                  
                }
                catch (IOException e)
                {
                    logger.error("Could not create tempFile in order to find temporary dir", e);
                    try
                    {
                        DirectoryInfo di = Directory.CreateDirectory("metamodel.tmp.dir");
                    }
                    catch (Exception e2)
                    {
                        throw new InvalidOperationException("Could not create directory for temporary files: "
                                                            + e2.Message);
                    }

                    //result.deleteOnExit();
                    fs.Dispose();      
                }
            }
            if (logger.isInfoEnabled())
            {
                logger.info("Using '{0}' as tmpdir.", result);
            }
            return result;
        } // getTempDir()

        public static StreamWriter getWriter(NOutputStream file, string encoding, string append) // throws IllegalStateException
        {
            bool append_to_bool = append.Equals("true", StringComparison.CurrentCultureIgnoreCase);
            bool insertBom      = ! append_to_bool;
            return getWriter(file, encoding, append_to_bool, insertBom);
        } // getWriter()

        public static StreamWriter getWriter(NOutputStream outputStream, string encoding) // throws IllegalStateException
        {
            return getWriter(outputStream, encoding, false);
        } // getWriter()

        public static StreamWriter getWriter(NOutputStream output_stream, string encoding_arg, bool insertBom) // throws IllegalStateException
        {
            //if (!((Stream)output_stream is BufferedStream))
            //{
            //    output_stream = new BufferedStream(output_stream);
            //}

            try
            {
                if (insertBom)
                {
                    StreamWriter writer = null; // new UnicodeWriter(output_stream, encoding_arg);
                    return writer;
                }
                else
                {
                    Encoding encoding = null;
                    switch (encoding_arg.ToLower())
                    {
                        case "ascii":
                            encoding = Encoding.ASCII;
                            break;

                        case "utf-8":
                            encoding = Encoding.UTF8;
                            break;

                        case "utf-32":
                            encoding = Encoding.UTF32;
                            break;

                        default:
                            break;
                    }

                    StreamWriter writer = null;
                    if (encoding != null)
                        writer = new StreamWriter(output_stream, encoding);
                    else
                        throw new IOException("Encoding " + encoding_arg + " not available");
                    return writer;
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        } // getWriter()

        public static StreamWriter getWriter(NOutputStream stream_arg, string encoding, bool append, bool insertBom) // throws IllegalStateException
        {
            if (append && insertBom)
            {
                throw new ArgumentException("Can not insert BOM into appending writer");
            }
            NOutputStream output_stream = getOutputStream(stream_arg, append);
            return getWriter(output_stream, encoding, insertBom.ToString());
        } // getWriter()

        //public static StreamWriter getWriter(NOutputStream file, String encoding) // throws IllegalStateException
        //{
        //    return getWriter(file, encoding, false);
        //} // getWriter()

        public static StreamReader getReader(NInputStream input_stream, String encoding_arg) // throws IllegalStateException
        {
            Encoding encoding_obj = null;
            try
            {
                if (encoding_arg == null || encoding_arg.ToLower().IndexOf("utf") != -1)
                {
                    byte[] bom = new byte[4];
                    int unread;

                    // auto-detect byte-order-mark
                    //@SuppressWarnings("resource")
                    NInputStream pushbackInputStream = new NInputStream(input_stream.SafeFileHandle, FileAccess.Read, bom.Length);
                    int n = pushbackInputStream.Read(bom, 0, bom.Length);

                    // Read ahead four bytes and check for BOM marks.
                    if ((bom[0] == (byte)0xEF) && (bom[1] == (byte)0xBB) && (bom[2] == (byte)0xBF))
                    {
                        encoding_arg = "UTF-8";
                        unread = n - 3;
                        encoding_obj = Encoding.UTF8;
                    }
                    else if ((bom[0] == (byte)0xFE) && (bom[1] == (byte)0xFF))
                    {
                        encoding_arg = "UTF-16BE";
                        unread = n - 2;
                    }
                    else if ((bom[0] == (byte)0xFF) && (bom[1] == (byte)0xFE))
                    {
                        encoding_arg = "UTF-16LE";
                        unread = n - 2;
                    }
                    else if ((bom[0] == (byte)0x00) && (bom[1] == (byte)0x00) && (bom[2] == (byte)0xFE)
                          && (bom[3] == (byte)0xFF))
                    {
                        encoding_arg = "UTF-32BE";
                        encoding_obj = Encoding.UTF32;
                        unread = n - 4;
                    }
                    else if ((bom[0] == (byte)0xFF) && (bom[1] == (byte)0xFE) && (bom[2] == (byte)0x00)
                          && (bom[3] == (byte)0x00))
                    {
                        encoding_obj = Encoding.UTF32;
                        encoding_arg = "UTF-32LE";
                        unread = n - 4;
                    }
                    else
                    {
                        unread = n;
                    }

                    if (unread > 0)
                    {
                        //pushbackInputStream.unread(bom, (n - unread), unread);
                    }
                    else if (unread < -1)
                    {
                        //pushbackInputStream.unread(bom, 0, 0);
                    }

                    input_stream = pushbackInputStream;
                }

                StreamReader inputStreamReader;
                if (encoding_arg == null)
                {
                    inputStreamReader = new StreamReader(input_stream);
                }
                else
                {
                    if (encoding_obj != null)
                        inputStreamReader = new StreamReader(input_stream, encoding_obj);
                    else
                        throw new IOException("getReader(): Encoding not available");
                }
                return inputStreamReader;
            }
            catch (IOException e)
            {
                throw new InvalidOperationException(e.Message);
            }
        } // getReader()

        //public static StreamReader getReader(NInputStream file, String encoding) // throws IllegalStateException
        //{
        //    NInputStream input_stream;
        //    try
        //    {
        //        input_stream = new BufferedInputStream(new FileInputStream(file));
        //    }
        //    catch (IOException e)
        //    {
        //        throw new InvalidOperationException(e.Message);
        //    }
        //    return getReader(input_stream, encoding);
        //} // getReader()

        public static String readInputStreamAsString(NInputStream in_stream, String encoding) // throws IllegalStateException
        {
            StreamReader reader = getReader(in_stream, encoding);
            return readAsString(reader);
        }

        public static String readFileAsString(NInputStream in_stream, String encoding) // throws IllegalStateException
        {
            StreamReader br = getReader(in_stream, encoding);
            return readAsString(br);
        } // readFileAsString()

        public static String readAsString(StreamReader reader) // throws IllegalStateException
        {
            StreamReader br = getBufferedReader(reader);
            try
            {
                StringBuilder sb = new StringBuilder();
                bool firstLine = true;
                for (String line = br.ReadLine(); line != null; line = br.ReadLine())
                {
                    if (firstLine)
                    {
                        firstLine = false;
                    }
                    else
                    {
                        sb.Append('\n');
                    }
                    sb.Append(line);
                }
                return sb.ToString();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
            finally
            {
                safeClose(br, reader);
            }
        } // readAsString()

        public static void safeClose(params Object[] objects)
        {
            bool debug_enabled = logger.isDebugEnabled();

            if (objects == null || objects.Length == 0)
            {
                logger.info("safeClose(...) was invoked with null or empty array: {}", objects);
                return;
            }

            foreach (Object obj in objects)
            {
                if (obj != null)
                {
                    if (debug_enabled)
                    {
                        logger.debug("Trying to safely close {}", obj);
                    }

                    if (obj is NFlushable)
                    {
                        try
                        {
                            ((NFlushable)obj).flush();
                        }
                        catch (Exception e)
                        {
                            if (debug_enabled)
                            {
                                logger.debug("Flushing Flushable failed", e);
                            }
                        }
                }

                if (obj is IDisposable)
                {
                    try
                    {
                        ((IDisposable)obj).Dispose();
                    }
                    catch (Exception e)
                    {
                        if (debug_enabled)
                        {
                            logger.debug("Closing AutoCloseable failed", e);
                        }
                    }
                }
                else
                {
                    logger.info("obj was not AutoCloseable, trying to find close() method via reflection.");

                    try
                    {
                        MethodInfo method = obj.GetType().GetMethod("close");
                        if (method == null)
                        {
                            logger.info("obj did not have a close() method, ignoring");
                        }
                        else
                        {
                            //method.setAccessible(true);
                            method.Invoke(obj, null);
                        }
                    }
                    catch (TargetInvocationException e)
                    {
                        logger.warn("Invoking close() by reflection threw exception", e);
                    }
                    catch (Exception e)
                    {
                        logger.warn("Could not invoke close() by reflection", e);
                    }
                }
            } // foreach (Object obj in objects)

            }
        } // safeClose()

        public static StreamWriter getBufferedWriter(NOutputStream stream_arg, String encoding) // throws IllegalStateException
        {
            StreamWriter writer = getWriter(stream_arg, encoding);
            return writer;
        } // getBufferedWriter()

        public static StreamReader getBufferedReader(NInputStream inputStream, String encoding) // throws IllegalStateException
        {
            StreamReader reader = getReader(inputStream, encoding);
            return reader;
        } // getBufferedReader()

        public static StreamReader getReader(NInputStream stream_arg) // throws IllegalStateException
        {
            return getReader(stream_arg, DEFAULT_ENCODING);
        } // getReader()

        public static String readFileAsString(NInputStream stream_arg) // throws IllegalStateException
        {
            return readFileAsString(stream_arg, DEFAULT_ENCODING);
        }

        public static StreamWriter getBufferedWriter(NOutputStream stream_arg) // throws IllegalStateException
        {
            return getBufferedWriter(stream_arg, DEFAULT_ENCODING);
        }

        public static StreamWriter getWriter(NOutputStream stream_arg) // throws IllegalStateException
        {
            return getWriter(stream_arg, DEFAULT_ENCODING);
        } // getWriter()

        public static void writeString(NOutputStream out_stream, String string_arg) // throws IllegalStateException
        {
            writeString(out_stream, string_arg, DEFAULT_ENCODING);
        } // writeString()

        public static void writeString(NOutputStream out_stream, String string_arg, String encoding) // throws IllegalStateException
        {
            StreamWriter writer = getWriter(out_stream, encoding);
            writeString(writer, string_arg, encoding);
        } // writeString()

        public static void writeString(StreamWriter writer, String string_arg) // throws IllegalStateException
        {
            writeString(writer, string_arg, DEFAULT_ENCODING);
        } // writeString()

        public static void writeString(StreamWriter writer, String string_arg, String encoding) // throws IllegalStateException
        {
            try 
            {
                writer.Write(string_arg);
            } 
            catch (Exception e) 
            {
                throw new InvalidOperationException(e.Message);
            } 
            finally 
            {
                safeClose(writer);
            }
        } // writeString()

        public static void writeStringAsFile(NOutputStream out_stream, String string_arg) // throws IllegalStateException
        {
            writeStringAsFile(out_stream, string_arg, DEFAULT_ENCODING);
        } // writeStringAsFile()

        public static void writeStringAsFile(NOutputStream stream_arg, String string_arg, String encoding) // throws IllegalStateException
        {
            StreamWriter bw = getBufferedWriter(stream_arg, encoding);
            writeString(bw, string_arg, encoding);
        } // writeStringAsFile()

        public static StreamReader getBufferedReader(NInputStream stream_arg) // throws IllegalStateException
        {
            return getBufferedReader(stream_arg, DEFAULT_ENCODING);
        } // getBufferedReader()

        public static void copy(StreamReader reader, StreamWriter writer) // throws IllegalStateException
        {
            StreamReader bufferedReader = getBufferedReader(reader);
             try
             {
                bool firstLine = true;
                for (String line = bufferedReader.ReadLine(); line != null; line = bufferedReader.ReadLine())
                {
                    if (firstLine)
                    {
                        firstLine = false;
                    }
                    else
                    {
                        writer.Write('\n');
                    }
                    writer.Write(line);
                }
            }
            catch (IOException e)
            {
                throw new InvalidOperationException(e.Message);
            }
        } // copy()

        public static StreamReader getBufferedReader(StreamReader reader)
        {
            if (reader is StreamReader)
            {
                return (StreamReader)reader;
            }
            return reader;
        } // getBufferedReader

        public static void copy(NInputStream from_stream, NOutputStream to_stream) // throws IllegalStateException
        {
            try
            {
                byte[] buffer = new byte[1024 * 32];
                int offset = 0;
                for (int value = from_stream.Read(buffer, offset++, 1); value != -1; value = from_stream.Read(buffer, offset++, 1))
                {
                    to_stream.Write(buffer, 0, value);
                }
            }
            catch (IOException e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }

        public static void copy(Resource from_res, Resource to_res) // throws IllegalStateException
        {
            Debug.Assert(from_res.isExists());

            NInputStream in_stream = from_res.read();
            try
            {
                NOutputStream out_stream = to_res.write();
                try
                {
                    copy(in_stream, out_stream);
                }
                finally
                {
                    safeClose(out_stream);
                }
            } 
            finally 
            {
                safeClose(in_stream);
            }
        } // copy()

        public static NOutputStream getOutputStream(NOutputStream stream_arg) // throws IllegalStateException
        {
            return getOutputStream(stream_arg, false);
        } // getOutputStream()

        public static NOutputStream getOutputStream(NOutputStream stream_arg, bool append)
        {
            try
            {
                return new NOutputStream(stream_arg.SafeFileHandle, FileAccess.Write);
            }
            catch (FileNotFoundException e)
            {
                throw new InvalidOperationException(e.Message);
            }
        } // getOutputStream()

        public static NInputStream getInputStream(NInputStream stream_arg) // throws IllegalStateException
        {
            //try
            //{
            //    return new BufferedInputStream(new FileInputStream(stream_arg));
            //}
            //catch (FileNotFoundException e)
            //{
            //    throw new InvalidOperationException(e.Message);
            //}
            //return new NInputStream(stream_arg.SafeFileHandle, FileAccess.Read);
            return stream_arg;
        } // getInputStream()

        public static byte[] readAsBytes(NInputStream input_stream)
        {
            //ByteArrayOutputStream baos = new ByteArrayOutputStream();
            MemoryStream memory_stream = new MemoryStream();
            StreamWriter baos   = new StreamWriter(memory_stream);
            StreamReader reader = new StreamReader(input_stream);
            try
            {
                copy(reader, baos);
            }
            finally
            {
                safeClose(input_stream);
            }
            string output_str = baos.ToString();
            byte[] result = new byte[output_str.Length];
            for (int i = 0; i < output_str.Length; i++)
            {
                result[i] = (byte)output_str[i];
            }
            return result;
        } // readAsBytes()
    } // FileHelper class
} // org.apache.metamodel.util namespace
