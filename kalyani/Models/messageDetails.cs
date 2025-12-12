using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoSherpa_project.Models
{
    [Table("messagedetails")]
    public class messageDetails
    {
        [Key]
        public int id { get; set; }
        public string message { get; set; }
        public bool isactive { get; set; }
        public string updatedby { get; set; }
        public DateTime? updatedOn { get; set; }
    }
}