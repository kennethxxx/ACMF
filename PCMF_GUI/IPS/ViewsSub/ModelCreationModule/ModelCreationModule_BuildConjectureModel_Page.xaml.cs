using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using System.Linq;
using System.Net;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using IPS.Common;
using IPS.ModelCreation;
using Visifire.Charts;
using Serialization;
using IPS.ModelManager;


namespace IPS.ViewsSub.ModelCreationModule
{
	public partial class ModelCreationModule_BuildConjectureModel_Page : UserControl
	{
        // Define Local Container
        MCBuildConjectureModelLocalContainer pBM_LocalContainer;
        MCBuildConjectureModelLocalContainer pBM_LocalContainerTemp;

        // Define Global Container
        MCDataCollectionGlobalContainer pDC_GlobalContainer = null;
        MCSetGroupGlobalContainer pSG_GlobalContainer = null;
        MCAbnormalIsolatedGlobalContainer pAbIso_GlobalContainer = null;

        // Define Global Parameter 
        int ChartSize_Height = 200;
        int ChartSize_Width = 735;
        Common.VisifireChart utilityChart = new Common.VisifireChart();

        // Define Global Event
        public event EventHandler FinishBuildModel;
        public event EventHandler FailBuildModel;


        public ModelCreationModule_BuildConjectureModel_Page()
		{
			// 必須將變數初始化
			InitializeComponent();
            pBM_LocalContainer = new MCBuildConjectureModelLocalContainer();
		}

        #region Init
        public void BindingContainer(
            MCDataCollectionGlobalContainer pDC,
            MCSetGroupGlobalContainer pSG,
            MCAbnormalIsolatedGlobalContainer pAbIso)
        {
            pDC_GlobalContainer = pDC;
            pSG_GlobalContainer = pSG;
            pAbIso_GlobalContainer = pAbIso;
        }

        


        public void InitionPage()
        {
            pBM_LocalContainer.Clear();

            ui_BM_Phase.ItemsSource = null;
            ui_BM_Phase.IsEnabled = false;
            ui_BM_Measure.ItemsSource = null;
            ui_BM_Measure.IsEnabled = false;
            ui_BM_Point.ItemsSource = null;
            ui_BM_Point.IsEnabled = false;

            ui_BM_MapeNNValue.Text = "?";
            ui_BM_MaxErrNNValue.Text = "?";
            ui_BM_MapeMRValue.Text = "?";
            ui_BM_MaxErrMRValue.Text = "?";

            ui_BM_CheckNN.IsChecked = true;
            ui_BM_CheckMR.IsChecked = true;
            ui_BM_CheckLight.IsChecked = true;

            Chart ChartTemp1 = new Chart();
            Chart ChartTemp2 = new Chart();
            Chart ChartTemp3 = new Chart();
            utilityChart.CreateChart(ChartTemp1, "Metrology Chart: ?", ChartSize_Width, ChartSize_Height -20, 10, 2, ui_BM_BuildMetrologyChart, 0.25);
            utilityChart.CreateChart(ChartTemp2, "RI Chart: ?", ChartSize_Width, ChartSize_Height - 20, 10, 2, ui_BM_RIChart, 0.25);
            utilityChart.CreateChart(ChartTemp3, "GSI Chart: ?", ChartSize_Width, ChartSize_Height - 20, 10, 2, ui_BM_GSIChart, 0.25);
            ui_PhaseContentHheader.Header = "??";

            ui_BM_SaveModel.IsEnabled = false;
            ui_BM_SaveModel.Visibility = Visibility.Collapsed;

            ui_BM_BuildModel.IsEnabled = true;
        }
        public void DestroyPage()
        {
            pBM_LocalContainer.Clear();

            ui_BM_Phase.ItemsSource = null;
            ui_BM_Phase.IsEnabled = false;
            ui_BM_Measure.ItemsSource = null;
            ui_BM_Measure.IsEnabled = false;
            ui_BM_Point.ItemsSource = null;
            ui_BM_Point.IsEnabled = false;

            ui_BM_MapeNNValue.Text = "?";
            ui_BM_MaxErrNNValue.Text = "?";
            ui_BM_MapeMRValue.Text = "?";
            ui_BM_MaxErrMRValue.Text = "?";

            ui_BM_CheckNN.IsChecked = true;
            ui_BM_CheckMR.IsChecked = true;
            ui_BM_CheckLight.IsChecked = true;

            Chart ChartTemp1 = new Chart();
            Chart ChartTemp2 = new Chart();
            Chart ChartTemp3 = new Chart();
            utilityChart.CreateChart(ChartTemp1, "Metrology Chart: ?", ChartSize_Width, ChartSize_Height - 20, 10, 2, ui_BM_BuildMetrologyChart, 0.25);
            utilityChart.CreateChart(ChartTemp2, "RI Chart: ?", ChartSize_Width, ChartSize_Height - 20, 10, 2, ui_BM_RIChart, 0.25);
            utilityChart.CreateChart(ChartTemp3, "GSI Chart: ?", ChartSize_Width, ChartSize_Height - 20, 10, 2, ui_BM_GSIChart, 0.25);
            ui_PhaseContentHheader.Header = "??";

            ui_BM_SaveModel.IsEnabled = false;
            ui_BM_SaveModel.Visibility = Visibility.Collapsed;

            ui_BM_BuildModel.IsEnabled = false;
        }
        #endregion

        #region Build Option
        private void hyperLinkOptions_Click(object sender, RoutedEventArgs e)
        {
            ModelCreationBuildOption buildOption = new ModelCreationBuildOption();
            //fix bug of silverlight 4 with childwindows ->> disable parent
            buildOption.Closed += (s, eargs) =>
            {
                Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
            };
            buildOption.Show();
        }
        #endregion

        #region Build Model
        private void ui_BM_BuildModel_Click(object sender, RoutedEventArgs e)
        {
            if (pAbIso_GlobalContainer.IsChangeAbIsoValue)
            {
               // MessageBox.Show("Oops... You need return to last step that you change Isolate or Abnormal selection to verify again!");
                MessageBox.Show("真糟糕... 你必須回上一步改變所選的 Isolate 或 Abnormal 然後再驗證一次!");
                return;
            }
            pBM_LocalContainerTemp = new MCBuildConjectureModelLocalContainer();

            
            ExecuteBPNNModule();
            
        }

        

