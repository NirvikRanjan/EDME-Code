using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoSherpa_project.Models
{
    [Table("tempchangeassignmentyearlist")]
    public class tempChangeAssignmentYearList
    {
        [Key]
        public long YearListID { get; set; }
        public long SaleYear { get; set; }
        public long ManagerID { get; set; }
        public long DealerID { get; set; }
    }
}