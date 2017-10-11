using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serilog.Sinks.InfluxDB
{
    public class InfluxDBDefaults
    {
        /// <summary>
        /// Default port for InfluxDB
        /// </summary>
        public const int DefaultPort = 8086;

        /// <summary>
        /// Default database name in InfluxDB
        /// </summary>
        public const string DefaultDbName = "LogDb";
    }
}
