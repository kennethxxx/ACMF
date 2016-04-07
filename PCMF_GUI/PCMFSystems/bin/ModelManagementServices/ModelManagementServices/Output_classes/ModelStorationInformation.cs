using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



// MCS Store model by this class
namespace ModelManagementServices
{
    public class ModelStorationInformation  //存使用者建的模 建完儲存
    {
        public string PK { get; set; }
        public string ServiceBrokerID { get; set; }
        public string COMPANY { get; set; }
        public DateTime createTime { get; set; }
        public string vMachineID { get; set; }
        public string CNCType { get; set; }
        public string cnc_number { get; set; } // Categorydef_cnc_number                    (input by user)
        public string ProductID { get; set; }
        public DateTime dataStartTime { get; set; }
        public DateTime dataEndTime { get; set; }
        public string createUser { get; set; }   
        public double[] matSizeList { get; set; }
        //public string[] modelNameList { get; set; }
        public double modelSize { get; set; }
     
        // IndicatorSpec_LCL    (input by user)
    }
}