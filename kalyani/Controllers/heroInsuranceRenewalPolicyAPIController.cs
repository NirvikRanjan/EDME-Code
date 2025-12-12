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
    public class heroInsuranceRenewalPolicyAPIController : ApiController
    {
        [Route("HeroPaymentConf/heroInsuranceRenewalPolicyAPI/newpolicyintimation")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult newpolicyintimation(List<paymentconfirmation> paymentDetails)
        {
            Logger logger = LogManager.GetLogger("apkRegLogger");
            List<paymentResponse> listsendpaymentResponse = new List<paymentResponse>();
            string JsonString = string.Empty;
            dynamic data = new JObject();
            string policyNumber = null;

            try
            {
                using (var db = new AutoSherDBContext())
                {
                    if (paymentDetails != null)
                    {
                        try
                        {
                            logger.Info("Pushing Payment confirmation{0} - Data {1}", DateTime.Now, JsonConvert.SerializeObject(paymentDetails));
                            foreach (var paymntrenewl in paymentDetails)
                            {
                                try
                                {
                                    if (db.insuranceassignedinteractions.Count(m => m.previouspolicynumber.ToUpper().Trim() == paymntrenewl.Previous_PolicyNumber.ToUpper().Trim()) > 0)
                                    {
                                        var insassignedInteraction = db.insuranceassignedinteractions.FirstOrDefault(m => m.previouspolicynumber.ToUpper().Trim() == paymntrenewl.Previous_PolicyNumber.ToUpper().Trim());
                                        paymentResponse sendpaymentResponse = new paymentResponse();
                                        policyNumber = paymntrenewl.New_PolicyNumber;
                                        paymntrenewl.assignedinteractionid = insassignedInteraction.id;
                                        paymntrenewl.customer_id = insassignedInteraction.customer_id;
                                        paymntrenewl.vehicle_id = insassignedInteraction.vehicle_vehicle_id;
                                        paymntrenewl.wyzuser_id = insassignedInteraction.wyzUser_id;
                                        paymntrenewl.dealer_id = insassignedInteraction.herodealerID;
                                        if (paymntrenewl.IS_online == "0")
                                        {
                                            paymntrenewl.Payment_Type = "Offline";
                                        }
                                        else if (paymntrenewl.IS_online == "1")
                                        {
                                            paymntrenewl.Payment_Type = "Online";
                                        }
                                        paymntrenewl.updateddate = DateTime.Now;
                                        paymntrenewl.updatetime = DateTime.Now.ToString("hh:mm:ss");
                                        paymntrenewl.responseStatus = "Success";
                                        db.Paymentconfirmations.Add(paymntrenewl);
                                        db.SaveChanges();
                                        sendpaymentResponse.status = "Success";
                                        sendpaymentResponse.policyNumber = policyNumber;
                                        listsendpaymentResponse.Add(sendpaymentResponse);
                                    }
                                    else
                                    {
                                        paymntrenewl.updateddate = DateTime.Now;
                                        paymntrenewl.updatetime = DateTime.Now.ToString("hh:mm:ss");
                                        paymntrenewl.assignedinteractionid = 0;
                                        paymntrenewl.customer_id = 0;
                                        paymntrenewl.vehicle_id = 0;
                                        paymntrenewl.wyzuser_id = 0;
                                        paymntrenewl.dealer_id = 0;
                                        if (paymntrenewl.IS_online == "0")
                                        {
                                            paymntrenewl.Payment_Type = "Offline";
                                        }
                                        else if (paymntrenewl.IS_online == "1")
                                        {
                                            paymntrenewl.Payment_Type = "Online";
                                        }
                                        paymntrenewl.responseStatus = "Successs";
                                        //paymntrenewl.responseStatus = "Policy Number Not Found";
                                        db.Paymentconfirmations.Add(paymntrenewl);
                                        db.SaveChanges();
                                        paymentResponse sendpaymentResponse = new paymentResponse();
                                        sendpaymentResponse.status = "Success";
                                        //sendpaymentResponse.status = "Policy Number Not Found";
                                        sendpaymentResponse.policyNumber = paymntrenewl.Previous_PolicyNumber;
                                        listsendpaymentResponse.Add(sendpaymentResponse);
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
                                    if (ex.StackTrace.Contains(':'))
                                    {
                                        exception = "Line: " + ex.StackTrace.Split(':')[(ex.StackTrace.Split(':').Count() - 1)] + " " + exception;
                                    }
                                    paymentResponse sendpaymentResponse = new paymentResponse();
                                    sendpaymentResponse.status = exception;
                                    sendpaymentResponse.policyNumber = policyNumber;
                                    listsendpaymentResponse.Add(sendpaymentResponse);

                                    logger.Info("Pushing Payment confirmation Exception{0}", exception);

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
                            if (ex.StackTrace.Contains(':'))
                            {
                                exception = "Line: " + ex.StackTrace.Split(':')[(ex.StackTrace.Split(':').Count() - 1)] + " " + exception;
                            }
                            logger.Info("Pushing Payment confirmation Exception{0}", exception);
                        }
                        return Ok(listsendpaymentResponse);
                    }
                    else
                    {
                        data.status = "Failed";
                        data.exception = "Incoming body is null";
                        return Ok<JObject>(data);

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
                if (ex.StackTrace.Contains(':'))
                {
                    exception = "Line: " + ex.StackTrace.Split(':')[(ex.StackTrace.Split(':').Count() - 1)] + " " + exception;
                }
                data.status = "Failed";
                data.exception = exception;
                return Ok<JObject>(data);
            }

        }

        public class paymentResponse
        {
            public string status;
            public string policyNumber;
            public string exception;
        }
    }
}