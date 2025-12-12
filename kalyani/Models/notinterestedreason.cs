using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoSherpa_project.Models
{
    [Table("notinterestedreason")]
    public class notinterestedreason
    {
        public int id { get; set; }
        public string reasons { get; set; }
        public bool isactive { get; set; }

    }
}