using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoSherpa_project.Models
{
    [Table("oem_dealer")]
    public class OEM_Dealer
    {
        public int id { get; set; }

        public int OEM_Id { get; set; }

        public int dealerID { get; set; }        
        public string dealerCode { get; set; }        
    }
}