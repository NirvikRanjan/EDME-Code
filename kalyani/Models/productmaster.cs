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
    [Table("productmaster")]
    public class productmaster
    {
        public long id { get; set; }

        public int Product_id { get; set; }

        public string Product_Name { get; set; }
    }
}