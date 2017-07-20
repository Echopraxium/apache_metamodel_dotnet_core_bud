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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/query/FilterItem.java
//import java.util.ArrayList;
//import java.util.Arrays;
//import java.util.Comparator;
//import java.util.HashSet;
//import java.util.List;
//import java.util.Set;

//import org.apache.metamodel.data.IRowFilter;
//import org.apache.metamodel.util.CollectionUtils;
//import org.apache.metamodel.util.FormatHelper;
//import org.apache.metamodel.util.ObjectComparator;
//import org.apache.metamodel.util.WildcardPattern;

using org.apache.metamodel.data;
using org.apache.metamodel.j2n.collections;
using org.apache.metamodel.schema;
using org.apache.metamodel.util;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.apache.metamodel.query
{
    /**
     * Represents a filter in a query that resides either within a WHERE clause or a
     * HAVING clause
     *
     * @see FilterClause
     * @see OperatorType
     * @see LogicalOperator
     */
    public class FilterItem : BaseObject, QueryItem //, Cloneable, IRowFilter
    {
        private static readonly long serialVersionUID = 2435322742894653227L;

        private Query                _query;
        private SelectItem           _selectItem;
        private OperatorType         _operator;
        private object               _operand;
        private NList<FilterItem>   _childItems;
        private LogicalOperator      _logicalOperator;
        private string               _expression;
        private HashSet<object>      _inValues;

        /**
         * Private constructor, used for cloning
         */
        private FilterItem(SelectItem selectItem, OperatorType operator_arg, object operand, NList<FilterItem> orItems,
                           string expression, LogicalOperator logicalOperator)
        {
            _selectItem      = selectItem;
            _operator        = operator_arg;
            //_operand         = validateOperand(operand);
            _childItems      = orItems;
            _expression      = expression;
            _logicalOperator = logicalOperator;
        } // private constructor

        /**
         * Creates a composite filter item based on other filter items. Each
         * provided filter items will be OR'ed meaning that if one of the evaluates
         * as true, then the composite filter will be evaluated as true
         *
         * @param items
         *            a list of items to include in the composite
         */
        public FilterItem(NList<FilterItem> items) : this(LogicalOperator.OR, items)
        {
        } // constructor

        ///**
        // * Creates a compound filter item based on other filter items. Each provided
        // * filter item will be combined according to the {@link LogicalOperator}.
        // *
        // * @param logicalOperator
        // *            the logical operator to apply
        // * @param items
        // *            an array of items to include in the composite
        // */
        //public FilterItem(LogicalOperator logicalOperator, params FilterItem[] items) :
        //                  this(logicalOperator, Arrays.asList(items))
        //{
        //}

        /**
         * Creates a compound filter item based on other filter items. Each provided
         * filter item will be combined according to the {@link LogicalOperator}.
         *
         * @param logicalOperator
         *            the logical operator to apply
         * @param items
         *            a list of items to include in the composite
         */
        public FilterItem(LogicalOperator logicalOperator, NList<FilterItem> items) :
            this(null, null, null, items, null, logicalOperator)
        {
            require("Child items cannot be null", _childItems != null);
            require("Child items cannot be empty", !NEnumerableUtils.IsEmpty<FilterItem>(_childItems));
        }

        /**
         * Creates a single filter item based on a SelectItem, an operator and an
         * operand.
         *
         * @param selectItem
         *            the selectItem to put constraints on, cannot be null
         * @param operator
         *            The operator to use. Can be OperatorType.EQUALS_TO,
         *            OperatorType.DIFFERENT_FROM,
         *            OperatorType.GREATER_THAN,OperatorType.LESS_THAN
         *            OperatorType.GREATER_THAN_OR_EQUAL,
         *            OperatorType.LESS_THAN_OR_EQUAL
         * @param operand
         *            The operand. Can be a constant like null or a String, a
         *            Number, a Boolean, a Date, a Time, a DateTime. Or another
         *            SelectItem
         * @throws IllegalArgumentException
         *             if the SelectItem is null or if the combination of operator
         *             and operand does not make sense.
         */
        public FilterItem(SelectItem selectItem, OperatorType operator_arg, object operand) // throws IllegalArgumentException
                : this(selectItem, operator_arg, operand, null, null, LogicalOperator.None)
        {

            if (_operand == null)
            {
                require("Can only use EQUALS or DIFFERENT_FROM operator with null-operand",
                        _operator == OperatorType.DIFFERENT_FROM || _operator == OperatorType.EQUALS_TO);
            }
            if (_operator == OperatorType.LIKE || _operator == OperatorType.NOT_LIKE)
            {
                ColumnType type = _selectItem.getColumn().getType();
                if (type != null)
                {
                    require("Can only use LIKE operator with strings", type.isLiteral()
                            && (_operand is String || _operand is SelectItem));
                }
            }
            require("SelectItem cannot be null", _selectItem != null);
        } // constructor

        /**
         * Creates a single unvalidated filter item based on a expression.
         * Expression based filters are typically NOT datastore-neutral but are
         * available for special "hacking" needs.
         *
         * Expression based filters can only be used for JDBC based datastores since
         * they are translated directly into SQL.
         *
         * @param expression
         *            An expression to use for the filter, for example
         *            "YEAR(my_date) = 2008".
         */
        public FilterItem(string expression) : this(null, null, null, null, expression, LogicalOperator.None)
        {
            require("Expression cannot be null", _expression != null);
        } // constructor


        public Query getQuery()
        {
            throw new NotImplementedException();
        }

        public string toString()
        {
            throw new NotImplementedException();
        }

        protected override void decorateIdentity(NList<object> identifiers)
        {
            throw new NotImplementedException();
        }

        protected object validateOperand(object operand)
        {
            if (operand is Column) {
                // gracefully convert to a select item.
                operand = new SelectItem((Column)operand);
            }
            return operand;
        }

        ///**
        // * Creates a compound filter item based on other filter items. Each provided
        // * filter items will be OR'ed meaning that if one of the evaluates as true,
        // * then the compound filter will be evaluated as true
        // *
        // * @param items
        // *            an array of items to include in the composite
        // */
        //public FilterItem(params FilterItem[] items) : this(Arrays.asList<FilterItem>(items))
        //{
        //}

        private void require(string errorMessage, bool b)
        {
            if (!b)
            {
                throw new ArgumentException(errorMessage);
            }
        }

        public SelectItem getSelectItem()
        {
            return _selectItem;
        }

        public OperatorType getOperator()
        {
            return _operator;
        }

        public Object getOperand()
        {
            return _operand;
        }

        public String getExpression()
        {
            return _expression;
        }

        public LogicalOperator getLogicalOperator()
        {
            return _logicalOperator;
        }

        public FilterItem setQuery(Query query)
        {
            _query = query;
            if (_childItems == null)
            {
                if (_expression == null)
                {
                    if (_selectItem.getQuery() == null)
                    {
                        _selectItem.setQuery(_query);
                    }
                    if (_operand is SelectItem) {
                        SelectItem operand = (SelectItem)_operand;
                        if (operand.getQuery() == null)
                        {
                            operand.setQuery(_query);
                        }
                    }
                }
            }
            else
            {
                foreach (FilterItem item in _childItems)
                {
                    if (item.getQuery() == null)
                    {
                        item.setQuery(_query);
                    }
                }
            }
            return this;
        }

        //@Override
        public String toSql()
        {
            return toSql(false);
        }

        /**
         * Parses the constraint as a SQL Where-clause item
         */
        //@Override
        public String toSql(bool includeSchemaInColumnPaths)
        {
            if (_expression != null)
            {
                return _expression;
            }

            StringBuilder sb = new StringBuilder();
            if (_childItems == null)
            {
                sb.Append(_selectItem.getSameQueryAlias(includeSchemaInColumnPaths));

                if (_operand == null && _operator == OperatorType.EQUALS_TO)
                {
                    sb.Append(" IS NULL");
                }
                else if (_operand == null && _operator == OperatorType.DIFFERENT_FROM)
                {
                    sb.Append(" IS NOT NULL");
                }
                else
                {
                    object operand = appendOperator(sb, _operand, _operator);

                    if (operand is SelectItem)
                    {
                        String selectItemString = ((SelectItem)operand)
                                                  .getSameQueryAlias(includeSchemaInColumnPaths);
                        sb.Append(selectItemString);
                    } 
                    else
                    {
                        ColumnType columnType = _selectItem.getExpectedColumnType();
                        string sqlValue = ""; // FormatHelper.formatSqlValue(columnType, operand);
                        sb.Append(sqlValue);
                    }
                }
            }
            else
            {
                sb.Append('(');
                for (int i = 0; i < _childItems.Count; i++)
                {
                    FilterItem item = _childItems[i];
                    if (i != 0)
                    {
                        sb.Append(' ');
                        sb.Append(_logicalOperator.ToString());
                        sb.Append(' ');
                    }
                    sb.Append(item.toSql());
                }
                sb.Append(')');
            }

            return sb.ToString();
        }

        public static Object appendOperator(StringBuilder sb, Object operand, OperatorType operator_arg)
        {
            sb.Append(' ');
            sb.Append(operator_arg.toSql());
            sb.Append(' ');

            if (operator_arg == OperatorType.IN || operator_arg == OperatorType.NOT_IN)
            {
                NList<object> operands = new NList<object>();
                operand = operands;
            }
            return operand;
        }

        /**
         * Does a "manual" evaluation, useful for CSV data and alike, where queries
         * cannot be created.
         */
        public bool evaluate(Row row)
        {
            require("Expression-based filters cannot be manually evaluated", _expression == null);

            if (_childItems == null)
            {
                // Evaluate a single constraint
                Object selectItemValue = row.getValue(_selectItem);
                Object operandValue    = _operand;

                if (_operand is SelectItem)
                {
                    SelectItem selectItem = (SelectItem)_operand;
                    operandValue = row.getValue(selectItem);
                }

                if (operandValue == null)
                {
                    if (_operator == OperatorType.DIFFERENT_FROM)
                    {
                        return (selectItemValue != null);
                    }
                    else if (_operator == OperatorType.EQUALS_TO)
                    {
                        return (selectItemValue == null);
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (selectItemValue == null)
                {
                    if (_operator == OperatorType.DIFFERENT_FROM)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return compare(selectItemValue, operandValue);
                }
            }
            else
            {

                // Evaluate several constraints
                if (_logicalOperator == LogicalOperator.AND)
                {
                    // require all results to be true
                    foreach (FilterItem item in _childItems)
                    {
                        bool result = item.evaluate(row);
                        if (! result)
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    // require at least one result to be true
                    foreach (FilterItem item in _childItems)
                    {
                        bool result = item.evaluate(row);
                        if (result)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
        }

        private bool compare(Object selectItemValue, Object operandValue)
        {
            IComparer<Object> comparator = ObjectComparator.getComparator();
            if (_operator == OperatorType.DIFFERENT_FROM)
            {
                return comparator.Compare(selectItemValue, operandValue) != 0;
            }
            else if (_operator == OperatorType.EQUALS_TO)
            {
                return comparator.Compare(selectItemValue, operandValue) == 0;
            }
            else if (_operator == OperatorType.GREATER_THAN)
            {
                return comparator.Compare(selectItemValue, operandValue) > 0;
            }
            else if (_operator == OperatorType.GREATER_THAN_OR_EQUAL)
            {
                return comparator.Compare(selectItemValue, operandValue) >= 0;
            }
            else if (_operator == OperatorType.LESS_THAN)
            {
                return comparator.Compare(selectItemValue, operandValue) < 0;
            }
            else if (_operator == OperatorType.LESS_THAN_OR_EQUAL)
            {
                return comparator.Compare(selectItemValue, operandValue) <= 0;
            }
            else if (_operator == OperatorType.LIKE)
            {
                //WildcardPattern matcher = new WildcardPattern((String)operandValue, '%');
                //return matcher.matches((String)selectItemValue);
                return false;
            }
            else if (_operator == OperatorType.NOT_LIKE)
            {
                //WildcardPattern matcher = new WildcardPattern((String)operandValue, '%');
                //return !matcher.matches((String)selectItemValue);
                return false;
            }
            else if (_operator == OperatorType.IN)
            {
                HashSet<object> inValues = getInValues();
                return inValues.Contains(selectItemValue);
            }
            else if (_operator == OperatorType.NOT_IN)
            {
                HashSet<object> inValues = getInValues();
                return !inValues.Contains(selectItemValue);
            }
            else
            {
                throw new InvalidOperationException("Operator could not be determined");
            }
        }

        /**
         * Lazy initializes a set (for fast searching) of IN values.
         *
         * @return a hash set appropriate for IN clause evaluation
         */
        private HashSet<object> getInValues()
        {
            if (_inValues == null)
            {
                if (_operand is HashSet<object>)
                {
                    _inValues = (HashSet<object>) _operand;
                }
                else
                {
                    NList<object> operands = new NList<object>();
                    operands.Add(_operand);
                    _inValues = new HashSet<Object>(operands);
                }
            }
            return _inValues;
        }

        // @Override
        protected FilterItem clone()
        {
            NList<FilterItem> orItems;
            if (_childItems == null)
            {
                orItems = null;
            }
            else
            {
                orItems = _childItems;
            }

            object operand;
            if (_operand is SelectItem)
            {
                operand = ((SelectItem)_operand).clone();
            }
            else
            {
                operand = _operand;
            }

            SelectItem selectItem;
            if (_selectItem == null)
            {
                selectItem = null;
            }
            else
            {
                selectItem = _selectItem.clone();
            }

            return new FilterItem(selectItem, _operator, operand, orItems, _expression, _logicalOperator);
        }

        public bool isReferenced(Column column)
        {
            if (column != null)
            {
                if (_selectItem != null)
                {
                    if (_selectItem.isReferenced(column))
                    {
                        return true;
                    }
                }
                if (_operand != null && _operand is SelectItem) {
                    if (((SelectItem)_operand).isReferenced(column))
                    {
                        return true;
                    }
                }
                if (_childItems != null)
                {
                    foreach (FilterItem item in _childItems)
                    {
                        if (item.isReferenced(column))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        //@Override
        protected void decorateIdentity(List<Object> identifiers)
        {
            identifiers.Add(_expression);
            identifiers.Add(_operand);
            identifiers.Add(_childItems);
            identifiers.Add(_operator);
            identifiers.Add(_selectItem);
            identifiers.Add(_logicalOperator);
        } // decorateIdentity

        /**
         * Gets the {@link FilterItem}s that this filter item consists of, if it is
         * a compound filter item.
         *
         * @deprecated use {@link #getChildItems()} instead
         */
        // @Deprecated
        public FilterItem[] getOrItems()
        {
            return getChildItems();
        }

        /**
         * Gets the number of child items, if this is a compound filter item.
         *
         * @deprecated use {@link #getChildItemCount()} instead.
         */
        //@Deprecated
        public int getOrItemCount()
        {
            return getChildItemCount();
        }

        /**
         * Get the number of child items, if this is a compound filter item.
         */
        public int getChildItemCount()
        {
            if (_childItems == null)
            {
                return 0;
            }
            return _childItems.Count;
        }

        /**
         * Gets the {@link FilterItem}s that this filter item consists of, if it is
         * a compound filter item.
         */
        public FilterItem[] getChildItems()
        {
            if (_childItems == null)
            {
                return null;
            }
            return _childItems.toArray(new FilterItem[_childItems.Count]);
        } // getChildItems()

        /**
         * Determines whether this {@link FilterItem} is a compound filter or not
         * (ie. if it has child items or not)
         */
        public bool isCompoundFilter()
        {
            return _childItems != null;
        }

        //@Override
        public override String ToString()
        {
            return toSql();
        }

        //@Override
        public bool accept(Row row)
        {
            return evaluate(row);
        }

        QueryItem QueryItem.setQuery(Query query)
        {
            throw new NotImplementedException();
        }
    } // FilterItem class
} // org.apache.metamodel.query
