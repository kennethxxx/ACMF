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
	public partial class ModelCreationModule_VerifyDQIx_Page : UserControl
	{
        // Define Local Container
        MCVerifyDQIxLocalContainer pVX_LocalContainer = null;

        // Define Global Container
        MCDataCollectionGlobalContainer     pDC_GlobalContainer = null;
        MCDataSelectionGlobalContainer      pDS_GlobalContainer = null;
        MCSetGroupGlobalContainer           pSG_GlobalContainer = null;
        MCVerifyDQIxGlobalContainer         pVX_GlobalContainer = null;
        MCAbnormalIsolatedGlobalContainer   pAbIso_GlobalContainer = null;

        // Define Global Parameter 
        int ChartSize_Height = 180;
        int ChartSize_Width = 735;
        Common.VisifireChart utilityChart = new Common.VisifireChart();

        // Define Global Event
        public event EventHandler ChangeToNextStep;
        public event EventHandler DestroyNextStep;

        public ModelCreationModule_VerifyDQIx_Page()
        {
            // 必須將變數初始化
            InitializeComponent();
            pVX_LocalContainer = new MCVerifyDQIxLocalContainer();
        }

        #region Init
        public void BindingContainer(
            MCDataCollectionGlobalContainer pDC,
            MCDataSelectionGlobalContainer  pDS,
            MCSetGroupGlobalContainer       pSG,
            MCVerifyDQIxGlobalContainer     pVX,
            MCAbnormalIsolatedGlobalContainer pAbIso
            )
        {
            pDC_GlobalContainer = pDC;
            pDS_GlobalContainer = pDS;
            pSG_GlobalContainer = pSG;
            pVX_GlobalContainer = pVX;
            pAbIso_GlobalContainer = pAbIso;
        }

        public void InitionPage()
        {
            pVX_LocalContainer.Clear();

            ui_VX_Lambda.Value              = pVX_LocalContainer.Init_Lambda;
            ui_VX_Threshold.Value           = pVX_LocalContainer.Init_Threshold;
            ui_VX_FilterPercentage.Value    = pVX_LocalContainer.Init_FilterPercentage;
            ui_VX_RefreshCounter.Value      = pVX_LocalContainer.Init_RefreshCounter;

            ui_VX_KSSType_KMW.IsChecked = false;
            ui_VX_KSSType_KISS.IsChecked = true;

            ui_VX_ClusterNumber.Value = pVX_LocalContainer.Init_ClusterNumber;

            ui_VX_KVSType_EK.IsChecked = true;
            ui_VX_KVSType_SSEK.IsChecked = false;

            ui_VX_FinAlpha.Value            = pVX_LocalContainer.Init_FinAlpha;
            ui_VX_FoutAlpha.Value           = pVX_LocalContainer.Init_FoutAlpha;

            ui_VX_VerificationMode_Tune.IsChecked = true;
            ui_VX_VerificationMode_Retrain.IsChecked = false;

            ui_VX_DQIxGroupList.ItemsSource = null;

            ui_VX_AbIsoDataGrid.ItemsSource = pAbIso_GlobalContainer.contextList;

            Chart ChartTemp = new Chart();
            utilityChart.CreateChart(ChartTemp, "DQIx Chart: Group?", ChartSize_Width, ChartSize_Height, 2, 1, ui_VX_DQIxChart, 0.25);

            ui_VX_VerifyDQIx.IsEnabled = true;
            ui_VX_NextStep.IsEnabled = false;
        }
        public void DestroyPage()
        {
            // 呼叫清空NextStep Tab動作
            if (DestroyNextStep != null)
            {
                DestroyNextStep(null, new EventArgs());
            }

            pVX_LocalContainer.Clear();

            ui_VX_Lambda.Value = 0.0;
            ui_VX_Threshold.Value = 0.0;
            ui_VX_FilterPercentage.Value = 0.0;
            ui_VX_RefreshCounter.Value = 0;

            ui_VX_KSSType_KMW.IsChecked = false;
            ui_VX_KSSType_KISS.IsChecked = true;

            ui_VX_ClusterNumber.Value = 0;

            ui_VX_KVSType_EK.IsChecked = true;
            ui_VX_KVSType_SSEK.IsChecked = false;

            ui_VX_FinAlpha.Value = 0.0;
            ui_VX_FoutAlpha.Value = 0.0;

            ui_VX_VerificationMode_Tune.IsChecked = true;
            ui_VX_VerificationMode_Retrain.IsChecked = false;

            ui_VX_DQIxGroupList.ItemsSource = null;

            ui_VX_AbIsoDataGrid.ItemsSource = null;

            Chart ChartTemp = new Chart();
            utilityChart.CreateChart(ChartTemp, "DQIx Chart: Group?", ChartSize_Width, ChartSize_Height, 2, 1, ui_VX_DQIxChart, 0.25);

            ui_VX_VerifyDQIx.IsEnabled = false;
            ui_VX_NextStep.IsEnabled = false;
        }
        #endregion

        #region Verify DQIx
        private void ui_VX_VerifyDQIx_Click(object sender, RoutedEventArgs e)
        {
            if (pAbIso_GlobalContainer.IsChangeAbIsoValue) // 沒有變動Isolation或 Abnormal的話 不用重新跑 DataTranfer
            {
                ExecuteDataTranferModule();
            }
            else
            {
                ExecuteKSSModule();
            }
        }

        void ExecuteDataTranferModule()
        {
            ModelCreationModuleAlgorithmFlowControler MCAlgrControler = new ModelCreationModuleAlgorithmFlowControler(this.Dispatcher);
            MCAlgrControler.ExecuteDataTranferModule_Fail += new EventHandler(ExecuteDataTranferModule_Fail);
            MCAlgrControler.ExecuteDataTranferModule_Finish += new EventHandler(ExecuteDataTranferModule_Finish);


            //Shell.waitingForm.SettingMessage("Execute Module[Data Transfer]: ");
            Shell.waitingForm.SettingMessage("執行模組[資料轉換]: ");
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

            ui_VX_NextStep.IsEnabled = false;

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
                    MessageBox.Show("錯誤訊息: 資料轉換設定失敗! 請再試一次");
                    //MessageBox.Show("Error: Data Transfer Settings are fail! Please try it again.");
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

                ui_VX_NextStep.IsEnabled = false;

                Shell.waitingForm.Close();
            }
        }

        void ExecuteKSSModule()
        {
            ModelCreationModuleAlgorithmFlowControler MCAlgrControler = new ModelCreationModuleAlgorithmFlowControler(this.Dispatcher);
            MCAlgrControler.ExecuteKSSModule_Fail += new EventHandler(ExecuteKSSModule_Fail);
            MCAlgrControler.ExecuteKSSModule_Finish += new EventHandler(ExecuteKSSModule_Finish);


            if (ui_VX_KSSType_KMW.IsChecked.Value)
                pVX_LocalContainer.KSSType = "KMW";
            else
                pVX_LocalContainer.KSSType = "KISS";

            pVX_LocalContainer.ClusterNumber = (Double)ui_VX_ClusterNumber.Value;

            //Shell.waitingForm.SettingMessage("Execute Module[KSS{" + pVX_LocalContainer.KSSType +"}]: ");
            Shell.waitingForm.SettingMessage("執行模組[KSS{" + pVX_LocalContainer.KSSType + "}]: ");
            Shell.waitingForm.SettingTip("KSS");
            Shell.waitingForm.Show();
            MCAlgrControler.ExecuteKSSModule(pVX_LocalContainer.KSSType, pVX_LocalContainer.ClusterNumber, pDC_GlobalContainer.Company, pDC_GlobalContainer.LoginUsername);
        }
        void ExecuteKSSModule_Fail(object sender, EventArgs e)
        {
            // 停留在原地 並移除Next Step 不能進入下個Step
            if (DestroyNextStep != null)
            {
                DestroyNextStep(null, new EventArgs());
            }

            ui_VX_NextStep.IsEnabled = false;

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
                    //MessageBox.Show("Error: Set KSS Module Parameter fail.");
                    MessageBox.Show("錯誤訊息: 設定 KSS 模組參數失敗.");
                    IsSuccess = false;
                }
                else
                {
                 //   MessageBox.Show("Error: Execute KSS Module fail.");
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

                ui_VX_NextStep.IsEnabled = false;

                Shell.waitingForm.Close();
            }
        }

        void ExecuteKVSModule()
        {
            ModelCreationModuleAlgorithmFlowControler MCAlgrControler = new ModelCreationModuleAlgorithmFlowControler(this.Dispatcher);
            MCAlgrControler.ExecuteKVSModule_Fail += new EventHandler(ExecuteKVSModule_Fail);
            MCAlgrControler.ExecuteKVSModule_Finish += new EventHandler(ExecuteKVSModule_Finish);

            if (ui_VX_KVSType_EK.IsChecked.Value)
                pVX_LocalContainer.KVSType = "EK";
            else
                pVX_LocalContainer.KVSType = "SS";

            pVX_LocalContainer.FinAlpha = (double)ui_VX_FinAlpha.Value;
            pVX_LocalContainer.FoutAlpha = (double)ui_VX_FoutAlpha.Value;

            
            if (ui_VX_VerificationMode_Tune.IsChecked.Value)
                pVX_LocalContainer.VerificationMode = ui_VX_VerificationMode_Tune.Content.ToString();
            else
                pVX_LocalContainer.VerificationMode = ui_VX_VerificationMode_Retrain.Content.ToString();

            //Shell.waitingForm.SettingMessage("Execute Module[KVS{" + pVX_LocalContainer.KVSType + "}]: ");
            Shell.waitingForm.SettingMessage("執行模組[KVS{" + pVX_LocalContainer.KVSType + "}]: ");
            Shell.waitingForm.SettingTip("KVS");
            Shell.waitingForm.Show();
            MCAlgrControler.ExecuteKVSModule(
                pVX_LocalContainer.KVSType,
                pVX_LocalContainer.FinAlpha,
                pVX_LocalContainer.FoutAlpha,
                pVX_LocalContainer.VerificationMode, 
                pDC_GlobalContainer.Company, pDC_GlobalContainer.LoginUsername);
        }
        void ExecuteKVSModule_Fail(object sender, EventArgs e)
        {
            // 停留在原地 並移除Next Step 不能進入下個Step
            if (DestroyNextStep != null)
            {
                DestroyNextStep(null, new EventArgs());
            }

            ui_VX_NextStep.IsEnabled = false;

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
                    MessageBox.Show("錯誤訊息: 設定 KVS 參數失敗");
                    IsSuccess = false;
                }
                else
                {
                    MessageBox.Show("錯誤訊息: 執行 KVS 模組失敗.");
                    IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Some Error Happen in CallKVS: " + ex.ToString());
                MessageBox.Show("CallKVS發生一些錯誤: " + ex.ToString());
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

                ui_VX_NextStep.IsEnabled = false;

                Shell.waitingForm.Close();
            }
        }

        void ExecuteDQIxModule_VerifyDQIx()
        {
            ModelCreationModuleAlgorithmFlowControler MCAlgrControler = new ModelCreationModuleAlgorithmFlowControler(this.Dispatcher);
            MCAlgrControler.ExecuteDQIxModule_VerifyDQIx_Fail += new EventHandler(ExecuteDQIxModule_VerifyDQIx_Fail);
            MCAlgrControler.ExecuteDQIxModule_VerifyDQIx_Finish += new EventHandler(ExecuteDQIxModule_VerifyDQIx_Finish);


            pVX_LocalContainer.Lambda = ui_VX_Lambda.Value;
            pVX_LocalContainer.Threshold = ui_VX_Threshold.Value;
            pVX_LocalContainer.FilterPercentage = ui_VX_FilterPercentage.Value;
            pVX_LocalContainer.RefreshCounter = ui_VX_RefreshCounter.Value;

            //Shell.waitingForm.SettingMessage("Execute Module[DQIx(Verifying DQIx): ");
            //Shell.waitingForm.SettingTip("DQIx: Data Quality Index of X");
            Shell.waitingForm.SettingMessage("執行模組[DQIx(Verifying DQIx): ");
            Shell.waitingForm.SettingTip("DQIx: X 的資料品質指標");
            Shell.waitingForm.Show();
            MCAlgrControler.ExecuteDQIxModule_VerifyDQIx(
                pVX_LocalContainer.Lambda,
                pVX_LocalContainer.Threshold,
                pVX_LocalContainer.FilterPercentage,
                pVX_LocalContainer.RefreshCounter, 
                pDC_GlobalContainer.Company, pDC_GlobalContainer.LoginUsername);
        }
        void ExecuteDQIxModule_VerifyDQIx_Fail(object sender, EventArgs e)
        {
            // 停留在原地 並移除Next Step 不能進入下個Step
            if (DestroyNextStep != null)
            {
                DestroyNextStep(null, new EventArgs());
            }

            ui_VX_NextStep.IsEnabled = false;

            Shell.waitingForm.Close();
        }
        void ExecuteDQIxModule_VerifyDQIx_Finish(object sender, EventArgs e)
        {
            bool IsSuccess = false;
            MCVerifyDQIxLocalContainer VX_LocalContainerTemp = new MCVerifyDQIxLocalContainer();
            try
            {
                Out_VerifyDQIxResult Out_VerifyDQIxResult_List = ((Get_DQIxResult_VerifyDQIxCompletedEventArgs)e).Result;
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
                MessageBox.Show("DQIx 模組發生一些錯誤 - 驗證DQIx: " + ex.ToString());
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
                pVX_LocalContainer.valueDQIxChartData = VX_LocalContainerTemp.valueDQIxChartData;
                pVX_LocalContainer.valueThresholdDQIxChartData = VX_LocalContainerTemp.valueThresholdDQIxChartData;
                pVX_LocalContainer.valueContextIDDQIxChartData = VX_LocalContainerTemp.valueContextIDDQIxChartData;
                VX_LocalContainerTemp = null;

                // 複製資料 from Local 到 Global
                pVX_GlobalContainer.Copy(pVX_LocalContainer);

                // 綁定Group 展示第一個Group
                LoadComboBox(GroupToComboBoxList(pSG_GlobalContainer.groupList), ui_VX_DQIxGroupList);
                if (ui_VX_DQIxGroupList.ItemsSource != null)
                {
                    ui_VX_DQIxGroupList.SelectedIndex = 0;
                }

                Shell.waitingForm.Close();

                ui_VX_NextStep.IsEnabled = true;
            }
            else
            {
                // 停留在原地 並移除Next Step 不能進入下個Step
                if (DestroyNextStep != null)
                {
                    DestroyNextStep(null, new EventArgs());
                }

                ui_VX_NextStep.IsEnabled = false;

                Shell.waitingForm.Close();
            }
        }
        #endregion

        #region Show DQIx
        // 選擇DQIxGroupList事件
        private void ui_VX_DQIxGroupList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ui_VX_DQIxGroupList.ItemsSource != null)
            {
                if (ui_VX_DQIxGroupList.SelectedIndex != -1)
                {
                    CreateDQIxChart();
                }
            }
        }

        // 畫DQIx 圖
        void CreateDQIxChart()
        {
            //point id -> start from value 0
            int GroupID = Int32.Parse(ui_VX_DQIxGroupList.SelectedValue.ToString());

            double[] valueDQIx = new double[pAbIso_GlobalContainer.iCurrentUnIsolatedRunCount];   //Verify DQIx Tab => DQIx Value
            double[] valueThresholdDQIx = new double[pAbIso_GlobalContainer.iCurrentUnIsolatedRunCount];   //Verify DQIx Tab => DQIx Threshold
            double[] valueContextIDDQIx = new double[pAbIso_GlobalContainer.iCurrentUnIsolatedRunCount];   //Verify DQIx Tab => DQIx ContextID

            int k = 0;
            for (int i = 0; i < pAbIso_GlobalContainer.iCurrentUnIsolatedRunCount; i++)
            {
                valueDQIx[k] = pVX_LocalContainer.valueDQIxChartData[i, GroupID];
                valueThresholdDQIx[k] = pVX_LocalContainer.valueThresholdDQIxChartData[i, GroupID];
                valueContextIDDQIx[k] = pVX_LocalContainer.valueContextIDDQIxChartData[i, GroupID];
                k++;
            }
            Chart DQIxChart = new Chart();
            utilityChart.CreateChart(DQIxChart, "DQIx Chart: Group" + (GroupID + 1), ChartSize_Width, ChartSize_Height, 1, 1, ui_VX_DQIxChart, 0.25);
            DQIxChart.AxesX[0].Interval = ComputeInterval((double)pAbIso_GlobalContainer.iCurrentUnIsolatedRunCount, 1, 20); //設定間距
            DQIxChart.AxesX[0].OnZoom += new EventHandler<AxisZoomEventArgs>(DQIxChart_OnZoom); //綁定縮放事件
            DQIxChart.Series.Add(utilityChart.CreateDataSeries(valueContextIDDQIx, valueDQIx, "Work Piece ID", RenderAs.Line, CommonValue.DQIX_COLOR));
            DQIxChart.Series.Add(utilityChart.CreateDataSeries(valueThresholdDQIx, "Threshold", RenderAs.Line, CommonValue.THRESHOLD_COLOR));
        }

        // DQIx 圖的縮放事件
        void DQIxChart_OnZoom(object sender, AxisZoomEventArgs e)
        {
            Axis axis = (Axis)sender;
            axis.Interval = ComputeInterval((double)e.MaxValue, (double)e.MinValue, 20);
        }

        // 秀全圖事件
        private void VX_ShowChartViewer(object sender, RoutedEventArgs e)
        {
            ModelCreationChartViewer ChartViewer = new ModelCreationChartViewer();
            ChartViewer.MoveBindChart(ui_VX_DQIxChart, null, null, ChartSize_Width, ChartSize_Height);
            ChartViewer.Closed += (s, eargs) =>  //fix bug of silverlight 4 with childwindows ->> disable parent
            {
                Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
            };
            ChartViewer.Show();
        }
        
        #endregion

        #region Next Step
        private void ui_VX_NextStep_Click(object sender, RoutedEventArgs e)
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
              // MessageBox.Show("The Train Data NOT ENOUGHT to Build a Model, We need Cancel This Action!");
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

        List<ComboxDataObj> GroupToComboBoxList(ObservableCollection<Group> GroupList)
        {
            ComboxDataObj combox;
            List<ComboxDataObj> list = new List<ComboxDataObj>();
            for (int i = 0; i < GroupList.Count; i++)
            {
                combox = new ComboxDataObj();
                combox.Name = GroupList[i].GroupName;
                combox.Value = i;
                list.Add(combox);
            }
            return list;
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