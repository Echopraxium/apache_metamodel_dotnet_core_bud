// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/util/EqualsBuilder.java
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
using org.apache.metamodel.j2n.slf4j;
using System;

namespace org.apache.metamodel.core.util
{
    /**
     * A helper class for implementing equals(...) methods.
     */
    public sealed class EqualsBuilder
    {
        private static readonly NLogger logger = NLoggerFactory.getLogger(typeof(EqualsBuilder).Name);

	    private bool _equals = true;

        public EqualsBuilder append(bool b)
        {
            logger.debug("append({})", b);
            if (_equals)
            {
                _equals = b;
            }
            return this;
        }

        public EqualsBuilder append(Object o1, Object o2)
        {
            if (_equals)
            {
                _equals = equals(o1, o2);
            }
            return this;
        }

        public static bool equals(Object obj1, Object obj2)
        {
            if (obj1 == obj2)
            {
                return true;
            }

            if (obj1 == null || obj2 == null)
            {
                return false;
            }

            Type class1 = obj1.GetType();
            Type class2 = obj2.GetType();
            if (class1.IsArray)
            {
                if (! class2.IsArray)
                {
                    return false;
                }
                else
                {
                    Type componentType1 = class1.GetElementType();
                    Type componentType2 = class2.GetElementType();
                    if (!componentType1.Equals(componentType2))
                    {
                        return false;
                    }

                    int length1 = ((Array) obj1).Length;
                    int length2 = ((Array) obj2).Length;
                    if (length1 != length2)
                    {
                        return false;
                    }
                    for (int i = 0; i < length1; i++)
                    {
                        Object elem1 = ((Array) obj1).GetValue(i);
                        Object elem2 = ((Array) obj2).GetValue(i);
                        if (!equals(elem1, elem2))
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            else
            {
                if (class2.IsArray)
                {
                    return false;
                }
            }

            return obj1.Equals(obj2);
        }

        public bool isEquals()
        {
            return _equals;
        }
    } // EqualsBuilder class
} // org.apache.metamodel.core.util
