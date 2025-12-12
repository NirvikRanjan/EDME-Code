using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using AutoSherpa_project.Models;
using AutoSherpa_project.Models.ViewModels;
using MySql.Data.MySqlClient;
using System.Reflection;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Net;
using Firebase.Database;
using Newtonsoft.Json.Linq;
using Microsoft.AspNet.SignalR;
using AutoSherpa_project.Models.API_Model;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;

namespace AutoSherpa_project.Controllers
{

    public class HomeController : Controller
    {

        public static List<string> autosherpa1 = new List<string>(new string[] { "ADITYABIRLA","EDME" });
        public static List<string> autosherpa3 = new List<string>(new string[] { "ADITYABIRLA","EDME" });

        [HttpGet]
        public ActionResult Index(int? id, string message)
        {
            //if (TempData["IsRoleError"] != null)
            //{
            //    ViewBag.error = TempData["IsRoleError"];
            //}

            if (message != null || TempData["ControllerName"] != null)
            {
                TempData["Exception"] = message;
            }

            if (id == 200 || id != 0)
            {
                Session["RoleFor"] = null;
            }
            return RedirectToAction("LoginPage", "Home");

            //return View();
        }

        [ActionName("LogInPage"), HttpGet]
        //public ActionResult ShowLogin(string forRole)
        public ActionResult ShowLogin()
        {
            try
            {
                if (TempData["IsRoleError"] != null)
                {
                    ViewBag.error = TempData["IsRoleError"];
                }

                if (TempData["Exception"] != null || TempData["ControllerName"] != null)
                {
                    ViewBag.Exception = TempData["Exception"];
                }

                //if (forRole != null && forRole != "")
                //{
                //    Session["RoleFor"] = forRole;
                //}
                //else
                //{
                //    TempData["IsRoleError"] = TempData["IsRoleError"];
                //    return RedirectToAction("Index", 1);
                //}

            }
            catch (Exception ex)
            {

            }
            return View();
        }

