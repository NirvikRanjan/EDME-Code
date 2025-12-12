using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoSherpa_project.Models
{
    [Table("nomineerelation")]
    public class nomineerelation
    {
        [Key]
        public int id { get; set; }
        public string relationship { get; set; }
        public string gender { get; set; }
    }
}