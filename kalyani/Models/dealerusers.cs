using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoSherpa_project.Models
{
    [Table("dealerusers")]
    public class dealerusers
    {
        [Key]
        public int DealerUsersID { get; set; }
        public long? DealerID { get; set; }
        public long? WyzuserID { get; set; }
        public bool? isActive { get; set; }

        public int? iccode { get; set; }
        public int? ishocre { get; set; }
    }
}