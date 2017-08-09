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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/data/ScalarFunctionDataSet.java
using org.apache.metamodel.data;
using org.apache.metamodel.j2n.collections;
using org.apache.metamodel.query;
using org.apache.metamodel.util;
using System;
using System.Collections.Generic;
using System.Text;
//import org.apache.metamodel.query.ScalarFunction;
//import org.apache.metamodel.query.SelectItem;
//import org.apache.metamodel.util.CollectionUtils;

namespace org.apache.metamodel.core.data
{
    /**
     * A {@link DataSet} that enhances another {@link DataSet} with
     * {@link ScalarFunction}s.
     */
    public class ScalarFunctionDataSet : AbstractDataSet, WrappingDataSet
    {
        private DataSet _dataSet;
        private List<SelectItem> _scalarFunctionSelectItemsToEvaluate;

        public ScalarFunctionDataSet(List<SelectItem> scalarFunctionSelectItemsToEvaluate, DataSet dataSet) :
                    base(CollectionUtils.concat(false, scalarFunctionSelectItemsToEvaluate,
                         NArrays.AsList(dataSet.getSelectItems())))
        {
            _scalarFunctionSelectItemsToEvaluate = scalarFunctionSelectItemsToEvaluate;
            _dataSet = dataSet;
        } // contructor

        // @Override
        public override bool next()
        {
            return _dataSet.next();
        }

        // @Override
        public override Row getRow()
        {
            Row row = _dataSet.getRow();
            return new ScalarFunctionRow(this, row);
        } // getRow()

        public List<SelectItem> getScalarFunctionSelectItemsToEvaluate()
        {
            return _scalarFunctionSelectItemsToEvaluate;
        }

        // @Override
        public DataSet getWrappedDataSet()
        {
            return _dataSet;
        }

    }
}
