
// https://github.com/apache/metamodel/blob/master/core/src/main/java/org/apache/metamodel/factory/DataContextPropertiesImpl.java
//import java.net.URI;
//import java.util.HashMap;
//import java.util.Map;
//import java.util.Properties;
//import java.util.Set;

//import javax.sql.DataSource;

//import org.apache.metamodel.schema.TableType;
//import org.apache.metamodel.util.BooleanComparator;
//import org.apache.metamodel.util.NumberComparator;
//import org.apache.metamodel.util.SimpleTableDef;
//import org.apache.metamodel.util.SimpleTableDefParser;

using org.apache.metamodel.core.util;
using org.apache.metamodel.j2n.data.numbers;
using org.apache.metamodel.schema;
using org.apache.metamodel.util;
using System;
using System.Collections.Generic;
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
using org.apache.metamodel.j2n.data;

namespace org.apache.metamodel.core.factory
{
    public class DataContextPropertiesImpl //: DataContextProperties
    {
        private static readonly long serialVersionUID = 1L;
        public static readonly string PROPERTY_USERNAME = "username";
        public static readonly string PROPERTY_PASSWORD = "password";
        public static readonly string PROPERTY_DRIVER_CLASS = "driver-class";
        public static readonly string PROPERTY_HOSTNAME = "hostname";
        public static readonly string PROPERTY_PORT = "port";
        public static readonly string PROPERTY_DATABASE = "database";
        public static readonly string PROPERTY_URL = "url";
        public static readonly string PROPERTY_CATALOG_NAME = "catalog";
        public static readonly string PROPERTY_RESOURCE_PROPERTIES = "resource";
        public static readonly string PROPERTY_IS_MULTILINE_VALUES_ENABLED = "multiline-values";
        public static readonly string PROPERTY_IS_FAIL_ON_INCONSISTENT_ROW_LENGTH = "fail-on-inconsistent-row-length";
        public static readonly string PROPERTY_ESCAPE_CHAR = "escape-char";
        public static readonly string PROPERTY_QUOTE_CHAR = "quote-char";
        public static readonly string PROPERTY_SEPARATOR_CHAR = "separator-char";
        public static readonly string PROPERTY_ENCODING = "encoding";
        public static readonly string PROPERTY_SKIP_EMPTY_COLUMNS = "skip-empty-columns";
        public static readonly string PROPERTY_SKIP_EMPTY_LINES = "skip-empty-lines";
        public static readonly string PROPERTY_COLUMN_NAME_LINE_NUMBER = "column-name-line-number";
        public static readonly string PROPERTY_DATA_CONTEXT_TYPE = "type";
        public static readonly string PROPERTY_TABLE_TYPES = "table-types";
        public static readonly string PROPERTY_DATA_SOURCE = "data-source";
        public static readonly string PROPERTY_TABLE_DEFS = "table-defs";

        private Dictionary<string, object> map;

        #region constructor
        public DataContextPropertiesImpl() : this(new Dictionary<String, Object>())
        {       
        } // constructor

        public DataContextPropertiesImpl(NProperties properties) : this()
        {
            Dictionary<string, string>.ValueCollection propertyNames = properties.stringPropertyNames();
            foreach (String key in propertyNames)
            {
                put(key, properties.get(key));
            }
        } // constructor

        public DataContextPropertiesImpl(Dictionary<string, object> map)
        {
            this.map = map;
        } // constructor
        #endregion constructor


        public object get(string key)
        {
            return map[key];
        } // get()

        public object put(string key, object value)
        {
            map.Add(key, value);
            return value;
        } // put()

        public string getString(string key)
        {
            object value = map[key];
            if (value == null)
            {
                return null;
            }
            return value.ToString();
        } // getString()

        public char? getChar(string key)
        {
            string str = getString(key);
            if (str == null || str.IsEmpty())
            {
                return null;
            }
            return str[0];
        } // getChar()

        public NInteger getInt(string key)
        {
            object obj = get(key);
            if (obj == null)
            {
                return null;
            }
            return NumberComparator.toNumber(obj).asInt();
        } // getInt()

        private bool? getBoolean(string key)
        {
            object obj = get(key);
            if (obj == null)
            {
                return null;
            }
            return BooleanComparator.toBoolean(obj);
        } // getBoolean()

        // @SuppressWarnings("unchecked")
        public Dictionary<string, object> getMap(string key)
        {
            object obj = get(key);
            if (obj == null)
            {
                return null;
            }
            if (obj is Dictionary<string, object>) {
                return (Dictionary<string, object>)obj;
            }
            if (obj is string)
            {
                // TODO: Try parse as JSON
            }
            throw new InvalidOperationException("Expected Map value for property '" + key + "'. Found " + obj.GetType().Name);
        } // getMap()

        // @Override
        public String getDataContextType()
        {
            return getString(PROPERTY_DATA_CONTEXT_TYPE);
        } // getDataContextType()

        public void setDataContextType(String type)
        {
            put(PROPERTY_DATA_CONTEXT_TYPE, type);
        }

        // @Override
        public Dictionary<String, Object> toMap()
        {
            return map;
        } // toMap()