        void ExecuteBPNNModule()
        {
            

            ModelCreationModuleAlgorithmFlowControler MCAlgrControler = new ModelCreationModuleAlgorithmFlowControler(this.Dispatcher);
            MCAlgrControler.ExecuteBPNNModule_Fail += new EventHandler(ExecuteBPNNModule_Fail);
            MCAlgrControler.ExecuteBPNNModule_Finish += new EventHandler(ExecuteBPNNModule_Finish);

           // Shell.waitingForm.SettingMessage("Execute Module[BPNN]: ");
            Shell.waitingForm.SettingMessage("執行模組[BPNN]: ");
            Shell.waitingForm.SettingTip("");
            Shell.waitingForm.Show();
            MCAlgrControler.ExecuteBPNNModule(pDC_GlobalContainer.Company, pDC_GlobalContainer.LoginUsername);
        }
        void ExecuteBPNNModule_Fail(object sender, EventArgs e)
        {
            // 停留在原地
            ui_BM_SaveModel.IsEnabled = false;
            ui_BM_SaveModel.Visibility = Visibility.Collapsed;

            Shell.waitingForm.Close();
        }
        void ExecuteBPNNModule_Finish(object sender, EventArgs e)
        {
            bool IsSuccess = false;
            try
            {
                Out_BPNN listResult = ((Get_BPNNResultCompletedEventArgs)e).Result;
                if (0 == listResult.listCOM_Ack[0][0])
                {
                    pBM_LocalContainerTemp.AllContextID_List = utilityChart.ConvertToRealPredictData(listResult.listOutAll_ContextID.ToArray());
                    pBM_LocalContainerTemp.pointSize = listResult.listOutAll_PredictValue.ToArray().Length;

                    pBM_LocalContainerTemp.AlgoValueBPNN = listResult.AlgoValue;
                    pBM_LocalContainerTemp.predictBPNN = utilityChart.ConvertToRealPredictData(listResult.listOutAll_PredictValue.ToArray());    //predict value
                    pBM_LocalContainerTemp.mapeNN = utilityChart.ConvertToRealPredictData(listResult.listOutAll_MAPE.ToArray(), pBM_LocalContainerTemp.pointSize, CommonValue.NUMBER_OF_PHASE);
                    pBM_LocalContainerTemp.maxErrNN = utilityChart.ConvertToRealPredictData(listResult.listOutAll_MaxError.ToArray(), pBM_LocalContainerTemp.pointSize, CommonValue.NUMBER_OF_PHASE);

                    //Y Result ---------------
                    pBM_LocalContainerTemp.valueY = GetRealYValue(
                    pAbIso_GlobalContainer.checkboxesIsolatedStatus,
                    pAbIso_GlobalContainer.TrainingCount,
                    pAbIso_GlobalContainer.RunningCount,
                    pAbIso_GlobalContainer.iCurrentIUnIsolatedTrainCount,
                    pAbIso_GlobalContainer.iCurrentUnIsolatedRunCount);

                    IsSuccess = true;
                }
                else if (1 == listResult.listCOM_Ack[0][0])
                {
                    //MessageBox.Show("Error: Set BPNN Module Parameter fail.");
                    MessageBox.Show("錯誤訊息: 設定 BPNN 模組參數失敗.");
                    IsSuccess = false;
                }
                else
                {
                  //  MessageBox.Show("Error: Execute BPNN Module fail.");
                    MessageBox.Show("錯誤訊息: 執行 BPNN 模組失敗.");
                    IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Some Error was Happen in BPNN Module: " + ex.ToString());
                MessageBox.Show("BPNN 模組發生一些錯誤: " + ex.ToString());
                IsSuccess = false;
            }

            if (IsSuccess)
            {
                Shell.waitingForm.Close();
                ExecuteMRModule();
            }
            else
            {
                // 停留在原地
                ui_BM_SaveModel.IsEnabled = false;
                ui_BM_SaveModel.Visibility = Visibility.Collapsed;

                Shell.waitingForm.Close();
            }
        }

        void ExecuteMRModule()
        {
            ModelCreationModuleAlgorithmFlowControler MCAlgrControler = new ModelCreationModuleAlgorithmFlowControler(this.Dispatcher);
            MCAlgrControler.ExecuteMRModule_Fail += new EventHandler(ExecuteMRModule_Fail);
            MCAlgrControler.ExecuteMRModule_Finish += new EventHandler(ExecuteMRModule_Finish);

            //Shell.waitingForm.SettingMessage("Execute Module[MR]: ");
            Shell.waitingForm.SettingMessage("執行模組[MR]: ");
            Shell.waitingForm.SettingTip("");
            Shell.waitingForm.Show();
            //MCAlgrControler.ExecuteBPNNModule(pDC_GlobalContainer.Company, pDC_GlobalContainer.LoginUsername);
            MCAlgrControler.ExecuteMRModule(pDC_GlobalContainer.Company, pDC_GlobalContainer.LoginUsername);
        }
        void ExecuteMRModule_Fail(object sender, EventArgs e)
        {
            // 停留在原地
            ui_BM_SaveModel.IsEnabled = false;
            ui_BM_SaveModel.Visibility = Visibility.Collapsed;

            Shell.waitingForm.Close();
        }
        void ExecuteMRModule_Finish(object sender, EventArgs e)
        {
            bool IsSuccess = false;
            try
            {
                Out_MR listResult = ((Get_MRResultCompletedEventArgs)e).Result;
                if (0 == listResult.listCOM_Ack[0][0])
                {
                    pBM_LocalContainerTemp.AlgoValueMR = listResult.AlgoValue;
                    pBM_LocalContainerTemp.predictMR = utilityChart.ConvertToRealPredictData(listResult.listOutAll_PredictValue.ToArray());  //predict value
                    pBM_LocalContainerTemp.mapeMR = utilityChart.ConvertToRealPredictData(listResult.listOutAll_MAPE.ToArray(), pBM_LocalContainerTemp.pointSize, CommonValue.NUMBER_OF_PHASE);
                    pBM_LocalContainerTemp.maxErrMR = utilityChart.ConvertToRealPredictData(listResult.listOutAll_MaxError.ToArray(), pBM_LocalContainerTemp.pointSize, CommonValue.NUMBER_OF_PHASE);

                    IsSuccess = true;
                }
                else if (1 == listResult.listCOM_Ack[0][0])
                {
                    //MessageBox.Show("Error: Building Conjecture Model: Setting MR Module Parameter fail.");
                    MessageBox.Show("錯誤訊息: 建立預測模型: 設定 MR 模組參數失敗.");
                    IsSuccess = false;
                }
                else
                {
                    //MessageBox.Show("Error: Building Conjecture Model: Execute MR Module fail.");
                    MessageBox.Show("錯誤訊息: 建立預測模型: 執行 MR 模組失敗.");
                    IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Some Error was Happen in MR Module: " + ex.ToString());
                MessageBox.Show("MR 模組發生一些錯誤: " + ex.ToString());
                IsSuccess = false;
            }

            if (IsSuccess)
            {
                Shell.waitingForm.Close();
                ExecuteRIModule();
            }
            else
            {
                // 停留在原地
                ui_BM_SaveModel.IsEnabled = false;
                ui_BM_SaveModel.Visibility = Visibility.Collapsed;

                Shell.waitingForm.Close();
            }
        }

        void ExecuteRIModule()
        {
            ModelCreationModuleAlgorithmFlowControler MCAlgrControler = new ModelCreationModuleAlgorithmFlowControler(this.Dispatcher);
            MCAlgrControler.ExecuteRIModule_Fail += new EventHandler(ExecuteRIModule_Fail);
            MCAlgrControler.ExecuteRIModule_Finish += new EventHandler(ExecuteRIModule_Finish);

            //Shell.waitingForm.SettingMessage("Execute Module[RI]: ");
            Shell.waitingForm.SettingMessage("執行模組[RI]: ");
            Shell.waitingForm.SettingTip("");
            Shell.waitingForm.Show();
            MCAlgrControler.ExecuteRIModule(
                pBM_LocalContainerTemp.AlgoValueMR, "MRConjectureHistory", utilityChart.ConvertdoubleToObservableCollection(pBM_LocalContainerTemp.predictMR),
                pBM_LocalContainerTemp.AlgoValueBPNN, "BPNNConjectureHistory", utilityChart.ConvertdoubleToObservableCollection(pBM_LocalContainerTemp.predictBPNN),
                pDC_GlobalContainer.Company, pDC_GlobalContainer.LoginUsername);
        }
        void ExecuteRIModule_Fail(object sender, EventArgs e)
        {
            // 停留在原地
            ui_BM_SaveModel.IsEnabled = false;
            ui_BM_SaveModel.Visibility = Visibility.Collapsed;

            Shell.waitingForm.Close();
        }
        void ExecuteRIModule_Finish(object sender, EventArgs e)
        {
            bool IsSuccess = false;
            try
            {
                Out_RI listResult = ((Get_RIResultCompletedEventArgs)e).Result;
                if (0 == listResult.listCOM_Ack[0][0])
                {
                    pBM_LocalContainerTemp.valueRI = utilityChart.ConvertToRealPredictData(listResult.listOutRI_Value.ToArray());    //RI value
                    pBM_LocalContainerTemp.thresholdRI = utilityChart.ConvertToRealPredictData(listResult.listOutRI_Threshold.ToArray());    //Threshold RI value

                    IsSuccess = true;
                }
                else if (1 == listResult.listCOM_Ack[0][0])
                {
                   // MessageBox.Show("Error: Building Conjecture Model: Setting RI Module Parameter fail.");
                    MessageBox.Show("錯誤訊息: 建立預測模型: 設定 RI 模組參數失敗.");
                    IsSuccess = false;
                }
                else if (2 == listResult.listCOM_Ack[0][0])
                {
                   // MessageBox.Show("Error: Building Conjecture Model: Execute RI Module Fail");
                    MessageBox.Show("錯誤訊息: 建立預測模型: 執行 RI 模組失敗");
                    IsSuccess = false;
                }
                else
                {
                  //  MessageBox.Show("Error: Building Conjecture Model: Execute RI Module Fail throw from NN or MR.");
                    MessageBox.Show("錯誤訊息: 建立預測模型: 執行從 NN 或 MR 扔出 RI 模組失敗 .");
                    IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
               // MessageBox.Show("Some Error was Happen in RI Module: " + ex.ToString());
                MessageBox.Show("RI 模組發生一些錯誤: " + ex.ToString());
                IsSuccess = false;
            }

            if (IsSuccess)
            {
                Shell.waitingForm.Close();
                ExecuteGSIModule();
            }
            else
            {
                // 停留在原地
                ui_BM_SaveModel.IsEnabled = false;
                ui_BM_SaveModel.Visibility = Visibility.Collapsed;

                Shell.waitingForm.Close();
            }

        }

        void ExecuteGSIModule()
        {
            ModelCreationModuleAlgorithmFlowControler MCAlgrControler = new ModelCreationModuleAlgorithmFlowControler(this.Dispatcher);
            MCAlgrControler.ExecuteGSIModule_Fail += new EventHandler(ExecuteGSIModule_Fail);
            MCAlgrControler.ExecuteGSIModule_Finish += new EventHandler(ExecuteGSIModule_Finish);

            //Shell.waitingForm.SettingMessage("Execute Module[GSI]: ");
            Shell.waitingForm.SettingMessage("執行模組[GSI]: ");
            Shell.waitingForm.SettingTip("");
            Shell.waitingForm.Show();
            MCAlgrControler.ExecuteGSIModule(
                pSG_GlobalContainer.groupList.Count,
                "MahalanobisDistance",
                pDC_GlobalContainer.Company, pDC_GlobalContainer.LoginUsername);
        }
        void ExecuteGSIModule_Fail(object sender, EventArgs e)
        {
            // 停留在原地
            ui_BM_SaveModel.IsEnabled = false;
            ui_BM_SaveModel.Visibility = Visibility.Collapsed;

            Shell.waitingForm.Close();
        }
        void ExecuteGSIModule_Finish(object sender, EventArgs e)
        {
            bool IsSuccess = false;
            try
            {
                Out_GSI listResult = ((Get_GSIResultCompletedEventArgs)e).Result;
                if (0 == listResult.listCOM_Ack[0][0])
                {
                    pBM_LocalContainerTemp.valueGSI = utilityChart.ConvertToRealPredictData(listResult.listOUTGSIvalue.ToArray());
                    pBM_LocalContainerTemp.thresholdGSI = utilityChart.ConvertToRealPredictData(listResult.listOUTGSI_RT.ToArray());
                    pBM_LocalContainerTemp.ContextID_ListGSI = utilityChart.ConvertToRealPredictData(listResult.listOUTcontextID.ToArray());

                    IsSuccess = true;
                }
                else if (1 == listResult.listCOM_Ack[0][0])
                {
                   // MessageBox.Show("Error: Building Conjecture Model: Setting GSI Module Parameter fail.");
                    MessageBox.Show("錯誤訊息: 建立預測模型: 設定 GSI 模組參數失敗.");
                    IsSuccess = false;
                }
                else
                {
                    //MessageBox.Show("Error: Building Conjecture Model: Execute GSI Module fail.");
                    MessageBox.Show("錯誤訊息: 建立預測模型: 執行 GSI 模組失敗.");
                    IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Some Error was Happen in GSI Module: " + ex.ToString());
                MessageBox.Show(" GSI Module 發生一些錯誤: " + ex.ToString());
                IsSuccess = false;
            }

            

            if (IsSuccess)
            {
                // 複製資料 from Temp 到 Local 
                pBM_LocalContainer.AllContextID_List = pBM_LocalContainerTemp.AllContextID_List;
                pBM_LocalContainer.pointSize = pBM_LocalContainerTemp.pointSize;
                pBM_LocalContainer.AlgoValueBPNN = pBM_LocalContainerTemp.AlgoValueBPNN;
                pBM_LocalContainer.predictBPNN = pBM_LocalContainerTemp.predictBPNN;
                pBM_LocalContainer.mapeNN = pBM_LocalContainerTemp.mapeNN;
                pBM_LocalContainer.maxErrNN = pBM_LocalContainerTemp.maxErrNN;
                pBM_LocalContainer.valueY = pBM_LocalContainerTemp.valueY;

                pBM_LocalContainer.AlgoValueMR = pBM_LocalContainerTemp.AlgoValueMR;
                pBM_LocalContainer.predictMR = pBM_LocalContainerTemp.predictMR;
                pBM_LocalContainer.mapeMR = pBM_LocalContainerTemp.mapeMR;
                pBM_LocalContainer.maxErrMR = pBM_LocalContainerTemp.maxErrMR;

                pBM_LocalContainer.valueRI = pBM_LocalContainerTemp.valueRI;
                pBM_LocalContainer.thresholdRI = pBM_LocalContainerTemp.thresholdRI;

                pBM_LocalContainer.valueGSI = pBM_LocalContainerTemp.valueGSI;
                pBM_LocalContainer.thresholdGSI = pBM_LocalContainerTemp.thresholdGSI;
                pBM_LocalContainer.ContextID_ListGSI = pBM_LocalContainerTemp.ContextID_ListGSI;

                // 綁定BM_Point 展示第一個Point
                ui_BM_Phase.IsEnabled = true;
                ui_BM_Measure.IsEnabled = true;
                ui_BM_Point.IsEnabled = true;

                LoadComboBox(pSG_GlobalContainer.listPointCombo, ui_BM_Point);
 
                LoadPhaseList(ui_BM_Phase);

                if (ui_BM_Point.ItemsSource != null)
                {
                    ui_BM_Point.SelectedIndex = 0;
                }

                ui_BM_BuildModel.IsEnabled = false;

                
                MessageBox.Show("建立預測模型成功.");

                Shell.waitingForm.Close();

                // 成功後即可上傳Model
                ui_BM_SaveModel.IsEnabled = true;
                ui_BM_SaveModel.Visibility = Visibility.Visible;
            }
            else
            {
                // 停留在原地
                ui_BM_SaveModel.IsEnabled = false;
                ui_BM_SaveModel.Visibility = Visibility.Collapsed;

                Shell.waitingForm.Close();
            }

        }
        
       


        #endregion

        #region Show Phase and Point
        private void ui_BM_Phase_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ui_BM_Phase.ItemsSource != null)
            {
                if (ui_BM_Point.ItemsSource != null)
                {
                    if (ui_BM_Point.SelectedIndex == -1)
                    {
                        ui_BM_Point.SelectedIndex = 0;
                    }
                    else
                    {
                        MAPEandMaxErr();
                        CreateBuildMetrologyChart();
                        CreateRIChart();
                        CreateGSIChart();
                        ZoomingMultiChart();
                    }
                }
            }
        }

        private void ui_BM_Point_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ui_BM_Point.ItemsSource != null)
            {
                if (ui_BM_Phase.ItemsSource != null)
                {
                    if (ui_BM_Phase.SelectedIndex == -1)
                    {
                        ui_BM_Phase.SelectedIndex = 0;
                    }
                    else
                    {
                        MAPEandMaxErr();
                        CreateBuildMetrologyChart();
                        CreateRIChart();
                        CreateGSIChart();
                        ZoomingMultiChart();
                    }
                }
            }
        }

