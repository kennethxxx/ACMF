using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Reflection;
using System.IO;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

using System.Data.SqlClient;
using System.Data;
using ICSharpCode.SharpZipLib.Zip;
using System.Configuration;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Threading;
using SevenZip;
using SevenZip.Sdk;
using System.Diagnostics;
using System.IO.Compression;
using MySql.Data.MySqlClient;
using BlobOperator;
using System.ServiceModel.Channels;


namespace ModelManagementServices
{
    // 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼、svc 和組態檔中的類別名稱 "Service1"。
    public class Service1 : IService1
    {
        // currentversion
        string currentversionstring = "V01.00.00.b01.20120213.001";
        private BlobOperatorClass BlobOperatorClient = null;

        public string CurrentVersion()
        {
            Assembly CurrentAsm = Assembly.GetExecutingAssembly();
            if (Attribute.IsDefined(CurrentAsm, typeof(AssemblyDescriptionAttribute)))
            {
                //版本寫在Description
                AssemblyDescriptionAttribute adAttr = (AssemblyDescriptionAttribute)Attribute.GetCustomAttribute(CurrentAsm, typeof(AssemblyDescriptionAttribute));
                if (adAttr != null)
                    return adAttr.Description;
                else
                    return "null";
            }
            return "null";
        }

       string mysqlconn = "SERVER = 140.116.86.172;UserID = root;PASSWORD = auto63906; database = da_in_mysql;charset = utf8";
       static string modelContainerName = "model";
       static string storageConnectionString = "DataConnectionString";
       static string connectionString = ConfigurationManager.AppSettings["DBConnectionString"];
       static string StorageConnectStringBlob = ConfigurationManager.AppSettings["StorageConnectStringBlob"];
       private string DllPath = "";
     //  string tempDir = Path.Combine(ConfigurationManager.AppSettings["EmulatedDiv"], ConfigurationManager.AppSettings["TempDir"]);
       string  tempDir ="";
       string ModelBlobContainerName = ConfigurationManager.AppSettings["ModelBlobContainerName"].ToLower();

        
        public Service1()
        {
            bool IsEmulated = false;
            try
            {
                IsEmulated = RoleEnvironment.IsEmulated;
            }
            catch (System.Exception)
            {
                IsEmulated = true;
            }

            if (IsEmulated)//本機模擬
            {
                tempDir = Path.Combine(ConfigurationManager.AppSettings["EmulatedDiv"], ConfigurationManager.AppSettings["TempDir"]);
                //MCSModelInfoFileName = Path.Combine(tempDir, ConfigurationManager.AppSettings["MCSModelInfoFileName"]);
            }
            else
            {
                LocalResource LocalTempDiv = RoleEnvironment.GetLocalResource(ConfigurationManager.AppSettings["CloudDiv"]);
                tempDir = Path.Combine(LocalTempDiv.RootPath, ConfigurationManager.AppSettings["TempDir"]);

                //MCSModelInfoFileName = ConfigurationManager.AppSettings["MCSModelInfoFileName"];
            }

        }
        




        public string getCNCType(string cncNumber)
        {
            Random r = new Random();
            int rd = r.Next(1, 3);

            return "QP-2040";
        }

        

