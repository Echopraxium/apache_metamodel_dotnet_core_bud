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
// https://github.com/qos-ch/slf4j/blob/master/slf4j-api/src/main/java/org/slf4j/Logger.java
namespace org.apache.metamodel.j2n.slf4j
{
    public interface NLogger
    {
        /**
         * Log a message at the TRACE level.
         *
         * @param msg the message string to be logged
         * @since 1.4
         */
        void trace(string msg);

        /**
         * Log a message at the DEBUG level.
         *
         * @param msg the message string to be logged
         */
        void debug(string msg);

        /**
         * Log a message at the DEBUG level according to the specified format
         * and argument.
         * <p/>
         * <p>This form avoids superfluous object creation when the logger
         * is disabled for the DEBUG level. </p>
         *
         * @param format the format string
         * @param arg    the argument
         */
        void debug(string format, params object[] args);


        void warn(string msg);
        void warn(string format, params object[] args);


        void error(string format, params object[] args);


        void info(string msg, params object[] args);

        bool isInfoEnabled();

        bool isDebugEnabled();
    } // NLogger class
} // org.apache.metamodel.j2n.slf4j namespace
