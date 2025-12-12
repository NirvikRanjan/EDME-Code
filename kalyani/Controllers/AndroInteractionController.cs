using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AutoSherpa_project.Models;
using AutoSherpa_project.Models.Schedulers;
using Newtonsoft.Json.Linq;
using NLog;
using Newtonsoft.Json;
using AutoSherpa_project.Models.API_Model;
using System.Text.RegularExpressions;
using System.Collections.Specialized;


namespace AutoSherpa_project.Controllers
{
    public class AndroInteractionController : ApiController
    {
        // GET: api/AndroInteraction
        [HttpGet, Route("api/AndroInteraction")]
        public IHttpActionResult Get()
        {
            dynamic data = new JObject();
            string JsonString = string.Empty;
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    var wyzAppData = db.tenants.FirstOrDefault(m => m.isDeactivated == true);

                    data.latestAppVersion = wyzAppData.appVersion;
                    data.urlLink = wyzAppData.apkURLPath;

                    //JsonString = JsonConvert.SerializeObject(data);
                    //JsonString = data;
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


                //dynamic data = new JObject();
                data.Exception = exception;
                //JsonString = JsonConvert.SerializeObject(data); 
            }
            return Ok<JObject>(data);
        }

        public IHttpActionResult Get(string id)
        {
            dynamic data = new JObject();
            string JsonString = string.Empty;
            try
            {
                using (var db = new AutoSherDBContext())
                {


                    var wyzAppData = db.tenants.FirstOrDefault(m => m.isDeactivated == true);

                    if (id.ToLower() == "fe")
                    {
                        data.latestAppVersion = wyzAppData.feAppVersion;
                        data.urlLink = wyzAppData.apkURLPath;
                    }
                    else if (id.ToLower() == "driver")
                    {
                        data.latestAppVersion = wyzAppData.driverAppVersion;
                        data.urlLink = wyzAppData.apkURLPath;
                    }
                    else
                    {
                        data.Exception = "Wrong Parameter";
                    }
                    //JsonString = JsonConvert.SerializeObject(data);
                    //JsonString = data;
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


                //dynamic data = new JObject();
                data.Exception = exception;
                //JsonString = JsonConvert.SerializeObject(data); 
            }
            return Ok<JObject>(data);
        }

        // GET: api/AndroInteraction/5
        //public string Get(string id)
        //{
        //    return "value";
        //}

        // POST: api/AndroInteraction
       [HttpPost, Route("api/AndroInteraction")]
        public IHttpActionResult Post([FromBody] AppLogCheck app)
        {
            dynamic data = new JObject();
            Logger logger = LogManager.GetLogger("apkRegLogger");
            try
            {
                //AppLogCheck app = new AppLogCheck();


                logger.Info("Request Body:\n" + JsonConvert.SerializeObject(app));

                using (var db = new AutoSherDBContext())
                {
                    string phNum = string.Empty;
                    string phIM = string.Empty;
                    if(!(string.IsNullOrEmpty(app.phoneNumber)))
                    {
                        phNum = app.phoneNumber.Trim();
                    }
                    if(!(string.IsNullOrEmpty(app.phoneIMEINo)))
                    {
                        phIM = app.phoneIMEINo.Trim();
                    }
                    
                    wyzuser user = new wyzuser();
                    if (db.wyzusers.Where(m => m.phoneNumber.Trim() == phNum && m.phoneIMEINo.Trim() == phIM).Count() > 1)
                    {
                        data.Exception = "Duplicate Phone Number";
                        return Ok<JObject>(data);
                    }
                    else
                    {
                        user = db.wyzusers.FirstOrDefault(m => m.phoneNumber.Trim() == phNum);
                        //user = db.wyzusers.FirstOrDefault(m => m.phoneNumber.Trim() == phNum && (m.phoneIMEINo.Trim() == phIM || m.phoneIMEINo1.Trim() == phIM));
                    }
                    if (user != null && !string.IsNullOrEmpty(app.registrationId))
                    {
                        user.registrationId = app.registrationId;
                        db.wyzusers.AddOrUpdate(user);
                        db.SaveChanges();

                        data.authenticationStatus = true;
                        data.userId = user.userName;
                        data.userRole = user.role;
                        data.userFName = user.firstName;
                        data.userLName = user.lastName == null ? "0" : user.lastName;
                        data.dealerId = user.dealerCode;
                        data.dealerName = user.dealerName;
                        data.userEmail = null;
                        data.jwtToken = app.registrationId;

                        string baseURL = string.Empty;
                        if (HomeController.autosherpa1.Contains(user.dealerCode))
                        {
                            baseURL = System.Configuration.ConfigurationManager.AppSettings["FireBase_autosherpa1"];
                        }
                        data.fireBaseUrl = baseURL;
                    }
                    else
                    {
                        data.authenticationStatus = false;
                        data.userId = null;
                        data.userRole = null;
                        data.userFName = null;
                        data.userLName = null;
                        data.dealerId = null;
                        data.dealerName = null;
                        data.userEmail = null;
                        data.jwtToken = null;
                        data.fireBaseUrl = null;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        data.Exception = ex.InnerException.InnerException.Message;
                    }
                    else
                    {
                        data.Exception = ex.InnerException.Message;
                    }
                }
                else
                {
                    data.Exception = ex.Message.ToString();
                }
                //dynamic data = new JObject();

            }
            logger.Info("Response Body: \n" + JsonConvert.SerializeObject(data));
            return Ok<JObject>(data);
        }
        [Route("api/AndroInteraction/processAudio")]
        [HttpPost()]
        public async Task<IHttpActionResult> processAudioFile()
        {
            List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();
            Dictionary<string, string> returnData = new Dictionary<string, string>();
            var context = HttpContext.Current;

            try
            {
                string dealerCode = string.Empty;
                using (var db = new AutoSherDBContext())
                {
                    dealerCode = db.dealers.FirstOrDefault().dealerCode;
                }

                var root = context.Server.MapPath("~/wyzAudioData/" + dealerCode + "/");
                //var root = "C:/AS_DotNet/WyzAudioFiles/"+ dealerCode+"/";
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }

                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    string name = "";
                    var docfiles = new List<string>();
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];
                        name = postedFile.FileName;
                        name = name.Trim('"');
                        name = (name.Replace('+', '_')).Replace('-', '_');
                        var filePath = HttpContext.Current.Server.MapPath("~/wyzAudioData/" + dealerCode + "/" + name);
                        postedFile.SaveAs(filePath);
                        docfiles.Add(filePath);
                    }

