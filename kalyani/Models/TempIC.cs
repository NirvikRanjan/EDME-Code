using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoSherpa_project.Models
{
    [Table("tempic")]

    public class TempIC
    {
        [Key]
        public long TempICID { get; set; }
        /*public long IcCodeID { get; set; }
        public long IcCode { get; set; }
        public long icproduct_id { get; set; }*/
        public long fksuboemid { get; set; }
        public long icproduct_id { get; set; }
    }
}