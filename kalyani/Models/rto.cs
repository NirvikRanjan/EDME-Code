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
    [Table("rto")]
    public class rto
    {
        public long id { get; set; }

        public long RTO_Id { get; set; }

        public string Name { get; set; }

        public long fkState_Id { get; set; }
        public long fkCity_Id { get; set; }
    }
}