        //更新MAPE Max ERROR值
        void MAPEandMaxErr()
        {
            ui_PhaseContentHheader.Header = CommonValue.phaseDic[Int32.Parse(ui_BM_Phase.SelectedValue.ToString())].ToString();

            ui_BM_MapeNNValue.Text = String.Format("{0:0.000}", Math.Round(pBM_LocalContainer.mapeNN[Int32.Parse(ui_BM_Point.SelectedValue.ToString()), Int32.Parse(ui_BM_Phase.SelectedValue.ToString()) - 1], 3));
            ui_BM_MapeMRValue.Text = String.Format("{0:0.000}", Math.Round(pBM_LocalContainer.mapeMR[Int32.Parse(ui_BM_Point.SelectedValue.ToString()), Int32.Parse(ui_BM_Phase.SelectedValue.ToString()) - 1], 3));
            ui_BM_MaxErrNNValue.Text = String.Format("{0:0.000}", Math.Round(pBM_LocalContainer.maxErrNN[Int32.Parse(ui_BM_Point.SelectedValue.ToString()), Int32.Parse(ui_BM_Phase.SelectedValue.ToString()) - 1], 3));
            ui_BM_MaxErrMRValue.Text = String.Format("{0:0.000}", Math.Round(pBM_LocalContainer.maxErrMR[Int32.Parse(ui_BM_Point.SelectedValue.ToString()), Int32.Parse(ui_BM_Phase.SelectedValue.ToString()) - 1], 3));
        }

