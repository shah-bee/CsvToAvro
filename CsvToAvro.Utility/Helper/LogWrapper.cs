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


        public LogWrapper(string LogLocation)
        {
            var config = new LoggingConfiguration();

            var fileTarget = new NLog.Targets.FileTarget();
            config.AddTarget("file", fileTarget);


            fileTarget.FileName = LogLocation + "/log.txt";
            fileTarget.Layout = "${longdate}|${level}|${message}|${exception:format = tostring}|${newLine}";

            var rule = new LoggingRule("*", LogLevel.Debug, fileTarget);
            config.LoggingRules.Add(rule);
            LogManager.Configuration = config;

        }


    }
}