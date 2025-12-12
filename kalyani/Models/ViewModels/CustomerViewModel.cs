using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoSherpa_project.Models.ViewModels
{
    public class CustomerViewModel
    {
        public customer customer { get; set; }
        public phone phone { get; set; }
        public email email { get; set; }
        public address address { get; set; }
        public vehicle vehicle { get; set; }
    }
}