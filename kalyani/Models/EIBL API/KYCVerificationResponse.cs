using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoSherpa_project.Models.EIBL_API
{
    public class KYCVerificationResponse
    {
        public string CustomerType { get; set; }
        public string Status { get; set; }
        public string DOB { get; set; }
        public string DOI { get; set; }
        public string CustomerName { get; set; }
        public string Link { get; set; }
        public string ProposalID { get; set; }
        public string QuoteID { get; set; }
        public string KYCID { get; set; }
        public long ICId { get; set; }
    }
}