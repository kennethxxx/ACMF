using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace OMC.Comm
{
    /// <summary>
    /// 製程工程師Step1,Get Status of Machines
    /// </summary>
    public class PE_GetStatusofMachines
    {
        public string vMachine_Name { get; set; }
        public string Machine_Name { get; set; }
        public string Machine_Type { get; set; }
        public string Machine_Status { get; set; }

        public PE_GetStatusofMachines(string vmachine_name, string machine_name, string machine_type, string machine_status)
        {
            vMachine_Name = vmachine_name;
            Machine_Name = machine_name;
            Machine_Type = machine_type;
            Machine_Status = machine_status;
        }
    }

    /// <summary>
    /// 製程工程師Step2選擇參數4：Rule
    /// </summary>
    public class PE_SelectRules
    {
        public bool Rule_Select { get; set; }
        public string TB_Rule_ID { get; set; }
        public string TB_Rule_Name { get; set; }
        public string TB_Rule_Description { get; set; }
        public string TB_Rule_SWRL { get; set; }
    }

    /// <summary>
    /// 製程工程師Step3選擇偏好機台
    /// </summary>
    public class PE_SelectMachineToSimulation
    {
        public bool Machine_Select { get; set; }
        public string Machine_Name { get; set; }
        public string Machine_Number_of_axes { get; set; }
        public string Machine_Status { get; set; }
        public string Company_Name { get; set; }
        public string Service_Broker { get; set; }
        public string vMachine { get; set; }
        public string Machine_Type { get; set; }
        public string Machine_CNC { get; set; }
    }

    /// <summary>
    /// 製程工程師Step6選擇偏好機台作實際機台加工
    /// </summary>
    public class PE_SelectMachineToCutting
    {
        public bool Machine_Select { get; set; }
        public string Machine_Type { get; set; }
        public string Machine_Name { get; set; }
        public string CNCID { get; set; }
        public string Company_Name { get; set; }
        public string Service_Broker { get; set; }
        public string vMachine { get; set; }
        //public string BlobURL { get; set; }
    }


    /* Scenario 2
     * 特定公司下目前正在執行的cnc
     */
    /*
    public class PE2_CompanyMachineInfomation
    {
        public string CompanyName { get; set; }
        public string ServiceBroker { get; set; }
        public string vMachine { get; set; }
        public string CNCName { get; set; }

        public PE2_CompanyMachineInfomation(string companyname, string servicebroker, string vmachine, string cncname)
        {
            CompanyName = companyname;
            ServiceBroker = servicebroker;
            vMachine = vmachine;
            CNCName = cncname;
        }
    }
    */

    /* Scenario 2
     * PE_抓取DataGrid的值
     */

    public class PE2_ToolSelection
    {
        public bool Tool_Select { get; set; }
        public string A_kind_of_Tool { get; set; }
        public string Tool_of_Diameter { get; set; }
        public string NC_File_Name { get; set; }
        public string BLOB_Address { get; set; }
        public string NC_Descriptions { get; set; }
    }

    /* Scenario 2
     * PE_抓取DataGrid的值
     */

    public class PE2_ToolSelectionToCutting
    {
        public string CNC_Name { get; set; }
        public string ToolNo { get; set; }
        public string ToolNC_Name { get; set; }
    }
}
