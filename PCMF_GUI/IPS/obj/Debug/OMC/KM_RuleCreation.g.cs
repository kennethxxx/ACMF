﻿#pragma checksum "D:\20120903-update GUI word\AMCSystems\IPS\OMC\KM_RuleCreation.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A6B3689CFF7405F42A386A5B7776167B"
//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.269
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


namespace OMC.Views {
    
    
    public partial class KM_RuleCreation : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Label label1;
        
        internal System.Windows.Shapes.Rectangle rectangle2;
        
        internal System.Windows.Controls.Label label2;
        
        internal System.Windows.Shapes.Rectangle rectangle1;
        
        internal System.Windows.Controls.Label label3;
        
        internal System.Windows.Controls.TextBlock textBlock1;
        
        internal System.Windows.Controls.TextBox TB_Rule_Name;
        
        internal System.Windows.Controls.TextBlock textBlock5;
        
        internal System.Windows.Controls.TextBlock textBlock17;
        
        internal System.Windows.Controls.TextBox TB_Rule_Description;
        
        internal System.Windows.Controls.TextBox TB_Rule_SWRL;
        
        internal System.Windows.Controls.Button BT_Rule_Add;
        
        internal System.Windows.Shapes.Rectangle rectangle3;
        
        internal System.Windows.Controls.Label label4;
        
        internal System.Windows.Controls.DataGrid DG_Rule;
        
        internal System.Windows.Controls.Button BT_Rule_Delete;
        
        internal System.Windows.Controls.TextBox textBox1;
        
        internal System.Windows.Controls.TextBlock textBlock2;
        
        internal System.Windows.Controls.ComboBox CB_DBSelect;
        
        internal System.Windows.Controls.Button BT_Rule_Sumbit;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/IPS;component/OMC/KM_RuleCreation.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.label1 = ((System.Windows.Controls.Label)(this.FindName("label1")));
            this.rectangle2 = ((System.Windows.Shapes.Rectangle)(this.FindName("rectangle2")));
            this.label2 = ((System.Windows.Controls.Label)(this.FindName("label2")));
            this.rectangle1 = ((System.Windows.Shapes.Rectangle)(this.FindName("rectangle1")));
            this.label3 = ((System.Windows.Controls.Label)(this.FindName("label3")));
            this.textBlock1 = ((System.Windows.Controls.TextBlock)(this.FindName("textBlock1")));
            this.TB_Rule_Name = ((System.Windows.Controls.TextBox)(this.FindName("TB_Rule_Name")));
            this.textBlock5 = ((System.Windows.Controls.TextBlock)(this.FindName("textBlock5")));
            this.textBlock17 = ((System.Windows.Controls.TextBlock)(this.FindName("textBlock17")));
            this.TB_Rule_Description = ((System.Windows.Controls.TextBox)(this.FindName("TB_Rule_Description")));
            this.TB_Rule_SWRL = ((System.Windows.Controls.TextBox)(this.FindName("TB_Rule_SWRL")));
            this.BT_Rule_Add = ((System.Windows.Controls.Button)(this.FindName("BT_Rule_Add")));
            this.rectangle3 = ((System.Windows.Shapes.Rectangle)(this.FindName("rectangle3")));
            this.label4 = ((System.Windows.Controls.Label)(this.FindName("label4")));
            this.DG_Rule = ((System.Windows.Controls.DataGrid)(this.FindName("DG_Rule")));
            this.BT_Rule_Delete = ((System.Windows.Controls.Button)(this.FindName("BT_Rule_Delete")));
            this.textBox1 = ((System.Windows.Controls.TextBox)(this.FindName("textBox1")));
            this.textBlock2 = ((System.Windows.Controls.TextBlock)(this.FindName("textBlock2")));
            this.CB_DBSelect = ((System.Windows.Controls.ComboBox)(this.FindName("CB_DBSelect")));
            this.BT_Rule_Sumbit = ((System.Windows.Controls.Button)(this.FindName("BT_Rule_Sumbit")));
        }
    }
}