        [ActionName("LogInPage"), HttpPost]
        public ActionResult ShowLoginPost(wyzuser user)
        {

            try
            {
                if (Session["UserName"] == null && Session["UserId"] == null && Session["UserRole"] == null)
                {
                    using (var db = new AutoSherDBContext())

                    {
                        bool allowIP = true;
                        string incomingIp = Request.UserHostAddress;
                        //var isAuthenticAcces = Task.Run(async () => await getAuthenticateUserLogin(db.dealers.FirstOrDefault().dealerCode)).Result;
                        //if (isAuthenticAcces == true)
                        {
                            if (db.wyzusers.Any(m => m.userName == user.userName && m.password == user.password && m.unAvailable == false))
                            {
                                var userData = db.wyzusers.SingleOrDefault(m => m.userName == user.userName && m.password == user.password);

                                if (allowIP == true)
                                {
                                    //if (Session["RoleFor"].ToString().ToLower() == userData.role.ToLower() || userData.role == "Admin" || userData.role == "SuperAdmin")
                                    {

                                        Session["username"] = userData.userName + userData.password; ;
                                        Session["UserName"] = userData.userName;
                                        Session["UserId"] = userData.id;
                                        Session["firstName"] = userData.firstName;
                                        Session["UserRole"] = userData.role;
                                        Session["UserRole1"] = userData.role1;
                                        
                                        Session["DealerCode"] = userData.dealerCode;
                                        

                                        Session["DealerName"] = userData.dealerName;

                                        
                                        Session["HeroDealerCode"] = userData.herodealer_Code;


                                        Session["HeroDealerId"] = userData.herodealer_ID;
                                        Session["RoleFor"] = userData.role;
                                        Session["OEM"] = db.dealers.FirstOrDefault(m => m.id == userData.dealer_id).OEM;
                                        Session["message"] = "";
                                        if (db.dealers.FirstOrDefault(m => m.id == userData.dealer_id).opsguruUrl != null)
                                        {
                                            Session["OpsGuruLink"] = db.dealers.FirstOrDefault(m => m.id == userData.dealer_id).opsguruUrl;
                                        }
                                        if (userData.role == "CRE")
                                        {
                                            if (db.MessageDetails.Count(m => m.isactive == true) > 0)
                                            {
                                                Session["message"] = db.MessageDetails.FirstOrDefault(m => m.isactive == true).message;
                                            }
                                        }
                                        navigationaccess navAccess = db.navaccess.FirstOrDefault(m => m.wyzuser_id == userData.id && m.isactive == true);



                                        List<formmapping> navForms = new NavigationTabController().getNavForms(userData.role1, userData.role, false, navAccess == null ? "" : navAccess.form_id, false);
                                        Session["navigations"] = JsonConvert.SerializeObject(navForms.Select(m => new { id = m.id, mainform_id = m.mainform_id, font_icon = m.font_icon, form_name = m.form_name, href_link = m.href_link }).ToList());

                                        //long? prodCode = userData.herodealer_Code;
                                        //if (prodCode.HasValue && prodCode != 0 && prodCode != null)
                                        //{
                                        //    string url = WalkInCustomerLink(prodCode);
                                        //    if (!string.IsNullOrEmpty(url))
                                        //    {
                                        //        Session["WalkinCustomerLink"] = url;
                                        //    }
                                        //    else
                                        //    {
                                        //        Session["WalkinCustomerLink"] = "#";
                                        //    }
                                            
                                        //}
                                        //else
                                        //{
                                        //    Session["WalkinCustomerLink"] = "#";
                                        //}
                                        

                                        #region AutoSave
                                        //recordLogDetails(1, userData.userName, userData.creManager, userData.dealerCode, userData.id, HttpContext.Session.SessionID);
                                        #endregion

                                        var followUpday = db.dealers.Where(m => m.id == userData.dealer_id).Select(x => new { x.followupdaylimit, x.bookingDayLimit, x.insbookingDayLimit, x.insfollowupdaylimit }).ToArray();

                                        string INSfollowUpMaxdate = "";
                                        string INSbookingMaxDate = "";

                                        if (followUpday[0].insbookingDayLimit == 0)
                                        {
                                            INSbookingMaxDate = "15";
                                        }
                                        else
                                        {
                                            INSbookingMaxDate = followUpday[0].insbookingDayLimit.ToString();
                                        }
                                        if (followUpday[0].insfollowupdaylimit == 0)
                                        {
                                            INSfollowUpMaxdate = "15";
                                        }
                                        else
                                        {
                                            INSfollowUpMaxdate = followUpday[0].insfollowupdaylimit.ToString();
                                        }

                                        Session["INSfollowUpDayLimit"] = INSfollowUpMaxdate + "," + INSbookingMaxDate;

                                        long roleID = 0;

                                        if (userData.role == "Admin")
                                        {

                                            formmapping firstNav;

                                            if (navForms.Count > 0)
                                            {
                                                firstNav = navForms.FirstOrDefault(m => m.mainform_id != 0);
                                                if (!string.IsNullOrEmpty(firstNav.href_link) && firstNav.href_link.Contains("/"))
                                                {
                                                    if (userData.insuranceRole == true)
                                                    {
                                                        Session["LoginUser"] = "Insurance";
                                                    }

                                                    //if (firstNav.href_link.Split('/').Count() == 3)
                                                    //{
                                                    //    return RedirectToAction(firstNav.href_link.Split('/')[1], firstNav.href_link.Split('/')[0], new { @id = firstNav.href_link.Split('/')[2] });
                                                    //}
                                                    //else
                                                    //{
                                                    //    return RedirectToAction(firstNav.href_link.Split('/')[1], firstNav.href_link.Split('/')[0]);
                                                    //}
                                                }
                                            }
                                            if (userData.insuranceRole == true)
                                            {
                                                Session["LoginUser"] = "Insurance";
                                                return RedirectToAction("policyAssignment", "policyAssignment");
                                            }

                                        }

                                        if (userData.role == "SuperAdmin")
                                        {

                                            formmapping firstNav;

                                            if (navForms.Count > 0)
                                            {
                                                firstNav = navForms.FirstOrDefault(m => m.mainform_id != 0);
                                                if (!string.IsNullOrEmpty(firstNav.href_link) && firstNav.href_link.Contains("/"))
                                                {
                                                    if (userData.insuranceRole == true)
                                                    {
                                                        Session["LoginUser"] = "Insurance";
                                                    }
                                                    if (firstNav.href_link.Split('/').Count() == 3)
                                                    {
                                                        return RedirectToAction(firstNav.href_link.Split('/')[1], firstNav.href_link.Split('/')[0], new { @id = firstNav.href_link.Split('/')[2] });
                                                    }
                                                    else
                                                    {
                                                        return RedirectToAction(firstNav.href_link.Split('/')[1], firstNav.href_link.Split('/')[0]);
                                                    }
                                                }
                                            }


                                            if (userData.insuranceRole == true)
                                            {
                                                Session["LoginUser"] = "Insurance";
                                                return RedirectToAction("ManageNavigation", "NavigationTab");
                                            }

                                        }
                                        if (userData.role == "AreaManager")
                                        {
                                            Session["LoginUser"] = "Insurance";
                                            return RedirectToAction("liveReports", "liveReports");
                                        }
                                        if (userData.role == "ChannelManager")
                                        {
                                            Session["LoginUser"] = "Insurance";
                                            return RedirectToAction("liveReports", "liveReports");
                                        }
                                        if (userData.role == "CREManager")
                                        {
                                            Session["LoginUser"] = "Insurance";
                                            return RedirectToAction("liveReports", "liveReports");
                                        }
                                        if (userData.role == "ICManager")
                                        {
                                            Session["LoginUser"] = "Insurance";
                                            return RedirectToAction("insuranceChangeAssignment", "policyAssignment");
                                        }
                                        //if (userData.userName == "")
                                        //{
                                        //    Session["LoginUser"] = "Insurance";
                                        //    return RedirectToAction("insuranceChangeAssignment", "policyAssignment");
                                        //}

                                        if (userData.role == "ZoneManager")
                                        {


                                            if (userData.insuranceRole == true)
                                            {
                                                Session["LoginUser"] = "Insurance";
                                                return RedirectToAction("liveReports", "liveReports");
                                            }

                                        }
                                        if (userData.role == "StateManager")
                                        {


                                            if (userData.insuranceRole == true)
                                            {
                                                Session["LoginUser"] = "Insurance";
                                                return RedirectToAction("liveReports", "liveReports");
                                            }

                                        }
                                        if (userData.role == "CityManager")
                                        {


                                            if (userData.insuranceRole == true)
                                            {
                                                Session["LoginUser"] = "Insurance";
                                                return RedirectToAction("liveReports", "liveReports");
                                            }

                                        }
                                        if (userData.role == "DealerManager")
                                        {


                                            if (userData.insuranceRole == true)
                                            {
                                                Session["LoginUser"] = "Insurance";
                                                return RedirectToAction("liveReports", "liveReports");
                                            }

                                        }
                                        else
                                        {
                                            var rol = db.roles.ToList();
                                            if (db.roles.Count(m => m.role1 == "SuperCRE") > 0)
                                            {
                                                roleID = db.roles.FirstOrDefault(m => m.role1 == "SuperCRE").id;
                                                var userRole = db.userroles.Where(m => m.users_id == userData.id).ToList();
                                                if (userRole.Any(m => m.roles_id == roleID))
                                                {
                                                    Session["IsSuperCRE"] = true;
                                                }
                                            }

                                            if (userData.insuranceRole == true)
                                            {
                                                Session["LoginUser"] = "Insurance";
                                                return RedirectToAction("Insurance", "Insurance", null);
                                            }
                                        }
                                    }
                                    //else
                                    //{
                                    //    TempData["IsRoleError"] = "Invalid Login Attempt!...";
                                    //    return RedirectToAction("LogInPage", "Home", null);
                                    //}
                                }
                                else
                                {
                                    TempData["IsRoleError"] = "Invalid IP Address!..";
                                    return RedirectToAction("LogInPage", "Home", null);
                                }
                            }
                            else
                            {
                                TempData["IsRoleError"] = "Invalid Username/Password!..";
                                return RedirectToAction("LogInPage", "Home", null);
                            }
                        }
                        //else
                        //{
                        //    TempData["IsRoleError"] = "Please Contact Admin..";
                        //    return RedirectToAction("LogInPage", "Home", null);
                        //}
                    }

                }
                else
                {
                    if (Session["RoleFor"].ToString().ToLower() == "cremanager")
                    {
                        return RedirectToAction("managerHomePage", "callLogReminder");
                    }
                    if (Session["RoleFor"].ToString().ToLower() == "icmanager")
                    {
                        return RedirectToAction("insuranceChangeAssignment", "policyAssignment");
                    }
                    if (Session["RoleFor"].ToString().ToLower() == "zonemanager" || Session["RoleFor"].ToString().ToLower() == "statemanager" || Session["RoleFor"].ToString().ToLower() == "citymanager" || Session["RoleFor"].ToString().ToLower() == "dealermanager")
                    {
                        return RedirectToAction("liveReports", "liveReports");
                    }
                    if (Session["RoleFor"].ToString().ToLower() == "admin")
                    {
                        return RedirectToAction("policyAssignment", "policyAssignment");
                    }
                    if (Session["RoleFor"].ToString().ToLower() == "superadmin")
                    {
                        return RedirectToAction("credetails","superadmin");
                    }
                    else
                    {
                        return RedirectToAction("Insurance", "Insurance", null);
                    }

                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("inner exception"))
                {
                    TempData["Exceptions"] = ex.InnerException.Message.ToString();
                }
                else
                {
                    TempData["Exceptions"] = ex.Message.ToString();
                }

                return RedirectToAction("LogOff");
            }
            return View();
        }

