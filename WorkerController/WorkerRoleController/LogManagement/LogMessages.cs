using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkerRoleController.LogManagement
{
    public class LogMessages
    {
        public const String MessageInformationStarting = "AutoScaling is Starting.";
        public const String MessageErrorReadingCscfgSecondsPerLoop = "Invalid Seconds Per Loop Configuration Value";
        public const String MessageWarningCertificateNotFound = "Can't find any Certificate installed on Virtual Machine";
        public const String MessageInformationCertificateImported = "Certificate imported successfully from virtual machine";
        public const String MessageErrorCertificateImported = "Certificate imported unsuccessfully from virtual machine";
        public const String MessageInformationStartingNewLoop = "Starting new loop.";
        public const String MessageInformationXmlConfigurationImported = "AutoScaling Xml Configuration imported successfully";
        public const String MessageErrorInvalidXmlConfigurationFile = "AutoScaling Xml Configuration File Validation problem";
        public const String MessageErrorXmlConfigurationFileImported = "AutoScaling Xml Configuration imported unsuccessfully";
        public const String MessageInformationDeploymentFileNamesImported = "Deploymet Files Name (*.cspkg and *.cscfg)  imported successfully";
        public const String MessageErrorDeploymentFileNamesImported = "Deploymet Files Name (*.cspkg and *.cscfg)  imported unsuccessfully";
        public const String MessageErrorDeploymentWorkerStarting = "Unexpected error occured on deployment worker starting";
        public const String MessageWarningSleeping = "Unexpected error occured when worker was sleeping";
        public const String MessageInformationStopWorker = "AutoScaling Worker is Stoping.";
        public const String MessageInformationOperationDeploymentSwap = "Deployment environment Swapped to ";
        public const String MessageInformationTopscheduleApplied = "Top schedule applied.";
        public const String MessageInformationOperationDeploymentCreated = "New deployment created.";
        public const String MessageInformationOperationDeploymentConfigurationUpdated = "Deployment Configuration File (*.cscfg) Updated";
        public const String MessageInformationOperationDeploymentStarted = "Deployment started running";
        public const String MessageErrorUnexpectedOccured = "Unexpeced error occured.";
        public const String MessageWarningDeploymentConfiguratedFileInexists = "Deployment configuration file inexists or impossible to be downloaded.";
        public const String MessageErrorOperationTimeoutCommand = "TIMEOUT - There was an error processing this operation command. \nOperation: ";
        public const String MessageWarningOperationCommunication = "There was an error in communication with server.";
        public const String MessageWarningDiagnosticsCpuUpperThresholdReached = "Diagnostics: CPU Load Upper threshold reached.";
        public const String MessageWarningDiagnosticsCpuLowerThresholdReached = "Diagnostics: CPU Load Lower threshold reached.";
        public const String MessageWarningDiagnosticsCpuLoadAverageError = "Diagnostics: Error calculating % CPU Load average.";
        public const String MessageWarningDiagnosticsInstanceCountBYCpuLoadDisabled = "Diagnostics: Instance Count By CPU Load disabled.\nDiagnostics Connection String not found";
        public const String MessageWarningSleepInvalidAverageVMBootTime = "Error sleeping during average VM Boot time.";
        public const String MessageWarningSleepInvalidAdditionalAfterIncrease = "Error sleeping additional time after increasing the VM instance number.";
        public const String MessageWarningSleepInvalidAdditionalAfterDecrease = "Error sleeping additional time after decreasing the VM instance number.";
        public const String MessageErrorGetInvalidAdditionalMinutesTOSleep = "Error getting additional minutes to sleep after decreasing or increasing the VM instance number.";
    }
}
