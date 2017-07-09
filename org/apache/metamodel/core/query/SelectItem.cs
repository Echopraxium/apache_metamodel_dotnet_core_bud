/**
* Licensed to the Apache Software Foundation (ASF) under one
* or more contributor license agreements. See the NOTICE file
* distributed with this work for additional information
* regarding copyright ownership. The ASF licenses this file
* to you under the Apache License, Version 2.0 (the
* "License"); you may not use this file except in compliance
* with the License. You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing,
* software distributed under the License is distributed on an
* "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
* KIND, either express or implied. See the License for the
* specific language governing permissions and limitations
* under the License.
*/
// https://github.com/apache/metamodel/blob/master/core/src/main/java/org/apache/metamodel/query/SelectItem.java
using org.apache.metamodel.util;
using org.apache.metamodel.schema;
using System;
using System.Text;
using org.apache.metamodel.j2cs.collections;
using org.apache.metamodel.j2cs.slf4j;

namespace org.apache.metamodel.query
{
    /**
     * Represents a SELECT item. SelectItems can take different forms:
     * <ul>
     * <li>column SELECTs (selects a column from a table)</li>
     * <li>column function SELECTs (aggregates the values of a column)</li>
     * <li>expression SELECTs (retrieves data based on an expression (only supported
     * for JDBC datastores)</li>
     * <li>expression function SELECTs (retrieves databased on a function and an
     * expression, only COUNT(*) is supported for non-JDBC datastores))</li>
     * <li>SELECTs from subqueries (works just like column selects, but in stead of
     * pointing to a column, it retrieves data from the select item of a subquery)</li>
     * </ul>
     * 
     * @see SelectClause
    */
    public class SelectItem : BaseObject, QueryItem //, ICloneable
    {
        public static readonly string  FUNCTION_APPROXIMATION_PREFIX = "APPROXIMATE ";

        private static readonly long   serialVersionUID = 317475105509663973L;
        private static readonly Logger logger           = LoggerFactory.getLogger(typeof(SelectItem).Name);

        // immutable fields (essense)
        private readonly Column         _column;
        private readonly FunctionType   _function;
        private readonly object[]       _functionParameters;
        private readonly string         _expression;
        private readonly SelectItem     _subQuerySelectItem;
        private readonly FromItem       _fromItem;

        // mutable fields (tweaking)
        private bool   _functionApproximationAllowed;
        private Query  _query;
        private string _alias;

        /**
         * All-arguments constructor
         * 
         * @param column
         * @param fromItem
         * @param function
         * @param functionParameters
         * @param expression
         * @param subQuerySelectItem
         * @param alias
         * @param functionApproximationAllowed
         */
        private SelectItem(Column column, FromItem fromItem, FunctionType function, object[] functionParameters,
                           string expression, SelectItem subQuerySelectItem, 
                           string alias, bool functionApproximationAllowed) : base()
        {
            _column                       = column;
            _fromItem                     = fromItem;
            _function                     = function;
            _functionParameters           = functionParameters;
            _expression                   = expression;
            _subQuerySelectItem           = subQuerySelectItem;
            _alias                        = alias;
            _functionApproximationAllowed = functionApproximationAllowed;
        } // constructor

        /**
         * Creates a simple SelectItem that selects from a column
         * 
         * @param column
         */
        public SelectItem(Column column) : this(null, column)
        {
        } // constructor

        /**
         * Creates a SelectItem that uses a function on a column, for example
         * SUM(price) or MAX(age)
         * 
         * @param function
         * @param column
         */
        public SelectItem(FunctionType function, Column column) : this(function, column, null)
        { } // constructor

        /**
         * Create a SelectItem that uses a function with parameters on a column.
         * 
         * @param function
         * @param functionParameters
         * @param column
         */
        public SelectItem(FunctionType function, object[] functionParameters, Column column) :
                          this(function, functionParameters, column, null)
        {  
        } // constructor

        /**
         * Creates a SelectItem that references a column from a particular
         * {@link FromItem}, for example a.price or p.age
         * 
         * @param column
         * @param fromItem
         */
        public SelectItem(Column column, FromItem fromItem) :
                          this(null, column, fromItem)
        {
            if (fromItem != null)
            {
                Table fromItemTable = fromItem.getTable();
                if (fromItemTable != null)
                {
                    Table columnTable = column.getTable();
                    if (columnTable != null && !columnTable.Equals(fromItemTable))
                    {
                        throw new InvalidOperationException("Column's table '" + columnTable.getName()
                                  + "' is not equal to referenced table: " + fromItemTable);
                    }
                }
            }
        } // constructor

