using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LogLevel = NLog.LogLevel;

namespace CsvToAvro.Utility.Helper
{
    public static class Helper
    {
        /// <summary>
        /// As per the documentation this is considered as start.
        /// </summary>
        private static DateTime AvroStartDate = new DateTime(1970, 1, 1);
        public static long ConvertDateToLong(this DateTime source)
        {
            return (AvroStartDate - source).Days;
        }

        public static long ConvertToLong(this DataRow source, string columnName, LogWrapper Logger)
        {
            try
            {
                CultureInfo provider = CultureInfo.InvariantCulture;

                DateTime result = DateTime.ParseExact(source[columnName].ToString(), new[] { "dd.MM.yyyy", "dd-MM-yyyy", "dd-MM-yyyy" },
                    provider, DateTimeStyles.None);

                return result.ConvertDateToLong();
            }
            catch (Exception exception)
            {
                Logger.Log(LogLevel.Error, exception, $"unable to cast Field : {columnName} " + source[columnName]);
            }
            return 0;
        }
    }
}
