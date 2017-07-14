using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvToAvro.Utility.Helper;
using CsvToAvro.Utility.Models;
using Microsoft.VisualBasic.FileIO;

namespace CsvToAvro.Utility.Claim
{
    class ClaimImporter :AbstractImporter
    {

        public ClaimImporter()
        {
            
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

        private IEnumerable<Models.Claim> FillClaimObjects()
        {
            var claimTable = importedData.FirstOrDefault(o => o.TableName.Equals(Constants.Claim));

            var claims = new List<Models.Claim>();

            if (claimTable != null)
                foreach (DataRow row in claimTable.Rows)
                {
                    try
                    {
                        var claim = new Models.Claim
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
                            Policy = GetPolicyByClaimNumber(row["KeyInternSchadenummer"].ToString()),
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

            return Enumerable.Empty<Models.Claim>();
        }

        private Policy GetPolicyByClaimNumber(string claimNumber)
        {
            var policyTable = importedData.FirstOrDefault(o => o.TableName.Equals(Constants.ClaimPolicy));

            var policy = new Policy();
            if (policyTable != null)
                foreach (DataRow row in policyTable.Rows)
                {
                    try
                    {
                        if (claimNumber.Equals(row["KeyInternSchadenummer"]))
                        {
                            policy.Section = GetSectionByClaimNumber(claimNumber).ToArray();
                            policy.PolicyCode = row["PolicyCode"].ToString();
                            policy.PolicyReference = row["PolicyReference"].ToString();
                            policy.SectionCode = row["SectionCode"].ToString();
                            policy.SectionReference = row["SectionReference"].ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        //// TODO : Log exception
                    }
                }

            return policy;
        }

        private IEnumerable<Section> GetSectionByClaimNumber(string claimNumber)
        {
            var sectionTable = importedData.FirstOrDefault(o => o.TableName.Equals(Constants.ClaimSection));

            var sections = new List<Section>();

            if (sectionTable != null)
                foreach (DataRow row in sectionTable.Rows)
                {
                    try
                    {
                        if (claimNumber.Equals(row["KeyInternSchadenummer"]))
                        {
                            var section = new Section
                            {
                                KeyIdPolis = row["KeyIdPolis"].ToString(),
                                KeyDekkingsNummer = row["KeyDekkingsNummer"].ToString(),
                                Transaction = GetTransactionByClaimNumber(claimNumber),
                            };

                            sections.Add(section);
                        }
                    }
                    catch (Exception ex)
                    {
                        //// TODO : Log exception
                    }
                }

            return sections;
        }

        private Transaction[] GetTransactionByClaimNumber(string claimNumber)
        {
            var transactionTable = importedData.FirstOrDefault(o => o.TableName.Equals(Constants.ClaimTransaction));

            var transactions = new List<Transaction>();

            if (transactionTable != null)
                foreach (DataRow row in transactionTable.Rows)
                {
                    try
                    {
                        if (claimNumber.Equals(row["KeyInternSchadenummer"]))
                        {
                            var transaction = new Transaction
                            {
                                KeyIdPolis = row["KeyIdPolis"].ToString(),
                                KeyDekkingsNummer = row["KeyDekkingsNummer"].ToString(),
                                KeySchadeBoekingsNummer = row["KeySchadeBoekingsNummer"].ToString(),
                                Payee = row["Payee"].ToString(),
                                RateOfExchange = row["RateOfExchange"].ToString(),
                                //TransactionAuthorisationDate =Convert.ToInt64(row["TransactionAuthorisationDate"]),
                                TransactionCurrencyCode = row["TransactionCurrencyCode"].ToString(),
                                //TransactionDate = row["TransactionDate"].ToString(),
                                TransactionReference = row["TransactionReference"].ToString(),
                                TransactionSequenceNumber = row["TransactionSequenceNumber"].ToString(),
                                TransactionTypeCode = row["TransactionTypeCode"].ToString(),
                                TransactionTypeDescription = row["TransactionTypeDescription"].ToString(),
                                TransactionComponent = GetTransactionComponentsByClaimNumber(claimNumber)
                            };

                            transactions.Add(transaction);
                        }
                    }
                    catch (Exception ex)
                    {
                        //// TODO : Log exception
                    }
                }

            return transactions.ToArray();
        }

        private TransactionComponent[] GetTransactionComponentsByClaimNumber(string claimNumber)
        {
            var transactionComponentTable = importedData.FirstOrDefault(o => o.TableName.Equals(Constants.ClaimTransactionComponent));

            var transactionComponents = new List<TransactionComponent>();

            if (transactionComponentTable != null)
                foreach (DataRow row in transactionComponentTable.Rows)
                {
                    try
                    {
                        if (claimNumber.Equals(row["KeyInternSchadenummer"]))
                        {
                            var transactionComponent = new TransactionComponent
                            {
                                KeyIdPolis = row["KeyIdPolis"].ToString(),
                                KeyDekkingsNummer = row["KeyDekkingsNummer"].ToString(),
                                KeySchadeBoekingsNummer = row["KeySchadeBoekingsNummer"].ToString(),
                                TransactionAmount = row["TransactionAmount"].ToString(),
                                TransactionComponentTypeCode = row["TransactionComponentTypeCode"].ToString(),
                                TransactionComponentTypeDescription = row["TransactionComponentTypeDescription"].ToString()

                            };

                            transactionComponents.Add(transactionComponent);
                        }
                    }
                    catch (Exception ex)
                    {
                        //// TODO : Log exception
                    }
                }

            return transactionComponents.ToArray();
        }

        private IEnumerable<Claimant> GetClaimantsByClaimNumber(string claimNumber)
        {
            var claimantTable = importedData.FirstOrDefault(o => o.TableName.Equals(Constants.ClaimClaimant));

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


        public override IEnumerable<object> Import()
        {
            throw new NotImplementedException();
        }
    }
}
