using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoSherpa_project.Models
{
    [Table("paymentstatus_response")]
    public class PaymentStatusResponse
    {
        [Key]
        public long Id { get; set; }
        public long? CustomerId { get; set; }
        public long? VehicleId { get; set; }
        public long? WyzuserId { get; set; }
        public string QuoteID { get; set; }
        public string ProposalID { get; set; }
        public string CPCode { get; set; }
        public string OEMID { get; set; }
        public string PaymentDoneDate { get; set; }
        public string TransactionAmount { get; set; }
        public string TransactionReferenceNumber { get; set; }
        public string Status { get; set; }
        public string NewPolicyNumber { get; set; }
        public string PreviousPolicyNumber { get; set; }
        public string ChassisNumber { get; set; }
        public string EngineNumber { get; set; }
        public string UpdatedDate { get; set; }
        public string UpdatedTime { get; set; }
    }
}