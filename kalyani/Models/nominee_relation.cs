using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoSherpa_project.Models
{
    [Table("nominee_relation")]
    public class nominee_relation
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string Nominee_Type { get; set; }
    }
}