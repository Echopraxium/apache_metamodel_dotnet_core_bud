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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/schema/AbstractColumn.java
using org.apache.metamodel.j2n.data.numbers;
using org.apache.metamodel.schema;
using System;
using System.Text;

namespace org.apache.metamodel.core.schema
{
    /**
     * Abstract {@link Column} implementation. Implements most common and trivial
     * methods.
     */
    public abstract class AbstractColumn : Column
    {
        private static readonly long serialVersionUID = 1L;

        // @Override
        public String getQuotedName()
        {
            String quote = getQuote();
            if (quote == null)
            {
                return getName();
            }
            return quote + getName() + quote;
        }

        // @Override
        public String getQualifiedLabel()
        {
            StringBuilder sb = new StringBuilder();
            Table table = getTable();
            if (table != null)
            {
                sb.Append(table.getQualifiedLabel());
                sb.Append('.');
            }
            sb.Append(getName());
            return sb.ToString();
        } // getQualifiedLabel()

        // @Override
        public int compareTo(Column that)
        {
            int diff = getQualifiedLabel().CompareTo(that.getQualifiedLabel());
            if (diff == 0)
            {
                diff = toString().CompareTo(that.ToString());
            }
            return diff;
        }

        // @Override
        public String toString()
        {
            return "Column[name=" + getName() + ",columnNumber=" + getColumnNumber() + ",type=" + getType() + ",nullable="
                    + isNullable() + ",nativeType=" + getNativeType() + ",columnSize=" + getColumnSize() + "]";
        }

        // @Override
        public int hashCode()
        {
            return getName().GetHashCode();
        }

        // @Override
        public bool equals(Object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj == this)
            {
                return true;
            }
            if (obj is Column) {
                Column other = (Column)obj;
                if (getColumnNumber() != other.getColumnNumber())
                {
                    return false;
                }

                if (!getName().Equals(other.getName()))
                {
                    return false;
                }
                if (getType() != other.getType())
                {
                    return false;
                }

                Table table1 = getTable();
                Table table2 = other.getTable();
                if (table1 == null)
                {
                    if (table2 != null)
                    {
                        return false;
                    }
                }
                else
                {
                    if (! table1.Equals(table2))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public virtual int getColumnNumber()
        {
            throw new NotImplementedException();
        }

        public virtual ColumnType getType()
        {
            throw new NotImplementedException();
        }

        public virtual Table getTable()
        {
            throw new NotImplementedException();
        }

        public virtual bool? isNullable()
        {
            throw new NotImplementedException();
        }

        public virtual string getRemarks()
        {
            throw new NotImplementedException();
        }

        public virtual NInteger getColumnSize()
        {
            throw new NotImplementedException();
        }

        public virtual string getNativeType()
        {
            throw new NotImplementedException();
        }

        public virtual bool isIndexed()
        {
            throw new NotImplementedException();
        }

        public virtual bool isPrimaryKey()
        {
            throw new NotImplementedException();
        }

        public virtual string getQuote()
        {
            throw new NotImplementedException();
        }

        public virtual string getName()
        {
            throw new NotImplementedException();
        }
    } // AbstractColumn class
} // org.apache.metamodel.core.schema
