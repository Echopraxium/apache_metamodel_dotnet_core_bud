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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/schema/AbstractTable.java
using org.apache.metamodel.j2n.slf4j;
using org.apache.metamodel.j2n.types;
using org.apache.metamodel.schema;
using org.apache.metamodel.util;
using System;
using System.Collections.Generic;
using System.Text;

//import java.util.ArrayList;
//import java.util.Arrays;
//import java.util.HashSet;
//import java.util.List;
//import java.util.Set;

//import org.apache.metamodel.MetaModelHelper;
//import org.apache.metamodel.util.Action;
//import org.apache.metamodel.util.CollectionUtils;
//import org.apache.metamodel.util.HasNameMapper;
//import org.apache.metamodel.util.Predicate;
//import org.slf4j.Logger;
//import org.slf4j.LoggerFactory;

namespace org.apache.metamodel.core.schema
{
    /**
     * Abstract {@link Table} implementation. Includes most common/trivial methods.
     */
    public abstract class AbstractTable : Table
    {
            private static readonly long serialVersionUID = 1L;

            private static readonly NLogger logger = NLoggerFactory.getLogger(typeof(AbstractTable).Name);

            // @Override
            public int getColumnCount()
            {
                return getColumns().Length;
            } // getColumnCount()

            public abstract Column[]       getColumns();
            public abstract Schema         getSchema();
            public abstract TableType      getType();
            public abstract Relationship[] getRelationships();
            public abstract string         getRemarks();
            public abstract string         getQuote();
            public abstract string         getName();

            // @Override
            public String[] getColumnNames()
            {
                Column[]     columns = getColumns();
                List<string> values  = CollectionUtils.map(columns, new HasNameMapper());
                return values.ToArray();
            } // getColumnNames()

