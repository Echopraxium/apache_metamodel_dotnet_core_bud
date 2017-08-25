
// https://github.com/apache/metamodel/blob/master/core/src/main/java/org/apache/metamodel/AbstractDataContext.java
//import java.util.ArrayList;
//import java.util.Arrays;
//import java.util.Comparator;
//import java.util.List;
//import java.util.concurrent.ConcurrentHashMap;
//import java.util.concurrent.ConcurrentMap;

//import org.apache.metamodel.data.DataSet;
//import org.apache.metamodel.query.CompiledQuery;
//import org.apache.metamodel.query.DefaultCompiledQuery;
//import org.apache.metamodel.query.Query;
//import org.apache.metamodel.query.builder.InitFromBuilder;
//import org.apache.metamodel.query.builder.InitFromBuilderImpl;
//import org.apache.metamodel.query.parser.QueryParser;
//import org.apache.metamodel.schema.Column;
//import org.apache.metamodel.schema.Schema;
//import org.apache.metamodel.schema.Table;

using org.apache.metamodel.query.builder;
using org.apache.metamodel.schema;
using System;
using org.apache.metamodel.core.data;
using org.apache.metamodel.query;
using org.apache.metamodel.core.query.parser;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using org.apache.metamodel.j2n.exceptions;
using System.Diagnostics;
using org.apache.metamodel.core.query;
using org.apache.metamodel.core;
using org.apache.metamodel.j2n.collections;
using org.apache.metamodel.core.schema.builder;
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
namespace org.apache.metamodel
{
    /**
     * Abstract implementation of the DataContext interface. Provides convenient
     * implementations of all trivial and datastore-independent methods.
     */
    public abstract class AbstractDataContext : DataContext
    {
        private static readonly string NULL_SCHEMA_NAME_TOKEN = "<metamodel.schema.name.null>";

        private readonly ConcurrentDictionary<string, Schema> _schemaCache          = new ConcurrentDictionary<String, Schema>();
        private readonly IComparer<string>                    _schemaNameComparator = SchemaNameComparator.getInstance();
        private string[]                                      _schemaNameCache;

        /**
         * {@inheritDoc}
         */
        public DataContext refreshSchemas()
        {
            _schemaCache.Clear();
            _schemaNameCache = null;
            onSchemaCacheRefreshed();
            return this;
        } // refreshSchemas()

        /**
         * Method invoked when schemas have been refreshed using
         * {@link #refreshSchemas()}. Can be overridden to add callback
         * functionality in subclasses.
         */
        protected void onSchemaCacheRefreshed()
        {
        } // onSchemaCacheRefreshed()

        protected string[] getNonEmptySchemaNames(string[] schema_names)
        {
            List<string> items = new List<string>();
            for (int i = 0; i < schema_names.Length; i++)
            {
                string schema_name = schema_names[i];
                if (schema_name != "")
                    items.Add(schema_name);
            }
            return items.ToArray();
        } // onSchemaCacheRefreshed()

        /**
         * {@inheritDoc}
         */
        public Schema[] getSchemas() //  throws MetaModelException
        {
            //string [] schema_names = getSchemaNames();
            string[] schema_names = getNonEmptySchemaNames(getSchemaNames());

            Schema [] schemas = new Schema[schema_names.Length];
            //Schema[] schemas = new Schema[non_empty_schema_names.Length];
            for (int i = 0; i < schema_names.Length; i++)
            {
                string schema_name = schema_names[i];
                Debug.WriteLine("schema_name: '" + schema_name + "'");

                Schema schema = null;
                if (_schemaCache.ContainsKey(schema_name))
                {
                    string schema_cache_key = getSchemaCacheKey(schema_name);
                    schema = _schemaCache[schema_cache_key];
                }

                if (schema == null)
                {
                    Schema newSchema = getSchemaByName(schema_name);
                    if (newSchema == null)
                    {
                        throw new MetaModelException(  "AbstractDataContext.getSchemas()\n"
                                                     + "    Declared schema does not exist: '" + schema_name + "'");
                    }

                    Schema existingSchema   = null;
                    string schema_cache_key = getSchemaCacheKey(schema_name);
                    if (_schemaCache.ContainsKey(schema_cache_key))
                       existingSchema = _schemaCache[schema_cache_key] = newSchema;

                    if (existingSchema == null)
                    {
                        schemas[i] = newSchema;
                    } else
                    {
                        schemas[i] = existingSchema;
                    }
                } else
                {
                    schemas[i] = schema;
                }
            }
            return schemas;
        } // getSchemas()

