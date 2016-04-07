using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using WorkerRoleController.WrcConfigFileParser;
using WorkerRoleController.WRScalingModule;
using Microsoft.WindowsAzure.Storage.Auth;
using WorkerRoleController.FileTransferation;

namespace WorkerRoleController
{
    public class Utils
    {
        public static DateTime Now
        {
            get
            {
                return DateTime.UtcNow;
            }
        }

        public static String GetConfigurations(int n)
        {
            String xmlFile = null;
            try
            {
                String scallingConfigurationBlobContainer = WorkerRoleController.Settings.GetSettingString(WorkerRoleController.Settings.Setting.ScalingConfigurationBlobContainer);

                switch (n)
                {
                    case 1:
                        xmlFile = WorkerRoleController.Settings.GetSettingString(WorkerRoleController.Settings.Setting.WorkerRoleControllerConfig);
                        break;
                    case 2:
                        xmlFile = WorkerRoleController.Settings.GetSettingString(WorkerRoleController.Settings.Setting.WRScalingRule);
                        break;
                    case 3:
                        xmlFile = WorkerRoleController.Settings.GetSettingString(WorkerRoleController.Settings.Setting.DriverOption);
                        break;
                    case 4:
                        xmlFile = WorkerRoleController.Settings.GetSettingString(WorkerRoleController.Settings.Setting.PrivateWorkerRoleControllerConfig);
                        break;
                    default:
                        break;
                }
                if (n == 3)
                {
                    string FileForder = "XML";   
                    string text; 
                    FileTransferation.FileTransferation FT = new FileTransferation.FileTransferation();
                    FT.DownLoadFile(FileForder, xmlFile);  //Download檔案

                    string Path = System.IO.Path.Combine(@"D:\FileDownload\", xmlFile);  //DownLoad下來的檔案路徑
                    StreamReader sr = new StreamReader(Path);
                    text = sr.ReadToEnd();
                    return text;
                    
                }
                else if(n==4)
                {
                    string FileForder = "XML";
                    string text;
                    FileTransferation.FileTransferation FT = new FileTransferation.FileTransferation();
                    FT.DownLoadFile(FileForder, xmlFile);  //Download檔案

                    string Path = System.IO.Path.Combine(@"D:\FileDownload\", xmlFile);  //DownLoad下來的檔案路徑
                    StreamReader sr = new StreamReader(Path);
                    text = sr.ReadToEnd();
                    return text;

                }
                else
                {
                    CloudStorageAccount acct = GetStorageAccount(WorkerRoleController.Settings.GetConfigurationTag(WorkerRoleController.Settings.Setting.ScalingConfigurationStorageConnectionString));
                    CloudBlobClient bClient = acct.CreateCloudBlobClient();
                    CloudBlobContainer bContainer = bClient.GetContainerReference(scallingConfigurationBlobContainer);
                    CloudBlockBlob blob = bContainer.GetBlockBlobReference(xmlFile);
                    string text;
                    using (var memoryStream = new MemoryStream())
                    {
                        blob.DownloadToStream(memoryStream);
                        text = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
                    }

                    return text;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static WRScaling.DeploymentInfoExtension GetDeploymentExtension(DeploymentInfo deployment)
        {
            CloudStorageAccount acct = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("ScalingConfigurationStorageConnectionString"));
            CloudBlobClient bClient = acct.CreateCloudBlobClient();
            CloudBlobContainer bDirectory = bClient.GetContainerReference(deployment.StoragePath);
            WRScaling.DeploymentInfoExtension ext = new WRScaling.DeploymentInfoExtension();
            int startAt = bDirectory.Uri.ToString().Length;
            String auxUri;
            foreach (CloudBlockBlob b in bDirectory.ListBlobs())
            {
                auxUri = b.Uri.ToString();
                if (auxUri.EndsWith(".cscfg"))
                    ext.DeploymentConfigurationNameFile = auxUri.Substring(startAt);
                else if (auxUri.EndsWith(".cspkg"))
                    ext.DeploymentPackageNameFile = auxUri.Substring(startAt);
            }
            ext.Name = deployment.Name;
            ext.Roles = deployment.Roles;
            ext.Storage = deployment.Storage;
            ext.StoragePath = deployment.StoragePath;
            return ext;
        }

        public static String GenerateBlobLink(String storageName, String storagePath)
        {
            return "https://" + storageName + ".blob.core.windows.net/" + storagePath;
        }

        public static string GetSettings(string containername, string settingsFile)
        {
            String plainText = null;
            try
            {
                //CloudStorageAccount csa = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("ScalingConfigurationStorageConnectionString"));
                StorageCredentials csa = new StorageCredentials("boyili", "XXep/w2zNyecD3jE8YzX2a/WHsaGgF5j70XFLzv9CBuNNRYSs5wiSQ77kXcoXFDsRRp85uJu34wEHd0I1YZF4Q==");
                CloudStorageAccount storageAccount = new CloudStorageAccount(csa, true);
                CloudBlobClient cbc = storageAccount.CreateCloudBlobClient();
                // Retrieve reference to a previously created container.
                CloudBlobContainer container = cbc.GetContainerReference(containername);

                byte[] ms = GetBytes(container, settingsFile);
                MemoryStream stream = new MemoryStream(ms);
                StreamReader rr = new StreamReader(stream);
                plainText = rr.ReadToEnd();
                
                if (String.IsNullOrEmpty(plainText))
                    throw new Exception();
                return plainText;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static byte[] GetBytes(CloudBlobContainer container, string fileName)
        {
            CloudBlockBlob blob = container.GetBlockBlobReference(fileName);
            using (var ms = new MemoryStream())
            {
                blob.DownloadToStream(ms);
                ms.Position = 0;
                return ms.ToArray();
            }
        }

        public static CloudStorageAccount GetStorageAccount(String scalingConfigurationStorageConnectionStringTag)
        {
            return CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue(scalingConfigurationStorageConnectionStringTag));
        }

        public static X509Certificate2 GetLocalCert()
        {
            X509Certificate2 x509Certificate2 = null;
            X509Store store = new X509Store("My", StoreLocation.CurrentUser);
            try
            {
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection x509Certificate2Collection = store.Certificates.Find(X509FindType.FindBySubjectName, "Mycert", false);
                x509Certificate2 = x509Certificate2Collection[0];

            }
            finally
            {
                store.Close();
            }
            return x509Certificate2;
        }
    }
}
