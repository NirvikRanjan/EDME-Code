using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoSherpa_project.Models
{
    [Table("oem_ic")]
    public class OEMInsuranceCompany
    {
        [Key]
        public long id { get; set; }
        public long product_id { get; set; }
        public string companyName { get; set; }
        public int fksuboem_id { get; set; }
        public string delearCode { get; set; }
    }
}