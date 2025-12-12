using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutoSherpa_project.Models.ViewModels
{
    //public class WinbackPolicy
    //{
    //    public long id { get; set; }

    //    public bool nonNsure { get; set; }
    //    public int previouspolicyno { get; set; }

    //    public string insurancecompany { get; set; }

    //    public string officeaddress { get; set; }

    //    public DateTime dateoffirstsale { get; set; }

    //    public string yearofmanufacture { get; set; }
    //    public DateTime policystartdate { get; set; }

    //    public int previouspolicytenure { get; set; }
    //    public DateTime policyenddate { get; set; }

    //    public int premium { get; set; }

    //    public bool vehicleinspection { get; set; }
    //    public int inspectionnumber { get; set; }
    //    public DateTime inspectiondate { get; set; }

    //    public bool transfercase { get; set; }
    //    public bool lastpolicy { get; set; }

    //    public string ncbdiscount { get; set; }

    //    public string entitledNCBdiscount { get; set; }

    //    public bool proof { get; set; }

    //    public string whetheravailedclaims { get; set; }

    //    public bool previouspolicycopy { get; set; }

    //    public bool certificationfrominsure { get; set; }

    //    public bool undertakingfromcustomer { get; set; }

    //    public bool noproof { get; set; }


    //    public bool customertype { get; set; }
    //    public string product { get; set; }
    //    public int policytenure { get; set; }

    //    public string oem { get; set; }

    //    public bool proposerdetails { get; set; }

    //    public string Proposertype { get; set; }

    //    public string proposername { get; set; }
    //    public string contactperson { get; set; }
    //    public varchar gender { get; set; }
    //    public string CompanyName { get; set; }
    //    public string addressline1 { get; set; }
    //    public string addressline2 { get; set; }
    //    public string addressline3 { get; set; }

    //    public string proposerstate { get; set; }

    //    public string proposercity { get; set; }
    //    public string registrationnumber { get; set; }
    //    public int pincode { get; set; }
    //    public string registergst { get; set; }

    //    public string Gstno { get; set; }

    //    public string fax { get; set; }
    //    public string Designation { get; set; }
    //    public int mobilenumber { get; set; }

    //    public int designationmobilenumber { get; set; }

    //    public int designationlandline { get; set; }
    //    public string landlinenumber { get; set; }

    //    public string emailid { get; set; }

    //    public DateTime dateofbirth { get; set; }

    //    public int age { get; set; }
    //    public string agebracket { get; set; }

    //    public string maritalstatus { get; set; }
    //    public string idproof { get; set; }

    //    public string vehicleclass { get; set; }

    //    public string vehicleclasshirertheft { get; set; }
    //    public bool chkvehicleclasshirertheft { get; set; }

    //    public DateTime dateoffirst { get; set; }
    //    public string rto { get; set; }
    //    public string frameno { get; set; }
    //    public int engineno { get; set; }
    //    public string modelname { get; set; }

    //    public string color { get; set; }


    //    public string nomineegender { get; set; }

    //    public int exshowroomprice { get; set; }

    //    public string tariff { get; set; }

    //    public DateTime dateofregistration { get; set; }
    //    public string registrationno { get; set; }



    //    public DateTime effectivedate { get; set; }

    //    public DateTime expirydate { get; set; }

    //    public string nomineename { get; set; }

    //    public int nomineeage { get; set; }

    //    public string relation { get; set; }

    //    public string fueltype { get; set; }

    //    public bool inbuiltcng { get; set; }
    //    public bool externalcng { get; set; }

    //    public string inbuilt { get; set; }
    //    public string external { get; set; }

    //    public string cpatenure { get; set; }

    //    public bool ndcover { get; set; }

    //    public string NDPlusCover { get; set; }
    //    public string TPPDExtendDiscount { get; set; }
    //    public bool rticover { get; set; }

    //    public bool accessories { get; set; }
    //    public string GeographicalAreaExtension { get; set; }
    //    public string financername { get; set; }

    //    public string financecompanybranch { get; set; }

    //    public string agreementtypewithfinancer { get; set; }
    //    public string tenure { get; set; }
    //    public int loanamount { get; set; }

    //    public int downpayment { get; set; }

    //    public int emi { get; set; }

    //    public string interest { get; set; }

    //    public varchar paymentmode { get; set; }

    //    public int incomelevel { get; set; }

    //    public string educationlevel { get; set; }

    //    public string occupationtype { get; set; }

    //    public string AnnualTurnover { get; set; }
    //    public string BusinessIndustry { get; set; }
    //    public string employeename { get; set; }
    //    public string PACoverOtherthanOwnerDriver { get; set; }
    //    public string InstrumentNo { get; set; }

    //    public DateTime InstrumentDate { get; set; }

    //    public string DrawnOn { get; set; }
    //    public string parkingtype { get; set; }

    //    public int averagekmdriven { get; set; }

    //    public string excutivename { get; set; }

    //    SAOD
    //    public int TPPreviousPolicyNo { get; set; }

    //    public string Tpinsurancecompany { get; set; }

    //    public string tpofficeaddress { get; set; }

    //    public DateTime tpdateoffirstsale { get; set; }

    //    public DateTime tpyearofmanufacture { get; set; }
    //    public DateTime tppolicystartdate { get; set; }
    //    public DateTime tppolicyenddate { get; set; }

    //    public int Odpolicynumber { get; set; }

    //    public string odinsurancecompany { get; set; }
    //    public DateTime odstartdate { get; set; }
    //    public DateTime odenddate { get; set; }

    //    public string previouscpatenure { get; set; }

    //    public DateTime previouscpastartdate { get; set; }
    //    public DateTime previouscpaenddate { get; set; }
    //    public string tppremiun { get; set; }


    //}
}