﻿#pragma checksum "D:\20120903-update GUI word\AMCSystems\IPS\OMC\KM_Ontology_Edit.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "933234AB2BA8041801BF77284D443C50"
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
    
    
    public partial class KM_Ontology_Edit : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Label label1;
        
        internal System.Windows.Controls.TabControl PE_ti_control;
        
        internal System.Windows.Controls.TabItem ti_class;
        
        internal System.Windows.Shapes.Rectangle rectangle2;
        
        internal System.Windows.Controls.Label label2;
        
        internal System.Windows.Controls.DataGrid DG_Edit_Class;
        
        internal System.Windows.Controls.Button BT_ClassAdd;
        
        internal System.Windows.Controls.Button BT_Edit_Class_F;
        
        internal System.Windows.Controls.TabItem ti_property;
        
        internal System.Windows.Shapes.Rectangle rectangle3;
        
        internal System.Windows.Controls.Label label3;
        
        internal System.Windows.Controls.DataGrid DG_Edit_Property;
        
        internal System.Windows.Controls.Button BT_PropertyAdd;
        
        internal System.Windows.Controls.Button BT_Edit_Property_F;
        
        internal System.Windows.Controls.TabItem ti_instance;
        
        internal System.Windows.Shapes.Rectangle rectangle4;
        
        internal System.Windows.Controls.Label label5;
        
        internal System.Windows.Controls.DataGrid DG_Edit_Instance;
        
        internal System.Windows.Controls.Button BT_InstanceAdd;
        
        internal System.Windows.Controls.Button BT_Edit_Instance_F;
        
        internal System.Windows.Controls.TabItem ti_constraint;
        
        internal System.Windows.Shapes.Rectangle rectangle5;
        
        internal System.Windows.Controls.Label label6;
        
        internal System.Windows.Controls.DataGrid DG_Edit_Constraint;
        
        internal System.Windows.Controls.Button BT_ConstraintAdd;
        
        internal System.Windows.Controls.Button BT_Edit_Constraint_F;
        
        internal System.Windows.Shapes.Rectangle rectangle1;
        
        internal System.Windows.Controls.Label label4;
        
        internal System.Windows.Controls.TextBlock textBlock6;
        
        internal System.Windows.Controls.ComboBox CB_DBSelect;
        
        internal System.Windows.Controls.TextBox textBox1;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/IPS;component/OMC/KM_Ontology_Edit.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.label1 = ((System.Windows.Controls.Label)(this.FindName("label1")));
            this.PE_ti_control = ((System.Windows.Controls.TabControl)(this.FindName("PE_ti_control")));
            this.ti_class = ((System.Windows.Controls.TabItem)(this.FindName("ti_class")));
            this.rectangle2 = ((System.Windows.Shapes.Rectangle)(this.FindName("rectangle2")));
            this.label2 = ((System.Windows.Controls.Label)(this.FindName("label2")));
            this.DG_Edit_Class = ((System.Windows.Controls.DataGrid)(this.FindName("DG_Edit_Class")));
            this.BT_ClassAdd = ((System.Windows.Controls.Button)(this.FindName("BT_ClassAdd")));
            this.BT_Edit_Class_F = ((System.Windows.Controls.Button)(this.FindName("BT_Edit_Class_F")));
            this.ti_property = ((System.Windows.Controls.TabItem)(this.FindName("ti_property")));
            this.rectangle3 = ((System.Windows.Shapes.Rectangle)(this.FindName("rectangle3")));
            this.label3 = ((System.Windows.Controls.Label)(this.FindName("label3")));
            this.DG_Edit_Property = ((System.Windows.Controls.DataGrid)(this.FindName("DG_Edit_Property")));
            this.BT_PropertyAdd = ((System.Windows.Controls.Button)(this.FindName("BT_PropertyAdd")));
            this.BT_Edit_Property_F = ((System.Windows.Controls.Button)(this.FindName("BT_Edit_Property_F")));
            this.ti_instance = ((System.Windows.Controls.TabItem)(this.FindName("ti_instance")));
            this.rectangle4 = ((System.Windows.Shapes.Rectangle)(this.FindName("rectangle4")));
            this.label5 = ((System.Windows.Controls.Label)(this.FindName("label5")));
            this.DG_Edit_Instance = ((System.Windows.Controls.DataGrid)(this.FindName("DG_Edit_Instance")));
            this.BT_InstanceAdd = ((System.Windows.Controls.Button)(this.FindName("BT_InstanceAdd")));
            this.BT_Edit_Instance_F = ((System.Windows.Controls.Button)(this.FindName("BT_Edit_Instance_F")));
            this.ti_constraint = ((System.Windows.Controls.TabItem)(this.FindName("ti_constraint")));
            this.rectangle5 = ((System.Windows.Shapes.Rectangle)(this.FindName("rectangle5")));
            this.label6 = ((System.Windows.Controls.Label)(this.FindName("label6")));
            this.DG_Edit_Constraint = ((System.Windows.Controls.DataGrid)(this.FindName("DG_Edit_Constraint")));
            this.BT_ConstraintAdd = ((System.Windows.Controls.Button)(this.FindName("BT_ConstraintAdd")));
            this.BT_Edit_Constraint_F = ((System.Windows.Controls.Button)(this.FindName("BT_Edit_Constraint_F")));
            this.rectangle1 = ((System.Windows.Shapes.Rectangle)(this.FindName("rectangle1")));
            this.label4 = ((System.Windows.Controls.Label)(this.FindName("label4")));
            this.textBlock6 = ((System.Windows.Controls.TextBlock)(this.FindName("textBlock6")));
            this.CB_DBSelect = ((System.Windows.Controls.ComboBox)(this.FindName("CB_DBSelect")));
            this.textBox1 = ((System.Windows.Controls.TextBox)(this.FindName("textBox1")));
        }
    }
}

