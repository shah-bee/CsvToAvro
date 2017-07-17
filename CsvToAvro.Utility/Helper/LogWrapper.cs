using NLog;
using NLog.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvToAvro.Utility.Helper
{
    public class LogWrapper
    {
        public Logger logger = LogManager.GetLogger("CsvToAvro");
        public static int ErrorCount = 0;

        public LogWrapper(string LogLocation)
        {
            ErrorCount = 0;

            var config = new LoggingConfiguration();

            var fileTarget = new NLog.Targets.FileTarget();
            config.AddTarget("file", fileTarget);


            fileTarget.FileName = LogLocation + "/log.txt";
            fileTarget.Layout = "${longdate}|${level}|${message}|${exception:format = tostring}|${newLine}";

            var rule = new LoggingRule("*", LogLevel.Debug, fileTarget);
            config.LoggingRules.Add(rule);
            LogManager.Configuration = config;
        }



        public void Log(LogLevel level, Exception exception, string message)
        {
            logger.Log(level, exception, message);
            ErrorCount++;
        }

    }
}