        // @Override
        public ResourceProperties getResourceProperties()
        {
            object resourceValue = get(PROPERTY_RESOURCE_PROPERTIES);
            if (resourceValue == null)
            {
                return null;
            }
            if (resourceValue is String)
            {
                return new SimpleResourceProperties((String)resourceValue);
            }
            if (resourceValue is Uri)
            {
                return new SimpleResourceProperties((Uri)resourceValue);
            }
            if (resourceValue is Dictionary<String, Object> ) 
            {
                // @SuppressWarnings("unchecked")
                Dictionary<String, Object> resourceMap = (Dictionary<String, Object>) resourceValue;
                return new ResourcePropertiesImpl(resourceMap);
            }
            throw new InvalidOperationException("Expected String, URI or Map value for property 'resource'. Found: "
                                                + resourceValue);
        }

        // @Override
        public NInteger getColumnNameLineNumber()
        {
            return getInt(PROPERTY_COLUMN_NAME_LINE_NUMBER);
        } // getColumnNameLineNumber()

        // @Override
        public bool? isSkipEmptyLines()
        {
            return getBoolean(PROPERTY_SKIP_EMPTY_LINES);
        } // isSkipEmptyLines()

        // @Override
        public bool? isSkipEmptyColumns()
        {
            return getBoolean(PROPERTY_SKIP_EMPTY_COLUMNS);
        } // isSkipEmptyColumns()

        //  @Override
        public String getEncoding()
        {
            return getString(PROPERTY_ENCODING);
        } // getEncoding()

        // @Override
        public char? getSeparatorChar()
        {
            return getChar(PROPERTY_SEPARATOR_CHAR);
        } // getSeparatorChar()

        // @Override
        public char? getQuoteChar()
        {
            return getChar(PROPERTY_QUOTE_CHAR);
        } // getQuoteChar()

        // @Override
        public char? getEscapeChar()
        {
            return getChar(PROPERTY_ESCAPE_CHAR);
        } // getEscapeChar()

        // @Override
        public bool? isFailOnInconsistentRowLength()
        {
            return getBoolean(PROPERTY_IS_FAIL_ON_INCONSISTENT_ROW_LENGTH);
        } // isFailOnInconsistentRowLength()

        // @Override
        public bool? isMultilineValuesEnabled()
        {
            return getBoolean(PROPERTY_IS_MULTILINE_VALUES_ENABLED);
        } // isMultilineValuesEnabled()

        // @Override
        public TableType[] getTableTypes()
        {
            Object obj = get(PROPERTY_TABLE_TYPES);
            if (obj == null)
            {
                return null;
            }
            if (obj is TableType[])
            {
                return (TableType[]) obj;
            }
            if (obj is TableType) 
            {
                return new TableType[] { (TableType)obj };
            }
            if (obj is String)
            {
                String str = (String)obj;
                if (str.StartsWith("[") && str.EndsWith("]"))
                {
                    str = str.Substring(1, str.Length - 2);
                }
                String[]    tokens     = str.Split(',');
                TableType[] tableTypes = new TableType[tokens.Length];
                for (int i = 0; i < tableTypes.Length; i++)
                {
                    tableTypes[i] = TableType.getTableType(tokens[i]);
                }
            }
            throw new InvalidOperationException("Expected TableType[] value for property '" + PROPERTY_TABLE_TYPES + "'. Found "
                                                + obj.GetType().Name);
        } // getTableTypes()

        // @Override
        public String getCatalogName()
        {
            return getString(PROPERTY_CATALOG_NAME);
        } // getCatalogName()

        // @Override
        public String getUrl()
        {
            return getString(PROPERTY_URL);
        } // getUrl()

        // @Override
        public NDataSource getDataSource()
        {
            return (NDataSource) get(PROPERTY_DATA_SOURCE);
        } // getDataSource()

        // @Override
        public String getUsername()
        {
            return getString(PROPERTY_USERNAME);
        } // getUsername()

        // @Override
        public String getPassword()
        {
            return getString(PROPERTY_PASSWORD);
        } // getPassword()

        // @Override
        public String getDriverClassName()
        {
            return getString(PROPERTY_DRIVER_CLASS);
        }

        // @Override
        public String getHostname()
        {
            return getString(PROPERTY_HOSTNAME);
        } // getHostname()

        // @Override
        public NInteger getPort()
        {
            return getInt(PROPERTY_PORT);
        } // getPort()

        // @Override
        public String getDatabaseName()
        {
            return getString(PROPERTY_DATABASE);
        } // getDatabaseName()

        // @Override
        public SimpleTableDef[] getTableDefs()
        {
            Object obj = get(PROPERTY_TABLE_DEFS);
            if (obj == null)
            {
                return null;
            }
            if (obj is SimpleTableDef[])
            {
                return (SimpleTableDef[])obj;
            }
            if (obj is SimpleTableDef)
            {
                return new SimpleTableDef[] { (SimpleTableDef)obj };
            }
            if (obj is String)
            {
                return SimpleTableDefParser.parseTableDefs((String)obj);
            }
            throw new InvalidOperationException("Expected SimpleTableDef[] value for property '" + PROPERTY_TABLE_DEFS
                    + "'. Found " + obj.GetType().Name);
        } // getTableDefs()
    } // DataContextPropertiesImpl class
} //  org.apache.metamodel.core.factory namespace
