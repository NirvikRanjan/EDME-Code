using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoSherpa_project.Models
{
    [Table("batchstart")]
    public class batchstart
    {
        [Key]
        public int id { get; set; }
        public string startedDate { get; set; }
        public string startedTime { get; set; }
        public long? fromlimit { get; set; }
    }
}