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
// https://github.com/apache/metamodel/blob/master/core/src/main/java/org/apache/metamodel/util/BaseObject.java
using org.apache.metamodel.j2n;
using org.apache.metamodel.j2n.collections;
using org.apache.metamodel.j2n.data.numbers;
using org.apache.metamodel.j2n.slf4j;
using org.apache.metamodel.core.util;
using System;
using System.Diagnostics;

namespace org.apache.metamodel.util
{
    public abstract class BaseObject : object
    {
        private static readonly NLogger logger = NLoggerFactory.getLogger(typeof(BaseObject).Name);

        public override string ToString()
        {
            // overridden version of toString() method that uses identity hash code
            // (to prevent hashCode() recursion due to logging!)
            return GetType().Name + "@"
                  + NInteger.ToHexString(NSystem.IdentityHashCode(this)); // Integer.toHexString(System.identityHashCode(this));
        } // ToString()

        /**
         * {@inheritDoc}
         */
        public int hashCode()
        {
            logger.debug("{}.hashCode()", this);
            int hash_code = -1;
            NList<object> list = new NList<object>();
            decorateIdentity(list);
            if (NEnumerableUtils.IsEmpty<object>(list))
            {
                list.Add(ToString());
            }
            hash_code -= list.Count;
            foreach (object obj in list)
            {
                hash_code += hashCode(obj);
            }
            return hash_code;
        } // hashCode()

        private static int hashCode(object obj)
        {
            if (obj == null)
            {
                logger.debug("obj is null, returning constant");
                return -17;
            }
            if (obj.GetType().IsArray)
            {
                logger.debug("obj is an array, returning a sum");
                // https://stackoverflow.com/questions/18633887/how-to-cast-an-object-that-is-an-array-back-to-an-array-or-something-more-gen
                int length    = ((object)obj as Array).GetLength(1);
                int hash_code = 4324;
                for (int i = 0; i < length; i++)
                {
                    object o = (object)((object)obj as Array).GetValue(i);
                    hash_code += hashCode(o);
                }
                return hash_code;
            }
            logger.debug("obj is a regular object, returning hashCode");
            return obj.GetHashCode();
        } // hashCode()

        /**
         * Override this method if the equals method should support different
         * subtypes. For example, if different subtypes of Number should be
         * supported, implement this method with:
         * 
         * <code>
         * obj instanceof Number
         * </code>
         * 
         * and make sure that the decorateIdentity(...) method will always return a
         * comparable list of identity-objects.
         * 
         * @param obj
         * @return true if the provided object's class is accepted for equals
         *         comparison
         */
        protected bool classEquals(BaseObject obj)
        {
            return GetType() == obj.GetType();
        } // classEquals()

        /**
         * {@inheritDoc}
         */
        public bool equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj == this)
            {
                return true;
            }

            if (obj is BaseObject)
            {
                BaseObject that = (BaseObject) obj;
                if (classEquals(that))
                {
                    NList<object> list1 = new NList<object>();
                    NList<object> list2 = new NList<object>();

                    decorateIdentity(list1);
                    that.decorateIdentity(list2);

                    if (list1.Count != list2.Count)
                    {
                        throw new InvalidOperationException("Two instances of the same class ("
                                                            + GetType().Name
                                                            + ") returned different size decorated identity lists");
                    }

                    if (NEnumerableUtils.IsEmpty<object>(list1))
                    {
                        Debug.Assert(NEnumerableUtils.IsEmpty<object>(list2));

                        list1.Add(ToString());
                        list2.Add(that.ToString());
                    }

                    EqualsBuilder eb = new EqualsBuilder();

                    while (list1.HasNext())
                    {
                        Debug.Assert(list2.HasNext());
                        object next1 = list1.Next();
                        object next2 = list2.Next();
                        eb.append(next1, next2);
                    }
                    Debug.Assert(! list2.HasNext());

                    return eb.isEquals();
                }
            }
            return false;
        } // equals()

        /**
         * Subclasses should implement this method and add all fields to the list
         * that are to be included in equals(...) and hashCode() evaluation
         * 
         * @param identifiers
         */
        public virtual void decorateIdentity(NList<object> identifiers)
        {
            throw new NotImplementedException();
        }
    } // BaseObject class
} // org.apache.metamodel.util namespace
