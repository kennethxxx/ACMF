using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using IPS.OntologyService;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using OMC.Comm;
using System.IO;
using System.Runtime.Serialization.Json;
using System.ComponentModel;
using System.Text;

namespace IPS.Comm
{
    public class CToolClass
    {
        //宣告服務
        public static IPS.OntologyService.OntologyServiceClient proxyOntologyService;   //宣告Ontology的服務
        //public static IPS.RecommendedCuttingTool.CuttingToolInterfaceClient proxyCuttingTool = new IPS.RecommendedCuttingTool.CuttingToolInterfaceClient();   //宣告CuttingTool的服務
        //public static IPS.RecommendedCuttingTool.FileData getFileData = new IPS.RecommendedCuttingTool.FileData();

        //建構子
        public CToolClass()
        {
            proxyOntologyService = new IPS.OntologyService.OntologyServiceClient();
            //proxyCuttingTool = new IPS.RecommendedCuttingTool.CuttingToolInterfaceClient();
            //getFileData = new IPS.RecommendedCuttingTool.FileData();
        }
    }

    /// <summary>
    /// Step1: 製程工程師輸入的客製化參數
    /// </summary>
    public class PEParameters
    {
        public string LeftTurretNCName { get; set; }
        public string RighttTurretNCName { get; set; }
        public string WorkPieceRadiusDiffer { get; set; }
        public string WheelInnerRadiu { get; set; }
        public List<OntologyDatabaseData> KnowledgeBaseData { get; set; }
        public List<PE_SelectRulesCT> SelectRuleList { get; set; }
    }

    /// <summary>
    /// 知識庫資料
    /// </summary>
    public class OntologyDatabaseData
    {
        public string OntologyList_id { get; set; }
        public string OntologyList_name { get; set; }
        public string Ontology_time { get; set; }
    }

    /// <summary>
    /// 製程工程師選擇Rule
    /// </summary>
    public class PE_SelectRulesCT
    {
        public bool Rule_Select { get; set; }
        public string idrules { get; set; }
        public string rules_name { get; set; }
        public string rules_description { get; set; }
        public string rules { get; set; }
    }

    /// <summary>
    /// Step2: 製程工程師推薦刀具
    /// </summary>
    /// 

    public class OntologyResultForNCFile : INotifyPropertyChanged
    {
        public string index { get; set; }
        public string FileName { get; set; }
        public string StageNo { get; set; }
        public string CuttingToolNo { get; set; }
        public string SelectCT { get; set; }
        public ObservableCollection<OntologyResultForCuttingTool> ReplaceableRuleSet { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string porpertyName)
        {
            var e = this.PropertyChanged;
            if (e != null)
            {
                e(this, new PropertyChangedEventArgs(porpertyName));
            }
        }

