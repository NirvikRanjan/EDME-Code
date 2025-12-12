using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoSherpa_project.Models
{
    [Table("models")]
    public class models
    {
        public int id { get; set; }
        public string modelnames { get; set; }
        public int brandId { get; set; }
        public bool isActive { get; set; }
    }
}