using System;

namespace CsvToAvro.Utility
{
    public static class Utilities
    {
        public static Type GetType(string typeName)
        {
            var type = Type.GetType("CsvToAvro.Utility.Models." + typeName);
            if (type != null) return type;
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeName);
                if (type != null)
                    return type;
            }
            return null;
        }
    }
}