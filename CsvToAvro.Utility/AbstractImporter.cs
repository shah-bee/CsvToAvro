using CsvToAvro.Utility.Helper;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace CsvToAvro.Utility
{
    public abstract class AbstractImporter
    {
        public List<DataTable> importedData;
        private readonly string importPath;
        private readonly string fileType;
        private readonly string fileName;
        private LogWrapper loggerWrapper;

        protected AbstractImporter(string importPath, string fileType, string fileName, LogWrapper logger)
        {
            this.importPath = importPath;
            this.fileType = fileType;
            this.fileName = fileName;
            this.loggerWrapper = logger;
            ReadData();
        }

        public abstract void Import();

        private void ReadData()
        {
            try
            {
                importedData = Directory.GetFiles(importPath, fileName).Select(GetDataTableFromCsvFile).ToList();
                if (!importedData.Any())
                {
                    loggerWrapper.Log(LogLevel.Fatal, null, "Data not found to be imported!");
                }
            }
            catch (DirectoryNotFoundException exception)
            {
                loggerWrapper.Log(LogLevel.Fatal, exception, "Directory not found!");
            }
        }

        /// <summary>
        /// Generates a consolidated datatable for the corresponding fileType, for example claim,policy,settlement etc.,
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private DataTable GetDataTableFromCsvFile(string fileName)
        {
            var csvData = new DataTable(DetermineTableName(fileName));
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
                        csvData.Rows.Add(fieldData);
                    }
                }
            }
            catch (Exception ex)
            {
                loggerWrapper.logger.Log(LogLevel.Error, "", ex);
            }

            return csvData;
        }

        private string DetermineTableName(string filePath)
        {
            if (filePath.ToLower().Contains("claimarrayclaimant"))
            {
                return Constants.ClaimClaimant;
            }
            else if (filePath.ToLower().Contains("claimarraypolicy"))
            {
                return Constants.ClaimPolicy;
            }
            else if (filePath.ToLower().Contains("claimarraysection"))
            {
                return Constants.ClaimSection;
            }
            else if (filePath.ToLower().Contains("claimarraytransactioncomponent"))
            {
                return Constants.ClaimTransactionComponent;
            }
            else if (filePath.ToLower().Contains("claimarraytransaction"))
            {
                return Constants.ClaimTransaction;
            }
            else
            {
                return Constants.Claim;
            }
        }
    }
}
