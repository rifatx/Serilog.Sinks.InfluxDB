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

using System.ComponentModel;

namespace Serilog.Sinks.InfluxDB
{
    /// <summary>
    /// Connection information for use by the InfluxDB sink.
    /// </summary>
    public class InfluxDBConnectionInfo
    {
        /// <summary>
        /// Constructs the <see cref="InfluxDBConnectionInfo"/> with the default port and database name.
        /// </summary>
        public InfluxDBConnectionInfo()
        {
            Port = InfluxDBDefaults.DefaultPort;
            DbName = InfluxDBDefaults.DefaultDbName;
        }

        /// <summary>
        /// Address of InfluxDB instance.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the port used for the connection.
        /// Default value is 8086.
        /// </summary>
        [DefaultValue(InfluxDBDefaults.DefaultPort)]
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the database name in InfluxDB.
        /// Default value is logDb.
        /// </summary>
        [DefaultValue(InfluxDBDefaults.DefaultDbName)]
        public string DbName { get; set; }

        /// <summary>
        /// Gets or sets the username used for authentication.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password used for authentication.
        /// </summary>
        public string Password { get; set; }
    }
}