        private String getSchemaCacheKey(String name)
        {
          if (name == null || name =="" )
          {
              return NULL_SCHEMA_NAME_TOKEN;
          }
          return name;
        } // getSchemaCacheKey()

        ///**
        // * m {@inheritDoc}
        // */
        //@Override
        public String[] getSchemaNames() // throws MetaModelException
        {
            if (_schemaNameCache == null)
            {
                _schemaNameCache = getSchemaNamesInternal();
            }
            String [] schemaNames = NArrays.CopyOf(_schemaNameCache, _schemaNameCache.Length);
            Array.Sort(schemaNames, _schemaNameComparator);
            return schemaNames;
        }

        ///**
        // * {@inheritDoc}
        // */
        //@Override
        public Schema getDefaultSchema() // throws MetaModelException
        {
            Schema result            = null;
            String defaultSchemaName = getDefaultSchemaName();
            // if (defaultSchemaName != null)
            if (defaultSchemaName != null && defaultSchemaName != "")
            {
                result = getSchemaByName(defaultSchemaName);
            }

            if (result == null)
            {
                Schema[] schemas = getSchemas();
                if (schemas.Length == 1)
                {
                    result = schemas[0];
                }
                else
                {
                    int highestTableCount = -1;
                    for (int i = 0; i < schemas.Length; i++)
                    {
                        Schema schema = schemas[i];                       
                        if (schema != null)
                        {
                            String name = schema.getName();
                            name = name.ToLower();
                            bool isInformationSchema = name.StartsWith("information") && name.EndsWith("schema");
                            int  table_count         = schema.getTableCount();
                            if ( (! isInformationSchema) && (table_count > highestTableCount))
                            {
                                highestTableCount = schema.getTableCount();
                                result = schema;
                            }
                        }
                    }
                }
            }
            return result;
       } // getDefaultSchema()

        ///**
        // * {@inheritDoc}
        // */
        //@Override
        public InitFromBuilder query()
        {
           return new InitFromBuilderImpl(this);
        } // query()

        ///**
        // * {@inheritDoc}
        // */
        //@Override
        public Query parseQuery(String queryString) // throws MetaModelException
        {
             QueryParser parser = new QueryParser(this, queryString);
             Query       query  = parser.parse();
             return query;
        } // parseQuery()

        public CompiledQuery compileQuery(Query query) //throws MetaModelException
        {
            return new DefaultCompiledQuery(query);
        } // compileQuery()

        public DataSet executeQuery(Query query) // throws MetaModelException;
        {
            throw new NotImplementedException("AbstractDataContext.executeQuery(Query)");
        }

        public DataSet executeQuery(CompiledQuery compiledQuery, params Object[] values)
        {
            Debug.Assert(compiledQuery is DefaultCompiledQuery);

            DefaultCompiledQuery defaultCompiledQuery = (DefaultCompiledQuery)compiledQuery;
            Query query = defaultCompiledQuery.cloneWithParameterValues(values);

            return executeQuery((CompiledQuery) query);
        } // executeQuery()

        ///**
        // * {@inheritDoc}
        // */
        public DataSet executeQuery(String queryString) // throws MetaModelException
        {
            Query   query   = parseQuery(queryString);
            DataSet dataSet = executeQuery(query.ToString());
            return dataSet;
        } // executeQuery()

