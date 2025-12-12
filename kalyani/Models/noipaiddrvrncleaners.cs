using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using MySql.Data.EntityFramework;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutoSherpa_project.Models
{
    [Table("noipaiddrvrncleaners")]
    public class noipaiddrvrncleaners
    {
        public long id1 { get; set; }

        public long id { get; set; }

        public long fkProduct_Id { get; set; }

        public string Vehicle_Class { get; set; }

        public long Cover_Amt { get; set; }
        public long Premium_Amt { get; set; }
        public string Liability_Type { get; set; }

    }
}