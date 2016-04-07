using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using Visifire.Charts;
//using IPS.Common;
using IPS.ModelCreation;
using System.Collections;
using System.Collections.ObjectModel;
using IPS.Common;
using Serialization;
using IPS.Views;

namespace IPS.Common
{
    public class MCDataCollectionLocalContainer //放置該頁面的暫時資料
    {
        public string XTableName = string.Empty;
        public string YTableName = string.Empty;

        public List<CheckBox> MetrologyTypeCheckStateList = new List<CheckBox>();
        public List<CheckBox> IndicatorTypeCheckStateList = new List<CheckBox>();
        public List<MetrologyPoint> MetrologyTypeList = new List<MetrologyPoint>();
        public List<MetrologyPoint> IndicatorTypeList = new List<MetrologyPoint>();
        public DateTime SearchStartTime = DateTime.Now;
        public DateTime SearchEndTime = DateTime.Now;
        public int SelectedIndicatorItemsCount = 0;
        public int SelectedMetrologyItemsCount = 0;
        public void Clear()
        {
            MetrologyTypeCheckStateList = new List<CheckBox>();
            IndicatorTypeCheckStateList = new List<CheckBox>();
            MetrologyTypeList = new List<MetrologyPoint>();
            IndicatorTypeList = new List<MetrologyPoint>();
            SearchStartTime = DateTime.Now;
            SearchEndTime = DateTime.Now;
            SelectedIndicatorItemsCount = 0;
            SelectedMetrologyItemsCount = 0;
        }
    }
    public class MCDataCollectionGlobalContainer //離開頁面後會固定裡面的資料
    {
        public string XTableName = string.Empty;
        public string YTableName = string.Empty;

        public string ServiceBrokerID = string.Empty;
        public string ProductID = string.Empty;
        public string vMachine = string.Empty;
        public string CNCnumber = string.Empty;
        public string NCprogram = string.Empty;
        public string CNCType = string.Empty; //
        public string model_Id = string.Empty; // "001"
        public string version = string.Empty; //"1.0"
        public ObservableCollection<int> allAction = null;
        public string LoginUsername = string.Empty;
        public string Company = string.Empty;

        public DateTime SearchStartTime = DateTime.Now;
        public DateTime SearchEndTime = DateTime.Now;

        public string[] listContextID;                  
        public double[,] listIndicatorPopulationValue;  
        public int[,] listActionPopulationValue;        
        public double[,] listPointPopulationValue;

        public string strSelectedContextID = string.Empty;

        public ObservableCollection<ModelCreation.Context> contextList = new ObservableCollection<ModelCreation.Context>();
        public Dictionary<String, ModelCreation.Indicator> listAllIndicators = new Dictionary<String, ModelCreation.Indicator>();
        public Dictionary<String, ModelCreation.Indicator> listAllPoints = new Dictionary<String, ModelCreation.Indicator>();

        public List<MetrologyPoint> MetrologyTypeList = new List<MetrologyPoint>();
        public List<MetrologyPoint> IndicatorTypeList = new List<MetrologyPoint>();
        public List<CheckBox> MetrologyTypeCheckStateList = new List<CheckBox>();
        public List<CheckBox> IndicatorTypeCheckStateList = new List<CheckBox>();

        public ObservableCollection<MetrologyPoint> SelectedMetrologyTypeList = new ObservableCollection<MetrologyPoint>();//有被選擇使用的Metrology項目
        public ObservableCollection<MetrologyPoint> SelectedIndicatorTypeList = new ObservableCollection<MetrologyPoint>();//有被選擇使用的Indicator項目

        public int TotalcontextListCount = 0;

