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
// https://github.com/apache/metamodel/blob/master/core/src/main/java/org/apache/metamodel/schema/ColumnTypeImpl.java
using org.apache.metamodel.j2cs;
using org.apache.metamodel.j2cs.data.date_time;
using org.apache.metamodel.j2cs.data.numbers;
using org.apache.metamodel.j2cs.reflection;
using org.apache.metamodel.j2cs.slf4j;
using org.apache.metamodel.j2cs.types;
using org.apache.metamodel.util;
using System;
using System.Collections.Generic;
using System.Net;
using System.Numerics;
using System.Reflection;

namespace org.apache.metamodel.schema
{
    /**
     * Default implementation of ColumnType
     */
    public class ColumnTypeImpl : ColumnType
    {
        private static readonly long serialVersionUID = 1L;
        public  static readonly Logger logger = LoggerFactory.getLogger(typeof(ColumnTypeImpl).Name);

        private string            _name;
        private SuperColumnType   _superColumnType;
        private Type              _javaType;
        private bool              _large_object;

        public ColumnTypeImpl(string name, SuperColumnType superColumnType) :
                              this(name, superColumnType, null)
        {
        } // constructor

        public ColumnTypeImpl(string name, SuperColumnType superColumnType, Type javaType) :
                              this(name, superColumnType, javaType, false)
        {
        } // constructor

        public ColumnTypeImpl(string name, SuperColumnType superColumnType, Type javaType, bool largeObject)
        {
            if (name == null)
            {
                throw new ArgumentException("Name cannot be null");
            }
            if (superColumnType == null)
            {
                throw new ArgumentException("SuperColumnType cannot be null");
            }
            _name            = name;
            _superColumnType = superColumnType;
            if (javaType == null)
            {
                _javaType = superColumnType.getJavaEquivalentClass();
            }
            else
            {
                _javaType = javaType;
            }
            _large_object = largeObject;
        } // constructor

        public string getName()
        {
            return _name;
        } //  getName()

        // public override Comparator<object> getComparator()
        public IComparer<object> getComparator()
        {
            if (isTimeBased())
            {
                return TimeComparator.getComparator();
            }
            if (isNumber())
            {
                return NumberComparator.getComparator();
            }
            if (isLiteral())
            {
                return ToStringComparator.getComparator();
            }
            return ObjectComparator.getComparator();
        } // getComparator()


        public bool isBoolean()
        {
            return _superColumnType == SuperColumnType.BOOLEAN_TYPE;
        }

        public bool isBinary()
        {
            return _superColumnType == SuperColumnType.BINARY_TYPE;
        }

        public bool isNumber()
        {
            return _superColumnType == SuperColumnType.NUMBER_TYPE;
        }

        public bool isTimeBased()
        {
            return _superColumnType == SuperColumnType.TIME_TYPE;
        }

        public bool isLiteral()
        {
            return _superColumnType == SuperColumnType.LITERAL_TYPE;
        }

        public bool isLargeObject()
        {
            return _large_object;
        }

        public Type getJavaEquivalentClass()
        {
            return _javaType;
        }

        public SuperColumnType getSuperType()
        {
            return _superColumnType;
        }

        public int getJdbcType() // throws IllegalStateException
        {
            string name = this.ToString();
            try
            {
                // We assume that the JdbcTypes class only consists of constant
                // integer types, so we make no assertions here
                FieldInfo[] fields = typeof(JdbcTypes).GetFields();
                for (int i = 0; i<fields.Length; i++) 
                {
                    FieldInfo field  = fields[i];
                    string fieldName = field.Name;
                    if (fieldName.Equals(name)) 
                    {
                        CsInteger value = CsInteger.ZERO;
                        field.GetValue(value);
                        return value;
                    }
                }
                throw new InvalidOperationException("No JdbcType found with field name: " + name);
            } 
            catch (Exception e) 
            {
                throw new InvalidOperationException("Could not access fields in JdbcTypes", e);
            }
        } // getJdbcType()

        public override string ToString()
        {
            return _name;
        } // ToString

        /**
         * Finds the ColumnType enum corresponding to the incoming JDBC
         * type-constant
         */
        public static ColumnType convertColumnType(int jdbcType)
        {
            try
            {
                FieldInfo[] fields = typeof(JdbcTypes).GetFields();
                // We assume that the JdbcTypes class only consists of constant
                // integer types, so we make no assertions here
                for (int i = 0; i<fields.Length; i++) 
                {
                    FieldInfo field = fields[i];
                    int value = (int) field.GetValue(null);
                    if (value == jdbcType) 
                    {
                        string fieldName = field.Name;
                        return valueOf(fieldName);
                    }
                }
            } 
            catch (Exception e) 
            {
                throw new InvalidOperationException("Could not access fields in JdbcTypes", e);
            }
            return ColumnTypeDefs.OTHER;
        } // convertColumnType()

