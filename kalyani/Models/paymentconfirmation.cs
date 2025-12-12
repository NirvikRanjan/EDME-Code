using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoSherpa_project.Models
{
    [Table("paymentconfirmation")]
    public class paymentconfirmation
    {
		public int id { get; set; }
        public string Payment_DateTime { get; set; }
		public string New_PolicyNumber { get; set; }
		public string Transaction_Reference_Number { get; set; }
        public string TransactionAmount { get; set; }
		public string Previous_PolicyNumber { get; set; }
        public string ChassisNumber { get; set; }
		public string EngineNumber{ get; set; }
		public string Payment_Type{ get; set; }
		public string IS_online { get; set; }
		public long assignedinteractionid{ get; set; }
		public DateTime? updateddate{ get; set; }
		public string updatetime{ get; set; }
		public string responseStatus{ get; set; }
		public long? customer_id { get; set; }
		public long? vehicle_id { get; set; }
		public long? wyzuser_id { get; set; }
		public long? dealer_id { get; set; }
    }
}