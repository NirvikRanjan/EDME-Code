using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoSherpa_project.Models
{
    [Table("soldNewCustomerVehicles")]
    public class soldNewCustomerVehicles
    {
        [Key]
        public long Id { get; set; }
        public long custmerId { get; set; }
        public string customerFName { get; set; }
        public string customerLName { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string vehicleRegNo { get; set; }
        public string chassisNo { get; set; }
        public string engineNo { get; set; }
        public string variant { get; set; }
        public string model { get; set; }
        public string dealershipName { get; set; }
        public DateTime? saleDate { get; set; }
        public string phoneList { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string pincode { get; set; }
        public string initial { get; set; }
        public long wyzuserid { get; set; }

    }
}