        /**
         * Finds the ColumnType enum corresponding to the incoming Java class.
         * 
         * @param cls
         * @return
         */
        public static ColumnType convertColumnType(Type cls)
        {
            if (cls == null)
            {
                throw new ArgumentException("Class cannot be null");
            }

            ColumnType type;
            if (cls == typeof(string))
            {
                type = ColumnTypeDefs.STRING;
            }
        else if (cls == typeof(bool)) {
            type = ColumnTypeDefs.BOOLEAN;
        }
        else if (cls == typeof(char) || cls == typeof(char[])) {
            type = ColumnTypeDefs.CHAR;
        }
        else if (cls == typeof(byte)) {
            type = ColumnTypeDefs.TINYINT;
        }
        else if (cls == typeof(short)) {
            type = ColumnTypeDefs.SMALLINT;
        }
            // https://stackoverflow.com/questions/9696660/what-is-the-difference-between-int-int16-int32-and-int64
            else if (cls == typeof(int) || cls == typeof(Int32)) {
                type = ColumnTypeDefs.INTEGER;
            }
            else if (cls == typeof(long) || cls == typeof(Int64)) {
                type = ColumnTypeDefs.BIGINT;
            }
            else if (cls == typeof(float)) {
                type = ColumnTypeDefs.FLOAT;
            }
            else if (cls == typeof(double)) {
                type = ColumnTypeDefs.DOUBLE;
            }
            // https://stackoverflow.com/questions/2863388/what-is-the-equivalent-of-the-java-bigdecimal-class-in-c
            // else if (cls == BigDecimal.class) {
            else if (cls == typeof(BigInteger)) {
                type = ColumnTypeDefs.DECIMAL;
            }
            //else if (cls == java.sql.Date.class) {
            else if (cls == typeof(DateTime))
            {
                type = ColumnTypeDefs.DATE;
            }
            else if (cls == typeof(CsTime))
            {
                type = ColumnTypeDefs.TIME;
            }
            else if (cls == typeof(Guid)) {
                type = ColumnTypeDefs.UUID;
            }
            else if (cls == typeof(CsTimeStamp).GetType()) {
                type = ColumnTypeDefs.TIMESTAMP;
            }
            else if (typeof(CsNumber).IsAssignableFrom(cls)) {
                type = ColumnTypeDefs.NUMBER;
            }
            else if (typeof(CsDate).IsAssignableFrom(cls))
            {
                // Date d;
                type = ColumnTypeDefs.TIMESTAMP;
            }

            else if (typeof(Dictionary<object, object>).IsAssignableFrom(cls))  
            {
                type = ColumnTypeDefs.MAP;
            }

            /*
            else if (Map.class.isAssignableFrom(cls)) {
                type = ColumnType.MAP;
            } 
            */

            else if (typeof(List<object>).IsAssignableFrom(cls)) 
            {
                type = ColumnTypeDefs.LIST;
            } 
            else if (typeof(HashSet<object>).IsAssignableFrom(cls))
            {
                type = ColumnTypeDefs.SET;
            }
            else if (cls == typeof(IPAddress).GetType()) {
                type = ColumnTypeDefs.INET;
            } 
            else 
            {
                type = ColumnTypeDefs.OTHER;
            }
            return type;
        } // convertColumnType()

        public static ColumnType valueOf(string fieldName)
        {
            try
            {
                FieldInfo column_type_field = TypeUtils.getField(typeof(ColumnType), fieldName);
                if (column_type_field != null) 
                {
                    // columnTypeField.setAccessible(true);
                    object o = new object();
                    column_type_field.GetValue(o);
                    ColumnType columnType = CsSystem.Cast(o, typeof(ColumnType));
                    return (ColumnType) columnType;
                }
            } 
           catch (Exception e) 
           {
                logger.error("Failed to resolve JDBC type in ColumnType constants: " + fieldName, e);
            }
            return null;
        } // valueOf()

        public Type getDotNetEquivalentClass<T>()
        {
            throw new NotImplementedException();
        } // getDotNetEquivalentClass()
    } // ColumnTypeImpl class
} // org.apache.metamodel.schema namespace
