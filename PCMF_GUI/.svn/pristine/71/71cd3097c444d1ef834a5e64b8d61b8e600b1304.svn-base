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
using IPS.Common;

namespace IPS.Views
{
    public partial class UDDIServiceMapping : ChildWindow
    {
        ServiceMappingInfo smInfo;
        SVInfo[] svInfo;
        ProviderInfo[] providerInfo;

        public UDDIServiceMapping(ServiceManager.ServiceMappingInfo smInfo)
        {
            InitializeComponent();
            this.smInfo = smInfo;
            txtFunctionName.Text = smInfo.functionName;

            LoadService();            
        }

        #region Load Services
        void LoadService()
        {
            Shell.waitingForm.SettingMessage("");
            Shell.waitingForm.Show();

            App.proxySM.GetServiceListCompleted += new EventHandler<GetServiceListCompletedEventArgs>(proxy_GetServiceListCompleted);
            App.proxySM.GetServiceListAsync("");
        }

        void proxy_GetServiceListCompleted(object sender, GetServiceListCompletedEventArgs e)
        {
            svInfo = e.Result.ToArray();
            LoadProvider();
            Shell.waitingForm.DialogResult = false;
        }
        #endregion

        #region Load Providers
        void LoadProvider()
        {
            App.proxySM.GetProviderInfoListCompleted += new EventHandler<GetProviderInfoListCompletedEventArgs>(proxy_GetProviderInfoListCompleted);
            App.proxySM.GetProviderInfoListAsync();
        }

        void proxy_GetProviderInfoListCompleted(object sender, GetProviderInfoListCompletedEventArgs e)
        {
            providerInfo = e.Result.ToArray();
            cmbProvider.ItemsSource = providerInfo;
            if (svInfo != null)
            {
                cmbService.ItemsSource = svInfo;
                cmbProvider.SelectedValue = this.smInfo.svInfo.businesskey;
                cmbService.SelectedValue = this.smInfo.svInfo.key;
            }
        }
        #endregion

        void FilterServiceByProvider(String providerKey)
        {
            IEnumerable<SVInfo> list = (from sv in svInfo
                                        where sv.businesskey.Equals(providerKey)
                                        select sv);
            cmbService.ItemsSource = list;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            SaveServiceMapping();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void cmbProvider_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbProvider.SelectedValue!=null)
                FilterServiceByProvider(cmbProvider.SelectedValue.ToString());
        }

        void LoadServiceInfo(String serviceKey)
        {
            IEnumerable<SVInfo> list = (from sv in svInfo
                                        where sv.key.Equals(serviceKey)
                                        select sv);
            foreach (SVInfo sv in list)
            {
                txtEndpoint.Text = sv.endpoint;
                txtServiceKey.Text = sv.key;
                txtDesc.Text = smInfo.functionName;
            }
        }

        private void cmbService_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbService.SelectedValue!=null)
                LoadServiceInfo(cmbService.SelectedValue.ToString());
        }

        #region Save Service Mapping
        void SaveServiceMapping()
        {
            Shell.waitingForm.SettingMessage("Saving Service Mapping");
            Shell.waitingForm.Show();

            App.proxySM.SaveServiceMappingCompleted += new EventHandler<SaveServiceMappingCompletedEventArgs>(proxy_SaveServiceMappingCompleted);
            App.proxySM.SaveServiceMappingAsync(txtFunctionName.Text,cmbService.SelectedValue.ToString(),StateManager.Username,StateManager.UserCompany);
        }

        void proxy_SaveServiceMappingCompleted(object sender, SaveServiceMappingCompletedEventArgs e)
        {
            bool flag = e.Result;
            if (flag)
            {
                CommonValue.FunctionKeys[txtFunctionName.Text] = ((IPS.ServiceManager.SVInfo)(cmbService.SelectionBoxItem)).endpoint;
                UpdateServiceProxy();
                Shell.waitingForm.DialogResult = false;
                this.DialogResult = true;
                this.Close();
            }
            else
                MessageBox.Show("There is an error! Please try it again.");
        }
        #endregion

        #region update service proxy after updating service mapping
        void UpdateServiceProxy()
        {            
            switch (txtFunctionName.Text)
            {
                case CommonValue.DATA_ACQUISITION:
                    //Data Acquisition
                    App.proxyDA = new DataAcquisition.ServiceClient();
                    App.proxyDA.Endpoint.Binding.OpenTimeout = TimeSpan.FromSeconds(CommonValue.SERVICE_TIMEOUT);
                    App.proxyDA.Endpoint.Address = new System.ServiceModel.EndpointAddress(CommonValue.FunctionKeys[CommonValue.DATA_ACQUISITION]);
                    break;
                case CommonValue.MODEL_CREATION:
                    //Model Creation
                    App.proxyMC = new ModelCreation.ServiceClient();
                    App.proxyMC.Endpoint.Binding.OpenTimeout = TimeSpan.FromSeconds(CommonValue.SERVICE_TIMEOUT);
                    App.proxyMC.Endpoint.Address = new System.ServiceModel.EndpointAddress(CommonValue.FunctionKeys[CommonValue.MODEL_CREATION]);
                    break;
                case CommonValue.MODEL_MANAGEMENT:
                    //Model Management
                    App.proxyMM = new ModelManager.Service1Client();
                    App.proxyMM.Endpoint.Binding.OpenTimeout = TimeSpan.FromSeconds(CommonValue.SERVICE_TIMEOUT);
                    App.proxyMM.Endpoint.Address = new System.ServiceModel.EndpointAddress(CommonValue.FunctionKeys[CommonValue.MODEL_MANAGEMENT]);
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}