        //*********************************************************************************************************
        //                          Upload Temp Model(程式切入點)
        //
        //   Step1 將VmRole中 C:\DTtemp\user\Models以及C:\DTtemp\user\temp\裡的檔案包成兩個zip檔,儲存至VmRole中 C:\DTtemp\user\zip中
        //         
        //   Step2 上傳於Blob成功後將VmRole中 C:\DTtemp\user\zip中的zip檔刪掉
        //
        //   Step3 再將 VmRole中 C:\DTtemp\user\Models以及C:\DTtemp\user\temp裡的檔案刪掉
        //
        //**********************************************************************************************************
        public string uploadTempModel(string UserName)//Upload的Main程式  先分別將temp以及Model兩個資料夾的所有檔案分別包成zip檔，存於 C:\DTtemp\Username\zip中，之後zip資料夾內檔案再上傳至雲端
        {
            try
            {
                // 先分別將temp以及Model兩個資料夾的檔案分別包成zip檔上傳至雲端
                string uploadTempStatus = uploadTempBlobData(UserName, "Temp");
                string uploadModelStatus = uploadTempBlobData(UserName, "Models");
                //上傳成功後動作
                if (uploadTempStatus == "successful!" && uploadModelStatus == "successful!")
                {
                    try
                    {
                        // 再將zip資料夾位置的UsernameTemp.zip刪除
                        File.Delete(tempDir + UserName + "/zip/" + UserName + "temp" + ".zip");
                        File.Delete(tempDir + UserName + "/zip/" + UserName + "models" + ".zip");

                        string modelsPath = tempDir + UserName + "/" + "Models"; //最初存放的兩個資料夾(Models & Temp)
                        string tempPath = tempDir + UserName + "/" + "Temp";
                        // 刪除Models裡的檔案
                        foreach (string d in Directory.GetFileSystemEntries(modelsPath))//找到所有資料夾下的檔案
                        {
                            if (File.Exists(d))
                                File.Delete(d); //直接删除其中的文件 
                        }
                        // 刪除temp裡的檔案

                        foreach (string d in Directory.GetFileSystemEntries(tempPath))
                        {
                            if (File.Exists(d))
                                File.Delete(d); //直接删除其中的文件 
                        }
                        return "success";
                    }
                    catch (Exception ex)
                    {
                        return "刪除Models以及temp資料夾錯誤:" + ex.ToString();
                    }
                }
                else
                {
                    return "error";
                }

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        //初步儲存於BLOB
        private string uploadTempBlobData(string UserName, string modelFileName)
        {                                      //UserName          Temp(Models)    
            try
            {
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(StorageConnectStringBlob);
                CloudBlobClient BlobClient = cloudStorageAccount.CreateCloudBlobClient();         //連線字串

                CloudBlobContainer CloudContainer = BlobClient.GetContainerReference("tempmodel");//container
                CloudContainer.CreateIfNotExist();

                NamesByte namesByte = getTempNamesByte(UserName, modelFileName);
                string[] modelList = namesByte.modelNameList.ToArray();//name
                double[] matSizes = namesByte.matSize.ToArray();//size
                byte[] modelSize = namesByte.modelByte;//zip檔
                /****************************************************************************************************

                                                      從本機上傳至 blob
                 
                 *****************************************************************************************************/

                var _blobs = CloudContainer.ListBlobs(new BlobRequestOptions() { UseFlatBlobListing = true });



                string tempPath = @"zip\" + System.DateTime.Now.ToString("yyyy_MM_dd") + @"\";  //zip\yyyy-MM-dd\
                string tempFileName = UserName + modelFileName + ".zip";//cotandyTemp.zip
                var blobs = CloudContainer.ListBlobs(new BlobRequestOptions() { UseFlatBlobListing = true });

                Byte[] item = (Byte[])modelSize;
                string catalog = tempPath + tempFileName;//blob中的位置zip\yyyy-MM-dd\cotandyTemp.zip
                catalog = catalog.ToLower();
                var blob = CloudContainer.GetBlockBlobReference(catalog);
                BlobStream blobstream = blob.OpenWrite();        //string blobAddressUri    The name of the blob, or the absolute URI to the blob.
                blobstream.Write(item, 0, item.Count());
                blobstream.Commit();
                blobstream.Close();
                blob.SetMetadata();

                return "successful!";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public NamesByte getTempNamesByte(string UserName, string fileType)//取得zip檔、name、size
        {                                       // UserName     Temp(Models)
            // ↓先建立要上傳的zip檔的暫時存放位置(把model跟temp抓過去名稱為Zip資料夾內)ex ex C:\DTtempcotandy\zip
            DirectoryInfo di = Directory.CreateDirectory(tempDir + UserName + "/zip");
            // 要上傳的資料夾位置  ↓                    C槽DTtemp      ex C:\DTtemp\cotandy\Temp
            string filePath = tempDir + UserName + "/" + fileType;
            // 要上傳雲端blob的檔案包成zip檔的位置 ↓                      ex C:\DTtemp\cotandy\zip\cotandyTemp.zip
            string uploadZipTemp = tempDir + UserName + "/zip/" + UserName + fileType + ".zip";

            List<string> modelList = new List<string>();//要接收NamesByte的宣告變數(filesname)
            List<double> matSize = new List<double>();//同上(size)
            byte[] modelByt;//同上

            string[] files = Directory.GetFiles(filePath);//從C:\DTtempcotandy\Temp中撈資料ex: a.mat,b.mat, bababab...取得名稱
            //傳回指定目錄中檔案的(名稱)包含路徑
            foreach (string file in files)//將每個找到的檔案資訊使用已宣告的變數儲存
            {
                FileInfo f = new FileInfo(file); // FileInfo提供建立、複製、刪除、移動和開啟檔案的執行個體 (Instance) 方法 
                matSize.Add(f.Length);//所有mat檔大小存於list double陣列中


                string modelName = f.Name.ToString();
                if (modelName.IndexOf(".xml") >= 1)
                {
                    modelName = f.Name.ToString().Replace(".xml", "");

                }
                else
                    modelName = f.Name.ToString().Replace(".mat", "");
                modelList.Add(modelName);//將每個名字有.mat的字串.mat去掉後存於list字串陣列中(為了在blob中不要顯現.mat)
            }

            string[] filenames = Directory.GetFiles(filePath);//ex: a.mat,b.mat, bababab...取得名稱
            byte[] buffer = new byte[4096];

            using (ZipFile zipFile = ZipFile.Create(uploadZipTemp))//在zip資料夾中壓縮暫存資料(壓縮資料夾路徑)
            {                                    //zipFilePath C:\DTtemp\cotandy\zip\cotandyTemp.zip
                zipFile.BeginUpdate();
                foreach (string file in filenames)
                {
                    zipFile.Add(file, System.IO.Path.GetFileName(file)); //取得檔案名稱包含副檔名,全部包在
                }                                                        //C:\DTtempcotandy\zip\cotandyTemp.zip中

                zipFile.CommitUpdate();   //包裝完成,將包裝完資料轉成byte 2進制檔等待上傳至blob

            }

            modelByt = File.ReadAllBytes(uploadZipTemp);//開啟二進位檔案，將檔案內容讀入位元組陣列，然後關閉檔案 。
            NamesByte namesByte = new NamesByte()
            {
                modelByte = modelByt,//zip檔
                modelNameList = modelList, //name
                matSize = matSize //size
            };
            return namesByte;
        }
       



        //*******************************************************************************
        //                          Download Temp MODEL (下載完砍掉,留最終Models)
        //********************************************************************************
        public string getTempBlob(string UserName)
        {
            try
            {
                DirectoryInfo di = Directory.CreateDirectory(tempDir + UserName + "/zip");//避免多個instance下載後找不到路徑

                string readModelsStatus = readTempBlobData(UserName, "models");//利用name和Type去blob中找zip檔
                string readTempStatus = readTempBlobData(UserName, "temp");
                string deleteModels = deleteTempBlobData(UserName, "models");//下載完砍掉暫存於blob中的  ex: cotandyTemp.zip
                string deleteTemp = deleteTempBlobData(UserName, "temp");
                File.Delete(tempDir + UserName + "/zip/" + UserName + "temp" + ".zip");//砍掉VmRole中的zip檔
                File.Delete(tempDir + UserName + "/zip/" + UserName + "models" + ".zip");

                if (readModelsStatus == "success" && readTempStatus == "success" && deleteModels == "successful!" && deleteTemp == "successful!")
                {
                    return "success";
                }
                else
                {
                    return "error";
                }

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        private string readTempBlobData(string userName, string modelFileType)
        {                                      //cotandy       Temp(Models)
            string blobpath = null;
            try
            {
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(StorageConnectStringBlob);
                CloudBlobClient BlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer CloudContainer = BlobClient.GetContainerReference("tempmodel");
                CloudContainer.CreateIfNotExist();
                var bloblists = CloudContainer.ListBlobs(new BlobRequestOptions() { UseFlatBlobListing = true });
                CloudBlob targetblob = null;
                byte[] _modelBytes = null;
                string zipName = userName + modelFileType; //下載後的zip檔檔案名稱
                zipName = zipName.ToLower();
                // 要下載解壓縮檔案的資料夾位置 ↓         
                string uncompressFilePath = tempDir + userName + "/" + modelFileType;  //ex C:\DTtemp\cotandy\Models
                // 要下載zip檔的資料夾位置  ↓              
                string downloadZipPath = tempDir + userName + "/zip/" + userName + modelFileType + ".zip";  // C:\DTtemp\cotandy\zip\cotandyTemp.zip

                foreach (var _blob in bloblists)
                {
                    if (_blob.Uri.Segments[_blob.Uri.Segments.Count() - 1].ToString().Replace(".zip", "") == zipName)
                    {           //找到ex:cotandyModels
                        blobpath = _blob.Uri.ToString();//當前blob的完整路徑
                        targetblob = (CloudBlob)_blob;
                        _modelBytes = targetblob.DownloadByteArray();
                        //filemode.append在檔案存在時開啟它並搜尋至檔案末端，或建立新檔案。
                        FileStream writer = new FileStream(downloadZipPath, FileMode.Append);   //PATH 目前 FileStream 物件將會封裝之檔案的相對或絕對路徑。FileMode 常數，判斷如何開啟或建立檔案。
                        writer.Write(_modelBytes, 0, _modelBytes.Length);
                        writer.Close();
                        Uncompress(downloadZipPath, uncompressFilePath);
                    }                    //source         target

                }
                return "success";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public string deleteTempBlobData(string dataID, string modelFileType)//建模未完成
        {                                   //username         Temp(Models)
            try
            {
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(StorageConnectStringBlob);
                CloudBlobClient BlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer CloudContainer = BlobClient.GetContainerReference("tempmodel");//container
                CloudContainer.CreateIfNotExist();
                var blobList = CloudContainer.ListBlobs(new BlobRequestOptions() { UseFlatBlobListing = true });

                bool fileExist = false;
                string zipName = dataID + modelFileType;//cotandyTemp
                zipName = zipName.ToLower();
                foreach (var blob in blobList)
                {                              //刪除blob中暫存的zip檔
                    if (blob.Uri.Segments[blob.Uri.Segments.Count() - 1].ToString().Replace(".zip", "") == zipName)
                    {
                        var deleteBlob = CloudContainer.GetBlobReference(blob.Uri.ToString());//當前blob的完整路徑
                        deleteBlob.Delete();
                        fileExist = true;
                    }
                }

                if (fileExist == true)
                {
                    return "successful!";
                }
                else
                {
                    return "file not exists!";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        /*******************************************************************************************************************
         
      

                 UPLOAD MODEL(傳最終版本Model至blob並以數字代表zip的版本   &   傳Metadata至SQL Azure   )
       


        //*******************************************************************************************************************/
        public string uploadModel(ModelStorationInformation modelStorationInformation, string UserName)
        {
            try
            {
                modelStorationInformation.createTime = System.DateTime.Now; //當metadata存到sql azure時,表示Models建好了(final)
                string uploadFinalModelStatus = uploadBlobData(modelStorationInformation, UserName);//傳最終Models到blob的function
                return "success";
            }
            catch (Exception)
            {
                return "false" + "2"; ;
            }
        }

        /**********************************************************************************************
             
                                   
                                                上傳最終模型 
         
             Step1.先將由modelStorationInformaiton class取得的模型資料存於Sql Azure中並以pk作為搜尋唯一鍵(檔)
             Step2.將模型存於Blob中再用pk.zip作為模型檔(唯一)

        ************************************************************************************************/
        private string uploadBlobData(ModelStorationInformation modelStorationInformaiton, string UserName)
        {                                     //對modelStorationInformaiton實作

            try
                {

                    String TmpModelFileLocalPath = tempDir + UserName + "\\";
                    String TmpModelFileLocalPathmodleinfo = TmpModelFileLocalPath + "\\" + "Models\\";

                    //================================================================
                    // 下載暫存資料
                    //================================================================

                    BlobOperatorClass BlobOperatoClient = new BlobOperatorClass(StorageConnectStringBlob, tempDir, UserName + ".tmp");
                    BlobOperatoClient.DownloadAsDirectory(TmpModelFileLocalPath, ModelBlobContainerName, UserName + "/temporary.file", true, true);
                    BlobOperatoClient.DownloadAsFile(TmpModelFileLocalPathmodleinfo + "MCS_MCSModelInfo.xml", ModelBlobContainerName, UserName + "/MCS_MCSModelInfo.xml", true);

                //================================================================
                // SQLAzure 
                //================================================================
                //string matSizeList = null;          //未來要刪
                //List<string> strmatSizes = new List<string>();//size轉為個別的string LIST
                //matSizeList = string.Join(",", strmatSizes);//size
                  
                string PK = Guid.NewGuid().ToString("N");
                string COMPANY = modelStorationInformaiton.COMPANY;
                DateTime createTime = DateTime.Now.Date;
                string vMachineID = modelStorationInformaiton.vMachineID;
                string CNCType = modelStorationInformaiton.CNCType;
                string cnc_number = modelStorationInformaiton.cnc_number;
                DateTime dateStartTime = modelStorationInformaiton.dataStartTime;
                DateTime dataEndTime = modelStorationInformaiton.dataEndTime;
                string createUser = modelStorationInformaiton.createUser;
                string Model_size = Get_ModelSize(TmpModelFileLocalPath);                                             
                string ProductID = modelStorationInformaiton.ProductID;                   
                string ServiceBrokerID = modelStorationInformaiton.ServiceBrokerID;
                
                 //將資料放入SQL Azure  
                SqlConnection con = new SqlConnection(connectionString);
                con.Close();
                con.Open();
                int ModelID = 0;
                int timecount = 0;
                string selectTime = "SELECT createTime from STRATEGY3 WHERE(Company='" + COMPANY + "' AND createUser='" + createUser + "'AND vMachineID='" + vMachineID + "'AND cnc_number='" + cnc_number + "'AND CNCType='" + CNCType + "'AND ProductID='" + ProductID + "' )";
                SqlCommand command = new SqlCommand(selectTime, con);
                SqlDataReader reader = command.ExecuteReader();

                // Call Read before accessing data.
                while (reader.Read())
                {
                    int i = 0;
                    if (DateTime.Now.Date == Convert.ToDateTime(reader[i]).Date)
                    {
                        timecount++;
                    } i++;
                }
                ModelID = timecount;
                if (ModelID == 0)//一開始或者上述有一者錯誤
                {
                    ModelID++;
                }
                else
                    ModelID = timecount + 1;
                con.Close();
                con.Open();                                                                                                                                                                                                                                                                                                                                                                                                                                                                        //未來要刪                                   //未來要刪                                  
                string sqlCommand = "INSERT INTO STRATEGY3(PK,COMPANY,createUser,ModelID,vMachineID,CNCType,cnc_number,createTime,dateStartTime,dataEndTime,modelSize,ProductID,ServiceBrokerID)VALUES('" + PK + "','" + COMPANY + "','" + createUser + "','" + ModelID + "','" + vMachineID + "','" + CNCType + "','" + cnc_number + "','" + DateTime.UtcNow.AddHours(0).ToString("yyyy-MM-dd HH:mm:ss") + "','" + dateStartTime.ToString("yyyy-MM-dd HH:mm:ss") + "','" + dataEndTime.ToString("yyyy-MM-dd HH:mm:ss") + "','" + Model_size + "','" + ProductID + "','"+ ServiceBrokerID+ "')";
                SqlCommand comm = new SqlCommand(sqlCommand, con);
                comm.ExecuteNonQuery();
                con.Close();
                //================================================================
                // 把Model上傳到Blob
                //================================================================
                    
                string Download_Index = PK;  //建最新的zip(in  blob's  model)
              
                string tempPath = @"\" + DateTime.Now.ToString("yyyy_MM_dd") + @"\";//yyyy_MM_dd\

                string tempFileName = Download_Index + ".zip";  //(最新的)pk.zip

                BlobOperatoClient.UploadAsDirectory(TmpModelFileLocalPath + @"Models", modelContainerName, tempPath + tempFileName, true);

                //================================================================
                // 刪除本機的 UserName/temporary.file 和 Model.zip
                //================================================================
              
                if (System.IO.Directory.Exists(tempDir + UserName)) //ex: @"C:\DTtemp\eMRC_dodo"
                {
                    try
                    {
                        System.IO.Directory.Delete(tempDir + UserName, true);
                    }

                    catch (System.IO.IOException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
              
                return "Store Model Success";
            } 
            catch (Exception ex)
            {
                return ex.Message.ToString() + UserName + "this is"; ;
            }
        }
        private string Get_ModelSize(string DirectoryPath) //取得zip檔、name、size
        {

            string FinalModelPath = DirectoryPath + @"Models"; //因為是final所以只要上傳Models  C:\\DTtemp\\eMRC_dodo\\Models

            DirectoryInfo Modrel_DirectoryPath = new DirectoryInfo(FinalModelPath);

            long Model_Size = 0;
            foreach (FileInfo Files in Modrel_DirectoryPath.GetFiles("*.*", SearchOption.AllDirectories))
            {
                Model_Size += Files.Length;
            }

            double Model_Size_KB = Model_Size / 1024.0;


            double Model_Size_MB = Model_Size / (1024.0 * 1024.0);

           
            return Model_Size_MB.ToString();
        }

        //***********************************************************************
        //                          DELETE MODEL
        //
        //                    Step1 先利用deleteBlobData刪除雲端之Bolb
        //                    Step2 再利用deleteModelInformation刪除SQL Azure之blob的Metadata
        //
        //************************************************************************
        public string deleteModel(string modelName)//引用function
        {
            try
            {

                deleteBlobData(modelName);

                if (deleteModelInformation(modelName) == "success")
                {
                    return "success!";
                }
                else
                {
                    return "fail in delete modelInformaiton!";
                }

            }
            catch (Exception ex)
            {
                return "fail!";
            }
        }
        private string deleteBlobData(string PK_ID)
        {
            try
            {
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(StorageConnectStringBlob);
                CloudBlobClient BlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer CloudContainer = BlobClient.GetContainerReference(modelContainerName);
                CloudContainer.CreateIfNotExist();                                      //Model
                var blobList = CloudContainer.ListBlobs(new BlobRequestOptions() { UseFlatBlobListing = true });



                bool fileExist = false;

                foreach (var blob in blobList)
                {
                    if (blob.Uri.Segments[blob.Uri.Segments.Count() - 1].ToString().Replace(".zip", "") == PK_ID)
                    {
                        var deleteBlob = CloudContainer.GetBlobReference(blob.Uri.ToString());
                        deleteBlob.Delete();
                        fileExist = true;
                    }
                }

                if (fileExist == true)
                {
                    return "successful!";
                }
                else
                {
                    return "file not exists!";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        private string deleteModelInformation(string PK_ID)
        {
            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                con.Close();
                con.Open();
                string sqlString = "DELETE FROM [STRATEGY3] WHERE PK = " + PK_ID + "";


                SqlCommand command = new SqlCommand(sqlString, con);
                command.ExecuteNonQuery();
                con.Close();
                return "success";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        //*******************************************************************************************
        //                                  模型及機台查詢功能模組
        //
        //             Step1 Client啟動時會呼叫"getModelFilterParameter"取得篩選模型之參數(sensor、modelStartDate、modelEndDate、cncType) 
        //              Step2 使用者輸入篩選的參數後呼叫getModelInformationList
        //              Step3 getModelInformationList先呼叫getModelIDList取的符合的ModelID
        //              Step4 getModelInformationList再利用modelIDgetModelInformationListModual查詢Model
        // 
        // *******************************************************************************************

        public List<ModelInformation> getAllModelInformation()
        {
            List<ModelInformation> _modelInformationList = new List<ModelInformation>();

            DataSet ds = new DataSet();
            SqlConnection con = new SqlConnection(connectionString);
            con.Close();
            con.Open();
            string sqlString = "SELECT [PK]" +
                              " ,[COMPANY] " +
                              " ,[CrateTime] " +
                              " ,[vMachineID] " +
                              " ,[CNCType] " +
                              " ,[cnc_number] " +
                              " ,[ModelID] " +
                              " ,[ProductID] " +        //未來要刪
                              " ,[dateStartTime] " +
                              " ,[dataEndTime] " +
                              " ,[createUser] " +
                //  " ,[modelNameList] " +     //未來要刪
                //   " ,[modelSize] " +
                              " ,[matSizeList] " +      //未來要刪
                          "  FROM [STRATEGY3]";

            SqlDataAdapter dapt = new SqlDataAdapter(sqlString, con);
            dapt.Fill(ds);//將找到的資料塞進ds中
            con.Close();


            int rowCount = ds.Tables[0].Rows.Count; //找出有多少筆record
            //_modelInformationList設定屬性
            for (int x = 0; x < rowCount; x++)
            {
                List<double> _matSize = new List<double>();
                //   List<string> _modelNames = new List<string>();



                string[] matSizes = ds.Tables[0].Rows[x]["matSizeList"].ToString().Split(',');//未來要刪
                foreach (string matsize in matSizes)
                {
                    _matSize.Add(Convert.ToDouble(matsize));
                }

                //string[] fileNameList = ds.Tables[0].Rows[x]["modelNameList"].ToString().Split(',');//未來要刪
                //foreach (string fileName in fileNameList)
                //{
                //    _modelNames.Add(fileName);
                //}
                _modelInformationList.Add(new ModelInformation()
                {
                    PK = ds.Tables[0].Rows[x]["PK"].ToString(),
                    ProductID = ds.Tables[0].Rows[x]["ProductID"].ToString(),      //未來要刪
                    COMPANY = ds.Tables[0].Rows[x]["COMPANY"].ToString(),
                    createTime = Convert.ToDateTime(ds.Tables[0].Rows[x]["createTime"].ToString()),
                    vMachineID = ds.Tables[0].Rows[x]["vMachineID"].ToString(),
                    CNCType = ds.Tables[0].Rows[x]["CNCType"].ToString(),
                    cnc_number = ds.Tables[0].Rows[x]["cnc_number"].ToString(),
                    ModelID = ds.Tables[0].Rows[x]["ModelID"].ToString(),
                    dataStartTime = Convert.ToDateTime(ds.Tables[0].Rows[x]["dataStartTime"].ToString()),
                    dataEndTime = Convert.ToDateTime(ds.Tables[0].Rows[x]["dataEndTime"].ToString()),
                    createUser = ds.Tables[0].Rows[x]["createUser"].ToString(),
                    matSizeList = _matSize.ToArray(),
                    //   modelNameList = _modelNames.ToArray(),
                    modelSize = Convert.ToDouble(ds.Tables[0].Rows[x]["modelSize"].ToString()),
                });
            }

            return _modelInformationList;
        }

        public ModelFilterParameters getModelFilterParameter(string UserCompany)//找篩選模型之參數(sensor、modelStartDate、modelEndDate、cncType)
        {
            ModelFilterParameters modelFilterParameter = new ModelFilterParameters();
            try
            {

                SqlConnection con = new SqlConnection(connectionString);
                string commandOfTime = "SELECT [dateStartTime] AS STARTTIME, [dataEndTime] AS ENDTIME FROM [STRATEGY3]";//as為資料庫更改表格呈現的名稱
                string commandOfvMachineID = "SELECT DISTINCT(vMachineID) FROM [STRATEGY3]";   //Distinct vMachineID種類
                string commandOfcnc_number = "SELECT DISTINCT(cnc_number) FROM [STRATEGY3]";   //Distinct cnc_number種類
                string commandOfProductID = "SELECT DISTINCT(ProductID) FROM [STRATEGY3]";  //Distinct ProductID種類
                string commandOfServiceBrokerID = "SELECT ServiceBrokerID FROM [ServiceBrokerInfo] WHERE Company = '"+ UserCompany+"'";  //Distinct ServiceBrokerID種類

                SqlDataAdapter daptOfServiceBrokerID = new SqlDataAdapter(commandOfServiceBrokerID, con);
                SqlDataAdapter daptOfvMachineID = new SqlDataAdapter(commandOfvMachineID, con);
                SqlDataAdapter daptOfTime = new SqlDataAdapter(commandOfTime, con);
                SqlDataAdapter daptOfcnc_number = new SqlDataAdapter(commandOfcnc_number, con);
                SqlDataAdapter daptOfProductID = new SqlDataAdapter(commandOfProductID, con);

                DataSet timeDS = new DataSet();
                DataSet vMachineIDDS = new DataSet();
                DataSet cnc_numberDS = new DataSet();
                DataSet ProductIDDS = new DataSet();
                DataSet ServiceBrokerIDS = new DataSet();

                // Fill Dataset

                daptOfTime.Fill(timeDS);
                daptOfvMachineID.Fill(vMachineIDDS);
                daptOfcnc_number.Fill(cnc_numberDS);
                daptOfProductID.Fill(ProductIDDS);
                daptOfServiceBrokerID.Fill(ServiceBrokerIDS);

                con.Close();

                //取得時間區間
                DateTime startTime = DateTime.Now.AddDays(1);   //目前時間加1天
                DateTime endTime = new DateTime();              // 起始日期
                int rowNum = timeDS.Tables[0].Rows.Count;
                for (int x = 0; x < rowNum; x++)
                {
                    if (Convert.ToDateTime(timeDS.Tables[0].Rows[x]["STARTTIME"]) < startTime)
                    {
                        startTime = Convert.ToDateTime(timeDS.Tables[0].Rows[x]["STARTTIME"]);
                    }

                    if (Convert.ToDateTime(timeDS.Tables[0].Rows[x]["ENDTIME"]) > endTime)
                    {
                        endTime = Convert.ToDateTime(timeDS.Tables[0].Rows[x]["ENDTIME"]);
                    }
                }


                // 取得參數種類名稱
                List<string> vMachineIDs = new List<string>();
                List<string> cnc_numbers = new List<string>();
                List<string> ProductIDs = new List<string>();
                List<string> ServiceBrokers = new List<string>();

                vMachineIDs.Add("All");  //加上預設全選字串 "All"
                cnc_numbers.Add("All");
                ProductIDs.Add("All");
               // ServiceBrokers.Add("All");

                int vMachineIDNum = vMachineIDDS.Tables[0].Rows.Count;
                int cnc_numberNum = cnc_numberDS.Tables[0].Rows.Count;
                int ProductIDNum = ProductIDDS.Tables[0].Rows.Count;
                int ServiceBrokerIDNum = ServiceBrokerIDS.Tables[0].Rows.Count;

                for (int x = 0; x < vMachineIDNum; x++)
                {
                    vMachineIDs.Add(vMachineIDDS.Tables[0].Rows[x][0].ToString()); // 因為只有一欄，所以欄位名稱不用指定
                }
                for (int x = 0; x < cnc_numberNum; x++)
                {
                    cnc_numbers.Add(cnc_numberDS.Tables[0].Rows[x][0].ToString());
                }
                for (int x = 0; x < ProductIDNum; x++)
                {
                    ProductIDs.Add(ProductIDDS.Tables[0].Rows[x][0].ToString());
                }
                for (int x = 0; x < ServiceBrokerIDNum; x++)
                {
                    ServiceBrokers.Add(ServiceBrokerIDS.Tables[0].Rows[x][0].ToString());
                }


                modelFilterParameter.modelStartDate = startTime;
                modelFilterParameter.modelEndDate = endTime;
                modelFilterParameter.vMachineID = vMachineIDs;
                modelFilterParameter.cnc_number = cnc_numbers;
                modelFilterParameter.ProductID = ProductIDs;
                modelFilterParameter.ServiceBrokerID = ServiceBrokers;

                return modelFilterParameter;
            }
            catch (Exception)
            {
                return modelFilterParameter;
            }
        }

        public ServiceBrokerServices.Vmachine[] getVmachineInformationList()
        {
            ServiceBrokerServices.Service1Client client = new ServiceBrokerServices.Service1Client();
            return client.getVmachineInformationList();
        } // Provide to huy     //未來要刪掉


        public List<ModelManagementServices.ServiceReference1.EquipmentInformation> getEquipmentInformationList(string ServiceBrokerID,string company)
        {
            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                string commandOfServiceBrokerIP = "SELECT ServiceBrokerIP FROM [ServiceBrokerInfo] WHERE ServiceBrokerID = '" + ServiceBrokerID + "' And Company = '" +company+"'" ;  //得到指定的ServiceBrokerIP種類

                SqlDataAdapter daptOfServiceBrokerIP = new SqlDataAdapter(commandOfServiceBrokerIP, con);
                DataSet ServiceBrokerIPS = new DataSet();
                daptOfServiceBrokerIP.Fill(ServiceBrokerIPS);
                string ServiceBrokerIP = ServiceBrokerIPS.Tables[0].Rows[0]["ServiceBrokerIP"].ToString(); 

                con.Close();
                // 請依需求設定對應之 Binding。
                Binding wcfBinding = new BasicHttpBinding();

                // 請依實際 URL 位置進行設定。
                EndpointAddress wcfEndpointAddress = new EndpointAddress("http://"+ServiceBrokerIP+"/ServiceBroker/Service1.svc");

               // WCFService.ServiceClient wcfService = new WCFService.ServiceClient(wcfBinding, wcfEndpointAddress);
                ServiceReference1.Service1Client ei = new ServiceReference1.Service1Client(wcfBinding, wcfEndpointAddress);
                List<ServiceReference1.EquipmentInformation> equipmentInformationList = new List<ServiceReference1.EquipmentInformation>();

              
             
                equipmentInformationList = ei.getEquipmentInformationList();

           
                //=====================

                return equipmentInformationList;
            }
            catch (Exception ex) {
                FileStream a;
                StreamWriter b;
                //1.檔案路徑。必須先建立檔案。 2.開啟檔案  3.執行檔案的寫入
                a = new FileStream(@"D:\DTtemp\test.txt", FileMode.Open, FileAccess.Write);
                b = new StreamWriter(a);
                b.WriteLine(ex.ToString());
                b.Flush();
                b.Close();
                return null;
               
            }

        } // Provide to huy          //未來要刪掉

        public string getEquipmentInformationLists(string ServiceBrokerID, string company)
        {
            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                string commandOfServiceBrokerIP = "SELECT ServiceBrokerIP FROM [ServiceBrokerInfo] WHERE ServiceBrokerID = '" + ServiceBrokerID + "' And Company = '" + company + "'";  //得到指定的ServiceBrokerIP種類
                SqlDataAdapter daptOfServiceBrokerIP = new SqlDataAdapter(commandOfServiceBrokerIP, con);
                DataSet ServiceBrokerIPS = new DataSet();
                daptOfServiceBrokerIP.Fill(ServiceBrokerIPS);
                string ServiceBrokerIP = ServiceBrokerIPS.Tables[0].Rows[0]["ServiceBrokerIP"].ToString();
                con.Close();
                Binding wcfBinding = new BasicHttpBinding();
                EndpointAddress wcfEndpointAddress = new EndpointAddress("http://" + ServiceBrokerIP + "/ServiceBroker/Service1.svc");
                // WCFService.ServiceClient wcfService = new WCFService.ServiceClient(wcfBinding, wcfEndpointAddress);
                ServiceReference1.Service1Client ei = new ServiceReference1.Service1Client(wcfBinding, wcfEndpointAddress);
               // List<ServiceReference1.EquipmentInformation> equipmentInformationList = new List<ServiceReference1.EquipmentInformation>();
               // equipmentInformationList = ei.getEquipmentInformationList();
                return ei.getEquipmentInformationList1();
            }
            catch (Exception ex)
            {
                FileStream a;
                StreamWriter b;
                //1.檔案路徑。必須先建立檔案。 2.開啟檔案  3.執行檔案的寫入
                a = new FileStream(@"D:\DTtemp\test.txt", FileMode.Open, FileAccess.Write);
                b = new StreamWriter(a);
                b.WriteLine(ex.ToString());
                b.Flush();
                b.Close();
                return null;

            }

        } // Provide to huy          //未來要刪掉
 
        public List<ModelInformation> getModelInformationList(string ServiceBrokerID, string vMachineID, string cnc_number, string ProductID, DateTime creationStartDate, DateTime creationEndDate, string userName, string userCompany)
        {
            SqlConnection con = new SqlConnection(connectionString);
            List<ModelInformation> ReturnModelInformationList = new List<ModelInformation>();

            string _PK = null;
            string _COMPANY = null;
            string _ProductID = null;
            string _createTime = null;
            string _vMachineID = null;
            string _CNCType = null;
            string _cnc_number = null;
            string _ModelID = null;
            string _datastarttime = null;
            string _dataendtime = null;
            string _createUser = null;
            List<double> _matSizeList = new List<double>();
            // List<string> _modelNames = new List<string>();
            double _modelSize = 0.0;
            string _ServiceBrokerID = null;

            string cmd = "SELECT" +
              "[PK]" +
              ",[COMPANY]" +
              ",[createUser]" +
              ",[createTime]" +
              ",[vMachineID]" +
              ",[CNCType]" +
              ",[cnc_number]" +
              ",[ModelID]" +
              ",[ProductID]" +
              ",[dateStartTime]" +
              ",[dataEndTime]" +

           //   ",[matSizeList]" +             //未來要刪
                //   ",[modelNameList]" +          //未來要刪
              ",[modelSize]" +
              ",[ServiceBrokerID]" +
              "FROM [STRATEGY3] ";
            cmd += "WHERE CONVERT(VARCHAR(24),createTime,120) BETWEEN '" + creationStartDate.AddHours(-8).ToString("yyyy-MM-dd HH:mm:ss") + "' AND '" + creationEndDate.AddHours(-8).ToString("yyyy-MM-dd HH:mm:ss") + "' " +
             "AND COMPANY='" + userCompany + "' " +
             "AND createUser='" + userName + "' ";

            if (vMachineID != "All")
            {
                cmd += "AND vMachineID='" + vMachineID + "' ";
            }

            if (cnc_number != "All")
            {
                cmd += "AND cnc_number='" + cnc_number + "' ";
            }

            if (ProductID != "All")
            {
                cmd += "AND ProductID='" + ProductID + "' ";
            }
            if (ServiceBrokerID != "All")
            {
               // cmd += "AND ServiceBrokerID='" + ServiceBrokerID + "' ";
            }

            cmd += "ORDER BY createTime DESC ";


            DataSet ds = new DataSet();
            SqlDataAdapter dapt = new SqlDataAdapter(cmd, con);
            dapt.Fill(ds);


            int TotalModelCount = ds.Tables[0].Rows.Count;


            for (int x = 0; x < TotalModelCount; x++)
            {
                if (ReturnModelInformationList.Count >= 105)
                {
                    continue; //太多的就不放進去
                }

                try
                {
                    _ModelID = ds.Tables[0].Rows[x]["ModelID"].ToString();
                    _PK = ds.Tables[0].Rows[x]["PK"].ToString();
                    _COMPANY = ds.Tables[0].Rows[x]["COMPANY"].ToString();
                    _createTime = ds.Tables[0].Rows[x]["createTime"].ToString();
                    _ProductID = ds.Tables[0].Rows[x]["ProductID"].ToString();//未來要刪
                    _vMachineID = ds.Tables[0].Rows[x]["vMachineID"].ToString();
                    _CNCType = ds.Tables[0].Rows[x]["CNCType"].ToString();
                    _cnc_number = ds.Tables[0].Rows[x]["cnc_number"].ToString();
                    _datastarttime = ds.Tables[0].Rows[x]["dateStartTime"].ToString();
                    _dataendtime = ds.Tables[0].Rows[x]["dataEndTime"].ToString();
                    _createUser = ds.Tables[0].Rows[x]["createUser"].ToString();
                    _ServiceBrokerID = ds.Tables[0].Rows[x]["ServiceBrokerID"].ToString();

                    //   string[] matSizes = ds.Tables[0].Rows[x]["matSizeList"].ToString().Split(',');  //未來要刪
                    //foreach (string matsize in matSizes)
                    //{
                    //    _matSizeList.Add(Convert.ToDouble(matsize));
                    //}
                    //string[] fileNameList = ds.Tables[0].Rows[x]["modelNameList"].ToString().Split(',');//未來要刪

                    //foreach (string fileName in fileNameList)
                    //{
                    //    _modelNames.Add(fileName);
                    //}
                    _modelSize = Convert.ToDouble(ds.Tables[0].Rows[x]["modelSize"].ToString());

                    ReturnModelInformationList.Add(new ModelInformation()
                    {
                        PK = _PK,
                        COMPANY = _COMPANY,
                        ProductID = _ProductID,
                        CNCType = _CNCType,
                        dataEndTime = Convert.ToDateTime(_dataendtime),
                        dataStartTime = Convert.ToDateTime(_datastarttime),
                        createTime = Convert.ToDateTime(_createTime).AddHours(8),
                        vMachineID = _vMachineID,
                        createUser = _createUser,
                        ModelID = _ModelID,
                        // modelNameList = _modelNames.ToArray(),
                        //  matSizeList = _matSizeList.ToArray(),
                        modelSize = _modelSize,
                        cnc_number = _cnc_number,
                        ServiceBrokerID = _ServiceBrokerID,
                    });
                }
                catch (System.Exception)
                {

                }

                //_matSizeList.Clear();
                //_modelNames.Clear();
            }

            if (ReturnModelInformationList.Count > 100)
            {
                while (ReturnModelInformationList.Count > 100)
                {
                    ReturnModelInformationList.RemoveAt(0);
                }
                ReturnModelInformationList.Add(null); //只要回傳時檢查筆數為0~100都算正常 101則是還有資料要抓
            }

            return ReturnModelInformationList;
        }





        //***************************************************************************************************************************
        // 
        //                          FAN OUT MODEL 
        //    Step1 使用者呼叫fanOutModel下達fan out model , fanOutModel呼叫ServiceBroker
        //    Step2 vMachine為了解決檔案大小問題會呼叫ServiceBroker ,ServiceBroker再呼叫fanOutModelControl
        //    Step3 fanOutModelControl呼叫readBlobData取得模型資料丟給fanOutModelControl
        //    Step4 fanOutModelControl再將模型與要fan out的資訊丟給vMachine
        //    Step5 vMachine response給Service Broker ,Service Broker response給 Model Management
        //****************************************************************************************************************************/
        public bool fanOutModel(List<Model_SendContent> ModelInfo)
        {                          //給modelID          通知service Broker找EquipmentList 機台資訊()          給username
            try
            { 
                //找IP
                string [] v_MachineIP= new string[ModelInfo.Count];
              //  string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["MysqlConnectionString"].ConnectionString;
               // ServiceBrokerServices.Service1Client vip = new ServiceBrokerServices.Service1Client();

                SqlConnection con = new SqlConnection(connectionString);
                string commandOfServiceBrokerIP = "SELECT ServiceBrokerIP FROM [ServiceBrokerInfo] WHERE ServiceBrokerID = '" + ModelInfo[0].ServiceBrokerID + "' And Company = '" + ModelInfo[0].Company + "'";  //得到指定的ServiceBrokerIP種類

                SqlDataAdapter daptOfServiceBrokerIP = new SqlDataAdapter(commandOfServiceBrokerIP, con);
                DataSet ServiceBrokerIPS = new DataSet();
                daptOfServiceBrokerIP.Fill(ServiceBrokerIPS);
                string ServiceBrokerIP = ServiceBrokerIPS.Tables[0].Rows[0]["ServiceBrokerIP"].ToString();

                con.Close();
                // 請依需求設定對應之 Binding。
                Binding wcfBinding = new BasicHttpBinding();

                // 請依實際 URL 位置進行設定。
                EndpointAddress wcfEndpointAddress = new EndpointAddress("http://" + ServiceBrokerIP + "/ServiceBroker/Service1.svc");

                // WCFService.ServiceClient wcfService = new WCFService.ServiceClient(wcfBinding, wcfEndpointAddress);
                ServiceBrokerServices.Service1Client vip = new ServiceBrokerServices.Service1Client(wcfBinding, wcfEndpointAddress);
                ServiceReference1.Service1Client Model= new ServiceReference1.Service1Client(wcfBinding, wcfEndpointAddress);


              


                //找相對應的SB服務裡的VM_IP
           
                    v_MachineIP = vip.get_vMachineIP(ModelInfo[0].vMachineID, ModelInfo.Count);

                    
                

                //找CreateTime
                List<ServiceReference1.Model_GetContent> FanOutModelInfo = new List<ServiceReference1.Model_GetContent>();   
                SqlConnection sqlcon = new SqlConnection(connectionString);
                        //撈出個別record
                        string date;
                        for (int i = 0; i < ModelInfo.Count; i++)
                        { ServiceReference1.Model_GetContent FOMI = new ServiceReference1.Model_GetContent();
                            string cmdd = "SELECT " +
                                          "createTime,ProductID " +
                                          "FROM STRATEGY3 WHERE PK ='" + ModelInfo[i].PK + "'";
                            DataTable sqlds = new DataTable();
                            SqlDataAdapter sqldapt = new SqlDataAdapter(cmdd, sqlcon);
                            sqldapt.Fill(sqlds);
                            //int count = sqlds.Rows.Count;
                            date = sqlds.Rows[0]["createTime"].ToString();
                            FOMI.cretateTime = Convert.ToDateTime(date).ToString("yyyy_MM_dd");
                            FOMI.PK = ModelInfo[i].PK;
                            FOMI.CNC_ID = ModelInfo[i].CNCID;
                            // FanOutModelInfo[i].cretateTime=ModelInfo[i].;
                            FOMI.ProductName = sqlds.Rows[0]["ProductID"].ToString();
                            FOMI.vMachineIP = v_MachineIP[i];
                            FanOutModelInfo.Add(FOMI);    
                        }
                        sqlcon.Close();
                        
                        Model.Endpoint.Binding.OpenTimeout = TimeSpan.FromMinutes(30);
                        Model.Endpoint.Binding.ReceiveTimeout = TimeSpan.FromMinutes(30);
                        Model.Endpoint.Binding.SendTimeout = TimeSpan.FromMinutes(30);
                        Model.Endpoint.Binding.CloseTimeout = TimeSpan.FromMinutes(30);
                        return Model.Get_Model_From_MM(FanOutModelInfo);
                    

              //  ServiceReference1.Service1Client Model = new ServiceReference1.Service1Client();
        
            }
            catch (Exception ex)
            {
                return false;
            }



        }



        public ServiceBrokerServices.ModelFull fanOutModelControl(string PK, List<FanOutEquipmentInformation> EquipmentList, string user)
        {
            try
            {
                //return vMachineList、equipmentList                      vMachineName  equipmentName
                ServiceBrokerServices.Model model = readBlobData(PK, user);
                if (model != null)
                {

                    List<string> vMachineList = new List<string>();
                    List<string> equipmentList = new List<string>();
                    foreach (FanOutEquipmentInformation info in EquipmentList)
                    {
                        vMachineList.Add(info.vMachineName);
                        equipmentList.Add(info.equipmentName);
                    }
                    // Delete a directory and all subdirectories with Directory static method...
                    ServiceBrokerServices.ModelFull modelfull = new ServiceBrokerServices.ModelFull()
                    {
                        equipmentList = equipmentList.ToArray(),
                        vMachineList = vMachineList.ToArray(),
                        model = model
                    };

                    return modelfull;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {

                return null;
            }
        }
        private ServiceBrokerServices.Model readBlobData(string PK_Data, string user)
        {
            string blobpath = null;
            try
            {
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(StorageConnectStringBlob);
                CloudBlobClient BlobClient = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer CloudContainer = BlobClient.GetContainerReference(modelContainerName);
                CloudContainer.CreateIfNotExist();
                var bloblists = CloudContainer.ListBlobs(new BlobRequestOptions() { UseFlatBlobListing = true });
                CloudBlob targetblob = null;
                List<ServiceBrokerServices.ModelItem> _modelItem = new List<ServiceBrokerServices.ModelItem>();
                byte[] _modelBytes = null; //取得  modelBytes、modelName
                SqlConnection con = new SqlConnection(connectionString);
                //撈出個別record
                string cmdd = "SELECT" +
                  "[PK]" +
                  ",[COMPANY]" +
                  ",[createTime]" +
                  ",[ProductID]" +
                  ",[vMachineID]" +
                  ",[CNCType]" +
                  ",[cnc_number]" +
                  ",[ModelID]" +
                  ",[dateStartTime]" +
                  ",[dataEndTime]" +
                  ",[createUser]" +
                  ",[matSizeList]" +

                  ",[modelSize]" +
                  ",[ServiceBrokerID]" +
                  "FROM [STRATEGY3] WHERE PK ='" + PK_Data + "'";
                DataSet ds = new DataSet();
                SqlDataAdapter dapt = new SqlDataAdapter(cmdd, con);
                dapt.Fill(ds);
                int count = ds.Tables[0].Rows.Count;

                string downlaodUncompressPath = tempDir + user + "/Models";//下載新Model儲存位置 C:/DTtemp/cotandy/Models
                string downloadZipFilePath = tempDir + user + "/zip/" + user + ".zip";

                foreach (var _blob in bloblists)
                {
                    if (_blob.Uri.Segments[_blob.Uri.Segments.Count() - 1].ToString().Replace(".zip", "") == PK_Data)//在blob中找   .zip
                    {
                        blobpath = _blob.Uri.ToString();
                        targetblob = (CloudBlob)_blob; //指定使用者選取的blob為targetblob    ex:2.zip
                        _modelBytes = targetblob.DownloadByteArray();
                        FileStream writer = new FileStream(downloadZipFilePath, FileMode.Append);
                        writer.Write(_modelBytes, 0, _modelBytes.Length);
                        writer.Close();
                        Uncompress(downloadZipFilePath, downlaodUncompressPath);
                    }
                }
                string[] files = Directory.GetFiles(downlaodUncompressPath);
                foreach (string file in files)
                {
                    byte[] mat = File.ReadAllBytes(file);//大小
                    FileInfo f = new FileInfo(file);
                    string modelName = f.Name.ToString();
                    if (modelName.IndexOf(".xml") >= 1)
                    {
                        modelName = f.Name.ToString().Replace(".xml", "");

                    }
                    else
                        modelName = f.Name.ToString().Replace(".mat", "");
                    _modelItem.Add(new ServiceBrokerServices.ModelItem()
                    { //找到一筆file就塞
                        modelBytes = mat,
                        modelName = modelName
                    });
                }
                ServiceBrokerServices.Model model = new ServiceBrokerServices.Model()
                {      //撈出值設定要傳給Vmachine的資料
                    modelItem = _modelItem.ToArray(),
                };

                return model;

            }
            catch (Exception ex)
            {
                return null;
            }
        }



        public static void Uncompress(string sourcePath, string targetPath)
        {
            using (ZipInputStream s = new ZipInputStream(File.OpenRead(sourcePath)))
            {
                // 若目的路徑不存在，則先建立路徑
                DirectoryInfo di = new DirectoryInfo(targetPath);

                if (!di.Exists)
                    di.Create();

                ZipEntry theEntry;

                // 逐一取出壓縮檔內的檔案(解壓縮)
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    int size = 2048;
                    byte[] data = new byte[2048];

                    Console.WriteLine("正在解壓縮: " + GetBasename(theEntry.Name));

                    // 寫入檔案
                    using (FileStream fs = new FileStream(di.FullName + "\\" + GetBasename(theEntry.Name), FileMode.Create))
                    {
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);

                            if (size > 0)
                                fs.Write(data, 0, size);
                            else
                                break;
                        }

                    }
                }


            }
        }


        public void UnZip(string[] args)
        {
            ZipInputStream s = new ZipInputStream(File.OpenRead(args[0]));

            ZipEntry theEntry;
            while ((theEntry = s.GetNextEntry()) != null)
            {

                string directoryName = Path.GetDirectoryName(args[1]);
                string fileName = Path.GetFileName(theEntry.Name);

                //生成解壓目錄
                Directory.CreateDirectory(directoryName);
                Directory.CreateDirectory(Path.GetDirectoryName(args[1] + theEntry.Name));
                if (fileName != String.Empty)
                {
                    //解壓檔到指定的目錄
                    FileStream streamWriter = File.Create(args[1] + theEntry.Name);

                    int size = 2048;
                    byte[] data = new byte[2048];
                    while (true)
                    {
                        size = s.Read(data, 0, data.Length);
                        if (size > 0)
                        {
                            streamWriter.Write(data, 0, size);
                        }
                        else
                        {
                            break;
                        }
                    }

                    streamWriter.Close();
                }
            }
            s.Close();
        }



        public static string GetBasename(string fullName)
        {
            string result;
            int lastBackSlash = fullName.LastIndexOf("\\");
            result = fullName.Substring(lastBackSlash + 1);

            return result;
        }

        //public List<ProductBasicInformation> getProductBasicInfoList()
        //{
        //     List<ProductBasicInformation> _productInformationList = new List<ProductBasicInformation>();

        //     DataSet ds = new DataSet();
        //     SqlConnection con = new SqlConnection(connectionString);
        //     con.Close();
        //     con.Open();

        //    string sqlString = "SELECT [ServiceBrokerID] " +
        //                      ",[vMachineID] " +
        //                       " ,[CNCID] " +
        //                       ",[CNCType] " +
        //                       ",[ProductID] " +
        //                       ",[ProductType] " +
        //                       ",[ActionCount] " +
        //                       ",[XTableName] " +
        //                       ",[YBlockTableName] " +
        //                       "  FROM [ProductBasicInfo]";

        //     SqlDataAdapter dapt = new SqlDataAdapter(sqlString, con);
        //     dapt.Fill(ds);//將找到的資料塞進ds中
        //     con.Close();


        //     int rowCount = ds.Tables[0].Rows.Count; //找出有多少筆record
        //     //_productInformationList設定屬性
        //     for (int x = 0; x < rowCount; x++)
        //     {

        //         _productInformationList.Add(new ProductBasicInformation()
        //         {
        //             ServiceBrokerID = ds.Tables[0].Rows[x]["ServiceBrokerID"].ToString(),
        //             vMachineID = ds.Tables[0].Rows[x]["vMachineID"].ToString(),
        //             CNCID = ds.Tables[0].Rows[x]["CNCID"].ToString(),
        //             CNCType = ds.Tables[0].Rows[x]["CNCType"].ToString(),
        //             ProductID = ds.Tables[0].Rows[x]["ProductID"].ToString(),
        //             ProductType = ds.Tables[0].Rows[x]["ProductType"].ToString()
        //            // ActionCount = ds.Tables[0].Rows[x]["ActionCount"].ToString(),
        //            // XTableName = ds.Tables[0].Rows[x]["XTableName"].ToString(),
        //            // YBlockTableName = ds.Tables[0].Rows[x]["YBlockTableName"].ToString()
        //         });
        //     }

        //     return _productInformationList;
        //}
    }




}
