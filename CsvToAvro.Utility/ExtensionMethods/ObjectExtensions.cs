using System;
using System.Collections.Generic;
using System.Data;
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

        public static DataTable MergeAll(this IList<DataTable> tables, string primaryKeyColumn)
        {
            if (!tables.Any())
                throw new ArgumentException("Tables must not be empty", nameof(tables));
            if (primaryKeyColumn != null)
                if (tables.Any(t => !t.Columns.Contains(primaryKeyColumn)))
                {
                    throw new ArgumentException("All tables must have the specified primarykey column " + primaryKeyColumn, nameof(primaryKeyColumn));
                }

            if (tables.Count == 1)
                return tables[0];

            DataTable table = new DataTable("TblUnion");
            table.BeginLoadData(); // Turns off notifications, index maintenance, and constraints while loading data
            foreach (DataTable t in tables)
            {
                table.Merge(t); // same as table.Merge(t, false, MissingSchemaAction.Add);
            }
            table.EndLoadData();

            if (primaryKeyColumn != null)
            {
                // since we might have no real primary keys defined, the rows now might have repeating fields
                // so now we're going to "join" these rows ...
                var pkGroups = table.AsEnumerable()
                    .GroupBy(r => r[primaryKeyColumn]);
                var dupGroups = pkGroups.Where(g => g.Count() > 1);
                foreach (var grpDup in dupGroups)
                {
                    // use first row and modify it
                    DataRow firstRow = grpDup.First();
                    foreach (DataColumn c in table.Columns)
                    {
                        if (firstRow.IsNull(c))
                        {
                            DataRow firstNotNullRow = grpDup.Skip(1).FirstOrDefault(r => !r.IsNull(c));
                            if (firstNotNullRow != null)
                                firstRow[c] = firstNotNullRow[c];
                        }
                    }
                    // remove all but first row
                    var rowsToRemove = grpDup.Skip(1);
                    foreach (DataRow rowToRemove in rowsToRemove)
                        table.Rows.Remove(rowToRemove);
                }
            }

            return table;
        }
    }
}
