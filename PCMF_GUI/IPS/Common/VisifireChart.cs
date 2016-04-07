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
using Visifire.Charts;
using System.Diagnostics;

namespace IPS.Common
{
    public class VisifireChart
    {
        public VisifireChart()
        {

        }

        /// <summary>
        /// Create chart template with data series
        /// </summary>
        /// <param name="chart">Chart object</param>
        /// <param name="strTitle">Title</param>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        /// <param name="axisInterval">interval of Axis X</param>
        /// <param name="gridInterval">interval of chart grid of Axis X</param>
        /// <param name="stPanel">Stachpanel for showing chart</param>
        public void CreateChart(Chart chart, String strTitle, int width, int height, int axisXInterval, int gridXInterval, StackPanel stPanel, double thickness)
        {
            stPanel.Children.Clear();   //clear old chart

            // Set the chart width and height
            chart.Width = width;
            chart.Height = height;

            chart.ZoomingEnabled = true;

            //Auto scale asixY
            Axis axisY = new Axis();
            axisY.ValueFormatString = "##0.##########";
            axisY.StartFromZero = false;
            chart.AxesY.Add(axisY);

            // Create a new instance of Title
            Title title = new Title();

            // Set title property
            title.Text = strTitle;

            // Add title to Titles collection
            chart.Titles.Add(title);

            chart.LightingEnabled = true;

            // Create a new Axis
            Axis axis = new Axis();
            // Set axis properties
            axis.Interval = axisXInterval;

            //Create a chart grid
            ChartGrid chartGrid = new ChartGrid();
            chartGrid.LineThickness = thickness;    // 0.25;
            chartGrid.Interval = gridXInterval;

            axis.Grids.Add(chartGrid);
            chart.AxesX.Add(axis);

            // Create a new instance of DataSeries - Fake dataseries to show legend when having only one dataseries wanted to show
            DataSeries dataSeries = new DataSeries();
            dataSeries.ShowInLegend = false;

            // Add dataSeries to Series collection.
            chart.Series.Add(dataSeries);

            // Add chart to stpMQE
            stPanel.Children.Add(chart);
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Create chart template with data series
        /// </summary>
        /// <param name="chart">Chart object</param>
        /// <param name="strTitle">Title</param>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        /// <param name="axisXInterval">interval of Axis X</param>
        /// <param name="gridXInterval">interval of chart grid of Axis X</param>
        /// <param name="AxisMinimum">Axis Y Minimum Value(null by default)</param>
        /// <param name="AxisMaximum">Axis Y Maximun Value(null by default)</param>
        /// <param name="stPanel">Stachpanel for showing chart</param>
        /// <param name="thickness"></param>
        public void CreateChart(Chart chart, String strTitle, String strAxisXTitle, String strAxisYTitle, int width, int height, int axisXInterval, int gridXInterval, object AxisYMinimum, object AxisYMaximum, StackPanel stPanel, double thickness)
        {
            stPanel.Children.Clear();   //clear old chart

            // Set the chart width and height
            chart.Width = width;
            chart.Height = height;

            chart.ZoomingEnabled = true;

            //Auto scale asixY
            Axis axisY = new Axis();
            axisY.ValueFormatString = "##0.##########";
            axisY.StartFromZero = false;

            if (strAxisYTitle != null)
            {
                axisY.Title = strAxisYTitle;
            }

            if (AxisYMinimum != null)
                axisY.AxisMinimum = AxisYMinimum;
            if (AxisYMaximum != null)
                axisY.AxisMaximum = AxisYMaximum;

            chart.AxesY.Add(axisY);

            // Create a new instance of Title
            Title title = new Title();

            // Set title property
            title.Text = strTitle;

            // Add title to Titles collection
            chart.Titles.Add(title);

            chart.LightingEnabled = true;

            // Create a new Axis
            Axis axis = new Axis();
            // Set axis properties
            axis.Interval = axisXInterval;

            if (strAxisXTitle!=null)
            {
                axis.Title = strAxisXTitle;
            }

            //Create a chart grid
            ChartGrid chartGrid = new ChartGrid();
            chartGrid.LineThickness = thickness;    // 0.25;
            chartGrid.Interval = gridXInterval;

            axis.Grids.Add(chartGrid);
            chart.AxesX.Add(axis);

            // Create a new instance of DataSeries - Fake dataseries to show legend when having only one dataseries wanted to show
            DataSeries dataSeries = new DataSeries();
            dataSeries.ShowInLegend = false;

            // Add dataSeries to Series collection.
            chart.Series.Add(dataSeries);

            // Add chart to stpMQE
            stPanel.Children.Add(chart);
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Create dataseres for visifire chart
        /// </summary>
        /// <param name="data">series of data</param>
        /// <param name="legend"></param>
        /// <param name="render"></param>
        /// <returns></returns>
        public DataSeries CreateDataSeries(double[] data, String legend, RenderAs render, Color color)
        {
            // Create a new instance of DataSeries
            DataSeries dataSeries = new DataSeries();

            // Set DataSeries property
            dataSeries.RenderAs = render;
            dataSeries.LegendText = legend;
            dataSeries.ShowInLegend = true;
            dataSeries.MovingMarkerEnabled = true;
            //dataSeries.LightingEnabled = true;
            dataSeries.MarkerEnabled = false;

            if (render == RenderAs.Point)
            {
                dataSeries.MarkerType = Visifire.Commons.MarkerTypes.Cross;
                dataSeries.MarkerEnabled = true;
            }

            if (color != null)
                dataSeries.Color = new System.Windows.Media.SolidColorBrush(color);

            // Create a DataPoint
            DataPoint dataPoint;

            for (int i = 0; i < data.Length; i++)
            {
                // Create a new instance of DataPoint
                dataPoint = new DataPoint();

                // Set YValue for a DataPoint
                dataPoint.YValue = data[i];

                // Add dataPoint to DataPoints collection
                dataSeries.DataPoints.Add(dataPoint);
            }

            return dataSeries;
        }

        /// <summary>
        /// Create dataseres for visifire chart with specific Axis X Label
        /// </summary>
        /// <param name="data">series of data</param>
        /// <param name="legend"></param>
        /// <param name="render"></param>
        /// <returns></returns>
        public DataSeries CreateDataSeries(double[] dataX, double[] dataY, String legend, RenderAs render, Color color)
        {
            // Create a new instance of DataSeries
            DataSeries dataSeries = new DataSeries();

            // Set DataSeries property
            dataSeries.RenderAs = render;
            dataSeries.LegendText = legend;
            dataSeries.ShowInLegend = true;
            dataSeries.MovingMarkerEnabled = true;
            //dataSeries.LightingEnabled = true;
            dataSeries.MarkerEnabled = false;
            dataSeries.MarkerScale = 0.005;

            if (render == RenderAs.Point)
            {
                dataSeries.MarkerType = Visifire.Commons.MarkerTypes.Cross;
                dataSeries.MarkerEnabled = true;
            }

            if (color != null)
                dataSeries.Color = new System.Windows.Media.SolidColorBrush(color);

            // Create a DataPoint
            DataPoint dataPoint;

            for (int i = 0; i < dataX.Length; i++)
            {
                // Create a new instance of DataPoint
                dataPoint = new DataPoint();

                // Set YValue for a DataPoint
                dataPoint.YValue = dataY[i];

                // Set XValue for a DataPoint
                dataPoint.AxisXLabel = dataX[i].ToString();

                // Add dataPoint to DataPoints collection
                dataSeries.DataPoints.Add(dataPoint);
            }

            return dataSeries;
        }

        /// <summary>
        /// Create dataseres for visifire chart
        /// </summary>
        /// <param name="data">series of data for pattern chart</param>
        /// <param name="legend"></param>
        /// <param name="render"></param>
        /// <returns></returns>
        public DataSeries CreateDataSeriesPatternChart(string[] dataX, double[] dataY, RenderAs render)
        {
            // Create a new instance of DataSeries
            DataSeries dataSeries = new DataSeries();

            // Set DataSeries property
            dataSeries.RenderAs = render;
            dataSeries.ShowInLegend = false;
            dataSeries.MovingMarkerEnabled = true;
            dataSeries.LightingEnabled = true;
            dataSeries.MarkerEnabled = false;
            dataSeries.LineThickness = 1;

            // Create a DataPoint
            DataPoint dataPoint;

            for (int i = 0; i < dataY.Length; i++)
            {
                // Create a new instance of DataPoint
                dataPoint = new DataPoint();

                // Set YValue for a DataPoint
                dataPoint.YValue = dataY[i];

                // Set XValue for a DataPoint
                dataPoint.AxisXLabel = dataX[i];

                // Add dataPoint to DataPoints collection
                dataSeries.DataPoints.Add(dataPoint);
            }

            return dataSeries;
        }

        /// <summary>
        /// Convert predict data from services into real data to draw a chart
        /// </summary>
        /// <param name="listValue">data services</param>
        /// <returns></returns>
        public double[,] ConvertToRealPredictData(System.Collections.ObjectModel.ObservableCollection<double>[] listValue)
        {
            return ConvertToRealPredictData(listValue, listValue.Length, listValue[0].Count);
        }

        public double[,] ConvertToRealPredictData(System.Collections.ObjectModel.ObservableCollection<double>[] listValue, int ListLenghth1, int ListLength2)
        {
            double[,] doubleArray = new double[ListLenghth1, ListLength2];

            for (int i = 0; i < ListLenghth1; i++)
            {
                for (int j = 0; j < ListLength2; j++)
                {
                    doubleArray[i, j] = listValue[i][j];
                }
            }
            return doubleArray;
        }

        public int[,] ConvertToRealPredictDataInt(System.Collections.ObjectModel.ObservableCollection<int>[] listValue)
        {
            return ConvertToRealPredictDataInt(listValue, listValue.Length, listValue[0].Count);
        }

        public int[,] ConvertToRealPredictDataInt(System.Collections.ObjectModel.ObservableCollection<int>[] listValue, int ListLenghth1, int ListLength2)
        {
            int[,] intArray = new int[ListLenghth1, ListLength2];
            for (int i = 0; i < ListLenghth1; i++)
            {
                for (int j = 0; j < ListLength2; j++)
                {
                    intArray[i, j] = listValue[i][j];
                }
            }
            return intArray;
        }

        public System.Collections.ObjectModel.ObservableCollection<System.Collections.ObjectModel.ObservableCollection<double>> ConvertdoubleToObservableCollection(double[,] doubleValuele)
        {
            int length1 = doubleValuele.GetLength(0);
            int length2 = doubleValuele.GetLength(1);
            System.Collections.ObjectModel.ObservableCollection<System.Collections.ObjectModel.ObservableCollection<double>> output = new System.Collections.ObjectModel.ObservableCollection<System.Collections.ObjectModel.ObservableCollection<double>>();
            System.Collections.ObjectModel.ObservableCollection<double> Temp = null;
            new System.Collections.ObjectModel.ObservableCollection<double>();
            for (int i = 0; i < length1; i++)
            {
                Temp = new System.Collections.ObjectModel.ObservableCollection<double>();
                for (int j = 0; j < length2; j++)
                {
                    Temp.Add(doubleValuele[i, j]);
                }
                output.Add(Temp);
            }
            return output;
        }
        
        public void CreateTrendLine(Chart chart, double trendValue, Color color)
        {
            TrendLine trend = new TrendLine();
            trend.Value = trendValue;
            trend.LineColor = new System.Windows.Media.SolidColorBrush(color);            
            chart.TrendLines.Add(trend);
        }
    }
}
