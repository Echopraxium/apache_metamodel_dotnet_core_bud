﻿/**
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
using org.apache.metamodel.core;
using org.apache.metamodel.core.data;
using org.apache.metamodel.core.query;
using org.apache.metamodel.data;
using org.apache.metamodel.j2n.slf4j;
using org.apache.metamodel.query;
using org.apache.metamodel.schema;
using System;
using System.Collections.Generic;
using org.apache.metamodel.j2n.collections;
using org.apache.metamodel.core.schema;
using org.apache.metamodel.util;
using org.apache.metamodel.j2n.data.numbers;
using org.apache.metamodel.core.convert;

namespace org.apache.metamodel
{
    /**
     * Abstract DataContext for data sources that do not support SQL queries
     * natively.
     * 
     * Instead this superclass only requires that a subclass can materialize a
     * single table at a time. Then the query will be executed by post processing
     * the datasets client-side.
     */
    public abstract class QueryPostprocessDataContext : AbstractDataContext //  implements HasReadTypeConverters
    {
        private static readonly NLogger logger = NLoggerFactory.getLogger(typeof(QueryPostprocessDataContext).Name);

        public static readonly String INFORMATION_SCHEMA_NAME = "information_schema";
        private readonly Dictionary<Column, TypeConverter<object, object>> _converters;

        public QueryPostprocessDataContext() : base()
        {
            _converters = new Dictionary<Column, TypeConverter<object, object>>();
        } // constructor

        public DataSet executeQuery(Query query)
        {
            List<SelectItem>  selectItems        = query.getSelectClause().getItems();
            List<FromItem>    fromItems          = query.getFromClause().getItems();
            List<FilterItem>  whereItems         = query.getWhereClause().getItems();
            List<SelectItem>  whereSelectItems   = query.getWhereClause().getEvaluatedSelectItems();
            List<GroupByItem> groupByItems       = query.getGroupByClause().getItems();
            List<SelectItem>  groupBySelectItems = query.getGroupByClause().getEvaluatedSelectItems();
            List<SelectItem>  havingSelectItems  = query.getHavingClause().getEvaluatedSelectItems();
            List<SelectItem>  orderBySelectItems = query.getOrderByClause().getEvaluatedSelectItems();

            List<FilterItem>  havingItems  = query.getHavingClause().getItems();
            List<OrderByItem> orderByItems = query.getOrderByClause().getItems();

            int firstRow = (query.getFirstRow() == null ? 1  : query.getFirstRow());
            int maxRows =  (query.getMaxRows()  == null ? -1 : query.getMaxRows());

            if (maxRows == 0)
            {
                // no rows requested - no reason to do anything
                return new EmptyDataSet(selectItems);
            }

            // check certain common query types that can often be optimized by
            // subclasses
            bool singleFromItem = fromItems.Count == 1;
            bool noGrouping     = groupByItems.IsEmpty() && havingItems.IsEmpty();
            if (singleFromItem && noGrouping)
            {
                FromItem fromItem = query.getFromClause().getItem(0);
                Table table = fromItem.getTable();
                if (table != null)
                {
                    // check for SELECT COUNT(*) queries
                    if (selectItems.Count == 1)
                    {
                        SelectItem selectItem = query.getSelectClause().getItem(0);
                        if (SelectItem.isCountAllItem(selectItem))
                        {
                            bool functionApproximationAllowed = selectItem.isFunctionApproximationAllowed();
                            if (isMainSchemaTable(table))
                            {
                                logger.debug("Query is a COUNT query with {} where items. Trying executeCountQuery(...)",
                                             whereItems.Count);
                                NNumber count = executeCountQuery(table, whereItems, functionApproximationAllowed);
                                if (count == null)
                                {
                                    logger.debug(
                                            "DataContext did not return any count query results. Proceeding with manual counting.");
                                }
                                else
                                {
                                    List<Row>     data   = new List<Row>(1);
                                    DataSetHeader header = new SimpleDataSetHeader(new SelectItem[] { selectItem });
                                    data.Add(new DefaultRow(header, new Object[] { count }));
                                    return new InMemoryDataSet(header, data);
                                }
                            }
                        }
                    }

                    bool is_simple_select = isSimpleSelect(query.getSelectClause());
                    if (is_simple_select)
                    {
                        // check for lookup query by primary key
                        if (whereItems.Count == 1)
                        {
                            FilterItem whereItem = whereItems[0];
                            SelectItem selectItem = whereItem.getSelectItem();
                            if (!whereItem.isCompoundFilter() && selectItem != null && selectItem.getColumn() != null)
                            {
                                Column column = selectItem.getColumn();
                                if (column.isPrimaryKey() && OperatorType.EQUALS_TO.Equals(whereItem.getOperator()))
                                {
                                    logger.debug(
                                            "Query is a primary key lookup query. Trying executePrimaryKeyLookupQuery(...)");
                                    if (table != null)
                                    {
                                        if (isMainSchemaTable(table))
                                        {
                                            Object operand = whereItem.getOperand();
                                            Row row = executePrimaryKeyLookupQuery(table, selectItems, column, operand);
                                            if (row == null)
                                            {
                                                logger.debug(
                                                       "DataContext did not return any GET query results. Proceeding with manual lookup.");
                                            }
                                            else
                                            {
                                                DataSetHeader header = new SimpleDataSetHeader(selectItems);
                                                return new InMemoryDataSet(header, row);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        // check for simple queries with or without simple criteria
                        if (orderByItems.IsEmpty())
                        {
                                DataSet ds = null;

                                // no WHERE criteria set
                                if (whereItems.IsEmpty())
                                {
                                    ds = materializeTable(table, selectItems, firstRow, maxRows);
                                    return ds;
                                }

                                ds = materializeTable(table, selectItems, whereItems, firstRow, maxRows);
                                return ds;
                            }
                        }
                    }
                }

                // Creates a list for all select items that are needed to execute query
                // (some may only be used as part of a filter, but not shown in result)
                List<SelectItem> workSelectItems = CollectionUtils.concat(true, selectItems, whereSelectItems,
                        groupBySelectItems, havingSelectItems, orderBySelectItems);

                // Materialize the tables in the from clause
                DataSet[] fromDataSets = new DataSet[fromItems.Count];
                for (int i = 0; i < fromDataSets.Length; i++)
                {
                    FromItem fromItem = fromItems[i];
                    fromDataSets[i] = materializeFromItem(fromItem, workSelectItems);
                }

                // Execute the query using the raw data
            DataSet dataSet = null; // MetaModelHelper.getCarthesianProduct(fromDataSets, whereItems);

            // we can now exclude the select items imposed by the WHERE clause (and
            // should, to make the aggregation process faster)
            workSelectItems = CollectionUtils.concat(true, selectItems, groupBySelectItems, havingSelectItems,
                                                     orderBySelectItems);

            if (groupByItems.Count > 0)
            {
                dataSet = MetaModelHelper.getGrouped(workSelectItems, dataSet, groupByItems);
            }
            else
            {
                dataSet = MetaModelHelper.getAggregated(workSelectItems, dataSet);
            }
            dataSet = MetaModelHelper.getFiltered(dataSet, havingItems);

            if (query.getSelectClause().isDistinct())
            {
                dataSet = MetaModelHelper.getSelection(selectItems, dataSet);
                dataSet = MetaModelHelper.getDistinct(dataSet);
                dataSet = MetaModelHelper.getOrdered(dataSet, orderByItems);
            }
            else
            {
                dataSet = MetaModelHelper.getOrdered(dataSet, orderByItems);
                dataSet = MetaModelHelper.getSelection(selectItems, dataSet);
            }

            dataSet = MetaModelHelper.getPaged(dataSet, firstRow, maxRows);
            return dataSet;
        } // executeQuery()

        /**
         * Determines if all the select items are 'simple' meaning that they just
         * represent scans of values in columns.
         *
         * @param clause
         * @return
         */
        private bool isSimpleSelect(SelectClause clause)
        {
            if (clause.isDistinct())
            {
                return false;
            }
            foreach (SelectItem item in clause.getItems())
            {
               if (item.getAggregateFunction() != null || item.getExpression() != null)
               {
                   return false;
               }
           }
            return true;
        } // isSimpleSelect()

        /**
         * Executes a simple count query, if possible. This method is provided to
         * allow subclasses to optimize count queries since they are quite common
         * and often a datastore can retrieve the count using some specialized means
         * which is much more performant than counting all records manually.
         * 
         * @param table
         *            the table on which the count is requested.
         * @param whereItems
         *            a (sometimes empty) list of WHERE items.
         * @param functionApproximationAllowed
         *            whether approximation is allowed or not.
         * @return the count of the particular table, or null if not available.
         */
        protected NNumber executeCountQuery(Table table, List<FilterItem> whereItems, bool functionApproximationAllowed)
        {
            return null;
        } // executeCountQuery()

        /**
         * Executes a query which obtains a row by primary key (as defined by
         * {@link Column#isPrimaryKey()}). This method is provided to allow
         * subclasses to optimize lookup queries since they are quite common and
         * often a datastore can retrieve the row using some specialized means which
         * is much more performant than scanning all records manually.
         * 
         * @param table
         *            the table on which the lookup is requested.
         * @param selectItems
         *            the items to select from the lookup query.
         * @param primaryKeyColumn
         *            the column that is the primary key
         * @param keyValue
         *            the primary key value that is specified in the lookup query.
         * @return the row if the particular table, or null if not available.
         */
        protected Row executePrimaryKeyLookupQuery(Table table, List<SelectItem> selectItems, 
                                                   Column primaryKeyColumn, Object keyValue)
        {
            return null;
        } // executePrimaryKeyLookupQuery()

        protected DataSet materializeFromItem(FromItem fromItem, List<SelectItem> selectItems)
        {
            DataSet dataSet;
            JoinType joinType = fromItem.getJoin();
            if (fromItem.getTable() != null)
            {
                // We need to materialize a single table
                Table table = fromItem.getTable();
                List<SelectItem> selectItemsToMaterialize = new List<SelectItem>();

                foreach (SelectItem selectItem in selectItems)
                {
                    FromItem selectedFromItem = selectItem.getFromItem();
                    if (selectedFromItem != null)
                    {
                       if (selectedFromItem.equals(fromItem))
                       {
                           selectItemsToMaterialize.Add(selectItem.replaceFunction(null));
                       }
                   }
                   else
                   {
                      // the select item does not specify a specific
                      // from-item
                      Column selectedColumn = selectItem.getColumn();
                      if (selectedColumn != null)
                      {
                          // we assume that if the table matches, we will use the
                          // column
                          if (selectedColumn.getTable() != null && selectedColumn.getTable().Equals(table))
                          {
                              selectItemsToMaterialize.Add(selectItem.replaceFunction(null));
                          }
                      }
                  }
              }

              if (logger.isDebugEnabled())
              {
                  logger.debug("calling materializeTable(" + table.getName() + "," + selectItemsToMaterialize + ",1,-1");
              }

              // Dispatching to the concrete subclass of
              // QueryPostprocessDataContextStrategy
              dataSet = materializeTable(table, selectItemsToMaterialize, 1, -1);

          }
          else if (joinType != JoinType.None)
          {
              // We need to (recursively) materialize a joined FromItem
              if (fromItem.getLeftSide() == null || fromItem.getRightSide() == null)
              {
                  throw new ArgumentException("Joined FromItem requires both left and right side: " + fromItem);
              }
              DataSet[] fromItemDataSets = new DataSet[2];

              // materialize left side
              List<SelectItem> leftOn = NArrays.AsList(fromItem.getLeftOn());
              fromItemDataSets[0] = materializeFromItem(fromItem.getLeftSide(),
                      CollectionUtils.concat(true, selectItems, leftOn));

              // materialize right side
              List<SelectItem> rightOn = NArrays.AsList(fromItem.getRightOn());
              fromItemDataSets[1] = materializeFromItem(fromItem.getRightSide(),
                                                        CollectionUtils.concat(true, selectItems, rightOn));

              FilterItem[] onConditions = new FilterItem[leftOn.Count];
              for (int i = 0; i < onConditions.Length; i++)
              {
                  FilterItem whereItem = new FilterItem(leftOn[i], OperatorType.EQUALS_TO, rightOn[i]);
                  onConditions[i] = whereItem;
              }

              switch (joinType)
              {
                  case JoinType.INNER:
                      dataSet = MetaModelHelper.getCarthesianProduct(fromItemDataSets, onConditions);
                      break;
                  case JoinType.LEFT:
                      dataSet = MetaModelHelper.getLeftJoin(fromItemDataSets[0], fromItemDataSets[1], onConditions);
                      break;
                  case JoinType.RIGHT:
                      dataSet = MetaModelHelper.getRightJoin(fromItemDataSets[0], fromItemDataSets[1], onConditions);
                      break;
                  default:
                      throw new ArgumentException("FromItem type not supported: " + fromItem);
              }
          }
          else if (fromItem.getSubQuery() != null)
          {
              // We need to (recursively) materialize a subquery
              dataSet = executeQuery(fromItem.getSubQuery());
          }
          else
          {
              throw new ArgumentException("FromItem type not supported: " + fromItem);
          }
          if (dataSet == null)
          {
              throw new ArgumentException("FromItem was not succesfully materialized: " + fromItem);
          }
          return dataSet;
        } // materializeFromItem()

        protected DataSet materializeTable(Table table, List<SelectItem> selectItems,
                                           List<FilterItem> whereItems, int firstRow, int maxRows)
        {
            if (table == null)
            {
                throw new ArgumentException("Table cannot be null");
            }

            if (selectItems == null || selectItems.IsEmpty())
            {
                // add any column (typically this occurs because of COUNT(*)
                // queries)
                Column[] columns = table.getColumns();
                if (columns.Length == 0)
                {
                    logger.warn("Queried table has no columns: {}", table);
                }
                else
                {
                    selectItems.Add(new SelectItem(columns[0]));
                }
            }

            Schema schema = table.getSchema();
            String schemaName;
            if (schema == null)
            {
                schemaName = null;
            }
            else
            {
                schemaName = schema.getName();
            }

            DataSet dataSet;
            if (INFORMATION_SCHEMA_NAME.Equals(schemaName))
            {
                DataSet informationDataSet = materializeInformationSchemaTable
                                             (table, buildWorkingSelectItems(selectItems, whereItems));
                informationDataSet = MetaModelHelper.getFiltered(informationDataSet, whereItems);
                informationDataSet = MetaModelHelper.getSelection(selectItems, informationDataSet);
                informationDataSet = MetaModelHelper.getPaged(informationDataSet, firstRow, maxRows);
                dataSet = informationDataSet;
            }
            else
            {
                DataSet tableDataSet = materializeMainSchemaTable(table, selectItems, whereItems, firstRow, maxRows);

                // conversion is done at materialization time, since it enables
                // the refined types to be used also in eg. where clauses.
                dataSet = new ConvertedDataSetInterceptor(_converters).intercept(tableDataSet);
            }

            return dataSet;
        } // materializeTable()

        private List<SelectItem> buildWorkingSelectItems(List<SelectItem> selectItems, List<FilterItem> whereItems)
        {
            List<SelectItem> primarySelectItems = new List<SelectItem>(selectItems.Count);
            foreach (SelectItem selectItem in selectItems)
            {
                ScalarFunction scalarFunction = selectItem.getScalarFunction();
                if (scalarFunction == null || isScalarFunctionMaterialized(scalarFunction))
                {
                    primarySelectItems.Add(selectItem);
                }
                else
                {
                    SelectItem copySelectItem = selectItem.replaceFunction(null);
                    primarySelectItems.Add(copySelectItem);
                }
            }
            List<SelectItem> evaluatedSelectItems = MetaModelHelper.getEvaluatedSelectItems(whereItems);
            return CollectionUtils.concat(true, primarySelectItems, evaluatedSelectItems);
        } // buildWorkingSelectItems()

        /**
         * Determines if the subclass of this class can materialize
         * {@link SelectItem}s with the given {@link ScalarFunction}. Usually scalar
         * functions are applied by MetaModel on the client side, but when possible
         * they can also be handled by e.g.
         * {@link #materializeMainSchemaTable(Table, List, int, int)} and
         * {@link #materializeMainSchemaTable(Table, List, List, int, int)} in which
         * case MetaModel will not evaluate it client-side.
         * 
         * @param function
         * @return
         */
        protected bool isScalarFunctionMaterialized(ScalarFunction function)
        {
            return false;
        } // isScalarFunctionMaterialized()

        //@Deprecated
        protected DataSet materializeTable(Table table, List<SelectItem> selectItems, int firstRow, int maxRows)
        {
            List<FilterItem> empty_list = new List<FilterItem>();
            return materializeTable(table, selectItems, empty_list, firstRow, maxRows);
        } // materializeTable()

        protected bool isMainSchemaTable(Table table)
        {
            Schema schema = table.getSchema();
            if (INFORMATION_SCHEMA_NAME.Equals(schema.getName()))
            {
                return false;
            }
            else
            {
                return true;
            }
        } // isMainSchemaTable()

        protected override string[] getSchemaNamesInternal() // throws MetaModelException
        {
            String [] schemaNames = new String[2];
            schemaNames[0] = INFORMATION_SCHEMA_NAME;
            schemaNames[1] = getMainSchemaName();
            return schemaNames;
        } // getSchemaNamesInternal()

        public override String getDefaultSchemaName() // throws MetaModelException
        {
           return getMainSchemaName();
        } // getDefaultSchemaName()

        protected override Schema getSchemaByNameInternal(String name) // throws MetaModelException
        {
            string mainSchemaName = getMainSchemaName();
            if (name == null || name == "")
            {
                if (mainSchemaName == null)
                {
                    return getMainSchema();
                }
                return null;
            } 

            if (name.Equals(mainSchemaName, StringComparison.CurrentCultureIgnoreCase))
            {
                return getMainSchema();
            }
            else if (name.Equals(INFORMATION_SCHEMA_NAME))
            {
                return getInformationSchema();
            }

            logger.warn("Could not find matching schema of name '{}'. Main schema name is: '{}'. Returning null.",
                        name, mainSchemaName);
                return null;
        } // getSchemaByNameInternal()

        private Schema getInformationSchema()
        {
            // Create schema
            MutableSchema informationSchema = new MutableSchema(INFORMATION_SCHEMA_NAME);
            MutableTable tablesTable        = new MutableTable("tables", TableType.TABLE, informationSchema);
            MutableTable columnsTable       = new MutableTable("columns", TableType.TABLE, informationSchema);
            MutableTable relationshipsTable = new MutableTable("relationships", TableType.TABLE, informationSchema);
            informationSchema.addTable(tablesTable).addTable(columnsTable).addTable(relationshipsTable);

            // Create "tables" table: name, type, num_columns, remarks
            tablesTable.addColumn(new MutableColumn("name", ColumnTypeConstants.VARCHAR, tablesTable, 0, false));
            tablesTable.addColumn(new MutableColumn("type", ColumnTypeConstants.VARCHAR, tablesTable, 1, true));
            tablesTable.addColumn(new MutableColumn("num_columns", ColumnTypeConstants.INTEGER, tablesTable, 2, true));
            tablesTable.addColumn(new MutableColumn("remarks", ColumnTypeConstants.VARCHAR, tablesTable, 3, true));

            // Create "columns" table: name, type, native_type, size, nullable,
            // indexed, table, remarks
            columnsTable.addColumn(new MutableColumn("name", ColumnTypeConstants.VARCHAR, columnsTable, 0, false));
            columnsTable.addColumn(new MutableColumn("type", ColumnTypeConstants.VARCHAR, columnsTable, 1, true));
            columnsTable.addColumn(new MutableColumn("native_type", ColumnTypeConstants.VARCHAR, columnsTable, 2, true));
            columnsTable.addColumn(new MutableColumn("size", ColumnTypeConstants.INTEGER, columnsTable, 3, true));
            columnsTable.addColumn(new MutableColumn("nullable", ColumnTypeConstants.BOOLEAN, columnsTable, 4, true));
            columnsTable.addColumn(new MutableColumn("indexed", ColumnTypeConstants.BOOLEAN, columnsTable, 5, true));
            columnsTable.addColumn(new MutableColumn("table", ColumnTypeConstants.VARCHAR, columnsTable, 6, false));
            columnsTable.addColumn(new MutableColumn("remarks", ColumnTypeConstants.VARCHAR, columnsTable, 7, true));

            // Create "relationships" table: primary_table, primary_column,
            // foreign_table, foreign_column
            relationshipsTable
                    .addColumn(new MutableColumn("primary_table", ColumnTypeConstants.VARCHAR, relationshipsTable, 0, false));
            relationshipsTable
                    .addColumn(new MutableColumn("primary_column", ColumnTypeConstants.VARCHAR, relationshipsTable, 1, false));
            relationshipsTable
                    .addColumn(new MutableColumn("foreign_table", ColumnTypeConstants.VARCHAR, relationshipsTable, 2, false));
            relationshipsTable
                    .addColumn(new MutableColumn("foreign_column", ColumnTypeConstants.VARCHAR, relationshipsTable, 3, false));

            MutableRelationship.createRelationship(tablesTable.getColumnByName("name"),
                    columnsTable.getColumnByName("table"));
            MutableRelationship.createRelationship(tablesTable.getColumnByName("name"),
                    relationshipsTable.getColumnByName("primary_table"));
            MutableRelationship.createRelationship(tablesTable.getColumnByName("name"),
                    relationshipsTable.getColumnByName("foreign_table"));
            MutableRelationship.createRelationship(columnsTable.getColumnByName("name"),
                    relationshipsTable.getColumnByName("primary_column"));
            MutableRelationship.createRelationship(columnsTable.getColumnByName("name"),
                    relationshipsTable.getColumnByName("foreign_column"));

            return informationSchema;
        } // getInformationSchema()

        public virtual DataSet materializeInformationSchemaTable(Table table, List<SelectItem> selectItems)
        {
            String              tableName         = table.getName();
            SelectItem[]        columnSelectItems = MetaModelHelper.createSelectItems(table.getColumns());
            SimpleDataSetHeader header            = new SimpleDataSetHeader(columnSelectItems);

            Table[] tables = getDefaultSchema().getTables(false);
            List<Row> data = new List<Row>();
            if ("tables".Equals(tableName))
            {
                // "tables" columns: name, type, num_columns, remarks
                foreach (Table t in tables)
                {
                    String typeString = null;
                    if (t.GetType() != null)
                    {
                        typeString = t.getType().ToString();
                    }
                    data.Add(new DefaultRow(header,
                             new Object[] { t.getName(), typeString, t.getColumnCount(), t.getRemarks() }));
                }
            }
            else if ("columns".Equals(tableName))
            {
                // "columns" columns: name, type, native_type, size, nullable,
                // indexed, table, remarks
                foreach (Table t in tables)
                {
                    foreach (Column c in t.getColumns())
                    {
                        String typeString = null;
                        if (t.GetType() != null)
                        {
                            typeString = c.getType().ToString();
                        }
                        data.Add(new DefaultRow(header, new Object[] { c.getName(), typeString, c.getNativeType(),
                                 c.getColumnSize(), c.isNullable(), c.isIndexed(), t.getName(), c.getRemarks() }));
                    }
                }
            }
            else if ("relationships".Equals(tableName))
            {
                // "relationships" columns: primary_table, primary_column,
                // foreign_table, foreign_column
                foreach (Relationship r in getDefaultSchema().getRelationships())
                {
                    Column[] primaryColumns = r.getPrimaryColumns();
                    Column[] foreignColumns = r.getForeignColumns();
                    Table pTable = r.getPrimaryTable();
                    Table fTable = r.getForeignTable();
                    for (int i = 0; i < primaryColumns.Length; i++)
                    {
                        Column pColumn = primaryColumns[i];
                        Column fColumn = foreignColumns[i];
                        data.Add(new DefaultRow(header,
                                new Object[] { pTable.getName(), pColumn.getName(), fTable.getName(), fColumn.getName() }));
                    }
                }
            }
            else
            {
                throw new ArgumentException("Cannot materialize non information_schema table: " + table);
            }

            DataSet dataSet;
            if (data.IsEmpty())
            {
                dataSet = new EmptyDataSet(selectItems);
            }
            else
            {
                dataSet = new InMemoryDataSet(header, data);
            }

            // Handle column subset
            DataSet selectionDataSet = MetaModelHelper.getSelection(selectItems, dataSet);
            dataSet = selectionDataSet;

            return dataSet;
        } // materializeInformationSchemaTable()

        /**
         * 
         * @return
         * 
         * @deprecated use {@link #getDefaultSchema()} instead
         */
        //@Deprecated
        protected Schema getMainSchemaInternal()
        {
            return getDefaultSchema();
        }

        /**
         * Adds a {@link TypeConverter} to this DataContext's query engine (Query
         * Postprocessor) for read operations. Note that this method should NOT be
         * invoked directly by consuming code. Rather use
         * {@link Converters#addTypeConverter(DataContext, Column, TypeConverter)}
         * to ensure conversion on both reads and writes.
         */
        public void addConverter(Column column, TypeConverter<object, object> converter)
        {
            _converters.Add(column, converter);
        }

        /**
         * @return the main schema that subclasses of this class produce
         */
        public abstract Schema getMainSchema(); // throws MetaModelException;

        /**
         * @return the name of the main schema that subclasses of this class produce
         */
        public abstract String getMainSchemaName(); // throws MetaModelException;

        /**
         * Execute a simple one-table query against a table in the main schema of
         * the subclasses of this class. This default implementation will delegate
         * to {@link #materializeMainSchemaTable(Table, List, int, int)} and apply
         * WHERE item filtering afterwards.
         * 
         * @param table
         * @param selectItems
         * @param whereItems
         * @param firstRow
         * @param maxRows
         * @return
         */
        public virtual DataSet materializeMainSchemaTable(Table table, List<SelectItem> selectItems, List<FilterItem> whereItems,
                                                          int firstRow, int maxRows)
        {
            List<SelectItem> workingSelectItems = buildWorkingSelectItems(selectItems, whereItems);
            DataSet dataSet;
            if (whereItems.IsEmpty())
            {
                // paging is pushed down to materializeMainSchemaTable
                dataSet = materializeMainSchemaTable(table, workingSelectItems, firstRow, maxRows);
                dataSet = MetaModelHelper.getSelection(selectItems, dataSet);
            }
            else
            {
                // do not push down paging, first we have to apply filtering
                dataSet = materializeMainSchemaTable(table, workingSelectItems, 1, -1);
                dataSet = MetaModelHelper.getFiltered(dataSet, whereItems);
                dataSet = MetaModelHelper.getPaged(dataSet, firstRow, maxRows);
                dataSet = MetaModelHelper.getSelection(selectItems, dataSet);
            }
            return dataSet;
        } // materializeMainSchemaTable()

        /**
         * Executes a simple one-table query against a table in the main schema of
         * the subclasses of this class. This default implementation will delegate
         * to {@link #materializeMainSchemaTable(Table, Column[], int, int)}.
         * 
         * @param table
         * @param selectItems
         * @param firstRow
         * @param maxRows
         * @return
         */
        public virtual DataSet materializeMainSchemaTable(Table table, List<SelectItem> selectItems, int firstRow, int maxRows)
        {
            Column[] columns = new Column[selectItems.Count];
            for (int i = 0; i < columns.Length; i++)
            {
                columns[i] = selectItems[i].getColumn();
            }
            DataSet dataSet = materializeMainSchemaTable(table, columns, firstRow, maxRows);

            dataSet = MetaModelHelper.getSelection(selectItems, dataSet);

            return dataSet;
        } // materializeMainSchemaTable()

        /**
         * Executes a simple one-table query against a table in the main schema of
         * the subclasses of this class. This default implementation will delegate
         * to {@link #materializeMainSchemaTable(Table, Column[], int)} and apply a
         * {@link FirstRowDataSet} if necessary.
         * 
         * @param table
         * @param columns
         * @param firstRow
         * @param maxRows
         * @return
         */
        public virtual DataSet materializeMainSchemaTable(Table table, Column[] columns, int firstRow, int maxRows)
        {
            int rowsToMaterialize;
            if (firstRow == 1)
            {
                rowsToMaterialize = maxRows;
            }
            else
            {
                rowsToMaterialize = maxRows + (firstRow - 1);
            }
            DataSet dataSet = materializeMainSchemaTable(table, columns, rowsToMaterialize);
            if (firstRow > 1)
            {
                dataSet = new FirstRowDataSet(dataSet, firstRow);
            }
            return dataSet;
        } // materializeMainSchemaTable()

        /**
         * Executes a simple one-table query against a table in the main schema of
         * the subclasses of this class.
         * 
         * @param table
         *            the table to query
         * @param columns
         *            the columns of the table to query
         * @param maxRows
         *            the maximum amount of rows needed or -1 if all rows are
         *            wanted.
         * @return a dataset with the raw table/column content.
         */
        public abstract DataSet materializeMainSchemaTable(Table table, Column[] columns, int maxRows);
    } // QueryPostprocessDataContext class
} // org.apache.metamodel namespace