                    returnData["fileName"] = name;
                    returnData["status"] = "1";
                    data.Add(returnData);
                    //result = Request.CreateResponse(HttpStatusCode.Created, docfiles);
                }
                else
                {
                    returnData["fileName"] = "";
                    returnData["status"] = "1";
                    data.Add(returnData);
                }
            }
            catch (Exception ex)
            {
                returnData["fileName"] = "";
                returnData["status"] = "1";
                data.Add(returnData);
                return Ok<List<Dictionary<string, string>>>(data);
            }
            return Ok<List<Dictionary<string, string>>>(data);
        }


        [Route("api/AndroInteraction/downloadApk")]
        [HttpGet]
        public IHttpActionResult downloadApk(string appfor, string appver)
        {
            dynamic data = new JObject();
            try
            {
                appfor = appfor.Trim().ToUpper();
                appver = appver.Trim();
                string dealerCode;
                using (var db = new AutoSherDBContext())
                {
                    dealerCode = db.dealers.FirstOrDefault().dealerCode;
                }

                string root = @"C:\AndroidApps\" + dealerCode + "\\" + appfor + "\\";
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
                DirectoryInfo dirInfo = new DirectoryInfo(root);

                string path = string.Empty;
                string fileName = appfor + "_" + appver + ".apk";
                string filePath = @"C:\AndroidApps\" + dealerCode + "\\" + appfor + "\\" + appfor + "_" + appver + ".apk";
                if (File.Exists(filePath))
                {
                    //FileInfo[] file = dirInfo.GetFiles("CRM_1.4.apk");
                    FileInfo[] file = dirInfo.GetFiles(fileName);

                    foreach (FileInfo fInfo in file)
                    {
                        path = root + fInfo.Name;
                        fileName = fInfo.Name;
                        break;
                    }

                    if (!string.IsNullOrEmpty(path))
                    {
                        var dataBytes = File.ReadAllBytes(path);
                        var dataStream = new MemoryStream(dataBytes);
                        return new returnApk(dataStream, Request, fileName);
                    }
                    else
                    {
                        data.result = "No file found...";
                        return Ok<JObject>(data);
                    }

                    //if (file.Count()>0)
                    //{
                    //    var dataBytes = File.ReadAllBytes(file[0].Name);
                    //    var dataStream = new MemoryStream(dataBytes);

                    //    return new returnApk(dataStream, Request, fileName);
                    //}
                    //else
                    //{
                    //    return Ok<string>("No file found...");
                    //}
                }
                else
                {
                    data.result = "No file found...";
                    return Ok<JObject>(data);
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

                data.exception = exception;
                return Ok<JObject>(data);
            }
        }

        [Route("api/AndroInteraction/getAppFilesName")]
        [HttpGet]
        public IHttpActionResult getAppFilesName(string appfor)
        {
            dynamic data = new JObject();
            List<string> filesList = new List<string>();
            try
            {
                appfor = appfor.Trim().ToUpper();
                string dealerCode;
                using (var db = new AutoSherDBContext())
                {
                    dealerCode = db.dealers.FirstOrDefault().dealerCode;
                }

                string root = @"C:\AndroidApps\" + dealerCode + "\\" + appfor + "\\";
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
                DirectoryInfo dirInfo = new DirectoryInfo(root);

                string filePath = @"C:\AndroidApps\" + dealerCode + "\\" + appfor + "\\";
                foreach (FileInfo fInfo in dirInfo.GetFiles())
                {
                    filesList.Add(fInfo.Name);
                }

                if (filesList.Count() > 0)
                {
                    data.result = string.Join(",", filesList);
                    return Ok<JObject>(data);
                }
                else
                {
                    data.result = "~/" + dealerCode + "/" + appfor + "/" + ".. Directory is Empty...";
                    return Ok<JObject>(data);
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

                data.exception = exception;
                return Ok<JObject>(data);
            }
        }

        //[Route("api/AndroInteraction/updateCustomerInfo")]
        //[HttpPost]
        //public IHttpActionResult saveCustAdress([FromBody] FEAppAddressUpdate addres)
        //{
        //    Logger logger = LogManager.GetLogger("apkRegLogger");
        //    dynamic data = new JObject();
        //    string firebaseKey = "";
        //    try
        //    {
        //        if (addres != null)
        //        {
        //            logger.Info("Incoming CustInfo/Address Updation is: " + JsonConvert.SerializeObject(addres));
        //            long cust_id = 0;
        //            long id = 0;
        //            using (var db = new AutoSherDBContext())
        //            {

        //                if (!string.IsNullOrEmpty(addres.appointmentbookedId))
        //                {
        //                    if (addres.appointmentbookedId.Contains("PolicyDrop_"))
        //                    {
        //                        id = long.Parse(addres.appointmentbookedId.Split('_')[1]);
        //                        cust_id = db.insPolicyDrop.FirstOrDefault(m => m.id == id).customer_id ?? default(long);
        //                        if (id > 0)
        //                        {
        //                            firebaseKey = db.Fieldexecutivefirebaseupdations.FirstOrDefault(m => m.inspolicydrop_id == id).firebasekey;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        id = long.Parse(addres.appointmentbookedId);
        //                        cust_id = db.appointmentbookeds.FirstOrDefault(m => m.appointmentId == id).customer_id ?? default(long);
        //                        if (id > 0)
        //                        {
        //                            firebaseKey = db.Fieldexecutivefirebaseupdations.FirstOrDefault(m => m.appointmentbookedid == id).firebasekey;
        //                        }
        //                    }
        //                }
        //                else if (addres.DriverScheduler_id != 0)
        //                {
        //                    driverscheduler schedulerData = db.driverSchedulers.FirstOrDefault(m => m.id == addres.DriverScheduler_id);

        //                    if (schedulerData != null)
        //                    {
        //                        firebaseKey = schedulerData.firebasekey;
        //                        cust_id = schedulerData.customer_id ?? default(long);
        //                    }
        //                    else
        //                    {
        //                        data.status = "failure";
        //                        data.exception = "Invalid Request";
        //                        return Ok<JObject>(data);
        //                    }
        //                }
        //                else
        //                {
        //                    data.status = "failure";
        //                    data.exception = "Invalid Request";
        //                    return Ok<JObject>(data);
        //                }


        //                if (cust_id > 0)
        //                {
        //                    address adr = db.addresses.FirstOrDefault(m => m.customer_Id == cust_id && m.isPreferred == true);
        //                    if (adr != null)
        //                    {
        //                        adr.isPreferred = false;
        //                        db.addresses.AddOrUpdate(adr);
        //                        db.SaveChanges();
        //                    }

        //                    address newAddress = new address();
        //                    newAddress.concatenatedAdress = addres.address;
        //                    newAddress.isPreferred = true;
        //                    newAddress.pincode = addres.pincode;
        //                    newAddress.customer_Id = cust_id;
        //                    db.addresses.Add(newAddress);
        //                    db.SaveChanges();

        //                }
        //                else
        //                {
        //                    logger.Error("\n\n----------FF/Driver App Address updation Custome Id not found customer id is zero");
        //                    data.status = "failure";
        //                    return Ok<JObject>(data);
        //                }
        //            }
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
        //        logger.Error("\n\n------------ FE/Driver App adrress update error------------\n" + exception);
        //        data.status = "failure";
        //        return Ok<JObject>(data);
        //    }
        //    data.status = "success";
        //    data.firebasekey = firebaseKey;
        //    return Ok<JObject>(data);
        //}

        //[Route("api/AndroInteraction/uploadFiles")]
        //[HttpPost()]
        //public async Task<IHttpActionResult> processFile()
        //{
        //    var context = HttpContext.Current;
        //    FEDocumentsReturn docReturn = new FEDocumentsReturn();
        //    docReturn.filePaths = new List<Dictionary<string, string>>();
        //    Logger logger = LogManager.GetLogger("apkRegLogger");
        //    try
        //    {
        //        string dealerCode = string.Empty, username = string.Empty, documentType = string.Empty, documentName = string.Empty, fileString = string.Empty, uploadedFileName = string.Empty;
        //        long custId = 0, userId = 0, insid = 0;
        //        string fileUploadPath = "", fireBaseKey = string.Empty;// context.Server.MapPath("~/UploadedFiles/" + Session["DealerCode"].ToString() + "/" + Session["UserName"].ToString() + "/");
        //        var httpRequest = HttpContext.Current.Request;
        //        using (var db = new AutoSherDBContext())
        //        {
        //            #region Assigning Form Data to Local Variables
        //            if (!string.IsNullOrEmpty(httpRequest.Form["userId"]))
        //            {
        //                userId = long.Parse(httpRequest.Form["userId"].ToString());
        //            }

        //            if (!string.IsNullOrEmpty(httpRequest.Form["custId"]))
        //            {
        //                custId = long.Parse(httpRequest.Form["custId"]);
        //            }
        //            string FormDataString = httpRequest.Form.ToString();

        //            if (!string.IsNullOrEmpty(httpRequest.Form["appointmentId"]))
        //            {
        //                if (httpRequest.Form["appointmentId"].Contains("PolicyDrop_"))
        //                {
        //                    insid = long.Parse(httpRequest.Form["appointmentId"].ToString().Split('_')[1]);
        //                    if (db.Fieldexecutivefirebaseupdations.Count(m => m.inspolicydrop_id == insid) > 0)
        //                    {
        //                        fireBaseKey = db.Fieldexecutivefirebaseupdations.FirstOrDefault(m => m.inspolicydrop_id == insid).firebasekey;
        //                    }
        //                    else
        //                    {
        //                        fireBaseKey = "NotFound";
        //                    }
        //                }
        //                else
        //                {
        //                    insid = long.Parse(httpRequest.Form["appointmentId"]);
        //                    if (db.Fieldexecutivefirebaseupdations.Count(m => m.appointmentbookedid == insid) > 0)
        //                    {
        //                        fireBaseKey = db.Fieldexecutivefirebaseupdations.FirstOrDefault(m => m.appointmentbookedid == insid).firebasekey;
        //                    }
        //                    else
        //                    {
        //                        fireBaseKey = "NotFound";
        //                    }
        //                }
        //            }

        //            if (!string.IsNullOrEmpty(httpRequest.Form["documentType"]))
        //            {
        //                documentType = httpRequest.Form["documentType"];
        //            }

        //            if (!string.IsNullOrEmpty(httpRequest.Form["documentName"]))
        //            {
        //                documentName = httpRequest.Form["documentName"];
        //            }

        //            logger.Info(string.Format("\n Incoming doc details:\n documentType:{0}\n documentName:{1}\n userid:{2}| custId:{3}| aptId:{4}", documentType, documentName, userId, custId, insid));
        //            #endregion


        //            dealerCode = db.dealers.FirstOrDefault().dealerCode;

        //            if (db.wyzusers.Any(m => m.id == userId))
        //            {
        //                username = db.wyzusers.FirstOrDefault(m => m.id == userId).userName;
        //            }
        //            else
        //            {
        //                docReturn.fileName = "";
        //                docReturn.status = "0";
        //                logger.Info("No WyzUser Id Found");
        //                return Ok<FEDocumentsReturn>(docReturn);
        //            }

        //            fileUploadPath = context.Server.MapPath("~/UploadedFiles/" + dealerCode + "/" + username + "/");

        //            if (!Directory.Exists(fileUploadPath))
        //            {
        //                Directory.CreateDirectory(fileUploadPath);
        //            }


        //            if (httpRequest.Files.Count > 0)
        //            {
        //                documentuploadhistory doc = new documentuploadhistory();
        //                doc.customerId = custId;
        //                doc.user = username;
        //                doc.userId = userId;
        //                doc.deptName = documentType;
        //                doc.documentName = documentName;

        //                //var read1 = HttpContext.Current.Request.conte
        //                var docfiles = new List<string>();
        //                foreach (string file in httpRequest.Files)
        //                {
        //                    string extension = string.Empty, name = string.Empty;

        //                    var postedFile = httpRequest.Files[file];
        //                    name = postedFile.FileName;
        //                    extension = Path.GetExtension(name);

        //                    string savingFileName = name.Split('.')[0] + "_" + custId + "_" + DateTime.Now.ToString("dd-MM-yyyy").Replace("-", "") + "_" + DateTime.Now.ToString("HH:mm:ss").Replace(":", "") + extension;
        //                    postedFile.SaveAs(fileUploadPath + savingFileName);


        //                    //for saving multiple files and fileName with comma(,) separate
        //                    fileString = fileString + fileUploadPath + savingFileName + ";";
        //                    uploadedFileName = uploadedFileName + name + ";";
        //                    Dictionary<string, string> filePath = new Dictionary<string, string>();
        //                    filePath["ImgURL"] = "/UploadedFiles/" + dealerCode + "/" + username + "/" + savingFileName;
        //                    docReturn.filePaths.Add(filePath);
        //                }

        //                uploadedFileName = uploadedFileName.Remove((uploadedFileName.Length - 1));
        //                fileString = fileString.Remove((fileString.Length - 1));

        //                doc.filePath = fileString;
        //                doc.uploadFileName = uploadedFileName;
        //                doc.uploadDateTime = DateTime.Now;

        //                db.documentuploadhistories.Add(doc);
        //                db.SaveChanges();

        //                docReturn.status = "1";
        //                docReturn.fileName = documentName;
        //                docReturn.fireBaseKey = fireBaseKey;
        //            }
        //            else
        //            {
        //                docReturn.fileName = "";
        //                docReturn.status = "0";
        //                logger.Info("No File Id Found");
        //            }
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

        //        logger.Error("FE app file upload api error: \n" + exception);

        //        docReturn.fileName = "";
        //        docReturn.status = "0";
        //        return Ok<FEDocumentsReturn>(docReturn);
        //    }
        //    return Ok<FEDocumentsReturn>(docReturn);
        //}
        //[Route("api/AndroInteraction/uploaddriverappfiles")]
        //[HttpPost()]
        //public async Task<IHttpActionResult> uploaddriverappfiles()
        //{
        //    var context = HttpContext.Current;
        //    FEDocumentsReturn docReturn = new FEDocumentsReturn();
        //    docReturn.filePaths = new List<Dictionary<string, string>>();
        //    Logger logger = LogManager.GetLogger("apkRegLogger");
        //    try
        //    {
        //        string dealerCode = string.Empty, documentType = string.Empty, documentName = string.Empty, fileString = string.Empty, uploadedFileName = string.Empty;
        //        long custId = 0, userId = 0, driverscheduler_id = 0;
        //        string fileUploadPath = "", driver_name = string.Empty, fireBaseKey = string.Empty;// context.Server.MapPath("~/UploadedFiles/" + Session["DealerCode"].ToString() + "/" + Session["UserName"].ToString() + "/");
        //        var httpRequest = HttpContext.Current.Request;

        //        using (var db = new AutoSherDBContext())
        //        {
        //            if (!string.IsNullOrEmpty(httpRequest.Form["driver_id"]))
        //            {
        //                userId = long.Parse(httpRequest.Form["driver_id"].ToString());
        //            }

        //            if (!string.IsNullOrEmpty(httpRequest.Form["custId"]))
        //            {
        //                custId = long.Parse(httpRequest.Form["custId"]);
        //            }

        //            if (!string.IsNullOrEmpty(httpRequest.Form["driverscheduler_id"]))
        //            {
        //                driverscheduler_id = long.Parse(httpRequest.Form["driverscheduler_id"]);
        //            }



        //            if (!string.IsNullOrEmpty(httpRequest.Form["documentType"]))
        //            {
        //                documentType = httpRequest.Form["documentType"];
        //            }

        //            if (!string.IsNullOrEmpty(httpRequest.Form["driver_name"]))
        //            {
        //                driver_name = httpRequest.Form["driver_name"];
        //            }

        //            if (!string.IsNullOrEmpty(httpRequest.Form["documentName"]))
        //            {
        //                documentName = httpRequest.Form["documentName"];
        //            }

        //            logger.Info(string.Format("\n Incoming/DriverApp doc details:\n documentType:{0}\n documentName:{1}\n Driver_userid:{2}| custId:{3}| driverScheduler_id:{4}", documentType, documentName, userId, custId, driverscheduler_id));



        //            dealerCode = db.dealers.FirstOrDefault().dealerCode;



        //            fileUploadPath = context.Server.MapPath("~/UploadedFiles/" + dealerCode + "/" + driver_name + "/");

        //            if (!Directory.Exists(fileUploadPath))
        //            {
        //                Directory.CreateDirectory(fileUploadPath);
        //            }


        //            if (httpRequest.Files.Count > 0)
        //            {
        //                documentuploadhistory doc = new documentuploadhistory();
        //                doc.customerId = custId;
        //                doc.user = driver_name;
        //                doc.userId = userId;
        //                doc.deptName = documentType;
        //                doc.documentName = documentName;

        //                //var read1 = HttpContext.Current.Request.conte
        //                var docfiles = new List<string>();
        //                foreach (string file in httpRequest.Files)
        //                {
        //                    string extension = string.Empty, name = string.Empty;

        //                    var postedFile = httpRequest.Files[file];
        //                    name = postedFile.FileName;
        //                    extension = Path.GetExtension(name);

        //                    string savingFileName = name.Split('.')[0] + "_" + custId + "_" + DateTime.Now.ToString("dd-MM-yyyy").Replace("-", "") + "_" + DateTime.Now.ToString("HH:mm:ss").Replace(":", "") + extension;
        //                    postedFile.SaveAs(fileUploadPath + savingFileName);


        //                    //for saving multiple files and fileName with comma(,) separate
        //                    fileString = fileString + fileUploadPath + savingFileName + ";";
        //                    uploadedFileName = uploadedFileName + name + ";";
        //                    Dictionary<string, string> filePath = new Dictionary<string, string>();
        //                    filePath["ImgURL"] = "/UploadedFiles/" + dealerCode + "/" + driver_name + "/" + savingFileName;
        //                    docReturn.filePaths.Add(filePath);
        //                }

        //                uploadedFileName = uploadedFileName.Remove((uploadedFileName.Length - 1));
        //                fileString = fileString.Remove((fileString.Length - 1));

        //                doc.filePath = fileString;
        //                doc.uploadFileName = uploadedFileName;
        //                doc.uploadDateTime = DateTime.Now;


        //                fireBaseKey = db.driverSchedulers.FirstOrDefault(m => m.id == driverscheduler_id).firebasekey;

        //                db.documentuploadhistories.Add(doc);
        //                db.SaveChanges();

        //                docReturn.status = "1";
        //                docReturn.fileName = documentName;
        //                docReturn.fireBaseKey = fireBaseKey;
        //            }
        //            else
        //            {
        //                docReturn.fileName = "";
        //                docReturn.status = "0";
        //                logger.Info("No File Id Found");
        //            }
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

        //        logger.Error("FE app file upload api error: \n" + exception);

        //        docReturn.fileName = "";
        //        docReturn.status = "0";
        //        return Ok<FEDocumentsReturn>(docReturn);
        //    }
        //    return Ok<FEDocumentsReturn>(docReturn);
        //}

        //[Route("api/AndroInteraction/savefedetails")]
        //[HttpPost]
        //public IHttpActionResult SaveFeDetails([FromBody] FETracking fedata)
        //{
        //    Logger logger = LogManager.GetLogger("apkRegLogger");
        //    dynamic data = new JObject();
        //    try
        //    {
        //        using (var db = new AutoSherDBContext())
        //        {
        //            if (fedata != null)
        //            {
        //                logger.Info("FE Login Details" + JsonConvert.SerializeObject(fedata));

        //                if (db.wyzusers.Count(m => m.role == "InsuranceAgent" && m.phoneNumber == fedata.PhoneNumber) > 0)
        //                {
        //                    fedata.WyzUserId = db.wyzusers.FirstOrDefault(m => m.role == "InsuranceAgent" && m.phoneNumber == fedata.PhoneNumber).id;
        //                    if (db.feTracking.Count(m => m.UniqueID == fedata.UniqueID && m.Flag == "login") > 0)
        //                    {
        //                        var updatefetrackdetails = db.feTracking.FirstOrDefault(m => m.UniqueID == fedata.UniqueID && m.Flag == "login");
        //                        updatefetrackdetails.Flag = fedata.Flag;
        //                        updatefetrackdetails.LogoutLocation = fedata.Location;
        //                        updatefetrackdetails.LogoutDate = fedata.LoginDate;
        //                        updatefetrackdetails.LogoutTime = fedata.LoginTime;
        //                        db.feTracking.AddOrUpdate(updatefetrackdetails);
        //                    }
        //                    else
        //                    {
        //                        db.feTracking.Add(fedata);
        //                    }
        //                    db.SaveChanges();

        //                    data.status = "Success";
        //                    return Ok<JObject>(data);
        //                }
        //                else
        //                {
        //                    logger.Info("FE login Captured Failed: No User Found for PhoneNumber" + fedata.PhoneNumber);
        //                    data.status = "Failed";
        //                    data.exception = "No User data found";
        //                    return Ok<JObject>(data);
        //                }
        //            }
        //            else
        //            {
        //                data.status = "Failed";
        //                data.exception = "Incoming body is null";
        //                return Ok<JObject>(data);

        //            }
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
        //        if (ex.StackTrace.Contains(':'))
        //        {
        //            exception = "Line: " + ex.StackTrace.Split(':')[(ex.StackTrace.Split(':').Count() - 1)] + " " + exception;
        //        }
        //        data.status = "Failed";
        //        data.exception = exception;
        //        return Ok<JObject>(data);
        //    }

        //}

        [Route("api/AndroInteraction/validateIMEINO")]
        [HttpPost]
        public IHttpActionResult validateIMEINO([FromBody] dynamic phoneNumber)
        {
            dynamic data = new JObject();
            Logger logger = LogManager.GetLogger("apkRegLogger");
            try
            {
                logger.Info("Request Body:\n" + phoneNumber);

                using (var db = new AutoSherDBContext())
                {
                    string phNum = phoneNumber.Value ;

                    wyzuser user = new wyzuser();
                    user = db.wyzusers.FirstOrDefault(m => m.phoneNumber == phNum);
                    if (user != null)
                    {
                        data.authenticationStatus = true;
                        data.IMEINO1 = user.phoneIMEINo;
                        data.IMEINO2 = user.phoneIMEINo1;
                    }
                    else
                    {
                        data.authenticationStatus = false;
                        data.Exception = "Details Not Found";
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        data.Exception = ex.InnerException.InnerException.Message;
                    }
                    else
                    {
                        data.Exception = ex.InnerException.Message;
                    }
                }
                else
                {
                    data.Exception = ex.Message.ToString();
                }
                //dynamic data = new JObject();

            }
            logger.Info("Response Body: \n" + JsonConvert.SerializeObject(data));
            return Ok<JObject>(data);
        }

    }

    public class AppLogCheck
    {
        public string phoneNumber { get; set; }
        public string phoneIMEINo { get; set; }
        public string registrationId { get; set; }
        public string latestAppVersion { get; set; }

        public string fileName { get; set; }
        public string dealerCode { get; set; }
        public string feRequest { get; set; }
        public string FEName { get; set; }
    }

    public class FEAppAddressUpdate
    {
        public string dealerCode { get; set; }
        public string address { get; set; }
        public string appointmentbookedId { get; set; }
        public long pincode { get; set; }
        public long DriverScheduler_id { get; set; }

    }

    public class smsSaving
    {
        public long custId { get; set; }
        public long vehiId { get; set; }
        public long driver_id { get; set; }

        [System.Web.Mvc.AllowHtml]
        public string response_string { get; set; }
        public string phNum { get; set; }
        public string message { get; set; }
    }

    public class FEDocumentsReturn
    {
        public string status { get; set; }
        public string fileName { get; set; }
        public List<Dictionary<string, string>> filePaths { get; set; }
        public string fireBaseKey { get; set; }
    }

    public class returnApk : IHttpActionResult
    {
        MemoryStream bookStuff;
        string PdfFileName;
        HttpRequestMessage httpRequestMessage;
        HttpResponseMessage httpResponseMessage;
        public returnApk(MemoryStream data, HttpRequestMessage request, string filename)
        {
            bookStuff = data;
            httpRequestMessage = request;
            PdfFileName = filename;
        }
        public System.Threading.Tasks.Task<HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
        {
            httpResponseMessage = httpRequestMessage.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(bookStuff);
            //httpResponseMessage.Content = new ByteArrayContent(bookStuff.ToArray());  
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = PdfFileName;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            return System.Threading.Tasks.Task.FromResult(httpResponseMessage);
        }
    }

}
