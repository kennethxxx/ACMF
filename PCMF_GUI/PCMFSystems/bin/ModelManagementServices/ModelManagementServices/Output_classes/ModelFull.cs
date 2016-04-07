using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ModelManagementServices
{
    public class ModelFull
    {
        public ServiceBrokerServices.Model model { get; set; }
        public List<string> vMachineList { get; set; }
        public List<string> equipmentList { get; set; }
    }
}