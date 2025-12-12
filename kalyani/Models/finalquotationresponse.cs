using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoSherpa_project.Models
{
    [Table("finalquotationresponse")]

    public class finalquotationresponse
    {
        [Key]
        public int id { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string PolicyNo { get; set; }
        public string HeroTransaction { get; set; }
        public string PremAmount { get; set; }
        public long vehicleid { get; set; }
        public long customerid { get; set; }
        public long winbackpolicyid { get; set; }
        public long policyresponseid { get; set; }
        public string campaignname { get; set; }
        public string crename { get; set; }
        public string policytype { get; set; }
        public string zerodep { get; set; }
        public string netpremium { get; set; }
        public string basicOD { get; set; }
        public int totalGST { get; set; }
        public DateTime? quotedate { get; set; }
        public string return_invoice_prem { get; set; }
        public string total_tp_prem { get; set; }
        public int ncvvalue { get; set; }
        public int baseIDV { get; set; }
        public string quote_id { get; set; }
        public string assign_id { get; set; }
    }
}