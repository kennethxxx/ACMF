using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using System.Data.SqlClient;
using System.Data;

using System.IO;
using System.Xml;
using System.Configuration;
using System.Reflection;
using SerializableContainer;
using BlobOperator;
using QueueMessages;

using System.Threading;
using SevenZip;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure.ServiceRuntime;
using TableOperatorClassNS;
using DataCollection;
using System.Collections.ObjectModel;
using System.Security.AccessControl;

namespace ModelCreationService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    // 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼、svc 和組態檔中的類別名稱 "Service1"。
    public class Service : IService
    {

        //connection string for database
        string DBConnectionString = "";
        string tempDir = "";
        string StorageConnectStringQueue = "";
        string StorageConnectStringBlob = "";
        string StorageConnectStringTable = "";
        string MCRWorkQueueName = "";
        string DebugTableName = "";
        string JobEventTableName = "";
        string ModelBlobContainerName = "";

        string CurrentInstanceName = "";
        string CurrentInstanceID = "";
        string CurrentInstanceURL = "";
        string CurrentServiceName = "ModelCreationService";

        String MCSModelInfoFileName = "";
        String MCSModelInfoFileName2 = "";
        String strpath = null;
#region MCS_MCSModelInfo.xml
        // ConditionInfo
        String inStrategyName;
        String inStrategyCreateTime;
        String inModifier;
        String inStartSearchTime;
        String inEndSearchTime;
        String inIsNormalUser;
        String inPhaseType;
        String inModelCreateTime;
        String inStrategyID;

        // ElementType
        Dictionary<String, List<String>> inElementTypeDic = new Dictionary<String, List<String>>();
        List<String> ElementTypeSummaryList = new List<String>();

        // Combination
        Dictionary<String, List<String>> inCombinationDic = new Dictionary<String, List<String>>();

        // PieceCount
        Dictionary<String, List<String>> inPieceCountDic = new Dictionary<String, List<String>>();

        // VariableDef
        Dictionary<String, List<String>> inVariableDefDic = new Dictionary<String, List<String>>();

        // FileList
        Dictionary<String, List<String>> inFileListDic = new Dictionary<String, List<String>>();

        // METROLOGY01O.xml
        //List<String> inMetrologyTableInfoList = new List<String>();
        //String inMetrologyTableName;
        String inMetrologyTableName;
        List<String> inMetrologyTableInfoList = new List<String>();

        // IndicatorRule
        //List<String> isIndicatorRuleList = new List<String>();
        List<String> inIndicatorRuleList = new List<String>();

        // Group
        Dictionary<String, Dictionary<String, List<String>>> inGroupListDic = new Dictionary<String, Dictionary<String, List<String>>>();

        // ExpertKnowledge
        Dictionary<String, List<String>> inExpertKnowledgeDic = new Dictionary<String, List<String>>();

        // ModuleList
        String isDQIx;
        String isDQIy;
        String isMDFR;
        String KSS_Type;
        String KVS_Type;
        String MR_Type;
        String GSI_Type;
        String NN_Type;
        String RI_Tpye;
        String isWS;

        // PROCESS01
        Dictionary<String, List<String>> inPROCESS01_IndicatorType = new Dictionary<String, List<String>>();
        String inPROCESSTableName;
        List<String> PROCESS01_BlockID = new List<String>();
        List<String> PROCESS01_BlockIDEmpty = new List<String>();

        // DCQV
        Dictionary<String, List<String>> inDCQV_TimePoint = new Dictionary<String, List<String>>();
        List<String> DCQV_TimePointMode = new List<String>();

        // FileTemporalRule
        Dictionary<String, List<String>> inFileTemporalRule_Indicator = new Dictionary<String, List<String>>();
        List<String> FileTemporalRule_IndicatorValue = new List<String>();

        // IndicatorRule
        Dictionary<String, List<String>> inIndicatorRule_Indicator = new Dictionary<String, List<String>>();
        List<String> IndicatorRule_IndicatorValueEmpty = new List<String>();

        // ContourInfo
        Dictionary<String, List<String>> inContourInfo = new Dictionary<String, List<String>>();
        List<String> ContourInfo_Value = new List<String>();

        // PointList
        Dictionary<String, List<String>> inPointList_Point = new Dictionary<String, List<String>>();
        List<String> PointList_PointValue = new List<String>();

        // MDFR_Parameters
        String inMDFREwmalamda;
        String inMDFREwmaWindow;
        String inMDFREwmaTolerance;
        String inMDFRVarConfidence;
        String inMDFRbaseSampleNum;
        String inMDFRRangeMultipleValue;

        // DQIx_Parameters
        String inDQIxLambda;
        String inDQIxConstant;
        String inDQIxDQIxFilterPercentage;
        String inDQIxDQIxRefreshCounter;

        // DQIy_Parameters
        String inDQIycorralpha;
        String inDQIyIsMixedModel;

        // KSS_Parameters
        String inKSSClusterNumber;

        // KVS_Parameters
        String inKVSFin_apha;
        String inKVSFout_apha;
        String inKVSOneByOneChoose;

        // BPNN_Parameters
        string inBPNNEpochsRange1;
        String inBPNNEpochsRange2;
        String inBPNNEpochsRange3;
        String inBPNNMomTermRange1;
        String inBPNNMomTermRange2;
        String inBPNNMomTermRange3;
        String inBPNNAlphaRange1;
        String inBPNNAlphaRange2;
        String inBPNNAlphaRange3;
        String inBPNNNodesRange1;
        String inBPNNNodesRange2;
        String inBPNNNodesRange3;
        String inBPNNOneByOneChoose;
        String inBPNNBPNNRefreshCounter;
        List<String> inBPNNNodesRange = new List<String>();

        // RI_Parameters
        List<String> inRITolerantMaxError = new List<String>();

        // MR_Parameters
        String inMR_TSVD_Condition_Number_Criteria;
        String inMR_TSVD_Energy_Ratio_Criteria;
        String inMRRefreshCounter;

        // GSI_Parameters
        String inGSIRefreshCounter;
        String inGSI_TSVD_Condition_Number_Criteria;
        String inGSI_TSVD_Energy_Ratio_Criteria;
        List<String> inGSIRT = new List<String>();
        List<String> inGSIThreshold = new List<String>();

        // AlqorithmPreference
        String inAlqorithmPreferencePreferredVMOutput;

        public int inGroupCount = 20;
        public int inIndicatorCount = 25;
        public int inBlockCount = 11;
        public int inTimePointCount = 11;

#endregion

        CreateXML CreateXMLObj;

        public Service()
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

            DBConnectionString = ConfigurationManager.AppSettings["DBConnectionString"];
            StorageConnectStringQueue = ConfigurationManager.AppSettings["StorageConnectStringQueue"];
            StorageConnectStringBlob = ConfigurationManager.AppSettings["StorageConnectStringBlob"];
            StorageConnectStringTable = ConfigurationManager.AppSettings["StorageConnectStringTable"];
            MCRWorkQueueName = ConfigurationManager.AppSettings["MCRWorkQueueName"];
            DebugTableName = ConfigurationManager.AppSettings["DebugTableName"].ToLower();
            JobEventTableName = ConfigurationManager.AppSettings["JobEventTableName"].ToLower();
            ModelBlobContainerName = ConfigurationManager.AppSettings["ModelBlobContainerName"].ToLower();
            


            if (IsEmulated)//本機模擬
            {
                CurrentInstanceName = "Emulated";
                CurrentInstanceID = "Emulated(0)";
                CurrentInstanceID += "_" + CurrentServiceName;
                CurrentInstanceURL = "";
            }
            else
            {
                CurrentInstanceName = RoleEnvironment.CurrentRoleInstance.Role.Name;
                CurrentInstanceID = RoleEnvironment.CurrentRoleInstance.Id;
                CurrentInstanceID += "_" + CurrentServiceName;
                CurrentInstanceURL = "";
                foreach (KeyValuePair<string, RoleInstanceEndpoint> AllEndpoint in RoleEnvironment.CurrentRoleInstance.InstanceEndpoints)
                {
                    CurrentInstanceURL += string.Format("[{0}:{1}]", AllEndpoint.Key, AllEndpoint.Value.IPEndpoint);
                }
            }

            

        }
        
        //建立ModelInfo.xml
        //public void SimulatedData()
        //{
        //    inStrategyName = "untitled26";
        //    inStrategyCreateTime = "20120907111604";
        //    inModifier = "User";
        //    inStartSearchTime = "2011/12/31 00:00:00";
        //    inEndSearchTime = "2012/09/07 00:00:00";
        //    inIsNormalUser = "False";
        //    inPhaseType = "1";
        //    inModelCreateTime = "20120907133632";
        //    inStrategyID = "101";



        //    //////////////////////////////////////////////////////////////////////////

        //    // ElementType
        //    Dictionary<String, List<String>> ElementTypeDic = new Dictionary<String, List<String>>();
        //    List<String> ElementTypeSummaryList = new List<String>();
        //    ElementTypeSummaryList.Add("Machine_Tool_Code");
        //    ElementTypeSummaryList.Add("Product_Name");
        //    inElementTypeDic.Add("ElementTypeSummary", ElementTypeSummaryList);

        //    // Combination
        //    Dictionary<String, List<String>> CombinationDic = new Dictionary<String, List<String>>();
        //    List<String> MachineToolCode_List = new List<String>();
        //    MachineToolCode_List.Add("CNC_001");
        //    List<String> ProductName_List = new List<String>();
        //    ProductName_List.Add("TestingPiece_1");
        //    inCombinationDic.Add("Machine_Tool_Code", MachineToolCode_List);
        //    inCombinationDic.Add("Product_Name", ProductName_List);

        //    // PieceCount
        //    Dictionary<String, List<String>> PieceCountDic = new Dictionary<String, List<String>>();
        //    List<String> PieceCount_List = new List<String>();
        //    PieceCount_List.Add("85");
        //    inPieceCountDic.Add("PieceCount", PieceCount_List);

        //    // VariableDef
        //    Dictionary<String, List<String>> VariableDefDic = new Dictionary<String, List<String>>();
        //    List<String> MetrologyDef_List = new List<String>();
        //    MetrologyDef_List.Add("Product_Type");
        //    MetrologyDef_List.Add("Product_Name");
        //    List<String> ProcessDef_List = new List<String>();
        //    ProcessDef_List.Add("None");
        //    inVariableDefDic.Add("METROLOGYDEF;METROLOGY", MetrologyDef_List);  // 屬性請照順序塞，每個屬性中間用分號隔開
        //    inVariableDefDic.Add("PROCESSDEF;PROCESS", ProcessDef_List);  // 屬性請照順序塞，每個屬性中間用分號隔開

        //    // FileList
        //    Dictionary<String, List<String>> FileListDic = new Dictionary<String, List<String>>();
        //    List<String> MetrologyXML_List = new List<String>();
        //    MetrologyXML_List.Add("Product_Type");
        //    MetrologyXML_List.Add("Product_Name");
        //    List<String> ProcessXML_List = new List<String>();
        //    ProcessXML_List.Add("None");
        //    inFileListDic.Add("METROLOGY01O.xml;METROLOGYDEF;METROLOGY01;1;O", MetrologyXML_List);
        //    inFileListDic.Add("PROCESS01O.xml;PROCESSDEF;PROCESS01;1;I", ProcessXML_List);

        //    // METROLOGY01O.xml
        //    inMetrologyTableName = "METROLOGY01O.xml";
        //    //List<String> inMetrologyTableInfoList = new List<String>();
        //    inMetrologyTableInfoList.Add("Angularity_1;0;1");
        //    inMetrologyTableInfoList.Add("Angularity_2;0;2");
        //    inMetrologyTableInfoList.Add("Angularity_3;0;3");
        //    inMetrologyTableInfoList.Add("Angularity_4;0;4");
        //    inMetrologyTableInfoList.Add("Angularity_5;0;5");
        //    inMetrologyTableInfoList.Add("Angularity_6;0;6");
        //    inMetrologyTableInfoList.Add("Parallelism_1;0;7");
        //    inMetrologyTableInfoList.Add("Perpendicularity_1;0;8");
        //    inMetrologyTableInfoList.Add("Perpendicularity_2;0;9");
        //    inMetrologyTableInfoList.Add("Roundness_1;0;10");
        //    inMetrologyTableInfoList.Add("straightness_1;0;11");
        //    inMetrologyTableInfoList.Add("straightness_2;0;12");
        //    inMetrologyTableInfoList.Add("straightness_3;0;13");
        //    inMetrologyTableInfoList.Add("straightness_4;0;14");
        //    inMetrologyTableInfoList.Add("straightness_5;0;15");
        //    inMetrologyTableInfoList.Add("straightness_6;0;16");
        //    inMetrologyTableInfoList.Add("straightness_7;0;17");
        //    inMetrologyTableInfoList.Add("straightness_8;0;18");
        //    inMetrologyTableInfoList.Add("straightness_9;0;19");
        //    inMetrologyTableInfoList.Add("straightness_10;0;20");

        //    // IndicatorRule
        //    //List<String> inIndicatorRuleList = new List<String>();
        //    inIndicatorRuleList.Add("None_Average_Filter1_Mean;21;-0.606;0.402;-0.606;0.402;1;1;1;1;1");

        //    // Group
        //    Dictionary<String, Dictionary<String, List<String>>> GroupListDic = new Dictionary<String, Dictionary<String, List<String>>>();
        //    Dictionary<String, List<String>> Group1Dic = new Dictionary<String, List<String>>();
        //    List<String> Group1IndicatorList = new List<String>();
        //    Group1IndicatorList.Add("1");
        //    Group1IndicatorList.Add("2");
        //    Group1IndicatorList.Add("3");
        //    Group1IndicatorList.Add("4");
        //    Group1IndicatorList.Add("5");
        //    List<String> Group1PointList = new List<String>();
        //    Group1PointList.Add("1");
        //    List<String> Group1ExpertList = new List<String>();
        //    Group1ExpertList.Add("1");
        //    Group1ExpertList.Add("2");
        //    Group1ExpertList.Add("3");
        //    Group1ExpertList.Add("4");
        //    Group1ExpertList.Add("5");
        //    Group1Dic.Add("GroupIndicatorList", Group1IndicatorList);
        //    Group1Dic.Add("GroupPointList", Group1PointList);
        //    Group1Dic.Add("GroupExpertList", Group1ExpertList);

        //    Dictionary<String, List<String>> Group2Dic = new Dictionary<String, List<String>>();
        //    List<String> Group2IndicatorList = new List<String>();
        //    Group2IndicatorList.Add("1");
        //    Group2IndicatorList.Add("2");
        //    Group2IndicatorList.Add("3");
        //    Group2IndicatorList.Add("4");
        //    Group2IndicatorList.Add("5");
        //    List<String> Group2PointList = new List<String>();
        //    Group2PointList.Add("1");
        //    List<String> Group2ExpertList = new List<String>();
        //    Group2ExpertList.Add("1");
        //    Group2ExpertList.Add("2");
        //    Group2ExpertList.Add("3");
        //    Group2ExpertList.Add("4");
        //    Group2ExpertList.Add("5");
        //    Group2Dic.Add("GroupIndicatorList", Group2IndicatorList);
        //    Group2Dic.Add("GroupPointList", Group2PointList);
        //    Group2Dic.Add("GroupExpertList", Group2ExpertList);

        //    inGroupListDic.Add("Group1", Group1Dic);
        //    inGroupListDic.Add("Group2", Group2Dic);

        //    //Dictionary<String, List<String>> inExpertKnowledgeDic = new Dictionary<String, List<String>>();
        //    List<String> inExpertKnowledgeList = new List<String>();
        //    inExpertKnowledgeList.Add("ByGroup");
        //    inExpertKnowledgeDic.Add("Accordance", inExpertKnowledgeList);


        //    isDQIx = "true";
        //    isDQIy = "true";
        //    isMDFR = "true";
        //    KSS_Type = "KISS";
        //    KVS_Type = "EK";
        //    MR_Type = "MMR";
        //    GSI_Type = "MahalanobisDistance";
        //    NN_Type = "BPNN";
        //    RI_Tpye = "BPNNConjectureHistory;MRConjectureHistory";
        //    isWS = "false";


        //    ///////////////I. PROCESS01I.xml    ////////////////////////////
        //    for (int i = 0; i < inBlockCount; i++)
        //    {
        //        // Block_ID value
        //        PROCESS01_BlockID.Add((i + 1).ToString());
        //    }
        //    for (int i = 0; i < inIndicatorCount; i++)
        //    {
        //        if (i == 1)
        //        {
        //            //name separator variableid
        //            inPROCESS01_IndicatorType.Add("Block_ID" + "," + "1" + "," + "22", PROCESS01_BlockID);
        //        }
        //        else
        //        {
        //            //name separator variableid
        //            inPROCESS01_IndicatorType.Add("IndicatorName" + (i + 1).ToString() + "," + "0" + "," + (21 + i).ToString(), PROCESS01_BlockIDEmpty);
        //        }
        //    }
        //    inPROCESSTableName = "PROCESS01I.xml";
        //    ///////////////////J. DCQV/////////////////////////
        //    for (int i = 0; i < inBlockCount; i++)
        //    {
        //        // variableid  standard  limition
        //        DCQV_TimePointMode.Add("22" + "," + "1" + "," + (i + 1).ToString());
        //    }
        //    //name  filename   dcqvid
        //    inDCQV_TimePoint.Add("TimePointMode" + "," + "PROCESS01I.xml" + "," + "1", DCQV_TimePointMode);

        //    ///////////////////K. FileTemporalRule/////////////////////////
        //    // variableid  limition type value
        //    FileTemporalRule_IndicatorValue.Add("22" + "," + "15" + "," + "trim" + "," + "15");
        //    for (int i = 0; i < inIndicatorCount; i++)
        //    {
        //        //name
        //        //variableid
        //        //dcqvid
        //        //rawdatafilename
        //        //description
        //        //lcl
        //        //ucl
        //        //lsl
        //        //usl
        //        //endcount
        //        //startcount
        //        //filterid    

        //        inFileTemporalRule_Indicator.Add(
        //            "IndicatorName" + (i + 1).ToString() + "," +
        //            "22" + "," +
        //            "1" + "," +
        //            "PROCESS01I.xml" + "," +
        //            "1st filter rule of variable" + "IndicatorName" + (i + 1).ToString() + "," +
        //            " " + "," + " " + "," + " " + "," + " " + "," +
        //            "1" + "," +
        //            "1" + "," +
        //            (i + 1).ToString()
        //            , FileTemporalRule_IndicatorValue);
        //    }

        //    ///////////////////N. IndicatorList/////////////////////////
        //    for (int i = 0; i < inIndicatorCount; i++)
        //    {
        //        //name
        //        //variableid                
        //        //lcl
        //        //ucl
        //        //lsl
        //        //usl
        //        //filterid  
        //        //algorithmid  
        //        //indicatorid 
        //        //enable
        //        //steptypeid

        //        inIndicatorRule_Indicator.Add(
        //            "IndicatorName" + (i + 1).ToString() + "," +
        //            (i + 1).ToString() + "," +
        //            "LCL value" + "," + "UCL value" + "," + "LSL value" + "," + "USL value" + "," +
        //            (i + 1).ToString() + "," +
        //            "1" + "," +
        //            (i + 1).ToString() +
        //            "1" + "," +
        //            "1" + ","
        //            , IndicatorRule_IndicatorValueEmpty);
        //    }

        //    ///////////////////O. ContourInfo/////////////////////////
        //    ContourInfo_Value.Add("ContourValue");

        //    for (int i = 0; i < inIndicatorCount; i++)
        //    {
        //        //name

        //        inContourInfo.Add(
        //            "Contour" + (i + 1).ToString()
        //            , ContourInfo_Value);
        //    }

        //    ///////////////////P. PointList/////////////////////////

        //    PointList_PointValue.Add("PointValue");

        //    for (int i = 0; i < inIndicatorCount; i++)
        //    {
        //        //name
        //        //lcl
        //        //ucl
        //        //lsl
        //        //usl
        //        //algorithmid  
        //        //Description
        //        //axisy
        //        //axisx
        //        //target
        //        //measureid
        //        //pointid

        //        inPointList_Point.Add(
        //            "Point" + (i + 1).ToString() + "," +
        //            "LCL value" + "," + "UCL value" + "," + "LSL value" + "," + "USL value" + "," +
        //            "1" + "," +
        //            "Mean to Product_Type - Product_Name:" + "," +
        //            "1" + "," +
        //            (i + 1).ToString() + "," +
        //            "0.01" + "," +
        //            "1" + "," +
        //            "1"
        //            , PointList_PointValue);
        //    }



        //    for (int GroupFinger = 0; GroupFinger < inGroupCount; GroupFinger++)
        //    {
        //        inRITolerantMaxError.Add("10");
        //        inGSIRT.Add("0.7");
        //        inGSIThreshold.Add("0.7");
        //    }
        //    /////////////////////T. MDFR_Parameters////////////////////////////////////////
        //    inMDFREwmalamda = "0.99";
        //    inMDFREwmaWindow = "60";
        //    inMDFREwmaTolerance = "0.99";
        //    inMDFRVarConfidence = "0.95";
        //    inMDFRbaseSampleNum = "60";
        //    inMDFRRangeMultipleValue = "3.5";
        //    ////////////////////U. DQIx_Parameters//////////////////////////
        //    inDQIxLambda = "1";
        //    inDQIxConstant = "3";
        //    inDQIxDQIxFilterPercentage = "0.95";
        //    inDQIxDQIxRefreshCounter = "3";
        //    ////////////////////V. DQIy_Parameters//////////////////////////
        //    inDQIycorralpha = "0.0001";
        //    inDQIyIsMixedModel = "1";
        //    ////////////////////W. KSS_Parameters//////////////////////////
        //    inKSSClusterNumber = "6";
        //    ////////////////////X. KVS_Parameters//////////////////////////
        //    inKVSFin_apha = "0.85";
        //    inKVSFout_apha = "0.85";
        //    inKVSOneByOneChoose = "Tune";
        //    ////////////////////Y. BPNN_Parameters//////////////////////////
        //    //inBPNNEpochsRange1 = "60";
        //    //inBPNNEpochsRange2 = "80";
        //    //inBPNNEpochsRange3 = "100";
        //    //inBPNNMomTermRange1 = "0.5";
        //    //inBPNNMomTermRange2 = "0.2";
        //    //inBPNNMomTermRange3 = "0.9";
        //    //inBPNNAlphaRange1 = "0.15";
        //    //inBPNNAlphaRange2 = "0.10";
        //    //inBPNNAlphaRange3 = "0.15";
        //    //inBPNNNodesRange1 = "19";//這邊可能需要調整
        //    //inBPNNNodesRange2 = "20";
        //    //inBPNNNodesRange3 = "21";
        //    //inBPNNOneByOneChoose = "Tune";
        //    //inBPNNBPNNRefreshCounter = "3";

        //    //inBPNNNodesRange.Add("19"); inBPNNNodesRange.Add("19"); inBPNNNodesRange.Add("19");
        //    //inBPNNNodesRange.Add("19"); inBPNNNodesRange.Add("19"); inBPNNNodesRange.Add("19");

        //    ////////////////////Z. RI_Parameters//////////////////////////
        //    //List
        //    ////////////////////AA. MR_Parameters//////////////////////////
        //    inMR_TSVD_Condition_Number_Criteria = "50";
        //    inMR_TSVD_Energy_Ratio_Criteria = "99.5";
        //    inMRRefreshCounter = "3";

        //    ////////////////////AB. GSI_Parameters//////////////////////////
        //    inGSIRefreshCounter = "3";
        //    inGSI_TSVD_Condition_Number_Criteria = "50";
        //    inGSI_TSVD_Energy_Ratio_Criteria = "99.5";

        //    ////////////////////AC. AlqorithmPreference//////////////////////////
        //    inAlqorithmPreferencePreferredVMOutput = "NN";

        //    return;
        //}

        public bool CreateXML(In_UserInfo In_userinfo)
        {
            try
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
                    strpath = Path.Combine(tempDir, In_userinfo.FullName);
                    strpath = Path.Combine(strpath, "Models");
                    MCSModelInfoFileName = Path.Combine(strpath, ConfigurationManager.AppSettings["MCSModelInfoFileName"]);
                }
                else
                {
                    LocalResource LocalTempDiv = RoleEnvironment.GetLocalResource(ConfigurationManager.AppSettings["CloudDiv"]);
                    tempDir = Path.Combine(LocalTempDiv.RootPath, ConfigurationManager.AppSettings["TempDir"]);

                    strpath = Path.Combine(tempDir, In_userinfo.FullName);
                    strpath = Path.Combine(strpath, "Models");
                    MCSModelInfoFileName = Path.Combine(strpath, ConfigurationManager.AppSettings["MCSModelInfoFileName"]);
                }

                if (!(Directory.Exists(strpath)))
                {
                    Directory.CreateDirectory(strpath);
                }

                AddDirectorySecurity(strpath, @"everyone", FileSystemRights.FullControl, AccessControlType.Allow);


                if (File.Exists(MCSModelInfoFileName))
                {
                    File.Delete(MCSModelInfoFileName);
                }
                else
                {
                    //File.Create(MCSModelInfoFileName);
                }

                CreateXMLObj = new CreateXML();

                CreateXMLObj.Create_XML_Document(MCSModelInfoFileName);

                return true;
            }
            catch (Exception ex)
            {
                String ErrorWord = ex.ToString();
                return false;
            }
            finally
            {
                
            }

            
            
        }

        public bool CheckXML(In_UserInfo In_userinfo)
        {
            try
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
                    strpath = Path.Combine(tempDir, In_userinfo.FullName);
                    strpath = Path.Combine(strpath, "Models");
                    MCSModelInfoFileName = Path.Combine(strpath, ConfigurationManager.AppSettings["MCSModelInfoFileName"]);
                }
                else
                {
                    LocalResource LocalTempDiv = RoleEnvironment.GetLocalResource(ConfigurationManager.AppSettings["CloudDiv"]);
                    tempDir = Path.Combine(LocalTempDiv.RootPath, ConfigurationManager.AppSettings["TempDir"]);

                    strpath = Path.Combine(tempDir, In_userinfo.FullName);
                    strpath = Path.Combine(strpath, "Models");
                    MCSModelInfoFileName = Path.Combine(strpath, ConfigurationManager.AppSettings["MCSModelInfoFileName"]);
                }       

                return true;
            }
            catch (Exception ex)
            {
                String ErrorWord = ex.ToString();
                return false;
            }
            finally
            {

            }



        }

        // currentversion    
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

        public List<string> getVmachine_ID()
        {
            List<string> vMachineIDNameList = null;

            string sqlCommand = "SELECT DISTINCT VMACHINE_ID FROM NCPROGRAMLIST";

            using (SqlConnection conn = new SqlConnection(DBConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter dapt = new SqlDataAdapter(sqlCommand, conn);
                    DataSet ds = new DataSet();

                    dapt.Fill(ds);

                    int vMmachineIDCount = ds.Tables[0].Rows.Count;
                    vMachineIDNameList = new List<string>();
                    for (int i = 0; i < vMmachineIDCount; i++)
                    {
                        vMachineIDNameList.Add(ds.Tables[0].Rows[i].ItemArray[0].ToString()); //Vmachine_ID
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    vMachineIDNameList = new List<string>();
                    conn.Close();
                    writeLog(ex);
                }
                conn.Close();
            }

            return vMachineIDNameList;
        }

        public List<string> getCNC_Number(string vMachineID)
        {
            List<string> CNCNumberNameList = null; new List<string>();

            string sqlCommand = "SELECT CNCMACHINENUM from NCPROGRAMLIST WHERE VMACHINE_ID = '" + vMachineID + "'";

            using (SqlConnection conn = new SqlConnection(DBConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter dapt = new SqlDataAdapter(sqlCommand, conn);
                    DataSet ds = new DataSet();

                    dapt.Fill(ds);

                    int CNCNumberCount = ds.Tables[0].Rows.Count;
                    CNCNumberNameList = new List<string>();

                    for (int i = 0; i < CNCNumberCount; i++)
                    {
                        CNCNumberNameList.Add(ds.Tables[0].Rows[i].ItemArray[0].ToString());
                    }

                    conn.Close();
                }
                catch (Exception ex)
                {
                    CNCNumberNameList = new List<string>();
                    conn.Close();
                    writeLog(ex);
                }
                conn.Close();
            }

            return CNCNumberNameList;
        }

        public List<string> getNCProgram_ID(string VmachineID, string CNC_Number)
        {
            List<string> NCProgramIDList = null;

            string sqlCommand = "SELECT NCPROGRAMID FROM NCPROGRAMLIST WHERE VMACHINE_ID = '" + VmachineID + "'" + "AND CNCMACHINENUM = '" + CNC_Number + "'";

            using (SqlConnection conn = new SqlConnection(DBConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter dapt = new SqlDataAdapter(sqlCommand, conn);
                    DataSet ds = new DataSet();

                    dapt.Fill(ds);

                    int NCProgramIDCount = ds.Tables[0].Rows.Count;
                    NCProgramIDList = new List<string>();

                    for (int i = 0; i < NCProgramIDCount; i++)
                    {
                        NCProgramIDList.Add(ds.Tables[0].Rows[i].ItemArray[0].ToString()); //NC Program
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    NCProgramIDList = new List<string>();
                    conn.Close();
                    writeLog(ex);
                }
                conn.Close();
            }

            return NCProgramIDList;
        }

        public Out_CategoryDef getCategoryDef()
        {
            GC.Collect();

            Out_CategoryDef categoryDef = new Out_CategoryDef();
            categoryDef.Ack = 1;

            //擷取STDB裡 Categorydef table中的資料
            string sqlCommand_position = "SELECT REALVALUE FROM CATEGORYDEF WHERE CATEGORYNAME ='position'";
            string sqlCommand_seat_number = "SELECT REALVALUE FROM CATEGORYDEF WHERE CATEGORYNAME ='seat_number'";
            string sqlCommand_producttype = "SELECT REALVALUE FROM CATEGORYDEF WHERE CATEGORYNAME ='producttype'";
            string sqlCommand_cnc_number = "SELECT REALVALUE FROM CATEGORYDEF WHERE CATEGORYNAME ='cnc_number'";
            string sqlCommand_G_Code = "SELECT REALVALUE FROM CATEGORYDEF WHERE CATEGORYNAME ='G_Code'";
            string sqlCommand_PostYType_1 = "SELECT REALVALUE FROM CATEGORYDEF WHERE CATEGORYNAME ='PostYType_1'";
            string sqlCommand_PostYType_2 = "SELECT REALVALUE FROM CATEGORYDEF WHERE CATEGORYNAME ='PostYType_2'";

            using (SqlConnection conn = new SqlConnection(DBConnectionString))
            {
                try
                {
                    conn.Open();

                    SqlDataAdapter dapt_position = new SqlDataAdapter(sqlCommand_position, conn);
                    SqlDataAdapter dapt_seat_number = new SqlDataAdapter(sqlCommand_seat_number, conn);
                    SqlDataAdapter dapt_producttype = new SqlDataAdapter(sqlCommand_producttype, conn);
                    SqlDataAdapter dapt_cnc_number = new SqlDataAdapter(sqlCommand_cnc_number, conn);
                    SqlDataAdapter dapt_G_Code = new SqlDataAdapter(sqlCommand_G_Code, conn);
                    SqlDataAdapter dapt_PostYType_1 = new SqlDataAdapter(sqlCommand_PostYType_1, conn);
                    SqlDataAdapter dapt_PostYType_2 = new SqlDataAdapter(sqlCommand_PostYType_2, conn);

                    DataSet ds_position = new DataSet();
                    DataSet ds_seat_number = new DataSet();
                    DataSet ds_producttype = new DataSet();
                    DataSet ds_cnc_number = new DataSet();
                    DataSet ds_G_Code = new DataSet();
                    DataSet ds_PostYType_1 = new DataSet();
                    DataSet ds_PostYType_2 = new DataSet();

                    dapt_position.Fill(ds_position);
                    dapt_seat_number.Fill(ds_seat_number);
                    dapt_producttype.Fill(ds_producttype);
                    dapt_cnc_number.Fill(ds_cnc_number);
                    dapt_G_Code.Fill(ds_G_Code);
                    dapt_PostYType_1.Fill(ds_PostYType_1);
                    dapt_PostYType_2.Fill(ds_PostYType_2);

                    categoryDef.position = new List<string>();
                    categoryDef.seat_number = new List<string>();
                    categoryDef.producttype = new List<string>();
                    categoryDef.cnc_number = new List<string>();
                    categoryDef.G_Code = new List<string>();
                    categoryDef.PostYType_1 = new List<string>();
                    categoryDef.PostYType_2 = new List<string>();

                    //將DataSet的資料塞進去List<string>裡
                    categoryDef.position.Add(ds_position.Tables[0].Rows[0].ItemArray[0].ToString());
                    categoryDef.seat_number.Add(ds_seat_number.Tables[0].Rows[0].ItemArray[0].ToString());
                    categoryDef.producttype.Add(ds_producttype.Tables[0].Rows[0].ItemArray[0].ToString());
                    categoryDef.cnc_number.Add(ds_cnc_number.Tables[0].Rows[0].ItemArray[0].ToString());
                    categoryDef.G_Code.Add(ds_G_Code.Tables[0].Rows[0].ItemArray[0].ToString());
                    categoryDef.PostYType_1.Add(ds_PostYType_1.Tables[0].Rows[0].ItemArray[0].ToString());
                    categoryDef.PostYType_2.Add(ds_PostYType_2.Tables[0].Rows[0].ItemArray[0].ToString());

                    categoryDef.Ack = 0;
                    conn.Close();
                }
                catch (Exception ex)
                {
                    conn.Close();
                    writeLog(ex);
                }
                conn.Close();
            }

            return categoryDef;
        }

        public List<string[]> getListOfPostYType_1Name()
        {
            GC.Collect();

            List<string[]> PostYType1Lsit = null;

            string sqlCommand = "SELECT METROLOGYKEY, VARIABLENAME, COLLAPSABLE FROM METROLOGYDEF ORDER BY METROLOGYKEY";

            using (SqlConnection conn = new SqlConnection(DBConnectionString))
            {

                try
                {
                    conn.Open();
                    SqlDataAdapter dapt = new SqlDataAdapter(sqlCommand, conn);
                    DataSet ds = new DataSet();

                    dapt.Fill(ds);

                    int PostYType1Count = ds.Tables[0].Rows.Count;
                    PostYType1Lsit = new List<string[]>();
                    string[] PY = null;
                    for (int i = 0; i < PostYType1Count; i++)
                    {
                        PY = new string[3];
                        PY[0] = ds.Tables[0].Rows[i].ItemArray[0].ToString();   //METROLOGYKEY
                        PY[1] = ds.Tables[0].Rows[i].ItemArray[1].ToString();   //VARIABLENAME
                        PY[2] = ds.Tables[0].Rows[i].ItemArray[2].ToString();   //COLLAPSABLE
                        PostYType1Lsit.Add(PY);
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    PostYType1Lsit = new List<string[]>();
                    conn.Close();
                    writeLog(ex);
                }
                conn.Close();
            }

            return PostYType1Lsit;
        }

        public List<string[]> getListOfPostYType_2Name()
        {
            GC.Collect();

            List<string[]> PostYType2Lsit = null;

            string sqlCommand = "SELECT METROLOGYKEY, VARIABLENAME, COLLAPSABLE FROM METROLOGYDEF WHERE COLLAPSABLE = 'roundness' ORDER BY VARIABLENAME";

            using (SqlConnection conn = new SqlConnection(DBConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter dapt = new SqlDataAdapter(sqlCommand, conn);
                    DataSet ds = new DataSet();

                    dapt.Fill(ds);

                    int PostYType2Count = ds.Tables[0].Rows.Count;
                    PostYType2Lsit = new List<string[]>();
                    string[] PY = null;
                    for (int i = 0; i < PostYType2Count; i++)
                    {
                        PY = new string[3];
                        PY[0] = ds.Tables[0].Rows[i].ItemArray[0].ToString();   //METROLOGYKEY
                        PY[1] = ds.Tables[0].Rows[i].ItemArray[1].ToString();   //VARIABLENAME
                        PY[2] = ds.Tables[0].Rows[i].ItemArray[2].ToString();   //COLLAPSABLE
                        PostYType2Lsit.Add(PY);
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    PostYType2Lsit = new List<string[]>();
                    conn.Close();
                    writeLog(ex);
                }
                conn.Close();
            }
            return PostYType2Lsit;
        }

        public List<MetrologyPoint> getListOfMetrology()  //回傳Metrologydef表中的Metrology與precisionlist表中的action對應
        {                                                 //以及資料欄位的對應ex:Actions=1,Datafield=field_1,MeasureType=Strnaightness_1,value=1
            GC.Collect();

            List<MetrologyPoint> returnList = new List<MetrologyPoint>();

            string sqlCommand = "";

            DataSet ds_MetrologyItems = null;
            DataSet ds_Mapping = null;

            using (SqlConnection conn = new SqlConnection(DBConnectionString))
            {
                try
                {
                    conn.Open();

                    //get all metrology items
                    sqlCommand = "SELECT METROLOGYKEY, VARIABLENAME, DEFTABLE, DEFFIELD, METROLOGYTYPE FROM METROLOGYDEF";
                    SqlDataAdapter dapt_MetrologyItems = new SqlDataAdapter(sqlCommand, conn);
                    ds_MetrologyItems = new DataSet();
                    dapt_MetrologyItems.Fill(ds_MetrologyItems);

                    //get all metrology-action mappings
                    sqlCommand = "SELECT METROLOGYITEM, ACTIONNUM FROM PRECISIONLIST";
                    SqlDataAdapter dapt_Mapping = new SqlDataAdapter(sqlCommand, conn);
                    ds_Mapping = new DataSet();
                    dapt_Mapping.Fill(ds_Mapping);

                    conn.Close();
                }
                catch (Exception ex)
                {
                    conn.Close();
                    writeLog(ex);
                }
                conn.Close();
            }

            if (ds_MetrologyItems != null && ds_Mapping != null)
            {

                int totalMetrology = ds_MetrologyItems.Tables[0].Rows.Count;
                int totalMapping = ds_Mapping.Tables[0].Rows.Count;
                int temp = 0;

                //fill data to objects
                for (int i = 0; i < totalMetrology; i++)
                {
                    MetrologyPoint point = new MetrologyPoint();
                    point.Value = Int32.Parse(ds_MetrologyItems.Tables[0].Rows[i]["METROLOGYKEY"].ToString());
                    point.Name = ds_MetrologyItems.Tables[0].Rows[i]["VARIABLENAME"].ToString();
                    point.MeasureType = ds_MetrologyItems.Tables[0].Rows[i]["METROLOGYTYPE"].ToString();
                    point.DataField = ds_MetrologyItems.Tables[0].Rows[i]["DEFFIELD"].ToString();
                    point.Actions = "";

                    for (int j = 0; j < totalMapping; j++)
                    {
                        temp = Int32.Parse(ds_Mapping.Tables[0].Rows[j]["METROLOGYITEM"].ToString());
                        if (temp == point.Value)
                            point.Actions += ds_Mapping.Tables[0].Rows[j]["ACTIONNUM"].ToString() + ",";
                    }

                    if (!"".Equals(point.Actions))
                        point.Actions = point.Actions.Substring(0, point.Actions.Length - 1);

                    returnList.Add(point);
                }
            }

            return returnList;   
        }

        public List<MetrologyPoint> getListOfIndicator() //回傳processdef表中的process敘述
                                                         //以及資料欄位的對應ex:Datafield=field_1,Name=Frequency1,value=1
          
        {
            GC.Collect();
            List<MetrologyPoint> returnList = null;

            string sqlCommand = "SELECT PROCESSKEY, VARIABLENAME, DEFTABLE, DEFFIELD FROM PROCESSDEF";

            DataSet ds = null;

            using (SqlConnection conn = new SqlConnection(DBConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter dapt = new SqlDataAdapter(sqlCommand, conn);
                    ds = new DataSet();
                    dapt.Fill(ds);

                    int TotalIndicator = ds.Tables[0].Rows.Count;

                    returnList = new List<MetrologyPoint>();
                    for (int i = 0; i < TotalIndicator; i++)
                    {
                        MetrologyPoint indicator = new MetrologyPoint();
                        indicator.Value = Int32.Parse(ds.Tables[0].Rows[i]["PROCESSKEY"].ToString());
                        indicator.Name = ds.Tables[0].Rows[i]["VARIABLENAME"].ToString();
                        indicator.DataField = ds.Tables[0].Rows[i]["DEFFIELD"].ToString();
                        returnList.Add(indicator);
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    returnList = new List<MetrologyPoint>();
                    conn.Close();
                    writeLog(ex);
                }
                conn.Close();
            }
            return returnList;
        }

        public List<string[]> getListOfIndicatorName()
        {
            GC.Collect();
            List<string[]> returnList = null;

            string sqlCommand = "SELECT PROCESSKEY,VARIABLENAME,DEFTABLE FROM PROCESSDEF ORDER BY PROCESSKEY";

            DataSet ds = null;

            using (SqlConnection conn = new SqlConnection(DBConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter dapt = new SqlDataAdapter(sqlCommand, conn);
                    ds = new DataSet();
                    dapt.Fill(ds);

                    int indicatorCount = ds.Tables[0].Rows.Count;
                    returnList = new List<string[]>();
                    for (int i = 0; i < indicatorCount; i++)
                    {
                        string[] s = new string[3];
                        s[0] = ds.Tables[0].Rows[i].ItemArray[0].ToString();   //PROCESSKEY
                        s[1] = ds.Tables[0].Rows[i].ItemArray[1].ToString();   //VARIABLENAME
                        s[2] = ds.Tables[0].Rows[i].ItemArray[2].ToString();   //DEFTABLE
                        returnList.Add(s);
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    returnList = new List<string[]>();
                    conn.Close();
                    writeLog(ex);
                }
                conn.Close();
            }

            return returnList;
        }

        public Out_IndicatorsPopulation getNewPopulation(List<MetrologyPoint> processKeyList, List<MetrologyPoint> metrologyKeyList, 
            DateTime startTime, DateTime endTime, String strXTableName, String strYTableName)
        {
            GC.Collect();
            //參數初始化

            Out_IndicatorsPopulation population = new Out_IndicatorsPopulation();

            try
            {
                int contextIDCount = 0;
                List<string> WorkPieceID_Metrology = new List<string>();
                List<string> WorkPieceID_Process = new List<string>();
                List<string> listContexID = new List<string>();
                List<DateTime> listProcessStartTime = new List<DateTime>();
                List<DateTime> listProcessEndTime = new List<DateTime>();
                List<DateTime> listMetrologyStartTime = new List<DateTime>();
                List<DateTime> listMetrologyEndTime = new List<DateTime>();
                List<double[]> listIndicatorPopulationValue = new List<double[]>();
                List<int[]> listActionPopulationValue = new List<int[]>();
                List<double[]> listPointPopulationValue = new List<double[]>();

                Dictionary<String, Indicator> listAllIndicators = new Dictionary<String, Indicator>();    //all indicators & value
                Dictionary<String, Indicator> listAllPoints = new Dictionary<String, Indicator>();    //all points & value
                List<Context> listContext = new List<Context>();    //all context id

                string startTimeYY = startTime.Year.ToString();
                string startTimeMM = startTime.Month.ToString();
                string startTimeDD = startTime.Day.ToString();
                string endTimeYY = endTime.Year.ToString();
                string endTimeMM = endTime.Month.ToString();
                string endTimeDD = endTime.Day.ToString();

                int indicatorIndexCount = 0;
                int pointIndexCount = 0;
                indicatorIndexCount = processKeyList.Count;
                pointIndexCount = metrologyKeyList.Count; ;

                double[] arrIndicatorNum = new double[indicatorIndexCount];
                double[] arrPointNum = new double[pointIndexCount];

                //population.indicatorIndexList = processKeyList.ToArray();
                //population.pointIndexList = metrologyKeyList.ToArray();

                //string sqlCommand_Metrology = "SELECT * FROM Metrology WHERE TIMETAG BETWEEN '" + startTimeYY + "-" + startTimeMM + "-" + startTimeDD + "' AND '" + endTimeYY + "-" + endTimeMM + "-" + endTimeDD + "' " +
                //    "AND [CONTEXTID] IN (SELECT [CONTEXTID] FROM Process) ORDER BY TIMETAG";

                //string sqlCommand_Process = "SELECT * FROM Process WHERE TIMETAG BETWEEN '" + startTimeYY + "-" + startTimeMM + "-" + startTimeDD + "' AND '" + endTimeYY + "-" + endTimeMM + "-" + endTimeDD + "' " +
                //    "AND [CONTEXTID] IN (SELECT [CONTEXTID] FROM Metrology) ORDER BY TIMETAG";

                string sqlCommand_Metrology = "SELECT * FROM  " + strYTableName + " WHERE TIMETAG BETWEEN '" + startTimeYY + "-" + startTimeMM + "-" + startTimeDD + "' AND '" + endTimeYY + "-" + endTimeMM + "-" + endTimeDD + "' " +
                    "AND [WORKPIECEID] IN (SELECT [WORKPIECEID] FROM " + strXTableName + ") ORDER BY TIMETAG";

                string sqlCommand_Process = "SELECT * FROM  " + strXTableName + " WHERE TIMETAG BETWEEN '" + startTimeYY + "-" + startTimeMM + "-" + startTimeDD + "' AND '" + endTimeYY + "-" + endTimeMM + "-" + endTimeDD + "' " +
                    "AND [WORKPIECEID] IN (SELECT [WORKPIECEID] FROM " + strYTableName + ") ORDER BY TIMETAG";

                SqlConnection con = new SqlConnection(DBConnectionString);
                con.Open();

                SqlDataAdapter dapt_Metrology = new SqlDataAdapter(sqlCommand_Metrology, con);
                SqlDataAdapter dapt_Process = new SqlDataAdapter(sqlCommand_Process, con);

                //new DataSet
                DataSet ds_Metrology = new DataSet();
                DataSet ds_Process = new DataSet();

                //將DataAdapter的資料塞進去DataSet
                dapt_Metrology.Fill(ds_Metrology);
                dapt_Process.Fill(ds_Process);

                con.Close();

                string tempName = "";

                Context contextItem;

                string buffer = "";

                //a set of context id
                List<string> contextId = new List<string>();

                //a set of process time
                List<string> processTime = new List<string>();

                //get all indicators & values
                for (int i = 0; i < ds_Process.Tables[0].Rows.Count; i++) //利用時間區間搜尋，依照使用者勾選的條件紀錄process表中的使用者選取資料
                {
                    for (int j = 0; j < indicatorIndexCount; j++)
                    {
                        //Indicator's Name
                        //tempName = ds_Process.Tables[0].Rows[i]["ActionNum"].ToString() + "_" + processKeyList[j].Name;//欄位名稱
                        tempName = ds_Process.Tables[0].Rows[i]["BlockID"].ToString() + "_" + processKeyList[j].Name;//欄位名稱

                        //check exist indicator???
                        Indicator indicator = null;
                        if (listAllIndicators.ContainsKey(tempName)) //比對由actionnum和indicator名稱是否有重複
                            indicator = listAllIndicators[tempName];

                        if (indicator == null)
                        {
                            indicator = new Indicator();
                            //20120806
                            //indicator.ContextID = ds_Process.Tables[0].Rows[i]["ContextID"].ToString();
                            indicator.ContextID = ds_Process.Tables[0].Rows[i]["WORKPIECEID"].ToString();
                            indicator.Number = listAllIndicators.Count + 1;
                            indicator.Name = tempName;
                            listAllIndicators.Add(tempName, indicator); //add new key to dictionary  //將使用者勾選的indicator作為欄位名，並由上往下塞

                            //contextIdProcessData.Add(Int32.Parse(indicator.ContextID));
                        }

                        buffer = ds_Process.Tables[0].Rows[i][processKeyList[j].DataField].ToString();//用來計算contextID的筆數

                        if (buffer == "")
                        {
                            buffer = "0";
                        }
                        indicator.ListItemValue.Add((double)Convert.ToDouble(buffer));

                        //contextid
                        //buffer = ds_Process.Tables[0].Rows[i]["ContextID"].ToString();
                        buffer = ds_Process.Tables[0].Rows[i]["WORKPIECEID"].ToString();
                        if (!contextId.Contains(buffer))
                        {
                            contextId.Add(buffer);
                            processTime.Add(ds_Process.Tables[0].Rows[i]["TIMETAG"].ToString());
                        }
                    }
                }

                contextIDCount = ds_Metrology.Tables[0].Rows.Count;
                if (contextIDCount > listAllIndicators[tempName].ListItemValue.Count)
                {
                    contextIDCount = listAllIndicators[tempName].ListItemValue.Count;
                }

                //all context id
                //int contextIdTemp = 0;
                for (int i = 0; i < contextIDCount; i++)
                {
                    //contextIdTemp = Int32.Parse(ds_Metrology.Tables[0].Rows[i]["ContextID"].ToString());
                    //contextItem = new Context(i + 1, ds_Metrology.Tables[0].Rows[i]["ContextID"].ToString());
                    contextItem = new Context(i + 1, ds_Metrology.Tables[0].Rows[i]["WORKPIECEID"].ToString());

                    contextItem.ProcessStartTime = processTime[i];
                    contextItem.ProcessEndTime = processTime[i];

                    //contextItem.ProcessStartTime = ds_Metrology.Tables[0].Rows[i]["TIMETAG"].ToString();
                    //contextItem.ProcessEndTime = ds_Metrology.Tables[0].Rows[i]["TIMETAG"].ToString();

                    contextItem.MetrologyStartTime = ds_Metrology.Tables[0].Rows[i]["TIMETAG"].ToString();
                    contextItem.MetrologyEndTime = ds_Metrology.Tables[0].Rows[i]["TIMETAG"].ToString();
                    listContext.Add(contextItem);
                }

                //get all points & values
                for (int i = 0; i < contextIDCount; i++)
                {
                    for (int k = 0; k < pointIndexCount; k++)
                    {
                        //Point's Name
                        tempName = metrologyKeyList[k].Name;

                        //check exist point???
                        Indicator point = null;
                        if (listAllPoints.ContainsKey(tempName))
                            point = listAllPoints[tempName];

                        if (point == null)
                        {
                            point = new Indicator();
                            //point.ContextID = ds_Metrology.Tables[0].Rows[i]["ContextID"].ToString();
                            point.ContextID = ds_Metrology.Tables[0].Rows[i]["WORKPIECEID"].ToString();
                            point.Number = listAllPoints.Count + 1;
                            point.Name = tempName;

                            listAllPoints.Add(tempName, point); //add new key to dictionary
                        }

                        buffer = ds_Metrology.Tables[0].Rows[i][metrologyKeyList[k].DataField].ToString();
                        if (buffer == "")
                        {
                            buffer = "0";
                        }
                        point.ListItemValue.Add((double)Convert.ToDouble(buffer));
                    }
                }

                #region old part
                for (int i = 0; i < contextIDCount; i++)
                {

                    //WorkPieceID_Metrology.Add((string)ds_Metrology.Tables[0].Rows[i]["ContextID"]);

                    //WorkPieceID_Process.Add((string)ds_Process.Tables[0].Rows[i]["ContextID"]);

                    WorkPieceID_Metrology.Add((string)ds_Metrology.Tables[0].Rows[i]["WORKPIECEID"]);

                    WorkPieceID_Process.Add((string)ds_Process.Tables[0].Rows[i]["WORKPIECEID"]);

                    //挑出CONTEXTID塞入list
                    listContexID.Add(ds_Metrology.Tables[0].Rows[i].ItemArray[0].ToString());

                    ////挑出ProcessStartTime塞入list,目前是用STDB Metrology裡的TIMETAG
                    //listProcessStartTime.Add((DateTime)ds_Process.Tables[0].Rows[i]["TIMETAG"]);

                    ////挑出ProcessEndTime塞入list,目前是用STDB Metrology裡的TIMETAG
                    //listProcessEndTime.Add((DateTime)ds_Process.Tables[0].Rows[i]["TIMETAG"]);

                    ////挑出MetrologyStartTime塞入list,目前是用STDB Metrology裡的TIMETAG
                    //listMetrologyStartTime.Add((DateTime)ds_Metrology.Tables[0].Rows[i]["TIMETAG"]);

                    ////挑出MetrologyEndTime塞入list,目前是用STDB Metrology裡的TIMETAG
                    //listMetrologyEndTime.Add((DateTime)ds_Metrology.Tables[0].Rows[i]["TIMETAG"]);


                    //挑出ProcessStartTime塞入list,目前是用STDB Metrology裡的TIMETAG
                    listProcessStartTime.Add(Convert.ToDateTime(ds_Process.Tables[0].Rows[i]["TIMETAG"]));

                    //挑出ProcessEndTime塞入list,目前是用STDB Metrology裡的TIMETAG
                    listProcessEndTime.Add(Convert.ToDateTime(ds_Process.Tables[0].Rows[i]["TIMETAG"]));

                    //挑出MetrologyStartTime塞入list,目前是用STDB Metrology裡的TIMETAG
                    listMetrologyStartTime.Add(Convert.ToDateTime(ds_Metrology.Tables[0].Rows[i]["TIMETAG"]));

                    //挑出MetrologyEndTime塞入list,目前是用STDB Metrology裡的TIMETAG
                    listMetrologyEndTime.Add(Convert.ToDateTime(ds_Metrology.Tables[0].Rows[i]["TIMETAG"]));

                    double[] arrIndicatorValue = new double[indicatorIndexCount];
                    double[] arrPointValue = new double[pointIndexCount];

                    int[] arrActionValue = new int[indicatorIndexCount];

                    buffer = "";

                    for (int j = 0; j < indicatorIndexCount; j++)
                    {
                        buffer = ds_Process.Tables[0].Rows[i][processKeyList[j].DataField].ToString();
                        if (buffer == "")
                        {
                            buffer = "0";
                        }
                        arrIndicatorValue[j] = (double)Convert.ToDouble(buffer);

                        //buffer = ds_Process.Tables[0].Rows[i]["ActionNum"].ToString();
                        buffer = ds_Process.Tables[0].Rows[i]["blockID"].ToString();
                        if (buffer == "")
                        {
                            buffer = "1";   //default action=1
                        }
                        arrActionValue[j] = Int16.Parse(buffer);
                    }
                    listIndicatorPopulationValue.Add(arrIndicatorValue);
                    listActionPopulationValue.Add(arrActionValue);

                    for (int k = 0; k < pointIndexCount; k++)
                    {
                        //int columnNumber = (int)metrologyKeyList[k] + 4;
                        buffer = ds_Metrology.Tables[0].Rows[i][metrologyKeyList[k].DataField].ToString();
                        arrPointValue[k] = (double)Convert.ToDouble(buffer);
                    }
                    listPointPopulationValue.Add(arrPointValue);
                }
                #endregion

                population.WorkPieceID_Metrology = WorkPieceID_Metrology;
                population.WorkPieceID_Process = WorkPieceID_Process;
                population.listContexID = listContexID;
                population.listProcessStartTime = listProcessStartTime;
                population.listProcessEndTime = listProcessEndTime;
                population.listMetrologyStartTime = listMetrologyStartTime;
                population.listMetrologyEndTime = listMetrologyEndTime;
                population.listIndicatorPopulationValue = listIndicatorPopulationValue;
                population.listPointPopulationValue = listPointPopulationValue;
                population.listActionPopulationValue = listActionPopulationValue;

                //Set ID for indicators & points
                int id = 1;
                foreach (var pair in listAllIndicators)
                {
                    pair.Value.Number = id;
                    id++;
                }

                //points
                id = 1;
                foreach (var pair in listAllPoints)
                {
                    pair.Value.Number = id;
                    id++;
                }

                population.listAllIndicators = listAllIndicators;
                population.listAllPoints = listAllPoints;
                population.listContext = listContext;

               // Write_SGXML(population, 6, 4);

                return population;
            }
            catch (Exception e)
            {
                //Ack
                population.Ack = 1;

                writeLog(e);


                return population;
            }
        }

        // init TemoleFolder
        public int InitTempleFolder(In_UserInfo In_userinfo)
        {            
            //tempDir = Path.Combine(ConfigurationManager.AppSettings["EmulatedDiv"], ConfigurationManager.AppSettings["TempDir"]);
            
            if (!(Directory.Exists(tempDir)))
            {
                Directory.CreateDirectory(tempDir);
            }

            AddDirectorySecurity(tempDir, @"everyone", FileSystemRights.FullControl, AccessControlType.Allow);
            //////////////////////////////////////////////////////////////////////////
            //建立DTtemp下使用者目錄
            creatUserFolder(tempDir, In_userinfo.Company, In_userinfo.User, true);

            String TmpModelFileLocalPath = tempDir + In_userinfo.FullName + "\\";
            String strXMLPath = TmpModelFileLocalPath + "Parameter.xml";

            BlobOperatorClass BlobOperatoClient = new BlobOperatorClass(StorageConnectStringBlob, tempDir, In_userinfo.FullName + ".tmp");
            BlobOperatoClient.DeleteBlob(ModelBlobContainerName, In_userinfo.FullName + "/parameter.file");
            BlobOperatoClient.DeleteBlob(ModelBlobContainerName, In_userinfo.FullName + "/result.file");
            string ddd = BlobOperatoClient.UploadAsDirectory(TmpModelFileLocalPath, ModelBlobContainerName, In_userinfo.FullName + "/temporary.file", true);
            if (ddd.CompareTo("success") == 0)
                return 0;
            else
                return 1;
        }

        public string ChecktJobState(string JobID, In_UserInfo In_userinfo)
        {
            string ReturnStr = "";
            TableOperatorClass TableOperatorClient = new TableOperatorClass(StorageConnectStringTable);
            DataEntity_JobEvent DEJE = null;
            string partitionKey = In_userinfo.Company.ToLower();
            string rowKey = JobID;
            try
            {
                if (TableOperatorClient.GetEntity<DataEntity_JobEvent>(JobEventTableName, partitionKey, rowKey, out DEJE))
                {
                    ReturnStr = DEJE.State;
                }
                else
                {
                    ReturnStr = "Wait";
                }
            }
            catch (Exception ex)
            {
                ReturnStr = "fail:" + ex.ToString();
            }
            return ReturnStr;
        }

        public string Set_DataTransferModule(int contextIDCount, int trainContextIDCount,
            int runContextIDCount,
            List<MetrologyPoint> processKeyList, List<MetrologyPoint> metrologyKeyList,
            DateTime startTime, DateTime endTime,
            List<MetrologyPoint> combinedIndicator, List<MetrologyPoint> combinedPoint,
            List<Group> groupValue, In_UserInfo In_userinfo,
            string vMachineId, string CNCType, string CNCNumber,
            string NCProgram, string model_Id, string version, int[] allAction,
            List<String> listAbnormalList,
            List<String> listIsolatedList,
            String XTable,
            String YTable,
            String strSelectedID,
            String strProductID)    //for writting xml file
        {
            string JobID = "";
            TableOperatorClass TableOperatorClient = new TableOperatorClass(StorageConnectStringTable);
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "In Set_DataTransfer", true);
            try
            {
                GC.Collect();
                /////先於Local端建置該User的個人資料夾以建/////
                creatUserFolder(tempDir, In_userinfo.Company, In_userinfo.User);
                ///////////////////////////////////////////////
                JobID = Execute_DataTrancsferModule(
                    contextIDCount, trainContextIDCount, runContextIDCount, processKeyList,
                metrologyKeyList, startTime, endTime, combinedIndicator,
                combinedPoint, groupValue, In_userinfo, vMachineId,
                CNCType, CNCNumber, NCProgram, model_Id,
                version, allAction, listAbnormalList, listIsolatedList, XTable, YTable, strSelectedID
                );

#region SQL
                //////////////////////////////////////////////////////////////////////////
                // 取得SYSDEF資料來建出ELEMENTTYPE [10/18/2012 autolab]
                List<string> vAVMNameList = null;

                string sqlCommand = "SELECT AVMNAME FROM SYSDEF ORDER BY ORDERNUMBER";

                using (SqlConnection conn = new SqlConnection(DBConnectionString))
                {
                    try
                    {
                        conn.Open();
                        SqlDataAdapter dapt = new SqlDataAdapter(sqlCommand, conn);
                        DataSet ds = new DataSet();

                        dapt.Fill(ds);

                        int vMmachineIDCount = ds.Tables[0].Rows.Count;
                        vAVMNameList = new List<string>();
                        for (int i = 0; i < vMmachineIDCount; i++)
                        {
                            vAVMNameList.Add(ds.Tables[0].Rows[i].ItemArray[0].ToString()); //AVMNAME
                        }
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        vAVMNameList = new List<string>();
                        conn.Close();
                        writeLog(ex);
                    }
                    conn.Close();
                }

                //////////////////////////////////////////////////////////////////////////
                // 取得VARDEF資料來建出VariableDef [10/18/2012 autolab]
                List<string> vVARDEFList_Metrology = null;
                List<string> vVARDEFList_Process = null;

                sqlCommand = "SELECT FIELDNAME FROM VARDEF WHERE TYPE = 'METROLOGY' ORDER BY ORDERNUMBER";

                using (SqlConnection conn = new SqlConnection(DBConnectionString))
                {
                    try
                    {
                        conn.Open();
                        SqlDataAdapter dapt = new SqlDataAdapter(sqlCommand, conn);
                        DataSet ds = new DataSet();

                        dapt.Fill(ds);

                        int vMmachineIDCount = ds.Tables[0].Rows.Count;
                        vVARDEFList_Metrology = new List<string>();
                        for (int i = 0; i < vMmachineIDCount; i++)
                        {
                            vVARDEFList_Metrology.Add(ds.Tables[0].Rows[i].ItemArray[0].ToString()); //FIELDNAME
                        }
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        vVARDEFList_Metrology = new List<string>();
                        conn.Close();
                        writeLog(ex);
                    }
                    conn.Close();
                }

                sqlCommand = "SELECT FIELDNAME FROM VARDEF WHERE TYPE = 'PROCESS' ORDER BY ORDERNUMBER";

                using (SqlConnection conn = new SqlConnection(DBConnectionString))
                {
                    try
                    {
                        conn.Open();
                        SqlDataAdapter dapt = new SqlDataAdapter(sqlCommand, conn);
                        DataSet ds = new DataSet();

                        dapt.Fill(ds);

                        int vMmachineIDCount = ds.Tables[0].Rows.Count;
                        vVARDEFList_Process = new List<string>();
                        for (int i = 0; i < vMmachineIDCount; i++)
                        {
                            vVARDEFList_Process.Add(ds.Tables[0].Rows[i].ItemArray[0].ToString()); //FIELDNAME
                        }
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        vVARDEFList_Process = new List<string>();
                        conn.Close();
                        writeLog(ex);
                    }
                    conn.Close();
                }

                List<string> vMetrologyBlock = null;
                List<string> vMetrologyBlockTotal = new List<string>();

                sqlCommand = "SELECT metrologyblock from YTableDEF WHERE TABLENAME = '" + YTable + "'";

                using (SqlConnection conn = new SqlConnection(DBConnectionString))
                {
                    try
                    {
                        conn.Open();
                        SqlDataAdapter dapt = new SqlDataAdapter(sqlCommand, conn);
                        DataSet ds = new DataSet();

                        dapt.Fill(ds);

                        int vMmachineIDCount = ds.Tables[0].Rows.Count;
                        vMetrologyBlock = new List<string>();
                        for (int i = 0; i < vMmachineIDCount; i++)
                        {
                            vMetrologyBlock.Add(ds.Tables[0].Rows[i].ItemArray[0].ToString()); //FIELDNAME
                        }
                        conn.Close();

                        foreach (String strBlocks in vMetrologyBlock)
                        {
                            String[] Title = strBlocks.Split(new Char[] { ',' });

                            foreach(String strBlock in Title)
                            {
                                if (!vMetrologyBlockTotal.Contains(strBlock))
                                {
                                    vMetrologyBlockTotal.Add(strBlock);
                                }
                            }
                        }
                        vMetrologyBlockTotal.Sort();

                    }
                    catch (Exception ex)
                    {
                        vMetrologyBlock = new List<string>();
                        conn.Close();
                        writeLog(ex);
                    }
                    conn.Close();
                }
#endregion
#region Completed


                CheckXML(In_userinfo);
                ///////////////////////////////////////////////////////////////////////////
                //ConditionInfo
                CreateXMLObj = new CreateXML();
                                
                inStrategyName = "untitled";
                inStrategyCreateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                inModifier = In_userinfo.User;
                inStartSearchTime = startTime.ToString("yyyyMMddHHmmss");     
                inIsNormalUser = "False";
                inPhaseType = "1";
                inModelCreateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                inStrategyID = "101";
                
                CreateXMLObj.Create_ConditionInfo(MCSModelInfoFileName, 
                    inStrategyName,      
                    inStrategyCreateTime,
                    inModifier,          
                    inStartSearchTime,   
                    inEndSearchTime,     
                    inIsNormalUser,      
                    inPhaseType,         
                    inModelCreateTime, 
                    inStrategyID);

                //////////////////////////////////////////////////////////////////////////
                //ElementType
                List<String> ElementTypeSummaryList = new List<String>();
                foreach (String strAVMNAME in vAVMNameList)
                {
                    ElementTypeSummaryList.Add(strAVMNAME);
                }
                
                inElementTypeDic.Add("ElementTypeSummary", ElementTypeSummaryList);

                CreateXMLObj.Create_ElementType(MCSModelInfoFileName, inElementTypeDic);

                //////////////////////////////////////////////////////////////////////////
                //Combination
                List<String> MachineToolCode_List = new List<String>();
                MachineToolCode_List.Add(CNCNumber);
                List<String> ProductName_List = new List<String>();
                ProductName_List.Add(strProductID);
                inCombinationDic.Add("Machine_Tool_Code", MachineToolCode_List);
                inCombinationDic.Add("Product_Name", ProductName_List);

                CreateXMLObj.Create_Combination(MCSModelInfoFileName, inCombinationDic);

                //////////////////////////////////////////////////////////////////////////
                //PieceCount
                List<String> PieceCount_List = new List<String>();
                PieceCount_List.Add(contextIDCount.ToString());
                inPieceCountDic.Add("PieceCount", PieceCount_List);
                CreateXMLObj.Create_PieceCount(MCSModelInfoFileName, inPieceCountDic);

                //////////////////////////////////////////////////////////////////////////
                //VariableDef
                List<String> MetrologyDef_List = new List<String>();
                foreach (string strMetrology in vVARDEFList_Metrology)
                {
                    MetrologyDef_List.Add(strMetrology);
                }
                
                
                List<String> ProcessDef_List = new List<String>();
                foreach (string strMetrology in vVARDEFList_Process)
                {
                    ProcessDef_List.Add(strMetrology);
                }

                inVariableDefDic.Add("METROLOGYDEF;METROLOGY", MetrologyDef_List);  // 屬性請照順序塞，每個屬性中間用分號隔開
                inVariableDefDic.Add("PROCESSDEF;PROCESS", ProcessDef_List);  // 屬性請照順序塞，每個屬性中間用分號隔開

                CreateXMLObj.Create_VariableDef(MCSModelInfoFileName, inVariableDefDic);

                //////////////////////////////////////////////////////////////////////////
                //FileList
                List<String> MetrologyXML_List = new List<String>();
                foreach (string strMetrology in vVARDEFList_Metrology)
                {
                    MetrologyXML_List.Add(strMetrology);
                }
                List<String> ProcessXML_List = new List<String>();
                foreach (string strMetrology in vVARDEFList_Process)
                {
                    ProcessXML_List.Add(strMetrology);
                }

                inFileListDic.Add("METROLOGY01O.xml;METROLOGYDEF;METROLOGY01;1;O", MetrologyXML_List);
                inFileListDic.Add("PROCESS01I.xml;PROCESSDEF;PROCESS01;1;I", ProcessXML_List);

                CreateXMLObj.Create_FileList(MCSModelInfoFileName, inFileListDic);



                ////////////////////////////////////////////////////////////////////////
                //Metrology01O.xml
                int iMetrologyCounter = 1;
                foreach (MetrologyPoint MP in metrologyKeyList)
                {
                    inMetrologyTableInfoList.Add(
                        MP.Name + "," +
                        "0" + "," +
                        iMetrologyCounter.ToString()
                    );
                    iMetrologyCounter++;
                }

                CreateXMLObj.Create_MetrologyTableInfo(MCSModelInfoFileName, inMetrologyTableInfoList, "METROLOGY01O.xml");

                ////////////////////////////////////////////////////////////////////////
                //Process01I.xml
                int iProcessCounter = 0;
                                
                foreach (MetrologyPoint MP in processKeyList)
                {
                    iProcessCounter++;
                    inPROCESS01_IndicatorType.Add(
                        MP.Name + "," +
                        "0" + "," +
                        (metrologyKeyList.Count + iProcessCounter).ToString(), PROCESS01_BlockIDEmpty
                    );
                    
                }

                //只有PROCESS.xml會多這一項
                inPROCESS01_IndicatorType.Add(
                        "Block_ID" + "," +
                        "1" + "," +
                        (inMetrologyTableInfoList.Count + inPROCESS01_IndicatorType.Count +1).ToString(), vMetrologyBlockTotal
                    );                

                CreateXMLObj.Create_ProcessTableInfo(MCSModelInfoFileName, inPROCESS01_IndicatorType, "PROCESS01I.xml");

                


                //////////////////////////////////////////////////////////////////////////
                //DCQV                
                foreach (String str in vMetrologyBlockTotal)
                {
                    DCQV_TimePointMode.Add((inMetrologyTableInfoList.Count + inPROCESS01_IndicatorType.Count).ToString() +
                        "," + "1" + "," + str);
                }

                //name  filename   dcqvid
                inDCQV_TimePoint.Add("TimePointMode" + "," + "PROCESS01I.xml" + "," + "1", DCQV_TimePointMode);

                CreateXMLObj.Create_DCQV(MCSModelInfoFileName, inDCQV_TimePoint);


                //移除Block_ID項目
                inPROCESS01_IndicatorType.Remove("Block_ID" + "," +
                        "1" + "," +
                        (inMetrologyTableInfoList.Count + inPROCESS01_IndicatorType.Count).ToString());

                ////////////////////////////////////////////////////////////////////////
                //FilterTemporalRule
                iProcessCounter = 0;
                
                int iFilterID = 1;

                foreach (String strBlock in vMetrologyBlockTotal)
                {
                    FileTemporalRule_IndicatorValue = new List<string>();

                    FileTemporalRule_IndicatorValue.Add((metrologyKeyList.Count + processKeyList.Count + 1).ToString() + "," +
                        strBlock + "," + "trim" + "," + strBlock);

                    foreach (MetrologyPoint MP in processKeyList)
                    {
                        inFileTemporalRule_Indicator.Add(
                            "None_" + MP.Name + "_Filter" + iFilterID + "," +   //name
                            (metrologyKeyList.Count + MP.Value).ToString() + "," +                //variableid
                            "1" + "," +                 //dcqvid
                            "PROCESS01I.xml" + "," +    //rawdatafilename
                            iFilterID + "th filter rule of variable" + MP.Name + "," +
                            "" + "," + "" + "," + "" + "," + "" + "," +
                            "1" + "," +                 //endcount
                            "1" + "," +                 //startcount
                            (iProcessCounter + 1).ToString()//filterid
                            , FileTemporalRule_IndicatorValue
                        );
                        iProcessCounter++;
                    } iFilterID++;
                }


                CreateXMLObj.Create_FilterTemporalRule(MCSModelInfoFileName, inFileTemporalRule_Indicator);


                

                //////////////////////////////////////////////////////////////////////////
                //AlgorithmList
                CreateXMLObj.Create_AlgorithmList(MCSModelInfoFileName);
#endregion

                ////////////////////////////////////////////////////////////////////////
                //IndicatorRule

                iProcessCounter = 0;
                iFilterID = 1;

                foreach (String strBlock in vMetrologyBlockTotal)
                {
                    FileTemporalRule_IndicatorValue = new List<string>();

                    FileTemporalRule_IndicatorValue.Add((metrologyKeyList.Count + processKeyList.Count + 1).ToString() + "," +
                        strBlock + "," + "trim" + "," + strBlock);

                    foreach (MetrologyPoint MP in processKeyList)
                    {
                        inIndicatorRuleList.Add(
                            "None_" + MP.Name + "_Filter" + iFilterID + "_Mean" + "," +   //name
                            (metrologyKeyList.Count + MP.Value).ToString() + "," +                //variableid
                            "Indicator with Mean algorithm of Variable " + MP.Name + ".," +
                            MP.LCL + "," +
                            //MP.UCL + "," +
                            "99999" + "," +
                            MP.LSL + "," +
                            //MP.USL + "," +
                            "99999" + "," +
                            (iProcessCounter + 1).ToString() + "," +    //filterid
                            "1" + "," +                                 //algorithmid
                            (iProcessCounter + 1).ToString()            //filterid 
                        );
                        iProcessCounter++;
                    } iFilterID++;
                }


                CreateXMLObj.Create_IndicatorRule(MCSModelInfoFileName, inIndicatorRuleList);

                ////////////////////////////////////////////////////////////////////////
                //IndicatorList                

                iProcessCounter = 0;
                iFilterID = 1;

                foreach (String strBlock in vMetrologyBlockTotal)
                {
                    FileTemporalRule_IndicatorValue = new List<string>();

                    FileTemporalRule_IndicatorValue.Add((metrologyKeyList.Count + processKeyList.Count + 1).ToString() + "," +
                        strBlock + "," + "trim" + "," + strBlock);

                    foreach (MetrologyPoint MP in processKeyList)
                    {
                        inIndicatorRule_Indicator.Add(
                            "None_" + MP.Name + "_Filter" + iFilterID + "_Mean" + "," +   //name
                            (metrologyKeyList.Count + MP.Value).ToString() + "," +                //variableid
                            MP.LCL + "," +
                            //MP.UCL + "," +
                            "99999" + "," +
                            MP.LSL + "," +
                            //MP.USL + "," +
                            "99999" + "," +

                            (iProcessCounter + 1).ToString() + "," +    //filterid
                            "1" + "," +                                 //algorithmid
                            (iProcessCounter + 1).ToString() + "," +    //indicatorid 
                            "1" + "," +                                 //enable
                            "1",                                        //steptypeid
                            IndicatorRule_IndicatorValueEmpty
                        );
                        iProcessCounter++;
                    } iFilterID++;
                }

                CreateXMLObj.Create_IndicatorList(MCSModelInfoFileName, inIndicatorRule_Indicator);

                //////////////////////////////////////////////////////////////////////////
                //ContourInfo
                List<String> ContourInfo_Value = new List<string>();
                ContourInfo_Value.Add("General");
                inContourInfo.Add("Product", ContourInfo_Value);

                List<String> ContourInfo_ShapeValue = new List<string>();
                ContourInfo_ShapeValue.Add("Square");
                inContourInfo.Add("Shape", ContourInfo_ShapeValue);

                List<String> ContourInfo_ColorGradientValue = new List<string>();
                ContourInfo_ColorGradientValue.Add("SunWellRainbow");
                inContourInfo.Add("ColorGradient", ContourInfo_ColorGradientValue);

                List<String> ContourInfo_ScaleXMinValue = new List<string>();
                ContourInfo_ScaleXMinValue.Add("0");
                inContourInfo.Add("ScaleXMin", ContourInfo_ScaleXMinValue);

                List<String> ContourInfo_ScaleXMaxValue = new List<string>();
                ContourInfo_ScaleXMaxValue.Add("1");
                inContourInfo.Add("ScaleXMax", ContourInfo_ScaleXMaxValue);

                List<String> ContourInfo_ScaleYMinValue = new List<string>();
                ContourInfo_ScaleYMinValue.Add("0");
                inContourInfo.Add("ScaleYMin", ContourInfo_ScaleYMinValue);

                List<String> ContourInfo_ScaleYMaxValue = new List<string>();
                ContourInfo_ScaleYMaxValue.Add("1");
                inContourInfo.Add("ScaleYMax", ContourInfo_ScaleYMaxValue);

                List<String> ContourInfo_ScaleZMinValue = new List<string>();
                ContourInfo_ScaleZMinValue.Add("0");
                inContourInfo.Add("ScaleZMin", ContourInfo_ScaleZMinValue);

                List<String> ContourInfo_ScaleZMaxValue = new List<string>();
                ContourInfo_ScaleZMaxValue.Add("0");
                inContourInfo.Add("ScaleZMax", ContourInfo_ScaleZMaxValue);
                
                CreateXMLObj.Create_ContourInfo(MCSModelInfoFileName, inContourInfo);

                //////////////////////////////////////////////////////////////////////////
                //PointList
                //List<String> PointList_PointValue = new List<string>();

                int iPointCounter = 0;
                foreach (MetrologyPoint MP in combinedPoint)
                {
                    List<String> PointList_PointValue = new List<string>();
                    PointList_PointValue.Add((iPointCounter + 1).ToString());

                    inPointList_Point.Add("Point" + (iPointCounter + 1).ToString() + "," +
                        MP.LCL + "," +
                        MP.UCL + "," +
                        //"99999" + "," +
                        MP.LSL + "," +
                        MP.USL + "," +
                        //"99999" + "," +
                        "1" + "," +
                        "Mean to Product_Type - Product_Name:" + MP.Name + "," +
                        "1" + "," +
                        (iPointCounter + 1).ToString() + "," +
                        "0.01" + "," +
                        "1" + "," +
                        (iPointCounter + 1).ToString()
                        , PointList_PointValue                  
                    );
                    iPointCounter++;
                }

                CreateXMLObj.Create_PointList(MCSModelInfoFileName, inPointList_Point);

                ////////////////////////////////////////////////////////////////////////
                //GroupList
                int iGroupCounter = 0;
                foreach (Group GV in groupValue)
                {
                    Dictionary<String, List<String>> GroupDic = new Dictionary<String, List<String>>();
                    inGroupListDic.Add("Group" + GV.GroupId, GroupDic);
                    //inGroupListDic.
                }

                CreateXMLObj.Create_GroupList(MCSModelInfoFileName, inGroupListDic);

                
                


                //////////////////////////////////////////////////////////////////////////
                //ExpertKnowledge
                List<String> inExpertKnowledgeList = new List<String>();
                inExpertKnowledgeList.Add("ByGroup");
                inExpertKnowledgeDic.Add("Accordance", inExpertKnowledgeList);

                CreateXMLObj.Create_ExpertKnowledge(MCSModelInfoFileName, inExpertKnowledgeDic);

                ////////////////////////////////////////////////////////////////////////
                //Groups
                iGroupCounter = 0;
                              


                inGroupListDic = new Dictionary<string, Dictionary<string, List<string>>>();
                foreach (Group GV in groupValue)
                {
                    Dictionary<String, List<String>> GroupDics = new Dictionary<String, List<String>>();

                    List<String> Group1IndicatorList = new List<String>();
                    List<String> Group1ExpertList = new List<String>();

                    //foreach (MetrologyPoint MPs in GV.IndicatorList)
                    //{
                    //    Group1IndicatorList.Add(MPs.Value.ToString());
                    //}

                    //////////////////////////////////////////////////////////////////////////

                    foreach (MetrologyPoint MPs in GV.IndicatorList)
                    {
                        iProcessCounter = 0;
                        foreach (String strBlock in vMetrologyBlockTotal)
                        {
                            foreach (MetrologyPoint MP in processKeyList)
                            {
                                if (strBlock + "_" + MP.Name == MPs.Name)
                                {
                                    Group1IndicatorList.Add((iProcessCounter + 1).ToString());//放入對應的IndicatorID
                                    Group1ExpertList.Add((iProcessCounter + 1).ToString());//放入對應的IndicatorID
                                }                                    

                                iProcessCounter++;
                            } iFilterID++;
                        }
                    }
                    
                    






                    //////////////////////////////////////////////////////////////////////////
                    
                    List<String> Group1PointList = new List<String>();
                        Group1PointList.Add(GV.GroupId.ToString());
                    //List<String> Group1ExpertList = new List<String>();
                    //foreach (MetrologyPoint MPs in GV.IndicatorList)
                    //{
                    //    Group1ExpertList.Add(MPs.Value.ToString());
                    //}

                        GroupDics.Add("GroupIndicatorList", Group1IndicatorList);
                        GroupDics.Add("GroupPointList", Group1PointList);
                        GroupDics.Add("GroupExpertList", Group1ExpertList);

                        inGroupListDic.Add("Group" + GV.GroupId, GroupDics);
                    //inGroupListDic.
                }

                CreateXMLObj.Create_GroupInfo(MCSModelInfoFileName, inGroupListDic);

                








                CreateXMLObj.Create_ModuleList(MCSModelInfoFileName, "true", "true", "true", 
                    "KISS", "EK", "MMR", "MahalanobisDistance",
                    "BPNN", "MRConjectureHistory;BPNNConjectureHistory", "false");

                foreach (Group GV in groupValue)
                {
                    inRITolerantMaxError.Add("5");
                }
                CreateXMLObj.Create_RI_Parameters(MCSModelInfoFileName, inRITolerantMaxError);

            }
            catch (System.Exception ex)
            {
                JobID = "";
                InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Error", "At Set_DataTransfer:" + ex.ToString(), true);
            }
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "Out Set_DataTransfer", true);
            GC.Collect();
            return JobID;
        }

        public int Get_DataTransferResult(string JobID, In_UserInfo In_userinfo)
        {
            TableOperatorClass TableOperatorClient = new TableOperatorClass(StorageConnectStringTable);
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "In Get_DataTransferResult", true);

            int out_Result = 1;

            try
            {
                GC.Collect();
                /////先於Local端建置該User的個人資料夾以建/////
                creatUserFolder(tempDir, In_userinfo.Company, In_userinfo.User);
                ///////////////////////////////////////////////
                out_Result = Return_DataTransferResult(TableOperatorClient, JobID, In_userinfo);
            }
            catch (System.Exception ex)
            {
                InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Error", "At Get_DataTransferResult:" + ex.ToString(), true);
                out_Result = 1;
            }
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "Out Get_DataTransferResult", true);
            return out_Result;
        }

        public string Set_MDFRModule(double EwmaLamda, double EwmaTolerance, double EwmaWindow, double VarConfidence, double baseSampleNum, double RangeMultipleValue, In_UserInfo In_userinfo)
        {
            string JobID = "";
            TableOperatorClass TableOperatorClient = new TableOperatorClass(StorageConnectStringTable);
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "In Set_MDFRModule", true);

            try
            {
                GC.Collect();
                /////先於Local端建置該User的個人資料夾以建/////
                creatUserFolder(tempDir, In_userinfo.Company, In_userinfo.User);
                ///////////////////////////////////////////////
                JobID = Execute_MDFRModule(EwmaLamda, EwmaTolerance, EwmaWindow, VarConfidence, baseSampleNum, RangeMultipleValue, In_userinfo);

                CheckXML(In_userinfo);
                CreateXMLObj = new CreateXML();

                inMDFREwmalamda = EwmaLamda.ToString();
                inMDFREwmaWindow = EwmaWindow.ToString();
                inMDFREwmaTolerance = EwmaTolerance.ToString();

                inMDFRVarConfidence = VarConfidence.ToString();
                inMDFRbaseSampleNum = baseSampleNum.ToString();
                inMDFRRangeMultipleValue = RangeMultipleValue.ToString();

                CreateXMLObj.Create_MDFR_Parameters(MCSModelInfoFileName, inMDFREwmalamda, inMDFREwmaWindow, inMDFREwmaTolerance,
                    inMDFRVarConfidence, inMDFRbaseSampleNum, inMDFRRangeMultipleValue);

            }
            catch (System.Exception ex)
            {
                JobID = "";
                InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Error", "At Set_MDFRModule:" + ex.ToString(), true);
            }
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "Out Set_MDFRModule", true);
            GC.Collect();
            return JobID;
        }

        public Out_MDFR Get_MDFRResult(string JobID, In_UserInfo In_userinfo)
        {
            TableOperatorClass TableOperatorClient = new TableOperatorClass(StorageConnectStringTable);
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "In Get_MDFRResult", true);

            Out_MDFR out_Result = new Out_MDFR();
            try
            {
                GC.Collect();
                /////先於Local端建置該User的個人資料夾以建/////
                creatUserFolder(tempDir, In_userinfo.Company, In_userinfo.User);
                ///////////////////////////////////////////////
                Return_MDFRResult(out_Result, TableOperatorClient, JobID, In_userinfo);
            }
            catch (System.Exception ex)
            {
                InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Error", "At Get_MDFRResult:" + ex.ToString(), true);
                out_Result = null;
            }
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "Out Get_MDFRResult", true);
            return out_Result;
        }

        public string Set_DQIyModule_GetDQIyPattern(In_UserInfo In_userinfo)
        {
            string JobID = "";
            TableOperatorClass TableOperatorClient = new TableOperatorClass(StorageConnectStringTable);
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "In Set_DQIyModule_GetDQIyPattern", true);
            try
            {
                GC.Collect();
                /////先於Local端建置該User的個人資料夾以建/////
                creatUserFolder(tempDir, In_userinfo.Company, In_userinfo.User);
                ///////////////////////////////////////////////
                JobID = Execute_DQIyModule_GetDQIyPattern(0.0001, 1, In_userinfo);
            }
            catch (System.Exception ex)
            {
                JobID = "";
                InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Error", "At Set_DQIyModule_GetDQIyPattern:" + ex.ToString(), true);
            }
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "Out Set_DQIyModule_GetDQIyPattern", true);
            GC.Collect();
            return JobID;
        }

        public Out_DQIy_CleanAbnormalY Get_DQIyResult_GetDQIyPattern(string JobID, In_UserInfo In_userinfo)
        {
            TableOperatorClass TableOperatorClient = new TableOperatorClass(StorageConnectStringTable);
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "In Get_DQIyResult_GetDQIyPattern", true);

            Out_DQIy_CleanAbnormalY out_Result = new Out_DQIy_CleanAbnormalY();
            try
            {
                GC.Collect();
                /////先於Local端建置該User的個人資料夾以建/////
                creatUserFolder(tempDir, In_userinfo.Company, In_userinfo.User);
                ///////////////////////////////////////////////
                Return_DQIyResult_GetDQIyPattern(out_Result, TableOperatorClient, JobID, In_userinfo);
            }
            catch (System.Exception ex)
            {
                InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Error", "At Get_DQIyResult_GetDQIyPattern:" + ex.ToString(), true);
                out_Result = null;
            }
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "Out Get_DQIyResult_GetDQIyPattern", true);
            return out_Result;
        }

        public string Set_KSSModule(string AlgorithmSelection, double ClusterNumber, In_UserInfo In_userinfo)
        {
            string JobID = "";
            TableOperatorClass TableOperatorClient = new TableOperatorClass(StorageConnectStringTable);
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "In Set_KSSModule", true);
            try
            {
                GC.Collect();
                /////先於Local端建置該User的個人資料夾以建/////
                creatUserFolder(tempDir, In_userinfo.Company, In_userinfo.User);
                ///////////////////////////////////////////////
                JobID = Execute_KSSModule(AlgorithmSelection, ClusterNumber, In_userinfo);
                CheckXML(In_userinfo);
                CreateXMLObj = new CreateXML();

                inKSSClusterNumber = ClusterNumber.ToString();

                CreateXMLObj.Create_KSS_Parameters(MCSModelInfoFileName, inKSSClusterNumber);

            }
            catch (System.Exception ex)
            {
                JobID = "";
                InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Error", "At Set_KSSModule:" + ex.ToString(), true);
            }
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "Out Set_KSSModule", true);
            GC.Collect();
            return JobID;
        }

        public Out_KSS Get_KSSResult(string JobID, In_UserInfo In_userinfo)
        {
            TableOperatorClass TableOperatorClient = new TableOperatorClass(StorageConnectStringTable);
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "In Get_KSSResult", true);

            Out_KSS out_Result = new Out_KSS();
            try
            {
                GC.Collect();
                /////先於Local端建置該User的個人資料夾以建/////
                creatUserFolder(tempDir, In_userinfo.Company, In_userinfo.User);
                ///////////////////////////////////////////////
                Return_KSSModule(out_Result, TableOperatorClient, JobID, In_userinfo);
            }
            catch (System.Exception ex)
            {
                InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Error", "At Get_KSSResult:" + ex.ToString(), true);
                out_Result = null;
            }
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "Out Get_KSSResult", true);
            return out_Result;
        }

        public string Set_KVSModule(double Fin_apha, double Fout_apha, string InOneByOneChoose, string AlgorithmSelection, In_UserInfo In_userinfo)
        {
            string JobID = "";
            TableOperatorClass TableOperatorClient = new TableOperatorClass(StorageConnectStringTable);
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "In Set_KVSModule", true);
            try
            {
                GC.Collect();
                /////先於Local端建置該User的個人資料夾以建/////
                creatUserFolder(tempDir, In_userinfo.Company, In_userinfo.User);
                ///////////////////////////////////////////////
                JobID = Execute_KVSModule(Fin_apha, Fout_apha, InOneByOneChoose, AlgorithmSelection, In_userinfo);
                CheckXML(In_userinfo);
                CreateXMLObj = new CreateXML();

                inKVSFin_apha = Fin_apha.ToString();
                inKVSFout_apha = Fout_apha.ToString();
                inKVSOneByOneChoose = InOneByOneChoose.ToString();

                CreateXMLObj.Create_KVS_Parameters(MCSModelInfoFileName, inKVSFin_apha, inKVSFout_apha, inKVSOneByOneChoose);
            }
            catch (System.Exception ex)
            {
                JobID = "";
                InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Error", "At Set_KVSModule:" + ex.ToString(), true);
            }
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "Out Set_KVSModule", true);
            GC.Collect();
            return JobID;
        }

        public Out_KVS Get_KVSResult(string JobID, In_UserInfo In_userinfo)
        {
            TableOperatorClass TableOperatorClient = new TableOperatorClass(StorageConnectStringTable);
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "In Get_KVSResult", true);

            Out_KVS out_Result = new Out_KVS();
            try
            {
                GC.Collect();
                /////先於Local端建置該User的個人資料夾以建/////
                creatUserFolder(tempDir, In_userinfo.Company, In_userinfo.User);
                ///////////////////////////////////////////////
                Return_KVSModule(out_Result, TableOperatorClient, JobID, In_userinfo);

                

            }
            catch (System.Exception ex)
            {
                InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Error", "At Get_KVSResult:" + ex.ToString(), true);
                out_Result = null;
            }
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "Out Get_KVSResult", true);
            return out_Result;
        }

        public string Set_DQIxModule_VerifyDQIx(double InLambda, double InConstant, double DQIxFilterPercentage, double DQIxRefreshCounter, In_UserInfo In_userinfo)
        {
            string JobID = "";
            TableOperatorClass TableOperatorClient = new TableOperatorClass(StorageConnectStringTable);
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "In Set_DQIxModule_VerifyDQIx", true);
            try
            {
                GC.Collect();
                /////先於Local端建置該User的個人資料夾以建/////
                creatUserFolder(tempDir, In_userinfo.Company, In_userinfo.User);
                ///////////////////////////////////////////////
                JobID = Execute_DQIxModule_VerifyDQIx(InLambda, InConstant, DQIxFilterPercentage, DQIxRefreshCounter, In_userinfo);
                CheckXML(In_userinfo);
                CreateXMLObj = new CreateXML();

                inDQIxLambda = InLambda.ToString();
                inDQIxConstant = InConstant.ToString();
                inDQIxDQIxFilterPercentage = DQIxFilterPercentage.ToString();
                inDQIxDQIxRefreshCounter = DQIxRefreshCounter.ToString();

                CreateXMLObj.Create_DQIx_Parameters(MCSModelInfoFileName, inDQIxLambda, inDQIxConstant, inDQIxDQIxFilterPercentage, inDQIxDQIxRefreshCounter);

            }
            catch (System.Exception ex)
            {
                JobID = "";
                InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Error", "At Set_DQIxModule_VerifyDQIx:" + ex.ToString(), true);
            }
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "Out Set_DQIxModule_VerifyDQIx", true);
            GC.Collect();
            return JobID;
        }

        public Out_VerifyDQIxResult Get_DQIxResult_VerifyDQIx(string JobID, In_UserInfo In_userinfo)
        {
            TableOperatorClass TableOperatorClient = new TableOperatorClass(StorageConnectStringTable);
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "In Get_DQIxResult_VerifyDQIx", true);

            Out_VerifyDQIxResult out_Result = new Out_VerifyDQIxResult();
            try
            {
                GC.Collect();
                /////先於Local端建置該User的個人資料夾以建/////
                creatUserFolder(tempDir, In_userinfo.Company, In_userinfo.User);
                ///////////////////////////////////////////////
                Return_DQIxResult_VerifyDQIx(out_Result, TableOperatorClient, JobID, In_userinfo);
            }
            catch (System.Exception ex)
            {
                InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Error", "At Get_DQIxResult_VerifyDQIx:" + ex.ToString(), true);
                out_Result = null;
            }
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "Out Get_DQIxResult_VerifyDQIx", true);
            return out_Result;
        }

        public string Set_DQIyModule_VerifyDQIy(double corralpha, double IsMixedModel, In_UserInfo In_userinfo)
        {
            string JobID = "";
            TableOperatorClass TableOperatorClient = new TableOperatorClass(StorageConnectStringTable);
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "In Set_DQIyModule_VerifyDQIy", true);
            try
            {
                GC.Collect();
                /////先於Local端建置該User的個人資料夾以建/////
                creatUserFolder(tempDir, In_userinfo.Company, In_userinfo.User);
                ///////////////////////////////////////////////
                JobID = Execute_DQIyModule_VerifyDQIy(corralpha, IsMixedModel, In_userinfo);
                CheckXML(In_userinfo);
                CreateXMLObj = new CreateXML();

                inDQIycorralpha = corralpha.ToString();
                inDQIyIsMixedModel = IsMixedModel.ToString();

                CreateXMLObj.Create_DQIy_Parameters(MCSModelInfoFileName, inDQIycorralpha, inDQIyIsMixedModel);
            }
            catch (System.Exception ex)
            {
                JobID = "";
                InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Error", "At Set_DQIyModule_VerifyDQIy:" + ex.ToString(), true);
            }
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "Out Set_DQIyModule_VerifyDQIy", true);
            GC.Collect();
            return JobID;
        }

        public Out_VerifyDQIyResult Get_DQIyResult_VerifyDQIy(string JobID, In_UserInfo In_userinfo)
        {
            TableOperatorClass TableOperatorClient = new TableOperatorClass(StorageConnectStringTable);
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "In Get_DQIyResult_VerifyDQIy", true);

            Out_VerifyDQIyResult out_Result = new Out_VerifyDQIyResult();
            try
            {
                GC.Collect();
                /////先於Local端建置該User的個人資料夾以建/////
                creatUserFolder(tempDir, In_userinfo.Company, In_userinfo.User);
                ///////////////////////////////////////////////
                Return_DQIyResult_VerifyDQIy(out_Result, TableOperatorClient, JobID, In_userinfo);
            }
            catch (System.Exception ex)
            {
                InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Error", "At Get_DQIyResult_VerifyDQIy:" + ex.ToString(), true);
                out_Result = null;
            }
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "Out Get_DQIyResult_VerifyDQIy", true);
            return out_Result;
        }

        public string Set_BPNNModule(
            double iMomTermRange_Min, double iMomTermRange_Int, double iMomTermRange_Max, 
            double iAlphaRange_Min, double iAlphaRange_Int, double iAlphaRange_Max, 
            double iEpochsRange_1, double iEpochsRange_2, double iEpochsRange_3, 
            List<double> iNodesRange, 
            string iInOneByOneChoose, double iBPNNRefreshCounter, 
            In_UserInfo In_userinfo)
        {
            string JobID = "";
            TableOperatorClass TableOperatorClient = new TableOperatorClass(StorageConnectStringTable);
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "In Set_BPNNModule", true);
            try
            {
                GC.Collect();
                /////先於Local端建置該User的個人資料夾以建/////
                creatUserFolder(tempDir, In_userinfo.Company, In_userinfo.User);
                ///////////////////////////////////////////////
                JobID = Execute_BPNNModule(iMomTermRange_Min, iMomTermRange_Int, iMomTermRange_Max, iAlphaRange_Min, iAlphaRange_Int, iAlphaRange_Max, iEpochsRange_1, iEpochsRange_2, iEpochsRange_3, iNodesRange, iInOneByOneChoose, iBPNNRefreshCounter, In_userinfo);

                CheckXML(In_userinfo);
                CreateXMLObj = new CreateXML();

                inBPNNEpochsRange1 = iEpochsRange_1.ToString();
                inBPNNEpochsRange2 = iEpochsRange_2.ToString();
                inBPNNEpochsRange3 = iEpochsRange_3.ToString();
                inBPNNMomTermRange1 = iMomTermRange_Min.ToString();
                inBPNNMomTermRange2 = iMomTermRange_Int.ToString();
                inBPNNMomTermRange3 = iMomTermRange_Max.ToString();
                inBPNNAlphaRange1 = iAlphaRange_Min.ToString();
                inBPNNAlphaRange2 = iAlphaRange_Int.ToString();
                inBPNNAlphaRange3 = iAlphaRange_Max.ToString();

                foreach (double dNode in iNodesRange)
                {
                    inBPNNNodesRange.Add(dNode.ToString());
                }

                inBPNNOneByOneChoose = iInOneByOneChoose;
                inBPNNBPNNRefreshCounter = iBPNNRefreshCounter.ToString();


                CreateXMLObj.Create_BPNN_Parameters(MCSModelInfoFileName,
                    inBPNNEpochsRange1, inBPNNEpochsRange2, inBPNNEpochsRange3,
                    inBPNNMomTermRange1, inBPNNMomTermRange2, inBPNNMomTermRange3,
                    inBPNNAlphaRange1, inBPNNAlphaRange2, inBPNNAlphaRange3,
                    "", "", "",
                    inBPNNOneByOneChoose,
                    inBPNNBPNNRefreshCounter,
                    inBPNNNodesRange);

            }
            catch (System.Exception ex)
            {
                JobID = "";
                InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Error", "At Set_BPNNModule:" + ex.ToString(), true);
            }
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "Out Set_BPNNModule", true);
            GC.Collect();
            return JobID;
        }

        public Out_BPNN Get_BPNNResult(string JobID, In_UserInfo In_userinfo)
        {
            TableOperatorClass TableOperatorClient = new TableOperatorClass(StorageConnectStringTable);
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "In Get_BPNNResult", true);

            Out_BPNN out_Result = new Out_BPNN();
            try
            {
                GC.Collect();
                /////先於Local端建置該User的個人資料夾以建/////
                creatUserFolder(tempDir, In_userinfo.Company, In_userinfo.User);
                ///////////////////////////////////////////////
                Return_BPNNResult(out_Result, TableOperatorClient, JobID, In_userinfo);
            }
            catch (System.Exception ex)
            {
                InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Error", "At Get_BPNNResult:" + ex.ToString(), true);
                out_Result = null;
            }
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "Out Get_BPNNResult", true);
            return out_Result;
        }

        public string Set_MRModule(string iInSelectAlgorithm, double iInMR_TSVD_Condition_Number_Criteria, double iInMR_TSVD_Energy_Ratio_Criteria, double iMRRefreshCounter, In_UserInfo In_userinfo)
        {
            string JobID = "";
            TableOperatorClass TableOperatorClient = new TableOperatorClass(StorageConnectStringTable);
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "In Set_MRModule", true);
            try
            {
                GC.Collect();
                /////先於Local端建置該User的個人資料夾以建/////
                creatUserFolder(tempDir, In_userinfo.Company, In_userinfo.User);
                ///////////////////////////////////////////////
                JobID = Execute_MRModule(iInSelectAlgorithm, iInMR_TSVD_Condition_Number_Criteria, iInMR_TSVD_Energy_Ratio_Criteria, iMRRefreshCounter, In_userinfo);

                CheckXML(In_userinfo);
                CreateXMLObj = new CreateXML();

                inMR_TSVD_Condition_Number_Criteria = iInMR_TSVD_Condition_Number_Criteria.ToString();
                inMR_TSVD_Energy_Ratio_Criteria = iInMR_TSVD_Energy_Ratio_Criteria.ToString();
                inMRRefreshCounter = iMRRefreshCounter.ToString();

                CreateXMLObj.Create_MR_Parameters(MCSModelInfoFileName, inMR_TSVD_Condition_Number_Criteria, inMR_TSVD_Energy_Ratio_Criteria, inMRRefreshCounter);
            }
            catch (System.Exception ex)
            {
                JobID = "";
                InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Error", "At Set_MRModule:" + ex.ToString(), true);
            }
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "Out Set_MRModule", true);
            GC.Collect();
            return JobID;
        }

        public Out_MR Get_MRResult(string JobID, In_UserInfo In_userinfo)
        {
            TableOperatorClass TableOperatorClient = new TableOperatorClass(StorageConnectStringTable);
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "In Get_MRResult", true);

            Out_MR out_Result = new Out_MR();
            try
            {
                GC.Collect();
                /////先於Local端建置該User的個人資料夾以建/////
                creatUserFolder(tempDir, In_userinfo.Company, In_userinfo.User);
                ///////////////////////////////////////////////
                Return_MRResult(out_Result, TableOperatorClient, JobID, In_userinfo);
            }
            catch (System.Exception ex)
            {
                InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Error", "At Get_MRResult:" + ex.ToString(), true);
                out_Result = null;
            }
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "Out Get_MRResult", true);
            return out_Result;
        }

        public string Set_RIModule(bool FirstAlgoValue, string FirstAlgoName, List<double[]> FitstPredictValue, bool SecondAlgoValue, string SecondAlgoName, List<double[]> SecondPredictValue, double iTolerant_MaxError, string iInSelectCalculator, In_UserInfo In_userinfo)
        {
            string JobID = "";
            TableOperatorClass TableOperatorClient = new TableOperatorClass(StorageConnectStringTable);
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "In Set_RIModule", true);
            try
            {
                GC.Collect();
                /////先於Local端建置該User的個人資料夾以建/////
                creatUserFolder(tempDir, In_userinfo.Company, In_userinfo.User);
                ///////////////////////////////////////////////
                JobID = Execute_RIModule(FirstAlgoValue, FirstAlgoName, FitstPredictValue, SecondAlgoValue, SecondAlgoName, SecondPredictValue, iTolerant_MaxError, iInSelectCalculator, In_userinfo);

                //CheckXML(In_userinfo);
                //CreateXMLObj = new CreateXML();

                
                
            }
            catch (System.Exception ex)
            {
                JobID = "";
                InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Error", "At Set_RIModule:" + ex.ToString(), true);
            }
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "Out Set_RIModule", true);
            GC.Collect();
            return JobID;
        }

        public Out_RI Get_RIResult(string JobID, In_UserInfo In_userinfo)
        {
            TableOperatorClass TableOperatorClient = new TableOperatorClass(StorageConnectStringTable);
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "In Get_RIResult", true);

            Out_RI out_Result = new Out_RI();
            try
            {
                GC.Collect();
                /////先於Local端建置該User的個人資料夾以建/////
                creatUserFolder(tempDir, In_userinfo.Company, In_userinfo.User);
                ///////////////////////////////////////////////
                Return_RIResult(out_Result, TableOperatorClient, JobID, In_userinfo);
            }
            catch (System.Exception ex)
            {
                InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Error", "At Get_RIResult:" + ex.ToString(), true);
                out_Result = null;
            }
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "Out Get_RIResult", true);
            return out_Result;
        }

        public string Set_GSIModule(int numberOfGroup, string iInSelectAlgorithm, double iGSI_RT, double iGSI_Threshold, 
            double iRefreshCounter, double iInGSI_TSVD_Condition_Number_Criteria, double iInGSI_TSVD_Energy_Ratio_Criteria, In_UserInfo In_userinfo)
        {
            string JobID = "";
            TableOperatorClass TableOperatorClient = new TableOperatorClass(StorageConnectStringTable);
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "In Set_GSIModule", true);
            try
            {
                GC.Collect();
                /////先於Local端建置該User的個人資料夾以建/////
                creatUserFolder(tempDir, In_userinfo.Company, In_userinfo.User);
                ///////////////////////////////////////////////
                JobID = Execute_GSIModule(numberOfGroup, iInSelectAlgorithm, iGSI_RT, iGSI_Threshold, iRefreshCounter, iInGSI_TSVD_Condition_Number_Criteria, iInGSI_TSVD_Energy_Ratio_Criteria, In_userinfo);

                //CreateXML(In_userinfo);

                CheckXML(In_userinfo);
                CreateXMLObj = new CreateXML();

                inMR_TSVD_Condition_Number_Criteria = iInGSI_TSVD_Condition_Number_Criteria.ToString();
                inMR_TSVD_Energy_Ratio_Criteria = iInGSI_TSVD_Energy_Ratio_Criteria.ToString();
                inMRRefreshCounter = iRefreshCounter.ToString();

                inGSIRefreshCounter = iRefreshCounter.ToString();
                inGSI_TSVD_Condition_Number_Criteria = iInGSI_TSVD_Condition_Number_Criteria.ToString();
                inGSI_TSVD_Energy_Ratio_Criteria = iInGSI_TSVD_Energy_Ratio_Criteria.ToString();


                for (int i = 0; i < numberOfGroup; i++ )
                {
                    inGSIRT.Add(iGSI_RT.ToString());
                    inGSIThreshold.Add(iGSI_Threshold.ToString());
                }
                

                CreateXMLObj.Create_GSI_Parameters(MCSModelInfoFileName, inMRRefreshCounter, inMR_TSVD_Condition_Number_Criteria, inGSI_TSVD_Energy_Ratio_Criteria,
                    inGSIRT, inGSIThreshold);


                CreateXMLObj.Create_AlgorithmPreference(MCSModelInfoFileName,"NN");

                //////////////////////////////////////////////////////////////////////////
                //
                try
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
                        strpath = Path.Combine(tempDir, In_userinfo.FullName);
                        strpath = Path.Combine(strpath, "Models");
                        MCSModelInfoFileName2 = Path.Combine(strpath, ConfigurationManager.AppSettings["MCSModelInfoFileName2"]);
                    }
                    else
                    {
                        LocalResource LocalTempDiv = RoleEnvironment.GetLocalResource(ConfigurationManager.AppSettings["CloudDiv"]);
                        tempDir = Path.Combine(LocalTempDiv.RootPath, ConfigurationManager.AppSettings["TempDir"]);

                        strpath = Path.Combine(tempDir, In_userinfo.FullName);
                        strpath = Path.Combine(strpath, "Models");
                        MCSModelInfoFileName2 = Path.Combine(strpath, ConfigurationManager.AppSettings["MCSModelInfoFileName2"]);
                    }
                }
                catch (Exception ex)
                {
                    String ErrorWord = ex.ToString();
                    
                }
                finally
                {
                
                }






                XmlDocument xdman = new XmlDocument();
                xdman.Load(MCSModelInfoFileName);
                xdman.Save(MCSModelInfoFileName2);


                //////////////////////////////////////////////////////////////////////////
                //編碼UTF-8轉ANSI


                StreamReader sr = new StreamReader(MCSModelInfoFileName2);
                StreamWriter sw = new StreamWriter(MCSModelInfoFileName, false, Encoding.Default); // or UTF-7, etc  

                sw.WriteLine(sr.ReadToEnd());

                sw.Close();
                sr.Close(); 

                //////////////////////////////////////////////////////////////////////////
                //上傳ModelZip檔案到Blob
                string ModelStorageContainerName = "tempmodel";
                string ModelStorageBlobPath = In_userinfo.FullName + "/" + "MCS_MCSModelInfo" + ".xml";

                BlobOperatorClass BlobOperatoClient = new BlobOperatorClass(StorageConnectStringBlob, tempDir, In_userinfo.FullName + ".tmp");
                if (BlobOperatoClient.UploadAsFile(MCSModelInfoFileName, ModelStorageContainerName, ModelStorageBlobPath, false).CompareTo("success") != 0)
                {
                    Thread.Sleep(3000);
                    // Try second time
                    if (BlobOperatoClient.UploadAsFile(MCSModelInfoFileName, ModelStorageContainerName, ModelStorageBlobPath, false).CompareTo("success") != 0)
                    {
                        
                    }
                }


            }
            catch (System.Exception ex)
            {
                JobID = "";
                InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Error", "At Set_GSIModule:" + ex.ToString(), true);
            }
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "Out Set_GSIModule", true);
            GC.Collect();
            return JobID;
        }

        public Out_GSI Get_GSIResult(string JobID, In_UserInfo In_userinfo)
        {
            TableOperatorClass TableOperatorClient = new TableOperatorClass(StorageConnectStringTable);
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "In Get_GSIResult", true);

            Out_GSI out_Result = new Out_GSI();
            try
            {
                GC.Collect();
                /////先於Local端建置該User的個人資料夾以建/////
                creatUserFolder(tempDir, In_userinfo.Company, In_userinfo.User);
                ///////////////////////////////////////////////
                Return_GSIResult(out_Result, TableOperatorClient, JobID, In_userinfo);
            }
            catch (System.Exception ex)
            {
                InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Error", "At Get_GSIResult:" + ex.ToString(), true);
                out_Result = null;
            }
            InsertLogEvent(TableOperatorClient, DebugTableName, CurrentInstanceName, CurrentInstanceID, CurrentInstanceURL, "Info", "Out Get_GSIResult", true);
            return out_Result;
        }

        // Upload Model
        public Out_UploadModel UploadModel(String vMachineID, String CNCType, String CNCNumber, String NCProgramID, DateTime DataStartTime, DateTime DataEndTime, In_UserInfo In_userinfo)
        {
            Out_UploadModel out_UploadModel = new Out_UploadModel();

            String TmpModelFileLocalPath = tempDir + In_userinfo.FullName + "\\";

            ///////////////////////////////////////////////
            // 下載暫存資料
            BlobOperatorClass BlobOperatoClient = new BlobOperatorClass(StorageConnectStringBlob, tempDir, In_userinfo.FullName + ".tmp");
            if (BlobOperatoClient.DownloadAsDirectory(TmpModelFileLocalPath, ModelBlobContainerName, In_userinfo.FullName + "/temporary.file", true, false).CompareTo("success") != 0)
            {
                Thread.Sleep(3000);
                // Try second time
                if (BlobOperatoClient.DownloadAsDirectory(TmpModelFileLocalPath, ModelBlobContainerName, In_userinfo.FullName + "/temporary.file", true, false).CompareTo("success") != 0)
                {
                    out_UploadModel.IsSuccess = false;
                    out_UploadModel.CanDoAgain = true;
                    out_UploadModel.ErrorMessage = "false-Download Temporary file fail.";
                    return out_UploadModel;
                }
            }

            //////////////////////////////////////////////////////////////////////////
            //取得Model的內容列表
            string ModelDirectory = TmpModelFileLocalPath + "Models\\";
            List<string> ModelFileNameList = new List<string>();
            List<string> ModelFileSizeList = new List<string>();
            String ModelFileNameString = "";
            String ModelFileSizeString = "";
            string[] files = Directory.GetFiles(ModelDirectory);
            foreach (string file in files)
            {
                FileInfo FI = new FileInfo(file);
                string ModelFileName = FI.Name;
                string ModelFileExtension = FI.Extension;
                ModelFileSizeList.Add(FI.Length.ToString());//size
                ModelFileNameList.Add(ModelFileName.Substring(0, ModelFileName.Length - ModelFileName.Length)); //name
            }
            ModelFileNameString = String.Join(",", ModelFileNameList.ToArray()); //轉成一連串的字串
            ModelFileSizeString = String.Join(",", ModelFileSizeList.ToArray());

            //////////////////////////////////////////////////////////////////////////
            //產生Model的唯一檔名
            string UnitModelName = Guid.NewGuid().ToString("N");

            //////////////////////////////////////////////////////////////////////////
            //壓縮Model檔案
            string ZipOutputPath = TmpModelFileLocalPath + UnitModelName + ".zip";
            SevenZipCompressor tmp = new SevenZipCompressor();
            tmp.ArchiveFormat = OutArchiveFormat.Zip; // 使用Win zip
            tmp.CompressFiles(ZipOutputPath, files);

            //////////////////////////////////////////////////////////////////////////
            //取得壓縮後的檔案大小
            FileInfo ModelZipFI = new FileInfo(ZipOutputPath);
            string ZipModelFileSize = ModelZipFI.Length.ToString();

            //////////////////////////////////////////////////////////////////////////
            //上傳ModelZip檔案到Blob
            string ModelStorageContainerName = "model";
            string ModelStorageBlobPath = DateTime.Now.ToString("yyyy_MM_dd") + "/" + UnitModelName + ".zip";

            if (BlobOperatoClient.UploadAsFileDirect(ZipOutputPath, ModelStorageContainerName, ModelStorageBlobPath, true).CompareTo("success") != 0)
            {
                Thread.Sleep(3000);
                // Try second time
                if (BlobOperatoClient.UploadAsFileDirect(ZipOutputPath, ModelStorageContainerName, ModelStorageBlobPath, true).CompareTo("success") != 0)
                {
                    out_UploadModel.IsSuccess = false;
                    out_UploadModel.CanDoAgain = true;
                    out_UploadModel.ErrorMessage = "false-Upload Model File fail.";
                    return out_UploadModel;
                }
            }

            //////////////////////////////////////////////////////////////////////////
            //註冊到SQLAzure
            using (SqlConnection con = new SqlConnection(DBConnectionString))
            {
                bool ExecuteState = true;
                string ErrorMessage = "";

                con.Open();
                int CurrentModelID = 0;
                try
                {
                    //取回目前DB上的紀錄數量
                    string SelectSQL = "SELECT CREATETIME FROM STRATEGY3 WHERE ("
                        + " COMPANY = '" + In_userinfo.Company + "'"
                        + " AND CREATEUSER = '" + In_userinfo.User + "'"
                        + " AND VMACHINEID = '" + vMachineID + "'"
                        + " AND CNC_NUMBER = '" + CNCNumber + "'"
                        + " AND CNCTYPE = '" + CNCType + "'"
                        + " AND NCPROGRAMID = '" + NCProgramID + "' )";

                    SqlCommand SC = new SqlCommand(SelectSQL, con);
                    SqlDataReader reader = SC.ExecuteReader();

                    CurrentModelID = 0;
                    // Call Read before accessing data.
                    while (reader.Read())
                    {
                        int i = 0;
                        if (DateTime.UtcNow.Date == Convert.ToDateTime(reader[i]).Date)
                        {
                            CurrentModelID++;
                        } i++;
                    }
                }
                catch (System.Exception ex)
                {
                    ExecuteState = false;
                    ErrorMessage = ex.ToString();
                }
                CurrentModelID++;
                con.Close();

                if (!ExecuteState)
                {
                    out_UploadModel.IsSuccess = false;
                    out_UploadModel.CanDoAgain = true;
                    out_UploadModel.ErrorMessage = "false-GetModelID fail. " + ErrorMessage;
                    return out_UploadModel;
                }
                //////////////////////////////////////////////////////////////////////////
                con.Open();
                try
                {
                    //註冊新紀錄
                    string InsertSQL =
                    "INSERT INTO STRATEGY3("
                    + " PK, COMPANY, CREATEUSER, MODELID, VMACHINEID, CNCTYPE"
                    + ", CNC_NUMBER, NCPROGRAMID, CREATETIME, DATESTARTTIME"
                    + ", DATAENDTIME, MODELNAMELIST, MODELSIZE, MATSIZELIST)"
                    + " VALUES('" + UnitModelName + "', '" + In_userinfo.Company + "', '" + In_userinfo.User + "', '" + CurrentModelID + "', '" + vMachineID + "', '" + CNCType
                    + "', '" + CNCNumber + "', '" + NCProgramID + "', '" + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
                    + "', '" + DataStartTime.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + DataEndTime.ToString("yyyy-MM-dd HH:mm:ss")
                    + "', '" + ModelFileNameString + "','" + ZipModelFileSize + "','" + ModelFileSizeString + "')";
                    SqlCommand comm = new SqlCommand(InsertSQL, con);
                    comm.ExecuteNonQuery();
                }
                catch (System.Exception ex)
                {
                    ExecuteState = false;
                    ErrorMessage = ex.ToString();
                }
                con.Close();
                if (!ExecuteState)
                {
                    out_UploadModel.IsSuccess = false;
                    out_UploadModel.CanDoAgain = true;
                    out_UploadModel.ErrorMessage = "false-Insert New Model Info fail. " + ErrorMessage;
                    return out_UploadModel;
                }
            }

            //////////////////////////////////////////////////////////////////////////
            //移除本機端資料
            if (Directory.Exists(tempDir + In_userinfo.FullName + "\\"))
            {
                foreach (string FilePath in Directory.GetFiles(tempDir + In_userinfo.FullName + "\\"))
                {
                    File.Delete(FilePath);
                }
                Directory.Delete(tempDir + In_userinfo.FullName + "\\", true);
            }

            //////////////////////////////////////////////////////////////////////////
            //移除Blob暫存資料
            if (BlobOperatoClient.DeleteBlob(ModelBlobContainerName, In_userinfo.FullName + "/temporary.file").CompareTo("success") != 0)
            {
                Thread.Sleep(3000);
                // Try second time
                if (BlobOperatoClient.DeleteBlob(ModelBlobContainerName, In_userinfo.FullName + "/temporary.file").CompareTo("success") != 0)
                {
                    out_UploadModel.IsSuccess = false;
                    out_UploadModel.CanDoAgain = false;
                    out_UploadModel.ErrorMessage = "false-Remove Temporary file fail.";
                    return out_UploadModel;
                }
            }

            out_UploadModel.IsSuccess = true;
            out_UploadModel.CanDoAgain = false;
            out_UploadModel.ErrorMessage = "";
            return out_UploadModel;
        }

        private void GetDataForDataTransfer(ref Dictionary<String, Indicator> listAllIndicators,
            ref Dictionary<String, Indicator> listAllPoints, List<MetrologyPoint> processKeyList,
            List<MetrologyPoint> metrologyKeyList, DateTime startTime, DateTime endTime,
            List<String> listIsolatedList, String XTable, String YTable, String strSelectedID, In_UserInfo In_userinfo)
        {
            GC.Collect();
            //參數初始化

            try
            {
                int contextIDCount = 0;

                //Dictionary<String, Indicator> listAllIndicators = new Dictionary<String, Indicator>();    //all indicators & value
                //Dictionary<String, Indicator> listAllPoints = new Dictionary<String, Indicator>();    //all points & value
                List<Context> listContext = new List<Context>();    //all context id

                string startTimeYY = startTime.Year.ToString();
                string startTimeMM = startTime.Month.ToString();
                string startTimeDD = startTime.Day.ToString();
                string endTimeYY = endTime.Year.ToString();
                string endTimeMM = endTime.Month.ToString();
                string endTimeDD = endTime.Day.ToString();

                int indicatorIndexCount = 0;
                int pointIndexCount = 0;
                indicatorIndexCount = processKeyList.Count;
                pointIndexCount = metrologyKeyList.Count; ;

                double[] arrIndicatorNum = new double[indicatorIndexCount];
                double[] arrPointNum = new double[pointIndexCount];

                //string sqlCommand_Metrology = "SELECT * FROM Metrology WHERE TIMETAG BETWEEN '" + startTimeYY + "-" + startTimeMM + "-" + startTimeDD + "' AND '" + endTimeYY + "-" + endTimeMM + "-" + endTimeDD + "' ORDER BY TIMETAG";
                //string sqlCommand_Process = "SELECT * FROM Process WHERE TIMETAG BETWEEN '" + startTimeYY + "-" + startTimeMM + "-" + startTimeDD + "' AND '" + endTimeYY + "-" + endTimeMM + "-" + endTimeDD + "' ORDER BY TIMETAG";

                //string sqlCommand_Metrology = "SELECT * FROM " + YTable + " WHERE TIMETAG BETWEEN '" + startTimeYY + "-" + startTimeMM + "-" + startTimeDD + "' AND '" + endTimeYY + "-" + endTimeMM + "-" + endTimeDD + "' ORDER BY TIMETAG";
                //string sqlCommand_Process = "SELECT * FROM " + XTable + "  WHERE TIMETAG BETWEEN '" + startTimeYY + "-" + startTimeMM + "-" + startTimeDD + "' AND '" + endTimeYY + "-" + endTimeMM + "-" + endTimeDD + "' ORDER BY TIMETAG";

                //////////////////////////////////////////////////////////////////////////
                //String TmpModelFileLocalPath = tempDir + In_userinfo.User + "\\";

                //if (!(Directory.Exists(TmpModelFileLocalPath)))
                //{
                //    Directory.CreateDirectory(TmpModelFileLocalPath);
                //}

                //String strXMLPath = TmpModelFileLocalPath + "DSXML.xml";
                ////////////////////////////////////////////////////////////
                ////下載Result.xml
                ////BlobOperatorClass BOC = new BlobOperatorClass(StorageConnectStringBlob, tempDir, In_userinfo.User + ".xml");
                ////BOC.DownloadAsFile(strXMLPath, ModelBlobContainerName, In_userinfo.User + "/DSXML.zip", true);
                ////////////////////////////////////////////////////////////////////////////
                //XmlDocument doc = new XmlDocument();

                //doc.Load(strXMLPath);

                //XmlNodeList myNodeList = doc.SelectNodes("//AllPiece");


                //String strContextID = "";
                //int iCounter = 0;

                //foreach (XmlNode xn in myNodeList)
                //{
                //    XmlElement xe = (XmlElement)xn;

                //    XmlNodeList mySubNodeList = xn.SelectNodes("RecordPiece");

                //    foreach (XmlNode xns in mySubNodeList)
                //    {

                //        XmlElement xes = (XmlElement)xns;

                //        if (iCounter == 0)
                //        {
                //            strContextID += "'" + xes.GetAttribute("ID") + "'";
                //        }
                //        else
                //        {
                //            strContextID += ",'" + xes.GetAttribute("ID") + "'";
                //        }

                //        iCounter++;
                        
                //    }

                //}


                //////////////////////////////////////////////////////////////////////////
                //string sqlCommand_Metrology = "SELECT * FROM " + YTable + " WHERE TIMETAG BETWEEN '" + startTimeYY + "-" + startTimeMM + "-" + startTimeDD + "' AND '" + endTimeYY + "-" + endTimeMM + "-" + endTimeDD + "' AND WorkPieceID IN (" + strContextID + ") ORDER BY TIMETAG";
                //string sqlCommand_Process = "SELECT * FROM " + XTable + "  WHERE TIMETAG BETWEEN '" + startTimeYY + "-" + startTimeMM + "-" + startTimeDD + "' AND '" + endTimeYY + "-" + endTimeMM + "-" + endTimeDD + "'  AND WorkPieceID IN (" + strContextID + ") ORDER BY TIMETAG";

                string sqlCommand_Metrology = "SELECT * FROM " + YTable + " WHERE TIMETAG BETWEEN '" + startTimeYY + "-" + startTimeMM + "-" + startTimeDD + "' AND '" + endTimeYY + "-" + endTimeMM + "-" + endTimeDD + "' AND WorkPieceID IN (" + strSelectedID + ") ORDER BY TIMETAG";
                string sqlCommand_Process = "SELECT * FROM " + XTable + "  WHERE TIMETAG BETWEEN '" + startTimeYY + "-" + startTimeMM + "-" + startTimeDD + "' AND '" + endTimeYY + "-" + endTimeMM + "-" + endTimeDD + "'  AND WorkPieceID IN (" + strSelectedID + ") ORDER BY TIMETAG";

                SqlConnection con = new SqlConnection(DBConnectionString);
                con.Open();

                SqlDataAdapter dapt_Metrology = new SqlDataAdapter(sqlCommand_Metrology, con);
                SqlDataAdapter dapt_Process = new SqlDataAdapter(sqlCommand_Process, con);

                //new DataSet
                DataSet ds_Metrology = new DataSet();
                DataSet ds_Process = new DataSet();

                //將DataAdapter的資料塞進去DataSet
                dapt_Metrology.Fill(ds_Metrology);
                dapt_Process.Fill(ds_Process);

                con.Close();

                string tempName = "";

                Context contextItem;

                string buffer = "";

                //get all indicators & values
                for (int i = 0; i < ds_Process.Tables[0].Rows.Count; i++)
                {
                    for (int j = 0; j < indicatorIndexCount; j++)
                    {
                        //Indicator's Name
                        //tempName = ds_Process.Tables[0].Rows[i]["ActionNum"].ToString() + "_" + processKeyList[j].Name;
                        tempName = ds_Process.Tables[0].Rows[i]["blockID"].ToString() + "_" + processKeyList[j].Name;

                        //check exist indicator???
                        Indicator indicator = null;
                        if (listAllIndicators.ContainsKey(tempName))
                            indicator = listAllIndicators[tempName];

                        if (indicator == null)
                        {
                            indicator = new Indicator();
                            //indicator.ContextID = ds_Process.Tables[0].Rows[i]["ContextID"].ToString();
                            indicator.ContextID = ds_Process.Tables[0].Rows[i]["WorkPieceID"].ToString();
                            indicator.Number = listAllIndicators.Count + 1;
                            indicator.Name = tempName;

                            listAllIndicators.Add(tempName, indicator); //add new key to dictionary
                        }

                        buffer = ds_Process.Tables[0].Rows[i][processKeyList[j].DataField].ToString();
                        if (buffer == "")
                        {
                            buffer = "0";
                        }
                        indicator.ListItemValue.Add((double)Convert.ToDouble(buffer));
                    }
                }

                contextIDCount = ds_Metrology.Tables[0].Rows.Count;
                if (contextIDCount > listAllIndicators[tempName].ListItemValue.Count)
                {
                    contextIDCount = listAllIndicators[tempName].ListItemValue.Count;
                }

                //all context id
                for (int i = 0; i < contextIDCount; i++)
                {
                    //if (listIsolatedList[i] == "0")
                    {
                        //contextItem = new Context(i + 1, ds_Process.Tables[0].Rows[i]["ContextID"].ToString());
                        contextItem = new Context(i + 1, ds_Process.Tables[0].Rows[i]["WorkPieceID"].ToString());
                        contextItem.ProcessStartTime = ds_Process.Tables[0].Rows[i]["TIMETAG"].ToString();
                        contextItem.ProcessEndTime = ds_Process.Tables[0].Rows[i]["TIMETAG"].ToString();
                        contextItem.MetrologyStartTime = ds_Metrology.Tables[0].Rows[i]["TIMETAG"].ToString();
                        contextItem.MetrologyEndTime = ds_Metrology.Tables[0].Rows[i]["TIMETAG"].ToString();
                        listContext.Add(contextItem);
                    }
                }

                //get all points & values
                for (int i = 0; i < contextIDCount; i++)
                {
                    //if (listIsolatedList[i] == "0")
                    {
                        for (int k = 0; k < pointIndexCount; k++)
                        {
                            //Point's Name
                            tempName = metrologyKeyList[k].Name;

                            //check exist point???
                            Indicator point = null;
                            if (listAllPoints.ContainsKey(tempName))
                                point = listAllPoints[tempName];

                            if (point == null)
                            {
                                point = new Indicator();
                                //point.ContextID = ds_Metrology.Tables[0].Rows[i]["ContextID"].ToString();
                                point.ContextID = ds_Metrology.Tables[0].Rows[i]["WorkPieceID"].ToString();
                                point.Number = listAllPoints.Count + 1;
                                point.Name = tempName;

                                listAllPoints.Add(tempName, point); //add new key to dictionary
                            }

                            buffer = ds_Metrology.Tables[0].Rows[i][metrologyKeyList[k].DataField].ToString();
                            if (buffer == "")
                            {
                                buffer = "0";
                            }
                            point.ListItemValue.Add((double)Convert.ToDouble(buffer));
                        }
                    }//if (listIsolatedList[i] == "0")
                }

                //Set ID for indicators & points
                int id = 1;
                foreach (var pair in listAllIndicators)
                {
                    pair.Value.Number = id;
                    id++;
                }

                //points
                id = 1;
                foreach (var pair in listAllPoints)
                {
                    pair.Value.Number = id;
                    id++;
                }
            }
            catch (Exception e)
            {
                //Ack
                writeLog(e);
            }
        }
        
        private Dictionary<String, Indicator> ConvertToDictionary(String[] key, Indicator[] value)
        {
            Dictionary<String, Indicator> returnValue = new Dictionary<string, Indicator>();
            for (int i = 0; i < key.Length; i++)
            {
                returnValue.Add(key[i], value[i]);
            }
            return returnValue;
        }

        private string Execute_DataTrancsferModule(int contextIDCount, int trainContextIDCount,
            int runContextIDCount,
            List<MetrologyPoint> processKeyList, List<MetrologyPoint> metrologyKeyList,
            DateTime startTime, DateTime endTime,
            List<MetrologyPoint> combinedIndicator, List<MetrologyPoint> combinedPoint,
            List<Group> groupValue, In_UserInfo In_userinfo,
            string vMachineId, string CNCType, string CNCNumber,
            string NCProgram, string model_Id, string version, int[] allAction,
            List<String> listAbnormalList,
            List<String> listIsolatedList,
            String XTable, String YTable, String strSelectedID)
        {
            int iCurrentIsolatedCount = 0;
            int iCurrentIsolatedTrainCount = 0;
            int iCurrentIsolatedRunCount = 0;

            List<String> listIsolatedTrainList = new List<String>();
            List<String> listIsolatedRunList = new List<String>();

            for (int ii = 0; ii < listIsolatedList.Count; ii++)
            {
                if (listIsolatedList[ii] == "1")
                {
                    iCurrentIsolatedCount++;
                }
                if (listIsolatedList[ii] == "1" && ii < trainContextIDCount)
                {
                    iCurrentIsolatedTrainCount++;
                }
                if (listIsolatedList[ii] == "1" && ii >= trainContextIDCount)
                {
                    iCurrentIsolatedRunCount++;
                }
                if (ii < trainContextIDCount)
                {
                    listIsolatedTrainList.Add(listIsolatedList[ii]);
                }
                if (ii >= trainContextIDCount)
                {
                    listIsolatedRunList.Add(listIsolatedList[ii]);
                }
            }

            

            Dictionary<String, Indicator> allInX = new Dictionary<string, Indicator>();//所有的action和x對應共有11*25=275個
            Dictionary<String, Indicator> allInY = new Dictionary<string, Indicator>();//所有的metrolog共有1*20=20個

            GetDataForDataTransfer(ref allInX, ref allInY, processKeyList, metrologyKeyList, startTime, endTime, listIsolatedList, XTable, YTable, strSelectedID, In_userinfo);

            //製程資料的筆數與量測資料的參數個數
            //processKeyList:GUI選取的製程參數序列  ex:1,3,5  第一個製程參數 第三個製程參數 第五個製程參數
            //metrologyKeyList:GUI選取的量測參數序列  ex:1,3,5  第一個量測參數 第三個量測參數 第五個量測參數

            int processKeyCount = combinedIndicator.Count;  // allInX.Count;
            int metrologyKeyCount = combinedPoint.Count;    // allInY.Count;

            /////////////////////////於instance內動態建立資料夾///////////////////
            /////先於Local端建置該User的個人資料夾以建/////
            creatUserFolder(tempDir, In_userinfo.Company, In_userinfo.User);


            ////////////////////////////※宣告參數///////////////////////////////////////
            //宣告DT所會用到的陣列
            double[,] All_X;
            double[,] All_Y;
            double[,] All_Xp;
            double[,] All_Yp;

            double[,] Train_X;
            double[,] Train_Y;
            double[,] Train_Xp;
            double[,] Train_Yp;

            double[,] Run_X;
            double[,] Run_Y;
            double[,] Run_Xp;
            double[,] Run_Yp;

            double[,] GoodRun_X;
            double[,] GoodRun_Y;
            double[,] GoodRun_Xp;
            double[,] GoodRun_Yp;

            double[,] AllContextID;
            double[,] TrainContextID;
            double[,] RunContextID;
            double[,] GoodRunContextID;

            double[,] AllContextIDp;
            double[,] TrainContextIDp;
            double[,] RunContextIDp;
            double[,] GoodRunContextIDp;

            string AllPieceID;
            string TrainPieceID;
            string RunPieceID;
            string GoodRunPieceID;

            double[,] All_IsAbnormal_X;
            double[,] All_IsAbnormal_y;
            double[,] IsAbnormal_X;
            double[,] IsAbnormal_y;

            double[,] All_IsAbnormal_y_And_IsOK_X;
            double[,] IsAbnormal_y_And_IsOK_X;

            double[,] AllIndicatorID;

            double[,] AllPointID;

            double[,] MeasureSpec;

            All_X = new double[processKeyCount, contextIDCount];   //在matlab裡要塞12*95  這裡就要new 12*95
            All_Xp = new double[processKeyCount, contextIDCount - iCurrentIsolatedCount];   //在matlab裡要塞12*95  這裡就要new 12*95

            for (int g = 0; g < processKeyCount; g++)//共勾選25筆
            {
                int iAll_XpIndex = 0;
                //process的column名稱   底下共95筆count
                for (int j = 0; j < allInX[combinedIndicator[g].Name].ListItemValue.Count; j++)
                {
                    if (listIsolatedList[j] == "0")
                    {
                        All_X[g, j] = allInX[combinedIndicator[g].Name].ListItemValue[j];//將column的值一一指定給All_X共95筆
                        All_Xp[g, iAll_XpIndex] = allInX[combinedIndicator[g].Name].ListItemValue[j];
                        iAll_XpIndex++;
                    }
                    else
                    {
                        All_X[g, j] = 0;
                    }
                }
            }

            //All_Y  (metrologyKeyCount*contextIDCount)  ex:(10*95)
            All_Y = new double[metrologyKeyCount, contextIDCount];
            All_Yp = new double[metrologyKeyCount, contextIDCount - iCurrentIsolatedCount];   //在matlab裡要塞12*95  這裡就要new 12*95

            for (int g = 0; g < metrologyKeyCount; g++)//group數目最多20
            {
                int iAll_YpIndex = 0;            //量測值名稱            95筆
                for (int j = 0; j < allInY[combinedPoint[g].Name].ListItemValue.Count; j++)
                {
                    //All_Y[g, j] = allInY[combinedPoint[g].Name].ListItemValue[j];
                    if (listIsolatedList[j] == "0")
                    {
                        All_Y[g, j] = allInY[combinedPoint[g].Name].ListItemValue[j];//寫入group數代表column  ex:1個group則1*95=95 2個則為2*95...
                        All_Yp[g, iAll_YpIndex] = allInY[combinedPoint[g].Name].ListItemValue[j];
                        iAll_YpIndex++;
                    }
                    else
                    {
                        All_Y[g, j] = 0;
                    }
                }
            }


            //塞Train_X  (12*73)
            Train_X = new double[processKeyCount, trainContextIDCount]; //勾選25個process
            Train_Xp = new double[processKeyCount, trainContextIDCount - iCurrentIsolatedTrainCount];

            for (int g = 0; g < processKeyCount; g++)
            {
                int iTrain_XpIndex = 0;
                for (int j = 0; j < trainContextIDCount; j++)
                {
                    if (listIsolatedTrainList[j] == "0")
                    {
                        Train_X[g, j] = allInX[combinedIndicator[g].Name].ListItemValue[j];
                        Train_Xp[g, iTrain_XpIndex] = allInX[combinedIndicator[g].Name].ListItemValue[j];
                        iTrain_XpIndex++;
                    }
                    else
                    {
                        Train_X[g, j] = 0;
                    }
                }
            }

            //塞Train_Y  (10*73)    //group數          訓練筆數
            Train_Y = new double[metrologyKeyCount, trainContextIDCount];
            Train_Yp = new double[metrologyKeyCount, trainContextIDCount - iCurrentIsolatedTrainCount];

            for (int g = 0; g < metrologyKeyCount; g++)//group數目
            {
                int iTrain_YpIndex = 0;
                for (int j = 0; j < trainContextIDCount; j++)
                {
                    if (listIsolatedTrainList[j] == "0")
                    {
                        Train_Y[g, j] = allInY[combinedPoint[g].Name].ListItemValue[j];
                        Train_Yp[g, iTrain_YpIndex] = allInY[combinedPoint[g].Name].ListItemValue[j];
                        iTrain_YpIndex++;
                    }
                    else
                    {
                        Train_Y[g, j] = 0;
                    }
                }
            }

            //Run_X  (12*22) & GoodRun_X  (12*22)
            Run_X = new double[processKeyCount, runContextIDCount];//所選的process數目 與測試筆數
            GoodRun_X = new double[processKeyCount, runContextIDCount];

            Run_Xp = new double[processKeyCount, runContextIDCount - iCurrentIsolatedRunCount];
            GoodRun_Xp = new double[processKeyCount, runContextIDCount - iCurrentIsolatedRunCount];

            for (int g = 0; g < processKeyCount; g++)
            {
                int iRun_XpIndex = 0;
                for (int j = 0; j < runContextIDCount; j++)
                {
                    if (listIsolatedRunList[j] == "0")
                    {
                        Run_X[g, j] = allInX[combinedIndicator[g].Name].ListItemValue[j + trainContextIDCount];//從訓練筆數後開始算
                        GoodRun_X[g, j] = allInX[combinedIndicator[g].Name].ListItemValue[j + trainContextIDCount];

                        Run_Xp[g, iRun_XpIndex] = allInX[combinedIndicator[g].Name].ListItemValue[j + trainContextIDCount];
                        GoodRun_Xp[g, iRun_XpIndex] = allInX[combinedIndicator[g].Name].ListItemValue[j + trainContextIDCount];

                        iRun_XpIndex++;
                    }
                    else
                    {
                        Run_X[g, j] = 0;
                        GoodRun_X[g, j] = 0;
                    }
                }
            }

            //塞Run_Y  (10*22)
            Run_Y = new double[metrologyKeyCount, runContextIDCount];//group數目和測試的metrology數
            GoodRun_Y = new double[metrologyKeyCount, runContextIDCount];

            Run_Yp = new double[metrologyKeyCount, runContextIDCount - iCurrentIsolatedRunCount];
            GoodRun_Yp = new double[metrologyKeyCount, runContextIDCount - iCurrentIsolatedRunCount];

            for (int g = 0; g < metrologyKeyCount; g++)//group數
            {
                int iRun_YpIndex = 0;
                for (int j = 0; j < runContextIDCount; j++)//測試筆數
                {
                    if (listIsolatedRunList[j] == "0")
                    {
                        Run_Y[g, j] = allInY[combinedPoint[g].Name].ListItemValue[j + trainContextIDCount];//從訓練筆數後開始
                        GoodRun_Y[g, j] = allInY[combinedPoint[g].Name].ListItemValue[j + trainContextIDCount];//

                        Run_Yp[g, iRun_YpIndex] = allInY[combinedPoint[g].Name].ListItemValue[j + trainContextIDCount];
                        GoodRun_Yp[g, iRun_YpIndex] = allInY[combinedPoint[g].Name].ListItemValue[j + trainContextIDCount];
                        iRun_YpIndex++;
                    }
                    else
                    {
                        Run_Y[g, j] = 0;
                        GoodRun_Y[g, j] = 0;
                    }
                }
            }

            //塞AllContextID  這裡是從1開始塞  塞到全部筆數的個數
            AllContextID = new double[1, contextIDCount];
            AllContextIDp = new double[1, contextIDCount - iCurrentIsolatedCount];

            int iAllContextIDIndex = 0;
            for (int j = 0; j < contextIDCount; j++)
            {


                if (listIsolatedList[j] == "0")
                {
                    AllContextID[0, j] = j + 1;
                    AllContextIDp[0, iAllContextIDIndex] = j + 1;
                    iAllContextIDIndex++;
                }
                else
                {
                    AllContextID[0, j] = 0;
                }

            }

            //塞TrainContextID  這裡是從1開始塞  塞到training的個數
            TrainContextID = new double[1, trainContextIDCount];
            TrainContextIDp = new double[1, trainContextIDCount - iCurrentIsolatedTrainCount];

            int iTrainContextIDIndex = 0;
            for (int j = 0; j < trainContextIDCount; j++)
            {
                if (listIsolatedTrainList[j] == "0")
                {
                    TrainContextID[0, j] = j + 1;
                    TrainContextIDp[0, iTrainContextIDIndex] = j + 1;
                    iTrainContextIDIndex++;
                }
                else
                {
                    TrainContextID[0, j] = 0;
                }
            }

            //RunContextID & GoodRunContextID
            RunContextID = new double[1, runContextIDCount];
            GoodRunContextID = new double[1, runContextIDCount];

            RunContextIDp = new double[1, runContextIDCount - iCurrentIsolatedRunCount];
            GoodRunContextIDp = new double[1, runContextIDCount - iCurrentIsolatedRunCount];

            int iRunContextIDIndex = 0;
            for (int j = 0; j < runContextIDCount; j++)
            {
                if (listIsolatedRunList[j] == "0")
                {
                    RunContextID[0, j] = j + trainContextIDCount + 1;
                    GoodRunContextID[0, j] = j + trainContextIDCount + 1;

                    RunContextIDp[0, iRunContextIDIndex] = j + trainContextIDCount + 1;
                    GoodRunContextIDp[0, iRunContextIDIndex] = j + trainContextIDCount + 1;
                    iRunContextIDIndex++;
                }
                else
                {
                    RunContextID[0, j] = 0;
                    GoodRunContextID[0, j] = 0;
                }
            }

            //塞AllPieceID  這裡要塞字串  這裡塞AllContextID轉換的字串
            AllPieceID = "";
            for (int j = 0; j < contextIDCount; j++)
            {
                if (listIsolatedList[j] == "0")
                {
                    AllPieceID = AllPieceID + j.ToString() + ",";
                }
                else
                {
                    //AllPieceID = AllPieceID + "0" + ",";
                }
            }
            //if (!AllPieceID.Equals(""))
            //    AllPieceID = AllPieceID.Substring(0, AllPieceID.Length - 1);

            //塞TrainPieceID 這裡要塞字串  這裡塞TrainContextID轉換的字串
            TrainPieceID = "";
            for (int j = 0; j < trainContextIDCount; j++)
            {
                if (listIsolatedTrainList[j] == "0")
                {
                    TrainPieceID = TrainPieceID + j.ToString() + ",";
                }
                else
                {
                    //TrainPieceID = TrainPieceID + "0" + ",";
                }
            }
            //if (!TrainPieceID.Equals(""))
            //    TrainPieceID = TrainPieceID.Substring(0, TrainPieceID.Length - 1);

            //塞RunPieceID 這裡要塞字串  這裡塞RunContextID轉換的字串
            RunPieceID = "";
            for (int j = 0; j < runContextIDCount; j++)
            {
                if (listIsolatedRunList[j] == "0")
                {
                    RunPieceID = RunPieceID + j.ToString() + ",";
                }
                else
                {
                    //RunPieceID = RunPieceID + "0" + ",";
                }
            }
            //if (!RunPieceID.Equals(""))
            //    RunPieceID = RunPieceID.Substring(0, RunPieceID.Length - 1);

            //塞GoodRunPieceID  這裡要塞字串  這裡塞GoodRunContextID轉換的字串
            GoodRunPieceID = "";
            for (int j = 0; j < runContextIDCount; j++)
            {
                if (listIsolatedRunList[j] == "0")
                {
                    GoodRunPieceID = GoodRunPieceID + j.ToString() + ",";
                }
                else
                {
                    //GoodRunPieceID = GoodRunPieceID + "0" + ",";
                }
            }
            //if (!GoodRunPieceID.Equals(""))
            //    GoodRunPieceID = GoodRunPieceID.Substring(0, GoodRunPieceID.Length - 1);

            //塞All_IsAbnormal_X   目前塞0
            if (listAbnormalList.Count != 0)
            {
                All_IsAbnormal_X = new double[1, listAbnormalList.Count];
                for (int i0 = 0; i0 < listAbnormalList.Count; i0++)
                {
                    All_IsAbnormal_X[0, i0] = Double.Parse(listAbnormalList[i0].ToString());
                }
            }
            else
            {
                All_IsAbnormal_X = new double[1, 1];
                All_IsAbnormal_X[0, 0] = 0;
            }

            //塞All_IsAbnormal_y   目前塞0
            if (listAbnormalList.Count != 0)
            {
                All_IsAbnormal_y = new double[1, listAbnormalList.Count];
                for (int i0 = 0; i0 < listAbnormalList.Count; i0++)
                {
                    All_IsAbnormal_y[0, i0] = 0;
                }
            }
            else
            {
                All_IsAbnormal_y = new double[1, 1];
                All_IsAbnormal_y[0, 0] = 0;
            }

            //塞IsAbnormal_X      目前塞0
            //IsAbnormal_X = new double[1, 1];
            //for (int i0 = 0; i0 < 1; i0++)
            //{
            //    IsAbnormal_X[0, i0] = 0;
            //}

            if (listAbnormalList.Count != 0)
            {
                IsAbnormal_X = new double[1, listAbnormalList.Count];
                for (int i0 = 0; i0 < listAbnormalList.Count; i0++)
                {
                    IsAbnormal_X[0, i0] = Double.Parse(listAbnormalList[i0].ToString());
                }
            }
            else
            {
                IsAbnormal_X = new double[1, 1];
                IsAbnormal_X[0, 0] = 0;
            }

            //塞IsAbnormal_y     目前塞0
            IsAbnormal_y = new double[1, 1];
            for (int i0 = 0; i0 < 1; i0++)
            {
                IsAbnormal_y[0, i0] = 0;
            }

            //////////////////////////////////////////////////////////////////////////
            //All_IsAbnormal_X = 0;
            //MWNumericArray mAll_IsAbnormal_X = All_IsAbnormal_X;

            ////塞All_IsAbnormal_y   目前塞0
            //All_IsAbnormal_y = 0;
            //MWNumericArray mAll_IsAbnormal_y = All_IsAbnormal_y;

            ////塞IsAbnormal_X      目前塞0
            //IsAbnormal_X = 0;
            //MWNumericArray mIsAbnormal_X = IsAbnormal_X;

            ////塞IsAbnormal_y     目前塞0
            //IsAbnormal_y = 0;
            //MWNumericArray mIsAbnormal_y = IsAbnormal_y;

            //塞All_IsAbnormal_y_And_IsOK_X   目前塞0
            All_IsAbnormal_y_And_IsOK_X = new double[metrologyKeyCount, contextIDCount];
            for (int p = 0; p < contextIDCount; p++)
            {
                for (int j = 0; j < metrologyKeyCount; j++)
                {
                    All_IsAbnormal_y_And_IsOK_X[j, p] = 0;
                }
            }

            //塞IsAbnormal_y_And_IsOK_X    目前塞0
            IsAbnormal_y_And_IsOK_X = new double[metrologyKeyCount, runContextIDCount];
            for (int p = 0; p < runContextIDCount; p++)
            {
                for (int j = 0; j < metrologyKeyCount; j++)
                {
                    IsAbnormal_y_And_IsOK_X[j, p] = 0;
                }
            }

            //sum of total size of all indicator and point groups
            int GroupIndicatorSetTotalSize = 0;
            int GroupPointSetTotalSize = 0;

            //Generate GroupIndicatorSize and GroupPointSize
            double[,] newGroupIndicatorSize = new double[groupValue.Count, 1];
            double[,] newGroupPointSize = new double[groupValue.Count, 1];

            for (int p = 0; p < groupValue.Count; p++)
            {
                //newGroupIndicatorSize[p, 0] = groupValue[p].IndicatorList.Count;
                foreach (Action action in groupValue[p].ActionList)
                {
                    newGroupIndicatorSize[p, 0] += action.IndicatorList.Count;
                    GroupIndicatorSetTotalSize += action.IndicatorList.Count;
                }

                newGroupPointSize[p, 0] = groupValue[p].PointList.Count;
                GroupPointSetTotalSize += groupValue[p].PointList.Count;
            }



            Dictionary<int, int> indicatorDic = new Dictionary<int, int>(); //key = indicator id, value => point mapping to AllIndicatorID
            Dictionary<int, int> pointDic = new Dictionary<int, int>(); //key = point id, value => point mapping to AllPointID

            //Generate AllIndicatorID
            AllIndicatorID = new double[processKeyCount, 1];
            for (int g = 0; g < processKeyCount; g++)
            {
                AllIndicatorID[g, 0] = allInX[combinedIndicator[g].Name].Number;
                indicatorDic.Add(allInX[combinedIndicator[g].Name].Number, g + 1);
            }


            //Generate AllPointID
            AllPointID = new double[metrologyKeyCount, 1];
            for (int g = 0; g < metrologyKeyCount; g++)
            {
                AllPointID[g, 0] = allInY[combinedPoint[g].Name].Number;
                pointDic.Add(allInY[combinedPoint[g].Name].Number, g + 1);
            }

            //Generate GroupIndicatorSite and GroupPointSite
            double[,] newGroupIndicatorSite = new double[GroupIndicatorSetTotalSize, 1];
            double[,] newGroupPointSite = new double[GroupPointSetTotalSize, 1];
            double[,] newIndicatorSpec = new double[4, GroupIndicatorSetTotalSize];  //注意矩陣 //Indicators

            int k = 0;  //for controling GroupIndicatorSetTotalSize
            int l = 0;  //for controling GroupPointSetTotalSize

            

            for (int p = 0; p < groupValue.Count; p++)
            {
                //GroupIndicatorSite
                for (int j = 0; j < groupValue[p].IndicatorList.Count; j++)
                {
                    //get order of the indicators in all X value
                    newGroupIndicatorSite[k, 0] = indicatorDic[allInX[groupValue[p].IndicatorList[j].Name].Number];
                    k++;

                    
                }

                //GroupPointSite
                for (int m = 0; m < groupValue[p].PointList.Count; m++)
                //for (int m = 0; m < 1; m++)
                {
                    //get order of the indicators in all Y value
                    newGroupPointSite[l, 0] = pointDic[allInY[groupValue[p].PointList[m].Name].Number];
                    l++;
                }
            }

            //////////////////////////////////////////////////////////////////////////
            //假的Indicator SPEC
            //Indicator spec
            //for (int g = 0; g < processKeyCount; g++)
            //{
            //    for (int n = 0; n < 4; n++)
            //    {
            //        //newIndicatorSpec[n, j] = 0;

            //        if (n == 0)//USL
            //        {
            //            newIndicatorSpec[n, g] = 999999;
            //        }
            //        else if (n == 1)//LSL
            //        {
            //            newIndicatorSpec[n, g] = -999999;
            //        }

            //        else if (n == 3)//LSL
            //        {
            //            newIndicatorSpec[n, g] = 999999;
            //        }

            //        else if (n == 4)//LSL
            //        {
            //            newIndicatorSpec[n, g] = -999999;
            //        }
            //    }
            //}

            int ggqq = 0;

            for (int p = 0; p < groupValue.Count; p++)
            {
                //GroupIndicatorSite
                for (int j = 0; j < groupValue[p].IndicatorList.Count; j++)
                {
                    
                    //if (n == 0)//USL
                    {
                        newIndicatorSpec[0, ggqq] = 999999;
                    }
                    //else if (n == 1)//LSL
                    {
                        newIndicatorSpec[1, ggqq] = -999999;
                    }

                    //else if (n == 3)//LSL
                    {
                        newIndicatorSpec[2, ggqq] = 999999;
                    }

                    //else if (n == 4)//LSL
                    {
                        newIndicatorSpec[3, ggqq] = -999999;
                    }
                    ggqq++;
                }

                
            }


            //塞MeasureSpec  目前塞0.015
            MeasureSpec = new double[5, metrologyKeyCount];  //Points
            for (int p = 0; p < metrologyKeyCount; p++)
            {

//                 foreach ( MetrologyPoint MMP in metrologyKeyList )
//                 {
// 
//                 }
                

                for (int j = 0; j < 5; j++)
                {

                    if (j == 0)//USL
                    {
                        MeasureSpec[j, p] = metrologyKeyList[p].USL;   //"Target不能是0"
                    }
                    else if (j == 1)//LSL
                    {
                        MeasureSpec[j, p] = metrologyKeyList[p].LSL;   //"Target不能是0"
                        //MeasureSpec[j, p] = 7.6;   //"Target不能是0"
                    }
                    else if (j == 2)//UCL
                    {
                        MeasureSpec[j, p] = metrologyKeyList[p].UCL;   //"Target不能是0"
                    }
                    else if (j == 3)//LCL
                    {
                        MeasureSpec[j, p] = metrologyKeyList[p].LCL;   //"Target不能是0"
                    }
                    else if (j == 4)//TARGET
                    {
                        MeasureSpec[j, p] = metrologyKeyList[p].TARGET;   //"Target不能是0"
                    }

                    //MeasureSpec[j, p] = 0.015;   //"Target不能是0"
                    //MeasureSpec[j, p] = 0.015;   //"Target不能是0"
                    //MeasureSpec[j, p] = 0.015;   //"Target不能是0"
                    //MeasureSpec[j, p] = 0.015;   //"Target不能是0"
                    //MeasureSpec[j, p] = 0.015;   //"Target不能是0"
                }
            }

            //////////////////////////////////////////////////////////////////////////

            String TmpModelFileLocalPath = tempDir + In_userinfo.FullName + "\\";
            String strXMLPath = TmpModelFileLocalPath + "Parameter.xml";
            SerializableContainerClass SCC = new SerializableContainerClass();

            SCC.dWrite(All_Xp, "All_Xp");
            SCC.dWrite(All_Yp, "All_Yp");

            SCC.dWrite(Train_Xp, "Train_Xp");
            SCC.dWrite(Train_Yp, "Train_Yp");

            SCC.dWrite(Run_Xp, "Run_Xp");
            SCC.dWrite(GoodRun_Xp, "GoodRun_Xp");

            SCC.dWrite(Run_Yp, "Run_Yp");
            SCC.dWrite(GoodRun_Yp, "GoodRun_Yp");

            SCC.dWrite(AllContextIDp, "AllContextIDp");
            SCC.dWrite(TrainContextIDp, "TrainContextIDp");

            SCC.dWrite(RunContextIDp, "RunContextIDp");
            SCC.dWrite(GoodRunContextIDp, "GoodRunContextIDp");

            SCC.sWrite(AllPieceID, "AllPieceID");
            SCC.sWrite(TrainPieceID, "TrainPieceID");
            SCC.sWrite(RunPieceID, "RunPieceID");
            SCC.sWrite(GoodRunPieceID, "GoodRunPieceID");

            SCC.dWrite(All_IsAbnormal_X, "All_IsAbnormal_X");
            SCC.dWrite(All_IsAbnormal_y, "All_IsAbnormal_y");

            SCC.dWrite(IsAbnormal_X, "IsAbnormal_X");
            SCC.dWrite(IsAbnormal_y, "IsAbnormal_y");

            SCC.dWrite(All_IsAbnormal_y_And_IsOK_X, "All_IsAbnormal_y_And_IsOK_X");
            SCC.dWrite(IsAbnormal_y_And_IsOK_X, "IsAbnormal_y_And_IsOK_X");

            for (int i2 = 0; i2 < groupValue.Count; i2++)
            {
                newGroupIndicatorSize[i2, 0] = groupValue[i2].IndicatorList.Count;
                newGroupPointSize[i2, 0] = groupValue[i2].PointList.Count;

                GroupIndicatorSetTotalSize += groupValue[i2].IndicatorList.Count;
                GroupPointSetTotalSize += groupValue[i2].PointList.Count;
            }

            SCC.dWrite(newGroupIndicatorSize, "newGroupIndicatorSize");
            SCC.dWrite(newGroupPointSize, "newGroupPointSize");

            SCC.dWrite(AllIndicatorID, "AllIndicatorID");
            SCC.dWrite(AllPointID, "AllPointID");

            SCC.dWrite(newGroupIndicatorSite, "newGroupIndicatorSite");
            SCC.dWrite(newGroupPointSite, "newGroupPointSite");

            SCC.dWrite(newIndicatorSpec, "newIndicatorSpec");

            SCC.dWrite(MeasureSpec, "MeasureSpec");

            SCC.Save(strXMLPath);

            //////////////////////////////////////////////////////////
            //將Parameter.xml傳到Blob
            BlobOperatorClass BlobOperatoClient = new BlobOperatorClass(StorageConnectStringBlob, tempDir, In_userinfo.FullName + ".tmp");
            BlobOperatoClient.UploadAsFile(strXMLPath, ModelBlobContainerName, In_userinfo.FullName + "/parameter.file", true);


            //////////////////////////////////////////////////////////
            //產生QMC
            QueueMessagesClass QMC = new QueueMessagesClass();
            QMC.Company = In_userinfo.Company;
            QMC.UserName = In_userinfo.User;
            QMC.MethodName = "ExecuteDataTransfer";

            // 製作ModelInfo的字串
            QMC.SetParameter("ModelInfo", createModelInformationXMLFile(vMachineId, CNCType, CNCNumber, NCProgram, model_Id, version, In_userinfo.Company, In_userinfo.User, startTime, endTime, processKeyList, metrologyKeyList, allAction, groupValue));
            QMC.ExecuteState = true;

            //////////////////////////////////////////////////////////
            // 送到Queue中
            QMC.Sent(StorageConnectStringQueue, MCRWorkQueueName);

            return QMC.MessageID;
        }

        private int Return_DataTransferResult(TableOperatorClass TOC, string JobID, In_UserInfo In_userinfo)
        {
            // Set Paths
            String TmpModelFileLocalPath = tempDir + In_userinfo.FullName + "\\";
            String strXMLPath = TmpModelFileLocalPath + "Parameter.xml";

            // Remove Job Entity
            RemoveJobEvent(TOC, JobEventTableName, In_userinfo.Company, JobID);

            //////////////////////////////////////////////////////////
            //下載Result.xml
            BlobOperatorClass BOC = new BlobOperatorClass(StorageConnectStringBlob, tempDir, In_userinfo.FullName + ".tmp");
            BOC.DownloadAsFile(strXMLPath, ModelBlobContainerName, In_userinfo.FullName + "/result.file", true);

            //////////////////////////////////////////////////////////
            // 檢查&轉換Parameter.xml to real parameter
            SerializableContainerClass SCCR = new SerializableContainerClass();
            SCCR.Load(strXMLPath);

            //////////////////////////////////////////////////////////
            //把所有回傳值寫入out_Class的物件裡
            double[,] sResult = SCCR.dRead("DataTransferResult");

            return Convert.ToInt32(sResult[0, 0]);
        }

        private string Execute_MDFRModule(double iEwmaLamda, double iEwmaTolerance, double iEwmaWindow, double iVarConfidence, double ibaseSampleNum, double iRangeMultipleValue, In_UserInfo In_userinfo)
        {
            String TmpModelFileLocalPath = tempDir + In_userinfo.FullName + "\\";
            String strXMLPath = TmpModelFileLocalPath + "Parameter.xml";

            //寫入EwmaLamda資訊
            double[,] EwmaLamda = new double[1, 1];
            EwmaLamda[0, 0] = iEwmaLamda;
            //寫入EwmaTolerance資訊
            double[,] EwmaTolerance = new double[1, 1];
            EwmaTolerance[0, 0] = iEwmaTolerance;
            //寫入EwmaWindow資訊
            double[,] EwmaWindow = new double[1, 1];
            EwmaWindow[0, 0] = iEwmaWindow;
            //寫入VarConfidence資訊
            double[,] VarConfidence = new double[1, 1];
            VarConfidence[0, 0] = iVarConfidence;
            //寫入baseSampleNum資訊
            double[,] baseSampleNum = new double[1, 1];
            baseSampleNum[0, 0] = ibaseSampleNum;
            //寫入RangeMultipleValue資訊
            double[,] RangeMultipleValue = new double[1, 1];
            RangeMultipleValue[0, 0] = iRangeMultipleValue;

            //////////////////////////////////////////////////////////
            // 產生XML轉換類別
            SerializableContainerClass SCC = new SerializableContainerClass();
            SCC.dWrite(EwmaLamda, "EwmaLamda");
            SCC.dWrite(EwmaTolerance, "EwmaTolerance");
            SCC.dWrite(EwmaWindow, "EwmaWindow");
            SCC.dWrite(VarConfidence, "VarConfidence");
            SCC.dWrite(baseSampleNum, "baseSampleNum");
            SCC.dWrite(RangeMultipleValue, "RangeMultipleValue");
            SCC.Save(strXMLPath);//存檔在暫時資料夾\User\Parameter.xml

            //////////////////////////////////////////////////////////
            //將Parameter.xml傳到Blob
            BlobOperatorClass BOC = new BlobOperatorClass(StorageConnectStringBlob, tempDir, In_userinfo.FullName + ".tmp");
            BOC.UploadAsFile(strXMLPath, ModelBlobContainerName, In_userinfo.FullName + "/parameter.file", true);

            //////////////////////////////////////////////////////////
            //產生QMC
            QueueMessagesClass QMC = new QueueMessagesClass();
            QMC.Company = In_userinfo.Company;
            QMC.UserName = In_userinfo.User;
            QMC.MethodName = "ExecuteMDFRModule";
            QMC.ExecuteState = true;

            //////////////////////////////////////////////////////////
            // 送到Queue中
            QMC.Sent(StorageConnectStringQueue, MCRWorkQueueName);
            return QMC.MessageID;
        }

        private void Return_MDFRResult(Out_MDFR out_MDFR, TableOperatorClass TOC, string JobID, In_UserInfo In_userinfo)
        {
            // Set Paths
            String TmpModelFileLocalPath = tempDir + In_userinfo.FullName + "\\";
            String strXMLPath = TmpModelFileLocalPath + "Parameter.xml";

            // Remove Job Entity
            RemoveJobEvent(TOC, JobEventTableName, In_userinfo.Company, JobID);

            //////////////////////////////////////////////////////////
            //下載Result.xml
            BlobOperatorClass BOC = new BlobOperatorClass(StorageConnectStringBlob, tempDir, In_userinfo.FullName + ".tmp");
            BOC.DownloadAsFile(strXMLPath, ModelBlobContainerName, In_userinfo.FullName + "/result.file", true);

            //////////////////////////////////////////////////////////
            // 檢查&轉換Parameter.xml to real parameter
            SerializableContainerClass SCCR = new SerializableContainerClass();
            SCCR.Load(strXMLPath);

            //////////////////////////////////////////////////////////
            //把所有回傳值寫入out_Class的物件裡
            double[,] sResult = SCCR.dRead("SetParameterResult");

            if (sResult[0, 0] == 1)
            {
                out_MDFR.listCOM_Ack = new List<double[]>();
                out_MDFR.listCOM_Ack.Add(new double[1]);
                out_MDFR.listCOM_Ack[0][0] = 1;
            }
            else
            {
                out_MDFR.listCOM_Ack = transferDoubleToList(SCCR.dRead("COM_Ack"));                     // (0)
                if (out_MDFR.listCOM_Ack[0][0] == 0)
                {
                    out_MDFR.listOutYFilterRuleResultbyGroup = transferDoubleToList(SCCR.dRead("OutYFilterRuleResultbyGroup")); // (1)
                    out_MDFR.listOutYFilterRuleGroupIndex = transferDoubleToList(SCCR.dRead("OutYFilterRuleGroupIndex"));    // (2)
                    out_MDFR.listOutYFilterRuleResult = transferDoubleToList(SCCR.dRead("OutYFilterRuleResult"));        // (3)
                    out_MDFR.listOutYabpieceResult = transferDoubleToList(SCCR.dRead("OutYabpieceResult"));           // (4)
                    out_MDFR.listOutSPCSPEC = transferDoubleToList(SCCR.dRead("OutSPCSPEC"));                  // (5)
                    out_MDFR.listOutewmaSPEC = transferDoubleToList(SCCR.dRead("OutewmaSPEC"));                 // (6)
                    out_MDFR.listOutRangeSPEC = transferDoubleToList(SCCR.dRead("OutRangeSPEC"));                // (7)
                    out_MDFR.listOutVarFSPEC = transferDoubleToList(SCCR.dRead("OutVarFSPEC"));                 // (8) 
                }
                else
                    out_MDFR.listCOM_Ack[0][0] = 2;
            }
        }

        private string Execute_DQIyModule_GetDQIyPattern(double icorralpha, double iIsMixedModel, In_UserInfo In_userinfo)
        {
            String TmpModelFileLocalPath = tempDir + In_userinfo.FullName + "\\";
            String strXMLPath = TmpModelFileLocalPath + "Parameter.xml";

            //寫入corralpha資訊
            double[,] corralpha = new double[1, 1];
            corralpha[0, 0] = icorralpha;

            //寫入IsMixedModel資訊
            double[,] IsMixedModel = new double[1, 1];
            IsMixedModel[0, 0] = iIsMixedModel;

            double[,] InOffline = new double[1, 1];  //注意矩陣的緯度
            InOffline[0, 0] = 0;

            //寫入InStage資訊
            double[,] InStage = new double[1, 1];    //注意矩陣的緯度
            InStage[0, 0] = 1;                      //1:GetDQIyPattern  2:VerifyDQIyResult  3:Build DQIyModel

            //////////////////////////////////////////////////////////
            //產生XML轉換類別
            SerializableContainerClass SCC = new SerializableContainerClass();
            SCC.dWrite(corralpha, "corralpha");
            SCC.dWrite(IsMixedModel, "IsMixedModel");
            SCC.dWrite(InOffline, "InOffline");
            SCC.dWrite(InStage, "InStage");
            SCC.Save(strXMLPath);//存檔

            //////////////////////////////////////////////////////////
            //將Parameter.xml傳到Blob
            BlobOperatorClass BOC = new BlobOperatorClass(StorageConnectStringBlob, tempDir, In_userinfo.FullName + ".tmp");
            BOC.UploadAsFile(strXMLPath, ModelBlobContainerName, In_userinfo.FullName + "/parameter.file", true);

            //////////////////////////////////////////////////////////
            //產生QMC
            QueueMessagesClass QMC = new QueueMessagesClass();
            QMC.Company = In_userinfo.Company;
            QMC.UserName = In_userinfo.User;
            QMC.MethodName = "ExecuteDQIyModule_GetDQIyPattern";
            QMC.ExecuteState = true;

            //////////////////////////////////////////////////////////
            // 送到Queue中
            QMC.Sent(StorageConnectStringQueue, MCRWorkQueueName);
            return QMC.MessageID;
        }

        private void Return_DQIyResult_GetDQIyPattern(Out_DQIy_CleanAbnormalY out_DQIy_CleanAbnormalY, TableOperatorClass TOC, string JobID, In_UserInfo In_userinfo)
        {
            // Set Paths
            String TmpModelFileLocalPath = tempDir + In_userinfo.FullName + "\\";
            String strXMLPath = TmpModelFileLocalPath + "Parameter.xml";

            // Remove Job Entity
            RemoveJobEvent(TOC, JobEventTableName, In_userinfo.Company, JobID);

            //////////////////////////////////////////////////////////
            //下載Result.xml
            BlobOperatorClass BOC = new BlobOperatorClass(StorageConnectStringBlob, tempDir, In_userinfo.FullName + ".tmp");
            BOC.DownloadAsFile(strXMLPath, ModelBlobContainerName, In_userinfo.FullName + "/result.file", true);

            //////////////////////////////////////////////////////////
            // 檢查&轉換Parameter.xml to real parameter
            SerializableContainerClass SCCR = new SerializableContainerClass();
            SCCR.Load(strXMLPath);

            //////////////////////////////////////////////////////////
            //把所有回傳值寫入out_Class的物件裡

            double[,] sResult = SCCR.dRead("SetParameterResult");

            if (sResult[0, 0] == 1)
            {
                out_DQIy_CleanAbnormalY.listCOM_Ack = new List<double[]>();
                out_DQIy_CleanAbnormalY.listCOM_Ack.Add(new double[1]);
                out_DQIy_CleanAbnormalY.listCOM_Ack[0][0] = 1;
            }
            else
            {
                out_DQIy_CleanAbnormalY.listCOM_Ack = transferDoubleToList(SCCR.dRead("COM_Ack"));//(0)
                if (out_DQIy_CleanAbnormalY.listCOM_Ack[0][0] == 0)
                {
                    out_DQIy_CleanAbnormalY.listContextID = transferDoubleToList(SCCR.dRead("ContextID"));//(1)
                    out_DQIy_CleanAbnormalY.listPointList = transferDoubleToList(SCCR.dRead("PointList"));//(2)
                    out_DQIy_CleanAbnormalY.listPatternListIndex4PatternID = transferDoubleToList(SCCR.dRead("PatternListIndex4PatternID"));//(3)
                    out_DQIy_CleanAbnormalY.listContextIDOfStepIndex = transferDoubleToList(SCCR.dRead("ContextIDOfStepIndex"));//(4)
                    out_DQIy_CleanAbnormalY.listArtUList = transferDoubleToList(SCCR.dRead("ArtUList"));//(5)
                    out_DQIy_CleanAbnormalY.listIndicatorIDIndex4ContextID = transferDoubleToList(SCCR.dRead("IndicatorIDIndex4ContextID"));//(6)
                    out_DQIy_CleanAbnormalY.listContextIDIndex4PointID = transferDoubleToList(SCCR.dRead("ContextIDIndex4PointID"));//(7)
                    out_DQIy_CleanAbnormalY.listSortPatternListOfPoint = transferDoubleToList(SCCR.dRead("SortPatternListOfPoint"));//(8)
                    TimeSpan TStart = new TimeSpan(Convert.ToInt64(SCCR.sRead("TimeSpandStart")));
                    TimeSpan TEnd = new TimeSpan(Convert.ToInt64(SCCR.sRead("TimeSpandEnd")));

                    //建模筆數、點位數、indicator數
                    int contextIDCount = out_DQIy_CleanAbnormalY.listContextID.Count;
                    int pointCount = out_DQIy_CleanAbnormalY.listPointList.Count;
                    int indicatorCount = out_DQIy_CleanAbnormalY.listArtUList.Count / contextIDCount;
                    int TotalCount = out_DQIy_CleanAbnormalY.listIndicatorIDIndex4ContextID.Count;// 表示有多少個sample*point [12/12/2011 pili7545]
                    int TotalPointCount = out_DQIy_CleanAbnormalY.listPatternListIndex4PatternID.Count;// 表示有多少個point [12/12/2011 pili7545]
                    int TotalIndicatorCount = out_DQIy_CleanAbnormalY.listArtUList.Count / contextIDCount;

                    //測試區
                    ////////////////listContextIDChart//////////////////////////////////////////////
                    List<double[]> listContextIDChart = new List<double[]>();

                    double[] contextIDRaw = new double[TotalCount];

                    for (int j = 0; j < TotalCount; j++)
                    {
                        contextIDRaw[j] = out_DQIy_CleanAbnormalY.listContextIDIndex4PointID[j][0];
                    }
                    listContextIDChart.Add(contextIDRaw);

                    out_DQIy_CleanAbnormalY.ContextIDChart = listContextIDChart;
                    out_DQIy_CleanAbnormalY.timeSpan = TEnd.Subtract(TStart);
                }
                else
                    out_DQIy_CleanAbnormalY.listCOM_Ack[0][0] = 2;
            }
        }

        private string Execute_KSSModule(string iAlgorithmSelection, double iClusterNumber, In_UserInfo In_userinfo)
        {
            String TmpModelFileLocalPath = tempDir + In_userinfo.FullName + "\\";
            String strXMLPath = TmpModelFileLocalPath + "Parameter.xml";

            //寫入InSelectAlgorithm資訊
            string InSelectAlgorithm = iAlgorithmSelection;   //1:'KISS'  or 2:'KMW' 

            //寫入ClusterNumber資訊
            double[,] ClusterNumber = new double[1, 1];
            ClusterNumber[0, 0] = iClusterNumber;

            //////////////////////////////////////////////////////////
            //產生XML轉換類別
            SerializableContainerClass SCC = new SerializableContainerClass();

            SCC.sWrite(InSelectAlgorithm, "InSelectAlgorithm");
            SCC.dWrite(ClusterNumber, "ClusterNumber");
            SCC.Save(strXMLPath);//存檔

            //////////////////////////////////////////////////////////
            //將Parameter.xml傳到Blob
            BlobOperatorClass BOC = new BlobOperatorClass(StorageConnectStringBlob, tempDir, In_userinfo.FullName + ".tmp");
            BOC.UploadAsFile(strXMLPath, ModelBlobContainerName, In_userinfo.FullName + "/parameter.file", true);

            //////////////////////////////////////////////////////////
            //產生QMC
            QueueMessagesClass QMC = new QueueMessagesClass();
            QMC.Company = In_userinfo.Company;
            QMC.UserName = In_userinfo.User;
            QMC.MethodName = "ExecuteKSSModule";
            QMC.ExecuteState = true;

            //////////////////////////////////////////////////////////
            // 送到Queue中
            QMC.Sent(StorageConnectStringQueue, MCRWorkQueueName);
            return QMC.MessageID;
        }

        private void Return_KSSModule(Out_KSS out_KSS, TableOperatorClass TOC, string JobID, In_UserInfo In_userinfo)
        {
            // Set Paths
            String TmpModelFileLocalPath = tempDir + In_userinfo.FullName + "\\";
            String strXMLPath = TmpModelFileLocalPath + "Parameter.xml";

            // Remove Job Entity
            RemoveJobEvent(TOC, JobEventTableName, In_userinfo.Company, JobID);

            //////////////////////////////////////////////////////////
            //下載Result.xml
            BlobOperatorClass BOC = new BlobOperatorClass(StorageConnectStringBlob, tempDir, In_userinfo.FullName + ".tmp");
            BOC.DownloadAsFile(strXMLPath, ModelBlobContainerName, In_userinfo.FullName + "/result.file", true);

            //////////////////////////////////////////////////////////
            // 檢查&轉換Parameter.xml to real parameter
            SerializableContainerClass SCCR = new SerializableContainerClass();
            SCCR.Load(strXMLPath);

            //////////////////////////////////////////////////////////
            //把所有回傳值寫入out_Class的物件裡

            double[,] sResult = SCCR.dRead("SetParameterResult");

            if (sResult[0, 0] == 1)
            {
                out_KSS.listCOM_Ack = new List<double[]>();
                out_KSS.listCOM_Ack.Add(new double[1]);
                out_KSS.listCOM_Ack[0][0] = 1;
            }
            else
            {
                out_KSS.listCOM_Ack = transferDoubleToList(SCCR.dRead("COM_Ack"));//(0)
                if (out_KSS.listCOM_Ack[0][0] == 0)
                {
                    TimeSpan StartSpan = new TimeSpan(Convert.ToInt64(SCCR.sRead("StartTick")));
                    TimeSpan EndSpan = new TimeSpan(Convert.ToInt64(SCCR.sRead("EndTick")));
                    out_KSS.timeSpan = EndSpan.Subtract(StartSpan);
                }
                else
                    out_KSS.listCOM_Ack[0][0] = 2;
            }
        }

        private string Execute_KVSModule(double iFin_apha, double iFout_apha, string iInOneByOneChoose, string iAlgorithmSelection, In_UserInfo In_userinfo)
        {
            String TmpModelFileLocalPath = tempDir + In_userinfo.FullName + "\\";
            String strXMLPath = TmpModelFileLocalPath + "Parameter.xml";

            //塞Fin_apha
            double[,] Fin_apha = new double[1, 1];  //注意矩陣的緯度
            Fin_apha[0, 0] = iFin_apha;        //由GUI設定

            //塞Fout_apha
            double[,] Fout_apha = new double[1, 1];
            Fout_apha[0, 0] = iFout_apha;      //由GUI設定

            //塞InOneByOneChoose
            string InOneByOneChoose = iInOneByOneChoose;

            //塞InSelectAlgorithm
            string InSelectAlgorithm = iAlgorithmSelection;

            //////////////////////////////////////////////////////////
            //產生XML轉換類別
            SerializableContainerClass SCC = new SerializableContainerClass();

            SCC.dWrite(Fin_apha, "Fin_apha");
            SCC.dWrite(Fout_apha, "Fout_apha");
            SCC.sWrite(InOneByOneChoose, "InOneByOneChoose");
            SCC.sWrite(InSelectAlgorithm, "InSelectAlgorithm");
            SCC.Save(strXMLPath);//存檔

            //////////////////////////////////////////////////////////
            //將Parameter.xml傳到Blob
            BlobOperatorClass BOC = new BlobOperatorClass(StorageConnectStringBlob, tempDir, In_userinfo.FullName + ".tmp");
            BOC.UploadAsFile(strXMLPath, ModelBlobContainerName, In_userinfo.FullName + "/parameter.file", true);

            //////////////////////////////////////////////////////////
            //產生QMC
            QueueMessagesClass QMC = new QueueMessagesClass();
            QMC.Company = In_userinfo.Company;
            QMC.UserName = In_userinfo.User;
            QMC.MethodName = "ExecuteKVSModule";
            QMC.ExecuteState = true;

            //////////////////////////////////////////////////////////
            // 送到Queue中
            QMC.Sent(StorageConnectStringQueue, MCRWorkQueueName);
            return QMC.MessageID;
        }

        private void Return_KVSModule(Out_KVS out_KVS, TableOperatorClass TOC, string JobID, In_UserInfo In_userinfo)
        {
            // Set Paths
            String TmpModelFileLocalPath = tempDir + In_userinfo.FullName + "\\";
            String strXMLPath = TmpModelFileLocalPath + "Parameter.xml";

            // Remove Job Entity
            RemoveJobEvent(TOC, JobEventTableName, In_userinfo.Company, JobID);

            //////////////////////////////////////////////////////////
            //下載Result.xml
            BlobOperatorClass BOC = new BlobOperatorClass(StorageConnectStringBlob, tempDir, In_userinfo.FullName + ".tmp");
            BOC.DownloadAsFile(strXMLPath, ModelBlobContainerName, In_userinfo.FullName + "/result.file", true);

            //////////////////////////////////////////////////////////
            // 檢查&轉換Parameter.xml to real parameter
            SerializableContainerClass SCCR = new SerializableContainerClass();
            SCCR.Load(strXMLPath);

            //////////////////////////////////////////////////////////
            //把所有回傳值寫入out_Class的物件裡

            double[,] sResult = SCCR.dRead("SetParameterResult");

            if (sResult[0, 0] == 1)
            {
                out_KVS.listCOM_Ack = new List<double[]>();
                out_KVS.listCOM_Ack.Add(new double[1]);
                out_KVS.listCOM_Ack[0][0] = 1;
            }
            else
            {
                out_KVS.listCOM_Ack = transferDoubleToList(SCCR.dRead("COM_Ack"));
                if (out_KVS.listCOM_Ack[0][0] == 0)
                {
                    out_KVS.listOutAllVariableID = transferDoubleToList(SCCR.dRead("OutAllVariableID")); // (1)
                    out_KVS.listOutAllVariableCutNum = transferDoubleToList(SCCR.dRead("OutAllVariableCutNum")); // (2)
                    out_KVS.listOutAlgorithmVariableID = transferDoubleToList(SCCR.dRead("OutAlgorithmVariableID"));// (3)
                    out_KVS.listOutAlgorithmVariableCutNum = transferDoubleToList(SCCR.dRead("OutAlgorithmVariableCutNum"));// (4)
                    out_KVS.listOutKeyVariableID = transferDoubleToList(SCCR.dRead("OutKeyVariableID"));// (5)
                    out_KVS.listOutKeyVariableCutNum = transferDoubleToList(SCCR.dRead("OutKeyVariableCutNum"));// (6)
                    out_KVS.listOutKeyVariableBetaValue = transferDoubleToList(SCCR.dRead("OutKeyVariableBetaValue"));// (5)
                    out_KVS.listOutKeyVariablePValue = transferDoubleToList(SCCR.dRead("OutKeyVariablePValue"));// (6)  
                }
                else
                    out_KVS.listCOM_Ack[0][0] = 2;
            }
        }

        private string Execute_DQIxModule_VerifyDQIx(double iInLambda, double iInConstant, double iDQIxFilterPercentage, double iDQIxRefreshCounter, In_UserInfo In_userinfo)
        {
            String TmpModelFileLocalPath = tempDir + In_userinfo.FullName + "\\";
            String strXMLPath = TmpModelFileLocalPath + "Parameter.xml";

            double[,] InLambda = new double[1, 1];  //注意矩陣的緯度
            InLambda[0, 0] = iInLambda;        //0.7 < InLambda < 1.2  Default:1

            //塞InConstant
            double[,] InConstant = new double[1, 1];
            InConstant[0, 0] = iInConstant;      //1.5 < InConstant < 5   Default:2

            //塞DQIxFilterPercentage
            double[,] DQIxFilterPercentage = new double[1, 1];
            DQIxFilterPercentage[0, 0] = iDQIxFilterPercentage;

            //塞DQIxRefreshCounter
            double[,] DQIxRefreshCounter = new double[1, 1];
            DQIxRefreshCounter[0, 0] = iDQIxRefreshCounter;

            //塞InOffline
            double[,] InOffline = new double[1, 1];
            InOffline[0, 0] = 0;

            //塞InStage
            double[,] InStage = new double[1, 1];
            InStage[0, 0] = 1;                          //  1  VerifyDQIx 2.ReTrain All Data

            //////////////////////////////////////////////////////////
            //產生XML轉換類別
            SerializableContainerClass SCC = new SerializableContainerClass();

            SCC.dWrite(InLambda, "InLambda");
            SCC.dWrite(InConstant, "InConstant");
            SCC.dWrite(DQIxFilterPercentage, "DQIxFilterPercentage");
            SCC.dWrite(DQIxRefreshCounter, "DQIxRefreshCounter");
            SCC.dWrite(InOffline, "InOffline");
            SCC.dWrite(InStage, "InStage");
            SCC.Save(strXMLPath);//存檔

            //////////////////////////////////////////////////////////
            //將Parameter.xml傳到Blob
            BlobOperatorClass BOC = new BlobOperatorClass(StorageConnectStringBlob, tempDir, In_userinfo.FullName + ".tmp");
            BOC.UploadAsFile(strXMLPath, ModelBlobContainerName, In_userinfo.FullName + "/parameter.file", true);

            //////////////////////////////////////////////////////////
            //產生QMC
            QueueMessagesClass QMC = new QueueMessagesClass();
            QMC.Company = In_userinfo.Company;
            QMC.UserName = In_userinfo.User;
            QMC.MethodName = "ExecuteDQIxModule_VerifyDQIx";
            QMC.ExecuteState = true;

            //////////////////////////////////////////////////////////
            // 送到Queue中
            QMC.Sent(StorageConnectStringQueue, MCRWorkQueueName);
            return QMC.MessageID;
        }

        private void Return_DQIxResult_VerifyDQIx(Out_VerifyDQIxResult out_VerifyDQIxResult, TableOperatorClass TOC, string JobID, In_UserInfo In_userinfo)
        {
            // Set Paths
            String TmpModelFileLocalPath = tempDir + In_userinfo.FullName + "\\";
            String strXMLPath = TmpModelFileLocalPath + "Parameter.xml";

            // Remove Job Entity
            RemoveJobEvent(TOC, JobEventTableName, In_userinfo.Company, JobID);

            //////////////////////////////////////////////////////////
            //下載Result.xml
            BlobOperatorClass BOC = new BlobOperatorClass(StorageConnectStringBlob, tempDir, In_userinfo.FullName + ".tmp");
            BOC.DownloadAsFile(strXMLPath, ModelBlobContainerName, In_userinfo.FullName + "/result.file", true);

            //////////////////////////////////////////////////////////
            // 檢查&轉換Parameter.xml to real parameter
            SerializableContainerClass SCCR = new SerializableContainerClass();
            SCCR.Load(strXMLPath);

            //////////////////////////////////////////////////////////
            //把所有回傳值寫入out_Class的物件裡

            double[,] sResult = SCCR.dRead("SetParameterResult");

            if (sResult[0, 0] == 1)
            {
                out_VerifyDQIxResult.listCOM_Ack = new List<double[]>();
                out_VerifyDQIxResult.listCOM_Ack.Add(new double[1]);
                out_VerifyDQIxResult.listCOM_Ack[0][0] = 1;
            }
            else
            {
                out_VerifyDQIxResult.listCOM_Ack = transferDoubleToList(SCCR.dRead("COM_Ack"));
                if (out_VerifyDQIxResult.listCOM_Ack[0][0] == 0)
                {
                    out_VerifyDQIxResult.listContextID = transferDoubleToList(SCCR.dRead("ContextID"));
                    out_VerifyDQIxResult.listDQIxFlag = transferDoubleToList(SCCR.dRead("DQIxFlag"));
                    out_VerifyDQIxResult.listDQIx = transferDoubleToList(SCCR.dRead("DQIx"));
                    out_VerifyDQIxResult.listDQIxThreshold = transferDoubleToList(SCCR.dRead("DQIxThreshold"));
                    out_VerifyDQIxResult.listTypeIDt = transferDoubleToList(SCCR.dRead("TypeID"));
                    out_VerifyDQIxResult.listStepTypeID = transferDoubleToList(SCCR.dRead("StepTypeID"));
                    out_VerifyDQIxResult.listAccuracyResult = transferDoubleToList(SCCR.dRead("AccuracyResult"));
                    out_VerifyDQIxResult.listSizeInfo = transferDoubleToList(SCCR.dRead("SizeInfo"));
                }
                else
                    out_VerifyDQIxResult.listCOM_Ack[0][0] = 2;
            }
        }

        private string Execute_DQIyModule_VerifyDQIy(double icorralpha, double iIsMixedModel, In_UserInfo In_userinfo)
        {
            String TmpModelFileLocalPath = tempDir + In_userinfo.FullName + "\\";
            String strXMLPath = TmpModelFileLocalPath + "Parameter.xml";

            //寫入corralpha資訊
            double[,] corralpha = new double[1, 1];
            corralpha[0, 0] = icorralpha;

            //寫入IsMixedModel資訊
            double[,] IsMixedModel = new double[1, 1];
            IsMixedModel[0, 0] = iIsMixedModel;

            //寫入InOffline資訊
            double[,] InOffline = new double[1, 1];
            InOffline[0, 0] = 0;

            //寫入InStage資訊
            double[,] InStage = new double[1, 1];
            InStage[0, 0] = 2;                          //1:GetDQIyPattern  2:VerifyDQIyResult  3:Build DQIyModel

            //////////////////////////////////////////////////////////
            //產生XML轉換類別
            SerializableContainerClass SCC = new SerializableContainerClass();
            SCC.dWrite(corralpha, "corralpha");
            SCC.dWrite(IsMixedModel, "IsMixedModel");
            SCC.dWrite(InOffline, "InOffline");
            SCC.dWrite(InStage, "InStage");
            SCC.Save(strXMLPath);//存檔

            //////////////////////////////////////////////////////////
            //將Parameter.xml傳到Blob
            BlobOperatorClass BOC = new BlobOperatorClass(StorageConnectStringBlob, tempDir, In_userinfo.FullName + ".tmp");
            BOC.UploadAsFile(strXMLPath, ModelBlobContainerName, In_userinfo.FullName + "/parameter.file", true);

            //////////////////////////////////////////////////////////
            //產生QMC
            QueueMessagesClass QMC = new QueueMessagesClass();
            QMC.Company = In_userinfo.Company;
            QMC.UserName = In_userinfo.User;
            QMC.MethodName = "ExecuteDQIyModule_VerifyDQIy";
            QMC.ExecuteState = true;

            //////////////////////////////////////////////////////////
            // 送到Queue中
            QMC.Sent(StorageConnectStringQueue, MCRWorkQueueName);
            return QMC.MessageID;
        }

        private void Return_DQIyResult_VerifyDQIy(Out_VerifyDQIyResult out_VerifyDQIyResult, TableOperatorClass TOC, string JobID, In_UserInfo In_userinfo)
        {
            // Set Paths
            String TmpModelFileLocalPath = tempDir + In_userinfo.FullName + "\\";
            String strXMLPath = TmpModelFileLocalPath + "Parameter.xml";

            // Remove Job Entity
            RemoveJobEvent(TOC, JobEventTableName, In_userinfo.Company, JobID);

            //////////////////////////////////////////////////////////
            //下載Result.xml
            BlobOperatorClass BOC = new BlobOperatorClass(StorageConnectStringBlob, tempDir, In_userinfo.FullName + ".tmp");
            BOC.DownloadAsFile(strXMLPath, ModelBlobContainerName, In_userinfo.FullName + "/result.file", true);

            //////////////////////////////////////////////////////////
            // 檢查&轉換Parameter.xml to real parameter
            SerializableContainerClass SCCR = new SerializableContainerClass();
            SCCR.Load(strXMLPath);

            //////////////////////////////////////////////////////////
            //把所有回傳值寫入out_Class的物件裡

            double[,] sResult = SCCR.dRead("SetParameterResult");

            if (sResult[0, 0] == 1)
            {
                out_VerifyDQIyResult.listCOM_Ack = new List<double[]>();
                out_VerifyDQIyResult.listCOM_Ack.Add(new double[1]);
                out_VerifyDQIyResult.listCOM_Ack[0][0] = 1;
            }
            else
            {
                out_VerifyDQIyResult.listCOM_Ack = transferDoubleToList(SCCR.dRead("COM_Ack"));
                if (out_VerifyDQIyResult.listCOM_Ack[0][0] == 0)
                {
                    out_VerifyDQIyResult.listContextID = transferDoubleToList(SCCR.dRead("ContextID")); //(1)
                    out_VerifyDQIyResult.listPointList = transferDoubleToList(SCCR.dRead("PointList")); //(2)
                    out_VerifyDQIyResult.listPatternListIndex4PatternID = transferDoubleToList(SCCR.dRead("PatternListIndex4PatternID")); //(3)
                    out_VerifyDQIyResult.listContextIDOfStepIndex = transferDoubleToList(SCCR.dRead("ContextIDOfStepIndex"));//(4)
                    out_VerifyDQIyResult.listArtUList = transferDoubleToList(SCCR.dRead("ArtUList"));//(5)
                    out_VerifyDQIyResult.listIndicatorIDIndex4ContextID = transferDoubleToList(SCCR.dRead("IndicatorIDIndex4ContextID"));//(6)
                    out_VerifyDQIyResult.listContextIDIndex4PointID = transferDoubleToList(SCCR.dRead("ContextIDIndex4PointID"));//(7)

                    out_VerifyDQIyResult.listDQIyResult = transferDoubleToList(SCCR.dRead("DQIyResult"));//(8)
                    out_VerifyDQIyResult.listDQIyResultIndex = transferDoubleToList(SCCR.dRead("DQIyResultIndex"));//(9)
                    out_VerifyDQIyResult.listAccuracyResult = transferDoubleToList(SCCR.dRead("AccuracyResult"));//(10)
                    out_VerifyDQIyResult.listRunArtUList = transferDoubleToList(SCCR.dRead("RunArtUList"));//(11)
                    out_VerifyDQIyResult.listRunDQIyData = transferDoubleToList(SCCR.dRead("RunDQIyData"));//(12)
                    out_VerifyDQIyResult.listRunContextInfo = transferDoubleToList(SCCR.dRead("RunContextInfo"));//(13)
                    out_VerifyDQIyResult.listIndicatorIDIndex4RunContextID = transferDoubleToList(SCCR.dRead("IndicatorIDIndex4RunContextID"));//(14)
                    out_VerifyDQIyResult.listRunContextIDIndex4PointID = transferDoubleToList(SCCR.dRead("RunContextIDIndex4PointID"));//(15)
                    out_VerifyDQIyResult.listSizeInfo = transferDoubleToList(SCCR.dRead("SizeInfo"));//(16)


                    out_VerifyDQIyResult.listDQIyResultChart = transferDoubleToList(SCCR.dRead("DQIyResultChart"));
                    TimeSpan StartSpan = new TimeSpan(Convert.ToInt64(SCCR.sRead("StartTick")));
                    TimeSpan EndSpan = new TimeSpan(Convert.ToInt64(SCCR.sRead("EndTick")));
                    out_VerifyDQIyResult.timeSpan = EndSpan.Subtract(StartSpan);
                }
                else
                    out_VerifyDQIyResult.listCOM_Ack[0][0] = 2;
            }
        }

        private string Execute_BPNNModule(double iMomTermRange_Min, double iMomTermRange_Int, double iMomTermRange_Max, double iAlphaRange_Min, double iAlphaRange_Int, double iAlphaRange_Max, double iEpochsRange_1, double iEpochsRange_2, double iEpochsRange_3, List<double> iNodesRange, string iInOneByOneChoose, double iBPNNRefreshCounter, In_UserInfo In_userinfo)
        {
            String TmpModelFileLocalPath = tempDir + In_userinfo.FullName + "\\";
            String strXMLPath = TmpModelFileLocalPath + "Parameter.xml";

            //寫入MomTermRange資訊
            double[,] MomTermRange = new double[3, 1];
            MomTermRange[0, 0] = iMomTermRange_Min;//0.5
            MomTermRange[1, 0] = iMomTermRange_Int;//0.2
            MomTermRange[2, 0] = iMomTermRange_Max;//0.9

            //寫入AlphaRange資訊
            double[,] AlphaRange = new double[3, 1];
            AlphaRange[0, 0] = iAlphaRange_Min;//0.15
            AlphaRange[1, 0] = iAlphaRange_Int;//0.10
            AlphaRange[2, 0] = iAlphaRange_Max;//0.15

            //寫入EpochsRange資訊
            double[,] EpochsRange = new double[3, 1];
            EpochsRange[0, 0] = iEpochsRange_1;//60
            EpochsRange[1, 0] = iEpochsRange_2;//80
            EpochsRange[2, 0] = iEpochsRange_3;//100

            //寫入NodesRange資訊
            int NodesRangeCount = iNodesRange.Count;
            double[,] NodesRange = new double[NodesRangeCount, 1];
            for (int i = 0; i < NodesRangeCount; i++)
            {
                NodesRange[i, 0] = iNodesRange[i];
            }

            //寫入OneByOneChoose資訊
            string InOneByOneChoose = iInOneByOneChoose;

            //寫入BPNNRefreshCounter資訊
            double[,] BPNNRefreshCounter = new double[1, 1];  //注意矩陣的緯度
            BPNNRefreshCounter[0, 0] = iBPNNRefreshCounter;

            //////////////////////////////////////////////////////////
            //產生XML轉換類別
            SerializableContainerClass SCC = new SerializableContainerClass();
            SCC.dWrite(MomTermRange, "MomTermRange");
            SCC.dWrite(AlphaRange, "AlphaRange");
            SCC.dWrite(EpochsRange, "EpochsRange");
            SCC.dWrite(NodesRange, "NodesRange");
            SCC.sWrite(InOneByOneChoose, "InOneByOneChoose");
            SCC.dWrite(BPNNRefreshCounter, "BPNNRefreshCounter");
            SCC.Save(strXMLPath);//存檔

            //////////////////////////////////////////////////////////
            //將Parameter.xml傳到Blob
            BlobOperatorClass BOC = new BlobOperatorClass(StorageConnectStringBlob, tempDir, In_userinfo.FullName + ".tmp");
            BOC.UploadAsFile(strXMLPath, ModelBlobContainerName, In_userinfo.FullName + "/parameter.file", true);

            //////////////////////////////////////////////////////////
            //產生QMC
            QueueMessagesClass QMC = new QueueMessagesClass();
            QMC.Company = In_userinfo.Company;
            QMC.UserName = In_userinfo.User;
            QMC.MethodName = "ExecuteBPNNModule";
            QMC.ExecuteState = true;

            //////////////////////////////////////////////////////////
            // 送到Queue中
            QMC.Sent(StorageConnectStringQueue, MCRWorkQueueName);
            return QMC.MessageID;
        }

        private void Return_BPNNResult(Out_BPNN out_BPNN, TableOperatorClass TOC, string JobID, In_UserInfo In_userinfo)
        {
            // Set Paths
            String TmpModelFileLocalPath = tempDir + In_userinfo.FullName + "\\";
            String strXMLPath = TmpModelFileLocalPath + "Parameter.xml";

            // Remove Job Entity
            RemoveJobEvent(TOC, JobEventTableName, In_userinfo.Company, JobID);

            //////////////////////////////////////////////////////////
            //下載Result.xml
            BlobOperatorClass BOC = new BlobOperatorClass(StorageConnectStringBlob, tempDir, In_userinfo.FullName + ".tmp");
            BOC.DownloadAsFile(strXMLPath, ModelBlobContainerName, In_userinfo.FullName + "/result.file", true);

            //////////////////////////////////////////////////////////
            // 檢查&轉換Parameter.xml to real parameter
            SerializableContainerClass SCCR = new SerializableContainerClass();
            SCCR.Load(strXMLPath);

            //////////////////////////////////////////////////////////
            //把所有回傳值寫入out_Class的物件裡

            double[,] sResult = SCCR.dRead("SetParameterResult");

            if (sResult[0, 0] == 1)
            {
                out_BPNN.listCOM_Ack = new List<double[]>();
                out_BPNN.listCOM_Ack.Add(new double[1]);
                out_BPNN.listCOM_Ack[0][0] = 1;
            }
            else
            {
                out_BPNN.listCOM_Ack = transferDoubleToList(SCCR.dRead("NNCOM_Ack"));
                if (out_BPNN.listCOM_Ack[0][0] == 0)
                {
                    out_BPNN.listAll_ConjectureID = transferDoubleToList(SCCR.dRead("NNAll_ConjectureID"));  // (1)
                    out_BPNN.listOutAll_ContextID = transferDoubleToList(SCCR.dRead("NNOutAll_ContextID"));  // (2)
                    out_BPNN.listOutAll_PhaseID = transferDoubleToList(SCCR.dRead("NNOutAll_PhaseID"));    // (3)
                    out_BPNN.listOutAll_PointID = transferDoubleToList(SCCR.dRead("NNOutAll_PointID"));    // (4)
                    out_BPNN.listOutAll_PredictValue = transferDoubleToList(SCCR.dRead("NNOutAll_PredictValue"));   // (5)
                    out_BPNN.listOutAll_ErrorValue = transferDoubleToList(SCCR.dRead("NNOutAll_ErrorValue")); // (6)
                    out_BPNN.listOutAll_MaxError = transferDoubleToList(SCCR.dRead("NNOutAll_MaxError"));   // (7)
                    out_BPNN.listOutAll_MAPE = transferDoubleToList(SCCR.dRead("NNOutAll_MAPE"));       // (8)
                    out_BPNN.listOutAll_RT = transferDoubleToList(SCCR.dRead("NNOutAll_RT"));         // (9)
                    out_BPNN.listOutAll_ConjectureTime = transferDoubleToList(SCCR.dRead("NNOutAll_ConjectureTime")); // (10)

                    if (SCCR.sRead("AlgoValue").CompareTo("0") == 0)
                        out_BPNN.AlgoValue = false;
                    else
                        out_BPNN.AlgoValue = true;
                }
                else
                    out_BPNN.listCOM_Ack[0][0] = 2;

                out_BPNN.listY = new List<double[]>();
            }
        }

        private string Execute_MRModule(string iInSelectAlgorithm, double iInMR_TSVD_Condition_Number_Criteria, double iInMR_TSVD_Energy_Ratio_Criteria, double iMRRefreshCounter, In_UserInfo In_userinfo)
        {
            String TmpModelFileLocalPath = tempDir + In_userinfo.FullName + "\\";
            String strXMLPath = TmpModelFileLocalPath + "Parameter.xml";

            //寫入MR_TSVD_Condition_Number_Criteria資訊
            double[,] MR_TSVD_Condition_Number_Criteria = new double[1, 1];
            MR_TSVD_Condition_Number_Criteria[0, 0] = iInMR_TSVD_Condition_Number_Criteria;

            //寫入MR_TSVD_Energy_Ratio_Criteria資訊
            double[,] MR_TSVD_Energy_Ratio_Criteria = new double[1, 1];
            MR_TSVD_Energy_Ratio_Criteria[0, 0] = iInMR_TSVD_Energy_Ratio_Criteria;

            //寫入MRRefreshCounter資訊
            double[,] MRRefreshCounter = new double[1, 1];
            MRRefreshCounter[0, 0] = iMRRefreshCounter;

            //寫入OneByOneChoose資訊
            string InSelectAlgorithm = iInSelectAlgorithm;

            //////////////////////////////////////////////////////////
            //產生XML轉換類別
            SerializableContainerClass SCC = new SerializableContainerClass();
            SCC.dWrite(MR_TSVD_Condition_Number_Criteria, "MR_TSVD_Condition_Number_Criteria");
            SCC.dWrite(MR_TSVD_Energy_Ratio_Criteria, "MR_TSVD_Energy_Ratio_Criteria");
            SCC.dWrite(MRRefreshCounter, "MRRefreshCounter");
            SCC.sWrite(InSelectAlgorithm, "InSelectAlgorithm");
            SCC.Save(strXMLPath);//存檔

            //////////////////////////////////////////////////////////
            //將Parameter.xml傳到Blob
            BlobOperatorClass BOC = new BlobOperatorClass(StorageConnectStringBlob, tempDir, In_userinfo.FullName + ".tmp");
            BOC.UploadAsFile(strXMLPath, ModelBlobContainerName, In_userinfo.FullName + "/parameter.file", true);

            //////////////////////////////////////////////////////////
            //產生QMC
            QueueMessagesClass QMC = new QueueMessagesClass();
            QMC.Company = In_userinfo.Company;
            QMC.UserName = In_userinfo.User;
            QMC.MethodName = "ExecuteMRModule";
            QMC.ExecuteState = true;

            //////////////////////////////////////////////////////////
            // 送到Queue中
            QMC.Sent(StorageConnectStringQueue, MCRWorkQueueName);
            return QMC.MessageID;
        }

        private void Return_MRResult(Out_MR out_MR, TableOperatorClass TOC, string JobID, In_UserInfo In_userinfo)
        {
            // Set Paths
            String TmpModelFileLocalPath = tempDir + In_userinfo.FullName + "\\";
            String strXMLPath = TmpModelFileLocalPath + "Parameter.xml";

            // Remove Job Entity
            RemoveJobEvent(TOC, JobEventTableName, In_userinfo.Company, JobID);

            //////////////////////////////////////////////////////////
            //下載Result.xml
            BlobOperatorClass BOC = new BlobOperatorClass(StorageConnectStringBlob, tempDir, In_userinfo.FullName + ".tmp");
            BOC.DownloadAsFile(strXMLPath, ModelBlobContainerName, In_userinfo.FullName + "/result.file", true);

            //////////////////////////////////////////////////////////
            // 檢查&轉換Parameter.xml to real parameter
            SerializableContainerClass SCCR = new SerializableContainerClass();
            SCCR.Load(strXMLPath);

            //////////////////////////////////////////////////////////
            //把所有回傳值寫入out_Class的物件裡

            double[,] sResult = SCCR.dRead("SetParameterResult");

            if (sResult[0, 0] == 1)
            {
                out_MR.listCOM_Ack = new List<double[]>();
                out_MR.listCOM_Ack.Add(new double[1]);
                out_MR.listCOM_Ack[0][0] = 1;
            }
            else
            {
                out_MR.listCOM_Ack = transferDoubleToList(SCCR.dRead("MRCOM_Ack"));
                if (out_MR.listCOM_Ack[0][0] == 0)
                {
                    out_MR.listAll_ConjectureID = transferDoubleToList(SCCR.dRead("MRAll_ConjectureID")); // (1)
                    out_MR.listOutAll_ContextID = transferDoubleToList(SCCR.dRead("MROutAll_ContextID")); // (2)
                    out_MR.listOutAll_PhaseID = transferDoubleToList(SCCR.dRead("MROutAll_PhaseID"));// (3)
                    out_MR.listOutAll_PointID = transferDoubleToList(SCCR.dRead("MROutAll_PointID"));// (4)
                    out_MR.listOutAll_PredictValue = transferDoubleToList(SCCR.dRead("MROutAll_PredictValue"));// (5)
                    out_MR.listOutAll_ErrorValue = transferDoubleToList(SCCR.dRead("MROutAll_ErrorValue"));// (6)
                    out_MR.listOutAll_MaxError = transferDoubleToList(SCCR.dRead("MROutAll_MaxError"));// (7)
                    out_MR.listOutAll_MAPE = transferDoubleToList(SCCR.dRead("MROutAll_MAPE"));// (8)
                    out_MR.listOutAll_RT = transferDoubleToList(SCCR.dRead("MROutAll_RT"));// (9)
                    out_MR.listOutAll_ConjectureTime = transferDoubleToList(SCCR.dRead("MROutAll_ConjectureTime"));// (10)

                    if (SCCR.sRead("AlgoValue").CompareTo("0") == 0)
                        out_MR.AlgoValue = false;
                    else
                        out_MR.AlgoValue = true;
                }
                else
                    out_MR.listCOM_Ack[0][0] = 2;

                out_MR.listY = new List<double[]>();
            }
        }

        private string Execute_RIModule(bool FirstAlgoValue, string FirstAlgoName, List<double[]> FitstPredictValue, bool SecondAlgoValue, string SecondAlgoName, List<double[]> SecondPredictValue, double iTolerant_MaxError, string iInSelectCalculator, In_UserInfo In_userinfo)
        {
            String TmpModelFileLocalPath = tempDir + In_userinfo.FullName + "\\";
            String strXMLPath = TmpModelFileLocalPath + "Parameter.xml";

            //寫入FirstAlgoValue資訊
            string InFirstAlgoValue = FirstAlgoValue ? "1" : "0";

            //寫入SecondAlgoValue資訊
            string InSecondAlgoValue = SecondAlgoValue ? "1" : "0";

            //寫入InFirstAlgoName資訊
            string InFirstAlgoName = FirstAlgoName;

            //寫入InSecondAlgoName資訊
            string InSecondAlgoName = SecondAlgoName;

            //寫入dFitstPredictValue資訊
            double[,] dFitstPredictValue = transferListToDouble(FitstPredictValue);

            //寫入dSecondPredictValue資訊
            double[,] dSecondPredictValue = transferListToDouble(SecondPredictValue);

            //寫入InSelectCalculator資訊
            string InSelectCalculator = iInSelectCalculator;

            //寫入Tolerant_MaxError資訊
            double[,] Tolerant_MaxError = new double[1, 1];
            Tolerant_MaxError[0, 0] = iTolerant_MaxError;

            //////////////////////////////////////////////////////////
            //產生XML轉換類別
            SerializableContainerClass SCC = new SerializableContainerClass();

            SCC.sWrite(InFirstAlgoValue, "InFirstAlgoValue");
            SCC.sWrite(InSecondAlgoValue, "InSecondAlgoValue");
            SCC.sWrite(InFirstAlgoName, "InFirstAlgoName");
            SCC.sWrite(InSecondAlgoName, "InSecondAlgoName");
            SCC.sWrite(InSelectCalculator, "InSelectCalculator"); //ThresholdUserSetting
            SCC.dWrite(dFitstPredictValue, "dFitstPredictValue");
            SCC.dWrite(dSecondPredictValue, "dSecondPredictValue");
            SCC.dWrite(Tolerant_MaxError, "Tolerant_MaxError");
            SCC.Save(strXMLPath);//存檔

            //////////////////////////////////////////////////////////
            //將Parameter.xml傳到Blob
            BlobOperatorClass BOC = new BlobOperatorClass(StorageConnectStringBlob, tempDir, In_userinfo.FullName + ".tmp");
            BOC.UploadAsFile(strXMLPath, ModelBlobContainerName, In_userinfo.FullName + "/parameter.file", true);

            //////////////////////////////////////////////////////////
            //產生QMC
            QueueMessagesClass QMC = new QueueMessagesClass();
            QMC.Company = In_userinfo.Company;
            QMC.UserName = In_userinfo.User;
            QMC.MethodName = "ExecuteRIModule";
            QMC.ExecuteState = true;

            //////////////////////////////////////////////////////////
            // 送到Queue中
            QMC.Sent(StorageConnectStringQueue, MCRWorkQueueName);
            return QMC.MessageID;
        }

        private void Return_RIResult(Out_RI out_RI, TableOperatorClass TOC, string JobID, In_UserInfo In_userinfo)
        {
            // Set Paths
            String TmpModelFileLocalPath = tempDir + In_userinfo.FullName + "\\";
            String strXMLPath = TmpModelFileLocalPath + "Parameter.xml";

            // Remove Job Entity
            RemoveJobEvent(TOC, JobEventTableName, In_userinfo.Company, JobID);

            //////////////////////////////////////////////////////////
            //下載Result.xml
            BlobOperatorClass BOC = new BlobOperatorClass(StorageConnectStringBlob, tempDir, In_userinfo.FullName + ".tmp");
            BOC.DownloadAsFile(strXMLPath, ModelBlobContainerName, In_userinfo.FullName + "/result.file", true);

            //////////////////////////////////////////////////////////
            // 檢查&轉換Parameter.xml to real parameter
            SerializableContainerClass SCCR = new SerializableContainerClass();
            SCCR.Load(strXMLPath);

            //////////////////////////////////////////////////////////
            //把所有回傳值寫入out_Class的物件裡

            double[,] sResult = SCCR.dRead("SetParameterResult");

            if (sResult[0, 0] == 1)
            {
                out_RI.listCOM_Ack = new List<double[]>();
                out_RI.listCOM_Ack.Add(new double[1]);
                out_RI.listCOM_Ack[0][0] = 1;
            }
            else if (sResult[0, 0] == 2)
            {
                //若FirstAlgoValue與SecondAlgoValue為null則sResult為2 listCOM_Ack設為3
                out_RI.listCOM_Ack = new List<double[]>();
                out_RI.listCOM_Ack.Add(new double[1]);
                out_RI.listCOM_Ack[0][0] = 3;
            }
            else
            {
                out_RI.listCOM_Ack = transferDoubleToList(SCCR.dRead("RICOM_Ack"));
                if (out_RI.listCOM_Ack[0][0] == 0)
                {
                    out_RI.listOutRI_Value = transferDoubleToList(SCCR.dRead("RIOutRI_Value"));  // (0)
                    out_RI.listOutRI_Threshold = transferDoubleToList(SCCR.dRead("RIOutRI_Threshold")); // (1)
                    out_RI.listOutTolerant_MaxError = transferDoubleToList(SCCR.dRead("RIOutTolerant_MaxError")); // (2)
                }
                else
                    out_RI.listCOM_Ack[0][0] = 2;
            }
        }

        private string Execute_GSIModule(int numberOfGroup, string iInSelectAlgorithm, double iGSI_RT, double iGSI_Threshold, double iRefreshCounter, double iInGSI_TSVD_Condition_Number_Criteria, double iInGSI_TSVD_Energy_Ratio_Criteria, In_UserInfo In_userinfo)
        {
            String TmpModelFileLocalPath = tempDir + In_userinfo.FullName + "\\";
            String strXMLPath = TmpModelFileLocalPath + "Parameter.xml";

            //寫入GSI_RT資訊
            double[,] GSI_RT = new double[1, numberOfGroup];
            for (int i = 0; i < numberOfGroup; i++)
                GSI_RT[0, i] = iGSI_RT;

            //寫入GSI_Threshold資訊
            double[,] GSI_Threshold = new double[1, numberOfGroup];
            for (int i = 0; i < numberOfGroup; i++)
                GSI_Threshold[0, i] = iGSI_Threshold;

            //寫入RefreshCounter資訊
            double[,] RefreshCounter = new double[1, 1];
            RefreshCounter[0, 0] = iRefreshCounter;        //由GUI設定

            //寫入GSI_TSVD_Condition_Number_Criteria資訊
            double[,] GSI_TSVD_Condition_Number_Criteria = new double[1, 1];
            GSI_TSVD_Condition_Number_Criteria[0, 0] = iInGSI_TSVD_Condition_Number_Criteria;

            //寫入GSI_TSVD_Energy_Ratio_Criteria資訊
            double[,] GSI_TSVD_Energy_Ratio_Criteria = new double[1, 1];
            GSI_TSVD_Energy_Ratio_Criteria[0, 0] = iInGSI_TSVD_Energy_Ratio_Criteria;

            //////////////////////////////////////////////////////////
            //產生XML轉換類別
            SerializableContainerClass SCC = new SerializableContainerClass();
            SCC.dWrite(GSI_RT, "GSI_RT");
            SCC.dWrite(GSI_Threshold, "GSI_Threshold");
            SCC.dWrite(RefreshCounter, "RefreshCounter");
            SCC.dWrite(GSI_TSVD_Condition_Number_Criteria, "GSI_TSVD_Condition_Number_Criteria");
            SCC.dWrite(GSI_TSVD_Energy_Ratio_Criteria, "GSI_TSVD_Energy_Ratio_Criteria");
            SCC.sWrite(iInSelectAlgorithm, "InSelectAlgorithm");
            SCC.Save(strXMLPath);//存檔

            //////////////////////////////////////////////////////////
            //將Parameter.xml傳到Blob
            BlobOperatorClass BOC = new BlobOperatorClass(StorageConnectStringBlob, tempDir, In_userinfo.FullName + ".tmp");
            BOC.UploadAsFile(strXMLPath, ModelBlobContainerName, In_userinfo.FullName + "/parameter.file", true);

            //////////////////////////////////////////////////////////
            //產生QMC
            QueueMessagesClass QMC = new QueueMessagesClass();
            QMC.Company = In_userinfo.Company;
            QMC.UserName = In_userinfo.User;
            QMC.MethodName = "ExecuteGSIModule";
            QMC.ExecuteState = true;

            //////////////////////////////////////////////////////////
            // 送到Queue中
            QMC.Sent(StorageConnectStringQueue, MCRWorkQueueName);
            return QMC.MessageID;
        }

        private void Return_GSIResult(Out_GSI conjectureModel, TableOperatorClass TOC, string JobID, In_UserInfo In_userinfo)
        {
            // Set Paths
            String TmpModelFileLocalPath = tempDir + In_userinfo.FullName + "\\";
            String strXMLPath = TmpModelFileLocalPath + "Parameter.xml";

            // Remove Job Entity
            RemoveJobEvent(TOC, JobEventTableName, In_userinfo.Company, JobID);

            //////////////////////////////////////////////////////////
            //下載Result.xml
            BlobOperatorClass BOC = new BlobOperatorClass(StorageConnectStringBlob, tempDir, In_userinfo.FullName + ".tmp");
            BOC.DownloadAsFile(strXMLPath, ModelBlobContainerName, In_userinfo.FullName + "/result.file", true);

            //////////////////////////////////////////////////////////
            // 檢查&轉換Parameter.xml to real parameter
            SerializableContainerClass SCCR = new SerializableContainerClass();
            SCCR.Load(strXMLPath);

            //////////////////////////////////////////////////////////
            //把所有回傳值寫入out_Class的物件裡

            double[,] sResult = SCCR.dRead("SetParameterResult");

            if (sResult[0, 0] == 1)
            {
                conjectureModel.listCOM_Ack = new List<double[]>();
                conjectureModel.listCOM_Ack.Add(new double[1]);
                conjectureModel.listCOM_Ack[0][0] = 1;
            }
            else
            {
                conjectureModel.listCOM_Ack = transferDoubleToList(SCCR.dRead("GSICOM_Ack"));
                if (conjectureModel.listCOM_Ack[0][0] == 0)
                {
                    conjectureModel.listOUTGSI_RT = transferDoubleToList(SCCR.dRead("GSIOUTGSI_RT")); // (1)
                    conjectureModel.listOUTGSI_RTsize = transferDoubleToList(SCCR.dRead("GSIOUTGSI_RTsize")); // (2)
                    conjectureModel.listOUTphaseID = transferDoubleToList(SCCR.dRead("GSIOUTphaseID"));  // (3)
                    conjectureModel.listOUTstepID = transferDoubleToList(SCCR.dRead("GSIOUTstepID")); // (4)
                    conjectureModel.listOUTcontextID = transferDoubleToList(SCCR.dRead("GSIOUTcontextID")); // (5)
                    conjectureModel.listOUTGSIvalue = transferDoubleToList(SCCR.dRead("GSIOUTGSIvalue")); // (6)
                    conjectureModel.listOUTGSIThreshold = transferDoubleToList(SCCR.dRead("GSIOUTGSIThreshold")); // (7)
                    conjectureModel.listOUTtop10_PhaseID = transferDoubleToList(SCCR.dRead("GSIOUTtop10_PhaseID")); // (8)
                    conjectureModel.listOUTtop10_ContextID = transferDoubleToList(SCCR.dRead("GSIOUTtop10_ContextID")); // (9)
                    conjectureModel.listOUTtop10_IndicatorID = transferDoubleToList(SCCR.dRead("GSIOUTtop10_IndicatorID"));  // (10)
                    conjectureModel.listOUTtop10_StepTypeID = transferDoubleToList(SCCR.dRead("GSIOUTtop10_StepTypeID")); // (11)
                    conjectureModel.listOUTtop10_ISIValue = transferDoubleToList(SCCR.dRead("GSIOUTtop10_ISIValue"));// (12)
                    conjectureModel.listOUTISIsize = transferDoubleToList(SCCR.dRead("GSIOUTISIsize")); // (13)
                }
                else
                    conjectureModel.listCOM_Ack[0][0] = 2;

            }
        }

        private string createModelInformationXMLFile(string vMachineId, string CNCType, string CNCNumber, string NCProgram, string model_Id, string version, string Company, string userName, DateTime collectedStartDate, DateTime collectedEndDate,
            List<MetrologyPoint> allIndicator, List<MetrologyPoint> allPoint, int[] allAction, List<Group> groupValue)
        {
            string ModelInfoString = string.Empty;
            XmlDocument doc = new XmlDocument();

            XmlElement company = doc.CreateElement("ModelInfo");
            doc.AppendChild(company);

            XmlElement vMachineID = doc.CreateElement("vMachineID");
            vMachineID.InnerText = vMachineId;
            company.AppendChild(vMachineID);//加入至GroupList節點底下

            XmlElement ModelCreatedDate = doc.CreateElement("ModelCreatedDate");
            ModelCreatedDate.InnerText = DateTime.Now.ToString();
            company.AppendChild(ModelCreatedDate);

            XmlElement ModelCreateCompany = doc.CreateElement("ModelCreateCompany");
            ModelCreateCompany.InnerText = Company;
            company.AppendChild(ModelCreateCompany);

            XmlElement ModelCreateUser = doc.CreateElement("ModelCreateUser");
            ModelCreateUser.InnerText = userName;
            company.AppendChild(ModelCreateUser);

            XmlElement indicatorStartDate = doc.CreateElement("indicatorStartDate");
            indicatorStartDate.InnerText = collectedStartDate.ToString();
            company.AppendChild(indicatorStartDate);

            XmlElement indicatorEndDate = doc.CreateElement("indicatorEndDate");
            indicatorEndDate.InnerText = collectedEndDate.ToString();
            company.AppendChild(indicatorEndDate);

            XmlElement cncType = doc.CreateElement("cncType");
            cncType.InnerText = CNCType;
            company.AppendChild(cncType);

            XmlElement cnc_number = doc.CreateElement("cnc_number");
            cnc_number.InnerText = CNCNumber;
            company.AppendChild(cnc_number);

            XmlElement NCProgram_ID = doc.CreateElement("NCProgram_ID");
            NCProgram_ID.SetAttribute("value", NCProgram);
            company.AppendChild(NCProgram_ID);

            XmlElement modelId = doc.CreateElement("modelId");
            modelId.SetAttribute("ID", model_Id);
            modelId.SetAttribute("Version", version);
            company.AppendChild(modelId);

            //a set of all indicators
            XmlElement indicatorRules = doc.CreateElement("indicatorRules");
            indicatorRules.SetAttribute("name", "indicatorRules");
            company.AppendChild(indicatorRules);

            foreach (MetrologyPoint mPoint in allIndicator)
            {
                XmlElement indicatorRule = doc.CreateElement("indicatorRule");
                indicatorRule.SetAttribute("PROCESSKEY", mPoint.Value.ToString());
                indicatorRule.SetAttribute("VARIABLENAME", mPoint.Name);
                indicatorRules.AppendChild(indicatorRule);
            }

            //a set of all actions
            XmlElement Action_DEF = doc.CreateElement("Action_DEF");
            Action_DEF.SetAttribute("name", "Actions");
            company.AppendChild(Action_DEF);

            foreach (int action in allAction)
            {
                XmlElement Action = doc.CreateElement("Action");
                Action.SetAttribute("Action_ID", action.ToString());
                Action_DEF.AppendChild(Action);
            }

            //a set of selected points
            XmlElement METROLOGY_DEF = doc.CreateElement("METROLOGY_DEF");
            METROLOGY_DEF.SetAttribute("name", "MetrologyItems");
            company.AppendChild(METROLOGY_DEF);

            foreach (MetrologyPoint mPoint in allPoint)
            {
                XmlElement MetrologyItems = doc.CreateElement("MetrologyList");
                MetrologyItems.SetAttribute("MetrologyItem", mPoint.Value.ToString());
                MetrologyItems.SetAttribute("MetrologyItemName", mPoint.Name);
                METROLOGY_DEF.AppendChild(MetrologyItems);
            }

            foreach (Group group in groupValue)
            {
                XmlElement Group = doc.CreateElement("Group");
                Group.SetAttribute("ID", group.GroupName);
                company.AppendChild(Group); //加入至company節點底下

                //metrology items (points)
                XmlElement MetrologyItem = doc.CreateElement("MetrologyItem");
                MetrologyItem.InnerText = group.PointList[0].Value.ToString();
                Group.AppendChild(MetrologyItem);

                //sensor
                XmlElement sensor = doc.CreateElement("Sensor");
                sensor.SetAttribute("ID", "1");
                Group.AppendChild(sensor);

                //actions
                foreach (Action action in group.ActionList)
                {
                    XmlElement Action = doc.CreateElement("Action");
                    Action.SetAttribute("ID", action.ActionNumber.ToString());
                    sensor.AppendChild(Action);

                    //process items (indicators)
                    XmlElement xNewIndicatorIndexs = doc.CreateElement("indicatorIndexs");
                    Action.AppendChild(xNewIndicatorIndexs);

                    foreach (MetrologyPoint indicatorIndexStr in action.IndicatorList)
                    {
                        XmlElement xNewIndicator = doc.CreateElement("PROCESSKEY");
                        xNewIndicator.InnerText = indicatorIndexStr.Value.ToString();
                        xNewIndicatorIndexs.AppendChild(xNewIndicator);
                    }
                }
            }
            try
            {
                ModelInfoString = doc.OuterXml;
            }
            catch (Exception ex)
            {
                ModelInfoString = string.Empty;
                writeLog(ex);
            }

            return ModelInfoString;
        }

        private void InsertLogEvent(TableOperatorClass TOC, string tableName, string HostName, string InstanceID, string URL, string Catalog, string Message, bool IsUsingDebug)
        {
            if (IsUsingDebug)
            {
                string partitionKey = HostName + "_" + InstanceID;
                string rowKey = DateTime.UtcNow.ToString("yyyy-MM-dd-HH:mm:ss") + "-" + System.Guid.NewGuid().ToString("N");

                DataEntity_LogEvent DELE = new DataEntity_LogEvent(partitionKey, rowKey);
                DELE.Time = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
                DELE.URL = URL;
                DELE.Catalog = Catalog;
                DELE.Message = Message;

                TOC.InsertEntity(tableName, DELE);
            }
        }

        private void RemoveJobEvent(TableOperatorClass TOC, string tableName, string JobName, string JobID)
        {
            string partitionKey = JobName;
            string rowKey = JobID;
            TOC.DeleteEntity<DataEntity_JobEvent>(tableName, partitionKey, rowKey);
        }

        private List<double[]> transferDoubleToList(double[,] newArray)
        {
            int xDimension = newArray.GetLength(0);
            int yDimension = newArray.GetLength(1);
            List<double[]> listX = new List<double[]>();
            for (int i = 0; i < xDimension; i++)
            {
                //每一次要List要新Add 一定要重新new裡面的元素 不然會被覆蓋掉
                double[] listY = new double[yDimension];
                for (int j = 0; j < yDimension; j++)
                {
                    listY[j] = newArray[i, j];
                }
                listX.Add(listY);
            }
            return listX;
        }

        private List<double[]> transferDoubleToList(double newdouble)
        {
            List<double[]> listX = new List<double[]>();
            double[] listY = new double[1];
            listY[0] = newdouble;
            listX.Add(listY);
            return listX;
        }

        private double[,] transferListToDouble(List<double[]> newList)
        {
            int xDimension = newList.Count;
            int yDimension = 0;
            if (newList.Count > 0)
            {
                yDimension = newList[0].GetLength(0);
            }
            else
            {
                yDimension = 0;
            }
            double[,] doublex = new double[xDimension, yDimension];

            for (int i = 0; i < xDimension; i++)
            {
                //每一次要List要新Add 一定要重新new裡面的元素 不然會被覆蓋掉
                double[] listY = newList[i];
                for (int j = 0; j < yDimension; j++)
                {
                    doublex[i, j] = listY[j];
                }
            }
            return doublex;
        }

        //新增DTtemp裡的資料夾 資料夾的名稱為userName  ex:DTtemp\CompanyName_userName
        private void creatUserFolder(string TempDirName, string CompanyName, string UserName, bool ClearBeforeCreate)
        {           
            if (TempDirName.CompareTo("") != 0 && CompanyName.CompareTo("") != 0 && UserName.CompareTo("") != 0)
            {
                // Specify a "currently active folder"
                string SourceDirPath = TempDirName + CompanyName + "_" + UserName + "\\"; //c:\DTtemp\CompanyName_userName\;
                if (ClearBeforeCreate)
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
                creatUserFolder(TempDirName, CompanyName, UserName);
            }
            else
            {
                if (TempDirName.CompareTo("") == 0)
                {
                    throw new Exception("TempDirName is Empty");
                }
                if (CompanyName.CompareTo("") == 0)
                {
                    throw new Exception("CompanyName is Empty");
                }
                if (UserName.CompareTo("") == 0)
                {
                    throw new Exception("UserName is Empty");
                }
            }
        }

        public static void AddDirectorySecurity(string FileName, string Account, FileSystemRights Rights, AccessControlType ControlType)
        {
            DirectoryInfo dInfo = new DirectoryInfo(FileName);
            DirectorySecurity dSecurity = dInfo.GetAccessControl();
            dSecurity.AddAccessRule(new FileSystemAccessRule(Account, Rights, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.None, ControlType));
            dInfo.SetAccessControl(dSecurity);
        }


        //新增DTtemp裡的資料夾 資料夾的名稱為userName  ex:DTtemp\CompanyName_userName
        private void creatUserFolder(string TempDirName, string CompanyName, string UserName)
        {

            //string SourceDirPath = TempDirName + CompanyName + "_" + UserName + "\\"; //c:\DTtemp\CompanyName_userName\;

            //if (Directory.Exists(SourceDirPath))
            //{
            //    foreach (string FilePath in Directory.GetFiles(SourceDirPath))
            //    {
            //        File.Delete(FilePath);
            //    }
            //    Directory.Delete(SourceDirPath, true);
            //}

            if (TempDirName.CompareTo("") != 0 && CompanyName.CompareTo("") != 0 && UserName.CompareTo("") != 0)
            {
                // Specify a "currently active folder"
                string SourceDirPath = TempDirName + CompanyName + "_" + UserName; //c:\DTtemp\CompanyName_userName;

                //於userName資料夾底下建置Models與Temp兩個子資料夾
                string ModelDirPath = System.IO.Path.Combine(SourceDirPath, "Models");
                string TempDirPath = System.IO.Path.Combine(SourceDirPath, "Temp");

                // Create the subfolder
                Directory.CreateDirectory(ModelDirPath);
                Directory.CreateDirectory(TempDirPath);

                StreamWriter SW = File.CreateText(SourceDirPath + "\\" + DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss") + ".create");
                SW.WriteLine("Created.");
                SW.Flush();
                SW.Close();
            }
            else
            {
                if (TempDirName.CompareTo("") == 0)
                {
                    throw new Exception("TempDirName is Empty");
                }
                if (CompanyName.CompareTo("") == 0)
                {
                    throw new Exception("CompanyName is Empty");
                }
                if (UserName.CompareTo("") == 0)
                {
                    throw new Exception("UserName is Empty");
                }
            }
        }

        private void writeLog(Exception ex)
        {
            FileStream thefile;
            StreamWriter zz;

            // Time Information Parameter
            DateTime nowtime;

            nowtime = DateTime.Now;

            thefile = new FileStream(tempDir + "file.log", FileMode.OpenOrCreate | FileMode.Append, FileAccess.Write);

            string errorMessage = ex.ToString();
            zz = new StreamWriter(thefile);
            zz.WriteLine("Error" + " :" + "  " + errorMessage + DateTime.UtcNow);

            zz.Close();
            thefile.Close();
        }


        ///////////////////////////////////////// MCS_201207/23 /////////////////////////////////////////////


        public Dictionary<string, List<string>> Get_ProductBasicInfo() {
           
            GC.Collect();
            List<string> Insert_productBasicInfo = new List<string>();
            Dictionary<string, List<string>>Return_productBasicInfo = new Dictionary<string, List<string>>();
            string sqlCommand = "";
            DataSet PDB_BasicInfo = null;
            using (SqlConnection conn = new SqlConnection(DBConnectionString))
            {
                try
                {
                    conn.Open();
                    //取得ProductbasicInfo表資訊
                    sqlCommand = "SELECT * FROM ProductbasicInfo";
                    SqlDataAdapter dapt_ProductbasicInfo = new SqlDataAdapter(sqlCommand, conn);
                    PDB_BasicInfo = new DataSet();
                    dapt_ProductbasicInfo.Fill(PDB_BasicInfo);
                    conn.Close();
                }
                catch (Exception ex)
                {
                    conn.Close();
                    writeLog(ex);
                }
                conn.Close();
            }
            if (PDB_BasicInfo != null)
            {
                int totalPDB_BasicInfo = PDB_BasicInfo.Tables[0].Rows.Count;
                for (int i = 0; i < totalPDB_BasicInfo; i++)
                {    
                    Insert_productBasicInfo = new List<string>();
                    Insert_productBasicInfo.Add(PDB_BasicInfo.Tables[0].Rows[i]["ServiceBrokerID"].ToString());
                    Insert_productBasicInfo.Add(PDB_BasicInfo.Tables[0].Rows[i]["vMachineID"].ToString());
                    Insert_productBasicInfo.Add(PDB_BasicInfo.Tables[0].Rows[i]["CNCID"].ToString());
                    Insert_productBasicInfo.Add(PDB_BasicInfo.Tables[0].Rows[i]["CNCType"].ToString());
                    Insert_productBasicInfo.Add(PDB_BasicInfo.Tables[0].Rows[i]["ProductID"].ToString());
                    Insert_productBasicInfo.Add(PDB_BasicInfo.Tables[0].Rows[i]["ProductType"].ToString());
                    Insert_productBasicInfo.Add(PDB_BasicInfo.Tables[0].Rows[i]["XTableName"].ToString());
                    Insert_productBasicInfo.Add(PDB_BasicInfo.Tables[0].Rows[i]["YBlockTableName"].ToString());
                    Return_productBasicInfo.Add(i.ToString(), Insert_productBasicInfo);
                }
            }
            return Return_productBasicInfo;
        }
        
        public List<MetrologyPoint> GetXTableDEF(string XTableName) 
        {

            GC.Collect();
            List<MetrologyPoint> returnList = null;
            string sqlCommand = "SELECT FieldName, IndicatorName, IndicatorType FROM XTableDEF WHERE TableName = '" + XTableName + "' ORDER BY XTableDEF.[Order] ";
            DataSet ds = null;
            using (SqlConnection conn = new SqlConnection(DBConnectionString))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter dapt = new SqlDataAdapter(sqlCommand, conn);
                    ds = new DataSet();
                    dapt.Fill(ds);
                    int TotalIndicator = ds.Tables[0].Rows.Count;
                    returnList = new List<MetrologyPoint>();
                    for (int i = 0; i < TotalIndicator; i++)
                    {
                        MetrologyPoint indicator = new MetrologyPoint();
                        indicator.MeasureType = ds.Tables[0].Rows[i]["IndicatorType"].ToString();
                        indicator.Name = ds.Tables[0].Rows[i]["IndicatorName"].ToString();
                        indicator.DataField = ds.Tables[0].Rows[i]["FieldName"].ToString();
                        indicator.Value = i + 1;
                        returnList.Add(indicator);
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    returnList = new List<MetrologyPoint>();
                    conn.Close();
                    writeLog(ex);
                }
                conn.Close();
            }
            return returnList;
        }
        
        public List<MetrologyPoint> GetYTableDEF(string YTableName)
        {

            GC.Collect();
            List<MetrologyPoint> returnList = new List<MetrologyPoint>();
            string sqlCommand = "";
            DataSet ds_MetrologyItems = null;
            using (SqlConnection conn = new SqlConnection(DBConnectionString))
            {
                try
                {
                    conn.Open();
                    //get all metrology items
                    sqlCommand = "SELECT FieldName, MetrologyName, MetrologyType, MetrologyBlock FROM YTableDEF WHERE TableName = '" + YTableName + "' ORDER BY YTableDEF.[Order] ";
                    SqlDataAdapter dapt_MetrologyItems = new SqlDataAdapter(sqlCommand, conn);
                    ds_MetrologyItems = new DataSet();
                    dapt_MetrologyItems.Fill(ds_MetrologyItems);                 
                    conn.Close();
                }
                catch (Exception ex)
                {
                    conn.Close();
                    writeLog(ex);
                }
                conn.Close();
            }

            if (ds_MetrologyItems != null)
            {

                int totalMetrology = ds_MetrologyItems.Tables[0].Rows.Count;
              
                int temp = 0;

                //fill data to objects
                for (int i = 0; i < totalMetrology; i++)
                {
                    MetrologyPoint point = new MetrologyPoint();
                    point.Name = ds_MetrologyItems.Tables[0].Rows[i]["MetrologyName"].ToString();
                    point.MeasureType = ds_MetrologyItems.Tables[0].Rows[i]["MetrologyType"].ToString();
                    point.DataField = ds_MetrologyItems.Tables[0].Rows[i]["FieldName"].ToString();
                    point.Actions = ds_MetrologyItems.Tables[0].Rows[i]["MetrologyBlock"].ToString();
                    point.Value = i + 1;
                    returnList.Add(point);
                }
            }

            return returnList;
        }

        public bool DCtoBlob(DCPInfo DCPInfos, string UserName, string XTable, string YTable)
        {
            XmlDocument xdDC = null;
            tempDir = Path.Combine(ConfigurationManager.AppSettings["EmulatedDiv"], ConfigurationManager.AppSettings["TempDir"]);
            xdDC = GenerateDataCollectionXML(DCPInfos);
            if (xdDC != null)
            {

                String TmpModelFileLocalPath = tempDir + UserName + "\\";

                if (!(Directory.Exists(TmpModelFileLocalPath)))
                {
                    Directory.CreateDirectory(TmpModelFileLocalPath);
                }

                String strXMLPath = TmpModelFileLocalPath + "DCXML.xml";
                //先存到暫存路徑裡         
                xdDC.Save(strXMLPath);
                //////////////////////////////////////////////////////////
                //將Parameter.xml傳到Blob
                BlobOperatorClass BlobOperatoClient = new BlobOperatorClass(StorageConnectStringBlob, tempDir, UserName + ".xml");
                BlobOperatoClient.UploadAsFile(strXMLPath, ModelBlobContainerName, UserName + "/DCXML.zip", true);

                return true;
            }
            else
            {
                return false;
            }

        }

        public bool DSToBlob(DCPInfo DCPInfos, string UserName, string XTable, string YTable)
        {
            String TmpModelFileLocalPath = tempDir + UserName + "\\";

            if (!(Directory.Exists(TmpModelFileLocalPath)))
            {
                Directory.CreateDirectory(TmpModelFileLocalPath);
            }

            String strXMLPath = TmpModelFileLocalPath + "DCXML.xml";
            //////////////////////////////////////////////////////////
            //下載Result.xml
            BlobOperatorClass BOC = new BlobOperatorClass(StorageConnectStringBlob, tempDir, UserName + ".xml");
            BOC.DownloadAsFile(strXMLPath, ModelBlobContainerName, UserName + "/DCXML.zip", true);

            XmlDocument xdDC = null;
            tempDir = Path.Combine(ConfigurationManager.AppSettings["EmulatedDiv"], ConfigurationManager.AppSettings["TempDir"]);
            xdDC = AppendDataSelectionXML(DCPInfos, TmpModelFileLocalPath, "DCXML.xml" );
            if (xdDC != null)
            {
                //先存到暫存路徑裡
                strXMLPath = TmpModelFileLocalPath + "DSXML.xml";
                xdDC.Save(strXMLPath);
                //////////////////////////////////////////////////////////
                //將Parameter.xml傳到Blob
                BlobOperatorClass BlobOperatoClient = new BlobOperatorClass(StorageConnectStringBlob, tempDir, UserName + ".xml");
                BlobOperatoClient.UploadAsFile(strXMLPath, ModelBlobContainerName, UserName + "/DSXML.zip", true);

                return true;
            }
            else
            {
                return false;
            }
            
        }

        public bool SGToBlob(DCPInfo DCPInfos, string UserName, string XTable, string YTable)
        {
            String TmpModelFileLocalPath = tempDir + UserName + "\\";
            String strXMLPath = TmpModelFileLocalPath + "DSXML.xml";
            //////////////////////////////////////////////////////////
            //下載Result.xml
            BlobOperatorClass BOC = new BlobOperatorClass(StorageConnectStringBlob, tempDir, UserName + ".xml");
            BOC.DownloadAsFile(strXMLPath, ModelBlobContainerName, UserName + "/DSXML.zip", true);

            XmlDocument xdDC = null;
            tempDir = Path.Combine(ConfigurationManager.AppSettings["EmulatedDiv"], ConfigurationManager.AppSettings["TempDir"]);
            xdDC = AppendDataSelectionXML(DCPInfos, TmpModelFileLocalPath, "DSXML.xml");
            if (xdDC != null)
            {
                //先存到暫存路徑裡
                strXMLPath = TmpModelFileLocalPath + "SGXML.xml";
                xdDC.Save(strXMLPath);
                //////////////////////////////////////////////////////////
                //將Parameter.xml傳到Blob
                BlobOperatorClass BlobOperatoClient = new BlobOperatorClass(StorageConnectStringBlob, tempDir, UserName + ".xml");
                BlobOperatoClient.UploadAsFile(strXMLPath, ModelBlobContainerName, UserName + "/SGXML.zip", true);

                return true;
            }
            else
            {
                return false;
            }

        }

        public List<String> FetchContextIDFromBlob(string UserName)
        {
            List<String> LSContextID = new List<String>();

            




            //////////////////////////////////////////////////////////////////////////
            return LSContextID;
        }

        XmlDocument GenerateDataCollectionXML(DCPInfo _DCPInfo)        
        {         
            XmlDocument doc = new XmlDocument();
            //建立DCP根節點 
            XmlElement root = doc.CreateElement("DC");
            //XmlAttribute attr = doc.CreateAttribute("xsi", "noNamespaceSchemaLocation", " ");
            //root.Attributes.Append(attr);

            //root.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");

            //產生DCP ID
            //DCPID = Guid.NewGuid().ToString();
            //root.SetAttribute("ID", DCPID);

            //建立DCType節點
            XmlElement DCType = doc.CreateElement("DCType");

            DCType.SetAttribute("StartTime", _DCPInfo.StartTime);                 //設定屬性
            DCType.SetAttribute("EndTime", _DCPInfo.EndTime);                     //設定屬性
            //DCType.SetAttribute("ConjectureType", _DADCPInfo.ConjectureType);       //設定屬性
            //DCType.SetAttribute("CollectionMethod", _DADCPInfo.CollectionMethod);   //設定屬性
            root.AppendChild(DCType);

            //建立Factory節點
            XmlElement Factory = doc.CreateElement("Factory");
            Factory.SetAttribute("Name", _DCPInfo.FactoryName);   //設定屬性

            //建立ServiceBroker節點
            XmlElement ServiceBroker = doc.CreateElement("ServiceBroker");
            //ServiceBroker.SetAttribute("Name", _DADCPInfo.ServiceBrokerInformation.Name);   //設定屬性
            ServiceBroker.SetAttribute("Name", _DCPInfo.CurrentInfo.ServiceBrokerID);   //設定屬性

            //建立v-Machine CNC節點
            XmlElement v_Machine, CNC;
            //foreach (vMachineInfo vM in _DADCPInfo.ServiceBrokerInformation.vMachineList)
            //{
            //    v_Machine = doc.CreateElement("v-Machine");
            //    v_Machine.SetAttribute("Name", vM.Name);
            //    foreach (CNCInfo CI in vM.CNCList)
            //    {
            //        CNC = doc.CreateElement("CNC");
            //        CNC.SetAttribute("Name", CI.Name);
            //        CNC.SetAttribute("Type", CI.Type);
            //        v_Machine.AppendChild(CNC);
            //    }
            //    ServiceBroker.AppendChild(v_Machine);
            //}
            v_Machine = doc.CreateElement("v-Machine");
            v_Machine.SetAttribute("Name", _DCPInfo.CurrentInfo.vMachineID);

            CNC = doc.CreateElement("CNC");
            CNC.SetAttribute("Name", _DCPInfo.CurrentInfo.CNCName);
            CNC.SetAttribute("Type", _DCPInfo.CurrentInfo.CNCType);
            v_Machine.AppendChild(CNC);

            root.AppendChild(v_Machine);


            Factory.AppendChild(ServiceBroker);
            root.AppendChild(Factory);

            //建立WorkPiece節點
            XmlElement WorkPiece = doc.CreateElement("WorkPiece");
            WorkPiece.SetAttribute("Name", _DCPInfo.WorkPieceName);   //設定屬性
            WorkPiece.SetAttribute("Type", _DCPInfo.WorkPieceType);   //設定屬性

            XmlElement _Data, _Item;

            //建立Y_Data節點
            _Data = doc.CreateElement("Y_Data");
            foreach (YItem YI in _DCPInfo.Y_Data)
            {
                _Item = doc.CreateElement("Y_Item");
                _Item.SetAttribute("Name", YI.Name);   //設定屬性
                _Item.SetAttribute("Type", YI.Type);   //設定屬性
                _Item.SetAttribute("Block", YI.Block);
                _Item.InnerText = YI.Description;
                _Data.AppendChild(_Item);
            }
            WorkPiece.AppendChild(_Data);

            //建立X_Data節點
            _Data = doc.CreateElement("X_Data");
            foreach (XItem XI in _DCPInfo.X_Data)
            {
                _Item = doc.CreateElement("X_Item");
                _Item.SetAttribute("Name", XI.Name);   //設定屬性
                _Item.SetAttribute("Type", XI.Type);   //設定屬性
                _Item.SetAttribute("Position", XI.Position);   //設定屬性
                _Item.InnerText = XI.Description;
                _Data.AppendChild(_Item);
            }
            WorkPiece.AppendChild(_Data);

            //建立WorkPiece節點
            //建立AllPiece節點
            _Data = doc.CreateElement("AllPiece");
            foreach (AllPiece allpiece in _DCPInfo.WP_Data)
            {
                _Item = doc.CreateElement("RecordPiece");
                _Item.SetAttribute("ID", allpiece.ID);   //設定屬性
                _Item.SetAttribute("ProcessStartTime", allpiece.Process_StartTime);   //設定屬性
                _Item.SetAttribute("ProcessEndTime", allpiece.Process_EndTime);   //設定屬性
                _Item.SetAttribute("MetrologyStartTime", allpiece.Metrology_StartTime);   //設定屬性
                _Item.SetAttribute("MetrologyEndTime", allpiece.Metrology_EndTime);   //設定屬性
                _Data.AppendChild(_Item);
            }
            WorkPiece.AppendChild(_Data);

            root.AppendChild(WorkPiece);
                       
            //結束
            doc.AppendChild(root);

            //DCPXMLString = doc.InnerXml;

            //if (DCPXMLString != null)
            //    return true;
            //else
            //    return false;

            //DCPXMLString = doc.InnerXml;

            if (doc.InnerXml != null)
                return doc;
            else
                return null;
        }

        XmlDocument AppendDataSelectionXML(DCPInfo _DCPInfo, String strFilePath, String strFileName)
        {
            XmlDocument doc = new XmlDocument();

            doc.Load(strFilePath + strFileName);

            XmlNodeList myNodeList = doc.SelectNodes("//WorkPiece");

            XmlElement _Data, _Item;
            //建立TrainPiece節點
            XmlElement TrainPiece = doc.CreateElement("TrainPiece");
            foreach (TrainPiece trainpiece in _DCPInfo.WP_TrainData)
            {
                _Item = doc.CreateElement("RecordPiece");
                _Item.SetAttribute("ID", trainpiece.ID);   //設定屬性
                _Item.SetAttribute("ProcessStartTime", trainpiece.Process_StartTime);   //設定屬性
                _Item.SetAttribute("ProcessEndTime", trainpiece.Process_EndTime);   //設定屬性
                _Item.SetAttribute("MetrologyStartTime", trainpiece.Metrology_StartTime);   //設定屬性
                _Item.SetAttribute("MetrologyEndTime", trainpiece.Metrology_EndTime);   //設定屬性
                TrainPiece.AppendChild(_Item);
            }
            myNodeList.Item(0).AppendChild(TrainPiece);
           
            //建立RunPiece節點
            XmlElement RunPiece = doc.CreateElement("RunPiece");

            foreach (RunPiece runPiece in _DCPInfo.WP_RunData)
            {
                _Item = doc.CreateElement("RecordPiece");
                _Item.SetAttribute("ID", runPiece.ID);   //設定屬性
                _Item.SetAttribute("ProcessStartTime", runPiece.Process_StartTime);   //設定屬性
                _Item.SetAttribute("ProcessEndTime", runPiece.Process_EndTime);   //設定屬性
                _Item.SetAttribute("MetrologyStartTime", runPiece.Metrology_StartTime);   //設定屬性
                _Item.SetAttribute("MetrologyEndTime", runPiece.Metrology_EndTime);   //設定屬性
                RunPiece.AppendChild(_Item);
            }          
            
            myNodeList.Item(0).AppendChild(RunPiece);

            doc.Save(strFilePath + "DSXML.xml");

            //foreach (XmlNode xn in myNodeList)
            //{
            //    XmlElement xe = (XmlElement)xn;

                

            //    //XmlNodeList mySubNodeList = xn.SelectNodes("parameter");

            //    //foreach (XmlNode xns in mySubNodeList)
            //    //{

            //    //    XmlElement xes = (XmlElement)xns;

            //    //    //if (xes.GetAttribute("name") == "Rule")
            //    //    {
            //    //        iRowCounter++;

            //    //        string strPlan = "";

            //    //        for (int iPlan = 0; iPlan < list_plan.Count; iPlan++)
            //    //        {
            //    //            if (xes.GetAttribute("planid") == (iPlan + 1).ToString())
            //    //            {
            //    //                strPlan = list_plan[iPlan];
            //    //            }
            //    //        }

            //    //        //DGV_FileContent.Rows.Add(xes.GetAttribute("ruleid"), strPlan, "", xes.GetAttribute("quality"), xes.GetAttribute("description"));
            //    //        DGV_ConfigContent.Rows.Add(xes.GetAttribute("ruleid"), strPlan, "", xes.GetAttribute("quality"), xes.GetAttribute("description"));
            //    //    }
            //    //}

            //}





            ////結束
            //doc.AppendChild(root);

            //DCPXMLString = doc.InnerXml;

            //if (DCPXMLString != null)
            //    return true;
            //else
            //    return false;

            //DCPXMLString = doc.InnerXml;

            if (doc.InnerXml != null)
                return doc;
            else
                return null;
        }

        XmlDocument AppendSetGroupXML(DCPInfo _DCPInfo, String strFilePath, String strFileName)
        {
            XmlDocument doc = new XmlDocument();

            doc.Load(strFilePath + strFileName);

            XmlNodeList myNodeList = doc.SelectNodes("//WorkPiece");

            XmlElement _Data, _Item;
            //建立TrainPiece節點
            XmlElement TrainPiece = doc.CreateElement("TrainPiece");
            foreach (TrainPiece trainpiece in _DCPInfo.WP_TrainData)
            {
                _Item = doc.CreateElement("RecordPiece");
                _Item.SetAttribute("ID", trainpiece.ID);   //設定屬性
                _Item.SetAttribute("ProcessStartTime", trainpiece.Process_StartTime);   //設定屬性
                _Item.SetAttribute("ProcessEndTime", trainpiece.Process_EndTime);   //設定屬性
                _Item.SetAttribute("MetrologyStartTime", trainpiece.Metrology_StartTime);   //設定屬性
                _Item.SetAttribute("MetrologyEndTime", trainpiece.Metrology_EndTime);   //設定屬性
                TrainPiece.AppendChild(_Item);
            }
            myNodeList.Item(0).AppendChild(TrainPiece);

            //建立RunPiece節點
            XmlElement RunPiece = doc.CreateElement("RunPiece");

            foreach (RunPiece runPiece in _DCPInfo.WP_RunData)
            {
                _Item = doc.CreateElement("RecordPiece");
                _Item.SetAttribute("ID", runPiece.ID);   //設定屬性
                _Item.SetAttribute("ProcessStartTime", runPiece.Process_StartTime);   //設定屬性
                _Item.SetAttribute("ProcessEndTime", runPiece.Process_EndTime);   //設定屬性
                _Item.SetAttribute("MetrologyStartTime", runPiece.Metrology_StartTime);   //設定屬性
                _Item.SetAttribute("MetrologyEndTime", runPiece.Metrology_EndTime);   //設定屬性
                RunPiece.AppendChild(_Item);
            }

            myNodeList.Item(0).AppendChild(RunPiece);

            doc.Save(strFilePath + "DSXML.xml");

            //foreach (XmlNode xn in myNodeList)
            //{
            //    XmlElement xe = (XmlElement)xn;



            //    //XmlNodeList mySubNodeList = xn.SelectNodes("parameter");

            //    //foreach (XmlNode xns in mySubNodeList)
            //    //{

            //    //    XmlElement xes = (XmlElement)xns;

            //    //    //if (xes.GetAttribute("name") == "Rule")
            //    //    {
            //    //        iRowCounter++;

            //    //        string strPlan = "";

            //    //        for (int iPlan = 0; iPlan < list_plan.Count; iPlan++)
            //    //        {
            //    //            if (xes.GetAttribute("planid") == (iPlan + 1).ToString())
            //    //            {
            //    //                strPlan = list_plan[iPlan];
            //    //            }
            //    //        }

            //    //        //DGV_FileContent.Rows.Add(xes.GetAttribute("ruleid"), strPlan, "", xes.GetAttribute("quality"), xes.GetAttribute("description"));
            //    //        DGV_ConfigContent.Rows.Add(xes.GetAttribute("ruleid"), strPlan, "", xes.GetAttribute("quality"), xes.GetAttribute("description"));
            //    //    }
            //    //}

            //}





            ////結束
            //doc.AppendChild(root);

            //DCPXMLString = doc.InnerXml;

            //if (DCPXMLString != null)
            //    return true;
            //else
            //    return false;

            //DCPXMLString = doc.InnerXml;

            if (doc.InnerXml != null)
                return doc;
            else
                return null;
        }
       // public void testdown(In_UserInfo User){


       //     GC.Collect();
       //     //參數初始化

       //     Out_IndicatorsPopulation population = new Out_IndicatorsPopulation();

       //     // Set Paths
       //     String TmpModelFileLocalPath = tempDir + User.FullName + "\\";
       //     String strXMLPath = TmpModelFileLocalPath + User.User + @"/DCXML.xml";

       //     //////////////////////////////////////////////////////////
       //     //下載Result.xml
       //     BlobOperatorClass BOC = new BlobOperatorClass(StorageConnectStringBlob, tempDir, User.FullName + ".xml");
       //     BOC.DownloadAsFile(strXMLPath, ModelBlobContainerName, User.User + "/DCXML.file", true);
        
        
       // }


       //public void Write_SGXML(Out_IndicatorsPopulation population, int Train, int Run) 
       // {
       //     try
       //     {
       //         XmlDocument doc = new XmlDocument();
       //         //建立DCP根節點 
       //         XmlElement root = doc.CreateElement("DC");
       //         XmlAttribute attr = doc.CreateAttribute("xsi", "noNamespaceSchemaLocation", " ");
       //         root.Attributes.Append(attr);

       //         root.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
       //         XmlElement _Data, _Itemroot, _Item;
       //         _Data = doc.CreateElement("Indicator");//indicator的根節點



       //         _Itemroot = doc.CreateElement("FieldInfo");

       //         _Item = doc.CreateElement("FieldName");
       //         _Item.SetAttribute("Type", "Const");
       //         _Item.SetAttribute("Name", "WorkPieceID");
       //         _Itemroot.AppendChild(_Item);
       //         _Item = doc.CreateElement("FieldName");
       //         _Item.SetAttribute("Type", "Const");
       //         _Item.SetAttribute("Name", "TimeStamp");
       //         _Itemroot.AppendChild(_Item);
       //         _Item = doc.CreateElement("FieldName");
       //         _Item.SetAttribute("Type", "Const");
       //         _Item.SetAttribute("Name", "BlockID");
       //         _Itemroot.AppendChild(_Item);
       //         _Data.AppendChild(_Itemroot);

       //         for (int i = 0; i < population.listIndicatorPopulationValue.Count; i++)
       //         {   _Itemroot = doc.CreateElement("Record");
       //             _Item = doc.CreateElement("FieldValue");
       //             string WID = population.WorkPieceID_Process[i].Trim();
       //             _Item.InnerText =WID;
       //             _Itemroot.AppendChild(_Item);
       //             _Item = doc.CreateElement("FieldValue");
       //             DateTime dt =population.listProcessStartTime[i].Date;
       //             _Item.InnerText = dt.ToString() ;
       //             _Itemroot.AppendChild(_Item);
       //             _Item = doc.CreateElement("FieldValue");
       //             int IA = population.listActionPopulationValue[i][0];
       //             _Item.InnerText =IA.ToString();
       //             _Itemroot.AppendChild(_Item);

       //             for (int k = 0; k < population.listIndicatorPopulationValue[i].Length; k++)
       //                 {
       //                     double dIP = population.listIndicatorPopulationValue[i][k];
       //                     _Item = doc.CreateElement("FieldValue");
       //                     _Item.InnerText = dIP.ToString();
       //                     _Itemroot.AppendChild(_Item);
       //                 } 
                   
       //            _Data.AppendChild(_Itemroot);
       //         }
       //         root.AppendChild(_Data);


       //         _Data = doc.CreateElement("Metrology");//metrology的根節點
                
       //         _Itemroot = doc.CreateElement("FieldInfo");
       //         _Item = doc.CreateElement("FieldName");
       //         _Item.SetAttribute("Type", "Const");
       //         _Item.SetAttribute("Name", "WorkPieceID");
       //         _Itemroot.AppendChild(_Item);
       //         _Item = doc.CreateElement("FieldName");
       //         _Item.SetAttribute("Type", "dateTime");
       //         _Item.SetAttribute("Name", "TimeStamp");
       //         _Itemroot.AppendChild(_Item);

       //         _Data.AppendChild(_Itemroot);
       //         for (int i = 0; i < population.listPointPopulationValue.Count; i++)
       //         {
       //             _Itemroot = doc.CreateElement("Record");
       //             _Item = doc.CreateElement("FieldValue");
       //             string WID = population.WorkPieceID_Metrology[i].Trim();
       //             _Item.InnerText = WID;
       //             _Itemroot.AppendChild(_Item);
       //             _Item = doc.CreateElement("FieldValue");
       //             DateTime dt = population.listMetrologyStartTime[i].Date;
       //             _Item.InnerText = dt.ToString();
       //             _Itemroot.AppendChild(_Item);
       //             for (int k = 0; k < population.listPointPopulationValue[i].Length; k++)
       //             {
       //                 double dPP = population.listPointPopulationValue[i][k];
       //                 _Item = doc.CreateElement("FieldValue");
       //                 _Item.InnerText = dPP.ToString();
       //                 _Itemroot.AppendChild(_Item);
       //             }

       //             _Data.AppendChild(_Itemroot);
       //         }
       //         root.AppendChild(_Data);
       //         doc.AppendChild(root);
       //         doc.Save(@"C:/DTtemp/123.xml");
       //     }
       //     catch (Exception ex)
       //     { 
            
            
            
       //     }






            
 
       // //     foreach ( )
       // //    {
               
       // //        _Item = doc.CreateElement("RecordPiece");
       // ////        _Item.SetAttribute("ID",allpiece.ID );   //設定屬性
       // ////        _Item.SetAttribute("TimeStamp",allpiece.TimeStamp);   //設定屬性
       // ////        _Data.AppendChild(_Item);
               
       // //   }











        
        
       // }


       //public void Write_SGXML(Out_IndicatorsPopulation population, int Train, int Run) {

       //     DCPInfo _DADCPInfo = new DCPInfo();
       //     DSXML(population, _DADCPInfo, Train, Run);
         
          
         
        
        
        
        
        
        
        
        
       // }

        //   void DSXML(Out_IndicatorsPopulation population,DCPInfo _DADCPInfo,int Train , int Run )        
        //{

        //    XmlDocument doc = new XmlDocument();
        //    //建立DCP根節點 
        //    XmlElement root = doc.CreateElement("DC");
        //    //XmlAttribute attr = doc.CreateAttribute("xsi", "noNamespaceSchemaLocation", " ");
        //    //root.Attributes.Append(attr);

        //    //root.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");

        //    //產生DCP ID
        //    //DCPID = Guid.NewGuid().ToString();
        //    //root.SetAttribute("ID", DCPID);

        //    //建立DCType節點
        //    XmlElement DCType = doc.CreateElement("DCType");

        //    DCType.SetAttribute("StartTime", _DADCPInfo.StartTime);                 //設定屬性
        //    DCType.SetAttribute("EndTime", _DADCPInfo.EndTime);                     //設定屬性
        //    //DCType.SetAttribute("ConjectureType", _DADCPInfo.ConjectureType);       //設定屬性
        //    //DCType.SetAttribute("CollectionMethod", _DADCPInfo.CollectionMethod);   //設定屬性
        //    root.AppendChild(DCType);

        //    //建立Factory節點
        //    XmlElement Factory = doc.CreateElement("Factory");
        //    Factory.SetAttribute("Name", _DADCPInfo.FactoryName);   //設定屬性

        //    //建立ServiceBroker節點
        //    XmlElement ServiceBroker = doc.CreateElement("ServiceBroker");
        //    ServiceBroker.SetAttribute("Name", _DADCPInfo.ServiceBrokerInformation.Name);   //設定屬性

        //    //建立v-Machine CNC節點
        //    XmlElement v_Machine, CNC;
        //    foreach (vMachineInfo vM in _DADCPInfo.ServiceBrokerInformation.vMachineList)
        //    {
        //        v_Machine = doc.CreateElement("v-Machine");
        //        v_Machine.SetAttribute("Name", vM.Name);
        //        foreach (CNCInfo CI in vM.CNCList)
        //        {
        //            CNC = doc.CreateElement("CNC");
        //            CNC.SetAttribute("Name", CI.Name);
        //            CNC.SetAttribute("Type", CI.Type);
        //            v_Machine.AppendChild(CNC);
        //        }
        //        ServiceBroker.AppendChild(v_Machine);
        //    }

        //    Factory.AppendChild(ServiceBroker);
        //    root.AppendChild(Factory);

        //    //建立WorkPiece節點
        //    XmlElement WorkPiece = doc.CreateElement("WorkPiece");
        //    WorkPiece.SetAttribute("Name", _DADCPInfo.WorkPieceName);   //設定屬性
        //    WorkPiece.SetAttribute("Type", _DADCPInfo.WorkPieceType);   //設定屬性
        //    WorkPiece.SetAttribute("XTable", _DADCPInfo.XTable);
        //    WorkPiece.SetAttribute("YTable", _DADCPInfo.YTable); 
        //    XmlElement _Data, _Item;

        //    //建立Y_Data節點
        //    _Data = doc.CreateElement("Y_Data");
        //    foreach (YItem YI in _DADCPInfo.Y_Data)
        //    {
        //        _Item = doc.CreateElement("Y_Item");
        //        _Item.SetAttribute("Name", YI.Name);   //設定屬性
        //        _Item.SetAttribute("Type", YI.Type);   //設定屬性
        //        _Item.InnerText = YI.Description;
        //        _Data.AppendChild(_Item);
        //    }
        //    WorkPiece.AppendChild(_Data);

        //    //建立X_Data節點
        //    _Data = doc.CreateElement("X_Data");
        //    foreach (XItem XI in _DADCPInfo.X_Data)
        //    {
        //        _Item = doc.CreateElement("X_Item");
        //        _Item.SetAttribute("Name", XI.Name);   //設定屬性
        //        _Item.SetAttribute("Type", XI.Type);   //設定屬性
        //        _Item.SetAttribute("Position", XI.Position);   //設定屬性
        //        _Item.InnerText = XI.Description;
        //        _Data.AppendChild(_Item);
        //    }
        //    WorkPiece.AppendChild(_Data);
        //    root.AppendChild(WorkPiece);
       
        //    //建立AllPiece節點
        //    _Data = doc.CreateElement("AllPiece");
        //    foreach (AllPiece allpiece in _DADCPInfo.WP_Data)
        //    {
               
        //        _Item = doc.CreateElement("RecordPiece");
        //        _Item.SetAttribute("ID",allpiece.ID );   //設定屬性
        //        _Item.SetAttribute("TimeStamp",allpiece.TimeStamp);   //設定屬性
        //        _Data.AppendChild(_Item);
               
        //    }
        //    WorkPiece.AppendChild(_Data);
        //    root.AppendChild(WorkPiece);

        //    _Data = doc.CreateElement("TrainPiece");
        //    for (int i = 0; i < Train; i++)
        //    {
        //        _Item = doc.CreateElement("RecordPiece");
        //        _Item.SetAttribute("ID", _DADCPInfo.WP_Data[i].ID);   //設定屬性
        //        _Item.SetAttribute("TimeStamp", _DADCPInfo.WP_Data[i].TimeStamp);   //設定屬性
        //        _Data.AppendChild(_Item);

        //    }
        //    WorkPiece.AppendChild(_Data);
        //    root.AppendChild(WorkPiece);

        //    _Data = doc.CreateElement("RunPiece");
        //    for (int i = Train; i < (Train + Run); i++)
        //    {
        //        _Item = doc.CreateElement("RecordPiece");
        //        _Item.SetAttribute("ID", _DADCPInfo.WP_Data[i].ID);   //設定屬性
        //        _Item.SetAttribute("TimeStamp", _DADCPInfo.WP_Data[i].TimeStamp);   //設定屬性
        //        _Data.AppendChild(_Item);

        //    }
        //    WorkPiece.AppendChild(_Data);
        //    root.AppendChild(WorkPiece);

        //    //結束
        //    doc.AppendChild(root);

        //    //DCPXMLString = doc.InnerXml;

        //    //if (DCPXMLString != null)
        //    //    return true;
        //    //else
        //    //    return false;

        //    //DCPXMLString = doc.InnerXml;

        //    if (doc.InnerXml != null) { }

        //    else
        //    { }
        //}















        /////////////////////////////////////////////////////////////////

        //public Out_YValue getRealValue(int trainContextIDCount, int runContextIDCount, List<double> processKeyList, List<double> metrologyKeyList)
        //{
        //    GC.Collect();
        //    List<double[]> yList = new List<double[]>();


        //    ////////////////////////////※宣告參數///////////////////////////////////////

        //    //製程資料的筆數與量測資料的參數個數
        //    //processKeyList:GUI選取的製程參數序列  ex:1,3,5  第一個製程參數 第三個製程參數 第五個製程參數
        //    //metrologyKeyList:GUI選取的量測參數序列  ex:1,3,5  第一個量測參數 第三個量測參數 第五個量測參數
        //    int processKeyCount = processKeyList.Count;
        //    int metrologyKeyCount = metrologyKeyList.Count;



        //    ///////////////////////////※資料庫連線////////////////////////////////////

        //    //宣告DT擷取建模資料的雲端資料庫(SQL Azure)連線字串
        //    //string connectionString = @"Data Source=CLOUD-PC\SQLEXPRESS;Initial Catalog=STDB_LocalTest;Integrated Security=True ;Trusted_Connection=Yes";
        //    //string connectionString = @"Server=tcp:f0kx1o7e0j.database.windows.net;Database=test;User ID=kimeyo@f0kx1o7e0j;Password=Cotandy60ggy;Trusted_Connection=False;Encrypt=True;";
        //    //string connectionString = @"Server=kcwatmfege.database.windows.net;Database=test;User ID=jerry75916;Password=Xatm092roosalsa;Trusted_Connection=False;Encrypt=True;";


        //    //宣告擷取雲端資料庫STDB裡的Metrology、Process兩個table的SQL語法
        //    string sqlCommand_Metrology = "select * from Metrology order by CONTEXTID ";
        //    string sqlCommand_Process = "select * from Process order by CONTEXTID ";

        //    SqlConnection con = new SqlConnection(DBConnectionString);
        //    con.Close();
        //    con.Open();

        //    SqlDataAdapter dapt_Process = new SqlDataAdapter(sqlCommand_Process, con);
        //    SqlDataAdapter dapt_Metrology = new SqlDataAdapter(sqlCommand_Metrology, con);

        //    //new DataSet
        //    DataSet ds_Process = new DataSet();
        //    DataSet ds_Metrology = new DataSet();

        //    //將DataAdapter的資料塞進去DataSet
        //    //先將整個table從資料庫裡抓出來存到記憶體裡的DataSet 之後再從DataSet裡撈資料
        //    dapt_Process.Fill(ds_Process);
        //    dapt_Metrology.Fill(ds_Metrology);


        //    //擷取出Y值給CreateConjectureModel用
        //    //yList.Clear();
        //    for (int i = 0; i < metrologyKeyCount; i++)
        //    {
        //        double[] yTrainList = new double[trainContextIDCount + trainContextIDCount + runContextIDCount + runContextIDCount + runContextIDCount];//Stage1,Stage2,FreeRun,Phase1,Phase2
        //        //塞Stage1的實際Y
        //        for (int j = 0; j < trainContextIDCount; j++)
        //        {
        //            int columnIndex = (int)metrologyKeyList[i] + 4;  //METROLOGY的table從左邊數來第幾個column
        //            yTrainList[j] = (double)Convert.ToDouble(ds_Metrology.Tables[0].Rows[j].ItemArray[columnIndex]);
        //        }
        //        //塞Stage2的實際Y
        //        for (int j = 0; j < trainContextIDCount; j++)
        //        {
        //            int columnIndex = (int)metrologyKeyList[i] + 4;  //METROLOGY的table從左邊數來第幾個column
        //            yTrainList[j + trainContextIDCount] = (double)Convert.ToDouble(ds_Metrology.Tables[0].Rows[j].ItemArray[columnIndex]);
        //        }
        //        //塞FreeRun的實際Y
        //        for (int j = 0; j < runContextIDCount; j++)
        //        {
        //            int columnIndex = (int)metrologyKeyList[i] + 4;  //METROLOGY的table從左邊數來第幾個column
        //            yTrainList[j + trainContextIDCount + trainContextIDCount] = (double)Convert.ToDouble(ds_Metrology.Tables[0].Rows[j + trainContextIDCount].ItemArray[columnIndex]);
        //        }
        //        //塞Phase1的實際Y
        //        for (int j = 0; j < runContextIDCount; j++)
        //        {
        //            int columnIndex = (int)metrologyKeyList[i] + 4;  //METROLOGY的table從左邊數來第幾個column
        //            yTrainList[j + trainContextIDCount + trainContextIDCount + runContextIDCount] = (double)Convert.ToDouble(ds_Metrology.Tables[0].Rows[j + trainContextIDCount].ItemArray[columnIndex]);
        //        }
        //        //塞Phase2的實際Y
        //        for (int j = 0; j < runContextIDCount; j++)
        //        {
        //            int columnIndex = (int)metrologyKeyList[i] + 4;  //METROLOGY的table從左邊數來第幾個column
        //            yTrainList[j + trainContextIDCount + trainContextIDCount + runContextIDCount + runContextIDCount] = (double)Convert.ToDouble(ds_Metrology.Tables[0].Rows[j + trainContextIDCount].ItemArray[columnIndex]);
        //        }
        //        yList.Add(yTrainList);

        //    }

        //    Out_YValue out_YValue = new Out_YValue();
        //    out_YValue.listY = yList;

        //    return out_YValue;
        //}


        //public Out_IndicatorsPopulation getPopulation(List<double> processKeyList, List<double> metrologyKeyList, DateTime startTime, DateTime endTime)
        //{
        //    GC.Collect();
        //    //參數初始化

        //    Out_IndicatorsPopulation population = new Out_IndicatorsPopulation();

        //    try
        //    {
        //        int contextIDCount = 0;

        //        List<string> listContexID = new List<string>();
        //        List<DateTime> listProcessStartTime = new List<DateTime>();
        //        List<DateTime> listProcessEndTime = new List<DateTime>();
        //        List<DateTime> listMetrologyStartTime = new List<DateTime>();
        //        List<DateTime> listMetrologyEndTime = new List<DateTime>();
        //        List<double[]> listIndicatorPopulationValue = new List<double[]>();
        //        List<double[]> listPointPopulationValue = new List<double[]>();

        //        string startTimeYY = startTime.Year.ToString();
        //        string startTimeMM = startTime.Month.ToString();
        //        string startTimeDD = startTime.Day.ToString();
        //        string endTimeYY = endTime.Year.ToString();
        //        string endTimeMM = endTime.Month.ToString();
        //        string endTimeDD = endTime.Day.ToString();

        //        //回傳所選取的indicator與point的編號
        //        int indicatorIndexCount = 0;
        //        int pointIndexCount = 0;
        //        indicatorIndexCount = processKeyList.Count;
        //        pointIndexCount = metrologyKeyList.Count; ;

        //        double[] arrIndicatorNum = new double[indicatorIndexCount];
        //        double[] arrPointNum = new double[pointIndexCount];
        //        population.indicatorIndexList = processKeyList.ToArray();
        //        population.pointIndexList = metrologyKeyList.ToArray();


        //        //string connectionString = @"Data Source=CLOUD-PC\SQLEXPRESS;Initial Catalog=STDB_LocalTest;Integrated Security=True ;Trusted_Connection=Yes";
        //        //string connectionString = @"Server=tcp:f0kx1o7e0j.database.windows.net;Database=test;User ID=kimeyo@f0kx1o7e0j;Password=Cotandy60ggy;Trusted_Connection=False;Encrypt=True;";

        //        //敏軒
        //        //string connectionString = @"Server=kcwatmfege.database.windows.net;Database=test;User ID=jerry75916;Password=Xatm092roosalsa;Trusted_Connection=False;Encrypt=True;";


        //        string sqlCommand_Metrology = "select * from Metrology where TIMETAG BETWEEN '" + startTimeYY + "-" + startTimeMM + "-" + startTimeDD + "' and '" + endTimeYY + "-" + endTimeMM + "-" + endTimeDD + "' ";
        //        string sqlCommand_Process = "select * from Process where TIMETAG BETWEEN '" + startTimeYY + "-" + startTimeMM + "-" + startTimeDD + "' and '" + endTimeYY + "-" + endTimeMM + "-" + endTimeDD + "'";

        //        SqlConnection con = new SqlConnection(DBConnectionString);
        //        con.Close();
        //        con.Open();

        //        SqlDataAdapter dapt_Metrology = new SqlDataAdapter(sqlCommand_Metrology, con);
        //        SqlDataAdapter dapt_Process = new SqlDataAdapter(sqlCommand_Process, con);

        //        //new DataSet
        //        DataSet ds_Metrology = new DataSet();
        //        DataSet ds_Process = new DataSet();

        //        //將DataAdapter的資料塞進去DataSet
        //        dapt_Metrology.Fill(ds_Metrology);
        //        dapt_Process.Fill(ds_Process);

        //        //計算CONTEXTID筆數
        //        contextIDCount = ds_Metrology.Tables[0].Rows.Count;


        //        for (int i = 0; i < contextIDCount; i++)
        //        {
        //            //挑出CONTEXTID塞入list
        //            listContexID.Add(ds_Metrology.Tables[0].Rows[i].ItemArray[0].ToString());

        //            //挑出ProcessStartTime塞入list,目前是用STDB Metrology裡的TIMETAG
        //            listProcessStartTime.Add((DateTime)ds_Metrology.Tables[0].Rows[i].ItemArray[1]);

        //            //挑出ProcessEndTime塞入list,目前是用STDB Metrology裡的TIMETAG
        //            listProcessEndTime.Add((DateTime)ds_Metrology.Tables[0].Rows[i].ItemArray[1]);

        //            //挑出MetrologyStartTime塞入list,目前是用STDB Metrology裡的TIMETAG
        //            listMetrologyStartTime.Add((DateTime)ds_Metrology.Tables[0].Rows[i].ItemArray[1]);

        //            //挑出MetrologyEndTime塞入list,目前是用STDB Metrology裡的TIMETAG
        //            listMetrologyEndTime.Add((DateTime)ds_Metrology.Tables[0].Rows[i].ItemArray[1]);

        //            double[] arrIndicatorValue = new double[indicatorIndexCount];
        //            double[] arrPointValue = new double[pointIndexCount];

        //            for (int j = 0; j < indicatorIndexCount; j++)
        //            {
        //                int columnNumber = (int)processKeyList[j] + 5;    //從STDB的第一欄算起的第幾欄
        //                string buffer = ds_Process.Tables[0].Rows[i].ItemArray[columnNumber].ToString();
        //                if (buffer == "")
        //                {
        //                    buffer = "0";
        //                }
        //                arrIndicatorValue[j] = (double)Convert.ToDouble(buffer);
        //            }
        //            listIndicatorPopulationValue.Add(arrIndicatorValue);

        //            for (int k = 0; k < pointIndexCount; k++)
        //            {
        //                int columnNumber = (int)metrologyKeyList[k] + 4;
        //                string buffer = ds_Metrology.Tables[0].Rows[i].ItemArray[columnNumber].ToString();
        //                arrPointValue[k] = (double)Convert.ToDouble(buffer);
        //            }
        //            listPointPopulationValue.Add(arrPointValue);
        //        }


        //        //將list塞入Out_IndicatorsPopulation物件屬性
        //        population.listContexID = listContexID;
        //        population.listProcessStartTime = listProcessStartTime;
        //        population.listProcessEndTime = listProcessEndTime;
        //        population.listMetrologyStartTime = listMetrologyStartTime;
        //        population.listMetrologyEndTime = listMetrologyEndTime;
        //        population.listIndicatorPopulationValue = listIndicatorPopulationValue;
        //        population.listPointPopulationValue = listPointPopulationValue;

        //        return population;
        //    }
        //    catch (Exception e)
        //    {
        //        //Ack
        //        population.Ack = 1;

        //        writeLog(e);

        //        return population;
        //    }
        //}


    }
}