            // @Override
            public bool equals(object obj)
            {
                if (obj == null)
                {
                    return false;
                }
                if (obj == this)
                {
                    return true;
                }
                if (obj is Table)
                {
                    Table other = (Table)obj;
                    if (!getQualifiedLabel().Equals(other.getQualifiedLabel()))
                    {
                        return false;
                    }
                    if (GetType() != other.GetType())
                    {
                        return false;
                    }
                    Schema sch1 = getSchema();
                    Schema sch2 = other.getSchema();
                    if (sch1 != null)
                    {
                        if (!sch1.Equals(sch2))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (sch2 != null)
                        {
                            return false;
                        }
                    }

                    try
                    {
                        String[] columnNames1 = getColumnNames();
                        String[] columnNames2 = other.getColumnNames();

                        if (columnNames1 != null && columnNames1.Length != 0)
                        {
                            if (columnNames2 != null && columnNames2.Length != 0)
                            {
                                if (!columnNames1.Equals(columnNames2))
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        // going "down stream" may throw exceptions, e.g. due to
                        // de-serialization issues. We will be tolerant to such
                        // exceptions
                        logger.debug("Caught (and ignoring) exception while comparing column names of tables", e);
                    }

                    return true;
                }
                return false;
            } // equals()


            public Column getColumn(int index) // throws IndexOutOfBoundsException
            {
                Column[] columns = getColumns();
                return columns[index];
            } // getColumn()

            // @Override
            public Column[] getPrimaryKeys()
            {
                List<Column> primaryKeyColumns = new List<Column>();
                Column[] columnsInTable = getColumns();
                foreach (Column column in columnsInTable)
                {
                    if (column.isPrimaryKey())
                    {
                        primaryKeyColumns.Add(column);
                    }
                }
                Column[] array_copy;
                array_copy = primaryKeyColumns.ToArray();
                return array_copy;
            } // getPrimaryKeys()


            public Column getColumnByName(String columnName)
            {
                if (columnName == null)
                {
                    return null;
                }

                List<Column> foundColumns = new List<Column>(1);

                // Search for column matches, case insensitive.
                foreach (Column column in getColumns())
                {
                    String candidateName = column.getName();
                    if (columnName.Equals(candidateName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        foundColumns.Add(column);
                    }
                }

                int numColumns = foundColumns.Count;

                if (logger.isDebugEnabled())
                {
                    logger.debug("Found {} column(s) matching '{}': {}", new Object[] { numColumns, columnName, foundColumns });
                }

                if (numColumns == 0)
                {
                    return null;
                }
                else if (numColumns == 1)
                {
                    // if there's only one, return it.
                    return foundColumns[0];
                }

                // If more matches are found, search case sensitive
                foreach (Column column in foundColumns)
                {
                    if (columnName.Equals(column.getName()))
                    {
                        return column;
                    }
                }

                // if none matches case sensitive, pick the first one.
                return foundColumns[0];
            } // getColumnByName()

            // @Override
            public int getRelationshipCount()
            {
                return getRelationships().Length;
            } // getRelationshipCount()


            public override String ToString()
            {
                return "Table[name=" + getName() + ",type=" + getType() + ",remarks=" + getRemarks() + "]";
            } // ToString()

            public int hashCode()
            {
                return getName().GetHashCode();
            } // hashCode()

            public String getQuotedName()
            {
                String quote = getQuote();
                if (quote == null)
                {
                    return getName();
                }
                return quote + getName() + quote;
            } // getQuotedName()

            // @Override
            public String getQualifiedLabel()
            {
                StringBuilder sb = new StringBuilder();
                Schema schema = getSchema();
                if (schema != null && schema.getName() != null)
                {
                    sb.Append(schema.getQualifiedLabel());
                    sb.Append('.');
                }
                sb.Append(getName());
                return sb.ToString();
            } // getQualifiedLabel()


            public int compareTo(Table that)
            {
                int diff = getQualifiedLabel().CompareTo(that.getQualifiedLabel());
                if (diff == 0)
                {
                    diff = ToString().CompareTo(that.ToString());
                }
                return diff;
            } // compareTo()


            #region -------- getNumberColumns() --------
            public Column[] getNumberColumns()
            {
                _Predicate_Impl_Number_ value = new _Predicate_Impl_Number_();
                Column[] values = CollectionUtils.filter(getColumns(), value).ToArray();
                return values;
            } // getNumberColumns()

            private class _Predicate_Impl_Number_ : metamodel.util.Predicate<Column>
            {
                public bool eval(Column col)
                {
                    ColumnType type = col.getType();
                    return type != null && type.isNumber();
                }
            } // _Predicate_Impl_Number_ class
            #endregion getNumberColumns()


            #region -------- getLiteralColumns() --------
            public Column[] getLiteralColumns()
            {
                _Predicate_Impl_Literal_ value = new _Predicate_Impl_Literal_();
                Column[] values = CollectionUtils.filter(getColumns(), value).ToArray();
                return values;
            } // getLiteralColumns()

            private class _Predicate_Impl_Literal_ : metamodel.util.Predicate<Column>
            {
                public bool eval(Column col)
                {
                    ColumnType type = col.getType();
                    return type != null && type.isLiteral();
                }
            } // _Predicate_Impl_Literal_ class
            #endregion getLiteralColumns()


            #region -------- getTimeBasedColumns() --------
            public Column[] getTimeBasedColumns()
            {
                _Predicate_Impl_TimeBased_ value = new _Predicate_Impl_TimeBased_();
                Column[] values = CollectionUtils.filter(getColumns(), value).ToArray();
                return values;
            } // getTimeBasedColumns()

            private class _Predicate_Impl_TimeBased_ : metamodel.util.Predicate<Column>
            {
                public bool eval(Column col)
                {
                    ColumnType type = col.getType();
                    return type != null && type.isTimeBased();
                }
            } // _Predicate_Impl_TimeBased_ class
            #endregion getLiteralColumns()



            #region -------- getBooleanColumns() --------
            public Column[] getBooleanColumns()
            {
                _Predicate_Impl_Boolean_ value = new _Predicate_Impl_Boolean_();
                Column[] values = CollectionUtils.filter(getColumns(), value).ToArray();
                return values;
            } // getBooleanColumns()

            private class _Predicate_Impl_Boolean_ : metamodel.util.Predicate<Column>
            {
                public bool eval(Column col)
                {
                    ColumnType type = col.getType();
                    return type != null && type.isBoolean();
                }
            } // _Predicate_Impl_Boolean_ class
            #endregion getBooleanColumns()


            #region -------- getIndexedColumns() --------
            public Column[] getIndexedColumns()
            {
                _Predicate_Impl_Indexed_ value = new _Predicate_Impl_Indexed_();
                Column[] values = CollectionUtils.filter(getColumns(), value).ToArray();
                return values;
            } // getIndexedColumns()

            private class _Predicate_Impl_Indexed_ : metamodel.util.Predicate<Column>
            {
                public bool eval(Column col)
                {
                    return col.isIndexed();
                }
            } // _Predicate_Impl_Indexed_ class
            #endregion getIndexedColumns()


            #region -------- getForeignKeyRelationships() --------
            public Relationship[] getForeignKeyRelationships()
            {
                _Predicate_Impl_ForeignKeyRelationships_ value = new _Predicate_Impl_ForeignKeyRelationships_(this);
                Relationship[] values = CollectionUtils.filter(getRelationships(), value).ToArray();
                return values;
            } // getForeignKeyRelationships()

            private class _Predicate_Impl_ForeignKeyRelationships_ : metamodel.util.Predicate<Relationship>
            {
                private AbstractTable _at;
                public _Predicate_Impl_ForeignKeyRelationships_(AbstractTable at_arg)
                {
                    _at = at_arg;
                }

                public bool eval(Relationship arg)
                {
                    return _at.equals(arg.getForeignTable());
                }
            } // _Predicate_Impl_ForeignKeyRelationships_ class
            #endregion getForeignKeyRelationships()


            #region -------- getPrimaryKeyRelationships() --------
            public Relationship[] getPrimaryKeyRelationships()
            {
                _Predicate_Impl_ForeignTable_ value = new _Predicate_Impl_ForeignTable_(this);
                Relationship[] values = CollectionUtils.filter(getRelationships(), value).ToArray();
                return values;
            } // getPrimaryKeyRelationships()

            private class _Predicate_Impl_ForeignTable_ : metamodel.util.Predicate<Relationship>
            {
                private AbstractTable _at;
                public _Predicate_Impl_ForeignTable_(AbstractTable at_arg)
                {
                    _at = at_arg;
                }

                public bool eval(Relationship arg)
                {
                    return _at.equals(arg.getPrimaryTable());
                }
            } // _Predicate_Impl_ForeignTable_ class
            #endregion getPrimaryKeyRelationships()



            #region -------- getRelationships() --------
            public Relationship[] getRelationships(Table other_table)
            {
                _Predicate_Impl_RelationShips_ value = new _Predicate_Impl_RelationShips_(this, other_table);
                Relationship[] values = CollectionUtils.filter(getRelationships(), value).ToArray();
                return values;
            } // getRelationships()

            private class _Predicate_Impl_RelationShips_ : metamodel.util.Predicate<Relationship>
            {
                private AbstractTable _at;
                private Table         _otherTable;
                public _Predicate_Impl_RelationShips_(AbstractTable at_arg, Table otherTable_arg)
                {
                    _at         = at_arg;
                    _otherTable = otherTable_arg;
                }

                public bool eval(Relationship relation)
                {
                    if (relation.getForeignTable() == _otherTable && relation.getPrimaryTable() == _at)
                    {
                        return true;
                    }
                    else if (relation.getForeignTable() == _at && relation.getPrimaryTable() == _otherTable)
                    {
                        return true;
                    }
                    return false;
                }
            } // _Predicate_Impl_ForeignTable_ class
            #endregion getRelationships()


            #region -------- getForeignKeys() --------
            public Column[] getForeignKeys()
            {
                HashSet<Column> columns        = new HashSet<Column>();
                Relationship[]  relationships  = getForeignKeyRelationships();
                _Action_Impl_ForeignKeys value = new _Action_Impl_ForeignKeys(this, columns);
                CollectionUtils.forEach(relationships, value);
                Column[] copy_values = new Column[columns.Count];
                columns.CopyTo(copy_values);
                return copy_values;
            } // getForeignKeys()

            //public final Column[] getForeignKeys()
            //    {
            //        final Set<Column> columns = new HashSet<Column>();
            //        final Relationship[] relationships = getForeignKeyRelationships();
            //        CollectionUtils.forEach(relationships, rel-> {
            //            Column[] foreignColumns = rel.getForeignColumns();
            //            for (Column column : foreignColumns)
            //            {
            //                columns.add(column);
            //            }
            //        });
            //        return columns.toArray(new Column[columns.size()]);
            //    }
            private class _Action_Impl_ForeignKeys : NConsumer<Relationship>
            {
                private HashSet<Column> _columns;
                public _Action_Impl_ForeignKeys(AbstractTable at_arg, HashSet<Column> columns_arg)
                {
                    _columns = columns_arg;
                }

                public override void accept(Relationship obj)
                {
                    Column[] foreignColumns = obj.getForeignColumns();
                    foreach (Column column in foreignColumns)
                    {
                        _columns.Add(column);
                    }
                }
            } // _Action_Impl_ForeignKeys class
            #endregion getForeignKeys()


            public Column[] getColumnsOfType(ColumnType columnType)
            {
                Column[] columns = getColumns();
                return MetaModelHelper.getColumnsByType(columns, columnType);
            } // getColumnsOfType()

            public Column[] getColumnsOfSuperType(SuperColumnType superColumnType)
            {
                Column[] columns = getColumns();
                return MetaModelHelper.getColumnsBySuperType(columns, superColumnType);
            } // getColumnsOfSuperType()
    } // AnstractTable class
} // org.apache.metamodel.core.schema namespace
