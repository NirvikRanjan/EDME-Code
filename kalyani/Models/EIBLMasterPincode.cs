using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoSherpa_project.Models
{
    [Table("EIBL_MasterPincode")]
    public class EIBLMasterPincode
    {
        public long Id { get; set; }
        public string PINCODE { get; set; }
        public int FKSTATE_ID { get; set; }
    }
}