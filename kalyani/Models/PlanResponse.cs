using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoSherpa_project.Models
{
    [Table("plan_response")]
    public class PlanResponse
    {
        [Key]
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public long Vehicleid { get; set; }
        public string Package { get; set; }
        public string SAOD { get; set; }
        public string SATP { get; set; }
        public string OEMID { get; set; }
        public string PreviousPolicyNo { get; set; }
        public string CPCode { get; set; }
        public bool IsActive { get; set; }
        public bool STATUS { get; set; }
    }
}