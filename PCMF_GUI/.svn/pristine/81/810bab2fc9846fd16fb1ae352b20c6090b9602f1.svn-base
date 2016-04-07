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
	public partial class ModelCreationModule_CleanMetrologyData_Page : UserControl
	{
        // Define Local Container

        // Define Global Container
        MCSetGroupGlobalContainer           pSG_GlobalContainer = null;
        MCClearAbnormalYGlobalContainer     pCAY_GlobalContainer = null;
        MCAbnormalIsolatedGlobalContainer   pAbIso_GlobalContainer = null;

        // Define Global Parameter 
        int ChartSize_Height = 180;
        int ChartSize_Width  = 735;
        Common.VisifireChart utilityChart = new Common.VisifireChart();

        // Define Global Event
        public event EventHandler ChangeToNextStep;
        public event EventHandler DestroyNextStep;

		public ModelCreationModule_CleanMetrologyData_Page()
		{
			// 必須將變數初始化
			InitializeComponent();
		}

        #region Init
        public void BindingContainer(
            MCSetGroupGlobalContainer pSG,
            MCClearAbnormalYGlobalContainer pCAY,
            MCAbnormalIsolatedGlobalContainer pAbIso
            )
        {
            pSG_GlobalContainer = pSG;
            pCAY_GlobalContainer = pCAY;
            pAbIso_GlobalContainer = pAbIso;
        }
        public void InitionPage()
        {
            Chart ContextIDChart = new Chart();
            Chart PatternChart = new Chart();
            utilityChart.CreateChart(ContextIDChart, "Metrology Chart: ?", ChartSize_Width, ChartSize_Height, 1, 1, ui_CM_MetrologyPointChart, 0.1);//產生空白實體
            utilityChart.CreateChart(PatternChart, "Pattern: ?", ChartSize_Width, ChartSize_Height, 1, 1, ui_CM_PatternChart, 0.1);//產生空白實體


            ui_CM_MetrologyIndicatorList.ItemsSource = pSG_GlobalContainer.combinedIndicator;

            LoadComboBox(pSG_GlobalContainer.listPointCombo, ui_CM_MetrologyPoint);
            ui_CM_MetrologyPoint.SelectedIndex = 0;

            ui_CM_AbIsoDataGrid.ItemsSource = pAbIso_GlobalContainer.contextList;

            ui_CM_NextStep.IsEnabled = true;
        }
        public void DestroyPage()
        {
            // 呼叫清空NextStep Tab動作
            if (DestroyNextStep != null)
            {
                DestroyNextStep(null, new EventArgs());
            }

            Chart ContextIDChart = new Chart();
            Chart PatternChart = new Chart();
            utilityChart.CreateChart(ContextIDChart, "Metrology Chart: ?", ChartSize_Width, ChartSize_Height, 1, 1, ui_CM_MetrologyPointChart, 0.1);//產生空白實體
            utilityChart.CreateChart(PatternChart, "Pattern: ?", ChartSize_Width, ChartSize_Height, 1, 1, ui_CM_PatternChart, 0.1);//產生空白實體


            ui_CM_MetrologyPoint.ItemsSource = null;
            ui_CM_PatternType.ItemsSource = null;
            ui_CM_MetrologyIndicatorList.ItemsSource = null;
            ui_CM_AbIsoDataGrid.ItemsSource = null;

            ui_CM_NextStep.IsEnabled = false;
        }
        #endregion

        #region Select and Show Event
        // 選擇MetrologyPoint事件
        private void ui_CM_MetrologyPoint_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ui_CM_MetrologyPoint.SelectedIndex == -1)
            {
                ui_CM_PatternType.ItemsSource = null;
                return;
            }

            pCAY_GlobalContainer.patternSize = Int32.Parse(pCAY_GlobalContainer.PatternListIndex4PatternID[ui_CM_MetrologyPoint.SelectedIndex, 0].ToString());
            LoadComboBox_update(ui_CM_MetrologyPoint.SelectedIndex, pCAY_GlobalContainer.patternSize, "Pattern", ui_CM_PatternType);
            if (ui_CM_PatternType.ItemsSource != null)
            {
                ui_CM_PatternType.SelectedIndex = 0;
            }
        }

        // 選擇PatternType事件
        private void ui_CM_PatternType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (ui_CM_MetrologyPoint.SelectedIndex == -1)
            {
                return;
            }
            if (ui_CM_PatternType.SelectedIndex == -1)
            {
                return;
            }
            CreatePatternChart();       //X Part Chart
            CreateContextIDChart();     //Y Part Chart
        }

        // 畫Pattern圖
        void CreatePatternChart()
        {
            //point id -> start from value 0
            int SelectedPointID = Int32.Parse(ui_CM_MetrologyPoint.SelectedValue.ToString());

            //pattern id -> start from value 0
            int SelectedPatternID = Int32.Parse(ui_CM_PatternType.SelectedValue.ToString());

            pCAY_GlobalContainer.patternSize = Int32.Parse(pCAY_GlobalContainer.PatternListIndex4PatternID[SelectedPointID, 0].ToString());

            int iStartContextIDOfStepIndex = Int32.Parse(pCAY_GlobalContainer.PatternListIndex4PatternID[SelectedPointID, 1].ToString()) - 1;
            int iRealPatternID = iStartContextIDOfStepIndex + SelectedPatternID;//起始位置加上PatternID的位移

            //number of context id of pattern
            int NumberOfContextIDOfPattern = (int)pCAY_GlobalContainer.ContextIDOfStepIndex[iRealPatternID, 1];

            ComboxDataObj PattenObj = (ComboxDataObj)ui_CM_PatternType.SelectedItem;
            Chart TempChart = new Chart(); // Create a new instance of Pattern Chart 
            utilityChart.CreateChart(TempChart, PattenObj.Name, ChartSize_Width, ChartSize_Height, 1, 1, ui_CM_PatternChart, 0.1);

            string[] xValue = new string[pSG_GlobalContainer.combinedIndicator.Count];

            int k = 0;
            foreach (MetrologyPoint item in pSG_GlobalContainer.combinedIndicator)
            {
                xValue[k] = item.Value.ToString();
                k++;
            }

            int ContextIDPosStart = ((int)pCAY_GlobalContainer.ContextIDOfStepIndex[iRealPatternID, 2]) - 1;
            int ContextIDPosEnd = ContextIDPosStart + NumberOfContextIDOfPattern;
            int iPatternSize = ((int)pCAY_GlobalContainer.ContextIDOfStepIndex[iRealPatternID, 1]);

            for (int i = ContextIDPosStart; i < ContextIDPosEnd; i++)
            {
                // 加入存取PATTERN的start end position資訊 [12/26/2011 pili7545]
                int iPatternChartSize = Int32.Parse(pCAY_GlobalContainer.IndicatorIDIndex4ContextID[i, 1].ToString());
                int iPatternChartStartPosition = Int32.Parse(pCAY_GlobalContainer.IndicatorIDIndex4ContextID[i, 2].ToString()) - 1;
                int iPatternChartEndPosition = iPatternChartStartPosition + iPatternChartSize;

                double[] patternValue = new double[iPatternChartSize];  //array value of each indicator

                int ii = 0;
                for (int j = iPatternChartStartPosition; j < iPatternChartEndPosition; j++)
                {
                    patternValue[ii] = pCAY_GlobalContainer.ArtUList[j, 1];   //tranfer value from pattern array to indicator line
                    ii++;
                }
                //add line chart to pattern chart
                TempChart.Series.Add(utilityChart.CreateDataSeriesPatternChart(xValue, patternValue, RenderAs.Line));
            }
            // Highlight dataseries when mouseover
            TempChart.MouseMove += (s, e) =>
            {
                ElementData elementData = (e.OriginalSource as FrameworkElement).Tag as ElementData;
                if (elementData != null)
                {
                    if (elementData.Element != null && elementData.Element.GetType() == typeof(DataSeries)) //點到線時 讓線的顏色落差變大
                    {
                        foreach (DataSeries ds in TempChart.Series)
                        {
                            ds.Opacity = 0.2;
                        }
                        (elementData.Element as DataSeries).Opacity = 1;
                    }
                    else
                    {
                        foreach (DataSeries ds in TempChart.Series)
                        {
                            ds.Opacity = 0.5;
                        }
                    }
                }
            };
        }

        // 畫ContextID 圖
        void CreateContextIDChart()
        {
            //point id -> start from value 0
            int SelectedPointID = Int32.Parse(ui_CM_MetrologyPoint.SelectedValue.ToString());

            //pattern id -> start from value 0
            int SelectedPatternID = Int32.Parse(ui_CM_PatternType.SelectedValue.ToString());

            int iStartContextIDOfStepIndex = Int32.Parse(pCAY_GlobalContainer.PatternListIndex4PatternID[SelectedPointID, 1].ToString()) - 1;
            int iRealPatternID = iStartContextIDOfStepIndex + SelectedPatternID; //起始位置加上PatternID的位移

            //number of context id of pattern
            int NumberOfContextIDOfPattern = (int)pCAY_GlobalContainer.ContextIDOfStepIndex[iRealPatternID, 1];
            int ContextIDPosStart = ((int)pCAY_GlobalContainer.ContextIDOfStepIndex[iRealPatternID, 2]) - 1;
            int ContextIDPosEnd = ContextIDPosStart + NumberOfContextIDOfPattern;

            // Create a new instance of Chart
            Chart TempChart = new Chart();
            utilityChart.CreateChart(TempChart, "Metrology Chart: " + pSG_GlobalContainer.combinedPoint[SelectedPointID].Name, "Sample ID", null, ChartSize_Width, ChartSize_Height, 1, 1, null, null, ui_CM_MetrologyPointChart, 0.1);
            int k = 0;
            double[] ContextIDAxisYValue = new double[NumberOfContextIDOfPattern];//axis Y value
            string[] ContextIDAxisXValue = new string[NumberOfContextIDOfPattern];//axis X label
            for (int i = ContextIDPosStart; i < ContextIDPosEnd; i++)
            {
                ContextIDAxisYValue[k] = pCAY_GlobalContainer.contextIDChartData[0, i]; //axis Y
                ContextIDAxisXValue[k] = pCAY_GlobalContainer.IndicatorIDIndex4ContextID[i, 0].ToString();  //axis X - Label
                k++;
            }
            DataSeries TempDataSeries = utilityChart.CreateDataSeriesPatternChart(ContextIDAxisXValue, ContextIDAxisYValue, RenderAs.Point);
            TempDataSeries.Color = new SolidColorBrush(CommonValue.NN_COLOR);   //顏色要在加進Chart前設定
            TempChart.Series.Add(TempDataSeries);
        }

        // 秀全圖事件
        private void CM_ShowChartViewer(object sender, RoutedEventArgs e)
        {
            ModelCreationChartViewer ChartViewer = new ModelCreationChartViewer();
            ChartViewer.MoveBindChart(ui_CM_MetrologyPointChart, ui_CM_PatternChart, null, ChartSize_Width, ChartSize_Height);
            ChartViewer.Closed += (s, eargs) =>  //fix bug of silverlight 4 with childwindows ->> disable parent
            {
                Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
            };
            ChartViewer.Show();
        }

        #endregion

        #region Next Step

        //進行下一步
        private void ui_CM_NextStep_Click(object sender, RoutedEventArgs e)
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
        
        void LoadComboBox(List<ComboxDataObj> list, ComboBox cmbox)
        {
            cmbox.ItemsSource = list;
        }
        
        // Load, Sort & bind list of value to combo box
        void LoadComboBox_update(int iPointOrder, int iPatternSize, String prefixName, ComboBox cmbox)
        {
            ComboxDataObj combox;
            List<ComboxDataObj> list = new List<ComboxDataObj>();

            int iStart = Int32.Parse(pCAY_GlobalContainer.PatternListIndex4PatternID[iPointOrder, 1].ToString()) - 1;

            int iEnd = iStart + iPatternSize;

            //////////////////////////////////////////////////////////////////////////
            List<int> Namelist = new List<int>();
            List<int> Indexlist = new List<int>();
            int iPatternIndex = 0;

            //重新排序
            for (int i = iStart; i < iEnd; i++)
            {
                Namelist.Add(Convert.ToInt32(pCAY_GlobalContainer.SortPatternListOfPoint[i, 0]));
                Indexlist.Add(iPatternIndex);
                iPatternIndex++;
            }

            for (int i = 0; i < iPatternIndex; i++)
            {
                combox = new ComboxDataObj();
                int nameindex = Namelist.IndexOf(i + 1);
                combox.Name = prefixName + (i + 1).ToString();
                combox.Value = Indexlist[nameindex];
                list.Add(combox);
                Namelist.RemoveAt(nameindex);
                Indexlist.RemoveAt(nameindex);
            }
            cmbox.ItemsSource = list;
        }
        #endregion
    }
}