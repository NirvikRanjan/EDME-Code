using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoSherpa_project.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace YourNamespace
    {
        [Table("verifyconfirmationresponse")]

        public class verifyconfirmationresponse
        {
            public int id { get; set; }
            public string payment_date_time { get; set; }
            public string reference_no { get; set; }
            public string payment_mode { get; set; }
            public string engine_no { get; set; }
            public string chassis_no { get; set; }
            public string policy_holder_name { get; set; }
            public string previous_policy_number { get; set; }
            public string new_policy_number { get; set; }
            public string transaction_amount { get; set; }
            public string is_attempted { get; set; }
            public string is_link_sent { get; set; }
            public string is_contacted { get; set; }
            public string is_appointment { get; set; }
            public string policy_type { get; set; }
            public string cp_name { get; set; }
            public string cp_code { get; set; }
            public string received_date_time { get; set; }
            public string status { get; set; }
            public string message_txt { get; set; }
            public string data { get; set; }
            public string policy_id { get; set; }
            public string policy_no { get; set; }
            public string payment_status { get; set; }
            public int reference_id { get; set; }
            public string encrypted_policy_id { get; set; }
            public string encrypted_policy_no { get; set; }
            public long customer_id { get; set; }
            public long vehicle_id { get; set; }
            public long wyzuser_id { get; set; }
            public string dealer_id { get; set; }
            public string payment_id { get; set; }
        }
    }

}