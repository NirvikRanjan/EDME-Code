using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoSherpa_project.Models
{
    [Table("tempchangeassignmenticlist")]
    public class tempChangeAssignmentIClist
    {
        [Key]
        public long IClistID { get; set; }
        public long ICID { get; set; }
        public long ICCode { get; set; }
        public long ManagerID { get; set; }
        public long DealerID { get; set; }
        public long? ratecard_id { get; set; }
    }
}