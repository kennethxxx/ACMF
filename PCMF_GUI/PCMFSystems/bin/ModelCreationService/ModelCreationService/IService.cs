using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ModelCreationService;
using System.IO;
using DataCollection;

namespace ModelCreationService
{
    // 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼和組態檔中的介面名稱 "IService1"。
    [ServiceContract]
    public interface IService
    {
        // 建立ModelInfo.xml [10/15/2012 autolab]
        [OperationContract]   // CreateXML
        bool CreateXML(In_UserInfo In_userinfo);

        [OperationContract]   // CurrentVersion
        string CurrentVersion();        
        
        [OperationContract]   //get Vmachine_ID
        List<string> getVmachine_ID();

        [OperationContract]   //get CNC_Number
        List<string> getCNC_Number(string VmachineID);

        [OperationContract]   //get NCProgram_ID
        List<string> getNCProgram_ID(string VmachineID, string CNC_Number);

        [OperationContract]   // 01.getCategoryDef
        Out_CategoryDef getCategoryDef();

        [OperationContract]   // 02.getNameListOfPostYType_1
        List<string[]> getListOfPostYType_1Name();

        [OperationContract]   // 03.getNameListOfPostYType_2
        List<string[]> getListOfPostYType_2Name();

        [OperationContract]
        List<MetrologyPoint> getListOfMetrology();

        [OperationContract]
        List<MetrologyPoint> getListOfIndicator();

        [OperationContract]   // 04.getListOfIndicatorName
        List<string[]> getListOfIndicatorName();

        [OperationContract]
        Out_IndicatorsPopulation getNewPopulation(List<MetrologyPoint> processKeyList, List<MetrologyPoint> metrologyKeyList, DateTime startTime, DateTime endTime, String strXTableName, String strYTableName);

        [OperationContract]
        int InitTempleFolder(In_UserInfo In_userinfo);

        [OperationContract]
        string ChecktJobState(string JobID, In_UserInfo In_userinfo);
        

        [OperationContract]
        string Set_DataTransferModule(int contextIDCount, int trainContextIDCount, int runContextIDCount, List<MetrologyPoint> processKeyList, 
            List<MetrologyPoint> metrologyKeyList, 
            DateTime startTime, DateTime endTime, 
            List<MetrologyPoint> combinedIndicator, List<MetrologyPoint> combinedPoint, 
            List<Group> groupValue, In_UserInfo In_userinfo,
            string vMachineId, string CNCType, string CNCNumber, 
            string NCProgram, string model_Id, string version, 
            int[] allAction,
            List<String> listAbnormalList,
            List<String> listIsolatedList,
            String XTable,
            String YTable,
            String strSelectedID,
            String strProductID);
        [OperationContract]
        string Set_MDFRModule(double EwmaLamda, double EwmaTolerance, double EwmaWindow, double VarConfidence, double baseSampleNum, double RangeMultipleValue, In_UserInfo In_userinfo);
        [OperationContract]
        string Set_DQIyModule_GetDQIyPattern(In_UserInfo In_userinfo);
        [OperationContract]
        string Set_KSSModule(string AlgorithmSelection, double ClusterNumber, In_UserInfo In_userinfo);
        [OperationContract]
        string Set_KVSModule(double Fin_apha, double Fout_apha, string InOneByOneChoose, string AlgorithmSelection, In_UserInfo In_userinfo);
        [OperationContract]
        string Set_DQIxModule_VerifyDQIx(double InLambda, double InConstant, double DQIxFilterPercentage, double DQIxRefreshCounter, In_UserInfo In_userinfo);
        [OperationContract]
        string Set_DQIyModule_VerifyDQIy(double corralpha, double IsMixedModel, In_UserInfo In_userinfo);
        [OperationContract]
        string Set_BPNNModule(double iMomTermRange_Min, double iMomTermRange_Int, double iMomTermRange_Max, double iAlphaRange_Min, double iAlphaRange_Int, double iAlphaRange_Max, double iEpochsRange_1, double iEpochsRange_2, double iEpochsRange_3, List<double> iNodesRange, string iInOneByOneChoose, double iBPNNRefreshCounter, In_UserInfo In_userinfo);
        [OperationContract]
        string Set_MRModule(string iInSelectAlgorithm, double iInMR_TSVD_Condition_Number_Criteria, double iInMR_TSVD_Energy_Ratio_Criteria, double iMRRefreshCounter, In_UserInfo In_userinfo);
        [OperationContract]
        string Set_RIModule(bool FirstAlgoValue, string FirstAlgoName, List<double[]> FitstPredictValue, bool SecondAlgoValue, string SecondAlgoName, List<double[]> SecondPredictValue, double iTolerant_MaxError, string iInSelectCalculator, In_UserInfo In_userinfo);
        [OperationContract]
        string Set_GSIModule(int numberOfGroup, string iInSelectAlgorithm, double iGSI_RT, double iGSI_Threshold, double iRefreshCounter, double iInGSI_TSVD_Condition_Number_Criteria, double iInGSI_TSVD_Energy_Ratio_Criteria, In_UserInfo In_userinfo);
        