        //畫Metrology圖
        public void CreateBuildMetrologyChart()
        {
            //preparing data for BPNN & MR
            int xNumber = getContextIDByPhase(Int32.Parse(ui_BM_Phase.SelectedValue.ToString()));

            //point
            int pointNumber = Int32.Parse(ui_BM_Point.SelectedValue.ToString());

            double[] bpNN = new double[xNumber];
            double[] MR = new double[xNumber];
            double[] yValue = new double[xNumber];
            double[] ContextIDList = new double[xNumber];

            int startPos = 0;
            int endPos = 0;

            // 更新train run片數 [12/1/2011 pili7545]
            getPositionValueOfPhase(Int32.Parse(ui_BM_Phase.SelectedValue.ToString()), pAbIso_GlobalContainer.iCurrentIUnIsolatedTrainCount, pAbIso_GlobalContainer.iCurrentUnIsolatedRunCount, ref startPos, ref endPos);

            int k = 0;
            for (int i = startPos; i <= endPos; i++)
            {
                bpNN[k] = pBM_LocalContainer.predictBPNN[pointNumber, i];
                MR[k] = pBM_LocalContainer.predictMR[pointNumber, i];
                yValue[k] = pBM_LocalContainer.valueY[pointNumber, i];
                ContextIDList[k] = pBM_LocalContainer.AllContextID_List[pointNumber, i];
                k++;
            }

            // Create a new instance of Chart
            Chart ChartTemp = new Chart();
            utilityChart.CreateChart(ChartTemp, "Metrology Chart: " + pSG_GlobalContainer.combinedPoint[pointNumber].Name, ChartSize_Width, ChartSize_Height -20, 1, 2, ui_BM_BuildMetrologyChart, 0.25);
            ChartTemp.AxesX[0].Interval = ComputeInterval((double)xNumber, 1, 20); //設定間距
            ChartTemp.Series.Add(utilityChart.CreateDataSeries(ContextIDList, bpNN, "NN", RenderAs.Line, Common.CommonValue.NN_COLOR));
            ChartTemp.Series.Add(utilityChart.CreateDataSeries(ContextIDList, MR, "MR", RenderAs.Line, Common.CommonValue.MR_COLOR));
            ChartTemp.Series.Add(utilityChart.CreateDataSeries(ContextIDList, yValue, "Metrology Data", RenderAs.Point, Common.CommonValue.YVALUE_COLOR));

            ChartTemp.Series[1].Enabled = (ui_BM_CheckNN.IsChecked.Value ? true : false);   //Enable or disable NN Chart 
            ChartTemp.Series[2].Enabled = (ui_BM_CheckMR.IsChecked.Value ? true : false);   //Enable or disable MR Chart

            pBM_LocalContainer.ChartMetrology = ChartTemp;
        }

