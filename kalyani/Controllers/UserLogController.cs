using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Firebase.Database;
using Firebase.Database.Query;
using System.Threading.Tasks;
using AutoSherpa_project.Models;
using AutoSherpa_project.Models.Schedulers;
using System.IO;
using Newtonsoft.Json;
using System.Data.Entity.Migrations;
using Quartz;
using Quartz.Impl;
using NLog;

namespace AutoSherpa_project.Controllers
{
    public class UserLogController : Controller
    {

        //public List<firebaseData> fireDataList = new List<firebaseData>();
        public static bool synchStop = false;
        public static List<long> vehicleIdList = new List<long>();
        public static string curUser { get; set; }
        [HttpGet]
        [ActionName("ViewLogs")]
        public ActionResult ViewLogs(int id)
        {
            try
            {
                if (id != 420)
                {
                    return RedirectToAction("LogOff", "Home");
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public ActionResult loadUserData(string from, string to, string filterOn)
        {
            List<userlogs> users = new List<userlogs>();
            int start = Convert.ToInt32(Request["start"]);
            int toIndex = Convert.ToInt32(Request["length"]), totalCount = 0;
            DateTime fromDate, toDate;
            string exception = "";
            try
            {
                using (var db = new AutoSherDBContext())
                {

                    totalCount = db.userlogs.Count();

                    if (toIndex < 0)
                    {
                        toIndex = 10;
                    }

                    if (toIndex > totalCount)
                    {
                        toIndex = totalCount;
                    }

                    if (filterOn != "All" && filterOn != "inspect")
                    {
                        fromDate = Convert.ToDateTime(from);
                        toDate = Convert.ToDateTime(to);

                        users = db.userlogs.Where(m => m.loginDateTime >= fromDate && m.loginDateTime <= toDate).OrderBy(m => m.id).Skip(start).Take(toIndex).ToList();
                    }
                    else if (filterOn == "inspect")
                    {
                        users = db.userlogs.Where(m => m.managername.Contains("Inspector")).OrderBy(m => m.id).Skip(start).Take(toIndex).ToList();
                    }
                    else
                    {
                        users = db.userlogs.OrderBy(m => m.id).Skip(start).Take(toIndex).ToList();
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

            return Json(new { data = users, draw = Request["draw"], recordsTotal = totalCount, recordsFiltered = totalCount, exception = exception }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult renameAndroFiles()
        {
            List<string> fileRenames = new List<string>();
            try
            {
                //DirectoryInfo d = new DirectoryInfo(@"C:\DirectoryToAccess");
                //FileInfo[] infos = d.GetFiles();
                //foreach (FileInfo f in infos)
                //{
                //    File.Move(f.FullName, f.FullName.Replace("abc_",""));
                //}
                string DirectoryPaths = Server.MapPath(@"~/wyzAudioData/INDUS/");
                DirectoryInfo d = new DirectoryInfo(DirectoryPaths);
                foreach (var file in d.GetFiles())
                {
                    string newFile = (file.FullName.Replace('+', '_')).Replace('-', '_');
                    Directory.Move(file.FullName, newFile);
                    fileRenames.Add(newFile);
                    //i++;
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }

            return Json(new { success = true, filesRenamed = string.Join(",", fileRenames) });
        }

        public ActionResult deleteBody(int length)
        {
            List<string> fileRemoved = new List<string>();
            long rmFileSize = 0;
            try
            {
                string DealerCode = "";
                using (var db = new AutoSherDBContext())
                {
                    DealerCode = db.dealers.FirstOrDefault().dealerCode;
                }


                string DirectoryPaths = Server.MapPath(@"~/wyzAudioData/" + DealerCode + "/");
                //var filesList = Path.GetFileNameWithoutExtension(DirectoryPaths);

                //foreach(var file in filesList)
                //{
                //    //new File().Delete(file.)
                //}

                DirectoryInfo d = new DirectoryInfo(DirectoryPaths);
                FileInfo[] infos = d.GetFiles();

                foreach (FileInfo f in infos)
                {
                    if (f.Extension == "")
                    {
                        rmFileSize = rmFileSize + f.Length;
                        fileRemoved.Add(f.FullName);
                        System.IO.File.Delete(f.FullName);
                    }
                }

            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }

            return Json(new { success = true, filesremoved = string.Join(",", fileRemoved), rmCount = fileRemoved.Count(), rmFileSize = rmFileSize });
        }

        public ActionResult getAuthenticate(string pass)
        {
            if (pass == "mnj@123$")
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        public ActionResult getDetails(string id)
        {
            string regNo = string.Empty;
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    long? vehicle_id = db.callinteractions.FirstOrDefault(m => m.uniqueIdGSM == id).vehicle_vehicle_id;

                    if (vehicle_id != 0)
                    {
                        regNo = db.vehicles.FirstOrDefault(m => m.vehicle_id == vehicle_id).chassisNo;

                        var datas = db.callhistorycubes.Where(m => m.vehicle_id == vehicle_id).Select(m => new { callDate = m.callDate, filepath = m.filepath, creName = m.Cre_Name }).ToList();
                        string data = JsonConvert.SerializeObject(datas);
                        return Json(new { success = true, data = data, regNo = regNo });
                    }
                    else
                    {
                        return Json(new { success = true, error = "UniqueId Doesn't matched......." });
                    }

                }
            }
            catch (Exception ex)
            {
                return Json(new { success = true, error = ex.Message });
            }
        }

        //public ActionResult captureInspectors(string sessionUser, string inspectedPage)
        //{
        //    try
        //    {
        //        using (var db = new AutoSherDBContext())
        //        {
        //            userlogs inspUser = new userlogs();

        //            inspUser.username = sessionUser;
        //            inspUser.managername = "Inspector - " + inspectedPage;
        //            inspUser.loginDateTime = DateTime.Now;
        //            inspUser.sessionid = "Inspected on the Page:" + inspectedPage;
        //            inspUser.hostaddress = Request.UserHostAddress;
        //            db.userlogs.Add(inspUser);
        //            db.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string exception = "";
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
        //        return Json(new { success = exception });
        //    }
        //    return Json(new { success = true });
        //}



        public ActionResult runTransJob(string jobName)
        {
            try
            {
                if (jobName == "START")
                {
                    IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;

                    scheduler.Start();

                    IJobDetail jobDetail = JobBuilder.Create<sqltoMysqlDataTransferJOB>().Build();

                    ITrigger trigger = TriggerBuilder.Create().WithIdentity("insertMysqlTrigger", "insertMysqlTriggerGroup")
                        .StartNow().WithSimpleSchedule().Build();

                    scheduler.ScheduleJob(jobDetail, trigger);
                }
                else if(jobName=="STOP")
                {

                }
                return Json(new { success = true });

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

    }
}