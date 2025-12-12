using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoSherpa_project.Models.ViewModels
{
    public class addCreVM
    {
        public string creName { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string confirmPass { get; set; }
        public string firstLocation { get; set; }
        public string firstWorkshop { get; set; }
        public string role { get; set; }
        public string moduleType { get; set; }
        public string phoneNumber { get; set; }
        public string IMEI { get; set; }
        public string locations { get; set; }
        public string workshops { get; set; }
        public string creManager { get; set; }
        public string sipExtensionId { get; set; }
        public string sipID { get; set; }
        public string gsmip { get; set; }
        public string callingType { get; set; }

    }
    public class creTableVM
    {
        public string creName { get; set; }
        public string location { get; set; }
        public long? location_cityId { get; set; }
        public long? workshop_id { get; set; }
        public string workshop { get; set; }
        public string phoneNumber { get; set; }
        public string IMEI { get; set; }
        public string userName { get; set; }
        public string passWord { get; set; }
        public string creManager { get; set; }
        public long wyzId { get; set; }
        public string extensionId { get; set; }
        public string workshopIds { get; set; }

    }
}