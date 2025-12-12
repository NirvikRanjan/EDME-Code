using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoSherpa_project.Models
{
    [Table("tempDealers")]
    public class tempDealers
    {
        [Key]
        public long tempDealersID { get; set; }
        public long DealerID { get; set; }
        public string DealerCode { get; set; }
    }
}