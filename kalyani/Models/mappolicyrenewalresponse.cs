using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoSherpa_project.Models
{
    [Table("mappolicyrenewalresponse")]
    public class mappolicyrenewalresponse
    {
        public long id { get; set; }
        public long policyrenewalresponseid { get; set; }
        public long insurancerenewalpolicyid { get; set; }

    }
}