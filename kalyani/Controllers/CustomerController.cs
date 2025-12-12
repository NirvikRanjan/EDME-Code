using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoSherpa_project.Models;
using AutoSherpa_project.Models.ViewModels;
using System.Data.Entity;
using MySql.Data.MySqlClient;
using System.Data.Entity.Migrations;
using Newtonsoft.Json;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;

namespace AutoSherpa_project.Controllers
{
    [AuthorizeFilter]
    public class CustomerController : Controller
    {
        // GET: Customer
        [HttpGet, ActionName("addCustomer")]
        public ActionResult addCustomer()
        {
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    int userrole = Convert.ToInt32(Session["UserRole1"]);
                    long userId = Convert.ToInt32(Session["UserId"].ToString());

                    //var locationId = db.wyzusers.FirstOrDefault(m => m.id == userId).location_cityId;
                    //var locationList = db.locations.Select(loc => new { id = loc.cityId, name = loc.name }).Where(m=>m.id==locationId).ToList();
                    //ViewBag.locationList = locationList;
                    //if (userrole == 2)
                    //{
                    //    var workshopList = db.workshops.Where(m=>m.isinsurance==true).Select(work => new { id = work.id, name = work.workshopName }).ToList();
                    //    ViewBag.workShop = workshopList;
                    //}
                    //else
                    
                        var workshopList = db.workshops.Where(m => m.isinsurance == false).Select(work => new { id = work.id, name = work.workshopName }).ToList();
                        ViewBag.workShop = workshopList;
                    
                    var listOfRoleId = db.userlocations.Where(r => r.userLocation_id == userId).Select(r => r.locationList_cityId).ToList();
                    var locationList = db.locations.Where(r => listOfRoleId.Contains(r.cityId)).Select(m => new { id = m.cityId, name = m.name }).ToList();
                    ViewBag.locationList = locationList;
                    var insCompany = db.insurancecompanies.Select(m => new { id = m.id, companyName = m.companyName,m.isactive }).Where(m=>m.isactive==true).OrderBy(m => m.companyName).ToList();
                    ViewBag.ddlinsurancelist = insCompany;

                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        //Drop down Populate workshop  base on city
        public ActionResult FillWorkshopDropdown(int? cityId)
            {
            try
            {

                using (var db = new AutoSherDBContext())
                {
                  var  workshopList = db.workshops.Where(x => x.location_cityId == cityId).Select(m => new { id = m.id, workshopName = m.workshopName }).ToList();

                    //if (workshopList.Count ==0)
                    //{
                    //    workshopList = db.workshops.ToList();
                    //}
                    return Json(new { workshopList = workshopList, JsonRequestBehavior.AllowGet });

                }
            }
            catch (Exception ex)
            {
            }
            return Json(new { workshopList = "", JsonRequestBehavior.AllowGet });

        }

        [HttpPost, ActionName("addCustomer")]
        public ActionResult saveCustomer(CustomerViewModel customerViewModel)
        {
            string logInUser = string.Empty;
            if (Session["LoginUser"].ToString() != null)
            {
                logInUser = Session["LoginUser"].ToString();
            }
            int userId = Convert.ToInt32(Session["UserId"].ToString());
            long managerId = 0;
            try
            {
                if (ModelState.IsValid)
                {
                    using (var db = new AutoSherDBContext())
                    {
                        using (DbContextTransaction dbTransaction = db.Database.BeginTransaction())
                        {
                            try
                            {
                                customerViewModel.customer.createdBy = (Session["UserId"].ToString());
                                customerViewModel.customer.createdDate = Convert.ToDateTime(DateTime.Today.ToString());
                                db.customers.Add(customerViewModel.customer);
                                db.SaveChanges();

                                customerViewModel.address.customer_Id = customerViewModel.customer.id;
                                customerViewModel.address.isPreferred = true;
                                db.addresses.Add(customerViewModel.address);

                                customerViewModel.email.customer_id = customerViewModel.customer.id;
                                customerViewModel.email.isPreferredEmail = true;
                                db.emails.Add(customerViewModel.email);

                                customerViewModel.vehicle.customerId = customerViewModel.customer.id.ToString();
                                customerViewModel.vehicle.customer_id = customerViewModel.customer.id;
                                customerViewModel.vehicle.Frame_no = customerViewModel.vehicle.chassisNo;
                                customerViewModel.vehicle.engineNo = customerViewModel.vehicle.engineNo;

                                customerViewModel.vehicle.chassisNo ="C"+customerViewModel.vehicle.chassisNo+"E"+customerViewModel.vehicle.engineNo;
                                    customerViewModel.vehicle.first_name = customerViewModel.customer.customerName;
                                    customerViewModel.vehicle.Mobile = customerViewModel.phone.phoneNumber;
                                    customerViewModel.vehicle.Address_Line_1 = customerViewModel.address.concatenatedAdress;
                                    customerViewModel.vehicle.email_Id = customerViewModel.email.emailAddress;
                                    customerViewModel.vehicle.policyDueDate =Convert.ToDateTime(customerViewModel.vehicle.Previous_Policy_Expiry);
                                    customerViewModel.vehicle.OEMWarrentyDate = Convert.ToDateTime(customerViewModel.vehicle.saleDate).AddYears(2).AddDays(-1);
                                    customerViewModel.vehicle.averageRunning = "false";
                                    db.vehicles.Add(customerViewModel.vehicle);

                                customerViewModel.phone.customer_id = customerViewModel.customer.id;
                                customerViewModel.phone.isPreferredPhone = true;
                                db.phones.Add(customerViewModel.phone);


                                db.SaveChanges();
                                if(Session["UserRole"].ToString()!="Admin")
                                {
                                    long campId = db.campaigns.FirstOrDefault(m => m.campaignName == "Customer Search").id;
                                    managerId = db.wyzusers.FirstOrDefault(m => m.userName == db.wyzusers.FirstOrDefault(x => x.id == userId).creManager && m.role == "CREManager").id;
                                     if (logInUser == "Insurance")
                                    {
                                        insuranceassignedinteraction insu_assign = new insuranceassignedinteraction();

                                        insu_assign.callMade = "No";
                                        insu_assign.displayFlag = false;
                                        insu_assign.uplodedCurrentDate = DateTime.Now;
                                        insu_assign.customer_id = customerViewModel.customer.id;
                                        insu_assign.vehicle_vehicle_id = customerViewModel.vehicle.vehicle_id;
                                        insu_assign.wyzUser_id = userId;
                                        insu_assign.campaign_id = campId;
                                        insu_assign.policyDueDate = Convert.ToDateTime(customerViewModel.vehicle.Previous_Policy_Expiry);
                                        insu_assign.location_id = customerViewModel.vehicle.location_cityId;
                                        insu_assign.isAutoAssigned = false;
                                        db.insuranceassignedinteractions.Add(insu_assign);
                                        db.SaveChanges();

                                    }
                                }
                                dbTransaction.Commit();
                                return Json(new { success = true, custId = customerViewModel.customer.id, vehId = customerViewModel.vehicle.vehicle_id });
                            }
                            catch (Exception ex)
                            {
                                dbTransaction.Rollback();
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

                                return Json(new { success = false, error = exception });
                            }

                        }
                    }
                }
                else
                {
                    return Json(new { success = false, error = "Please Fill Required Fields" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        public ActionResult checkVehicle(string data)
        {
            string id = data.Split(',')[1];
            string regNo = data.Split(',')[0];
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    if (id == "1")//search for vehicle reg no.
                    {

                        if (db.vehicles.Any(m => m.vehicleRegNo.ToLower() == regNo.ToLower()))
                        {
                            return Json(new { success = true, cameFor = "vehReg" });
                        }
                        else
                        {
                            return Json(new { success = false, cameFor = "vehReg" });
                        }
                    }
                    else if (id == "2")//search for vehicle reg no.
                    {
                        if (db.vehicles.Any(m => m.chassisNo.ToLower() == regNo.ToLower()))
                        {
                            return Json(new { success = true, cameFor = "chassis" });
                        }
                        else
                        {
                            return Json(new { success = false, cameFor = "chassis" });
                        }
                    }

                    else if (id == "3")//search for vehicle reg no.
                    {
                        if (db.vehicles.Any(m => m.chassisNo.ToLower() == regNo.ToLower()))
                        {
                            return Json(new { success = true, cameFor = "engine" });
                        }
                        else
                        {
                            return Json(new { success = false, cameFor = "engine" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return Json(new { success = false });
        }

        public ActionResult getWorkshopByCity(int? cityId)
        {
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    var locationList = db.locations.ToList();
                    var workshopList = db.workshops.Where(user => user.location_cityId == cityId).Select(user => user.location).SingleOrDefault();
                    return Json(new { success = true, data = workshopList });

                }

            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public ActionResult getCREofCREManager(long? customerId, long? vehicleId, string userrole)
        {

            try
            {
                using (var db = new AutoSherDBContext())
                {
                    string user = Session["UserName"].ToString();
                    long userId = Convert.ToInt64(Session["userId"].ToString());
                    if (Session["UserRole"].ToString() == "CREManager")
                    {
                        var wyzuserList = db.wyzusers.Where(m => m.creManager == user).Select(m => new { m.id, name = m.firstName + "(" + m.userName + ")" }).ToList();

                        return Json(new { success = true, data = wyzuserList });

                    }

                }
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

                return Json(new { success = false, error = exception });
            }
            return Json(new { success = false, error = "No CRE's Foud" });
        }
        public ActionResult getCREManager(long? workshpId)
        {

            try
            {
                using (var db = new AutoSherDBContext())
                {
                    string user = Session["UserName"].ToString();
                    long userId = Convert.ToInt64(Session["userId"].ToString());
                    bool isinsurance = db.wyzusers.FirstOrDefault(u => u.id == userId).insuranceRole;

                    if (Session["DealerName"].ToString() == "INDUS MOTORS")
                    {
                        if (Session["UserRole"].ToString() == "Admin")
                        {
                            if (isinsurance)
                            {
                                var cres = db.wyzusers.Where(u => u.role == "CRE" && u.unAvailable==false && u.role1 == "2").Select(m => new { creId = m.id, creName = m.firstName }).ToList();
                                return Json(new { cres });
                            }
                            else
                            {
                                var cres = db.wyzusers.Where(u => u.role == "CRE" && u.unAvailable == false && u.workshop_id == workshpId && u.role1 == "1").Select(m => new { creId = m.id, creName = m.firstName }).ToList();
                                return Json(new { cres });
                            }
                        }
                        if (Session["UserRole"].ToString() == "RM" || Session["UserRole"].ToString() == "WM")
                        {
                            var creManagerIdList = db.AccessLevels.Where(m => m.rmId == userId).Select(m => m.creId).Distinct().ToList();
                            var cres = db.wyzusers.Where(x => creManagerIdList.Contains(x.id) && x.workshop_id == workshpId).Select(m => new { creId = m.id, creName = m.firstName }).ToList();
                            return Json(new { cres });

                        }
                        else
                        {
                            if (isinsurance)
                            {
                                var cres = db.wyzusers.Where(u => u.creManager == user && u.role == "CRE" && u.unAvailable == false && u.role1 == "2").Select(m => new { creId = m.id, creName = m.firstName }).ToList();
                                return Json(new { cres });
                            }
                            else
                            {
                                var cres = db.wyzusers.Where(u => u.creManager == user && u.role == "CRE" && u.unAvailable == false && u.workshop_id == workshpId && u.role1 == "1").Select(m => new { creId = m.id, creName = m.firstName }).ToList();
                                return Json(new { cres });
                            }
                        }
                    }
                    else
                    {
                        if (Session["UserRole"].ToString() == "Admin")
                        {
                            if (isinsurance)
                            {
                                var cres = db.wyzusers.Where(u => u.role == "CRE" && u.unAvailable == false && u.role1 == "2").Select(m => new { creId = m.id, creName = m.userName }).ToList();
                                return Json(new { cres });
                            }
                            else
                            {
                                var cres = db.wyzusers.Where(u => u.role == "CRE" && u.unAvailable == false && u.workshop_id == workshpId && u.role1 == "1").Select(m => new { creId = m.id, creName = m.userName }).ToList();
                                return Json(new { cres });
                            }
                        }
                        if (Session["UserRole"].ToString() == "RM" || Session["UserRole"].ToString() == "WM")
                        {
                            var creManagerIdList = db.AccessLevels.Where(m => m.rmId == userId).Select(m => m.creId).Distinct().ToList();
                            var cres = db.wyzusers.Where(x => creManagerIdList.Contains(x.id) && x.workshop_id == workshpId).Select(m => new { creId = m.id, creName = m.userName }).ToList();
                            return Json(new { cres });

                        }
                        else
                        {
                            if (isinsurance)
                            {
                                var cres = db.wyzusers.Where(u => u.creManager == user && u.role == "CRE" && u.unAvailable == false && u.role1 == "2").Select(m => new { creId = m.id, creName = m.userName }).ToList();
                                return Json(new { cres });
                            }
                            else
                            {
                                var cres = db.wyzusers.Where(u => u.creManager == user && u.role == "CRE" && u.unAvailable == false && u.workshop_id == workshpId && u.role1 == "1").Select(m => new { creId = m.id, creName = m.userName }).ToList();
                                return Json(new { cres });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Json(new { });
        }


        #region
        public ActionResult searchCustomerData()
        {
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    string userRole = Session["UserRole"].ToString();
                    long userId = long.Parse(Session["UserId"].ToString());
                    if (userRole == "CREManager" || userRole == "Admin")
                    {
                        ViewBag.givenAccess = db.wyzusers.FirstOrDefault(x => x.id == userId).searchAssignAccess;
                    }
                }
            }
            catch(Exception ex)
            {

            }
            return View();
        }

        public ActionResult searchCustomerDetails(int searchtype,string searchpattern)
        {
            long wyzUserId = long.Parse(Session["UserId"].ToString());
            long heroDealerId = Convert.ToInt64(Session["HeroDealerId"]);
            string loginUser = Session["LoginUser"].ToString(), userRole = Session["UserRole"].ToString();// = "Service";

            List<searchcustomerData> searchDetails = new List<searchcustomerData>();
            try
            {

                using (var db = new AutoSherDBContext())
                {
                    db.Database.CommandTimeout = 900;
                    string str = @"CALL customer_search_for_insurance_hero(@searchtype,@pattern,@indealerid,@inuserid);";

                    MySqlParameter[] sqlParameter = new MySqlParameter[]
                    {
                            new MySqlParameter("searchtype", searchtype),
                            new MySqlParameter("pattern",searchpattern),
                            new MySqlParameter("indealerid",heroDealerId),
                            new MySqlParameter("inuserid",wyzUserId),

                    };
                    searchDetails = db.Database.SqlQuery<searchcustomerData>(str, sqlParameter).ToList();
                   
                        foreach (var res in searchDetails)
                        {
                            long ins_id = Convert.ToInt32(res.insuranceassignedinteraction_id);

                            long vehID = long.Parse(res.vehicle_id);
                            long workID_Assign = 0;
                            long? workID_wyzuser = db.wyzusers.FirstOrDefault(x => x.id == wyzUserId).workshop_id;


                            if (userRole == "CRE")
                            { if (loginUser == "Insurance")
                                    {
                                        res.userrole = "Insurance";
                                        enablecustomer360profile(res, "Insurance");
                                    }
                  
                            }
                            else if (userRole == "CREManager")
                            {
                                if (loginUser == "Insurance")
                                {
                                    res.userrole = "Insurance";
                                    //res.assignSearch = "Yes";
                                    enablecustomer360profile(res, "Insurance");
                                }
                            }
                            else if (userRole == "Admin")
                            {
                                if (loginUser == "Insurance")
                                {
                                    res.userrole = "Insurance";
                                    //res.assignSearch = "Yes";
                                    enablecustomer360profile(res, "Insurance");
                                }
                            }
                        }
        

                }
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

                return Json(new { success = false, error = exception });
            }
                return Json(new { success = true, data = searchDetails });


        }
        public searchcustomerData enablecustomer360profile(searchcustomerData custSearch, string typeOfDispo)
        {
            long wyzUserId = long.Parse(Session["UserId"].ToString());
            string userName = Session["UserName"].ToString();
            string loginUser = Session["LoginUser"].ToString();// = "Service";
            string dealerCode = Session["DealerCode"].ToString();
            bool isSuperCre = false;
            try
            {
                if (Session["IsSuperCRE"] != null && Convert.ToBoolean(Session["IsSuperCRE"]) == true)
                {
                    isSuperCre = true;
                }

                using (var db = new AutoSherDBContext())
                {
                   // bool userControl = db.dealers.FirstOrDefault(m => m.dealerCode == dealerCode).userControl ?? default(bool);
                     if (typeOfDispo == "Insurance")
                    {
                        int ins_id = Convert.ToInt32(custSearch.insuranceassignedinteraction_id);
                        long? vehicleIdpolicy = Convert.ToInt64(custSearch.vehicle_id);
                        DateTime policyDueDate = DateTime.Now;
                        DateTime fromDate = DateTime.Now.AddDays(-90);
                        DateTime todate = DateTime.Now.AddDays(90);
                        bool isPolicyDueDate = false;

                        if (db.insurances.Count(m => m.vehicle_id == vehicleIdpolicy) > 0)
                        {
                            policyDueDate = db.insurances.Where(m => m.vehicle_id == vehicleIdpolicy).OrderByDescending(m => m.policyDueDate).FirstOrDefault().policyDueDate ?? (default(DateTime));
                            if (policyDueDate != null)
                            {
                                policyDueDate = Convert.ToDateTime(policyDueDate);
                                isPolicyDueDate = true;
                            }
                        }
                        else
                        {
                            isPolicyDueDate = false;
                        }

                        if (loginUser != "Insurance")
                        {
                            custSearch.userControl = "Hdd";
                        }
                        else if (db.insuranceassignedinteractions.Any(m => m.id == ins_id))
                        {
                            insuranceassignedinteraction insuAssign = db.insuranceassignedinteractions.FirstOrDefault(m => m.id == ins_id);
                            long assignWyzId = insuAssign.wyzUser_id ?? default(long);

                            string creManager = db.wyzusers.FirstOrDefault(m => m.id == assignWyzId).creManager;
                            if (string.IsNullOrEmpty(custSearch.assignedCre) && assignWyzId != 0)
                            {
                                custSearch.assignedCre = db.wyzusers.FirstOrDefault(m => m.id == assignWyzId).userName;
                                custSearch.finaldispoId = insuAssign.finalDisposition_id == null ? "0" : insuAssign.finalDisposition_id.ToString();
                            }
                            if(creManager==userName)
                            {
                                custSearch.assignSearch = "Yes";
                            }
                            else
                            {
                                custSearch.assignSearch = "No";
                            }
                            if (wyzUserId == assignWyzId && insuAssign.finalDisposition_id != 35)
                            {
                                custSearch.userControl = "Shw";
                            }
                            else if (isSuperCre == true)
                            {
                                custSearch.userControl = "Shw";
                            }
                            else 
                            {
                                custSearch.userControl = "Hdd";
                            }
                            //else if (userControl)
                            //{
                            //    string assignCreManager = string.Empty;
                            //    if (assignWyzId != 0)
                            //    {
                            //        assignCreManager = db.wyzusers.FirstOrDefault(m => m.id == assignWyzId).creManager;
                            //        if (isPolicyDueDate == true)
                            //        {
                            //            if (assignCreManager == loggedIdCreManager && db.userroles.Any(m => m.users_id == wyzUserId && m.roles_id == 35) && (policyDueDate.Date > fromDate.Date && policyDueDate.Date < todate.Date))
                            //            {
                            //                custSearch.userControl = "Shw";
                            //            }
                            //            else
                            //            {
                            //                custSearch.userControl = "Hdd";
                            //            }
                            //        }
                            //        else
                            //        {
                            //            custSearch.userControl = "Hdd";
                            //        }
                            //    }
                            //    else if (db.userroles.Any(m => m.users_id == wyzUserId && m.roles_id == 35) && (policyDueDate.Date > fromDate.Date && policyDueDate.Date < todate.Date))
                            //    {
                            //        custSearch.userControl = "Shw";
                            //    }
                            //    else
                            //    {
                            //        custSearch.userControl = "Hdd";
                            //    }
                            //}
                            //else
                            //{
                            //    custSearch.userControl = "Shw";
                            //}
                        }
                        else
                        {
                            if(isSuperCre==true)
                            {
                                custSearch.userControl = "Shw";
                            }
                            else
                            {
                                custSearch.userControl = "Hdd";
                            }
                            //if (userControl)
                            //{
                            //    custSearch.userControl = "Hdd";
                            //}
                            //else
                            //{
                            //    if (custSearch.assignedCre == "Not Assigned" && db.dealers.FirstOrDefault().insunassignedblock == true)
                            //    {
                            //        custSearch.userControl = "Hdd";
                            //    }
                            //    else
                            //    {
                            //        custSearch.userControl = "Shw";
                            //    }
                            //}
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return custSearch;
            }

            return custSearch;
        }
        public ActionResult assignCallManually(long? custmerId, long? vehicleId,long creId, long assignId)
        {
            int count = 0;
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    long Id = Convert.ToInt64(Session["UserId"]);
                    vehicle veh = db.vehicles.Where(u => u.vehicle_id == vehicleId).FirstOrDefault();

                        if (assignId == 0 && db.insuranceassignedinteractions.Count(m => m.vehicle_vehicle_id == vehicleId && m.customer_id == custmerId) == 0)
                        {
                            insuranceassignedinteraction ins = new insuranceassignedinteraction();

                            ins.callMade = "NO";
                            ins.interactionType = "Enquiry";
                            ins.customer_id = custmerId;
                            ins.vehicle_vehicle_id = vehicleId;
                            ins.wyzUser_id = creId;
                            ins.displayFlag = true;
                            ins.uplodedCurrentDate = DateTime.Now;
                            ins.updatedOnDate = DateTime.Now;
                            ins.location_id = veh.fkcity_id;
                            ins.isAutoAssigned = false;

                                if (veh.Previous_Policy_Expiry != null)
                                {
                                    ins.policyDueDate = Convert.ToDateTime(veh.Previous_Policy_Expiry);
                                }
                                ins.insuranceCompanyName = veh.Previous_Insurer_Name;
                                ins.isAutoAssigned = false;
                                ins.upload_id = 0;
                                ins.assigned_wyzuser_id = creId;
                                ins.assigned_manager_id = Id;
                                db.insuranceassignedinteractions.Add(ins);
                                db.SaveChanges();

                                assignedcallsreport assignReport = new assignedcallsreport();
                                assignReport.assignInteractionID = ins.id;
                                assignReport.assignedDate = DateTime.Now;
                                assignReport.assignmentType = "Insurance";
                                assignReport.moduletypeId = 2;
                                long dueType = Convert.ToInt64(ins.policyDueType);
                                assignReport.dueType = dueType;
                                assignReport.dueDate = veh.policyDueDate;
                                assignReport.vehicleId = Convert.ToInt64(vehicleId);
                                assignReport.wyzuserId = Convert.ToInt64(creId);
                                assignReport.campaignId = ins.campaign_id;
                                assignReport.assigned_manager_id = Id;
                                assignReport.isautoassigned = false;
                                db.assignedcallsreports.AddOrUpdate(assignReport);
                                db.SaveChanges();
                        }
                        else if (assignId != 0 && db.insuranceassignedinteractions.Count(m => m.id == assignId && m.customer_id == custmerId && m.vehicle_vehicle_id == vehicleId) > 0)
                        {
                            insuranceassignedinteraction ins = db.insuranceassignedinteractions.FirstOrDefault(m => m.id == assignId);
                            ins.assigned_manager_id = Id;
                            ins.wyzUser_id = creId;
                            ins.assigned_wyzuser_id = creId;
                            ins.isAutoAssigned = false;
                            db.insuranceassignedinteractions.AddOrUpdate(ins);
                            db.SaveChanges();

                            if (ins.wyzUser_id != 0 && ins.wyzUser_id != null)
                            {
                                change_assignment_records chng = new change_assignment_records();
                                chng.assignedinteraction_id = ins.id;
                                chng.last_wyzuserId = Convert.ToInt64(ins.wyzUser_id);
                                chng.campaign_id = ins.campaign_id ?? default(long);
                                chng.moduletypeIs = 2;
                                chng.new_wyzuserId = Convert.ToInt64(creId);
                                chng.updatedDate = DateTime.Now;
                                chng.updatedType = "Enquiry";
                                chng.assigned_manager_id = Id;
                                db.change_assignment_records.Add(chng);
                                db.SaveChanges();
                            }
                            else
                            {
                                assignedcallsreport assignReport = new assignedcallsreport();
                                assignReport.assignInteractionID = ins.id;
                                assignReport.assignedDate = DateTime.Now;
                                assignReport.assignmentType = "Insurance";
                                assignReport.moduletypeId = 2;
                                long dueType = Convert.ToInt64(ins.policyDueType);
                                assignReport.dueType = dueType;
                                assignReport.dueDate = veh.nextServicedate;
                                assignReport.vehicleId = Convert.ToInt64(vehicleId);
                                assignReport.wyzuserId = Convert.ToInt64(creId);
                                assignReport.campaignId = ins.campaign_id;
                                assignReport.assigned_manager_id = Id;
                                assignReport.isautoassigned = false;
                                db.assignedcallsreports.Add(assignReport);
                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            return Json(new { success = false, exception = "Data Duplication Found" });
                        }
                    return Json(new { success = true });
                }
            }
            catch (Exception ex)
            {
                string exception = "";
                if (ex.Message.Contains("inner exception"))
                {
                    exception = ex.InnerException.Message;
                }
                else
                {
                    exception = ex.Message;
                }
                return Json(new { success = false, exception = exception });
            }
            return Json(new { success = false });
        }

        #endregion
    }
}