        /**
         * Creates a SelectItem that uses a function on a column from a particular
         * {@link FromItem}, for example SUM(a.price) or MAX(p.age)
         * 
         * @param function
         * @param column
         * @param fromItem
         */
        public SelectItem(FunctionType function, Column column, FromItem fromItem) :
                          this(column, fromItem, function, null, null, null, null, false)
        {
             if (column == null)
             {
                 throw new ArgumentException("column=null");
             }
        } // constructor

        /**
         * Creates a SelectItem that uses a function with parameters on a column
         * from a particular {@link FromItem}, for example
         * MAP_VALUE('path.to.value', doc)
         * 
         * @param function
         * @param functionParameters
         * @param column
         * @param fromItem
         */
        public SelectItem(FunctionType function, object[] functionParameters, Column column, FromItem fromItem) :
                          this(column, fromItem, function, functionParameters, null, null, null, false)
        {
            if (column == null)
            {
                throw new ArgumentException("column=null");
            }
        } // constructor

        /**
         * Creates a SelectItem based on an expression. All expression-based
         * SelectItems must have aliases.
         * 
         * @param expression
         * @param alias
         */
        public SelectItem(string expression, string alias): this(null, expression, alias)
        {
        } // constructor

        /**
         * Creates a SelectItem based on a function and an expression. All
         * expression-based SelectItems must have aliases.
         * 
         * @param function
         * @param expression
         * @param alias
         */
        public SelectItem(FunctionType function, string expression, string alias):
                      this(null, null, function, null, expression, null, alias, false)
        {
            if (expression == null)
            {
                 throw new ArgumentException("expression=null");
            }
        } // constructor

        /**
         * Creates a SelectItem that references another select item in a subquery
         * 
         * @param subQuerySelectItem
         * @param subQueryFromItem
         *            the FromItem that holds the sub-query
         */
        public SelectItem(SelectItem subQuerySelectItem, FromItem subQueryFromItem):
                      this(null, subQueryFromItem, null, null, null, subQuerySelectItem, null, false)
        {
            if (subQueryFromItem.getSubQuery() == null)
            {
                throw new ArgumentException("Only sub-query based FromItems allowed.");
            }
            if (     subQuerySelectItem.getQuery() != null
                && ! subQuerySelectItem.getQuery().Equals(subQueryFromItem.getSubQuery()))
            {
                throw new ArgumentException("The SelectItem must exist in the sub-query");
            }
        } // constructor

        /**
         * Subclasses should implement this method and add all fields to the list
         * that are to be included in equals(...) and hashCode() evaluation
         * 
         * @param identifiers
        */
        protected override void decorateIdentity(CsList<Object> identifiers)
        {
            identifiers.add(_expression);
            identifiers.add(_alias);
            identifiers.add(_column);
            identifiers.add(_function);
            identifiers.add(_functionApproximationAllowed);
            if (_fromItem == null && _column != null && _column.getTable() != null)
            {
                // add a FromItem representing the column's table - this makes equal
                // comparison work when the only difference is whether or not
                // FromItem is specified
                identifiers.add(new FromItem(_column.getTable()));
            }
            else
            {
                identifiers.add(_fromItem);
            }
            identifiers.add(_subQuerySelectItem);
        } // decorateIdentity()

        /**
         * Generates a COUNT(*) select item
         */
        public static SelectItem getCountAllItem()
        {
            return new SelectItem(FunctionTypeDefs.COUNT, "*", null);
        } // getCountAllItem()

        public static bool isCountAllItem(SelectItem item)
        {
            if (    item != null && item.getFunction() != null 
                 && item.getFunction().ToString().Equals("COUNT")
                 && item.getExpression() == "*")
            {
                return true;
            }
            return false;
        } // isCountAllItem()

        public string getAlias()
        {
            return _alias;
        } // getAlias()

        public SelectItem setAlias(string alias)
        {
            _alias = alias;
            return this;
        }

