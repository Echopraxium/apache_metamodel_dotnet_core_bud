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
using org.apache.metamodel;
using org.apache.metamodel.core.data;
using org.apache.metamodel.core.query;
using org.apache.metamodel.core.query.parser;
using org.apache.metamodel.data;
using org.apache.metamodel.j2n.collections;
using org.apache.metamodel.j2n.slf4j;
using org.apache.metamodel.query;
using org.apache.metamodel.schema;
using org.apache.metamodel.util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using amm_schema = org.apache.metamodel.schema;
using amm_util   = org.apache.metamodel.util;


//import java.util.*;
//import java.util.Map.Entry;
//import java.util.stream.Collectors;

//import org.apache.metamodel.data.CachingDataSetHeader;
//import org.apache.metamodel.data.DataSet;
//import org.apache.metamodel.data.DataSetHeader;
//import org.apache.metamodel.data.DefaultRow;
//import org.apache.metamodel.data.EmptyDataSet;
//import org.apache.metamodel.data.FilteredDataSet;
//import org.apache.metamodel.data.FirstRowDataSet;
//import org.apache.metamodel.data.IRowFilter;
//import org.apache.metamodel.data.InMemoryDataSet;
//import org.apache.metamodel.data.MaxRowsDataSet;
//import org.apache.metamodel.data.Row;
//import org.apache.metamodel.data.ScalarFunctionDataSet;
//import org.apache.metamodel.data.SimpleDataSetHeader;
//import org.apache.metamodel.data.SubSelectionDataSet;
//import org.apache.metamodel.query.FilterItem;
//import org.apache.metamodel.query.FromItem;
//import org.apache.metamodel.query.GroupByItem;
//import org.apache.metamodel.query.OrderByItem;
//import org.apache.metamodel.query.Query;
//import org.apache.metamodel.query.ScalarFunction;
//import org.apache.metamodel.query.SelectItem;
//import org.apache.metamodel.query.parser.QueryParser;
//import org.apache.metamodel.schema.Column;
//import org.apache.metamodel.schema.ColumnType;
//import org.apache.metamodel.schema.Schema;
//import org.apache.metamodel.schema.SuperColumnType;
//import org.apache.metamodel.schema.Table;
//import org.apache.metamodel.util.AggregateBuilder;
//import org.apache.metamodel.util.CollectionUtils;
//import org.apache.metamodel.util.ObjectComparator;
//import org.slf4j.Logger;
//import org.slf4j.LoggerFactory;

namespace org.apache.metamodel.core
{
    /**
     * This class contains various helper functionality to common tasks in
     * MetaModel, eg.:
     * 
     * <ul>
     * <li>Easy-access for traversing common schema items</li>
     * <li>Manipulate data in memory. These methods are primarily used to enable
     * queries for non-queryable data sources like CSV files and spreadsheets.</li>
     * <li>Query rewriting, traversing and manipulation.</li>
     * </ul>
     * 
     * The class is mainly intended for internal use within the framework
     * operations, but is kept stable, so it can also be used by framework users.
     */
    public sealed class MetaModelHelper
    {
            private readonly static NLogger logger = NLoggerFactory.getLogger(typeof(MetaModelHelper).Name);

            private MetaModelHelper()
            {
                // Prevent instantiation
            }

            /**
             * Creates an array of tables where all occurences of tables in the provided
             * list of tables and columns are included
             */
            public static Table[] getTables(IList<Table> tableList, IEnumerable<Column> columnList)
            {
                HashSet<Table> set = new HashSet<Table>();
                foreach (Table t in set)
                {
                    tableList.Add(t);
                }

                foreach (Column column in columnList)
                {
                    set.Add(column.getTable());
                }
                Table[] values = new Table[set.Count];
                set.CopyTo(values);
                return values;
            } // getTables()

            /**
             * Determines if a schema is an information schema
             * 
             * @param schema
             * @return
             */
            public static bool isInformationSchema(Schema schema)
            {
                String name = schema.getName();
                return isInformationSchema(name);
            } // isInformationSchema()

            /**
             * Determines if a schema name is the name of an information schema
             * 
             * @param name
             * @return
             */
            public static bool isInformationSchema(String name)
            {
                if (name == null)
                {
                    return false;
                }
                return QueryPostprocessDataContext.INFORMATION_SCHEMA_NAME.Equals(name.ToLower());
            } // isInformationSchema()

            /**
             * Converts a list of columns to a corresponding array of tables
             * 
             * @param columns
             *            the columns that the tables will be extracted from
             * @return an array containing the tables of the provided columns.
             */
            public static Table[] getTables(IEnumerable<Column> columns)
            {
                List<Table> result = new List<Table>();
                foreach (Column column in columns)
                {
                    Table table = column.getTable();
                    if (!result.Contains(table))
                    {
                        result.Add(table);
                    }
                }
                Table[] values = result.ToArray();
                return values;
            } // getTables()

