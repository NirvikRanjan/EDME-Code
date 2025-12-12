using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoSherpa_project.Models
{
    [Table("EIBL_MasterCity")]
    public class EIBLMasterCity
    {
        [Key]
        public long Id { get; set; }
        public long CITY_ID { get; set; }
        public string CITY_NAME { get; set; }
        public string CITY_CODE { get; set; }
        public int FKSTATE_ID { get; set; }
    }
}