﻿#pragma checksum "D:\VS_Project\本體論系統\AMCSystems20121106-更新MMbyXML\AMCSystems20121030-更新DAUserFriendly\IPS\Views\ServiceMgtModule.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E58DF214DD124AEDC011F0BD81D71043"
//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.18052
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


namespace IPS.Views {
    
    
    public partial class ServiceMgtModule : System.Windows.Controls.Page {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Border LinksBorder;
        
        internal System.Windows.Controls.TabControl tabControl1;
        
        internal System.Windows.Controls.TabItem tabServiceList;
        
        internal System.Windows.Controls.DataGrid dataGridService;
        
        internal System.Windows.Controls.Button btnPublish;
        
        internal System.Windows.Controls.Button btnDelete;
        
        internal System.Windows.Controls.Button btnUDDISetting;
        
        internal System.Windows.Controls.TabItem tabServiceMapping;
        
        internal System.Windows.Controls.DataGrid dataGridMapping;
        
        internal System.Windows.Controls.Button btnChange;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/IPS;component/Views/ServiceMgtModule.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.LinksBorder = ((System.Windows.Controls.Border)(this.FindName("LinksBorder")));
            this.tabControl1 = ((System.Windows.Controls.TabControl)(this.FindName("tabControl1")));
            this.tabServiceList = ((System.Windows.Controls.TabItem)(this.FindName("tabServiceList")));
            this.dataGridService = ((System.Windows.Controls.DataGrid)(this.FindName("dataGridService")));
            this.btnPublish = ((System.Windows.Controls.Button)(this.FindName("btnPublish")));
            this.btnDelete = ((System.Windows.Controls.Button)(this.FindName("btnDelete")));
            this.btnUDDISetting = ((System.Windows.Controls.Button)(this.FindName("btnUDDISetting")));
            this.tabServiceMapping = ((System.Windows.Controls.TabItem)(this.FindName("tabServiceMapping")));
            this.dataGridMapping = ((System.Windows.Controls.DataGrid)(this.FindName("dataGridMapping")));
            this.btnChange = ((System.Windows.Controls.Button)(this.FindName("btnChange")));
        }
    }
}