        //畫RI圖
        public void CreateRIChart()
        {
            //preparing data for BPNN & MR
            int xNumber = getContextIDByPhase(Int32.Parse(ui_BM_Phase.SelectedValue.ToString()));

            //point
            int pointNumber = Int32.Parse(ui_BM_Point.SelectedValue.ToString());

            double[] RI = new double[xNumber];
            double[] RI_RT = new double[xNumber];
            double[] ContextIDList = new double[xNumber];

            int startPos = 0;
            int endPos = 0;

            getPositionValueOfPhase(Int32.Parse(ui_BM_Phase.SelectedValue.ToString()), pAbIso_GlobalContainer.iCurrentIUnIsolatedTrainCount, pAbIso_GlobalContainer.iCurrentUnIsolatedRunCount, ref startPos, ref endPos);

            int k = 0;
            for (int i = startPos; i <= endPos; i++)
            {
                RI[k] = pBM_LocalContainer.valueRI[pointNumber, i];
                RI_RT[k] = pBM_LocalContainer.thresholdRI[pointNumber, 0];
                ContextIDList[k] = pBM_LocalContainer.AllContextID_List[pointNumber, i];
                k++;
            }

            // Create a new instance of Chart
            Chart ChartTemp = new Chart();
            utilityChart.CreateChart(ChartTemp, "RI Chart: " + pSG_GlobalContainer.combinedPoint[pointNumber].Name, null, null, ChartSize_Width, ChartSize_Height -20, 1, 2, 0, 1, ui_BM_RIChart, 0.25);
            ChartTemp.AxesX[0].Interval = ComputeInterval((double)xNumber, 1, 20); //設定間距
            ChartTemp.Series.Add(utilityChart.CreateDataSeries(ContextIDList, RI, "RI", RenderAs.Line, Common.CommonValue.RI_GSI_COLOR));
            ChartTemp.Series.Add(utilityChart.CreateDataSeries(ContextIDList, RI_RT, "Threshold", RenderAs.Line, Common.CommonValue.THRESHOLD_COLOR));

            pBM_LocalContainer.ChartRI = ChartTemp;
        }

        // 畫GSI圖
        public void CreateGSIChart()
        {
            //preparing data for GSI
            int SelectedPhase = Int32.Parse(ui_BM_Phase.SelectedValue.ToString());
            int xNumber = getContextIDByPhase(SelectedPhase);
            int pointNumber = Int32.Parse(ui_BM_Point.SelectedValue.ToString()) + 1;
            String strPointName = ((ComboxDataObj)ui_BM_Point.SelectedItem).Name;

            int groupNumber = getGroupNumberByPointValue(strPointName, pSG_GlobalContainer.groupList);

            double[] GSI = null;
            double[] GSI_RT = new double[xNumber];
            double[] ContextIDList = null;
            if (SelectedPhase == 1 || SelectedPhase == 2)
            {
                GSI = new double[pAbIso_GlobalContainer.iCurrentIUnIsolatedTrainCount];
                ContextIDList = new double[pAbIso_GlobalContainer.iCurrentIUnIsolatedTrainCount];
            }
            else
            {
                GSI = new double[pAbIso_GlobalContainer.iCurrentUnIsolatedRunCount];
                ContextIDList = new double[pAbIso_GlobalContainer.iCurrentUnIsolatedRunCount];
            }

            int startPos = 0;
            int endPos = 0;

            getPositionValueOfGSIPhase(pSG_GlobalContainer.groupList.Count, groupNumber, SelectedPhase, pAbIso_GlobalContainer.iCurrentIUnIsolatedTrainCount, pAbIso_GlobalContainer.iCurrentUnIsolatedRunCount, ref startPos, ref endPos);

            int k = 0;
            for (int i = startPos; i <= endPos; i++)
            {
                GSI[k] = pBM_LocalContainer.valueGSI[0, i];
                ContextIDList[k] = pBM_LocalContainer.ContextID_ListGSI[0, i];
                GSI_RT[k] = pBM_LocalContainer.thresholdGSI[0, pointNumber - 1];
                k++;
            }

            // Create a new instance of Chart
            Chart ChartTemp = new Chart();
            utilityChart.CreateChart(ChartTemp, "GSI Chart: Group" + (groupNumber + 1), null, null, ChartSize_Width, ChartSize_Height -20, 1, 2, 0, 1, ui_BM_GSIChart, 0.25);
            ChartTemp.AxesX[0].Interval = ComputeInterval((double)ContextIDList.Length, 1, 20); //設定間距
            ChartTemp.Series.Add(utilityChart.CreateDataSeries(ContextIDList, GSI, "GSI", RenderAs.Line, Common.CommonValue.RI_GSI_COLOR));
            ChartTemp.Series.Add(utilityChart.CreateDataSeries(ContextIDList, GSI_RT, "Threshold", RenderAs.Line, Common.CommonValue.THRESHOLD_COLOR));
            pBM_LocalContainer.ChartGSI = ChartTemp;

        }

