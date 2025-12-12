using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoSherpa_project.Models
{
    [Table("hero_ic_tbl")]
    public class hero_ic_tbl
    {
        [Key]
        public int id { get; set; }
        public string companyName { get; set; }
        public long? iccode { get; set; }
        public bool isactive { get; set; }
        public string createdBy { get; set; }
        public DateTime? createdDatecreatedDate { get; set; }
        public string table_name { get; set; }
    }
}