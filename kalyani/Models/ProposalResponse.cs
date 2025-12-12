using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoSherpa_project.Models
{
    [Table("proposalresponse")]

    public class ProposalResponse
    {
        public int id { get; set; }
        public string quote_id { get; set; }
        public long customerid { get; set; }
        public long vehicleid { get; set; }
        public string proposal_id { get; set; }
        public string policy_id { get; set; }
        public string payment_id { get; set; }
        public int premium_value { get; set; }
        public string assign_id { get; set; }
        public string status { get; set; }
        public string message_txt { get; set; }
        public string data { get; set; }
        public string paymentMode { get; set; }
        public int sentstatus { get; set; }

    }

}
