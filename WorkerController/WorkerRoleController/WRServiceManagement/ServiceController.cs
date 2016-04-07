using System;
using System.Linq;
using Microsoft.Samples.WindowsAzure.ServiceManagement;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using WorkerRoleController.WrcConfigFileParser;

namespace WorkerRoleController.WRServiceManagement
{
    class ServiceController
    {

        private const String CurrentDeploymentSlotType = WorkerRoleController.WRScalingModule.DeploymentSlotType.Staging;

        private const int PollTimeoutInSeconds = 1800;
        private const int WaitingPerAsyncOperationLoopInSeconds = 2;
        private const int WaitingPerAsyncOperationLoopInMilliSeconds = WaitingPerAsyncOperationLoopInSeconds * WorkerRoleController.Settings.SecondToMilli;

        private static readonly WorkerRoleController.WRServiceManagement.ServiceOperation[] asyncOperations = { WorkerRoleController.WRServiceManagement.ServiceOperation.Start, WorkerRoleController.WRServiceManagement.ServiceOperation.Stop, WorkerRoleController.WRServiceManagement.ServiceOperation.Delete, WorkerRoleController.WRServiceManagement.ServiceOperation.ChangeDeploymenConfigurations, WorkerRoleController.WRServiceManagement.ServiceOperation.Swap };

        private static Object response;

        public static Object GetResponse()
        {
            return response;
        }

        public static bool DoOperation(String subscriptionId, X509Certificate2 cert, DeploymentInfo deploymentInfo, HostedServiceInfo service, WorkerRoleController.WRServiceManagement.ScallingOperation operation)
        {
            String logMessagePrefix = "Work on Deployment [" + deploymentInfo.Name + " < " + service.UrlPrefix + " < " + subscriptionId + "]\n";
                
            response = null;
            if (operation.Operation == WorkerRoleController.WRServiceManagement.ServiceOperation.Nothing)
                return true;
            try
            {
                IServiceManagement serviceManagement = ServiceManagementHelper.CreateServiceManagementChannel("WindowsAzureEndPoint", cert);
                string trackingId = null;
                HttpStatusCode? statusCode = null;
                string statusDescription = null;

                using (OperationContextScope scope = new OperationContextScope((IContextChannel)serviceManagement))
                {
                    try
                    {
                        switch (operation.Operation)
                        {
                            case WorkerRoleController.WRServiceManagement.ServiceOperation.Start: 
                                StartService(serviceManagement, subscriptionId, deploymentInfo, service);
                                break;
                            case WorkerRoleController.WRServiceManagement.ServiceOperation.Run:
                                UpdateDeploymentStatus(serviceManagement, subscriptionId, deploymentInfo, service, WorkerRoleController.WRServiceManagement.WRDeploymentStatus.Running.ToString());
                                break;
                            case WorkerRoleController.WRServiceManagement.ServiceOperation.Stop:
                                UpdateDeploymentStatus(serviceManagement, subscriptionId, deploymentInfo, service, WorkerRoleController.WRServiceManagement.WRDeploymentStatus.Suspended.ToString());
                                break;
                            case WorkerRoleController.WRServiceManagement.ServiceOperation.GetDeployment: 
                                response = GetDeployment(serviceManagement, subscriptionId, deploymentInfo, service);
                                break;
                            case WorkerRoleController.WRServiceManagement.ServiceOperation.Delete:
                                DeleteDelployment(serviceManagement, subscriptionId, service, deploymentInfo);
                                break;
                            case WorkerRoleController.WRServiceManagement.ServiceOperation.ChangeDeploymenConfigurations:
                                UpdateDeploymentConfigurations(serviceManagement, subscriptionId, deploymentInfo, service, operation.DeploymentInfo);
                                break;
                            case WorkerRoleController.WRServiceManagement.ServiceOperation.Swap:
                                SwapDeploymentSlot(serviceManagement, subscriptionId, deploymentInfo, service);
                                break;
                            default: throw new Exception() ;
                        }
                        if (WebOperationContext.Current.IncomingResponse != null && asyncOperations.Contains(operation.Operation))
                        {
                            trackingId = WebOperationContext.Current.IncomingResponse.Headers[Constants.OperationTrackingIdHeader];
                            statusCode = WebOperationContext.Current.IncomingResponse.StatusCode;
                            statusDescription = WebOperationContext.Current.IncomingResponse.StatusDescription;
                        }
                    }
                    catch (CommunicationException ce)
                    {
                        ServiceManagementError error = null;
                        HttpStatusCode httpStatusCode = 0;
                        string operationId;
                        ServiceManagementHelper.TryGetExceptionDetails(ce, out error, out httpStatusCode, out operationId);
                        if (error == null)
                        {
                            Console.WriteLine(ce.Message);
                        }
                        else
                        {
                            
                            throw;
                        }
                    }
                    finally
                    {
                        if (statusCode != null)
                        {
                        }
                    }
                }
                if (trackingId != null && statusCode != null && statusCode == HttpStatusCode.Accepted)
                {
                    WaitForAsyncOperation(serviceManagement, trackingId, subscriptionId);
                }
                return true;
            }
            catch (TimeoutException)
            {
                return false;
            }     
        }
       
