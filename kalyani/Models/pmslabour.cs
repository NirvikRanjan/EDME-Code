using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoSherpa_project.Models
{
    [Table("pmslabour")]

    public class pmslabour
    {
        public int id { get; set; }
        public long cityid { get; set; }
        public long modelid { get; set; }
        public string fuelType { get; set; }
        public long mileageid { get; set; }
        public int labourAmount { get; set; }
        public float partsAmount { get; set; }
        public int wheelAlignment { get; set; }
        public int wheelBalancing { get; set; }
        public float engineOil { get; set; }
        public int oilFillter { get; set; }
        public int brakeFluid { get; set; }
        public int coolant { get; set; }
        public int sparkPlug { get; set; }
        public int airFilter { get; set; }
        public int fuelFilter { get; set; }
        public int belt { get; set; }
        //  public int other1 { get; set; }
        //  public int other2 { get; set; }
        //   public int total { get; set; }
        public float pmsLabourwithtax { get; set; }
    }
}