            /**
             * Creates a subset array of columns, where only columns that are contained
             * within the specified table are included.
             * 
             * @param table
             * @param columns
             * @return an array containing the columns that exist in the table
             */
            public static Column[] getTableColumns(Table table, IEnumerable<Column> columns)
            {
                if (table == null)
                {
                    return new Column[0];
                }
                List<Column> result = new List<Column>();
                foreach (Column column in columns)
                {
                    bool sameTable = table.Equals(column.getTable());
                    if (sameTable)
                    {
                        result.Add(column);
                    }
                }
                Column[] values = result.ToArray();
                return values;
            } // getTableColumns()

            public static List<Row> readDataSetFull(DataSet dataSet)
            {
                List<Row> result;
                if (dataSet is InMemoryDataSet)
                {
                    // if dataset is an in memory dataset we have a shortcut to avoid
                    // creating a new list
                    result = ((InMemoryDataSet)dataSet).getRows();
                }
                else
                {
                    result = new List<Row>();
                    while (dataSet.next())
                    {
                        result.Add(dataSet.getRow());
                    }
                }
                dataSet.close();
                return result;
            }

            public static DataSet getDistinct(DataSet dataSet)
            {
                SelectItem[] selectItems = dataSet.getSelectItems();
                GroupByItem[] groupByItems = new GroupByItem[selectItems.Length];
                for (int i = 0; i < groupByItems.Length; i++)
                {
                    groupByItems[i] = new GroupByItem(selectItems[i]);
                }
                return getGrouped(NArrays.AsList(selectItems), dataSet, groupByItems);
            } // getDistinct()

            public static Table[] getTables(Column[] columns)
            {
                return getTables(NArrays.AsList<Column>(columns));
            } // getTables()

            public static DataSet getPaged(DataSet dataSet, int firstRow, int maxRows)
            {
                if (firstRow > 1)
                {
                    dataSet = new FirstRowDataSet(dataSet, firstRow);
                }
                if (maxRows != -1)
                {
                    dataSet = new MaxRowsDataSet(dataSet, maxRows);
                }
                return dataSet;
            } // getPaged()

            public static List<SelectItem> getEvaluatedSelectItems(List<FilterItem> items)
            {
                List<SelectItem> result = new List<SelectItem>();
                foreach (FilterItem item in items)
                {
                    addEvaluatedSelectItems(result, item);
                }
                return result;
            }

            private static void addEvaluatedSelectItems(List<SelectItem> result, FilterItem item)
            {
                FilterItem[] orItems = item.getChildItems();
                if (orItems != null)
                {
                    foreach (FilterItem filterItem in orItems)
                    {
                        addEvaluatedSelectItems(result, filterItem);
                    }
                }

                SelectItem selectItem = item.getSelectItem();
                if (selectItem != null && !result.Contains(selectItem))
                {
                    result.Add(selectItem);
                }

                Object operand = item.getOperand();
                if (operand != null && operand is SelectItem && !result.Contains((SelectItem)operand))
                {
                    result.Add((SelectItem)operand);
                }
            } // addEvaluatedSelectItems()

            private class _getColumnsByType_Predicate_impl : amm_util.Predicate<Column>
            {
                private ColumnType _superColumnType;
                public _getColumnsByType_Predicate_impl(ColumnType superColumnType_arg)
                {
                    _superColumnType = superColumnType_arg;
                }

                public bool eval(Column arg)
                {
                    return arg.getType().getSuperType() == _superColumnType;
                }
            } // _getColumnsByType_Predicate_impl class

            public static Column[] getColumnsByType(Column[] columns, ColumnType columnType)
            {
                // column => { return column.getType() == columnType; }
                return CollectionUtils.filter(columns, new _getColumnsByType_Predicate_impl(columnType)).ToArray(); // new Column[0]);
            } // getColumnsByType()

            public static DataSet getOrdered(DataSet dataSet, List<OrderByItem> orderByItems)
            {
                return getOrdered(dataSet, orderByItems.ToArray()); //(new OrderByItem[orderByItems.Count]));
            } // getOrdered()

            /**
             * This method returns the select item of the given alias name.
             * 
             * @param query
             * @return
             */
            public static SelectItem getSelectItemByAlias(Query query, String alias)
            {
                List<SelectItem> selectItems = query.getSelectClause().getItems();
                foreach (SelectItem selectItem in selectItems)
                {
                    if (selectItem.getAlias() != null && selectItem.getAlias().Equals(alias))
                    {
                        return selectItem;
                    }
                }
                return null;
            } // getSelectItemByAlias()

