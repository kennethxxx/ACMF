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
	public partial class ModelCreationModule_CleanProcessData_Page : UserControl
	{
        // Define Local Container

        // Define Global Container 
        MCDataCollectionGlobalContainer     pDC_GlobalContainer = null;
        MCDataSelectionGlobalContainer      pDS_GlobalContainer = null;
        MCSetGroupGlobalContainer           pSG_GlobalContainer = null;
        MCClearAbnormalYGlobalContainer     pCAY_GlobalContainer = null;
        MCAbnormalIsolatedGlobalContainer   pAbIso_GlobalContainer = null;

        // Define Global Parameter 
        int ChartSize_Height = 180;
        int ChartSize_Width = 735;
        Common.VisifireChart utilityChart = new Common.VisifireChart();

        // Define Global Event
        public event EventHandler ChangeToNextStep;
        public event EventHandler DestroyNextStep;

		public ModelCreationModule_CleanProcessData_Page()
		{
			// 必須將變數初始化
			InitializeComponent();
		}

        #region Init
        public void BindingContainer(
            MCDataCollectionGlobalContainer pDC,
            MCDataSelectionGlobalContainer pDS,
            MCSetGroupGlobalContainer pSG,
            MCClearAbnormalYGlobalContainer pCAY,
            MCAbnormalIsolatedGlobalContainer pAbIso)
        {
            pDC_GlobalContainer = pDC;
            pDS_GlobalContainer = pDS;
            pSG_GlobalContainer = pSG;
            pCAY_GlobalContainer = pCAY;
            pAbIso_GlobalContainer = pAbIso;

        }
        public void InitionPage()
        {
            ui_CP_IndicatorList.ItemsSource = pSG_GlobalContainer.combinedIndicator;
            ui_CP_MetrologyList.ItemsSource = pSG_GlobalContainer.combinedPoint;
            ui_CP_AbIsoDataGrid.ItemsSource = pAbIso_GlobalContainer.contextList;

            Chart IndicatorChart = new Chart();
            Chart MetrologyChart = new Chart();
            utilityChart.CreateChart(IndicatorChart, "??", ChartSize_Width, ChartSize_Height, 2, 1, ui_CP_SPCIndicatorChart, 0.25); //產生空白實體
            utilityChart.CreateChart(MetrologyChart, "??", ChartSize_Width, ChartSize_Height, 2, 1, ui_CP_SPCMetrologyChart, 0.25); //產生空白實體

            if (ui_CP_IndicatorList.ItemsSource != null)
            {
                ui_CP_IndicatorList.SelectedIndex = 0;
            }

            if (ui_CP_MetrologyList.ItemsSource != null)
            {
                ui_CP_MetrologyList.SelectedIndex = 0;
            }
            

            ui_CP_NextStep.IsEnabled = true;
        }
        public void DestroyPage()
        {
            // 呼叫清空NextStep Tab動作
            if (DestroyNextStep != null)
            {
                DestroyNextStep(null, new EventArgs());
            }

            ui_CP_IndicatorList.ItemsSource = null;
            ui_CP_MetrologyList.ItemsSource = null;
            ui_CP_AbIsoDataGrid.ItemsSource = null;

            Chart IndicatorChart = new Chart();
            Chart MetrologyChart = new Chart();
            utilityChart.CreateChart(IndicatorChart, "??", ChartSize_Width, ChartSize_Height, 2, 1, ui_CP_SPCIndicatorChart, 0.25); //產生空白實體
            utilityChart.CreateChart(MetrologyChart, "??", ChartSize_Width, ChartSize_Height, 2, 1, ui_CP_SPCMetrologyChart, 0.25); //產生空白實體

            ui_CP_NextStep.IsEnabled = false;
        }
        #endregion

        #region Show Indicator and Metrology Event
        // 點擊秀Indicator值事件
        private void ui_CP_IndicatorList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MetrologyPoint selectedRow = (MetrologyPoint)ui_CP_IndicatorList.SelectedItem;
            if (selectedRow != null)
            {
                CreateXChart(selectedRow.Name);
            }
        }

        // 點擊Metrology值事件
        private void ui_CP_MetrologyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MetrologyPoint selectedRow = (MetrologyPoint)ui_CP_MetrologyList.SelectedItem;
            if (selectedRow != null)
            {
                CreateYChart(selectedRow.Name);
            }
        }

        // 秀Indicator圖
        void CreateXChart(string indicatorName)
        {
            Indicator indicator = pDC_GlobalContainer.listAllIndicators[indicatorName];//listIndicatorPopulationValue
            double[] tempxValue = new double[indicator.ListItemValue.Count];
            for (int i = 0; i < tempxValue.Length; i++)
            {
                tempxValue[i] = indicator.ListItemValue[i]; //listIndicatorPopulationValue[i, positionArray];
            }
            Chart IndicatorChart = new Chart();//X Chart
            utilityChart.CreateChart(IndicatorChart, "Summary Chart: " + indicatorName, ChartSize_Width, ChartSize_Height, 1, 1, ui_CP_SPCIndicatorChart, 0.25); //產生實體
            IndicatorChart.AxesX[0].Interval = ComputeInterval((double)tempxValue.Length, 1, 20); //設定間距
            IndicatorChart.AxesX[0].OnZoom += new EventHandler<AxisZoomEventArgs>(XChart_OnZoom);
            IndicatorChart.Series.Add(utilityChart.CreateDataSeries(tempxValue, indicatorName, RenderAs.Line, CommonValue.DQIX_COLOR)); //加入Indicator值
        }

        // 秀Metrology圖
        void CreateYChart(string pointName)
        {
            Indicator point = pDC_GlobalContainer.listAllPoints[pointName];//listPointPopulationValue
            MetrologyPoint selectedPoint = GetPositionObjByName(pointName, pSG_GlobalContainer.combinedPoint);

            double[] TempyValue = new double[point.ListItemValue.Count];
            for (int i = 0; i < TempyValue.Length; i++)
            {
                TempyValue[i] = point.ListItemValue[i];
            }

            //Y Chart
            Chart MetrologyChart = new Chart();
            utilityChart.CreateChart(MetrologyChart, "Metrology Chart: " + pointName, ChartSize_Width, ChartSize_Height, 1, 1, ui_CP_SPCMetrologyChart, 0.25); //產生實體
            MetrologyChart.AxesX[0].Interval = ComputeInterval((double)TempyValue.Length, 1, 20); //設定間距
            MetrologyChart.AxesX[0].OnZoom += new EventHandler<AxisZoomEventArgs>(YChart_OnZoom);
            MetrologyChart.Series.Add(utilityChart.CreateDataSeries(TempyValue, pointName, RenderAs.Line, IPS.Common.CommonValue.DQIX_COLOR)); //加入Metrology值

            //Add Trend Line... USL, UCL, ...
            utilityChart.CreateTrendLine(MetrologyChart, selectedPoint.UCL, IPS.Common.CommonValue.TREND_UCL_COLOR);
            utilityChart.CreateTrendLine(MetrologyChart, selectedPoint.USL, IPS.Common.CommonValue.TREND_USL_COLOR);
            utilityChart.CreateTrendLine(MetrologyChart, selectedPoint.LCL, IPS.Common.CommonValue.TREND_UCL_COLOR);
            utilityChart.CreateTrendLine(MetrologyChart, selectedPoint.LSL, IPS.Common.CommonValue.TREND_USL_COLOR);
        }

        // Indicator圖的縮放事件
        void XChart_OnZoom(object sender, AxisZoomEventArgs e)
        {
            Axis axis = (Axis)sender;
            axis.Interval = ComputeInterval((double)e.MaxValue, (double)e.MinValue, 20);
        }

        // Metrology圖的縮放事件
        void YChart_OnZoom(object sender, AxisZoomEventArgs e)
        {
            Axis axis = (Axis)sender;
            axis.Interval = ComputeInterval((double)e.MaxValue, (double)e.MinValue, 20);
        }

        // 秀全圖事件
        private void CP_ShowChartViewer(object sender, RoutedEventArgs e)
        {
            ModelCreationChartViewer ChartViewer = new ModelCreationChartViewer();

            ChartViewer.MoveBindChart(ui_CP_SPCIndicatorChart, ui_CP_SPCMetrologyChart, null, ChartSize_Width , ChartSize_Height);
            //fix bug of silverlight 4 with childwindows ->> disable parent
            ChartViewer.Closed += (s, eargs) =>
            {
                Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
            };
            ChartViewer.Show();
        }

        // 取得Position物件
        MetrologyPoint GetPositionObjByName(string name, ObservableCollection<MetrologyPoint> list)
        {
            MetrologyPoint SelectedPoint = null;
            int i = 0;
            foreach (MetrologyPoint obj in list)
            {
                if (obj.Name == name)
                {
                    SelectedPoint = obj;
                    break;
                }
                i++;
            }
            return SelectedPoint;
        }

        #endregion

        #region Next Step

        // 進入下一步
        private void ui_CP_NextStep_Click(object sender, RoutedEventArgs e)
        {
            if (pAbIso_GlobalContainer.IsChangeAbIsoValue) // 沒有變動Isolation或 Abnormal的話 不用重新跑 DataTranfer
            {
                ExecuteDataTranferModule();
            }
            else
            {
                ExecuteMDFRModule();
            }
        }

        void ExecuteDataTranferModule()
        {
            ModelCreationModuleAlgorithmFlowControler MCAlgrControler = new ModelCreationModuleAlgorithmFlowControler(this.Dispatcher);
            MCAlgrControler.ExecuteDataTranferModule_Fail += new EventHandler(ExecuteDataTranferModule_Fail);
            MCAlgrControler.ExecuteDataTranferModule_Finish += new EventHandler(ExecuteDataTranferModule_Finish);


           // Shell.waitingForm.SettingMessage("Execute Module[Data Transfer]: ");
            Shell.waitingForm.SettingMessage("執行模組[[Data Transfer]: ");
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
            // 停留在原地 並移除Next Step 再從跑DataTranfer
            
            if (DestroyNextStep != null)
            {
                DestroyNextStep(null, new EventArgs());
            }
            
            pAbIso_GlobalContainer.IsChangeAbIsoValue = true;

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
                    MessageBox.Show("錯誤訊息:資料轉換設定失敗! 請再試一次");
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
                ExecuteMDFRModule();
            }
            else
            {
                // 停留在原地 並移除Next 下次再從跑DataTranfer
                if (DestroyNextStep != null)
                {
                    DestroyNextStep(null, new EventArgs());
                }
                pAbIso_GlobalContainer.IsChangeAbIsoValue = true;
                Shell.waitingForm.Close();
            }
        }

        void ExecuteMDFRModule()
        {
            ModelCreationModuleAlgorithmFlowControler MCAlgrControler = new ModelCreationModuleAlgorithmFlowControler(this.Dispatcher);
            MCAlgrControler.ExecuteMDFRModule_Fail += new EventHandler(ExecuteMDFRModule_Fail);
            MCAlgrControler.ExecuteMDFRModule_Finish += new EventHandler(ExecuteMDFRModule_Finish);

            //Shell.waitingForm.SettingMessage("Execute Module[MDFR]: ");
            Shell.waitingForm.SettingMessage("執行模組[MDFR]: ");
           // Shell.waitingForm.SettingTip("MDFR: Metrology Data Filter Rule");
            Shell.waitingForm.SettingTip("MDFR: 過濾量測資料規則");
            Shell.waitingForm.Show();

            MCAlgrControler.ExecuteMDFRModule(pDC_GlobalContainer.Company, pDC_GlobalContainer.LoginUsername);
        }
        void ExecuteMDFRModule_Fail(object sender, EventArgs e)
        {
            // 停留在原地 並移除Next Step
            if (DestroyNextStep != null)
            {
                DestroyNextStep(null, new EventArgs());
            }
            
            Shell.waitingForm.Close();
        }
        void ExecuteMDFRModule_Finish(object sender, EventArgs e)
        {
            bool IsSuccess = false;
            try
            {

                Out_MDFR list = ((Get_MDFRResultCompletedEventArgs)e).Result;
                if (0.0 == list.listCOM_Ack[0][0])
                {
                    IsSuccess = true;
                }
                else if (1.0 == list.listCOM_Ack[0][0])
                {
                    //MessageBox.Show("Error: Set MDFR Parameter fail.");
                    MessageBox.Show("錯誤訊息: 設定 MDFR 參數失敗.");
                    IsSuccess = false;
                }
                else
                {
                   // MessageBox.Show("Error: Execute MDFR Module fail.");
                    MessageBox.Show("錯誤訊息: 執行 MDFR 模組失敗.");
                    IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("錯誤訊息: " + ex.ToString());
                //MessageBox.Show("Error: " + ex.ToString());
            }

            if (IsSuccess)
            {
                Shell.waitingForm.Close();
                ExecuteDQIyModule_GetDQIyPattern();//cal DQIy Pattern
            }
            else
            {
                // 停留在原地 並移除Next Step
                if (DestroyNextStep != null)
                {
                    DestroyNextStep(null, new EventArgs());
                }
                Shell.waitingForm.Close();
            }
        }

        void ExecuteDQIyModule_GetDQIyPattern()
        {
            ModelCreationModuleAlgorithmFlowControler MCAlgrControler = new ModelCreationModuleAlgorithmFlowControler(this.Dispatcher);
            MCAlgrControler.ExecuteDQIyModule_GetDQIyPattern_Fail += new EventHandler(ExecuteDQIyModule_GetDQIyPattern_Fail);
            MCAlgrControler.ExecuteDQIyModule_GetDQIyPattern_Finish += new EventHandler(ExecuteDQIyModule_GetDQIyPattern_Finish);

            //Shell.waitingForm.SettingMessage("Execute Module[DQIy(GetDQIyPattern)]: ");
            Shell.waitingForm.SettingMessage("執行模組[DQIy(GetDQIyPattern)]: ");
            //Shell.waitingForm.SettingTip("DQIy: Data Quality Index of y");
            Shell.waitingForm.SettingTip("DQIy: y 的資料品質指標");
            Shell.waitingForm.Show();

            MCAlgrControler.ExecuteDQIyModule_GetDQIyPattern(pDC_GlobalContainer.Company, pDC_GlobalContainer.LoginUsername);
        }
        void ExecuteDQIyModule_GetDQIyPattern_Fail(object sender, EventArgs e)
        {
            // 停留在原地 並移除Next Step
            if (DestroyNextStep != null)
            {
                DestroyNextStep(null, new EventArgs());
            }

            Shell.waitingForm.Close();
        }
        void ExecuteDQIyModule_GetDQIyPattern_Finish(object sender, EventArgs e)
        {

            bool IsSuccess = false;
            MCClearAbnormalYGlobalContainer pCAY_GlobalContainerTemp = new MCClearAbnormalYGlobalContainer();
            try
            {
                
                Out_DQIy_CleanAbnormalY CleanAbnormalYResult = ((Get_DQIyResult_GetDQIyPatternCompletedEventArgs)e).Result;

                if (0.0 == CleanAbnormalYResult.listCOM_Ack[0][0])
                {
                    ObservableCollection<double>[] DoubleListTemp = null;
                    //取出資料
                    DoubleListTemp = CleanAbnormalYResult.listPointList.ToArray();
                    pCAY_GlobalContainerTemp.PointList = utilityChart.ConvertToRealPredictData(DoubleListTemp);                    // listPointList
                    pCAY_GlobalContainerTemp.metrologyPointSize = DoubleListTemp.Length;

                    DoubleListTemp = CleanAbnormalYResult.listPatternListIndex4PatternID.ToArray();
                    pCAY_GlobalContainerTemp.PatternListIndex4PatternID = utilityChart.ConvertToRealPredictData(DoubleListTemp);   // listPatternListIndex4PatternID
                    pCAY_GlobalContainerTemp.patternSize = Int32.Parse(pCAY_GlobalContainerTemp.PatternListIndex4PatternID[0, 0].ToString());

                    DoubleListTemp = CleanAbnormalYResult.listContextIDOfStepIndex.ToArray();
                    pCAY_GlobalContainerTemp.ContextIDOfStepIndex = utilityChart.ConvertToRealPredictData(DoubleListTemp);         // listContextIDOfStepIndex

                    DoubleListTemp = CleanAbnormalYResult.listArtUList.ToArray();
                    pCAY_GlobalContainerTemp.ArtUList = utilityChart.ConvertToRealPredictData(DoubleListTemp);                     // listArtUList

                    DoubleListTemp = CleanAbnormalYResult.listIndicatorIDIndex4ContextID.ToArray();
                    pCAY_GlobalContainerTemp.IndicatorIDIndex4ContextID = utilityChart.ConvertToRealPredictData(DoubleListTemp);//listIndicatorIDIndex4ContextID

                    DoubleListTemp = CleanAbnormalYResult.listContextIDIndex4PointID.ToArray();
                    pCAY_GlobalContainerTemp.ContextIDIndex4PointID = utilityChart.ConvertToRealPredictData(DoubleListTemp);         //listContextIDIndex4PointID

                    DoubleListTemp = CleanAbnormalYResult.listSortPatternListOfPoint.ToArray();
                    pCAY_GlobalContainerTemp.SortPatternListOfPoint = utilityChart.ConvertToRealPredictData(DoubleListTemp);         //listSortPatternListOfPoint

                    DoubleListTemp = CleanAbnormalYResult.ContextIDChart.ToArray();
                    pCAY_GlobalContainerTemp.contextIDChartData = utilityChart.ConvertToRealPredictData(DoubleListTemp);                         //ContextIDChart
                    //////////////////////////////////////////////////////////////////////////
                    IsSuccess = true;
                }
                else if (1.0 == CleanAbnormalYResult.listCOM_Ack[0][0])
                {
                    //MessageBox.Show("Error: Set DQIy Module - GetDQIyPattern Parameter fail.");
                    MessageBox.Show("錯誤訊息: 設定 DQIy 模組 - GetDQIyPattern 參數失敗.");
                    IsSuccess = false;
                }
                else
                {
                    //MessageBox.Show("Error: Execute DQIy Module - GetDQIyPattern fail.");
                    MessageBox.Show("錯誤訊息: 設定 DQIy 模組 - GetDQIyPattern 參數失敗.");
                    IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error: " + ex.ToString());
                MessageBox.Show("錯誤訊息: " + ex.ToString());
            }

            if (IsSuccess)
            {
                // 把pCAY_GlobalContainerTemp複製到pCAY_GlobalContainer
                pCAY_GlobalContainer.Clear();
                pCAY_GlobalContainer.Copy(pCAY_GlobalContainerTemp);

                // 呼叫切換到DataSelection Tab動作
                if (ChangeToNextStep != null)
                {
                    ChangeToNextStep(null, new EventArgs());
                }
                Shell.waitingForm.Close();
            }
            else
            {
                // 停留在原地 並移除Next Step
                if (DestroyNextStep != null)
                {
                    DestroyNextStep(null, new EventArgs());
                }
                Shell.waitingForm.Close();
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
               MessageBox.Show("訓練資料量「不夠多」 來建模, 我們必須取消這個動作!");
                return false;
            }

            return true;
        }

        #endregion

        #region Common Method

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

        // Check wherether a MetrologyPoint's NAme exists in a list of MetrologyPoint or not
        bool CheckExistIndicatorOrPoint(String pValue, ObservableCollection<MetrologyPoint> list)
        {
            foreach (MetrologyPoint point in list)
            {
                if (point.Name.Equals(pValue))
                {
                    return true;    //exist
                }
            }
            return false;   //not exist
        }

        List<ComboxDataObj> GetPointListForCombobox(ObservableCollection<MetrologyPoint> list)
        {
            List<ComboxDataObj> listPoint = new List<ComboxDataObj>();
            for (int i = 0; i < list.Count; i++)
            {
                ComboxDataObj obj = new ComboxDataObj();
                obj.Name = list[i].Name;
                obj.Value = i;
                listPoint.Add(obj);
            }
            return listPoint;
        }
        #endregion
    }
}