        public ActionResult LogOff()
        {
            string error = string.Empty;
            try
            {
                if (HttpContext.Session != null)
                {
                    // recordLogDetails(2, "", "", "", 0, HttpContext.Session.SessionID);
                }


                using (var db = new AutoSherDBContext())
                {
                    if (db.Database.Exists())
                    {
                        db.Database.Connection.Close();
                    }
                }

                FormsAuthentication.SignOut();
                Session.Abandon();
                Session.Clear();
                Session.RemoveAll();

                if (TempData["Exceptions"] != null)
                {
                    error = TempData["Exceptions"].ToString();
                }

                TempData["ControllerName"] = JsonConvert.SerializeObject(TempData["ControllerName"]);
            }
            catch (Exception ex)
            {

            }

            return RedirectToAction("Index", "Home", new { @id = 0, @message = error });
        }

        [AuthorizeFilter]
        [ActionName("FollowUpToday")]
        public ActionResult getFollowUpTableDataOfCRE(string id)
        {
            string typeOfDipo = id;
            List<callinteraction> followUpdata = new List<callinteraction>();//repo.getListOfFollowUpOfTodayCRE(folowupDataId);
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    string cre = Session["UserName"].ToString();
                    int userId = Convert.ToInt32(Session["UserId"].ToString());
                    wyzuser userdata = new wyzuser();
                    userdata = db.wyzusers.SingleOrDefault(m => m.id == userId);
                    string dealerCode = userdata.dealerCode;
                    List<long> folowupDataId = new List<long>();

                    List<FollowUpNotificationModel> followupdata = new List<FollowUpNotificationModel>();

                    bool role = true;

                    if (typeOfDipo == "insurance")
                    {
                        followupdata = db.Database.SqlQuery<FollowUpNotificationModel>("call insurance_followUp_notifications(@id,@dealerCode);", new MySqlParameter[] { new MySqlParameter("@id", userId), new MySqlParameter("@dealerCode", dealerCode) }).ToList();
                        //followupdata = insurRepo.getFollowupNotificationOfInsu(userdata.getId(), getUserLogindealerCode());
                        role = true;
                        ViewBag.role = role;
                    }
                    else
                    {
                        followupdata = db.Database.SqlQuery<FollowUpNotificationModel>("call FollowUpFilter(@id,@dealerCode);", new MySqlParameter[] { new MySqlParameter("@id", userId), new MySqlParameter("@dealerCode", dealerCode) }).ToList();
                        role = false;
                        ViewBag.role = role;
                    }

                    foreach (FollowUpNotificationModel sa in followupdata)
                    {

                        folowupDataId.Add(sa.callInteraction_id);
                        ViewBag.role = role;

                    }


                    if (folowupDataId != null)
                    {
                        followUpdata = db.callinteractions.Include("vehicle").Include("insurancedispositions").Include("customer").Include("customer.phones").Include("appointmentbooked").Include("servicebooked").Include("srdispositions").Where(m => folowupDataId.Contains(m.id)).ToList();

                    }
                }

