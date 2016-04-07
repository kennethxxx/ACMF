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
using IPS.DataAcquisition;
using Visifire.Charts;
using IPS.Common;
using System.Net.Browser;

namespace IPS.Views
{
    public partial class HistoricalDataModule : Page
    {
        VisifireChart utilityChart = new VisifireChart();
        Chart nnmrChart;
        Chart riChart;
        Chart gsiChart;

        public HistoricalDataModule()
        {
            InitializeComponent();
            //init parameter
            InitSystem();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        void InitSystem()
        {
            //  [11/24/2011 pili7545]
            startDatePicker.SelectedDate = DateTime.Now;
            endDatePicker.SelectedDate = DateTime.Now;

            //Load vMachine
            LoadVMachineList();
        }

        void LoadVMachineList()
        {
            //Shell.waitingForm.SettingMessage("Loading VMachine");
            Shell.waitingForm.SettingMessage("載入 VMachine");
            Shell.waitingForm.Show();
            
            App.proxyDA.getVmachineListCompleted += (s, e) =>
            {
                cmbVMachine.ItemsSource = e.Result.ToList();
                Shell.waitingForm.Close();
            };
            App.proxyDA.getVmachineListAsync();
        }

        void LoadEquipmentList()
        {
            if (cmbVMachine.SelectedValue != null)
            {
                string vMachine = cmbVMachine.SelectedValue.ToString();

                //Shell.waitingForm.SettingMessage("Loading Equipment");
                Shell.waitingForm.SettingMessage("載入機台");
                Shell.waitingForm.Show();
                
                App.proxyDA.getEquipmentByNameCompleted += (s, e) =>
                {
                    cmbEquipment.ItemsSource = e.Result.ToList();
                    Shell.waitingForm.Close();
                };
                App.proxyDA.getEquipmentByNameAsync(vMachine);
            }
        }

        void SearchVMResults()
        {
            string vMachine = cmbVMachine.SelectedValue.ToString();
            string equipment = cmbEquipment.SelectedValue.ToString();
            string phase = cmbPhase.SelectedValue.ToString();

            DateTime startDate = startDatePicker.SelectedDate.Value;
            DateTime endDate = endDatePicker.SelectedDate.Value;

            //Shell.waitingForm.SettingMessage("Searching VM Results");
            Shell.waitingForm.SettingMessage("搜尋 VM 結果");
            Shell.waitingForm.Show();
            
            App.proxyDA.getModelResultCompleted += (s, e) =>
            {
                searchResult = e.Result;

                //Load Point
                LoadComboBox(e.Result.bpnnValue.OutAll_PredictValueList.GetLength(0), "Point", cmbPoint);

                BuildMQEChart(searchResult.contextsId);

                Shell.waitingForm.Close();
            };
            App.proxyDA.getModelResultAsync(startDate, endDate, phase, vMachine, equipment);
        }

        ModelPredictiveValue searchResult;

        void LoadComboBox(int size, String prefixName, ComboBox cmbox)
        {
            ComboxDataObj combox;
            List<ComboxDataObj> list = new List<ComboxDataObj>();
            for (int i = 0; i < size - 1; i++)
            {
                combox = new ComboxDataObj();
                combox.Name = prefixName + (i + 1);
                combox.Value = i;
                list.Add(combox);
            }
            cmbox.ItemsSource = list;
        }

        void MAPEandMaxErr(int point)
        {
            headerMAPEError.Visibility = Visibility.Visible;
            headerMAPEError.Header = cmbPhase.SelectedValue.ToString();

            txtMapeNN.Text = Math.Round(searchResult.bpnnValue.OutAll_MAPE[point], 3).ToString();
            txtMaxErrNN.Text = Math.Round(searchResult.bpnnValue.OutAll_MaxError[point], 3).ToString();

            txtMapeMR.Text = Math.Round(searchResult.mrValue.OutAll_MAPE[point], 3).ToString();
            txtMaxErrMR.Text = Math.Round(searchResult.mrValue.OutAll_MaxError[point], 3).ToString();
        }

        void BuildNNChart(Chart chart, double[] x, double[] y)
        {
            for (int i = 0; i < x.Length; i++)
            {
                //NN
                chart.Series[1].DataPoints[i].XValue = x[i];
                chart.Series[1].DataPoints[i].YValue = y[i];
            }
        }

        void BuildMRChart(Chart chart, double[] x, double[] y)
        {
            for (int i = 0; i < x.Length; i++)
            {
                //MR
                chart.Series[2].DataPoints[i].XValue = x[i];
                chart.Series[2].DataPoints[i].YValue = y[i];
            }
        }

        void BuildYValueChart(Chart chart, double[] x, double[] y)
        {
            for (int i = 0; i < x.Length; i++)
            {
                //MR
                chart.Series[3].DataPoints[i].XValue = x[i];
                chart.Series[3].DataPoints[i].YValue = y[i];
            }
        }

        void BuildRIGSIChart(Chart chart, double[] x, double[] y, double[] threshold)
        {
            for (int i = 0; i < x.Length; i++)
            {
                //RI
                chart.Series[1].DataPoints[i].XValue = x[i];
                chart.Series[1].DataPoints[i].YValue = y[i];

                //Threshold
                chart.Series[2].DataPoints[i].XValue = x[i];
                chart.Series[2].DataPoints[i].YValue = threshold[i];
            }
        }

        private void cmbPoint_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPoint.SelectedValue != null)
                BuildChart(searchResult.contextsId, Int16.Parse(cmbPoint.SelectedValue.ToString()));
        }

