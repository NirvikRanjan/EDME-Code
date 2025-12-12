using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace AutoSherpa_project.Models.EIBL_API
{
    public static class GenerateToken
    {
        private static readonly HttpClient client = new HttpClient();
        public static async Task<string> GenerateTokenAsync()
        {
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    GenerateAPIToken generateTokenCreds = await db.GenerateAPITokens.Where(m => m.IsActive == true).FirstOrDefaultAsync();

                    if (generateTokenCreds != null)
                    {
                        string URL = generateTokenCreds.URL;
                        string CLIENT_ID = generateTokenCreds.CLIENT_ID;
                        string CLIENT_SECRET = generateTokenCreds.CLIENT_SECRET;

                        var requestBody = new
                        {
                            ClientId = CLIENT_ID,
                            ClientSecret = CLIENT_SECRET
                        };

                        var json = JsonConvert.SerializeObject(requestBody);

                        var content = new StringContent(json, Encoding.UTF8, "application/json");

                        var response = await client.PostAsync(URL, content);

                        if (response.IsSuccessStatusCode)
                        {
                            var tokenContent = await response.Content.ReadAsStringAsync();
                            var tokenObject = JObject.Parse(tokenContent);
                            var token = tokenObject["Token"].ToString();

                            return token;
                        }
                        else
                        {
                            return string.Empty;
                        }
                    }
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}