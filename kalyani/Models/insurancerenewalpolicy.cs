using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoSherpa_project.Models
{
    [Table("insurancerenewalpolicy")]
    public class insurancerenewalpolicy
    {
        public int id { get; set; }
        public string vehicleregno { get; set; }
        public DateTime? vehicleregdate { get; set; }
        public string modelname { get; set; }
        public string modelvarinat { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string salutation { get; set; }
        public string customername { get; set; }
        public string middlename { get; set; }
        public string lastname { get; set; }
        public string Address { get; set; }
        public string Address_Line_2 { get; set; }
        public string cityname { get; set; }
        public long cityid { get; set; }
        public string pincode { get; set; }
        public string insurancecompany { get; set; }
        public string policynumber { get; set; }
        public string policyexpirydate { get; set; }
        public string nomineename { get; set; }
        public int nomineeage { get; set; }
        public string nomineerelationship { get; set; }
        public int nomineerelationshipid { get; set; }
        public string renewwaltype { get; set; }
        public string madeexistingclaim { get; set; }
        public int Basic_OD_Tenure { get; set; }
        public int Basic_TP_Tenure { get; set; }
        public int Geo_OD_Ext { get; set; }
        public int Geo_TP_Ext { get; set; }
        public int CompulsoryPremium_Amt { get; set; }
        public int Zero_Dep_Tenure { get; set; }
        public int RTI_Tenure { get; set; }
        public int Basic_IDV_Tenure { get; set; }
        public int NCB_Per_Tenure { get; set; }
        public int NCB_Value_Tenure { get; set; }
        public int GST_Per { get; set; }
        public double grosspremium { get; set; }
        public double totalpremium { get; set; }
        public string reasonoptingoutcpa { get; set; }
        public string existingpolicyclaim { get; set; }
        public long customerid { get; set; }
        public long vehicleid { get; set; }
        public string disposeddate { get; set; }
        public string disposedtime { get; set; }
        public string packagetype { get; set; }
        public long policytenure { get; set; }
        public string cpaopted  { get; set; }
        public string cpatenure  { get; set; }
        public string additionalcoverzerodep  { get; set; }
        public string additionalcoverRTI  { get; set; }
        public string riskstartdate  { get; set; }
        public string state  { get; set; }
        public long stateid  { get; set; }
        public double totalgstamt  { get; set; }
        public int creID  { get; set; }
        public string creName  { get; set; }
        public string campaignName { get; set; }
        public bool ispolicygenerated { get; set; }
        public long? insuranceassignedinteractionId { get; set; }
        public long? herodealerId { get; set; }
        [NotMapped]
        public string ResponseCode { get; set; }
        [NotMapped]
        public string ResponseMessage { get; set; }

       


    }
}