        public void Clear()
        {
            vMachine = string.Empty;
            CNCnumber = string.Empty;
            NCprogram = string.Empty;
            CNCType = string.Empty; //
            model_Id = string.Empty; // "001"
            version = string.Empty; //"1.0"
            allAction = null;
            LoginUsername = string.Empty;
            Company = string.Empty;

            SearchStartTime = DateTime.Now;
            SearchEndTime = DateTime.Now;

            listContextID = null;
            listIndicatorPopulationValue = null;
            listActionPopulationValue = null;
            listPointPopulationValue = null;

            strSelectedContextID = string.Empty;

            contextList = new ObservableCollection<ModelCreation.Context>();
            listAllIndicators = new Dictionary<String, ModelCreation.Indicator>();
            listAllPoints = new Dictionary<String, ModelCreation.Indicator>();

            MetrologyTypeList = new List<MetrologyPoint>();
            IndicatorTypeList = new List<MetrologyPoint>();
            MetrologyTypeCheckStateList = new List<CheckBox>();
            IndicatorTypeCheckStateList = new List<CheckBox>();

            SelectedMetrologyTypeList = new ObservableCollection<MetrologyPoint>();//有被選擇使用的Metrology項目
            SelectedIndicatorTypeList = new ObservableCollection<MetrologyPoint>();//有被選擇使用的Indicator項目

            TotalcontextListCount = 0;
        }
        public void Copy(MCDataCollectionGlobalContainer p)
        {
            vMachine = p.vMachine;
            CNCnumber = p.CNCnumber;
            NCprogram = p.NCprogram;
            CNCType = p.CNCType; //
            CNCnumber = p.CNCnumber;
            vMachine = p.vMachine;
            model_Id = p.model_Id; // "001"
            version = p.version; //"1.0"
            allAction = p.allAction;
            LoginUsername = p.LoginUsername;
            Company = p.Company;

            SearchStartTime = p.SearchStartTime;
            SearchEndTime = p.SearchEndTime;

            listContextID = p.listContextID;
            listIndicatorPopulationValue = p.listIndicatorPopulationValue;
            listActionPopulationValue = p.listActionPopulationValue;
            listPointPopulationValue = p.listPointPopulationValue;

            strSelectedContextID = p.strSelectedContextID;

            contextList = p.contextList;
            listAllIndicators = p.listAllIndicators;
            listAllPoints = p.listAllPoints;

            MetrologyTypeList = p.MetrologyTypeList;
            IndicatorTypeList = p.IndicatorTypeList;
            MetrologyTypeCheckStateList = p.MetrologyTypeCheckStateList;
            IndicatorTypeCheckStateList = p.IndicatorTypeCheckStateList;

            SelectedMetrologyTypeList = p.SelectedMetrologyTypeList;
            SelectedIndicatorTypeList = p.SelectedIndicatorTypeList;

            TotalcontextListCount = p.TotalcontextListCount;
        }

        
    }

    

    public class MCDataSelectionLocalContainer
    {
        public string XTableName = string.Empty;
        public string YTableName = string.Empty;

        public int InitTrainingPercentage = 76;
        public int TotalContextCount = 0;
        public int TrainingCount = 0;
        public int RunningCount = 0;
        public int TrainingPercentage = 0;
        public int RunningPercentage = 0;
        public ObservableCollection<ModelCreation.Context> TrainingContextList = new ObservableCollection<ModelCreation.Context>();
        public ObservableCollection<ModelCreation.Context> RunningContextList = new ObservableCollection<ModelCreation.Context>();
        public void Clear()
        {
            InitTrainingPercentage = 76;
            TotalContextCount = 0;
            TrainingCount = 0;
            RunningCount = 0;
            TrainingPercentage = 0;
            RunningPercentage = 0;
            TrainingContextList = new ObservableCollection<ModelCreation.Context>();
            RunningContextList = new ObservableCollection<ModelCreation.Context>();
        }
    }
    public class MCDataSelectionGlobalContainer
    {
        public string XTableName = string.Empty;
        public string YTableName = string.Empty;

        public int TrainingCount = 0;
        public int RunningCount = 0;
        public int TrainingPercentage = 0;
        public int RunningPercentage = 0;
        public ObservableCollection<ModelCreation.Context> TrainingContextList = new ObservableCollection<ModelCreation.Context>();
        public ObservableCollection<ModelCreation.Context> RunningContextList = new ObservableCollection<ModelCreation.Context>();
        public void Clear()
        {
            TrainingCount = 0;
            RunningCount = 0;
            TrainingPercentage = 0;
            RunningPercentage = 0;
            TrainingContextList = new ObservableCollection<ModelCreation.Context>();
            RunningContextList = new ObservableCollection<ModelCreation.Context>();
        }
        public void Copy(MCDataSelectionGlobalContainer p)
        {
            TrainingCount = p.TrainingCount;
            RunningCount = p.RunningCount;
            TrainingPercentage = p.TrainingPercentage;
            RunningPercentage = p.RunningPercentage;
            TrainingContextList = p.TrainingContextList;
            RunningContextList = p.RunningContextList;
        }
    }



    public class MCSetGroupLocalContainer
    {
        public string XTableName = string.Empty;
        public string YTableName = string.Empty;

        public double InitPoint_USL = 0.015;
        public double InitPoint_UCL = 0.015;
        public double InitPoint_LSL = 0.0;
        public double InitPoint_LCL = 0.0;

