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
    [Table("oem_master")]
    public class oem_master
    {
        public long id { get; set; }

        public int OEM_Id { get; set; }

        public string OEM_Name { get; set; }
        public string OEM_Type_Code { get; set; }
    }
}