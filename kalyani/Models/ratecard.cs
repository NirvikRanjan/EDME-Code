
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoSherpa_project.Models

{
    [Table("ratecarddump")]
    public class ratecard
    {
        [Key]
        public int ID { get; set; }
        public string Policy_no { get; set; }
        public int? Policy_id { get; set; }
        public string Insurance_Company { get; set; }
        public string POLICY_EFFECTIVE_DATE { get; set; }
        public string POLICY_EXPIRY_DATE { get; set; }
        public string Renewal_Date { get; set; }
        public int? FKPRODUCT_ID { get; set; }
        public string Customer_name { get; set; }
        public string SALUTATION { get; set; }
        public string FIRST_NAME { get; set; }
        public string MIDDLE_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string GENDER { get; set; }
        public string CITY_NAME { get; set; }
        public string STATE_NAME { get; set; }
        public string STATE_CODE { get; set; }
        public string MobileNo { get; set; }
        public string veh_regist_no_com { get; set; }
        public string veh_regist_no { get; set; }
        public string vehicle_class { get; set; }
        public int? fkRTO_Id { get; set; }
        public int? cubic_capacity { get; set; }
        public string EngineNo { get; set; }
        public string FrameNoNo { get; set; }
        public string DealerName { get; set; }
        public string DealerCode { get; set; }
        public string Model_Name { get; set; }
        public int? DealerId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public decimal? EXSHOWROOM_PRICE { get; set; }
        public string Customer_Type { get; set; }
        public decimal? Basic_idv { get; set; }
        public decimal? SGST_PER { get; set; }
        public decimal? SGST_AMT { get; set; }
        public decimal? CGST_PER { get; set; }
        public decimal? CGST_AMT { get; set; }
        public decimal? IGST_PER { get; set; }
        public decimal? IGST_AMT { get; set; }
        public decimal? UGST_AMT { get; set; }
        public decimal? TOTALGST_PER { get; set; }
        public decimal? GROSS_PREM { get; set; }
        public string REGISTRATION_DATE { get; set; }
        public string Previous_Policy_Start { get; set; }
        public string Previous_Policy_Expiry { get; set; }
        public string TP_POLICY_NO { get; set; }
        public string TPPOLICY_EXPIRY_DATE { get; set; }
        public string TPPOLICY_EFFECTIVE_DATE { get; set; }
        public int? PREV_TP_TENURE { get; set; }
        public int? Veh_Age { get; set; }
        public string Renewal_Type { get; set; }
        public string Policy_expiring_on { get; set; }
        public int? Policy_Tenure { get; set; }
        public int? Prev_Policy_Tenure { get; set; }
        public string Date_Of_First_Sale { get; set; }
        public decimal? premium_amount { get; set; }
        public int? Created_By { get; set; }
        public string Created_Date { get; set; }
        public string Created_Proc_Name { get; set; }
        public string Created_MachineIp { get; set; }
        public int? Created_Conn_Id { get; set; }
        public int? updated_By { get; set; }
        public string updated_Date { get; set; }
        public string updated_Proc_Name { get; set; }
        public string updated_MachineIp { get; set; }
        public int? updated_Conn_Id { get; set; }
        public string New_policy_no { get; set; }
        public string Policy_created_Date { get; set; }
        public int? Transaction_ID { get; set; }
        public decimal? Transaction_Amount { get; set; }
        public int? New_fkproduct_id { get; set; }
        public string NEW_JSON_REQUEST { get; set; }
        public string NEW_JSON_RESPONSE { get; set; }
        public string NEW_SEND_DATE { get; set; }
        public int? IsGeoGraphical { get; set; }
        public int? Pincode { get; set; }
        public string email { get; set; }
        public string addr1 { get; set; }
        public string addr3 { get; set; }
        public string Model_Variant { get; set; }
        public string Vehicle_Reg_date { get; set; }
        public string Previous_Policy_Type { get; set; }
        public string Previous_Policy_Number { get; set; }
        //public string Middle_Name { get; set; }
        public string CP_REG_CITY_Name { get; set; }
        public int? CP_REG_CITY_ID { get; set; }
        public decimal? UGST_PER { get; set; }
        public string zero_dep { get; set; }
        public decimal? NilDepreciation_Premium { get; set; }
        public string IsRTI { get; set; }
        public int? return_to_invoice { get; set; }
        public int? FKOEM_ID { get; set; }
        public string OEM_NAME { get; set; }
        public decimal? RTI_Premium { get; set; }
        public string DisCount_HandiCaped { get; set; }
        public int? HandiCaped { get; set; }
        public int? IsNilDepreciation { get; set; }
        public string IsHandiCaped { get; set; }
        public decimal? Grand_Total { get; set; }
        public decimal? Main_OD { get; set; }
        public decimal? Basic_OD { get; set; }
        public decimal? Basic_TPPD { get; set; }
        public decimal? Ext_TPPD { get; set; }
        public decimal? CompulsoryPremium_Amt { get; set; }
        public decimal? Prev_Ncb_per { get; set; }
        public int? num_of_claim { get; set; }
        public decimal? Ncb_Per { get; set; }
        public int? Ncb_Slab { get; set; }
        public decimal? Ncb_value { get; set; }
        public int? GST_NUM { get; set; }
        public string GSTString { get; set; }
        public string Flag_val { get; set; }
        public string Policy_type { get; set; }
        public decimal? OD_Discount { get; set; }
        public decimal? OD_disc_per { get; set; }
        public string Zone_name { get; set; }
        public string MessageText { get; set; }
        public string RequestString { get; set; }
        public string DataSend_date { get; set; }
        public int? Previous_Policy_Insurer { get; set; }
        public int? DataSent_Count { get; set; }
        public int? Claim_count { get; set; }
        //public string TP_Policy_No { get; set; }
        public string TP_Start_Date { get; set; }
        public string TP_End_Date { get; set; }
        public string Nominee_Name { get; set; }
        public string Nominee_Relationship { get; set; }
        public int? Nominee_Age { get; set; }
        public int? vehicle_idd { get; set; }
        public string Flag_status { get; set; }
        public int? upload_id { get; set; }
        public int? IS_Renewal { get; set; }
        public int? is_payment { get; set; }
        public string Alt_Mobile_no { get; set; }
        public string RenPolicyLink { get; set; }
        public string DOB { get; set; }
        public string DOI { get; set; }
        public string Hypothecation { get; set; }
        public int? PA_Owner_Driver { get; set; }
        public int? TP_Insurer { get; set; }
        public string Last_Year_addon { get; set; }
        public string Campaign { get; set; }
        public int FKSUBOEM_ID { get; set; }
        public string SUBOEM_NAME { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
