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
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.PeriodicBatching;
using Serilog.Formatting.Display;
using System.Linq;
using InfluxData.Net.InfluxDb;
using InfluxData.Net.Common.Enums;
using InfluxData.Net.InfluxDb.Infrastructure;
using InfluxData.Net.InfluxDb.Models;

namespace Serilog.Sinks.InfluxDB
{
    class InfluxDBSink : PeriodicBatchingSink //InfluxDBSink
    {
        readonly string _source;

        readonly InfluxDBConnectionInfo _connectionInfo;

        readonly InfluxDbClient _influxDbClient;

        /// <summary>
        /// A reasonable default for the number of events posted in
        /// each batch.
        /// </summary>
        public const int DefaultBatchPostingLimit = 100;

        /// <summary>
        /// A reasonable default time to wait between checking for event batches.
        /// </summary>
        public static readonly TimeSpan DefaultPeriod = TimeSpan.FromSeconds(30);

        /// <summary>
        /// Construct a sink emailing with the specified details.
        /// </summary>
        /// <param name="connectionInfo">Connection information used to construct the SMTP client and mail messages.</param>
        /// <param name="batchSizeLimit">The maximum number of events to post in a single batch.</param>
        /// <param name="period">The time to wait between checking for event batches.</param>
        /// <param name="textFormatter">Supplies culture-specific formatting information, or null.</param>
        /// <param name="subjectLineFormatter">Supplies culture-specific formatting information, or null.</param>
        public InfluxDBSink(InfluxDBConnectionInfo connectionInfo, string source, int batchSizeLimit, TimeSpan period)
            : base(batchSizeLimit, period)
        {
            if (connectionInfo == null)
            {
                throw new ArgumentNullException(nameof(connectionInfo));
            }

            _source = source;
            _connectionInfo = connectionInfo;
            _influxDbClient = CreateInfluxDbClient();

            CreateDatabase();
        }

        /// <summary>
        /// Free resources held by the sink.
        /// </summary>
        /// <param name="disposing">If true, called because the object is being disposed; if false,
        /// the object is being disposed from the finalizer.</param>
        protected override void Dispose(bool disposing)
        {
            // First flush the buffer
            base.Dispose(disposing);
        }

        /// <summary>
        /// Emit a batch of log events, running asynchronously.
        /// </summary>
        /// <param name="events">The events to emit.</param>
        /// <remarks>Override either <see cref="PeriodicBatchingSink.EmitBatch"/> or <see cref="PeriodicBatchingSink.EmitBatchAsync"/>,
        /// not both.</remarks>
        protected override async Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {
            if (events == null)
                throw new ArgumentNullException(nameof(events));

            var payload = new StringWriter();
            List<Point> points = new List<Point>(events.Count());

            foreach (var logEvent in events)
            {
                var p = new Point();

                p.Name = _source;
                p.Fields = logEvent.Properties.ToDictionary(k => k.Key, v => (object)v.Value.ToString());

                if (logEvent.Exception != null)
                {
                    p.Tags.Add("exception", logEvent.Exception.ToString());
                }

                p.Tags.Add("level", logEvent.Level.ToString());

                if (logEvent.MessageTemplate != null)
                {
                    p.Tags.Add("messageTemplate", logEvent.MessageTemplate.Text);
                }

                p.Timestamp = logEvent.Timestamp.UtcDateTime;

                points.Add(p);
            }

            Console.WriteLine(payload.ToString());

            await _influxDbClient.Client.WriteAsync(points, _connectionInfo.DbName);
        }
        private InfluxDbClient CreateInfluxDbClient()
        {
            return new InfluxDbClient(
                string.Format("{0}:{1}", _connectionInfo.Address, _connectionInfo.Port),
                _connectionInfo.Username,
                _connectionInfo.Password,
                InfluxDbVersion.Latest);
        }
        private void CreateDatabase()
        {
            var dbList = _influxDbClient.Database.GetDatabasesAsync().Result;
            if (!dbList.Any(db => db.Name == _connectionInfo.DbName))
            {
                var _ = _influxDbClient.Database.CreateDatabaseAsync(_connectionInfo.DbName).Result;
            }
        }
    }
}
