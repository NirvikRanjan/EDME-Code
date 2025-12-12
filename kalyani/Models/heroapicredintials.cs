using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoSherpa_project.Models
{
    [Table("heroapicredintials")]
    public class heroapicredintials
    {
        [Key]
        public int id { get; set; }
        public string apiname { get; set; }
        public string apiurl { get; set; }
        public string apikey { get; set; }

    }
}