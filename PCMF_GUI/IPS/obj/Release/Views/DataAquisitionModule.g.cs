﻿#pragma checksum "D:\VS_Project\本體論系統\AMCSystems20121106-更新MMbyXML\AMCSystems20121030-更新DAUserFriendly\IPS\Views\DataAquisitionModule.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "139D6EE214D555871C223C47B12B757E"
//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.18052
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------

using IPS.ViewsSub.DataAquisitionModule;
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


namespace IPS.Views {
    
    
    public partial class DataAquisitionModule : System.Windows.Controls.Page {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Border LinksBorder;
        
        internal System.Windows.Controls.TabControl ui_DATabControl;
        
        internal System.Windows.Controls.TabItem ui_HistDCP_MainTab;
        
        internal IPS.ViewsSub.DataAquisitionModule.DataAquisitionModule_HistoryDCPView_Page ui_DataAquisitionModule_HistoryDCPView_Page;
        
        internal System.Windows.Controls.TabItem ui_CNDCP_MainTab;
        
        internal IPS.ViewsSub.DataAquisitionModule.DataAquisitionModule_CreateNewDCP_Page ui_DataAquisitionModule_CreateNewDCP_Page;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/IPS;component/Views/DataAquisitionModule.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.LinksBorder = ((System.Windows.Controls.Border)(this.FindName("LinksBorder")));
            this.ui_DATabControl = ((System.Windows.Controls.TabControl)(this.FindName("ui_DATabControl")));
            this.ui_HistDCP_MainTab = ((System.Windows.Controls.TabItem)(this.FindName("ui_HistDCP_MainTab")));
            this.ui_DataAquisitionModule_HistoryDCPView_Page = ((IPS.ViewsSub.DataAquisitionModule.DataAquisitionModule_HistoryDCPView_Page)(this.FindName("ui_DataAquisitionModule_HistoryDCPView_Page")));
            this.ui_CNDCP_MainTab = ((System.Windows.Controls.TabItem)(this.FindName("ui_CNDCP_MainTab")));
            this.ui_DataAquisitionModule_CreateNewDCP_Page = ((IPS.ViewsSub.DataAquisitionModule.DataAquisitionModule_CreateNewDCP_Page)(this.FindName("ui_DataAquisitionModule_CreateNewDCP_Page")));
        }
    }
}