        void BuildMQEChart(double[] contextID)
        {
            double[] tempYData = new double[contextID.Length];
            for (int i = 0; i < contextID.Length; i++)
            {
                tempYData[i] = 0;
            }

            // Create a new instance of Chart
            nnmrChart = new Chart();
            utilityChart.CreateChart(nnmrChart, "Metrology Chart", 750, 300, 2, 2, stpMetrologyChart, 0.25);
            nnmrChart.Series.Add(utilityChart.CreateDataSeries(contextID, tempYData, "NN", RenderAs.Line, CommonValue.NN_COLOR));
            nnmrChart.Series.Add(utilityChart.CreateDataSeries(contextID, tempYData, "MR", RenderAs.Line, CommonValue.MR_COLOR));
            //nnmrChart.Series.Add(utilityChart.CreateDataSeries(contextID, "NN", RenderAs.Line, CommonValue.NN_COLOR));
            //nnmrChart.Series.Add(utilityChart.CreateDataSeries(contextID, "MR", RenderAs.Line, CommonValue.MR_COLOR));
            //nnmrChart.Series.Add(utilityChart.CreateDataSeries(contextID, "Y-Value", RenderAs.Line, CommonValue.YVALUE_COLOR));

            // Create a new instance of Chart
            riChart = new Chart();
            utilityChart.CreateChart(riChart, "RI", 750, 200, 2, 2, stpRIChart, 0.25);
            riChart.Series.Add(utilityChart.CreateDataSeries(contextID, tempYData, "RI", RenderAs.Line, CommonValue.RI_GSI_COLOR));
            riChart.Series.Add(utilityChart.CreateDataSeries(contextID, tempYData, "Threshold", RenderAs.Line, CommonValue.THRESHOLD_COLOR));

            // Create a new instance of Chart
            gsiChart = new Chart();
            utilityChart.CreateChart(gsiChart, "GSI", 750, 200, 2, 2, stpGSIChart, 0.25);
            gsiChart.Series.Add(utilityChart.CreateDataSeries(contextID, tempYData, "GSI", RenderAs.Line, CommonValue.RI_GSI_COLOR));
            gsiChart.Series.Add(utilityChart.CreateDataSeries(contextID, tempYData, "Threshold", RenderAs.Line, CommonValue.THRESHOLD_COLOR));

            ZoomingMultiChartMQETab();
        }

        void BuildChart(double[] contextID, int point)
        {
            //NN Chart
            BuildNNChart(nnmrChart, contextID, searchResult.bpnnValue.OutAll_PredictValueList[point]);

            //MR Chart
            BuildMRChart(nnmrChart, contextID, searchResult.mrValue.OutAll_PredictValueList[point]);

            //Y Value Chart
            //BuildYValueChart(nnmrChart, contextID, searchResult.metrologyVlaue.ToArray());

            //RI Chart
            BuildRIGSIChart(riChart, contextID, searchResult.riValue.OutAll_PredictValueList[point], searchResult.riValue.OutRI_Threshold);

            //GSI Chart
            BuildRIGSIChart(gsiChart, contextID, searchResult.gsiValue.OutAll_PredictValueList[0], searchResult.gsiValue.OUTGSI_Threshold);
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchVMResults();
        }

