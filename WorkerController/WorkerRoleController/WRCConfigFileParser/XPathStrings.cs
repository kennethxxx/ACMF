using System;
using System.Linq;

namespace WorkerRoleController.ConfigFileParser
{
    internal class XPathStrings
    {
        public const string ElementGeneralConfiguration = "/root/MonitoringWRPeriod";

        public const string ElementSubscriptions = "/root/subscriptions/subscription";

        public const string ElementStorages = "storages/storage";

        public const string ElementDeployments = "deployments/deployment";

        public const string ElementRoles = "roles/role";

        public const string ElementHostedServices = "services/hostedservice";

        public XPathStrings()
        {
        }
    }
}