        public static ObservableCollection<OntologyResultForNCFile> get_String(OntologyResultForNCFile objNCInfo,string sonStr)
        {
            ObservableCollection<OntologyResultForCuttingTool> currentList = new ObservableCollection<OntologyResultForCuttingTool>();
            ObservableCollection<OntologyResultForNCFile> AllList = new ObservableCollection<OntologyResultForNCFile>();

            DataContractJsonSerializer serinferForNC = new DataContractJsonSerializer(typeof(ObservableCollection<OntologyResultForNCFile>));
            MemoryStream responseStreamNC = new MemoryStream(Encoding.UTF8.GetBytes(sonStr));
            ObservableCollection<OntologyResultForNCFile>  InferNC = serinferForNC.ReadObject(responseStreamNC) as ObservableCollection<OntologyResultForNCFile>;
            OntologyResultForNCFile _parameter = new OntologyResultForNCFile();

            foreach (OntologyResultForNCFile _result in InferNC)
            {
                if (_result.FileName.Equals(objNCInfo.FileName) & _result.CuttingToolNo.Equals(objNCInfo.CuttingToolNo) & _result.StageNo.Equals(objNCInfo.StageNo))
                {
                    foreach (OntologyResultForCuttingTool _CTparameters in objNCInfo.ReplaceableRuleSet)
                    {
                        OntologyResultForCuttingTool _parameters = new OntologyResultForCuttingTool();
                        _parameter.index = _CTparameters.index;
                        _parameters.selectReplace = _CTparameters.selectReplace;
                        _parameters.CuttingToolNo = _result.CuttingToolNo;
                        _parameters.ReplaceableCuttingToolNo = _CTparameters.ReplaceableCuttingToolNo;
                        _parameters.FinishingRule = _CTparameters.FinishingRule;
                        _parameters.RoughingRule = _CTparameters.RoughingRule;
                        _parameters.ToolNWPHardnessRule = _CTparameters.ToolNWPHardnessRule;
                        _parameters.ExternalDiameterProcessingRule = _CTparameters.ExternalDiameterProcessingRule;
                        _parameters.InternalDiameterProcessingCylinderRule = _CTparameters.InternalDiameterProcessingCylinderRule;
                        _parameters.InternalDiameterProcessingCuboidRule = _CTparameters.InternalDiameterProcessingCuboidRule;
                        currentList.Add(_parameters);
                    }
                    _parameter.index = _result.index;
                    _parameter.FileName = _result.FileName;
                    _parameter.SelectCT = _result.CuttingToolNo; //預設為nc檔寫的刀
                    _parameter.StageNo = _result.StageNo;
                    _parameter.CuttingToolNo = _result.CuttingToolNo;
                    _parameter.ReplaceableRuleSet = currentList;
                }
            }
            AllList.Add(_parameter);
            return AllList;
        }

    }

    public class OntologyResultForCuttingTool : INotifyPropertyChanged
    {
        public string index { get; set; }
        public string selectReplace { get; set; }
        public string CuttingToolNo { get; set; }
        public string ReplaceableCuttingToolNo { get; set; }
        public string RoughingRule { get; set; }
        public string FinishingRule { get; set; }
        public string ToolNWPHardnessRule { get; set; }
        public string ExternalDiameterProcessingRule { get; set; }
        public string InternalDiameterProcessingCylinderRule { get; set; }
        public string InternalDiameterProcessingCuboidRule { get; set; }
        public string sortindex { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string porpertyName)
        {
            var e = this.PropertyChanged;
            if (e != null)
            {
                e(this, new PropertyChangedEventArgs(porpertyName));
            }
        }
    }

    public class NCFileClassAll
    {
        public bool selectReplace2 { get; set; }
        public string FileName2 { get; set; }
        public string CuttingToolList2 { get; set; }
        public string StageNo2 { get; set; }
        public string CuttingToolNo2 { get; set; }
        public string ProcessingPatterns2 { get; set; }
        public string RValue2 { get; set; }
        public string BeforeReplace2 { get; set; }
        public string BeforeResultPic2 { get; set; }
        public string SelectCT2 { get; set; }
        public string ReplaceableCuttingToolNo2 { get; set; }
        public string ReplaceableCuttingToolNoRvalue2 { get; set; }
    }

    /// <summary>
    /// Step3: 進行碰撞檢測所有參數，
    /// </summary>
    public class VECollisionInfo
    {
        public string FileName { get; set; }
        public List<SelectCuttingToolCollision> CuttingToolList { get; set; }
        public string MachiningTime { get; set; }
        public string CollisionDetection { get; set; }
    }

    /// <summary>
    /// 碰撞所選所的Stage編號與刀具
    /// </summary>
    public class SelectCuttingToolCollision
    {
        public string StageNo { get; set; }
        public string SelectCT { get; set; }
    }

    /// <summary>
    /// Step4: 確認所有參數
    /// </summary>
    public class ConfirmCTNCInfo
    {
        public string L_CuttingToolNCInfo { get; set; }
        public string R_CuttingToolNCInfo { get; set; }
        public List<ConfirmCTNCInfo> FileName { get; set; }
    }
}
