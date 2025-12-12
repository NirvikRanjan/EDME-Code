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
    [Table("income_level")]
    public class income_level
    {
        public long id2 { get; set; }
        public string Id { get; set; }
        public string name { get; set; }

        public long id1 { get; set; }
    }
}