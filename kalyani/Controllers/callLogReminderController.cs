using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoSherpa_project.Models;
using MySql.Data.MySqlClient;
using AutoSherpa_project.Models.ViewModels;
using Newtonsoft.Json;

namespace AutoSherpa_project.Controllers
{
    [AuthorizeFilter]
    public class callLogReminderController : Controller
    {
        // GET: callLogReminder

        #region  Insurance Reminder Starts
        public ActionResult insuranceReminder()
        {
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    int UserId = Convert.ToInt32(Session["UserId"].ToString());

                    var ddlCampaign = db.campaigns.Where(m => m.campaignType == "Insurance").Select(m => new { id = m.id, campaignName = m.campaignName }).ToList();
                    ViewBag.ddlCampaign = ddlCampaign;


                    var ddlCRE = db.wyzusers.Where(m => m.insuranceRole == true).Select(m => new { id = m.id, userName = m.userName }).ToList();
                    ViewBag.ddlCRE = ddlCRE;


                    var ddlmodelList = db.modelslists.Select(m => new { id = m.id, model = m.model }).ToList();
                    ViewBag.ddlmodelList = ddlmodelList;


                    var ddlsisposition = db.calldispositiondatas.Select(m => new { id = m.id, dispo = m.disposition }).ToList();
                    ViewBag.ddlsisposition = ddlsisposition;

                    var locationids = db.insuranceassignedinteractions.Where(m => m.assigned_manager_id == UserId).Select(m => m.location_id).ToList();
                    var ddllocation = db.locations.Where(m => locationids.Contains(m.cityId)).Select(m => new { id = m.cityId, location = m.name }).OrderBy(m => m.location).ToList();
                    ViewBag.ddllocation = ddllocation;


                    var renewalType = db.renewaltypes.Select(m => new { id = m.id, name = m.renewalTypeName }).OrderBy(m => m.name).ToList();
                    ViewBag.renewalType = renewalType;

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
            return View();
        }

