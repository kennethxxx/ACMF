using System;
using System.Linq;

using System.Security.Cryptography.X509Certificates;
using WorkerRoleController.WrcConfigFileParser;
using Microsoft.Samples.WindowsAzure.ServiceManagement;

namespace WorkerRoleController.WRServiceManagement
{
    class ServiceStatusController
    {

        public static Deployment GetOnlineDeploymentStatus(String subscriptionId, X509Certificate2 cert, DeploymentInfo deploymentInfo, HostedServiceInfo hostedService)
        {
            if (WorkerRoleController.WRServiceManagement.ServiceController.DoOperation(subscriptionId, cert, deploymentInfo, hostedService, new WorkerRoleController.WRServiceManagement.ScallingOperation(WorkerRoleController.WRServiceManagement.ServiceOperation.GetDeployment)))
            {
                return (Deployment)WorkerRoleController.WRServiceManagement.ServiceController.GetResponse();
            }
            else
                return null;
        }

        public static bool StartDeploymentIfNeeded(String subscriptionId, X509Certificate2 cert, DeploymentInfo deploymentInfo, HostedServiceInfo hostedService, Deployment deployment)
        {
            if (deployment == null)
            {
                if (InitiateDeployment(subscriptionId, cert, deploymentInfo, hostedService))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool HandleWithDeploymentPrioritySchedule(String subscriptionId, X509Certificate2 cert, DeploymentInfo deploymentInfo, HostedServiceInfo hostedService,
            WorkerRoleController.WRServiceManagement.NextServiceStatus status, Deployment actualDeployment)
        {
            if (actualDeployment != null && status.Delete)
            {
                if (!DeleteDeployment(subscriptionId, cert, deploymentInfo, hostedService, actualDeployment.Status))
                    throw new Exception();
                return true;
            }
            else if (actualDeployment != null && status.Stop)
            {
                if (!StopDeployment(subscriptionId, cert, deploymentInfo, hostedService, actualDeployment.Status))
                    throw new Exception();
                return true;
            }
            else if (actualDeployment == null && status.Stop)
            {
                if (!InitiateDeployment(subscriptionId, cert, deploymentInfo, hostedService))
                {
                    throw new Exception();
                }
                return true;
            }
            return false;
        }

        public static bool StopDeployment(String subscriptionId, X509Certificate2 cert, DeploymentInfo deploymentInfo, HostedServiceInfo hostedService, String actualStatus)
        {
            if (!actualStatus.Equals(WorkerRoleController.WRServiceManagement.WRDeploymentStatus.Suspended.ToString()) && !actualStatus.Equals(WorkerRoleController.WRServiceManagement.WRDeploymentStatus.Suspending.ToString()) && !actualStatus.Equals(WorkerRoleController.WRServiceManagement.WRDeploymentStatus.SuspendedTransitioning.ToString()))
            {
                return WorkerRoleController.WRServiceManagement.ServiceController.DoOperation(subscriptionId, cert, deploymentInfo, hostedService, new WorkerRoleController.WRServiceManagement.ScallingOperation(WorkerRoleController.WRServiceManagement.ServiceOperation.Stop));
            }
            return true;
        }

        public static bool DeleteDeployment(String subscriptionId, X509Certificate2 cert, DeploymentInfo deploymentInfo, HostedServiceInfo hostedService, String actualStatus)
        {
            if (!actualStatus.Equals(WorkerRoleController.WRServiceManagement.WRDeploymentStatus.Deleting.ToString()))
            {
                if (StopDeployment(subscriptionId, cert, deploymentInfo, hostedService, actualStatus))
                {
                    return WorkerRoleController.WRServiceManagement.ServiceController.DoOperation(subscriptionId, cert, deploymentInfo, hostedService, new WorkerRoleController.WRServiceManagement.ScallingOperation(WorkerRoleController.WRServiceManagement.ServiceOperation.Delete));
                }
                else
                    return false;
            }
            return true;
        }

        private static bool InitiateDeployment(String subscriptionId, X509Certificate2 cert, DeploymentInfo deploymentInfo, HostedServiceInfo hostedService)
        {
            return WorkerRoleController.WRServiceManagement.ServiceController.DoOperation(subscriptionId, cert, deploymentInfo, hostedService, new WorkerRoleController.WRServiceManagement.ScallingOperation(WorkerRoleController.WRServiceManagement.ServiceOperation.Start));
        }

        public static bool SwapDeployment(String subscriptionId, X509Certificate2 cert, DeploymentInfo deploymentInfo, HostedServiceInfo hostedService, Deployment deployment, String nextSlot)
        {
            if (nextSlot.ToLower().Equals(WorkerRoleController.WRScalingModule.DeploymentSlotType.Staging.ToLower()))
            {
                if (DeleteDeployment(subscriptionId, cert, deploymentInfo, hostedService, deployment.Status))
                {
                    return InitiateDeployment(subscriptionId, cert, deploymentInfo, hostedService);
                }
            }
            else if (WorkerRoleController.WRServiceManagement.ServiceController.DoOperation(subscriptionId, cert, deploymentInfo, hostedService, new WorkerRoleController.WRServiceManagement.ScallingOperation(WorkerRoleController.WRServiceManagement.ServiceOperation.Swap)))
            {
                deployment = ServiceStatusController.GetOnlineDeploymentStatus(subscriptionId, cert, deploymentInfo, hostedService);
                return deployment.DeploymentSlot.ToLower().Equals(nextSlot.ToLower());
            }
            return false;
        }
    }
}
