using AutoSherpa_project.Models;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutoSherpa_project.Controllers
{
    [AuthorizeFilter]
    public class liveReportsController : Controller
    {
        // GET: liveReports
        public ActionResult liveReports()
        {
            try
            {
                string userName = Session["UserName"].ToString();
                string role = Session["UserRole"].ToString();
                int userId = Convert.ToInt32(Session["UserId"]);

                using (var db = new AutoSherDBContext())
                {
                    var campaignList = db.campaigns.Select(m => new { id = m.id, campaignName = m.campaignName }).ToList();
                    var renewalList = db.renewaltypes.Select(m => new { id = m.id, renewalTypeName = m.renewalTypeName }).ToList();
                    var dealerList = db.Herodealers.Where(m=>m.IsActive==true).Select(m => new {id=m.DealerID, DealerCity = m.DealerCity, DealerState = m.DealerState, DealerZone = m.DealerZone , DealerName=m.DealerName}).ToList();
                    
                    ViewBag.ddlcampaignList = campaignList.OrderBy(m => m.campaignName).ToList();
                    ViewBag.renewalList = renewalList.OrderBy(m => m.renewalTypeName).Distinct().ToList();


                    //ViewBag.DealerCity = db.Herodealers.Select(m => new { DealerCity = m.DealerCity }).Distinct().ToList();
                    //ViewBag.DealerState = db.Herodealers.Select(m => new { DealerState = m.DealerState }).Distinct().ToList();
                    //ViewBag.DealerZone = db.Herodealers.Select(m => new { DealerZone = m.DealerZone }).Distinct().ToList();
                    //ViewBag.DealerName = db.Herodealers.Select(m => new { DealerName = m.DealerName }).Distinct().ToList();

                    ////ViewBag.DealerCity = dealerList.Select(m =>new { DealerCity= m.DealerCity } ).Distinct().ToList();
                    ////ViewBag.DealerState = dealerList.Select(m => new { DealerState = m.DealerState }).Distinct().ToList();
                    ViewBag.DealerZone = dealerList.Select(m => new { DealerZone = m.DealerZone }).Distinct().ToList();
                    ////ViewBag.DealerName = dealerList.Select(m => new { DealerName = m.DealerName }).Distinct().ToList();
                    ViewBag.dealerclassificationData = JsonConvert.SerializeObject(dealerList);


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
                TempData["ControllerName"] = "InsuranceLiveForm";

                return RedirectToAction("LogOff", "Home");
            }
            return View();
        }

        public ActionResult getLiveReports(string reportParameter)
        {
            string exception = "";
            DataTable INSReports = new DataTable();

            try
            {
                using (var db = new AutoSherDBContext())
                {

                    string role = Session["UserRole"].ToString();
                    long userId = Convert.ToInt64(Session["UserId"]);

                    livereportFilter filter = new livereportFilter();
                    if (reportParameter != null)
                    {
                        filter = JsonConvert.DeserializeObject<livereportFilter>(reportParameter);
                    }

                    string CREName = string.Empty;
                    string manageruserName = string.Empty;
                    string zonemanageruserName = string.Empty;
                    string statemanageruserName = string.Empty;
                    string citymanageruserName = string.Empty;
                    string dealermanageruserName = string.Empty;
                    string chanelManager = string.Empty;
                    string AreaManager = string.Empty;
                    string RM = string.Empty;


                    string DealerCity = filter.DealerCity == null ? "" : filter.DealerCity;
                    string DealerName = filter.DealerName == null ? "" : filter.DealerName;
                    string campaignName = filter.campaignName == null ? "" : filter.campaignName;
                    string DealerZone = filter.DealerZone == null ? "" : filter.DealerZone;
                    string DealerState = filter.DealerState == null ? "" : filter.DealerState;
                    string renewalTypeName = filter.renewalTypeName == null ? "" : filter.renewalTypeName;
                    int reportId = Convert.ToInt32(filter.reportId);
                  
                    if (role == "CRE")
                    {
                        CREName = userId.ToString();
                        manageruserName = "";
                        zonemanageruserName = "";
                        statemanageruserName = "";
                        citymanageruserName = "";
                        dealermanageruserName = "";
                        chanelManager = "";
                        AreaManager = "";
                        RM = "";
                    }
                    else if (role == "Admin")
                    {
                        CREName = "";
                        manageruserName = "";
                        zonemanageruserName = "";
                        statemanageruserName = "";
                        citymanageruserName = "";
                        dealermanageruserName = "";
                        chanelManager = "";
                        AreaManager = "";
                        RM = "";
                    }
                    else if (role == "CREManager")
                    {
                        CREName = "";
                        manageruserName = Session["UserName"].ToString();
                        zonemanageruserName = "";
                        statemanageruserName = "";
                        citymanageruserName = "";
                        dealermanageruserName = "";
                        chanelManager = "";
                        AreaManager = "";
                        RM = "";
                    }
                    else if (role == "ZoneManager")
                    {
                        CREName = "";
                        manageruserName = "";
                        zonemanageruserName = Session["UserName"].ToString();
                        statemanageruserName = "";
                        citymanageruserName = "";
                        dealermanageruserName = "";
                        chanelManager = "";
                        AreaManager = "";
                        RM = "";
                    }
                    else if (role == "StateManager")
                    {
                        CREName = "";
                        manageruserName = "";
                        zonemanageruserName = "";
                        statemanageruserName = Session["UserName"].ToString();
                        citymanageruserName = "";
                        dealermanageruserName = "";
                        chanelManager = "";
                        AreaManager = "";
                        RM = "";
                    }
                    else if (role == "CityManager")
                    {
                        CREName = "";
                        manageruserName = "";
                        zonemanageruserName = "";
                        statemanageruserName = "";
                        citymanageruserName = Session["UserName"].ToString();
                        dealermanageruserName = "";
                        chanelManager = "";
                        AreaManager = "";
                        RM = "";
                    }
                    else if (role == "DealerManager")
                    {
                        CREName = "";
                        manageruserName = "";
                        zonemanageruserName = "";
                        statemanageruserName = "";
                        citymanageruserName = "";
                        dealermanageruserName = Session["UserName"].ToString();
                        chanelManager = "";
                        AreaManager = "";
                        RM = "";
                    }
                    else if (role == "ChannelManager")
                    {
                        CREName = "";
                        manageruserName = "";
                        zonemanageruserName = "";
                        statemanageruserName = "";
                        citymanageruserName = "";
                        dealermanageruserName = "";
                        chanelManager = Session["UserName"].ToString();
                        AreaManager = "";
                        RM = "";
                    }
                    else if (role == "AreaManager")
                    {
                        CREName = "";
                        manageruserName = "";
                        zonemanageruserName = "";
                        statemanageruserName = "";
                        citymanageruserName = "";
                        dealermanageruserName = "";
                        chanelManager = "";
                        AreaManager = Session["UserName"].ToString();
                        RM = "";
                    }
                    else if (role == "RM")
                    {
                        CREName = "";
                        manageruserName = "";
                        zonemanageruserName = "";
                        statemanageruserName = "";
                        citymanageruserName = "";
                        dealermanageruserName = "";
                        chanelManager = "";
                        AreaManager = "";
                        RM = Session["UserName"].ToString();
                    }

                    string conStr = ConfigurationManager.ConnectionStrings["AutoSherContext"].ConnectionString;
                    using (MySqlConnection connection = new MySqlConnection(conStr))
                    {
                        using (MySqlCommand cmd = new MySqlCommand("livereport_hero_ins", connection))
                        {
                            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                            adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                            adapter.SelectCommand.Parameters.Add(new MySqlParameter("reportid", reportId));
                            adapter.SelectCommand.Parameters.Add(new MySqlParameter("incampaign", campaignName));
                            adapter.SelectCommand.Parameters.Add(new MySqlParameter("inpolicydue", renewalTypeName));
                            adapter.SelectCommand.Parameters.Add(new MySqlParameter("inzone", DealerZone));
                            adapter.SelectCommand.Parameters.Add(new MySqlParameter("indealername", DealerName));
                            adapter.SelectCommand.Parameters.Add(new MySqlParameter("incity", DealerCity));
                            adapter.SelectCommand.Parameters.Add(new MySqlParameter("instate", DealerState));
                            adapter.SelectCommand.Parameters.Add(new MySqlParameter("incremanager", manageruserName));
                            adapter.SelectCommand.Parameters.Add(new MySqlParameter("increid", CREName));
                            adapter.SelectCommand.Parameters.Add(new MySqlParameter("inzonemanager", zonemanageruserName));
                            adapter.SelectCommand.Parameters.Add(new MySqlParameter("instatemanager", statemanageruserName));
                            adapter.SelectCommand.Parameters.Add(new MySqlParameter("inchanelManager", chanelManager));
                            adapter.SelectCommand.Parameters.Add(new MySqlParameter("inAreaManager", AreaManager));
                            adapter.SelectCommand.Parameters.Add(new MySqlParameter("inRM", RM));
                            adapter.Fill(INSReports);
                        }
                    }

                    var results = JsonConvert.SerializeObject(INSReports, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    var result = Json(new { data = results, exception = exception }, JsonRequestBehavior.AllowGet);
                    result.MaxJsonLength = int.MaxValue;
                    return result;
                }
            }
            catch (Exception ex)
            {
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
                var results = JsonConvert.SerializeObject(INSReports, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                return Json(new { data = results, exception = exception }, JsonRequestBehavior.AllowGet);
            }
        }
    }
    public class livereportFilter
    {
        public long reportId { get; set; }
        public string DealerCity { get; set; }
        public string DealerState { get; set; }
        public string DealerZone { get; set; }
        public string DealerName { get; set; }
        public string campaignName { get; set; }
        public string renewalTypeName { get; set; }
    }
}