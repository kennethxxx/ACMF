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
using IPS.Common;

namespace IPS.Views
{
    public partial class ModelCreationBuildOption : ChildWindow
    {
        public ModelCreationBuildOption()
        {
            InitializeComponent();
            LoadDefaultData();
        }

        void LoadDefaultData()
        {
            if ("MahalanobisDistance".Equals(BuildModelOption.GSI_ALGORITHM))
                radMhalanobis.IsChecked = true;
            else
                radEuclidean.IsChecked = true;

            // 強制使用MR [2/2/2012 pili7545]
            //if ("MR".Equals(BuildModelOption.MR_ALGORITHM))
                radMR.IsChecked = true;
            //else
            //    radMMR.IsChecked = true;

            if ("BPNN".Equals(BuildModelOption.NN_ALGORITHM))
                radBPNN.IsChecked = true;
            else
                radGRNN.IsChecked = true;
            
            //Miss RI ???
        }

        void UpdateOptionData()
        {
            if (radMR.IsChecked.Value)
                BuildModelOption.MR_ALGORITHM = "MR";
            else
                BuildModelOption.MR_ALGORITHM = "MMR";

            if (radBPNN.IsChecked.Value)
                BuildModelOption.NN_ALGORITHM = "BPNN";
            else
                BuildModelOption.NN_ALGORITHM = "GRNN"; 
            
            if (radMhalanobis.IsChecked.Value)
                BuildModelOption.GSI_ALGORITHM = "MahalanobisDistance";
            else
                BuildModelOption.GSI_ALGORITHM = "EuclideanDistance";

            //Miss RI ???
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateOptionData();
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

