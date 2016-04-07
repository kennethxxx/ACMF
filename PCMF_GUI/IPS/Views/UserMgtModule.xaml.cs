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
using IPS.Common;

namespace IPS.Views
{
    public partial class UserMgtModule : Page
    {
        //static UserMgtService.Service1Client proxy;   //User Management Proxy

        List<UserMgtService.UserInformation> userList;

        UserManager userManagerForm;

        public UserMgtModule()
        {
            InitializeComponent();

            //proxy = new UserMgtService.Service1Client();
            //proxy.Endpoint.Binding.OpenTimeout = TimeSpan.FromSeconds(CommonValue.SERVICE_TIMEOUT);

            LoadUserList();
        }

        void LoadUserList()
        {
            Shell.waitingForm.SettingMessage("Loading User List");
            Shell.waitingForm.Show();
            App.proxyUM.getUserInformationCompleted += (s, e) =>
            {
                userList = e.Result.ToList();
                dataGridUser.ItemsSource = userList;
                Shell.waitingForm.Close();
            };
            App.proxyUM.getUserInformationAsync();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            userManagerForm = new UserManager();
            //fix bug of silverlight 4 with childwindows ->> disable parent
            userManagerForm.Closed += (s, eargs) => { 
                Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
                UserManager form = ((UserManager)s);
                if(form.DialogResult.Value)
                    LoadUserList();
            };
            userManagerForm.Show();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            UserMgtService.UserInformation selectedUser = (UserMgtService.UserInformation)dataGridUser.SelectedItem;

            if (selectedUser != null)
            {
                selectedUser = getUserInformation(selectedUser.userName);
                userManagerForm = new UserManager(selectedUser);
                //fix bug of silverlight 4 with childwindows ->> disable parent
                userManagerForm.Closed += (s, eargs) =>
                {
                    Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
                    UserManager form = ((UserManager)s);
                    if (form.DialogResult.Value)
                        LoadUserList();
                };
                userManagerForm.Show();
            }else
                MessageBox.Show("Please choose a user from the list!");
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

            UserMgtService.UserInformation selectedUser = (UserMgtService.UserInformation)dataGridUser.SelectedItem;

            if (selectedUser != null)
            {
                if (MessageBox.Show("Do you really want to delete this user?", "", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    selectedUser = getUserInformation(selectedUser.userName);
                    Shell.waitingForm.SettingMessage("Deleting User");
                    Shell.waitingForm.Show();
                    App.proxyUM.deleteAccountCompleted += (s, eargs) =>
                    {
                        Shell.waitingForm.Close();
                        if (eargs.Result)
                        {
                            Shell.waitingForm.Close();
                            LoadUserList();
                        }
                        else
                        {
                            MessageBox.Show("Not success!");
                        }
                    };
                    App.proxyUM.deleteAccountAsync(selectedUser.userName, selectedUser.userPassword);
                }
            }
            else
            {
                MessageBox.Show("Please choose a user from the list!");
            }
        }

        UserMgtService.UserInformation getUserInformation(string username)
        {
            foreach (UserMgtService.UserInformation userInfo in userList)
            {
                if (userInfo.userName == username)
                    return userInfo;
            }
            return null;
        }
    }
}
