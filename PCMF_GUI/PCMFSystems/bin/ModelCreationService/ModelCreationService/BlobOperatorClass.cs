using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.IO;
using SevenZip;

namespace BlobOperatorClassNS
{
    public class BlobOperatorClass
    {
        private String ConnectionString = "";
        private CloudStorageAccount cloudStorageAccount = null;
        private CloudBlobClient BlobClient = null;

        private BlobOperatorClass()
        {

        }

        /// <summary>
        /// InitBlobOperator
        /// </summary>
        /// <param name="CloudStorageConnectionString">Azure Cloud Storage Connect String</param>
        /// <param name="SevenZipDLLPath">Local Machine 7-zip Dll Path</param>
        public BlobOperatorClass(string CloudStorageConnectionString, string SevenZipDLLPath)
        {
            ConnectionString = CloudStorageConnectionString;
            cloudStorageAccount = CloudStorageAccount.Parse(ConnectionString);
            BlobClient = cloudStorageAccount.CreateCloudBlobClient();
            SevenZipCompressor.SetLibraryPath(SevenZipDLLPath);
        }

        /// <summary>
        /// Download a File Blob
        /// </summary>
        /// <param name="TargetFilePath">Local Save Full File Name</param>
        /// <param name="BlobContainerName">Blob Container Name</param>
        /// <param name="BlobFileName">Blob File Name</param>
        /// <param name="DeleteBlobAfterDownload">Set True will Delete Blob after Download finished</param>
        /// <returns>'success' for preface, other is error message.</returns>
        public string DownloadAsFile(string TargetFilePath, String BlobContainerName, String BlobFileName, bool DeleteBlobAfterDownload)
        {
            try
            {
                string TempleFileName = Path.GetTempFileName();
                FileInfo FI = new FileInfo(TargetFilePath);
                string TargetDirPath = FI.DirectoryName;
                if (!Directory.Exists(TargetDirPath))
                {
                    Directory.CreateDirectory(TargetDirPath);
                }
                else
                {
                    if (File.Exists(TargetFilePath))
                    {
                        File.Delete(TargetFilePath);
                    }
                }

                CloudBlobContainer CloudContainer = BlobClient.GetContainerReference(BlobContainerName);
                CloudContainer.CreateIfNotExist();
                var bloblists = CloudContainer.ListBlobs(new BlobRequestOptions() { UseFlatBlobListing = true });

                byte[] _modelBytes = null;
                foreach (CloudBlob _blob in bloblists)
                {
                    if (_blob.Container.Name.CompareTo(BlobContainerName) == 0 && _blob.Name.CompareTo(BlobFileName) == 0) //在Blob中找檔案
                    {
                        _modelBytes = _blob.DownloadByteArray();
                        FileStream writer = new FileStream(TempleFileName, FileMode.Append);
                        writer.Write(_modelBytes, 0, _modelBytes.Length);
                        writer.Flush();
                        writer.Close();
                        if (DeleteBlobAfterDownload)
                        {
                            _blob.DeleteIfExists();
                        }
                        break;
                    }
                }

                using (SevenZipExtractor tmp = new SevenZipExtractor(TempleFileName))
                {
                    if (tmp.ArchiveFileData.Count != 1)
                    {
                        return "ArchiveFile isn't only one.";
                    }
                    tmp.ExtractFiles(TargetDirPath, tmp.ArchiveFileData[0].Index);
                    if (File.Exists(TargetDirPath + "\\" + tmp.ArchiveFileData[0].FileName))
                    {
                        if (TargetFilePath.CompareTo(TargetDirPath + "\\" + tmp.ArchiveFileData[0].FileName) != 0)
                        {
                            File.Copy(TargetDirPath + "\\" + tmp.ArchiveFileData[0].FileName, TargetFilePath, true);
                            File.Delete(TargetDirPath + "\\" + tmp.ArchiveFileData[0].FileName);
                        }
                    }
                }

                if (File.Exists(TempleFileName))
                {
                    File.Delete(TempleFileName);
                }
            }
            catch (System.Exception ex)
            {
                return ex.ToString();
            }
            
            return "success";
        }