        /**
         * {@inheritDoc}
         */
        public virtual Schema getSchemaByName(String name)  // throws MetaModelException
        {
            Schema schema = null;
            string schema_cache_key = getSchemaCacheKey(name);
            if (_schemaCache.ContainsKey(schema_cache_key))
                 schema = _schemaCache[schema_cache_key];

            if (schema == null)
            {
                if (name == null)
                {
                    schema = getSchemaByNameInternal(null);
                }
                else
                {
                    String[] schemaNames = getSchemaNames();
                    foreach (String schemaName in schemaNames)
                    {
                        if (name.Equals(schemaName, StringComparison.CurrentCultureIgnoreCase))
                        {
                            schema = getSchemaByNameInternal(name);
                            break;
                        }
                    }
                    if (schema == null)
                    {
                        foreach (String schemaName in schemaNames)
                        {
                            if (name.Equals(schemaName, StringComparison.CurrentCultureIgnoreCase))
                            {
                                // try again with "schemaName" as param instead of
                                // "name".
                                schema = getSchemaByNameInternal(schemaName);
                                break;
                            }
                        }
                    }
                }
                if (schema != null)
                {
                    Schema existingSchema = null;
                    if (_schemaCache.ContainsKey(getSchemaCacheKey(schema.getName())))
                        existingSchema = _schemaCache[getSchemaCacheKey(schema.getName())] = schema;
                    if (existingSchema != null)
                    {
                        // race conditions may cause two schemas to be created.
                        // We'll favor the existing schema if possible, since schema
                        // may contain lazy-loading logic and so on.
                        return existingSchema;
                    }
                }
            }
            return schema;
        } // getSchemaByName()

        //    /**
        //     * {@inheritDoc}
        //     */
        //    @Override
        public Column getColumnByQualifiedLabel(String columnName)
        {
            Schema schema = null;

            if (columnName == null)
            {
                return null;
            }

            String[] tokens = tokenizePath(columnName, 3);
            if (tokens != null)
            {
                schema = getSchemaByToken(tokens[0]);
                if (schema != null)
                {
                    Table table = schema.getTableByName(tokens[1]);
                    if (table != null)
                    {
                        Column column = table.getColumnByName(tokens[2]);
                        if (column != null)
                        {
                            return column;
                        }
                    }
                }
            }

            schema = null;
            String[] schemaNames = getSchemaNames();
            foreach (String schemaName in schemaNames)
            {
                if (schemaName == null)
                {
                    // search without schema name (some databases have only a single
                    // schema with no name)
                    schema = getSchemaByName(null);
                    if (schema != null)
                    {
                        Column column = getColumn(schema, columnName);
                        if (column != null)
                        {
                            return column;
                        }
                    }
                }
                else
                {
                    // Search case-sensitive
                    Column col = searchColumn(schemaName, columnName, columnName);
                    if (col != null)
                    {
                        return col;
                    }
                }
            }

            String columnNameInLowerCase = columnName.ToLower();
            foreach (String schemaName in schemaNames)
            {
                if (schemaName != null)
                {
                    // search case-insensitive
                    String schameNameInLowerCase = schemaName.ToLower();
                    Column col = searchColumn(schameNameInLowerCase, columnName, columnNameInLowerCase);
                    if (col != null)
                    {
                       return col;
                    }
                }
            }

            schema = getDefaultSchema();
            if (schema != null)
            {
                Column column = getColumn(schema, columnName);
                if (column != null)
                {
                    return column;
                }
            }

            return null;
        } // getColumnByQualifiedLabel()

