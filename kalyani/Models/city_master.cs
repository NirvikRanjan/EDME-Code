using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Web.Mvc;

namespace AutoSherpa_project.Models
{
    [Table("city_master")]
    public class city_master
    {
        [Key]
        public long id { get; set; }
        public string city_name { get; set; }
        public long fkState_Id { get; set; }
    }
}