using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoSherpa_project.Models
{
    [Table("brandmodel")]
    public class brandmodel
    {
        public int id { get; set; }
        public string modelname { get; set; }
        public string brandname { get; set; }
        public int BrandModelId { get; set; }
        public bool isActive { get; set; }
    }
}