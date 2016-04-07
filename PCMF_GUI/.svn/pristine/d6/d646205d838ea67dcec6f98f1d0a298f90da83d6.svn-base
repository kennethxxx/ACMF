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
using IPS.ServiceManager;
using System.IO;

namespace IPS.Views
{
    public partial class UDDIPublishService : ChildWindow
    {
        //public OpenFileDialog fileDialog = null;
        //byte[] fileBuffer;

        public UDDIPublishService()
        {
            InitializeComponent();
            LoadProvider();
            LoadServiceGroup();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            SaveService();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        #region Load Provider

        /// <summary>
        /// Load provider list for setting UDDI
        /// </summary>
        void LoadProvider()
        {
            App.proxySM.GetProviderInfoListCompleted += (s, e) =>
            {
                ProviderInfo[] collection = e.Result.ToArray();
                cmbProvider.ItemsSource = collection;
            };
            App.proxySM.GetProviderInfoListAsync();
        }

        /// <summary>
        /// Load service group list for setting ACS
        /// </summary>
        void LoadServiceGroup()
        {
            App.proxyUM.getAllServiceGroupCompleted += (s, e) =>
            {
                string[] collection = e.Result.ToArray();
                cmbServiceGroup.ItemsSource = collection;
            };
            App.proxyUM.getAllServiceGroupAsync();
        }

        #endregion

        /// <summary>
        /// Save a service to UDDI server then save to ACS
        /// </summary>
        void SaveService()
        {
            string strProvider = cmbProvider.SelectedValue.ToString();
            string strServiceName = txtServiceName.Text.Trim();
            string strEndpoint = txtEndpoint.Text.Trim();
            string strServiceGroup = cmbServiceGroup.SelectedValue.ToString();
            //string strCertPass = txtPassword.Password;

            Shell.waitingForm.SettingMessage("Saving Service");
            Shell.waitingForm.Show();
            App.proxySM.PublishAServiceCompleted += (s, e) =>
            {
                bool flag = e.Result;
                Shell.waitingForm.DialogResult = false;
                if (flag)
                {
                    //MessageBox.Show("Success!");
                    //this.DialogResult = true;
                    SaveACSService(strServiceName, strEndpoint, strServiceGroup);
                }
                else
                    MessageBox.Show("Not Success!");
            };
            App.proxySM.PublishAServiceAsync(strProvider, strServiceName, strEndpoint);
        }

        /// <summary>
        /// Register a service to ACS(access control service)
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="endpoint"></param>
        /// <param name="certFile"></param>
        /// <param name="certpassword"></param>
        /// <param name="serviceGroup"></param>
        void SaveACSService(string serviceName, string endpoint, string serviceGroup)
        {
            Shell.waitingForm.Show();
            App.proxyUM.CreateNewRelyingPartyCompleted += (s, e) =>
            {
                string result = e.Result;
                MessageBox.Show(result);
                Shell.waitingForm.DialogResult = false;
                this.DialogResult = true;
            };
            App.proxyUM.CreateNewRelyingPartyAsync(serviceName, endpoint, serviceGroup);
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            //fileDialog = new OpenFileDialog();
            //fileDialog.Multiselect = false;
            //fileDialog.Filter = "Personal Information Exchange (.pfx)|*.pfx";
            //bool? retval = fileDialog.ShowDialog();
            //if (retval != null && retval == true)
            //{
            //    string filename = fileDialog.File.Name;
            //    Stream strm = fileDialog.File.OpenRead();
            //    fileBuffer = new byte[strm.Length];
            //    strm.Read(fileBuffer, 0, (int)strm.Length);
            //    strm.Dispose();
            //    strm.Close();
            //    txtFilePath.Text = filename;
            //}
        }
    }
}

