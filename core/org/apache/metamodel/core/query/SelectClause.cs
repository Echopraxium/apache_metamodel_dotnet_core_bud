
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
 */// https://github.com/apache/metamodel/blob/master/core/src/main/java/org/apache/metamodel/query/SelectClause.java
using org.apache.metamodel.j2n.collections;
using org.apache.metamodel.query;
using org.apache.metamodel.schema;
using System;
using System.Text;

namespace org.apache.metamodel.core.query
{
    /**
     * Represents the SELECT clause of a query containing SelectItems.
     * 
     * @see SelectItem
     */
    public class SelectClause : AbstractQueryClause<SelectItem>
    {
        private static readonly long serialVersionUID = -2458447191169901181L;

        private bool _distinct = false;

        public SelectClause(Query query) :  base(query, PREFIX_SELECT, DELIM_COMMA)
        {
           
        }

        public SelectItem getSelectItem(Column column)
        {
            if (column != null)
            {
                foreach (SelectItem item in getItems())
                {
                    if (column.Equals(item.getColumn()))
                    {
                        return item;
                    }
                }
            }
            return default(SelectItem);
        } // getSelectItem()

        // @Override
        public override String toSql(bool? includeSchemaInColumnPaths)
        {
            if (getItems().Count == 0)
            {
                return "";
            }

            String        sql = base.toSql(includeSchemaInColumnPaths);
            StringBuilder sb = new StringBuilder(sql);
            if (_distinct)
            {
                sb.Insert(PREFIX_SELECT.Length, "DISTINCT ");
            }
            return sb.ToString();
        }

        public bool isDistinct()
        {
            return _distinct;
        }

        public void setDistinct(bool distinct)
        {
            _distinct = distinct;
        }

        // @Override
        public override void decorateIdentity(NList<Object> identifiers)
        {
            base.decorateIdentity(identifiers);
            identifiers.Add(_distinct);
        }
    }
}
