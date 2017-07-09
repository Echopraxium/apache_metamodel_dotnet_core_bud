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
// https://github.com/apache/metamodel/blob/master/core/src/main/java/org/apache/metamodel/schema/ColumnType.java
using System;
using org.apache.metamodel.util;
using System.Collections.Generic;
using System.Net;
using org.apache.metamodel.j2cs.data;
using org.apache.metamodel.j2cs.attributes;
using System.Numerics;
using org.apache.metamodel.j2cs.types;
using org.apache.metamodel.j2cs.collections;

namespace org.apache.metamodel.schema
{
    /**
     * Represents the data-type of columns.
    */
    [CsSerializableAttribute]
    public interface ColumnType : HasName //, ISerializable
    {
        // [J2Cs: inherited from HasName, cannot be redeclared]
        // string getName();

        bool isBoolean();
        bool isBinary();
        bool isNumber();
        bool isTimeBased();
        bool isLiteral();
        bool isLargeObject();

        /**
         * @return a DotNet class that is appropriate for handling column values of
         *   this column type
         */
        // Type<?> getJavaEquivalentClass();
        Type getDotNetEquivalentClass<T>();

        SuperColumnType getSuperType();

        //public void GetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    throw new NotImplementedException();
        //}

        // public Comparator<Object> getComparator();
        IComparer<object> getComparator();

        /**
         * Gets the JDBC type as per the {@link Types} class.
         * 
         * @return an int representing one of the constants in the {@link Types}
         *         class.
         * @throws IllegalStateException
         *             in case getting the JDBC type was unsuccesful.
         */
        int getJdbcType(); // throws IllegalStateException;
    } // ColumnType interface

    public class ColumnTypeDefs
    {
        /*
         * Literal
         */
        public static readonly ColumnType CHAR = new ColumnTypeImpl("CHAR", SuperColumnType.LITERAL_TYPE);
        public static readonly ColumnType VARCHAR = new ColumnTypeImpl("VARCHAR", SuperColumnType.LITERAL_TYPE);
        public static readonly ColumnType LONGVARCHAR = new ColumnTypeImpl("LONGVARCHAR", SuperColumnType.LITERAL_TYPE);
        public static readonly ColumnType CLOB = new ColumnTypeImpl("CLOB", SuperColumnType.LITERAL_TYPE, typeof(CsClob), true);
        public static readonly ColumnType NCHAR = new ColumnTypeImpl("NCHAR", SuperColumnType.LITERAL_TYPE);
        public static readonly ColumnType NVARCHAR = new ColumnTypeImpl("NVARCHAR", SuperColumnType.LITERAL_TYPE);
        public static readonly ColumnType LONGNVARCHAR = new ColumnTypeImpl("LONGNVARCHAR", SuperColumnType.LITERAL_TYPE);
        public static readonly ColumnType NCLOB = new ColumnTypeImpl("NCLOB", SuperColumnType.LITERAL_TYPE, typeof(CsClob), true);

        /*
         * Numbers
         */
        public static readonly ColumnType TINYINT = new ColumnTypeImpl("TINYINT", SuperColumnType.NUMBER_TYPE, typeof(short));
        public static readonly ColumnType SMALLINT = new ColumnTypeImpl("SMALLINT", SuperColumnType.NUMBER_TYPE, typeof(short));
        public static readonly ColumnType INTEGER = new ColumnTypeImpl("INTEGER", SuperColumnType.NUMBER_TYPE, typeof(int));
        public static readonly ColumnType BIGINT = new ColumnTypeImpl("BIGINT", SuperColumnType.NUMBER_TYPE, typeof(BigInteger));
        public static readonly ColumnType FLOAT = new ColumnTypeImpl("FLOAT", SuperColumnType.NUMBER_TYPE, typeof(double));
        public static readonly ColumnType DOUBLE = new ColumnTypeImpl("DOUBLE", SuperColumnType.NUMBER_TYPE, typeof(double));
        public static readonly ColumnType NUMERIC = new ColumnTypeImpl("NUMERIC", SuperColumnType.NUMBER_TYPE, typeof(double));
        public static readonly ColumnType DECIMAL = new ColumnTypeImpl("DECIMAL", SuperColumnType.NUMBER_TYPE, typeof(double));
        public static readonly ColumnType UUID = new ColumnTypeImpl("UUID", SuperColumnType.NUMBER_TYPE, typeof(Guid));

