using NesivosHakodesh.Core.Config;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace NesivosHakodesh.Providers.Utils
{
    public class Logger
    {
        private static ILogger _logger = null;
        private static ILogger SeriLog
        {
            get
            {
                if (_logger == null)
                {
                    _logger = new LoggerConfiguration()
                        .WriteTo
                        .File(AppSettingsProvider.GetOtherSettings().LogToFile, rollingInterval: RollingInterval.Day)
                        .CreateLogger();
                }
                return _logger;
            }
        }


        public static void Log(string log)
        {
            Debug.WriteLine(DateTime.Now + " :: " + log);

            SeriLog.Information(log);
        }
    }
}
