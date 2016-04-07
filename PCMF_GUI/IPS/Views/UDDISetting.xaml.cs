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

namespace IPS.Views
{
    public partial class UDDISetting : ChildWindow
    {
        public UDDISetting()
        {
            InitializeComponent();
            LoadUDDIConfiguration();
        }

        #region Load Configuration
        void LoadUDDIConfiguration()
        {
            Shell.waitingForm.SettingMessage("Loading UDDI Configuration");
            Shell.waitingForm.Show();
            
            App.proxySM.GetUDDIConnectionInfoCompleted += new EventHandler<GetUDDIConnectionInfoCompletedEventArgs>(proxy_GetUDDIConnectionInfoCompleted);
            App.proxySM.GetUDDIConnectionInfoAsync();
        }

        void proxy_GetUDDIConnectionInfoCompleted(object sender, GetUDDIConnectionInfoCompletedEventArgs e)
        {
            ConnectionInfo connInfo = e.Result;
            txtURL.Text = connInfo.url;
            txtUsername.Text = connInfo.username;
            txtPassword.Password = connInfo.password;
            Shell.waitingForm.DialogResult = false;
        }
        #endregion

        #region Save Configuration
        void SaveUDDIConfiguration()
        {
            Shell.waitingForm.SettingMessage("Saving UDDI Configuration");
            Shell.waitingForm.Show();
            
            App.proxySM.SaveUDDIConnectionInfoCompleted += new EventHandler<SaveUDDIConnectionInfoCompletedEventArgs>(proxy_SaveUDDIConnectionInfoCompleted);
            ConnectionInfo info = new ConnectionInfo();
            info.url = txtURL.Text;
            info.username = txtUsername.Text;
            info.password = txtPassword.Password;
            App.proxySM.SaveUDDIConnectionInfoAsync(info);
        }

        void proxy_SaveUDDIConnectionInfoCompleted(object sender, SaveUDDIConnectionInfoCompletedEventArgs e)
        {
            bool flag = e.Result;
            Shell.waitingForm.DialogResult = false;
        }
        #endregion

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            SaveUDDIConfiguration();
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

