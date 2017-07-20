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
// https://github.com/apache/metamodel/blob/master/core/src/main/java/org/apache/metamodel/util/AbstractResource.java
using org.apache.metamodel.j2n.exceptions;
using org.apache.metamodel.j2n.io;
using System;
using System.IO;
//import java.io.InputStream;
//import java.io.OutputStream;

namespace org.apache.metamodel.util
{
    /**
     * Abstract implementation of many methods in {@link Resource}
     */
    public abstract class AbstractResource : Resource
    {
        public void read(CsAction<NInputStream> readCallback)
        {
            NInputStream in_stream = read();
            try
            {
                readCallback.run(in_stream);
            }
            catch (Exception e)
            {
                throw new NResourceException("Error occurred in read callback", e);
            }
            finally
            {
                FileHelper.safeClose(in_stream);
            }
        } // read()

        public E read<E>(Func<NInputStream, E> readCallback)
        {
            NInputStream in_stream = read();
            try
            {
                E result = readCallback.eval(in_stream);
                return result;
            }
            catch (Exception e)
            {
                throw new NResourceException("Error occurred in read callback", e);
            }
            finally
            {
                FileHelper.safeClose(in_stream);
            }
        } // read()

        public void write(CsAction<NOutputStream> writeCallback) // throws ResourceException
        {
            NOutputStream out_stream = write();
            try
            {
                writeCallback.run(out_stream);
            }
            catch (Exception e)
            {
                throw new NResourceException("Error occurred in write callback", e);
            }
            finally
            {
                FileHelper.safeClose(out_stream);
            }
        } // write()

        public void append(CsAction<NOutputStream> appendCallback) // throws ResourceException
        {
            NOutputStream out_stream = append();
            try
            {
                appendCallback.run(out_stream);
            }
            catch (Exception e)
            {
                throw new NResourceException("Error occurred in append callback", e);
            }
            finally
            {
                FileHelper.safeClose(out_stream);
            }
        } // append()

        public override String ToString()
        {
            return this.GetType().Name + "[" + getQualifiedPath() + "]";
        }

        public string getQualifiedPath()
        {
            throw new NotImplementedException();
        }

        public bool isReadOnly()
        {
            throw new NotImplementedException();
        }

        public bool isExists()
        {
            throw new NotImplementedException();
        }

        public long getSize()
        {
            throw new NotImplementedException();
        }

        public long getLastModified()
        {
            throw new NotImplementedException();
        }

        public NOutputStream write()
        {
            throw new NotImplementedException();
        }

        public NOutputStream append()
        {
            throw new NotImplementedException();
        }

        public NInputStream read()
        {
            throw new NotImplementedException();
        }

        public string getName()
        {
            throw new NotImplementedException();
        }

        public void append(CsAction<NInputStream> appendCallback)
        {
            throw new NotImplementedException();
        }
    } // AbstractResource class
} // org.apache.metamodel.core.util namespace
