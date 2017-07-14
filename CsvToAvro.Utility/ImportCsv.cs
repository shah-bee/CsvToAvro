using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using CsvToAvro.Utility.ExtensionMethods;
using CsvToAvro.Utility.Models;
using Microsoft.VisualBasic.FileIO;

namespace CsvToAvro.Utility
{
    public class ImportCsv
    {
        private List<DataTable> importedData;
        private List<object> objectList;
        private readonly string importDirectoryPath;
        private readonly string fileType;
        public ImportCsv(string directoryPath, string file)
        {
            importDirectoryPath = directoryPath;
            fileType = file;
            importedData = new List<DataTable>();
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

        private string DetermineTableName(string filePath)
        {
            switch (fileType.ToLower())
            {
                case "claim":
                    return DetermineTableNameForClaim(filePath);
                default:
                    return string.Empty;
            }
        }

        private string DetermineTableNameForClaim(string filePath)
        {
            if (filePath.ToLower().Contains("claimarrayclaimant"))
            {
                return "claimant";
            }
            else if (filePath.ToLower().Contains("claimarraypolicy"))
            {
                return "claimpolicy";
            }
            else if (filePath.ToLower().Contains("claimarraysection"))
            {
                return "claimsection";
            }
            else if (filePath.ToLower().Contains("claimarraytransactioncomponent"))
            {
                return "claimtransactioncomponent";
            }
            else if (filePath.ToLower().Contains("claimarraytransaction"))
            {
                return "claimtransaction";
            }
            else
            {
                return "claim";
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

            importedData = Directory.GetFiles(importDirectoryPath, filePath).Select(GetDataTableFromCsvFile).ToList();

            switch (fileType.ToLower())
            {
                case "claim":
                    return FillClaimObjects();
                case "policy":
                    break;
            }
            return Enumerable.Empty<object>();
        }

        private IEnumerable<Claim> FillClaimObjects()
        {
            var claimTable = importedData.FirstOrDefault(o => o.TableName.Equals("claim"));

            var claims = new List<Claim>();

            if (claimTable != null)
                foreach (DataRow row in claimTable.Rows)
                {
                    try
                    {
                        var claim = new Claim
                        {
                            KeyInternSchadenummer = row["KeyInternSchadenummer"].ToString(),
                            BackgroundNarrative = row["BackgroundNarrative"].ToString(),
                            CatastropheCode = row["CatastropheCode"].ToString(),
                            CatastropheDescription = row["CatastropheDescription"].ToString(),
                            ClaimCode = row["ClaimCode"].ToString(),
                            ClaimCountry = row["ClaimCountry"].ToString(),
                            ClaimDeniedIndicator = row["ClaimDeniedIndicator"].ToString(),
                            ClaimDescription = row["ClaimDescription"].ToString(),
                            ClaimDiary = row["ClaimDiary"].ToString(),
                            ClaimEventCode = row["ClaimEventCode"].ToString(),
                            ClaimEventDescription = row["ClaimEventDescription"].ToString(),
                            ClaimHandler = row["ClaimHandler"].ToString(),
                            ClaimHandlerCode = row["ClaimHandlerCode"].ToString(),
                            ClaimInsured = row["ClaimInsured"].ToString(),
                            ClaimLastModified = row["ClaimLastModified"].ToString(),
                            ClaimLeadIndicator =
                                (ClaimLeadIndicator)
                                    Enum.Parse(typeof(ClaimLeadIndicator), row["ClaimLeadIndicator"].ToString()),
                            ClaimLocationState = row["ClaimLocationState"].ToString(),
                            //ClaimOpenDate = Convert.ToInt64(row["ClaimOpenDate"]),
                            ClaimReference = row["ClaimReference"].ToString(),
                            //ClaimReportDate = Convert.ToInt64(row["ClaimReportDate"]),
                            ClaimStatus = row["ClaimStatus"].ToString(),
                            //ClaimYearOfAccount = Convert.ToInt64(row["ClaimYearOfAccount"]),
                            //CloseDate = Convert.ToInt64(row["CloseDate"]),
                            CoverageNarrative = row["CoverageNarrative"].ToString(),
                            CoverholderWithClaimsAuthority = row["CoverholderWithClaimsAuthority"].ToString(),
                            //DateOfDeclinature = Convert.ToInt64(row["DateOfDeclinature"]),
                            //DateOfLoss = Convert.ToInt64(row["DateOfLoss"]),
                            GeographicalOriginOfTheClaim = row["GeographicalOriginOfTheClaim"].ToString(),
                            LineageReference = row["LineageReference"].ToString(),
                            LitigationCode = row["LitigationCode"].ToString(),
                            LitigationDescription = row["LitigationDescription"].ToString(),
                            MaximumPotentialLoss = row["MaximumPotentialLoss"].ToString(),
                            MaximumPotentialLossCurrency = row["MaximumPotentialLossCurrency"].ToString(),
                            MaximumPotentialLossPercentage = row["MaximumPotentialLossPercentage"].ToString(),
                            OriginalCurrencyCode = row["OriginalCurrencyCode"].ToString(),
                            PreviousClaimReference = row["PreviousClaimReference"].ToString(),
                            PreviousSourceSystem = row["PreviousSourceSystem"].ToString(),
                            PreviousSourceSystemDescription = row["PreviousSourceSystemDescription"].ToString(),
                            ReasonDeclined = row["ReasonDeclined"].ToString(),
                            ReserveNarrative = row["ReserveNarrative"].ToString(),
                            ServiceProviderReference = row["ServiceProviderReference"].ToString(),
                            SettlementCurrencyCode = row["SettlementCurrencyCode"].ToString(),
                            SubrogationSalvageIndicator = row["SubrogationSalvageIndicator"].ToString(),
                            TPAHandleIndicator = row["TPAHandleIndicator"].ToString(),
                            TacticsNarrative = row["TacticsNarrative"].ToString(),
                            TriageCode = row["TriageCode"].ToString(),
                            XCSClaimCode = row["XCSClaimCode"].ToString(),
                            XCSClaimRef = row["XCSClaimRef"].ToString(),
                            Policy = new Policy
                            {
                                Section = new Section
                                {
                                    Transaction = new Transaction
                                    {
                                        TransactionComponent = new TransactionComponent()
                                    }
                                }
                            },
                            Claimant = GetClaimantsByClaimNumber(row["KeyInternSchadenummer"].ToString()).ToArray()

                        };
                        claims.Add(claim);
                    }
                    catch (Exception ex)
                    {
                        //TODO :: LOG EXCEPTION and continue creating the claim
                    }
                }
            if (claims.Any())
            {
                return claims;
            }

            return Enumerable.Empty<Claim>();
        }


        private IEnumerable<Claimant> GetClaimantsByClaimNumber(string claimNumber)
        {
            var claimantTable = importedData.FirstOrDefault(o => o.TableName.Equals("claimant"));

            var claimants = new List<Claimant>();

            if (claimantTable != null)
                claimants.AddRange(from DataRow row in claimantTable.Rows
                                   where claimNumber.Equals(row["KeyInternSchadenummer"].ToString())
                                   select new Claimant
                                   {
                                       ClaimantName = row["ClaimantName"].ToString(),
                                       ClaimantAddressArea = row["ClaimantAddressArea"].ToString(),
                                       ClaimantAddressCity = row["ClaimantAddressCity"].ToString(),
                                       ClaimantAddressStreet = row["ClaimantAddressStreet"].ToString(),
                                       ClaimantCode = row["ClaimantCode"].ToString(),
                                       ClaimantPostCode = row["ClaimantPostCode"].ToString()
                                   });
            return claimants;
        }

        //private void MapRowToObject(DataRow row, object target, object value = null)
        //{
        //    if (target != null)
        //    {
        //        PropertyInfo[] properties = value?.GetType().GetProperties() ?? target.GetType().GetProperties();

        //        foreach (var prop in properties)
        //        {
        //            PropertyInfo pi = value == null ? target.GetType().GetProperty(prop.Name) : value.GetType().GetProperty(prop.Name);

        //            if (HandleAsPrimitive(pi.PropertyType))
        //            {
        //                if (row.Table.Columns.Contains(prop.Name))
        //                {
        //                    //set the value of the object's property
        //                    if (value == null)
        //                    {
        //                        target.SetPropertyValue(prop.Name, row[prop.Name]);
        //                    }
        //                    else
        //                    {
        //                        value.SetPropertyValue(prop.Name, row[prop.Name]);
        //                    }
        //                    //.SetValue(target, value, null);
        //                }
        //            }
        //            else if (pi.PropertyType.IsClass)
        //            {
        //                object outerPropertyValue = prop.PropertyType.GetConstructor(new Type[] { }).Invoke(new object[] { });

        //                foreach (PropertyInfo info2 in prop.PropertyType.GetProperties())
        //                {
        //                    try
        //                    {
        //                        if (info2.CanWrite)
        //                        {
        //                            if (row.Table.Columns.Contains(info2.Name))
        //                            {
        //                                object innerPropertyValue = Convert.ChangeType(row[info2.Name], info2.PropertyType);
        //                                info2.SetValue(outerPropertyValue, innerPropertyValue, null);
        //                            }
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                    }
        //                }
        //                //MapRowToObject(row, target, outerPropertyValue);
        //                prop.SetValue(target, outerPropertyValue, null);
        //                //MapRowToObject(row, prop.Name);
        //            }
        //            //else if (pi.PropertyType.IsEnum)
        //            //{
        //            //    // TODO: Handle Enum values
        //            //}
        //        }
        //    }
        //}

        //private IEnumerable<object> SyncProperties(string propertyName)
        //{
        //    var objType = Utilities.GetType(propertyName);

        //    if (objType != null)
        //    {
        //        foreach (DataRow dr in importedData.Rows)
        //        {
        //            var targetObject = Activator.CreateInstance(objType);
        //            MapRowToObject(dr, targetObject);
        //            objectList.Add(targetObject);
        //        }
        //    }

        //    return objectList;
        //}

        //public static bool HandleAsPrimitive(Type type)
        //{
        //    bool isNullable = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        //    return isNullable || type.IsPrimitive || type.IsEnum || type == typeof(String) || type == typeof(Decimal) || type == typeof(DateTime);
        //}

        //private static bool SyncPropery(string propertyName, object source, object target)
        //{
        //    //keyInfo.ThrowIfArgumentNull("keyInfo");
        //    //source.ThrowIfArgumentNull("source");
        //    //target.ThrowIfArgumentNull("target");

        //    var sourceType = source.GetType();
        //    var propertyInfo = sourceType.GetProperty(propertyName);
        //    if (propertyInfo == null)
        //        return false;
        //    var propertyType = propertyInfo.PropertyType;
        //    var sourceValue = propertyInfo.GetValue(source);
        //    var targetValue = propertyInfo.GetValue(target);

        //    if (sourceValue != null && sourceValue.Equals(targetValue))
        //    {
        //        return false;
        //    }
        //    else if (HandleAsPrimitive(propertyType))
        //    {
        //        propertyInfo.SetValue(target, sourceValue);
        //    }
        //    //else if (propertyType.IsArray && keyInfo.IsArrayElement)
        //    //{
        //    //    SyncArrayElement(keyInfo, propertyInfo, target, sourceValue as object[], targetValue as object[]);
        //    //}
        //    //else if (sourceValue == null || targetValue == null)
        //    //{
        //    //    propertyInfo.SetValue(target, sourceValue);
        //    //}
        //    //else if (IsGenericList(propertyType))
        //    //{
        //    //    SyncListElement(keyInfo, propertyInfo, target, sourceValue as IList, targetValue as IList);
        //    //}
        //    else if (propertyType.IsClass)
        //    {
        //        SyncObject(sourceType.GetProperties(), sourceValue, targetValue);
        //    }
        //    else
        //    {
        //        throw new Exception("not handled: " + propertyType.Name);
        //    }

        //    return true;
        //}

        //private static void SyncObject(IEnumerable<PropertyInfo> keyInfos, object source, object target)
        //{
        //    foreach (var keyInfo in keyInfos)
        //    {
        //        SyncPropery(keyInfo.Name, source, target);
        //    }
        //}

    }
}
