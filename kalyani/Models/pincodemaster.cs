using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Web.Mvc;

namespace AutoSherpa_project.Models
{
    [Table("pincodemaster")]
    public class pincodemaster
    {
        public long id { get; set; }

        public long PIN_Id { get; set; }

        public long PIN_Code { get; set; }
        public long fkState_Id { get; set; }

        public long fkCity_Id { get; set; }

    }
}