        /**
         * 
         * @return
         * @deprecated use {@link #getAggregateFunction()} or
         *             {@link #getScalarFunction()} instead,
         *             or {@link #hasFunction()} to check if a
         *             function is set at all.
         */
        // [J2Cs: [System.Obsolete] annotation commented out else there are compiler warnings 612] 
        // [System.Obsolete]
        public FunctionType getFunction()
        {
            return _function;
        }

        public bool hasFunction()
        {
            return _function != null;
        }

        public AggregateFunction getAggregateFunction()
        {
            if (_function is AggregateFunction) {
                return (AggregateFunction)_function;
            }
            return null;
        } // getAggregateFunction()

        public ScalarFunction getScalarFunction()
        {
            if (_function is ScalarFunction) {
                return (ScalarFunction)_function;
            }
            return null;
        } // getScalarFunction()

        /**
         * Gets any parameters to the {@link #getFunction()} used.
         * 
         * @return
         */
        public object[] getFunctionParameters()
        {
            return _functionParameters;
        }

        /**
         * @return if this is a function based SelectItem where function calculation
         *         is allowed to be approximated (if the datastore type has an
         *         approximate calculation method). Approximated function results
         *         are as the name implies not exact, but might be valuable as an
         *         optimization in some cases.
         */
        public bool isFunctionApproximationAllowed()
        {
            return _functionApproximationAllowed;
        }

        public void setFunctionApproximationAllowed(bool functionApproximationAllowed)
        {
            _functionApproximationAllowed = functionApproximationAllowed;
        }

        public Column getColumn()
        {
            return _column;
        } // getColumn()

        /**
         * Tries to infer the {@link ColumnType} of this {@link SelectItem}. For
         * expression based select items, this is not possible, and the method will
         * return null.
         * 
         * @return
         */
        public ColumnType getExpectedColumnType()
        {
            if (_subQuerySelectItem != null)
            {
                return _subQuerySelectItem.getExpectedColumnType();
            }
            if (_function != null)
            {
                if (_column != null)
                {
                    return _function.getExpectedColumnType(_column.getType());
                }
                else
                {
                    return _function.getExpectedColumnType(null);
                }
            }
            if (_column != null)
            {
                return _column.getType();
            }
            return null;
        } // getExpectedColumnType()

        /**
         * Returns an "expression" that this select item represents. Expressions are
         * not necesarily portable across {@link DataContext} implementations, but
         * may be useful for utilizing database-specific behaviour in certain cases.
         * 
         * @return
         */
        public string getExpression()
        {
            return _expression;
        }

        public SelectItem setQuery(Query query)
        {
            _query = query;
            return this;
        }

        public Query getQuery()
        {
            return _query;
        }

        public SelectItem getSubQuerySelectItem()
        {
            return _subQuerySelectItem;
        }

        /**
         * @deprecated use {@link #getFromItem()} instead
         */
        [System.Obsolete]
        public FromItem getSubQueryFromItem()
        {
            return _fromItem;
        }

        public FromItem getFromItem()
        {
            return _fromItem;
        }

        /**
         * @return the name that this SelectItem can be referenced with, if
         *         referenced from a super-query. This will usually be the alias,
         *         but if there is no alias, then the column name will be used.
         */
        public string getSuperQueryAlias()
        {
            return getSuperQueryAlias(true);
        }

        /**
         * @return the name that this SelectItem can be referenced with, if
         *         referenced from a super-query. This will usually be the alias,
         *         but if there is no alias, then the column name will be used.
         * 
         * @param includeQuotes
         *            indicates whether or not the output should include quotes, if
         *            the select item's column has quotes associated (typically
         *            true, but false if used for presentation)
         */
        public string getSuperQueryAlias(bool includeQuotes)
        {
            if (_alias != null)
            {
                return _alias;
            }
            else if (_column != null)
            {
                StringBuilder sb = new StringBuilder();
                if (_function != null)
                {
                    if (_functionApproximationAllowed)
                    {
                        sb.Append(FUNCTION_APPROXIMATION_PREFIX);
                    }
                    sb.Append(_function.getFunctionName());
                    sb.Append('(');
                }
                if (includeQuotes)
                {
                    sb.Append(_column.getQuotedName());
                }
                else
                {
                    sb.Append(_column.getName());
                }
                if (_function != null)
                {
                    sb.Append(')');
                }
                return sb.ToString();
            }
            else
            {
                logger.debug("Could not resolve a reasonable super-query alias for SelectItem: {}", toSql());
                return toStringNoAlias().ToString();
            }
        } // getSuperQueryAlias()