        public ObservableCollection<MetrologyPoint> SelectedMetrologyTypeList = new ObservableCollection<MetrologyPoint>();//有被選擇使用的Metrology項目
        public ObservableCollection<MetrologyPoint> SelectedIndicatorTypeList = new ObservableCollection<MetrologyPoint>();//有被選擇使用的Indicator項目

        public ObservableCollection<Group> groupList = new ObservableCollection<Group>();
        public int groupCount = 0;
        public int MaximumIndicatorCount = 0;

        public List<MetrologyPoint> UngroupedMetrologyTypeList;

        public List<CheckBox> checkboxesAction1 = new List<CheckBox>();
        public List<CheckBox> checkboxesAction2 = new List<CheckBox>();
        public List<CheckBox> checkboxesPoints = new List<CheckBox>();
        public void Clear()
        {
            InitPoint_USL = 0.015;
            InitPoint_UCL = 0.015;
            InitPoint_LSL = 0.0;
            InitPoint_LCL = 0.0;

            SelectedMetrologyTypeList = new ObservableCollection<MetrologyPoint>();//有被選擇使用的Metrology項目
            SelectedIndicatorTypeList = new ObservableCollection<MetrologyPoint>();//有被選擇使用的Indicator項目

            groupList = new ObservableCollection<Group>();
            groupCount = 0;
            MaximumIndicatorCount = 0;

            UngroupedMetrologyTypeList = null;

            checkboxesAction1 = new List<CheckBox>();
            checkboxesAction2 = new List<CheckBox>();
            checkboxesPoints = new List<CheckBox>();
        }
        
    }
    public class MCSetGroupGlobalContainer
    {
        public string XTableName = string.Empty;
        public string YTableName = string.Empty;

        public ObservableCollection<MetrologyPoint> SelectedMetrologyTypeList = new ObservableCollection<MetrologyPoint>();//有被選擇使用的Metrology項目
        public ObservableCollection<MetrologyPoint> SelectedIndicatorTypeList = new ObservableCollection<MetrologyPoint>();//有被選擇使用的Indicator項目
        public ObservableCollection<Group> groupList = new ObservableCollection<Group>();
        public int groupCount = 0;
        public int MaximumIndicatorCount = 0;
        public ObservableCollection<MetrologyPoint> combinedIndicator;
        public ObservableCollection<MetrologyPoint> combinedPoint;
        public List<ComboxDataObj> listPointCombo;
        public void Clear()
        {
            SelectedMetrologyTypeList = new ObservableCollection<MetrologyPoint>();//有被選擇使用的Metrology項目
            SelectedIndicatorTypeList = new ObservableCollection<MetrologyPoint>();//有被選擇使用的Indicator項目
            groupList = new ObservableCollection<Group>();
            groupCount = 0;
            MaximumIndicatorCount = 0;
            combinedIndicator = null;
            combinedPoint = null;
            listPointCombo = null;
        }
    }



    public class MCClearAbnormalYGlobalContainer
    {
        public string XTableName = string.Empty;
        public string YTableName = string.Empty;

        public double[,] PointList;
        public int metrologyPointSize = 0;
        public double[,] PatternListIndex4PatternID;
        public double[,] ContextIDOfStepIndex;
        public double[,] ArtUList;
        public double[,] IndicatorIDIndex4ContextID;
        public double[,] ContextIDIndex4PointID;
        public double[,] SortPatternListOfPoint;
        public double[,] contextIDChartData;
        public int patternSize = 0;
        public void Clear()
        {
            PointList = null;
            metrologyPointSize = 0;
            PatternListIndex4PatternID = null;
            ContextIDOfStepIndex = null;
            ArtUList = null;
            IndicatorIDIndex4ContextID = null;
            ContextIDIndex4PointID = null;
            SortPatternListOfPoint = null;
            contextIDChartData = null;
            patternSize = 0;
        }
        public void Copy(MCClearAbnormalYGlobalContainer p) 
        {
            PointList = p.PointList;
            metrologyPointSize = p.metrologyPointSize;
            PatternListIndex4PatternID = p.PatternListIndex4PatternID;
            ContextIDOfStepIndex = p.ContextIDOfStepIndex;
            ArtUList = p.ArtUList;
            IndicatorIDIndex4ContextID = p.IndicatorIDIndex4ContextID;
            ContextIDIndex4PointID = p.ContextIDIndex4PointID;
            SortPatternListOfPoint = p.SortPatternListOfPoint;
            contextIDChartData = p.contextIDChartData;
            patternSize = p.patternSize;
        }
    }



