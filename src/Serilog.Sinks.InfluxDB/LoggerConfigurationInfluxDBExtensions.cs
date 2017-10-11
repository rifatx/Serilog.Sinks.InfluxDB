// Copyright 2014 Serilog Contributors
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.InfluxDB;

namespace Serilog
{
    /// <summary>
    /// Adds the WriteTo.InfluxDB() extension method to <see cref="LoggerConfiguration"/>.
    /// </summary>
    public static class LoggerConfigurationInfluxDBExtensions
    {
        const string DefaultOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message} - {Properties}{NewLine}{Exception}{Properties}";

        public static LoggerConfiguration InfluxDB(
            this LoggerSinkConfiguration loggerConfiguration,
            string source,
            string address,
            int port,
            string dbName,
            string username,
            string password,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            int batchPostingLimit = InfluxDBSink.DefaultBatchPostingLimit,
            TimeSpan? period = null)
        {
            if (loggerConfiguration == null) throw new ArgumentNullException("loggerConfiguration");
            if (string.IsNullOrEmpty(address)) throw new ArgumentNullException("address");
            if (port <= 0) throw new ArgumentException("port");
            if (string.IsNullOrEmpty(dbName)) throw new ArgumentException("dbName");

            var connectionInfo = new InfluxDBConnectionInfo
            {
                Address = address,
                Port = port,
                DbName = dbName,
                Username = username,
                Password = password
            };

            return InfluxDB(loggerConfiguration, source,connectionInfo, restrictedToMinimumLevel, batchPostingLimit, period);
        }

        /// <summary>
        /// Adds the WriteTo.InfluxDB() extension method to <see cref="LoggerConfiguration"/>.
        /// </summary>
        public static LoggerConfiguration InfluxDB(
            this LoggerSinkConfiguration loggerConfiguration,
            string source,
            string address,
            string dbName,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            int batchPostingLimit = InfluxDBSink.DefaultBatchPostingLimit,
            TimeSpan? period = null)
        {
            var connectionInfo = new InfluxDBConnectionInfo
            {
                Address = address,
                Port = InfluxDBDefaults.DefaultPort,
                DbName = dbName,
                Username = string.Empty,
                Password = string.Empty
            };

            return InfluxDB(loggerConfiguration, source, connectionInfo, restrictedToMinimumLevel, batchPostingLimit, period);
        }

        /// <summary>
        /// Adds the WriteTo.InfluxDB() extension method to <see cref="LoggerConfiguration"/>.
        /// </summary>
        public static LoggerConfiguration InfluxDB(
            this LoggerSinkConfiguration loggerConfiguration,
            string source,
            string address,
            string dbName,
            string username,
            string password,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            int batchPostingLimit = InfluxDBSink.DefaultBatchPostingLimit,
            TimeSpan? period = null)
        {
            var connectionInfo = new InfluxDBConnectionInfo
            {
                Address = address,
                Port = InfluxDBDefaults.DefaultPort,
                DbName = dbName,
                Username = username,
                Password = password
            };

            return InfluxDB(loggerConfiguration, source, connectionInfo, restrictedToMinimumLevel, batchPostingLimit, period);
        }

        /// <summary>
        /// Adds the WriteTo.InfluxDB() extension method to <see cref="LoggerConfiguration"/>.
        /// </summary>
        public static LoggerConfiguration InfluxDB(
            this LoggerSinkConfiguration loggerConfiguration,
            string source,
            InfluxDBConnectionInfo connectionInfo,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            int batchPostingLimit = InfluxDBSink.DefaultBatchPostingLimit,
            TimeSpan? period = null)
        {
            if (connectionInfo == null) throw new ArgumentNullException("connectionInfo");

            var defaultedPeriod = period ?? InfluxDBSink.DefaultPeriod;

            return loggerConfiguration.Sink(
                new InfluxDBSink(connectionInfo, source, batchPostingLimit, defaultedPeriod),
                restrictedToMinimumLevel);
        }
    }
}