        /// <summary>
        /// Download a Directory Blob
        /// </summary>
        /// <param name="TargetDirPath">Local Save Directory Name</param>
        /// <param name="BlobContainerName">Blob Container Name</param>
        /// <param name="BlobFileName">Blob File Name</param>
        /// <param name="ClearTargetDirBeforeDownload">Set True will Clear Directory Before Strat Download</param>
        /// <param name="DeleteBlobAfterDownload">Set True will Delete Blob after Download finished</param>
        /// <returns>'success' for preface, other is error message.</returns>
        public string DownloadAsDirectory(string TargetDirPath, String BlobContainerName, String BlobFileName, bool ClearTargetDirBeforeDownload, bool DeleteBlobAfterDownload)
        {

            try
            {
                string TempleFileName = Path.GetTempFileName();
                if (!Directory.Exists(TargetDirPath))
                {
                    Directory.CreateDirectory(TargetDirPath);
                }
                else
                {
                    if (ClearTargetDirBeforeDownload)
                    {
                        Directory.Delete(TargetDirPath, true);
                        Directory.CreateDirectory(TargetDirPath);
                    }
                }

                CloudBlobContainer CloudContainer = BlobClient.GetContainerReference(BlobContainerName);
                CloudContainer.CreateIfNotExist();
                var bloblists = CloudContainer.ListBlobs(new BlobRequestOptions() { UseFlatBlobListing = true });
                
                byte[] _modelBytes = null;
                foreach (CloudBlob _blob in bloblists)
                {
                    if (_blob.Container.Name.CompareTo(BlobContainerName) == 0 && _blob.Name.CompareTo(BlobFileName) == 0) //在Blob中找檔案
                    {
                        _modelBytes = _blob.DownloadByteArray();
                        FileStream writer = new FileStream(TempleFileName, FileMode.Append);
                        writer.Write(_modelBytes, 0, _modelBytes.Length);
                        writer.Flush();
                        writer.Close();
                        if (DeleteBlobAfterDownload)
                        {
                            _blob.DeleteIfExists();
                        }
                        break;
                    }
                }

                using (SevenZipExtractor tmp = new SevenZipExtractor(TempleFileName))
                {
                    try
                    {
                        tmp.ExtractArchive(TargetDirPath);
                    }
                    catch (System.Exception ex)
                    {
                        return ex.ToString();
                    }
                }

                if (File.Exists(TempleFileName))
                {
                    File.Delete(TempleFileName);
                }
            }
            catch (System.Exception ex)
            {
                return ex.ToString();
            }
            
            return "success";
        }

