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
    [Table("financier")]
    public class Financier
    {
        public long id { get; set; }

        public int Financer_Id { get; set; }


        public string Financer_Code { get; set; }

        public string Name { get; set; }


        public string Add1 { get; set; }

        public int? PIN { get; set; }
    }
}