using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoSherpa_project.Models
{
    [Table("herodealer")]
    public class herodealer
    {
        [Key]
    public int DealerID{get;set;}
    public string DealerName { get; set; }
    public string DisplayName { get; set; }
    public string DealerCode { get; set; }
    public string DealerClassification { get; set; }
    public string DealerState { get; set; }
    public string DealerZone { get; set; }
    public string DealerCity { get; set; }
    public int? DealerFlag { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string CreatedBy { get; set; }
    public int? DealerHO { get; set; }
    public bool IsActive { get; set; }
    public string AllocationFlag { get; set; }
        public IEnumerable<wyzuser> wyzusers { get; set; }
    }

    
}