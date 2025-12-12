using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using MySql.Data.EntityFramework;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;




namespace AutoSherpa_project.Models
{
    [Table("WinbackPolicy")]
    public class WinbackPolicy
    {
        public long id { get; set; }

        public bool nonNsure { get; set; }
        public long cubiccapacity { get; set; }
        public int previouspolicyno { get; set; }

        public string insurancecompany { get; set; }
        public string insurancecompanyname { get; set; }

        public string officeaddress { get; set; }
        [Column(TypeName = "date")]
        public DateTime? dateoffirstsale { get; set; }

        public string yearofmanufacture { get; set; }
        public DateTime? policystartdate { get; set; }

        public int previouspolicytenure { get; set; }
        public DateTime? policyenddate { get; set; }

        public int premium { get; set; }

        public bool vehicleinspection { get; set; }

        public bool SAODvehicleInspectioncheckBox { get; set; }
        public int inspectionnumber { get; set; }
        public DateTime? inspectiondate { get; set; }

        public bool transfercase { get; set; }
        public bool lastpolicy { get; set; }

        public string ncbdiscount { get; set; }

        public string entitledNCBdiscount { get; set; }

        public bool proof { get; set; }

        public string whetheravailedclaims { get; set; }

        public bool previouspolicycopy { get; set; }

        public bool certificationfrominsure { get; set; }

        public bool undertakingfromcustomer { get; set; }

        public bool noproof { get; set; }


        public bool customertype { get; set; }
        public string product { get; set; }
        public int policytenure { get; set; }

        public string oem { get; set; }

        public bool proposerdetails { get; set; }

        public string Proposertype { get; set; }

        public string proposername { get; set; }

        public string ProposerFirstName { get; set; }

        public string ProposerMiddleName { get; set; }
        public string ProposerLastName { get; set; }
        public string contactperson { get; set; }

        public string ContactpersonFirstName { get; set; }
        public string ContactpersonMiddleName { get; set; }
        public string ContactpersonLastName { get; set; }
        public string gender { get; set; }
        public string CompanyName { get; set; }
        public string addressline1 { get; set; }
        public string addressline2 { get; set; }
        public string addressline3 { get; set; }

        
        public long proposerstate { get; set; }
        public string proposerstatename { get; set; }
        public long proposercity { get; set; }
        public string proposercityname { get; set; }
        public string registrationnumber { get; set; }
        public int pincode { get; set; }

        public string PincodeDropdownOne { get; set; }
        public int PincodeDropdownTwo { get; set; }
        public string registergst { get; set; }

        public string Gstno { get; set; }

        public string fax { get; set; }
        public string fax2 { get; set; }
        public string Designation { get; set; }
        public long mobilenumber { get; set; }
        public long SecondMobileNumber { get; set; }
        public long designationmobilenumber { get; set; }

        public int designationlandline { get; set; }
        public int designationlandline2 { get; set; }
        public int designationlandline3 { get; set; }
        public int designationlandline4 { get; set; }
        public string landlinenumber { get; set; }

        public string emailid { get; set; }

        public DateTime? dateofbirth { get; set; }

        public bool RadioButtonDateofBirth { get; set; }

         public int age { get; set; }
        public bool RadioButton_Age { get; set; }
        public string agebracket { get; set; }
        public bool RadioButton_AgeBracket { get; set; }
        public string maritalstatus { get; set; }
        public string idproof { get; set; }

        public string IdproofValue { get; set; }

        public string policy_type { get; set; }
        public string vehicleclass { get; set; }
        public string vehicleclassprivate { get; set; }

        public string vehicleclasscommercial { get; set; }
        public string vehicleclasshirertheft { get; set; }
        public bool chkvehicleclasshirertheft { get; set; }

        public DateTime? dateoffirst { get; set; }
        public string rto { get; set; }
        public string frameno { get; set; }
        public string engineno { get; set; }
        public string modelname { get; set; }

        public string color { get; set; }

        public string colorDropDown { get; set; }
        public string nomineegender { get; set; }

        public Double exshowroomprice { get; set; }

        public string tariff { get; set; }

        public DateTime? dateofregistration { get; set; }
        public string registrationno { get; set; }

        public string registrationno2 { get; set; }

        public string registrationno3 { get; set; }

        public string registrationno4 { get; set; }

        public string BasicIDV { get; set; }
        public DateTime? effectivedate { get; set; }

        public DateTime? expirydate { get; set; }
       
        public string nomineename { get; set; }

        public int nomineeage { get; set; }

        public string relation { get; set; }

        public string fueltype { get; set; }

        public bool inbuiltcng { get; set; }
        public bool externalcng { get; set; }

        public string ExternalKitPrice { get; set; }

