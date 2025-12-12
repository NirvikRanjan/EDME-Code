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
    [Table("map_cp_financier_mapping")]
    public class map_cp_financier_mapping
    {
        public long id { get; set; }
        public long MAP_Id { get; set; }
        public long fkCP_Id { get; set; }
        public long fkFinancer_Id { get; set; }
    }
}