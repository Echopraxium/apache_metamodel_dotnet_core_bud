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
// https://github.com/apache/metamodel/blob/master/core/src/main/java/org/apache/metamodel/util/CollectionUtils.java
using org.apache.metamodel.j2n.collections;
using org.apache.metamodel.j2n.data.numbers;
using org.apache.metamodel.j2n.exceptions;
using org.apache.metamodel.j2n.types;
using System;
using System.Collections.Generic;

namespace org.apache.metamodel.util
{
    public class CollectionUtils
    {
        private CollectionUtils()
        {
            // prevent instantiation
        }

        public static List<E> filter <E, SuperE> (E[] items, Predicate<SuperE> predicate) where E: SuperE
        {
            return filter(NArrays.AsList(items), predicate);
        } // filter()

        public static List<E> filter<E, SuperE>(IEnumerable<E> items, Predicate<SuperE> predicate) where E : SuperE
        {
            List<E> result = new List<E>();
            foreach (E e in items)
            {
                // if (predicate.eval(e).booleanValue())
                if (predicate.eval(e))
                {
                    result.Add(e);
                }
            }
            return result;
        } // filter()

        public static List<O> map <I, SuperI, O> (I[] items, NFunc<SuperI, O> func) where I : SuperI
        {
            return map(NArrays.AsList(items), func);
        } // map()

        public static List<O> map <I, SuperI, O> (IEnumerable<I> items, NFunc<SuperI, O> func) where I : SuperI
        {
            List<O> result = new List<O>();
            foreach (I item in items)
            {
                O output = func.eval((SuperI) item);
                result.Add(output);
            }
            return result;
        } // map()


        // https://stackoverflow.com/questions/36449343/what-is-the-c-sharp-equivalent-of-java-8-java-util-function-consumer
        public static void forEach<E> (E[] items, NConsumer<E> action)
        {
            forEach(NArrays.AsList(items), action);
        } // forEach()

        public static void forEach<E>(IEnumerable<E> items, NConsumer<E> action)
         {
            foreach (E item in items)
            {
                 try
                 {
                    action.accept(item);
                 }
                 catch (Exception e)
                 {
                    if (e is NRuntimeException) 
                    {
                        throw (NRuntimeException) e;
                    }
                    throw new InvalidOperationException("Action threw exception", e);
                }
            }
        }  // forEach()

        //        /**
        // * Searches a map for a given key. The key can be a regular map key, or a
        // * simple expression of the form:
        // * 
        // * <ul>
        // * <li>foo.bar (will lookup 'foo', and then 'bar' in a potential nested map)
        // * </li>
        // * <li>foo.bar[0].baz (will lookup 'foo', then 'bar' in a potential nested
        // * map, then pick the first element in case it is a list/array and then pick
        // * 'baz' from the potential map at that position).
        // * </ul>
        // * 
        // * @param map
        // *            the map to search in
        // * @param key
        // *            the key to resolve
        // * @return the object in the map with the given key/expression. Or null if
        // *         it does not exist.
        // */
         public static Object find(Dictionary<object, object> map, String key)
         {
             if (map == null || key == null)
             {
                 return null;
             }
             Object result = map[key];
             if (result == null)
             {
                 return find(map, key, 0);
             }
             return result;
         } // find()

