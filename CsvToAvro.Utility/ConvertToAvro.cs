using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvToAvro.Utility.Models;
using Microsoft.Hadoop.Avro.Container;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace CsvToAvro.Utility
{
    public class ConvertToAvro
    {

        public ConvertToAvro(string exportLocation, IEnumerable<object> values, string fileType)
        {
            if (exportLocation == null) throw new ArgumentNullException(nameof(exportLocation));

            IEnumerable<Claim> result = values.Cast<Claim>();

            using (var buffer = new MemoryStream())
            {
                //Data is compressed using the Deflate codec.
                using (var w = AvroContainer.CreateWriter<Claim>(buffer, Codec.Deflate))
                {
                    using (var writer = new SequentialWriter<Claim>(w, 24))
                    {
                        // Serialize the data to stream by using the sequential writer
                        result.ForEach(writer.Write);
                    }
                }

                //Save stream to file
                Console.WriteLine("Saving serialized data to file...");
                if (!WriteFile(buffer, exportLocation, fileType))
                {
                    Console.WriteLine("Error during file operation. Quitting method");
                }
            }
        }


        //Saving memory stream to a new file with the given path
        private bool WriteFile(MemoryStream InputStream, string path, string fileType)
        {
            path = path + "\\" + fileType + ".avro";

            if (!File.Exists(path))
            {
                try
                {
                    using (FileStream fs = File.Create(path))
                    {
                        InputStream.Seek(0, SeekOrigin.Begin);
                        InputStream.CopyTo(fs);
                    }
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("The following exception was thrown during creation and writing to the file \"{0}\"", path);
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Can not create file \"{0}\". File already exists", path);
                return false;

            }
        }
    }
}
