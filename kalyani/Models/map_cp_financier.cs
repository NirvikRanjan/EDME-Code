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
    [Table("map_cp_financier")]
    public class map_cp_financier
    {
        public long id { get; set; }

        public long Financer_Id { get; set; }

        public string Financer_Code { get; set; }

        public string Name { get; set; }

        public string Add1 { get; set; }

        public string Add2 { get; set; }


        public long fkState_Id { get; set; }

        public long fkCity_Id { get; set; }
    }
}