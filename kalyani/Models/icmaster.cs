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
    [Table("icmaster")]
    public class icmaster
    {
        public long id { get; set; }
        public int Insur_Comp_ID { get; set; }

     public string   Company_Name { get; set; }
    }
}