        // 綁定縮放事件
        void ZoomingMultiChart()
        {
            if (pBM_LocalContainer != null)
            {
                if (pBM_LocalContainer.ChartMetrology != null)
                {
                    pBM_LocalContainer.ChartMetrology.AxesX[0].OnZoom += new EventHandler<AxisZoomEventArgs>(MetrologyChart_OnZoom);
                    pBM_LocalContainer.ChartMetrology.AxesX[0].Scroll += new EventHandler<AxisScrollEventArgs>(MetrologyChart_Scroll);
                }

                if (pBM_LocalContainer.ChartGSI != null)
                {
                    pBM_LocalContainer.ChartGSI.AxesX[0].OnZoom += new EventHandler<AxisZoomEventArgs>(GSIChart_OnZoom);
                    pBM_LocalContainer.ChartGSI.AxesX[0].Scroll += new EventHandler<AxisScrollEventArgs>(GSIChart_Scroll);
                }

                if (pBM_LocalContainer.ChartRI != null)
                {
                    pBM_LocalContainer.ChartRI.AxesX[0].OnZoom += new EventHandler<AxisZoomEventArgs>(RIChart_OnZoom);
                    pBM_LocalContainer.ChartRI.AxesX[0].Scroll += new EventHandler<AxisScrollEventArgs>(RIChart_Scroll);
                }
            }
        }

        void MetrologyChart_Scroll(object sender, AxisScrollEventArgs e)
        {
            Axis axis = sender as Axis;
            pBM_LocalContainer.ChartGSI.AxesX[0].ScrollBarOffset = axis.ScrollBarOffset;
            pBM_LocalContainer.ChartRI.AxesX[0].ScrollBarOffset = axis.ScrollBarOffset;
        }

        void RIChart_Scroll(object sender, AxisScrollEventArgs e)
        {
            Axis axis = sender as Axis;
            pBM_LocalContainer.ChartMetrology.AxesX[0].ScrollBarOffset = axis.ScrollBarOffset;
            pBM_LocalContainer.ChartGSI.AxesX[0].ScrollBarOffset = axis.ScrollBarOffset;
        }

        void GSIChart_Scroll(object sender, AxisScrollEventArgs e)
        {
            Axis axis = sender as Axis;
            pBM_LocalContainer.ChartMetrology.AxesX[0].ScrollBarOffset = axis.ScrollBarOffset;
            pBM_LocalContainer.ChartRI.AxesX[0].ScrollBarOffset = axis.ScrollBarOffset;
        }

        void MetrologyChart_OnZoom(object sender, AxisZoomEventArgs e)
        {
            Axis axis = sender as Axis;
            axis.Interval = ComputeInterval((double)e.MaxValue, (double)e.MinValue, 20);
            pBM_LocalContainer.ChartGSI.AxesX[0].Interval = ComputeInterval((double)e.MaxValue, (double)e.MinValue, 20);
            pBM_LocalContainer.ChartGSI.AxesX[0].Zoom(e.MaxValue, e.MinValue);
            pBM_LocalContainer.ChartGSI.AxesX[0].ScrollBarOffset = axis.ScrollBarOffset;
            pBM_LocalContainer.ChartRI.AxesX[0].Interval = ComputeInterval((double)e.MaxValue, (double)e.MinValue, 20);
            pBM_LocalContainer.ChartRI.AxesX[0].Zoom(e.MaxValue, e.MinValue);
            pBM_LocalContainer.ChartRI.AxesX[0].ScrollBarOffset = axis.ScrollBarOffset;
        }

        void RIChart_OnZoom(object sender, AxisZoomEventArgs e)
        {
            Axis axis = sender as Axis;
            axis.Interval = ComputeInterval((double)e.MaxValue, (double)e.MinValue, 20);
            pBM_LocalContainer.ChartMetrology.AxesX[0].Interval = ComputeInterval((double)e.MaxValue, (double)e.MinValue, 20);
            pBM_LocalContainer.ChartMetrology.AxesX[0].Zoom(e.MaxValue, e.MinValue);
            pBM_LocalContainer.ChartMetrology.AxesX[0].ScrollBarOffset = axis.ScrollBarOffset;
            pBM_LocalContainer.ChartGSI.AxesX[0].Interval = ComputeInterval((double)e.MaxValue, (double)e.MinValue, 20);
            pBM_LocalContainer.ChartGSI.AxesX[0].Zoom(e.MaxValue, e.MinValue);
            pBM_LocalContainer.ChartGSI.AxesX[0].ScrollBarOffset = axis.ScrollBarOffset;
        }

        void GSIChart_OnZoom(object sender, AxisZoomEventArgs e)
        {
            Axis axis = sender as Axis;
            axis.Interval = ComputeInterval((double)e.MaxValue, (double)e.MinValue, 20);
            pBM_LocalContainer.ChartMetrology.AxesX[0].Interval = ComputeInterval((double)e.MaxValue, (double)e.MinValue, 20);
            pBM_LocalContainer.ChartMetrology.AxesX[0].Zoom(e.MaxValue, e.MinValue);
            pBM_LocalContainer.ChartMetrology.AxesX[0].ScrollBarOffset = axis.ScrollBarOffset;
            pBM_LocalContainer.ChartRI.AxesX[0].Interval = ComputeInterval((double)e.MaxValue, (double)e.MinValue, 20);
            pBM_LocalContainer.ChartRI.AxesX[0].Zoom(e.MaxValue, e.MinValue);
            pBM_LocalContainer.ChartRI.AxesX[0].ScrollBarOffset = axis.ScrollBarOffset;
        }

        private void ui_BM_CheckNN_Click(object sender, RoutedEventArgs e)
        {
            if (pBM_LocalContainer != null)
            {
                if (pBM_LocalContainer.ChartMetrology != null)
                {
                    pBM_LocalContainer.ChartMetrology.Series[1].Enabled = (ui_BM_CheckNN.IsChecked.Value ? true : false);
                }
            }
        }

