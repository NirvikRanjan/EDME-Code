using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoSherpa_project.Models
{
    [Table("quotationresponse")]

    public class Quotationresponse
    {
        [Key]
        public long id { get; set; }
        public long customerId { get; set; }
        public long vehicleid { get; set; }
        public string QuoteID { get; set; }
        public string ProposalID { get; set; }
        public DateTime? QuoteDate { get; set; } = DateTime.Now;
        public string InsuranceCompany { get; set; }
        public long ProductId { get; set; }
        public int BasicIDV { get; set; }
        public int Basic { get; set; }
        public int NCBValue { get; set; }
        public int CPAAmount { get; set; }
        public decimal OD { get; set; }
        public string Liability { get; set; }
        public string ADDON { get; set; }
        public string ADDONTOTAL { get; set; }
        public int TotalGST { get; set; }
        public int Gross { get; set; }
        public string PreviousPolicyNo { get; set; }
        public string CPCode { get; set; }
        public int OEMID { get; set; }
        public long Non_Electric_Acc { get; set; }
        public long Electric_Acc { get; set; }
        public long Vehicle_Basic_Premium { get; set; }
        public long Sub_Total_Basic_Prem { get; set; }
        public long PA_Cover_Owner_Driver { get; set; }
        public long Sub_Total_Addition { get; set; }
        public long Basic_Third_Party_Liability { get; set; }

        //public long BASIC_PREM_NONELECT_ACC { get; set; }
        //public long BASIC_PREM_ELECT_ACC { get; set; }
        //public long BASIC_PREM_VEHICLE_NON_DISC { get; set; }
        //public long SUB_TOTAL_PREM { get; set; }
        //public long PA_OWNER_DRIVER { get; set; }

        //KYC Verification API response fields
        public string CustomerType { get; set; }
        public string KYCStatus { get; set; }
        public string DOB { get; set; }
        public string DOI { get; set; }
        public string CustomerName { get; set; }
        public string Link { get; set; }
        public string KYCResponseProposalID { get; set; }
        public string KYCResponseQuoteID { get; set; }
        public string KYCID { get; set; }
        public long KYCICId { get; set; }
        public string IsPaymentLinkSent { get; set; }
        public DateTime? PayLinkSentDateTime { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentDoneDate { get; set; }
        public string TransactionReferenceNumber { get; set; }
        public string FinalQuoteStatus { get; set; }
        public DateTime? FinalStatusUpdatedDateTime { get; set; }
        public bool IsQuoteFailed { get; set; }
        public string QuoteFailedRemarks { get; set; }
    }
}
