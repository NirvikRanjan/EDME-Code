using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoSherpa_project.Models
{
    [Table("priority")]
    public class priority
    {
        [Key]
            public int id { get;set;}
            public string name { get;set;}
            public bool isactive { get;set;}
    }
}