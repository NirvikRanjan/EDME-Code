using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoSherpa_project.Models
{
    [Table("tempchangeassignmentcrelist")]
    public class tempChangeAssignmentCrelist
    {
        [Key]
        public long CrelistID { get; set; }
        public long wyzuserId { get; set; }
        public long ManagerID { get; set; }
        public long DealerID { get; set; }
        public string DealerCode { get; set; }
    }
}