        ///**
        // * Searches for a particular column within a schema
        // * 
        // * @param schemaNameSearch
        // *            the schema name to use for search
        // * @param columnNameOriginal
        // *            the original column name
        // * @param columnNameSearch
        // *            the column name as it should be searched for (either the same
        // *            as original, or lower case in case of case-insensitive search)
        // * @return
        // */
        private Column searchColumn(String schemaNameSearch, String columnNameOriginal, String columnNameSearch)
        {
            if (columnNameSearch.StartsWith(schemaNameSearch))
            {
                Schema schema = getSchemaByName(schemaNameSearch);
                if (schema != null)
                {
                    String tableAndColumnPath = columnNameOriginal.Substring(schemaNameSearch.Length);

                    if (tableAndColumnPath[0] == '.')
                    {
                        tableAndColumnPath = tableAndColumnPath.Substring(1);

                        Column column = getColumn(schema, tableAndColumnPath);
                        if (column != null)
                        {
                            return column;
                        }
                    }
                }
            }
            return null;
        } // searchColumn()

        private Column getColumn(Schema schema, String tableAndColumnPath)
        {
            Table    table      = null;
            String   columnPath = tableAndColumnPath;
            String[] tableNames = schema.getTableNames(false);
            foreach (String tableName in tableNames)
            {
                if (tableName != null)
                {
                    // search case-sensitive
                    if (isStartingToken(tableName, tableAndColumnPath))
                    {
                        table = schema.getTableByName(tableName);
                        columnPath = tableAndColumnPath.Substring(tableName.Length);

                        if (columnPath[0] == '.')
                        {
                            columnPath = columnPath.Substring(1);
                            break;
                        }
                    }
                }
            }

            if (table == null)
            {
                String tableAndColumnPathInLowerCase = tableAndColumnPath.ToLower();
                foreach (String tableName in tableNames)
                {
                    if (tableName != null)
                    {
                        String tableNameInLowerCase = tableName.ToLower();
                        // search case-insensitive
                        if (isStartingToken(tableNameInLowerCase, tableAndColumnPathInLowerCase))
                        {
                            table = schema.getTableByName(tableName);
                            columnPath = tableAndColumnPath.Substring(tableName.Length);

                            if (columnPath[0] == '.')
                            {
                                columnPath = columnPath.Substring(1);
                                break;
                            }
                        }
                    }
                }
            }

            if (table == null && tableNames.Length == 1)
            {
                table = schema.getTables(false)[0];
            }

            if (table != null)
            {
                Column column = table.getColumnByName(columnPath);
                if (column != null)
                {
                    return column;
                }
            }

            return null;
        } // getColumn()

        ///**
        // * {@inheritDoc}
        // */
        //@Override
        public Table getTableByQualifiedLabel(String tableName)
        {
            if (tableName == null)
            {
                return null;
            }

            Schema   schema = null;
            String[] tokens = tokenizePath(tableName, 2);
            if (tokens != null)
            {
                schema = getSchemaByToken(tokens[0]);
                if (schema != null)
                {
                    Table table = schema.getTableByName(tokens[1]);
                    if (table != null)
                    {
                        return table;
                    }
                }
            }

            schema = null;
            String[] schemaNames = getSchemaNames();
            foreach (String schemaName_1 in schemaNames)
            {
                if (schemaName_1 == null)
                {
                    // there's an unnamed schema present.
                    schema = getSchemaByName(null);
                    if (schema != null)
                    {
                        Table table = schema.getTableByName(tableName);
                        return table;
                    }
                }
                else
                {
                    // case-sensitive search
                    if (isStartingToken(schemaName_1, tableName))
                    {
                        schema = getSchemaByName(schemaName_1);
                    }
                }
            }

            if (schema == null)
            {
                String tableNameInLowerCase = tableName.ToLower();
                foreach (String schemaName_2 in schemaNames)
                {
                    if (schemaName_2 != null)
                    {
                        // case-insensitive search
                        String schemaNameInLowerCase = schemaName_2.ToLower();
                        if (isStartingToken(schemaNameInLowerCase, tableNameInLowerCase))
                        {
                            schema = getSchemaByName(schemaName_2);
                        }
                    }
                }
            }

            if (schema == null)
            {
                schema = getDefaultSchema();
            }

            String tablePart  = tableName.ToLower();
            String schemaName = schema.getName();
            if (schemaName != null && isStartingToken(schemaName.ToLower(), tablePart))
            {
                tablePart = tablePart.Substring(schemaName.Length);
                if (tablePart.StartsWith("."))
                {
                    tablePart = tablePart.Substring(1);
                }
            }

            return schema.getTableByName(tablePart);
        } // getTableByQualifiedLabel()

