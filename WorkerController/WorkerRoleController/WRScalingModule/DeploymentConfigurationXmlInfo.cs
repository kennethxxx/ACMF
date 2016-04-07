using System;
using System.Linq;

namespace WorkerRoleController.WRScalingModule
{
    public static class DeploymentConfigurationXmlInfo
    {

        public const String DocumentRoot = "documentRoot";
        public const String ElementServiceConfiguration = "ServiceConfiguration";
        public const String ElementRole = "Role";
        public const String AttributeName = "name";
        public const String AttributeValue = "value";
        public const String ElementInstances = "Instances";
        public const String ElementConfigurationSettings = "ConfigurationSettings";
        public const String ElementConfigurationSettingElement = "Setting";
        public const String AttributeInstancesCount = "count";
        public const String Separator = "/";
        public const String ElementDiagnostics = "Diagnostics";
        public const String ElementDiagnosticsDefault = "Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString";
    }
}
