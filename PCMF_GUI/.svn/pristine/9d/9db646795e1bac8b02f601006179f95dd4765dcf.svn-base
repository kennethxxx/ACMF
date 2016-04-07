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


namespace IPS.ViewsSub.ModelCreationModule
{
	public partial class ModelCreationModule_VerifyDQIy_Page : UserControl
	{
        // Define Local Container
        MCVerifyDQIyLocalContainer pVY_LocalContainer = null;

        // Define Global Container
        MCDataCollectionGlobalContainer     pDC_GlobalContainer = null;
        MCDataSelectionGlobalContainer      pDS_GlobalContainer = null;
        MCSetGroupGlobalContainer           pSG_GlobalContainer = null;
        MCVerifyDQIxGlobalContainer         pVX_GlobalContainer = null;
        MCVerifyDQIyGlobalContainer         pVY_GlobalContainer = null;
        MCAbnormalIsolatedGlobalContainer   pAbIso_GlobalContainer = null;

        // Define Global Parameter 
        int ChartSize_Height = 180;
        int ChartSize_Width = 735;
        Common.VisifireChart utilityChart = new Common.VisifireChart();

        // Define Global Event
        public event EventHandler ChangeToNextStep;
        public event EventHandler DestroyNextStep;

		public ModelCreationModule_VerifyDQIy_Page()
		{
			// 必須將變數初始化
			InitializeComponent();
            pVY_LocalContainer = new MCVerifyDQIyLocalContainer();
		}

        #region Init
        public void BindingContainer(
            MCDataCollectionGlobalContainer pDC,
            MCDataSelectionGlobalContainer pDS,
            MCSetGroupGlobalContainer pSG,
            MCVerifyDQIxGlobalContainer pVX,
            MCVerifyDQIyGlobalContainer pVY,
            MCAbnormalIsolatedGlobalContainer pAbIso
            )
        {
            pDC_GlobalContainer = pDC;
            pDS_GlobalContainer = pDS;
            pSG_GlobalContainer = pSG;
            pVX_GlobalContainer = pVX;
            pVY_GlobalContainer = pVY;
            pAbIso_GlobalContainer = pAbIso;
        }
        public void InitionPage()
        {
            pVY_LocalContainer.Clear();

            ui_VY_Vigilance.Value =     pVY_LocalContainer.Init_Vigilance;
            ui_VY_Threadhold.Value =    pVY_LocalContainer.Init_Threadhold;
            ui_VY_Accuracy.Value =      pVY_LocalContainer.Init_Accuracy;

            ui_VY_MetrologyPointList.ItemsSource = null;

            Chart ChartTemp1 = new Chart();
            Chart ChartTemp2 = new Chart();
            utilityChart.CreateChart(ChartTemp1, "DQIy Chart: ?", ChartSize_Width, ChartSize_Height, 10, 1, ui_VY_DQIYChart, 0.25);
            utilityChart.CreateChart(ChartTemp2, "Metrology Chart: ?", ChartSize_Width, ChartSize_Height, 10, 1, ui_VY_MetrologyDQIYChart, 0.25);

            ui_VY_AbIsoDataGrid.ItemsSource = pAbIso_GlobalContainer.contextList;

            ui_VY_VerifyDQIy.IsEnabled = true;
            ui_VY_NextStep.IsEnabled = false;
        }
        public void DestroyPage()
        {
            // 呼叫清空NextStep Tab動作
            if (DestroyNextStep != null)
            {
                DestroyNextStep(null, new EventArgs());
            }

            pVY_LocalContainer.Clear();

            ui_VY_Vigilance.Value = 0.0;
            ui_VY_Threadhold.Value = 0.0;
            ui_VY_Accuracy.Value = 0;

            ui_VY_MetrologyPointList.ItemsSource = null;

            Chart ChartTemp1 = new Chart();
            Chart ChartTemp2 = new Chart();
            utilityChart.CreateChart(ChartTemp1, "DQIy Chart: ?", ChartSize_Width, ChartSize_Height, 10, 1, ui_VY_DQIYChart, 0.25);
            utilityChart.CreateChart(ChartTemp2, "Metrology Chart: ?", ChartSize_Width, ChartSize_Height, 10, 1, ui_VY_MetrologyDQIYChart, 0.25);

            ui_VY_AbIsoDataGrid.ItemsSource = null;

            ui_VY_VerifyDQIy.IsEnabled = false;
            ui_VY_NextStep.IsEnabled = false;
        }
        #endregion

