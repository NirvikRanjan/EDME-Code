using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoSherpa_project.Models
{
    [Table("EIBL_MasterState")]
    public class EIBLMasterState
    {
        [Key]
        public long Id { get; set; }
        public int STATE_ID { get; set; }
        public string STATE_NAME { get; set; }
        public string STATE_CODE { get; set; }
    }
}