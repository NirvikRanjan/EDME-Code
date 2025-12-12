using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoSherpa_project.Models.EIBL_API
{
    public class GetPlanAPIResponse
    {
        public List<VehicleCover> VehicleCover {  get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class VehicleCover
    {
        public string Package { get; set; }
        public string SAOD { get; set; }
        public string SATP { get; set; }
        public string OEMID { get; set; }
        public string PreviousPolicyNo { get; set; }
        public string CPCode { get; set; }
        public bool STATUS { get; set; }
        public bool IsActive { get; set; }
    }
}