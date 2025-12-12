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
    [Table("tarrifplans")]
    public class tarrifplans
    {
        public long id { get; set; }
         public string iccode { get; set; }

     public string discount { get; set; }
    }
}