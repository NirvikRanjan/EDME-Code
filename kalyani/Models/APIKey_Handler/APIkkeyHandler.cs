using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace AutoSherpa_project.Models.APIKey_Handler
{
    public class APIkkeyHandler:DelegatingHandler
    {
        //set a default API key 
        //private const string yourApiKey = string.Empty;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if ( request.RequestUri.PathAndQuery.Contains("/HeroPaymentConf/heroInsuranceRenewalPolicyAPI"))
            {
                string yourApiKey = string.Empty;
                using(var db=new AutoSherDBContext())
                {
                    yourApiKey = db.tenants.FirstOrDefault().sherpa_hero_api_key;
                }

                bool isValidAPIKey = false;
                IEnumerable<string> lsHeaders;
                //Validate that the api key exists

                if (yourApiKey != null)
                {
                    var checkApiKeyExists = request.Headers.TryGetValues("SHERPA_API_KEY", out lsHeaders);

                    if (checkApiKeyExists)
                    {
                        if (lsHeaders.FirstOrDefault().Equals(yourApiKey))
                        {
                            isValidAPIKey = true;
                        }
                    }
                }
                else
                {
                    return request.CreateResponse(HttpStatusCode.Forbidden, "API KEY NOT PRESENT IN DATABASE");
                }

                //If the key is not valid, return an http status code.
                if (!isValidAPIKey)
                    return request.CreateResponse(HttpStatusCode.Forbidden, "Invalid API Key");

                //Allow the request to process further down the pipeline
                var response = await base.SendAsync(request, cancellationToken);

                //Return the response back up the chain
                return response;
            }
            else
            {
                var response = await base.SendAsync(request, cancellationToken);
                return response;

            }

        }
    }
}