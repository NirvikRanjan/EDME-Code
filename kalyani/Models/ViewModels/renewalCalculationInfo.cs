using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoSherpa_project.Models.ViewModels
{
    public class renewalCalculationInfo
    {
        public long id { get; set; }
        public long vehicle_id { get; set; }
        public int SAOD_Basic_OD_Tenure1 { get; set; }
        public int SAOD_Basic_TP_Tenure1 { get; set; }
        public int SAOD_Zero_Dep_Tenure1 { get; set; }
        public int SAOD_RTI_Tenure1 { get; set; }
        public int SAOD_Basic_IDV_Tenure1 { get; set; }
        public int SAOD_NCB_Level_Tenure1 { get; set; }
        public int SAOD_NCB_Value_Tenure1 { get; set; }
        public int SAOD_NCB_Per_Tenure1 { get; set; }
        public int Ex_SAOD_Basic_OD_Tenure1 { get; set; }
        public int Ex_SAOD_Basic_TP_Tenure1 { get; set; }
        public int Ex_SAOD_Zero_Dep_Tenure1 { get; set; }
        public int Ex_SAOD_RTI_Tenure1 { get; set; }
        public int Ex_SAOD_Basic_IDV_Tenure1 { get; set; }
        public int Ex_SAOD_NCB_Level_Tenure1 { get; set; }
        public int Ex_SAOD_NCB_Value_Tenure1 { get; set; }
        public int Ex_SAOD_NCB_Per_Tenure1 { get; set; }
        public int Basic_OD_Tenure1 { get; set; }
        public int Basic_TP_Tenure1 { get; set; }
        public int Geo_OD_Ext1 { get; set; }
        public int Geo_TP_Ext1 { get; set; }
        public int CompulsoryPremium_Amt1 { get; set; }
        public int Zero_Dep_Tenure1 { get; set; }
        public int RTI_Tenure1 { get; set; }
        public int Basic_IDV_Tenure1 { get; set; }
        public int NCB_Level_Tenure1 { get; set; }
        public int NCB_Value_Tenure1 { get; set; }
        public int NCB_Per_Tenure1 { get; set; }
        public int Ex_Basic_OD_Tenure1 { get; set; }
        public int Ex_Basic_TP_Tenure1 { get; set; }
        public int Ex_Geo_OD_Ext1 { get; set; }
        public int Ex_Geo_TP_Ext1 { get; set; }
        public int Ex_CompulsoryPremium_Amt1 { get; set; }
        public int Ex_Zero_Dep_Tenure1 { get; set; }
        public int Ex_RTI_Tenure1 { get; set; }
        public int Ex_Basic_IDV_Tenure1 { get; set; }
        public int Ex_NCB_Level_Tenure1 { get; set; }
        public int Ex_NCB_Value_Tenure1 { get; set; }
        public int Ex_NCB_Per_Tenure1 { get; set; }
        public int Basic_OD_Tenure2 { get; set; }
        public int Basic_TP_Tenure2 { get; set; }
        public int Geo_OD_Ext2 { get; set; }
        public int Geo_TP_Ext2 { get; set; }
        public int CompulsoryPremium_Amt2 { get; set; }
        public int Zero_Dep_Tenure2 { get; set; }
        public int RTI_Tenure2 { get; set; }
        public int Basic_IDV_Tenure2 { get; set; }
        public int Basic_IDV_Tenure22 { get; set; }
        public int NCB_Level_Tenure2 { get; set; }
        public int NCB_Value_Tenure2 { get; set; }
        public int NCB_Per_Tenure2 { get; set; }
        public int Ex_Basic_OD_Tenure2 { get; set; }
        public int Ex_Basic_TP_Tenure2 { get; set; }
        public int Ex_Geo_OD_Ext2 { get; set; }
        public int Ex_Geo_TP_Ext2 { get; set; }
        public int Ex_CompulsoryPremium_Amt2 { get; set; }
        public int Ex_Zero_Dep_Tenure2 { get; set; }
        public int Ex_RTI_Tenure2 { get; set; }
        public int Ex_Basic_IDV_Tenure21 { get; set; }
        public int Ex_Basic_IDV_Tenure2 { get; set; }
        public int Ex_NCB_Level_Tenure2 { get; set; }
        public int Ex_NCB_Value_Tenure2 { get; set; }
        public int Ex_NCB_Per_Tenure2 { get; set; }
        public int Basic_OD_Tenure3 { get; set; }
        public int Basic_TP_Tenure3 { get; set; }
        public int Geo_OD_Ext3 { get; set; }
        public int Geo_TP_Ext3 { get; set; }
        public int CompulsoryPremium_Amt3 { get; set; }
        public int Zero_Dep_Tenure3 { get; set; }
        public int RTI_Tenure3 { get; set; }
        public int Basic_IDV_Tenure3 { get; set; }
        public int NCB_Level_Tenure3 { get; set; }
        public int NCB_Value_Tenure3 { get; set; }
        public int NCB_Per_Tenure3 { get; set; }
        public int Ex_Basic_OD_Tenure3 { get; set; }
        public int Ex_Basic_TP_Tenure3 { get; set; }
        public int Ex_Geo_OD_Ext3 { get; set; }
        public int Ex_Geo_TP_Ext3 { get; set; }
        public int Ex_CompulsoryPremium_Amt3 { get; set; }
        public int Ex_Zero_Dep_Tenure3 { get; set; }
        public int Ex_RTI_Tenure3 { get; set; }
        public int Ex_Basic_IDV_Tenure3 { get; set; }
        public int Ex_NCB_Level_Tenure3 { get; set; }
        public int Ex_NCB_Value_Tenure3 { get; set; }
        public int Ex_NCB_Per_Tenure3 { get; set; }
        public int GST_Per { get; set; }
        public string packagepremium { get; set; }
        public string policyadditionalcoverzerodep { get; set; }
        public string policyadditionalcoverreturntoinvoice { get; set; }
        public string existingpolicyclaim { get; set; }
        public string packageoptingoutcpareason { get; set; }
        public bool isrenewalquotation { get; set; }
        public int Ex_Basic_OD_WithOutNCB_Tenure1 { get; set; }
        public int Ex_Basic_OD_WithOutNCB_Tenure2 { get; set; }
        public int Ex_Basic_OD_WithOutNCB_Tenure3 { get; set; }
        public int Basic_OD_WithOutNCB_Tenure1 { get; set; }
        public int Basic_OD_WithOutNCB_Tenure2 { get; set; }
        public int Basic_OD_WithOutNCB_Tenure3 { get; set; }
        public int SAOD_Basic_OD_WithOutNCB_Tenure1 { get; set; }
        public int EX_SAOD_Basic_OD_WithOutNCB_Tenure1 { get; set; }
        public int SGST_PER { get; set; }

    }
}