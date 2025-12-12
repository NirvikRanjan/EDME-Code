using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using MySql.Data.EntityFramework;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace AutoSherpa_project.Models
{
    [Table("cpa_code")]
    public class cpa_code
    {
        public long id { get; set; }
        public string CODE { get; set; }
        public string TEXT { get; set; }
    }
}