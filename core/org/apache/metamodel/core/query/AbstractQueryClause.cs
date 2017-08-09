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
// https://github.com/apache/metamodel/blob/master/core/src/main/java/org/apache/metamodel/query/AbstractQueryClause.java
using org.apache.metamodel.j2n.collections;
using org.apache.metamodel.query;
using org.apache.metamodel.util;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.apache.metamodel.core.query
{
    /**
     * Represents an abstract clause in a query. Clauses contains IQueryItems and
     * provide basic ways of adding, modifying and removing these.
     * 
     * @param <E>
     *            the type of query item this QueryClause handles
     * 
     * @see Query
     */
    public abstract class AbstractQueryClause<E> : BaseObject // where E : QueryItem, QueryClause<E>
    {
        private static readonly long serialVersionUID = 3987346267433022231L;

        public static readonly String PREFIX_SELECT   = "SELECT ";
        public static readonly String PREFIX_FROM     = " FROM ";
        public static readonly String PREFIX_WHERE    = " WHERE ";
        public static readonly String PREFIX_GROUP_BY = " GROUP BY ";
        public static readonly String PREFIX_HAVING   = " HAVING ";
        public static readonly String PREFIX_ORDER_BY = " ORDER BY ";
        public static readonly String DELIM_COMMA     = ", ";
        public static readonly String DELIM_AND       = " AND ";

        private Query             _query;
        private readonly List<E>  _items = new List<E>();
        private String            _prefix;
        private String            _delim;

        public AbstractQueryClause(Query query, String prefix, String delim)
        {
            _query = query;
            _prefix = prefix;
            _delim = delim;
        } // constructor

        // @Override
        public QueryClause<E> setItems(/*@SuppressWarnings("unchecked")*/ params E[] items)
        {
            _items.Clear();
            return addItems(items);
        } // setItems()

        // @Override
        public QueryClause<E> addItems(/*@SuppressWarnings("unchecked")*/ params E[] items) 
        {
            foreach (E item in items) 
            {
                addItem(item);
            }
            return (QueryClause<E>) this;
        } // addItems()

        // @Override
        public QueryClause<E> addItems(IEnumerable<E> items)
        {
            foreach (E item in items)
            {
                addItem(item);
            }
            return (QueryClause<E>) this;
        } // addItems()

        public QueryClause<E> addItem(int index, E item)
        {
            if (((QueryItem)item).getQuery() == null)
            {
                ((QueryItem)item).setQuery(_query);
            }
            _items.Insert(index, item);
            return (QueryClause<E>) this;
        } // addItem()

        // @Override
        public QueryClause<E> addItem(E item)
        {
            return addItem(getItemCount(), item);
        } // addItem()

        // @Override
        public int getItemCount()
        {
            return _items.Count;
        }

        // @Override
        public int indexOf(E item)
        {
            return _items.IndexOf(item);
        }

        // @Override
        public bool isEmpty()
        {
            return getItemCount() == 0;
        }

        // @Override
        public E getItem(int index)
        {
            return _items[index];
        }

        // @Override
        public List<E> getItems()
        {
            return _items;
        }

        // @Override
        public QueryClause<E> removeItem(int index)
        {
            _items.RemoveAt(index);
            return (QueryClause<E>) this;
        }

        // @Override
        public QueryClause<E> removeItem(E item)
        {
            _items.Remove(item);
            return (QueryClause<E>) this;
        }

        // @Override
        public QueryClause<E> removeItems()
        {
            _items.Clear();
            return (QueryClause<E>)this;
        }

        // @Override
        public virtual String toSql()
        {
            return toSql(false);
        } // toSql()

        // @Override
        public virtual String toSql(bool? includeSchemaInColumnPaths)
        {
            if (_items.Count == 0)
            {
                return "";
            }
            StringBuilder sb = new StringBuilder(_prefix);
            for (int i = 0; i < _items.Count; i++)
            {
                E item = _items[i];
                if (i != 0)
                {
                    sb.Append(_delim);
                }
                String sql = ((QueryItem)item).toSql(includeSchemaInColumnPaths);
                sb.Append(sql);
            }
            return sb.ToString();
        } // toSql()

        // @Override
        public override String ToString()
        {
            return toSql();
        }

        // @Override
        public override void decorateIdentity(NList<Object> identifiers)
        {
            identifiers.Add(_items);
        }
    } // AbstractQueryClause class
} // org.apache.metamodel.core.query namespace
