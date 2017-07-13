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
        public IEnumerable<object> ImportAllFiles(string path, string fileType)
        {
            switch (fileType.ToLowerInvariant())
            {
                case "claim":
                    return FillObjectWithData(path, "EDF " + fileType + "*.csv", fileType);
                default:
                    return Enumerable.Empty<object>();
            }
        }

        /// <summary>
        /// Generates a consolidated datatable for the corresponding fileType, for example claim,policy,settlement etc.,
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public DataTable GetDataTableFromCsvFile(string fileName)
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

        private IEnumerable<object> FillObjectWithData(string path, string fileName, string fileType)
        {
            var objList = new List<object>();

            var dataTables = Directory.GetFiles(path, fileName).Select(GetDataTableFromCsvFile).ToList();

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

            var consolidatedDataTable = dataTables.MergeAll(primaryKey);

            var objType = Utilities.GetType(fileType);

            if (objType != null)
            {
                //Get the properties for the class object type
                PropertyInfo[] properties = objType.GetProperties();

                foreach (DataRow dr in consolidatedDataTable.Rows)
                {
                    var tmpObj = Activator.CreateInstance(objType);

                    foreach (var property in properties)
                    {
                        //check to see if the table has a column with the specified field name
                        if (dr.Table.Columns.Contains(property.Name))
                        {
                            //set the value of the object's property
                            tmpObj.SetPropertyValue(property.Name, dr[property.Name]);
                        }
                    }
                    objList.Add(tmpObj);
                }
            }
            return objList;
        }
    }
}