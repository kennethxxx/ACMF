using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using WorkerRoleController.StatisticAndMonitoringLayer;
using WorkerRoleController.PrivateWRConfigFileParser;
using System.Collections.Generic;
using System.Web.Services;
using AppUtil;
using VimApi;


namespace WorkerRoleController.WRScalingModule
{
    public class PrivateWRScaling
    {
        private string xmlConfigurationSource;
        private PrivateAutoScalingConfiguration wRScalingXmlConfig;
        private DeploymentWorker deploymentWorker;
        private DeploymentInfo deploymentInformation;

        //private static readonly string subscriberID = "2561364b-5b73-42a7-86aa-25029bc520af";

        public void PrivateWR_Scaling(int nextInstanceCount, PrivateAutoScalingConfiguration wRScalingXmlConfig)
        {
            deploymentWorker = GetDeploymentWorkerInfo(wRScalingXmlConfig);

            if (!deploymentWorker.Equals(null) && !deploymentInformation.Equals(null))
            {
                System.Diagnostics.Debug.WriteLine("WR_ScalingStart----" + DateTime.Now);

                int nowInstanceCount = 3;
                System.Diagnostics.Debug.WriteLine("nextInstanceCount:" + nextInstanceCount.ToString() + " - nowInstanceCount:" + nowInstanceCount.ToString());
                string startTime;
                if (nextInstanceCount >= nowInstanceCount && nextInstanceCount != 0)    //如果需求worker數比現有worker數多
                {
                    //DBConnect dbconnect = new DBConnect("tsp2014expr");
                    startTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    deploymentWorker.TurnOnWR();
                    System.Diagnostics.Debug.WriteLine("ADD----" + DateTime.Now);
                    //dbconnect.InsertWCRActionStatus("AddWRInstance", startTime);
                }
                else
                {
                    //DBConnect dbconnect = new DBConnect("tsp2014expr");
                    startTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    deploymentWorker.SuspendWR();
                    System.Diagnostics.Debug.WriteLine("DELETE----" + DateTime.Now);
                    //dbconnect.InsertWCRActionStatus("DelWRInstance", startTime);
                }

                System.Diagnostics.Debug.WriteLine("WR_ScalingEnd----" + DateTime.Now);
            }
        }

        public PrivateAutoScalingConfiguration GetPrivateWRScalingConfigXmlFile()
        {
            try
            {
                xmlConfigurationSource = Utils.GetConfigurations(4);
                PrivateWrcConfigXmlFileParser.ParseXml(xmlConfigurationSource);
                //System.Diagnostics.Debug.WriteLine(WrcConfigXmlFileParser.ParseXml(xmlConfigurationSource));
                this.wRScalingXmlConfig = PrivateWrcConfigXmlFileParser.ParsedConfig;
                return wRScalingXmlConfig;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.Message);
            }
            return null;
        }

