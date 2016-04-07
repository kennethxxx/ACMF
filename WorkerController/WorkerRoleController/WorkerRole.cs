using System;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure.ServiceRuntime;
using WorkerRoleController.WRInstanceEvaluator;
using WorkerRoleController.WrcConfigFileParser;
using WorkerRoleController.WRScalingModule;
using WorkerRoleController.FileTransferation;
using WorkerRoleController.DriverOptionParser;
using WorkerRoleController.PrivateWRConfigFileParser;

namespace WorkerRoleController
{
    public class WorkerRole : RoleEntryPoint
    {
        private bool firstTime;
        private int nextInstanceCount;
        private AutoScalingConfiguration wRScalingXmlConfig;
        private PrivateAutoScalingConfiguration PrivatewRScalingXmlConfig;

        public override void Run()
        {
            firstTime = true;
            int monitoringWRSecond = 0;
            bool stop = false;
            string DriverOption;
            do
            {
                try
                {
                    //WREvaluator wre = new WREvaluator();
                    nextInstanceCount = 2;//Wre.GetVMCount();
                    DriverOptionXmlParser DriverParser = new DriverOptionXmlParser();
                    DriverOption = DriverParser.GetDriverOption();
                    switch (DriverOption)
                    { 
                        case "Public":
                                        WRScaling wrs = new WRScaling();
                                        wRScalingXmlConfig = wrs.GetWRScalingConfigXmlFile();
                                        monitoringWRSecond = wRScalingXmlConfig.WRPeriod.SecondsPerLoop;
                                        if (!monitoringWRSecond.Equals(null) && nextInstanceCount != -1)
                                        {
                                            wrs.WR_Scaling(nextInstanceCount, wRScalingXmlConfig);
                                        }
                                        break;
                        case "Private":
                                        PrivateWRScaling pwrs = new PrivateWRScaling();
                                        PrivatewRScalingXmlConfig = pwrs.GetPrivateWRScalingConfigXmlFile();
                                        monitoringWRSecond = PrivatewRScalingXmlConfig.WRPeriod.SecondsPerLoop;
                                        if (!monitoringWRSecond.Equals(null) && nextInstanceCount != -1)
                                        {
                                            pwrs.PrivateWR_Scaling(nextInstanceCount, PrivatewRScalingXmlConfig);
                                        }
                                        break;
                        default: break;
                    }
                }
                catch (Exception ex)
                {
                    if (!firstTime)
                        System.Diagnostics.Debug.Write(ex.Message);
                }
                try
                {
                    Thread.Sleep(monitoringWRSecond);
                }
                catch (Exception ex)
                {
                    if (!firstTime)
                        System.Diagnostics.Debug.Write(ex.Message);
                }
            } while (!stop);

        }

        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = 12;
            return base.OnStart();
        }
    }
}
