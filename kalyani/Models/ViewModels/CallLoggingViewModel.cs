using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutoSherpa_project.Models.ViewModels
{
    public class CallLoggingViewModel
    {
        public wyzuser wyzuser { get; set; }
        public customer cust { get; set; }

        public customer custoAdd { get; set; }
        public vehicle vehi { get; set; }
        public callinteraction callinteraction { get; set; }

        public List<workshop> workshopList { get; set; }
        public List<workshop> allworkshopList { get; set; }
        public List<location> locationList { get; set; }
        public List<fieldwalkinlocation> walkinlocationList { get; set; }
        public List<renewaltype> renewaltypes { get; set; }
        public List<nomineerelation> nomineerelationsLists { get; set; }
        public List<nominee_relation> nominee_Relations { get; set; }
        public List<insurancecompany> companiesList { get; set; }
        public insurance LatestInsurance { get; set; }
        public List<smstemplate> smstemplates { get; set; }
        public calldispositiondata finaldispostion { get; set; }
        public documentuploadhistory docHistory { get; set; }
        public citystate citystates { get; set; }
        public address addressesAdd { get; set; }
        public email emailAdd { get; set; }

        public ListingForm listingForm { get; set; }
        public srdisposition srdisposition { get; set; }
        public callinteraction callinterService { get; set; }
        public servicebooked servicebooked { get; set; }
        public List<phone> phonesAdd { get; set; }
        public NewCar newCar { get; set; }

        //Supporting for Insurance
        public insurancedisposition insudisposition { get; set; }
        public appointmentbooked appointbooked { get; set; }

        public phone phoneAdd { get; set; }


        public List<smstemplate> templates { get; set; }

        public List<emailtemplate> emailtemplates { get; set; }

        //for email
        public Email email { get; set; }

        //for Customer UpdateForm
        public string submittedBtnName { get; set; }

        public List<string> modeOfContact { get; set; }
        public List<string> modeOfDay { get; set; }

        //Chethan Added for Insurance 
        public List<fieldwalkinlocation> walkinlocationLists { get; set; }
        public insuranceassignedinteraction insuranceassignedinteraction { get; set; }

        public long prefferedPhone { get; set; }
        public long prefferedEmail { get; set; }


        public List<phone> custPhoneList { get; set; }


        public long CustomerId { get; set; }

        public long VehicleId { get; set; }

        public long UserId { get; set; }

        public string DealerCode { get; set; }

        public string OEM { get; set; }

        public string InsAssignCRE { get; set; }
        public string InsAssignWorkShop { get; set; }
        public string InsAssignCampaign { get; set; }
        public string INSAssignDealerName { get; set; }
        public string InsAssignCampaignId { get; set; }

        public List<SelectListItem> citystatesList { get; set; }
        public List<SelectListItem> winbackcitystatesList { get; set; }

        public List<SelectListItem> stateList { get; set; }


        public string PolicyDueDateset { get; set; }
        // public insurancerenewalpolicy insurancerenewalpolicy { get; set; }

        public double SAODGrossPremium { get; set; }
        public double SAODTotalPremium { get; set; }

        public double PACKAGEGrossPremiumTenure { get; set; }

        public double PACKAGETotalPremiumTenure { get; set; }
        public bool policyadditionalcoverzerodep { get; set; }
        public bool policyadditionalcoverreturntoinvoice { get; set; }
        public bool isexpired { get; set; }
        public string packageoptingoutcpareason { get; set; }
        public string packageoptingoutcpayesno { get; set; }
        public double GST_AMT { get; set; }
        public long? insuranceassignedinteractionId { get; set; }
        public long? herodealerId { get; set; }
        public renewalCalculationInfo RenewalCalculationInfo { get; set; }


        //winback tables
        public WinbackPolicy winbackPolicy { get; set; }
        public List<winbackpolicyrenewalresponse> Winbackpolicyrenewalresponses { get; set; }

        public List<productmaster> Productmaster { get; set; }


        public List<icmaster> Icmaster { get; set; }
        public ncb_master Ncb_master { get; set; }
        public List<oem_master> Oem_master { get; set; }
        public List<Financier> Financiers { get; set; }

        public List<rto> rtos { get; set; }

        public List<color> colors { get; set; }

        public List<model_master> model_masters { get; set; }

        public List<exshowroomprice> exshowroomprices { get; set; }
        public List<noipaiddrvrncleaners> Noipaiddrvrncleaners { get; set; }

        public List<soipaiddrvrncleaners> Soipaiddrvrncleaners { get; set; }

        public List<noiempotpaiddevrncleaners> Noiempotpaiddevrncleaners { get; set; }

        public List<soiempotppaiddrvrncleaners> Soiempotppaiddrvrncleaners { get; set; }

        public List<map_cp_financier_mapping> Map_cp_financier_mapping { get; set; }
        public virtual List<map_cp_financier> map_cp_financiers { get; set; }

        public virtual List<state_master> state_masters { get; set; }

        public virtual List<city_master> city_masters { get; set; }

        public virtual List<pincodemaster> pincodemasters { get; set; }
        public virtual List<salution> Salutions { get; set; }
        public virtual List<income_level> Income_Levels { get; set; }
        public virtual List<parking_type> Parking_Types { get; set; }

        public virtual List<occupation_type> Occupation_Types { get; set; }
        public virtual List<maritial_status> Maritial_Statuses { get; set; }

        public virtual List<fueltype> Fueltypes { get; set; }

        public virtual List<qualification> Qualifications { get; set; }
        public virtual List<id_proofs> Id_Proofs { get; set; }
        public bool isuploadedDate { get; set; }

        //public List<map_cp_financier> map_Cp_Financiers { get; set; }




        public tarrifplans Tarrifplans { get; set; }
        public string Product { get; set; }

        public string productId { get; set; }


        public bool isProposeRenew { get; set; }

        public Quotationresponse quotationData { get; set; }
        public string companyList { get; set; }
        public string HeroDealerLocation { get; set; }
        public string CampaignName { get; set; }
        public string PrefferedPhoneNumber { get; set; }
        public string QuatationFinalStatus { get; set; }
        public DateTime? policyduedate { get; set; }
        public string isPolicyDueDateExpired { get; set; }
        public WinbackRolloverPolicyVM winRollPolicy {get; set;}
        public List<EIBLMasterState> eiblMasterStates {get; set;}
        public string stateId {get; set;}
        public string cityId {get; set;}
        public string pincode {get; set;}
    }
}