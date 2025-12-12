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
    [Table("businesstype")]
    public class businesstype
    {
        public long id1 { get; set; }
        public string id { get; set; }
        public string name { get; set; }
    }
}