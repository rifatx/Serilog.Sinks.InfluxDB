using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serilog.Sinks.InfluxDB.ConsoleTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var influxDBLogger = new LoggerConfiguration()
                .WriteTo.InfluxDB("consoleApp", "http://localhost", "logDb")
                .Enrich.WithProperty("loggername", "ConsoleTester")
                .Enrich.WithThreadId()
                .CreateLogger();

            for (int i = 0; i < 100; i++)
            {
                influxDBLogger.Error(new Exception("the exception message " + i.ToString()), "error message " + i.ToString());
            }

            influxDBLogger.Dispose();
        }
    }
}
