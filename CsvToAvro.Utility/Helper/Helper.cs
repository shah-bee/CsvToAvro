using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static long ConvertToLong(this string source)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;

            DateTime result = DateTime.ParseExact(source, new string[] { "dd.MM.yyyy", "dd-MM-yyyy", "dd-MM-yyyy" }, provider, DateTimeStyles.None);

            return result.ConvertDateToLong();

        }
    }
}