        private void ui_BM_CheckMR_Click(object sender, RoutedEventArgs e)
        {
            if (pBM_LocalContainer != null)
            {
                if (pBM_LocalContainer.ChartMetrology != null)
                {
                    pBM_LocalContainer.ChartMetrology.Series[2].Enabled = (ui_BM_CheckMR.IsChecked.Value ? true : false);
                }
            }
        }

        // 秀全圖事件
        private void BM_ShowChartViewer(object sender, RoutedEventArgs e)
        {
            ModelCreationChartViewer ChartViewer = new ModelCreationChartViewer();
            ChartViewer.MoveBindChart(ui_BM_BuildMetrologyChart, ui_BM_RIChart, ui_BM_GSIChart, ChartSize_Width, ChartSize_Height -20);
            ChartViewer.Closed += (s, eargs) =>  //fix bug of silverlight 4 with childwindows ->> disable parent
            {
                Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
            };
            ChartViewer.Show();
        }

        #endregion
        
        #region Save Model
        //準備回存模型
        private void ui_BM_SaveModel_Click(object sender, RoutedEventArgs e)
        {
            if (pAbIso_GlobalContainer.IsChangeAbIsoValue)
            {
               // MessageBox.Show("Oops... You should return to last step that you change Isolate or Abnormal selection to verify again!");
                MessageBox.Show("真遭... 你必須回上一步改變所選的 Isolate 或 Abnormal 然後再驗證一次!");
                return;
            }

            ui_BM_SaveModel.IsEnabled = false;

           // Shell.waitingForm.SettingMessage("Store Model to Cloud Storage");
            Shell.waitingForm.SettingMessage("儲存模型到雲端");
            Shell.waitingForm.Show();

            //App.proxyMC.UploadModelCompleted += new EventHandler<UploadModelCompletedEventArgs>(UploadModelCompletedEvent);
            App.proxyMM.uploadModelCompleted += new EventHandler<uploadModelCompletedEventArgs>(UploadModelCompletedEvent);
            //App.proxyMC.UploadModelAsync(
            //    //DC_GlobalContainer

            //    pDC_GlobalContainer.ServiceBrokerID,
            //    pDC_GlobalContainer.ProductID,
            //    pDC_GlobalContainer.vMachine,
            //    pDC_GlobalContainer.CNCType,
            //    pDC_GlobalContainer.CNCnumber,
            //    //pDC_GlobalContainer.NCprogram,
            //    pDC_GlobalContainer.SearchStartTime,
            //    pDC_GlobalContainer.SearchEndTime,
            //    new In_UserInfo() { Company = pDC_GlobalContainer.Company, User = pDC_GlobalContainer.LoginUsername }
            //    );

            ModelStorationInformation MSI = new ModelStorationInformation();
              MSI.createUser=pDC_GlobalContainer.LoginUsername;
              MSI.COMPANY = pDC_GlobalContainer.Company;
            MSI.cnc_number = pDC_GlobalContainer.CNCnumber;
            MSI.CNCType = pDC_GlobalContainer.CNCType;
            MSI.createTime = DateTime.Now;
            MSI.dataStartTime = pDC_GlobalContainer.SearchStartTime;
            MSI.dataEndTime = pDC_GlobalContainer.SearchEndTime;
            MSI.vMachineID = pDC_GlobalContainer.vMachine;
            MSI.ProductID = pDC_GlobalContainer.ProductID;
            MSI.ServiceBrokerID = pDC_GlobalContainer.ServiceBrokerID;

            App.proxyMM.uploadModelAsync(
                MSI,
               pDC_GlobalContainer.Company+ "_"+pDC_GlobalContainer.LoginUsername 
                );
        }