                return View(followUpdata);
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message.ToString();
                if (ex.Message.Contains("inner exception"))
                {
                    ViewBag.error = ex.InnerException.Message;
                }
            }
            return View(followUpdata);
        }





        [HttpGet]
        [ActionName("changepasswordcre")]
        public ActionResult changePassowrd()
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
            return View();
            //return RedirectToAction("Index");
        }

        [HttpPost]
        [ActionName("changepasswordcre")]
        public ActionResult changePassowrdPost(changePassword passwords)
        {
            int userId = Convert.ToInt32(Session["UserId"].ToString());
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    if (db.wyzusers.Any(m => m.password == passwords.curPassword && m.id == userId))
                    {
                        wyzuser user = db.wyzusers.SingleOrDefault(m => m.password == passwords.curPassword && m.id == userId);
                        user.password = passwords.newPass;
                        db.wyzusers.AddOrUpdate(user);
                        db.SaveChanges();
                    }
                    else
                    {
                        TempData["NotMatch"] = true;
                        ViewBag.error = true;
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return RedirectToAction("LogOff");
        }


        //Insurance Notifocation
        [AuthorizeFilter]
        public ActionResult getInsuranceNotificationToday()
        {
            List<FollowUpNotificationModel> followUps = new List<FollowUpNotificationModel>();
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    int userId = Convert.ToInt32(Session["UserId"].ToString());
                    wyzuser userdata = new wyzuser();
                    userdata = db.wyzusers.SingleOrDefault(m => m.id == userId);
                    string dealerCode = userdata.dealerCode;

                    followUps = db.Database.SqlQuery<FollowUpNotificationModel>("call insurance_followUp_notifications(@id,@dealercode);", new MySqlParameter[] { new MySqlParameter("@id", userId), new MySqlParameter("@dealercode", dealerCode) }).ToList();

                    return Json(new { success = true, data = followUps });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message.ToString() });
            }
        }

        [AuthorizeFilter]
        public ActionResult redirectToCallLogging(string id)
        {

            try
            {
                if (id.Contains(','))
                {
                    string pageRequestFor = id.Split(',')[1];
                    Session["inComingParameter"] = null;

                    string userParameter = string.Empty;
                    if (pageRequestFor == "S" || pageRequestFor == "I")
                    {
                        userParameter = id.Split(',')[2];
                        userParameter = userParameter + "," + id.Split(',')[3];

                        Session["inComingParameter"] = id;
                        return RedirectToAction("customer360", "customer360", new { id = userParameter });
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Exceptions"] = ex.Message.ToString();
                if (ex.Message.Contains("inner exception"))
                {
                    TempData["Exceptions"] = ex.InnerException.Message;
                }
                return RedirectToAction("LogOff", "Home");
            }
            TempData["Exceptions"] = "Invalid Url Operation....";
            return RedirectToAction("LogOff");
        }

        #region user log book
        //public async Task recordLogDetails(int operation, string username, string managername, string dealercode, long wyzuserId, string curSession)
        //{
        //    try
        //    {

        //        if (operation == 1)//Login
        //        {
        //            userlogs user = new userlogs();

        //            user.wyzuserId = wyzuserId;
        //            user.username = username;
        //            user.managername = managername;
        //            user.dealerCode = dealercode;
        //            user.loginDateTime = DateTime.Now;
        //            user.sessionid = curSession;
        //            user.hostaddress = Request.UserHostAddress;
        //            using (var db = new AutoSherDBContext())
        //            {
        //                db.userlogs.Add(user);
        //                db.SaveChanges();
        //            }
        //        }
        //        else if (operation == 2)//LogOut
        //        {
        //            using (var db = new AutoSherDBContext())
        //            {
        //                userlogs user = new userlogs();
        //                if (db.userlogs.Any(m => m.sessionid == curSession && m.logoutDateTime == null))
        //                {
        //                    user = db.userlogs.FirstOrDefault(m => m.sessionid == curSession && m.logoutDateTime == null);
        //                    user.logoutDateTime = DateTime.Now;
        //                    db.userlogs.AddOrUpdate(user);
        //                    db.SaveChanges();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        #endregion

        #region Login firebase
        public async Task<bool> getAuthenticateUserLogin(string DealerCode)
        {
            try
            {
                //dynamic data = new JObject();
                using (var db = new AutoSherDBContext())
                {
                    string baseURL = string.Empty;
                   
                    if (HomeController.autosherpa1.Contains(DealerCode))
                    {
                        baseURL = System.Configuration.ConfigurationManager.AppSettings["FireBase_autosherpa1"];
                    }
                  
                    var firebaseBaseURL = new FirebaseClient(baseURL);
                    string Url = DealerCode + "_Authentication/";

                    var firebasevalue = await firebaseBaseURL.Child(Url).OnceAsync<FireBaseAuthentication>();
                    FireBaseAuthentication authentic = (from x in firebasevalue select x.Object).FirstOrDefault();
                    if (authentic.userLogin == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                string exception;

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
                    exception = "Line: " + ex.StackTrace.Split(':')[(ex.StackTrace.Split(':').Count() - 1)] + " " + exception;
                }
                // return Json(new { success = false, exception });
                return false;
            }

        }

        #endregion

        //[AuthorizeFilter]
        //public ActionResult getInsuranceNotificationAppointmentDate()
        //{
        //    List<AppointmentNotificationModel> followUpss = new List<AppointmentNotificationModel>();
        //    try
        //    {
        //        using (var db = new AutoSherDBContext())
        //        {
        //            int userId = Convert.ToInt32(Session["UserId"].ToString());
        //            //int userId = 308;
        //            dealerusers dealerId = new dealerusers();
        //            dealerId = db.Dealerusers.SingleOrDefault(m => m.WyzuserID == userId);
        //            int dealerCode = (int)dealerId.DealerID;
        //            //appointmentbooked appointmentbooked = new appointmentbooked();
        //            //appointmentbooked = db.appointmentbookeds.SingleOrDefault(m => m.wyzuser_id == );
        //            //string inappointmentdate = appointmentbooked.appointmentDate.ToString();
        //            //string inappointmenttime = appointmentbooked.appointmentFromTime;
        //            //string inappointmentdate = DateTime.Now.ToString("yyyy-MM-dd");
        //            //string inappointmenttime = DateTime.Now.ToString("HH:mm");

        //            followUpss = db.Database.SqlQuery<AppointmentNotificationModel>("insurance_appointment_notifications(@id,@dealercode,@inappointmentdate,@inappointmenttime);",
        //                new MySqlParameter[] { new MySqlParameter("@id", userId), 
        //                new MySqlParameter("@dealercode", dealerCode),
        //                new MySqlParameter("@inappointmentdate", inappointmentdate),
        //                new MySqlParameter("@inappointmenttime", inappointmenttime) }).ToList();

        //            return Json(new { success = true, data = followUpss });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, error = ex.Message.ToString() });
        //    }
        //}


        [AuthorizeFilter]
        public ActionResult getInsuranceNotificationAppointmentDate()
        {
            List<AppointmentNotificationModel> followUpss = new List<AppointmentNotificationModel>();
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    int userId = Convert.ToInt32(Session["UserId"].ToString());
                    wyzuser userdata = new wyzuser();
                    userdata = db.wyzusers.SingleOrDefault(m => m.id == userId);
                    string dealerCode = userdata.dealerCode;

                    followUpss = db.Database.SqlQuery<AppointmentNotificationModel>("insurance_appointment_notifications(@id,@dealercode);", new MySqlParameter[] { new MySqlParameter("@id", userId), new MySqlParameter("@dealercode", dealerCode) }).ToList();

                    return Json(new { success = true, data = followUpss });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message.ToString() });
            }
        }

        //public string WalkInCustomerLink(long? producerCode)
        //{
        //    try
        //    {
        //        long? heroProducerCode = producerCode;

        //        if (heroProducerCode.HasValue && heroProducerCode.Value != 0)
        //        {
        //            var inputData = new
        //            {
        //                targetLob = "motor",
        //                intent = "BUY_2W_LANDING",
        //                producerCode = heroProducerCode.Value
        //            };

        //            string jsonData = JsonConvert.SerializeObject(inputData);
        //            string salt = ConfigurationManager.AppSettings["EncryptionSalt"];
        //            string iv = ConfigurationManager.AppSettings["EncryptionIV"];

        //            string encryptedToken = EncryptAES256(salt, iv, jsonData);

        //            string walkInCustomerUrl = $"https://test10bn.tataaig.com/buy-online/redirection?src=2&token={encryptedToken}";

        //            return walkInCustomerUrl;
        //        }
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        private static string EncryptAES256(string salt, string ivHex, string jsonInputData)
        {
            try
            {
                byte[] key = Encoding.UTF8.GetBytes(salt);
                byte[] iv = StringToHexaByteArray(ivHex); //IV supports 16bytes Hexa

                using (var aes = Aes.Create())
                {
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    aes.Key = Encoding.UTF8.GetBytes(salt);
                    aes.IV = iv;

                    using (var encryptor = aes.CreateEncryptor())
                    using (var memoryStream = new MemoryStream())
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        byte[] dataBytes = Encoding.UTF8.GetBytes(jsonInputData);
                        cryptoStream.Write(dataBytes, 0, dataBytes.Length);
                        cryptoStream.FlushFinalBlock();

                        byte[] encryptedBytes = memoryStream.ToArray();
                        return $"{ivHex}:{BitConverter.ToString(encryptedBytes).Replace("-", "").ToLower()}";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Encryption failed", ex);
            }
        }

        private static byte[] StringToHexaByteArray(string hex)
        {
            int length = hex.Length;
            byte[] bytes = new byte[length / 2];
            for (int i = 0; i < length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }

    }


    public class FireBaseAuthentication
    {
        public int userLogin { get; set; }
    }
    public class changePassword
    {
        [Required]
        public string curPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string newPass { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string confirmPass { get; set; }
    }
}
