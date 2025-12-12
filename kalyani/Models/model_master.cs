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
    [Table("model_master")]
    public class model_master
    {
        public long id { get; set; }

        public int Model_Id { get; set; }

        public string Model_Name { get; set; }
        public long fkManufacturer_Id { get; set; }

        public long Cubic_Capacity { get; set; }

        public string Old_Model_Code { get; set; }
        public long Seating_capacity { get; set; }
    }
}