        private DeploymentWorker GetDeploymentWorkerInfo(PrivateAutoScalingConfiguration PrivatewRScalingXmlConfig)
        {
            deploymentInformation = new DeploymentInfo();
            foreach (SubscriptionInfo subAux in PrivatewRScalingXmlConfig.Subscriptions)
            {
                deploymentInformation.SubscriptionInfo = subAux;
                foreach (MySQLInfo databaseAux in subAux.Databases)
                {
                    deploymentInformation.Databases = databaseAux;
                }
                foreach (VcenterInfo VcenterInfoAux in subAux.Vcenter)
                {
                    deploymentInformation.VcenterInfo = VcenterInfoAux;
                    try
                    {
                        foreach (RoleInfo role in VcenterInfoAux.Roles)
                        {
                            deploymentInformation.RoleInfo = role;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            deploymentWorker = new DeploymentWorker(deploymentInformation);
            return deploymentWorker;
        }

        public struct DeploymentInfo
        {
            private MySQLInfo databases;
            private SubscriptionInfo subscriptionInfo;

            private VcenterInfo vcenterInfo;
            private RoleInfo roleInfo;
            private String code;

            public RoleInfo RoleInfo
            {
                set
                {
                    this.roleInfo = value;
                }
                get
                {
                    return this.roleInfo;
                }
            }

            public MySQLInfo Databases
            {
                set
                {
                    this.databases = value;
                }
                get
                {
                    return this.databases;
                }
            }

            public SubscriptionInfo SubscriptionInfo
            {
                set
                {
                    this.subscriptionInfo = value;
                }
                get
                {
                    return this.subscriptionInfo;
                }
            }

            public VcenterInfo VcenterInfo
            {
                set
                {
                    this.vcenterInfo = value;
                }
                get
                {
                    return vcenterInfo;
                }
            }

            public String Code
            {
                set
                {
                    this.Code = value;
                }
                get
                {
                    return Code;
                }
            }
        }

        public class DeploymentWorker
        {
            private readonly DeploymentInfo deploymentInfo;

            private static AppUtil.AppUtil cb = null;

            public DeploymentWorker(DeploymentInfo DeploymentInfo)
            {
                this.deploymentInfo = DeploymentInfo;
            }

            #region 開啟Instance
            public void TurnOnWR()
            {
                ReadyforPowerOn(deploymentInfo);
                cb.connect();
                PowerOps();
                cb.disConnect();
            }
            #endregion

            #region 暫停Instance
            public void SuspendWR()
            {
                ReadyforSuspend(deploymentInfo);
                cb.connect();
                PowerOps();
                cb.disConnect();
            }
            #endregion

            public static OptionSpec[] PowerOnConstructOptions()
            {
                OptionSpec[] useroptions = new OptionSpec[3];
                useroptions[0] = new OptionSpec("vmname", "String", 1
                                                , "Name of Virtual Machine"
                                                , null);
                useroptions[1] = new OptionSpec("operation", "String", 1
                                                , "Operation [on|off|suspend|reset|rebootGuest]"
                                                , null);
                useroptions[2] = new OptionSpec("hostname", "String", 1
                                                , "Name of the host system"
                                                , null);
                return useroptions;
            }

            private static void ReadyforPowerOn(DeploymentInfo deploymentInfo)
            {
                
                //             UCVMPowerOPT obj = new UCVMPowerOPT();
                //             obj1 = obj;
                string[] args;

                string url0 = "--url";
                string url1 = @"https://" + deploymentInfo.VcenterInfo.vIP + "/sdk" + "/sdk";

                string userN0 = "--username";
                string userN1 = deploymentInfo.VcenterInfo.vaccount;

                string pass0 = "--password";
                string pass1 = deploymentInfo.VcenterInfo.vpassword;

                string vmN0 = "--vmname";
                string vmN1 = "boyi_Worker1";                    //虛擬機的開機是以名字為主(可能到時候要用個array存取所有worker的名字)

                string popt0 = "--operation";
                string popt1 = "on";
                string hostN0 = "--hostname";
                string hostN1 = deploymentInfo.VcenterInfo.vIP;//可以不填

                args = new string[] { url0, url1, userN0, userN1, pass0, pass1, vmN0, vmN1, popt0, popt1, hostN0, hostN1 };

                cb = AppUtil.AppUtil.initialize("VmPowerOps", PowerOnConstructOptions(), args);

            }

            private static void ReadyforSuspend(DeploymentInfo deploymentInfo)
            {
                //             UCVMPowerOPT obj = new UCVMPowerOPT();
                //             obj1 = obj;
                string[] args;

                string url0 = "--url";
                string url1 = @"https://" + deploymentInfo.VcenterInfo.vIP + "/sdk" + "/sdk";

                string userN0 = "--username";
                string userN1 = deploymentInfo.VcenterInfo.vaccount;

                string pass0 = "--password";
                string pass1 = deploymentInfo.VcenterInfo.vpassword;

                string vmN0 = "--vmname";
                string vmN1 = "boyiliTest4";

                string popt0 = "--operation";
                string popt1 = "suspend";
                string hostN0 = "--hostname";
                string hostN1 = deploymentInfo.VcenterInfo.vIP;//可以不填

                args = new string[] { url0, url1, userN0, userN1, pass0, pass1, vmN0, vmN1, popt0, popt1, hostN0, hostN1 };

                cb = AppUtil.AppUtil.initialize("VmPowerOps", PowerOnConstructOptions(), args);

            }

            public static void PowerOps()
            {              
                try
                {
                    String powerOnHostName = cb.get_option("hostname");
                    String vmName = cb.get_option("vmname");
                    String powerOperation = cb.get_option("operation");
                    //ArrayList morlist = null;                   
                    VimApi.ManagedObjectReference vmmor = null;
                    String errmsg = "";
                    vmmor = cb.getServiceUtil().GetDecendentMoRef(null,
                                                                  "VirtualMachine",
                                                                  vmName);
                    if (vmmor == null)
                    {

                        errmsg = "Unable to find VirtualMachine named : "
                                 + cb.get_option("vmname")
                                 + " in Inventory";

                    }

                    //TODO: find required host.            
                    //ManagedObjectReference hostmor = cb.getServiceUtil().GetDecendentMoRef(null, "HostSystem", powerOnHostName);
                    VimApi.ManagedObjectReference hostmor = cb.getServiceUtil().GetFirstDecendentMoRef(null, "HostSystem");
                    bool nonTaskOp = false;
                    ManagedObjectReference taskmor = null;

                    if (powerOperation.Equals("shutdownGuest"))
                    {
                        cb.getConnection().Service.ShutdownGuest(vmmor);
                        nonTaskOp = true;

                    }
                    else if (powerOperation.Equals("suspend"))
                    {
                        taskmor = cb.getConnection().Service.SuspendVM_Task(vmmor);
                    }
                    else if (powerOperation.Equals("off"))
                    {
                        taskmor = cb.getConnection().Service.PowerOffVM_Task(vmmor);
                    }
                    else if (powerOperation.Equals("on"))
                    {
                        taskmor = cb.getConnection().Service.PowerOnVM_Task(vmmor, hostmor);
                    }
                    else
                    {
                        throw new Exception("Invalid power operation : " + powerOperation);
                    }

                    // If we get a valid task reference, monitor the task for success or failure
                    // and report task completion or failure.
                    if (taskmor != null)
                    {

                        cb.log.LogLine("Got Valid Task Reference");

                        object[] result = cb.getServiceUtil().WaitForValues
                           (taskmor, new string[] { "info.state", "info.error" },
                              new string[] { "state" }, // info has a property - state for state of the task
                              new object[][] { new object[] { VimApi.TaskInfoState.success, VimApi.TaskInfoState.error } }
                           );

                        // Wait till the task completes.
                        if (result[0].Equals(VimApi.TaskInfoState.success))
                        {
                            cb.log.LogLine("VmPowerOps : : Successful " + powerOperation
                                           + " for VM : "
                                           + vmName);
                        }
                        else
                        {
                            cb.log.LogLine("VmPowerOps : Failed " + powerOperation
                                           + " for VM : "
                                           + vmName);
                        }
                    }
                    else if (nonTaskOp)
                    {
                        cb.log.LogLine("VmPowerOps : Successful " + powerOperation
                                   + " for VM : "
                                   + vmName);
                    }
                } // end of try
                catch (Exception e)
                {
                    //MessageBox.Show("VmPowerOps : Failed " + powerOperation + " for VM : " + vmName + e, "Error Information");
                    cb.log.LogLine("VmPowerOps : Failed ");
                    throw e;

                }

            }
        }
    }
}
