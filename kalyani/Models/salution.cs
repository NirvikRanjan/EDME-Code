using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Web.Mvc;

namespace AutoSherpa_project.Models
{
    [Table("salution")]
    public class salution
    {
        public long id { get; set; }
        public string name { get; set; }
    }
}