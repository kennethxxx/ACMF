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
using System.Windows.Navigation;
using IPS.ServiceManager;
using System.Threading;
using IPS.Common;
namespace IPS.Views
{
    public partial class ServiceMgtModule : Page
    {
        UDDIPublishService publishForm;
        UDDISetting settingForm;
        UDDIServiceMapping mappingForm;

        //public static ServiceManager.ServiceManagerClient proxy;
        //public static UserMgtService.Service1Client proxyUser;

        public ServiceMgtModule()
        {
            InitializeComponent();

            //proxy = new ServiceManager.ServiceManagerClient();
            //proxyUser = new UserMgtService.Service1Client();

            LoadService();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void btnPublish_Click(object sender, RoutedEventArgs e)
        {
            publishForm = new UDDIPublishService();
            publishForm.Closed += (s, eargs) =>
            {
                //fix bug of silverlight 4 with childwindows ->> diable parent
                Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
                UDDIPublishService publishChildWindow = ((UDDIPublishService)s);
                if (publishChildWindow.DialogResult.Value)
                {
                    LoadService();
                }
            };
            publishForm.Show();
        }

        private void btnUDDISetting_Click(object sender, RoutedEventArgs e)
        {
            settingForm = new UDDISetting();
            settingForm.Closed += (s, eargs) =>
            {
                //fix bug of silverlight 4 with childwindows ->> diable parent
                Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
            };
            settingForm.Show(); 
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            ServiceManager.ServiceMappingInfo selectedItem = (ServiceManager.ServiceMappingInfo)dataGridMapping.SelectedItem;
            if (selectedItem != null)
            {
                mappingForm = new UDDIServiceMapping(selectedItem);
                mappingForm.Closed += (s, eargs) =>
                {
                    //fix bug of silverlight 4 with childwindows ->> diable parent
                    Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
                    UDDIServiceMapping mappingChildWindow = ((UDDIServiceMapping)s);
                    if (mappingChildWindow.DialogResult.Value)
                    {
                        LoadServiceMapping();
                    }
                };
                mappingForm.Show();
            }
        }

        #region Load Services
        public void LoadService()
        {
            Shell.waitingForm.SettingMessage("Loading Service List");
            Shell.waitingForm.Show();
            
            App.proxySM.GetServiceListCompleted += (s, e) =>
            {
                SVInfo[] collection = e.Result.ToArray();
                if (collection != null) dataGridService.ItemsSource = collection;
                Shell.waitingForm.DialogResult = false;
            };
            App.proxySM.GetServiceListAsync("");
        }

        #endregion

        /// <summary>
        /// Delete a service in UDDI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            ServiceManager.SVInfo objSVInfo = (ServiceManager.SVInfo)dataGridService.SelectedItem;
            if (objSVInfo != null)
            {
                
                if (MessageBox.Show("Do you really want to delete this service?", "Delete", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    DeleteService(objSVInfo.key, objSVInfo.name);
                }
            }
        }

        #region Delete Service

        void DeleteService(String serviceKey, string serviceName)
        {
            Shell.waitingForm.SettingMessage("Deleting The Service");
            Shell.waitingForm.Show();

            App.proxySM.DeleteAServiceCompleted += (s, e) =>
            {
                bool flag = e.Result;
                if (flag)
                {
                    //MessageBox.Show("Success!");
                    DeleteServiceOnACS(serviceName);
                }
                else
                {
                    MessageBox.Show("Not Success!");
                }
            };
            App.proxySM.DeleteAServiceAsync(serviceKey);
        }

        void DeleteServiceOnACS(string serviceName)
        {
            Shell.waitingForm.SettingMessage("Deleting The Service On ACS");
            App.proxyUM.DeleteRelyingPartyCompleted += (s, e) =>
            {
                string result = e.Result;
                MessageBox.Show(result);
                Shell.waitingForm.DialogResult = false;
                LoadService();
            };
            App.proxyUM.DeleteRelyingPartyAsync(serviceName);
        }

        #endregion

        #region Load Service Mapping
        void LoadServiceMapping()
        {
            Shell.waitingForm.SettingMessage("Loading Service Mapping");
            Shell.waitingForm.Show();
            
            App.proxySM.GetServiceMappingCompleted += (s, e) =>
            {
                ServiceMappingInfo[] collection = e.Result.ToArray();
                if (collection != null) dataGridMapping.ItemsSource = collection;
                Shell.waitingForm.DialogResult = false;
            };
            
            App.proxySM.GetServiceMappingAsync(StateManager.Username,StateManager.UserCompany);
        }

        #endregion

        private void tabServiceList_Loaded(object sender, RoutedEventArgs e)
        {
            //Get all services from UDDI Server
            LoadService();
        }

        private void tabServiceList_GotFocus(object sender, RoutedEventArgs e)
        {
            LoadService();
        }

        private void tabServiceMapping_GotFocus(object sender, RoutedEventArgs e)
        {
            LoadServiceMapping();
        }
    }
}