            public class _Column_Predicate_BySuperType_Impl_ : amm_util.Predicate<Column>
            {
                private SuperColumnType _superColumnType;
                public _Column_Predicate_BySuperType_Impl_(SuperColumnType superColumnType_arg)
                {
                    _superColumnType = superColumnType_arg;
                } // constructor

                public bool eval(Column arg)
                {
                    return arg.getType().getSuperType() == _superColumnType;
                }
            } // _Column_Predicate_BySuperType_Impl_ class

            public static Column[] getColumnsBySuperType(Column[] columns, SuperColumnType superColumnType)
            {
                return CollectionUtils.filter<Column, Column>(columns, new _Column_Predicate_BySuperType_Impl_(superColumnType))
                                                              .ToArray(); // ToArray<Column>(new Column[0]);
            } // getColumnsBySuperType()

            /**
             * Performs a right join (aka right outer join) operation on two datasets.
             * 
             * @param ds1
             *            the left dataset
             * @param ds2
             *            the right dataset
             * @param onConditions
             *            the conditions to join by
             * @return the right joined result dataset
             */
            public static DataSet getRightJoin(DataSet ds1, DataSet ds2, FilterItem[] onConditions)
            {
                SelectItem[] ds1selects = ds1.getSelectItems();
                SelectItem[] ds2selects = ds2.getSelectItems();
                SelectItem[] leftOrderedSelects = new SelectItem[ds1selects.Length + ds2selects.Length];
                Array.Copy(ds1selects, 0, leftOrderedSelects, 0, ds1selects.Length);
                Array.Copy(ds2selects, 0, leftOrderedSelects, ds1selects.Length, ds2selects.Length);

                // We will reuse the left join algorithm (but switch the datasets
                // around)
                DataSet dataSet = getLeftJoin(ds2, ds1, onConditions);

                dataSet = getSelection(leftOrderedSelects, dataSet);
                return dataSet;
            } // getRightJoin()

            public static SelectItem[] createSelectItems(params Column[] columns)
            {
                SelectItem[] items = new SelectItem[columns.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = new SelectItem(columns[i]);
                }
                return items;
            } // createSelectItems()

            public static Query parseQuery(DataContext dc, String queryString)
            {
                QueryParser parser = new QueryParser(dc, queryString);
                return parser.parse();
            }

            /**
             * Examines a query and extracts an array of FromItem's that refer
             * (directly) to tables (hence Joined FromItems and SubQuery FromItems are
             * traversed but not included).
             * 
             * @param q
             *            the query to examine
             * @return an array of FromItem's that refer directly to tables
             */
            public static FromItem[] getTableFromItems(Query q)
            {
                List<FromItem> result = new List<FromItem>();
                List<FromItem> items = q.getFromClause().getItems();
                foreach (FromItem item in items)
                {
                    result.AddRange(getTableFromItems(item));
                }
                return result.ToArray(); // (new FromItem[result.Count]);
            } // getTableFromItems()

            public static List<FromItem> getTableFromItems(FromItem item)
            {
                List<FromItem> result = new List<FromItem>();
                if (item.getTable() != null)
                {
                    result.Add(item);
                }
                else if (item.getSubQuery() != null)
                {
                    FromItem[] sqItems = getTableFromItems(item.getSubQuery());
                    for (int i = 0; i < sqItems.Length; i++)
                    {
                        result.Add(sqItems[i]);
                    }
                }
                else if (item.getJoin() != JoinType.None)
                {
                    FromItem leftSide = item.getLeftSide();
                    result.AddRange(getTableFromItems(leftSide));

                    FromItem rightSide = item.getRightSide();
                    result.AddRange(getTableFromItems(rightSide));
                }
                else
                {
                    throw new InvalidOperationException("FromItem was neither of Table type, SubQuery type or Join type: " + item);
                }
                return result;
            } // getTableFromItems()

            /**
             * Executes a single row query, like "SELECT COUNT(*), MAX(SOME_COLUMN) FROM
             * MY_TABLE" or similar.
             * 
             * @param dataContext
             *            the DataContext object to use for executing the query
             * @param query
             *            the query to execute
             * @return a row object representing the single row returned from the query
             * @throws MetaModelException
             *             if less or more than one Row is returned from the query
             */
            public static Row executeSingleRowQuery(DataContext dataContext, Query query) // throws MetaModelException
            {
                DataSet dataSet = dataContext.executeQuery(query);
                bool next = dataSet.next();
                if (!next)
                {
                    throw new MetaModelException("No rows returned from query: " + query);
                }
                Row row = dataSet.getRow();
                next = dataSet.next();
                if (next)
                {
                    throw new MetaModelException("More than one row returned from query: " + query);
                }
                dataSet.close();
                return row;
            } // executeSingleRowQuery()

