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
using NLog;
using System.Globalization;

namespace CsvToAvro.Utility.Claim
{
    public class ClaimImporter : AbstractImporter
    {
        public IEnumerable<Models.Claim> Claims;

        private LogWrapper Logger;
        public ClaimImporter(string path, string fileType, string fileName, LogWrapper logger) : base(path, fileType, fileName, logger)
        {
            this.Logger = logger;
        }

        public override void Import()
        {
            Claims = GetClaims();
        }

        private IEnumerable<Models.Claim> GetClaims()
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
                            //ClaimOpenDate = row["ClaimOpenDate"].ToString().ConvertToLong(),//, "yyyy-mm-dd", CultureInfo.InvariantCulture).ConvertDateToLong(),
                            ClaimReference = row["ClaimReference"].ToString(),
                            //ClaimReportDate = row["ClaimReportDate"].ToString().ConvertToLong(),
                            ClaimStatus = row["ClaimStatus"].ToString(),
                            //ClaimYearOfAccount = row["ClaimYearOfAccount"].ToString().ConvertToLong(),
                           // CloseDate = row["CloseDate"].ToString().ConvertToLong(),
                            CoverageNarrative = row["CoverageNarrative"].ToString(),
                            CoverholderWithClaimsAuthority = row["CoverholderWithClaimsAuthority"].ToString(),
                         //   DateOfDeclinature = row["DateOfDeclinature"].ToString().ConvertToLong(),
                          //  DateOfLoss = row["DateOfLoss"].ToString().ConvertToLong(),
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
                        Logger.logger.Log(LogLevel.Error, ex, string.Format("While importing EDF claim  Claimnumber - {0} : ", row["KeyInternSchadenummer"]));
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
                        Logger.logger.Log(LogLevel.Error, ex,
                            string.Format("While importing EDF claim policy  - {0} : Claimnumber and PolicyCode : {1}", row["KeyInternSchadenummer"], row["PolicyCode"]));
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
                        Logger.logger.Log(LogLevel.Error, ex,
    string.Format("While importing EDF claim section for - {0} : ClaimNumber and PolicyId : {1}", row["KeyInternSchadenummer"], row["KeyIdPolis"]));

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
                              //  TransactionAuthorisationDate = row["TransactionAuthorisationDate"].ToString().ConvertToLong(),
                                TransactionCurrencyCode = row["TransactionCurrencyCode"].ToString(),
                                //TransactionDate = row["TransactionDate"].ToString().ConvertToLong(),
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
                        Logger.logger.Log(LogLevel.Error, ex,
                            string.Format("While importing EDF claim transaction for - {0} : ClaimNumber and PolicyId : {1}", row["KeyInternSchadenummer"], row["KeyIdPolis"]), null);

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
                        Logger.logger.Log(LogLevel.Error, ex,
                           string.Format("While importing EDF claim transaction component for - {0} : ClaimNumber and PolicyId : {1}", row["KeyInternSchadenummer"], row["KeyIdPolis"]));
                    }
                }

            return transactionComponents.ToArray();
        }

        private IEnumerable<Claimant> GetClaimantsByClaimNumber(string claimNumber)
        {
            var claimantTable = importedData.FirstOrDefault(o => o.TableName.Equals(Constants.ClaimClaimant));

            var claimants = new List<Claimant>();

            if (claimantTable != null)

                foreach (DataRow row in claimantTable.Rows)
                {
                    if (claimNumber.Equals(row["KeyInternSchadenummer"].ToString()))
                    {
                        try
                        {
                            var claimant = new Claimant
                            {
                                ClaimantName = row["ClaimantName"].ToString(),
                                ClaimantAddressArea = row["ClaimantAddressArea"].ToString(),
                                ClaimantAddressCity = row["ClaimantAddressCity"].ToString(),
                                ClaimantAddressStreet = row["ClaimantAddressStreet"].ToString(),
                                ClaimantCode = row["ClaimantCode"].ToString(),
                                ClaimantPostCode = row["ClaimantPostCode"].ToString()
                            };

                            claimants.Add(claimant);
                        }
                        catch (Exception exception)
                        {
                            Logger.logger.Log(LogLevel.Error, exception,
                           string.Format("While importing EDF Claim Claimant for - {0} : ClaimNumber", row["KeyInternSchadenummer"]));

                        }
                    }

                }

            return claimants;
        }


    }
}
