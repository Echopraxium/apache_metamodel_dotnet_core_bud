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
// https://github.com/qos-ch/slf4j/blob/master/slf4j-api/src/main/java/org/slf4j/LoggerFactory.java
namespace org.apache.metamodel.j2n.slf4j
{
    public class NLoggerFactory
    {
        private static NLoggerImpl _logger = new NLoggerImpl();

        /**
         * Return a logger named according to the name parameter using the
         * statically bound {@link ILoggerFactory} instance.
         * 
         * @param name
         *            The name of the logger.
         * @return logger
         */
        public static NLogger getLogger(string name)
        {
            // ILoggerFactory iLoggerFactory = getILoggerFactory();
            // return iLoggerFactory.getLogger(name);
            return _logger;
        }
    } // NLoggerFactory class
} // org.apache.metamodel.j2n.slf4j namespace