            /**
             * Creates a subset array of columns, where only columns that are contained
             * within the specified table are included.
             * 
             * @param table
             * @param columns
             * @return an array containing the columns that exist in the table
             */
            public static Column[] getTableColumns(Table table, Column[] columns)
            {
                return getTableColumns(table, NArrays.AsList(columns));
            } // getTableColumns()

            #region -------- getCarthesianProduct() --------
            public static DataSet getCarthesianProduct(params DataSet[] fromDataSets)
            {
                return getCarthesianProduct(fromDataSets, new FilterItem[0]);
            } // getCarthesianProduct()

            public static DataSet getCarthesianProduct(DataSet[] fromDataSets, params FilterItem[] filterItems)
            {
                return getCarthesianProduct(fromDataSets,filterItems.toList<FilterItem>());
            } // getCarthesianProduct()

            public static DataSet getCarthesianProduct(DataSet[] fromDataSets, IEnumerable<FilterItem> whereItems)
            {
                Debug.Assert(fromDataSets.Length > 0);
                // First check if carthesian product is even nescesary
                if (fromDataSets.Length == 1)
                {
                    return getFiltered(fromDataSets[0], whereItems);
                }
                // do a nested loop join, no matter what
                IEnumerator<DataSet> dsIter = NArrays.AsList(fromDataSets).GetEnumerator();

                DataSet joined = dsIter.Current;

                while (dsIter.MoveNext())
                {
                    joined = nestedLoopJoin(dsIter.Current, joined, (whereItems));
                }

                return joined;
            } // getCarthesianProduct()
            #endregion getCarthesianProduct()

            /**
             * Executes a simple nested loop join. The innerLoopDs will be copied in an
             * in-memory dataset.
             *
             */
            public static InMemoryDataSet nestedLoopJoin(DataSet innerLoopDs, DataSet outerLoopDs, IEnumerable<FilterItem> filtersIterable)
            {
                List<FilterItem> filters = new List<FilterItem>();
                foreach (FilterItem fi in filtersIterable)
                {
                    filters.Add(fi);
                }
                List<Row> innerRows = innerLoopDs.toRows();

                List<SelectItem> allItems = new List<SelectItem>(NArrays.AsList<SelectItem>(outerLoopDs.getSelectItems()));
                allItems.AddRange(NArrays.AsList(innerLoopDs.getSelectItems()));

                HashSet<FilterItem> applicable_filters = applicableFilters(filters, allItems);

                DataSetHeader jointHeader = new CachingDataSetHeader(allItems);

                List<Row> resultRows = new List<Row>();
                foreach (Row outerRow in outerLoopDs)
                {
                    foreach (Row innerRow in innerRows)
                    {

                        Object[] joinedRowObjects = new Object[outerRow.getValues().Length + innerRow.getValues().Length];

                        Array.Copy(outerRow.getValues(), 0, joinedRowObjects, 0, outerRow.getValues().Length);
                        Array.Copy(innerRow.getValues(), 0, joinedRowObjects, outerRow.getValues().Length,
                                   innerRow.getValues().Length);

                        Row joinedRow = new DefaultRow(jointHeader, joinedRowObjects);
                        IEnumerable<FilterItem> selected_items = applicable_filters.Where(fi => isJoinedRowAccepted(fi, joinedRow));

                        if (applicable_filters.IsEmpty() || (selected_items != null  && selected_items.Count<FilterItem>() != 0))
                        {
                            resultRows.Add(joinedRow);
                        }
                    }
                }

                return new InMemoryDataSet(jointHeader, resultRows);
            } // nestedLoopJoin()

            private static bool isJoinedRowAccepted(FilterItem fi, Row joinedRow)
            {
                return fi.accept(joinedRow);
            }

            private static bool selectFilterItems(HashSet<SelectItem> items, FilterItem fi)
            {
                IList<SelectItem> fiSelectItems = new List<SelectItem>();
                fiSelectItems.Add(fi.getSelectItem());
                Object operand = fi.getOperand();
                if (operand is SelectItem)
                {
                    fiSelectItems.Add((SelectItem)operand);
                }

                return items.ContainsAll(fiSelectItems);
            } // selectFilterItems()

            /**
             * Filters the FilterItems such that only the FilterItems are returned,
             * which contain SelectItems that are contained in selectItemList
             * 
             * @param filters
             * @param selectItemList
             * @return
             */
            //[J2N]: OLd code and Not ported
            private static HashSet<FilterItem> applicableFilters(IList<FilterItem> filters, IList<SelectItem> selectItemList)
            {
                HashSet<FilterItem> result = new HashSet<FilterItem>();
                HashSet<SelectItem> items  = new HashSet<SelectItem>(selectItemList);
                //    List<FilterItem>    selected_items = filters.toList<FilterItem>();

                //    return selected_items.filter<SelectItem, SelectItem>(fi => fi).collect(Collectors.toSet());
                return result;
            } // applicableFilters()