         private static Object find(Dictionary<object, object> map, String key, int fromIndex)
         {
              int indexOfDot        = key.IndexOf('.', fromIndex);
              int indexOfBracket    = key.IndexOf('[', fromIndex);

              int indexOfEndBracket = -1;
              int arrayIndex        = -1;

              bool hasDot           = indexOfDot != -1;
              bool hasBracket       = indexOfBracket != -1;

              if (hasBracket)
              {
                  // also check that there is an end-bracket
                  indexOfEndBracket = key.IndexOf("]", indexOfBracket);
                  hasBracket        = indexOfEndBracket != -1;
                  if (hasBracket)
                  {
                      String indexString = key.Substring(indexOfBracket + 1, indexOfEndBracket);
                      try
                      {
                          arrayIndex = NInteger.ParseInt(indexString);
                      }
                      catch (FormatException e)
                      {
                          // not a valid array/list index
                          hasBracket = false;
                      }
                      }
                  }

                  if (hasDot && hasBracket)
                  {
                      if (indexOfDot > indexOfBracket)
                      {
                          hasDot = false;
                      }
                     else
                     {
                          hasBracket = false;
                     }
                  }

                    if (hasDot)
                    {
                        String prefix = key.Substring(0, indexOfDot);
                        Object nestedObject = map[prefix];
                        if (nestedObject == null)
                        {
                            return find(map, key, indexOfDot + 1);
                        }
                        if (nestedObject is Dictionary<String, object>) 
                        {
                            String remainingPart = key.Substring(indexOfDot + 1);
                            //@SuppressWarnings("unchecked")
                            Dictionary<object, object> nestedMap = (Dictionary<object, object>) nestedObject;
                            return find(nestedMap, remainingPart);
                        }
                    }

                    if (hasBracket)
                    {
                        String prefix       = key.Substring(0, indexOfBracket);
                        Object nestedObject = map[prefix];
                        if (nestedObject == null)
                        {
                            return find(map, key, indexOfBracket + 1);
                        }

                        String remainingPart = key.Substring(indexOfEndBracket + 1);

                        try
                        {
                            Object valueAtIndex;
                            if (nestedObject is List<object>) 
                            {
                                valueAtIndex = ((List<object>) nestedObject)[arrayIndex];
                            } 
                            else if (nestedObject.GetType().IsArray)
                            {
                                valueAtIndex = ((Array)nestedObject).GetValue(arrayIndex);
                            }
                            else
                            {
                                // no way to extract from a non-array and non-list
                                valueAtIndex = null;
                            }

                            if (valueAtIndex != null)
                            {
                                if (remainingPart.StartsWith("."))
                                {
                                    remainingPart = remainingPart.Substring(1);
                                }

                                if (remainingPart.IsEmpty())
                                {
                                    return valueAtIndex;
                                }

                                if (valueAtIndex is Dictionary<object, object>) 
                                {
                                    //@SuppressWarnings("unchecked")
                                    Dictionary<object, object> nestedMap = (Dictionary<object, object>) valueAtIndex;
                                    return find(nestedMap, remainingPart);
                                } else
                                {
                                    // not traversing any further. Should we want to add
                                    // support for double-sided arrays, we could do it here.
                                }
                            }

                        }
                        catch (NIndexOutOfBoundsException e)
                        {
                            return null;
                        }
                    }

                    return null;
              } // find()

            /**
             * Concatenates two arrays
             * 
             * @param existingArray
             *            an existing array
             * @param elements
             *            the elements to add to the end of it
             * @return
             */
             //@SuppressWarnings("unchecked")
        //    public static <E> E[] array(final E[] existingArray, final E... elements)
        //        {
        //            if (existingArray == null)
        //            {
        //                return elements;
        //            }
        //            Object result = Array.newInstance(existingArray.getClass().getComponentType(),
        //                    existingArray.length + elements.length);
        //            System.arraycopy(existingArray, 0, result, 0, existingArray.length);
        //            System.arraycopy(elements, 0, result, existingArray.length, elements.length);
        //            return (E[])result;
        //        }

                public static List<E> concat<E>(bool removeDuplicates, IList<E> firstCollection,
                                                params IList<E>[] collections)
                {
                    List<E> result;
                    if (removeDuplicates)
                    {
                        result = new List<E>();
                        addElements(removeDuplicates, result, firstCollection);
                    }
                    else
                    {
                        result = new List<E>(firstCollection);
                    }
                    foreach (IList<object> collection in collections)
                    {
                        // @SuppressWarnings("unchecked")
                        IList<E> elems = (IList<E>) collection;
                        addElements(removeDuplicates, result, elems);
                    }
                    return result;
            } // concat()

            private static void addElements<E>(bool removeDuplicates, List<E> result,
                                               IList<E> elements)
            {
                foreach (E item in elements)
                {
                    if (removeDuplicates)
                    {
                        if (! result.Contains(item))
                        {
                            result.Add(item);
                        }
                    }
                    else
                    {
                        result.Add(item);
                    }
                }
            } // addElements()

