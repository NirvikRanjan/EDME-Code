using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoSherpa_project.Models
{
    [Table("batchend")]
    public class batchend
    {
        [Key]
        public int id { get; set; }
        public string endDate { get; set; }
        public string endTime { get; set; }
        public long? tolimit { get; set; }
        public int startbatchId { get; set; }
    }
}