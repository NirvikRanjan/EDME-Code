using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoSherpa_project.Models.EIBL_API
{
    public class ModelVariantVM
    {
        public int FKMODEL_ID { get; set; }
        public string MODEL_CODE { get; set; }
        public int VARIANT_ID { get; set; }
        public string VARIANT_CODE { get; set; }
        public int FKMAKE_ID { get; set; }
        public string Make_Name { get; set; }
    }
}