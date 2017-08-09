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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/data/ScalarFunctionRow.java
using org.apache.metamodel.data;
using org.apache.metamodel.query;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.apache.metamodel.core.data
{
    /**
     * A {@link Row} implementation that applies {@link ScalarFunction}s when
     * requested. This class closely interacts with the
     * {@link ScalarFunctionDataSet}.
     */
    public sealed class ScalarFunctionRow : AbstractRow
    {
        private static readonly long serialVersionUID = 1L;

        private ScalarFunctionDataSet _scalarFunctionDataSet;
        private Row                   _row;

        public ScalarFunctionRow(ScalarFunctionDataSet scalarFunctionDataSet, Row row)
        {
            _scalarFunctionDataSet = scalarFunctionDataSet;
            _row = row;
        } // constructor

        // @Override
        public override Object getValue(int index) // throws IndexOutOfBoundsException
        {
            List<SelectItem> scalarFunctionSelectItems = _scalarFunctionDataSet
                    .getScalarFunctionSelectItemsToEvaluate();
            int scalarFunctionCount = scalarFunctionSelectItems.Count;
            if (index >= scalarFunctionCount) {
                return _row.getValue(index - scalarFunctionCount);
            }
            SelectItem selectItem                = scalarFunctionSelectItems[index];
            SelectItem selectItemWithoutFunction = selectItem.replaceFunction(null);
            return selectItem.getScalarFunction().evaluate(_row, selectItem.getFunctionParameters(), selectItemWithoutFunction);
        } // getValue()

        //  @Override
        public override Style getStyle(int index) // throws IndexOutOfBoundsException
        {
            List<SelectItem> scalarFunctionSelectItems = _scalarFunctionDataSet
                    .getScalarFunctionSelectItemsToEvaluate();
            int scalarFunctionCount = scalarFunctionSelectItems.Count;
            if (index >= scalarFunctionCount) {
                _row.getStyle(index - scalarFunctionCount);
            }
            return StyleConstants.NO_STYLE;
        } // getStyle()

        // @Override
        public override DataSetHeader getHeader()
        {
            return _scalarFunctionDataSet.getHeader();
        }
    } // ScalarFunctionRow class
} //  org.apache.metamodel.core.data
