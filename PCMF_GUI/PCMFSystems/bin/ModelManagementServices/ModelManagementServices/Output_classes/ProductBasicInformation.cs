using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ModelManagementServices
{
    public class ProductBasicInformation //呈現給gui使用者選取SB.得到的資訊來找Model
    {
        public string ServiceBrokerID { get; set; }
        public string vMachineID { get; set; }
        public string CNCID { get; set; }
        public string CNCType { get; set; }
        public string ProductID { get; set; }            
        public string ProductType { get; set; }
        public string ActionCount { get; set; }
        public string XTableName { get; set; }
        public string YBlockTableName { get; set; }

        
 
     
     
    }
}