using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
namespace AutoSherpa_project.Models
{
    [Table("winbackpolicyrenewalresponse")]
    public class winbackpolicyrenewalresponse
    {
        [Key]
        public long id { get; set; }
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string PolicyNo { get; set; }

        public string HeroQuoteID { get; set; }
        public double Basic_Prem_Vehicle { get; set; }
        public Decimal Basic_Prem_Access { get; set; }

        public Decimal Basic_Prem_Electr_Access { get; set; }

        public Decimal Extra_Prem_Geographic_ExtOD { get; set; }

        public Decimal Extra_Prem_Geographic_ExtTP { get; set; }

        public Decimal Discount_AntiTheft { get; set; }

        public Decimal Discount_AA_Mem { get; set; }

        public Decimal DisCount_HandiCaped { get; set; }


        public Decimal Discount_Others { get; set; }

        public double NCB_Per { get; set; }

        public double NCB_Value { get; set; }

        public double Total_Deductions { get; set; }
        public double Total_ODP_A { get; set; }
        public Decimal TPPD_Vehicle_Prem { get; set; }

        public Decimal Ext_TPPD { get; set; }
        public Decimal PA_Owner_Driver { get; set; }

        public Decimal PA_UnNamed_Driver { get; set; }

        public Decimal PA_Pillion_Rider { get; set; }
        public Decimal NilDepreciation_Premium { get; set; }

        public Decimal LLiab_Paid_Driver { get; set; }
        public Decimal LLiab_Other { get; set; }

        public Decimal Total_Liability_Prem_B { get; set; }
        public double Total_Prem_A_B { get; set; }

        public Decimal Grand_Total { get; set; }
        public Decimal Service_Tax { get; set; }

        public Decimal Service_Tax_Per { get; set; }
        public double Gross_Prem { get; set; }

        public Decimal RTI_Premium { get; set; }
        public double IGST_Per { get; set; }

        public double IGST_Amt { get; set; }
        public Decimal SGST_Per { get; set; }
        public Decimal SGST_Amt { get; set; }
        public Decimal CGST_Per { get; set; }

        public Decimal CGST_Amt { get; set; }
        public Decimal UGST_Per { get; set; }

        public Decimal UGST_Amt { get; set; }
        public Decimal GST_Cal_Amt { get; set; }

        public string Gst_No { get; set; }
        public Decimal Flood_Cess_Per { get; set; }

        public Decimal Flood_Cess_Amt { get; set; }
        public Decimal CPA_disc_amt { get; set; }

        public string IMT44 { get; set; }
        public string Prev_policy_No { get; set; }

        public string IsNDTaken { get; set; }
        public string IsRTITaken { get; set; }

        public int Product_id { get; set; }
        public string Vehicle_Class { get; set; }

        public int RTOId { get; set; }
        public DateTime FirstSaleDate { get; set; }
        public DateTime PolicyeffetiveDate { get; set; }

        public DateTime AssignDate { get; set; }

        public string IDV { get; set; }

        public string Basic_IDV { get; set; }
        public Double ShowRoomPrice { get; set; }
        public int Cubic_Capacity { get; set; }
        public int policy_Tenure { get; set; }

        public string IsIMT43 { get; set; }
        public int CPATenure { get; set; }
        public int OemDMapId { get; set; }

        public long customerId { get; set; }

        public long vehicleid { get; set; }
        public long TariffDisc { get; set; }

        public long wibbackid { get; set; }
        public int Insur_Comp_ID { get; set; }

        [NotMapped]
        public string productName { get; set; }

        [NotMapped]
        public string ICName { get; set; }


    }
}