        void UploadModelCompletedEvent(object sender, uploadModelCompletedEventArgs e)
        {
            bool IsSuccess = false;
            bool IsCanDoAgain = false;
            App.proxyMM.uploadModelCompleted -= new EventHandler<uploadModelCompletedEventArgs>(UploadModelCompletedEvent);
            
            try
            {
                //Out_UploadModel result = ((uploadModelCompletedEventArgs)e).Result;
                String result = ((uploadModelCompletedEventArgs)e).Result;
                if (result == "success")
                {
                    //MessageBox.Show("Store Model is Successful.");
                    MessageBox.Show("模型儲存成功.");
                    IsSuccess = true;
                }
                else
                {
                    //MessageBox.Show(result.ErrorMessage);
                    //IsCanDoAgain = result.CanDoAgain; // 雲端資料還在時 = true
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error: " + ex.ToString());
                MessageBox.Show("錯誤訊息: " + ex.ToString());
            }

            Shell.waitingForm.Close();

            if (IsSuccess)
            {
                if (FinishBuildModel != null) // 回到第一頁
                {
                    FinishBuildModel(null, new EventArgs());
                }
            }
            else if (IsCanDoAgain)
            {
                ui_BM_SaveModel.IsEnabled = true;
            }
            else 
            {
                if (FailBuildModel != null) // 回到第一頁
                {
                    FailBuildModel(null, new EventArgs());
                }
            }
            
        }
        #endregion

        #region Common Method
        double[,] GetRealYValue(List<string> lsIsolatedStatus, int iTotalTrains, int iTotalRuns, int iTrains, int iRuns)
        {
            double[,] returnYValue = new double[pSG_GlobalContainer.combinedPoint.Count, iTrains * 2 + iRuns * 3];

            for (int i = 0; i < pSG_GlobalContainer.combinedPoint.Count; i++)
            {
                int iReturnYValueIndex = 0;

                //Stage1 Real Y
                double[] point = pDC_GlobalContainer.listAllPoints[pSG_GlobalContainer.combinedPoint[i].Name].ListItemValue.ToArray();

                //Stage1 Real Y
                for (int j = 0; j < iTotalTrains; j++)
                {
                    if (lsIsolatedStatus[j] == "0")
                    {
                        returnYValue[i, iReturnYValueIndex] = point[j];
                        iReturnYValueIndex++;
                    }
                }

                //Stage2 Real Y
                for (int j = 0; j < iTotalTrains; j++)
                {
                    if (lsIsolatedStatus[j] == "0")
                    {
                        returnYValue[i, iReturnYValueIndex] = point[j];
                        iReturnYValueIndex++;
                    }
                }

                //Free run Real Y
                for (int j = 0; j < iTotalRuns; j++)
                {
                    if (lsIsolatedStatus[j + iTotalTrains] == "0")
                    {
                        returnYValue[i, iReturnYValueIndex] = point[j + iTotalTrains];
                        iReturnYValueIndex++;
                    }
                }

                //Phase1 Real Y
                for (int j = 0; j < iTotalRuns; j++)
                {
                    if (lsIsolatedStatus[j + iTotalTrains] == "0")
                    {
                        returnYValue[i, iReturnYValueIndex] = point[j + iTotalTrains];
                        iReturnYValueIndex++;
                    }
                }

                //Phase2 Real Y
                for (int j = 0; j < iTotalRuns; j++)
                {
                    if (lsIsolatedStatus[j + iTotalTrains] == "0")
                    {
                        returnYValue[i, iReturnYValueIndex] = point[j + iTotalTrains];
                        iReturnYValueIndex++;
                    }
                }
            }
            return returnYValue;
        }

        void LoadComboBox(List<ComboxDataObj> list, ComboBox cmbox)
        {
            cmbox.ItemsSource = list;
        }

        //讀取Phase資訊
        void LoadPhaseList(ComboBox ListItem)
        {
            ComboxDataObj combox;
            List<ComboxDataObj> list = new List<ComboxDataObj>();
            for (int i = 0; i < CommonValue.NUMBER_OF_PHASE; i++)
            {
                combox = new ComboxDataObj();
                combox.Name = CommonValue.phaseDic[i + 1].ToString();
                combox.Value = (i + 1);
                list.Add(combox);
            }
            ListItem.ItemsSource = list;
        }

        int ComputeInterval(double value1, double value2, int IntervalSize)
        {
            double difvalue = Math.Abs(value1 - value2);

            if (difvalue <= IntervalSize)
            {
                return 1;
            }
            else
            {
                return Convert.ToInt32(Math.Ceiling(difvalue / (double)IntervalSize) - 1);
            }
        }

        // Get Group number from value of a point
        int getGroupNumberByPointValue(String strPointName, ObservableCollection<Group> groups)
        {
            for (int i = 0; i < groups.Count; i++)
            {
                foreach (MetrologyPoint point in groups[i].PointList)
                {
                    if (point.Name == strPointName)
                        return i;
                }
            }
            return 0;
        }

        // adds group for computation...
        void getPositionValueOfGSIPhase(int groupTotal, int groupNumber, int phase, int trainValue, int runValue, ref int startPos, ref int endPos)
        {
            int phaseId = -1;   //no phase 1 or 2
            switch (phase)
            {
                case 1:     //"Stage1"
                case 2:     //"Stage2":
                    startPos = groupNumber * pAbIso_GlobalContainer.iCurrentIUnIsolatedTrainCount;
                    endPos = startPos + pAbIso_GlobalContainer.iCurrentIUnIsolatedTrainCount - 1;
                    break;
                case 3:     //"FreeRun":
                    startPos = groupTotal * pAbIso_GlobalContainer.iCurrentIUnIsolatedTrainCount;
                    startPos += groupNumber * pAbIso_GlobalContainer.iCurrentIUnIsolatedTrainCount;
                    endPos = startPos + pAbIso_GlobalContainer.iCurrentUnIsolatedRunCount - 1;
                    break;
                case 4:     //"Phase1":
                    phaseId = 0;
                    startPos = groupTotal * pAbIso_GlobalContainer.iCurrentIUnIsolatedTrainCount;
                    startPos += groupTotal * pAbIso_GlobalContainer.iCurrentUnIsolatedRunCount;
                    startPos += (phaseId + groupNumber) * pAbIso_GlobalContainer.iCurrentUnIsolatedRunCount;
                    startPos += groupNumber * pAbIso_GlobalContainer.iCurrentUnIsolatedRunCount;

                    endPos = startPos + pAbIso_GlobalContainer.iCurrentUnIsolatedRunCount - 1;
                    break;
                case 5:     //"Phase2":
                    phaseId = 1;
                    startPos = groupTotal * pAbIso_GlobalContainer.iCurrentIUnIsolatedTrainCount;
                    startPos += groupTotal * pAbIso_GlobalContainer.iCurrentUnIsolatedRunCount;
                    startPos += (phaseId + groupNumber) * pAbIso_GlobalContainer.iCurrentUnIsolatedRunCount;
                    startPos += groupNumber * pAbIso_GlobalContainer.iCurrentUnIsolatedRunCount;

                    endPos = startPos + pAbIso_GlobalContainer.iCurrentUnIsolatedRunCount - 1;
                    break;
            }
        }

        // Get position values in array based on phase, trainValue, runValue for BPNN chart
        void getPositionValueOfPhase(int phase, int iCurrentIsolatedTrainCount, int iCurrentIsolatedRunCount, ref int startPos, ref int endPos)
        {
            switch (phase)
            {
                case 1:     //"Stage1"
                    startPos = 0;
                    endPos = iCurrentIsolatedTrainCount - 1;
                    break;
                case 2:     //"Stage2":
                    startPos = iCurrentIsolatedTrainCount;
                    endPos = iCurrentIsolatedTrainCount * 2 - 1;
                    break;
                case 3:     //"FreeRun":
                    startPos = iCurrentIsolatedTrainCount * 2;
                    endPos = iCurrentIsolatedTrainCount * 2 + iCurrentIsolatedRunCount - 1;
                    break;
                case 4:     //"Phase1":
                    startPos = iCurrentIsolatedTrainCount * 2 + iCurrentIsolatedRunCount;
                    endPos = iCurrentIsolatedTrainCount * 2 + iCurrentIsolatedRunCount * 2 - 1;
                    break;
                case 5:     //"Phase2":
                    startPos = iCurrentIsolatedTrainCount * 2 + iCurrentIsolatedRunCount * 2;
                    endPos = iCurrentIsolatedTrainCount * 2 + iCurrentIsolatedRunCount * 3 - 1;
                    break;
            }
        }

        // Get total number of contextID based on selected phase
        int getContextIDByPhase(int phase)
        {
            int contextID = 0;

            if (phase == 1 || phase == 2)   //"Stage1" || "Stage2"
            {
                //contextID = trainValue;
                contextID = pAbIso_GlobalContainer.iCurrentIUnIsolatedTrainCount;
            }
            else
            {
                //contextID = runValue;
                contextID = pAbIso_GlobalContainer.iCurrentUnIsolatedRunCount;
            }
            return contextID;
        }

        #endregion




        

        
	}
}