        private static void SwapDeploymentSlot(IServiceManagement serviceManagement, String subscriptionId, DeploymentInfo deploymentInfo, HostedServiceInfo service)
        {
            SwapDeploymentInput input = new SwapDeploymentInput
            {
                Production = deploymentInfo.Name,
                SourceDeployment = deploymentInfo.Name
            };
            serviceManagement.SwapDeployment(subscriptionId, service.UrlPrefix, input);

        }

        private static void WaitForAsyncOperation(IServiceManagement service, string trackingId, String subscriptionId)
        {
            Operation tracking;
            int count = 0;
            do
            {
                tracking = service.GetOperationStatus(subscriptionId, trackingId);
                System.Threading.Thread.Sleep(WaitingPerAsyncOperationLoopInMilliSeconds);

                count++;
                if (PollTimeoutInSeconds > 0 && count > PollTimeoutInSeconds)
                {
                    break;
                }
            }
            while (tracking.Status != OperationState.Failed && tracking.Status != OperationState.Succeeded);

            if (tracking.Status != OperationState.InProgress)
            {
                //Trace.Write("Done", "Async Operation Result");
            }
            if (tracking.Status == OperationState.Failed)
            {
                //Trace.Write("Failed", "Async Operation Result");
                throw new Exception();
            }
        }

        private static void UpdateDeploymentConfigurations(IServiceManagement serviceManagement, String subscriptionId, DeploymentInfo deploymentInfo, HostedServiceInfo service, String configurationsBase64)
        {
            var input = new ChangeConfigurationInput();
            input.Configuration = configurationsBase64;
            if (!string.IsNullOrEmpty(deploymentInfo.Name))
            {
                serviceManagement.ChangeConfiguration(subscriptionId, service.UrlPrefix, deploymentInfo.Name, input);
            }
            else if (!string.IsNullOrEmpty(CurrentDeploymentSlotType))
            {
                serviceManagement.ChangeConfigurationBySlot(subscriptionId, service.UrlPrefix, CurrentDeploymentSlotType, input);
            }
        }

        private static Deployment GetDeployment(IServiceManagement serviceManagement, String subscriptionId, DeploymentInfo deploymentInfo, HostedServiceInfo service)
        {
            try
            {
                Deployment deployment = null;

                if (!string.IsNullOrEmpty(deploymentInfo.Name))
                {
                    deployment = serviceManagement.GetDeployment(subscriptionId, service.UrlPrefix, deploymentInfo.Name);
                }
                else if (!string.IsNullOrEmpty(CurrentDeploymentSlotType))
                {
                    deployment = serviceManagement.GetDeploymentBySlot(subscriptionId, service.UrlPrefix, CurrentDeploymentSlotType);
                }
                return deployment;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static void StartService(IServiceManagement serviceManagement ,String subscriptionId, DeploymentInfo deploymentInfo, HostedServiceInfo service)
        {
            
            String uriPrefix = Utils.GenerateBlobLink(deploymentInfo.Storage.Name, deploymentInfo.StoragePath);

            String cfgPath = uriPrefix + ((WorkerRoleController.WRScalingModule.WRScaling.DeploymentInfoExtension)deploymentInfo).DeploymentConfigurationNameFile;
            CreateDeploymentInput input = new CreateDeploymentInput
            {
                Name = deploymentInfo.Name,
                Configuration = ServiceManagementHelper.EncodeToBase64String(Utils.GetSettings(deploymentInfo.StoragePath , cfgPath)),
            };

            input.PackageUrl = new Uri(uriPrefix + ((WorkerRoleController.WRScalingModule.WRScaling.DeploymentInfoExtension)deploymentInfo).DeploymentPackageNameFile);
            
            input.Label = ServiceManagementHelper.EncodeToBase64String(deploymentInfo.Name+" Label");

            serviceManagement.CreateOrUpdateDeployment(subscriptionId, service.UrlPrefix, CurrentDeploymentSlotType, input);
        
        }

        private static void DeleteDelployment(IServiceManagement serviceManagement, String subscriptionId, HostedServiceInfo service,DeploymentInfo deploymentInfo)
        {
            if (!string.IsNullOrEmpty(deploymentInfo.Name))
            {
                serviceManagement.DeleteDeployment(subscriptionId, service.UrlPrefix, deploymentInfo.Name);
            }
            else if (!string.IsNullOrEmpty(deploymentInfo.Name))
            {
                serviceManagement.DeleteDeploymentBySlot(subscriptionId, service.UrlPrefix, CurrentDeploymentSlotType);
            }
        }
      
        private static void UpdateDeploymentStatus(IServiceManagement serviceManagement ,String subscriptionId, DeploymentInfo deployment ,HostedServiceInfo service, String state)
        {
            var input = new UpdateDeploymentStatusInput()
            {
                Status = state
            };
            if (!string.IsNullOrEmpty(deployment.Name))
            {
                serviceManagement.UpdateDeploymentStatus(subscriptionId, service.UrlPrefix, deployment.Name, input);
            }
            else if (!string.IsNullOrEmpty(CurrentDeploymentSlotType))
            {
                serviceManagement.UpdateDeploymentStatusBySlot(subscriptionId, service.UrlPrefix, CurrentDeploymentSlotType, input);
            }
        }

        

    }
}
