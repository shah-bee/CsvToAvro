using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvToAvro.Utility
{
    public abstract class AbstractImporter
    {
        public List<DataTable> importedData;
        public List<object> objectList;
        public readonly string importDirectoryPath;
        public readonly string fileType;

        public abstract IEnumerable<object> Import();

        public AbstractImporter()
        {

        }
    }
}
