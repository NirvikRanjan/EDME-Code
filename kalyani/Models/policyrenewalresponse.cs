using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoSherpa_project.Models
{
    [Table("policyrenewalresponse")]
    public class policyrenewalresponse
    {
        public long id { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string PolicyNo { get; set; }
        public string HeroTransaction { get; set; }
        public string request { get; set; }
        public string response { get; set; }
        public long insurancerenewalpolicyid { get; set; }
        public long vehicleid { get; set; }
        public long customerid { get; set; }
        public long? insuranceassignedinteractionId { get; set; }
        public DateTime? updatedDate { get; set; }
        public string updatedTime { get; set; }
        public long? wyzuser_id { get; set; }
        public long? dealer_id { get; set; }

    }
}