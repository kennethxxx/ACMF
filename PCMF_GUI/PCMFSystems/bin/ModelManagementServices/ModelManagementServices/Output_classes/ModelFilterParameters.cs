using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// Cloud GUI filter model by this parameters
namespace ModelManagementServices
{
    public class ModelFilterParameters   //挑Fan out要用的Model
    {
        public List<string> ServiceBrokerID { get; set; }
        public List<string> vMachineID { get; set; }
        public DateTime modelStartDate { get; set; }
        public DateTime modelEndDate { get; set; }
        public List<string> cnc_number { get; set; }
        public List<string> ProductID { get; set; }
    }

}