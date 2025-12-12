using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoSherpa_project.Models.EIBL_API
{
    public class QuotationAPIResponse
    {
        public string QuoteID { get; set; }
        public string ProposalID { get; set; }
        public string InsuranceCompany { get; set; }
        public long ProductID { get; set; }
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
        public string Remark { get; set; }
        public bool IsQuoteFailed { get; set; }

        //public long BASIC_PREM_NONELECT_ACC { get; set; }
        //public long BASIC_PREM_ELECT_ACC { get; set; }
        //public long BASIC_PREM_VEHICLE_NON_DISC { get; set; }
        //public long SUB_TOTAL_PREM { get; set; }
        //public long PA_OWNER_DRIVER { get; set; }
    }
}