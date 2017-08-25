﻿/**
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
// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/intercept/DataSetInterceptor.java
using org.apache.metamodel.core.data;
using System;
using System.Collections.Generic;
using System.Text;

namespace org.apache.metamodel.core.intercept
{
    /**
     * An {@link Interceptor} for {@link DataSet}s, allowing to touch, enrich or
     * modify a dataset before it is returned to the user.
     */
    public interface DataSetInterceptor : Interceptor<DataSet>
    {
    } // DataSetInterceptor interface
} // org.apache.metamodel.core.intercept
