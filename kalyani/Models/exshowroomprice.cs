using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using MySql.Data.EntityFramework;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutoSherpa_project.Models
{
    [Table("exshowroomprice")]
    public class exshowroomprice
    {
        public long id1 { get; set; }
        public long id { get; set; }
        public long fkModel_Id { get; set; }
        public long ExShowroom_Price_New_Veh { get; set; }
        public long ExShowroom_Price_Old_Veh { get; set; }

        public long Range_Factor_Per_New_Veh { get; set; }
        public long Range_Factor_Per_Old_Veh { get; set; }

        public long Max_Allowed_Price_New_Veh { get; set; }
        public long Max_Allowed_Price_Old_Veh { get; set; }
        public long Min_Allowed_Price_New_Veh { get; set; }

        public long Min_Allowed_Price_Old_Veh { get; set; }

        public long Max_CSD_Disc_Per { get; set; }
        public long Min_CSD_Show_Price { get; set; }

        public long IsActive { get; set; }

        public DateTime Effective_date_From { get; set; }

        public DateTime Effective_date_To { get; set; }

        public long Created_By { get; set; }
        public DateTime Created_Date { get; set; }
        public string Created_Proc_Name { get; set; }
        public string Created_MachineIp { get; set; }
        public string Created_Conn_Id { get; set; }
        public string Created_App_Name { get; set; }

        public DateTime Updated_By { get; set; }
        public DateTime Updated_Date { get; set; }
        public string Updated_Proc_Name { get; set; }


        public string Updated_MachineIP { get; set; }

        public string Updated_Conn_Id { get; set; }

        public string Updated_App_Name { get; set; }

        public string IsDeleted { get; set; }
    }
}