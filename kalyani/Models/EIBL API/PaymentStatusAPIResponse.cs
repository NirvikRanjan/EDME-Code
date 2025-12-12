using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Security.Policy;
using System.Text;

namespace AutoSherpa_project.Models.EIBL_API
{
    public class PaymentStatusAPIResponse
    {
        public string Payment_Date { get; set; }
        public string Transaction_Amount { get; set; }
        public string Transaction_Reference_Number { get; set; }
        public string Status { get; set; }
        public string POLICY_NO { get; set; }
        public string Previous_Policy_No { get; set; }
        public string Chassis_Number { get; set; }
        public string Engine_Number { get; set; }
        public string Updated_Date { get; set; }
        public string Updated_Time { get; set; }
    }
}