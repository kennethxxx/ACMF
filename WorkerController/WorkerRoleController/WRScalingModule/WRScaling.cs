using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Samples.WindowsAzure.ServiceManagement;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using WorkerRoleController.StatisticAndMonitoringLayer;
using WorkerRoleController.WrcConfigFileParser;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Auth;

namespace WorkerRoleController.WRScalingModule
{
    public class WRScaling
    {
        private string xmlConfigurationSource;
        private AutoScalingConfiguration wRScalingXmlConfig;
        private DeploymentWorker deploymentWorker;
        private DeploymentPackage package;

        //private static readonly string subscriberID = "2561364b-5b73-42a7-86aa-25029bc520af";

        public void WR_Scaling(int nextInstanceCount, AutoScalingConfiguration wRScalingXmlConfig)
        {
            deploymentWorker = GetDeploymentWorker(wRScalingXmlConfig);

            if (!deploymentWorker.Equals(null) && !package.Equals(null))
            {
                System.Diagnostics.Debug.WriteLine("WR_ScalingStart----" + DateTime.Now);

                IServiceManagement serviceManagement = null;
                string instanceName = package.RoleInfo.Name;
                int nowInstanceCount = deploymentWorker.GetInstanceCount();
                System.Diagnostics.Debug.WriteLine("nextInstanceCount:" + nextInstanceCount.ToString() + " - nowInstanceCount:" + nowInstanceCount.ToString());
                string startTime;
                if (nextInstanceCount > nowInstanceCount && nextInstanceCount != 0)
                {
                    if (nowInstanceCount == 0)
                    {
                        //DBConnect dbconnect = new DBConnect("tsp2014expr");
                        startTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        serviceManagement = ServiceManagementHelper.CreateServiceManagementChannel("WindowsAzureEndPoint", package.Certificate);
                        string resultstate = deploymentWorker.Deploy1StWR(instanceName, serviceManagement, package);
                        System.Diagnostics.Debug.WriteLine("Deploy1StWR----" + DateTime.Now);
                        //dbconnect.InsertWCRActionStatus("Deploy1StWR", startTime);

                        if (resultstate == "Succeeded" && resultstate.Equals("Succeeded"))
                        {
                            startTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            deploymentWorker.AddWRInstance(instanceName, nextInstanceCount, serviceManagement, package);
                            System.Diagnostics.Debug.WriteLine("AddWRInstance----" + DateTime.Now);
                            //dbconnect.InsertWCRActionStatus("AddWRInstance", startTime);
                        }
                    }
                    else
                    {
                        //DBConnect dbconnect = new DBConnect("tsp2014expr");
                        startTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        serviceManagement = ServiceManagementHelper.CreateServiceManagementChannel("WindowsAzureEndPoint", package.Certificate);
                        deploymentWorker.AddWRInstance(instanceName, nextInstanceCount, serviceManagement, package);
                        System.Diagnostics.Debug.WriteLine("ADD----" + DateTime.Now);
                        //dbconnect.InsertWCRActionStatus("AddWRInstance", startTime);
                    }

                }
                else if (nextInstanceCount < nowInstanceCount)
                {
                    //DBConnect dbconnect = new DBConnect("tsp2014expr");
                    startTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    serviceManagement = ServiceManagementHelper.CreateServiceManagementChannel("WindowsAzureEndPoint", package.Certificate);
                    deploymentWorker.DelWRInstance(instanceName, nextInstanceCount, serviceManagement, package);
                    System.Diagnostics.Debug.WriteLine("DELETE----" + DateTime.Now);
                    //dbconnect.InsertWCRActionStatus("DelWRInstance", startTime);
                }

                System.Diagnostics.Debug.WriteLine("WR_ScalingEnd----" + DateTime.Now);
            }
        }

