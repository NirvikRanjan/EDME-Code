using AutoSherpa_project.Models;
using AutoSherpa_project.Models.EIBL_API;
using AutoSherpa_project.Models.Schedulers;
using AutoSherpa_project.Models.ViewModels;
using AutoSherpa_project.Models.YourNamespace;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Google.Apis.Auth.OAuth2;
using Google.Protobuf.WellKnownTypes;
using LiteDB;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR.Hosting;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Quartz;
using Quartz.Impl;
using Quartz.Util;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Net.Security;
using System.Reactive.Joins;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.Routing;
using static Google.Apis.Requests.BatchRequest;
using static Google.Apis.Requests.RequestError;
using static System.Windows.Forms.AxHost;
using Logger = NLog.Logger;



namespace AutoSherpa_project.Controllers
{
    [AuthorizeFilter]
    public class customer360Controller : Controller
    {
        // GET: customer360
        private string firebasefilepath = string.Empty;
        public ActionResult customer360(string id)
        {
            CallLoggingViewModel callLog = new CallLoggingViewModel();
            try
            {
                //clear360ProfilePrevSession(); // To Clear Session values used in 360 degree form...........
                Session["RoleFor"] = null;
                Session["PageFor"] = null;
                Session["CusId"] = null;
                Session["VehiId"] = null;
                Session["typeOfDispo"] = null;
                Session["appointBookId"] = null;
                Session["isCallInitiated"] = null;
                Session["AndroidUniqueId"] = null;
                Session["GSMUniqueId"] = null;
                Session["DialedNumber"] = null;
                Session["psfDayType"] = null;
                Session["interactionid"] = null;
                Session["MakeCallFrom"] = null;
                Session["isPSFRM"] = null;
                Session["NCReason"] = null;

                bool isMCP = false;

                if (Session["inComingParameter"] != null)
                {
                    id = string.Empty;
                    id = Session["inComingParameter"].ToString();
                }
                else
                {

                    if (Session["UserRole"].ToString() != "CREManager" && Session["UserRole"].ToString() != "RM")
                    {
                        TempData["Exceptions"] = "Invalid Url Operation..[Customer360]....";
                        return RedirectToAction("LogOff", "Home");
                    }
                }



                //Session.Timeout = 90;
                string logFor = string.Empty;
                Dictionary<string, int> userLogData = new Dictionary<string, int>();
                int UserId = Convert.ToInt32(Session["UserId"].ToString());
                string contactNumber = string.Empty, emailAddress = string.Empty;
                callLog.wyzuser = new wyzuser();
                callLog.cust = new customer();
                callLog.vehi = new vehicle();
                callLog.callinteraction = new callinteraction();
                callLog.workshopList = new List<workshop>();
                callLog.allworkshopList = new List<workshop>();
                callLog.locationList = new List<location>();
                callLog.walkinlocationList = new List<fieldwalkinlocation>();
                callLog.renewaltypes = new List<renewaltype>();
                callLog.companiesList = new List<insurancecompany>();
                callLog.LatestInsurance = new insurance();
                callLog.smstemplates = new List<smstemplate>();
                callLog.listingForm = new ListingForm();
                callLog.phonesAdd = new List<phone>();
                callLog.listingForm.upsellleads = new List<upselllead>();
                callLog.insudisposition = new insurancedisposition();
                callLog.appointbooked = new appointmentbooked();
                callLog.newCar = new NewCar();
                callLog.walkinlocationLists = new List<fieldwalkinlocation>();
                callLog.emailtemplates = new List<emailtemplate>();
                callLog.custPhoneList = new List<phone>();
                callLog.nomineerelationsLists = new List<nomineerelation>();
                callLog.nominee_Relations = new List<nominee_relation>();
                callLog.Financiers = new List<Financier>();
                callLog.Icmaster = new List<icmaster>();
                callLog.rtos = new List<rto>();
                callLog.colors = new List<color>();
                callLog.model_masters = new List<model_master>();
                callLog.Noipaiddrvrncleaners = new List<noipaiddrvrncleaners>();
                callLog.Soipaiddrvrncleaners = new List<soipaiddrvrncleaners>();
                callLog.Noiempotpaiddevrncleaners = new List<noiempotpaiddevrncleaners>();
                callLog.Soiempotppaiddrvrncleaners = new List<soiempotppaiddrvrncleaners>();
                callLog.Map_cp_financier_mapping = new List<map_cp_financier_mapping>();
                callLog.map_cp_financiers = new List<map_cp_financier>();
                callLog.state_masters = new List<state_master>();
                callLog.city_masters = new List<city_master>();
                callLog.pincodemasters = new List<pincodemaster>();
                callLog.Salutions = new List<salution>();
                callLog.Income_Levels = new List<income_level>();
                callLog.Parking_Types = new List<parking_type>();
                callLog.Occupation_Types = new List<occupation_type>();
                callLog.Maritial_Statuses = new List<maritial_status>();
                callLog.Fueltypes = new List<fueltype>();
                callLog.Qualifications = new List<qualification>();
                callLog.Id_Proofs = new List<id_proofs>();

                for (int i = 0; i < 4; i++)
                {

                    callLog.phonesAdd.Add(new phone());
                }

                if (id.Contains(','))
                {
                    if (id.Split(',')[1] == "I")
                    {
                        ViewBag.typeOfDispo = "insurance";
                        logFor = "insurance";
                    }
                    string pageFor = "";
                    if (isMCP)
                    {
                        pageFor = "CRE";
                    }
                    else
                    {
                        pageFor = id.Split(',')[0];
                    }

                    Session["CusId"] = Convert.ToInt32(id.Split(',')[2]);
                    Session["VehiId"] = Convert.ToInt32(id.Split(',')[3]);
                    Session["typeOfDispo"] = logFor;

                    int cusid = Convert.ToInt32(id.Split(',')[2]);
                    int vehId = Convert.ToInt32(id.Split(',')[3]);
                    ViewBag.vehiId = vehId;


                    callLog.CustomerId = cusid;
                    callLog.VehicleId = vehId;
                    callLog.UserId = UserId;
                    using (var db = new AutoSherDBContext())
                    {

                        if (id.Split(',')[1] == "I")
                        {
                            callLog.smstemplates = getSMSTemplate("Insurance");

                            callLog = get360ProfileData(callLog, db, "Insurance", Session["DealerCode"].ToString(), Session["OEM"].ToString());
                            if (callLog.cust.emails.Count > 0)
                            {
                                emailAddress = callLog.cust.emails.FirstOrDefault().emailAddress;
                            }
                            if (callLog.cust.phones.Count > 0)
                            {
                                contactNumber = callLog.cust.phones.FirstOrDefault().phoneNumber;
                            }

                            //callLog.insurancerenewalpolicy =callLog.vehi(insurancerenewalpolicy);

                            Session["VehiReg"] = callLog.vehi.chassisNo;
                            ViewBag.typeOfDispo = "insurance";

                            if (pageFor == "Shw" || pageFor == "CRE")
                            {
                                Session["PageFor"] = "CRE";
                                if (pageFor == "Shw")
                                {
                                    Session["MakeCallFrom"] = "InsuranceSearch";
                                }
                            }
                            else if (pageFor == "Hdd")
                            {
                                Session["PageFor"] = "Search";
                            }
                            else if (pageFor == "CREManager")
                            {
                                Session["PageFor"] = "CREManager";
                            }
                            else if (pageFor == "CRE")
                            {
                                Session["PageFor"] = "CRE";
                            }


                            long countInteraction = db.callinteractions.Count(k => k.vehicle_vehicle_id == vehId && k.customer_id == cusid && k.insuranceAssignedInteraction_id != null);
                            if (countInteraction > 0)
                            {
                                long lastid = db.callinteractions.Where(k => k.vehicle_vehicle_id == vehId && k.customer_id == cusid && k.insuranceAssignedInteraction_id != null).Max(model => model.id);
                                if (lastid > 0)
                                {
                                    callLog.callinteraction = db.callinteractions.Include("insurancedispositions").Include("vehicle").Include("appointmentbooked").FirstOrDefault(m => m.id == lastid);

                                }
                            }
                            long insu_ass_id = callLog.callinteraction.insuranceAssignedInteraction_id ?? default(long);
                        }
                    }
                }
                else
                {
                    Session.RemoveAll();
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["Exceptions"] = ex.Message.ToString();
                if (ex.Message.Contains("inner exception"))
                {
                    TempData["Exceptions"] = ex.InnerException.Message;
                }
                TempData["ControllerName"] = "Call Log";
                return RedirectToAction("LogOff", "Home");
            }


            return View(callLog);
        }

        public CallLoggingViewModel get360ProfileData(CallLoggingViewModel callLog, AutoSherDBContext db, string DispoType, string DealerCode, string OEM)
        {

            long UserId = callLog.UserId;
            long vehId = callLog.VehicleId, cusid = callLog.CustomerId;
            callLog.wyzuser = db.wyzusers.Include("location").Include("workshop ").SingleOrDefault(m => m.id == UserId);
            // var a = db.wyzusers.Include("location").Include("workshop ").AsQueryable();
            // var b = db.customers.Include("emails").Include("addresses").Include("vehicles").Include("insurances").AsQueryable();

            callLog.cust = db.customers.Include("emails").Include("addresses").Include("vehicles").Include("insurances").FirstOrDefault(m => m.id == cusid);

            var userVehicleData = db.vehicles.FirstOrDefault(m => m.vehicle_id == vehId);
            callLog.vehi = userVehicleData;

            long? fkSubOemId = 0;
            string cpCode = string.Empty;
            if (callLog.vehi != null)
            {
                callLog.HeroDealerLocation = db.Herodealers.Where(m => m.DealerCode == callLog.vehi.CP_CODE).Select(m => m.DealerCity).FirstOrDefault();
                callLog.CampaignName = db.campaigns.Where(m => m.id == callLog.vehi.campaign_id).Select(m => m.campaignName).FirstOrDefault();

                fkSubOemId = callLog.vehi.oemid;
                cpCode = callLog.vehi.CP_CODE;
            }

            ViewBag.InsuranceCompany = db.OEMInsuranceCompany.Where(m => m.fksuboem_id == fkSubOemId && m.delearCode == cpCode).Select(m => new { id = m.product_id, companyName = m.companyName }).ToList();
            ViewBag.InsuranceComapnyProvider = db.insurancecompanies.Where(m => m.isactive).Select(m => new { id = m.companyName, companyName = m.companyName }).ToList();
            ViewBag.FinalizeAddOns = db.addoncovers.Select(m => new { addOnCode = m.addOn_code, addOnName = m.coverName, addOnCategory = m.category }).ToList();
            var categoryMap = new Dictionary<string, List<string>> {
                { "basic", db.addoncovers.Where(a => a.category.Contains("basic")).Select(a => a.addOn_code).ToList() },
                { "suggested", db.addoncovers.Where(a => a.category.Contains("suggested")).Select(a => a.addOn_code).ToList() },
                { "popular", db.addoncovers.Where(a => a.category.Contains("popular")).Select(a => a.addOn_code).ToList() }
            };
            ViewBag.CategoryMap = JsonConvert.SerializeObject(categoryMap);
            var brandnameresult = db.brands.Where(m => m.fksuboem_id == callLog.vehi.oemid).Select(m => m.brandname).FirstOrDefault();
            ViewBag.brandnameresult = brandnameresult;

            //api buttons disable if Policyduedate expires 
            callLog.policyduedate = db.vehicles.Where(m => m.vehicle_id == vehId && m.customer_id == cusid).Select(p => p.policyDueDate).FirstOrDefault();
            if (callLog.policyduedate != null)
            {
                var policyDueDate = Convert.ToDateTime(callLog.policyduedate).Date;

                if (policyDueDate >= DateTime.Now.Date)
                {
                    callLog.isPolicyDueDateExpired = "true";
                }
                else
                {
                    callLog.isPolicyDueDateExpired = "false";
                }
            }

            callLog.PrefferedPhoneNumber = db.phones.Where(m => m.customer_id == cusid).OrderByDescending(m => m.isPreferredPhone == true).Select(m => m.phoneNumber).FirstOrDefault();

            callLog.QuatationFinalStatus = db.quotationresponses.Where(m => m.vehicleid == vehId && m.customerId == cusid).OrderByDescending(m => m.FinalStatusUpdatedDateTime).Select(m => m.FinalQuoteStatus).FirstOrDefault();

            #region Winback Workflow Fields Values Auto Populate Section
            if (callLog.vehi?.campaign_id == 34 || callLog.vehi?.campaign_id == 36)
            {
                ViewBag.PreviousPolicyNo = userVehicleData.Previous_Policy_Number;

                if (userVehicleData.Previous_Policy_Expiry != null)
                {
                    ViewBag.PreviousODPolicyExpiryDate = Convert.ToDateTime(userVehicleData.Previous_Policy_Expiry).ToString("yyyy-MM-dd");
                }

                if (userVehicleData.policyeffectivedate != null)
                {
                    ViewBag.PreviousODPolicyEffectiveDate = Convert.ToDateTime(userVehicleData.policyeffectivedate).ToString("yyyy-MM-dd");
                }

                if (userVehicleData.TP_Start_Date != null)
                {
                    ViewBag.PreviousTPPolicyStartDate = Convert.ToDateTime(userVehicleData.TP_Start_Date).ToString("yyyy-MM-dd");
                }

                if (userVehicleData.TP_End_Date != null)
                {
                    ViewBag.PreviousTPPolicyExpiryDate = Convert.ToDateTime(userVehicleData.TP_End_Date).ToString("yyyy-MM-dd");
                }

                if (callLog.cust.dob != null && callLog.cust.dob != "0000-00-00")
                {
                    ViewBag.DOB = Convert.ToDateTime(callLog.cust.dob).ToString("yyyy-MM-dd");
                }
                
                if (userVehicleData.doi != null)
                {
                    ViewBag.DOI = Convert.ToDateTime(userVehicleData.doi).ToString("yyyy-MM-dd");
                }
                
                if (userVehicleData.saleDate != null)
                {
                    ViewBag.InvoiceDate = Convert.ToDateTime(userVehicleData.saleDate).ToString("yyyy-MM-dd");
                }
                
                if (userVehicleData.Vehicle_Reg_date != null)
                {
                    ViewBag.RegistrationDate = Convert.ToDateTime(userVehicleData.Vehicle_Reg_date).ToString("yyyy-MM-dd");
                }

                ViewBag.VehicleClass = userVehicleData.vehicle_class;
                ViewBag.ProposerType = userVehicleData.Customer_Type;
                ViewBag.VehicleType = userVehicleData.vech_type_code;
                ViewBag.ChassisNo = userVehicleData.Frame_no;
                ViewBag.EngineNo = userVehicleData.engineNo;

                var stateName = userVehicleData.state_name?.ToUpper();
                var stateID = db.EIBLMasterStates.Where(x => x.STATE_NAME.ToUpper() == stateName).Select(x => x.STATE_ID).FirstOrDefault();
                callLog.stateId = stateID.ToString();

                var cityName = userVehicleData.city_name?.ToUpper();
                var cityID = db.EIBLMasterCities.Where(x => x.CITY_NAME.ToUpper() == cityName && x.FKSTATE_ID == stateID).Select(x => x.CITY_ID).FirstOrDefault();
                callLog.cityId = cityID.ToString();

                callLog.pincode = userVehicleData.pincode;

                ViewBag.FuelType = userVehicleData.fuelType;
                ViewBag.SeatingCapacity = ((userVehicleData.seating.ToString() != "0" && !string.IsNullOrEmpty(userVehicleData.seating.ToString())) ? userVehicleData.seating.ToString() : null);
                ViewBag.CubicCapacity = userVehicleData.Cubic_capacity; //string dt
                ViewBag.BatteryCapacity = userVehicleData.kilowatt; //decimal nullable dt
                ViewBag.ExShowroomPrice = userVehicleData.ExShowroom_Price; //int nullable
                ViewBag.YearOfManufacture = userVehicleData.yearofmanufacture; //int nullable
                ViewBag.RegistrationNo = userVehicleData.vehicleRegNo;
                ViewBag.ProductID = userVehicleData.Product_id; // int nullable
                ViewBag.Salutation = userVehicleData.salutation;
                ViewBag.FirstName = userVehicleData.first_name;
                ViewBag.LastName = userVehicleData.last_name;
                ViewBag.CompanySalutation = userVehicleData.companysalutation;
                ViewBag.CompanyName = userVehicleData.companyName;
                ViewBag.Designation = userVehicleData.designation;
                ViewBag.CPCodeOrDealerCode = userVehicleData.CP_CODE;
                ViewBag.OEMID = userVehicleData.FKOEM_ID; //int nullable
                ViewBag.IDV = userVehicleData.Basic_IDV_Tenure1; //int
                ViewBag.Gender = userVehicleData.gender;
                ViewBag.CustAddress = userVehicleData.Address_Line_1 + userVehicleData.Address_Line_2;
                ViewBag.MobileNumber = userVehicleData.Mobile;
                ViewBag.EmailAddress = userVehicleData.email;
                ViewBag.PanNo = userVehicleData.pan_no;
                ViewBag.TTPolicyNo = userVehicleData.TP_Policy_No;
            }
            #endregion

            callLog.eiblMasterStates = db.EIBLMasterStates.ToList();

            callLog.winbackPolicy = db.WinbackPolicy.Where(m => m.vehicleid == vehId & m.customerId == cusid).OrderByDescending(m => m.id).FirstOrDefault();

            if (callLog.vehi.Renewal_Type == null)
            {
                callLog.vehi.Renewal_Type = "PACKAGE";
            }

            if ((!(string.IsNullOrEmpty(callLog.vehi.model))) && db.model_masters.Count(m => m.Model_Name == callLog.vehi.model) > 0)
            {
                callLog.vehi.modelid = db.model_masters.FirstOrDefault(m => m.Model_Name == callLog.vehi.model).Model_Id;
            }


            //callLog.cust.emails = callLog.cust.emails.OrderByDescending(m => m.isPreferredEmail == true).ToList();
            callLog.cust.emails = db.Database.SqlQuery<email>("select * from email p join(select max(email_Id)emid from email where customer_id=@custId group by emailAddress)ph on ph.emid=p.email_Id and p.customer_id=@custId order by email_Id desc Limit 0,5", new MySqlParameter("@custId", cusid)).ToList();
            callLog.cust.emails = callLog.cust.emails.OrderByDescending(m => m.isPreferredEmail == true).ToList();

            //callLog.cust.addresses = callLog.cust.addresses.OrderByDescending(m => m.isPreferred == true).ToList();
            callLog.cust.addresses = db.Database.SqlQuery<address>("select * from address p join(select max(id)addId from address where customer_id=@custId group by concatenatedAdress)ph on ph.addId=p.id and p.customer_id=@custId where p.concatenatedAdress!='' order by isPreferred=true  desc,id desc Limit 0,5", new MySqlParameter("@custId", cusid)).ToList();
            callLog.custPhoneList = db.Database.SqlQuery<phone>("select * from phone p join(select max(phone_Id)phid from phone where customer_id=@custId group by phoneNumber)ph on ph.phid=p.phone_Id and p.customer_id=@custId order by phone_Id desc Limit 0,5", new MySqlParameter("@custId", cusid)).ToList();
            callLog.custPhoneList = callLog.custPhoneList.OrderByDescending(m => m.isPreferredPhone == true).ToList();
            callLog.nomineerelationsLists = db.Nomineerelations.OrderBy(m => m.relationship).ToList();
            var Viewcitystates = db.citystates.Select(h => new { state = h.state }).OrderBy(m => m.state).Distinct().ToList();
            var newList = Viewcitystates.OrderBy(x => x.state).ToList(); // ToList optional
            //string tableName=db.
            //callLog.RenewalCalculationInfo = db.Database.SqlQuery<renewalCalculationInfo>("select* from abv").FirstOrDefault();

            callLog.citystatesList = new List<SelectListItem>();
            callLog.stateList = new List<SelectListItem>();

            foreach (var city in newList)
            {
                callLog.citystatesList.Add(new SelectListItem { Text = city.state, Value = city.state });
                callLog.stateList.Add(new SelectListItem { Text = city.state, Value = city.state });
            }

            //callLog.winbackPolicy.proposerstate = 
            //var test = db.Financiers.ToList();
            List<phone> phone = db.phones.Where(m => m.customer_id == cusid).ToList();
            List<email> emails = db.emails.Where(m => m.customer_id == cusid).ToList();
            List<address> addresses = db.addresses.Where(m => m.customer_Id == cusid).ToList();
            callLog.Oem_master = db.oem_masters.ToList();
            callLog.Productmaster = db.Productmaster.ToList();
            callLog.Icmaster = db.Icmaster.ToList();
            callLog.rtos = db.rtos.ToList();
            callLog.nominee_Relations = db.Nominee_Relations.ToList();
            callLog.colors = db.colors.ToList();
            callLog.Financiers = db.Financiers.Take(100).ToList();
            callLog.model_masters = db.model_masters.Take(100).ToList();
            // callLog.Noipaiddrvrncleaners = db.Noipaiddrvrncleaners.ToList();
            // callLog.Soipaiddrvrncleaners = db.Soipaiddrvrncleaners.ToList();
            // callLog.Noiempotpaiddevrncleaners = db.Noiempotpaiddevrncleaners.ToList();
            // callLog.Soiempotppaiddrvrncleaners = db.Soiempotppaiddrvrncleaners.ToList();
            // callLog.Map_cp_financier_mapping = db.Map_cp_financier_mapping.ToList();
            // callLog.map_cp_financiers = db.map_cp_financiers.ToList();
            callLog.state_masters = db.state_masters.Take(100).ToList();
            callLog.city_masters = db.city_masters.Take(100).ToList();
            callLog.pincodemasters = db.pincodemasters.Take(100).ToList();
            callLog.Salutions = db.Salutions.Take(100).ToList();
            callLog.Income_Levels = db.Income_Levels.Take(100).ToList();
            callLog.Parking_Types = db.Parking_Types.Take(100).ToList();
            callLog.Occupation_Types = db.Occupation_Types.Take(100).ToList();
            callLog.Maritial_Statuses = db.Maritial_Statuses.Take(100).ToList();
            //callLog.Fueltypes = db.Fueltypes.ToList();
            callLog.Qualifications = db.Qualifications.Take(100).ToList();
            callLog.Id_Proofs = db.Id_Proofs.Take(100).ToList();

            callLog.workshopList = db.workshops.Where(m => m.workshopName == db.wyzusers.FirstOrDefault(K => K.id == UserId).location.name).ToList();
            if (DispoType == "Insurance")
            {

                callLog.walkinlocationList = db.fieldwalkinlocations.ToList();
                callLog.renewaltypes = db.renewaltypes.ToList();
                //callLog.walkinlocationLists = db.fieldwalkinlocations.Where(m => m.typeOfLocation == "2").ToList();
            }

            if (db.insurances.Where(m => m.vehicle != null && m.vehicle.vehicle_id == vehId).Count() != 0)
            {
                var lastInsurence = db.insurances.Where(m => m.vehicle != null && m.vehicle.vehicle_id == vehId).OrderByDescending(k => k.policyIssueDate).FirstOrDefault();

                if (lastInsurence != null)
                {
                    callLog.LatestInsurance = lastInsurence;
                }

            }

            //Assign CRE Data Workshop ect--------------------------->Insurance
            if (db.insuranceassignedinteractions.Count(m => m.customer_id == cusid && m.vehicle_vehicle_id == vehId) > 0)
            {
                insuranceassignedinteraction interasction = db.insuranceassignedinteractions.Include("wyzuser").Include("campaign").Include("wyzuser.workshop").FirstOrDefault(m => m.customer_id == cusid && m.vehicle_vehicle_id == vehId);
                if (interasction.wyzuser != null)
                {
                    callLog.InsAssignCRE = interasction.wyzuser.firstName + "(" + interasction.wyzuser.userName + ")";
                    if (interasction.location_id != 0 && interasction != null)
                    {
                        if (db.workshops.Count(m => m.location_cityId == interasction.location_id) > 0)
                        {
                            callLog.InsAssignWorkShop = db.workshops.FirstOrDefault(m => m.location_cityId == interasction.location_id).workshopName;
                        }
                        else
                        {
                            callLog.InsAssignWorkShop = "-";
                        }
                    }
                    else
                    {
                        callLog.InsAssignWorkShop = "-";
                    }

                }
                else
                {
                    callLog.InsAssignCRE = "Not Assigned";
                    callLog.InsAssignWorkShop = "-";
                }


                if (interasction.campaign != null)
                {

                    callLog.InsAssignCampaign = interasction.campaign.campaignName;
                    callLog.InsAssignCampaignId = interasction.campaign_id.ToString();
                }
                else
                {
                    callLog.InsAssignCampaign = "-";
                }
                if (interasction.herodealerID != null && interasction.herodealerID != 0)
                {
                    int delerId = Convert.ToInt32(interasction.herodealerID);
                    if (db.Herodealers.Count(m => m.DealerID == delerId) > 0)
                    {
                        callLog.INSAssignDealerName = db.Herodealers.FirstOrDefault(m => m.DealerID == delerId).DisplayName;
                    }
                    else
                    {
                        callLog.INSAssignDealerName = "-";
                    }
                }
                callLog.insuranceassignedinteractionId = interasction.id;
                callLog.herodealerId = interasction.herodealerID;

                if (interasction.uplodedCurrentDate != null)
                {
                    if (interasction.uplodedCurrentDate < Convert.ToDateTime("25-12-2021"))
                    {
                        callLog.isuploadedDate = true;
                    }
                    else
                    {
                        callLog.isuploadedDate = false;
                    }
                }
            }


            callLog.emailtemplates = getEmailTemplateList(DispoType);
            callLog.allworkshopList = db.workshops.Where(m => m.isinsurance == true).ToList();

            return callLog;
        }

        public List<emailtemplate> getEmailTemplateList(string typeofDis)
        {
            List<emailtemplate> template = new List<emailtemplate>();
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    if (typeofDis == "Service")
                    {
                        template = db.emailtemplates.Where(m => m.sendingType == "Manual" && (m.moduleType == 1 || m.moduleType == 3) && m.inActive == false).ToList();
                    }
                    else if (typeofDis == "Insurance")
                    {
                        template = db.emailtemplates.Where(m => m.sendingType == "Manual" && (m.moduleType == 3 || m.moduleType == 2) && m.inActive == false).ToList();
                    }
                    else if (typeofDis == "PSF")
                    {
                        template = db.emailtemplates.Where(m => m.sendingType == "Manual" && (m.moduleType == 3 || m.moduleType == 4) && m.inActive == false).ToList();
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return template;
        }

        [HttpPost, ActionName("customer360")]
        public ActionResult customer360(CallLoggingViewModel callLogging)
        {
            Logger logger = LogManager.GetLogger("apkRegLogger");

            string DispoType = Session["typeOfDispo"].ToString();
            string submissionResult = string.Empty, assignError = "";
            long cusId = Convert.ToInt32(Session["CusId"].ToString());
            long vehiId = Convert.ToInt32(Session["VehiId"].ToString());

            if (cusId != callLogging.CustomerId && vehiId != callLogging.VehicleId)
            {
                TempData["Exceptions"] = "Invalid disposition...";

                logger.Info("\n\n------- " + DispoType.ToUpper() + "Invalid Submition: " + DateTime.Now + "\n Disposing CustId: " + cusId + "Disposing VehId: " + vehiId + " User: " + Session["UserName"].ToString() + "\n Prev-Open Cust: " + callLogging.CustomerId + " Prev-Open VehiId" + callLogging.VehicleId);
                return RedirectToAction("LogOff", "Home");
            }

            logger.Info("\n\n------- " + DispoType.ToUpper() + " Submition started : " + DateTime.Now + "\n CustId: " + cusId + " VehId: " + vehiId + " User: " + Session["UserName"].ToString());
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    ListingForm listingFormData = callLogging.listingForm;
                    callinteraction callinteraction = callLogging.callinteraction;
                    servicebooked serviceBooked = callLogging.servicebooked;
                    calldispositiondata disposition = callLogging.finaldispostion;
                    insurancedisposition insudisposition = callLogging.insudisposition;
                    appointmentbooked appointmentbooked = callLogging.appointbooked;
                    customer custNew = callLogging.cust;

                    if (callinteraction == null)
                    {
                        callinteraction = new callinteraction();
                    }


                    if (DispoType == "insurance")
                    {
                        if (recordDisposition(0, "insurance", db, 0) == 0404)
                        {
                            assignError = assignDispoCRE("Insurance");
                            if (assignError != "True")
                            {
                                //goto errorWhileAssig;
                            }
                        }
                    }

                    if (insudisposition.typeOfDisposition == "Contact")
                    {
                        string action = insudisposition.dispoCustAns;

                        if (callinteraction == null)
                        {
                            callinteraction = new callinteraction();
                        }

                        try
                        {
                            if (DispoType == "insurance")
                            {
                                submissionResult = insuranceDataSubmit(action, callinteraction, listingFormData, insudisposition, appointmentbooked);
                                Session["appointBookId"] = null;
                            }
                        }
                        catch (Exception ex)
                        {

                        }

                    }
                    else if (insudisposition.typeOfDisposition == "NonContact")
                    {
                        long userId = Convert.ToInt32(Session["UserId"].ToString());
                        using (DbContextTransaction dbTransaction = db.Database.BeginTransaction())
                        {
                            try
                            {
                                {
                                    submissionResult = nonContact(callinteraction, listingFormData, insudisposition);//Non Contacts for insurance
                                    if (submissionResult == "True")
                                    {
                                        //if (currentDisposition == 6 || currentDisposition == 7 || currentDisposition == 8)
                                        //{
                                        //    autosmsday(userId, vehiId, cusId, "INSNONCONTACT", "Insurance", 0, 0, 0, "", 0);
                                        //}
                                        submissionResult = "True";
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                dbTransaction.Rollback();
                                if (ex.InnerException != null)
                                {
                                    if (ex.InnerException.InnerException != null)
                                    {
                                        submissionResult = ex.InnerException.InnerException.Message;
                                    }
                                    else
                                    {
                                        submissionResult = ex.InnerException.Message;
                                    }
                                }
                                else
                                {
                                    submissionResult = ex.Message;
                                }
                            }
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        submissionResult = ex.InnerException.InnerException.Message;
                    }
                    else
                    {
                        submissionResult = ex.InnerException.Message;
                    }
                }
                else
                {
                    submissionResult = ex.Message;
                }


            }
            TempData["SubmissionResult"] = submissionResult;
            logger.Info("\n\n------- " + DispoType.ToUpper() + " Submition Ended : " + DateTime.Now);
            if (DispoType == "insurance")
            {
                return RedirectToAction("ReturnToBucket", new { @id = 2 });
            }
            else if (DispoType == "Service")
            {
                return RedirectToAction("ReturnToBucket", new { @id = 1 });
            }
            else
            {
                return RedirectToAction("ReturnToBucket", new { @id = 1 });
            }

        }
        public string insuranceDataSubmit(string action, callinteraction callInter, ListingForm listing, insurancedisposition irData, appointmentbooked appointmentbooked)
        {
            string submisstionResult = string.Empty;
            long cusId = Convert.ToInt32(Session["CusId"].ToString());
            long vehiId = Convert.ToInt32(Session["VehiId"].ToString());
            long userId = Convert.ToInt32(Session["UserId"].ToString());
            using (var db = new AutoSherDBContext())
            {
                try
                {
                    bool iskalyani = false;
                    if (Session["OEM"].ToString() == "MARUTI SUZUKI")
                    {
                        iskalyani = true;
                    }
                    submisstionResult = _insuranceOutBoundSubmit(callInter, irData, listing, appointmentbooked, action);

                }
                catch (Exception ex)
                {

                }

            }
            return submisstionResult;
        }
        public string _insuranceOutBoundSubmit(callinteraction callinteraction, insurancedisposition ir_disposition, ListingForm listingForm, appointmentbooked appointmentbooked, string bookingMode)
        {
            long cusId = Convert.ToInt32(Session["CusId"].ToString());
            long vehiId = Convert.ToInt32(Session["VehiId"].ToString());
            long userId = Convert.ToInt32(Session["UserId"].ToString());
            long lastinsuraceId = 0;
            bool bookedExist = false;
            long agentId = 0;
            using (var db = new AutoSherDBContext())
            {
                using (DbContextTransaction dbTrans = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (db.insuranceassignedinteractions.Count(m => m.customer_id == cusId && m.vehicle_vehicle_id == vehiId) > 0)
                        {
                            bool isSuperCre = false;
                            // bool isINSSuperControl = false;
                            bool isUserControl = db.dealers.FirstOrDefault().userControl ?? default(bool);

                            if (Session["IsSuperCRE"] != null && Convert.ToBoolean(Session["IsSuperCRE"]) == true)
                            {
                                isSuperCre = true;

                                // isINSSuperControl = db.dealers.FirstOrDefault().INSsuperControl ?? default(bool);
                            }

                            string callLogFrom = string.Empty;

                            if (Session["MakeCallFrom"] != null)
                            {
                                callLogFrom = Session["MakeCallFrom"].ToString();
                            }
                            else
                            {
                                callLogFrom = "bucket";
                            }

                            if (callLogFrom != "bucket")
                            {
                                if (isSuperCre == true)
                                {
                                    long userIdExist = db.insuranceassignedinteractions.FirstOrDefault(m => m.customer_id == cusId && m.vehicle_vehicle_id == vehiId).wyzUser_id ?? default(long);
                                    if (userIdExist != 0)
                                    {
                                        userId = userIdExist;
                                    }
                                }

                            }

                            appointmentbooked appointExist = new appointmentbooked();
                            long? appointMaxId = 0;
                            if (bookingMode == "Book Appointment" || bookingMode == "Renewal Not Required" || bookingMode == "ConfirmedInsu" || bookingMode == "Reschedule" || bookingMode == "Cancel Appointment" || bookingMode == "Call Me Later" || bookingMode == "INS Others")
                            {
                                int currentDisposition = 0, cancelDisposition = 0;
                                long ass_id = 0;
                                if (bookingMode != "INS Others" && bookingMode != "Cancel Appointment" && bookingMode != "ConfirmedInsu")
                                {
                                    currentDisposition = db.calldispositiondatas.FirstOrDefault(m => m.disposition == bookingMode).dispositionId;
                                }
                                else if (bookingMode == "ConfirmedInsu")
                                {
                                    currentDisposition = db.calldispositiondatas.FirstOrDefault(m => m.disposition == "Confirmed").dispositionId;
                                }
                                else if (bookingMode == "Cancel Appointment")
                                {
                                    currentDisposition = db.calldispositiondatas.FirstOrDefault(m => m.disposition == "Cancelled").dispositionId;
                                }
                                else if (bookingMode == "INS Others")
                                {
                                    string dispo = listingForm.othersINS;
                                    if (dispo == "")
                                    {
                                        currentDisposition = db.calldispositiondatas.FirstOrDefault(m => m.disposition == "INS Others").dispositionId;
                                    }
                                    else
                                    {
                                        currentDisposition = db.calldispositiondatas.FirstOrDefault(m => m.disposition == dispo).dispositionId;
                                    }
                                }


                                if (bookingMode == "Book Appointment" || bookingMode == "Reschedule")
                                {
                                    long appId = Convert.ToInt64(Session["appointBookId"]);
                                    int appIdCount = db.appointmentbookeds.Count(m => m.appointmentId == appId && m.customer_id == cusId);
                                    if (appIdCount > 0)
                                    {
                                        appointmentbooked rescheduleappointment = new appointmentbooked();


                                        cancelDisposition = db.calldispositiondatas.FirstOrDefault(m => m.disposition == "Cancelled").dispositionId;
                                        rescheduleappointment = db.appointmentbookeds.FirstOrDefault(m => m.appointmentId == appId && m.customer_id == cusId);
                                        rescheduleappointment.insuranceBookStatus_id = cancelDisposition;

                                        db.appointmentbookeds.AddOrUpdate(rescheduleappointment);
                                        //Session["AppointmentDate"] = appointmentbooked.appointmentDate;
                                        db.SaveChanges();
                                        bookedExist = true;
                                    }

                                    if (appointmentbooked.appointmentDate == DateTime.Now.Date)
                                    {
                                        appointmentbooked.typeOfPickup = "Today";
                                    }
                                    else if (appointmentbooked.appointmentDate == DateTime.Now.AddDays(1).Date)
                                    {
                                        appointmentbooked.typeOfPickup = "Tomorrow";
                                    }
                                    else if (appointmentbooked.appointmentDate > DateTime.Now && appointmentbooked.appointmentDate <= DateTime.Now.AddDays(7).Date)
                                    {
                                        appointmentbooked.typeOfPickup = "In 7 days";
                                    }
                                    else if (appointmentbooked.appointmentDate > DateTime.Now && appointmentbooked.appointmentDate <= DateTime.Now.AddDays(15).Date)
                                    {
                                        appointmentbooked.typeOfPickup = "In 15 days";
                                    }
                                    else if (appointmentbooked.appointmentDate > DateTime.Now && appointmentbooked.appointmentDate <= DateTime.Now.AddDays(30).Date)
                                    {
                                        appointmentbooked.typeOfPickup = "In 30 days";
                                    }



                                    appointmentbooked.customer_id = cusId;
                                    appointmentbooked.chaserId = userId;
                                    appointmentbooked.wyzuser_id = userId;
                                    appointmentbooked.vehicle_id = vehiId;
                                    appointmentbooked.insuranceBookStatus_id = currentDisposition;

                                    db.appointmentbookeds.Add(appointmentbooked);
                                    db.SaveChanges();
                                }
                                else
                                /*when user clicks renewal not required,cancel,confirm,call me later,paid if the appointment is already booked then appointment table updated otherwise no action required in appointment table.*/
                                {
                                    appointMaxId = getInsuranceAppointmentStatus();
                                    if (appointMaxId != 0)
                                    {
                                        appointExist = db.appointmentbookeds.FirstOrDefault(m => m.appointmentId == appointMaxId);

                                        if (bookingMode == "ConfirmedInsu" || bookingMode == "Renewal Not Required" || bookingMode == "Cancel Appointment" || bookingMode == "Call Me Later" || bookingMode == "INS Others")
                                        {
                                            appointExist.customer_id = cusId;
                                            appointExist.vehicle_id = vehiId;
                                            appointExist.wyzuser_id = userId;
                                            appointExist.insuranceBookStatus_id = currentDisposition;
                                            db.appointmentbookeds.AddOrUpdate(appointExist);
                                            db.SaveChanges();
                                        }
                                    }
                                }

                                ass_id = recordDisposition(2, "insurance", db, currentDisposition, 0, isSuperCre);



                                //***************** CallInteraction starts******************//

                                callinteraction.callCount = 1;
                                callinteraction.callDate = DateTime.Now.ToString("dd-MM-yyyy");
                                callinteraction.callMadeDateAndTime = DateTime.Now;
                                callinteraction.callTime = DateTime.Now.ToString("HH:mm:ss");
                                callinteraction.dealerCode = Session["DealerCode"].ToString();
                                callinteraction.isCallinitaited = "No";
                                if (Session["isCallInitiated"] != null)
                                {
                                    callinteraction.isCallinitaited = "Yes";
                                    if (Session["MakeCallFrom"] != null)
                                    {
                                        callinteraction.makeCallFrom = Session["MakeCallFrom"].ToString();
                                    }
                                    else
                                    {
                                        callinteraction.makeCallFrom = "insurance";
                                    }

                                    if (Session["AndroidUniqueId"] != null)
                                    {
                                        callinteraction.uniqueidForCallSync = int.Parse(Session["AndroidUniqueId"].ToString());
                                    }
                                    else if (Session["GSMUniqueId"] != null)
                                    {
                                        callinteraction.uniqueIdGSM = Session["GSMUniqueId"].ToString().Split(';')[0];
                                    }
                                }
                                callinteraction.callStatus = Convert.ToBoolean(Session["NCReason"]);
                                if (Session["MakeCallFrom"] != null)
                                {
                                    callinteraction.makeCallFrom = Session["MakeCallFrom"].ToString();
                                }
                                else
                                {
                                    callinteraction.makeCallFrom = "insurance";
                                }

                                if (Session["DialedNumber"] != null)
                                {
                                    callinteraction.dailedNoIs = Session["DialedNumber"].ToString();
                                }
                                callinteraction.insuranceAssignedInteraction_id = ass_id;
                                callinteraction.customer_id = cusId;
                                callinteraction.vehicle_vehicle_id = vehiId;
                                callinteraction.wyzUser_id = userId;

                                if (bookingMode == "Book Appointment" || bookingMode == "Reschedule")
                                {
                                    callinteraction.appointmentBooked_appointmentId = appointmentbooked.appointmentId;
                                }
                                else
                                {
                                    if (appointMaxId != 0)
                                    {
                                        callinteraction.appointmentBooked_appointmentId = appointExist.appointmentId;

                                    }

                                }



                                callinteraction.agentName = Session["UserName"].ToString();
                                callinteraction.campaign_id = db.insuranceassignedinteractions.FirstOrDefault(m => m.id == ass_id).campaign_id;

                                callinteraction.chasserCall = false;
                                db.callinteractions.Add(callinteraction);
                                db.SaveChanges();


                                if (Session["GSMUniqueId"] != null)
                                {
                                    gsmsynchdata gsm = new gsmsynchdata();
                                    gsm.Callinteraction_id = callinteraction.id;
                                    gsm.CallMadeDateTime = DateTime.Now;
                                    gsm.UniqueGsmId = Session["GSMUniqueId"].ToString().Split(';')[0];
                                    gsm.TenantUrl = Session["GSMUniqueId"].ToString().Split(';')[1];

                                    db.gsmsynchdata.Add(gsm);
                                    db.SaveChanges();
                                }
                                long callId = callinteraction.id;

                                //***************** CallInteraction End******************

                                string remarks = "", comments = "";


                                int upselCount = 0;
                                //********************** insurance_Disposition Saving part ********************
                                if (ir_disposition.typeOfAutherization == "Unauthorized Dealer")
                                {
                                    ir_disposition.dateOfRenewal = listingForm.dateOfRenewalNonAuth;
                                    ir_disposition.insuranceProvidedBy = listingForm.insuranceProvidedUnAuth;
                                }

                                if (listingForm.VehicleSoldYes == "VehicleSold Yes")
                                {
                                    soldNewCustomerVehicles vehiclesold = new soldNewCustomerVehicles();
                                    vehiclesold.custmerId = cusId;
                                    vehiclesold.wyzuserid = userId;
                                    vehiclesold.customerFName = listingForm.customerFName;
                                    vehiclesold.customerLName = listingForm.customerLName;
                                    vehiclesold.state = listingForm.state;
                                    vehiclesold.city = listingForm.city;
                                    vehiclesold.vehicleRegNo = listingForm.vehicleRegNo;
                                    vehiclesold.chassisNo = listingForm.chassisNo;
                                    vehiclesold.engineNo = listingForm.engineNo;
                                    vehiclesold.variant = listingForm.variant;
                                    vehiclesold.model = listingForm.model;
                                    vehiclesold.dealershipName = listingForm.dealershipName;
                                    vehiclesold.saleDate = listingForm.saleDate;
                                    List<string> phoneList = listingForm.phoneList;
                                    string phoneNo = "";
                                    foreach (var phone in phoneList)
                                    {
                                        phoneNo = phoneNo + "," + phone;
                                    }
                                    vehiclesold.addressLine1 = listingForm.addressLine1;
                                    vehiclesold.addressLine2 = listingForm.addressLine2;
                                    vehiclesold.pincode = listingForm.pincode;
                                    vehiclesold.initial = listingForm.initial;
                                    db.SoldNewCustomerVehicles.Add(vehiclesold);
                                    db.SaveChanges();
                                }

                                if (listingForm.assignedSA != null)
                                {
                                    long asignedSaId = db.taggingusers.FirstOrDefault(m => m.name == listingForm.assignedSA).id;
                                    ir_disposition.assignedToSA = asignedSaId;
                                }

                                ir_disposition.othersINS = listingForm.othersINS;


                                ir_disposition.InOutCallName = "OutCall";
                                ir_disposition.callDispositionData_id = currentDisposition;
                                ir_disposition.callInteraction_id = callinteraction.id;
                                ir_disposition.callinteraction = callinteraction;
                                ir_disposition.upsellCount = upselCount;



                                //   if (isSuperCre == true && isINSSuperControl == false)
                                if (isSuperCre == true)
                                {
                                    ir_disposition.customerComments = ir_disposition.customerComments + " - By-" + Session["UserName"].ToString();
                                    ir_disposition.comments = ir_disposition.comments + " - By-" + Session["UserName"].ToString();
                                }
                                db.insurancedispositions.Add(ir_disposition);
                                db.SaveChanges();

                                db.Database.ExecuteSqlCommand("call Triggerinsertinsurancecallhistrycube(@newid);", new MySqlParameter("@newid", callId));

                            }
                            dbTrans.Commit();

                            //------------------------- Triggering AutoSMS ----------------------------

                            if (bookingMode == "Book Appointment" || bookingMode == "Reschedule")
                            {
                                // autosmsday(userId, vehiId, cusId, "APPOINTMENT", "Insurance", 0, 0, 0, "", 0);
                            }
                            string customMsg = string.Empty;
                            //Complaints
                            if (ir_disposition.renewalNotRequiredReason != null && (ir_disposition.renewalNotRequiredReason == "Dissatisfied with previous service" || ir_disposition.renewalNotRequiredReason == "Dissatisfied with Sales" || ir_disposition.renewalNotRequiredReason == "Dissatisfied with Insurance"))
                            {
                                string smsTypeva = "COMPLAINT";
                                // autosmsday(userId, vehiId, cusId, smsTypeva, "Insurance", 0, 0, 0, "COMPLAINT", 0);

                                // dissat sms
                                int taggingId = 0;
                                if (ir_disposition.renewalNotRequiredReason == "Dissatisfied with Sales")
                                {
                                    taggingId = 26;
                                }
                                else if (ir_disposition.renewalNotRequiredReason == "Dissatisfied with Insurance")
                                {
                                    taggingId = 27;
                                }
                                else
                                {
                                    taggingId = 29;
                                }

                                string smsType1 = "INSDISSAT";

                                customMsg = "INS DISSAT : " + ir_disposition.renewalNotRequiredReason;
                                //autosmsday(userId, vehiId, cusId, smsType1, "Insurance", taggingId, 0, 0, customMsg, 0);

                            }

                            // feedback sms
                            if (listingForm.CustomerfeedbackRNR == "Yes")
                            {
                                long departmentId = long.Parse(ir_disposition.departmentForFB);

                                complainttype complainttype = db.complainttypes.FirstOrDefault(m => m.id == departmentId);

                                string dept = complainttype.departmentName;
                                customMsg = "INS Feedback : " + dept;
                                // autosmsday(userId, vehiId, cusId, "INSFEEDBACK", "Insurance", 0, 0, departmentId, customMsg, 0);
                            }
                        }
                        else
                        {


                        }
                    }
                    catch (Exception ex)
                    {
                        dbTrans.Rollback();
                        string exception = string.Empty;
                        if (ex.InnerException != null)
                        {
                            if (ex.InnerException.InnerException != null)
                            {
                                exception = ex.InnerException.InnerException.Message;
                            }
                            else
                            {
                                exception = ex.InnerException.Message;
                            }
                        }
                        else
                        {
                            exception = ex.Message;
                        }

                        if (ex.StackTrace.Contains(':'))
                        {
                            exception += " " + ex.StackTrace.Split(':')[(ex.StackTrace.Split(':').Count() - 1)];
                        }

                        Logger logger = LogManager.GetLogger("apkRegLogger");

                        logger.Info("_insuranceBound Lock: \n" + ex.StackTrace + "\n" + exception);
                        return exception;
                    }

                }

            }
            return "True";
        }

        public string nonContact(callinteraction callInter, ListingForm listing, insurancedisposition irData)
        {
            long cusId = Convert.ToInt32(Session["CusId"].ToString());
            long vehiId = Convert.ToInt32(Session["VehiId"].ToString());
            long userId = Convert.ToInt32(Session["UserId"].ToString());
            int currentDisposition = 0;
            string submissionResult = "";
            long ass_id = 0;
            using (var db = new AutoSherDBContext())
            {
                using (DbContextTransaction dbTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        bool isSuperCre = false;
                        //bool isINSSuperControl = false;
                        bool isUserControl = db.dealers.FirstOrDefault().userControl ?? default(bool);

                        if (Session["IsSuperCRE"] != null && Convert.ToBoolean(Session["IsSuperCRE"]) == true)
                        {
                            isSuperCre = true;

                            // isINSSuperControl = db.dealers.FirstOrDefault().INSsuperControl ?? default(bool);
                        }

                        string callLogFrom = string.Empty;

                        if (Session["MakeCallFrom"] != null)
                        {
                            callLogFrom = Session["MakeCallFrom"].ToString();
                        }
                        else
                        {
                            callLogFrom = "bucket";
                        }

                        if (callLogFrom != "bucket")
                        {
                            if (isSuperCre == true)
                            {
                                long userIdExist = db.insuranceassignedinteractions.FirstOrDefault(m => m.customer_id == cusId && m.vehicle_vehicle_id == vehiId).wyzUser_id ?? default(long);
                                if (userIdExist != 0)
                                {
                                    userId = userIdExist;
                                }
                            }

                        }

                        if (irData.dispoNotTalk == "Other")
                        {
                            irData.dispoNotTalk = "NoOther";
                        }
                        currentDisposition = db.calldispositiondatas.FirstOrDefault(m => m.disposition == irData.dispoNotTalk).dispositionId;
                        long? insuranceMaxId = 0;
                        long? appId = Convert.ToInt64(Session["appointBookId"]);

                        insuranceMaxId = getInsuranceAppointmentStatus();
                        appointmentbooked insuranceExit = new appointmentbooked();
                        if (insuranceMaxId != 0)
                        {
                            insuranceExit = db.appointmentbookeds.FirstOrDefault(m => m.appointmentId == insuranceMaxId);
                            insuranceExit.customer_id = cusId;
                            insuranceExit.vehicle_id = vehiId;
                            insuranceExit.wyzuser_id = userId;
                            db.appointmentbookeds.AddOrUpdate(insuranceExit);
                            db.SaveChanges();
                        }

                        //**************** Updating AssignedInteraction *********************

                        ass_id = recordDisposition(2, "insurance", db, currentDisposition);

                        //****************************End***************************

                        //***************** CallInteraction starts******************
                        callInter.callCount = 1;
                        callInter.callDate = DateTime.Now.ToString("dd-MM-yyyy");
                        callInter.callMadeDateAndTime = DateTime.Now;
                        callInter.callTime = DateTime.Now.ToString("HH:mm:ss");
                        callInter.dealerCode = Session["DealerCode"].ToString();
                        callInter.isCallinitaited = "No";

                        if (Session["isCallInitiated"] != null)
                        {
                            callInter.isCallinitaited = "Yes";
                            if (Session["MakeCallFrom"] != null)
                            {
                                callInter.makeCallFrom = Session["MakeCallFrom"].ToString();
                            }
                            else
                            {
                                callInter.makeCallFrom = "insurance";
                            }

                            if (Session["UniqueId"] != null)
                            {
                                callInter.uniqueIdGSM = Session["UniqueId"].ToString();
                            }
                        }
                        callInter.callStatus = Convert.ToBoolean(Session["NCReason"]);
                        if (Session["MakeCallFrom"] != null)
                        {
                            callInter.makeCallFrom = Session["MakeCallFrom"].ToString();
                        }
                        else
                        {
                            callInter.makeCallFrom = "Service";
                        }
                        if (Session["DialedNumber"] != null)
                        {
                            callInter.dailedNoIs = Session["DialedNumber"].ToString();
                        }

                        callInter.makeCallFrom = "insurance";
                        callInter.insuranceAssignedInteraction_id = ass_id;
                        callInter.customer_id = cusId;
                        callInter.vehicle_vehicle_id = vehiId;
                        callInter.wyzUser_id = userId;
                        callInter.agentName = Session["UserName"].ToString();
                        callInter.chasserCall = false;
                        callInter.campaign_id = db.insuranceassignedinteractions.FirstOrDefault(m => m.id == ass_id).campaign_id;
                        if (insuranceMaxId != 0)
                        {
                            callInter.appointmentBooked_appointmentId = insuranceExit.appointmentId;
                        }
                        int taggId = 0;
                        string camp = "";
                        callInter.tagging_id = taggId;
                        callInter.tagging_name = camp;
                        callInter.chasserCall = false;
                        db.callinteractions.Add(callInter);
                        db.SaveChanges();

                        if (Session["GSMUniqueId"] != null)
                        {
                            gsmsynchdata gsm = new gsmsynchdata();
                            gsm.Callinteraction_id = callInter.id;
                            gsm.CallMadeDateTime = DateTime.Now;
                            gsm.UniqueGsmId = Session["GSMUniqueId"].ToString().Split(';')[0];
                            gsm.TenantUrl = Session["GSMUniqueId"].ToString().Split(';')[1];

                            db.gsmsynchdata.Add(gsm);
                            db.SaveChanges();
                        }
                        irData.customerComments = irData.customerComments;
                        irData.comments = irData.comments;


                        int upselCount = 0;

                        //********************** Insurance_Disposition Saving part ********************
                        irData.InOutCallName = "OutCall";
                        irData.cityName = listing.cityName;
                        irData.callDispositionData_id = currentDisposition;
                        irData.callInteraction_id = callInter.id;
                        irData.upsellCount = upselCount;

                        if (isSuperCre == true)
                        {
                            irData.customerComments = irData.customerComments + " - By-" + Session["UserName"].ToString();
                            irData.comments = irData.comments + " - By-" + Session["UserName"].ToString();
                        }

                        db.insurancedispositions.Add(irData);
                        db.SaveChanges();
                        db.Database.ExecuteSqlCommand("call Triggerinsertinsurancecallhistrycube(@newid);", new MySqlParameter("@newid", callInter.id));
                        dbTransaction.Commit();
                        submissionResult = "True";
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();

                        if (ex.InnerException != null)
                        {
                            if (ex.InnerException.InnerException != null)
                            {
                                submissionResult = ex.InnerException.InnerException.Message;
                            }
                            else
                            {
                                submissionResult = ex.InnerException.Message;
                            }
                        }
                        else
                        {
                            submissionResult = ex.Message;
                        }

                    }
                }
            }

            return submissionResult;
        }
        public string assignDispoCRE(string dispoType)
        {
            long cusId = Convert.ToInt32(Session["CusId"].ToString());
            long vehiId = Convert.ToInt32(Session["VehiId"].ToString());
            long userId = Convert.ToInt32(Session["UserId"].ToString());

            try
            {
                using (var db = new AutoSherDBContext())
                {
                    using (var dbTrans = db.Database.BeginTransaction())
                    {
                        long campId = db.campaigns.FirstOrDefault(m => m.campaignName == "Customer Search").id;
                        long managerId = db.wyzusers.FirstOrDefault(m => m.userName == db.wyzusers.FirstOrDefault(x => x.id == userId).creManager && m.role == "CREManager").id;


                        if (dispoType == "Insurance")
                        {
                            insuranceassignedinteraction insu_assign = new insuranceassignedinteraction();

                            insu_assign.callMade = "No";
                            insu_assign.displayFlag = false;
                            insu_assign.uplodedCurrentDate = DateTime.Now;
                            insu_assign.customer_id = cusId;
                            insu_assign.vehicle_vehicle_id = vehiId;
                            insu_assign.wyzUser_id = userId;
                            insu_assign.campaign_id = campId;
                            // insu_assign.policyDueDate = DateTime.Now.AddDays(2);

                            if (db.insuranceforecasteddatas.Any(m => m.vehicle_id == vehiId))
                            {
                                insuranceforecasteddata insForecast = new insuranceforecasteddata();

                                if (insForecast.location_id != 0)
                                {
                                    insu_assign.location_id = insForecast.location_id;
                                }
                                else
                                {
                                    insu_assign.location_id = db.vehicles.FirstOrDefault(m => m.vehicle_id == vehiId).insduelocation_id;
                                }
                            }
                            else
                            {
                                if (db.vehicles.Count(m => m.vehicle_id == vehiId) > 0)
                                {
                                    vehicle veh = db.vehicles.FirstOrDefault(m => m.vehicle_id == vehiId);
                                    insu_assign.location_id = veh.insduelocation_id;
                                    if (veh.Previous_Policy_Expiry != null)
                                    {
                                        insu_assign.policyDueDate = Convert.ToDateTime(veh.Previous_Policy_Expiry);
                                    }
                                }

                            }
                            db.insuranceassignedinteractions.Add(insu_assign);
                            db.SaveChanges();


                            assignedcallsreport assignCalls = new assignedcallsreport();

                            assignCalls.assignInteractionID = insu_assign.id;
                            assignCalls.assignedDate = DateTime.Now;
                            assignCalls.assignmentType = "Service";
                            assignCalls.moduletypeId = 1;
                            assignCalls.uploadId = "0";
                            assignCalls.vehicleId = vehiId;
                            assignCalls.wyzuserId = userId;
                            assignCalls.campaignId = campId;
                            assignCalls.assigned_manager_id = managerId;

                            if (db.insuranceforecasteddatas.Any(m => m.vehicle_id == vehiId))
                            {
                                insuranceforecasteddata insForecast = new insuranceforecasteddata();

                                insForecast = db.insuranceforecasteddatas.FirstOrDefault(m => m.vehicle_id == vehiId);
                                assignCalls.dueType = insForecast.renewaltype;
                                assignCalls.dueDate = insForecast.policyexpirydate;
                            }
                            else
                            {
                            }
                            assignCalls.isautoassigned = false;

                            db.assignedcallsreports.Add(assignCalls);
                            db.SaveChanges();
                        }

                        dbTrans.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        return ex.InnerException.InnerException.Message;
                    }
                    else
                    {
                        return ex.InnerException.Message;
                    }
                }
                else
                {
                    return ex.Message;
                }
            }

            return "True";
        }

        public long getInsuranceAppointmentStatus()
        {
            long appointMaxId = 0, presentAssignId = 0;
            long cusId = Convert.ToInt32(Session["CusId"].ToString());
            long vehiId = Convert.ToInt32(Session["VehiId"].ToString());
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    if (db.appointmentbookeds.Count(m => m.customer_id == cusId && m.vehicle_id == vehiId && m.insuranceBookStatus_id != 35) > 0)
                    {
                        appointMaxId = db.appointmentbookeds.Where(m => m.customer_id == cusId && m.vehicle_id == vehiId).Max(m => m.appointmentId);
                    }

                    if (appointMaxId != 0)
                    {
                        if (db.callinteractions.Count(m => m.appointmentBooked_appointmentId == appointMaxId) > 0)
                        {
                            presentAssignId = recordDisposition(0, "insurance", db, 0);
                            if (presentAssignId != 0404)
                            {
                                long maxCallInterId = db.callinteractions.Where(m => m.appointmentBooked_appointmentId == appointMaxId).Max(m => m.id);
                                long? LatestAssinIdInCallInter = db.callinteractions.FirstOrDefault(m => m.id == maxCallInterId).insuranceAssignedInteraction_id;

                                if (LatestAssinIdInCallInter == presentAssignId)
                                {
                                    return appointMaxId;
                                }
                                else
                                {
                                    return 0;
                                }
                            }
                            else
                            {
                                return 0;
                            }
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return appointMaxId;
        }

        public long recordDisposition(int stage, string typeOfDipo, AutoSherDBContext db, int? finalDispoId = 0, long assignWyzId = 0, bool isSuperCre = false, bool isEscalation = false, long pd_location = 0, string policyDropDate = "", long feid = 0, string pinCode = "", long pickupId = 0)
        {
            long cusId = Convert.ToInt32(Session["CusId"].ToString());
            long vehiId = Convert.ToInt32(Session["VehiId"].ToString());
            long userId = Convert.ToInt32(Session["UserId"].ToString());
            //try
            //{
            //using (var db = new AutoSherDBContext())
            //{
            if (typeOfDipo == "insurance")
            {
                if (stage == 0)//NotDialed
                {
                    if (db.insuranceassignedinteractions.Any(m => m.customer_id == cusId && m.vehicle_vehicle_id == vehiId))
                    {
                        return db.insuranceassignedinteractions.FirstOrDefault(m => m.customer_id == cusId && m.vehicle_vehicle_id == vehiId).id;
                    }
                    else
                    {
                        return 0404;
                    }
                }
                else if (stage == 1)//Initiated
                {
                    if (db.insuranceassignedinteractions.Where(m => m.customer_id == cusId && m.vehicle_vehicle_id == vehiId && m.wyzUser_id == userId).Count() != 0)
                    {
                        insuranceassignedinteraction assInter = db.insuranceassignedinteractions.FirstOrDefault(m => m.customer_id == cusId && m.vehicle_vehicle_id == vehiId && m.wyzUser_id == userId);
                        assInter.callMade = "Initiated";
                        //assInter.uplodedCurrentDate = DateTime.Now;
                        db.SaveChanges();
                        return assInter.id;
                    }
                }
                else if (stage == 2)//Yes
                {
                    bool isUserControl = db.dealers.FirstOrDefault().userControl ?? default(bool);
                    long loginUserId = long.Parse(Session["UserId"].ToString());

                    insuranceassignedinteraction assInter = db.insuranceassignedinteractions.FirstOrDefault(m => m.customer_id == cusId && m.vehicle_vehicle_id == vehiId);
                    assInter.callMade = "Yes";
                    //assInter.uplodedCurrentDate = DateTime.Now;
                    //swapping finaldisposition to lastDisposition..............
                    calldispositiondata calldispositiondata = new calldispositiondata();
                    if (assInter.finalDisposition_id != null)
                    {
                        calldispositiondata = db.calldispositiondatas.FirstOrDefault(m => m.id == assInter.finalDisposition_id);
                        assInter.lastDisposition = calldispositiondata.disposition;
                    }
                    if (pd_location != 0 && policyDropDate != "")
                    {
                        assInter.pd_locationId = pd_location;
                        assInter.appointmentDate = Convert.ToDateTime(policyDropDate);
                        if (feid != 0 && pickupId != 0)
                        {
                            assInter.FEID = feid;
                            assInter.policyPincode = pinCode;
                            assInter.pickUPID = pickupId;
                        }
                    }
                    assInter.finalDisposition_id = finalDispoId;

                    if ((isSuperCre == true && assInter.wyzUser_id != loginUserId && (assInter.wyzUser_id == null || assInter.wyzUser_id == 0)))
                    {
                        change_assignment_records changeAssignRecord = new change_assignment_records();
                        changeAssignRecord.assignedinteraction_id = assInter.id;
                        changeAssignRecord.campaign_id = assInter.campaign_id ?? default(long);
                        changeAssignRecord.last_wyzuserId = assInter.wyzUser_id ?? default(long);

                        changeAssignRecord.new_wyzuserId = loginUserId;
                        changeAssignRecord.updatedDate = DateTime.Now;
                        changeAssignRecord.moduletypeIs = 2;
                        db.change_assignment_records.Add(changeAssignRecord);

                        assInter.wyzUser_id = loginUserId; //-----------------> change assignments......

                        db.SaveChanges();
                    }

                    db.insuranceassignedinteractions.AddOrUpdate(assInter);
                    db.SaveChanges();
                    return assInter.id;

                }
                else if (stage == 3)//No
                {
                    if (db.insuranceassignedinteractions.Where(m => m.customer_id == cusId && m.vehicle_vehicle_id == vehiId && m.wyzUser_id == userId).Count() != 0)
                    {
                        insuranceassignedinteraction assInter = db.insuranceassignedinteractions.FirstOrDefault(m => m.customer_id == cusId && m.vehicle_vehicle_id == vehiId && m.wyzUser_id == userId);
                        assInter.callMade = "No";
                        //assInter.uplodedCurrentDate = DateTime.Now;

                        //swapping finaldisposition to lastDisposition..............
                        calldispositiondata calldispositiondata = new calldispositiondata();
                        if (assInter.finalDisposition_id != null)
                        {
                            calldispositiondata = db.calldispositiondatas.FirstOrDefault(m => m.id == assInter.finalDisposition_id);
                            assInter.lastDisposition = calldispositiondata.disposition;
                        }
                        assInter.finalDisposition_id = finalDispoId;
                        db.SaveChanges();
                        return assInter.id;

                    }
                }
            }
            //}
            //}
            //catch (Exception ex)
            //{

            //}
            return 0;
        }

        [HttpPost, ActionName("saverenewalPolicy")]
        public ActionResult saverenewalPolicy(CallLoggingViewModel callLogging)
        {
            Logger logger = LogManager.GetLogger("apkRegLogger");
            string DispoType = Session["typeOfDispo"].ToString();
            string submissionResult = string.Empty, assignError = "";
            long cusId = Convert.ToInt32(Session["CusId"].ToString());
            long vehiId = Convert.ToInt32(Session["VehiId"].ToString());



            if (cusId != callLogging.CustomerId && vehiId != callLogging.VehicleId)
            {
                TempData["Exceptions"] = "Invalid disposition...";

                logger.Info("\n\n------- " + DispoType.ToUpper() + "Invalid Submition: " + DateTime.Now + "\n Disposing CustId: " + cusId + "Disposing VehId: " + vehiId + " User: " + Session["UserName"].ToString() + "\n Prev-Open Cust: " + callLogging.CustomerId + " Prev-Open VehiId" + callLogging.VehicleId);
                return RedirectToAction("LogOff", "Home");
            }

            logger.Info("\n\n------- " + DispoType.ToUpper() + " Submition started : " + DateTime.Now + "\n CustId: " + cusId + " VehId: " + vehiId + " User: " + Session["UserName"].ToString());
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    // winbackpolicyrenewalresponse winbackpolicyDetails = new winbackpolicyrenewalresponse();
                    vehicle policydetails = callLogging.vehi;
                    insurancerenewalpolicy renewaldetails = new insurancerenewalpolicy();
                    renewaldetails.customerid = cusId;
                    renewaldetails.vehicleid = vehiId;

                    if (callLogging.isProposeRenew == true)
                    {
                        var vehicleDetails = db.vehicles.FirstOrDefault(m => m.vehicle_id == vehiId);

                        callLogging.winbackPolicy = new WinbackPolicy();
                        callLogging.winbackPolicy.customerId = cusId;
                        callLogging.winbackPolicy.isProposeRenew = callLogging.isProposeRenew;

                        //callLogging.winbackPolicy.registrationnumber = policydetails.vehicleRegNo;
                        //var winbackpolicyDetails = db.Winbackpolicyrenewalresponses.FirstOrDefault(m => m.wibbackid==vehiId);
                        //callLogging.winbackPolicy.exshowroomprice = Convert.ToDouble(24344);
                        //callLogging.winbackPolicy.hero = policydetails.;
                        //callLogging.winbackPolicy.HeroQuoteID = policydetails.vehicle_class;
                        //callLogging.winbackPolicy.vehicleclass = policydetails.vehicle_class;
                        //callLogging.winbackPolicy.rto = Convert.ToString(vehicleDetails.fkRTO_Id);
                        //callLogging.winbackPolicy.dateoffirstsale = vehicleDetails.saleDate
                        //callLogging.winbackPolicy.policytenure =Convert.ToInt32(policydetails.Prev_Policy_Tenure);

                        //callLogging.winbackPolicy.policytenure ='1';
                        //callLogging.winbackPolicy.cpatenure =Convert.ToString(policydetails.tenure);

                        //if (!(string.IsNullOrEmpty(callLogging.winbackPolicy.cpatenure)))
                        //{
                        //    callLogging.winbackPolicy.cpatenure = Convert.ToString(policydetails.tenure);
                        //}

                        //else
                        //{
                        //    callLogging.winbackPolicy.cpatenure = "0";
                        //}

                        //callLogging.winbackPolicy.oem = policydetails.extendedWarentyDue;


                        // callLogging.winbackPolicy.policy_type =policydetails.Previous_Policy_Type;
                        // callLogging.winbackPolicy.oem = vehicleDetails.oemInvoiceNo;

                        //callLogging.winbackPolicy.modelname = Convert.ToString(vehicleDetails.model);

                        //callLogging.winbackPolicy.color =policydetails.color;

                        //callLogging.winbackPolicy.BasicIDV = Convert.ToString(policydetails.Basic_IDV_Tenure1);

                        //callLogging.winbackPolicy.emailid = policydetails.email_Id;
                        //callLogging.winbackPolicy.Anniversary_Date = policydetails.an;
                        //callLogging.winbackPolicy.insurancecompany = Convert.ToString(policydetails.insurcompid);
                        //callLogging.winbackPolicy.insurancecompany =policydetails.insuranceCompanyName;
                        //callLogging.winbackPolicy.Tpinsurancecompany =policydetails.insuranceCompanyName;
                        //callLogging.winbackPolicy.TPPreviousPolicyNo =Convert.ToInt32(policydetails.Previous_Policy_Number);


                        //winbackpolicyDetails.AssignDate = DateTime.Now;

                        //callLogging.winbackPolicy.Gstno =winbackpolicyDetails.Gst_No;

                        callLogging.winbackPolicy.registrationnumber = policydetails.vehicleRegNo == null ? "KA05AB6596" : policydetails.vehicleRegNo;
                        callLogging.winbackPolicy.product = callLogging.winbackPolicy.productId = callLogging.productId;

                        callLogging.winbackPolicy.vehicleclass = policydetails.vehicle_class == null ? "P" : "C";

                        callLogging.winbackPolicy.rto = vehicleDetails.fkRTO_Id == 0 ? "975" : Convert.ToString(vehicleDetails.fkRTO_Id);

                        callLogging.winbackPolicy.effectivedate = vehicleDetails.policyeffectivedate;

                        callLogging.winbackPolicy.dateoffirstsale = policydetails.saleDate;

                        callLogging.winbackPolicy.cubiccapacity = Convert.ToInt32(policydetails.Cubic_capacity);

                        callLogging.winbackPolicy.IsIMT43 = false;
                        callLogging.winbackPolicy.IsIMT44 = false;

                        callLogging.winbackPolicy.rticover = callLogging.policyadditionalcoverreturntoinvoice;
                        callLogging.winbackPolicy.ndcover = callLogging.policyadditionalcoverzerodep;

                        callLogging.winbackPolicy.oem = "2";
                        callLogging.winbackPolicy.policy_type = policydetails.Renewal_Type == null ? "PACKAGE" : policydetails.Renewal_Type;


                        callLogging.winbackPolicy.accessories = false;
                        callLogging.winbackPolicy.FitmentDate = Convert.ToDateTime("01/01/1900");
                        callLogging.winbackPolicy.ExternalKitPrice = "0";
                        callLogging.winbackPolicy.fueltype = vehicleDetails.fuelType;
                        callLogging.winbackPolicy.OOCover_Amt = 0;
                        callLogging.winbackPolicy.OPCover_Amt = 0;
                        callLogging.winbackPolicy.PDCover_Amt = 0;
                        callLogging.winbackPolicy.VoluntryDisc = 0;
                        callLogging.winbackPolicy.HandicapedDiscount = false;
                        callLogging.winbackPolicy.AddOn_Id = 0;
                        callLogging.winbackPolicy.GeographicalAreaCheckBox = false;

                        callLogging.winbackPolicy.whetheravailedclaims = Convert.ToString(policydetails.claimcount);
                        callLogging.winbackPolicy.ncbdiscount = Convert.ToString(policydetails.discount);

                        callLogging.winbackPolicy.IsAntiTheft = false;
                        callLogging.winbackPolicy.is_AA_membership = false;
                        callLogging.winbackPolicy.frameno = vehicleDetails.Frame_no;
                        callLogging.winbackPolicy.engineno = vehicleDetails.engineNo;

                        callLogging.winbackPolicy.modelname = Convert.ToString('2');
                        callLogging.winbackPolicy.insurancecompany = "2";

                        callLogging.winbackPolicy.color = policydetails.color == null ? "RED" : policydetails.color;

                        callLogging.winbackPolicy.yearofmanufacture = Convert.ToString(policydetails.yearofmanufacture);

                        callLogging.winbackPolicy.customerId = Convert.ToInt32(policydetails.customerId);
                        callLogging.winbackPolicy.inspectionnumber = 0;
                        callLogging.winbackPolicy.inspectiondate = Convert.ToDateTime("01/01/1900");
                        callLogging.winbackPolicy.CPAexclusionCode = "";
                        callLogging.winbackPolicy.CPAexclusionText = "";
                        callLogging.winbackPolicy.dateofregistration = Convert.ToDateTime(policydetails.Vehicle_Reg_date);
                        callLogging.winbackPolicy.CompanyName = policydetails.insuranceCompanyName;
                        callLogging.winbackPolicy.BusinessIndustry = "";
                        callLogging.winbackPolicy.AnnualTurnover = "";
                        callLogging.winbackPolicy.fax = "";
                        callLogging.winbackPolicy.fax2 = "";
                        callLogging.winbackPolicy.proposername = policydetails.salutation;
                        callLogging.winbackPolicy.ProposerFirstName = policydetails.first_name;
                        callLogging.winbackPolicy.ProposerMiddleName = policydetails.Middle_Name;
                        callLogging.winbackPolicy.ProposerLastName = policydetails.last_name;
                        callLogging.winbackPolicy.addressline1 = policydetails.Address_Line_1;
                        callLogging.winbackPolicy.addressline2 = policydetails.Address_Line_2;
                        callLogging.winbackPolicy.addressline3 = "";
                        callLogging.winbackPolicy.proposerstate = policydetails.fkState_id;
                        callLogging.winbackPolicy.proposerstatename = policydetails.state_name;
                        callLogging.winbackPolicy.proposercity = policydetails.fkcity_id;
                        callLogging.winbackPolicy.proposercityname = policydetails.city_name;
                        callLogging.winbackPolicy.pincode = Convert.ToInt32(policydetails.pincode);
                        callLogging.winbackPolicy.mobilenumber = Convert.ToInt64(policydetails.Mobile);
                        callLogging.winbackPolicy.SecondMobileNumber = Convert.ToInt64(policydetails.mobile2);
                        callLogging.winbackPolicy.age = policydetails.Nominee_Age;
                        callLogging.winbackPolicy.maritalstatus = policydetails.status;
                        callLogging.winbackPolicy.idproof = "";
                        callLogging.winbackPolicy.incomelevel = "5000-10000";
                        callLogging.winbackPolicy.educationlevel = "NA";
                        callLogging.winbackPolicy.occupationtype = "NA";
                        callLogging.winbackPolicy.employeename = "";
                        callLogging.winbackPolicy.parkingtype = policydetails.parkingtype;
                        callLogging.winbackPolicy.averagekmdriven = Convert.ToInt32(policydetails.avgkml);
                        callLogging.winbackPolicy.excutivename = "";
                        callLogging.winbackPolicy.nomineename = policydetails.Nominee_Name;
                        callLogging.winbackPolicy.nomineegender = "";
                        callLogging.winbackPolicy.relation = policydetails.Nominee_Relationship;
                        callLogging.winbackPolicy.nomineeage = policydetails.Nominee_Age;
                        callLogging.winbackPolicy.previouscpaenddate = Convert.ToDateTime(policydetails.Previous_Policy_Expiry);
                        callLogging.winbackPolicy.policyenddate = policydetails.policyexpirydate;
                        callLogging.winbackPolicy.effectivedate = policydetails.policyeffectivedate;
                        callLogging.winbackPolicy.Branch_Office_Location_Code = policydetails.branchName;
                        callLogging.winbackPolicy.odenddate = policydetails.TP_End_Date;
                        callLogging.winbackPolicy.NOIPaidDrvrnCleaners = "";
                        callLogging.winbackPolicy.SOIPaidDrvrnCleaners = "";
                        callLogging.winbackPolicy.NOIEmpOTPaidDrvrnCleaners = "";
                        callLogging.winbackPolicy.SOIEmpOTPaidDrvrnCleaners = "";
                        callLogging.winbackPolicy.Suminsuredperperson = "";
                        callLogging.winbackPolicy.Membership_No = 0;
                        callLogging.winbackPolicy.entitledNCBdiscount = "1";
                        callLogging.winbackPolicy.BasicIDV = policydetails.Basic_IDV_Tenure1 == 0 ? "63623" : Convert.ToString(policydetails.Basic_IDV_Tenure1);

                        callLogging.winbackPolicy.Proposertype = callLogging.winbackPolicy.Proposertype == null ? "I" : "C";

                        callLogging.winbackPolicy.emailid = policydetails.email == null ? "abc@gmail.com" : policydetails.email;


                        if (policydetails.Renewal_Type == "SAOD")
                        {
                            callLogging.winbackPolicy.cpatenure = "0";
                            callLogging.winbackPolicy.CPAreason = null; //making these changes as per previous developers logic
                            callLogging.winbackPolicy.exshowroomprice = Convert.ToDouble(policydetails.Ex_SAOD_Basic_IDV_Tenure1);

                        }
                        else
                        {
                            callLogging.winbackPolicy.cpatenure = callLogging.packageoptingoutcpayesno == "Y" ? "1" : "0";
                            callLogging.winbackPolicy.CPAreason = String.Equals(callLogging.packageoptingoutcpayesno, "N", comparisonType: StringComparison.CurrentCultureIgnoreCase) ? null : "";
                            callLogging.winbackPolicy.exshowroomprice = Convert.ToDouble(policydetails.Ex_Basic_IDV_Tenure1);
                        }

                        if (String.Equals(policydetails.packagepremium, "Package1", StringComparison.InvariantCultureIgnoreCase))
                        {
                            callLogging.winbackPolicy.policytenure = 1;
                        }
                        else if (String.Equals(policydetails.packagepremium, "Package2", StringComparison.InvariantCultureIgnoreCase))
                        {
                            callLogging.winbackPolicy.policytenure = 2;
                        }
                        else if (String.Equals(policydetails.packagepremium, "Package3", StringComparison.InvariantCultureIgnoreCase))
                        {
                            callLogging.winbackPolicy.policytenure = 3;
                        }



                        //Winbackpolicy(callLogging);

                    }

                    else
                    {




                        if (policydetails.Renewal_Type.ToUpper() == "PACKAGE".ToUpper())
                        {
                            if (callLogging.isexpired)
                            {
                                if (policydetails.packagepremium.ToUpper() == "Package1".ToUpper())
                                {
                                    renewaldetails.Basic_OD_Tenure = policydetails.Ex_Basic_OD_WithOutNCB_Tenure1;
                                    renewaldetails.Basic_TP_Tenure = policydetails.Ex_Basic_TP_Tenure1;
                                    renewaldetails.Geo_OD_Ext = policydetails.Ex_Geo_OD_Ext1;
                                    renewaldetails.Geo_TP_Ext = policydetails.Ex_Geo_TP_Ext1;
                                    renewaldetails.CompulsoryPremium_Amt = policydetails.Ex_CompulsoryPremium_Amt1;
                                    renewaldetails.Zero_Dep_Tenure = policydetails.Ex_Zero_Dep_Tenure1;
                                    renewaldetails.RTI_Tenure = policydetails.Ex_RTI_Tenure1;
                                    renewaldetails.Basic_IDV_Tenure = policydetails.Ex_Basic_IDV_Tenure1;
                                    renewaldetails.NCB_Per_Tenure = policydetails.Ex_NCB_Per_Tenure1;
                                    renewaldetails.NCB_Value_Tenure = policydetails.Ex_NCB_Value_Tenure1;
                                    renewaldetails.policytenure = 1;
                                }
                                else if (policydetails.packagepremium.ToUpper() == "Package2".ToUpper())
                                {
                                    renewaldetails.Basic_OD_Tenure = policydetails.Ex_Basic_OD_WithOutNCB_Tenure2;
                                    renewaldetails.Basic_TP_Tenure = policydetails.Ex_Basic_TP_Tenure2;
                                    renewaldetails.Geo_OD_Ext = policydetails.Ex_Geo_OD_Ext2;
                                    renewaldetails.Geo_TP_Ext = policydetails.Ex_Geo_TP_Ext2;
                                    renewaldetails.CompulsoryPremium_Amt = policydetails.Ex_CompulsoryPremium_Amt2;
                                    renewaldetails.Zero_Dep_Tenure = policydetails.Ex_Zero_Dep_Tenure2;
                                    renewaldetails.RTI_Tenure = policydetails.Ex_RTI_Tenure2;
                                    renewaldetails.Basic_IDV_Tenure = policydetails.Ex_Basic_IDV_Tenure2;
                                    renewaldetails.NCB_Per_Tenure = policydetails.Ex_NCB_Per_Tenure2;
                                    renewaldetails.NCB_Value_Tenure = policydetails.Ex_NCB_Value_Tenure2;
                                    renewaldetails.policytenure = 2;

                                }
                                else if (policydetails.packagepremium.ToUpper() == "Package3".ToUpper())
                                {
                                    renewaldetails.Basic_OD_Tenure = policydetails.Ex_Basic_OD_WithOutNCB_Tenure3;
                                    renewaldetails.Basic_TP_Tenure = policydetails.Ex_Basic_TP_Tenure3;
                                    renewaldetails.Geo_OD_Ext = policydetails.Ex_Geo_OD_Ext3;
                                    renewaldetails.Geo_TP_Ext = policydetails.Ex_Geo_TP_Ext3;
                                    renewaldetails.CompulsoryPremium_Amt = policydetails.Ex_CompulsoryPremium_Amt3;
                                    renewaldetails.Zero_Dep_Tenure = policydetails.Ex_Zero_Dep_Tenure3;
                                    renewaldetails.RTI_Tenure = policydetails.Ex_RTI_Tenure3;
                                    renewaldetails.Basic_IDV_Tenure = policydetails.Ex_Basic_IDV_Tenure3;
                                    renewaldetails.NCB_Per_Tenure = policydetails.Ex_NCB_Per_Tenure3;
                                    renewaldetails.NCB_Value_Tenure = policydetails.Ex_NCB_Value_Tenure3;
                                    renewaldetails.policytenure = 3;

                                }
                            }
                            else
                            {
                                if (policydetails.packagepremium.ToUpper() == "Package1".ToUpper())
                                {
                                    renewaldetails.Basic_OD_Tenure = policydetails.Basic_OD_WithOutNCB_Tenure1;
                                    renewaldetails.Basic_TP_Tenure = policydetails.Basic_TP_Tenure1;
                                    renewaldetails.Geo_OD_Ext = policydetails.Geo_OD_Ext1;
                                    renewaldetails.Geo_TP_Ext = policydetails.Geo_TP_Ext1;
                                    renewaldetails.CompulsoryPremium_Amt = policydetails.CompulsoryPremium_Amt1;
                                    renewaldetails.Zero_Dep_Tenure = policydetails.Zero_Dep_Tenure1;
                                    renewaldetails.RTI_Tenure = policydetails.RTI_Tenure1;
                                    renewaldetails.Basic_IDV_Tenure = policydetails.Basic_IDV_Tenure1;
                                    renewaldetails.NCB_Per_Tenure = policydetails.NCB_Per_Tenure1;
                                    renewaldetails.NCB_Value_Tenure = policydetails.NCB_Value_Tenure1;
                                    renewaldetails.policytenure = 1;
                                }
                                else if (policydetails.packagepremium.ToUpper() == "Package2".ToUpper())
                                {
                                    renewaldetails.Basic_OD_Tenure = policydetails.Basic_OD_WithOutNCB_Tenure2;
                                    renewaldetails.Basic_TP_Tenure = policydetails.Basic_TP_Tenure2;
                                    renewaldetails.Geo_OD_Ext = policydetails.Geo_OD_Ext2;
                                    renewaldetails.Geo_TP_Ext = policydetails.Geo_TP_Ext2;
                                    renewaldetails.CompulsoryPremium_Amt = policydetails.CompulsoryPremium_Amt2;
                                    renewaldetails.Zero_Dep_Tenure = policydetails.Zero_Dep_Tenure2;
                                    renewaldetails.RTI_Tenure = policydetails.RTI_Tenure2;
                                    renewaldetails.Basic_IDV_Tenure = policydetails.Basic_IDV_Tenure2;
                                    renewaldetails.NCB_Per_Tenure = policydetails.NCB_Per_Tenure2;
                                    renewaldetails.NCB_Value_Tenure = policydetails.NCB_Value_Tenure2;
                                    renewaldetails.policytenure = 2;

                                }
                                else if (policydetails.packagepremium.ToUpper() == "Package3".ToUpper())
                                {
                                    renewaldetails.Basic_OD_Tenure = policydetails.Basic_OD_WithOutNCB_Tenure3;
                                    renewaldetails.Basic_TP_Tenure = policydetails.Basic_TP_Tenure3;
                                    renewaldetails.Geo_OD_Ext = policydetails.Geo_OD_Ext3;
                                    renewaldetails.Geo_TP_Ext = policydetails.Geo_TP_Ext3;
                                    renewaldetails.CompulsoryPremium_Amt = policydetails.CompulsoryPremium_Amt3;
                                    renewaldetails.Zero_Dep_Tenure = policydetails.Zero_Dep_Tenure3;
                                    renewaldetails.RTI_Tenure = policydetails.RTI_Tenure3;
                                    renewaldetails.Basic_IDV_Tenure = policydetails.Basic_IDV_Tenure3;
                                    renewaldetails.NCB_Per_Tenure = policydetails.NCB_Per_Tenure3;
                                    renewaldetails.NCB_Value_Tenure = policydetails.NCB_Value_Tenure3;
                                    renewaldetails.policytenure = 3;
                                }
                            }

                            renewaldetails.cpaopted = callLogging.packageoptingoutcpayesno;
                            renewaldetails.GST_Per = 18;
                            //renewaldetails.GST_Per = policydetails.GST_Per;
                            renewaldetails.grosspremium = callLogging.PACKAGEGrossPremiumTenure;
                            renewaldetails.totalpremium = callLogging.PACKAGETotalPremiumTenure;
                        }
                        else if (policydetails.Renewal_Type.ToUpper() == "SAOD".ToUpper())
                        {
                            if (callLogging.isexpired)
                            {
                                renewaldetails.Basic_OD_Tenure = policydetails.Ex_SAOD_Basic_OD_Tenure1;
                                renewaldetails.Basic_TP_Tenure = policydetails.Ex_SAOD_Basic_TP_Tenure1;
                                renewaldetails.Zero_Dep_Tenure = policydetails.Ex_SAOD_Zero_Dep_Tenure1;
                                renewaldetails.RTI_Tenure = policydetails.Ex_SAOD_RTI_Tenure1;
                                renewaldetails.Basic_IDV_Tenure = policydetails.Ex_SAOD_Basic_IDV_Tenure1;
                                renewaldetails.NCB_Value_Tenure = policydetails.Ex_SAOD_NCB_Value_Tenure1;
                            }
                            else
                            {
                                renewaldetails.Basic_OD_Tenure = policydetails.SAOD_Basic_OD_Tenure1;
                                renewaldetails.Basic_TP_Tenure = policydetails.SAOD_Basic_TP_Tenure1;
                                renewaldetails.Zero_Dep_Tenure = policydetails.SAOD_Zero_Dep_Tenure1;
                                renewaldetails.RTI_Tenure = policydetails.SAOD_RTI_Tenure1;
                                renewaldetails.Basic_IDV_Tenure = policydetails.SAOD_Basic_IDV_Tenure1;
                                renewaldetails.NCB_Value_Tenure = policydetails.SAOD_NCB_Value_Tenure1;

                            }
                            renewaldetails.cpaopted = "Y";
                            renewaldetails.policytenure = 1;
                            renewaldetails.grosspremium = callLogging.SAODGrossPremium;
                            renewaldetails.totalpremium = callLogging.SAODTotalPremium;
                            renewaldetails.GST_Per = 18;

                        }

                        renewaldetails.additionalcoverzerodep = callLogging.policyadditionalcoverzerodep ? "Y" : "N";
                        renewaldetails.additionalcoverRTI = callLogging.policyadditionalcoverreturntoinvoice ? "Y" : "N";

                        renewaldetails.riskstartdate = policydetails.Previous_Policy_Start;
                        renewaldetails.reasonoptingoutcpa = callLogging.packageoptingoutcpareason;
                        renewaldetails.existingpolicyclaim = policydetails.existingpolicyclaim;
                        renewaldetails.vehicleregno = policydetails.vehicleRegNo;
                        renewaldetails.vehicleregdate = policydetails.Vehicle_Reg_date;
                        renewaldetails.modelname = policydetails.model;
                        renewaldetails.modelvarinat = policydetails.variant;
                        renewaldetails.email = policydetails.email;
                        renewaldetails.mobile = policydetails.Mobile;
                        renewaldetails.salutation = policydetails.salutation;
                        renewaldetails.customername = policydetails.first_name;
                        renewaldetails.middlename = policydetails.Middle_Name;
                        renewaldetails.lastname = policydetails.last_name;
                        renewaldetails.Address = policydetails.Address_Line_1;
                        renewaldetails.Address_Line_2 = policydetails.Address_Line_2;
                        renewaldetails.cityname = policydetails.city_name;
                        renewaldetails.pincode = policydetails.pincode;
                        renewaldetails.nomineename = policydetails.Nominee_Name;
                        renewaldetails.nomineeage = policydetails.Nominee_Age;
                        renewaldetails.nomineerelationship = policydetails.Nominee_Relationship;
                        if (db.Nomineerelations.Count(m => m.relationship.ToLower().Trim() == policydetails.Nominee_Relationship.ToLower().Trim()) > 0)
                        {
                            renewaldetails.nomineerelationshipid = db.Nomineerelations.FirstOrDefault(m => m.relationship.ToLower().Trim() == policydetails.Nominee_Relationship.ToLower().Trim()).id;
                        }
                        renewaldetails.insurancecompany = policydetails.Previous_Insurer_Name;
                        renewaldetails.policynumber = policydetails.Previous_Policy_Number;
                        renewaldetails.policyexpirydate = policydetails.Previous_Policy_Expiry;
                        renewaldetails.renewwaltype = policydetails.Renewal_Type;
                        renewaldetails.renewwaltype = policydetails.Renewal_Type;
                        renewaldetails.packagetype = policydetails.packagepremium;
                        renewaldetails.disposeddate = DateTime.Now.ToString("yyyy-MM-dd");
                        renewaldetails.disposedtime = DateTime.Now.ToString("hh:mm tt");
                        renewaldetails.reasonoptingoutcpa = callLogging.packageoptingoutcpareason;
                        renewaldetails.creName = Session["UserName"].ToString();
                        renewaldetails.creID = Convert.ToInt32(Session["UserId"]);
                        renewaldetails.totalgstamt = callLogging.GST_AMT;
                        renewaldetails.campaignName = callLogging.InsAssignCampaign;
                        renewaldetails.cityid = policydetails.fkcity_id;
                        renewaldetails.stateid = policydetails.fkState_id; ;
                        renewaldetails.ispolicygenerated = false;
                        renewaldetails.insuranceassignedinteractionId = callLogging.insuranceassignedinteractionId;
                        renewaldetails.herodealerId = callLogging.herodealerId;
                        db.Insurancerenewalpolicies.Add(renewaldetails);
                        db.SaveChanges();
                        //autoHeroCreatePolicy(renewaldetails);
                    }
                    return Json(new { success = true, error = "" });

                }
            }

            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        submissionResult = ex.InnerException.InnerException.Message;
                    }
                    else
                    {
                        submissionResult = ex.InnerException.Message;
                    }
                }
                else
                {
                    submissionResult = ex.Message;
                }

