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
    public class ReadCsvFile
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


        public DataTable GetDataTableFromCSVFile(string csv_file_path)
        {
            var csvData = new DataTable();
            try
            {
                using (var csvReader = new TextFieldParser(csv_file_path))
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
                    } //end foreach
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
                    } //end while
                } //end using
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

            foreach (var file in Directory.GetFiles(path, fileName))
            {
                var objType = Utilities.GetType(fileType);
                if (objType != null)
                {
                    //Get the properties for the class object type
                    PropertyInfo[] _filefields;
                    _filefields = objType.GetProperties();
                    //Create a list to store the data objects

                    //Get the data from the CSV file and populate the object listDataTable 
                    var tmpTable = GetDataTableFromCSVFile(file);
                    //the method we talked about at the beginning
                    foreach (DataRow dr in tmpTable.Rows)
                    {
                        var tmpObj = Activator.CreateInstance(objType);
                        foreach (var pinfo in _filefields)
                        {
                            //check to see if the table has a column with the specified field name
                            if (dr.Table.Columns.Contains(pinfo.Name))
                            {
                                //set the value of the object's property
                                tmpObj.SetPropertyValue(pinfo.Name, dr[pinfo.Name]);
                                // pinfo.SetValue(tmpObj, UtilitIES.ChangeType(dr[pinfo.Name], pinfo.PropertyType), null);
                            } //end if
                        } //end foreach
                        objList.Add(tmpObj);
                    } //end foreach


                    //Dispose of the object list since the data has been committed to the database
                    // objList = null;
                } //end if(!objType.Equals(null))
            } //end foreach

            return objList;
        }
    }
}