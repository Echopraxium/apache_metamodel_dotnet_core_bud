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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/query/FromItem.java
using org.apache.metamodel.j2n;
using org.apache.metamodel.j2n.collections;
using org.apache.metamodel.schema;
using org.apache.metamodel.util;
using System;
using System.Text;

namespace org.apache.metamodel.query
{
    /**
     * Represents a FROM item. FROM items can take different forms:
     * <ul>
     * <li>table FROMs (eg. "FROM products p")</li>
     * <lI>join FROMs with an ON clause (eg. "FROM products p INNER JOIN orders o ON
     * p.id = o.product_id")</li>
     * <li>subquery FROMs (eg. "FROM (SELECT * FROM products) p")</li>
     * <li>expression FROM (any string based from item)</li>
     * </ul>
     * 
     * @see FromClause
     */
    public class FromItem : BaseObject, QueryItem, NCloneable
    {
        private static readonly long serialVersionUID = -6559220014058975193L;

        private Table         _table;
        private string        _alias;
        private Query         _subQuery;
        private JoinType      _join;
        private FromItem      _leftSide;
        private FromItem      _rightSide;
        private SelectItem[]  _leftOn;
        private SelectItem[]  _rightOn;
        private Query         _query;
        private string        _expression;

        /**
         * Private constructor, used for cloning
         */
        private FromItem()
        {
        } // private constructor

        /**
         * Constructor for table FROM clauses
         */
        public FromItem(Table table)
        {
            _table = table;
        }

        /**
         * Constructor for sub-query FROM clauses
         * 
         * @param subQuery
         *            the subquery to use
         */
        public FromItem(Query subQuery)
        {
            _subQuery = subQuery;
        }

        /**
         * Constructor for join FROM clauses that join two tables using their
         * relationship. The primary table of the relationship will be the left side
         * of the join and the foreign table of the relationship will be the right
         * side of the join.
         * 
         * @param join
         *            the join type to use
         * @param relationship
         *            the relationship to use for joining the tables
         */
        public FromItem(JoinType join, Relationship relationship)
        {
            _join = join;
            _leftSide = new FromItem(relationship.getPrimaryTable());
            Column[] columns = relationship.getPrimaryColumns();
            _leftOn = new SelectItem[columns.Length];
            for (int i = 0; i < columns.Length; i++)
            {
                _leftOn[i] = new SelectItem(columns[i]);
            }
            _rightSide = new FromItem(relationship.getForeignTable());
            columns = relationship.getForeignColumns();
            _rightOn = new SelectItem[columns.Length];
            for (int i = 0; i < columns.Length; i++)
            {
                _rightOn[i] = new SelectItem(columns[i]);
            }
        }

        /**
         * Constructor for advanced join types with custom relationships
         * 
         * @param join
         *            the join type to use
         * @param leftSide
         *            the left side of the join
         * @param rightSide
         *            the right side of the join
         * @param leftOn
         *            what left-side select items to use for the ON clause
         * @param rightOn
         *            what right-side select items to use for the ON clause
         */
        public FromItem(JoinType join, FromItem leftSide, FromItem rightSide, SelectItem[] leftOn, SelectItem[] rightOn)
        {
            _join = join;
            _leftSide = leftSide;
            _rightSide = rightSide;
            _leftOn = leftOn;
            _rightOn = rightOn;
        }

        /**
         * Creates a single unvalidated from item based on a expression. Expression
         * based from items are typically NOT datastore-neutral but are available
         * for special "hacking" needs.
         * 
         * Expression based from items can only be used for JDBC based datastores
         * since they are translated directly into SQL.
         * 
         * @param expression
         *            An expression to use for the from item, for example "MYTABLE".
         */
        public FromItem(string expression)
        {
            if (expression == null)
            {
                throw new ArgumentException("Expression cannot be null");
            }
            _expression = expression;
        }

        public String getAlias()
        {
            return _alias;
        }

        public String getSameQueryAlias()
        {
            if (_alias != null)
            {
                return _alias;
            }
            if (_table != null)
            {
                return _table.getQuotedName();
            }
            return null;
        }

        public FromItem setAlias(String alias)
        {
            _alias = alias;
            return this;
        }

        public Table getTable()
        {
            return _table;
        }

        public Query getSubQuery()
        {
            return _subQuery;
        }

        public JoinType getJoin()
        {
            return _join;
        }

        public FromItem getLeftSide()
        {
            return _leftSide;
        }

        public FromItem getRightSide()
        {
            return _rightSide;
        }

        public SelectItem[] getLeftOn()
        {
            return _leftOn;
        }

        public SelectItem[] getRightOn()
        {
            return _rightOn;
        }

        public String getExpression()
        {
            return _expression;
        }

        public String toSql()
        {
            return toSql(false);
        }

