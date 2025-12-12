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
    [Table("ncb_master")]
    public class ncb_master
    {
        public long id { get; set; }
        public int fkProduct_Id { get; set; }

        public string Vehicle_Class { get; set; }

        public int NCB_Years { get; set; }

        public int NCB_Per { get; set; }
    }
}