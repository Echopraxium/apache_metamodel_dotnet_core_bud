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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/util/SimpleTableDefParser.java
using System;
using System.Collections.Generic;
using org.apache.metamodel.j2n.collections;
using org.apache.metamodel.schema;

namespace org.apache.metamodel.core.util
{
    public class SimpleTableDefParser
    {
        /**
         * Parses an array of table definitions.
         * 
         * @param tableDefinitionsText
         * @return
         */
        public static SimpleTableDef[] parseTableDefs(String tableDefinitionsText)
        {
            if (tableDefinitionsText == null)
            {
                return null;
            }

            tableDefinitionsText = tableDefinitionsText.Replace("\n", "").Replace("\t", "").Replace("\r", "")
                                                       .Replace("  ", " ");

            List<SimpleTableDef> tableDefs = new List<SimpleTableDef>();
            String[] tableDefinitionTexts = tableDefinitionsText.Split(';');
            foreach (String tableDefinitionText in tableDefinitionTexts)
            {
                SimpleTableDef tableDef = parseTableDef(tableDefinitionText);
                if (tableDef != null)
                {
                    tableDefs.Add(tableDef);
                }
            }

            if (tableDefs.IsEmpty())
            {
                return null;
            }

            return tableDefs.ToArray(); // (new SimpleTableDef[tableDefs.size()]);
        }

        /**
         * Parses a single table definition
         * 
         * @param tableDefinitionText
         * @return
         */
        protected static SimpleTableDef parseTableDef(String tableDefinitionText)
        {
            if (tableDefinitionText == null || tableDefinitionText.Trim().IsEmpty())
            {
                return null;
            }

            int startColumnSection = tableDefinitionText.IndexOf("(");
            if (startColumnSection == -1)
            {
                throw new ArgumentException("Failed to parse table definition: " + tableDefinitionText
                                            + ". No start parenthesis found for column section.");
            }

            int endColumnSection = tableDefinitionText.IndexOf(")", startColumnSection);
            if (endColumnSection == -1)
            {
                throw new ArgumentException("Failed to parse table definition: " + tableDefinitionText
                        + ". No end parenthesis found for column section.");
            }

            String tableName = tableDefinitionText.Substring(0, startColumnSection).Trim();
            if (tableName.IsEmpty())
            {
                throw new ArgumentException("Failed to parse table definition: " + tableDefinitionText
                        + ". No table name found.");
            }

            String columnSection = tableDefinitionText.Substring(startColumnSection + 1, endColumnSection);

            String[]         columnDefinitionTexts = columnSection.Split(',');
            List<String>     columnNames           = new List<String>();
            List<ColumnType> columnTypes           = new List<ColumnType>();

            foreach (String columnDefinition_item in columnDefinitionTexts)
            {
                string columnDefinition = columnDefinition_item.Trim();
                if (!columnDefinition.IsEmpty())
                {
                    int separator = columnDefinition.LastIndexOf(" ");
                    String columnName = columnDefinition.Substring(0, separator).Trim();
                    String columnTypeString = columnDefinition.Substring(separator).Trim();
                    ColumnType columnType = ColumnTypeImpl.valueOf(columnTypeString);

                    columnNames.Add(columnName);
                    columnTypes.Add(columnType);
                }
            }
            return new SimpleTableDef(tableName, columnNames.ToArray(),
                                      columnTypes.ToArray()); // new ColumnType[columnTypes.size()]));
        } // parseTableDef()
    } // SimpleTableDefParser class
} // org.apache.metamodel.core.util