        public String toSql(bool? includeSchemaInColumnPaths)
        {
            String stringNoAlias = toStringNoAlias(includeSchemaInColumnPaths);
            StringBuilder sb = new StringBuilder(stringNoAlias);
            if (_join != JoinType.None && _alias != null)
            {
                sb.Insert(0, '(');
                sb.Append(')');
            }
            if (_alias != null)
            {
                sb.Append(' ');
                sb.Append(_alias);
            }
            return sb.ToString();
        }

        public String toStringNoAlias()
        {
            return toStringNoAlias(false);
        }

        public String toStringNoAlias(bool? includeSchemaInColumnPaths)
        {
            if (_expression != null)
            {
                return _expression;
            }
            StringBuilder sb = new StringBuilder();
            if (_table != null)
            {
                if (_table.getSchema() != null && _table.getSchema().getName() != null)
                {
                    sb.Append(_table.getSchema().getName());
                    sb.Append('.');
                }
                sb.Append(_table.getQuotedName());
            }
            else if (_subQuery != null)
            {
                sb.Append('(');
                sb.Append(_subQuery.toSql(includeSchemaInColumnPaths));
                sb.Append(')');
            }
            else if (_join != JoinType.None)
            {
                String leftSideAlias  = _leftSide.getSameQueryAlias();
                String rightSideAlias = _rightSide.getSameQueryAlias();
                sb.Append(_leftSide.toSql());
                sb.Append(' ');
                sb.Append(_join);
                sb.Append(" JOIN ");
                sb.Append(_rightSide.toSql());
                for (int i = 0; i < _leftOn.Length; i++)
                {
                    if (i == 0)
                    {
                        sb.Append(" ON ");
                    }
                    else
                    {
                        sb.Append(" AND ");
                    }
                    SelectItem primary = _leftOn[i];
                    appendJoinOnItem(sb, leftSideAlias, primary);

                    sb.Append(" = ");

                    SelectItem foreign = _rightOn[i];
                    appendJoinOnItem(sb, rightSideAlias, foreign);
                }
            }
            return sb.ToString();
        } // toStringNoAlias()

        private void appendJoinOnItem(StringBuilder sb, String sideAlias, SelectItem onItem)
        {
            FromItem fromItem = onItem.getFromItem();
            if (fromItem != null && fromItem.getSubQuery() != null && fromItem.getAlias() != null)
            {
                // there's a corner case scenario where an ON item references a
                // subquery being joined. In that case the getSuperQueryAlias()
                // method will include the subquery alias.
                string super_query_alias = onItem.getSuperQueryAlias();
                sb.Append(super_query_alias);
                return;
            }

            if (_join != JoinType.None && _leftSide.getJoin() != JoinType.None)
            {
                sb.Append(onItem.toSql());
                return;
            }

            if (sideAlias != null)
            {
                sb.Append(sideAlias);
                sb.Append('.');
            }
            String superQueryAlias = onItem.getSuperQueryAlias();
            sb.Append(superQueryAlias);
        } // appendJoinOnItem()

        /**
         * Gets the alias of a table, if it is registered (and visible, ie. not part
         * of a sub-query) in the FromItem
         * 
         * @param table
         *            the table to get the alias for
         * @return the alias or null if none is found
         */
        public String getAlias(Table table)
        {
            String result = null;
            if (table != null)
            {
                // Search recursively through left and right side, unless they
                // are sub-query FromItems
                if (table.Equals(_table))
                {
                    result = _alias;
                }
                else if (_join != JoinType.None)
                {
                    result = _rightSide.getAlias(table);
                    if (result == null)
                    {
                        result = _leftSide.getAlias(table);
                    }
                }
            }
            return result;
        } // getAlias()

        public Query getQuery()
        {
            return _query;
        }

        public QueryItem setQuery(Query query)
        {
            _query = query;
            return this;
        }

        public FromItem clone()
        {
            FromItem f    = new FromItem();
            f._alias      = _alias;
            f._join       = _join;
            f._table      = _table;
            f._expression = _expression;
            if (_subQuery != null)
            {
                f._subQuery = _subQuery.clone();
            }
            if (_leftOn != null && _leftSide != null && _rightOn != null && _rightSide != null)
            {
                f._leftSide  = _leftSide.clone();
                f._leftOn    = (SelectItem[]) _leftOn.Clone();
                f._rightSide = _rightSide.clone();
                f._rightOn   = (SelectItem[]) _rightOn.Clone();
            }
            return f;
        } // clone()

        public override void decorateIdentity(NList<Object> identifiers)
        {
            identifiers.add(_table);
            identifiers.add(_alias);
            identifiers.add(_subQuery);
            identifiers.add(_join);
            identifiers.add(_leftSide);
            identifiers.add(_rightSide);
            identifiers.add(_leftOn);
            identifiers.add(_rightOn);
            identifiers.add(_expression);
        } // decorateIdentity()

        public override String ToString()
        {
            return toSql();
        }

        public string toString()
        {
            throw new NotImplementedException();
        }
    } // FromItem class
} // org.apache.metamodel.query
