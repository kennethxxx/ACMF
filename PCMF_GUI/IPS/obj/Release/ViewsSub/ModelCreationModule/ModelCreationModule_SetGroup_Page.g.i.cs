﻿#pragma checksum "C:\研究所\AMCSystems20121106-更新MMbyXML\AMCSystems20121106-更新MMbyXML\AMCSystems20121030-更新DAUserFriendly\IPS\ViewsSub\ModelCreationModule\ModelCreationModule_SetGroup_Page.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "5AB574C0DF6F81D60C8CC9259B239819"
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
    
    
    public partial class ModelCreationModule_SetGroup_Page : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.DataGrid ui_SG_PointListSetting;
        
        internal System.Windows.Controls.NumericUpDown ui_SG_setUSLPoint;
        
        internal System.Windows.Controls.NumericUpDown ui_SG_setUCLPoint;
        
        internal System.Windows.Controls.NumericUpDown ui_SG_setLCLPoint;
        
        internal System.Windows.Controls.NumericUpDown ui_SG_setLSLPoint;
        
        internal System.Windows.Controls.NumericUpDown ui_SG_setTargetPoint;
        
        internal System.Windows.Controls.Button ui_SG_SetPointSpcApply;
        
        internal System.Windows.Controls.Button ui_SG_SetPointSpcApplyAll;
        
        internal System.Windows.Controls.ComboBox ui_SG_GroupList;
        
        internal System.Windows.Controls.Button ui_SG_AddNewGroup;
        
        internal System.Windows.Controls.Button ui_SG_AddAllGroup;
        
        internal System.Windows.Controls.Button ui_SG_DelSelectedGroup;
        
        internal System.Windows.Controls.Button ui_SG_DelAllGroup;
        
        internal System.Windows.Controls.Button ui_SG_NextStep;
        
        internal System.Windows.Controls.HeaderedContentControl hccGroupPoint;
        
        internal System.Windows.Controls.ScrollViewer scrollViewer5;
        
        internal System.Windows.Controls.DataGrid ui_SG_UngroupedMetrologyList;
        
        internal System.Windows.Controls.HeaderedContentControl ui_SG_IndicatorActionHeader1;
        
        internal System.Windows.Controls.ScrollViewer scrollViewer6;
        
        internal System.Windows.Controls.DataGrid ui_SG_IndicatorActionList1;
        
        internal System.Windows.Controls.HeaderedContentControl ui_SG_IndicatorActionHeader2;
        
        internal System.Windows.Controls.ScrollViewer scrollViewer7;
        
        internal System.Windows.Controls.DataGrid ui_SG_IndicatorActionList2;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/IPS;component/ViewsSub/ModelCreationModule/ModelCreationModule_SetGroup_Page.xam" +
                        "l", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ui_SG_PointListSetting = ((System.Windows.Controls.DataGrid)(this.FindName("ui_SG_PointListSetting")));
            this.ui_SG_setUSLPoint = ((System.Windows.Controls.NumericUpDown)(this.FindName("ui_SG_setUSLPoint")));
            this.ui_SG_setUCLPoint = ((System.Windows.Controls.NumericUpDown)(this.FindName("ui_SG_setUCLPoint")));
            this.ui_SG_setLCLPoint = ((System.Windows.Controls.NumericUpDown)(this.FindName("ui_SG_setLCLPoint")));
            this.ui_SG_setLSLPoint = ((System.Windows.Controls.NumericUpDown)(this.FindName("ui_SG_setLSLPoint")));
            this.ui_SG_setTargetPoint = ((System.Windows.Controls.NumericUpDown)(this.FindName("ui_SG_setTargetPoint")));
            this.ui_SG_SetPointSpcApply = ((System.Windows.Controls.Button)(this.FindName("ui_SG_SetPointSpcApply")));
            this.ui_SG_SetPointSpcApplyAll = ((System.Windows.Controls.Button)(this.FindName("ui_SG_SetPointSpcApplyAll")));
            this.ui_SG_GroupList = ((System.Windows.Controls.ComboBox)(this.FindName("ui_SG_GroupList")));
            this.ui_SG_AddNewGroup = ((System.Windows.Controls.Button)(this.FindName("ui_SG_AddNewGroup")));
            this.ui_SG_AddAllGroup = ((System.Windows.Controls.Button)(this.FindName("ui_SG_AddAllGroup")));
            this.ui_SG_DelSelectedGroup = ((System.Windows.Controls.Button)(this.FindName("ui_SG_DelSelectedGroup")));
            this.ui_SG_DelAllGroup = ((System.Windows.Controls.Button)(this.FindName("ui_SG_DelAllGroup")));
            this.ui_SG_NextStep = ((System.Windows.Controls.Button)(this.FindName("ui_SG_NextStep")));
            this.hccGroupPoint = ((System.Windows.Controls.HeaderedContentControl)(this.FindName("hccGroupPoint")));
            this.scrollViewer5 = ((System.Windows.Controls.ScrollViewer)(this.FindName("scrollViewer5")));
            this.ui_SG_UngroupedMetrologyList = ((System.Windows.Controls.DataGrid)(this.FindName("ui_SG_UngroupedMetrologyList")));
            this.ui_SG_IndicatorActionHeader1 = ((System.Windows.Controls.HeaderedContentControl)(this.FindName("ui_SG_IndicatorActionHeader1")));
            this.scrollViewer6 = ((System.Windows.Controls.ScrollViewer)(this.FindName("scrollViewer6")));
            this.ui_SG_IndicatorActionList1 = ((System.Windows.Controls.DataGrid)(this.FindName("ui_SG_IndicatorActionList1")));
            this.ui_SG_IndicatorActionHeader2 = ((System.Windows.Controls.HeaderedContentControl)(this.FindName("ui_SG_IndicatorActionHeader2")));
            this.scrollViewer7 = ((System.Windows.Controls.ScrollViewer)(this.FindName("scrollViewer7")));
            this.ui_SG_IndicatorActionList2 = ((System.Windows.Controls.DataGrid)(this.FindName("ui_SG_IndicatorActionList2")));
        }
    }
}

