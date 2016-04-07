using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// get model List (in blob) by this informaiton
namespace ModelManagementServices
{
    public class ModelInformation  //呈現給gui使用者選取的PS.查詢模用
    {
        public string PK { get; set; }
        public string COMPANY { get; set; }
        public DateTime createTime { get; set; }
        public string vMachineID { get; set; }
        public string CNCType { get; set; }
        public string cnc_number { get; set; }             // Categorydef_cnc_number                    (input by user)
        public string ModelID { get; set; }
        public string ProductID { get; set; } 
        public DateTime dataStartTime { get; set; }
        public DateTime dataEndTime { get; set; }
        public string createUser { get; set; }         
        public double[] matSizeList { get; set; }
        public string[] modelNameList { get; set; }
        public double modelSize { get; set; }
        public string ServiceBrokerID { get; set; }
      
    }
}