        [OperationContract]
        int Get_DataTransferResult(string JobID, In_UserInfo In_userinfo);
        [OperationContract]
        Out_MDFR Get_MDFRResult(string JobID, In_UserInfo In_userinfo);
        [OperationContract]
        Out_DQIy_CleanAbnormalY Get_DQIyResult_GetDQIyPattern(string JobID, In_UserInfo In_userinfo);
        [OperationContract]
        Out_KSS Get_KSSResult(string JobID, In_UserInfo In_userinfo);
        [OperationContract]
        Out_KVS Get_KVSResult(string JobID, In_UserInfo In_userinfo);
        [OperationContract]
        Out_VerifyDQIxResult Get_DQIxResult_VerifyDQIx(string JobID, In_UserInfo In_userinfo);
        [OperationContract]
        Out_VerifyDQIyResult Get_DQIyResult_VerifyDQIy(string JobID, In_UserInfo In_userinfo);
        [OperationContract]
        Out_BPNN Get_BPNNResult(string JobID, In_UserInfo In_userinfo);
        [OperationContract]
        Out_MR Get_MRResult(string JobID, In_UserInfo In_userinfo);
        [OperationContract]
        Out_RI Get_RIResult(string JobID, In_UserInfo In_userinfo);
        [OperationContract]
        Out_GSI Get_GSIResult(string JobID, In_UserInfo In_userinfo);

        [OperationContract]
        Out_UploadModel UploadModel(String vMachineID, String CNCType, String CNCNumber, String NCProgramID, DateTime DataStartTime, DateTime DataEndTime, In_UserInfo In_userinfo);
        

        //////////////////////////////////////MCS 20120723///////////////////////////////////////////////////////////////
        [OperationContract]
        Dictionary<string, List<string>> Get_ProductBasicInfo();

        [OperationContract]
        List<MetrologyPoint> GetXTableDEF(string XTableName);

        [OperationContract]
        List<MetrologyPoint> GetYTableDEF(string YTableName);

        [OperationContract]
        bool DCtoBlob(DCPInfo DCPInfos, string UserName, string XTable, string YTable);

        [OperationContract]
        bool DSToBlob(DCPInfo DCPInfos, string UserName, string XTable, string YTable);

        [OperationContract]
        bool SGToBlob(DCPInfo DCPInfos, string UserName, string XTable, string YTable);

        //[OperationContract]
        //void testdown(In_UserInfo User);

        [OperationContract]
        List<String> FetchContextIDFromBlob(string UserName);

        //[OperationContract]
        //void Write_SGXML(Out_IndicatorsPopulation population, int Train, int Run);






        // TODO: 在此新增您的服務作業
    }

    [MessageContract]
    public class FileUploadMessage
    {
        [MessageHeader(MustUnderstand = true)]
        public string FileName;

        [MessageHeader(MustUnderstand = true)]
        public string FileType;

        [MessageBodyMember(Order = 1)]
        public Stream FileData;

    }
}
