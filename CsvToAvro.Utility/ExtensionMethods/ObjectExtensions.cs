using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CsvToAvro.Utility.ExtensionMethods
{
    static class ObjectExtensions
    {
        public static void SetPropertyValue<T>(this object obj, string propertyName, T propertyValue)
        {
            PropertyInfo pi = obj.GetType().GetProperty(propertyName);

            if (pi != null && pi.CanWrite)
            {
                try
                {
                    pi.SetValue
                        (
                            obj,
                            Convert.ChangeType(propertyValue, pi.PropertyType),
                            null
                        );
                }
                //TODO: Log exception and continue execution
                catch (Exception ex)
                {
                    
                }
            }
        }
    }
}
