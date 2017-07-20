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
using System.Reflection;

namespace org.apache.metamodel.j2n.reflection
{
    public class NTypeUtils
    {
        public static FieldInfo getField(Type type_arg, string name_arg)
        {
            FieldInfo   result_field = null;
            FieldInfo[] fields = type_arg.GetType().GetFields();
            foreach (FieldInfo f in fields)
            {
                if (f.Name == name_arg)
                {
                    result_field = f;
                    break;
                }      
            }
            return result_field;
        } // getField()
    } // NTypeUtils class
} // org.apache.metamodel.j2n.reflection namespace
