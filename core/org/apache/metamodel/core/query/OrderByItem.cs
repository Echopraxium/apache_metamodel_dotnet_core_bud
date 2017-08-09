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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/query/OrderByItem.java
using org.apache.metamodel.j2n;
using org.apache.metamodel.j2n.collections;
using org.apache.metamodel.query;
using org.apache.metamodel.util;
using System;
using System.Text;

namespace org.apache.metamodel.core.query
{
    /**
     * Represents an ORDER BY item. An OrderByItem sorts the resulting DataSet
     * according to a SelectItem that may or may not be a part of the query already.
     * 
     * @see OrderByClause
     * @see SelectItem
     */
    public class OrderByItem : BaseObject, QueryItem, NCloneable
    {
	    public enum Direction
        {
            None, ASC, DESC
        }

        private static readonly long serialVersionUID = -8397473619828484774L;

        private SelectItem _selectItem;
	    private Direction  _direction;
        private Query      _query;

        /**
	     * Creates an OrderByItem
	     * 
	     * @param selectItem
	     *            the select item to order
	     * @param direction
	     *            the direction to order the select item
	     */
        public OrderByItem(SelectItem selectItem, Direction direction)
        {
            if (selectItem == null)
            {
                throw new ArgumentException("SelectItem cannot be null");
            }
            _selectItem = selectItem;
            _direction = direction;
        } // constructor

        /**
	     * Creates an OrderByItem
	     * 
	     * @param selectItem
	     * @param ascending
	     * @deprecated user OrderByItem(SelectItem, Direction) instead
	     */
        // @Deprecated
        public OrderByItem(SelectItem selectItem, bool ascending)
        {
            if (selectItem == null)
            {
                throw new ArgumentException("SelectItem cannot be null");
            }
            _selectItem = selectItem;
            if (ascending)
            {
                _direction = Direction.ASC;
            }
            else
            {
                _direction = Direction.DESC;
            }
        } // constructor

        /**
	     * Creates an ascending OrderByItem
	     * 
	     * @param selectItem
	     */
        public OrderByItem(SelectItem selectItem) : this(selectItem, Direction.ASC)
        {   
        } // constructor


        // @Override
        public String toSql(bool? includeSchemaInColumnPaths)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_selectItem.getSameQueryAlias(includeSchemaInColumnPaths) + ' ');
            sb.Append(_direction);
            return sb.ToString();
        } // toSql()

        // @Override
        public String toSql()
        {
            return toSql(false);
        } // toSql()

        public bool isAscending()
        {
            return (_direction == Direction.ASC);
        }

        public bool isDescending()
        {
            return (_direction == Direction.DESC);
        }

        public Direction getDirection()
        {
            return _direction;
        }

        public OrderByItem setDirection(Direction direction)
        {
            _direction = direction;
            return this;
        }

        public SelectItem getSelectItem()
        {
            return _selectItem;
        }

        public Query getQuery()
        {
            return _query;
        }

        public QueryItem setQuery(Query query)
        {
            _query = query;
            if (_selectItem != null)
            {
                _selectItem.setQuery(query);
            }
            return this;
        } // setQuery()

        // @Override
        public OrderByItem clone()
        {
            OrderByItem o = new OrderByItem(_selectItem.clone());
            o._direction = _direction;
            return o;
        }

        // @Override
        public override void decorateIdentity(NList<Object> identifiers)
        {
            identifiers.Add(_direction);
            identifiers.Add(_selectItem);
        } // decorateIdentity()

        // @Override
        public String toString()
        {
            return toSql();
        }
    } // OrderByItem class
} // org.apache.metamodel.core.query namespace
