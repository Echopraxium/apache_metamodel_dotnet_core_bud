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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/schema/JdbcTypes.java
namespace org.apache.metamodel.schema
{
    /**
     * This is a copy of the content (comments removed) of Java 6.0's
     * java.sql.Types. It is backwards compatible with older versions, but have
     * additional types (confirmed by JavaTypesTest). It is being used to convert
     * JDBC types to ColumnType enumerations.
     */
    public sealed class JdbcTypes
    {

        // Prevent instantiation
        private JdbcTypes()
        {
        }

        public readonly static int BIT = -7;
        public readonly static int TINYINT = -6;
        public readonly static int SMALLINT = 5;
        public readonly static int INTEGER = 4;
        public readonly static int BIGINT = -5;
        public readonly static int FLOAT = 6;
        public readonly static int REAL = 7;
        public readonly static int DOUBLE = 8;
        public readonly static int NUMERIC = 2;
        public readonly static int DECIMAL = 3;
        public readonly static int CHAR = 1;
        public readonly static int VARCHAR = 12;
        public readonly static int LONGVARCHAR = -1;
        public readonly static int DATE = 91;
        public readonly static int TIME = 92;
        public readonly static int TIMESTAMP = 93;
        public readonly static int BINARY = -2;
        public readonly static int VARBINARY = -3;
        public readonly static int LONGVARBINARY = -4;
        public readonly static int NULL = 0;
        public readonly static int OTHER = 1111;
        public readonly static int JAVA_object = 2000;
        public readonly static int DISTINCT = 2001;
        public readonly static int STRUCT = 2002;
        public readonly static int ARRAY = 2003;
        public readonly static int BLOB = 2004;
        public readonly static int CLOB = 2005;
        public readonly static int REF = 2006;
        public readonly static int DATALINK = 70;
        public readonly static int BOOLEAN = 16;
        public readonly static int ROWID = -8;
        public static readonly int NCHAR = -15;
        public static readonly int NVARCHAR = -9;
        public static readonly int LONGNVARCHAR = -16;
        public static readonly int NCLOB = 2011;
        public static readonly int SQLXML = 2009;
    } // JdbcTypes class
} //  org.apache.metamodel.schema namespace