    public class MCAbnormalIsolatedGlobalContainer
    {
        public string XTableName = string.Empty;
        public string YTableName = string.Empty;

        public ObservableCollection<ModelCreation.Context> contextList;
        public int TotalcontextListCount = 0;
        public List<CheckBox> checkboxesCleanProcessIsolated;// = new List<CheckBox>();
        public List<CheckBox> checkboxesCleanProcessAbnormal;// = new List<CheckBox>();
        public int iCurrentIUnIsolatedAndAbnormalTrainCount;
        public int iCurrentIUnIsolatedTrainCount;
        public int iCurrentUnIsolatedRunCount;
        public ObservableCollection<string> checkboxesAbnormalStatusOC;// = new ObservableCollection<string>(checkboxesAbnormalStatus);
        public ObservableCollection<string> checkboxesIsolatedStatusOC;// = new ObservableCollection<string>(checkboxesIsolatedStatus);
        public List<String> checkboxesAbnormalStatus;
        public List<String> checkboxesIsolatedStatus;
        public int TrainingCount = 0;
        public int RunningCount = 0;

        public bool IsChangeAbIsoValue = false;
        public void Clear()
        {
            contextList = null;
            TotalcontextListCount = 0;
            checkboxesCleanProcessIsolated = null;
            checkboxesCleanProcessAbnormal = null;
            iCurrentIUnIsolatedAndAbnormalTrainCount = 0;
            iCurrentIUnIsolatedTrainCount = 0;
            iCurrentUnIsolatedRunCount = 0;
            checkboxesAbnormalStatusOC = null;
            checkboxesIsolatedStatusOC = null;
            checkboxesAbnormalStatus = null;
            checkboxesIsolatedStatus = null;
            TrainingCount = 0;
            RunningCount = 0;
            IsChangeAbIsoValue = false;
        }
    }



    public class MCVerifyDQIxLocalContainer
    {
        public string XTableName = string.Empty;
        public string YTableName = string.Empty;

        public double Init_Lambda = 1.0;
        public double Init_Threshold = 2.0;
        public double Init_FilterPercentage = 0.95;
        public int Init_RefreshCounter = 3;
        public int Init_ClusterNumber = 6;
        public double Init_FinAlpha = 0.85;
        public double Init_FoutAlpha = 0.85;

        public double[,] valueDQIxChartData;
        public double[,] valueThresholdDQIxChartData;   //Verify DQIx Tab => Threshold Value
        public double[,] valueContextIDDQIxChartData;   //Verify DQIx Tab => ContextID Value

        public String KSSType = "";
        public String KVSType = "";
        public Double ClusterNumber = 0;
        public double FinAlpha = 0;
        public double FoutAlpha = 0;
        public String VerificationMode = "";
        public Double Lambda = 0;
        public Double Threshold = 0;
        public Double FilterPercentage = 0;
        public Double RefreshCounter = 0;

        public void Clear()
        {
            Init_Lambda = 1.0;
            Init_Threshold = 2.0;
            Init_FilterPercentage = 0.95;
            Init_RefreshCounter = 3;
            Init_ClusterNumber = 6;
            Init_FinAlpha = 0.85;
            Init_FoutAlpha = 0.85;

            KSSType = "";
            KVSType = "";
            ClusterNumber = 0;
            FinAlpha = 0;
            FoutAlpha = 0;
            VerificationMode = "";
            Lambda = 0;
            Threshold = 0;
            FilterPercentage = 0;
            RefreshCounter = 0;

            valueDQIxChartData = null;
            valueThresholdDQIxChartData = null;
            valueContextIDDQIxChartData = null;
        }
    }

    public class MCVerifyDQIxGlobalContainer
    {
        public string XTableName = string.Empty;
        public string YTableName = string.Empty;

        public String KSSType = "";
        public String KVSType = "";
        public String VerificationMode = "";
        public double Lambda = 0;
        public double Threshold = 0;
        public double FilterPercentage = 0;
        public double RefreshCounter = 0;
        public double ClusterNumber = 0;
        public double FinAlpha = 0;
        public double FoutAlpha = 0;

        public double[,] valueDQIxChartData;
        public double[,] valueThresholdDQIxChartData;   //Verify DQIx Tab => Threshold Value
        public double[,] valueContextIDDQIxChartData;   //Verify DQIx Tab => ContextID Value