                return Json(new { success = false, error = submissionResult });

            }
        }

        #region SMS Send
        public ActionResult sendWhatsappMsg(string phNum, string msg, string reason, string responseFrom)
        {
            int smsId = 0;
            int locId = 0;

            if (responseFrom == "message")
            {
                smsId = Convert.ToInt32(Session["smsId"].ToString());
                locId = Convert.ToInt32(Session["locId"].ToString());
            }

            int custId = Convert.ToInt32(Session["CusId"].ToString());
            int vehiId = Convert.ToInt32(Session["VehiId"].ToString());
            int UserId = Convert.ToInt32(Session["UserId"].ToString());

            try
            {
                using (var db = new AutoSherDBContext())
                {

                    smsinteraction smsinteraction = new smsinteraction();

                    smsinteraction.interactionDate = DateTime.Now.ToString("dd-MM-yyyy");
                    smsinteraction.interactionDateAndTime = DateTime.Now;
                    smsinteraction.interactionTime = DateTime.Now.ToString("HH:mm:ss");
                    smsinteraction.interactionType = "WhatsApp Msg";
                    smsinteraction.responseFromGateway = "";
                    smsinteraction.customer_id = custId;
                    smsinteraction.vehicle_vehicle_id = vehiId;
                    smsinteraction.wyzUser_id = UserId;
                    smsinteraction.mobileNumber = phNum.Substring(2, 10);
                    if (responseFrom == "message")
                    {
                        smsinteraction.smsType = smsId.ToString();
                    }
                    smsinteraction.smsMessage = msg;
                    smsinteraction.isAutoSMS = false;

                    if (responseFrom == "message")
                    {
                        smsinteraction.smsType = smsId.ToString();
                    }

                    if (reason == "Send Successfully")
                    {
                        smsinteraction.smsStatus = true;
                    }
                    else
                    {
                        smsinteraction.smsStatus = false;
                    }
                    smsinteraction.reason = reason;
                    db.smsinteractions.Add(smsinteraction);
                    db.SaveChanges();
                }

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult getTemplateMessage(int smsId, int locId, string msgType)
        {

            if (msgType != "email")
            {
                Session["smsId"] = smsId;
                Session["locId"] = locId;
            }
            int UserId = Convert.ToInt32(Session["UserId"].ToString());
            int vehiId = Convert.ToInt32(Session["VehiId"].ToString());
            try
            {
                //smstemplate template = new smstemplate();
                using (var db = new AutoSherDBContext())
                {

                    if (msgType == "sms")
                    {
                        string str = @"CALL sendsms(@inwyzuser_id,@invehicle_id,@inlocid,@insmsid,@ininsid);";

                        MySqlParameter[] sqlParameter = new MySqlParameter[]
                        {
                       new MySqlParameter("@inwyzuser_id",UserId.ToString()),
                       new MySqlParameter("@invehicle_id",vehiId.ToString()),
                       new MySqlParameter("@inlocid",locId.ToString()),
                       new MySqlParameter("@insmsid",smsId.ToString()),
                       new MySqlParameter("@ininsid","0"),
                        };

                        var incomingTemplate = db.Database.SqlQuery<string>(str, sqlParameter).FirstOrDefault();

                        if (incomingTemplate != null)
                        {
                            string template = incomingTemplate.ToString();
                            return Json(new { success = true, sms = template });
                        }
                        else
                        {
                            return Json(new { success = false, error = "SMS Template Text Is Empty!." });
                        }
                    }


                }
                return Json(new { success = false, error = "SMS Template Text Is Empty!." });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        public ActionResult getQutationTemplate(int smsId, int locId, string msgType, int quationID)
        {

            if (msgType != "email")
            {
                Session["smsId"] = smsId;
                Session["locId"] = locId;
            }
            int UserId = Convert.ToInt32(Session["UserId"].ToString());
            int vehiId = Convert.ToInt32(Session["VehiId"].ToString());
            try
            {
                //smstemplate template = new smstemplate();
                using (var db = new AutoSherDBContext())
                {

                    if (msgType == "sms")
                    {
                        string str = @"CALL sendsms_quotation(@inwyzuser_id,@invehicle_id,@inlocid,@insmsid,@ininsqtid);";

                        MySqlParameter[] sqlParameter = new MySqlParameter[]
                        {
                       new MySqlParameter("@inwyzuser_id",UserId.ToString()),
                       new MySqlParameter("@invehicle_id",vehiId.ToString()),
                       new MySqlParameter("@inlocid",locId.ToString()),
                       new MySqlParameter("@insmsid",smsId.ToString()),
                       new MySqlParameter("@ininsqtid",quationID),
                        };

                        var incomingTemplate = db.Database.SqlQuery<string>(str, sqlParameter).FirstOrDefault();

                        if (incomingTemplate != null)
                        {
                            string template = incomingTemplate.ToString();
                            return Json(new { success = true, sms = template });
                        }
                        else
                        {
                            return Json(new { success = false, error = "SMS Template Text Is Empty!." });
                        }
                    }


                }
                return Json(new { success = false, error = "SMS Template Text Is Empty!." });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }


        #endregion


        #region Upload Documents
        public ActionResult uploadDocument(CallLoggingViewModel callLogging)
        {
            //if(document!=null || document!="")
            //{
            documentuploadhistory doc = new documentuploadhistory();
            doc = callLogging.docHistory;
            List<string> fileError = new List<string>();
            string fileString = string.Empty;
            string uploadedFileName = string.Empty;
            string finalFileLink = string.Empty;
            string fileUploadPath = Server.MapPath("~/UploadedFiles/" + Session["DealerCode"].ToString() + "/" + Session["UserName"].ToString() + "/");
            string fileURL = string.Empty;
            string fileLink = string.Empty;
            int custId = Convert.ToInt32(Session["CusId"].ToString());
            int vehiId = Convert.ToInt32(Session["VehiId"].ToString());
            try
            {
                using (var db = new AutoSherDBContext())
                {

                    db.Configuration.LazyLoadingEnabled = false;

                    if (!Directory.Exists(fileUploadPath))
                    {
                        Directory.CreateDirectory(fileUploadPath);
                    }



                    foreach (var file in doc.fileList)
                    {
                        string extension = string.Empty;
                        extension = Path.GetExtension(file.FileName);
                        if (extension == ".docx" || extension == ".png" || extension == ".PNG" || extension == ".xlsx" || extension == ".xls" || extension == ".jpg" || extension == ".jpeg" || extension == ".pdf")
                        {
                            //if(extension==".pdf")
                            //{
                            //    long fileSize = file.ContentLength * 1024;

                            //    if(fileSize<=1024)
                            //    {s

                            //    }
                            //}
                            string savingFileName = file.FileName.Split('.')[0] + "_" + custId + "_" + DateTime.Now.ToString("dd-MM-yyyy").Replace("-", "") + "_" + DateTime.Now.ToString("HH:mm:ss").Replace(":", "") + extension;
                            //file.FileName = savingFileName;
                            //file.SaveAs(fileUploadPath + Path.GetFileName(file.FileName));
                            file.SaveAs(fileUploadPath + savingFileName);
                            fileString = fileString + fileUploadPath + savingFileName + ";";
                            uploadedFileName = uploadedFileName + file.FileName + ";";
                            finalFileLink = finalFileLink + savingFileName + ";";
                        }
                        else
                        {
                            fileError.Add("cann't upload File " + file.FileName);
                        }
                    }
                    int UserId = Convert.ToInt32(Session["UserId"].ToString());

                    uploadedFileName = uploadedFileName.Remove((uploadedFileName.Length - 1));
                    finalFileLink = finalFileLink.Remove((finalFileLink.Length - 1));
                    fileString = fileString.Remove((fileString.Length - 1));

                    doc.customerId = custId;
                    doc.filePath = fileString;
                    doc.uploadFileName = uploadedFileName;
                    doc.uploadDateTime = DateTime.Now;
                    doc.user = Session["UserName"].ToString();
                    doc.userId = UserId;

                    db.documentuploadhistories.Add(doc);
                    db.SaveChanges();
                    //fileUploadPath = fileUploadPath.Remove(0);
                    fileURL = "/UploadedFiles/" + Session["DealerCode"].ToString() + "/" + Session["UserName"].ToString() + "/";
                    fileLink = finalFileLink + "&" + fileURL;
                }
                var UserName = Session["UserName"].ToString();
                //return JavaScript("fileSuccess()");
                return Json(new { success = true, files = fileLink, UserName, docId = doc.id });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
            //}


            return Json(new { success = false });
        }
        #endregion

        #region Getting Already Stored Files 
        public ActionResult getDocuments()
        {
            //string fileURL = "/UploadedFiles/" + Session["DealerCode"].ToString() + "/" + Session["UserName"].ToString() + "/";
            string fileLink = string.Empty;
            long cusId = Convert.ToInt32(Session["CusId"].ToString());
            long userId = Convert.ToInt32(Session["UserId"].ToString());
            string usernames = string.Empty, docFileName = string.Empty;
            string mainFileStream = string.Empty;
            string fileNames = string.Empty;
            string deptName = string.Empty;
            string uploadedDateTime = string.Empty;
            string indiFileName = string.Empty;
            string docId = string.Empty;
            List<documentuploadhistory> doc = new List<documentuploadhistory>();
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    if (db.documentuploadhistories.Any(m => m.customerId == cusId))
                    {
                        doc = db.documentuploadhistories.Where(m => m.customerId == cusId).OrderByDescending(m => m.uploadDateTime).ToList();
                    }

                    for (int i = 0; i < doc.Count(); i++)
                    {
                        string fileURL = "/UploadedFiles/" + Session["DealerCode"].ToString() + "/" + doc[i].user + "/";

                        string uploadedFileName = string.Empty;
                        List<string> filePath = new List<string>();
                        filePath = doc[i].filePath.Split(';').ToList();
                        for (int k = 0; k < filePath.Count(); k++)
                        {
                            if (k == 0)
                            {
                                uploadedFileName = (filePath[k].Substring(filePath[k].IndexOf(doc[i].user))).Split('\\')[1];
                            }
                            else
                            {
                                uploadedFileName = uploadedFileName + ";" + (filePath[k].Substring(filePath[k].IndexOf(doc[i].user))).Split('\\')[1];
                            }
                        }

                        fileLink = uploadedFileName + "#" + fileURL;
                        if (i == 0)
                        {
                            docId = doc[i].id.ToString();
                            usernames = doc[i].user;
                            mainFileStream = fileLink;
                            docFileName = doc[i].uploadFileName;
                            fileNames = doc[i].documentName;
                            deptName = doc[i].deptName;
                            uploadedDateTime = Convert.ToDateTime(doc[i].uploadDateTime).ToString("dd-MM-yyyy") + " " + Convert.ToDateTime(doc[i].uploadDateTime).TimeOfDay;
                        }
                        else
                        {
                            docId = docId + "," + doc[i].id.ToString();
                            docFileName = docFileName + "%" + doc[i].uploadFileName;
                            mainFileStream = mainFileStream + "%" + fileLink;
                            usernames = usernames + "," + doc[i].user;
                            fileNames = fileNames + "," + doc[i].documentName;
                            deptName = deptName + "," + doc[i].deptName;
                            uploadedDateTime = uploadedDateTime + "," + Convert.ToDateTime(doc[i].uploadDateTime).ToString("dd-MM-yyyy") + " " + Convert.ToDateTime(doc[i].uploadDateTime).TimeOfDay;
                        }
                        fileLink = string.Empty;
                    }
                    return Json(new { success = true, files = mainFileStream, docId = docId, docFileName = docFileName, fileName = fileNames, usernames = usernames, deptName = deptName, uploadedDateTime = uploadedDateTime });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
        #endregion
        public ActionResult getUniqueID()
        {
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    long last_id = db.uniqueidforcalls.Max(m => m.id);
                    int last_num = db.uniqueidforcalls.FirstOrDefault(m => m.id == last_id).callinitiating_id;
                    last_num = last_num + 1;

                    uniqueidforcall uniqueidforcall = new uniqueidforcall();
                    uniqueidforcall.callinitiating_id = last_num;
                    db.uniqueidforcalls.Add(uniqueidforcall);
                    db.SaveChanges();

                    return Json(new { success = true, unqId = uniqueidforcall.id });
                }
            }
            catch (Exception ex)
            {

            }
            return Json(new { success = false });
        }


        #region Android Click to call
        //public ActionResult initializeCall(string phNum, long uniqueid)
        //{
        //    Logger logger = LogManager.GetLogger("apkRegLogger");

        //    long cusId, vehiId, UniqId;
        //    Dictionary<string, string> dict = (Dictionary<string, string>)Session["CurLogDetails"];
        //    cusId = Convert.ToInt32(Session["CusId"].ToString());
        //    vehiId = Convert.ToInt32(Session["VehiId"].ToString());
        //    string vehiRegNum = "", custName = "unkown";
        //    int WyzUserId = Convert.ToInt32(Session["UserId"].ToString());
        //    try
        //    {
        //        string androidKey = "";
        //        if (HomeController.autosherpa1.Contains(Session["DealerCode"].ToString()))
        //        {
        //            androidKey = System.Configuration.ConfigurationManager.AppSettings["androidKeyautosherpa1"];
        //        }
        //        logger.Info("\n\n -------- Clik to call Initiated Phone Number {0} Unique Id {1}  : ",phNum,uniqueid);

        //        using (var db = new AutoSherDBContext())
        //        {
        //            customer cust = db.customers.FirstOrDefault(m => m.id == cusId);
        //            vehicle vehicle = db.vehicles.FirstOrDefault(m => m.vehicle_id == vehiId);
        //            UniqId = db.uniqueidforcalls.FirstOrDefault(m => m.id == uniqueid).callinitiating_id;
        //            Session["DialedNumber"] = phNum;
        //            if (cust.customerName != null)
        //            {
        //                custName = cust.customerName;
        //            }


        //            if (vehicle.vehicleRegNo != null)
        //            {
        //                vehiRegNum = vehicle.vehicleRegNo;
        //            }
        //            else if (vehicle.chassisNo != null)
        //            {
        //                vehiRegNum = vehicle.chassisNo;
        //            }

        //            string baseURL = "https://fcm.googleapis.com/fcm/send";
        //            string toKey = db.wyzusers.FirstOrDefault(m => m.id == WyzUserId).registrationId;
        //            if (toKey == null || toKey == "")
        //            {
        //                return Json(new { success = false, error = "Unable to connect, no reg.key found" });
        //            }

        //            dynamic data1 = new JObject();
        //            data1.to = toKey;
        //            dynamic data2 = new JObject();
        //            data2.command = "Intiate";
        //            data2.phonenumber = phNum;
        //            data2.id = UniqId;
        //            data2.makeCallFrom = "service";
        //            data2.type = "Web";
        //            data2.customername = custName;
        //            data2.priority = "high";
        //            data2.vehicleRegNo = vehiRegNum;
        //            data1.data = data2;


        //            //var body = JsonConvert.SerializeObject(data1);

        //            WebRequest request = WebRequest.Create(baseURL);
        //            var httprequest = (HttpWebRequest)request;

        //            httprequest.PreAuthenticate = true;
        //            httprequest.Method = "POST";
        //            httprequest.ContentType = "application/json";
        //            httprequest.Headers["Authorization"] = "key=" + androidKey;
        //            httprequest.Accept = "application/json";

        //            using (var streamWriter = new StreamWriter(httprequest.GetRequestStream()))
        //            {
        //                var bodyContent = JsonConvert.SerializeObject(data1);
        //                streamWriter.Write(bodyContent);
        //                logger.Info("\n\n -------- click to call data -------------|: " +bodyContent);

        //                streamWriter.Flush();
        //                streamWriter.Close();
        //            }

        //            HttpWebResponse response = null;
        //            response = (HttpWebResponse)httprequest.GetResponse();

        //            string response_string = string.Empty;
        //            using (Stream strem = response.GetResponseStream())
        //            {
        //                StreamReader sr = new StreamReader(strem);
        //                response_string = sr.ReadToEnd();
        //                sr.Close();
        //            }
        //            logger.Info("\n\n -------- click to call response -------------|: " + response_string);

        //            dynamic api_result = JObject.Parse(response_string);
        //            int success = api_result.success;
        //            long multiCast = api_result.multicast_id;
        //            if (success == 0)
        //            {
        //                string error = api_result.results[0].error;
        //                Session["NCReason"] = false;
        //                return Json(new { success = false, error = error });
        //            }
        //            else if (success == 1)
        //            {
        //                Session["NCReason"] = true;
        //                Session["isCallInitiated"] = "initiated";
        //                Session["AndroidUniqueId"] = UniqId;
        //                return Json(new { success = true });
        //            }
        //            //string status=response_string.
        //            Session["DialedNumber"] = phNum;

        //        }
        //        logger.Info("\n\n -------- click to call Ended -------------" );

        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Info("\n\n -------- click to call Exception -------------|: " + ex.Message);
        //        return Json(new { success = false, error = ex.Message });

        //    }
        //    return Json(new { success = false });
        //}


        /*public async Task<ActionResult> initializeCall(string phNum, long uniqueid)
        {
            long cusId, vehiId, UniqId;
            string vehiRegNum = "", custName = "unknown";
            int WyzUserId = Convert.ToInt32(Session["UserId"].ToString());
            cusId = Convert.ToInt32(Session["CusId"].ToString());
            vehiId = Convert.ToInt32(Session["VehiId"].ToString());
            string _fcmUrl = "https://fcm.googleapis.com/v1/projects/autosherpa3/messages:send";

            try
            {


                using (var db = new AutoSherDBContext())
                {
                    var cust = db.customers.FirstOrDefault(m => m.id == cusId);
                    var vehicle = db.vehicles.FirstOrDefault(m => m.vehicle_id == vehiId);
                    UniqId = db.uniqueidforcalls.FirstOrDefault(m => m.id == uniqueid).callinitiating_id;
                    string toKey = db.wyzusers.FirstOrDefault(m => m.id == WyzUserId).registrationId;
                    firebasefilepath = db.dealers.Select(m => m.andriodappfilepath).FirstOrDefault();
                    string accessToken = await GetAccessToken(firebasefilepath);
                    if (cust?.customerName != null)
                    {
                        custName = cust.customerName;
                    }

                    if (vehicle?.vehicleRegNo != null)
                    {
                        vehiRegNum = vehicle.vehicleRegNo;
                    }
                    else if (vehicle?.chassisNo != null)
                    {
                        vehiRegNum = vehicle.chassisNo;
                    }

                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                        // client.DefaultRequestHeaders.Add("Content-Type", "application/json");

                        var message = new
                        {
                            message = new
                            {
                                token = toKey,
                                data = new
                                {
                                    phonenumber = phNum,
                                    makeCallFrom = "service",
                                    type = "Web",
                                    customername = custName,
                                    priority = "high",
                                    vehicleRegNo = vehiRegNum
                                }
                            }
                        };

                        var jsonMessage = JsonConvert.SerializeObject(message);
                        var content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");

                        //var response = await client.PostAsync(_fcmUrl, content);
                        //var response_string = await response.Content.ReadAsStringAsync(); // Get the response as a string
                        //response.EnsureSuccessStatusCode();

                        //// Parse the API response
                        //dynamic api_result = JObject.Parse(response_string);
                        //int success = api_result.success;
                        //long multiCast = api_result.multicast_id;

                        //if (success == 0)
                        //{
                        //    string error = api_result.results[0].error;
                        //    Session["NCReason"] = false;
                        //    return Json(new { success = false, error = error });
                        //}
                        //else if (success == 1)
                        //{
                        //    Session["NCReason"] = true;
                        //    Session["isCallInitiated"] = "initiated";
                        //    Session["AndroidUniqueId"] = UniqId;
                        //    Session["DialedNumber"] = phNum;
                        //    return Json(new { success = true });
                        //}


                        HttpResponseMessage response = await client.PostAsync(_fcmUrl, content);
                        var responseString = await response.Content.ReadAsStringAsync(); // Get the response as a string


                        if (response.IsSuccessStatusCode)
                        {
                            // Parse the API response
                            dynamic apiResult = JObject.Parse(responseString);

                            // Check if "name" exists in the response (message ID)
                            if (apiResult.name != null)
                            {
                                // Success case, FCM message was sent
                                Session["NCReason"] = true;
                                Session["isCallInitiated"] = "initiated";
                                Session["AndroidUniqueId"] = UniqId;
                                Session["DialedNumber"] = phNum;
                                return Json(new { success = true, messageId = apiResult.name.ToString() });
                            }
                            else
                            {
                                // Handle case where name field is missing
                                return Json(new { success = false, error = "Message ID is missing in the response." });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }

            return Json(new { success = false });
        }*/

        #region
        //   new ajay flutter code
        public async Task<ActionResult> initializeCall(string phnum, long uniqueid)
        {
            long cusid, vehiid, uniqid;
            string vehicleRegNum = "", custName = "Unknown";

            if (Session["userid"] == null || Session["cusid"] == null || Session["vehiid"] == null)
                return Json(new { success = false, error = "Session expired or invalid." });

            int wyzUserId = Convert.ToInt32(Session["userid"]);
            cusid = Convert.ToInt32(Session["cusid"]);
            vehiid = Convert.ToInt32(Session["vehiid"]);

            //string fcmUrl = "https://fcm.googleapis.com/v1/projects/click-to-call-509b1/messages:send";
            string fcmUrl = ConfigurationManager.AppSettings["FcmUrl"];

            try
            {
                using (var db = new AutoSherDBContext())
                {
                    var cust = db.customers.FirstOrDefault(m => m.id == cusid);
                    var vehicle = db.vehicles.FirstOrDefault(m => m.vehicle_id == vehiid);
                    uniqid = db.uniqueidforcalls.FirstOrDefault(m => m.id == uniqueid)?.callinitiating_id ?? 0;

                    string toKey = db.wyzusers.FirstOrDefault(m => m.id == wyzUserId)?.registrationId;
                    if (string.IsNullOrEmpty(toKey))
                        return Json(new { success = false, error = "User registration ID not found." });

                    var firebaseFilePath = db.dealers.Select(m => m.andriodappfilepath).FirstOrDefault();
                    if (string.IsNullOrEmpty(firebaseFilePath) || !System.IO.File.Exists(firebaseFilePath))
                        return Json(new { success = false, error = "Firebase service account file not found." });

                    string accessToken = await GetAccessToken(firebaseFilePath);
                    if (string.IsNullOrEmpty(accessToken))
                        return Json(new { success = false, error = "Failed to obtain access token." });

                    if (!string.IsNullOrEmpty(cust?.customerName))
                        custName = cust.customerName;

                    if (!string.IsNullOrEmpty(vehicle?.vehicleRegNo))
                        vehicleRegNum = vehicle.vehicleRegNo;
                    else if (!string.IsNullOrEmpty(vehicle?.chassisNo))
                        vehicleRegNum = vehicle.chassisNo;

                    var message = new
                    {
                        message = new
                        {
                            token = toKey,
                            notification = new
                            {
                                title = "Incoming Call from CRM",
                                body = $"Click to call {custName}"
                            },
                            data = new
                            {
                                phonenumber = phnum,
                                makeCallFrom = "service",
                                type = "web",
                                customername = custName,
                                priority = "high",
                                vehicleRegNo = vehicleRegNum,
                                click_action = "FLUTTER_NOTIFICATION_CLICK"
                            },
                            android = new
                            {
                                priority = "high",
                                notification = new
                                {
                                    click_action = "FLUTTER_NOTIFICATION_CLICK",
                                    channel_id = "high_importance_channel"
                                }
                            }
                        }
                    };

                    var jsonMessage = JsonConvert.SerializeObject(message);
                    var content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");

                    var requestMessage = new HttpRequestMessage(HttpMethod.Post, fcmUrl)
                    {
                        Content = content
                    };
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    using (var client = new HttpClient())
                    {
                        client.Timeout = TimeSpan.FromSeconds(10);
                        var response = await client.SendAsync(requestMessage);
                        string responseString = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            dynamic apiResult = JObject.Parse(responseString);
                            if (apiResult["name"] != null)
                            {
                                Session["ncreason"] = true;
                                Session["iscallinitiated"] = "initiated";
                                Session["androiduniqueid"] = uniqid;
                                Session["dialednumber"] = phnum;

                                return Json(new { success = true, messageid = apiResult["name"].ToString() });
                            }
                            return Json(new { success = false, error = "Message ID is missing in the response." });
                        }
                        return Json(new { success = false, error = $"FCM request failed: {responseString}" });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = $"An error occurred: {ex.Message}" });
            }
        }

        #endregion

        #endregion

        #region Insurance AssignmentHistory History
        public ActionResult getAssignmentHistoryOfVehicleId(string moduleType)
        {
            int UserId = Convert.ToInt32(Session["UserId"].ToString());
            long vehiId = Convert.ToInt32(Session["VehiId"].ToString());
            List<long> takenList = new List<long>();
            List<SMSInteractionhistory> sms_int_data = new List<SMSInteractionhistory>();
            try
            {
                using (var db = new AutoSherDBContext())
                {

                    {
                        string campaign = "";
                        string reassign = "";
                        string reassignhistory = "";
                        string Cremangeruser = "";

                        string dealer = db.wyzusers.SingleOrDefault(m => m.id == UserId).dealerName;
                        List<assignedcallsreport> assign = db.assignedcallsreports.Where(m => m.vehicleId == vehiId && m.moduletypeId == 2).ToList();

                        foreach (var assign_li in assign)
                        {
                            SMSInteractionhistory smseachData = new SMSInteractionhistory();

                            if (!takenList.Contains(assign_li.assignInteractionID))
                            {
                                takenList.Add(assign_li.assignInteractionID);

                                wyzuser wyz = new wyzuser();
                                string user = "";
                                if (assign_li.wyzuserId != 0)
                                {
                                    long id = assign_li.wyzuserId;
                                    user = db.wyzusers.FirstOrDefault(m => m.id == id).userName;
                                }
                                if (assign_li.isConverted == true)
                                {
                                    smseachData.convertedstatus = "Converted";
                                }
                                else
                                {
                                    smseachData.convertedstatus = "-";
                                }
                                smseachData.resonforDrop = db.Database.SqlQuery<string>("select  whendeleted  from deletedstatusreport  where moduletype_id=2 and assigned_id=@id;", new MySqlParameter("@id", assign_li.assignInteractionID)).FirstOrDefault();
                                if (smseachData.resonforDrop == null)
                                {
                                    smseachData.resonforDrop = "-";
                                }
                                insuranceassignedinteraction assignAvail = new insuranceassignedinteraction();
                                assignAvail = db.insuranceassignedinteractions.SingleOrDefault(m => m.id == assign_li.assignInteractionID && m.vehicle_vehicle_id == vehiId);

                                string avail = "Removed";
                                if (assignAvail != null)
                                {
                                    avail = "Active";
                                }

                                string managerUser = "Auto";
                                bool autoassigned = db.assignedcallsreports.FirstOrDefault(m => m.assignInteractionID == assign_li.assignInteractionID).isautoassigned;
                                if (autoassigned == false)
                                {
                                    long id = assign_li.wyzuserId;
                                    Cremangeruser = db.wyzusers.FirstOrDefault(m => m.id == id).creManager;
                                    managerUser = Cremangeruser;
                                }
                                //if (assignAvail != null && assignAvail.isAutoAssigned == false)
                                //{
                                //    long id = assign_li.wyzuserId;
                                //    Cremangeruser = db.wyzusers.FirstOrDefault(m => m.id == id).creManager;
                                //    managerUser = Cremangeruser;
                                //}

                                insurancecallhistorycube callhistory = new insurancecallhistorycube();


                                int countcalhistory = db.insurancecallhistorycubes.Count(m => m.insuranceAssignedInteraction_id == assign_li.assignInteractionID && m.vehicle_vehicle_id == vehiId);
                                if (countcalhistory > 0)
                                {
                                    callhistory = db.insurancecallhistorycubes.FirstOrDefault(m => m.insuranceAssignedInteraction_id == assign_li.assignInteractionID && m.vehicle_vehicle_id == vehiId);
                                }

                                string lastDispo = "";
                                if (callhistory.SecondaryDisposition != null)
                                {
                                    lastDispo = callhistory.SecondaryDisposition;
                                }
                                long? cid = assign_li.campaignId;
                                if (cid != 0)
                                {
                                    campaign camp = db.campaigns.Where(c => c.id == cid).FirstOrDefault();
                                    campaign = camp.campaignName;
                                }


                                int countassignmentRecorrd = db.change_assignment_records.Count(m => m.assignedinteraction_id == assign_li.assignInteractionID && m.moduletypeIs == 2);

                                if (countassignmentRecorrd > 0)
                                {
                                    List<change_assignment_records> chng = db.change_assignment_records.Where(m => m.assignedinteraction_id == assign_li.assignInteractionID && m.moduletypeIs == 2).ToList();
                                    reassign = "Yes";
                                    foreach (change_assignment_records c in chng)
                                    {
                                        wyzuser w = db.wyzusers.Where(m => m.id == c.new_wyzuserId).FirstOrDefault();

                                        string d = c.updatedDate.ToString("dd/MM/yyyy");
                                        reassignhistory += w.userName + " -> " + d + "<br>";
                                    }
                                }
                                else
                                {
                                    reassign = "No";
                                    reassignhistory = "";
                                }

                                string assignid = "ASSIGNID" + assign_li.assignInteractionID;
                                smseachData.campaign = campaign;
                                smseachData.reassign = reassign;
                                smseachData.reassignhistory = reassignhistory;

                                smseachData.assignDate = assign_li.assignedDate;
                                smseachData.WyzuserName = user;
                                smseachData.reason = managerUser;
                                smseachData.assignedId = assignid;
                                smseachData.smsMessage = lastDispo;
                                smseachData.smsType = avail;

                                //smseachData.assignedBy = db.wyzusers.FirstOrDefault(m => m.id == assign_li.assigned_manager_id).userName;

                                sms_int_data.Add(smseachData);
                            }
                        }
                    }
                }
                return Json(new { data = sms_int_data, draw = Request["draw"], recordsTotal = sms_int_data.Count, recordsFiltered = sms_int_data.Count }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string exception = "";
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        exception = ex.InnerException.InnerException.Message;
                    }
                    else
                    {
                        exception = ex.InnerException.Message;
                    }
                }
                else
                {
                    exception = ex.Message;
                }
                return Json(new { data = "", draw = Request["draw"], recordsTotal = 0, recordsFiltered = 0, exception = exception }, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region Renewal Interaction History
        public ActionResult getrenewalHistoryOfVehicle()
        {
            long cusId = Convert.ToInt32(Session["CusId"].ToString());
            List<insurancerenewalpolicy> policy_data = new List<insurancerenewalpolicy>();
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    List<insurancerenewalpolicy> policy_int = db.Insurancerenewalpolicies.Where(m => m.customerid == cusId).OrderByDescending(m => m.id).ToList();

                    foreach (var ren_li in policy_int)
                    {
                        insurancerenewalpolicy policyData = new insurancerenewalpolicy();
                        policyData.disposeddate = ren_li.disposeddate;
                        policyData.creName = ren_li.creName;
                        policyData.id = ren_li.id;
                        policyData.insurancecompany = ren_li.insurancecompany;
                        policyData.renewwaltype = ren_li.renewwaltype;
                        policyData.Basic_IDV_Tenure = ren_li.Basic_IDV_Tenure;
                        policyData.Basic_OD_Tenure = ren_li.Basic_OD_Tenure;
                        policyData.NCB_Value_Tenure = ren_li.NCB_Value_Tenure;
                        policyData.Zero_Dep_Tenure = ren_li.Zero_Dep_Tenure;
                        policyData.RTI_Tenure = ren_li.RTI_Tenure;
                        policyData.Basic_TP_Tenure = ren_li.Basic_TP_Tenure;
                        policyData.grosspremium = ren_li.grosspremium;
                        policyData.totalgstamt = ren_li.totalgstamt;
                        policyData.totalpremium = ren_li.totalpremium;
                        policyData.campaignName = ren_li.campaignName;
                        policyData.disposedtime = ren_li.disposedtime;
                        if (db.Policyrenewalresponses.Count(m => m.insurancerenewalpolicyid == ren_li.id) > 0)
                        {
                            var policyResponse = db.Policyrenewalresponses.FirstOrDefault(m => m.insurancerenewalpolicyid == ren_li.id);
                            policyData.ResponseCode = policyResponse.ResponseCode;
                            if (!string.IsNullOrEmpty(policyResponse.ResponseMessage))
                            {
                                policyData.ResponseMessage = policyResponse.ResponseMessage.Length > 20 ? policyResponse.ResponseMessage.Substring(0, 20) : policyResponse.ResponseMessage;

                            }
                        }
                        policy_data.Add(policyData);
                    }
                }

                return Json(new { data = policy_data });
            }
            catch (Exception ex)
            {
                string exception = "";

                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        exception = ex.InnerException.InnerException.Message;
                    }
                    else
                    {
                        exception = ex.InnerException.Message;
                    }
                }
                else
                {
                    exception = ex.Message;
                }

                return Json(new { data = "", exception });

            }
        }
        #endregion

        #region SMS Interaction History
        public ActionResult getSMSHistoryOfCustomer()
        {
            long cusId = Convert.ToInt32(Session["CusId"].ToString());
            List<SMSInteractionhistory> sms_int_data = new List<SMSInteractionhistory>();
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    List<smsinteraction> sms_int = db.smsinteractions.Include("wyzuser").Where(m => m.customer_id == cusId).OrderByDescending(m => m.smsId).ToList();

                    foreach (var sms_li in sms_int)
                    {
                        SMSInteractionhistory smseachData = new SMSInteractionhistory();
                        //sms_li.wyzuser = db.smsinteractions.FirstOrDefault(m => m.customer_id == cusId).wyzuser;
                        string user = "";
                        if (sms_li.wyzuser != null)
                        {
                            user = sms_li.wyzuser.userName;
                        }

                        smseachData.interactionDate = sms_li.interactionDate;
                        smseachData.interactionTime = sms_li.interactionTime;
                        smseachData.WyzuserName = user;
                        smseachData.reason = sms_li.interactionType;
                        smseachData.smsMessage = sms_li.smsMessage;
                        smseachData.mobileNumber = sms_li.mobileNumber;

                        if (!string.IsNullOrEmpty(sms_li.smsType))
                        {
                            if (int.Parse(sms_li.smsType) != 0)
                            {
                                if (int.Parse(sms_li.smsType) == 004)
                                {
                                    smseachData.smsType = "Driver App";
                                }
                                else
                                {
                                    long smsTypeId = int.Parse(sms_li.smsType);
                                    smstemplate smsTemp = db.smstemplates.SingleOrDefault(m => m.smsId == smsTypeId);
                                    if (smsTemp != null)
                                    {
                                        smseachData.smsType = smsTemp.smsType;
                                    }
                                }
                            }

                            else
                            {
                                if (sms_li.isAutoSMS == true)
                                {
                                    smseachData.smsType = "AUTO";
                                }
                                else
                                {
                                    smseachData.smsType = "-";
                                }
                            }
                        }
                        else
                        {
                            smseachData.smsType = "Doc Upload";
                        }



                        if (sms_li.smsStatus)
                        {
                            smseachData.smsStatus = true;
                        }
                        else
                        {

                            smseachData.smsStatus = false;
                        }
                        sms_int_data.Add(smseachData);
                    }
                }

                return Json(new { success = true, data = sms_int_data });
            }
            catch (Exception ex)
            {
                string exception = "";

                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        exception = ex.InnerException.InnerException.Message;
                    }
                    else
                    {
                        exception = ex.InnerException.Message;
                    }
                }
                else
                {
                    exception = ex.Message;
                }

                return Json(new { success = false, exception });

            }
        }
        #endregion

        #region Insurance Interaction History
        public ActionResult getCallHistory(string typeIs)
        {

            //Logger logger = LogManager.GetLogger("apkRegLogger");
            string gsmAndroid = "";
            long vehicalId = Convert.ToInt32(Session["VehiId"].ToString());

            //Parameters of Paging..........
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            //string searchPattern = Request["search[value]"];

            int maxCount = 0;

            List<interactiondata> interDataList = new List<interactiondata>();

            string exception = "";
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    if (typeIs == "INS")
                    {
                        maxCount = db.insurancecallhistorycubes.Where(m => m.vehicle_vehicle_id == vehicalId).Count();
                        List<insurancecallhistorycube> insuranceCallHistoryByVehicle = db.insurancecallhistorycubes.Where(m => m.vehicle_vehicle_id == vehicalId).OrderByDescending(X => X.callDate).ThenByDescending(x => x.callTime).Skip(start).Take(length).ToList();

                        if (insuranceCallHistoryByVehicle != null && insuranceCallHistoryByVehicle.Count() > 0)
                        {
                            insuranceCallHistoryByVehicle = insuranceCallHistoryByVehicle.OrderByDescending(m => m.id).ToList();
                        }

                        foreach (var call_data in insuranceCallHistoryByVehicle)
                        {
                            interactiondata interData = new interactiondata();


                            if (call_data.callDate != null)
                            {
                                interData.CallDate = Convert.ToDateTime(call_data.callDate).ToString("dd-MM-yyyy");
                            }
                            else
                            {
                                interData.CallDate = "-";
                            }

                            interData.Campaign = call_data.campaignTYpe;

                            if (Session["DealerCode"].ToString() == "INDUS")
                            {
                                var wyzUser = db.wyzusers.FirstOrDefault(m => m.id == call_data.wyzUser_id);

                                if (wyzUser != null)
                                {
                                    interData.CreId = wyzUser.firstName + "(" + wyzUser.userName + ")";
                                }
                                else
                                {
                                    interData.CreId = call_data.creName;
                                }
                            }
                            else
                            {
                                interData.CreId = call_data.creName;
                            }

                            interData.AssignId = "AASIGNID" + call_data.insuranceAssignedInteraction_id.ToString();
                            interData.Time = call_data.callTime.ToString();

                            if (string.IsNullOrEmpty(call_data.callType))
                            {
                                interData.CallType = call_data.callType;
                            }
                            else
                            {
                                interData.CallType = "-";
                            }
                            if (call_data.gsm_android != null)
                            {
                                gsmAndroid = "(" + call_data.gsm_android + ")";
                            }
                            interData.IsCallInitiated = call_data.isCallinitaited + gsmAndroid;
                            if (!string.IsNullOrEmpty(call_data.makeCallFrom))
                            {
                                interData.CallMadeType = call_data.makeCallFrom;
                            }
                            else
                            {
                                interData.CallMadeType = "-";
                            }

                            if (!string.IsNullOrEmpty(call_data.dailedNumber))
                            {
                                interData.DailedNo = call_data.dailedNumber;
                            }
                            else
                            {
                                interData.DailedNo = "-";
                            }

                            if (!string.IsNullOrEmpty(call_data.SecondaryDisposition))
                            {
                                interData.SecondaryDispo = call_data.SecondaryDisposition;
                            }
                            else
                            {
                                interData.SecondaryDispo = "-";
                            }

                            if (!string.IsNullOrEmpty(call_data.SecondaryDisposition))
                            {
                                if (call_data.SecondaryDisposition == "Call Me Later")
                                {
                                    interData.Details = Convert.ToDateTime(call_data.followUpDate).ToString("dd-MM-yyyy") + " " + call_data.followUpTime;
                                }
                                else if (call_data.SecondaryDisposition == "Book Appointment")
                                {
                                    if (!string.IsNullOrEmpty(call_data.Reshedule_Status) && call_data.Reshedule_Status == "Rescheduled")
                                    {
                                        interData.Details = call_data.typeOfPickup + "|" + call_data.Tertiary_disposition + "|" + call_data.Reshedule_Status;
                                    }
                                    else
                                    {
                                        {
                                            interData.Details = call_data.typeOfPickup + "|" + Convert.ToDateTime(call_data.appointmentDate).ToString("dd-MM-yyyy") + "  " + call_data.appointmentFromTime + "|" + call_data.insuranceCompany;
                                        }

                                    }
                                }
                                else if (call_data.SecondaryDisposition == "Renewal Not Required")
                                {
                                    interData.Details = Convert.ToDateTime(call_data.callDate).ToString("dd-MM-yyyy") + "  " + call_data.callTime.ToString().Substring(0, call_data.callTime.ToString().IndexOf(":") + 3) + "|" + call_data.renewalNotRequiredReason;
                                }
                                else
                                {
                                    interData.Details = Convert.ToDateTime(call_data.callDate).ToString("dd-MM-yyyy") + "  " + call_data.callTime.ToString().Substring(0, call_data.callTime.ToString().IndexOf(":") + 3);
                                }
                            }
                            else
                            {
                                interData.Details = "-";
                            }


                            if (!string.IsNullOrEmpty(call_data.comments))
                            {
                                interData.CreRemarks = call_data.comments;
                            }
                            else
                            {
                                interData.CreRemarks = "-";
                            }

                            if (!string.IsNullOrEmpty(call_data.customerComments))
                            {
                                interData.Feedback = call_data.customerComments;
                            }
                            else
                            {
                                interData.Feedback = "-";
                            }



                            //filepath
                            if (!string.IsNullOrEmpty(call_data.filepath))
                            {
                                interData.FilePath = call_data.filepath;
                            }
                            else
                            {
                                //var callsynchData = db.callSyncDatas.FirstOrDefault(m => m.callinteraction_id == call_data.cicallinteraction_id);
                                //if (callsynchData != null)
                                //{
                                //    interData.FilePath = callsynchData.filepath;
                                //}
                                //else
                                //{
                                var callinteraction = db.callinteractions.FirstOrDefault(m => m.id == call_data.cicallinteraction_id);
                                if (callinteraction != null)
                                {
                                    interData.FilePath = callinteraction.filePath;
                                }
                                else
                                {
                                    interData.FilePath = "-";
                                }
                                //}
                            }


                            interDataList.Add(interData);
                        }
                    }
                }

                return Json(new { data = interDataList, draw = Request["draw"], recordsTotal = maxCount, recordsFiltered = maxCount, exception = exception }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        exception = ex.InnerException.InnerException.Message;
                    }
                    else
                    {
                        exception = ex.InnerException.Message;
                    }
                }
                else
                {
                    exception = ex.Message;
                }
                return Json(new { data = "", draw = Request["draw"], recordsTotal = 0, recordsFiltered = 0, exception = exception }, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        public ActionResult ReturnToBucket(int id)
        {
            if (TempData["SubmissionResult"] != null)
            {
                TempData["SubmissionResult"] = TempData["SubmissionResult"];
            }
            if (id == 2)
            {
                return RedirectToAction("Insurance", "Insurance");
            }
            else
            {
                return RedirectToAction("Insurance", "Insurance");

            }
        }

        public List<smstemplate> getSMSTemplate(string typeofDis)
        {
            List<smstemplate> smstemplates = new List<smstemplate>();

            using (var db = new AutoSherDBContext())
            {
                if (typeofDis == "Insurance")
                {
                    smstemplates = db.smstemplates.Where(m => (m.deliveryType == "Manual" || m.deliveryType == "Both") && (m.moduletype == 2 || m.moduletype == 3) && m.inActive == false && m.isWhatsapp == false).ToList();
                }
            }
            return smstemplates;
        }
        public ActionResult getCityNames(string state)
        {

            try
            {
                using (var db = new AutoSherDBContext())
                {
                    var cities = db.citystates.Where(m => m.state == state).OrderBy(m => m.city).Select(m => m.city).ToList();
                    return Json(new { success = true, cities });
                }
            }
            catch (Exception ex)
            {

            }
            return Json(new { succee = false, cities = new citystate() });
        }

        //public void autoHeroCreatePolicy(insurancerenewalpolicy insurancerenewalpolicy)
        //{

        //    IScheduler autosmsScheduler = StdSchedulerFactory.GetDefaultScheduler().Result;

        //    autosmsScheduler.Start();
        //    IJobDetail jobDetail = JobBuilder.Create<createpolicyAutoJob>().Build();

        //    string trigername = "AutoCreatePolicyTriggerName" + DateTime.Now.Millisecond + insurancerenewalpolicy.vehicleid;
        //    string trigerGroupName = "AutoCreatePolicyTriggerGroup" + DateTime.Now.Millisecond + insurancerenewalpolicy.vehicleid;

        //    ITrigger trigger = TriggerBuilder.Create().WithIdentity(trigername, trigerGroupName).StartNow().WithSimpleSchedule().Build();

        //    jobDetail.JobDataMap["RenewalDetails"] = JsonConvert.SerializeObject(insurancerenewalpolicy);


        //    autosmsScheduler.ScheduleJob(jobDetail, trigger);

        //}

        #region DialPad  Click to call
        public ActionResult dialpadclicktoCall(string phNum)
        {
            try
            {
                Session["DialedNumber"] = phNum;
                return Json(new { success = true, error = "" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
        #endregion



        [HttpPost, ActionName("Winbackpolicy")]

        //public ActionResult Winbackpolicy(CallLoggingViewModel callLogging )
        //{

        //    long cusId = Convert.ToInt32(Session["cusId"].ToString());
        //    long vehiId = Convert.ToInt32(Session["vehiId"].ToString());





        //    try
        //    {
        //        using (var db = new AutoSherDBContext())
        //       {
        //            callLogging.winbackPolicy.customerId =cusId;
        //            callLogging.winbackPolicy.vehicleid = vehiId;
        //           // string maritalstatus = callLogging.winbackPolicy.maritalstatus;

        //            if (db.state_masters.Count(m=>m.State_Id == callLogging.winbackPolicy.proposerstate)>0)
        //            {
        //                callLogging.winbackPolicy.proposerstatename = db.state_masters.FirstOrDefault(m => m.State_Id == callLogging.winbackPolicy.proposerstate).State_Name;
        //            }

        //            if (db.city_masters.Count(m => m.id == callLogging.winbackPolicy.proposercity) > 0)
        //            {
        //                callLogging.winbackPolicy.proposercityname = db.city_masters.FirstOrDefault(m => m.id == callLogging.winbackPolicy.proposercity).city_name;
        //            }
        //            //if(callLogging.winbackPolicy.oem=="4" || callLogging.winbackPolicy.oem == "2")

        //            if (!(string.IsNullOrEmpty(callLogging.winbackPolicy.modelname)))
        //            {
        //                int modelId = Convert.ToInt32(callLogging.winbackPolicy.modelname);
        //                if (db.model_masters.Count(m => m.Model_Id == modelId) > 0)
        //                {
        //                    callLogging.winbackPolicy.cubiccapacity = db.model_masters.FirstOrDefault(m => m.Model_Id == modelId).Cubic_Capacity;
        //                }

        //            }


        //            //if (string.IsNullOrEmpty(maritalstatus))
        //            //{
        //            //    callLogging.winbackPolicy.maritalstatus = "Single";
        //            //}

        //            //if (callLogging.winbackPolicy.maritalstatus == null)
        //            //{
        //            //    callLogging.winbackPolicy.maritalstatus = "Single";
        //            //}

        //            //if (!(string.IsNullOrEmpty(callLogging.winbackPolicy.modelname)))
        //            //{
        //            //    int modelId = 0;
        //            //    var modelDetails = db.model_masters.FirstOrDefault(m => m.Model_Name == callLogging.winbackPolicy.modelname);
        //            //    if (modelDetails != null)
        //            //    {
        //            //        modelId = modelDetails.Model_Id;

        //            //        callLogging.winbackPolicy.cubiccapacity = db.model_masters.FirstOrDefault(m => m.Model_Id == modelId).Cubic_Capacity;

        //            //    }
        //            //}


        //            callLogging.winbackPolicy.product = callLogging.winbackPolicy.productId;



        //            db.WinbackPolicy.Add(callLogging.winbackPolicy);


        //            db.SaveChanges();
        //            autosendQuotation(callLogging.winbackPolicy);
        //            return Json(new { success = true, message = "Saved Successfully", JsonRequestBehavior.AllowGet });
        //        }



        //    }


        


            
        //    //catch (Exception ex)
        //    //{
        //    //    return Json(new { success = false, message = "Failed", JsonRequestBehavior.AllowGet });
        //    //}
        //    //catch (NullPointerException ex)
        //    //{
        //    //    Console.WriteLine("Variable is Null!");
        //    //}
        //    catch (Exception ex)
        //    {
        //        //ex.InnerException
        //        string exception = string.Empty;
        //        if (ex.InnerException != null)
        //        {
        //            if (ex.InnerException.InnerException != null)
        //            {
        //                exception = ex.InnerException.InnerException.Message;
        //            }
        //            else
        //            {
        //                exception = ex.InnerException.Message;
        //            }
        //        }
        //        else
        //        {
        //            exception = ex.Message;
        //        }
        //        return Json(new { success = false, message = "Failed", JsonRequestBehavior.AllowGet });
        //    }






        //}








        public ActionResult getfinalproposal()
        {
            long cusId = Convert.ToInt32(Session["cusId"].ToString());
            long vehiId = Convert.ToInt32(Session["vehiId"].ToString());

            try
            {

                using (var db = new AutoSherDBContext())
                {

                    List<winbackpolicyrenewalresponse> finalproposal = db.Winbackpolicyrenewalresponses.Where(m => m.customerId == cusId && m.vehicleid == vehiId).OrderByDescending(m => m.id).ToList();
                    foreach (var prod in finalproposal)
                    {

                        if (db.Productmaster.Count(m => m.Product_id == prod.Product_id) > 0)
                        {
                            prod.productName = db.Productmaster.FirstOrDefault(m => m.Product_id == prod.Product_id).Product_Name;
                        }

                    }

                    return Json(new { data = finalproposal, JsonRequestBehavior.AllowGet });
                }

            }
            catch (Exception ex)
            {
                string exception = string.Empty;
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        exception = ex.InnerException.InnerException.Message;
                    }
                    else
                    {
                        exception = ex.InnerException.Message;
                    }
                }
                else
                {
                    exception = ex.Message;
                }
                return Json(new { data = false, JsonRequestBehavior.AllowGet });
            }

        }


        public ActionResult getfinalproposalResponse()
        {
            long cusId = Convert.ToInt32(Session["cusId"].ToString());
            long vehiId = Convert.ToInt32(Session["vehiId"].ToString());

            try
            {

                using (var db = new AutoSherDBContext())
                {

                    List<finalquotationresponse> finalproposal = db.Finalquotationresponses.Where(m => m.customerid == cusId && m.vehicleid == vehiId).OrderByDescending(m => m.id).ToList();


                    return Json(new { data = finalproposal, JsonRequestBehavior.AllowGet });
                }

            }
            catch (Exception ex)
            {
                string exception = string.Empty;
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        exception = ex.InnerException.InnerException.Message;
                    }
                    else
                    {
                        exception = ex.InnerException.Message;
                    }
                }
                else
                {
                    exception = ex.Message;
                }
                return Json(new { data = false, JsonRequestBehavior.AllowGet });
            }

        }




        #region finalQuotationproposal
        //public ActionResult sendFinalProposal(long renewalresponseId, string campaignId)
        //{

        //    try
        //    {
        //        using (var db = new AutoSherDBContext())
        //        {
        //            winbackpolicyrenewalresponse finalProposal = db.Winbackpolicyrenewalresponses.FirstOrDefault(m => m.id == renewalresponseId);
        //            FinalautosendQuotation(finalProposal, campaignId);
        //            return Json(new { success = true, JsonRequestBehavior.AllowGet });
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        string exception = string.Empty;
        //        if (ex.InnerException != null)
        //        {
        //            if (ex.InnerException.InnerException != null)
        //            {
        //                exception = ex.InnerException.InnerException.Message;
        //            }
        //            else
        //            {
        //                exception = ex.InnerException.Message;
        //            }
        //        }
        //        else
        //        {
        //            exception = ex.Message;
        //        }
        //        return Json(new { data = false, JsonRequestBehavior.AllowGet });
        //    }
        //}
        #endregion


        #region QuotationSentAPI
        //public void autosendQuotation(WinbackPolicy winbackPolicyDetails)
        //{

        //    IScheduler autosmsScheduler = StdSchedulerFactory.GetDefaultScheduler().Result;

        //    autosmsScheduler.Start();
        //    IJobDetail jobDetail = JobBuilder.Create<WinbackpolicyAutoJob>().Build();

        //    string trigername = "AutoCreatePolicyTriggerName" + DateTime.Now.Millisecond + winbackPolicyDetails.vehicleid;
        //    string trigerGroupName = "AutoCreatePolicyTriggerGroup" + DateTime.Now.Millisecond + winbackPolicyDetails.vehicleid;

        //    ITrigger trigger = TriggerBuilder.Create().WithIdentity(trigername, trigerGroupName).StartNow().WithSimpleSchedule().Build();

        //    jobDetail.JobDataMap["quotationDetails"] = JsonConvert.SerializeObject(winbackPolicyDetails);


        //    autosmsScheduler.ScheduleJob(jobDetail, trigger);

        //}

        #endregion



        #region FinalRequest QuotationSentAPI

        //public void FinalautosendQuotation(winbackpolicyrenewalresponse renewalResponse,string campaignId)
        //{

        //    IScheduler autosmsScheduler = StdSchedulerFactory.GetDefaultScheduler().Result;

        //    autosmsScheduler.Start();
        //    IJobDetail jobDetail = JobBuilder.Create<WinBackPolicyFinalProposalAutoJob>().Build();

        //    string trigername = "FinalQuoteTriggerName" + DateTime.Now.Millisecond + renewalResponse.vehicleid;
        //    string trigerGroupName = "FinalQuoteTriggerGroup" + DateTime.Now.Millisecond + renewalResponse.vehicleid;

        //    ITrigger trigger = TriggerBuilder.Create().WithIdentity(trigername, trigerGroupName).StartNow().WithSimpleSchedule().Build();

        //    jobDetail.JobDataMap["FinalquotationDetails"] = JsonConvert.SerializeObject(renewalResponse);
        //    jobDetail.JobDataMap["CampaignId"] = campaignId;


        //    autosmsScheduler.ScheduleJob(jobDetail, trigger);

        //}




        #endregion

        public ActionResult UpdatePhoneAndEmail(long mobilenumber)
        {
            //public ActionResult UpdatePhoneAndEmail(string mobile, string email)
            long cusId = Convert.ToInt32(Session["cusId"].ToString());
            long vehiId = Convert.ToInt32(Session["vehiId"].ToString());
            int UserId = Convert.ToInt32(Session["UserId"].ToString());

            try
            {
                using (var db = new AutoSherDBContext())
                {
                    var update1 = db.vehicles.Where(m => m.customer_id == cusId && m.vehicle_id == vehiId).ToList();
                    var update = update1.OrderByDescending(m => m.vehicle_id).FirstOrDefault();

                    update.Mobile = Convert.ToString(mobilenumber);
                    //update.email = emailid;

                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully", JsonRequestBehavior.AllowGet });


                }
            }

            catch (Exception ex)
            {
                //ex.InnerException
                string exception = string.Empty;
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        exception = ex.InnerException.InnerException.Message;
                    }
                    else
                    {
                        exception = ex.InnerException.Message;
                    }
                }
                else
                {
                    exception = ex.Message;
                }
                return Json(new { success = false, message = "Failed", JsonRequestBehavior.AllowGet });
            }

        }

        public ActionResult UpdateEmail(string emailid)
        {
            //public ActionResult UpdatePhoneAndEmail(string mobile, string email)
            long cusId = Convert.ToInt32(Session["cusId"].ToString());
            long vehiId = Convert.ToInt32(Session["vehiId"].ToString());
            int UserId = Convert.ToInt32(Session["UserId"].ToString());

            try
            {
                using (var db = new AutoSherDBContext())
                {
                    var update12 = db.vehicles.Where(m => m.customer_id == cusId && m.vehicle_id == vehiId).ToList();
                    var update2 = update12.OrderByDescending(m => m.vehicle_id).FirstOrDefault();

                    //update.Mobile = Convert.ToString(mobilenumber);
                    update2.email = emailid;

                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully", JsonRequestBehavior.AllowGet });


                }
            }

            catch (Exception ex)
            {
                //ex.InnerException
                string exception = string.Empty;
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        exception = ex.InnerException.InnerException.Message;
                    }
                    else
                    {
                        exception = ex.InnerException.Message;
                    }
                }
                else
                {
                    exception = ex.Message;
                }
                return Json(new { success = false, message = "Failed", JsonRequestBehavior.AllowGet });
            }

        }

        public ActionResult CancelEmail1234(string oldEmail)
        {
            //public ActionResult UpdatePhoneAndEmail(string mobile, string email)
            long cusId = Convert.ToInt32(Session["cusId"].ToString());
            long vehiId = Convert.ToInt32(Session["vehiId"].ToString());
            int UserId = Convert.ToInt32(Session["UserId"].ToString());

            return View();

        }

        public ActionResult CancelMobileNumber1234()
        {
            //public ActionResult UpdatePhoneAndEmail(string mobile, string email)
            long cusId = Convert.ToInt32(Session["cusId"].ToString());
            long vehiId = Convert.ToInt32(Session["vehiId"].ToString());
            int UserId = Convert.ToInt32(Session["UserId"].ToString());

            return View();

        }

        //void createHeroPolicy(insurancerenewalpolicy policy)
        //{
        //    string requestbody = string.Empty;
        //    Logger logger = LogManager.GetLogger("apkRegLogger");

        //    try
        //    {

        //        policyrenewalresponse renewalResponse = new policyrenewalresponse();
        //        mappolicyrenewalresponse maprenewalResponse = new mappolicyrenewalresponse();

        //        dynamic renewaldata = new JObject();

        //        renewaldata.cpA_Tenure = policy.cpaopted == "Y" ? 1 : 0;

        //        if (policy.reasonoptingoutcpa == "Owner not having valid Driving license")
        //        {
        //            renewaldata.cpA_Reason = "NODL";
        //        }
        //        else if (policy.reasonoptingoutcpa == "Owner having seperate General PA Cover (Non-motor prodcts)")
        //        {
        //            renewaldata.cpA_Reason = "GENPA";

        //        }
        //        else if (policy.reasonoptingoutcpa == "Owner having standalone PA Cover(Motor Product)")
        //        {
        //            renewaldata.cpA_Reason = "SCPA";
        //        }
        //        else
        //        {
        //            renewaldata.cpA_Reason = "";
        //        }

        //        renewaldata.policy_type = policy.renewwaltype;
        //        renewaldata.policy_tenure = policy.policytenure;
        //        renewaldata.claim_on_previous_policy = policy.existingpolicyclaim;
        //        renewaldata.current_ncb = policy.NCB_Value_Tenure;
        //        renewaldata.cpa_opted = policy.cpaopted;
        //        renewaldata.zd_opted = policy.additionalcoverzerodep;
        //        renewaldata.rti_opted = policy.additionalcoverRTI;
        //        renewaldata.risk_start_date = DateTime.Now.ToString("yyyy-MM-yy");
        //        renewaldata.policy_expiry_date = policy.policyexpirydate;
        //        renewaldata.first_name = policy.customername;
        //        renewaldata.middle_name = policy.middlename;
        //        renewaldata.last_name = policy.lastname;
        //        renewaldata.email = policy.email;
        //        renewaldata.mobile_no = policy.mobile;
        //        if (!(string.IsNullOrEmpty(policy.Address)))
        //        {
        //            renewaldata.address1 = policy.Address.Replace(@"\", string.Empty);
        //        }
        //        if (!(string.IsNullOrEmpty(policy.Address_Line_2)))
        //        {
        //            renewaldata.address2 = policy.Address_Line_2.Replace(@"\", string.Empty);
        //        }
        //        renewaldata.address3 = null;
        //        renewaldata.city = policy.cityid;
        //        renewaldata.state = policy.stateid;
        //        renewaldata.pin = policy.pincode;
        //        renewaldata.nominee_name = policy.nomineename;
        //        renewaldata.nominee_age = policy.nomineeage;
        //        renewaldata.nominee_relationship = policy.nomineerelationshipid;
        //        renewaldata.vehRegDate = policy.vehicleregdate;
        //        renewaldata.vehReg1 = policy.vehicleregno.Substring(0, 2);
        //        renewaldata.vehReg2 = policy.vehicleregno.Substring(2, 2);
        //        renewaldata.vehReg3 = policy.vehicleregno.Substring(4, 2);
        //        renewaldata.vehReg4 = policy.vehicleregno.Substring(6, (policy.vehicleregno.Length - 6));
        //        renewaldata.total_premium_collected = Convert.ToInt64(policy.totalpremium);
        //        renewaldata.pre_Policy_No = policy.policynumber;
        //        renewaldata.GrossPremiumWoGST = policy.grosspremium;
        //        renewaldata.TotalGSTAmount = policy.totalgstamt;
        //        renewaldata.GSTPercentage = policy.GST_Per;


        //        //string baseURL = "http://www.heroibipl.com/HeroANinjaAPI_UAT/api/HeroCreatePolicy/CreatePolicy";
        //        string baseURL = "http://www.heroibipl.com/HeroANinjaAPI_UAT/api/HeroCreatePolicy/CreatePolicy";

        //        WebRequest request = WebRequest.Create(baseURL);
        //        var httprequest = (HttpWebRequest)request;

        //        // httprequest.PreAuthenticate = true;
        //        httprequest.Method = "POST";
        //        httprequest.ContentType = "application/json";
        //        httprequest.Headers["HERO_API_KEY"] = "H$E@R0@123";

        //        httprequest.Accept = "application/json";

        //        using (var streamWriter = new StreamWriter(httprequest.GetRequestStream()))
        //        {
        //            var bodyContent = JsonConvert.SerializeObject(renewaldata);
        //            streamWriter.Write(bodyContent);
        //            requestbody = bodyContent;
        //            logger.Info("\n\n heroibipl Create Policy Request: " + bodyContent);
        //            streamWriter.Flush();
        //            streamWriter.Close();
        //        }

        //        HttpWebResponse response = null;
        //        response = (HttpWebResponse)httprequest.GetResponse();

        //        string response_string = string.Empty;
        //        using (Stream strem = response.GetResponseStream())
        //        {
        //            StreamReader sr = new StreamReader(strem);

        //            response_string = sr.ReadToEnd();
        //            sr.Close();
        //        }
        //        response_string = response_string.TrimStart(new char[] { '[' }).TrimEnd(new char[] { ']' });

        //        dynamic api_result = JObject.Parse(response_string);
        //        renewalResponse.ResponseCode = api_result.ResponseCode;
        //        renewalResponse.ResponseMessage = api_result.ResponseMessage;
        //        renewalResponse.PolicyNo = api_result.PolicyNo;
        //        renewalResponse.HeroTransaction = api_result.HeroTransaction;
        //        renewalResponse.request = requestbody;
        //        renewalResponse.response = response_string;
        //        renewalResponse.insurancerenewalpolicyid = policy.id;
        //        renewalResponse.vehicleid = policy.vehicleid;
        //        renewalResponse.customerid = policy.customerid;
        //        renewalResponse.wyzuser_id = policy.creID;
        //        renewalResponse.dealer_id = policy.herodealerId;
        //        renewalResponse.insuranceassignedinteractionId = policy.insuranceassignedinteractionId;
        //        renewalResponse.updatedDate = DateTime.Now;
        //        renewalResponse.updatedTime = DateTime.Now.ToShortTimeString();

        //        using (var db = new AutoSherDBContext())
        //        {
        //            db.Policyrenewalresponses.Add(renewalResponse);
        //            if (api_result.ResponseCode == "200")
        //            {
        //                insurancerenewalpolicy existngpolicy = db.Insurancerenewalpolicies.FirstOrDefault(m => m.id == policy.id);
        //                existngpolicy.ispolicygenerated = true;
        //                db.Insurancerenewalpolicies.AddOrUpdate(existngpolicy);
        //            }
        //            db.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.InnerException != null)
        //        {
        //            if (ex.InnerException.InnerException != null)
        //            {
        //                logger.Info("\n\n -------- heroibipl OutBlock Exception --------\n" + ex.InnerException.InnerException.Message);
        //            }
        //            else
        //            {
        //                logger.Info("\n\n -------- heroibipl OutBlock Exception --------\n" + ex.InnerException.Message);
        //            }
        //        }
        //        else
        //        {
        //            logger.Info("\n\n -------- heroibipl OutBlock Exception --------\n" + ex.Message);
        //        }

        //    }

        //}

        private static async Task<string> GetAccessToken(string firebasefilepath)
        {
            //string serviceAccountFile = Server.MapPath("~/path/to/your/service-account-file.json");
            // string serviceAccountFile = "E:/path/service-account-file.json";
            string serviceAccountFile = firebasefilepath;
            string[] scopes = { "https://www.googleapis.com/auth/cloud-platform" };

            var credential = GoogleCredential.FromFile(serviceAccountFile)
                                            .CreateScoped(scopes);

            var accessToken = await GetAccessTokenAsync(credential);
            return accessToken;
        }

        private static async Task<string> GetAccessTokenAsync(GoogleCredential credential)
        {

            var tokenRequest = credential.UnderlyingCredential.GetAccessTokenForRequestAsync();
            var tokenResponse = await tokenRequest;
            return tokenResponse;
        }

        #region Generate PDF

        private AutoSherDBContext db = new AutoSherDBContext();
        //[HttpPost]
        [Obsolete]
        public async Task<ActionResult> AutoPDF(string quoteId)
        {
            try
            {
                long quoteid = long.Parse(quoteId);
                long vehiId = Convert.ToInt32(Session["VehiId"].ToString());
                long custId = Convert.ToInt32(Convert.ToString(Session["CusId"]));
                var UserName = Session["UserName"].ToString();
                var dealerName = Session["DealerName"].ToString();
                var dealerCode = Session["DealerCode"].ToString();

                CallLoggingViewModel cvm = new CallLoggingViewModel();
                cvm.cust = db.customers.Where(m => m.id == custId).OrderByDescending(m => m.id).FirstOrDefault();
                cvm.wyzuser = db.wyzusers.FirstOrDefault(m => m.userName == UserName && m.dealerCode == dealerCode);

                string userFolder = dealerCode.Replace(" ", "_");
                string userFolderPath = UserName.Replace(" ", "_");

                // Define the nested folder path structure
                string folderPath = Server.MapPath("~/UploadedFiles/" + userFolder + "/" + userFolderPath);

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string uniqueIdentifier = Guid.NewGuid().ToString();
                string fileName = "EDME_Premium_Worksheet_" + cvm.cust.customerName.Replace(" ", "_") + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + "_" + uniqueIdentifier + ".pdf";
                string filePath = Path.Combine(folderPath, fileName);
                int UserId = Convert.ToInt32(Session["UserId"].ToString());


                documentuploadhistory doc = new documentuploadhistory();
                doc.user = UserName;
                doc.uploadFileName = fileName;
                doc.documentName = "Quotation";
                doc.uploadDateTime = DateTime.Now;
                doc.userId = UserId;
                doc.filePath = filePath;
                doc.deptName = "Insurance";
                doc.customerId = custId;
                db.documentuploadhistories.Add(doc);
                db.SaveChanges();

                cvm.quotationData = db.quotationresponses.Where(m => m.vehicleid == vehiId && m.id == quoteid).OrderByDescending(m => m.id).FirstOrDefault();
                cvm.vehi = db.vehicles.Where(m => m.vehicle_id == vehiId).OrderByDescending(m => m.vehicle_id).FirstOrDefault();

                string htmlString = RenderViewToString("GeneratePDF", cvm);
                var pdfBytes = new ViewAsPdf("GeneratePDF", cvm).BuildPdf(this.ControllerContext);
                if (pdfBytes == null || pdfBytes.Length == 0)
                {
                    throw new Exception("PDF generation failed.");
                }

                System.IO.File.WriteAllBytes(filePath, pdfBytes);

                //string messageTxt = "Success";
                //return Json(new { success = true, messageTxt, JsonRequestBehavior.AllowGet });
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                //return File(pdfBytes, "application/pdf", "Quotation.pdf");
            }
            catch (Exception ex)
            {
                //System.IO.File.WriteAllText("C:\\temp\\debug.txt", ex.Message);
                //return Json(new { success = false, error = ex.Message });
                throw ex;
            }
        }


        private string RenderViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                if (viewResult.View != null)
                {
                    var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                    viewResult.View.Render(viewContext, sw);
                    viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                    return sw.GetStringBuilder().ToString();
                }
                else
                {
                    throw new Exception("View not found: " + viewName);
                }
            }
        }

        #endregion


        #region EIBL Renewal, Winback and Rollover Policy Implementation

        [HttpPost]
        public async Task<ActionResult> GetPlanDetailsAsync()
        {
            long cusId = Convert.ToInt32(Session["cusId"].ToString());
            long vehiId = Convert.ToInt32(Session["vehiId"].ToString());

            try
            {
                string URL = ConfigurationManager.AppSettings["GetPlan_ApiUrl"];
                string accessToken = await GenerateToken.GenerateTokenAsync();

                using (var db = new AutoSherDBContext())
                {
                    var userVehicleInsDetails = await db.vehicles.Where(m => m.vehicle_id == vehiId && m.customer_id == cusId).Select(m => new { m.oemid, m.Previous_Policy_Number, m.CP_CODE, m.campaign_id }).FirstOrDefaultAsync();

                    int? policy_type = GetPolicyTypeBasedOnCampaign(userVehicleInsDetails?.campaign_id ?? null);

                    var requestBody = new
                    {
                        OEM_ID = userVehicleInsDetails?.oemid ?? 0,
                        PreviousPolicyNo = userVehicleInsDetails?.Previous_Policy_Number ?? "",
                        CPCode = userVehicleInsDetails?.CP_CODE ?? "",
                        Type = policy_type
                    };

                    using (var httpClient = new HttpClient())
                    {
                        string content = JsonConvert.SerializeObject(requestBody);

                        StringContent stringContent = new StringContent(content, Encoding.UTF8, "application/json");

                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                        HttpResponseMessage response = await httpClient.PostAsync(URL, stringContent);

                        string jsonResponse = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            GetPlanAPIResponse getPlanAPIResponse = new GetPlanAPIResponse();

                            getPlanAPIResponse = JsonConvert.DeserializeObject<GetPlanAPIResponse>(jsonResponse);

                            if (getPlanAPIResponse.ErrorMessage != null)
                            {
                                return Json(new { success = false, message = $"{getPlanAPIResponse.ErrorMessage}" });
                            }

                            if (getPlanAPIResponse?.VehicleCover != null)
                            {
                                List<PlanResponse> existPlanList = await db.planresponse.Where(m => m.CustomerId == cusId && m.Vehicleid == vehiId).ToListAsync();
                                if (existPlanList != null && existPlanList.Count > 0)
                                {
                                    db.planresponse.RemoveRange(existPlanList);
                                    await db.SaveChangesAsync();
                                }

                                foreach (var data in getPlanAPIResponse.VehicleCover)
                                {
                                    PlanResponse planResponse = new PlanResponse();

                                    planResponse.CustomerId = cusId;
                                    planResponse.Vehicleid = vehiId;
                                    planResponse.SATP = data.SATP;
                                    planResponse.SAOD = data.SAOD;
                                    planResponse.Package = data.Package;
                                    planResponse.OEMID = data.OEMID;
                                    planResponse.PreviousPolicyNo = data.PreviousPolicyNo;
                                    planResponse.CPCode = data.CPCode;
                                    planResponse.STATUS = data.STATUS;
                                    planResponse.IsActive = data.IsActive;

                                    db.planresponse.Add(planResponse);
                                }
                                await db.SaveChangesAsync();

                                PlanResponse latestPlan = await db.planresponse.Where(m => m.CustomerId == cusId && m.Vehicleid == vehiId).OrderByDescending(m => m.Id).FirstOrDefaultAsync();

                                return Json(new { success = true, latestPlanData = latestPlan });
                            }
                            else
                            {
                                if (getPlanAPIResponse.ErrorMessage != null)
                                {
                                    return Json(new { success = false, message = $"{getPlanAPIResponse?.ErrorMessage ?? ""}" });
                                }

                                JObject jsonRespObj = JObject.Parse(jsonResponse);

                                if (jsonRespObj.ContainsKey("Message"))
                                {
                                    string message = jsonRespObj["Message"]?.ToString();
                                    return Json(new { success = false, message = $"Error Message: {getPlanAPIResponse?.ErrorMessage ?? ""} {message ?? ""}" });
                                }

                                return Json(new { success = false, message = $"Error Message: {getPlanAPIResponse?.ErrorMessage ?? ""}" });
                            }

                        }
                        else
                        {
                            JObject jsonRespObj = JObject.Parse(jsonResponse);

                            if (jsonRespObj.ContainsKey("Message"))
                            {
                                string message = jsonRespObj["Message"]?.ToString();
                                return Json(new { success = false, message = $"Error Message: {message ?? ""}" });
                            }

                            return Json(new { success = false, message = $"Error Message: {jsonResponse ?? ""}" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string exception = ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? ex.Message;

                return Json(new { success = false, message = $"Server Error: {exception}" });
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetQuoteAsync(string policyNo, string insCompanyId, string finalizeAddonCode, string idv, string quotationPlan, string claimValue, string ncbValue, string noOfClaims, string lastClaimDate)
        {
            long cusId = Convert.ToInt32(Session["cusId"].ToString());
            long vehiId = Convert.ToInt32(Session["vehiId"].ToString());

            try
            {
                string URL = ConfigurationManager.AppSettings["GetQuote_ApiUrl"];
                string accessToken = await GenerateToken.GenerateTokenAsync();

                using (var db = new AutoSherDBContext())
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            int claim = 0;
                            int ncbPer = 0;
                            int noOfClaim = 0;
                            if (!string.IsNullOrEmpty(claimValue))
                            {
                                claim = int.Parse(claimValue);
                            }
                            if (!string.IsNullOrEmpty(ncbValue))
                            {
                                ncbPer = int.Parse(ncbValue);
                            }
                            if (!string.IsNullOrEmpty(noOfClaims))
                            {
                                noOfClaim = int.Parse(noOfClaims);
                            }

                            var userVehicleInsDetails = await db.vehicles.Where(m => m.vehicle_id == vehiId && m.customer_id == cusId).Select(m => new { m.CP_CODE, m.oemid, m.campaign_id }).FirstOrDefaultAsync();

                            int? policy_type = GetPolicyTypeBasedOnCampaign(userVehicleInsDetails?.campaign_id ?? null);

                            //string last_claim_date = !string.IsNullOrEmpty(lastClaimDate) ? Convert.ToDateTime(lastClaimDate).ToString("yyyy-MM-dd") : "";

                            var requestPayload = new
                            {
                                PreviousPolicyNo = policyNo ?? "",
                                IDV = !string.IsNullOrEmpty(idv) ? long.Parse(idv) : 0,
                                ADDONCode = finalizeAddonCode,
                                PLAN = quotationPlan,
                                ProductID = insCompanyId,
                                CPCode = userVehicleInsDetails?.CP_CODE ?? "",
                                OEMID = userVehicleInsDetails?.oemid ?? 0,
                                Claim = claim,
                                NCB_Per = ncbPer,
                                No_Of_Claim = noOfClaim,
                                Last_Claim_Date = lastClaimDate,
                                Type = policy_type
                            };

                            using (var httpClient = new HttpClient())
                            {
                                string jsonRequestPayload = JsonConvert.SerializeObject(requestPayload);
                                StringContent httpContent = new StringContent(jsonRequestPayload, Encoding.UTF8, "application/json");

                                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                                HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(URL, httpContent);

                                string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();

                                if (httpResponseMessage.IsSuccessStatusCode)
                                {
                                    List<QuotationAPIResponse> quotationResponse = new List<QuotationAPIResponse>();
                                    try
                                    {
                                        quotationResponse = JsonConvert.DeserializeObject<List<QuotationAPIResponse>>(jsonResponse);
                                    }
                                    catch (Exception ex)
                                    {
                                        if (jsonResponse.Contains("ErrorMessage") && jsonResponse.Contains("ErrorCode"))
                                        {
                                            string errorMessage = HandleJsonErrorResponse(jsonResponse);

                                            return Json(new { success = false, message = $"<span style='color:red;font-weight:bold;'>Error Message:</span> {errorMessage}, {ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? ex.Message}" }, JsonRequestBehavior.AllowGet);
                                        }

                                        return Json(new { success = false, message = $"<span style='color:red;font-weight:bold;'>Error Message:</span> Error while deserializing json object. {jsonResponse}, {ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? ex.Message}" }, JsonRequestBehavior.AllowGet);
                                    }

                                    if (quotationResponse != null)
                                    {
                                        var oldQuotations = db.quotationresponses.Where(m => m.customerId == cusId && m.vehicleid == vehiId).ToList();
                                        if (oldQuotations != null && oldQuotations.Count > 0)
                                        {
                                            db.quotationresponses.RemoveRange(oldQuotations);
                                            await db.SaveChangesAsync();
                                        }

                                        foreach (var data in quotationResponse)
                                        {
                                            Quotationresponse quoteResponse = new Quotationresponse();

                                            quoteResponse.customerId = cusId;
                                            quoteResponse.vehicleid = vehiId;
                                            quoteResponse.QuoteID = data.QuoteID;
                                            quoteResponse.ProposalID = data.ProposalID;
                                            quoteResponse.PreviousPolicyNo = data.PreviousPolicyNo;
                                            quoteResponse.InsuranceCompany = data.InsuranceCompany;
                                            quoteResponse.ProductId = data.ProductID;
                                            quoteResponse.BasicIDV = data.BasicIDV;
                                            quoteResponse.Basic = data.Basic;
                                            quoteResponse.NCBValue = data.NCBValue;
                                            quoteResponse.CPAAmount = data.CPAAmount;
                                            quoteResponse.OD = data.OD;
                                            quoteResponse.Liability = data.Liability;
                                            quoteResponse.ADDON = data.ADDON;
                                            quoteResponse.ADDONTOTAL = data.ADDONTOTAL;
                                            quoteResponse.TotalGST = data.TotalGST;
                                            quoteResponse.Gross = data.Gross;
                                            quoteResponse.CPCode = data.CPCode;
                                            quoteResponse.OEMID = data.OEMID;
                                            quoteResponse.Non_Electric_Acc = data.Non_Electric_Acc;
                                            quoteResponse.Electric_Acc = data.Electric_Acc;
                                            quoteResponse.Vehicle_Basic_Premium = data.Vehicle_Basic_Premium;
                                            quoteResponse.Sub_Total_Basic_Prem = data.Sub_Total_Basic_Prem;
                                            quoteResponse.PA_Cover_Owner_Driver = data.PA_Cover_Owner_Driver;
                                            quoteResponse.Sub_Total_Addition = data.Sub_Total_Addition;
                                            quoteResponse.Basic_Third_Party_Liability = data.Basic_Third_Party_Liability;
                                            quoteResponse.FinalQuoteStatus = "KYC PENDING";
                                            quoteResponse.FinalStatusUpdatedDateTime = DateTime.Now;
                                            quoteResponse.IsQuoteFailed = data.IsQuoteFailed;
                                            quoteResponse.QuoteFailedRemarks = data.Remark;

                                            db.quotationresponses.Add(quoteResponse);
                                        }
                                        await db.SaveChangesAsync();
                                        dbTransaction.Commit();

                                        string quotationStatus = db.quotationresponses.Where(m => m.vehicleid == vehiId && m.customerId == cusId).OrderByDescending(m => m.FinalStatusUpdatedDateTime).Select(m => m.FinalQuoteStatus).AsNoTracking().FirstOrDefault();

                                        return Json(new { success = true, status = quotationStatus ?? "", message = "Quote Generated Successfully." }, JsonRequestBehavior.AllowGet);
                                    }
                                    else
                                    {
                                        if (jsonResponse.Contains("ErrorMessage") && jsonResponse.Contains("ErrorCode"))
                                        {
                                            string errorMessage = HandleJsonErrorResponse(jsonResponse);

                                            return Json(new { success = false, message = $"<span style='color:red;font-weight:bold;'>Error Message:</span>  {errorMessage}" }, JsonRequestBehavior.AllowGet);
                                        }

                                        return Json(new { success = false, message = $"<span style='color:red;font-weight:bold;'>Error Message:</span> Quotation Data not found. {jsonResponse}" }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                else
                                {
                                    JObject jsonRespObj = JObject.Parse(jsonResponse);

                                    if (jsonRespObj.ContainsKey("Message"))
                                    {
                                        return Json(new { success = false, message = $"<span style='color:red;font-weight:bold;'>Error Message:</span> {jsonRespObj["Message"]}" }, JsonRequestBehavior.AllowGet);
                                    }

                                    return Json(new { success = false, message = $"<span style='color:red;font-weight:bold;'>Error Message:</span> {jsonResponse}" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();

                            string exception = ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? ex.Message;

                            return Json(new { success = false, message = $"<span style='color:red;font-weight:bold;'>Error Message:</span> <b>{exception}</b>" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string exception = ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? ex.Message;

                return Json(new { success = false, message = $"<span style='color:red;font-weight:bold;'>Error Message:</span> {exception}" });
            }
        }

        [HttpPost]
        public async Task<ActionResult> KYCVerificationAsync(string quoteId, string panNumber, string aadharNumber, string ckycNumber, string customerType)
        {
            string returnURL = Request.UrlReferrer?.ToString() ?? "";
            long cusId = Convert.ToInt32(Session["cusId"].ToString());
            long vehiId = Convert.ToInt32(Session["vehiId"].ToString());
            try
            {
                long rowId = 0;
                if (!string.IsNullOrEmpty(quoteId))
                {
                    rowId = long.Parse(quoteId);
                }

                string URL = ConfigurationManager.AppSettings["VerifyKYC_ApiUrl"];
                string accessToken = await GenerateToken.GenerateTokenAsync();

                using (var db = new AutoSherDBContext())
                {
                    Quotationresponse quoteData = await db.quotationresponses.Where(m => m.id == rowId && m.customerId == cusId && m.vehicleid == vehiId).FirstOrDefaultAsync();

                    if (quoteData != null)
                    {
                        string customerName = string.Empty;
                        string gender = string.Empty;
                        string dob = string.Empty;

                        var userVehicleInsDetails = await db.vehicles.Where(m => m.customer_id == cusId && m.vehicle_id == vehiId).FirstOrDefaultAsync();
                        var customerDetails = await db.customers.Where(m => m.id == cusId).FirstOrDefaultAsync();

                        if (customerDetails != null)
                        {
                            gender = customerDetails.gender;

                            if (!string.IsNullOrEmpty(customerDetails.dob) && customerDetails.dob != "0000-00-00")
                            {
                                dob = Convert.ToDateTime(customerDetails.dob).ToString("MM/dd/yyyy");
                            }
                        }

                        if (userVehicleInsDetails != null)
                        {
                            var firstName = userVehicleInsDetails.first_name?.Trim() ?? "";
                            var middleName = userVehicleInsDetails.Middle_Name?.Trim() ?? "";
                            var lastName = userVehicleInsDetails.last_name?.Trim() ?? "";

                            customerName = string.Join(" ", new[] { firstName, middleName, lastName }.Where(s => !string.IsNullOrEmpty(s)));
                        }

                        var requestBody = new
                        {
                            PreviousPolicyNumber = quoteData.PreviousPolicyNo,
                            CustomerType = customerType,
                            PANNo = panNumber,
                            Aadhar = aadharNumber,
                            DOB = dob,
                            DOI = userVehicleInsDetails?.doi,
                            CustomerName = customerName?.Trim(),
                            ProposalID = quoteData.ProposalID,
                            QuoteID = quoteData.QuoteID,
                            KYCNO = ckycNumber,
                            ICId = quoteData.ProductId,
                            Gender = gender,
                            Return_URL = returnURL
                        };

                        using (var httpClient = new HttpClient())
                        {
                            string jsonContent = JsonConvert.SerializeObject(requestBody);
                            StringContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(URL, httpContent);

                            string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();

                            if (httpResponseMessage.IsSuccessStatusCode)
                            {
                                KYCVerificationResponse kycResponse = new KYCVerificationResponse();
                                try
                                {
                                    if (jsonResponse.Contains("ErrorMessage") && jsonResponse.Contains("ErrorCode"))
                                    {
                                        string errorMessage = HandleJsonErrorResponse(jsonResponse);
                                        return Json(new { success = false, message = $"Error: {errorMessage} to verify KYC." }, JsonRequestBehavior.AllowGet);
                                    }

                                    try
                                    {
                                        kycResponse = JsonConvert.DeserializeObject<KYCVerificationResponse>(jsonResponse);
                                    }
                                    catch (Exception ex)
                                    {
                                        return Json(new { success = false, message = $"Error while deserializing json object. | {jsonResponse}" }, JsonRequestBehavior.AllowGet);
                                    }

                                    if (kycResponse == null)
                                    {
                                        return Json(new { success = false, message = "KYC Verify response is empty.." }, JsonRequestBehavior.AllowGet);
                                    }

                                    quoteData.CustomerName = kycResponse.CustomerName;
                                    quoteData.KYCStatus = kycResponse.Status;
                                    quoteData.DOB = kycResponse.DOB;
                                    quoteData.DOI = kycResponse.DOI;
                                    quoteData.Link = kycResponse.Link;
                                    quoteData.CustomerType = kycResponse.CustomerType;
                                    quoteData.KYCResponseProposalID = kycResponse.ProposalID;
                                    quoteData.KYCResponseQuoteID = kycResponse.QuoteID;
                                    quoteData.KYCID = kycResponse.KYCID;
                                    quoteData.KYCICId = kycResponse.ICId;
                                    if (kycResponse.Status == "Verified")
                                    {
                                        quoteData.FinalQuoteStatus = "INITIATE PAYMENT";
                                    }
                                    else if (kycResponse.Status == "Not Verified")
                                    {
                                        quoteData.FinalQuoteStatus = "NOT VERIFIED";
                                    }
                                    else
                                    {
                                        quoteData.FinalQuoteStatus = "";
                                    }
                                    quoteData.FinalStatusUpdatedDateTime = DateTime.Now;

                                    db.quotationresponses.AddOrUpdate(quoteData);
                                    await db.SaveChangesAsync();

                                    Quotationresponse kycVerifyQuoteData = await db.quotationresponses.Where(m => m.id == rowId && m.customerId == cusId && m.vehicleid == vehiId).FirstOrDefaultAsync();

                                    return Json(new { success = true, kycVerifyData = kycVerifyQuoteData, status = kycVerifyQuoteData?.FinalQuoteStatus ?? "" }, JsonRequestBehavior.AllowGet);
                                }
                                catch (Exception ex)
                                {
                                    string exception = ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? ex.Message;
                                    return Json(new { success = false, message = $"Error: {exception}" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            else
                            {
                                JObject jsonRespObj = JObject.Parse(jsonResponse);

                                if (jsonRespObj.ContainsKey("Message"))
                                {
                                    return Json(new { success = false, message = $"Error: {jsonRespObj["Message"]}" }, JsonRequestBehavior.AllowGet);
                                }

                                return Json(new { success = false, message = $"Error: {jsonResponse}" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        return Json(new { success = false, message = "ProposalID and QuoteID not found. Request not sent" });
                    }
                }
            }
            catch (Exception ex)
            {
                string exception = ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? ex.Message;

                return Json(new { success = false, message = $"Server Error: {exception}" });
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateKYCVerificationStatusAsync(string quoteId)
        {
            long cusId = Convert.ToInt32(Session["cusId"].ToString());
            long vehiId = Convert.ToInt32(Session["vehiId"].ToString());
            try
            {
                long quoteRowId = 0;
                if (!string.IsNullOrEmpty(quoteId))
                {
                    quoteRowId = long.Parse(quoteId);
                }

                string URL = ConfigurationManager.AppSettings["UpdateKYC_ApiUrl"];
                string accessToken = await GenerateToken.GenerateTokenAsync();

                using (var db = new AutoSherDBContext())
                {
                    Quotationresponse quoteData = await db.quotationresponses.Where(m => m.id == quoteRowId && m.customerId == cusId && m.vehicleid == vehiId).FirstOrDefaultAsync();

                    if (quoteData?.KYCResponseProposalID != null)
                    {
                        var requestBody = new
                        {
                            ProposalID = quoteData.KYCResponseProposalID
                        };

                        using (var httpClient = new HttpClient())
                        {
                            string jsonContent = JsonConvert.SerializeObject(requestBody);
                            StringContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(URL, httpContent);

                            string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();

                            if (httpResponseMessage.IsSuccessStatusCode)
                            {
                                try
                                {
                                    dynamic kycStatusResponse = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

                                    if (kycStatusResponse != null)
                                    {
                                        quoteData.KYCStatus = kycStatusResponse.Status;
                                        if (kycStatusResponse.Status?.ToString() == "Verified")
                                        {
                                            quoteData.FinalQuoteStatus = "INITIATE PAYMENT";
                                        }
                                        else if (kycStatusResponse.Status?.ToString() == "Not Verified")
                                        {
                                            quoteData.FinalQuoteStatus = "NOT VERIFIED";
                                        }
                                        else
                                        {
                                            quoteData.FinalQuoteStatus = "";
                                        }
                                        quoteData.FinalStatusUpdatedDateTime = DateTime.Now;

                                        db.quotationresponses.AddOrUpdate(quoteData);
                                        await db.SaveChangesAsync();
                                    }

                                    Quotationresponse kycVerifyStatusQuoteData = await db.quotationresponses.Where(m => m.id == quoteRowId && m.customerId == cusId && m.vehicleid == vehiId).FirstOrDefaultAsync();

                                    return Json(new { success = true, kycVerifyStatusData = kycVerifyStatusQuoteData, status = kycVerifyStatusQuoteData?.FinalQuoteStatus ?? "" }, JsonRequestBehavior.AllowGet);
                                }
                                catch (Exception ex)
                                {
                                    return Json(new { success = false, message = $"Error while deserializing json object. {jsonResponse}" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            else
                            {
                                JObject jsonRespObj = JObject.Parse(jsonResponse);

                                if (jsonRespObj.ContainsKey("Message"))
                                {
                                    return Json(new { success = false, message = $"Error: {jsonRespObj["Message"]}" }, JsonRequestBehavior.AllowGet);
                                }

                                return Json(new { success = false, message = $"Error: {jsonResponse}" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        return Json(new { success = false, message = "KYC ID not found. Please verify KYC." });
                    }
                }
            }
            catch (Exception ex)
            {
                string exception = ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? ex.Message;

                return Json(new { success = false, message = $"Server Error: {exception}" });
            }
        }

        [HttpPost]
        public async Task<ActionResult> InitiateOnlinePaymentAsync(string quoteId, string vehiclRegNo, string vehicleRegDate, string policyRenEmail, string policyRenMobileNo, string policyRenNomineeName, string policyRenNomineeAge, string policyRenNomineeRelationship)
        {
            long cusId = Convert.ToInt32(Session["cusId"].ToString());
            long vehiId = Convert.ToInt32(Session["vehiId"].ToString());

            try
            {
                long quoteRowId = 0;
                if (!string.IsNullOrEmpty(quoteId))
                {
                    quoteRowId = long.Parse(quoteId);
                }

                string URL = ConfigurationManager.AppSettings["InitiateOnlinePayment_ApiUrl"];
                string accessToken = await GenerateToken.GenerateTokenAsync();

                using (var db = new AutoSherDBContext())
                {
                    Quotationresponse quoteData = await db.quotationresponses.Where(m => m.id == quoteRowId && m.customerId == cusId && m.vehicleid == vehiId).FirstOrDefaultAsync();

                    if (quoteData != null)
                    {
                        vehicle userVehicleData = await db.vehicles.Where(m => m.vehicle_id == vehiId && m.customer_id == cusId).FirstOrDefaultAsync();
                        string phoneNumber = await db.phones.Where(m => m.customer_id == cusId).OrderByDescending(m => m.isPreferredPhone == true).Select(m => m.phoneNumber).FirstOrDefaultAsync();

                        string registrationNumber = string.Empty;
                        string registrationDate = string.Empty;

                        if (!string.IsNullOrEmpty(vehiclRegNo))
                        {
                            registrationNumber = VehicleRegNumberFormat(vehiclRegNo);
                        }

                        if (!string.IsNullOrEmpty(vehicleRegDate))
                        {
                            registrationDate = Convert.ToDateTime(vehicleRegDate).ToString("yyyy-MM-dd");
                        }

                        int? policy_type = GetPolicyTypeBasedOnCampaign(userVehicleData?.campaign_id ?? null);

                        var requestBody = new
                        {
                            ProposalId = quoteData.KYCResponseProposalID ?? "",
                            ICId = quoteData.ProductId,
                            GrossPremium = quoteData.Gross,
                            Registration_Number = registrationNumber,
                            Registration_Date = registrationDate,
                            Email_Address = policyRenEmail ?? userVehicleData?.email ?? "",
                            Phone_Number = policyRenMobileNo ?? phoneNumber ?? "",
                            Nominee_Name = policyRenNomineeName ?? userVehicleData?.Nominee_Name ?? "",
                            Nominee_Age = !string.IsNullOrEmpty(policyRenNomineeAge) ? int.Parse(policyRenNomineeAge) : userVehicleData?.Nominee_Age ?? 0,
                            Nominee_Relationship = policyRenNomineeRelationship ?? userVehicleData?.Nominee_Relationship ?? "",
                            Type = policy_type
                        };

                        using (var httpClient = new HttpClient())
                        {
                            string jsonContent = JsonConvert.SerializeObject(requestBody);
                            StringContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(URL, httpContent);

                            string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();

                            if (httpResponseMessage.IsSuccessStatusCode)
                            {
                                if (jsonResponse.Contains("Link Sent"))
                                {
                                    quoteData.IsPaymentLinkSent = "Sent";
                                    quoteData.PayLinkSentDateTime = DateTime.Now;
                                    quoteData.FinalQuoteStatus = "PENDING PAYMENT";
                                    quoteData.FinalStatusUpdatedDateTime = DateTime.Now;

                                    db.quotationresponses.AddOrUpdate(quoteData);
                                    await db.SaveChangesAsync();

                                    string quotationStatus = await db.quotationresponses.Where(m => m.id == quoteRowId && m.vehicleid == vehiId && m.customerId == cusId).Select(m => m.FinalQuoteStatus).FirstOrDefaultAsync();

                                    return Json(new { success = true, paymentLinkStatus = "Success", message = $"{jsonResponse}", status = quotationStatus ?? "" }, JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    if (jsonResponse.Contains("ErrorMessage") && jsonResponse.Contains("ErrorCode"))
                                    {
                                        string errorMessage = HandleJsonErrorResponse(jsonResponse);
                                        return Json(new { success = false, message = $"Error: {errorMessage}" }, JsonRequestBehavior.AllowGet);
                                    }

                                    quoteData.IsPaymentLinkSent = "Not Sent";
                                    quoteData.FinalQuoteStatus = "PRE-VALIDATION FAILED";
                                    quoteData.FinalStatusUpdatedDateTime = DateTime.Now;

                                    db.quotationresponses.AddOrUpdate(quoteData);
                                    await db.SaveChangesAsync();

                                    string quotationStatus = await db.quotationresponses.Where(m => m.id == quoteRowId && m.vehicleid == vehiId && m.customerId == cusId).Select(m => m.FinalQuoteStatus).FirstOrDefaultAsync();

                                    return Json(new { success = true, paymentLinkStatus = "Failed", message = $"Payment Link not sent. Error: {jsonResponse}", status = quotationStatus ?? "" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            else
                            {
                                JObject jsonRespObj = JObject.Parse(jsonResponse);

                                if (jsonRespObj.ContainsKey("Message"))
                                {
                                    return Json(new { success = false, message = $"Error: {jsonRespObj["Message"]}" }, JsonRequestBehavior.AllowGet);
                                }

                                return Json(new { success = false, message = $"Error: {jsonResponse}" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        return Json(new { success = false, message = "Quotation data not found. Request not sent" });
                    }
                }
            }
            catch (Exception ex)
            {
                string exception = ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? ex.Message;

                return Json(new { success = false, message = $"Server Error: {exception}" });
            }
        }

        [HttpPost]
        public async Task<ActionResult> OnlinePaymentVerificationAsync(string quoteId)
        {
            long cusId = Convert.ToInt32(Session["cusId"].ToString());
            long vehiId = Convert.ToInt32(Session["vehiId"].ToString());
            long wyzUserId = Convert.ToInt32(Session["UserId"].ToString());

            try
            {
                long quoteRowId = 0;
                if (!string.IsNullOrEmpty(quoteId))
                {
                    quoteRowId = long.Parse(quoteId);
                }

                string URL = ConfigurationManager.AppSettings["UpdateOnlinePayment_ApiUrl"];
                string accessToken = await GenerateToken.GenerateTokenAsync();

                using (var db = new AutoSherDBContext())
                {
                    Quotationresponse quoteData = await db.quotationresponses.Where(m => m.id == quoteRowId && m.customerId == cusId && m.vehicleid == vehiId).FirstOrDefaultAsync();

                    if (quoteData?.KYCResponseProposalID != null && quoteData?.KYCResponseQuoteID != null)
                    {
                        var requestBody = new
                        {
                            ProposalId = quoteData.KYCResponseProposalID,
                            QuotationId = quoteData.KYCResponseQuoteID
                        };

                        using (var httpClient = new HttpClient())
                        {
                            string jsonContent = JsonConvert.SerializeObject(requestBody);
                            StringContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(URL, httpContent);

                            string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();

                            if (httpResponseMessage.IsSuccessStatusCode)
                            {
                                try
                                {
                                    List<PaymentStatusAPIResponse> paymentStatusResponse = JsonConvert.DeserializeObject<List<PaymentStatusAPIResponse>>(jsonResponse);

                                    if (paymentStatusResponse != null && paymentStatusResponse.Count > 0)
                                    {
                                        foreach (var paymentStatus in paymentStatusResponse)
                                        {
                                            quoteData.PaymentDoneDate = paymentStatus.Payment_Date ?? "";
                                            quoteData.PaymentStatus = paymentStatus.Status ?? "";
                                            quoteData.TransactionReferenceNumber = paymentStatus.Transaction_Reference_Number ?? "";

                                            if (paymentStatus.Status == "Success")
                                            {
                                                quoteData.FinalQuoteStatus = "PAYMENT SUCCESS";
                                            }
                                            else if (paymentStatus.Status == "Failed")
                                            {
                                                quoteData.FinalQuoteStatus = "PAYMENT EXPIRED";
                                            }
                                            else
                                            {
                                                quoteData.FinalQuoteStatus = "";
                                            }
                                            quoteData.FinalStatusUpdatedDateTime = DateTime.Now;

                                            db.quotationresponses.AddOrUpdate(quoteData);

                                            PaymentStatusResponse paymentconfirmation = new PaymentStatusResponse
                                            {
                                                CustomerId = cusId,
                                                VehicleId = vehiId,
                                                WyzuserId = wyzUserId,
                                                QuoteID = quoteData.KYCResponseQuoteID ?? "",
                                                ProposalID = quoteData.KYCResponseProposalID ?? "",
                                                CPCode = quoteData.CPCode ?? "",
                                                OEMID = quoteData.OEMID.ToString() ?? "",
                                                PaymentDoneDate = paymentStatus.Payment_Date ?? "",
                                                TransactionAmount = paymentStatus.Transaction_Amount ?? "",
                                                TransactionReferenceNumber = paymentStatus.Transaction_Reference_Number ?? "",
                                                Status = paymentStatus.Status ?? "",
                                                NewPolicyNumber = paymentStatus.POLICY_NO ?? "",
                                                PreviousPolicyNumber = paymentStatus.Previous_Policy_No ?? "",
                                                ChassisNumber = paymentStatus.Chassis_Number ?? "",
                                                EngineNumber = paymentStatus.Engine_Number ?? "",
                                                UpdatedDate = paymentStatus.Updated_Date ?? "",
                                                UpdatedTime = paymentStatus.Updated_Time ?? ""
                                            };

                                            db.PaymentStatuses.Add(paymentconfirmation);
                                        }
                                        await db.SaveChangesAsync();

                                        Quotationresponse payStatusQuoteData = await db.quotationresponses.Where(m => m.id == quoteRowId && m.customerId == cusId && m.vehicleid == vehiId).OrderByDescending(m => m.id).FirstOrDefaultAsync();

                                        return Json(new { success = true, payStatusQuoteData = payStatusQuoteData, status = payStatusQuoteData?.FinalQuoteStatus ?? "", message = "Payment verified success." }, JsonRequestBehavior.AllowGet);
                                    }
                                    else
                                    {
                                        return Json(new { success = false, message = "Payment not done. Payment status response is empty." }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    string exception = ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? ex.Message;
                                    return Json(new { success = false, message = $"Error while deserializing json object. {jsonResponse}, {exception}" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            else
                            {
                                JObject jsonRespObj = JObject.Parse(jsonResponse);

                                if (jsonRespObj.ContainsKey("Message"))
                                {
                                    return Json(new { success = false, message = $"Error: {jsonRespObj["Message"]}" }, JsonRequestBehavior.AllowGet);
                                }

                                return Json(new { success = false, message = $"Error: {jsonResponse}" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        return Json(new { success = false, message = "Proposal or Quote ID is empty. Payment verify request not sent" });
                    }
                }
            }
            catch (Exception ex)
            {
                string exception = ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? ex.Message;

                return Json(new { success = false, message = $"Server Error: {exception}" });
            }
        }

        [HttpPost]
        public async Task<ActionResult> UploadWinbackPolicyDetailsAsync(string policyWinbackRolloverDetails)
        {
            if (policyWinbackRolloverDetails == null)
            {
                return Json(new { success = false, message = "Policy data is empty, Winback upload request not sent." }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                WinbackRolloverPolicyVM winbackRolloverPolicyDetails = JsonConvert.DeserializeObject<WinbackRolloverPolicyVM>(policyWinbackRolloverDetails);
                if (winbackRolloverPolicyDetails != null)
                {
                    #region Request data conversion according to APIs requirement

                    int.TryParse(winbackRolloverPolicyDetails.RenewalType, out int renewalType);
                    int.TryParse(winbackRolloverPolicyDetails.ProductID, out int productId);
                    int.TryParse(winbackRolloverPolicyDetails.CustomerResidentState, out int custResidentState);
                    int.TryParse(winbackRolloverPolicyDetails.City, out int city);
                    long.TryParse(winbackRolloverPolicyDetails.Pincode, out long pincode);
                    int.TryParse(winbackRolloverPolicyDetails.Make, out int makeId);
                    int.TryParse(winbackRolloverPolicyDetails.Model, out int modelId);
                    int.TryParse(winbackRolloverPolicyDetails.Variant, out int variantId);
                    long.TryParse(winbackRolloverPolicyDetails.SeatingCapacity, out long seatingCapacity);
                    long.TryParse(winbackRolloverPolicyDetails.CubicCapacity, out long cubicCapacity);
                    decimal.TryParse(winbackRolloverPolicyDetails.BatteryCapacity, out decimal batteryCapacity);
                    int.TryParse(winbackRolloverPolicyDetails.YearOfManufacture, out int mfgYear);
                    int.TryParse(winbackRolloverPolicyDetails.RTO, out int rtoId);
                    long.TryParse(winbackRolloverPolicyDetails.ExShowroomPrice, out long exShowRoomPrice);
                    long.TryParse(winbackRolloverPolicyDetails.VoluntaryDisc, out long voluntaryDisc);
                    int.TryParse(winbackRolloverPolicyDetails.AntiTheft, out int isAntiTheft);
                    int.TryParse(winbackRolloverPolicyDetails.AAIMembership, out int aaiMembership);
                    int.TryParse(winbackRolloverPolicyDetails.PaidDriver, out int paidDriver);
                    int.TryParse(winbackRolloverPolicyDetails.UnnamedPassenger, out int unnamedPassenger);
                    long.TryParse(winbackRolloverPolicyDetails.CoverAmount, out long coverAmt);
                    int.TryParse(winbackRolloverPolicyDetails.LegalLiabilityPaidDriver, out int llDriver);
                    int.TryParse(winbackRolloverPolicyDetails.IsOtherEmployee, out int otherEmployee);
                    int.TryParse(winbackRolloverPolicyDetails.OtherEmployeeCount, out int otherEmployeeCount);
                    int.TryParse(winbackRolloverPolicyDetails.IMT23, out int imt23);
                    int.TryParse(winbackRolloverPolicyDetails.IsHandicapped, out int isHandicapped);
                    int.TryParse(winbackRolloverPolicyDetails.NCBSlabPer, out int ncbSlabPer);
                    int.TryParse(winbackRolloverPolicyDetails.CPATenure, out int cpaTenure);
                    long.TryParse(winbackRolloverPolicyDetails.IDV, out long idv);
                    long.TryParse(winbackRolloverPolicyDetails.ElectricalAccessoriesPrice, out long eleAccessoriesPrice);
                    long.TryParse(winbackRolloverPolicyDetails.NonElectricalAccessoriesPrice, out long nonEleAccessoriesPrice);
                    long.TryParse(winbackRolloverPolicyDetails.BiFuelKitPrice, out long biFuelKitPrice);
                    int.TryParse(winbackRolloverPolicyDetails.OEMID, out int oemId);
                    int.TryParse(winbackRolloverPolicyDetails.Claim, out int isClaim);
                    int.TryParse(winbackRolloverPolicyDetails.ClaimCount, out int claimCount);
                    int.TryParse(winbackRolloverPolicyDetails.TransferCase, out int isTransfer);
                    int.TryParse(winbackRolloverPolicyDetails.NDCover, out int isNDCover);
                    int.TryParse(winbackRolloverPolicyDetails.RTICover, out int isRTICover);
                    int.TryParse(winbackRolloverPolicyDetails.LastYearEP, out int isLastYearEP);
                    int.TryParse(winbackRolloverPolicyDetails.BHRegistration, out int isBhRegistration);

                    var policyEffectiveDate = !string.IsNullOrEmpty(winbackRolloverPolicyDetails.PolicyStartDate)
                        ? Convert.ToDateTime(winbackRolloverPolicyDetails.PolicyStartDate).ToString("yyyy-MM-dd HH:mm:ss.fff")
                        : DateTime.MinValue.ToString("yyyy-MM-dd HH:mm:ss.fff");

                    var policyExpiryDate = !string.IsNullOrEmpty(winbackRolloverPolicyDetails.PolicyEndDate)
                        ? Convert.ToDateTime(winbackRolloverPolicyDetails.PolicyEndDate).ToString("yyyy-MM-dd HH:mm:ss.fff")
                        : DateTime.MinValue.ToString("yyyy-MM-dd HH:mm:ss.fff");

                    var tpPolicyEffectiveDate = !string.IsNullOrEmpty(winbackRolloverPolicyDetails.TTPolicyStartDate)
                        ? Convert.ToDateTime(winbackRolloverPolicyDetails.TTPolicyStartDate).ToString("yyyy-MM-dd HH:mm:ss.fff")
                        : DateTime.MinValue.ToString("yyyy-MM-dd HH:mm:ss.fff");

                    var tpPolicyExpiryDate = !string.IsNullOrEmpty(winbackRolloverPolicyDetails.TTPolicyEndDate)
                        ? Convert.ToDateTime(winbackRolloverPolicyDetails.TTPolicyEndDate).ToString("yyyy-MM-dd HH:mm:ss.fff")
                        : DateTime.MinValue.ToString("yyyy-MM-dd HH:mm:ss.fff");

                    var dob = !string.IsNullOrEmpty(winbackRolloverPolicyDetails.DOB) ? Convert.ToDateTime(winbackRolloverPolicyDetails.DOB).ToString("yyyy-MM-dd") : null;

                    var doi = !string.IsNullOrEmpty(winbackRolloverPolicyDetails.DOI) ? Convert.ToDateTime(winbackRolloverPolicyDetails.DOI).ToString("yyyy-MM-dd") : null;

                    var invoiceDate = !string.IsNullOrEmpty(winbackRolloverPolicyDetails.InvoiceDate) ? Convert.ToDateTime(winbackRolloverPolicyDetails.InvoiceDate).ToString("yyyy-MM-dd") : null;

                    var registrationDate = !string.IsNullOrEmpty(winbackRolloverPolicyDetails.RegistrationDate) ? Convert.ToDateTime(winbackRolloverPolicyDetails.RegistrationDate).ToString("yyyy-MM-dd") : null;

                    var lastClaimDate = !string.IsNullOrEmpty(winbackRolloverPolicyDetails.LastClaimDate) ? Convert.ToDateTime(winbackRolloverPolicyDetails.LastClaimDate).ToString("yyyy-MM-dd") : null;

                    string registrationNo = VehicleRegNumberFormat(winbackRolloverPolicyDetails.RegistrationNo);
                    string nomineeName = null;
                    string nomineeRelation = null;
                    string address2 = null;
                    int nomineeAge = 0;

                    #endregion

                    var requestPayload = new
                    {
                        POLICY_NO = winbackRolloverPolicyDetails.PreviousPolicyNo,
                        RENEWAL_TYPE = renewalType,
                        POLICY_TYPE = winbackRolloverPolicyDetails.PolicyType,
                        POLICY_EFFECTIVE_DATE = policyEffectiveDate,
                        POLICY_EXPIRY_DATE = policyExpiryDate,
                        TPPOLICY_EFFECTIVE_DATE = tpPolicyEffectiveDate,
                        TPPOLICY_EXPIRY_DATE = tpPolicyExpiryDate,
                        FKPRODUCT_ID = productId,
                        CPCODE = winbackRolloverPolicyDetails.CPOrDealerCode,
                        PROPOSER_TYPE = winbackRolloverPolicyDetails.ProposerType,
                        SALUTATION = winbackRolloverPolicyDetails.Salutation,
                        FIRST_NAME = winbackRolloverPolicyDetails.FirstName,
                        MIDDLE_NAME = winbackRolloverPolicyDetails.MiddleName,
                        LAST_NAME = winbackRolloverPolicyDetails.LastName,
                        GENDER = winbackRolloverPolicyDetails.Gender,
                        ADD1 = winbackRolloverPolicyDetails.Address,
                        ADD2 = address2,
                        FKSTATE_ID = custResidentState,
                        FKCITY_ID = city,
                        PIN = pincode,
                        MOB_NO = winbackRolloverPolicyDetails.PhoneNumber,
                        EMAIL = winbackRolloverPolicyDetails.EmailAddress,
                        PROP_DOB = dob,
                        COMPANY_SALUTATION = winbackRolloverPolicyDetails.CompanySalutation,
                        COMPANY_NAME = winbackRolloverPolicyDetails.CompanyName,
                        DESIGNATION = winbackRolloverPolicyDetails.Designation,
                        DOI = doi,
                        VEHICLE_CLASS = winbackRolloverPolicyDetails.VehicleClass,
                        CHASSIS_NO = winbackRolloverPolicyDetails.ChassisNo,
                        ENGINE_NO = winbackRolloverPolicyDetails.EngineNo,
                        FKMAKE_ID = makeId,
                        FKMODEL_ID = modelId,
                        FKVARIANT_ID = variantId,
                        SEATING_CAPACITY = seatingCapacity,
                        CUBIC_CAPACITY = cubicCapacity,
                        FUEL_TYPE = winbackRolloverPolicyDetails.FuelType,
                        KILOWATT = batteryCapacity,
                        MFG_YEAR = mfgYear,
                        INVOICE_DATE = invoiceDate,
                        FKRTO_ID = rtoId,
                        VEH_REGIST_NO = registrationNo ?? "",
                        REGISTRATION_DATE = registrationDate,
                        EXSHOWROOM_PRICE = exShowRoomPrice,
                        VOLUNTARY_DISC = voluntaryDisc,
                        ISANTI_THEFT_DEVICE = isAntiTheft,
                        ISAA_MEMBERSHIP = aaiMembership,
                        ISPAID_DRIVER = paidDriver,
                        ISUNNAMED_PASS = unnamedPassenger,
                        COVER_AMOUNT = coverAmt,
                        ISLL_DRIVER = llDriver,
                        ISOTHER_EMP = otherEmployee,
                        OTHER_EMP_COUNT = otherEmployeeCount,
                        ISIMT23 = imt23,
                        IS_HANDICAPPED = isHandicapped,
                        GEO_AREA = winbackRolloverPolicyDetails.GeoArea,
                        NCB_SLAB_PER = ncbSlabPer,
                        CPA_TENURE = cpaTenure,
                        NOMINEE_NAME = nomineeName,
                        NOMINEE_AGE = nomineeAge,
                        NOMINEE_RELATION = nomineeRelation,
                        COVER_TYPE = winbackRolloverPolicyDetails.CoverType,
                        VEH_IDV = idv,
                        VEH_TYPE_CODE = winbackRolloverPolicyDetails.VehicleType,
                        BIOFUEL_KIT_PRICE = biFuelKitPrice,
                        ELECTRICAL_ACC_PRICE = eleAccessoriesPrice,
                        NON_ELECTRICAL_ACC_PRICE = nonEleAccessoriesPrice,
                        OEMID = oemId,
                        PAN_NO = winbackRolloverPolicyDetails.PanNo,
                        AADHAR_CARD_NO = winbackRolloverPolicyDetails.AadharNo,
                        CLAIM = isClaim,
                        NO_OF_CLAIM = claimCount,
                        LAST_CLAIM_DATE = lastClaimDate,
                        TP_POLICY_NO = winbackRolloverPolicyDetails.TPPolicyNo,
                        IS_TRANSFER = isTransfer,
                        ISLASTYEAR_ZERODEP = isNDCover,
                        ISLASTYEAR_RTI = isRTICover,
                        ISLASTYEAR_EP = isLastYearEP,
                        ISBH_REGISTRATION = isBhRegistration
                    };

                    string URL = ConfigurationManager.AppSettings["UploadWinbackPolicy_ApiUrl"];
                    string accessToken = await GenerateToken.GenerateTokenAsync();

                    using (HttpClient httpClient = new HttpClient())
                    {
                        string jsonRequestPayload = JsonConvert.SerializeObject(requestPayload);
                        StringContent content = new StringContent(jsonRequestPayload, Encoding.UTF8, "application/json");

                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                        HttpResponseMessage httpResponse = await httpClient.PostAsync(URL, content);

                        string jsonResponse = await httpResponse.Content.ReadAsStringAsync();

                        if (httpResponse.IsSuccessStatusCode)
                        {
                            if (jsonResponse.Contains("DETAILS HAS BEEN UPDATED FOR THIS POLICY"))
                            {
                                return Json(new { success = true, message = "Policy Details Uploaded Successfully.!" }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                if (jsonResponse.Contains("ErrorMessage") && jsonResponse.Contains("ErrorCode"))
                                {
                                    string errorMessage = HandleJsonErrorResponse(jsonResponse);

                                    if (errorMessage != null)
                                    {
                                        return Json(new { success = false, message = $"<b><span style='color:red;'>ERROR MESSAGE:</span></b> <b>{errorMessage}</b>" }, JsonRequestBehavior.AllowGet);
                                    }

                                    return Json(new { success = false, message = $"<b><span style='color:red;'>ERROR MESSAGE:</span></b> <b>{jsonResponse}</b>" }, JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    return Json(new { success = false, message = $"<b><span style='color:red;'>ERROR MESSAGE:</span></b> <b>{jsonResponse}</b>" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                        else
                        {
                            JObject jsonRespObj = JObject.Parse(jsonResponse);

                            if (jsonRespObj.ContainsKey("Message"))
                            {
                                return Json(new { success = false, message = $"<b><span style='color:red;'>ERROR MESSAGE:</span></b> <b>{jsonRespObj["Message"]}</b>" }, JsonRequestBehavior.AllowGet);
                            }

                            return Json(new { success = false, message = $"<b><span style='color:red;'>ERROR MESSAGE:</span></b> <b>{jsonResponse}</b>" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    return Json(new { success = false, message = $"<b><span style='color:red;'>ERROR MESSAGE:</span></b> <b>Policy Data is empty, Policy upload request not sent</b>" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                string exception = ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? ex.Message;
                return Json(new { success = false, message = $"<b><span style='color:red;'>SERVER ERROR MESSAGE:</span></b> <b>{exception}</b>" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetMakeModelVariantFromAPIAsync()
        {
            try
            {
                long cusId = Convert.ToInt32(Session["cusId"].ToString());
                long vehiId = Convert.ToInt32(Session["vehiId"].ToString());

                string URL = ConfigurationManager.AppSettings["GetVariant_ApiUrl"].ToString();
                string accessToken = await GenerateToken.GenerateTokenAsync();

                using (var db = new AutoSherDBContext())
                {
                    var requestData = await db.vehicles.Where(m => m.customer_id == cusId && m.vehicle_id == vehiId).Select(m => new { m.CP_CODE, m.FKOEM_ID }).FirstOrDefaultAsync();
                    var requestPayload = new
                    {
                        DEALER_CODE = requestData.CP_CODE ?? "",
                        FKOEM_ID = requestData.FKOEM_ID ?? 0
                    };

                    using (var client = new HttpClient())
                    {
                        string jsonRequestPayload = JsonConvert.SerializeObject(requestPayload);
                        StringContent content = new StringContent(jsonRequestPayload, Encoding.UTF8, "application/json");

                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                        HttpResponseMessage responseContent = await client.PostAsync(URL, content);
                        string jsonResponseContent = await responseContent.Content.ReadAsStringAsync();

                        if (responseContent.IsSuccessStatusCode)
                        {
                            if (jsonResponseContent.Contains("ErrorCode") && jsonResponseContent.Contains("ErrorMessage"))
                            {
                                string errorMessage = HandleJsonErrorResponse(jsonResponseContent);
                                return Json(new { success = false, message = $"Error: {errorMessage}" }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                List<ModelVariantVM> responseDataObject = JsonConvert.DeserializeObject<List<ModelVariantVM>>(jsonResponseContent);

                                var uniqueMakes = responseDataObject.Select(c => new { c.FKMAKE_ID, c.Make_Name }).Distinct().ToList();
                                var uniqueModels = responseDataObject.Select(c => new { c.FKMODEL_ID, c.MODEL_CODE }).Distinct().ToList();
                                var uniqueVariants = responseDataObject.Select(c => new { c.VARIANT_ID, c.VARIANT_CODE }).Distinct().ToList();


                                if (responseDataObject.Count > 0 && responseDataObject != null)
                                {
                                    var defaultValues = await db.vehicles.Where(x => x.customer_id == cusId & x.vehicle_id == vehiId).Select(x => new { x.model, x.variant, x.oemid, x.campaign_id }).FirstOrDefaultAsync();

                                    if(defaultValues.campaign_id == 34)
                                    {
                                        var makeValue = await db.brands.Where(x => x.fksuboem_id == defaultValues.oemid).Select(x => x.brandname.ToUpper()).FirstOrDefaultAsync();

                                        var makeDefaultValue = uniqueMakes.Where(m => m.Make_Name.ToUpper() == makeValue).Select(m => m.FKMAKE_ID).FirstOrDefault();
                                        var modeltDefaultValue = uniqueModels.Where(m => m.MODEL_CODE.ToUpper() == defaultValues.model.ToUpper()).Select(m => m.FKMODEL_ID).FirstOrDefault();
                                        var variantDefaultValue = uniqueVariants.Where(m => m.VARIANT_CODE.ToUpper() == defaultValues.variant.ToUpper()).Select(m => m.VARIANT_ID).FirstOrDefault();

                                        return Json(new { success = true, makeLists = uniqueMakes, modelLists = uniqueModels, variantLists = uniqueVariants, makeDefaultValue = makeDefaultValue, modelDefaultValue = modeltDefaultValue, variantDefaultValues = variantDefaultValue, message = "Make, Model and Variant Data fetched successfully." }, JsonRequestBehavior.AllowGet);
                                    }

                                    return Json(new { success = true, makeLists = uniqueMakes, modelLists = uniqueModels, variantLists = uniqueVariants, makeDefaultValue = "", modelDefaultValue = "", variantDefaultValues = "", message = "Make, Model and Variant Data fetched successfully." }, JsonRequestBehavior.AllowGet);
                                }
                                return Json(new { success = false, data = "", message = "Make, Model and Variant Data not found. Response is empty" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            JObject jsonDataObj = JObject.Parse(jsonResponseContent);

                            if (jsonDataObj.ContainsKey("Message"))
                            {
                                return Json(new { success = false, message = $"Response not success, Error Message: {jsonDataObj["Message"]}" }, JsonRequestBehavior.AllowGet);
                            }

                            return Json(new { success = false, message = $"Response not success, Error Message: {jsonResponseContent}" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string exception = ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? ex.Message;
                return Json(new { success = false, message = $"Server Error: {exception}" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetRTODetailsFromAPIAsync()
        {
            try
            {
                long cusId = Convert.ToInt32(Session["cusId"].ToString());
                long vehiId = Convert.ToInt32(Session["vehiId"].ToString());

                string URL = ConfigurationManager.AppSettings["GetRTOData_ApiUrl"].ToString();
                string accessToken = await GenerateToken.GenerateTokenAsync();

                using (var db = new AutoSherDBContext())
                {
                    var requestData = await db.vehicles.Where(m => m.customer_id == cusId && m.vehicle_id == vehiId).Select(m => new { m.DealerId, m.FKOEM_ID, m.fkRTO_Id, m.campaign_id }).FirstOrDefaultAsync();
                    var requestPayload = new
                    {
                        FKOEM_ID = requestData.FKOEM_ID ?? 0,
                        FKDealer_ID = requestData.DealerId ?? 0
                    };

                    using (var client = new HttpClient())
                    {
                        string jsonRequestPayload = JsonConvert.SerializeObject(requestPayload);
                        StringContent content = new StringContent(jsonRequestPayload, Encoding.UTF8, "application/json");

                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                        HttpResponseMessage responseContent = await client.PostAsync(URL, content);
                        string jsonResponseContent = await responseContent.Content.ReadAsStringAsync();

                        if (responseContent.IsSuccessStatusCode)
                        {
                            if (jsonResponseContent.Contains("ErrorCode") && jsonResponseContent.Contains("ErrorMessage"))
                            {
                                string errorMessage = HandleJsonErrorResponse(jsonResponseContent);
                                return Json(new { success = false, message = $"Error: {errorMessage}" }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                List<RTODataVM> responseDataObject = JsonConvert.DeserializeObject<List<RTODataVM>>(jsonResponseContent);

                                if (responseDataObject.Count > 0 && responseDataObject != null)
                                {
                                    if(requestData.campaign_id == 34)
                                    {
                                        return Json(new { success = true, rtoList = responseDataObject, defaultRTOId = requestData.fkRTO_Id, message = "RTO Data fetched successfully." }, JsonRequestBehavior.AllowGet);
                                    }
                                    //return Json(new { success = true, rtoList = responseDataObject, defaultRTOId = "", message = "RTO Data fetched successfully." }, JsonRequestBehavior.AllowGet);

                                    var jsonResult = Json(new
                                    {
                                        success = true,
                                        rtoList = responseDataObject,
                                        defaultRTOId = "",
                                        message = "RTO Data fetched successfully."
                                    }, JsonRequestBehavior.AllowGet);

                                    jsonResult.MaxJsonLength = int.MaxValue;

                                    return jsonResult;

                                }
                                return Json(new { success = false, data = "", message = "RTO Data not found. Response is empty" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            JObject jsonDataObj = JObject.Parse(jsonResponseContent);

                            if (jsonDataObj.ContainsKey("Message"))
                            {
                                return Json(new { success = false, message = $"Response not success, Error Message: {jsonDataObj["Message"]}" }, JsonRequestBehavior.AllowGet);
                            }

                            return Json(new { success = false, message = $"Response not success, Error Message: {jsonResponseContent}" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string exception = ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? ex.Message;
                return Json(new { success = false, message = $"Server Error: {exception}" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetCityByStateAsync(string stateId)
        {
            try
            {
                int.TryParse(stateId, out int selectedStateId);
                using (var db = new AutoSherDBContext())
                {
                    List<EIBLMasterCity> masterCities = await db.EIBLMasterCities.Where(city => city.FKSTATE_ID == selectedStateId).ToListAsync();
                    List<EIBLMasterPincode> masterPincodes = await db.EIBLMasterPincodes.Where(pincode => pincode.FKSTATE_ID == selectedStateId).ToListAsync();
                    return Json(new { success = true, citiesList = masterCities, pincodesList = masterPincodes }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                string exception = ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? ex.Message;
                return Json(new { success = false, message = exception }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetQuotationResponse()
        {
            long cusId = Convert.ToInt32(Session["cusId"].ToString());
            long vehiId = Convert.ToInt32(Session["vehiId"].ToString());

            try
            {
                using (var db = new AutoSherDBContext())
                {
                    List<Quotationresponse> quoteData = db.quotationresponses.Where(m => m.customerId == cusId && m.vehicleid == vehiId).ToList();

                    return Json(new { data = quoteData }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                string exception = ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? ex.Message;

                return Json(new { data = false, message = exception }, JsonRequestBehavior.AllowGet);
            }
        }

        private string VehicleRegNumberFormat(string regNo)
        {
            if (string.IsNullOrEmpty(regNo))
            {
                return regNo;
            }

            regNo = regNo.ToUpper();

            string first2Chars = regNo.Substring(0, 2);
            string next2Digits = regNo.Substring(2, 2);

            string remaining = regNo.Substring(4);
            string next2Chars = remaining.Length > 5 ? remaining.Substring(0, 2) : remaining.Substring(0, 1);
            string last4Digits = remaining.Substring(next2Chars.Length);

            return $"{first2Chars}-{next2Digits}-{next2Chars}-{last4Digits}";
        }

        private string HandleJsonErrorResponse(string jsonResponse)
        {
            JObject jsonResponseObj = JObject.Parse(jsonResponse);
            string errorMessage = jsonResponseObj["ErrorMessage"]?.ToString();
            string errorCode = jsonResponseObj["ErrorCode"]?.ToString();

            if (!string.IsNullOrEmpty(errorMessage) && !string.IsNullOrWhiteSpace(errorMessage))
            {
                return errorMessage;
            }
            return null;
        }

        private int? GetPolicyTypeBasedOnCampaign(int? campaignId)
        {
            if (campaignId == 33)
            {
                return 0;
            }
            else if (campaignId == 34)
            {
                return 1;
            }
            else if (campaignId == 36)
            {
                return 2;
            }
            else
            {
                return null;
            }
        }

        #endregion

    }
}



public class PolicyResponse
{
    public string status { get; set; }
    public string message_txt { get; set; }
    public PolicyData data { get; set; }
}

public class PolicyData
{
    public string policy_id { get; set; }
    public string policy_no { get; set; }
    public string payment_status { get; set; }
    public int reference_id { get; set; }
    public string encrypted_policy_id { get; set; }
    public string encrypted_policy_no { get; set; }
}
