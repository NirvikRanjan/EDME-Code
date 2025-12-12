using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoSherpa_project.Models
{
    [Table("marketingcampaign")]
    public class marketingcampaign
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public DateTime? createdDate { get; set; }
        public bool isactive { get; set; }
    }
}