        public DateTime? ExternalCNGDateOfFitment { get; set; }

        public string inbuilt { get; set; }
        public string external { get; set; }

        public string cpatenure { get; set; }

        public string CPAexclusionCode { get; set; }

        public string CPAexclusionText { get; set; }

        public string CPAreason { get; set; }

        public string Branch_Office_Location_Code { get; set; }
        public DateTime? Anniversary_Date { get; set; }
        public bool ndcover { get; set; }

        public string NDPlusCover { get; set; }
        public bool NDPlusCoverCheckBox { get; set; }
        public string TPPDExtendDiscount { get; set; }

        public bool TPPDExtendedDiscountChkBox { get; set; }
        public bool rticover { get; set; }

        public bool HandicapedDiscount { get; set; }

        public bool accessories { get; set; }

        public string AccessoriesCategory { get; set; }

        public string AccessoriesName { get; set; }

        public string FitmentAmount { get; set; }
        public DateTime? FitmentDate { get; set; }
        public string GeographicalAreaExtension { get; set; }
        public bool GeographicalAreaCheckBox { get; set; }

        public bool GeoGraphicalArea_Nepal { get; set; }

        public bool GeoGraphicalArea_Bangladesh { get; set; }

        public bool GeoGraphicalArea_Srilanka { get; set; }

        public bool GeoGraphicalArea_Bhutan { get; set; }
        public string NOIPaidDrvrnCleaners { get; set; }
        public string SOIPaidDrvrnCleaners { get; set; }
        public string NOIEmpOTPaidDrvrnCleaners { get; set; }
        public string SOIEmpOTPaidDrvrnCleaners { get; set; }
        public string MAP_CP_Financer_Mapping { get; set; }
        public string MAP_CP_Financer { get; set; }
        public bool IsIMT43 { get; set; }

        public bool IsIMT44 { get; set; }
        public bool IsAntiTheft { get; set; }

        public bool is_AA_membership { get; set; }
        public long AddOn_Id { get; set; }

        public long OPCover_Amt { get; set; }

        public long OOCover_Amt { get; set; }

        public long PDCover_Amt { get; set; }

        public long VoluntryDisc { get; set; }
        public bool VoluntryDiscCheckBox { get; set; }
        public bool EntitledNCBDisocunt { get; set; }
        public string financername { get; set; }

        public string financecompanybranch { get; set; }

        public string agreementtypewithfinancer { get; set; }
        public string tenure { get; set; }
        public int loanamount { get; set; }

        public int downpayment { get; set; }

        public int emi { get; set; }

        public string interest { get; set; }

        public string paymentmode { get; set; }

        public string incomelevel { get; set; }

        public int Membership_No { get; set; }

        public string educationlevel { get; set; }

        public string occupationtype { get; set; }

        public string AnnualTurnover { get; set; }
        public string BusinessIndustry { get; set; }
        public string employeename { get; set; }

        public bool PACoverCheckBox { get; set; }
        public string PACoverOtherthanOwnerDriver { get; set; }

        public string Suminsuredperperson{ get; set; }

        public bool Un_NamedDriver { get; set; }

        public bool Un_NamesdPassenger { get; set; }
        public string InstrumentNo { get; set; }

        public DateTime? InstrumentDate { get; set; }

        public string DrawnOn { get; set; }
        public string parkingtype { get; set; }

        public int averagekmdriven { get; set; }

        public string excutivename { get; set; }

        //SAOD
        public int TPPreviousPolicyNo { get; set; }

        public string Tpinsurancecompany { get; set; }

        public string tpofficeaddress { get; set; }

        public DateTime? tpdateoffirstsale { get; set; }

        public DateTime? tpyearofmanufacture { get; set; }
        public DateTime? tppolicystartdate { get; set; }
        public DateTime? tppolicyenddate { get; set; }

        public int Odpolicynumber { get; set; }

        public string odinsurancecompany { get; set; }
        public DateTime? odstartdate { get; set; }
        public DateTime? odenddate { get; set; }
        //public int saodpremium { get; set; }
        public string previouscpatenure { get; set; }

        public DateTime? previouscpastartdate { get; set; }
        public DateTime? previouscpaenddate { get; set; }
        public string tppremiun { get; set; }
        public int saodinspectionnumber { get; set; }
       public bool SAODTransferCase { get; set; }
        public bool SAODproof { get; set; }
       public string SAODwhetherAvailed { get; set; }

        //[StringLength(60)]

        public bool ODRenewalPolicy { get; set; }
        public long customerId { get; set; }
        
        public long vehicleid { get; set; }
        [NotMapped]
        public string productId { get; set; }

        public bool isProposeRenew { get; set; }


    }

}