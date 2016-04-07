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
using IPS.UserMgtService;

namespace IPS.Views
{
    public partial class UserManager : ChildWindow
    {
        //static UserMgtService.Service1Client proxy;   //User Management App.proxyUM

        UserInformation selectedUser = null;

        public UserManager()
        {
            InitializeComponent();

            //proxy = new UserMgtService.Service1Client();
            //proxy.Endpoint.Binding.OpenTimeout = TimeSpan.FromSeconds(CommonValue.SERVICE_TIMEOUT);

            LoadJobTitle();
        }

        public UserManager(UserInformation userInfo)
        {
            //Edit
            InitializeComponent();

            //proxy = new UserMgtService.Service1Client();
            //proxy.Endpoint.Binding.OpenTimeout = TimeSpan.FromSeconds(CommonValue.SERVICE_TIMEOUT);

            this.selectedUser = userInfo;
            LoadEditUserData(userInfo);

            txtUsername.IsEnabled = false;

            LoadJobTitle();            
        }

        void LoadJobTitle()
        {
            Shell.waitingForm.SettingMessage("");
            Shell.waitingForm.Show();

            App.proxyUM.getAllRolesCompleted += (s, e) =>
            {
                Shell.waitingForm.Close();
                cmbJobTitle.ItemsSource = e.Result.ToArray();
                if (selectedUser != null)
                    cmbJobTitle.SelectedValue = selectedUser.userRole;
                else 
                    cmbJobTitle.SelectedIndex = 0;
            };
            App.proxyUM.getAllRolesAsync();
        }

        void LoadEditUserData(UserInformation selectedUser)
        {
            txtUsername.Text = selectedUser.userName;
            txtPassword.Password = selectedUser.userPassword;
            txtEmployeeID.Text = selectedUser.employeeID;
            txtEmail.Text = selectedUser.userEmail;
            txtPhone.Text = selectedUser.userPhone;
            txtAddress.Text = selectedUser.userAddress;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password;
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string address = txtAddress.Text.Trim();
            string employeeID = txtEmployeeID.Text.Trim();
            string role = cmbJobTitle.SelectedValue.ToString();

            if (selectedUser == null)
            {
                if (username != "" && password != "")
                {
                    Shell.waitingForm.SettingMessage("Create User");
                    Shell.waitingForm.Show();
                    
                    App.proxyUM.registerCloudUserCompleted += (s, eargs) =>
                    {
                        Shell.waitingForm.Close();
                        if (eargs.Result)
                        {
                            MessageBox.Show("Successful.");
                            this.DialogResult = true;
                        }
                        else
                        {
                            MessageBox.Show("Fail.");
                        }
                    };
                    App.proxyUM.registerCloudUserAsync(username, password, phone, email, address, employeeID, role);
                }
                else
                {
                    MessageBox.Show("Please enter username and password.");
                }
            }
            else
            {
                //Edit
                if (password != "")
                {
                    Shell.waitingForm.SettingMessage("Update User");
                    Shell.waitingForm.Show();
                    
                    App.proxyUM.modeifyAccountCompleted += (s, eargs) =>
                    {
                        Shell.waitingForm.Close();
                        if (eargs.Result)
                        {
                            this.DialogResult = true;
                        }
                        else
                        {
                            MessageBox.Show("Fail!");
                        }
                    };
                    App.proxyUM.modeifyAccountAsync(username, password, selectedUser.userPassword, phone, email, address, employeeID, role, selectedUser.userRole);
                }
                else
                {
                    MessageBox.Show("Please enter password.");
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

