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
// https://github.com/apache/metamodel/blob/57e48be23844537690bdd6191ceb78adb4266e92/core/src/main/java/org/apache/metamodel/schema/AbstractRelationship.java
using org.apache.metamodel.j2n.collections;
using org.apache.metamodel.schema;
using org.apache.metamodel.util;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.apache.metamodel.core.schema
{
    public abstract class AbstractRelationship : BaseObject, Relationship
    {
        private static readonly long serialVersionUID = 1L;

        protected static Table checkSameTable(List<Column> columns)
        {
            if (columns == null || columns.Count == 0)
            {
                throw new ArgumentException(
                        "At least one key-column must exist on both "
                                + "primary and foreign side for "
                                + "a relation to exist.");
            }
            Table table = null;
            for (int i = 0; i < columns.Count; i++)
            {
                Column column = columns[i];
                if (i == 0)
                {
                    table = column.getTable();
                }
                else
                {
                    if (table != column.getTable())
                    {
                        throw new ArgumentException("Key-columns did not have same table");
                    }
                }
            }
            return table;
        }

        // @Override
        public Table getForeignTable()
        {
            return getForeignColumns()[0].getTable();
        }

        // @Override
        public Table getPrimaryTable()
        {
            return getPrimaryColumns()[0].getTable();
        }

        // @Override
        public String toString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Relationship[");
            sb.Append("primaryTable=" + getPrimaryTable().getName());
            List<Column> columns = NArrays.AsList<Column>(getPrimaryColumns());
            sb.Append(",primaryColumns=[");
            for (int i = 0; i < columns.Count; i++)
            {
                if (i != 0)
                {
                    sb.Append(", ");
                }
                sb.Append(columns[i].getName());
            }
            sb.Append("]");
            sb.Append(",foreignTable=" + getForeignTable().getName());
            columns = NArrays.AsList<Column>(getForeignColumns());
            sb.Append(",foreignColumns=[");
            for (int i = 0; i < columns.Count; i++)
            {
                if (i != 0)
                {
                    sb.Append(", ");
                }
                sb.Append(columns[i].getName());
            }
            sb.Append("]");
            sb.Append("]");
            return sb.ToString();
        }

        public int compareTo(Relationship that)
        {
            return toString().CompareTo(that.ToString());
        }

        // @Override
        protected void decorateIdentity(List<Object> identifiers)
        {
            identifiers.Add(getPrimaryColumns());
            identifiers.Add(getForeignColumns());
        }

        // @Override
        protected bool classEquals(BaseObject obj)
        {
            return obj is Relationship;
        }

        // @Override
        public bool containsColumnPair(Column pkColumn, Column fkColumn)
        {
            if (pkColumn != null && fkColumn != null)
            {
                List<Column> primaryColumns = NArrays.AsList<Column>(getPrimaryColumns());
                List<Column> foreignColumns = NArrays.AsList<Column>(getForeignColumns());
                for (int i = 0; i < primaryColumns.Count; i++)
                {
                    if (pkColumn.Equals(primaryColumns[i])
                            && fkColumn.Equals(foreignColumns[i]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public Column[] getPrimaryColumns()
        {
            throw new NotImplementedException();
        }

        public Column[] getForeignColumns()
        {
            throw new NotImplementedException();
        }
    }
}