        private void cmbVMachine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadEquipmentList();
        }

        #region zooming multi chart - MQE Tab

        void ZoomingMultiChartMQETab()
        {
            if (nnmrChart != null)
            {
                nnmrChart.AxesX[0].OnZoom += new EventHandler<AxisZoomEventArgs>(MetrologyChart_OnZoom);
                nnmrChart.AxesX[0].Scroll += new EventHandler<AxisScrollEventArgs>(MetrologyChart_Scroll);
            }

            if (riChart != null)
            {
                riChart.AxesX[0].OnZoom += new EventHandler<AxisZoomEventArgs>(RIChart_OnZoom);
                riChart.AxesX[0].Scroll += new EventHandler<AxisScrollEventArgs>(RIChart_Scroll);
            }

            if (gsiChart != null)
            {
                gsiChart.AxesX[0].OnZoom += new EventHandler<AxisZoomEventArgs>(GSIChart_OnZoom);
                gsiChart.AxesX[0].Scroll += new EventHandler<AxisScrollEventArgs>(GSIChart_Scroll);
            }
        }

        void MetrologyChart_Scroll(object sender, AxisScrollEventArgs e)
        {
            Axis axis = sender as Axis;
            riChart.AxesX[0].ScrollBarOffset = axis.ScrollBarOffset;
            gsiChart.AxesX[0].ScrollBarOffset = axis.ScrollBarOffset;
        }

        void RIChart_Scroll(object sender, AxisScrollEventArgs e)
        {
            Axis axis = sender as Axis;
            nnmrChart.AxesX[0].ScrollBarOffset = axis.ScrollBarOffset;
            gsiChart.AxesX[0].ScrollBarOffset = axis.ScrollBarOffset;
        }

        void GSIChart_Scroll(object sender, AxisScrollEventArgs e)
        {
            Axis axis = sender as Axis;
            nnmrChart.AxesX[0].ScrollBarOffset = axis.ScrollBarOffset;
            riChart.AxesX[0].ScrollBarOffset = axis.ScrollBarOffset;
        }

        void MetrologyChart_OnZoom(object sender, AxisZoomEventArgs e)
        {
            Axis axis = sender as Axis;
            riChart.AxesX[0].Zoom(e.MaxValue, e.MinValue);
            riChart.AxesX[0].ScrollBarOffset = axis.ScrollBarOffset;

            gsiChart.AxesX[0].Zoom(e.MaxValue, e.MinValue);
            gsiChart.AxesX[0].ScrollBarOffset = axis.ScrollBarOffset;
        }

        void RIChart_OnZoom(object sender, AxisZoomEventArgs e)
        {
            Axis axis = sender as Axis;
            nnmrChart.AxesX[0].Zoom(e.MaxValue, e.MinValue);
            nnmrChart.AxesX[0].ScrollBarOffset = axis.ScrollBarOffset;

            gsiChart.AxesX[0].Zoom(e.MaxValue, e.MinValue);
            gsiChart.AxesX[0].ScrollBarOffset = axis.ScrollBarOffset;
        }

        void GSIChart_OnZoom(object sender, AxisZoomEventArgs e)
        {
            Axis axis = sender as Axis;
            nnmrChart.AxesX[0].Zoom(e.MaxValue, e.MinValue);
            nnmrChart.AxesX[0].ScrollBarOffset = axis.ScrollBarOffset;

            riChart.AxesX[0].Zoom(e.MaxValue, e.MinValue);
            riChart.AxesX[0].ScrollBarOffset = axis.ScrollBarOffset;
        }

        #endregion

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            InitSystem();
            cmbEquipment.ItemsSource = "";
            cmbPoint.ItemsSource = "";
            stpMetrologyChart.Children.Clear();
            stpRIChart.Children.Clear();
            stpGSIChart.Children.Clear();
        }

    }
}
