using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace AutoSherpa_project.Models.Schedulers
{
    public class schedulerCommonFunction
    {
        public void stopScheduler(string schedulerName)
        {
            Logger logger = LogManager.GetLogger("apkRegLogger");
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    schedulers schedulerDetails = db.schedulers.FirstOrDefault(m => m.scheduler_name == schedulerName);
                    schedulerDetails.IsItRunning = false;
                    db.schedulers.AddOrUpdate(schedulerDetails);
                    db.SaveChanges();
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

                if (ex.StackTrace.Contains(':'))
                {
                    exception = exception + "Line: " + ex.StackTrace.Split(':')[(ex.StackTrace.Split(':').Count() - 1)];
                }

                logger.Error(schedulerName + "scheduler StatusChanging Error:\n" + exception);
            }
        }
        public void startScheduler(string schedulerName)
        {
            Logger logger = LogManager.GetLogger("apkRegLogger");
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    schedulers schedulerDetails = db.schedulers.FirstOrDefault(m => m.scheduler_name == schedulerName);

                    schedulerDetails.IsItRunning = true;
                    schedulerDetails.LastRun = DateTime.Now;
                    db.schedulers.AddOrUpdate(schedulerDetails);
                    db.SaveChanges();
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

                if (ex.StackTrace.Contains(':'))
                {
                    exception = exception + "Line: " + ex.StackTrace.Split(':')[(ex.StackTrace.Split(':').Count() - 1)];
                }

                logger.Error(schedulerName + "scheduler StatusChanging Error:\n" + exception);
            }
        }
        
        public string Generatehash512(string text)
        {

            byte[] message = Encoding.UTF8.GetBytes(text);

            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            SHA512Managed hashString = new SHA512Managed();
            string hex = "";
            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;

        }
        public static bool ValidateCertificate(HttpRequestMessage requestMessage, X509Certificate2 certificate2, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        



        public static string UrlEncode(string url)
        {
            string encodedUrl = Uri.EscapeUriString(url);
            string encodedUrl1 = encodedUrl.Replace(",", "%2C");
            return encodedUrl1;
        }


        public static string ConvertToQueryString(Dictionary<string, string> formData)
        {
            StringBuilder queryStringBuilder = new StringBuilder();

            foreach (var kvp in formData)
            {
                if (queryStringBuilder.Length > 0)
                {
                    queryStringBuilder.Append('&');
                }

                queryStringBuilder.Append(kvp.Key);
                queryStringBuilder.Append('=');
                queryStringBuilder.Append(kvp.Value);
            }

            return queryStringBuilder.ToString();
        }

        public async Task<string> GetTokenForAll()
        {
            string accessToken = string.Empty;
            var url = "https://uatapigw-tataaig.auth.ap-south-1.amazoncognito.com/oauth2/token";
            var requestContent = new FormUrlEncodedContent(new[]
            {
               new KeyValuePair<string, string>("grant_type", "client_credentials"),
               new KeyValuePair<string, string>("scope", "https://api.iorta.in/write"),
               new KeyValuePair<string, string>("client_id", "5qdbqng8plqp1ko2sslu695n2g"),
               new KeyValuePair<string, string>("client_secret", "gki6eqtltmjj37gpqq0dt52dt651o079dn6mls62ptkvsa2b45c")
            });

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");


                try
                {
                    var response = await httpClient.PostAsync(url, requestContent);
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Parse the JSON response to get the access token
                    var jsonObject = JObject.Parse(responseBody);
                    accessToken = jsonObject["access_token"].ToString(); // Store the access token in the variable
                }
                catch (HttpRequestException e)
                {
                    return $"Error: {e.Message}"; // Return error message
                }
            }

            return accessToken; // This line will never be reached
        }

        public async Task<string> DownloadNewPolicy(string encPolicyNo)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(encPolicyNo))
                throw new ArgumentException("Policy number cannot be null or empty.", nameof(encPolicyNo));

            var url = $"https://uatapigw.tataaig.com/tw-motor-renewal/v1/new-policy-download/{encPolicyNo}";
            var apiKey = "g8hoqi8TBA2mBpxgMohdTcWxAfv6JsJ6wLztOWm4";

            // Fetch the token asynchronously
            string accessToken = await GetTokenForAll();

            using (var httpClient = new HttpClient())
            {
                // Configure request headers
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);

                try
                {
                    // Send GET request and handle response
                    using (var response = await httpClient.GetAsync(url))
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();

                        // Check for success status
                        if (response.IsSuccessStatusCode)
                        {
                            var responseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                            return responseObject?.message_txt?.ToString() ?? "No message text available";
                        }
                        else
                        {
                            // Handle errors in the response
                            var errorObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                            var errorMessage = errorObject?.message_txt?.ToString() ?? "Unknown error occurred.";
                            throw new Exception($"Error downloading policy: {errorMessage}");
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    // Log and rethrow HttpRequestException
                    // LogError(ex); // Example logging function
                    throw new Exception("An error occurred while sending the request.", ex);
                }
                catch (Exception ex)
                {
                    // Log and rethrow unexpected exceptions
                    // LogError(ex); // Example logging function
                    throw new Exception("An unexpected error occurred.", ex);
                }
            }
        }

    }
}