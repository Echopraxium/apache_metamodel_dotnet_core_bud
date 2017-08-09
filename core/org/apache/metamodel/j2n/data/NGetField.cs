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
// import java.io.ObjectInputStream.GetField;
using System;
using System.Reflection;

namespace org.apache.metamodel.j2n.data
{
    public class NGetField
    {
        private Object      _o;
        private Type        _object_type;
        private FieldInfo[] _fields;

        public NGetField(Object o_arg, FieldInfo[] fields_arg)
        {
            _o           = o_arg;
            _object_type = _o.GetType();
            _fields      = fields_arg;
        }

        // public override FieldAttributes Attributes => throw new NotImplementedException();

        // public override Type FieldType => throw new NotImplementedException();

        public Type DeclaringType
        {
            get { return _object_type;  }
        }

        public string Name => throw new NotImplementedException();

        public object GetValue(object obj)
        {
            throw new NotImplementedException();
        }
    
        public NDataSource get(string s, object o)
        {
            throw new NotImplementedException("NGetField.get()");
        }
    }
}