        /*
         * Time based
         */
        public static readonly ColumnType DATE = new ColumnTypeImpl("DATE", SuperColumnType.TIME_TYPE);
        public static readonly ColumnType TIME = new ColumnTypeImpl("TIME", SuperColumnType.TIME_TYPE);
        public static readonly ColumnType TIMESTAMP = new ColumnTypeImpl("TIMESTAMP", SuperColumnType.TIME_TYPE);

        /*
         * Booleans
         */
        public static readonly ColumnType BIT = new ColumnTypeImpl("BIT", SuperColumnType.BOOLEAN_TYPE);
        public static readonly ColumnType BOOLEAN = new ColumnTypeImpl("BOOLEAN", SuperColumnType.BOOLEAN_TYPE);

        /*
         * Binary types
         */
        public static readonly ColumnType BINARY = new ColumnTypeImpl("BINARY", SuperColumnType.BINARY_TYPE);
        public static readonly ColumnType VARBINARY = new ColumnTypeImpl("VARBINARY", SuperColumnType.BINARY_TYPE);
        public static readonly ColumnType LONGVARBINARY = new ColumnTypeImpl("LONGVARBINARY", SuperColumnType.BINARY_TYPE);
        public static readonly ColumnType BLOB = new ColumnTypeImpl("BLOB", SuperColumnType.BINARY_TYPE, typeof(CsClob), true);

        /*
         * Other types (as defined in {@link Types}).
         */
        public static readonly ColumnType NULL = new ColumnTypeImpl("NULL", SuperColumnType.OTHER_TYPE);
        public static readonly ColumnType OTHER = new ColumnTypeImpl("OTHER", SuperColumnType.OTHER_TYPE);
        public static readonly ColumnType JAVA_object = new ColumnTypeImpl("JAVA_OBJECT", SuperColumnType.OTHER_TYPE);
        public static readonly ColumnType DISTINCT = new ColumnTypeImpl("DISTINCT", SuperColumnType.OTHER_TYPE);
        public static readonly ColumnType STRUCT = new ColumnTypeImpl("STRUCT", SuperColumnType.OTHER_TYPE);
        public static readonly ColumnType ARRAY = new ColumnTypeImpl("ARRAY", SuperColumnType.OTHER_TYPE);
        public static readonly ColumnType REF = new ColumnTypeImpl("REF", SuperColumnType.OTHER_TYPE);
        public static readonly ColumnType DATALINK = new ColumnTypeImpl("DATALINK", SuperColumnType.OTHER_TYPE);
        public static readonly ColumnType ROWID = new ColumnTypeImpl("ROWID", SuperColumnType.OTHER_TYPE);
        public static readonly ColumnType SQLXML = new ColumnTypeImpl("SQLXML", SuperColumnType.OTHER_TYPE);
        public static readonly ColumnType INET = new ColumnTypeImpl("INET", SuperColumnType.OTHER_TYPE, typeof(CsList<IPAddress>));

        /*
         * Additional types (added by MetaModel for non-JDBC datastores)
         */
        public static readonly ColumnType LIST = new ColumnTypeImpl("LIST", SuperColumnType.OTHER_TYPE, typeof(CsList<object>));
        public static readonly ColumnType MAP = new ColumnTypeImpl("MAP", SuperColumnType.OTHER_TYPE, typeof(Dictionary<object, object>));
        public static readonly ColumnType SET = new ColumnTypeImpl("SET", SuperColumnType.OTHER_TYPE, typeof(HashSet<object>));
        public static readonly ColumnType STRING = new ColumnTypeImpl("STRING", SuperColumnType.LITERAL_TYPE);
        public static readonly ColumnType NUMBER = new ColumnTypeImpl("NUMBER", SuperColumnType.NUMBER_TYPE);
    } // ColumnTypeDefs class
} // org.apache.metamodel.schema namespace
