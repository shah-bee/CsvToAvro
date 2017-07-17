using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvToAvro.Utility.Helper;
using Microsoft.Hadoop.Avro.Container;
using NLog;

namespace CsvToAvro.Utility
{
    public class ExportToAvro
    {
        public bool IsImported = false;
        private readonly string exportLocation;
        private readonly IEnumerable<Models.Claim> values;
        private readonly string fileType;
        private readonly LogWrapper logger;

        /// <summary>
        /// Export the data to the specified location with avro.
        /// </summary>
        /// <param name="exportLocation"></param>
        /// <param name="values"></param>
        /// <param name="fileType"></param>
        /// <param name="logger"></param>
        public ExportToAvro(string exportLocation, IEnumerable<Models.Claim> values, string fileType, LogWrapper logger)
        {
            this.exportLocation = exportLocation;
            this.values = values;
            this.fileType = fileType;
            this.logger = logger;
        }

        public bool Export()
        {
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
                if (!WriteFile(buffer))
                {
                    Console.WriteLine("Error during file operation. Quitting method");
                }
                else
                {
                    IsImported = true;
                }
            }

            return IsImported;
        }
        
        //Saving memory stream to a new file with the given path
        private bool WriteFile(MemoryStream inputStream)
        {
            Directory.CreateDirectory(exportLocation);

            var exportPath = exportLocation + "\\" + fileType + "-" + DateTime.Now.ToShortDateString() + ".avro";

            if (!File.Exists(exportPath))
            {
                try
                {
                    using (var fs = File.Create(exportPath))
                    {
                        inputStream.Seek(0, SeekOrigin.Begin);
                        inputStream.CopyTo(fs);
                    }
                    return true;
                }
                catch (Exception e)
                {
                    logger.Log(LogLevel.Error, e,
                        $"The following exception was thrown during creation and writing to the file{exportLocation}");
                    return false;
                }
            }

            return true;
        }

        //using (var buffer = new MemoryStream())
        ////Creating a Memory Stream buffer

        ////Reading and deserializing data

        //// Uncomment to check for the deserailization of the avro file.
        //{
        //    Console.WriteLine("Reading data from file...");

        //    //Reading data from Object Container File
        //    if (!ReadFile(buffer, exportLocation + "\\" + fileType + "-" + DateTime.Now.ToShortDateString() + ".avro"))
        //    {
        //        Console.WriteLine("Error during file operation. Quitting method");
        //        return;
        //    }

        //    Console.WriteLine("Deserializing Sample Data Set...");

        //    //Prepare the stream for deserializing the data
        //    buffer.Seek(0, SeekOrigin.Begin);

        //    //Create a SequentialReader for type SensorData which will derserialize all serialized objects from the given stream
        //    //It allows iterating over the deserialized objects because it implements IEnumerable<T> interface
        //    using (var reader = new SequentialReader<Models.Claim>(
        //        AvroContainer.CreateReader<Models.Claim>(buffer, true)))
        //    {
        //        var results = reader.Objects;

        //        //Finally, verify that deserialized data matches the original one
        //        Console.WriteLine("Comparing Initial and Deserialized Data Sets...");
        //        int count = 1;
        //        var pairs = values.Zip(results, (serialized, deserialized) => new { expected = serialized, actual = deserialized }).ToList();
        //        foreach (var pair in pairs)
        //        {
        //            bool isEqual = this.Equal(pair.expected, pair.actual);
        //            Console.WriteLine("For Pair {0} result of Data Set Identity Comparison is {1}", count, isEqual.ToString());
        //            count++;
        //        }
        //    }
        //}
    }

    //private bool Equal(Models.Claim left, Models.Claim right)
    //{
    //    return left.Equals(right);
    //}

    ////Reading a file content using given path to a memory stream
    //private bool ReadFile(MemoryStream OutputStream, string path)
    //{
    //    try
    //    {
    //        using (FileStream fs = File.Open(path, FileMode.Open))
    //        {
    //            fs.CopyTo(OutputStream);
    //        }
    //        return true;
    //    }
    //    catch (Exception e)
    //    {
    //        Console.WriteLine("The following exception was thrown during reading from the file \"{0}\"", path);
    //        Console.WriteLine(e.Message);
    //        return false;
    //    }
    //}
}