using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ModelManagementServices
{
    public class Model_SendContent //呈現給gui使用者選取SB.得到的資訊來找Model
    {
        public string PK { get; set; }
        public string vMachineID { get; set; }
        public string CNCID { get; set; }
        public string ServiceBrokerID { get; set; }
        public string Company { get; set; }

    }
}