﻿#pragma checksum "D:\Huy.Q.Tran\CloudProjectSVN\CloudProject\SourceCode\IPS\IPS\Views\DataAquisitionSetting.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A2C1F1DD4A98DC6E25452E580CB57132"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
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
    
    
    public partial class DataAquisitionSetting : System.Windows.Controls.ChildWindow {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock txtVMachine;
        
        internal System.Windows.Controls.TextBlock txtEquipment;
        
        internal System.Windows.Controls.TextBlock txtSensor;
        
        internal System.Windows.Controls.RadioButton radManual;
        
        internal System.Windows.Controls.RadioButton radPeriod;
        
        internal System.Windows.Controls.StackPanel spPeriod;
        
        internal System.Windows.Controls.TextBox txtHours;
        
        internal System.Windows.Controls.Button CancelButton;
        
        internal System.Windows.Controls.Button OKButton;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/IPS;component/Views/DataAquisitionSetting.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.txtVMachine = ((System.Windows.Controls.TextBlock)(this.FindName("txtVMachine")));
            this.txtEquipment = ((System.Windows.Controls.TextBlock)(this.FindName("txtEquipment")));
            this.txtSensor = ((System.Windows.Controls.TextBlock)(this.FindName("txtSensor")));
            this.radManual = ((System.Windows.Controls.RadioButton)(this.FindName("radManual")));
            this.radPeriod = ((System.Windows.Controls.RadioButton)(this.FindName("radPeriod")));
            this.spPeriod = ((System.Windows.Controls.StackPanel)(this.FindName("spPeriod")));
            this.txtHours = ((System.Windows.Controls.TextBox)(this.FindName("txtHours")));
            this.CancelButton = ((System.Windows.Controls.Button)(this.FindName("CancelButton")));
            this.OKButton = ((System.Windows.Controls.Button)(this.FindName("OKButton")));
        }
    }
}

