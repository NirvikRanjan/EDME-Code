using AutoSherpa_project.Models;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office.Word;
using DocumentFormat.OpenXml.Spreadsheet;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using NLog;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutoSherpa_project.Controllers
{
    [AuthorizeFilter]
    public class policyAssignmentController : Controller
    {
        #region Call Assignment
        // GET: policyAssignment
        [AuthorizeFilter]
        public ActionResult policyAssignment()
        {
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    int UserId = Convert.ToInt32(Session["UserId"].ToString());
                    var insCompany = db.insurancecompanies.Select(m => new { id = m.id + "." + m.iccode + "." + m.ratecardtbl_id, companyName = m.companyName, m.isactive }).Where(m => m.isactive == true).OrderBy(m => m.companyName).ToList();
                    var dealerlist = db.Herodealers.Select(m => new { id = m.DealerID + "." + m.DealerCode, name = m.DealerName + " " + m.DealerCity + "(" + m.DealerCode + ")", m.IsActive, m.DealerClassification }).ToList();
                    var dealerClassification = dealerlist.Select(m => new { DealerClassification = m.DealerClassification }).Distinct().ToList();
                    ViewBag.ddlinsurancelist = insCompany;
                    //ViewBag.dealerClassification = db.Herodealers.Select(m=>new { DealerClassification = m.DealerClassification }).Distinct().ToList();
                    ViewBag.dealerClassification = dealerClassification;
                    ViewBag.dealerlist = dealerlist;
                    ViewBag.dealerclassificationData = JsonConvert.SerializeObject(dealerlist);
                    var BrandS = db.brands.Where(m => m.isactive == true).ToList();
                    ViewBag.BrandList = BrandS;
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
            return View();
        }

        public ActionResult getratecarddata(string assignmentFilters)
        {
            List<vehicle> ratecarddetails = new List<vehicle>();
            List<policyasgbmnt> policyassignmentDetails = new List<policyasgbmnt>();
            assignmentFilters asignFilters = JsonConvert.DeserializeObject<assignmentFilters>(assignmentFilters);
            int fromIndex = Convert.ToInt32(Request["start"]);
            int toIndex = Convert.ToInt32(Request["length"]);
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    string procedureName = string.Empty, procedureCount = string.Empty;
                    string frmdateex = asignFilters.frmdate == null || asignFilters.frmdate == "" ? "" : Convert.ToDateTime(asignFilters.frmdate).ToString("yyyy-MM-dd");
                    string todateex = asignFilters.todate == null || asignFilters.todate == "" ? "" : Convert.ToDateTime(asignFilters.todate.ToString()).ToString("yyyy-MM-dd");
                    string oem = string.IsNullOrEmpty(asignFilters.oem) ? "" : asignFilters.oem;
                    string dealerName = string.IsNullOrEmpty(asignFilters.dealerCodes) ? "" : asignFilters.dealerCodes;
                    string ic = string.IsNullOrEmpty(asignFilters.icnames) ? "" : asignFilters.icnames;
                    if (asignFilters.procedureName == "Renewal")
                    {
                        procedureName = "HERO_fetchDetails";
                        procedureCount = "HERO_fetchDetailsCount";
                    }
                    else if (asignFilters.procedureName == "Winback")
                    {
                        procedureName = "HERO_Winback_fetchDetails";
                        procedureCount = "HERO_Winback_fetchDetailsCount";
                    }

                    List<string> IcCodeID = asignFilters.icnames == null || asignFilters.icnames == "" ? null : asignFilters.icnames.Split(',').Select(x => (x.Trim())).ToList();
                    List<string> DealerID = asignFilters.dealerCodes == null || asignFilters.dealerCodes == "" ? null : asignFilters.dealerCodes.Split(',').Select(x => (x.Trim())).ToList();
                    if (toIndex > 0 && toIndex <= 100)
                    {
                        db.Database.ExecuteSqlCommand("TRUNCATE TABLE tempic");
                        List<TempIC> tempIC = new List<TempIC>();
                        if (IcCodeID != null)
                        {
                            foreach (string icid in IcCodeID)
                            {
                                /*long IcCode = Convert.ToInt64(icid.Split('.')[0]);
                                long iccodeId = Convert.ToInt64(icid.Split('.')[1]);
                                long ratecardId = Convert.ToInt64(icid.Split('.')[2]);
                                TempIC tempICadd = new TempIC();
                                tempICadd.IcCode = iccodeId;
                                tempICadd.IcCodeID = IcCode;
                                tempICadd.icproduct_id = ratecardId;*/

                                //Edme
                                long IcoproductCode = Convert.ToInt64(icid);
                                long brandid = Convert.ToInt64(oem);
                                string newfksuboemid = db.brands.Where(m => m.id == brandid).Select(m => m.fksuboem_id).FirstOrDefault().ToString();
                                TempIC tempICadd = new TempIC();
                                tempICadd.icproduct_id = IcoproductCode;
                                tempICadd.fksuboemid = long.Parse(newfksuboemid);
                                tempIC.Add(tempICadd);
                            }
                            db.TempICs.AddRange(tempIC);
                        }
                        db.Database.ExecuteSqlCommand("TRUNCATE TABLE tempDealers");
                        List<tempDealers> listtempDealrs = new List<tempDealers>();
                        if (DealerID != null)
                        {
                            foreach (string did in DealerID)
                            {
                                long dealerid = Convert.ToInt64(did.Split('.')[0]);
                                var dcode = db.oem_dealer.Where(m => m.dealerID == dealerid).Select(m => m.dealerCode).FirstOrDefault();
                                //long dealercode = Convert.ToInt64(dcode);
                                //long dealercode = Convert.ToInt64(did.Split('.')[1]);
                                tempDealers tempDealrs = new tempDealers();
                                tempDealrs.DealerID = dealerid;
                                tempDealrs.DealerCode = dcode;
                                listtempDealrs.Add(tempDealrs);
                            }
                            db.TempDealers.AddRange(listtempDealrs);
                        }
                        db.SaveChanges();
                    }

                    string str1 = @"CALL " + procedureName + "(@InPolicyExpiryFromDate,@InPolicyExpiryToDate,@startwith,@inlength,@inoem,@indealername,@ininscompany)";

                    MySqlParameter[] parameter1 = new MySqlParameter[] {
                        new MySqlParameter("InPolicyExpiryFromDate", frmdateex),
                        new MySqlParameter("InPolicyExpiryToDate", todateex),
                        new MySqlParameter("startwith", fromIndex),
                        new MySqlParameter("inlength", toIndex),
                        new MySqlParameter("inoem", oem),
                        new MySqlParameter("indealername", dealerName),
                        new MySqlParameter("ininscompany", ic)

                        };
                    int totaldataavailable = db.Database.SqlQuery<int>("call " + procedureCount + "(@InPolicyExpiryFromDate,@InPolicyExpiryToDate,@inoem,@indealername,@ininscompany)",
                      new MySqlParameter[]
                      {
                                new MySqlParameter("@InPolicyExpiryFromDate",frmdateex),
                                new MySqlParameter("@InPolicyExpiryToDate",todateex),
                                new MySqlParameter("@inoem", oem),
                                new MySqlParameter("@indealername", dealerName),
                                new MySqlParameter("@ininscompany", ic)

                      }).FirstOrDefault();

                    policyassignmentDetails = db.Database.SqlQuery<policyasgbmnt>(str1, parameter1).ToList();
                    // Store the fetched data in session
                    int totalNoOfRecords = policyassignmentDetails.Count;
                    HttpContext.Session["PolicyAssignmentDetails"] = policyassignmentDetails;
                    var JsonData = Json(new { data = policyassignmentDetails, draw = Request["draw"], recordsTotal = totalNoOfRecords, recordsFiltered = totalNoOfRecords, totaldataavailable, exception = "" }, JsonRequestBehavior.AllowGet);
                    JsonData.MaxJsonLength = int.MaxValue;
                    return JsonData;
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
                return Json(new { success = false, data = "", draw = Request["draw"], recordsTotal = 0, recordsFiltered = 0, exception = exception }, JsonRequestBehavior.AllowGet);

            }

        }


        public ActionResult assignpolicydata(string assignmentFilters)
        {
            //List<vehicle> ratecarddetails = new List<vehicle>();
            //List<resultsassignment> allAssignment = new List<resultsassignment>();
            assignmentFilters asignFilters = JsonConvert.DeserializeObject<assignmentFilters>(assignmentFilters);
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    string frmdateex = asignFilters.frmdate == null || asignFilters.frmdate == "" ? "" : Convert.ToDateTime(asignFilters.frmdate).ToString("yyyy-MM-dd");
                    string todateex = asignFilters.todate == null || asignFilters.todate == "" ? "" : Convert.ToDateTime(asignFilters.todate.ToString()).ToString("yyyy-MM-dd");
                    string procedureName = string.Empty;
                    string oem = string.IsNullOrEmpty(asignFilters.oem) ? "" : asignFilters.oem;
                    string dealerName = string.IsNullOrEmpty(asignFilters.dealerCodes) ? "" : asignFilters.dealerCodes;
                    string ic = string.IsNullOrEmpty(asignFilters.icnames) ? "" : asignFilters.icnames;
                    if (asignFilters.procedureName == "Renewal")
                    {
                        procedureName = "`HERO_AssignData`";
                    }
                    else if (asignFilters.procedureName == "Winback")
                    {
                        procedureName = "`HERO_Winback_AssignData`";
                    }
                    string str = @"CALL" + procedureName + "(@InPolicyExpiryFromDate,@InPolicyExpiryToDate,@inoem,@indealername,@ininscompany)";

                    MySqlParameter[] parameter = new MySqlParameter[] {
                         new MySqlParameter("InPolicyExpiryFromDate", frmdateex),
                         new MySqlParameter("InPolicyExpiryToDate", todateex),
                         new MySqlParameter("inoem", oem),
                         new MySqlParameter("indealername", dealerName),
                         new MySqlParameter("ininscompany", ic)

                         };

                    //allAssignment = db.Database.SqlQuery<resultsassignment>(str, parameter).ToList();
                    db.Database.ExecuteSqlCommandAsync(str, parameter);
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                    //DataTable allAssignment = new DataTable();

                    //string conStr = ConfigurationManager.ConnectionStrings["AutoSherContext"].ConnectionString;
                    //using (MySqlConnection connection = new MySqlConnection(conStr))
                    //{
                    //    using (MySqlCommand cmd = new MySqlCommand(procedureName, connection))
                    //    {
                    //        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    //        adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    //        adapter.SelectCommand.Parameters.Add(new MySqlParameter("InPolicyExpiryFromDate", frmdateex));
                    //        adapter.SelectCommand.Parameters.Add(new MySqlParameter("InPolicyExpiryToDate", todateex));
                    //        adapter.Fill(allAssignment);
                    //    }
                    //}

                    //var results = JsonConvert.SerializeObject(allAssignment, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    //var result = Json(new { data = results, success = true }, JsonRequestBehavior.AllowGet);
                    //result.MaxJsonLength = int.MaxValue;
                    //return result;
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
                return Json(new { success = false, message = exception }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult droppolicyData()
        {
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    int totalCount = db.Database.SqlQuery<int>("call TempDeleteData").FirstOrDefault();
                    return Json(new { success = true });

                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }
        #endregion


        #region Change Assignemnt
        public ActionResult insuranceChangeAssignment()
        {
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    int UserId = Convert.ToInt32(Session["UserId"].ToString());
                    string creManeger = Session["UserName"].ToString();

                    var creList = db.wyzusers.Where(m => m.role == "CRE" && m.unAvailable == false && m.creManager == creManeger).Select(m => new { id = m.id, name = m.firstName + "-" + m.userName, creName = m.userName }).OrderBy(m => m.creName).ToList();

                    var insCompany = db.insurancecompanies.Select(m => new { id = m.id + "." + m.iccode + "." + m.ratecardtbl_id, companyName = m.companyName, m.isactive }).Where(m => m.isactive == true).OrderBy(m => m.companyName).ToList();
                    //  var dealerlist = db.Herodealers.Select(m => new { id = m.DealerID + "." + m.DealerCode, name = m.DealerName + " " + m.DealerCity + "(" + m.DealerCode + ")", m.IsActive, m.DealerClassification }).Where(m => m.IsActive == true).ToList();
                    var renewalList = db.renewaltypes.Select(m => new { id = m.id, name = m.renewalTypeName }).ToList();
                    var campaignList = db.campaigns.Where(m => m.isactive == true).Select(m => new { id = m.id, name = m.campaignName }).ToList();
                    var marketingcampaignList = db.Marketingcampaigns.Where(m => m.isactive == true).Select(m => new { id = m.id, name = m.name }).ToList();
                    var priorityList = db.priority.Where(m => m.isactive == true).Select(m => new { id = m.id, name = m.name }).ToList();
                    //tagic ICManager
                    if (Session["UserRole"].ToString() == "ICManager")
                    {
                        var dealerlist = db.Herodealers.Where(m => m.DealerClassification == "Top" && m.IsActive == true && (m.AllocationFlag == "IC" || m.AllocationFlag == "2")).Select(m => new { id = m.DealerID + "." + m.DealerCode, name = m.DealerName + " " + m.DealerCity + "(" + m.DealerCode + ")", m.IsActive, m.DealerClassification }).ToList();
                        ViewBag.dealerlist = dealerlist;
                    }
                    // ho manager Cremanager
                    else if (Session["UserName"].ToString() == "HO_Manager")
                    {
                        var dealerlist = db.Herodealers.Where(m => m.DealerClassification == "Top" && m.IsActive == true && (m.AllocationFlag == "3")).Select(m => new { id = m.DealerID + "." + m.DealerCode, name = m.DealerName + " " + m.DealerCity + "(" + m.DealerCode + ")", m.IsActive, m.DealerClassification }).ToList();
                        ViewBag.dealerlist = dealerlist;
                    }
                    else
                    {
                        var dealerlist = db.Herodealers.Where(m => m.DealerClassification == "Top" && m.IsActive == true && (m.AllocationFlag == "IC" || m.AllocationFlag == "1")).Select(m => new { id = m.DealerID, name = m.DealerName + " " + m.DealerCity + "(" + m.DealerCode + ")", m.IsActive, m.DealerClassification }).ToList();
                        ViewBag.dealerlist = dealerlist;
                    }





                    ViewBag.ddlinsurancelist = insCompany;
                    ViewBag.ddlrenewalList = renewalList.OrderBy(m => m.name);
                    ViewBag.campaignList = campaignList.OrderBy(m => m.name);
                    ViewBag.marketingcampaignList = marketingcampaignList.OrderBy(m => m.name);
                    ViewBag.priorityList = priorityList.OrderBy(m => m.name);
                    ViewBag.creList = creList;


                    var brandS = db.brands.ToList();
                    ViewBag.brandlist = brandS;

                    var segment = db.segments.ToList();
                    ViewBag.segmentList = segment;



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
            return View();
        } 

        public ActionResult getChangeAssignmentData(string changeassignmentFilters)
        {
            Logger logger = LogManager.GetLogger("apkRegLogger");
            logger.Info("\n\n -------- Change Assignment Started--------" + DateTime.Now + "\n\n");

            List<changeAssignmentINS> changepolicyassignmentDetails = new List<changeAssignmentINS>();
            assignmentFilters asignFilters = JsonConvert.DeserializeObject<assignmentFilters>(changeassignmentFilters);
            int fromIndex = Convert.ToInt32(Request["start"]);
            int toIndex = Convert.ToInt32(Request["length"]);
            try
            {
                using (var db = new AutoSherDBContext())
                {


                    //long dealerCode = Session["HeroDealerCode"].ToString();
                    string dealerCode = Session["HeroDealerCode"].ToString();
                    long dealerId = Convert.ToInt32(Session["HeroDealerId"].ToString());

                    if (!(string.IsNullOrEmpty(asignFilters.dealerCodes)) && (Session["UserRole"].ToString() == "ICManager"))
                    {

                        dealerId = Convert.ToInt64(asignFilters.dealerCodes.Split('.')[0]);
                        //dealerCode = Convert.ToInt64(asignFilters.dealerCodes.Split('.')[1]);
                    }


                    //var test = Convert.ToInt32(asignFilters.dealerCodes);
                    //var dealerCode1 = db.Herodealers.Where(x => x.DealerID == test).ToList().FirstOrDefault();
                    //dealerCode = Convert.ToInt64(dealerCode1.DealerCode);
                    // dealerId = Convert.ToInt64(asignFilters.dealerCodes);


                    // long dealerCode1 = Convert.ToInt64(Session["HeroDealerCode"].ToString());




                    int UserId = Convert.ToInt32(Session["UserId"].ToString());

                    string dispositionStatus = asignFilters.dispositionStatus == null ? "" : asignFilters.dispositionStatus;

                    string Campaign = asignFilters.Campaign == null ? "" : asignFilters.Campaign;

                    //string herodealerCode = asignFilters.dealerCodes == null ? "" : asignFilters.dealerCodes;
                    string marketingCampaign = asignFilters.marketingCampaign == null ? "" : asignFilters.marketingCampaign;
                    string Priority = asignFilters.Priority == null ? "" : asignFilters.Priority;
                    string frmdateex = asignFilters.frmdate == null || asignFilters.frmdate == "" ? "" : Convert.ToDateTime(asignFilters.frmdate).ToString("yyyy-MM-dd");
                    string todateex = asignFilters.todate == null || asignFilters.todate == "" ? "" : Convert.ToDateTime(asignFilters.todate.ToString()).ToString("yyyy-MM-dd");
                    string renewalType = asignFilters.renewalType == null || asignFilters.renewalType == "" ? "" : asignFilters.renewalType;

                    List<string> IcCodeID = asignFilters.icnames == null || asignFilters.icnames == "" ? null : asignFilters.icnames.Split(',').Select(x => (x.Trim())).ToList();
                    List<string> dispoStatus = asignFilters.dispositionStatus == null || asignFilters.dispositionStatus == "" ? null : asignFilters.dispositionStatus.Split(',').Select(x => (x.Trim())).ToList();
                    List<string> priority = asignFilters.Priority == null || asignFilters.Priority == "" ? null : asignFilters.Priority.Split(',').Select(x => (x.Trim())).ToList();
                    List<string> campaign = asignFilters.Campaign == null || asignFilters.Campaign == "" ? null : asignFilters.Campaign.Split(',').Select(x => (x.Trim())).ToList();
                    List<string> creLists = asignFilters.cre_list == null || asignFilters.cre_list == "" ? null : asignFilters.cre_list.Split(',').Select(x => (x.Trim())).ToList();
                    List<string> yearLists = asignFilters.yearofsale == null || asignFilters.yearofsale == "" ? null : asignFilters.yearofsale.Split(',').Select(x => (x.Trim())).ToList();
                    List<string> segments = asignFilters.segment == null || asignFilters.segment == "" ? null : asignFilters.segment.Split(',').Select(x => (x.Trim())).ToList();
                    List<string> make = asignFilters.make == null || asignFilters.make == "" ? null : asignFilters.make.Split(',').Select(x => (x.Trim())).ToList();
                    List<string> policyType = asignFilters.policyType == null || asignFilters.policyType == "" ? null : asignFilters.policyType.Split(',').Select(x => (x.Trim())).ToList();
                    List<string> model = asignFilters.model == null || asignFilters.model == "" ? null : asignFilters.model.Split(',').Select(x => (x.Trim())).ToList();
                    List<string> premiumsize = asignFilters.premium == null || asignFilters.premium == "" ? null : asignFilters.premium.Split(',').Select(x => (x.Trim())).ToList();
                    List<string> quoteStatus = asignFilters.quoteStatus == null || asignFilters.quoteStatus == "" ? null : asignFilters.quoteStatus.Split(',').Select(x => (x.Trim())).ToList();

                    //if (toIndex > 0 && toIndex <= 100)
                    {
                        if (db.TempChangeAssignmentIClists.Count(m => m.ManagerID == UserId) > 0)
                        {
                            db.Database.ExecuteSqlCommand("delete from tempchangeassignmenticlist  where ManagerID=@managerid and DealerID=@indealerid ", new MySqlParameter("@managerid", UserId), new MySqlParameter("@indealerid", dealerId));
                        }
                        if (db.GetTempChangeAssignmentCrelists.Count(m => m.ManagerID == UserId) > 0)
                        {
                            db.Database.ExecuteSqlCommand("delete from tempchangeassignmentcrelist  where ManagerID=@managerid and DealerID=@indealerid ", new MySqlParameter("@managerid", UserId), new MySqlParameter("@indealerid", dealerId));
                        }

                        if (db.TempChangeAssignmentYearLists.Count(m => m.ManagerID == UserId) > 0)
                        {
                            db.Database.ExecuteSqlCommand("delete from tempchangeassignmentyearlist  where ManagerID=@managerid and DealerID=@indealerid", new MySqlParameter("@managerid", UserId), new MySqlParameter("@indealerid", dealerId));
                        }



                        List<tempChangeAssignmentIClist> tempIC = new List<tempChangeAssignmentIClist>();
                        if (IcCodeID != null)
                        {
                            foreach (string icid in IcCodeID)
                            {
                                long IcCode = Convert.ToInt64(icid.Split('.')[0]);
                                long iccodeId = Convert.ToInt64(icid.Split('.')[1]);
                                long ratecardId = Convert.ToInt64(icid.Split('.')[2]);
                                tempChangeAssignmentIClist tempICadd = new tempChangeAssignmentIClist();
                                tempICadd.ICCode = Convert.ToInt64(iccodeId);
                                tempICadd.ICID = Convert.ToInt64(IcCode);
                                tempICadd.ratecard_id = Convert.ToInt64(ratecardId);
                                tempICadd.ManagerID = UserId;

                                tempICadd.DealerID = dealerId;



                                tempIC.Add(tempICadd);
                            }
                            db.TempChangeAssignmentIClists.AddRange(tempIC);
                            logger.Info("\n\n -------- IC Aded--------\n\n");
                        }

                        List<tempChangeAssignmentYearList> tempYear = new List<tempChangeAssignmentYearList>();
                        if (yearLists != null)
                        {
                            foreach (string year in yearLists)
                            {
                                tempChangeAssignmentYearList tempyearId = new tempChangeAssignmentYearList();
                                tempyearId.SaleYear = Convert.ToInt64(year);
                                tempyearId.ManagerID = UserId;
                                tempyearId.DealerID = dealerId;
                                tempYear.Add(tempyearId);
                            }
                            db.TempChangeAssignmentYearLists.AddRange(tempYear);
                            logger.Info("\n\n -------- Year Aded--------\n\n");
                        }
                        List<tempChangeAssignmentCrelist> tempCRE = new List<tempChangeAssignmentCrelist>();
                        if (creLists != null)
                        {
                            foreach (string cres in creLists)
                            {
                                tempChangeAssignmentCrelist tempCreLists = new tempChangeAssignmentCrelist();
                                tempCreLists.ManagerID = UserId;
                                tempCreLists.DealerCode = dealerCode;
                                tempCreLists.DealerID = dealerId;
                                tempCreLists.wyzuserId = Convert.ToInt64(cres);
                                tempCRE.Add(tempCreLists);
                            }
                            db.GetTempChangeAssignmentCrelists.AddRange(tempCRE);
                            logger.Info("\n\n -------- CRE Aded--------\n\n");
                        }

                        db.SaveChanges();
                    }


                    //int totaldataavailable = db.Database.SqlQuery<int>("call HERO_Change_Assignment_Count(@inManagerID,@inpolicyexpiryfrom,@inpolicyexpiryTo,@InDealerID,@InRenewalType,@disp,@incampaign_id,@inpriorityid,@inmarketcampign)",
                    //      new MySqlParameter[]
                    //      {
                    //            new MySqlParameter("@inManagerID",UserId),
                    //            new MySqlParameter("@inpolicyexpiryfrom",frmdateex),
                    //            new MySqlParameter("@inpolicyexpiryTo",todateex),
                    //            new MySqlParameter("@InDealerID",dealerId),
                    //            new MySqlParameter("@InRenewalType", renewalType),
                    //             new MySqlParameter("disp", dispositionStatus),
                    //    new MySqlParameter("incampaign_id", Campaign),
                    //    new MySqlParameter("inpriorityid", Priority),
                    //    new MySqlParameter("inmarketcampign", marketingCampaign),
                    //      }).FirstOrDefault();

                    //string str1 = @"CALL HERO_Change_Assignment_View(@inManagerID,@inpolicyexpiryfrom,@inpolicyexpiryTo,@InDealerID,@InRenewalType,@startindex,@InLenth)";
                    //MySqlParameter[] parameter1 = new MySqlParameter[] {
                    //    new MySqlParameter("inManagerID", UserId),
                    //    new MySqlParameter("inpolicyexpiryfrom", frmdateex),
                    //    new MySqlParameter("inpolicyexpiryTo", todateex),
                    //    new MySqlParameter("InDealerID", dealerId),
                    //    new MySqlParameter("InRenewalType", renewalType),
                    //    new MySqlParameter("startindex", fromIndex),
                    //    new MySqlParameter("InLenth", toIndex)
                    //    };
                    // new code ho manager comma seperate 
                    //if ()
                    //{
                    //    dealerCode = asignFilters.dealerCodes;
                    //}
                    string str1 = @"call HERO_Change_Assignment_Count(@inManagerID,@inpolicyexpiryfrom,@inpolicyexpiryTo,@InDealerID,@InRenewalType,@disp,@incampaign_id,@inpriorityid,@dealeridin,@insegment,@inmake,@inmodel,@inpremiumsize,@inpolicytype, @inyearOfSale,@inQuoteStatus)";

                    MySqlParameter[] parameter1 = new MySqlParameter[] {
                          new MySqlParameter("@inManagerID",UserId),
                          new MySqlParameter("@inpolicyexpiryfrom",frmdateex),
                          new MySqlParameter("@inpolicyexpiryTo",todateex),
                          new MySqlParameter("@InDealerID",dealerId),
                          new MySqlParameter("@InRenewalType", renewalType),
                          new MySqlParameter("disp", dispositionStatus ),
                          //new MySqlParameter("incampaign_id", Campaign),
                          new MySqlParameter("@incampaign_id", campaign != null ? string.Join(",", campaign) : ""),
                          new MySqlParameter("inpriorityid", Priority),
                          //new MySqlParameter("inmarketcampign", marketingCampaign),
                          new MySqlParameter("dealeridin", dealerCode),
                          new MySqlParameter("insegment", segments != null ? string.Join(",", segments) : ""),
                          new MySqlParameter("inmake", make != null ? string.Join(",", make) : ""),
                          new MySqlParameter("inmodel", model != null ? string.Join(",", model) : ""),
                          new MySqlParameter("inpremiumsize", premiumsize != null ? string.Join(",", premiumsize) : ""),
                          new MySqlParameter("inpolicytype", policyType != null ? string.Join(",", policyType) : ""),
                          new MySqlParameter("inyearOfSale", yearLists != null ? string.Join(",", yearLists) : ""),
                          new MySqlParameter("inQuoteStatus", quoteStatus != null ? string.Join(",", quoteStatus) : "")
                    };

                    changepolicyassignmentDetails = db.Database.SqlQuery<changeAssignmentINS>(str1, parameter1).ToList();
                    var JsonData = Json(new { data = changepolicyassignmentDetails, draw = Request["draw"], recordsTotal = changepolicyassignmentDetails.Count, recordsFiltered = changepolicyassignmentDetails.Count, totaldataavailable = changepolicyassignmentDetails.Count, exception = "" }, JsonRequestBehavior.AllowGet);
                    JsonData.MaxJsonLength = int.MaxValue;
                    return JsonData;
                    //return Json(new { success = true, message = "", totaldataavailable= totaldataavailable }, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                string exception = string.Empty;
                if (ex.Message.Contains("inner exception"))
                {
                    exception = ex.InnerException.Message;
                }
                else
                {
                    exception = ex.Message;
                }
                logger.Info("\n\n -------- Change Assignment Catch--------" + exception + "\n\n");
                return Json(new { success = false, data = "", draw = Request["draw"], recordsTotal = 0, recordsFiltered = 0, exception = exception }, JsonRequestBehavior.AllowGet);
                //return Json(new { success = false, message = exception, totaldataavailable = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult changeassignmentasign(string changeassignmentFilters)
        {
            List<resultsassignment> allAssignment = new List<resultsassignment>();

            try
            {

                int UserId = Convert.ToInt32(Session["UserId"].ToString());

                assignmentFilters asignFilters = JsonConvert.DeserializeObject<assignmentFilters>(changeassignmentFilters);

                using (var db = new AutoSherDBContext())
                {

                    //long dealerCode = Convert.ToInt64(Session["HeroDealerCode"].ToString());
                    string dealerCode = Session["HeroDealerCode"].ToString();
                    long dealerId = Convert.ToInt64(Session["HeroDealerId"].ToString());

                    if (!(string.IsNullOrEmpty(asignFilters.dealerCodes)))
                    {

                        dealerId = Convert.ToInt64(asignFilters.dealerCodes.Split('.')[0]);
                        //dealerCode = Convert.ToInt64(asignFilters.dealerCodes.Split('.')[1]);
                        dealerCode = asignFilters.dealerCodes.Split('.')[1];
                    }

                    List<string> creLists = asignFilters.cre_list == null || asignFilters.cre_list == "" ? null : asignFilters.cre_list.Split(',').Select(x => (x.Trim())).ToList();


                    string frmdateex = asignFilters.frmdate == null || asignFilters.frmdate == "" ? "" : Convert.ToDateTime(asignFilters.frmdate).ToString("yyyy-MM-dd");
                    string todateex = asignFilters.todate == null || asignFilters.todate == "" ? "" : Convert.ToDateTime(asignFilters.todate.ToString()).ToString("yyyy-MM-dd");
                    int renewalType = asignFilters.renewalType == null || asignFilters.renewalType == "" ? 0 : Convert.ToInt32(asignFilters.renewalType);
                    string dispositionStatus = asignFilters.dispositionStatus == null ? "" : asignFilters.dispositionStatus;
                    string Campaign = asignFilters.Campaign == null ? "" : asignFilters.Campaign;
                    string marketingCampaign = asignFilters.marketingCampaign == null ? "" : asignFilters.marketingCampaign;
                    string Priority = asignFilters.Priority == null ? "" : asignFilters.Priority;
                    string herodealerCode = asignFilters.dealerCodes == null ? "" : asignFilters.dealerCodes;
                    string segments = asignFilters.segment == null ? "" : asignFilters.segment;
                    string make = asignFilters.make == null ? "" : asignFilters.make;
                    string model = asignFilters.model == null ? "" : asignFilters.model;
                    string premiumsize = asignFilters.premium == null ? "" : asignFilters.premium;
                    string policytype = asignFilters.policyType == null ? "" : asignFilters.policyType;

                    if (db.TempTargetCrelists.Count(m => m.ManagerID == UserId) > 0)
                    {
                        db.Database.ExecuteSqlCommand("delete from temptargetcrelist  where ManagerID=@managerid and DealerID=@indealerid", new MySqlParameter("@managerid", UserId), new MySqlParameter("@indealerid", dealerId));
                    }

                    List<tempTargetCrelist> tempTargetCRE = new List<tempTargetCrelist>();
                    if (creLists != null)
                    {
                        foreach (string cres in creLists)
                        {
                            tempTargetCrelist tempTargetCreLists = new tempTargetCrelist();
                            tempTargetCreLists.ManagerID = UserId;
                            tempTargetCreLists.DealerCode = dealerCode;
                            tempTargetCreLists.DealerID = dealerId;
                            tempTargetCreLists.wyzuserId = Convert.ToInt64(cres);
                            tempTargetCRE.Add(tempTargetCreLists);
                        }
                        db.TempTargetCrelists.AddRange(tempTargetCRE);

                        db.SaveChanges();

                        string str = @"CALL HERO_ChangeAssignment(@inManagerID,@InPolicyExpiryFromDate,@InPolicyExpiryToDate,@InDealerID,@InrenewalType,@disp,@incampaign_id,@inpriorityid,@dealeridin,@segment,@make,@model,@premiumsize,@policytype)";

                        MySqlParameter[] parameter = new MySqlParameter[] {
                        new MySqlParameter("inManagerID", UserId),
                        new MySqlParameter("InPolicyExpiryFromDate", frmdateex),
                        new MySqlParameter("InPolicyExpiryToDate", todateex),
                        new MySqlParameter("InDealerID", dealerId),
                        new MySqlParameter("InRenewalType", renewalType),
                        new MySqlParameter("disp", dispositionStatus),
                        new MySqlParameter("incampaign_id", Campaign),
                        new MySqlParameter("inpriorityid", Priority),
                        //new MySqlParameter("inmarketcampign", marketingCampaign),
                        new MySqlParameter("dealeridin", herodealerCode),
                        new MySqlParameter("segment", segments),
                            new MySqlParameter("make", make),
                        new MySqlParameter("model", model),
                        new MySqlParameter("premiumsize", premiumsize),
                        new MySqlParameter("policytype", policytype),

                        };

                        //allAssignment = db.Database.SqlQuery<resultsassignment>(str, parameter).ToList();
                        db.Database.ExecuteSqlCommandAsync(str, parameter);
                        return Json(new { success = true, allAssignment }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { success = false, message = "No Cres To Assign Data" }, JsonRequestBehavior.AllowGet);

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
                return Json(new { success = false, message = exception }, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region Data Assignement Summary
        public ActionResult assignmentSummary()
        {
            return View();
        }
        public ActionResult viewAssignmentSummary(string assignmentFilters)
        {
            string Query = string.Empty;
            string role = Session["UserRole"].ToString();
            int UserId = Convert.ToInt32(Session["UserId"].ToString());

            if (role == "CREManager")
            {
                Query = "select (select campaignName from campaign where id=ca.campaign_id) as 'campaign Name',(select userName from wyzuser where id=ca.last_wyzuserId) as 'Last User' ,(select userName from wyzuser where id=ca.new_wyzuserId) as 'New User',(select userName from wyzuser where id=ca.assigned_manager_id) as 'Assigned By' ,(select DealerName from herodealer where DealerID=ca.herodealer_ID) as 'Dealer' ,updatedDate from change_assignment_records as ca where updatedDate > NOW()- interval 3 day and assigned_manager_id=" + UserId + " order by id desc;";
            }
            else if(role=="Admin")
            {
                //Query= "select DealerName, totaldata, AssignedData, AlreadyAssignedData,`2` as 'NIC', `3` as 'IL',`4` as 'TAGIC',`6` as 'Bharti AXA',UplodedDate from hero_assignmentsummary where UplodedDate  > NOW()- interval 3 day order by id desc";
                //Query = "select DealerName, totaldata, AssignedData, (totaldata-AssignedData) as NotAssignedData,now() as AssignedDateTime from hero_assignmentsummary where UplodedDate  > NOW()- interval 3 day order by id desc";
                return Json(new { data = new object[] { }, success = true }, JsonRequestBehavior.AllowGet);
            }
            assignmentFilters asignFilters = JsonConvert.DeserializeObject<assignmentFilters>(assignmentFilters);
            try
            {
                using (var db = new AutoSherDBContext())
                {

                    string frmdateex = asignFilters.frmdate == null || asignFilters.frmdate == "" ? "" : Convert.ToDateTime(asignFilters.frmdate).ToString("yyyy-MM-dd");
                    string todateex = asignFilters.todate == null || asignFilters.todate == "" ? "" : Convert.ToDateTime(asignFilters.todate.ToString()).ToString("yyyy-MM-dd");

                    DataTable summaryData = new DataTable();
                    string connectionString = ConfigurationManager.ConnectionStrings["AutoSherContext"].ConnectionString;
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();
                        using (MySqlTransaction tran = connection.BeginTransaction())
                        {
                            using (MySqlCommand cmd = new MySqlCommand())
                            {
                                cmd.Connection = connection;
                                cmd.Transaction = tran;
                                cmd.CommandText = Query;

                                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                                {
                                    using (MySqlCommandBuilder cb = new MySqlCommandBuilder(adapter))
                                    {
                                        adapter.Fill(summaryData);
                                    }
                                };
                            }
                        }
                    }
                    var results = JsonConvert.SerializeObject(summaryData, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    var result = Json(new { data = results, success = true }, JsonRequestBehavior.AllowGet);
                    result.MaxJsonLength = int.MaxValue;
                    return result;
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
                return Json(new { success = false, message = exception }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult downloadAssignmentReport(string fromDate, string toDate, string status, string expFromDate, string expToDate, string expStatus)
        {
            DataTable summaryData = new DataTable();
            fromDate = Convert.ToDateTime(fromDate).ToString("yyyy/MM/dd");
            toDate = Convert.ToDateTime(toDate).ToString("yyyy/MM/dd");
            long totalCount = 0;
            string Query = string.Empty;
            string QueryCount = string.Empty;
            string role = Session["UserRole"].ToString();
            int UserId = Convert.ToInt32(Session["UserId"].ToString());

            if (role == "CREManager")
            {
                QueryCount = "SELECT COUNT(*) FROM change_assignment_records AS ca WHERE CAST(updatedDate AS DATE) BETWEEN @fromDate AND @toDate";
                Query = "SELECT (SELECT campaignName FROM campaign WHERE id = ca.campaign_id) AS 'Campaign Name', " +
                        "(SELECT userName FROM wyzuser WHERE id = ca.last_wyzuserId) AS 'Last User', " +
                        "(SELECT userName FROM wyzuser WHERE id = ca.new_wyzuserId) AS 'New User', " +
                        "(SELECT userName FROM wyzuser WHERE id = ca.assigned_manager_id) AS 'Assigned By', " +
                        "(SELECT DealerName FROM herodealer WHERE DealerID = ca.herodealer_ID) AS 'Dealer', " +
                        "DATE_FORMAT(ca.updatedDate, '%d-%m-%Y') AS 'Updated Date' " +
                        "FROM change_assignment_records AS ca " +
                        "WHERE CAST(ca.updatedDate AS DATE) BETWEEN @fromDate AND @toDate AND ca.assigned_manager_id = @userId " +
                        "ORDER BY ca.id DESC";
            }
            else if (role == "Admin")
            {
                if (status == "No")
                {
                    QueryCount = "SELECT COUNT(*) FROM hero_assignmentsummary WHERE CAST(UplodedDate AS DATE) BETWEEN @fromDate AND @toDate";
                    Query = "SELECT DealerName, totaldata, AssignedData, AlreadyAssignedData, " +
                            "DATE_FORMAT(UplodedDate, '%d/%m/%Y') AS 'UploadedDate', AssignedBy, uploadId, notassignedData,Notassignedreason " +
                            "FROM hero_assignmentsummary WHERE CAST(UplodedDate AS DATE) BETWEEN @fromDate AND @toDate ORDER BY id DESC";
                }
                else if (expStatus == "No")
                {
                    QueryCount = "SELECT COUNT(*) FROM hero_assignmentsummary WHERE CAST(policyexpirydate AS DATE) BETWEEN @expFromDate AND @expToDate";
                    Query = "SELECT DealerName, totaldata, AssignedData, AlreadyAssignedData, " +
                            "DATE_FORMAT(policyexpirydate, '%d/%m/%Y') AS 'policyexpirydate', AssignedBy, uploadId, notassignedData,Notassignedreason " +
                            "FROM hero_assignmentsummary WHERE CAST(policyexpirydate AS DATE) BETWEEN @expFromDate AND @expToDate ORDER BY id DESC";
                }
                else if (status == "Yes")
                {
                    QueryCount = "SELECT COUNT(*) FROM hero_assignmentsummary WHERE CAST(UplodedDate AS DATE) BETWEEN @fromDate AND @toDate";
                    Query = "SELECT DealerName, totaldata, AssignedData, AlreadyAssignedData, " +
                            "DATE_FORMAT(UplodedDate, '%d/%m/%Y') AS 'UploadedDate' " +
                            "FROM hero_assignmentsummary WHERE CAST(UplodedDate AS DATE) BETWEEN @fromDate AND @toDate ORDER BY id DESC";
                }
                else if (expStatus == "Yes")
                {
                    QueryCount = "SELECT COUNT(*) FROM hero_assignmentsummary WHERE CAST(policyexpirydate AS DATE) BETWEEN @expFromDate AND @expToDate";
                    Query = "SELECT DealerName, totaldata, AssignedData, AlreadyAssignedData, " +
                            "DATE_FORMAT(policyexpirydate, '%d/%m/%Y') AS 'policyexpirydate' " +
                            "FROM hero_assignmentsummary WHERE CAST(policyexpirydate AS DATE) BETWEEN @expFromDate AND @expToDate ORDER BY id DESC";
                }
            }

            try
            {
                if (!(string.IsNullOrEmpty(QueryCount) && string.IsNullOrEmpty(Query)))
                {
                    using (AutoSherDBContext dBContext = new AutoSherDBContext())
                    {
                        totalCount = dBContext.Database.SqlQuery<int>(QueryCount,
                            new MySqlParameter("@fromDate", fromDate),
                        new MySqlParameter("@toDate", toDate),
                             new MySqlParameter("@expFromDate", expFromDate),
                        new MySqlParameter("@expToDate", expToDate)).FirstOrDefault();

                    }

                    for (long i = 0; i < totalCount; i += 500)
                    {
                        string connectionString = ConfigurationManager.ConnectionStrings["AutoSherContext"].ConnectionString;
                        using (MySqlConnection connection = new MySqlConnection(connectionString))
                        {
                            connection.Open();
                            using (MySqlTransaction tran = connection.BeginTransaction())
                            {
                                using (MySqlCommand cmd = new MySqlCommand())
                                {
                                    cmd.Connection = connection;
                                    cmd.Transaction = tran;
                                    cmd.CommandText = Query + " LIMIT @start, 500";

                                    // Add parameters dynamically
                                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                                    cmd.Parameters.AddWithValue("@toDate", toDate);
                                    cmd.Parameters.AddWithValue("@userId", UserId);
                                    cmd.Parameters.AddWithValue("@start", i);

                                    // If expStatus is "Yes" or "No", add the expected parameters
                                    if (expStatus == "No" || expStatus == "Yes")
                                    {
                                        cmd.Parameters.AddWithValue("@expFromDate", expFromDate);
                                        cmd.Parameters.AddWithValue("@expToDate", expToDate);
                                    }

                                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                                    {
                                        using (MySqlCommandBuilder cb = new MySqlCommandBuilder(adapter))
                                        {
                                            adapter.Fill(summaryData);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (summaryData.Rows.Count <= 0)
                    {
                        return Json(new { success = false, error = "No Upload Summary Found." }, JsonRequestBehavior.AllowGet);
                    }

                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode("Allocation_Summary", System.Text.Encoding.ASCII));
                    using (ExcelPackage pck = new ExcelPackage())
                    {
                        ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Allocation_Summary");
                        ws.Cells["A1"].LoadFromDataTable(summaryData, true);
                        Session["DownloadExcel_FileManager"] = pck.GetAsByteArray();
                    }
                }

                var results = JsonConvert.SerializeObject(summaryData, Formatting.Indented,
                    new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                var result = Json(new { data = results, success = true }, JsonRequestBehavior.AllowGet);
                result.MaxJsonLength = int.MaxValue;
                return result;
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = "An error occurred: " + ex.Message });
            }
        }


        public ActionResult DownloadRateCardData(string policyType)
        {
            var policyassignmentDetails = HttpContext.Session["PolicyAssignmentDetails"] as List<policyasgbmnt>;
            if (policyassignmentDetails == null)
            {
                return Json(new { success = false, message = "No data available for download." }, JsonRequestBehavior.AllowGet);
            }

            string fileName = "SummaryData.xlsx";  // Default if no valid policyType
            if (!string.IsNullOrEmpty(policyType))
            {
                fileName = $"{policyType}.xlsx";
            }

            using (var package = new ExcelPackage())
            {
                //var worksheet = package.Workbook.Worksheets.Add("RateCardData");
                var worksheet = package.Workbook.Worksheets.Add(fileName);

                // Add header
                worksheet.Cells[1, 1].Value = "OEM";
                worksheet.Cells[1, 2].Value = "Dealer";
                worksheet.Cells[1, 3].Value = "Location";
                worksheet.Cells[1, 4].Value = "Producer Code";
                worksheet.Cells[1, 5].Value = "Renewal Base Count";
                worksheet.Cells[1, 6].Value = "Number of Logins";
                worksheet.Cells[1, 7].Value = "Premium Amount";

                // Add data
                for (int i = 0; i < policyassignmentDetails.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = policyassignmentDetails[i].oem;
                    worksheet.Cells[i + 2, 2].Value = policyassignmentDetails[i].dealer;
                    worksheet.Cells[i + 2, 3].Value = policyassignmentDetails[i].location;
                    worksheet.Cells[i + 2, 4].Value = policyassignmentDetails[i].producerCode;
                    worksheet.Cells[i + 2, 5].Value = policyassignmentDetails[i].renewalBaseCount;
                    worksheet.Cells[i + 2, 6].Value = policyassignmentDetails[i].noOfLogins;
                    worksheet.Cells[i + 2, 7].Value = policyassignmentDetails[i].premiumAmount;
                }

                // Format the response
                var stream = new MemoryStream();
                package.SaveAs(stream);
                var content = stream.ToArray();
                //return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "RateCardData.xlsx");
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        public ActionResult DownloadALL()
        {
            Logger logger = LogManager.GetLogger("apkRegLogger");

            try
            {

                if (Session["DownloadExcel_FileManager"] != null)
                {
                    logger.Info("Forecast Download Started");

                    byte[] data = Session["DownloadExcel_FileManager"] as byte[];
                    Session["DownloadExcel_FileManager"] = null;
                    logger.Info("Forecast Download Ended");

                    return File(data, "application/octet-stream", "Allocation_Summary" + DateTime.Now.ToShortDateString() + DateTime.Now.TimeOfDay + ".xlsx");
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


        public JsonResult getBrandByModel(int brand)
        {
            using (var db = new AutoSherDBContext())
            {
                var modelName = db.models.Where(x => x.brandId == brand).Select(m => new { id = m.id, model = m.modelnames }).ToList();
                return Json(new { modelName, JsonRequestBehavior.AllowGet });
            }
        }

        public ActionResult GetOemsByDealerAndBrandId(string oemid)
        {
            int oemID = 0;
            if (!string.IsNullOrEmpty(oemid))
            {
                oemID = int.Parse(oemid);
            }
            using (var db = new AutoSherDBContext())
            {
                var dealernames = db.oem_dealer
                             .Where(o => o.OEM_Id == oemID)
                             .Join(db.Herodealers,
                                   o => o.dealerID,
                                   h => h.DealerID,
                                   (o, h) => new { h.DealerID, h.DealerName, h.DealerCode })
                             .ToList();
                int? suboemfkid = db.brands.Where(m => m.id == oemID).Select(m => m.fksuboem_id).FirstOrDefault();
                var oemiclist = db.OEMInsuranceCompany.Where(m => m.fksuboem_id == suboemfkid).ToList();

                //return Json(new { dealerNames = dealernames, oemiclist = oemiclist }, JsonRequestBehavior.AllowGet);
                var result = new { dealerNames = dealernames, oemiclist = oemiclist };

                return new JsonResult
                {
                    Data = result,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    MaxJsonLength = int.MaxValue
                };
            }
        }

        #endregion
    }

    public class policyasgbmnt
    {
        public string frameno { get; set; }
        public string firstname { get; set; }
        public string previousinsurername { get; set; }
        public string previouspolicyexpiry { get; set; }
        public string previouspolicytype { get; set; }
        public string cpname { get; set; }
        public string premiumAmount { get; set; }
        public string noOfLogins { get; set; }
        public string renewalBaseCount { get; set; }
        public string producerCode { get; set; }
        public string location { get; set; }
        public string dealer { get; set; }
        public string oem { get; set; }
        public string icnames { get; set; }
    }
    class changeAssignmentINS
    {
        public string CustomerName { get; set; }
        public string creName { get; set; }
        public string Count { get; set; }
        public string FrameNo { get; set; }
        public string policyDueDate { get; set; }
        public string YearOfSale { get; set; }
        public string insuranceCompanyName { get; set; }
        public string AssignedCRE { get; set; }
    }
    public class assignmentFilters
    {
        public string icnames { get; set; }
        public string dealerCodes { get; set; }
        public string renewalType { get; set; }
        public string cre_list { get; set; }
        public string yearofsale { get; set; }
        public string frmdate { get; set; }
        public string todate { get; set; }
        public string procedureName { get; set; }
        public string marketingCampaign { get; set; }
        public string Campaign { get; set; }
        public string Priority { get; set; }
        public string dispositionStatus { get; set; }
        public string segment { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public string premium { get; set; }
        public string policyType { get; set; }
        public string oem { get; set; }
        public string quoteStatus { get; set; }
    }
    public class resultsassignment
    {
        public string DealerName { get; set; }
        public string AssignedData { get; set; }
    }
}