using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoSherpa_project.Models;
using AutoSherpa_project.Models.ViewModels;
using Microsoft.Ajax.Utilities;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace AutoSherpa_project.Controllers
{
    [AuthorizeFilter]
    public class InsuranceController : Controller
    {
        [ActionName("Insurance"), HttpGet]

        //[AuthorizeFilter]
        public ActionResult CallLog()
        {
            InsurenceViewModel insurenceLogVM = new InsurenceViewModel();

            Session["RoleFor"] = null;
            Session["isCallInitiated"] = null;
            Session["AndroidUniqueId"] = null;
            Session["GSMUniqueId"] = null;
            Session["PageFor"] = null;
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    int UserId = Convert.ToInt32(Session["UserId"].ToString());
                    var insCompany = db.insurancecompanies.Where(m => m.isactive == true).OrderBy(m => m.companyName).ToList();
                    var ddlcampaignList = db.campaigns.Where(m => (m.campaignType == "Forecast" || m.campaignType == "Insurance") && m.isactive == true).ToList();
                    var ddlmarketingcampaignList = db.Marketingcampaigns.Where(m => m.isactive == true).OrderBy(m => m.name).ToList();
                    var ddlpriorityLists = db.priority.Where(m => m.isactive == true).OrderBy(m => m.name).ToList();
                    var ddlnotIntReasons = db.notinterestedreasons.Where(m => m.isactive == true).ToList();
                    ViewBag.ddlnotIntReason = ddlnotIntReasons;
                    ViewBag.ddlcampaignList = ddlcampaignList;
                    ViewBag.ddlpriorityLists = ddlpriorityLists;
                    ViewBag.ddlmarketingcampaignList = ddlmarketingcampaignList;
                    ViewBag.ddlinsurancelist = insCompany;

                    var brands = db.brands.Where(m => m.isactive == true).ToList();
                    ViewBag.ddlBrandList = brands;

                    var models = db.models.Where(m => m.isActive == true).ToList();
                    ViewBag.ddlModleList = models;


                    var segments = db.segments.ToList();
                    ViewBag.ddlSegmentList = segments;


                    if (TempData["SubmissionResult"] != null)
                    {
                        if (TempData["SubmissionResult"].ToString() != "True")
                        {
                            ViewBag.dispositionResult = TempData["SubmissionResult"].ToString();
                            ViewBag.dispoError = true;
                        }
                        else
                        {
                            ViewBag.dispoError = false;
                            ViewBag.dispositionResult = "Disposition submitted";
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

                if (ex.StackTrace.Contains(":"))
                {
                    exception += ex.StackTrace.Split(':')[(ex.StackTrace.Split(':').Count() - 1)];
                }

                TempData["Exceptions"] = exception;
                TempData["ControllerName"] = "Insurance";

                return RedirectToAction("LogOff", "Home");
            }
            return View(insurenceLogVM);
        }

        #region Dashboard Count

        public ActionResult DashBoard()
        {
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    string userRole = Session["UserRole"].ToString();
                    long userId = long.Parse(Session["UserId"].ToString());
                    if (userRole == "CREManager" || userRole == "Admin" || userRole == "Others" || userRole == "CRE")
                    {


                    }
                }
            }
            catch (Exception ex)
            {

            }

            return View();
        }

        public ActionResult loadDashCounts()
        {
            int FreeCalls, pendingFollow, overDue, totalContacts, NonContacts, totalCall, totalAttemptedPerDay, totalRedFlag, attemptsIns, pending, totalAppointments, toatalConversions;
            try
            {
                int UserId = Convert.ToInt32(Session["UserId"].ToString());
                using (var db = new AutoSherDBContext())
                {
                    FreeCalls = db.Database.SqlQuery<int>("call insuranceDashboardCounts(@wyzUserId,@boardId);", new MySqlParameter[] { new MySqlParameter("@wyzUserId", UserId), new MySqlParameter("@boardId", 1) }).FirstOrDefault();
                    pendingFollow = db.Database.SqlQuery<int>("call insuranceDashboardCounts(@wyzUserId,@boardId);", new MySqlParameter[] { new MySqlParameter("@wyzUserId", UserId), new MySqlParameter("@boardId", 2) }).FirstOrDefault();
                    NonContacts = db.Database.SqlQuery<int>("call insuranceDashboardCounts(@wyzUserId,@boardId);", new MySqlParameter[] { new MySqlParameter("@wyzUserId", UserId), new MySqlParameter("@boardId", 3) }).FirstOrDefault();
                    overDue = db.Database.SqlQuery<int>("call insuranceDashboardCounts(@wyzUserId,@boardId);", new MySqlParameter[] { new MySqlParameter("@wyzUserId", UserId), new MySqlParameter("@boardId", 4) }).FirstOrDefault();
                    totalRedFlag = db.Database.SqlQuery<int>("call insuranceDashboardCounts(@wyzUserId,@boardId);", new MySqlParameter[] { new MySqlParameter("@wyzUserId", UserId), new MySqlParameter("@boardId", 5) }).FirstOrDefault();
                    attemptsIns = db.Database.SqlQuery<int>("call insuranceDashboardCounts(@wyzUserId,@boardId);", new MySqlParameter[] { new MySqlParameter("@wyzUserId", UserId), new MySqlParameter("@boardId", 6) }).FirstOrDefault();
                    pending = db.Database.SqlQuery<int>("call insuranceDashboardCounts(@wyzUserId,@boardId);", new MySqlParameter[] { new MySqlParameter("@wyzUserId", UserId), new MySqlParameter("@boardId", 7) }).FirstOrDefault();
                    totalAppointments = db.Database.SqlQuery<int>("call insuranceDashboardCounts(@wyzUserId,@boardId);", new MySqlParameter[] { new MySqlParameter("@wyzUserId", UserId), new MySqlParameter("@boardId", 8) }).FirstOrDefault();
                    totalContacts = db.Database.SqlQuery<int>("call insuranceDashboardCounts(@wyzUserId,@boardId);", new MySqlParameter[] { new MySqlParameter("@wyzUserId", UserId), new MySqlParameter("@boardId", 9) }).FirstOrDefault();
                    totalCall = db.Database.SqlQuery<int>("call insuranceDashboardCounts(@wyzUserId,@boardId);", new MySqlParameter[] { new MySqlParameter("@wyzUserId", UserId), new MySqlParameter("@boardId", 10) }).FirstOrDefault();
                    totalAttemptedPerDay = db.Database.SqlQuery<int>("call insuranceDashboardCounts(@wyzUserId,@boardId);", new MySqlParameter[] { new MySqlParameter("@wyzUserId", UserId), new MySqlParameter("@boardId", 11) }).FirstOrDefault();
                    toatalConversions = db.Database.SqlQuery<int>("call insuranceDashboardCounts(@wyzUserId,@boardId);", new MySqlParameter[] { new MySqlParameter("@wyzUserId", UserId), new MySqlParameter("@boardId", 12) }).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                string exception;
                if (ex.Message.Contains("inner exception"))
                {
                    if (ex.InnerException.Message.Contains("inner exception"))
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

            return Json(new { success = true, FreeCalls, pendingFollow, NonContacts, overDue, totalRedFlag, attemptsIns, pending, totalAppointments, totalContacts, totalCall, totalAttemptedPerDay, toatalConversions });
        }
        #endregion

        #region Getting Bucket Data
        public ActionResult GetBucketData(string insurenceData)
        {
            string icName, instatus, ExpiryfromDate, ExpirytoDate, renewalType, flag, campaign, orderFilter, policyType, fuelType, segment, oem, appointmentType, notIntReasons, nonContactReasons;
            int marketingCampaign, priority;
            string exception = "";
            List<insuranceBuketData> insuracecallLogs = null;

            int fromIndex = Convert.ToInt32(Request["start"]);
            int toIndex = Convert.ToInt32(Request["length"]);
            string searchPattern = Request["search[value]"].Trim();

            //Find Order Column
            var insortfilter = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortOrder = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            orderFilter = insortfilter + "_" + sortOrder;

            int UserId = Convert.ToInt32(Session["UserId"].ToString());

            InsurenceFilter filter = new InsurenceFilter();
            if (insurenceData != null)
            {
                filter = JsonConvert.DeserializeObject<InsurenceFilter>(insurenceData);
            }

            int totalCount = 0;
            long patternCount = 0;

            if (searchPattern != "")
            {
                filter.isFiltered = true;
            }
            icName = filter.icName == null ? "" : filter.icName;

            ExpiryfromDate = filter.exipiryFromDate == null ? "" : Convert.ToDateTime(filter.exipiryFromDate.ToString()).ToString("yyyy-MM-dd");
            ExpirytoDate = filter.exipiryToDate == null ? "" : Convert.ToDateTime(filter.exipiryToDate.ToString()).ToString("yyyy-MM-dd");
            flag = filter.Flag == null ? "" : filter.Flag;
            campaign = filter.Campaign == null ? "" : filter.Campaign;
            renewalType = filter.renewalType == null ? "" : filter.renewalType;
            marketingCampaign = filter.marketingCampaign == null || filter.marketingCampaign == "" ? 0 : Convert.ToInt32(filter.marketingCampaign);
            priority = filter.priority == null || filter.priority == "" ? 0 : Convert.ToInt32(filter.priority);
            policyType = filter.policyType == null ? "" : filter.policyType;
            fuelType = filter.fuelType == null ? "" : filter.fuelType;
            segment = filter.segment == null ? "" : filter.segment;
            oem = filter.oem == null ? "" : filter.oem;
            appointmentType = filter.appointmentType == null ? "" : filter.appointmentType;
            notIntReasons = filter.notinterestedreason == null ? "" : filter.notinterestedreason;
            nonContactReasons = filter.nonContactReasons == null ? "" : filter.nonContactReasons;
            instatus = filter.status == null ? "" : filter.status;


            using (var db = new AutoSherDBContext())
            {
                try
                {
                    if (filter.getDataFor == 1) //Scheduled Calls
                    {

                        totalCount = getScheduledBucketCount(UserId, "", "", "", "", flag, "", 0, 0, "", "", "", "");
                        if (ExpiryfromDate == "" && ExpirytoDate == "" && renewalType == "" && searchPattern == "" && campaign == "" && marketingCampaign == 0 && priority == 0 && policyType == "" && fuelType == "" && segment == "" && oem == "" && instatus == "")
                        {
                            filter.isFiltered = false;
                        }
                        if (toIndex < 0)
                        {
                            toIndex = 10;
                        }
                        if (toIndex > totalCount)
                        {
                            toIndex = totalCount;
                        }
                        insuracecallLogs = getScheduledCall(UserId, ExpiryfromDate, ExpirytoDate, searchPattern, renewalType, flag, campaign, marketingCampaign, priority, orderFilter, fromIndex, toIndex, policyType, fuelType, segment, oem, instatus);
                        if (filter.isFiltered == true)
                        {
                            patternCount = getScheduledBucketCount(UserId, ExpiryfromDate, ExpirytoDate, searchPattern, renewalType, flag, campaign, marketingCampaign, priority, policyType, fuelType, segment, oem);
                        }
                    }
                    else if (filter.getDataFor == 2) //Pending FollowUp
                    {

                        totalCount = getBucket234Count(4, UserId, "", "", "", "", "", "", "", 0, 0, "", "", "", "", "", "");
                        if (ExpiryfromDate == "" && ExpirytoDate == "" && renewalType == "" && searchPattern == "" && campaign == "" && marketingCampaign == 0 && priority == 0 && fuelType == "" && segment == "" && oem == "" && fuelType == "" && instatus == "")
                        {
                            filter.isFiltered = false;
                        }
                        if (toIndex < 0)
                        {
                            toIndex = 10;
                        }

                        if (toIndex > totalCount)
                        {
                            toIndex = totalCount;
                        }
                        insuracecallLogs = getBucket234(4, UserId, icName, ExpiryfromDate, ExpirytoDate, searchPattern, renewalType, flag, campaign, marketingCampaign, priority, orderFilter, fromIndex, toIndex, policyType, appointmentType, segment, oem, notIntReasons, fuelType, instatus);

                        if (filter.isFiltered == true)
                        {
                            patternCount = getBucket234Count(4, UserId, icName, ExpiryfromDate, ExpirytoDate, searchPattern, renewalType, flag, campaign, marketingCampaign, priority, policyType, appointmentType, segment, oem, notIntReasons, fuelType);
                        }

                    }
                    else if (filter.getDataFor == 3)//Appointments
                    {

                        totalCount = getBucket234Count(25, UserId, "", "", "", "", "", "", "", 0, 0, "", "", "", "", "", "");
                        if (ExpiryfromDate == "" && ExpirytoDate == "" && renewalType == "" && searchPattern == "" && campaign == "" && marketingCampaign == 0 && priority == 0 && policyType == "" && appointmentType == "" && segment == "" && oem == "" && fuelType == "" && instatus == "")
                        {
                            filter.isFiltered = false;
                        }
                        if (toIndex < 0)
                        {
                            toIndex = 10;
                        }

                        if (toIndex > totalCount)
                        {
                            toIndex = totalCount;
                        }
                        insuracecallLogs = getBucket234(25, UserId, icName, ExpiryfromDate, ExpirytoDate, searchPattern, renewalType, flag, campaign, marketingCampaign, priority, orderFilter, fromIndex, toIndex, policyType, appointmentType, segment, oem, notIntReasons, fuelType, instatus);
                        if (filter.isFiltered == true)
                        {
                            patternCount = getBucket234Count(25, UserId, icName, ExpiryfromDate, ExpirytoDate, searchPattern, renewalType, flag, campaign, marketingCampaign, priority, policyType, appointmentType, segment, oem, notIntReasons, fuelType);
                        }
                    }
                    else if (filter.getDataFor == 4) //Renewal not required
                    {


                        totalCount = getBucket234Count(26, UserId, "", "", "", "", "", "", "", 0, 0, "", "", "", "", "", "");
                        if (ExpiryfromDate == "" && ExpirytoDate == "" && renewalType == "" && searchPattern == "" && campaign == "" && marketingCampaign == 0 && priority == 0 && notIntReasons == "" && fuelType == "" && segment == "" && oem == "" && instatus == "")
                        {
                            filter.isFiltered = false;
                        }
                        if (toIndex < 0)
                        {
                            toIndex = 10;
                        }

                        if (toIndex > totalCount)
                        {
                            toIndex = totalCount;
                        }
                        insuracecallLogs = getBucket234(26, UserId, icName, ExpiryfromDate, ExpirytoDate, searchPattern, renewalType, flag, campaign, marketingCampaign, priority, orderFilter, fromIndex, toIndex, policyType, appointmentType, segment, oem, notIntReasons, fuelType, instatus);
                        if (filter.isFiltered == true)
                        {
                            patternCount = getBucket234Count(26, UserId, icName, ExpiryfromDate, ExpirytoDate, searchPattern, renewalType, flag, campaign, marketingCampaign, priority, policyType, appointmentType, segment, oem, notIntReasons, fuelType);
                        }
                    }

                    else if (filter.getDataFor == 5) //NonContacts
                    {
                        totalCount = getnoncontactBucketCount(UserId, "", "", "", "", "", "", "", 0, 0, "", "", "", "", "");
                        if (ExpiryfromDate == "" && ExpirytoDate == "" && renewalType == "" && searchPattern == "" && campaign == "" && marketingCampaign == 0 && priority == 0 && policyType == "" && segment == "" && oem == "" && fuelType == "" && nonContactReasons == "" && instatus == "")
                        {
                            filter.isFiltered = false;
                        }
                        if (toIndex < 0)
                        {
                            toIndex = 10;
                        }
                        if (toIndex > totalCount)
                        {
                            toIndex = totalCount;
                        }

                        insuracecallLogs = getNonContacts(1, UserId, icName, ExpiryfromDate, ExpirytoDate, searchPattern, renewalType, flag, campaign, marketingCampaign, priority, orderFilter, fromIndex, toIndex, policyType, segment, oem, fuelType, nonContactReasons, instatus);
                        if (filter.isFiltered == true)
                        {
                            patternCount = getnoncontactBucketCount(UserId, icName, ExpiryfromDate, ExpirytoDate, searchPattern, renewalType, flag, campaign, marketingCampaign, priority, policyType, segment, oem, fuelType, nonContactReasons);
                        }

                    }
                    //else if (filter.getDataFor == 6) //N+1 Day
                    //{

                    //     totalCount = getNthand1Count(1, UserId,"","","","","");//en error...
                    //    if (campaignName == "" && ExpiryfromDate == "" && ExpirytoDate == "" && searchPattern == "" && renewalType == "")
                    //    {
                    //        filter.isFiltered = false;
                    //    }
                    //    if (toIndex < 0)
                    //    {
                    //        toIndex = 10;
                    //    }

                    //    if (toIndex > totalCount)
                    //    {
                    //        toIndex = totalCount;
                    //    }

                    //    insuracecallLogs = getNthand1(1, UserId, campaignName,  ExpiryfromDate, ExpirytoDate, searchPattern, renewalType,  fromIndex, toIndex);
                    //    if (filter.isFiltered == true)
                    //    {
                    //        patternCount = getNthand1(1, UserId, campaignName, ExpiryfromDate, ExpirytoDate, searchPattern, renewalType, 0, totalCount).Count;
                    //    }
                    //}
                    //else if (filter.getDataFor == 7)//Nth Day
                    //{

                    //    totalCount = getNthand1Count(2, UserId, "", "", "", "", "");
                    //    if (campaignName == "" && ExpiryfromDate == "" && ExpirytoDate == "" && searchPattern == "" && renewalType == "")
                    //    {
                    //        filter.isFiltered = false;
                    //    }
                    //    if (toIndex < 0)
                    //    {
                    //        toIndex = 10;
                    //    }

                    //    if (toIndex > totalCount)
                    //    {
                    //        toIndex = totalCount;
                    //    }

                    //    insuracecallLogs = getNthand1(2, UserId, campaignName, ExpiryfromDate, ExpirytoDate, searchPattern, renewalType, fromIndex, toIndex);
                    //    if (filter.isFiltered == true)
                    //    {
                    //        patternCount = getNthand1(2, UserId, campaignName, ExpiryfromDate, ExpirytoDate, searchPattern, renewalType, 0, totalCount).Count;
                    //    }
                    //}
                    else if (filter.getDataFor == 8) //Future Followup
                    {

                        totalCount = getfuturefollowupBucketCount(UserId, "", "", "", "", "", "", "", 0, 0, "", "", "", "");
                        if (icName == "" && ExpiryfromDate == "" && ExpirytoDate == "" && renewalType == "" && searchPattern == "" && campaign == "" && marketingCampaign == 0 && priority == 0 && policyType == "" && segment == "" && oem == "" && fuelType == "" && instatus == "")
                        {
                            filter.isFiltered = false;
                        }
                        if (toIndex < 0)
                        {
                            toIndex = 10;
                        }

                        if (toIndex > totalCount)
                        {
                            toIndex = totalCount;
                        }

                        insuracecallLogs = getFutureFollow(UserId, icName, ExpiryfromDate, ExpirytoDate, searchPattern, renewalType, flag, campaign, marketingCampaign, priority, orderFilter, fromIndex, toIndex, policyType, segment, oem, fuelType, instatus);
                        if (filter.isFiltered == true)
                        {
                            patternCount = getfuturefollowupBucketCount(UserId, icName, ExpiryfromDate, ExpirytoDate, searchPattern, renewalType, flag, campaign, marketingCampaign, priority, policyType, segment, oem, fuelType);
                        }
                    }
                    else if (filter.getDataFor == 9) //Due Today
                    {

                        totalCount = getdueTodayBucketCount(UserId, "", "", "", "", "", "", "", 0, 0, "", "", "", "");
                        if (icName == "" && ExpiryfromDate == "" && ExpirytoDate == "" && renewalType == "" && searchPattern == "" && campaign == "" && marketingCampaign == 0 && priority == 0 && instatus == "")
                        {
                            filter.isFiltered = false;
                        }
                        if (toIndex < 0)
                        {
                            toIndex = 10;
                        }

                        if (toIndex > totalCount)
                        {
                            toIndex = totalCount;
                        }

                        insuracecallLogs = getdueToday(UserId, icName, ExpiryfromDate, ExpirytoDate, searchPattern, renewalType, flag, campaign, marketingCampaign, priority, orderFilter, fromIndex, toIndex, policyType, segment, oem, fuelType, instatus);
                        if (filter.isFiltered == true)
                        {
                            patternCount = getdueTodayBucketCount(UserId, icName, ExpiryfromDate, ExpirytoDate, searchPattern, renewalType, flag, campaign, marketingCampaign, priority, policyType, segment, oem, fuelType);
                        }
                    }


                }
                catch (Exception ex)
                {

                    if (ex.Message.Contains("inner exception"))
                    {
                        exception = ex.InnerException.Message;
                    }
                    else
                    {
                        exception = ex.Message;
                    }
                }
            }

            if (insuracecallLogs != null)
            {
                if (filter.isFiltered == true)
                {
                    var JsonData = Json(new { data = insuracecallLogs, draw = Request["draw"], recordsTotal = totalCount, recordsFiltered = patternCount, exception = exception }, JsonRequestBehavior.AllowGet);
                    JsonData.MaxJsonLength = int.MaxValue;
                    return JsonData;
                }
                else if (filter.isFiltered == false)
                {
                    var JsonData = Json(new { data = insuracecallLogs, draw = Request["draw"], recordsTotal = totalCount, recordsFiltered = totalCount, exception = exception }, JsonRequestBehavior.AllowGet);
                    JsonData.MaxJsonLength = int.MaxValue;
                    return JsonData;
                }
            }

            return Json(new { data = "", draw = Request["draw"], recordsTotal = 0, recordsFiltered = 0, exception = exception }, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Scheduled Call Bucket
        public int getScheduledBucketCount(long UserId, string ExpiryfromDate, string ExpirytoDate, string searchPattern, string renewalType, string flag, string campaign, int marketingCampaign, int priority, string policyType, string fuelType, string segment, string oem)
        {
            int totalCount = 0;
            using (var db = new AutoSherDBContext())
            {
                totalCount = db.Database.SqlQuery<int>("call insuranceScheduledCallsCount(@wyzuserid,@instartdate,@inenddate,@pattern,@inrenType,@inflag,@incampaign,@inmarketingcampaign,@inpriority,@inpolicyType,@infuelType,@insegment,@inoem)",
                            new MySqlParameter("@wyzuserid", UserId),
                            //new MySqlParameter("@inicname", icName),
                            new MySqlParameter("@instartdate", ExpiryfromDate),
                            new MySqlParameter("@inenddate", ExpirytoDate),
                            new MySqlParameter("@pattern", searchPattern),
                            new MySqlParameter("@inrenType", renewalType),
                            new MySqlParameter("@inflag", flag),
                            new MySqlParameter("@incampaign", campaign),
                            new MySqlParameter("@inmarketingcampaign", marketingCampaign),
                            new MySqlParameter("@inpriority", priority),
                            new MySqlParameter("@inpolicyType", policyType),
                            new MySqlParameter("@infuelType", fuelType),
                            new MySqlParameter("@insegment", segment),
                            new MySqlParameter("@inoem", oem)

                            ).FirstOrDefault();

                return totalCount;
            }
        }
        public List<insuranceBuketData> getScheduledCall(long id, string fromexpirydaterange, string toexpirydaterange, string searchPattern, string renewalType, string flag, string campaign, int marketingCampaign, int priority, string orderFilter, long fromIndex, long toIndex, string policyType, string fuelType, string segment, string oem, string status)
        {
            List<insuranceBuketData> callLogsInsurence = null;
            using (var db = new AutoSherDBContext())
            {
                string str = @"CALL insuranceScheduledCalls(@wyzuserid,@instartdate,@inenddate,@pattern,@inrenType,@inflag,@incampaign,@inmarketingcampaign,@inpriority,@insortfilter,@start_with,@length,@inpolicyType,@infuelType,@insegment,@inoem,@instatus)";

                MySqlParameter[] sqlParameter = new MySqlParameter[]
                {
                        new MySqlParameter("@wyzuserid", id),
                         
                            //new MySqlParameter("@inicname", icName),
                            new MySqlParameter("@instartdate", fromexpirydaterange),
                            new MySqlParameter("@inenddate", toexpirydaterange),
                            new MySqlParameter("@pattern", searchPattern),
                            new MySqlParameter("@inrenType", renewalType),
                            new MySqlParameter("@inflag", flag),
                            new MySqlParameter("@incampaign", campaign),
                            new MySqlParameter("@inmarketingcampaign", marketingCampaign),
                            new MySqlParameter("@inpriority", priority),
                            new MySqlParameter("@insortfilter", orderFilter),
                            new MySqlParameter("start_with", fromIndex),
                            new MySqlParameter("length", toIndex),
                            new MySqlParameter("@inpolicyType", policyType),
                            new MySqlParameter("@infuelType", fuelType),
                            new MySqlParameter("@insegment", segment),
                            new MySqlParameter("@inoem", oem),
                            new MySqlParameter("@instatus",status)
                };
                callLogsInsurence = db.Database.SqlQuery<insuranceBuketData>(str, sqlParameter).ToList();
            }
            return callLogsInsurence;
        }
        #endregion

        #region Pending Followup,Appointments,Renewal Not Required Bucket 
        public int getBucket234Count(long dispositiontype, long wyzuserid, string icName, string expiryFromDate, string expiryToDate, string pattern, string renewalType, string flag, string campaign, int marketingCampaign, int priority, string policyType, string appointmentType, string segment, string oem, string reasons, string fuelType)
        {
            int totalCount = 0;
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    totalCount = db.Database.SqlQuery<int>("call insurancefilterforDispositionContactsCount" +
                          "(@dispositiontype,@wyzuserid,@inicname,@instartdate,@inenddate,@pattern,@inrenType,@inflag,@incampaign,@inmarketingcampaign,@inpriority,@inpolicyType,@inappointmentType,@insegment,@inoem,@notintreason,@fueltype)",
                          new MySqlParameter[] {
                            new MySqlParameter("@dispositiontype", dispositiontype),
                            new MySqlParameter("@wyzuserid",wyzuserid),
                            new MySqlParameter("@inicname", icName),
                            new MySqlParameter("@instartdate",expiryFromDate),
                            new MySqlParameter("@inenddate",expiryToDate),
                            new MySqlParameter("@pattern", pattern),
                            new MySqlParameter("@inrenType",renewalType),
                            new MySqlParameter("@inflag",flag),
                            new MySqlParameter("@incampaign",campaign),
                            new MySqlParameter("@inmarketingcampaign", marketingCampaign),
                            new MySqlParameter("@inpriority", priority),
                            new MySqlParameter("@inpolicyType", policyType),
                            new MySqlParameter("@inappointmentType", appointmentType),
                            new MySqlParameter("@insegment", segment),
                            new MySqlParameter("@inoem", oem),
                            new MySqlParameter("@notintreason", reasons),
                            new MySqlParameter("@fueltype", fuelType)
                          }).FirstOrDefault();

                    return totalCount;

                }
            }
            catch (Exception ex)
            {

            }
            return totalCount;

        }
        public List<insuranceBuketData> getBucket234(long typeOfdispo, long id, string icName, string expiryFromDate, string expiryToDate, string searchPattern, string renewalType, string flag, string campaign, int marketingCampaign, int priority, string orderFilter, long fromIndex, long toIndex, string policyType, string appointmentType, string segment, string oem, string reason, string fuelType, string status)
        {
            List<insuranceBuketData> dispositionLoadInsurances = new List<insuranceBuketData>();
            using (var db = new AutoSherDBContext())
            {
                string str = @"CALL insurancefilterforDispositionContacts(@dispositiontype,@wyzuserid,@inicname,@instartdate,@inenddate,@pattern,@inrenType,@inflag,@incampaign,@inmarketingcampaign,@inpriority,@insortfilter,@start_with,@length,@inpolicyType,@inappointmentType,@insegment,@inoem,@notintreason,@fueltype,@instatus);";

                MySqlParameter[] sqlParameter = new MySqlParameter[]
                {

                        new MySqlParameter("dispositiontype", typeOfdispo),
                        new MySqlParameter("wyzuserid", id),
                        new MySqlParameter("inicname", icName),
                        new MySqlParameter("instartdate", expiryFromDate),
                        new MySqlParameter("inenddate", expiryToDate),
                        new MySqlParameter("pattern", searchPattern),
                        new MySqlParameter("inrenType", renewalType),
                        new MySqlParameter("inflag", flag),
                        new MySqlParameter("incampaign", campaign),
                          new MySqlParameter("@inmarketingcampaign", marketingCampaign),
                            new MySqlParameter("@inpriority", priority),
                            new MySqlParameter("@insortfilter", orderFilter),
                        new MySqlParameter("start_with", fromIndex),
                         new MySqlParameter("length", toIndex),
                         new MySqlParameter("@inpolicyType", policyType),
                            new MySqlParameter("@inappointmentType", appointmentType),
                            new MySqlParameter("@insegment", segment),
                            new MySqlParameter("@inoem", oem),
                            new MySqlParameter("@notintreason", reason),
                            new MySqlParameter("@fueltype", fuelType),
                            new MySqlParameter("@instatus", status)
                };
                dispositionLoadInsurances = db.Database.SqlQuery<insuranceBuketData>(str, sqlParameter).ToList();
            }

            return dispositionLoadInsurances;
        }
        #endregion

        #region NonContact Bucket Details
        public int getnoncontactBucketCount(long UserId, string icName, string fromexpiryDate, string toexpiryDate, string searchPattern, string renewalType, string flag, string campaign, int marketingCampaign, int priority, string policyType, string segments, string oem, string fuelType, string reasons)
        {
            int totalCount = 0;
            using (var db = new AutoSherDBContext())
            {
                totalCount = db.Database.SqlQuery<int>("call insurancefilterforNonContactsCount( @dispositiontype,@wyzuserid,@inicname,@instartdate,@inenddate,@pattern,@inrenType,@inflag,@incampaign,@inmarketingcampaign,@inpriority,@inpolicyType,@insegment,@inoem,@fueltype,@reason)",
                       new MySqlParameter("@dispositiontype", 1),
                       new MySqlParameter("@wyzuserid", UserId),
                       new MySqlParameter("@inicname", icName),
                       new MySqlParameter("@instartdate", fromexpiryDate),
                       new MySqlParameter("@inenddate", toexpiryDate),
                      new MySqlParameter("@pattern", searchPattern),
                    new MySqlParameter("@inrenType", renewalType),
                      new MySqlParameter("inflag", flag),
                        new MySqlParameter("incampaign", campaign),
                            new MySqlParameter("@inmarketingcampaign", marketingCampaign),
                            new MySqlParameter("@inpriority", priority),
                         new MySqlParameter("@inpolicyType", policyType),
                             new MySqlParameter("@insegment", segments),
                            new MySqlParameter("@inoem", oem),
                            new MySqlParameter("@fueltype", fuelType),
                            new MySqlParameter("@reason", reasons)).FirstOrDefault();


                return totalCount;
            }
        }

        public List<insuranceBuketData> getNonContacts(long typeOfdispo, long id, string icName, string fromexpiryDate, string toexpiryDate, string searchPattern, string renewalType, string flag, string campaign, int marketingCampaign, int priority, string orderFilter, long fromIndex, long toIndex, string policyType, string segments, string oem, string fuelType, string reasons, string status)
        {
            List<insuranceBuketData> dispositionLoadInsurances = new List<insuranceBuketData>();

            using (var db = new AutoSherDBContext())
            {
                string str = @"CALL insurancefilterforNonContacts(@dispositiontype,@wyzuserid,@inicname,@instartdate,@inenddate,@pattern,@inrenType,@inflag,@incampaign,@inmarketingcampaign,@inpriority,@insortfilter,@start_with,@length,@inpolicyType,@insegment,@inoem,@fueltype,@reason,@instatus);";

                MySqlParameter[] sqlParameter = new MySqlParameter[]
                {
                        new MySqlParameter("dispositiontype", typeOfdispo),
                        new MySqlParameter("wyzuserid", id),
                        new MySqlParameter("inicname", icName),
                        new MySqlParameter("instartdate",fromexpiryDate),
                        new MySqlParameter("inenddate", toexpiryDate),
                        new MySqlParameter("pattern", searchPattern),
                        new MySqlParameter("inrenType", renewalType),
                          new MySqlParameter("inflag", flag),
                        new MySqlParameter("incampaign", campaign),
                          new MySqlParameter("@inmarketingcampaign", marketingCampaign),
                            new MySqlParameter("@inpriority", priority),
                            new MySqlParameter("@insortfilter", orderFilter),
                        new MySqlParameter("start_with", fromIndex),
                        new MySqlParameter("length", toIndex),
                         new MySqlParameter("@inpolicyType", policyType),
                             new MySqlParameter("@insegment", segments),
                            new MySqlParameter("@inoem", oem),
                            new MySqlParameter("@fueltype", fuelType),
                            new MySqlParameter("@reason", reasons),
                            new MySqlParameter("@instatus", status)
                };
                dispositionLoadInsurances = db.Database.SqlQuery<insuranceBuketData>(str, sqlParameter).ToList();
            }

            return dispositionLoadInsurances;
        }
        #endregion

        #region N+1 day and Nth Day Bucket Details
        //public int getNthand1Count(long sbday, long inChasers_id, string campaignName,  string instartdate, string inenddate, string pattern,string renewalType)
        //{
        //    int totalCount = 0;
        //    using (var db = new AutoSherDBContext())
        //    {
        //        totalCount = db.Database.SqlQuery<int>("call InsuranceAppointmentPreviousDayCallsCount(@sbday,@wyzuserid,@inicname,@instartdate,@inenddate,@pattern,@inrenType)",
        //                 new MySqlParameter[] {
        //                        new MySqlParameter("@sbday",sbday),
        //                        new MySqlParameter("@wyzuserid", inChasers_id),                              
        //                        new MySqlParameter("@inicname",campaignName),
        //                        new MySqlParameter("@instartdate",instartdate),
        //                        new MySqlParameter("@inenddate",inenddate),


        //                        new MySqlParameter("@pattern",pattern),
        //                         new MySqlParameter("@inrenType",renewalType),

        //                 }).FirstOrDefault();

        //        return totalCount;

        //    }
        //}

        //public List<insuranceBuketData> getNthand1(long typeofdispo, long chasser_id, string campaignName,  string fromexpirydaterange, string toexpirydaterange, string searchpattern, string renewalType,  long fromindex, long toindex)
        //{
        //    List<insuranceBuketData> calllogajaxloads = new List<insuranceBuketData>();
        //    using (var db = new AutoSherDBContext())
        //    {
        //        string str = @"call insuranceappointmentpreviousdaycalls(@sbday,@wyzuserid,@inicname,@instartdate,@inenddate,@pattern,@inrenType,@start_with,@length);";

        //        MySqlParameter[] sqlparameter = new MySqlParameter[]
        //        {
        //                new MySqlParameter("sbday", (int) typeofdispo),
        //                new MySqlParameter("wyzuserid", (int)chasser_id),

        //                new MySqlParameter("inicname", campaignName),

        //                new MySqlParameter("instartdate", fromexpirydaterange),
        //                new MySqlParameter("inenddate", toexpirydaterange),
        //                new MySqlParameter("pattern", searchpattern),
        //                new MySqlParameter("inrenType", renewalType),
        //                    new MySqlParameter("start_with", fromindex),
        //                     new MySqlParameter("length", toindex)


        //        };
        //        calllogajaxloads = db.Database.SqlQuery<insuranceBuketData>(str, sqlparameter).ToList();
        //    }

        //    return calllogajaxloads;
        //}
        #endregion

        #region Future FollowUp Bucket Details
        public int getfuturefollowupBucketCount(int UserId, string icName, string fromexpiryDate, string toexpiryDate, string searchPattern, string renewalType, string flag, string campaign, int marketingCampaign, int priority, string policyType, string segments, string oem, string fuelType)
        {
            int totalCount = 0;
            using (var db = new AutoSherDBContext())
            {
                totalCount = db.Database.SqlQuery<int>("call insurancefilterforFutureFollowupCount(@wyzuserid,@inicname,@instartdate,@inenddate,@pattern,@inrenType,@inflag,@incampaign,@inmarketingcampaign,@inpriority,@inpolicyType,@insegment,@inoem,@fueltype)",
                          new MySqlParameter[]
                          {
                                new MySqlParameter("@wyzuserid",UserId),
                                new MySqlParameter("@inicname",icName),
                                new MySqlParameter("@instartdate",fromexpiryDate),
                                new MySqlParameter("@inenddate",toexpiryDate),

                                new MySqlParameter("@pattern",searchPattern),
                                new MySqlParameter("@inrenType",renewalType),
                                  new MySqlParameter("inflag", flag),
                        new MySqlParameter("incampaign", campaign),
                            new MySqlParameter("@inmarketingcampaign", marketingCampaign),
                            new MySqlParameter("@inpriority", priority),
                             new MySqlParameter("@inpolicyType", policyType),
                             new MySqlParameter("@insegment", segments),
                            new MySqlParameter("@inoem", oem),
                            new MySqlParameter("@fueltype", fuelType)

                          }).FirstOrDefault();
                return totalCount;
            }
        }

        public List<insuranceBuketData> getFutureFollow(long id, string icName, string fromexpiryDate, string toexpiryDate, string searchPattern, string renewalType, string flag, string campaign, int marketingCampaign, int priority, string orderFilter, long fromIndex, long toIndex, string policyType, string segments, string oem, string fuelType, string status)
        {
            List<insuranceBuketData> dispositionLoadInsurances = new List<insuranceBuketData>();
            using (var db = new AutoSherDBContext())
            {
                string str = @"CALL insurancefilterforFutureFollowup( @wyzuserid,@inicname,@instartdate,@inenddate,@pattern, @inrenType,@inflag,@incampaign,@inmarketingcampaign,@inpriority,@insortfilter,@start_with,@length,@inpolicyType,@insegment,@inoem,@fueltype,@instatus);";

                MySqlParameter[] sqlParameter = new MySqlParameter[]
                {

                        new MySqlParameter("wyzuserid", id),
                        new MySqlParameter("inicname", icName),
                        new MySqlParameter("instartdate", fromexpiryDate),
                        new MySqlParameter("inenddate", toexpiryDate),

                        new MySqlParameter("pattern", searchPattern),
                        new MySqlParameter("inrenType", renewalType),
                        new MySqlParameter("inflag", flag),
                        new MySqlParameter("incampaign", campaign),
                          new MySqlParameter("@inmarketingcampaign", marketingCampaign),
                            new MySqlParameter("@inpriority", priority),
                            new MySqlParameter("@insortfilter", orderFilter),
                         new MySqlParameter("start_with", fromIndex),
                          new MySqlParameter("length", toIndex),
                           new MySqlParameter("@inpolicyType", policyType),
                             new MySqlParameter("@insegment", segments),
                            new MySqlParameter("@inoem", oem),
                            new MySqlParameter("@fueltype", fuelType),
                            new MySqlParameter("@@instatus", status)
            };
                dispositionLoadInsurances = db.Database.SqlQuery<insuranceBuketData>(str, sqlParameter).ToList();
            }
            return dispositionLoadInsurances;
        }
        #endregion

        #region Due Today Bucket Details
        public int getdueTodayBucketCount(int UserId, string icName, string fromexpiryDate, string toexpiryDate, string searchPattern, string renewalType, string flag, string campaign, int marketingCampaign, int priority, string policyType, string segments, string oem, string fuelType)
        {
            int totalCount = 0;
            using (var db = new AutoSherDBContext())
            {
                totalCount = db.Database.SqlQuery<int>("call insurancepolicyexprytodaycallscount(@wyzuserid,@inicname,@instartdate,@inenddate,@pattern,@inrenType,@inflag,@incampaign,@inmarketingcampaign,@inpriority,@inpolicyType,@insegment,@inoem,@fueltype)",
                          new MySqlParameter[]
                          {
                                new MySqlParameter("@wyzuserid",UserId),
                                new MySqlParameter("@inicname",icName),
                                new MySqlParameter("@instartdate",fromexpiryDate),
                                new MySqlParameter("@inenddate",toexpiryDate),

                                new MySqlParameter("@pattern",searchPattern),
                                new MySqlParameter("@inrenType",renewalType),
                                  new MySqlParameter("inflag", flag),
                        new MySqlParameter("incampaign", campaign),
                            new MySqlParameter("@inmarketingcampaign", marketingCampaign),
                            new MySqlParameter("@inpriority", priority),
                           new MySqlParameter("@inpolicyType", policyType),
                             new MySqlParameter("@insegment", segments),
                            new MySqlParameter("@inoem", oem),
                            new MySqlParameter("@fueltype", fuelType)

                          }).FirstOrDefault();
                return totalCount;
            }
        }

        public List<insuranceBuketData> getdueToday(long id, string icName, string fromexpiryDate, string toexpiryDate, string searchPattern, string renewalType, string flag, string campaign, int marketingCampaign, int priority, string orderFilter, long fromIndex, long toIndex, string policyType, string segments, string oem, string fuelType, string status)
        {
            List<insuranceBuketData> dispositionLoadInsurances = new List<insuranceBuketData>();
            using (var db = new AutoSherDBContext())
            {
                string str = @"CALL insurancepolicyexprytodaycalls( @wyzuserid,@inicname,@instartdate,@inenddate,@pattern, @inrenType,@inflag,@incampaign,@inmarketingcampaign,@inpriority,@insortfilter,@start_with,@length,@inpolicyType,@insegment,@inoem,@fueltype,@instatus);";

                MySqlParameter[] sqlParameter = new MySqlParameter[]
                {

                        new MySqlParameter("wyzuserid", id),
                        new MySqlParameter("inicname", icName),
                        new MySqlParameter("instartdate", fromexpiryDate),
                        new MySqlParameter("inenddate", toexpiryDate),

                        new MySqlParameter("pattern", searchPattern),
                        new MySqlParameter("inrenType", renewalType),
                        new MySqlParameter("inflag", flag),
                        new MySqlParameter("incampaign", campaign),
                          new MySqlParameter("@inmarketingcampaign", marketingCampaign),
                            new MySqlParameter("@inpriority", priority),
                            new MySqlParameter("@insortfilter", orderFilter),
                         new MySqlParameter("start_with", fromIndex),
                          new MySqlParameter("length", toIndex),
                           new MySqlParameter("@inpolicyType", policyType),
                             new MySqlParameter("@insegment", segments),
                            new MySqlParameter("@inoem", oem),
                            new MySqlParameter("@fueltype", fuelType),
                            new MySqlParameter("@instatus", status)
            };
                dispositionLoadInsurances = db.Database.SqlQuery<insuranceBuketData>(str, sqlParameter).ToList();
            }
            return dispositionLoadInsurances;
        }
        #endregion


        #region FilterCount
        public ActionResult getFilterCount(string insurenceData)
        {
            List<insurenceFilterCount> dispositionLoadInsurances = new List<insurenceFilterCount>();

            int UserId = Convert.ToInt32(Session["UserId"].ToString());

            InsurenceFilter filter = new InsurenceFilter();
            if (insurenceData != null)
            {
                filter = JsonConvert.DeserializeObject<InsurenceFilter>(insurenceData);
            }

            if (filter.isFiltered == true)
            {

                string icName, ExpiryfromDate, ExpirytoDate, flag, campaign, renewalType, priority, marketingCampaign;
                icName = filter.icName == null ? "" : filter.icName;
                ExpiryfromDate = filter.exipiryFromDate == null ? "" : Convert.ToDateTime(filter.exipiryFromDate.ToString()).ToString("yyyy-MM-dd");
                ExpirytoDate = filter.exipiryToDate == null ? "" : Convert.ToDateTime(filter.exipiryToDate.ToString()).ToString("yyyy-MM-dd");
                flag = filter.Flag == null ? "" : filter.Flag;
                campaign = filter.Campaign == null ? "" : filter.Campaign;
                renewalType = filter.renewalType == null ? "" : filter.renewalType;
                marketingCampaign = filter.marketingCampaign == null ? "" : filter.marketingCampaign;
                priority = filter.priority == null ? "" : filter.priority;


                //  if ((campId != 0 || tagIId != 0 || duetypeId != 0 || locId != 0 || modelIds != 0 || fromLSDDate != "" || toLSDDate != "" || fromDateNew != "" || toDateNew != "") && (filter.followUP_From_date == null && filter.followUP_To_Date == null && filter.Booked_From_date == null && filter.Booked_To_Date == null && filter.ServiceBooked_type == null && filter.BookingStatus == null && filter.LastDispostion == null && filter.lastcall_From_date == null && filter.lastcall_To_Date == null && filter.reasons == null && filter.droppedCount == null && filter.visit_Type == null))
                {
                    try
                    {
                        using (var db = new AutoSherDBContext())
                        {
                            string str = @"CALL insurance_filtered_counts(@inwyzuser_id,@incamp_id,@pfrmdate,@ptodate,@inflag,@inpriority_id,@inmarcketcapign,@inicname );";

                            MySqlParameter[] sqlParameter = new MySqlParameter[]
                            {
                        new MySqlParameter("inwyzuser_id", UserId),
                        new MySqlParameter("incamp_id", campaign),
                        new MySqlParameter("pfrmdate", ExpiryfromDate),
                        new MySqlParameter("ptodate", ExpirytoDate),
                        new MySqlParameter("inflag", flag),
                        new MySqlParameter("inpriority_id", priority),
                        //new MySqlParameter("inrentype", renewalType),
                        new MySqlParameter("inmarcketcapign", marketingCampaign),
                        new MySqlParameter("inicname", icName),
                        };
                            dispositionLoadInsurances = db.Database.SqlQuery<insurenceFilterCount>(str, sqlParameter).ToList();
                        }
                        return Json(new { success = true, filterls = dispositionLoadInsurances });
                    }
                    catch (Exception ex)
                    {
                        string counterexception;
                        if (ex.Message.Contains("inner exception"))
                        {
                            if (ex.InnerException.Message.Contains("inner exception"))
                            {
                                counterexception = ex.InnerException.InnerException.Message;
                            }
                            else
                            {
                                counterexception = ex.InnerException.Message;
                            }

                        }
                        else
                        {
                            counterexception = ex.Message;
                        }

                        return Json(new { success = false, error = counterexception });
                    }
                }
            }
            return Json(new { success = false, filterls = dispositionLoadInsurances });
        }
        #endregion

    }
}