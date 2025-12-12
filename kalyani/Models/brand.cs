using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoSherpa_project.Models
{
    [Table("brand")]
    public class brand
    {
        public int id { get; set; }
        public string brandname { get; set; }
        public int? fksuboem_id { get; set; }
        public bool isactive { get; set; }
        public int? brandoemid { get; set; }
    }
}