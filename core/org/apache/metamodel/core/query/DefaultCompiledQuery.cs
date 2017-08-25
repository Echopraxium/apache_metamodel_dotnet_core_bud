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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/query/DefaultCompiledQuery.java
using org.apache.metamodel.j2n.collections;
using org.apache.metamodel.j2n.data.numbers;
using org.apache.metamodel.query;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.apache.metamodel.core.query
{
    /**
     * Represents a default implementation of the {@link CompiledQuery} interface.
     * This implementation does not actually do anything to prepare the query, but
     * allows creating a clone of the originating query with the parameters replaced
     * by values.
     */
    public class DefaultCompiledQuery : CompiledQuery
    {
        private Query _query;
        private List<QueryParameter> _parameters;

        public DefaultCompiledQuery(Query query)
        {
            _query = query;
            _parameters = createParameterList();
        } // constructor

        /**
         * Clones the query while replacing query parameters with corresponding
         * values.
         * 
         * @param values
         * @return
         */
        public Query cloneWithParameterValues(Object[] values)
        {
            NAtomicInteger parameterIndex = new NAtomicInteger(0);
            Query clonedQuery = _query.clone();
            replaceParametersInQuery(values, parameterIndex, _query, clonedQuery);
            return clonedQuery;
        } // cloneWithParameterValues()

        private void replaceParametersInQuery(Object[] values, NAtomicInteger parameterIndex, 
                                              Query originalQuery, Query newQuery)
        {
            replaceParametersInFromClause(values,  parameterIndex, originalQuery, newQuery);
            replaceParametersInWhereClause(values, parameterIndex, originalQuery, newQuery);
        }

        private void replaceParametersInWhereClause(Object[] values, NAtomicInteger parameterIndex,
                Query originalQuery, Query newQuery)
        {
            // creates a clone of the original query, but rebuilds a completely new
            // where clause based on parameter values

            List<FilterItem> items = originalQuery.getWhereClause().getItems();
            int i = 0;
            foreach (FilterItem filterItem in items)
            {
                FilterItem newFilter = copyFilterItem(filterItem, values, parameterIndex);
                if (filterItem != newFilter)
                {
                    newQuery.getWhereClause().removeItem(i);
                    newQuery.getWhereClause().addItem(i, newFilter);
                }
                i++;
            }
        }

        private void replaceParametersInFromClause(Object[] values, NAtomicInteger parameterIndex, 
                                                   Query originalQuery, Query newQuery)
        {
            List<FromItem> fromItems = originalQuery.getFromClause().getItems();
            int i = 0;
            foreach (FromItem fromItem in fromItems)
            {
                Query subQuery = fromItem.getSubQuery();
                if (subQuery != null)
                {
                    Query newSubQuery = newQuery.getFromClause().getItem(i).getSubQuery();
                    replaceParametersInQuery(values, parameterIndex, subQuery, newSubQuery);

                    newQuery.getFromClause().removeItem(i);
                    newQuery.getFromClause().addItem(i, new FromItem(newSubQuery).setAlias(fromItem.getAlias()));
                }
                i++;
            }
        }

        private FilterItem copyFilterItem(FilterItem item, Object[] values, NAtomicInteger parameterIndex)
        {
            if (item.isCompoundFilter())
            {
                FilterItem[] childItems    = item.getChildItems();
                FilterItem[] newChildItems = new FilterItem[childItems.Length];
                for (int i = 0; i < childItems.Length; i++)
                {
                    FilterItem childItem = childItems[i];
                    FilterItem newChildItem = copyFilterItem(childItem, values, parameterIndex);
                    newChildItems[i] = newChildItem;
                }
                NList<FilterItem> elements = new NList<FilterItem>();
                foreach (FilterItem element in newChildItems)
                {
                    elements.Add(element);
                }
                FilterItem newFilter = new FilterItem(item.getLogicalOperator(), elements);
                return newFilter;
            }
            else
            {
                if (item.getOperand() is QueryParameter)                     
                {
                    Object newOperand = values[parameterIndex.getAndIncrement()];
                    FilterItem newFilter = new FilterItem(item.getSelectItem(), item.getOperator(), newOperand);
                    return newFilter;
                } 
                else 
                {
                    return item;
                }
            }
        } // copyFilterItem()

        private List<QueryParameter> createParameterList()
        {
            List<QueryParameter> parameters = new List<QueryParameter>();

            buildParameterListInFromClause(parameters,  _query);
            buildParameterListInWhereClause(parameters, _query);
            return parameters;
        } // createParameterList()

        private void buildParameterListInWhereClause(List<QueryParameter> parameters, Query query)
        {
            List<FilterItem> items = query.getWhereClause().getItems();
            foreach (FilterItem item in items)
            {
                buildParameterFromFilterItem(parameters, item);
            }
        } // buildParameterListInWhereClause()

        private void buildParameterListInFromClause(List<QueryParameter> parameters, Query query)
        {
            List<FromItem> fromItems = query.getFromClause().getItems();
            foreach (FromItem fromItem in fromItems)
            {
                Query subQuery = fromItem.getSubQuery();
                if (subQuery != null)
                {
                    buildParameterListInFromClause(parameters, subQuery);
                    buildParameterListInWhereClause(parameters, subQuery);
                }
            }
        }

        // @Override
        public List<QueryParameter> getParameters()
        {
            return _parameters;
        }

        // @Override
        public String toSql()
        {
            return _query.toSql();
        }

        // @Override
        public String toString()
        {
            return GetType().Name + "[" + toSql() + "]";
        }

        // @Override
        public void close()
        {
            // do nothing
        }

        private void buildParameterFromFilterItem(List<QueryParameter> parameters, FilterItem item)
        {
            if (item.isCompoundFilter())
            {
                FilterItem[] childItems = item.getChildItems();
                foreach (FilterItem childItem in childItems)
                {
                    buildParameterFromFilterItem(parameters, childItem);
                }
            }
            else
            {
                if (item.getOperand() is QueryParameter) {
                    parameters.Add((QueryParameter)item.getOperand());
                }
            }
        }
    }
}