            private class _Filtered_Func_impl_ : NFunc<FilterItem, object>
            {
                public object eval(FilterItem arg)
                {
                    return arg;
                }
            } // _Filtered_Func_impl_ class

            public static DataSet getFiltered(DataSet dataSet, IEnumerable<FilterItem> filterItems)
            {
                NFunc<FilterItem, object> func = new _Filtered_Func_impl_();
                List<object> filters = CollectionUtils.map<FilterItem, FilterItem, Object>(filterItems, func);

                if (filters.IsEmpty())
                {
                    return dataSet;
                }

                return new FilteredDataSet(dataSet, (IRowFilter[]) filters.ToArray()); // (new IRowFilter[filters.Count]));
            } // getFiltered()

            public static DataSet getFiltered(DataSet dataSet, params FilterItem[] filterItems)
            {
                return getFiltered(dataSet, NArrays.AsList(filterItems));
            }

            public static DataSet getSelection(List<SelectItem> selectItems, DataSet dataSet)
            {
                List<SelectItem> dataSetSelectItems = NArrays.AsList(dataSet.getSelectItems());

                // check if the selection is already the same
                if (selectItems.Equals(dataSetSelectItems))
                {
                    // return the DataSet unmodified
                    return dataSet;
                }

                List<SelectItem> scalarFunctionSelectItemsToEvaluate = new List<SelectItem>();

                foreach (SelectItem selectItem in selectItems)
                {
                    if (selectItem.getScalarFunction() != null)
                    {
                        if (!dataSetSelectItems.Contains(selectItem) && dataSetSelectItems.Contains(selectItem.replaceFunction(
                                null)))
                        {
                            scalarFunctionSelectItemsToEvaluate.Add(selectItem);
                        }
                    }
                }

                if (scalarFunctionSelectItemsToEvaluate.IsEmpty())
                {
                    return new SubSelectionDataSet(selectItems, dataSet);
                }

                ScalarFunctionDataSet scalaFunctionDataSet = new ScalarFunctionDataSet(
                                                                     scalarFunctionSelectItemsToEvaluate, dataSet);
                return new SubSelectionDataSet(selectItems, scalaFunctionDataSet);
            } // getSelection()

            public static DataSet getSelection(SelectItem[] selectItems, DataSet dataSet)
            {
                return getSelection(NArrays.AsList(selectItems), dataSet);
            }

            public static DataSet getGrouped(List<SelectItem> selectItems, DataSet dataSet, IList<GroupByItem> groupByItems)
            {
                return getGrouped(selectItems, dataSet, groupByItems); // (new GroupByItem[groupByItems.Count]));
            } // getGrouped()

