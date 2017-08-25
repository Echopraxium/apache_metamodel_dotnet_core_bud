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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/data/DocumentSourceDataSet.java
using org.apache.metamodel.core.convert;
using org.apache.metamodel.data;
using org.apache.metamodel.j2n.slf4j;
using System;
using System.Collections.Generic;
using amm_data = org.apache.metamodel.core.data;

namespace org.apache.metamodel.core.data
{
    /**
     * A {@link DataSet} that uses a {@link DocumentSource} as it's source.
     */
    public class DocumentSourceDataSet : AbstractDataSet
    {

        private static readonly NLogger logger = NLoggerFactory.getLogger(typeof(DocumentSourceDataSet).Name);

        private DocumentSource             _document_source;
        private DocumentConverter          _converter;
        private volatile amm_data.Document _document;

        public DocumentSourceDataSet(DataSetHeader header, DocumentSource documentSource, DocumentConverter converter): base(header)
        {
            _document_source = documentSource;
            _converter       = converter;
        } // constructor

        //@Override
        public override bool next()
        {
            _document = _document_source.next();
            if (_document == null)
            {
                return false;
            }
            return true;
        }

        //@Override
        public override Row getRow()
        {
            if (_document == null)
            {
                return null;
            }
            DataSetHeader header = getHeader();
            return _converter.convert(_document, header);
        }

        //@Override
        public override void close()
        {
            base.close();
            try
            {
                _document_source.close();
            }
            catch (Exception e)
            {
                logger.warn("Failed to close DocumentSource: {}", _document, e);
            }
        } // close()
    } // DocumentSourceDataSet class
} //  org.apache.metamodel.core.data
