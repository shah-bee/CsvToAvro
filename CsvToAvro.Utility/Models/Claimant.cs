//<auto-generated />

using System.Runtime.Serialization;

namespace CsvToAvro.Utility.Models
{
    /// <summary>
    /// Used to serialize and deserialize Avro record CsvToAvro.Claim.Claimant.
    /// </summary>
    [DataContract(Namespace = "CsvToAvro.Claim")]
    public partial class Claimant
    {
        private const string JsonSchema = @"{""type"":""record"",""name"":""CsvToAvro.Claim.Claimant"",""fields"":[{""name"":""ClaimantAddressArea"",""type"":""string""},{""name"":""ClaimantAddressCity"",""type"":""string""},{""name"":""ClaimantAddressStreet"",""type"":""string""},{""name"":""ClaimantCode"",""type"":""string""},{""name"":""ClaimantName"",""type"":""string""},{""name"":""ClaimantPostCode"",""type"":""string""}]}";

        /// <summary>
        /// Gets the schema.
        /// </summary>
        public static string Schema
        {
            get
            {
                return JsonSchema;
            }
        }
      
        /// <summary>
        /// Gets or sets the ClaimantAddressArea field.
        /// </summary>
        [DataMember]
        public string ClaimantAddressArea { get; set; }
              
        /// <summary>
        /// Gets or sets the ClaimantAddressCity field.
        /// </summary>
        [DataMember]
        public string ClaimantAddressCity { get; set; }
              
        /// <summary>
        /// Gets or sets the ClaimantAddressStreet field.
        /// </summary>
        [DataMember]
        public string ClaimantAddressStreet { get; set; }
              
        /// <summary>
        /// Gets or sets the ClaimantCode field.
        /// </summary>
        [DataMember]
        public string ClaimantCode { get; set; }
              
        /// <summary>
        /// Gets or sets the ClaimantName field.
        /// </summary>
        [DataMember]
        public string ClaimantName { get; set; }
              
        /// <summary>
        /// Gets or sets the ClaimantPostCode field.
        /// </summary>
        [DataMember]
        public string ClaimantPostCode { get; set; }
                
        /// <summary>
        /// Initializes a new instance of the <see cref="Claimant"/> class.
        /// </summary>
        public Claimant()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Claimant"/> class.
        /// </summary>
        /// <param name="claimantAddressArea">The claimantAddressArea.</param>
        /// <param name="claimantAddressCity">The claimantAddressCity.</param>
        /// <param name="claimantAddressStreet">The claimantAddressStreet.</param>
        /// <param name="claimantCode">The claimantCode.</param>
        /// <param name="claimantName">The claimantName.</param>
        /// <param name="claimantPostCode">The claimantPostCode.</param>
        public Claimant(string claimantAddressArea, string claimantAddressCity, string claimantAddressStreet, string claimantCode, string claimantName, string claimantPostCode)
        {
            this.ClaimantAddressArea = claimantAddressArea;
            this.ClaimantAddressCity = claimantAddressCity;
            this.ClaimantAddressStreet = claimantAddressStreet;
            this.ClaimantCode = claimantCode;
            this.ClaimantName = claimantName;
            this.ClaimantPostCode = claimantPostCode;
        }
    }
}