        public ActionResult loadInsuranceDashboardCounts()
        {
            int FreshCallsInsCount, followupInsCount, nonContactsIns, overDueInsCount, totalRedFlag, attemptsIns, pendingIns, appointment, totalContactsInsCREperDay, totalCallsperDay;
            try
            {
                int UserId = Convert.ToInt32(Session["UserId"].ToString());
                string managerUsername = Session["UserName"].ToString();
                string roles = Session["UserRole"].ToString();

                using (var db = new AutoSherDBContext())
                {
                    db.Database.CommandTimeout = 900;
                    if (roles == "CREManager")
                    {
                        FreshCallsInsCount = db.Database.SqlQuery<int>("call insuranceDashboardCountsCREManager(@incremanager,@indashboardId);", new MySqlParameter[] { new MySqlParameter("@incremanager", managerUsername), new MySqlParameter("@indashboardId", 1) }).FirstOrDefault();

                        followupInsCount = db.Database.SqlQuery<int>("call insuranceDashboardCountsCREManager(@incremanager,@indashboardId);", new MySqlParameter[] { new MySqlParameter("@incremanager", managerUsername), new MySqlParameter("@indashboardId", 2) }).FirstOrDefault();

                        nonContactsIns = db.Database.SqlQuery<int>("call insuranceDashboardCountsCREManager(@incremanager,@indashboardId);", new MySqlParameter[] { new MySqlParameter("@incremanager", managerUsername), new MySqlParameter("@indashboardId", 3) }).FirstOrDefault();

                        overDueInsCount = db.Database.SqlQuery<int>("call insuranceDashboardCountsCREManager(@incremanager,@indashboardId);", new MySqlParameter[] { new MySqlParameter("@incremanager", managerUsername), new MySqlParameter("@indashboardId", 4) }).FirstOrDefault();

                        totalRedFlag = db.Database.SqlQuery<int>("call insuranceDashboardCountsCREManager(@incremanager,@indashboardId);", new MySqlParameter[] { new MySqlParameter("@incremanager", managerUsername), new MySqlParameter("@indashboardId", 5) }).FirstOrDefault();

                        attemptsIns = db.Database.SqlQuery<int>("call insuranceDashboardCountsCREManager(@incremanager,@indashboardId);", new MySqlParameter[] { new MySqlParameter("@incremanager", managerUsername), new MySqlParameter("@indashboardId", 6) }).FirstOrDefault();

                        pendingIns = db.Database.SqlQuery<int>("call insuranceDashboardCountsCREManager(@incremanager,@indashboardId);", new MySqlParameter[] { new MySqlParameter("@incremanager", managerUsername), new MySqlParameter("@indashboardId", 7) }).FirstOrDefault();

                        appointment = db.Database.SqlQuery<int>("call insuranceDashboardCountsCREManager(@incremanager,@indashboardId);", new MySqlParameter[] { new MySqlParameter("@incremanager", managerUsername), new MySqlParameter("@indashboardId", 8) }).FirstOrDefault();

                        totalContactsInsCREperDay = db.Database.SqlQuery<int>("call insuranceDashboardCountsCREManager(@incremanager,@indashboardId);", new MySqlParameter[] { new MySqlParameter("@incremanager", managerUsername), new MySqlParameter("@indashboardId", 9) }).FirstOrDefault();

                        totalCallsperDay = db.Database.SqlQuery<int>("call insuranceDashboardCountsCREManager(@incremanager,@indashboardId);", new MySqlParameter[] { new MySqlParameter("@incremanager", managerUsername), new MySqlParameter("@indashboardId", 10) }).FirstOrDefault();

                    }
                    else
                    {
                        FreshCallsInsCount = db.Database.SqlQuery<int>("call insuranceDashboardCountschanelManager(@incremanager,@indashboardId);", new MySqlParameter[] { new MySqlParameter("@incremanager", managerUsername), new MySqlParameter("@indashboardId", 1) }).FirstOrDefault();

                        followupInsCount = db.Database.SqlQuery<int>("call insuranceDashboardCountschanelManager(@incremanager,@indashboardId);", new MySqlParameter[] { new MySqlParameter("@incremanager", managerUsername), new MySqlParameter("@indashboardId", 2) }).FirstOrDefault();

                        nonContactsIns = db.Database.SqlQuery<int>("call insuranceDashboardCountschanelManager(@incremanager,@indashboardId);", new MySqlParameter[] { new MySqlParameter("@incremanager", managerUsername), new MySqlParameter("@indashboardId", 3) }).FirstOrDefault();

                        overDueInsCount = db.Database.SqlQuery<int>("call insuranceDashboardCountschanelManager(@incremanager,@indashboardId);", new MySqlParameter[] { new MySqlParameter("@incremanager", managerUsername), new MySqlParameter("@indashboardId", 4) }).FirstOrDefault();

                        totalRedFlag = db.Database.SqlQuery<int>("call insuranceDashboardCountschanelManager(@incremanager,@indashboardId);", new MySqlParameter[] { new MySqlParameter("@incremanager", managerUsername), new MySqlParameter("@indashboardId", 5) }).FirstOrDefault();

                        attemptsIns = db.Database.SqlQuery<int>("call insuranceDashboardCountschanelManager(@incremanager,@indashboardId);", new MySqlParameter[] { new MySqlParameter("@incremanager", managerUsername), new MySqlParameter("@indashboardId", 6) }).FirstOrDefault();

                        pendingIns = db.Database.SqlQuery<int>("call insuranceDashboardCountschanelManager(@incremanager,@indashboardId);", new MySqlParameter[] { new MySqlParameter("@incremanager", managerUsername), new MySqlParameter("@indashboardId", 7) }).FirstOrDefault();

                        appointment = db.Database.SqlQuery<int>("call insuranceDashboardCountschanelManager(@incremanager,@indashboardId);", new MySqlParameter[] { new MySqlParameter("@incremanager", managerUsername), new MySqlParameter("@indashboardId", 8) }).FirstOrDefault();

                        totalContactsInsCREperDay = db.Database.SqlQuery<int>("call insuranceDashboardCountschanelManager(@incremanager,@indashboardId);", new MySqlParameter[] { new MySqlParameter("@incremanager", managerUsername), new MySqlParameter("@indashboardId", 9) }).FirstOrDefault();

                        totalCallsperDay = db.Database.SqlQuery<int>("call insuranceDashboardCountschanelManager(@incremanager,@indashboardId);", new MySqlParameter[] { new MySqlParameter("@incremanager", managerUsername), new MySqlParameter("@indashboardId", 10) }).FirstOrDefault();

                    }
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

            return Json(new { success = true, FreshCallsInsCount, followupInsCount, nonContactsIns, overDueInsCount, totalRedFlag, attemptsIns, pendingIns, appointment, totalContactsInsCREperDay, totalCallsperDay });
        }
        public ActionResult getInsuranceBucket(string insurancereminderData)
        {
            List<CallLogDispositionLoadReminderMR> Insurancedetails = null;
            List<CallLogDispositionLoadReminderMR> InsurancedetailsCounts = null;

            string expfromDate, exptoDate, campaignName, CREIds, appfromDate, apptoDate, flag, callfromDate, calltoDate, attempt, models, reason, appType, location;

            //Parameters of Paging..........
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchPattern = Request["search[value]"];
            int UserId = Convert.ToInt32(Session["UserId"].ToString());
            string managerUsername = Session["UserName"].ToString();
            string exception = "";

            insuranceFilter filter = new insuranceFilter();
            if (insurancereminderData != null)
            {
                filter = JsonConvert.DeserializeObject<insuranceFilter>(insurancereminderData);
            }
            if (searchPattern != "")
            {
                filter.isFiltered = true;
            }

            campaignName = filter.campaign == null ? "" : filter.campaign;
            CREIds = filter.CRES == null ? "" : filter.CRES;
            expfromDate = filter.expiryfromDate == null ? "" : Convert.ToDateTime(filter.expiryfromDate.ToString()).ToString("yyyy-MM-dd");
            exptoDate = filter.expirytoDate == null ? "" : Convert.ToDateTime(filter.expirytoDate.ToString()).ToString("yyyy-MM-dd");
            appfromDate = filter.appointmentFromDate == null ? "" : Convert.ToDateTime(filter.appointmentFromDate.ToString()).ToString("yyyy-MM-dd");
            apptoDate = filter.appointmentToDate == null ? "" : Convert.ToDateTime(filter.appointmentToDate.ToString()).ToString("yyyy-MM-dd");
            callfromDate = filter.callfromDate == null ? "" : Convert.ToDateTime(filter.callfromDate.ToString()).ToString("yyyy-MM-dd");
            calltoDate = filter.calltoDate == null ? "" : Convert.ToDateTime(filter.calltoDate.ToString()).ToString("yyyy-MM-dd");
            flag = filter.flag == null ? "" : filter.flag;
            attempt = filter.attempts == null ? "" : filter.attempts;
            reason = filter.reasons == null ? "" : filter.reasons;
            models = filter.model == null ? "" : filter.model;
            appType = filter.appointmentType == null ? "" : filter.appointmentType;
            location = filter.LocationId == null ? "" : filter.LocationId;

            long fromIndex, toIndex;
            long totalCount = 0;
            long totalFilteredCount = 0;
            long patternCount = 0;

            using (var db = new AutoSherDBContext())
            {
                try
                {
                    if (filter.getDataFor == 1)
                    {
                        if(campaignName=="" && expfromDate=="" && exptoDate=="" && flag=="" && CREIds=="" && searchPattern=="")
                        {
                            filter.isFiltered = false;
                        }

                        totalCount = InsuranceassignedListOfUserMRCount(managerUsername, CREIds, campaignName, expfromDate, exptoDate, flag, models, searchPattern);
                        if (filter.isFiltered == true)
                        {
                            totalFilteredCount = InsuranceassignedListOfUserMRCount(managerUsername, "", "", "", "", "", "", "");
                        }

                        fromIndex = start;
                        toIndex = length;
                        if (toIndex < 0)
                        {
                            toIndex = 10;
                        }
                        if (toIndex > totalCount)
                        {
                            toIndex = totalCount;
                        }
                        Insurancedetails = InsuranceassignedListOfUserMR(managerUsername, CREIds, campaignName, expfromDate, exptoDate, flag, models, searchPattern, fromIndex, toIndex);

                    }
                    else if (filter.getDataFor == 2)
                    {
                        if (campaignName == "" && expfromDate == "" && exptoDate == "" && flag == "" && CREIds == "" && searchPattern == "")
                        {
                            filter.isFiltered = false;
                        }
                        long dispoType = 4;

                        totalCount = getInsurancefollowUpCallLogTableDataMRCount(managerUsername, CREIds, expfromDate, exptoDate, campaignName, appfromDate, apptoDate, callfromDate, calltoDate, appType, models, flag, reason, searchPattern, dispoType);
                        if (filter.isFiltered == true)
                        {
                            totalFilteredCount = getInsurancefollowUpCallLogTableDataMRCount(managerUsername, "", "", "", "", "", "", "", "", "", "", "", "", "", dispoType);
                        }
                        fromIndex = start;
                        toIndex = length;
                        if (toIndex < 0)
                        {
                            toIndex = 10;
                        }
                        if (toIndex > totalCount)
                        {
                            toIndex = totalCount;
                        }
                        Insurancedetails = getInsurancefollowUpCallLogTableDataMR(managerUsername, CREIds, expfromDate, exptoDate, campaignName, appfromDate, apptoDate, callfromDate, calltoDate, appType, models, flag, reason, searchPattern, dispoType, fromIndex, toIndex);


                    }
                    else if (filter.getDataFor == 3)
                    {
                        if (expfromDate == "" && exptoDate == "" && flag == "" && CREIds == "" && searchPattern == "" && appType=="" && appfromDate=="" && apptoDate=="")
                        {
                            filter.isFiltered = false;
                        }

                        long dispoType = 25;

                        totalCount = getInsurancefollowUpCallLogTableDataMRCount(managerUsername, CREIds, expfromDate, exptoDate, campaignName, appfromDate, apptoDate, callfromDate, calltoDate, appType, models, flag, reason, searchPattern, dispoType);
                        if (filter.isFiltered == true)
                        {
                            totalFilteredCount = getInsurancefollowUpCallLogTableDataMRCount(managerUsername, "", "", "", "", "", "", "", "", "", "", "", "", "", dispoType);
                        }
                        fromIndex = start;
                        toIndex = length;
                        if (toIndex < 0)
                        {
                            toIndex = 10;
                        }
                        if (toIndex > totalCount)
                        {
                            toIndex = totalCount;
                        }
                        Insurancedetails = getInsurancefollowUpCallLogTableDataMR(managerUsername, CREIds, expfromDate, exptoDate, campaignName, appfromDate, apptoDate, callfromDate, calltoDate, appType, models, flag, reason, searchPattern, dispoType, fromIndex, toIndex);
                    }
                    else if (filter.getDataFor == 4)
                    {
                        if (campaignName == "" && expfromDate == "" && exptoDate == "" && flag == "" && reason == "" && searchPattern == "")
                        {
                            filter.isFiltered = false;
                        }
                        long dispoType = 26;

                        totalCount = getInsurancefollowUpCallLogTableDataMRCount(managerUsername, CREIds, expfromDate, exptoDate, campaignName, appfromDate, apptoDate, callfromDate, calltoDate, appType, models, flag, reason, searchPattern, dispoType);
                        if (filter.isFiltered == true)
                        {
                            totalFilteredCount = getInsurancefollowUpCallLogTableDataMRCount(managerUsername, "", "", "", "", "", "", "", "", "", "", "", "", "", dispoType);
                        }
                        fromIndex = start;
                        toIndex = length;
                        if (toIndex < 0)
                        {
                            toIndex = 10;
                        }
                        if (toIndex > totalCount)
                        {
                            toIndex = totalCount;
                        }
                        Insurancedetails = getInsurancefollowUpCallLogTableDataMR(managerUsername, CREIds, expfromDate, exptoDate, campaignName, appfromDate, apptoDate, callfromDate, calltoDate, appType, models, flag, reason, searchPattern, dispoType, fromIndex, toIndex);

                    }
                    else if (filter.getDataFor == 5)
                    {
                        if (campaignName == "" && expfromDate == "" && exptoDate == "" && flag == "" && CREIds == "" && searchPattern == "" && callfromDate=="" && calltoDate=="" && attempt=="")
                        {
                            filter.isFiltered = false;
                        }
                        long dispoType = 1;

                        totalCount = getInsurancenonContactsServerDataTableMRCount(managerUsername, searchPattern, expfromDate, exptoDate, campaignName, callfromDate, calltoDate, flag, attempt, CREIds, dispoType);
                        if (filter.isFiltered == true)
                        {
                            totalFilteredCount = getInsurancenonContactsServerDataTableMRCount(managerUsername, "", "", "", "", "", "", "", "", "", dispoType);
                        }

                        fromIndex = start;
                        toIndex = length;
                        if (toIndex < 0)
                        {
                            toIndex = 10;
                        }
                        if (toIndex > totalCount)
                        {
                            toIndex = totalCount;
                        }
                        Insurancedetails = getInsurancenonContactsServerDataTableMR(managerUsername, CREIds, searchPattern, expfromDate, exptoDate, campaignName, callfromDate, calltoDate, flag, attempt, dispoType, fromIndex, toIndex);

                    }
                    else if (filter.getDataFor == 6)
                    {

                        if ( expfromDate == "" && exptoDate == "" && flag == "" && CREIds == "" && searchPattern == "" && appType=="")
                        {
                            filter.isFiltered = false;
                        }

                        totalCount = getInsurancePreviousDayServerDataTableMRCount(managerUsername, "1");
                        

                        fromIndex = start;
                        toIndex = length;

                        if (toIndex < 0)
                        {
                            toIndex = 10;
                        }
                        if (toIndex > totalCount)
                        {
                            toIndex = totalCount;
                        }
                        InsurancedetailsCounts = getInsurancePreviousDayServerDataTableMR(managerUsername, "1", CREIds, searchPattern, expfromDate, exptoDate, appType, flag, fromIndex, toIndex);
                        if (filter.isFiltered == true)
                        {
                            patternCount = getInsurancePreviousDayServerDataTableMR(managerUsername, "1", CREIds, searchPattern, expfromDate, exptoDate, appType, flag, 0, totalCount).Count;
                        }
                    }
                    else if (filter.getDataFor == 7)
                    {
                        if (expfromDate == "" && exptoDate == "" && flag == "" && CREIds == "" && searchPattern == "" && appType == "")
                        {
                            filter.isFiltered = false;
                        }

                        totalCount = getInsurancePreviousDayServerDataTableMRCount(managerUsername, "2");

                        fromIndex = start;
                        toIndex = length;
                        if (toIndex < 0)
                        {
                            toIndex = 10;
                        }
                        if (toIndex > totalCount)
                        {
                            toIndex = totalCount;
                        }
                        InsurancedetailsCounts = getInsurancePreviousDayServerDataTableMR(managerUsername, "2", CREIds, searchPattern, expfromDate, exptoDate, appType, flag, fromIndex, toIndex);
                        if (filter.isFiltered == true)
                        {
                            patternCount = getInsurancePreviousDayServerDataTableMR(managerUsername, "2", CREIds, searchPattern, expfromDate, exptoDate, appType, flag, 0, totalCount).Count;
                        }
                    }
                    else if (filter.getDataFor == 8)
                    {

                        if (campaignName=="" && expfromDate == "" && exptoDate == "" && flag == "" && CREIds == "" && searchPattern == "" && appType == "")
                        {
                            filter.isFiltered = false;
                        }

                        totalCount = getInsuranceFuturefollowUpDataMRCount(managerUsername);
                        
                        fromIndex = start;
                        toIndex = length;
                        if (toIndex < 0)
                        {
                            toIndex = 10;
                        }
                        if (toIndex > totalCount)
                        {
                            toIndex = totalCount;
                        }
                        InsurancedetailsCounts = getInsuranceFuturefollowUpDataMR(managerUsername, CREIds, expfromDate, exptoDate, campaignName, models, flag, searchPattern, fromIndex, toIndex);
                        if (filter.isFiltered == true)
                        {
                            patternCount = getInsuranceFuturefollowUpDataMR(managerUsername, CREIds, expfromDate, exptoDate, campaignName, models, flag, searchPattern, 0, totalCount).Count;
                        }
                    }
                    else if (filter.getDataFor == 9)
                    {

                        if (campaignName == "" && expfromDate == "" && exptoDate == "" && flag == "" && CREIds == "" && searchPattern == "" && appType == "" && location=="" && callfromDate=="" && calltoDate=="" && attempt=="")
                        {
                            filter.isFiltered = false;
                        }

                        totalCount = getpaidBucketCount("", "", "", "", "", "", "", "", managerUsername, "", "", "");
                        fromIndex = start;
                        toIndex = length;
                        if (toIndex < 0)
                        {
                            toIndex = 10;
                        }
                        if (toIndex > totalCount)
                        {
                            toIndex = totalCount;
                        }
                        InsurancedetailsCounts = getPaid(searchPattern, expfromDate, exptoDate, campaignName, callfromDate, calltoDate, flag, attempt, managerUsername, CREIds, 1, location, "", fromIndex, toIndex);
                        if (filter.isFiltered == true)
                        {
                            patternCount = getPaid(searchPattern, expfromDate, exptoDate, campaignName, callfromDate, calltoDate, flag, attempt, managerUsername, CREIds, 1, location,"", 0,totalCount).Count;
                        }
                    }
                    else if (filter.getDataFor == 10)
                    {

                        if (campaignName == "" && expfromDate == "" && exptoDate == "" && flag == "" && CREIds == "" && searchPattern == "" && appType == "" && location == "" && callfromDate == "" && calltoDate == "" && attempt == "")
                        {
                            filter.isFiltered = false;
                        }

                        totalCount = getotherBucketCount("", "", "", "", "", "", "", "", managerUsername, "", "", "");

                        fromIndex = start;
                        toIndex = length;
                        if (toIndex < 0)
                        {
                            toIndex = 10;
                        }
                        if (toIndex > totalCount)
                        {
                            toIndex = totalCount;
                        }
                        InsurancedetailsCounts = getOthers(searchPattern, expfromDate, exptoDate, campaignName, callfromDate, calltoDate, flag, attempt, managerUsername, CREIds,1,location, "",fromIndex,toIndex);
                            getInsuranceFuturefollowUpDataMR(managerUsername, CREIds, expfromDate, exptoDate, campaignName, models, flag, searchPattern, fromIndex, toIndex);
                        if (filter.isFiltered == true)
                        {
                            patternCount = getOthers(searchPattern, expfromDate, exptoDate, campaignName, callfromDate, calltoDate, flag, attempt, managerUsername, CREIds,1, location, "", 0, totalCount).Count;
                        }
                    }
                    else if (filter.getDataFor == 11)
                    {

                        if (campaignName == "" && expfromDate == "" && exptoDate == "" && flag == "" && CREIds == "" && searchPattern == "" && appType == "" && location == "" )
                        {
                            filter.isFiltered = false;
                        }

                        totalCount = getreceiptdBucketCount(managerUsername,"");

                        fromIndex = start;
                        toIndex = length;
                        if (toIndex < 0)
                        {
                            toIndex = 10;
                        }
                        if (toIndex > totalCount)
                        {
                            toIndex = totalCount;
                        }
                        InsurancedetailsCounts = getreceiptCall(managerUsername,CREIds,campaignName,expfromDate,exptoDate,flag,"",searchPattern, location, "", fromIndex,toIndex);
                        if (filter.isFiltered == true)
                        {
                            patternCount = getreceiptCall(managerUsername, CREIds, campaignName, expfromDate, exptoDate, flag, "", searchPattern, location, "", 0, totalCount).Count;
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
            if (Insurancedetails != null)
            {
                if (filter.isFiltered == true)
                {

                    var JsonData = Json(new { data = Insurancedetails, draw = Request["draw"], recordsTotal = totalFilteredCount, recordsFiltered = totalCount, exception = exception });
                    JsonData.MaxJsonLength = int.MaxValue;
                    return JsonData;
                }
                else
                {
                    var JsonData = Json(new { data = Insurancedetails, draw = Request["draw"], recordsTotal = totalCount, recordsFiltered = totalCount, exception = exception });
                    JsonData.MaxJsonLength = int.MaxValue;
                    return JsonData;
                }

            }
            if(InsurancedetailsCounts!=null)
            {
                if (filter.isFiltered == true)
                {
                    var JsonData = Json(new { data = InsurancedetailsCounts, draw = Request["draw"], recordsTotal = totalCount, recordsFiltered = patternCount, exception = exception }, JsonRequestBehavior.AllowGet);
                    JsonData.MaxJsonLength = int.MaxValue;
                    return JsonData;
                }
                else if (filter.isFiltered == false)
                {
                    var JsonData = Json(new { data = InsurancedetailsCounts, draw = Request["draw"], recordsTotal = totalCount, recordsFiltered = totalCount, exception = exception }, JsonRequestBehavior.AllowGet);
                    JsonData.MaxJsonLength = int.MaxValue;
                    return JsonData;
                }
            }

            return Json(new { data = "", draw = Request["draw"], recordsTotal = 0, recordsFiltered = 0, exception = exception });
        }


        public long InsuranceassignedListOfUserMRCount(string managerUsername, string CREIds, string campaignName, string expfromDate, string exptoDate, string flag, string models, string searchPattern)
        {
            long totalCount = 0;
            //try
            //{
            using (var db = new AutoSherDBContext())
            {
                totalCount = db.Database.SqlQuery<int>("call cremanagerinsuranceScheduledCallsCount(@cremanagerid,@wyzuserid,@in_campaign_id,@instartdate,@inenddate,@in_flag,@in_model,@pattern)",
                          new MySqlParameter("@cremanagerid", managerUsername),
                          new MySqlParameter("@wyzuserid", CREIds),
                          new MySqlParameter("@in_campaign_id", campaignName),
                          new MySqlParameter("@instartdate", expfromDate),
                          new MySqlParameter("@inenddate", exptoDate),
                          new MySqlParameter("@in_flag", flag),
                          new MySqlParameter("@in_model", models),
                      new MySqlParameter("@pattern", searchPattern)).FirstOrDefault();

                return totalCount;
            }
            //}
            //catch (Exception ex)
            //{

            //}
            return totalCount;
        }
        public List<CallLogDispositionLoadReminderMR> InsuranceassignedListOfUserMR(string userLoginName, string crename, string campaignName, string fromexpirydaterange, string toexpirydaterange, string flag, string modelName, string searchPattern, long fromIndex, long toIndex)
        {
            List<CallLogDispositionLoadReminderMR> dispositionLoads = null;
            //try
            //{
            using (var db = new AutoSherDBContext())
            {
                string str = @"CALL cremanagerinsuranceScheduledCalls(@cremanagerid,@wyzuserid,@in_campaign_id,@instartdate,@inenddate,@in_flag,@in_model,@pattern,@start_with,@length)";
                MySqlParameter[] param = new MySqlParameter[]
                {
                        new MySqlParameter("@cremanagerid", userLoginName),
                        new MySqlParameter("@wyzuserid", crename),
                        new MySqlParameter("@in_campaign_id", campaignName),
                        new MySqlParameter("@instartdate", fromexpirydaterange),
                        new MySqlParameter("@inenddate", toexpirydaterange),
                        new MySqlParameter("@in_flag", flag),
                        new MySqlParameter("@in_model", modelName),
                        new MySqlParameter("@pattern", searchPattern),
                        new MySqlParameter("@start_with", fromIndex),
                        new MySqlParameter("@length", toIndex)
                };

                dispositionLoads = db.Database.SqlQuery<CallLogDispositionLoadReminderMR>(str, param).ToList();
            }
            // }
            //catch (Exception ex)
            //{

            //}

            return dispositionLoads;
        }

        public long getInsurancefollowUpCallLogTableDataMRCount(string username, string crename, string fromexpirydaterange, string toexpirydaterange, string campaignName, string appointmentFromDate, string appointmentToDate, string callFromDate, string callToDate, string appointmentType, string modelName, string flag, string reasons, string searchPattern, long buckettype)
        {
            long totalCount = 0;
            //try
            //{
            using (var db = new AutoSherDBContext())
            {
                totalCount = db.Database.SqlQuery<int>("call cremanagerinsurancefilterforDispositionContactsCount( @cremanagerid,@instartdate,@inenddate,@incampaignname,@inflag,@infromApdate,@intoApdate,@infromCalldate,@intoCalldate,@inAptype,@inmodel, @inreasons,@pattern, @wyzuserid, @dispositiontype)",
                      new MySqlParameter("@cremanagerid", username),
                      new MySqlParameter("@instartdate", fromexpirydaterange),
                      new MySqlParameter("@inenddate", toexpirydaterange),
                      new MySqlParameter("@incampaignname", campaignName),
                      new MySqlParameter("@inflag", flag),
                      new MySqlParameter("@infromApdate", appointmentFromDate),
                      new MySqlParameter("@intoApdate", appointmentToDate),
                      new MySqlParameter("@infromCalldate", callFromDate),
                      new MySqlParameter("@intoCalldate", callToDate),
                      new MySqlParameter("@inAptype", appointmentType),
                      new MySqlParameter("@inmodel", modelName),
                      new MySqlParameter("@inreasons", reasons),
                      new MySqlParameter("@pattern", searchPattern),
                      new MySqlParameter("@wyzuserid", crename),
                  new MySqlParameter("@dispositiontype", buckettype)).FirstOrDefault();

                return totalCount;
            }
            //}
            //catch (Exception ex)
            //{

            //}
            return totalCount;
        }

        public List<CallLogDispositionLoadReminderMR> getInsurancefollowUpCallLogTableDataMR(string username, string crename, string fromexpirydaterange, string toexpirydaterange, string campaignName, string appointmentFromDate, string appointmentToDate, string callFromDate, string callToDate, string appointmentType, string modelName, string flag, string reasons, string searchPattern, long buckettype, long fromIndex, long toIndex)
        {
            List<CallLogDispositionLoadReminderMR> dispositionLoads = null;
            //try
            //{
            using (var db = new AutoSherDBContext())
            {
                string str = @"CALL cremanagerinsurancefilterforDispositionContacts(@cremanagerid,@instartdate,@inenddate,@incampaignname,
@inflag,@infromApdate,@intoApdate,@infromCalldate,@intoCalldate,@inAptype,@inmodel
 ,@inreasons,@pattern,@wyzuserid,@dispositiontype,@start_with,@length )";
                MySqlParameter[] param = new MySqlParameter[]
                {
                        new MySqlParameter("@cremanagerid", username),
                        new MySqlParameter("@instartdate", fromexpirydaterange),
                        new MySqlParameter("@inenddate", toexpirydaterange),
                        new MySqlParameter("@incampaignname", campaignName),
                        new MySqlParameter("@inflag", flag),
                        new MySqlParameter("@infromApdate",appointmentFromDate),
                        new MySqlParameter("@intoApdate", appointmentToDate),
                        new MySqlParameter("@infromCalldate", callFromDate),
                        new MySqlParameter("@intoCalldate", callToDate),
                        new MySqlParameter("@inAptype", appointmentType),
                        new MySqlParameter("@inmodel", modelName),
                        new MySqlParameter("@inreasons", reasons),
                        new MySqlParameter("@pattern", searchPattern),
                        new MySqlParameter("@wyzuserid", crename),
                        new MySqlParameter("@dispositiontype", buckettype),
                        new MySqlParameter("@start_with", fromIndex),
                        new MySqlParameter("@length", toIndex)
                };
                dispositionLoads = db.Database.SqlQuery<CallLogDispositionLoadReminderMR>(str, param).ToList();
            }
            //}
            //catch (Exception ex)
            //{

            //}

            return dispositionLoads;


        }


        public long getInsurancenonContactsServerDataTableMRCount(string managerUsername, string searchPattern, string expfromDate, string exptoDate, string campaignName, string callfromDate, string calltoDate, string flag, string attempt, string CREIds, long dispoType)
        {

            long totalCount = 0;
            using (var db = new AutoSherDBContext())
            {
                totalCount = db.Database.SqlQuery<int>("call cremanagerinsurancefilterforNonContactsCount(@cremanagerid ,@pattern,@instartdate,@inenddate,@incampaignname," +
                             "@incallFromDate,@incallToDate,@inflag,@inattempts,@wyzuserid,@dispositiontype)",
                            new MySqlParameter("@cremanagerid", managerUsername),
                            new MySqlParameter("@pattern", searchPattern),
                            new MySqlParameter("@instartdate", expfromDate),
                            new MySqlParameter("@inenddate", exptoDate),
                            new MySqlParameter("@incampaignname", campaignName),
                            new MySqlParameter("@incallFromDate", callfromDate),
                            new MySqlParameter("@incallToDate", calltoDate),
                            new MySqlParameter("@inflag", flag),
                            new MySqlParameter("@inattempts", attempt),
                            new MySqlParameter("@wyzuserid", CREIds),
                            new MySqlParameter("@dispositiontype", dispoType)).FirstOrDefault();

                return totalCount;
            }

        }

        public List<CallLogDispositionLoadReminderMR> getInsurancenonContactsServerDataTableMR(string username, string crename, string searchPattern, string fromexpirydaterange, string toexpirydaterange, string campaignName, string callFromDate, string callToDate, string flag, string attempts, long buckettype, long fromIndex, long toIndex)
        {
            List<CallLogDispositionLoadReminderMR> dispositionLoads = null;
            //try
            //{
            using (var db = new AutoSherDBContext())
            {
                string str = @"CALL cremanagerinsurancefilterforNonContacts(@cremanagerid,@pattern,@instartdate,@inenddate,@incampaignname,@incallFromDate," +
                        "@incallToDate,@inflag,@inattempts,@wyzuserid,@dispositiontype,@start_with,@length)";
                MySqlParameter[] param = new MySqlParameter[]
                {

                         new MySqlParameter("@cremanagerid", username),
                           new MySqlParameter("@pattern", searchPattern),
                           new MySqlParameter("@instartdate", fromexpirydaterange),
                           new MySqlParameter("@inenddate", toexpirydaterange),
                           new MySqlParameter("@incampaignname", campaignName),
                           new MySqlParameter("@incallFromDate", callFromDate),
                           new MySqlParameter("@incallToDate", callToDate),
                           new MySqlParameter("@inflag", flag),
                           new MySqlParameter("@inattempts", attempts),
                           new MySqlParameter("@wyzuserid", crename),
                           new MySqlParameter("@dispositiontype", buckettype),
                           new MySqlParameter("@start_with",fromIndex),
                       new MySqlParameter("@length", toIndex)
                };
                dispositionLoads = db.Database.SqlQuery<CallLogDispositionLoadReminderMR>(str, param).ToList();
            }
            // }
            //catch (Exception ex)
            //{

            //}

            return dispositionLoads;

        }

        public long getInsurancePreviousDayServerDataTableMRCount(string incremanager, string dispo)
        {
            long totalCount = 0;
            //try
            //{
            using (var db = new AutoSherDBContext())
            {
                totalCount = db.Database.SqlQuery<int>("call cremanagerInsurancePreviousDayCount(@sbday,@incremanager)",
                      new MySqlParameter("@sbday", dispo),
                  new MySqlParameter("@incremanager", incremanager)).FirstOrDefault();

                return totalCount;
            }
            //}
            //catch (Exception ex)
            //{

            //}
            return totalCount;

        }
        public List<CallLogDispositionLoadReminderMR> getInsurancePreviousDayServerDataTableMR(string incremanager, string dispo, string crename, string searchPattern, string fromexpirydaterange, string toexpirydaterange, string appointmentType, string flag, long fromIndex, long toIndex)
        {
            List<CallLogDispositionLoadReminderMR> dispositionLoads = null;
            //try
            //{
            using (var db = new AutoSherDBContext())
            {
                string str = @"CALL cremanagerInsurancePreviousDay(@sbday,@incremanager,@inChasers_id,@instatus,@inflag,@instartdate,@inenddate,@pattern,@startwith,@length)";
                MySqlParameter[] param = new MySqlParameter[]
                {
                         new MySqlParameter("@sbday", dispo),
                           new MySqlParameter("@incremanager", incremanager),
                           new MySqlParameter("@inChasers_id", crename),
                           new MySqlParameter("@instatus", appointmentType),
                           new MySqlParameter("@inflag", flag),
                           new MySqlParameter("@instartdate", fromexpirydaterange),
                           new MySqlParameter("@inenddate", toexpirydaterange),
                           new MySqlParameter("@pattern", searchPattern),
                           new MySqlParameter("@startwith", fromIndex),
                       new MySqlParameter("@length", 900)
                };
                dispositionLoads = db.Database.SqlQuery<CallLogDispositionLoadReminderMR>(str, param).ToList();
            }
            //}
            //catch (Exception ex)
            //{

            //}

            return dispositionLoads;

        }

        public long getInsuranceFuturefollowUpDataMRCount(string incremanager)
        {
            long totalCount = 0;
            //try
            //{
            using (var db = new AutoSherDBContext())
            {
                totalCount = db.Database.SqlQuery<int>("call cremanagerInsuranceFutureFollowupCount(@incremanager)",
                  new MySqlParameter("@incremanager", incremanager)).FirstOrDefault();

                return totalCount;
            }
            //}
            //catch (Exception ex)
            //{

            //}
            return totalCount;


        }

        public List<CallLogDispositionLoadReminderMR> getInsuranceFuturefollowUpDataMR(string incremanager, string crename, string fromexpirydaterange, string toexpirydaterange, string campaignName, string modelName, string flag, string searchPattern, long fromIndex, long toIndex)
        {
            List<CallLogDispositionLoadReminderMR> dispositionLoads = null;
            //try
            //{
            using (var db = new AutoSherDBContext())
            {
                string str = @"CALL cremanagerInsuranceFutureFollowup(@increManager,@inwyzuserid,@instartdate,@inenddate,@incampaignname,@inflag,@inmodel,@pattern,@start_with,@length)";
                MySqlParameter[] param = new MySqlParameter[]
                {
                         new MySqlParameter("@increManager", incremanager),
                           new MySqlParameter("@inwyzuserid", crename),
                           new MySqlParameter("@instartdate", fromexpirydaterange),
                           new MySqlParameter("@inenddate", toexpirydaterange),
                           new MySqlParameter("@incampaignname", campaignName),
                           new MySqlParameter("@inflag", flag),
                           new MySqlParameter("@inmodel", modelName),
                           new MySqlParameter("@pattern", searchPattern),
                           new MySqlParameter("@start_with", fromIndex),
                       new MySqlParameter("@length", toIndex)
                };
                dispositionLoads = db.Database.SqlQuery<CallLogDispositionLoadReminderMR>(str, param).ToList();
            }
            //}
            //catch (Exception ex)
            //{

            //}

            return dispositionLoads;


        }

        #region Paid Bucket Details
        public int getpaidBucketCount(string searchPattern, string ExpiryfromDate, string ExpirytoDate, string campaignName, string callFromDate, string callToDate, string flag, string attempts,string cremanagerid, string UserId, string locId, string renewaltype)
        {
            int totalCount = 0;
            using (var db = new AutoSherDBContext())
            {
                totalCount = db.Database.SqlQuery<int>("call cremanagerInsurancefilterforPaidCount(@pattern,@instartdate,@inenddate,@incampaignname,@incallFromDate,@incallToDate,@inflag,@inattempts,@cremanagerid,@wyzuserid,@dispositiontype,@locId,@inrenType)",
                            new MySqlParameter("@pattern", searchPattern),
                            new MySqlParameter("@instartdate", ExpiryfromDate),
                            new MySqlParameter("@inenddate", ExpirytoDate),
                            new MySqlParameter("@incampaignname", campaignName),
                            new MySqlParameter("@incallFromDate", callFromDate),
                            new MySqlParameter("@incallToDate", callToDate),
                            new MySqlParameter("@inflag", flag),
                            new MySqlParameter("@inattempts", attempts),
                            new MySqlParameter("@cremanagerid", cremanagerid),
                            new MySqlParameter("@wyzuserid", UserId),
                            new MySqlParameter("@dispositiontype", 1),
                            new MySqlParameter("@locId", locId),
                            new MySqlParameter("@inrenType", renewaltype)).FirstOrDefault();
                return totalCount;
            }
        }

        public List<CallLogDispositionLoadReminderMR> getPaid(string pattern, string instartdate, string inenddate, string incampaignname, string incallFromDate, string incallToDate, string inflag, string inattempts, string cremanagerid, string wyzuserid, long dispositiontype, string locId, string inrenType, long start_with, long length)
        {
            List<CallLogDispositionLoadReminderMR> dispositionLoadInsurances = new List<CallLogDispositionLoadReminderMR>();
            using (var db = new AutoSherDBContext())
            {
                string str = @"CALL cremanagerInsurancefilterforPaid(@pattern,@instartdate, @inenddate,@incampaignname,@incallFromDate,@incallToDate,@inflag,@inattempts,@cremanagerid,@wyzuserid, @dispositiontype,@locId,@inrenType,@start_with,@length);";

                MySqlParameter[] sqlParameter = new MySqlParameter[]
                {
                        new MySqlParameter("pattern", pattern),
                        new MySqlParameter("instartdate", instartdate),
                        new MySqlParameter("inenddate", inenddate),
                        new MySqlParameter("incampaignname", incampaignname),
                        new MySqlParameter("incallFromDate", incallFromDate),
                        new MySqlParameter("incallToDate", incallToDate),
                        new MySqlParameter("inflag", inflag),
                        new MySqlParameter("inattempts", inattempts),
                        new MySqlParameter("@cremanagerid", cremanagerid),
                        new MySqlParameter("wyzuserid", wyzuserid),
                        new MySqlParameter("dispositiontype", dispositiontype),
                        new MySqlParameter("locId", locId),
                        new MySqlParameter("inrenType", inrenType),
                        new MySqlParameter("start_with", start_with),
                        new MySqlParameter("length", length)
            };
                dispositionLoadInsurances = db.Database.SqlQuery<CallLogDispositionLoadReminderMR>(str, sqlParameter).ToList();
            }

            return dispositionLoadInsurances;
        }
        #endregion

        #region OtherBucket Details

        public int getotherBucketCount(string searchPattern, string ExpiryfromDate, string ExpirytoDate, string campaignName, string callFromDate, string callToDate, string flag, string attempts, string incremanager, string crename, string locId, string renewaltype)
        {
            int totalCount = 0;
            using (var db = new AutoSherDBContext())
            {
                totalCount = db.Database.SqlQuery<int>("call cremanagerInsurancefilterforOthersCount(@pattern,@instartdate,@inenddate,@incampaignname,@incallFromDate,@incallToDate,@inflag,@inattempts,@incremanager,@wyzuserid,@dispositiontype,@locId,@inrenType)",
                           new MySqlParameter("@pattern", searchPattern),
                           new MySqlParameter("@instartdate", ExpiryfromDate),
                           new MySqlParameter("@inenddate", ExpirytoDate),
                           new MySqlParameter("@incampaignname", campaignName),
                           new MySqlParameter("@incallFromDate", callFromDate),
                           new MySqlParameter("@incallToDate", callToDate),
                           new MySqlParameter("@inflag", flag),
                           new MySqlParameter("@inattempts", attempts),
                           new MySqlParameter("@incremanager", incremanager),
                           new MySqlParameter("@wyzuserid", crename),
                           new MySqlParameter("@dispositiontype", 1),
                           new MySqlParameter("@locId", locId),
                           new MySqlParameter("@inrenType", renewaltype)).FirstOrDefault();
                return totalCount;
            }
        }

        public List<CallLogDispositionLoadReminderMR> getOthers(string pattern, string instartdate, string inenddate, string incampaignname, string incallFromDate, string incallToDate, string inflag, string inattempts, string incremanager, string crename, long dispositiontype, string locId, string inrenType, long start_with, long length)
        {
            List<CallLogDispositionLoadReminderMR> dispositionLoadInsurances = new List<CallLogDispositionLoadReminderMR>();
            using (var db = new AutoSherDBContext())
            {
                string str = @"CALL cremanagerInsurancefilterforOthers(@pattern,@instartdate, @inenddate,@incampaignname,@incallFromDate,@incallToDate,@inflag,@inattempts,@incremanager,@wyzuserid, @dispositiontype,@locId,@inrenType,@start_with,@length);";

                MySqlParameter[] sqlParameter = new MySqlParameter[]
                {
                        new MySqlParameter("pattern", pattern),
                        new MySqlParameter("instartdate", instartdate),
                        new MySqlParameter("inenddate", inenddate),
                        new MySqlParameter("incampaignname", incampaignname),
                        new MySqlParameter("incallFromDate", incallFromDate),
                        new MySqlParameter("incallToDate", incallToDate),
                        new MySqlParameter("inflag", inflag),
                        new MySqlParameter("inattempts", inattempts),
                        new MySqlParameter("incremanager", incremanager),
                        new MySqlParameter("wyzuserid", crename),
                        new MySqlParameter("dispositiontype", dispositiontype),
                        new MySqlParameter("locId", locId),
                        new MySqlParameter("inrenType", inrenType),
                        new MySqlParameter("start_with", start_with),
                        new MySqlParameter("length", length)
            };
                dispositionLoadInsurances = db.Database.SqlQuery<CallLogDispositionLoadReminderMR>(str, sqlParameter).ToList();
            }

            return dispositionLoadInsurances;
        }
        #endregion

        #region Receipt Bucket Details
        public int getreceiptdBucketCount(string cremanagerid, string creName)
        {
            int totalCount = 0;
            using (var db = new AutoSherDBContext())
            {
                totalCount = db.Database.SqlQuery<int>("call cremanagerInsurancefilterReceiptCount(@cremanagerid,@wyzuserid)",
                            new MySqlParameter("@cremanagerid", cremanagerid),new MySqlParameter("@wyzuserid", creName)).FirstOrDefault();
                return totalCount;
            }
        }

        public List<CallLogDispositionLoadReminderMR> getreceiptCall(string cremanagerid,string creName, string campaignName, string fromexpirydaterange, string toexpirydaterange, string flag, string modelName, string searchPattern, string locId, string renewalType, long fromIndex, long toIndex)
        {
            List<CallLogDispositionLoadReminderMR> callLogsInsurence = null;

            using (var db = new AutoSherDBContext())
            {
                string str = @"CALL cremanagerInsurancefilterReceipt(@incremanager,@wyzuserid,@in_campaign_id,@instartdate,@inenddate,@in_flag,@in_model,@pattern,@locId,@inrenType,@start_with,@length);";
                MySqlParameter[] sqlParameter = new MySqlParameter[]
                {
                        new MySqlParameter("incremanager", cremanagerid),
                        new MySqlParameter("wyzuserid", creName),
                        new MySqlParameter("in_campaign_id", campaignName),
                        new MySqlParameter("instartdate", fromexpirydaterange),
                        new MySqlParameter("inenddate", toexpirydaterange),
                        new MySqlParameter("in_flag", flag),
                        new MySqlParameter("in_model", modelName),
                        new MySqlParameter("pattern", searchPattern),
                        new MySqlParameter("locId", locId),
                        new MySqlParameter("inrenType", renewalType),
                        new MySqlParameter("start_with", fromIndex),
                        new MySqlParameter("length", toIndex)
                };
                callLogsInsurence = db.Database.SqlQuery<CallLogDispositionLoadReminderMR>(str, sqlParameter).ToList();
            }
            return callLogsInsurence;
        }
        #endregion

        #endregion Insurance Reminder Ends



        #region Ajax Calls
        public ActionResult listWorkshops(int selectedCity)
        {
            //List<workshop> workshoplist = new List<workshop>();
            AutoSherDBContext dba = new AutoSherDBContext();
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    db.Configuration.ProxyCreationEnabled = false;
                    var workshoplist = dba.workshops.Where(m => m.location_cityId == selectedCity).Select(m => new { id = m.id, workshopName = m.workshopName }).ToList();
                    //var data = JsonConvert.SerializeObject(workshoplist);

                    return Json(workshoplist);
                }

            }
            catch (Exception ex)
            {

            }
            return Json(new { workshoplist = new List<workshop>() }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getCRESListBasedOnWorkshop(long? workshopId, string crefor)
        {

            try
            {
                using (var db = new AutoSherDBContext())
                {


                    //var listOfRoleId = db.userworkshops.Where(r => r.us == workshopId).Select(r => r.workshopList_id).ToList();
                    //var workshopsList = db.workshops.Where(r => listOfRoleId.Contains(r.id)).Select(m => new { id = m.id, name = m.workshopName }).ToList();

                    //var  workshopListId = db.userworkshops.Where(x => x.workshopList_id == workshopId).Select(m=>m.userWorkshop_id).ToList();

                    //foreach(var workshopids in workshopListId)
                    //{
                    if (crefor == "Service")
                    {
                        var creList = db.wyzusers.Where(m => m.workshop_id == workshopId && m.insuranceRole == false && m.role == "CRE" && m.unAvailable == false && m.role1 == "1").Select(m => new { id = m.id, creName = m.userName }).OrderBy(m => m.creName).ToList();
                        return Json(creList);
                    }
                    else if (crefor == "PSF")
                    {
                        var creList = db.wyzusers.Where(m => m.workshop_id == workshopId && m.role == "CRE" && m.unAvailable == false && m.role1 == "4").Select(m => new { id = m.id, creName = m.userName }).OrderBy(m => m.creName).ToList();
                        return Json(creList);
                    }
                    else if (crefor == "Other")
                    {
                        var creList = db.wyzusers.Where(m => m.workshop_id == workshopId && m.role == "CRE" && m.unAvailable == false).Select(m => new { id = m.id, creName = m.userName }).OrderBy(m => m.creName).ToList();
                        return Json(creList);
                    }

                    //}
                    // var creList = (from w in db.wyzusers join uw in db.userworkshops on w.workshop_id equals uw.userWorkshop_id  where uw.workshopList_id== workshopId && w.role == "CRE" && uw.userWorkshop_id==w.workshop_id select new { id = w.id,creName = w.userName}).ToList();

                    //var creList = db.wyzusers.Where(m => m.workshop_id == workshopListId && m.insuranceRole==false && m.role == "CRE" && m.role1=="1").Select(m => new { id = m.id, creName = m.userName }).OrderBy(m=>m.creName).ToList();
                    // var creList = db.wyzusers.Where(m => workshopListId.Contains(m.workshop_id)&&  m.role == "CRE" ).Select(m => new {  }).OrderBy(m=>m.creName).ToList();

                }
            }
            catch (Exception ex)
            {

            }
            return Json(new { creList = new List<wyzuser>() }, JsonRequestBehavior.AllowGet);

        }
        #endregion



        #region View Manager Home
        public ActionResult managerHomePage()
        {
            return View();
        }
        #endregion

    }
}