        public AutoScalingConfiguration GetWRScalingConfigXmlFile()
        {
            try
            {
                xmlConfigurationSource = Utils.GetConfigurations(1);
                WrcConfigXmlFileParser.ParseXml(xmlConfigurationSource);
                //System.Diagnostics.Debug.WriteLine(WrcConfigXmlFileParser.ParseXml(xmlConfigurationSource));
                this.wRScalingXmlConfig = WrcConfigXmlFileParser.ParsedConfig;
                return wRScalingXmlConfig;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.Message);
            }
            return null;
        }

        private DeploymentWorker GetDeploymentWorker(AutoScalingConfiguration wRScalingXmlConfig)
        {
            package = new DeploymentPackage();
            DeploymentInfoExtension cPFile = new DeploymentInfoExtension();
            package.Deployment = cPFile;
            package.Certificate = Utils.GetLocalCert();
            foreach (SubscriptionInfo subAux in wRScalingXmlConfig.Subscriptions)
            {
                package.SubscriptionInfo = subAux;
                foreach (StorageInfo storageAux in subAux.Storages)
                {
                    package.Storages = storageAux;
                }
                foreach (HostedServiceInfo hostedServiceAux in subAux.HostedServices)
                {
                    package.HostedService = hostedServiceAux;
                    foreach (DeploymentInfo deploymentAux in hostedServiceAux.Deployments)
                    {
                        package.DeploymentInfo = deploymentAux;
                        try
                        {
                            foreach (RoleInfo role in deploymentAux.Roles)
                            {
                                package.RoleInfo = role;
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
            deploymentWorker = new DeploymentWorker(package);
            return deploymentWorker;
        }

        public class DeploymentInfoExtension : DeploymentInfo
        {
            public String DeploymentConfigurationNameFile { get; set; }

            public String DeploymentPackageNameFile { get; set; }
        }

        public struct DeploymentPackage
        {
            private StorageInfo storages;
            private X509Certificate2 certificate;
            private SubscriptionInfo subscriptionInfo;

            private HostedServiceInfo hostedService;
            private DeploymentInfo deploymentInfo;
            private DeploymentInfoExtension deployment;
            private RoleInfo roleInfo;
            private String code;

            public DeploymentInfo DeploymentInfo
            {
                set
                {
                    this.deploymentInfo = value;
                }
                get
                {
                    return this.deploymentInfo;
                }
            }

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

            public StorageInfo Storages
            {
                set
                {
                    this.storages = value;
                }
                get
                {
                    return this.storages;
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

            public X509Certificate2 Certificate
            {
                set
                {
                    this.certificate = value;
                }
                get
                {
                    return certificate;
                }
            }

            public HostedServiceInfo HostedService
            {
                set
                {
                    this.hostedService = value;
                }
                get
                {
                    return hostedService;
                }
            }

            public DeploymentInfoExtension Deployment
            {
                set
                {
                    this.deployment = value;
                }
                get
                {
                    return deployment;
                }
            }

            public String Code
            {
                set
                {
                    this.code = value;
                }
                get
                {
                    return code;
                }
            }
        }

        public class DeploymentWorker
        {
            private readonly DeploymentPackage package;

            public DeploymentWorker(DeploymentPackage package)
            {
                this.package = package;
            }

            public int GetInstanceCount()
            {
                Deployment deployment;
                int nowInstanceCount = 0;
                try
                {
                    IServiceManagement serviceManagement = ServiceManagementHelper.CreateServiceManagementChannel("WindowsAzureEndPoint", package.Certificate);
                    using (OperationContextScope scope = new OperationContextScope((IContextChannel)serviceManagement))
                    {
                        deployment = null;
                        if (!string.IsNullOrEmpty(package.HostedService.Name) && !string.IsNullOrEmpty(package.DeploymentInfo.Name))
                        {
                            deployment = serviceManagement.GetDeployment(package.SubscriptionInfo.Id, package.HostedService.Name, package.DeploymentInfo.Name);
                        }
                        else if (!string.IsNullOrEmpty(DeploymentSlotType.Production))
                        {
                            deployment = serviceManagement.GetDeploymentBySlot(package.SubscriptionInfo.Id, package.HostedService.Name, DeploymentSlotType.Production);
                        }
                        else if (!string.IsNullOrEmpty(DeploymentSlotType.Staging))
                        {
                            deployment = serviceManagement.GetDeploymentBySlot(package.SubscriptionInfo.Id, package.HostedService.Name, DeploymentSlotType.Staging);
                        }
                    }

                    if (!deployment.Equals(null))
                    {
                        StringReader sReader = new StringReader(ServiceManagementHelper.DecodeFromBase64String(deployment.Configuration));
                        XmlDocument doc = new XmlDocument();
                        doc.Load(sReader);
                        XmlElement xmlRootElement = doc.DocumentElement;

                        try
                        {
                            foreach (XmlNode child in xmlRootElement.ChildNodes)
                            {
                                if (child.Name.Equals(DeploymentConfigurationXmlInfo.ElementRole) &&
                                    child.Attributes[DeploymentConfigurationXmlInfo.AttributeName] != null &&
                                    child.Attributes[DeploymentConfigurationXmlInfo.AttributeName].Value.Equals(package.RoleInfo.Name))
                                {
                                    foreach (XmlNode subChild in child.ChildNodes)
                                    {
                                        if (subChild.Name.Equals(DeploymentConfigurationXmlInfo.ElementInstances) &&
                                            subChild.Attributes[DeploymentConfigurationXmlInfo.AttributeInstancesCount] != null &&
                                            !String.IsNullOrEmpty(subChild.Attributes[DeploymentConfigurationXmlInfo.AttributeInstancesCount].Value))
                                        {
                                            nowInstanceCount = Int32.Parse(subChild.Attributes[DeploymentConfigurationXmlInfo.AttributeInstancesCount].Value.Trim());
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                        return nowInstanceCount;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Write(ex.Message);
                    return nowInstanceCount;
                }
                return nowInstanceCount;
            }

            #region 佈署Instance
            public string Deploy1StWR(string instanceName, IServiceManagement serviceManagement, DeploymentPackage package)
            {
                //CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("ScalingConfigurationStorageConnectionString"));
                StorageCredentials credentials = new StorageCredentials(package.Storages.Name, package.Storages.Key);
                CloudStorageAccount storageAccount = new CloudStorageAccount(credentials, true);
                CloudBlobClient client = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = client.GetContainerReference(package.DeploymentInfo.StoragePath);

                int startAt = container.Uri.ToString().Length;
                String auxUri;
                foreach (CloudBlockBlob b in container.ListBlobs())
                {
                    auxUri = b.Uri.ToString();
                    if (auxUri.EndsWith(".cscfg"))
                        package.Deployment.DeploymentConfigurationNameFile = auxUri.Substring(startAt);
                    else if (auxUri.EndsWith(".cspkg"))
                        package.Deployment.DeploymentPackageNameFile = auxUri.Substring(startAt);
                }

                String uriPrefix = Utils.GenerateBlobLink(package.DeploymentInfo.Storage.Name, package.DeploymentInfo.StoragePath);
                //String uriPrefix = Utils.GenerateBlobLink("mmdblab", "tomcat-workerrole-vs2010/"); //mmdblab tomcat-workerrole-vs2010/

                string reqId = null;
                using (OperationContextScope scope = new OperationContextScope(serviceManagement as IClientChannel))
                {
                    String cfgPath = uriPrefix + package.Deployment.DeploymentConfigurationNameFile;
                    CreateDeploymentInput input = new CreateDeploymentInput
                    {
                        //Name = "ncku-mmdblab", //"TomcatWorkerRole"
                        Name = package.DeploymentInfo.Name,
                        //Configuration = ServiceManagementHelper.EncodeToBase64String(Utils.GetSettings("tomcat-workerrole-vs2010", cfgPath)),
                        Configuration = ServiceManagementHelper.EncodeToBase64String(Utils.GetSettings((package.DeploymentInfo.StoragePath).Replace("/", ""), cfgPath)),
                        PackageUrl = new Uri(uriPrefix + package.Deployment.DeploymentPackageNameFile),
                        //Label = ServiceManagementHelper.EncodeToBase64String("ncku-mmdblab");
                        Label = ServiceManagementHelper.EncodeToBase64String(package.DeploymentInfo.Name),
                        //Label = Convert.ToBase64String(Encoding.UTF8.GetBytes(package.DeploymentInfo.Name))
                    };
                    serviceManagement.CreateOrUpdateDeployment(package.SubscriptionInfo.Id, package.HostedService.Name, package.RoleInfo.SlotType, input);

                    object propertyValue;
                    if (System.ServiceModel.OperationContext.Current.IncomingMessageProperties.TryGetValue("httpResponse", out propertyValue))
                    {
                        System.ServiceModel.Channels.HttpResponseMessageProperty response = (System.ServiceModel.Channels.HttpResponseMessageProperty)propertyValue;
                        reqId = response.Headers["x-ms-request-id"];
                    }
                }

                while (true)
                {
                    string status = serviceManagement.GetOperationStatus(package.SubscriptionInfo.Id, reqId).Status;
                    System.Diagnostics.Debug.WriteLine("Processing...:" + status);
                    if (status == "Succeeded")
                        break;
                    System.Threading.Thread.Sleep(20000);
                }
                string resultstate = RunDelopment(serviceManagement, package);
                return resultstate;
            }
            #endregion

            #region 增加Instance
            public void AddWRInstance(string instanceName, int vMnum, IServiceManagement serviceManagement, DeploymentPackage package)
            {
                if (!vMnum.Equals(null) && !vMnum.Equals("") && vMnum != -1)
                {
                    this.ChangeInstaceCount(serviceManagement, vMnum, package);
                }
            }
            #endregion

            #region 刪除Instance
            public void DelWRInstance(string instanceName, int vMnum, IServiceManagement serviceManagement, DeploymentPackage package)
            {
                string uristring = "http://140.116.86.249:8080/Tsp2014StatisticMonitoringModule/services/StatisticMonitoringClass?wsdl";
                List<string> methodAndParams = new List<string>();
                methodAndParams.Add("Get_WRJobStatus");
                WRStatisticAndMonitoringLayer callService = new WRStatisticAndMonitoringLayer();
                int onlineUsers = (int)callService.ServiceNoParamsInfoJws(uristring, methodAndParams);
                System.Diagnostics.Debug.WriteLine("onlineUsers:" + onlineUsers.ToString() + " - vMnum:" + vMnum.ToString());
                if (!vMnum.Equals(null) && !vMnum.Equals("") && vMnum != -1 && onlineUsers == 0 && vMnum != 0)
                {
                    string resultState = UpdateDeploymentStatusBySlot(serviceManagement, package);
                    System.Diagnostics.Debug.WriteLine("UpdateDeploymentStatusBySlot...:" + resultState);
                    if (resultState == "Succeeded" && resultState.Equals("Succeeded"))
                    {
                        string resultStateChangeInstaceCount = this.ChangeInstaceCount(serviceManagement, vMnum, package);
                        System.Diagnostics.Debug.WriteLine("ChangeInstaceCount...:" + resultStateChangeInstaceCount);
                        if (resultStateChangeInstaceCount == "Succeeded" && resultStateChangeInstaceCount.Equals("Succeeded"))
                        {
                            string resultstate = RunDelopment(serviceManagement, package);
                            System.Diagnostics.Debug.WriteLine("RunDelopment...:" + resultstate);

                        }
                    }
                }
                else if (onlineUsers == 0 && vMnum == 0)
                {
                    string resultstate = UpdateDeploymentStatusBySlot(serviceManagement, package);
                    if (resultstate == "Succeeded" && resultstate.Equals("Succeeded"))
                    {
                        this.DeleteDelployment(serviceManagement, package);
                    }
                }
            }
            #endregion

            private string ChangeInstaceCount(IServiceManagement serviceManagement, int count, DeploymentPackage package)
            {
                string reqId = string.Empty;
                object propertyValue;
                using (OperationContextScope scope = new OperationContextScope(serviceManagement as IClientChannel))
                {
                    //CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("ScalingConfigurationStorageConnectionString"));
                    StorageCredentials credentials = new StorageCredentials(package.Storages.Name, package.Storages.Key);
                    CloudStorageAccount storageAccount = new CloudStorageAccount(credentials, true);
                    CloudBlobClient client = storageAccount.CreateCloudBlobClient();
                    //CloudBlobContainer container = client.GetContainerReference("tomcat-workerrole-vs2010/"); //tomcat-workerrole-vs2010
                    CloudBlobContainer container = client.GetContainerReference(package.DeploymentInfo.StoragePath); //tomcat-workerrole-vs2010

                    int startAt = container.Uri.ToString().Length;
                    String auxUri;
                    foreach (CloudBlockBlob b in container.ListBlobs())
                    {
                        auxUri = b.Uri.ToString();
                        if (auxUri.EndsWith(".cscfg"))
                            package.Deployment.DeploymentConfigurationNameFile = auxUri.Substring(startAt);
                        else if (auxUri.EndsWith(".cspkg"))
                            package.Deployment.DeploymentPackageNameFile = auxUri.Substring(startAt);
                    }

                    String uriPrefix = Utils.GenerateBlobLink(package.DeploymentInfo.Storage.Name, package.DeploymentInfo.StoragePath); //mmdblab tomcat-workerrole-vs2010/
                    //String uriPrefix = Utils.GenerateBlobLink("mmdblab", "tomcat-workerrole-vs2010/"); //mmdblab tomcat-workerrole-vs2010/
                    String cfgPath = uriPrefix + package.Deployment.DeploymentConfigurationNameFile;

                    //XDocument doc = XDocument.Parse(Utils.GetSettings("tomcat-workerrole-vs2010", cfgPath));
                    XDocument doc = XDocument.Parse(Utils.GetSettings((package.DeploymentInfo.StoragePath).Replace("/", ""), cfgPath));
                    var result = (from s1 in doc.Descendants(XName.Get("Instances", "http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceConfiguration"))
                                  where
                                       s1.Parent.Attribute("name").Value == package.RoleInfo.Name
                                  select s1).FirstOrDefault();
                    result.Attribute("count").Value = count.ToString();
                    string finalContent = doc.ToString();

                    ChangeConfigurationInput input = new ChangeConfigurationInput
                    {
                        Configuration = Convert.ToBase64String(Encoding.UTF8.GetBytes(finalContent))
                    };

                    //serviceManagement.ChangeConfigurationBySlot(subscriberID, "mmdblab", "production", input);
                    serviceManagement.ChangeConfigurationBySlot(package.SubscriptionInfo.Id, package.HostedService.Name, package.RoleInfo.SlotType, input);

                    if (System.ServiceModel.OperationContext.Current.IncomingMessageProperties.TryGetValue("httpResponse", out propertyValue))
                    {
                        System.ServiceModel.Channels.HttpResponseMessageProperty response = (System.ServiceModel.Channels.HttpResponseMessageProperty)propertyValue;
                        reqId = response.Headers["x-ms-request-id"];
                    }

                    while (true)
                    {
                        string status = serviceManagement.GetOperationStatus(package.SubscriptionInfo.Id, reqId).Status;
                        System.Diagnostics.Debug.WriteLine("ChangeInstaceCount...:" + status);
                        if (status == "Succeeded")
                            return status;
                        System.Threading.Thread.Sleep(20000);
                    }
                }
            }

            private string UpdateDeploymentStatusBySlot(IServiceManagement serviceManagement, DeploymentPackage package)
            {
                if (!string.IsNullOrEmpty(package.DeploymentInfo.Name))
                {
                    string reqId = string.Empty;
                    object propertyValue;
                    using (OperationContextScope scope = new OperationContextScope(serviceManagement as IClientChannel))
                    {
                        var input = new UpdateDeploymentStatusInput()
                        {
                            Status = "Suspended"
                        };
                        serviceManagement.UpdateDeploymentStatusBySlot(package.SubscriptionInfo.Id, package.HostedService.Name, package.RoleInfo.SlotType, input);

                        if (System.ServiceModel.OperationContext.Current.IncomingMessageProperties.TryGetValue("httpResponse", out propertyValue))
                        {
                            System.ServiceModel.Channels.HttpResponseMessageProperty response = (System.ServiceModel.Channels.HttpResponseMessageProperty)propertyValue;
                            reqId = response.Headers["x-ms-request-id"];
                        }

                        while (true)
                        {
                            string status = serviceManagement.GetOperationStatus(package.SubscriptionInfo.Id, reqId).Status;
                            System.Diagnostics.Debug.WriteLine("UpdateDeploymentStatusBySlot...:" + status);
                            if (status == "Succeeded")
                                return status;
                            System.Threading.Thread.Sleep(20000);
                        }
                    }
                }
                return "false";
            }

            private void DeleteDelployment(IServiceManagement serviceManagement, DeploymentPackage package)
            {
                if (!string.IsNullOrEmpty(package.DeploymentInfo.Name))
                {
                    string reqId = string.Empty;
                    object propertyValue;
                    using (OperationContextScope scope = new OperationContextScope(serviceManagement as IClientChannel))
                    {
                        serviceManagement.DeleteDeploymentBySlot(package.SubscriptionInfo.Id, package.HostedService.UrlPrefix, package.RoleInfo.SlotType);

                        if (System.ServiceModel.OperationContext.Current.IncomingMessageProperties.TryGetValue("httpResponse", out propertyValue))
                        {
                            System.ServiceModel.Channels.HttpResponseMessageProperty response = (System.ServiceModel.Channels.HttpResponseMessageProperty)propertyValue;
                            reqId = response.Headers["x-ms-request-id"];
                        }

                        while (true)
                        {
                            string status = serviceManagement.GetOperationStatus(package.SubscriptionInfo.Id, reqId).Status;
                            System.Diagnostics.Debug.WriteLine("DeleteDelployment...:" + status);
                            //Console.WriteLine(string.Format("Processing...:" + status);
                            if (status == "Succeeded")
                                break;
                            System.Threading.Thread.Sleep(20000);
                        }
                    }
                }
            }

            public string RunDelopment(IServiceManagement serviceManagement, DeploymentPackage package)
            {
                if (!string.IsNullOrEmpty(package.DeploymentInfo.Name))
                {
                    string reqId = string.Empty;
                    object propertyValue;
                    using (OperationContextScope scope = new OperationContextScope(serviceManagement as IClientChannel))
                    {
                        var input = new UpdateDeploymentStatusInput()
                        {
                            Status = "Running" //Running
                        };
                        //serviceManagement.UpdateDeploymentStatus(subscriberID, "mmdblab", "ncku-mmdblab", input);
                        serviceManagement.UpdateDeploymentStatus(package.SubscriptionInfo.Id, package.HostedService.Name, package.DeploymentInfo.Name, input);

                        if (System.ServiceModel.OperationContext.Current.IncomingMessageProperties.TryGetValue("httpResponse", out propertyValue))
                        {
                            System.ServiceModel.Channels.HttpResponseMessageProperty response =
                                (System.ServiceModel.Channels.HttpResponseMessageProperty)propertyValue;
                            reqId = response.Headers["x-ms-request-id"];
                        }
                    }

                    while (true)
                    {
                        string status = serviceManagement.GetOperationStatus(package.SubscriptionInfo.Id, reqId).Status;
                        System.Diagnostics.Debug.WriteLine("Running...:" + status);
                        if (status == "Succeeded")
                            return status;
                        //break;
                        System.Threading.Thread.Sleep(20000);
                    }

                }
                return "false";

            }
        }
    }
}