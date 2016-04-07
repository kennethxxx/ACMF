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
    public partial class DataAquisitionSetting : ChildWindow
    {
        public DataAquisitionSetting(DataAcquisition.vMachineState selectedVM)
        {
            InitializeComponent();

            txtVMachine.Text = selectedVM.vMachineName;
            txtEquipment.Text = selectedVM.equipmentName;
            txtSensor.Text = selectedVM.sensor;
        }

        void StartDataAcquisitionWithManual(string vmName, string equipmentName, string sensorName)
        {
            Shell.waitingForm.SettingMessage("Acquisition By Manual");
            Shell.waitingForm.Show();

            App.proxyDA.uploadSTDBCompleted += (s, e) =>
            {
                MessageBox.Show(e.Result);
                Shell.waitingForm.Close();
                this.DialogResult = true;
            };
            App.proxyDA.uploadSTDBAsync(vmName, equipmentName, sensorName);
        }

        void StartDataAcquisitionWithPeriodTime(string vmName, string equipmentName, string sensorName, string hour)
        {
            Shell.waitingForm.SettingMessage("Acquisition By Time");
            Shell.waitingForm.Show();

            App.proxyDA.startPeriodTimeUploadSTDBCompleted += (s, e) =>
            {
                MessageBox.Show(e.Result);
                Shell.waitingForm.Close();
                this.DialogResult = true;
            };
            App.proxyDA.startPeriodTimeUploadSTDBAsync(vmName, equipmentName, sensorName, hour);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (radManual.IsChecked.Value)
                StartDataAcquisitionWithManual(txtVMachine.Text, txtEquipment.Text, txtSensor.Text);
            else
                StartDataAcquisitionWithPeriodTime(txtVMachine.Text, txtEquipment.Text, txtSensor.Text, txtHours.Text.Trim());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void radManual_Checked(object sender, RoutedEventArgs e)
        {
            spPeriod.Visibility = Visibility.Collapsed;
        }

        private void radPeriod_Checked(object sender, RoutedEventArgs e)
        {
            spPeriod.Visibility = Visibility.Visible;
        }
    }
}

