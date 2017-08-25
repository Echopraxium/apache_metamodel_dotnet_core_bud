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

// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/factory/ResourceFactoryRegistryImpl.java
using org.apache.metamodel.core.factory;
using org.apache.metamodel.j2n;
using org.apache.metamodel.j2n.exceptions;
using org.apache.metamodel.util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace org.apache.metamodel.core.factory
{
    public class ResourceFactoryRegistryImpl : ResourceFactoryRegistry
    {
        private static ResourceFactoryRegistry DEFAULT_INSTANCE = new ResourceFactoryRegistryImpl();

        public static ResourceFactoryRegistry getDefaultInstance()
        {
            return DEFAULT_INSTANCE;
        }

        private List<ResourceFactory> factories;

        public ResourceFactoryRegistryImpl()
        {
            factories = new List<ResourceFactory>();
        }

        //  @Override
        public void addFactory(ResourceFactory factory)
        {
            factories.Add(factory);
        }

        // @Override
        public void clearFactories()
        {
            factories.Clear();
        }

        // @Override
        public IList<ResourceFactory> getFactories()
        {
            ReadOnlyCollection<ResourceFactory> result = new ReadOnlyCollection<ResourceFactory>(factories);
            return result; // Collections.unmodifiableList(_tables);
        }

        //@Override
        public Resource createResource(ResourceProperties properties)
        {
            foreach (ResourceFactory factory in factories)
            {
                if (factory.accepts(properties))
                {
                    return factory.create(properties);
                }
            }
            throw new NUnsupportedResourcePropertiesException();
        }

        public void discoverFromClasspath()
        {
            NServiceLoader<ResourceFactory> serviceLoader = NServiceLoader<ResourceFactory>.load(typeof(ResourceFactory));
            foreach (ResourceFactory factory in serviceLoader) 
            {
                addFactory(factory);
            }
        } // discoverFromClasspath()
    } // ResourceFactoryRegistryImpl class
} //  org.apache.metamodel.core.factory