        #region Verify DQIy
        private void ui_VY_VerifyDQIy_Click(object sender, RoutedEventArgs e)
        {
            pVY_LocalContainer.Vigilance = ui_VY_Vigilance.Value;   // ???
            pVY_LocalContainer.Threadhold = ui_VY_Threadhold.Value; // ???
            pVY_LocalContainer.Accuracy = 0;

            if (pAbIso_GlobalContainer.IsChangeAbIsoValue) // 沒有變動Isolation或 Abnormal的話 不用重新跑 KSS KVS 和VerifyDQIx
            {
                ExecuteDataTranferModule();
            }
            else
            {
                ExecuteDQIyModule_VerifyDQIy();
            }
        }

        void ExecuteDataTranferModule()
        {
            ModelCreationModuleAlgorithmFlowControler MCAlgrControler = new ModelCreationModuleAlgorithmFlowControler(this.Dispatcher);
            MCAlgrControler.ExecuteDataTranferModule_Fail += new EventHandler(ExecuteDataTranferModule_Fail);
            MCAlgrControler.ExecuteDataTranferModule_Finish += new EventHandler(ExecuteDataTranferModule_Finish);

           // Shell.waitingForm.SettingMessage("Execute Module[Data Transfer]: ");
            Shell.waitingForm.SettingMessage("執行模組[Data Transfer]: ");
            Shell.waitingForm.SettingTip("");
            Shell.waitingForm.Show();

            MCAlgrControler.ExecuteDataTranferModule(
                pDC_GlobalContainer.TotalcontextListCount,
                pDS_GlobalContainer.TrainingCount,
                pDS_GlobalContainer.RunningCount,
                pSG_GlobalContainer.SelectedIndicatorTypeList,
                pSG_GlobalContainer.SelectedMetrologyTypeList,
                pDC_GlobalContainer.SearchStartTime,
                pDC_GlobalContainer.SearchEndTime,
                pSG_GlobalContainer.combinedIndicator,
                pSG_GlobalContainer.combinedPoint,
                pSG_GlobalContainer.groupList,
                pDC_GlobalContainer.vMachine,
                pDC_GlobalContainer.CNCType,
                pDC_GlobalContainer.CNCnumber,
                pDC_GlobalContainer.NCprogram,
                pDC_GlobalContainer.model_Id,
                pDC_GlobalContainer.version,
                pDC_GlobalContainer.allAction,
                pAbIso_GlobalContainer.checkboxesAbnormalStatusOC,
                pAbIso_GlobalContainer.checkboxesIsolatedStatusOC,
                pDC_GlobalContainer.XTableName,
                pDC_GlobalContainer.YTableName,
                pDC_GlobalContainer.strSelectedContextID,
                pDC_GlobalContainer.ProductID,
                pDC_GlobalContainer.Company, pDC_GlobalContainer.LoginUsername
                );
        }
        void ExecuteDataTranferModule_Fail(object sender, EventArgs e)
        {
            // 停留在原地 並移除Next Step 不能進入下個Step 需再從跑DataTranfer 

            if (DestroyNextStep != null)
            {
                DestroyNextStep(null, new EventArgs());
            }

            pAbIso_GlobalContainer.IsChangeAbIsoValue = true;

            ui_VY_NextStep.IsEnabled = false;

            Shell.waitingForm.Close();
        }
        void ExecuteDataTranferModule_Finish(object sender, EventArgs e)
        {
            bool IsSuccess = false;
            try
            {
                int result = ((Get_DataTransferResultCompletedEventArgs)e).Result;

                if (result == 0)
                {
                    IsSuccess = true;
                }
                else
                {
                    //MessageBox.Show("Error: Data Transfer Settings are fail! Please try it again.");
                    MessageBox.Show("錯誤訊息: 資料轉換設定失敗! 請再試一次");
                    IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error: " + ex.ToString());
                MessageBox.Show("錯誤訊息: " + ex.ToString());
                IsSuccess = false;
            }

            if (IsSuccess)
            {
                Shell.waitingForm.Close();
                pAbIso_GlobalContainer.IsChangeAbIsoValue = false; //取消掉Abnormal 和 Isolate的變更旗標
                ExecuteKSSModule();
            }
            else
            {
                // 停留在原地 並移除Next Step 不能進入下個Step 需再從跑DataTranfer 
                if (DestroyNextStep != null)
                {
                    DestroyNextStep(null, new EventArgs());
                }
                pAbIso_GlobalContainer.IsChangeAbIsoValue = true;

                ui_VY_NextStep.IsEnabled = false;

                Shell.waitingForm.Close();
            }
        }

        void ExecuteKSSModule()
        {
            ModelCreationModuleAlgorithmFlowControler MCAlgrControler = new ModelCreationModuleAlgorithmFlowControler(this.Dispatcher);
            MCAlgrControler.ExecuteKSSModule_Fail += new EventHandler(ExecuteKSSModule_Fail);
            MCAlgrControler.ExecuteKSSModule_Finish += new EventHandler(ExecuteKSSModule_Finish);

            //Shell.waitingForm.SettingMessage("Execute Module[KSS{" + pVX_GlobalContainer.KSSType + "}]: ");
            Shell.waitingForm.SettingMessage("執行模組[KSS{" + pVX_GlobalContainer.KSSType + "}]: ");
            Shell.waitingForm.Show();
            MCAlgrControler.ExecuteKSSModule(pVX_GlobalContainer.KSSType, pVX_GlobalContainer.ClusterNumber, pDC_GlobalContainer.Company, pDC_GlobalContainer.LoginUsername);
        }
        void ExecuteKSSModule_Fail(object sender, EventArgs e)
        {
            // 停留在原地 並移除Next Step 不能進入下個Step
            if (DestroyNextStep != null)
            {
                DestroyNextStep(null, new EventArgs());
            }

            ui_VY_NextStep.IsEnabled = false;

            Shell.waitingForm.Close();
        }
        void ExecuteKSSModule_Finish(object sender, EventArgs e)
        {
            bool IsSuccess = false;
            try
            {
                Out_KSS list = ((Get_KSSResultCompletedEventArgs)e).Result;
                if (0 == list.listCOM_Ack[0][0])
                {
                    IsSuccess = true;
                }
                else if (1 == list.listCOM_Ack[0][0])
                {
                   // MessageBox.Show("Error: Set KSS Module Parameter fail.");
                    MessageBox.Show("錯誤訊息: 設定 KSS 模組失敗.");
                    IsSuccess = false;
                }
                else
                {
                    //MessageBox.Show("Error: Execute KSS Module fail.");
                    MessageBox.Show("錯誤訊息: 執行 KSS 模組失敗.");
                    IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error: " + ex.ToString());
                MessageBox.Show("錯誤訊息: " + ex.ToString());
                IsSuccess = false;
            }

            if (IsSuccess)
            {
                Shell.waitingForm.Close();
                ExecuteKVSModule();
            }
            else
            {
                // 停留在原地 並移除Next Step 不能進入下個Step
                if (DestroyNextStep != null)
                {
                    DestroyNextStep(null, new EventArgs());
                }

                ui_VY_NextStep.IsEnabled = false;

                Shell.waitingForm.Close();
            }
        }

        void ExecuteKVSModule()
        {
            ModelCreationModuleAlgorithmFlowControler MCAlgrControler = new ModelCreationModuleAlgorithmFlowControler(this.Dispatcher);
            MCAlgrControler.ExecuteKVSModule_Fail += new EventHandler(ExecuteKVSModule_Fail);
            MCAlgrControler.ExecuteKVSModule_Finish += new EventHandler(ExecuteKVSModule_Finish);

            //Shell.waitingForm.SettingMessage("Execute Module[KVS{" + pVX_GlobalContainer.KVSType + "}]: ");
            Shell.waitingForm.SettingMessage("執行模組[KVS{" + pVX_GlobalContainer.KVSType + "}]: ");
            Shell.waitingForm.SettingTip("KVS");
            Shell.waitingForm.Show();
            MCAlgrControler.ExecuteKVSModule(
                pVX_GlobalContainer.KVSType, 
                pVX_GlobalContainer.FinAlpha, 
                pVX_GlobalContainer.FoutAlpha, 
                pVX_GlobalContainer.VerificationMode, 
                pDC_GlobalContainer.Company, pDC_GlobalContainer.LoginUsername);
        }
        void ExecuteKVSModule_Fail(object sender, EventArgs e)
        {
            // 停留在原地 並移除Next Step 不能進入下個Step
            if (DestroyNextStep != null)
            {
                DestroyNextStep(null, new EventArgs());
            }

            ui_VY_NextStep.IsEnabled = false;

            Shell.waitingForm.Close();
        }
        void ExecuteKVSModule_Finish(object sender, EventArgs e)
        {
            bool IsSuccess = false;

            try
            {
                Out_KVS list = ((Get_KVSResultCompletedEventArgs)e).Result;
                if (0 == list.listCOM_Ack[0][0])
                {
                    IsSuccess = true;
                }
                else if (1 == list.listCOM_Ack[0][0])
                {
                    //MessageBox.Show("Error: Set KVS Parameter fail.");
                    MessageBox.Show("錯誤訊息: 設定 KVS 參數失敗.");
                    IsSuccess = false;
                }
                else
                {
                    //MessageBox.Show("Error: Execute KVS Module fail.");
                    MessageBox.Show("錯誤訊息: 執行 KVS 模組失敗.");
                    IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Some Error Happen in CallKVS: " + ex.ToString());
                MessageBox.Show("CallKVS 發生一些錯誤: " + ex.ToString());
                IsSuccess = false;
            }

            if (IsSuccess)
            {
                Shell.waitingForm.Close();
                ExecuteDQIxModule_VerifyDQIx();
            }
            else
            {
                // 停留在原地 並移除Next Step 不能進入下個Step
                if (DestroyNextStep != null)
                {
                    DestroyNextStep(null, new EventArgs());
                }

                ui_VY_NextStep.IsEnabled = false;

                Shell.waitingForm.Close();
            }
        }

        void ExecuteDQIxModule_VerifyDQIx()
        {
            ModelCreationModuleAlgorithmFlowControler MCAlgrControler = new ModelCreationModuleAlgorithmFlowControler(this.Dispatcher);
            MCAlgrControler.ExecuteDQIxModule_VerifyDQIx_Fail += new EventHandler(ExecuteDQIxModule_VerifyDQIx_Fail);
            MCAlgrControler.ExecuteDQIxModule_VerifyDQIx_Finish += new EventHandler(ExecuteDQIxModule_VerifyDQIx_Finish);

            //Shell.waitingForm.SettingMessage("Execute Module[DQIx(Verifying DQIx): ");
            //Shell.waitingForm.SettingTip("DQIx: Data Quality Index of X");
            Shell.waitingForm.SettingMessage("執行模組[DQIx(Verifying DQIx): ");
            Shell.waitingForm.SettingTip("DQIx: X 的資料品質指標");
            Shell.waitingForm.Show();
            MCAlgrControler.ExecuteDQIxModule_VerifyDQIx(
                pVX_GlobalContainer.Lambda, 
                pVX_GlobalContainer.Threshold, 
                pVX_GlobalContainer.FilterPercentage, 
                pVX_GlobalContainer.RefreshCounter, 
                pDC_GlobalContainer.Company, pDC_GlobalContainer.LoginUsername);
        }
        void ExecuteDQIxModule_VerifyDQIx_Fail(object sender, EventArgs e)
        {
            // 停留在原地 並移除Next Step 不能進入下個Step
            if (DestroyNextStep != null)
            {
                DestroyNextStep(null, new EventArgs());
            }

            ui_VY_NextStep.IsEnabled = false;

            Shell.waitingForm.Close();
        }
        void ExecuteDQIxModule_VerifyDQIx_Finish(object sender, EventArgs e)
        {
            bool IsSuccess = false;
            
            try
            {
                Out_VerifyDQIxResult Out_VerifyDQIxResult_List = ((Get_DQIxResult_VerifyDQIxCompletedEventArgs)e).Result;

                // 使用相同條件算出來的DQIx資料會相同，故不保留
                MCVerifyDQIxLocalContainer VX_LocalContainerTemp = new MCVerifyDQIxLocalContainer();
                if (0 == Out_VerifyDQIxResult_List.listCOM_Ack[0][0])
                {
                    VX_LocalContainerTemp.valueDQIxChartData = utilityChart.ConvertToRealPredictData(Out_VerifyDQIxResult_List.listDQIx.ToArray());
                    VX_LocalContainerTemp.valueThresholdDQIxChartData = utilityChart.ConvertToRealPredictData(Out_VerifyDQIxResult_List.listDQIxThreshold.ToArray());
                    VX_LocalContainerTemp.valueContextIDDQIxChartData = utilityChart.ConvertToRealPredictData(Out_VerifyDQIxResult_List.listContextID.ToArray());
                    IsSuccess = true;
                }
                else if (1 == Out_VerifyDQIxResult_List.listCOM_Ack[0][0])
                {
                    //MessageBox.Show("Error: Set DQIx Module - Verifying DQIx Parameter fail.");
                    MessageBox.Show("錯誤訊息: 設定 DQIx 模組 - 驗證 DQIx 參數失敗.");
                    IsSuccess = false;
                }
                else
                {
                    //MessageBox.Show("Error: Execute DQIx Module - Verifying DQIx fail.");
                    MessageBox.Show("錯誤訊息: 執行 DQIx 模組 - 驗證 DQIx 失敗.");
                    IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Some Error Happen in DQIx Module - Verifying DQIx: " + ex.ToString());
                MessageBox.Show("DQIx 模組發生一些錯誤 - 驗證 DQIx: " + ex.ToString());
                IsSuccess = false;
            }

            Shell.waitingForm.Close();

            if (IsSuccess)
            {
                Shell.waitingForm.Close();
                ExecuteDQIyModule_VerifyDQIy();
            }
            else
            {
                // 停留在原地 並移除Next Step 不能進入下個Step
                if (DestroyNextStep != null)
                {
                    DestroyNextStep(null, new EventArgs());
                }

                ui_VY_NextStep.IsEnabled = false;

                Shell.waitingForm.Close();
            }
        }

        void ExecuteDQIyModule_VerifyDQIy()
        {
            ModelCreationModuleAlgorithmFlowControler MCAlgrControler = new ModelCreationModuleAlgorithmFlowControler(this.Dispatcher);
            MCAlgrControler.ExecuteDQIyModule_VerifyDQIy_Fail += new EventHandler(ExecuteDQIyModule_VerifyDQIy_Fail);
            MCAlgrControler.ExecuteDQIyModule_VerifyDQIy_Finish += new EventHandler(ExecuteDQIyModule_VerifyDQIy_Finish);

            

            //Shell.waitingForm.SettingMessage("Execute Module[DQIy(Verifying DQIy)]: ");
            //Shell.waitingForm.SettingTip("DQIy: Data Quality Index of y");
            Shell.waitingForm.SettingMessage("執行模組[DQIy(Verifying DQIy)]: ");
            Shell.waitingForm.SettingTip("DQIy: y的資料品質指標");
            
            Shell.waitingForm.Show();
            MCAlgrControler.ExecuteDQIyModule_VerifyDQIy(pDC_GlobalContainer.Company, pDC_GlobalContainer.LoginUsername);
        }
        void ExecuteDQIyModule_VerifyDQIy_Fail(object sender, EventArgs e)
        {
            // 停留在原地 並移除Next Step 不能進入下個Step
            if (DestroyNextStep != null)
            {
                DestroyNextStep(null, new EventArgs());
            }

            ui_VY_NextStep.IsEnabled = false;

            Shell.waitingForm.Close();
        }
        void ExecuteDQIyModule_VerifyDQIy_Finish(object sender, EventArgs e)
        {
            bool IsSuccess = false;

            MCVerifyDQIyLocalContainer VY_LocalContainerTemp = new MCVerifyDQIyLocalContainer();
            try
            {
                Out_VerifyDQIyResult Out_VerifyDQIyResult_List = ((Get_DQIyResult_VerifyDQIyCompletedEventArgs)e).Result;
                if (0 == Out_VerifyDQIyResult_List.listCOM_Ack[0][0])
                {
                    ObservableCollection<double>[] listPointListTemp = Out_VerifyDQIyResult_List.listPointList.ToArray();  //point
                    VY_LocalContainerTemp.metrologyPointSize = listPointListTemp.Length;
                    VY_LocalContainerTemp.valueDQIyChartData = utilityChart.ConvertToRealPredictData(Out_VerifyDQIyResult_List.listDQIyResultChart.ToArray());
                    IsSuccess = true;
                }
                else if (1 == Out_VerifyDQIyResult_List.listCOM_Ack[0][0])
                {
                    //MessageBox.Show("Error: Set DQIy Module - Verifying DQIy Parameter fail.");
                    MessageBox.Show("錯誤訊息: 設定 DQIy 模組 - 驗證 DQIy 參數失敗.");
                    IsSuccess = false;
                }
                else
                {
                    //MessageBox.Show("Error: Execute DQIy Module - Verifying DQIy fail.");
                    MessageBox.Show("錯誤訊息: 執行 DQIy 模組 - 驗證 DQIy 失敗.");
                    IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Some Error Happen in DQIy Module - Verifying DQIy: " + ex.ToString());
                MessageBox.Show("DQIy 模組發生錯誤訊息- 驗證 DQIy: " + ex.ToString());
                IsSuccess = false;
            }

            if (IsSuccess)
            {
                // 清除Next Step
                if (DestroyNextStep != null)
                {
                    DestroyNextStep(null, new EventArgs());
                }

                // 複製資料 from Temp 到 Local 
                pVY_LocalContainer.metrologyPointSize = VY_LocalContainerTemp.metrologyPointSize;
                pVY_LocalContainer.valueDQIyChartData = VY_LocalContainerTemp.valueDQIyChartData;
                VY_LocalContainerTemp = null;

                // 複製資料 from Local 到 Global
                pVY_GlobalContainer.Copy(pVY_LocalContainer);

                // 綁定Group 展示第一個Group
                LoadComboBox(pSG_GlobalContainer.listPointCombo, ui_VY_MetrologyPointList);

                if (ui_VY_MetrologyPointList.ItemsSource != null)
                {
                    ui_VY_MetrologyPointList.SelectedIndex = 0;
                }

                Shell.waitingForm.Close();

                ui_VY_NextStep.IsEnabled = true;
            }
            else
            {
                // 停留在原地 並移除Next Step 不能進入下個Step
                if (DestroyNextStep != null)
                {
                    DestroyNextStep(null, new EventArgs());
                }

                ui_VY_NextStep.IsEnabled = false;

                Shell.waitingForm.Close();
            }
        }

        #endregion


        #region Show DQIy

        private void ui_VY_MetrologyPointList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ui_VY_MetrologyPointList.ItemsSource != null)
            {
                if (ui_VY_MetrologyPointList.SelectedIndex != -1)
                {
                    CreateDQIyChart();
                }
            }

        }

        void CreateDQIyChart()
        {
            //point id -> start from value 0
            int PointID = Int32.Parse(ui_VY_MetrologyPointList.SelectedValue.ToString());

            //contextID value
            double[] contextID = new double[pAbIso_GlobalContainer.iCurrentUnIsolatedRunCount];    //Verify DQIy Tab => DQIy Chart

            //value for DQIy chart
            double[] valueDQIy = new double[pAbIso_GlobalContainer.iCurrentUnIsolatedRunCount];    //Verify DQIy Tab => DQIy Chart
            double[] valueThresholdDQIy = new double[pAbIso_GlobalContainer.iCurrentUnIsolatedRunCount];   //Verify DQIy Tab => DQIy Chart

            //value for metrology chart
            double[] valueY = new double[pAbIso_GlobalContainer.iCurrentUnIsolatedRunCount];   //Verify DQIy Tab => Metrology Chart
            double[] valueUpper = new double[pAbIso_GlobalContainer.iCurrentUnIsolatedRunCount];   //Verify DQIy Tab => Metrology Chart
            double[] valueLower = new double[pAbIso_GlobalContainer.iCurrentUnIsolatedRunCount];   //Verify DQIy Tab => Metrology Chart

            //int startPos = runValue*pointId;
            //int endPos = startPos + runValue;
            int startPos = pAbIso_GlobalContainer.iCurrentUnIsolatedRunCount * PointID;
            int endPos = startPos + pAbIso_GlobalContainer.iCurrentUnIsolatedRunCount;

            int k = 0;
            for (int i = startPos; i < endPos; i++)
            {
                contextID[k] = pVY_LocalContainer.valueDQIyChartData[i, 0];
                valueDQIy[k] = pVY_LocalContainer.valueDQIyChartData[i, 1];
                valueThresholdDQIy[k] = pVY_LocalContainer.valueDQIyChartData[i, 2];
                valueY[k] = pVY_LocalContainer.valueDQIyChartData[i, 4];
                valueUpper[k] = pVY_LocalContainer.valueDQIyChartData[i, 5];
                valueLower[k] = pVY_LocalContainer.valueDQIyChartData[i, 6];
                k++;
            }

            Chart PointDQIyChart = new Chart();
            Chart MetrologyDQIyChart = new Chart();

            // Create a new instance of Chart - DQIy point chart
            utilityChart.CreateChart(PointDQIyChart, "DQIy Chart: " + pSG_GlobalContainer.combinedPoint[PointID].Name, ChartSize_Width, ChartSize_Height, 10, 1, ui_VY_DQIYChart, 0.25);
            PointDQIyChart.AxesX[0].Interval = ComputeInterval((double)pAbIso_GlobalContainer.iCurrentUnIsolatedRunCount, 1, 20); //設定間距
            PointDQIyChart.Series.Add(utilityChart.CreateDataSeries(contextID, valueDQIy, "Work Piece ID", RenderAs.Line, IPS.Common.CommonValue.DQIY_COLOR));
            PointDQIyChart.Series.Add(utilityChart.CreateDataSeries(valueThresholdDQIy, "Threshold", RenderAs.Line, IPS.Common.CommonValue.THRESHOLD_COLOR));
            // 綁定同步移動事件
            PointDQIyChart.AxesX[0].OnZoom += (s, e) =>
            {
                Axis axis = s as Axis;
                axis.Interval = ComputeInterval((double)e.MaxValue, (double)e.MinValue, 20);
                MetrologyDQIyChart.AxesX[0].Zoom(e.MaxValue, e.MinValue);
                MetrologyDQIyChart.AxesX[0].ScrollBarOffset = axis.ScrollBarOffset;
            };
            PointDQIyChart.AxesX[0].Scroll += (s, e) =>
            {
                Axis axis = s as Axis;
                MetrologyDQIyChart.AxesX[0].ScrollBarOffset = axis.ScrollBarOffset;
            };

            // Create a new instance of Chart - DQIy metrology chart
            utilityChart.CreateChart(MetrologyDQIyChart, "Metrology Chart: " + pSG_GlobalContainer.combinedPoint[PointID].Name, ChartSize_Width, ChartSize_Height -20, 1, 1, ui_VY_MetrologyDQIYChart, 0.25);
            MetrologyDQIyChart.AxesX[0].Interval = ComputeInterval((double)pAbIso_GlobalContainer.iCurrentUnIsolatedRunCount, 1, 20); //設定間距
            MetrologyDQIyChart.Series.Add(utilityChart.CreateDataSeries(contextID, valueY, "Metrology Data", RenderAs.Point, IPS.Common.CommonValue.YVALUE_COLOR));
            MetrologyDQIyChart.Series.Add(utilityChart.CreateDataSeries(valueUpper, "UCL", RenderAs.Line, IPS.Common.CommonValue.THRESHOLD_COLOR));
            MetrologyDQIyChart.Series.Add(utilityChart.CreateDataSeries(valueLower, "LCL", RenderAs.Line, IPS.Common.CommonValue.THRESHOLD_COLOR));
            // 綁定同步移動事件
            MetrologyDQIyChart.AxesX[0].OnZoom += (s, e) =>
            {
                Axis axis = s as Axis;
                axis.Interval = ComputeInterval((double)e.MaxValue, (double)e.MinValue, 20);
                PointDQIyChart.AxesX[0].Zoom(e.MaxValue, e.MinValue);
                PointDQIyChart.AxesX[0].ScrollBarOffset = axis.ScrollBarOffset;
            };
            MetrologyDQIyChart.AxesX[0].Scroll += (s, e) =>
            {
                Axis axis = s as Axis;
                PointDQIyChart.AxesX[0].ScrollBarOffset = axis.ScrollBarOffset;
            };
        }

        // 秀全圖事件
        private void VY_ShowChartViewer(object sender, RoutedEventArgs e)
        {
            ModelCreationChartViewer ChartViewer = new ModelCreationChartViewer();
            ChartViewer.MoveBindChart(ui_VY_DQIYChart, ui_VY_MetrologyDQIYChart, null, ChartSize_Width, ChartSize_Height -20);
            ChartViewer.Closed += (s, eargs) =>  //fix bug of silverlight 4 with childwindows ->> disable parent
            {
                Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
            };
            ChartViewer.Show();
        }
        #endregion

        #region Next Step
        private void ui_VY_NextStep_Click(object sender, RoutedEventArgs e)
        {
            // 呼叫切換到DataSelection Tab動作
            if (ChangeToNextStep != null)
            {
                ChangeToNextStep(null, new EventArgs());
            }
        }
        #endregion

        #region Abnormal & Isolated
        // Isolated & Abnormal 選取視窗讀取事件
        private void ui_AbIsoDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            DataGrid TargetDataGrid = (DataGrid)sender;
            CheckBox IsolatedCheckBox = TargetDataGrid.Columns[2].GetCellContent(e.Row) as CheckBox;   //IsolatedCheckBox
            IsolatedCheckBox.IsChecked = pAbIso_GlobalContainer.checkboxesCleanProcessIsolated[e.Row.GetIndex()].IsChecked;
            CheckBox AbnormalCheckBox = TargetDataGrid.Columns[3].GetCellContent(e.Row) as CheckBox;   //AbnormalCheckBox
            AbnormalCheckBox.IsChecked = pAbIso_GlobalContainer.checkboxesCleanProcessAbnormal[e.Row.GetIndex()].IsChecked;
            TargetDataGrid.Columns[3].Visibility = Visibility.Collapsed; //先將Abnormal的欄位取消掉，等確認Abnormal的功能正確後再開啟
        }

        // Isolated & Abnormal 選取視窗Isolated點擊事件
        private void ui_AbIsoDataGridIsolatedCheck_Click(object sender, RoutedEventArgs e)
        {
            CheckBox IsolatedCheckBox = (CheckBox)e.OriginalSource; //取得Checkbox目標的狀態
            DataGrid TargetDataGrid = (DataGrid)GetParentByChildren((CheckBox)sender, typeof(DataGrid)); //取得目標DataGrid
            int Index = TargetDataGrid.SelectedIndex;

            if (pAbIso_GlobalContainer.checkboxesCleanProcessAbnormal[Index].IsChecked == true) // Isolation不能和Abnormal一起勾選
            {
                IsolatedCheckBox.IsChecked = pAbIso_GlobalContainer.checkboxesCleanProcessIsolated[Index].IsChecked;
            }

            pAbIso_GlobalContainer.checkboxesCleanProcessIsolated[Index].IsChecked = IsolatedCheckBox.IsChecked;
            if (pAbIso_GlobalContainer.checkboxesCleanProcessIsolated[Index].IsChecked == true)
            {
                pAbIso_GlobalContainer.checkboxesIsolatedStatus[Index] = "1";
            }
            else
            {
                pAbIso_GlobalContainer.checkboxesIsolatedStatus[Index] = "0";
            }

            //轉換成ObservableCommection
            pAbIso_GlobalContainer.checkboxesIsolatedStatusOC = new ObservableCollection<string>(pAbIso_GlobalContainer.checkboxesIsolatedStatus);

            // 檢查Isolate And Abnormal後的剩餘筆數是否滿足建模的最低筆數限制
            if (!RefreashIsolateAndAbnormalTrainRunCount())
            {
                bool IsChangeAbIsoValueOri = pAbIso_GlobalContainer.IsChangeAbIsoValue;
                IsolatedCheckBox.IsChecked = false; // 不滿足的話 取消此項動作
                ui_AbIsoDataGridIsolatedCheck_Click(sender, e);
                pAbIso_GlobalContainer.IsChangeAbIsoValue = IsChangeAbIsoValueOri;
            }
            else
            {
                pAbIso_GlobalContainer.IsChangeAbIsoValue = true; // 確定有變動到
            }
        }

        // Isolated & Abnormal 選取視窗Abnormal點擊事件
        private void ui_AbIsoDataGridAbnormalCheck_Click(object sender, RoutedEventArgs e)
        {
            CheckBox AbnormalCheckBox = (CheckBox)e.OriginalSource; //取得Checkbox目標的狀態
            DataGrid TargetDataGrid = (DataGrid)GetParentByChildren((CheckBox)sender, typeof(DataGrid)); //取得目標DataGrid
            int Index = TargetDataGrid.SelectedIndex;

            if (pAbIso_GlobalContainer.checkboxesCleanProcessIsolated[Index].IsChecked == true) // Abnormal不能和Isolation一起勾選
            {
                AbnormalCheckBox.IsChecked = pAbIso_GlobalContainer.checkboxesCleanProcessAbnormal[Index].IsChecked;
            }

            pAbIso_GlobalContainer.checkboxesCleanProcessAbnormal[Index].IsChecked = AbnormalCheckBox.IsChecked;

            pAbIso_GlobalContainer.checkboxesAbnormalStatus.Clear();
            for (int i = 0; i < pAbIso_GlobalContainer.checkboxesCleanProcessAbnormal.Count; i++)
            {
                if (pAbIso_GlobalContainer.checkboxesCleanProcessAbnormal[i].IsChecked == true)
                {
                    pAbIso_GlobalContainer.checkboxesAbnormalStatus.Add((i + 1).ToString());
                }
            }
            //轉換成ObservableCommection
            pAbIso_GlobalContainer.checkboxesAbnormalStatusOC = new ObservableCollection<string>(pAbIso_GlobalContainer.checkboxesAbnormalStatus);

            // 檢查Isolate And Abnormal後的剩餘筆數是否滿足建模的最低筆數限制
            if (!RefreashIsolateAndAbnormalTrainRunCount())
            {
                bool IsChangeAbIsoValueOri = pAbIso_GlobalContainer.IsChangeAbIsoValue;
                AbnormalCheckBox.IsChecked = false; // 不滿足的話 取消此項動作
                ui_AbIsoDataGridAbnormalCheck_Click(sender, e);
                pAbIso_GlobalContainer.IsChangeAbIsoValue = IsChangeAbIsoValueOri;
            }
            else
            {
                pAbIso_GlobalContainer.IsChangeAbIsoValue = true; // 確定有變動到
            }
        }

        // 更新Isolate 和 Abnormal的在Train Run時的筆數
        bool RefreashIsolateAndAbnormalTrainRunCount()
        {
            //更新train run片數
            pAbIso_GlobalContainer.iCurrentIUnIsolatedAndAbnormalTrainCount = 0;
            pAbIso_GlobalContainer.iCurrentIUnIsolatedTrainCount = 0;
            pAbIso_GlobalContainer.iCurrentUnIsolatedRunCount = 0;
            for (int ii = 0; ii < pAbIso_GlobalContainer.TotalcontextListCount; ii++)
            {
                if (pAbIso_GlobalContainer.checkboxesCleanProcessIsolated[ii].IsChecked == false && ii < pAbIso_GlobalContainer.TrainingCount)
                {
                    pAbIso_GlobalContainer.iCurrentIUnIsolatedTrainCount++;
                    if (pAbIso_GlobalContainer.checkboxesCleanProcessAbnormal[ii].IsChecked == false)
                    {
                        pAbIso_GlobalContainer.iCurrentIUnIsolatedAndAbnormalTrainCount++;
                    }
                }
                if (pAbIso_GlobalContainer.checkboxesCleanProcessIsolated[ii].IsChecked == false && ii >= pAbIso_GlobalContainer.TrainingCount)
                {
                    pAbIso_GlobalContainer.iCurrentUnIsolatedRunCount++;
                }
            }

            if (pAbIso_GlobalContainer.iCurrentIUnIsolatedAndAbnormalTrainCount < (pSG_GlobalContainer.MaximumIndicatorCount + 5))
            {
                //MessageBox.Show("The Train Data NOT ENOUGHT to Build a Model, We need Cancel This Action!");
                MessageBox.Show("訓練資料「不足夠」建模, 我們必須取消這動作!");
                return false;
            }

            return true;
        }
        #endregion

        #region Common Method
        void LoadComboBox(List<ComboxDataObj> list, ComboBox cmbox)
        {
            cmbox.ItemsSource = list;
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
        private UIElement GetParentByChildren(UIElement Children, Type targetType)
        {
            UIElement parent = (UIElement)VisualTreeHelper.GetParent(Children);
            if (parent.GetType() == targetType)
            {
                return parent;
            }
            else
            {
                return GetParentByChildren(parent, targetType);
            }
        }
        #endregion

        


        

        

        

        


	}
}