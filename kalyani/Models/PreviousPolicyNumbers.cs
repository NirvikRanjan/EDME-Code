using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoSherpa_project.Models
{
    [Table("previous_policyno")]
    public class PreviousPolicyNumbers
    {
        [Key]
        public long Id { get; set; }
        public string PolicyNo { get; set; }
        public DateTime? FetchedOn { get; set; } = DateTime.Now;
    }
}