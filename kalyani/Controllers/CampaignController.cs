using AutoSherpa_project.Models;
using AutoSherpa_project.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace AutoSherpa_project.Controllers
{
    [AuthorizeFilter]
    public class CampaignController : Controller
    {
        // GET: Campaign
        [HttpGet]
        [ActionName("Campaign")]
        public ActionResult CampaignGet()
        {
            return View();
        }
        
        public ActionResult CampaignPost(campaign campaignMod)
        {
            try
            {
                using (AutoSherDBContext dBContext = new AutoSherDBContext())
                {
                    if (dBContext.campaigns.Where(m => m.campaignName.ToLower() == campaignMod.campaignName).Count() == 0)
                    {
                        campaignMod.isactive = true;
                        campaignMod.isValid = true;
                        dBContext.campaigns.Add(campaignMod);
                        dBContext.SaveChanges();
                    }
                    else
                    {
                        ViewBag.Success = false;
                    }
                    return Json(new { success = true });
                }
            }
            catch(Exception ex)
            {
                return Json(new { success = false,error=ex.Message });
            }
        }
    }
}