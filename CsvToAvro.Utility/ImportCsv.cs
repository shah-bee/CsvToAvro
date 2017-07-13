using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using CsvToAvro.Utility.ExtensionMethods;
using Microsoft.VisualBasic.FileIO;

namespace CsvToAvro.Utility
{
    public class ImportCsv
    {
        private DataTable importedData;
        private List<object> objectList;
        private readonly string importDirectoryPath;
        private readonly string fileType;
        public ImportCsv(string directoryPath, string file)
        {
            importDirectoryPath = directoryPath;
            fileType = file;
        }

        public IEnumerable<object> ImportAllFiles()
        {
            switch (fileType.ToLowerInvariant())
            {
                case "claim":
                    return FillObjectWithData("EDF " + fileType + "*.csv");
                default:
                    return Enumerable.Empty<object>();
            }
        }

        /// <summary>
        /// Generates a consolidated datatable for the corresponding fileType, for example claim,policy,settlement etc.,
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private DataTable GetDataTableFromCsvFile(string fileName)
        {
            var csvData = new DataTable();
            try
            {
                using (var csvReader = new TextFieldParser(fileName))
                {
                    csvReader.SetDelimiters("|");
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    //read column names from the first row
                    var colFields = csvReader.ReadFields();
                    //iterate each column to create the DataColumn for the DataTable structure
                    foreach (var column in colFields)
                    {
                        var datcolumn = new DataColumn(column);
                        csvData.Columns.Add(datcolumn);
                    }
                    //now on to the data
                    while (!csvReader.EndOfData)
                    {
                        var fieldData = csvReader.ReadFields();
                        ////Making empty value as null
                        //for (var i = 0; i < fieldData.Length; i++)
                        //{
                        //    if (fieldData[i] == "")
                        //        fieldData[i] = null;
                        //} //end for
                        //add the DataRow
                        csvData.Rows.Add(fieldData);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //add finally stuff here
            }
            return csvData;
        }

        private IEnumerable<object> FillObjectWithData(string filePath)
        {
            objectList = new List<object>();

            var dataTables = Directory.GetFiles(importDirectoryPath, filePath).Select(GetDataTableFromCsvFile).ToList();

            string primaryKey = string.Empty;

            switch (fileType.ToLower())
            {
                case "claim":
                    primaryKey = "KeyInternSchadenummer";
                    break;
                case "policy":
                    primaryKey = "KeyPolisId";
                    break;
            }

            importedData = dataTables.MergeAll(primaryKey);

            return SyncProperties(fileType);

        }

        private void MapRowToObject(DataRow row, string propertyName)
        {
            var objType = Utilities.GetType(propertyName);

            if (objType != null)
            {
                //Get the properties for the class object type
                PropertyInfo[] properties = objType.GetProperties();

                var tmpObj = Activator.CreateInstance(objType);

                foreach (var prop in properties)
                {
                    PropertyInfo pi = objType.GetProperty(prop.Name);

                    if (HandleAsPrimitive(pi.PropertyType))
                    {
                        if (row.Table.Columns.Contains(prop.Name))
                        {
                            //set the value of the object's property
                            tmpObj.SetPropertyValue(prop.Name, row[prop.Name]);
                        }
                    }
                  ////  else if (pi.PropertyType.IsClass)
                  //  {
                  //      MapRowToObject(row, prop.Name);
                  //  }
                    //else if (pi.PropertyType.IsEnum)
                    //{
                    //    // TODO: Handle Enum values
                    //}

                }
                objectList.Add(tmpObj);
            }
        }

        private IEnumerable<object> SyncProperties(string propertyName)
        {

            foreach (DataRow dr in importedData.Rows)
            {
                MapRowToObject(dr, propertyName);
            }

            return objectList;
        }

        public static bool HandleAsPrimitive(Type type)
        {
            bool isNullable = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
            return isNullable || type.IsPrimitive || type.IsEnum || type == typeof(String) || type == typeof(Decimal) || type == typeof(DateTime);
        }
    }
}
