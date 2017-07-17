using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvToAvro.Utility.Helper;
using CsvToAvro.Utility.Models;
using Microsoft.Hadoop.Avro.Container;
using LogLevel = NLog.LogLevel;

namespace CsvToAvro.Utility
{
    public class ExportToAvro
    {
        private readonly LogWrapper _logger;
        public static bool IsImported = false;

        /// <summary>
        /// </summary>
        /// <param name="exportLocation"></param>
        /// <param name="values"></param>
        /// <param name="fileType"></param>
        /// <param name="logger"></param>
        public ExportToAvro(string exportLocation, IEnumerable<Models.Claim> values, string fileType, LogWrapper logger)
        {
            _logger = logger;
            if (exportLocation == null) throw new ArgumentNullException(nameof(exportLocation));


            using (var buffer = new MemoryStream())
            {
                //Data is compressed using the Deflate codec.
                using (var w = AvroContainer.CreateWriter<Models.Claim>(buffer, Codec.Deflate))
                {
                    using (var writer = new SequentialWriter<Models.Claim>(w, 24))
                    {
                        // Serialize the data to stream by using the sequential writer
                        values.ToList().ForEach(writer.Write);
                    }
                }

                //Save stream to file
                Console.WriteLine("Saving serialized data to file...");
                if (!WriteFile(buffer, exportLocation, fileType))
                {
                    Console.WriteLine("Error during file operation. Quitting method");
                }
                else
                {
                    IsImported = true;
                }
            }

            //Reading and deserializing data
            //Creating a Memory Stream buffer
            using (var buffer = new MemoryStream())
            {
                Console.WriteLine("Reading data from file...");

                //Reading data from Object Container File
                if (!ReadFile(buffer, exportLocation + "\\" + fileType + "-" + DateTime.Now.ToShortDateString() + ".avro"))
                {
                    Console.WriteLine("Error during file operation. Quitting method");
                    return;
                }

                Console.WriteLine("Deserializing Sample Data Set...");

                //Prepare the stream for deserializing the data
                buffer.Seek(0, SeekOrigin.Begin);

                //Create a SequentialReader for type SensorData which will derserialize all serialized objects from the given stream
                //It allows iterating over the deserialized objects because it implements IEnumerable<T> interface
                using (var reader = new SequentialReader<Models.Claim>(
                    AvroContainer.CreateReader<Models.Claim>(buffer, true)))
                {
                    var results = reader.Objects;

                    //Finally, verify that deserialized data matches the original one
                    Console.WriteLine("Comparing Initial and Deserialized Data Sets...");
                    int count = 1;
                    var pairs = values.Zip(results, (serialized, deserialized) => new { expected = serialized, actual = deserialized }).ToList();
                    foreach (var pair in pairs)
                    {
                        bool isEqual = this.Equal(pair.expected, pair.actual);
                        Console.WriteLine("For Pair {0} result of Data Set Identity Comparison is {1}", count, isEqual.ToString());
                        count++;
                    }
                }
            }

        }

        private bool Equal(Models.Claim left, Models.Claim right)
        {
            return left.Equals(right);
        }

        //Reading a file content using given path to a memory stream
        private bool ReadFile(MemoryStream OutputStream, string path)
        {
            try
            {
                using (FileStream fs = File.Open(path, FileMode.Open))
                {
                    fs.CopyTo(OutputStream);
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("The following exception was thrown during reading from the file \"{0}\"", path);
                Console.WriteLine(e.Message);
                return false;
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

            Directory.CreateDirectory(exportLocation);

            exportLocation = exportLocation + "\\" + fileType + "-" + DateTime.Now.ToShortDateString() + ".avro";

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
                    _logger.Log(LogLevel.Error, e,
                        $"The following exception was thrown during creation and writing to the file{exportLocation}");
                    return false;
                }
            }

            return true;
        }
    }
}