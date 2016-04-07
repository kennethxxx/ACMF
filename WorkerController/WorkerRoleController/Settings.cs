using System;
using System.Linq;
using Microsoft.WindowsAzure.ServiceRuntime;
using WorkerRoleController.WrcConfigFileParser;

namespace WorkerRoleController
{
    public class Settings
    {
        public const int MinuteToSecond = 60;
        public const int DefaultAdditionalMinutesToSleep = 10;

        public const int SecondToMilli = 1000;

        public enum Setting
        {
            DriverOption,
            ScalingConfigurationStorageConnectionString,
            ScalingConfigurationBlobContainer,
            PrivateWorkerRoleControllerConfig,
            WorkerRoleControllerConfig,
            WRScalingRule,
            ScalingSecondsPerLoop,
            ScalingLogTableName,
            ScalingLogTableEntitiesPartionKey,
            ScalingAverageVMBootTime,
            ScalingMinutesToSleepAfterIncreaseVMNumber,
            ScalingMinutesToSleepAfterDecreaseVMNumber
        }

        private const String ScalingConfigurationStorageConnectionString = "ScalingConfigurationStorageConnectionString";
        private const String ScalingConfigurationBlobContainer = "ScalingConfigurationBlobContainer";
        private const String WorkerRoleControllerConfig = "WorkerRoleControllerConfig";
        private const String WRScalingRule = "WRScalingRule";
        private const String DriverOption = "DriverOption";
        private const String PrivateWorkerRoleControllerConfig = "PrivateWorkerRoleControllerConfig";

        public static void UpdateGeneralConfigurations(MonitoringWRPeriod configurations)
        {
            config = configurations;
        }

        private static MonitoringWRPeriod config;

        public static int GetSettingInteger(Setting setting)
        {
            try
            {
                switch (setting)
                {
                    case Setting.ScalingSecondsPerLoop:
                        return config.SecondsPerLoop;
                    default: throw new Exception();
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public static String GetSettingString(Setting setting)
        {
                String value;
                switch (setting)
                {

                    case Setting.ScalingConfigurationStorageConnectionString:
                        value = GetConfigurationValue(ScalingConfigurationStorageConnectionString);
                        break;
                    case Setting.ScalingConfigurationBlobContainer:
                        value = GetConfigurationValue(ScalingConfigurationBlobContainer);
                        break;
                    case Setting.WorkerRoleControllerConfig:
                        value = GetConfigurationValue(WorkerRoleControllerConfig);
                        break;
                    case Setting.WRScalingRule:
                        value = GetConfigurationValue(WRScalingRule);
                        break;
                    case Setting.ScalingLogTableName:
                        return config.ScalingLogTableName;
                    case Setting.ScalingLogTableEntitiesPartionKey:
                        return config.ScalingLogTableEntitiesPartionKey;
                    case Setting.DriverOption:
                        value = GetConfigurationValue(DriverOption);
                        break;
                    case Setting.PrivateWorkerRoleControllerConfig:
                        value = GetConfigurationValue(PrivateWorkerRoleControllerConfig);
                        break;
                    default: throw new Exception();
                }
                if (!String.IsNullOrEmpty(value))
                {
                    return value;
                }
                else throw new Exception();
        }

        private static String GetConfigurationValue(String tag)
        {
            return RoleEnvironment.GetConfigurationSettingValue(tag);
        }

        public static String GetConfigurationTag(Setting setting)
        {
            switch (setting)
            {
                case Setting.ScalingConfigurationStorageConnectionString:
                    return ScalingConfigurationStorageConnectionString;
                default: throw new Exception();
            }
        }

    }
}
