﻿#pragma checksum "C:\研究所\AMCSystems20121106-更新MMbyXML\AMCSystems20121106-更新MMbyXML\AMCSystems20121030-更新DAUserFriendly\IPS\ViewsSub\ModelCreationModule\ModelCreationModule_CleanMetrologyData_Page.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "8E0457CEAECFC203A2FC19EA528CABDC"
//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.18444
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace IPS.ViewsSub.ModelCreationModule {
    
    
    public partial class ModelCreationModule_CleanMetrologyData_Page : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.ComboBox ui_CM_MetrologyPoint;
        
        internal System.Windows.Controls.ComboBox ui_CM_PatternType;
        
        internal System.Windows.Controls.DataGrid ui_CM_MetrologyIndicatorList;
        
        internal System.Windows.Controls.Button ui_CM_NextStep;
        
        internal System.Windows.Controls.StackPanel ui_CM_PatternChart;
        
        internal System.Windows.Controls.StackPanel ui_CM_MetrologyPointChart;
        
        internal System.Windows.Controls.DataGrid ui_CM_AbIsoDataGrid;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/IPS;component/ViewsSub/ModelCreationModule/ModelCreationModule_CleanMetrologyDat" +
                        "a_Page.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ui_CM_MetrologyPoint = ((System.Windows.Controls.ComboBox)(this.FindName("ui_CM_MetrologyPoint")));
            this.ui_CM_PatternType = ((System.Windows.Controls.ComboBox)(this.FindName("ui_CM_PatternType")));
            this.ui_CM_MetrologyIndicatorList = ((System.Windows.Controls.DataGrid)(this.FindName("ui_CM_MetrologyIndicatorList")));
            this.ui_CM_NextStep = ((System.Windows.Controls.Button)(this.FindName("ui_CM_NextStep")));
            this.ui_CM_PatternChart = ((System.Windows.Controls.StackPanel)(this.FindName("ui_CM_PatternChart")));
            this.ui_CM_MetrologyPointChart = ((System.Windows.Controls.StackPanel)(this.FindName("ui_CM_MetrologyPointChart")));
            this.ui_CM_AbIsoDataGrid = ((System.Windows.Controls.DataGrid)(this.FindName("ui_CM_AbIsoDataGrid")));
        }
    }
}

