using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Hadoop.Avro.Container;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace CsvToAvro.Utility
{
    public class ExportToAvro
    {
        /// <summary>
        /// </summary>
        /// <param name="exportLocation"></param>
        /// <param name="values"></param>
        /// <param name="fileType"></param>
        public ExportToAvro(string exportLocation, IEnumerable<object> values, string fileType)
        {
            if (exportLocation == null) throw new ArgumentNullException(nameof(exportLocation));

            var result = values.Cast<Claim.Claim>();

            using (var buffer = new MemoryStream())
            {
                //Data is compressed using the Deflate codec.
                using (var w = AvroContainer.CreateWriter<Claim.Claim>(buffer, Codec.Deflate))
                {
                    using (var writer = new SequentialWriter<Claim.Claim>(w, 24))
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

        /// <summary>
        /// </summary>
        /// <param name="InputStream"></param>
        /// <param name="exportLocation"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        //Saving memory stream to a new file with the given path
        private bool WriteFile(MemoryStream InputStream, string exportLocation, string fileType)
        {
            exportLocation = exportLocation + "\\" + fileType + ".avro";

            if (!File.Exists(exportLocation))
            {
                try
                {
                    using (var fs = File.Create(exportLocation))
                    {
                        InputStream.Seek(0, SeekOrigin.Begin);
                        InputStream.CopyTo(fs);
                    }
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(
                        "The following exception was thrown during creation and writing to the file \"{0}\"",
                        exportLocation);
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
            Console.WriteLine("Can not create file \"{0}\". File already exists", exportLocation);
            return false;
        }
    }
}