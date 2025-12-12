using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Web.Mvc;

namespace AutoSherpa_project.Models
{
    [Table("state_master")]
    public class state_master
    {
        public long id { get; set; }

        public long State_Id { get; set; }

        public string State_Code { get; set; }
        public string State_Name { get; set; }
    }
}