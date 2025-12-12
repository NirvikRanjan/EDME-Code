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
    [Table("id_proof")]
    public class id_proofs
    {
        public long id1 { get; set; }
        public string Id { get; set; }
        public string name { get; set; }
    }
}