        public void Clear()
        {
            KSSType = "";
            KVSType = "";
            VerificationMode = "";
            Lambda = 0;
            Threshold = 0;
            FilterPercentage = 0;
            RefreshCounter = 0;
            ClusterNumber = 0;
            FinAlpha = 0;
            FoutAlpha = 0;

            valueDQIxChartData = null;
            valueThresholdDQIxChartData = null;
            valueContextIDDQIxChartData = null;
        }
        public void Copy(MCVerifyDQIxLocalContainer p) 
        {
            KSSType = p.KSSType;
            KVSType = p.KVSType;
            VerificationMode = p.VerificationMode;
            Lambda = p.Lambda;
            Threshold = p.Threshold;
            FilterPercentage = p.FilterPercentage;
            RefreshCounter = p.RefreshCounter;
            ClusterNumber = p.ClusterNumber;
            FinAlpha = p.FinAlpha;
            FoutAlpha = p.FoutAlpha;

            valueDQIxChartData = p.valueDQIxChartData;
            valueThresholdDQIxChartData = p.valueThresholdDQIxChartData;
            valueContextIDDQIxChartData = p.valueContextIDDQIxChartData;
        }
    }



    public class MCVerifyDQIyLocalContainer
    {
        public string XTableName = string.Empty;
        public string YTableName = string.Empty;

        public double Init_Vigilance = 0.99;
        public double Init_Threadhold = 0.0;
        public double Init_Accuracy = 95;

        public double Vigilance = 0;
        public double Threadhold = 0;
        public double Accuracy = 0;

        public int metrologyPointSize = 0;
        public double[,] valueDQIyChartData;

        public void Clear()
        {
            Init_Vigilance = 0.99;
            Init_Threadhold = 0.0;
            Init_Accuracy = 95;

            Vigilance = 0;
            Threadhold = 0;
            Accuracy = 0;

            metrologyPointSize = 0;
            valueDQIyChartData = null;
        }
    }

    public class MCVerifyDQIyGlobalContainer
    {
        public string XTableName = string.Empty;
        public string YTableName = string.Empty;

        public double Vigilance = 0;
        public double Threadhold = 0;
        public double Accuracy = 0;

        public int metrologyPointSize = 0;
        public double[,] valueDQIyChartData;

        public void Clear()
        {
            Vigilance = 0;
            Threadhold = 0;
            Accuracy = 0;

            metrologyPointSize = 0;
            valueDQIyChartData = null;
        }
        public void Copy(MCVerifyDQIyLocalContainer p) 
        {
            Vigilance = p.Vigilance;
            Threadhold = p.Threadhold;
            Accuracy = p.Accuracy;

            metrologyPointSize = p.metrologyPointSize;
            valueDQIyChartData = p.valueDQIyChartData;
        }
    }

    public class MCBuildConjectureModelLocalContainer
    {
        public string XTableName = string.Empty;
        public string YTableName = string.Empty;

        public Chart ChartMetrology = null;
        public Chart ChartRI = null;
        public Chart ChartGSI = null;

        // Model資料
        public double[,] AllContextID_List;  //List all CONTEXTID
        public int pointSize = 0;  //Create Conjecture Model Tab
        
        //NN Result
        public bool AlgoValueBPNN = false;
        public double[,] predictBPNN;  //List all predict value of BPNN
        public double[,] mapeNN;   //List all MAPE value of BPNN
        public double[,] maxErrNN;   //List all MAX ERR value of BPNN

        //MR Result
        public bool AlgoValueMR = false;
        public double[,] predictMR;  //List all predict value of MR
        public double[,] mapeMR;   //List all MAPE value of MR
        public double[,] maxErrMR;   //List all MAX ERR value of MR

        //Y Result
        public double[,] valueY;   //List all Y value

        //RI Result
        public double[,] valueRI;  //Value of RI
        public double[,] thresholdRI;  //threshold of RI

        //GSI Result
        public double[,] valueGSI;  //Value of GSI
        public double[,] thresholdGSI;  //threshold of GSI
        public double[,] ContextID_ListGSI;  //Value of ContextID_List

        public void Clear() 
        {
            ChartMetrology = null;
            ChartRI = null;
            ChartGSI = null;
            // Model資料
            AllContextID_List = null;
            pointSize = 0;  //Create Conjecture Model Tab

            //NN Result
            AlgoValueBPNN = false;
            predictBPNN = null;
            mapeNN = null;
            maxErrNN = null;

            //MR Result
            AlgoValueMR = false;
            predictMR = null;
            mapeMR = null;
            maxErrMR = null;

            //Y Result
            valueY = null;

            //RI Result
            valueRI = null;
            thresholdRI = null;

            //GSI Result
            valueGSI = null;
            thresholdGSI = null;
            ContextID_ListGSI = null;
        } 
    }
}
