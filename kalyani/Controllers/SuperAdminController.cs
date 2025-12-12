using AutoSherpa_project.Models;
using AutoSherpa_project.Models.EIBL_API;
using Firebase.Database;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Internal;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AutoSherpa_project.Controllers
{
    [AuthorizeFilter]
    public class SuperAdminController : Controller
    {
        #region Event Status
        // GET: EventStatus
        //public ActionResult Event()
        //{
        //    return View();
        //}
        //public ActionResult getEventDetails()
        //{

        //    List<eventstaus> listofevent = new List<eventstaus>();
        //    string exception = "";
        //    int maxCount = 0;
        //    try
        //    {
        //        //Parameters of Paging..........
        //        int start = Convert.ToInt32(Request["start"]);
        //        int length = Convert.ToInt32(Request["length"]);
        //        string searchPattern = Request["search[value]"];
        //        string role = Session["UserRole"].ToString();




        //        using (var db = new AutoSherDBContext())
        //        {
        //            if (role == "SuperAdmin")
        //            {

        //                if (!string.IsNullOrEmpty(searchPattern))
        //                {
        //                    maxCount = db.eventStaus.Count(m => m.Eventname.Contains(searchPattern));

        //                    if (maxCount < length)
        //                    {
        //                        length = maxCount;
        //                    }

        //                    listofevent = db.eventStaus.Where(m => m.Eventname.Contains(searchPattern)).OrderBy(m => m.id).Skip(start).Take(length).ToList();
        //                }
        //                else
        //                {
        //                    maxCount = db.eventStaus.Count();

        //                    if (maxCount < length)
        //                    {
        //                        length = maxCount;
        //                    }
        //                    listofevent = db.eventStaus.OrderBy(m => m.id).Skip(start).Take(length).ToList();
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
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
        //        if (ex.StackTrace.Contains(':'))
        //        {
        //            exception = "Line: " + ex.StackTrace.Split(':')[(ex.StackTrace.Split(':').Count() - 1)] + " " + exception;
        //        }

        //    }

        //    return Json(new { data = listofevent, recordsTotal = maxCount, recordsFiltered = maxCount, exception });
        //    // return Json(new { data = "", recordsTotal = 0, recordsFiltered = 0 });

        //}

        #endregion

        #region Manage Dealer table Fields

        public ActionResult manageFlagField()
        {
            dealer dealerdetails = new dealer();
            string dealercode = (Session["DealerCode"]).ToString();
            try
            {
                using (var db = new AutoSherDBContext())
                {

                    dealerdetails = db.dealers.Where(m => m.dealerCode == dealercode).FirstOrDefault();

                    return View(dealerdetails);
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

                return Json(new { success = false, exception }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult updateDetails(string colName, string colValues)
        {

            try
            {
                using (var db = new AutoSherDBContext())
                {
                    dealer data = db.dealers.FirstOrDefault();


                    if (data != null)
                    {
                        if (colName == "insfollowupdaylimit")
                        {
                            data.insfollowupdaylimit = Convert.ToInt32(colValues);
                        }
                        else if (colName == "insbookingdaylimit")
                        {
                            data.insbookingDayLimit = Convert.ToInt32(colValues);
                        }
                        db.dealers.AddOrUpdate(data);
                        db.SaveChanges();
                    }

                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
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

                return Json(new { success = false, exception }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Count Number
        public ActionResult transbitlykarix()
        {
            try
            {
                string lastInsertedDate = string.Empty;
                string lastInsertedTime = string.Empty;
                int mysqltotalCount = 0;
                string querry = string.Empty;


                using (var db = new AutoSherDBContext())
                {
                    var dealerlist = db.Herodealers.Select(m => new { id =  m.DealerCode, name = m.DealerName + " " + m.DealerCity + "(" + m.DealerCode + ")", m.IsActive, m.DealerClassification }).ToList();
                    ViewBag.dealerlist = dealerlist;
                    mysqltotalCount = db.Database.SqlQuery<int>("SELECT count(*) FROM trans_bitly_karix;").FirstOrDefault();
                    ViewBag.mysqltotalCount = mysqltotalCount;

                    if ((db.Batchends.Count()) > 0)
                    {
                        var batchDetails = db.Batchends.OrderByDescending(m => m.id).FirstOrDefault();
                        lastInsertedDate = batchDetails.endDate;
                        lastInsertedTime = batchDetails.endTime;
                    }
                }
                ViewBag.lastInsertedDate = lastInsertedDate;
                ViewBag.lastInsertedTime = lastInsertedTime;

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
                ViewBag.TotalRecords = exception;
            }
            return View();
        } 
        public ActionResult loadSQLData()
        {
            try
            {
                int mysqltotalCount = 0;
                string querry = string.Empty;

                using (var db = new AutoSherDBContext())
                {
                    mysqltotalCount = db.Database.SqlQuery<int>("SELECT count(*) FROM trans_bitly_karix;").FirstOrDefault();                
                }

                querry = "select count(*) from trans_bitly_karix  TK(nolock)  where  TK.request_type = 'heroninja' and IS_Renewal=0 and  Previous_Policy_Expiry >= dateadd( day,-95, CONVERT(date,GETDATE()))";
                long totalCount = fetchSqlRead(querry);
                ViewBag.TotalRecords = totalCount;

                return Json(new { success=true, TotalRecords=totalCount , todaysdatatopull = totalCount - mysqltotalCount });

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
                ViewBag.TotalRecords = exception;
            }
            return View();
        }
        public long fetchSqlRead(string query)
        {
            long totalCount = 0;
            string constring = @"Data Source=10.200.1.74;Initial Catalog=herocorp;uid='autosherpa';password='P@ssw0rd@123';Connection Timeout=600;MultipleActiveResultSets=true";
            using (SqlConnection con = new SqlConnection(constring))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    totalCount = (int)cmd.ExecuteScalar();
                    con.Close();
                }
            }
            return totalCount;
        }

        public ActionResult getFilteredDatatCount(string DealerCode, string fromDate, string toDate)
        {
            try
            {
                string querry = "select count(*) from trans_bitly_karix TK(nolock)  where TK.CP_CODE ="+DealerCode+"and TK.Previous_Policy_Expiry between '"+fromDate+"' and '"+toDate+"'";
                long totalCount = fetchSqlRead(querry);
                return Json(new { success = true, totalCount = totalCount });
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
                return Json(new { success = true, totalCount = exception });
            }
        }

        #endregion

        #region dealer
        public ActionResult addDealers()
        {
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    var dealerlist = db.Herodealers.Select(m => new { id = m.DealerID , name = m.DealerName,m.DealerZone,m.DealerState , m.IsActive, m.DealerClassification }).ToList();
                    var dealerClassification = dealerlist.Select(m => new { DealerClassification = m.DealerClassification }).Distinct().ToList();
                    var dealerZone = dealerlist.Select(m => new { DealerZone = m.DealerZone }).Distinct().ToList();
                    var dealerState = dealerlist.Select(m => new { DealerState = m.DealerState }).Distinct().ToList();
         

                    ViewBag.dealerClassification = JsonConvert.SerializeObject(dealerClassification);
                    ViewBag.dealerZone = JsonConvert.SerializeObject(dealerZone);
                    ViewBag.dealerState = JsonConvert.SerializeObject(dealerState);
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        public ActionResult DealerGetDealerDetails()
        {
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    List<herodealer> dealerList = db.Herodealers.OrderByDescending(m=>m.IsActive).ToList();
                    
                    var JsonData = Json(new { data = dealerList, draw = Request["draw"], recordsTotal = dealerList.Count, recordsFiltered = dealerList.Count, exception = "" }, JsonRequestBehavior.AllowGet);
                    JsonData.MaxJsonLength = int.MaxValue;
                    return JsonData;
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
                return Json(new { data = "", exception = exception });
            }
        }


        [HttpPost]
        public ActionResult saveDealerDetails(herodealer dealerdetails)
        {
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    if(db.Herodealers.Any(m=>m.DealerCode==dealerdetails.DealerCode))
                    {
                        return Json(new { success = false, error = "Duplicate Dealer Code" +dealerdetails.DealerCode }, JsonRequestBehavior.AllowGet);
                    }

                    dealerdetails.DealerHO = 1;
                        dealerdetails.DealerFlag = 1;
                        dealerdetails.IsActive = true;
                        dealerdetails.CreatedBy = Session["UserName"].ToString();
                        dealerdetails.CreatedDate = DateTime.Now;
                        db.Herodealers.Add(dealerdetails);
                        db.SaveChanges();
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
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
                return Json(new { success = false, error = exception }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult dealerActions(long? dealerId,string actionType,string displayName)
        {
            herodealer dealerDetails = new herodealer();
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    if (db.Herodealers.Count(m => m.DealerID == dealerId) > 0)
                    {
                        dealerDetails = db.Herodealers.FirstOrDefault(m => m.DealerID == dealerId);
                        if (actionType == "Activate" || actionType == "Deactivate")
                        {
                            if (actionType == "Activate")
                            {
                                dealerDetails.IsActive = true;
                                dealerDetails.DealerClassification = "Top";
                            }
                            else if (actionType == "Deactivate")
                            {
                                dealerDetails.IsActive = false;
                                dealerDetails.DealerClassification = "Inactive";
                            }
                            db.Herodealers.AddOrUpdate(dealerDetails);
                        }

                        if (actionType == "ChangeCategoryTagic" || actionType == "ChangeCategoryDealer")
                        {
                            if (actionType == "ChangeCategoryTagic")
                            {
                                dealerDetails.AllocationFlag = "2";
                               
                            }
                            else if (actionType == "ChangeCategoryDealer")
                            {
                                dealerDetails.AllocationFlag = "1";
                                
                            }
                            db.Herodealers.AddOrUpdate(dealerDetails);
                        }


                        else if (actionType == "Edit")
                        {
                            dealerDetails.DisplayName = displayName;
                            db.Herodealers.AddOrUpdate(dealerDetails);
                        }
                        else if(actionType== "Delet")
                        {
                            db.Herodealers.Remove(dealerDetails);
                        }
                        db.SaveChanges();
                    }


                 
                  
                   
                }
                return Json(new { success = true, message=actionType }, JsonRequestBehavior.AllowGet);
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
                return Json(new { success = false, message = exception }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region InsuranceCompanyDetails
        public ActionResult addinsuranceCompany()
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public ActionResult getinsuranceCompanies()
        {
            try
            {
                using (AutoSherDBContext db = new AutoSherDBContext())
                {

                    List<insurancecompany> inscomp = db.insurancecompanies.ToList();
                    var JsonData = Json(new { data = inscomp, draw = Request["draw"], recordsTotal = inscomp.Count, recordsFiltered = inscomp.Count, exception = "" }, JsonRequestBehavior.AllowGet);
                    JsonData.MaxJsonLength = int.MaxValue;
                    return JsonData;
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
                return Json(new { data = "", exception = exception });

            }
        }


        [HttpPost]
        public ActionResult saveICdetails(insurancecompany icdetails)
        {
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    if (db.insurancecompanies.Any(m => m.iccode == icdetails.iccode))
                    {
                        return Json(new { success = false, error = "Duplicate Insurance  Code" + icdetails.iccode }, JsonRequestBehavior.AllowGet);
                    }
                    icdetails.ratecardtbl_id = icdetails.iccode;
                    icdetails.createdBy =  Session["UserName"].ToString();
                    icdetails.createdDate = DateTime.Now;

                    db.insurancecompanies.Add(icdetails);
                       db.SaveChanges();
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
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
                return Json(new { success = false, error = exception }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult insurancecompanyActions(long? id, string actionType)
        {
            insurancecompany companyDetails = new insurancecompany();
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    if (db.insurancecompanies.Count(m => m.id == id) > 0)
                    {
                        companyDetails = db.insurancecompanies.FirstOrDefault(m => m.id == id);
                        if (actionType == "Activate" || actionType == "Deactivate")
                        {
                            if (actionType == "Activate")
                            {
                                companyDetails.isactive = true;
                            }
                            else if (actionType == "Deactivate")
                            {
                                companyDetails.isactive = false;
                            }
                            db.insurancecompanies.AddOrUpdate(companyDetails);
                        }
                        else if (actionType == "Delet")
                        {
                            db.insurancecompanies.Remove(companyDetails);
                        }
                        db.SaveChanges();
                    }
                }
                return Json(new { success = true, message = actionType }, JsonRequestBehavior.AllowGet);
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
                return Json(new { success = false, message = exception }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region CRE Details

        public ActionResult creDetails()
        {
            try
            {
                using (var db = new AutoSherDBContext())
                {

                    if (TempData["exception"]!= null)
                    {
                        ViewBag.exception = TempData["exception"].ToString();
                        ViewBag.isexception = true;
                    }
                    else if(TempData["uploadId"] !=null )
                    {

                        int uploadId = Convert.ToInt32(TempData["uploadId"]);
                        var uploadDetails = db.Useruploads.Where(m => m.id == uploadId).Select(m => new { m.uploadedPath, m.discardedPath, m.duplicateCREPath,m.fileName, m.totalRecords, m.totalUploaded, m.totalDiscarded }).FirstOrDefault();

                        ViewBag.outputtable = JsonConvert.SerializeObject(uploadDetails);
                        ViewBag.issuccess = true;
                        ViewBag.totaluploaded = "Total "+ uploadDetails.totalUploaded +"  Records Uploaded";
                        
                    }


                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        public ActionResult loadDealerDetails()
        {
            try
            {
                using (AutoSherDBContext db = new AutoSherDBContext())
                {

                    var dealerDetails = db.Herodealers.Where(m => m.IsActive==true).Select(m => new { DealerCode = m.DealerCode, DealerName = m.DealerName }).OrderBy(m=>m.DealerName).ToList();
                    var JsonData = Json(new { data = dealerDetails, draw = Request["draw"], recordsTotal = dealerDetails.Count, recordsFiltered = dealerDetails.Count, exception = "" }, JsonRequestBehavior.AllowGet);
                    JsonData.MaxJsonLength = int.MaxValue;
                    return JsonData;
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
                return Json(new { data = "", exception = exception });
            }

        }

        public ActionResult creGetDetails(bool userStatus)
        {
            try
            {
                int fromIndex = Convert.ToInt32(Request["start"]);
                int toIndex = Convert.ToInt32(Request["length"]);
                string searchPattern = Request["search[value]"].Trim();
                long totalCount = 0;
                using (AutoSherDBContext db = new AutoSherDBContext())
                {
                    totalCount = db.wyzusers.Where(m =>(m.role == "CRE" || m.role == "CREManager") && (m.unAvailable == userStatus)).Count();
                    if (searchPattern!=null)
                    {
                        var user = db.wyzusers.Where(m => (m.role == "CRE" || m.role == "CREManager") &&(m.unAvailable==userStatus) && (m.userName.Contains(searchPattern) || m.phoneNumber.Contains(searchPattern))).Select(m => new { id = m.id, creManager = m.creManager, dealerName = m.dealerName, userName = m.userName, firstName = m.firstName, password = m.password, phoneNumber = m.phoneNumber, phoneIMEINo = m.phoneIMEINo, phoneIMEINo1 = m.phoneIMEINo1, herodealer_Code = m.herodealer_Code, unAvailable=m.unAvailable }).OrderByDescending(x => x.id).Skip(fromIndex).Take(toIndex).ToList();
                        var JsonData = Json(new { data = user, draw = Request["draw"], recordsTotal = totalCount, recordsFiltered = totalCount, exception = "" }, JsonRequestBehavior.AllowGet);
                        JsonData.MaxJsonLength = int.MaxValue;
                        return JsonData;
                    }
                    else
                    {
                        var user = db.wyzusers.Where(m => (m.role == "CRE" || m.role == "CREManager") && (m.unAvailable == userStatus)).Select(m => new { id = m.id, creManager = m.creManager, dealerName = m.dealerName, userName = m.userName, firstName = m.firstName, password = m.password, phoneNumber = m.phoneNumber, phoneIMEINo = m.phoneIMEINo, phoneIMEINo1 = m.phoneIMEINo1, herodealer_Code = m.herodealer_Code, unAvailable = m.unAvailable }).OrderByDescending(x => x.id).Skip(fromIndex).Take(toIndex).ToList();
                        var JsonData = Json(new { data = user, draw = Request["draw"], recordsTotal = totalCount, recordsFiltered = totalCount, exception = "" }, JsonRequestBehavior.AllowGet);
                        JsonData.MaxJsonLength = int.MaxValue;
                        return JsonData;
                    }
                   
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
                return Json(new { data = "", exception = exception });
            }
        }
        public ActionResult creGetUploads()
        {
            try
            {
                int fromIndex = Convert.ToInt32(Request["start"]);
                int toIndex = Convert.ToInt32(Request["length"]);
                string searchPattern = Request["search[value]"].Trim();
                long totalCount = 0;
                using (AutoSherDBContext db = new AutoSherDBContext())
                {
                    totalCount = db.Useruploads.Count();
                    if (searchPattern!=null&&searchPattern!="")
                    {
                        var user = db.Useruploads.Where(m => (m.fileName.Contains(searchPattern) || m.fileName.Contains(searchPattern))).Select(m => new { id = m.id, fileName = m.fileName, uploadedPath = m.uploadedPath, discardedPath = m.discardedPath, duplicateCREPath = m.duplicateCREPath, totalUploaded = m.totalUploaded, totalDiscarded = m.totalDiscarded, totalRecords = m.totalRecords,m.uploadedDate,m.uploadedTime}).OrderByDescending(x => x.id).Skip(fromIndex).Take(toIndex).ToList();
                        var JsonData = Json(new { data = user, draw = Request["draw"], recordsTotal = totalCount, recordsFiltered = totalCount, exception = "" }, JsonRequestBehavior.AllowGet);
                        JsonData.MaxJsonLength = int.MaxValue;
                        return JsonData;
                    }
                    else
                    {
                        var user = db.Useruploads.Select(m => new { id = m.id, fileName = m.fileName, uploadedPath = m.uploadedPath, discardedPath = m.discardedPath, duplicateCREPath = m.duplicateCREPath, totalUploaded = m.totalUploaded, totalDiscarded = m.totalDiscarded, totalRecords = m.totalRecords, m.uploadedDate, m.uploadedTime }).OrderByDescending(x => x.id).Skip(fromIndex).Take(toIndex).ToList();
                        var JsonData = Json(new { data = user, draw = Request["draw"], recordsTotal = totalCount, recordsFiltered = totalCount, exception = "" }, JsonRequestBehavior.AllowGet);
                        JsonData.MaxJsonLength = int.MaxValue;
                        return JsonData;
                    }
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
                return Json(new { data = "", exception = exception });
            }
        }
        public ActionResult editPhoneAndIMEI(string id, string newPhone, string newImei, string newImei1)
        {
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    long wyzId = long.Parse(id);
                    var wyzData = db.wyzusers.FirstOrDefault(x => x.id == wyzId);

                    if (newPhone.StartsWith("+91"))
                    {
                        wyzData.phoneNumber = newPhone;
                    }
                    else
                    {
                        wyzData.phoneNumber = "+91" + newPhone;
                    }

                    if(db.wyzusers.Count(m=>m.phoneNumber.Trim()==wyzData.phoneNumber.Trim() && m.id!=wyzId)>0)
                    {
                        return Json(new { success = false, exception = "Phone Number Already Exists" }, JsonRequestBehavior.AllowGet);
                    }
                    wyzData.phoneIMEINo = newImei==null||newImei==""?newImei: wyzData.phoneIMEINo = newImei.Trim();
                    wyzData.phoneIMEINo1 = newImei1 == null|| newImei1 == ""? newImei1 : wyzData.phoneIMEINo1 = newImei1.Trim();
                    //if (!string.IsNullOrEmpty(newImei))
                    //    {
                    //        wyzData.phoneIMEINo = newImei.Trim();
                    //    }
                    //if(!string.IsNullOrEmpty(newImei1))
                    // {
                    //    wyzData.phoneIMEINo1 = newImei1.Trim();
                    //}
                    wyzData.phoneNumber = wyzData.phoneNumber.Trim();

                    db.wyzusers.AddOrUpdate(wyzData);
                    db.SaveChanges();
                }
                return Json(new { success = true, exception = "" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                string exception = "";
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
                return Json(new { success = false, exception = exception }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult wyzuserActions(long? userId, string actionType)
        {
            wyzuser wyzuserDetails = new wyzuser();
            dealerusers dealerusers = new dealerusers();
            bool isCre = false;
                        try
            {
                using (var db = new AutoSherDBContext())
                {
                    if (db.wyzusers.Count(m => m.id == userId) > 0)
                    {
                        wyzuserDetails = db.wyzusers.FirstOrDefault(m => m.id == userId);
                        string role = wyzuserDetails.role;
                        if(!(string.IsNullOrEmpty(role)))
                        {
                            isCre = role.Trim()== "CRE" ? true : false;
                        }

                        if (isCre)
                        {
                            dealerusers = db.Dealerusers.FirstOrDefault(m => m.WyzuserID == userId);
                        }
                        if (actionType == "Activate" || actionType == "Deactivate")
                        {
                            if (actionType == "Activate")
                            {
                                wyzuserDetails.unAvailable = false;
                                wyzuserDetails.DateOfUserCreation = Convert.ToDateTime(DateTime.Today.ToString("dd-MM-yyyy"));
                                if (isCre)
                                {
                                    dealerusers.isActive = true;
                                }
                            }
                            else if (actionType == "Deactivate")
                            {
                                if(db.insuranceassignedinteractions.Count(m=>m.wyzUser_id==userId)>0)
                                {
                                    return Json(new { success = false, message = "Data is Already Assigned. Please Clear the Data Before Deactivating." }, JsonRequestBehavior.AllowGet);
                                }
                                wyzuserDetails.unAvailable = true;
                                wyzuserDetails.phoneIMEINo = null;
                                wyzuserDetails.phoneIMEINo1 = null;
                                wyzuserDetails.phoneNumber = null;
                                wyzuserDetails.DateOfUserDeactivation =Convert.ToDateTime(DateTime.Today.ToString("dd-MM-yyyy"));
                                if (isCre)
                                {
                                    dealerusers.isActive = false;
                                }
                            }
                            db.wyzusers.AddOrUpdate(wyzuserDetails);
                            db.Dealerusers.AddOrUpdate(dealerusers);
                        }
                        db.SaveChanges();
                    }
                }
                return Json(new { success = true, message = actionType }, JsonRequestBehavior.AllowGet);
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
                return Json(new { success = false, message = exception }, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion

        #region CRE Details Upload

        public ActionResult DownloadExcelCREDetails()
        {
            try
            {

                using (AutoSherDBContext db = new AutoSherDBContext())
                {
                    ViewBag.ActiveCREDetails = db.wyzusers.Where(m => (m.role == "CRE" || m.role == "CREManager") && (m.unAvailable == false)).Select(m => new { id = m.id, dealerName = m.dealerName, herodealer_Code = m.herodealer_Code, userName = m.userName, firstName = m.firstName, phoneNumber = m.phoneNumber, phoneIMEINo = m.phoneIMEINo, phoneIMEINo1 = m.phoneIMEINo1, role = m.role, unAvailable = m.unAvailable, userActivationDate = m.DateOfUserCreation, userDeactivationDate = m.DateOfUserDeactivation }).OrderByDescending(x => x.id).ToList();
                    ViewBag.DeactiveCREDetails = db.wyzusers.Where(m => (m.role == "CRE" || m.role == "CREManager") && (m.unAvailable == true)).Select(m => new { id = m.id, dealerName = m.dealerName, herodealer_Code = m.herodealer_Code, userName = m.userName, firstName = m.firstName, phoneNumber = m.phoneNumber, phoneIMEINo = m.phoneIMEINo, phoneIMEINo1 = m.phoneIMEINo1, role = m.role, unAvailable = m.unAvailable, userActivationDate = m.DateOfUserCreation, userDeactivationDate = m.DateOfUserDeactivation }).OrderByDescending(x => x.id).ToList();

                }

                DataTable Dt = new DataTable();
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode("Super_Admin_Active__Deactive_User_IDs", System.Text.Encoding.UTF8));
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws1_Active = pck.Workbook.Worksheets.Add("Active");
                    ExcelWorksheet ws2_Deactive = pck.Workbook.Worksheets.Add("Deactive");
                    ws1_Active.Cells[1, 1].Value = "Dealer Name";
                    ws1_Active.Cells[1, 2].Value = "Dealer Code";
                    ws1_Active.Cells[1, 3].Value = "User Name";
                    ws1_Active.Cells[1, 4].Value = "Name";
                    ws1_Active.Cells[1, 5].Value = "Mobile";
                    ws1_Active.Cells[1, 6].Value = "IMEI No 1";
                    ws1_Active.Cells[1, 7].Value = "IMEI No 2";
                    ws1_Active.Cells[1, 8].Value = "Role";
                    ws1_Active.Cells[1, 9].Value = "Activation Date";
                    ws1_Active.Cells[1, 10].Value = "Deactivation Date";

                    int ws1_rowNo = 2;
                    foreach (var item in ViewBag.ActiveCREDetails)
                    {
                        ws1_Active.Cells[ws1_rowNo, 1].Value = item.dealerName;
                        ws1_Active.Cells[ws1_rowNo, 2].Value = item.herodealer_Code;
                        ws1_Active.Cells[ws1_rowNo, 3].Value = item.userName;
                        ws1_Active.Cells[ws1_rowNo, 4].Value = item.firstName;
                        ws1_Active.Cells[ws1_rowNo, 5].Value = item.phoneNumber;
                        ws1_Active.Cells[ws1_rowNo, 6].Value = item.phoneIMEINo;
                        ws1_Active.Cells[ws1_rowNo, 7].Value = item.phoneIMEINo1;
                        ws1_Active.Cells[ws1_rowNo, 8].Value = item.role;
                        ws1_Active.Cells[ws1_rowNo, 9].Value = item.userActivationDate;
                        ws1_Active.Cells[ws1_rowNo, 10].Value = item.userDeactivationDate;
                        ws1_rowNo++;
                    }

                    ws2_Deactive.Cells[1, 1].Value = "Dealer Name";
                    ws2_Deactive.Cells[1, 2].Value = "Dealer Code";
                    ws2_Deactive.Cells[1, 3].Value = "User Name";
                    ws2_Deactive.Cells[1, 4].Value = "Name";
                    ws2_Deactive.Cells[1, 5].Value = "Mobile";
                    ws2_Deactive.Cells[1, 6].Value = "IMEI No 1";
                    ws2_Deactive.Cells[1, 7].Value = "IMEI No 2";
                    ws2_Deactive.Cells[1, 8].Value = "Role";
                    ws2_Deactive.Cells[1, 9].Value = "Activation Date";
                    ws2_Deactive.Cells[1, 10].Value = "Deactivation Date";

                    int ws2_rowNo = 2;
                    foreach (var item in ViewBag.DeactiveCREDetails)
                    {
                        ws2_Deactive.Cells[ws2_rowNo, 1].Value = item.dealerName;
                        ws2_Deactive.Cells[ws2_rowNo, 2].Value = item.herodealer_Code;
                        ws2_Deactive.Cells[ws2_rowNo, 3].Value = item.userName;
                        ws2_Deactive.Cells[ws2_rowNo, 4].Value = item.firstName;
                        ws2_Deactive.Cells[ws2_rowNo, 5].Value = item.phoneNumber;
                        ws2_Deactive.Cells[ws2_rowNo, 6].Value = item.phoneIMEINo;
                        ws2_Deactive.Cells[ws2_rowNo, 7].Value = item.phoneIMEINo1;
                        ws2_Deactive.Cells[ws2_rowNo, 8].Value = item.role;
                        ws2_Deactive.Cells[ws2_rowNo, 9].Value = item.userActivationDate;
                        ws2_Deactive.Cells[ws2_rowNo, 10].Value = item.userDeactivationDate;
                        ws2_rowNo++;
                    }

                    string fileName = "Super_Admin_Active__Deactive_User_IDs.xlsx";
                    Session["CREDetails_FileName"] = fileName;
                    Session["DownloadExcel_CREDetails"] = pck.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
            }
            return Json(new { JsonRequestBehavior.AllowGet });
        }
        public ActionResult ExcelExport()
        {
            try
            {

                DataTable Dt = new DataTable();
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode("wyzuser_SampleExcel", System.Text.Encoding.UTF8));
                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet1");
                    ws.Cells[1, 1].Value = "creManager";
                    //ws.Cells[1, 2].Value = "dealerName";
                    ws.Cells[1, 2].Value = "firstName";
                    ws.Cells[1, 3].Value = "lastName";
                    ws.Cells[1, 4].Value = "password";
                    ws.Cells[1, 5].Value = "userName";
                    ws.Cells[1, 6].Value = "phoneNumber";
                    ws.Cells[1, 7].Value = "role";
                    ws.Cells[1, 8].Value = "phoneIMEINo";
                    ws.Cells[1, 9].Value = "phoneIMEINo1";
                    //ws.Cells[1, 10].Value = "herodealer_code";
                    ws.Cells[1, 10].Value = "Dealer_code";
                    string fileName = "wyzuser_SampleExcel.xlsx";
                    Session["FileName"] = fileName;
                    Session["DownloadExcel_FileManager"] = pck.GetAsByteArray();
                }
            }
            catch (Exception ex)
            {
            }
            return Json(new { JsonRequestBehavior.AllowGet });
        }
        public ActionResult Download()
        {
            try
            {
                string fileName = Session["FileName"].ToString();

                if (Session["DownloadExcel_FileManager"] != null)
                {
                    byte[] data = Session["DownloadExcel_FileManager"] as byte[];
                    return File(data, "application/octet-stream", fileName);
                }
                else
                {
                    return new EmptyResult();
                }
            }
            catch (Exception ex)
            {

            }
            return new EmptyResult();
        }
        [HttpPost]
        public ActionResult uploadWyzuserFile()
        {
            try
            {
                string fileNameAppend = "_" + DateTime.Now.ToString("yyyyMMddhhmmss");
                DataTable originalColumn = new DataTable();
                DataTable excelColumn = new DataTable();
                long uploadId = 0;
                bool isError = false;
                string customErrors = string.Empty;
                var request = Request;
                for (int i = 0; i < request.Files.Count; i++)
                {
                    var file = request.Files[i];
                    string fileUploadPath = Server.MapPath("~/UploadedFiles/" + Session["DealerCode"].ToString() + "/CREUPLOADS/" + Session["UserName"].ToString() + "/AllRecords/");
                    if (!(Directory.Exists(fileUploadPath)))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(fileUploadPath);
                    }
                    string filename = file.FileName.Split('.')[0];
                    filename = Regex.Replace(filename, "[^a-zA-Z0-9_.]+", "");
                    string ext = System.IO.Path.GetExtension(file.FileName);
                    string final_name = filename + fileNameAppend + ext;
                    var fullPath = Path.Combine(fileUploadPath, final_name);
                    string[] imageArray = file.FileName.Split('.');
                    if (imageArray.Length != 0)
                    {
                        string extansion = imageArray[imageArray.Length - 1].ToLower();
                        file.SaveAs(fullPath);
                        FileInfo existingFile = new FileInfo(fullPath);
                        using (ExcelPackage package = new ExcelPackage(existingFile))
                        {
                            originalColumn.Columns.Add(("creManager").Trim(), typeof(string));
                            originalColumn.Columns.Add(("firstName").Trim(), typeof(string));
                            originalColumn.Columns.Add(("lastName").Trim(), typeof(string));
                            originalColumn.Columns.Add(("password").Trim(), typeof(string));
                            originalColumn.Columns.Add(("userName").Trim(), typeof(string));
                            originalColumn.Columns.Add(("phoneNumber").Trim(), typeof(string));
                            originalColumn.Columns.Add(("role").Trim(), typeof(string));
                            originalColumn.Columns.Add(("phoneIMEINo").Trim(), typeof(string));
                            originalColumn.Columns.Add(("phoneIMEINo1").Trim(), typeof(string));
                            //bkg added
                            //originalColumn.Columns.Add(("herodealer_code").Trim(), typeof(string));
                            originalColumn.Columns.Add(("Dealer_code").Trim(), typeof(string));

                            ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                            int colCount = worksheet.Dimension.End.Column;
                            int maxRows = worksheet.Dimension.End.Row;
                            int rowCount = 1;
                            for (int row = 1; row <= rowCount; row++)
                            {
                                for (int col = 1; col <= colCount; col++)
                                {
                                    if (worksheet.Cells[row, col].Value != null && worksheet.Cells[row, col].Value.ToString().Trim().Length > 0)
                                    {
                                        if (excelColumn.Columns.Contains(worksheet.Cells[row, col].Value.ToString().Trim()))
                                        {

                                        }
                                        else
                                        {
                                            excelColumn.Columns.Add(worksheet.Cells[row, col].Value?.ToString().Trim());
                                        }
                                    }
                                }
                            }

                            if (excelColumn.Columns.Count != originalColumn.Columns.Count)
                            {
                                customErrors = "Columns in the uploaded files are Changed";
                                TempData["exception"] = customErrors;
                                isError = true;
                            }
                            else if (maxRows <= 1)
                            {
                                customErrors = "You cannot Upload Empty file..";
                                TempData["exception"] = customErrors;
                                isError = true;

                            }
                            DataTable d = CompareTwoDataTable(originalColumn, excelColumn);
                            if (d.Columns.Count > 0)
                            {
                                customErrors = "Columns in the uploaded files are changed, expected colum -  " + d.Columns[0].ColumnName;
                                TempData["exception"] = customErrors;
                                isError = true;
                            }
                            if (isError == false)
                            {
                              uploadId= insertData(fullPath);
                                TempData["uploadId"] = uploadId;
                            }
                        }

                    }
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
                TempData["exception"] = exception;
                TempData["noexception"] = false; ;

            }
            return RedirectToAction("creDetails");

        }
        public static DataTable CompareTwoDataTable(DataTable orgialCol, DataTable excelCol)
        {
            DataTable d3 = new DataTable();

            try
            {
                for (int i = 0; i < orgialCol.Columns.Count; i++)
                {
                    for (int j = 0; j < excelCol.Columns.Count; j++)
                    {
                        if (orgialCol.Columns[i].ColumnName == excelCol.Columns[j].ColumnName)
                        {
                            break;
                        }
                        else if (excelCol.Columns.Count - 1 == j)
                        {
                            d3.Columns.Add(orgialCol.Columns[i].ColumnName);
                            return d3;
                        }
                    }
                }
                return d3;

            }
            catch (Exception ex)
            {

            }
            return d3;
        }
        public static DataSet GetDataTableFromExcel(string path, bool hasHeader = true)
        {
            DataSet FinalRecords = new DataSet();
            DataTable errorTable = new DataTable();
            string errorcolumn = string.Empty;
            string keyRowNo = string.Empty;
            bool errorinRow = false;
            using (var db = new AutoSherDBContext())
            {
                using (var pck = new OfficeOpenXml.ExcelPackage())
                {
                    using (var stream = System.IO.File.OpenRead(path))
                    {
                        pck.Load(stream);
                    }
                    var ws = TrimEmptyRows(pck.Workbook.Worksheets.First());


                    DataTable wyzuserDatatable = new DataTable();
                    foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                    {
                        wyzuserDatatable.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                        errorTable.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                    }
                    wyzuserDatatable.Columns["creManager"].DataType = typeof(string);
                    errorTable.Columns["creManager"].DataType = typeof(string);
                    wyzuserDatatable.Columns["firstName"].DataType = typeof(string);
                    errorTable.Columns["firstName"].DataType = typeof(string);
                    wyzuserDatatable.Columns["lastName"].DataType = typeof(string);
                    errorTable.Columns["lastName"].DataType = typeof(string);
                    wyzuserDatatable.Columns["password"].DataType = typeof(string);
                    errorTable.Columns["password"].DataType = typeof(string);
                    wyzuserDatatable.Columns["userName"].DataType = typeof(string);
                    errorTable.Columns["userName"].DataType = typeof(string);
                    wyzuserDatatable.Columns["phoneNumber"].DataType = typeof(string);
                    errorTable.Columns["phoneNumber"].DataType = typeof(string);
                    wyzuserDatatable.Columns["role"].DataType = typeof(string);
                    errorTable.Columns["role"].DataType = typeof(string);
                    wyzuserDatatable.Columns["phoneIMEINo"].DataType = typeof(string);
                    errorTable.Columns["phoneIMEINo"].DataType = typeof(string);
                    wyzuserDatatable.Columns["phoneIMEINo1"].DataType = typeof(string);
                    errorTable.Columns["phoneIMEINo1"].DataType = typeof(string);
                    //bkg addedd
                    //wyzuserDatatable.Columns["herodealer_code"].DataType = typeof(string);
                    //errorTable.Columns["herodealer_code"].DataType = typeof(string);
                    wyzuserDatatable.Columns["Dealer_code"].DataType = typeof(string);
                    errorTable.Columns["Dealer_code"].DataType = typeof(string);


                    DataColumn errocol = new DataColumn("Error", typeof(string));
                    errocol.AllowDBNull = true;
                    errorTable.Columns.Add(errocol);

                    var startRow = hasHeader ? 2 : 1;
                    for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                    {
                        var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                        if (wsRow.Value != null)
                        {
                            DataRow row = wyzuserDatatable.Rows.Add();
                            foreach (var cell in wsRow)
                            {
                                try
                                {
                                    if (wyzuserDatatable.Columns[cell.Start.Column - 1].DataType == typeof(decimal))
                                    {
                                        try
                                        {
                                            if (cell.Text.ToString().Trim().Length > 0)
                                            {
                                                decimal val = Convert.ToDecimal(cell.Text);
                                                row[cell.Start.Column - 1] = val;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            if (errorinRow)
                                            {
                                                errorcolumn = errorcolumn + " ,  Error in Column Name  " + wyzuserDatatable.Columns[cell.Start.Column - 1].ColumnName + " - values -" + cell.Text.ToString();

                                            }
                                            else
                                            {
                                                errorcolumn = "Error in Column Name  " + wyzuserDatatable.Columns[cell.Start.Column - 1].ColumnName + " - values -" + cell.Text.ToString();

                                            }
                                            keyRowNo = rowNum.ToString();
                                            errorinRow = true;
                                        }
                                    }
                                    else if (wyzuserDatatable.Columns[cell.Start.Column - 1].DataType == typeof(string))
                                    {
                                        try
                                        {
                                            if (cell.Text.ToString().Trim().Length > 0)
                                            {
                                                string val = cell.Text.ToString();
                                                row[cell.Start.Column - 1] = val;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            if (errorinRow)
                                            {
                                                errorcolumn = errorcolumn + " ,  Error in Column Name  " + wyzuserDatatable.Columns[cell.Start.Column - 1].ColumnName + " - values -" + cell.Text.ToString();

                                            }
                                            else
                                            {
                                                errorcolumn = "Error in Column Name  " + wyzuserDatatable.Columns[cell.Start.Column - 1].ColumnName + " - values -" + cell.Text.ToString();

                                            }
                                            keyRowNo = rowNum.ToString();
                                            errorinRow = true;
                                        }
                                    }

                                }
                                catch (Exception ex)
                                {

                                }
                            }
                            if (errorinRow)
                            {
                                errorTable.ImportRow(row);
                                errorTable.AcceptChanges();
                                row.Delete();
                                wyzuserDatatable.AcceptChanges();
                                errorinRow = false;
                                int no = errorTable.Rows.Count - 1;
                                errorTable.Rows[no]["Error"] = errorcolumn;
                                errorcolumn = string.Empty;
                                errorTable.AcceptChanges();
                            }
                        }


                    }
                    wyzuserDatatable.AcceptChanges();
                    FinalRecords.Tables.Add(wyzuserDatatable);
                    FinalRecords.Tables.Add(errorTable);
                    return FinalRecords;
                }
            }
        }
        private static ExcelWorksheet TrimEmptyRows(ExcelWorksheet worksheet)
        {
            //loop all rows in a file
            for (int i = worksheet.Dimension.Start.Row; i <=
           worksheet.Dimension.End.Row; i++)
            {
                bool isRowEmpty = true;
                //loop all columns in a row
                for (int j = worksheet.Dimension.Start.Column; j <= worksheet.Dimension.End.Column; j++)
                {
                    if (worksheet.Cells[i, j].Value != null)
                    {
                        isRowEmpty = false;
                        break;
                    }
                }
                if (isRowEmpty)
                {
                    worksheet.DeleteRow(i);
                }
            }
            return worksheet;
        }
        public long insertData(string path)
        {
            DataSet excelDataTables = new DataSet();
            DataTable finalTable = new DataTable();
            DataTable errorTable = new DataTable();
            int duplicatecount = 0;
            int duplicatephonecount = 0;
            int totaluploaded = 0;
            bool isinvalidphone = false;
            List<wyzuser> allduplicatewyzusers = new List<wyzuser>();
            List<wyzuser> wyzuserDetails = new List<wyzuser>();
            string allduplicatewyzusersphonenumbers = string.Empty;
            string JSONString = string.Empty;

            using (var db = new AutoSherDBContext())
            {
                db.Database.CommandTimeout = 600;
                excelDataTables = GetDataTableFromExcel(path);
                finalTable = excelDataTables.Tables[0];
                errorTable = excelDataTables.Tables[1];

                if (finalTable.Rows.Count > 0)
                {

                    finalTable = finalTable.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is DBNull || string.IsNullOrWhiteSpace(field as string))).CopyToDataTable();

                    //bkg added
                    if (finalTable.Columns.Contains("Dealer_code"))
                    {
                        finalTable.Columns["Dealer_code"].ColumnName = "herodealer_code";
                    }

                    JSONString = JsonConvert.SerializeObject(finalTable);
                    wyzuserDetails = JsonConvert.DeserializeObject<List<wyzuser>>(JSONString);

                    var herodealerList = db.Herodealers.ToList();


                    foreach (wyzuser wyzuser_single in wyzuserDetails)
                    {
                        if (wyzuser_single.phoneNumber != null)
                        {
                            wyzuser_single.phoneNumber = wyzuser_single.phoneNumber.Trim();
                            if (wyzuser_single.phoneNumber.Length > 13)
                            {
                                isinvalidphone = true;
                            }

                            else if (wyzuser_single.phoneNumber.Length == 12)
                            {
                                string firstcharacter = wyzuser_single.phoneNumber.Substring(0, 2);
                                if (firstcharacter == "91")
                                {
                                    wyzuser_single.phoneNumber = "+" + wyzuser_single.phoneNumber;
                                }
                                else
                                {
                                    isinvalidphone = true;
                                }

                            }
                            else if (wyzuser_single.phoneNumber.Contains("+91"))
                            {
                                wyzuser_single.phoneNumber = wyzuser_single.phoneNumber;
                            }
                            else
                            {

                                if (wyzuser_single.phoneNumber.Length == 10)
                                {
                                    wyzuser_single.phoneNumber = "+91" + wyzuser_single.phoneNumber;
                                }
                                else
                                {
                                    isinvalidphone = true;
                                }
                            }
                        }

                        if (wyzuser_single.userName != null && wyzuser_single.userName != "")
                        {
                            wyzuser_single.userName = wyzuser_single.userName.Trim();
                            if (db.wyzusers.Any(m => m.userName.Trim() == wyzuser_single.userName))
                            {
                                duplicatecount++;
                                allduplicatewyzusers.Add(wyzuser_single);
                            }
                            else
                            {
                                if (isinvalidphone == true || db.wyzusers.Any(m => m.phoneNumber.Trim() == wyzuser_single.phoneNumber.Trim()))
                                {
                                    duplicatephonecount++;
                                    allduplicatewyzusersphonenumbers = allduplicatewyzusersphonenumbers + "," + wyzuser_single.userName;
                                    wyzuser_single.phoneNumber = "";
                                    wyzuser_single.phoneIMEINo = "";
                                    wyzuser_single.phoneIMEINo1 = "";
                                }
                                wyzuser addwyzuser = new wyzuser();
                                addwyzuser = wyzuser_single;

                                addwyzuser.workshop_id = 1;
                                addwyzuser.location_cityId = 1;
                                var dealerdetails = db.dealers.FirstOrDefault();
                                addwyzuser.dealer_id = dealerdetails.id;
                                addwyzuser.dealerCode = dealerdetails.dealerCode;
                                addwyzuser.dealerId = dealerdetails.dealerId;
                                addwyzuser.updatedDate = DateTime.Now;
                                addwyzuser.updatedBy = Convert.ToInt64(Session["UserId"]);
                                addwyzuser.insuranceRole = true;
                                if((!string.IsNullOrEmpty(addwyzuser.role)))
                                {
                                    addwyzuser.role = addwyzuser.role.Trim();
                                    
                                    if (addwyzuser.role.ToLower()== "admin")
                                    {
                                        addwyzuser.role = "Admin";
                                    }
                                    if (addwyzuser.role.ToLower()=="cre")
                                    {
                                        addwyzuser.role = "CRE";
                                    }
                                    else if (addwyzuser.role.ToLower()== "cremanager")
                                    {
                                        addwyzuser.role = "CREManager";
                                    }
                                    else if (addwyzuser.role.ToLower()== "others")
                                    {
                                        addwyzuser.role = "OTHERS";
                                    }
                                    else if (addwyzuser.role.ToLower()== "superadmin")
                                    {
                                        addwyzuser.role = "SuperAdmin";
                                    }
                                    else if (addwyzuser.role.ToLower()== "zonemanager")
                                    {
                                        addwyzuser.role = "ZoneManager";
                                    }
                                    else if (addwyzuser.role.ToLower()== "statemanager")
                                    {
                                        addwyzuser.role = "StateManager";
                                    }
                                }
                                if (addwyzuser.phoneIMEINo != null && addwyzuser.phoneIMEINo != "")
                                {
                                    addwyzuser.phoneIMEINo = addwyzuser.phoneIMEINo.Trim();
                                }
                                if (addwyzuser.phoneIMEINo1 != null && addwyzuser.phoneIMEINo1 != "")
                                {
                                    addwyzuser.phoneIMEINo1 = addwyzuser.phoneIMEINo1.Trim();
                                }
                                if (addwyzuser.role.ToUpper() == "CRE")
                                {
                                    addwyzuser.role1 = "2";
                                }
                                else if (addwyzuser.role == "CREManager")
                                {
                                    addwyzuser.role1 = "1";
                                }

                                //long? dealerCode = Convert.ToInt64(wyzuser_single.herodealer_Code);
                                string dealerCode = wyzuser_single.herodealer_Code.ToString();
                                if ((herodealerList.Count(m => m.DealerCode == dealerCode) > 0) && (herodealerList.Any(m => m.DealerCode == dealerCode)))
                                {
                                    addwyzuser.herodealer_ID = herodealerList.Where(m => m.DealerCode == dealerCode).FirstOrDefault().DealerID;
                                    addwyzuser.dealerName = herodealerList.Where(m => m.DealerCode == dealerCode).FirstOrDefault().DealerName;
                                    string ZoneManager= herodealerList.Where(m => m.DealerCode == dealerCode).FirstOrDefault().DealerZone;
                                    ZoneManager = ZoneManager + "_zonemanager".ToLower().Trim();
                                    addwyzuser.ZoneManager = ZoneManager.ToLower().Trim();
                                    string StateManager = herodealerList.Where(m => m.DealerCode == dealerCode).FirstOrDefault().DealerState;
                                    StateManager = StateManager + "_statemanager";
                                    addwyzuser.StateManager = StateManager.ToLower().Trim();

                                    if(herodealerList.Count(m=>m.DealerCode==dealerCode && m.DealerClassification!="Top")>0)
                                    {
                                        herodealer activateherodealers = new herodealer();
                                        activateherodealers = herodealerList.FirstOrDefault(m => m.DealerCode == dealerCode && m.DealerClassification != "Top");
                                        activateherodealers.DealerClassification = "Top";
                                        activateherodealers.IsActive = true;
                                        db.Herodealers.AddOrUpdate(activateherodealers);
                                        //db.SaveChanges();
                                    }
                                }
                                db.wyzusers.Add(addwyzuser);
                                db.SaveChanges();
                                totaluploaded++;

                                if (addwyzuser.role.ToUpper() == "CRE" )
                                {
                                    dealerusers dealeruser = new dealerusers();
                                    dealeruser.DealerID = addwyzuser.herodealer_ID;
                                    dealeruser.WyzuserID = addwyzuser.id;
                                    dealeruser.isActive = true;
                                    db.Dealerusers.Add(dealeruser);
                                    db.SaveChanges();
                                }
                            }
                        }
                        isinvalidphone = false;
                    }
                }


                    string Fullfilename = Path.GetFileName(path);
                string filename = Fullfilename.Split('.')[0];
                string ext = Fullfilename.Split('.')[1];

                useruploads UserUploads = new useruploads();
                    UserUploads.fileName = filename;
                    UserUploads.rawfilePath = path;
                    UserUploads.duplicatephoneNumber = "("+ duplicatephonecount+")"+ allduplicatewyzusersphonenumbers;
                    UserUploads.duplicateUser = "("+ duplicatecount+")"+ string.Join(",", allduplicatewyzusers);
                    UserUploads.totalRecords = finalTable.Rows.Count + errorTable.Rows.Count;
                    UserUploads.totalDiscarded = errorTable.Rows.Count+  duplicatecount;
                    if (errorTable.Rows.Count > 0)
                    {
                        string DiscardUploadPath = Server.MapPath("~/UploadedFiles/" + Session["DealerCode"].ToString() + "/CREUPLOADS/" + Session["UserName"].ToString() + "/DiscardedRecords/");
                        if (!(Directory.Exists(DiscardUploadPath)))
                        {
                            DirectoryInfo di = Directory.CreateDirectory(DiscardUploadPath);
                        }
                        string destinationFile = System.IO.Path.Combine(DiscardUploadPath, filename + "_Discarded."+ext);
                        DataSetToExcel(errorTable, destinationFile,"Discarded CRE");
                        UserUploads.discardedPath = destinationFile;

                    }
                    if (totaluploaded > 0)
                    {
                        string uploadedFilePath = Server.MapPath("~/UploadedFiles/" + Session["DealerCode"].ToString() + "/CREUPLOADS/" + Session["UserName"].ToString() + "/CompletedFiles/");

                        if (!(Directory.Exists(uploadedFilePath)))
                        {
                            DirectoryInfo di = Directory.CreateDirectory(uploadedFilePath);
                        }
                        string destinationFile = System.IO.Path.Combine(uploadedFilePath, filename+"_Uploaded." + ext);
                        DataSetToExcel(finalTable, destinationFile,"Uploaded");
                        UserUploads .uploadedPath= destinationFile;
                        UserUploads.totalUploaded =Convert.ToInt32(totaluploaded);

                    }
                    if (allduplicatewyzusers.Count>0)
                    {

                        string DeletefileUploadPath = Server.MapPath("~/UploadedFiles/" + Session["DealerCode"].ToString() + "/CREUPLOADS/" + Session["UserName"].ToString() + "/DuplicareCRE/");

                        if (!(Directory.Exists(DeletefileUploadPath)))
                        {
                            DirectoryInfo di = Directory.CreateDirectory(DeletefileUploadPath);
                        }
                        string destinationFile = System.IO.Path.Combine(DeletefileUploadPath, filename+"_Duplicates." + ext);
                        using (ExcelPackage pck = new ExcelPackage())
                        {
                            pck.Workbook.Worksheets.Add("Duplicate UserName");
                            pck.Workbook.Worksheets.MoveToStart("Duplicate UserName");
                            ExcelWorksheet ws = pck.Workbook.Worksheets[1];
                            var excelExport = allduplicatewyzusers.Select(m => new { m.creManager, m.firstName, m.lastName, m.password, m.userName, m.phoneNumber, m.role, m.phoneIMEINo, m.phoneIMEINo1, m.herodealer_Code }).ToList();
                            ws.Cells["A1"].LoadFromCollection(Collection: excelExport, PrintHeaders: true);
                            pck.SaveAs(new FileInfo(destinationFile));
                        }
                        UserUploads.duplicateCREPath = destinationFile;
                    }

                UserUploads.uploadedDate = DateTime.Now;
                UserUploads.uploadedTime = DateTime.Now.ToShortTimeString();
                db.Useruploads.Add(UserUploads);
                db.SaveChanges();
                return UserUploads.id;
            }

        }
        private static void DataSetToExcel(DataTable table, string filePath,string sheetName)
        {
            using (ExcelPackage pck = new ExcelPackage())
            {
                {
                    pck.Workbook.Worksheets.Add(sheetName);
                    pck.Workbook.Worksheets.MoveToStart(sheetName);
                    ExcelWorksheet ws = pck.Workbook.Worksheets[1];
                    ws.Cells["A1"].LoadFromDataTable(table, true);
                    int numCol = ws.Dimension.Columns;
                    if(sheetName== "Discarded CRE")
                    {
                        ws.Column(numCol).Style.Font.Color.SetColor(ColorTranslator.FromHtml("#FF0000"));
                    }
                }

                pck.SaveAs(new FileInfo(filePath));
            }
        }

        #endregion

        #region paymentConformation

        public ActionResult paymentConfirmationView()
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public ActionResult GetPaymentDetails(DataTablesParam param)
        {

            List<paymentconfirmation> paymentList = new List<paymentconfirmation>();
            try
            {
                List<paymentconfirmation> paymentDetails = new List<paymentconfirmation>();
                int pageNo = 1;
                int totalCount = 0;
                using (var db = new AutoSherDBContext())
                {
                    if (param.iDisplayStart >= param.iDisplayLength)
                    {
                        pageNo = (param.iDisplayStart / param.iDisplayLength) + 1;
                    }
                    if (param.sSearch != null)
                    {
                        totalCount = db.Paymentconfirmations.Where(p => (p.New_PolicyNumber.Contains(param.sSearch.Trim()) || p.Previous_PolicyNumber.Contains(param.sSearch.Trim()))).Count();
                        paymentDetails = db.Paymentconfirmations.Where(p => (p.New_PolicyNumber.Contains(param.sSearch.Trim()) || p.Previous_PolicyNumber.Contains(param.sSearch.Trim()))).OrderByDescending(x => x.id).Skip((pageNo - 1) * param.iDisplayLength).Take(param.iDisplayLength).ToList();
                    }
                    else
                    {
                        totalCount = db.Paymentconfirmations.Count();
                        paymentDetails = db.Paymentconfirmations.OrderByDescending(x => x.id).Skip((pageNo - 1) * param.iDisplayLength).Take(param.iDisplayLength).ToList();
                    }
                    return Json(new { aaData = paymentDetails, sEcho = param.sEcho, iTotalDisplayRecords = totalCount, iTotalRecords = totalCount, exception = "" }, JsonRequestBehavior.AllowGet);
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
                return Json(new { aaData = "", sEcho = param.sEcho, iTotalDisplayRecords = 0, iTotalRecords = 0, exception = exception }, JsonRequestBehavior.AllowGet);
            }
        }



        #endregion
        //#region Video Display
        //[HttpGet]
        //public ActionResult UploadVideo()
        //{
        //    List<videoDetails> videolist = new List<videoDetails>();
        //    using (var db = new AutoSherDBContext())
        //    {
        //        videolist = db.VideoDetails.OrderByDescending(m => m.UploadedDate).ToList();
        //    }

        //    return View(videolist);
        //}
        //[HttpPost]
        //public ActionResult UploadVideo(HttpPostedFileBase videoUpload)
        //{
        //    if (videoUpload != null)
        //    {
        //        string fileName = Path.GetFileName(videoUpload.FileName);
        //        int fileSize = videoUpload.ContentLength;
        //        int Size = fileSize / 1000;
        //        string fileUploadPath = Server.MapPath("~/VideoFileUpload/");
        //        if (!(Directory.Exists(fileUploadPath)))
        //        {
        //            DirectoryInfo di = Directory.CreateDirectory(fileUploadPath);
        //        }
        //        videoUpload.SaveAs(fileUploadPath + fileName);


        //        using (var db = new AutoSherDBContext())
        //        {
        //            videoDetails videoInfo = new videoDetails();
        //            videoInfo.FileName = fileName;
        //            videoInfo.FileSize = Size;
        //            videoInfo.FilePath = "~/VideoFileUpload/" + fileName;
        //            db.VideoDetails.Add(videoInfo);
        //            db.SaveChanges();
        //        }
        //    }
        //    return RedirectToAction("UploadVideo");
        //}
        //#endregion

        #region Add Message
        public ActionResult addMessage()
        {
            return View();
        }
        [HttpPost]
        public ActionResult addMessage(messageDetails messagedetails)
        {
            messageDetails existdetails = new messageDetails();
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    //db.MessageDetails.Where(x => x.isactive == true).ToList().ForEach(x =>
                    //{
                    //    x.isactive = false;
                    //});
                    if(db.MessageDetails.Count()>0)
                    {
                        existdetails = db.MessageDetails.FirstOrDefault();
                    }
                    existdetails.message = messagedetails.message;
                    existdetails.isactive = true;
                    existdetails.updatedOn = DateTime.Now;
                    existdetails.updatedby = Session["UserName"].ToString();
                    db.MessageDetails.AddOrUpdate(existdetails);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
            }
            catch(Exception ex)
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
                return Json(new { success = false , data = "", exception = exception });
            }
        }

        public ActionResult getMessage()
        {
            try
            {
               
                using (AutoSherDBContext db = new AutoSherDBContext())
                {
                    var messageDetails = db.MessageDetails.ToList();
                        var JsonData = Json(new { data = messageDetails, draw = Request["draw"],exception = "" }, JsonRequestBehavior.AllowGet);
                        JsonData.MaxJsonLength = int.MaxValue;
                        return JsonData;


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
                return Json(new { data = "", exception = exception });
            }
        }

        #endregion


        public ActionResult ChangeAssignment()
        {
            return View();
        }

        public ActionResult ModalpopupSubmit(string SourceCre, string TargetCre)
        {
            long SourceCre1 = Convert.ToInt32(SourceCre);
            long TargetCre1 = Convert.ToInt32(TargetCre);
            //long heroDealerid = Convert.ToInt32(Session["heroDealerid"].ToString());

            try
            {
                using (var db = new AutoSherDBContext())
                {
                    var assignedinterctionDetails = db.insuranceassignedinteractions.Where(m => m.wyzUser_id == SourceCre1).ToList();
                      if (assignedinterctionDetails!=null)
                    {
                        foreach (var interactionDetails in assignedinterctionDetails)
                        {
                            interactionDetails.assigned_manager_id = Convert.ToInt32(Session["UserId"]);
                            interactionDetails.wyzUser_id = SourceCre1;
                            interactionDetails.assigned_wyzuser_id = SourceCre1;
                            interactionDetails.isAutoAssigned = false;
                            db.insuranceassignedinteractions.AddOrUpdate(interactionDetails);
                            db.SaveChanges();

                            change_assignment_records chng = new change_assignment_records();
                            chng.assignedinteraction_id = interactionDetails.id;
                            chng.last_wyzuserId = Convert.ToInt64(interactionDetails.wyzUser_id);
                            chng.campaign_id = interactionDetails.campaign_id ?? default(long);
                            chng.moduletypeIs = 2;
                            chng.new_wyzuserId = Convert.ToInt64(SourceCre1);
                            chng.updatedDate = DateTime.Now;
                            chng.updatedType = "Enquiry";
                            chng.assigned_manager_id = Convert.ToInt32(Session["UserId"]);
                            db.change_assignment_records.Add(chng);
                            db.SaveChanges();

                        }


                    }
                    

                }

                return Json(new { success = true, message = "Success", JsonRequestBehavior.AllowGet });
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


        public ActionResult getwyzuserdata(long? dealerId, string actionType)
        {

            List <wyzuser> sourceCreList = new List <wyzuser>();
            List<wyzuser> TargetCreList = new List<wyzuser>();
            List<long?> wyzuserList = new List<long?>();
            List<long?> SourcewyzuserList = new List<long?>();
            List<long?> TargetwyzuserList = new List<long?>();
            //List<dealerusers> sourceCreList = new List<dealerusers>();
            //List<dealerusers> TargetCreList = new List<dealerusers>();

            List<dealerusers> dealerList = new List<dealerusers>();
            using (var db = new AutoSherDBContext())
            {
                dealerList = db.Dealerusers.ToList();

                if (actionType == "ChangeCategoryTagic")
                {


                    SourcewyzuserList = dealerList.Where( m =>m.DealerID==dealerId && m.iccode == null).Select(m => m.WyzuserID).ToList();
                    TargetwyzuserList = dealerList.Where(m => m.iccode != null &&  m.DealerID == dealerId).Select(m => m.WyzuserID).ToList();
                }
                else if (actionType == "ChangeCategoryDealer")
                {
                    SourcewyzuserList = dealerList.Where(m =>m.DealerID==dealerId && m.iccode != null).Select(m => m.WyzuserID).ToList();
                    TargetwyzuserList = dealerList.Where(m => m.iccode == null && m.DealerID == dealerId).Select(m => m.WyzuserID).ToList();
                }
               
                sourceCreList = db.wyzusers.Where(m => SourcewyzuserList.Contains(m.id)  && m.role=="CRE" && m.unAvailable==false).ToList();
                TargetCreList = db.wyzusers.Where(m => TargetwyzuserList.Contains(m.id) && m.role == "CRE" && m.unAvailable == false).ToList();
              
                
               
               // TargetCreList = db.Dealerusers.Where(m => m.DealerID == dealerId).ToList();

                // return Json(new { success = true, sourceCreList= SourcewyzuserList, TargetCreList = TargetwyzuserList,  JsonRequestBehavior.AllowGet });
                return Json(new { success = true, sourceCreList = sourceCreList.Select(m => new {  m.userName, m.id }).ToList(), TargetCreList = TargetCreList.Select(m => new { m.userName , m.id }).ToList(), JsonRequestBehavior.AllowGet });


            }

            return View();
        }

        #region EIBL Winback Policy Data Fetching Section

        [HttpGet]
        [ActionName("fetchWinbackRollover")]
        public ActionResult FetchWinbackRolloverPolicyCustomerDetails()
        {
            using (var db = new AutoSherDBContext())
            {
                var brands = db.brands.Where(m => m.isactive == true).ToList();
                ViewBag.BrandList = brands;
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetDealerBasedOnOEMAsync(string oemid)
        {
            int oemID = 0;
            if (!string.IsNullOrEmpty(oemid))
            {
                oemID = int.Parse(oemid);
            }
            using (var db = new AutoSherDBContext())
            {
                var dealernames = await db.oem_dealer
                             .Where(o => o.OEM_Id == oemID)
                             .Join(db.Herodealers,
                                   o => o.dealerID,
                                   h => h.DealerID,
                                   (o, h) => new { h.DealerID, h.DealerName, h.DealerCode })
                             .ToListAsync();

                return Json(new { dealerNames = dealernames}, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> FetchWinbackRolloverPolicyCustomerDetailsAsync(string oemId, string dealerId, string fromDate, string toDate, string type)
        {
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    int.TryParse(oemId, out int oemID);
                    int.TryParse(type, out int campaignType);

                    List<int> dealerIDs = dealerId.Split(',').Select(id => int.TryParse(id, out int parsedID) ? parsedID : (int?)null).Where(id => id.HasValue).Select(id => id.Value).ToList();

                    List<string> dealerCodes = await db.Herodealers.Where(m => dealerIDs.Contains(m.DealerID)).Select(m => m.DealerCode).ToListAsync();

                    string cpCodes = string.Join(",", dealerCodes);

                    var oemAndFkSubOemIds = await db.brands.Where(m => m.id == oemID).Select(m => new { m.fksuboem_id, m.brandoemid}).FirstOrDefaultAsync();

                    var requestPayload = new
                    {
                        FromDate = fromDate,
                        ToDate = toDate,
                        CPCode = cpCodes ?? "",
                        OEMID = oemAndFkSubOemIds.brandoemid ?? 0,
                        Type = campaignType,
                        SUBOEM_ID = oemAndFkSubOemIds.fksuboem_id ?? 0

                        //FromDate = fromDate,
                        //ToDate = toDate,
                        //CPCode = "SK1027700",
                        //OEMID = 15,
                        //Type = campaignType,
                    };

                    string jsonRequestPayload = JsonConvert.SerializeObject(requestPayload);

                    string URL = ConfigurationManager.AppSettings["FetchWinbackPolicy_ApiUrl"];
                    string accessToken = await GenerateToken.GenerateTokenAsync();

                    using (var httpClient = new HttpClient())
                    {
                        StringContent content = new StringContent(jsonRequestPayload, Encoding.UTF8, "application/json");

                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                        HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(URL, content);

                        string jsonResponseContent = await httpResponseMessage.Content.ReadAsStringAsync();

                        if (httpResponseMessage.IsSuccessStatusCode)
                        {
                            try
                            {
                                if (jsonResponseContent.TrimStart().StartsWith("["))
                                {
                                    var jsonContentArray = JArray.Parse(jsonResponseContent);

                                    int totalRecordsFetched = jsonContentArray.Count();

                                    if (totalRecordsFetched == 0)
                                    {
                                        return Json(new { success = false, message = $"Data not found for this Dealer. The response is empty or null" }, JsonRequestBehavior.AllowGet);
                                    }

                                    var winbackRateCards = new List<WinbackRateCardDump>();
                                    foreach (var item in jsonContentArray)
                                    {
                                        var winbackRateCard = item.ToObject<WinbackRateCardDump>();
                                        winbackRateCards.Add(winbackRateCard);
                                    }

                                    db.WinbackRateCards.AddRange(winbackRateCards);
                                    await db.SaveChangesAsync();

                                    return Json(new { success = true, message = $"Winback Policy Data Fetched and Saved Successfully.<br><b>Recieved Total Data Count: {totalRecordsFetched}</b>" }, JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    JObject jsonResponse = JObject.Parse(jsonResponseContent);

                                    if (jsonResponse.ContainsKey("ErrorMessage"))
                                    {
                                        string errorMessage = jsonResponse["ErrorMessage"]?.ToString();
                                        return Json(new { success = false, message = $"Error: {errorMessage}" }, JsonRequestBehavior.AllowGet);
                                    }

                                    return Json(new { success = false, message = $"The response is empty or null. No data to process." }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            catch (Exception ex)
                            {
                                string exception = ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? ex.Message;
                                return Json(new { success = false, message = $"Error while saving Winback Policy Data.<br>Error: {exception}" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            JObject jResponseObject = JObject.Parse(jsonResponseContent);
                            if (jResponseObject.ContainsKey("Message"))
                            {
                                return Json(new { success = false, message = $"Error: {jResponseObject["Message"]}" }, JsonRequestBehavior.AllowGet);
                            }

                            return Json(new { success = false, message = $"Error: {httpResponseMessage.StatusCode}, {jsonResponseContent}" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Server Error: {ex.InnerException?.InnerException.Message ?? ex.InnerException?.Message ?? ex.Message}" }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

    }
}