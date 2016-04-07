using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.IO;
using System.Web;
using System.Configuration;
using WorkerRoleController.FileTransfer;

namespace WorkerRoleController.FileTransferation
{
    class FileTransferation
    {
        public void DownLoadFile(string ForderName, string FileName)
        {
            try
            {
                BasicHttpBinding binding1 = new BasicHttpBinding();
                binding1.OpenTimeout = new TimeSpan(0, 40, 0);
                binding1.CloseTimeout = new TimeSpan(0, 40, 0);
                binding1.SendTimeout = new TimeSpan(0, 40, 0);
                binding1.MaxReceivedMessageSize = 2147483647;
                binding1.MaxBufferSize = 2147483647;
                WorkerRoleController.FileTransfer.FileTransferChannel IOStep1 = ChannelFactory<FileTransferChannel>.CreateChannel(binding1,
                    new EndpointAddress(ConfigurationManager.ConnectionStrings["ConnectResponseDataString"].ConnectionString));
                IOStep1.Open();

                DownloadRequest requestData = new DownloadRequest();
                requestData.FileName = FileName;
                requestData.ForderName = ForderName;
                RemoteFileInfo fileInfo = new RemoteFileInfo();
                fileInfo = IOStep1.DownloadFile(requestData);
                //IOStep1.Close();

                String SavePath = @"D:\FileDownload\";

                if (!Directory.Exists(SavePath))                //如果檔案目錄不存在則建立
                {
                    Directory.CreateDirectory(SavePath);
                }
                if (File.Exists(SavePath + fileInfo.FileName)) //如果檔案位置已有相同檔案需要刪除
                {
                    File.Delete(SavePath + fileInfo.FileName);
                }

                FileStream targetStream = null;  //宣告接收的FileStream

                string SaveFilePath = Path.Combine(SavePath, fileInfo.FileName); //將字串合併成檔案路徑

                using (targetStream = new FileStream(SaveFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    byte[] buffer = new byte[6500];
                    int bytesRead = 0;
                    while ((bytesRead = fileInfo.FileByteStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        targetStream.Write(buffer, 0, bytesRead);
                    }
                    targetStream.Close();
                }
                IOStep1.Close();
            }
            catch (Exception ex)
            {
                // Trap the error, if any.
                System.Web.HttpContext.Current.Response.Write("Error : " + ex.Message);
            }
        }

        public void UpLoadFile(string ForderName, string FileName)
        {
            try
            {
                BasicHttpBinding binding1 = new BasicHttpBinding();
                binding1.OpenTimeout = new TimeSpan(0, 40, 0);
                binding1.CloseTimeout = new TimeSpan(0, 40, 0);
                binding1.SendTimeout = new TimeSpan(0, 40, 0);
                WorkerRoleController.FileTransfer.FileTransferChannel IOStep1 = ChannelFactory<FileTransferChannel>.CreateChannel(binding1,
                        new EndpointAddress(ConfigurationManager.ConnectionStrings["ConnectResponseDataString"].ConnectionString));
                IOStep1.Open();

                RemoteFileInfo uploadRequestInfo = new RemoteFileInfo();
                string filePath = System.IO.Path.Combine(@"D:\", ForderName, FileName);
                System.IO.FileInfo fileInfo = new FileInfo(filePath);
                if (!fileInfo.Exists)
                    throw new System.IO.FileNotFoundException("File not found", fileInfo.Name);

                FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                uploadRequestInfo.FileName = FileName;
                uploadRequestInfo.Length = fileInfo.Length;
                uploadRequestInfo.FileByteStream = stream;
                IOStep1.UploadFile(uploadRequestInfo);
                //clientUpload.UploadFile(stream);
                IOStep1.Close();
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write("Error : " + ex.Message);
            }
        }
    }
}