        public string getSameQueryAlias()
        {
            return getSameQueryAlias(false);
        }

        /**
         * @return an alias that can be used in WHERE, GROUP BY and ORDER BY clauses
         *         in the same query
         */
        public string getSameQueryAlias(bool includeSchemaInColumnPath)
        {
            if (_column != null)
            {
                StringBuilder sb = new StringBuilder();
                string columnPrefix = getToStringColumnPrefix(includeSchemaInColumnPath);
                sb.Append(columnPrefix);
                sb.Append(_column.getQuotedName());
                if (_function != null)
                {
                    if (_functionApproximationAllowed)
                    {
                        sb.Insert(0, FUNCTION_APPROXIMATION_PREFIX + _function.getFunctionName() + "(");
                    }
                    else
                    {
                        sb.Insert(0, _function.getFunctionName() + "(");
                    }
                    sb.Append(")");
                }
                return sb.ToString();
            }
            string alias = getAlias();
            if (alias == null)
            {
                alias = toStringNoAlias(includeSchemaInColumnPath).ToString();
                logger.debug("Could not resolve a reasonable same-query alias for SelectItem: {}", toSql());
            }
            return alias;
        } // getSameQueryAlias()

        public string toSql()
        {
            return toSql(false);
        }

        public string toSql(bool includeSchemaInColumnPath)
        {
            StringBuilder sb = toStringNoAlias(includeSchemaInColumnPath);
            if (_alias != null)
            {
                sb.Append(" AS ");
                sb.Append(_alias);
            }
            return sb.ToString();
        }

        public StringBuilder toStringNoAlias()
        {
            return toStringNoAlias(false);
        }

        public StringBuilder toStringNoAlias(bool includeSchemaInColumnPath)
        {
            StringBuilder sb = new StringBuilder();
            if (_column != null)
            {
                sb.Append(getToStringColumnPrefix(includeSchemaInColumnPath));
                sb.Append(_column.getQuotedName());
            }
            if (_expression != null)
            {
                sb.Append(_expression);
            }
            if (_fromItem != null && _subQuerySelectItem != null)
            {
                if (_fromItem.getAlias() != null)
                {
                    sb.Append(_fromItem.getAlias() + '.');
                }
                sb.Append(_subQuerySelectItem.getSuperQueryAlias());
            }
            if (_function != null)
            {
                StringBuilder functionBeginning = new StringBuilder();
                if (_functionApproximationAllowed)
                {
                    functionBeginning.Append(FUNCTION_APPROXIMATION_PREFIX);
                }

                functionBeginning.Append(_function.getFunctionName());
                functionBeginning.Append('(');
                object[] functionParameters = getFunctionParameters();
                if (functionParameters != null && functionParameters.Length != 0)
                {
                    for (int i = 0; i < functionParameters.Length; i++)
                    {
                        functionBeginning.Append('\'');
                        functionBeginning.Append(functionParameters[i]);
                        functionBeginning.Append('\'');
                        functionBeginning.Append(',');
                    }
                }
                sb.Insert(0, functionBeginning.ToString());
                sb.Append(")");
            }
            return sb;
        } // toStringNoAlias()

        //[J2Cs: Stub ]
        private string getToStringColumnPrefix(bool includeSchemaInColumnPath)
        {
            return "";
        }

        //private string getToStringColumnPrefix(bool includeSchemaInColumnPath)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    if (_fromItem != null && _fromItem.getAlias() != null)
        //    {
        //        sb.Append(_fromItem.getAlias());
        //        sb.Append('.');
        //    }
        //    else
        //    {
        //        Table table = _column.getTable();
        //        string tableLabel;
        //        if (_query == null)
        //        {
        //            tableLabel = null;
        //        }
        //        else
        //        {
        //            tableLabel = _query.getFromClause().getAlias(table);
        //        }
        //        if (table != null)
        //        {
        //            if (tableLabel == null)
        //            {
        //                tableLabel = table.getQuotedName();
        //                if (includeSchemaInColumnPath)
        //                {
        //                    Schema schema = table.getSchema();
        //                    if (schema != null)
        //                    {
        //                        tableLabel = schema.getQuotedName() + "." + tableLabel;
        //                    }
        //                }
        //            }
        //            sb.Append(tableLabel);
        //            sb.Append('.');
        //        }
        //    }
        //    return sb.ToString();
        //} // getToStringColumnPrefix()