            public static DataSet getGrouped(List<SelectItem> selectItems, DataSet dataSet, GroupByItem[] groupByItems)
            {
                DataSet result = dataSet;
                if (groupByItems != null && groupByItems.Length > 0)
                {
                    Dictionary<Row, Dictionary<SelectItem, List<Object>>> uniqueRows = new Dictionary<Row, Dictionary<SelectItem, List<Object>>>();

                    SelectItem[] groupBySelects = new SelectItem[groupByItems.Length];
                    for (int i = 0; i < groupBySelects.Length; i++)
                    {
                        groupBySelects[i] = groupByItems[i].getSelectItem();
                    }
                    DataSetHeader groupByHeader = new CachingDataSetHeader(groupBySelects);

                    // Creates a list of SelectItems that have aggregate functions
                    List<SelectItem> functionItems = getAggregateFunctionSelectItems(selectItems);

                    // Loop through the dataset and identify groups
                    while (dataSet.next())
                    {
                        Row row = dataSet.getRow();

                        // Subselect a row prototype with only the unique values that
                        // define the group
                        Row uniqueRow = row.getSubSelection(groupByHeader);

                        // function input is the values used for calculating aggregate
                        // functions in the group
                        Dictionary<SelectItem, List<Object>> functionInput;
                        if (!uniqueRows.ContainsKey(uniqueRow))
                        {
                            // If this group already exist, use an existing function
                            // input
                            functionInput = new Dictionary<SelectItem, List<Object>>();
                            foreach (SelectItem item in functionItems)
                            {
                                functionInput.Add(item, new List<Object>());
                            }
                            uniqueRows.Add(uniqueRow, functionInput);
                        }
                        else
                        {
                            // If this is a new group, create a new function input
                            functionInput = uniqueRows[uniqueRow];
                        }

                        // Loop through aggregate functions to check for validity
                        foreach (SelectItem item in functionItems)
                        {
                            List<Object> objects = functionInput[item];
                            Column column = item.getColumn();
                            if (column != null)
                            {
                                Object value = row.getValue(new SelectItem(column));
                                objects.Add(value);
                            }
                            else if (SelectItem.isCountAllItem(item))
                            {
                                // Just use the empty string, since COUNT(*) don't
                                // evaluate values (but null values should be prevented)
                                objects.Add("");
                            }
                            else
                            {
                                throw new ArgumentException("Expression function not supported: " + item);
                            }
                        }
                    }

                    dataSet.close();
                    List<Row>     resultData   = new List<Row>();
                    DataSetHeader resultHeader = new CachingDataSetHeader(selectItems);

                    int count = uniqueRows.Count;
                    // Loop through the groups to generate aggregates
                    foreach (KeyValuePair<Row, Dictionary<SelectItem, List<Object>>> key_value in uniqueRows)
                    {
                        Row row = key_value.Key;
                        Dictionary<SelectItem, List<Object>> functionInput = key_value.Value;
                        Object[] resultRow = new Object[selectItems.Count];
                        // Loop through select items to generate a row
                        int i = 0;
                        foreach (SelectItem item in selectItems)
                        {
                            int uniqueRowIndex = row.indexOf(item);
                            if (uniqueRowIndex != -1)
                            {
                                // If there's already a value for the select item in the
                                // row, keep it (it's one of the grouped by columns)
                                resultRow[i] = row.getValue(uniqueRowIndex);
                            }
                            else
                            {
                                // Use the function input to calculate the aggregate
                                // value
                                List<Object> objects = functionInput[item];
                                if (objects != null)
                                {
                                    Object functionResult = item.getAggregateFunction().evaluate(objects.ToArray());
                                    resultRow[i] = functionResult;
                                }
                                else
                                {
                                    if (item.getAggregateFunction() != null)
                                    {
                                        logger.error("No function input found for SelectItem: {}", item);
                                    }
                                }
                            }
                            i++;
                        }
                        resultData.Add(new DefaultRow(resultHeader, resultRow, null));
                    }

                    if (resultData.IsEmpty())
                    {
                        result = new EmptyDataSet(selectItems);
                    }
                    else
                    {
                        result = new InMemoryDataSet(resultHeader, resultData);
                    }
                }
                result = getSelection(selectItems, result);
                return result;
            } // getGrouped()

            /**
             * Applies aggregate values to a dataset. This method is to be invoked AFTER
             * any filters have been applied.
             * 
             * @param workSelectItems
             *            all select items included in the processing of the query
             *            (including those originating from other clauses than the
             *            SELECT clause).
             * @param dataSet
             * @return
             */
            public static DataSet getAggregated(List<SelectItem> workSelectItems, DataSet dataSet)
            {
                List<SelectItem> functionItems = getAggregateFunctionSelectItems(workSelectItems);
                if (functionItems.IsEmpty())
                {
                    return dataSet;
                }

                AggregateBuilder<Object> t;
                Dictionary<SelectItem, AggregateBuilder<Object>> aggregateBuilders = new Dictionary<SelectItem, AggregateBuilder<Object>>();
                foreach (SelectItem item in functionItems)
                {
                    aggregateBuilders.Add(item, item.getAggregateFunction().createAggregateBuilder<object>());
                }

                DataSetHeader  header;
                bool           onlyAggregates;
                if (functionItems.Count != workSelectItems.Count)
                {
                    onlyAggregates = false;
                    header         = new CachingDataSetHeader(workSelectItems);
                }
                else
                {
                    onlyAggregates = true;
                    header         = new SimpleDataSetHeader(workSelectItems);
                }

                List<Row> resultRows = new List<Row>();
                while (dataSet.next())
                {
                    Row inputRow = dataSet.getRow();
                    foreach (SelectItem item in functionItems)
                    {
                        AggregateBuilder<object> aggregateBuilder = aggregateBuilders[item];
                        Column column = item.getColumn();
                        if (column != null)
                        {
                            Object value = inputRow.getValue(new SelectItem(column));
                            aggregateBuilder.add(value);
                        }
                        else if (SelectItem.isCountAllItem(item))
                        {
                            // Just use the empty string, since COUNT(*) don't
                            // evaluate values (but null values should be prevented)
                            aggregateBuilder.add("");
                        }
                        else
                        {
                            throw new ArgumentException("Expression function not supported: " + item);
                        }
                    }

                    // If the result should also contain non-aggregated values, we
                    // will keep those in the rows list
                    if (! onlyAggregates)
                    {
                        Object[] values = new Object[header.size()];
                        for (int i = 0; i < header.size(); i++)
                        {
                            Object value = inputRow.getValue(header.getSelectItem(i));
                            if (value != null)
                            {
                                values[i] = value;
                            }
                        }
                        resultRows.Add(new DefaultRow(header, values));
                    }
                }
                dataSet.close();

                // Collect the aggregates
                Dictionary<SelectItem, Object> functionResult = new Dictionary<SelectItem, Object>();
                foreach (SelectItem item in functionItems)
                {
                    AggregateBuilder<object> aggregateBuilder = aggregateBuilders[item];
                    Object result = aggregateBuilder.getAggregate();
                    functionResult.Add(item, result);
                }

                // if there are no result rows (no matching records at all), we still
                // need to return a record with the aggregates
                bool noResultRows = resultRows.IsEmpty();

                if (onlyAggregates || noResultRows)
                {
                    // We will only create a single row with all the aggregates
                    Object[] values = new Object[header.size()];
                    for (int i = 0; i < header.size(); i++)
                    {
                        values[i] = functionResult[header.getSelectItem(i)];
                    }
                    Row row = new DefaultRow(header, values);
                    resultRows.Add(row);
                }
                else
                {
                    // We will create the aggregates as well as regular values
                    for (int i = 0; i < resultRows.Count; i++)
                    {
                        Row row = resultRows[i];
                        Object[] values = row.getValues();
                        foreach (KeyValuePair<SelectItem, Object> entry in functionResult)
                        {
                            SelectItem item = entry.Key;
                            int itemIndex = row.indexOf(item);
                            if (itemIndex != -1)
                            {
                                Object value = entry.Value;
                                values[itemIndex] = value;
                            }
                        }
                        resultRows[i] = new DefaultRow(header, values);
                    }
                }
                return new InMemoryDataSet(header, resultRows);  
            } // getAggregated()

