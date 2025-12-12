using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoSherpa_project.Models
{
    [Table("sainteraction")]
    public class sainteraction
    {
        public int id { get; set; }
        public long roassignid { get; set; }
        public long wyzuser_id { get; set; }
        public DateTime calldate { get; set; }
        public string calltime { get; set; }
        public DateTime? callmadeon { get; set; }
        public string saname { get; set; }
        public long vehicle_id { get; set; }
        public long customer_id { get; set; }
        public string reasonforpendingro { get; set; }
        public string partname { get; set; }
        public string partnumber { get; set; }
        public string pendingreson_remarks { get; set; }
        public DateTime? requestdate { get; set; }
        public DateTime? orderdate { get; set; }
        public DateTime? etd { get; set; }
    }
}