        public bool equalsIgnoreAlias(SelectItem that)
        {
            return equalsIgnoreAlias(that, false);
        }

        public bool equalsIgnoreAlias(SelectItem that, bool exactColumnCompare)
        {
            if (that == null)
            {
                return false;
            }
            if (that == this)
            {
                return true;
            }

            EqualsBuilder eb = new EqualsBuilder();
            if (exactColumnCompare)
            {
                eb.append(this._column == that._column);
                eb.append(this._fromItem, that._fromItem);
            }
            else
            {
                eb.append(this._column, that._column);
            }
            eb.append(this._function, that._function);
            eb.append(this._functionApproximationAllowed, that._functionApproximationAllowed);
            eb.append(this._expression, that._expression);
            if (_subQuerySelectItem != null)
            {
                eb.append(_subQuerySelectItem.equalsIgnoreAlias(that._subQuerySelectItem));
            }
            else
            {
                if (that._subQuerySelectItem != null)
                {
                    eb.append(false);
                }
            }
            return eb.isEquals();
        } // equalsIgnoreAlias()

        //protected SelectItem clone()
        //{
        //    return clone(null);
        //}

        /**
         * Creates a clone of the {@link SelectItem} for use within a cloned
         * {@link Query}.
         * 
         * @param clonedQuery
         *            a new {@link Query} object that represents the clone-to-be of
         *            a query. It is expected that {@link FromItem}s have already
         *            been cloned in this {@link Query}.
         * @return
         */
        //protected SelectItem clone(Query clonedQuery)
        //{
        //    SelectItem subQuerySelectItem = (_subQuerySelectItem == null ? null : _subQuerySelectItem.clone());
        //    FromItem fromItem;
        //    if (_fromItem == null)
        //    {
        //        fromItem = null;
        //    }
        //    else if (clonedQuery != null && _query != null)
        //    {
        //        int indexOfFromItem = _query.getFromClause().indexOf(_fromItem);
        //        if (indexOfFromItem != -1)
        //        {
        //            fromItem = clonedQuery.getFromClause().getItem(indexOfFromItem);
        //        }
        //        else
        //        {
        //            fromItem = _fromItem.clone();
        //        }
        //    }
        //    else
        //    {
        //        fromItem = _fromItem.clone();
        //    }

        //    SelectItem s = new SelectItem(_column, fromItem, _function, _functionParameters, _expression,
        //            subQuerySelectItem, _alias, _functionApproximationAllowed);
        //    return s;
        //} // clone()

        /**
         * Creates a copy of the {@link SelectItem}, with a different
         * {@link FunctionType}.
         * 
         * @param function
         * @return
         */
        public SelectItem replaceFunction(FunctionType function)
        {
            return new SelectItem(_column, _fromItem, function, _functionParameters, _expression, _subQuerySelectItem,
                    _alias, _functionApproximationAllowed);
        }

        /**
         * Creates a copy of the {@link SelectItem}, with a different
         * {@link #isFunctionApproximationAllowed()} flag set.
         * 
         * @param functionApproximationAllowed
         * @return
         */
        public SelectItem replaceFunctionApproximationAllowed(bool functionApproximationAllowed)
        {
            return new SelectItem(_column, _fromItem, _function, _functionParameters, _expression, _subQuerySelectItem,
                    _alias, functionApproximationAllowed);
        }

        /**
         * Investigates whether or not this SelectItem references a particular
         * column. This will search for direct references and indirect references
         * via subqueries.
         * 
         * @param column
         * @return a boolean that is true if the specified column is referenced by
         *         this SelectItem and false otherwise.
         */
        public bool isReferenced(Column column)
        {
            if (column != null)
            {
                if (column.Equals(_column))
                {
                    return true;
                }
                if (_subQuerySelectItem != null)
                {
                    return _subQuerySelectItem.isReferenced(column);
                }
            }
            return false;
        }

        public override string ToString()
        {
            return toSql();
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        QueryItem QueryItem.setQuery(Query query)
        {
            throw new NotImplementedException();
        }

        public string toString()
        {
            throw new NotImplementedException();
        }
    }
} // org.apache.metamodel.query namespace