            public class _Ordered_Comparer_impl_ : IComparer<Row>
            {
                private Query           _query;
                private int[]           _sortIndexes;
                private IComparer<Row>  _valueComparator;
                private OrderByItem[]   _orderByItems;

                public _Ordered_Comparer_impl_(IComparer<Row> valueComparator_arg, OrderByItem[] orderByItems_arg)
                {
                    _valueComparator  = valueComparator_arg;
                    _orderByItems = orderByItems_arg;
                } // constructor

                public int Compare(Row o1, Row o2)
                {
                    for (int i = 0; i < _sortIndexes.Length; i++)
                    {
                        int sortIndex = _sortIndexes[i];
                        Object sortObj1 = o1.getValue(sortIndex);
                        Object sortObj2 = o2.getValue(sortIndex);
                        int compare = _valueComparator.Compare((Row)sortObj1, (Row)sortObj2);
                        if (compare != 0)
                        {
                            OrderByItem orderByItem = _orderByItems[i];
                            bool ascending = orderByItem.isAscending();
                            if (ascending)
                            {
                                return compare;
                            }
                            else
                            {
                                return compare * -1;
                            }
                        }
                    }
                    return 0;
                } // Compare()
            } // _Ordered_Comparer_impl_ class

            public static DataSet getOrdered(DataSet dataSet, params OrderByItem[] orderByItems)
            {
                List<Row> values               = new List<Row>();
                IComparer<Row> valueComparator = null;

                if (orderByItems != null && orderByItems.Length != 0)
                {
                    int[] sortIndexes = new int[orderByItems.Length];
                    for (int i = 0; i < orderByItems.Length; i++)
                    {
                        OrderByItem item     = orderByItems[i];
                        int         index_of = dataSet.indexOf(item.getSelectItem());
                        sortIndexes[i]       = index_of;
                    }

                    values = readDataSetFull(dataSet);
                    if (values.IsEmpty())
                    {
                        return new EmptyDataSet(dataSet.getSelectItems());
                    }

                    valueComparator = ObjectComparator.getComparator();

                    // create a comparator for doing the actual sorting/ordering
                    IComparer<Row> comparator = new _Ordered_Comparer_impl_(valueComparator, orderByItems);
                }
                values.Sort(valueComparator); // Collections.sort(data, comparator);

                dataSet = new InMemoryDataSet(values);
                return dataSet;
            } // getOrdered()
          