        ///**
        // * Tokenizes a path for a table or a column.
        // * 
        // * @param tableName
        // * @param expectedParts
        // * @return
        // */
        private String[] tokenizePath(String path, int expectedParts)
        {
            List<String> tokens = new List<String>(expectedParts);

            bool inQuotes = false;
            StringBuilder currentToken = new StringBuilder();
            for (int i = 0; i < path.Length; i++)
            {
                char c = path[i];
                if (c == '.' && !inQuotes)
                {
                    // token finished
                    tokens.Add(currentToken.ToString());
                    currentToken.Length = 0; // setLength(0);

                    if (tokens.Count > expectedParts)
                    {
                        // unsuccesfull - return null
                        return null;
                    }
                }
                else if (c == '"')
                {
                    if (inQuotes)
                    {
                        if (i + 1 < path.Length && path[i + 1] != '.')
                        {
                            // unsuccesfull - return null
                            return null;
                        }
                    }
                    else
                    {
                        if (currentToken.Length > 0)
                        {
                            // unsuccesfull - return null
                            return null;
                        }
                    }
                    inQuotes = !inQuotes;
                }
                else
                {
                    currentToken.Append(c);
                }
            }

            if (currentToken.Length > 0)
            {
                tokens.Add(currentToken.ToString());
            }

            if (tokens.Count == expectedParts - 1)
            {
                // add a special-meaning "null" which will be interpreted as the
                // default schema (since the schema wasn't specified).
                tokens.Insert(0, null);
            }
            else if (tokens.Count != expectedParts)
            {
                return null;
            }

            return tokens.ToArray(); // (new String[tokens.Count]);
        } // tokenizePath()

        private Schema getSchemaByToken(String token)
        {
            if (token == null)
            {
                return getDefaultSchema();
            }
            try
            {
                return getSchemaByName(token);
            }
            catch (NRuntimeException e)
            {
                // swallow this exception - the attempt did not work and the null
                // will be treated.
                return null;
            }
        } // tokenizePath()

        private bool isStartingToken(String partName, String fullName)
        {
            if (fullName.StartsWith(partName))
            {
                int length = partName.Length;
                if (length == 0)
                {
                    return false;
                }
                if (fullName.Length > length)
                {
                    char nextChar = fullName[length];
                    if (isQualifiedPathDelim(nextChar))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected bool isQualifiedPathDelim(char c)
        {
            return c == '.' || c == '"';
        } // isQualifiedPathDelim()

        ///**
        // * Gets schema names from the non-abstract implementation. These schema
        // * names will be cached except if the {@link #refreshSchemas()} method is
        // * called.
        // * 
        // * @return an array of schema names.
        // */
        protected abstract String[] getSchemaNamesInternal();

        ///**
        // * Gets the name of the default schema.
        // * 
        // * @return the default schema name.
        // */
        public abstract String getDefaultSchemaName();

        ///**
        // * Gets a specific schema from the non-abstract implementation. This schema
        // * object will be cached except if the {@link #refreshSchemas()} method is
        // * called.
        // * 
        // * @param name
        // *            the name of the schema to get
        // * @return a schema object representing the named schema, or null if no such
        // *         schema exists.
        // */
        protected abstract Schema getSchemaByNameInternal(String name);
    } // AbstractDataContext class
} // org.apache.metamodel.query.builder namespace