        //        public static <E> E[] arrayRemove(E[] array, E elementToRemove)
        //        {
        //            @SuppressWarnings("unchecked")
        //            E[] result = (E[])arrayRemoveInternal(array, elementToRemove);
        //            return result;
        //        }

        //        public static Object arrayRemove(Object array, Object elementToRemove)
        //        {
        //            return arrayRemoveInternal(array, elementToRemove);
        //        }

        //        private static Object arrayRemoveInternal(Object array, Object elementToRemove)
        //        {
        //            boolean found = false;
        //            final int oldLength = Array.getLength(array);
        //            if (oldLength == 0)
        //            {
        //                return array;
        //            }
        //            final int newLength = oldLength - 1;
        //            final Object result = Array.newInstance(array.getClass().getComponentType(), newLength);
        //            int nextIndex = 0;
        //            for (int i = 0; i < oldLength; i++)
        //            {
        //                final Object e = Array.get(array, i);
        //                if (e.equals(elementToRemove))
        //                {
        //                    found = true;
        //                }
        //                else
        //                {
        //                    if (nextIndex == newLength)
        //                    {
        //                        break;
        //                    }
        //                    Array.set(result, nextIndex, e);
        //                    nextIndex++;
        //                }
        //            }
        //            if (!found)
        //            {
        //                return array;
        //            }
        //            return result;
        //        }

        //    @SuppressWarnings("unchecked")
        //    public static <E> E[] arrayOf(Class<E> elementClass, Object arrayOrElement)
        //        {
        //            if (arrayOrElement == null)
        //            {
        //                return null;
        //            }
        //            if (arrayOrElement.getClass().isArray())
        //            {
        //                return (E[])arrayOrElement;
        //            }
        //            Object result = Array.newInstance(elementClass, 1);
        //            Array.set(result, 0, arrayOrElement);
        //            return (E[])result;
        //        }

        //        public static <E> List<E> filter(E[] items, java.util.function.Predicate<? super E> predicate)
        //        {
        //            return filter(Arrays.asList(items), predicate);
        //        }

        //        public static <E> List<E> filter(Iterable<E> items, java.util.function.Predicate<? super E> predicate)
        //        {
        //            List<E> result = new ArrayList<E>();
        //            for (E e : items)
        //            {
        //                if (predicate.test(e))
        //                {
        //                    result.add(e);
        //                }
        //            }
        //            return result;
        //        }

        //public static <E> boolean isNullOrEmpty(E[] arr)
        //{
        //    return arr == null || arr.length == 0;
        //}

        //public static boolean isNullOrEmpty(Collection<?> col)
        //{
        //    return col == null || col.isEmpty();
        //}

        ///**
        // * General purpose list converter method. Will convert arrays, collections,
        // * iterables etc. into lists.
        // * 
        // * If the argument is a single object (such as a String or a POJO) it will
        // * be wrapped in a single-element list.
        // * 
        // * Null will be converted to the empty list.
        // * 
        // * @param obj
        // *            any object
        // * @return a list representation of the object
        // */
        //public static List<?> toList(Object obj)
        //{
        //    final List<Object> result;
        //    if (obj == null)
        //    {
        //        result = Collections.emptyList();
        //    }
        //    else if (obj instanceof List) {
        //        @SuppressWarnings("unchecked")
        //            List<Object> list = (List<Object>)obj;
        //        result = list;
        //    } else if (obj.getClass().isArray())
        //    {
        //        int length = Array.getLength(obj);
        //        result = new ArrayList<Object>(length);
        //        for (int i = 0; i < length; i++)
        //        {
        //            result.add(Array.get(obj, i));
        //        }
        //    }
        //    else if (obj instanceof Iterable) {
        //        result = new ArrayList<Object>();
        //        for (Object item : (Iterable <?>) obj)
        //        {
        //            result.add(item);
        //        }
        //    } else if (obj instanceof Iterator) {
        //        result = new ArrayList<Object>();
        //        Iterator <?> it = (Iterator <?>) obj;
        //        while (it.hasNext())
        //        {
        //            result.add(it.next());
        //        }
        //    } else {
        //        result = new ArrayList<Object>(1);
        //        result.add(obj);
        //    }
        //    return result;
        //}
    } // CollectionUtils class
} // org.apache.metamodel.util namespace