        /**
         * Determines if a query contains {@link ScalarFunction}s in any clause of
         * the query EXCEPT for the SELECT clause. This is a handy thing to
         * determine because decorating with {@link ScalarFunctionDataSet} only
         * gives you select-item evaluation so if the rest of the query is pushed to
         * an underlying datastore, then it may create issues.
         * 
         * @param query
         * @return
         */
        public static bool containsNonSelectScalaFunctions(Query query)
        {
            // check FROM clause
            List<FromItem> fromItems = query.getFromClause().getItems();
            foreach (FromItem fromItem in fromItems)
            {
                // check sub-queries
                Query subQuery = fromItem.getSubQuery();
                if (subQuery != null)
                {
                    if (containsNonSelectScalaFunctions(subQuery))
                    {
                        return true;
                    }
                    if (! getScalarFunctionSelectItems(subQuery.getSelectClause().getItems()).IsEmpty())
                    {
                        return true;
                    }
                }
            } 

            // check WHERE clause
            if (! getScalarFunctionSelectItems(query.getWhereClause().getEvaluatedSelectItems()).IsEmpty())
            {
                return true;
            }

            // check GROUP BY clause
            if (! getScalarFunctionSelectItems(query.getGroupByClause().getEvaluatedSelectItems()).IsEmpty())
            {
                return true;
            }

            // check HAVING clause
            if (! getScalarFunctionSelectItems(query.getHavingClause().getEvaluatedSelectItems()).IsEmpty())
            {
                return true;
            }

            // check ORDER BY clause
            if (! getScalarFunctionSelectItems(query.getOrderByClause().getEvaluatedSelectItems()).IsEmpty())
            {
                return true;
            }

            return false;
        } // containsNonSelectScalaFunctions()

        /**
         * Performs a left join (aka left outer join) operation on two datasets.
         * 
         * @param ds1
         *            the left dataset
         * @param ds2
         *            the right dataset
         * @param onConditions
         *            the conditions to join by
         * @return the left joined result dataset
         */
        public static DataSet getLeftJoin(DataSet ds1, DataSet ds2, FilterItem[] onConditions)
        {
            if (ds1 == null)
            {
                throw new ArgumentException("Left DataSet cannot be null");
            }
            if (ds2 == null)
            {
                throw new ArgumentException("Right DataSet cannot be null");
            }
            SelectItem[] si1 = ds1.getSelectItems();
            SelectItem[] si2 = ds2.getSelectItems();
            SelectItem[] selectItems = new SelectItem[si1.Length + si2.Length];
            Array.Copy(si1, 0, selectItems, 0, si1.Length);
            Array.Copy(si2, 0, selectItems, si1.Length, si2.Length);

            List<Row> resultRows = new List<Row>();
            List<Row> ds2data    = readDataSetFull(ds2);
            if (ds2data.IsEmpty())
            {
                // no need to join, simply return a new view (with null values) on
                // the previous dataset.
                return getSelection(selectItems, ds1);
            }

            DataSetHeader header = new CachingDataSetHeader(selectItems);

            while (ds1.next())
            {
                // Construct a single-row dataset for making a carthesian product
                // against ds2
                Row ds1row = ds1.getRow();
                List<Row> ds1rows = new List<Row>();
                ds1rows.Add(ds1row);

                DataSet carthesianProduct = getCarthesianProduct(new DataSet[] { new InMemoryDataSet(
                                new CachingDataSetHeader(si1), ds1rows), new InMemoryDataSet(new CachingDataSetHeader(si2),
                                        ds2data) }, onConditions);
                List<Row> carthesianRows = readDataSetFull(carthesianProduct);
                if (carthesianRows.Count > 0)
                {
                    resultRows.AddRange(carthesianRows);
                }
                else
                {
                    Object[] values = ds1row.getValues();
                    Object[] row    = new Object[selectItems.Length];
                    Array.Copy(values, 0, row, 0, values.Length);
                    resultRows.Add(new DefaultRow(header, row));
                }
            }
            ds1.close();

            if (resultRows.IsEmpty())
            {
                return new EmptyDataSet(selectItems);
            }

            return new InMemoryDataSet(header, resultRows);
        } // getLeftJoin()

        public class _AggregateFunctionSelectItems_Predicate_Impl_ : amm_util.Predicate<SelectItem>
        {
            public bool eval(SelectItem arg)
            {
                return arg.getAggregateFunction() != null;
            }
        } // _AggregateFunctionSelectItems_Predicate_Impl_

        public static List<SelectItem> getAggregateFunctionSelectItems(IEnumerable<SelectItem> selectItems)
        {
            return CollectionUtils.filter<SelectItem, SelectItem>(selectItems, new _AggregateFunctionSelectItems_Predicate_Impl_());
        }

        public class _ScalarFunctionSelectItems_Predicate_Impl_ : amm_util.Predicate<SelectItem>
        {
            public bool eval(SelectItem arg)
            {
                return arg.getScalarFunction() != null;
            }
        } // _ScalarFunctionSelectItems_Predicate_Impl_

        public static List<SelectItem> getScalarFunctionSelectItems(IEnumerable<SelectItem> selectItems)
        {
            return CollectionUtils.filter<SelectItem, SelectItem>(selectItems, new _ScalarFunctionSelectItems_Predicate_Impl_());
        } // getScalarFunctionSelectItems()
    } // MetaModelHelper class
} // org.apache.metamodel.core namespace