        /// <summary>
        /// Upload a Directory to Blob
        /// </summary>
        /// <param name="SourceDirPath">Local Directory Path</param>
        /// <param name="BlobContainerName">Blob Container Name</param>
        /// <param name="BlobFileName">Blob File Name</param>
        /// <param name="DeleteSourceDirAfterUploaded">Set True will Delete Directory After Download finished</param>
        /// <returns>'success' for preface, other is error message.</returns>
        public string UploadAsDirectory(string SourceDirPath, String BlobContainerName, String BlobFileName, bool DeleteSourceDirAfterUploaded)
        {
            try
            {
                string TempleFileName = Path.GetTempFileName();

                SevenZipCompressor tmp = new SevenZipCompressor();
                tmp.CompressDirectory(SourceDirPath, TempleFileName);


                // 壓縮完後將檔案刪除
                if (DeleteSourceDirAfterUploaded)
                {
                    if (Directory.Exists(SourceDirPath))
                    {
                        foreach (string FilePath in Directory.GetFiles(SourceDirPath))
                        {
                            File.Delete(FilePath);
                        }
                        Directory.Delete(SourceDirPath, true);
                    }
                }

                try
                {
                    CloudBlobContainer CloudContainer = BlobClient.GetContainerReference(BlobContainerName);//container
                    CloudContainer.CreateIfNotExist(); //建一個container                   
                    byte[] buffer = new byte[4096];   // Create a byte array of file stream length 
                    FileStream fs = new FileStream(TempleFileName, FileMode.Open, FileAccess.Read);
                    byte[] ImageData = new byte[fs.Length]; //fs.Length為取得檔案大小   , byte[] ImageData將fs轉會位元組
                    //Read block of bytes from stream into the byte array
                    fs.Read(ImageData, 0, System.Convert.ToInt32(fs.Length));//從資料流讀取位元組區塊，並將資料寫入指定緩衝區。
                    //Close the File Stream   fs.Close(); 
                    var blobs = CloudContainer.ListBlobs(new BlobRequestOptions() { UseFlatBlobListing = true });
                    var blob = CloudContainer.GetBlockBlobReference(BlobFileName);
                    blob.DeleteIfExists();
                    BlobStream blobstream = blob.OpenWrite();
                    blobstream.Write(ImageData, 0, ImageData.Count());//將檔案寫入blob中
                    blobstream.Flush();
                    blobstream.Close();
                    fs.Close();
                    if (File.Exists(TempleFileName))
                    {
                        File.Delete(TempleFileName);
                    }
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return "success";
        }

        /// <summary>
        /// Upload a file to Blob
        /// </summary>
        /// <param name="SourceFilePath">Local File Full Path</param>
        /// <param name="BlobContainerName">Blob Container Name</param>
        /// <param name="BlobFileName">Blob File Name</param>
        /// <param name="DeleteSourceFileAfterUploaded">Set True will Delete File After Download finished</param>
        /// <returns>'success' for preface, other is error message.</returns>
        public string UploadAsFile(string SourceFilePath, String BlobContainerName, String BlobFileName, bool DeleteSourceFileAfterUploaded)
        {
            try
            {
                string TempleFileName = Path.GetTempFileName();
                SevenZipCompressor tmp = new SevenZipCompressor();
                tmp.CompressFiles(TempleFileName, SourceFilePath);

                // 壓縮完後將檔案刪除
                if (DeleteSourceFileAfterUploaded)
                {
                    if (File.Exists(SourceFilePath))
                    {
                        File.Delete(SourceFilePath);
                    }
                }

                try
                {
                    CloudBlobContainer CloudContainer = BlobClient.GetContainerReference(BlobContainerName);//container
                    CloudContainer.CreateIfNotExist(); //建一個container                   
                    byte[] buffer = new byte[4096];   // Create a byte array of file stream length 
                    FileStream fs = new FileStream(TempleFileName, FileMode.Open, FileAccess.Read);
                    byte[] ImageData = new byte[fs.Length]; //fs.Length為取得檔案大小   , byte[] ImageData將fs轉會位元組
                    
                    //Read block of bytes from stream into the byte array
                    fs.Read(ImageData, 0, System.Convert.ToInt32(fs.Length));//從資料流讀取位元組區塊，並將資料寫入指定緩衝區。
                    
                    //Close the File Stream   fs.Close(); 
                    var blobs = CloudContainer.ListBlobs(new BlobRequestOptions() { UseFlatBlobListing = true });
                    var blob = CloudContainer.GetBlockBlobReference(BlobFileName);
                    blob.DeleteIfExists();
                    BlobStream blobstream = blob.OpenWrite();
                    blobstream.Write(ImageData, 0, ImageData.Count());//將檔案寫入blob中
                    blobstream.Flush();
                    blobstream.Close();
                    fs.Close();

                    if (File.Exists(TempleFileName))
                    {
                        File.Delete(TempleFileName);
                    }
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return "success";
        }

    }
}

