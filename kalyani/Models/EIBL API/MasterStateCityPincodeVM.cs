using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoSherpa_project.Models.EIBL_API
{
    public class MasterStateCityPincodeVM
    {
        public List<States> StateList { get; set; }
        public List<Cities> CityList { get; set; }
        public List<Pincodes> PIN_CODE_List { get; set; }
    }

    public class States 
    {
        public int STATE_ID { get; set; }
        public string STATE_NAME { get; set; }
        public string STATE_CODE { get; set; }
    }

    public class Cities
    {
        public long CITY_ID { get; set; }
        public string CITY_NAME { get; set; }
        public string CITY_CODE { get; set; }
        public int FKSTATE_ID { get; set; }
    }

    public class Pincodes
    {
        public string PINCODE { get; set; }
        public int FKSTATE_ID { get; set; }
    }

}