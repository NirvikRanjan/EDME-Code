using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoSherpa_project.Models
{
    [Table("generate_api_token")]
    public class GenerateAPIToken
    {
        public int Id { get; set; }
        public string URL { get; set; }
        public string CLIENT_ID { get; set; }
        public string CLIENT_SECRET { get; set; }
        public bool IsActive { get; set; }
    }
}