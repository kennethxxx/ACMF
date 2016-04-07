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
using Visifire.Charts;

namespace IPS.ViewsSub.ModelCreationModule
{
    public partial class ModelCreationChartViewer : ChildWindow
    {
        public ModelCreationChartViewer()
        {
            InitializeComponent();
        }

        private StackPanel StackPanelParameter1 = null;
        private StackPanel StackPanelParameter2 = null;
        private StackPanel StackPanelParameter3 = null;
        private int OriChartSize_Width = 0;
        private int OriChartSize_Height = 0;

        private int ZoonInChartSize_Width = 1040;
        private int ZoonInChartSize_Height = 300;


        public void MoveBindChart(StackPanel SP1, StackPanel SP2, StackPanel SP3, int ChartSize_Width, int ChartSize_Height)
        {
            StackPanelParameter1 = SP1;
            StackPanelParameter2 = SP2;
            StackPanelParameter3 = SP3;
            OriChartSize_Width = ChartSize_Width;
            OriChartSize_Height = ChartSize_Height;

            ChartSpace_1.Children.Clear();
            ChartSpace_2.Children.Clear();
            ChartSpace_3.Children.Clear();

            if (StackPanelParameter1 != null)
            {
                Chart Temp = (Chart)StackPanelParameter1.Children[0];
                StackPanelParameter1.Children.Clear();
                Temp.Width = ZoonInChartSize_Width;
                Temp.Height = ZoonInChartSize_Height;
                ChartSpace_1.Children.Add(Temp);
            }

            if (StackPanelParameter2 != null)
            {
                Chart Temp = (Chart)StackPanelParameter2.Children[0];
                StackPanelParameter2.Children.Clear();
                Temp.Width = ZoonInChartSize_Width;
                Temp.Height = ZoonInChartSize_Height;
                ChartSpace_2.Children.Add(Temp);
            }

            if (StackPanelParameter3 != null)
            {
                Chart Temp = (Chart)StackPanelParameter3.Children[0];
                StackPanelParameter3.Children.Clear();
                Temp.Width = ZoonInChartSize_Width;
                Temp.Height = ZoonInChartSize_Height;
                ChartSpace_3.Children.Add(Temp);
            }
        }

        private void MoveoutBindChart()
        {
            if (StackPanelParameter1 != null)
            {
                Chart Temp = (Chart)ChartSpace_1.Children[0];
                ChartSpace_1.Children.Clear();
                Temp.Width = OriChartSize_Width;
                Temp.Height = OriChartSize_Height;
                StackPanelParameter1.Children.Add(Temp);
            }

            if (StackPanelParameter2 != null)
            {
                Chart Temp = (Chart)ChartSpace_2.Children[0];
                ChartSpace_2.Children.Clear();
                Temp.Width = OriChartSize_Width;
                Temp.Height = OriChartSize_Height;
                StackPanelParameter2.Children.Add(Temp);
            }

            if (StackPanelParameter3 != null)
            {
                Chart Temp = (Chart)ChartSpace_3.Children[0];
                ChartSpace_3.Children.Clear();
                Temp.Width = OriChartSize_Width;
                Temp.Height = OriChartSize_Height;
                StackPanelParameter3.Children.Add(Temp);
            }
        }

        private void ui_Cancel_Click(object sender, RoutedEventArgs e)
        {
            MoveoutBindChart();
            this.DialogResult = false;
        }

        private void ChildWindow_Closed(object sender, EventArgs e)
        {

        }
    }
}

