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
using IPS.ServiceManager;
using IPS.UserMgtService;
using System.Collections.ObjectModel;
using IPS.BPELInvoker;

namespace IPS.Views
{
    public partial class LoginModule : ChildWindow
    {
        private int DebugModeOpenCount = 0;
        public bool IsOpenDebugMode = false;       // 顯示Service URL
        private bool IsOpenLocalDebugMode = false;  // 顯示並使用Local Service URL 

        public LoginModule()
        {
            InitializeComponent();
            DebugModeOpenCount = 0;
            IsOpenDebugMode = false;
            IsOpenLocalDebugMode = false;
            ui_LoginErrorString.Text = "";
            ui_LoginErrorString.Visibility = Visibility.Collapsed;
        }

        private void ui_ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            BPELInvoker.Service1Client client = new BPELInvoker.Service1Client();
            ui_LoginErrorString.Text = "";
            ui_LoginErrorString.Visibility = Visibility.Collapsed;

            if (!"".Equals(ui_UserName.Text.Trim()))
            {
                if (IsOpenLocalDebugMode)
                {
                    if (!SetServiceEndpoint(
                        CommonValue.FunctionKeys[CommonValue.DATA_ACQUISITION],
                        CommonValue.FunctionKeys[CommonValue.MODEL_CREATION],
                        CommonValue.FunctionKeys[CommonValue.MODEL_MANAGEMENT],
                        CommonValue.ONTOLOGY,
                        CommonValue.ONTOLOGYTOC)
                        )
                    {
                        //ui_LoginErrorString.Text = "Setting Local Service Configuration Fail!";
                        ui_LoginErrorString.Text = "Failed!";
                        ui_LoginErrorString.Visibility = Visibility.Visible;
                    }else
                        this.DialogResult = true;
                }
                else 
                {
                    // Shell.waitingForm.SettingMessage("Authenticating");
                    // Shell.waitingForm.Show();
                    // App.proxyUM.validateAccountCompleted += new EventHandler<validateAccountCompletedEventArgs>(validateAccountCompletedEventHandler);
                    // App.proxyUM.validateAccountAsync(txtUsername.Text.Trim(), txtPassword.Password.Trim());

                    //如果要使用上面的Valid Account功能 要將GetServiceMapping();註解
                    GetServiceMapping();
                }
            }
            else
            {
                //ui_LoginErrorString.Text = "User name is Empty!";
                ui_LoginErrorString.Text = "使用者名稱不能空值!";
                ui_LoginErrorString.Visibility = Visibility.Visible;
            }
        }

        private void validateAccountCompletedEventHandler(object s, validateAccountCompletedEventArgs se)
        {
            App.proxyUM.validateAccountCompleted -= new EventHandler<validateAccountCompletedEventArgs>(validateAccountCompletedEventHandler);
            Shell.waitingForm.Close();

            bool IsValidateAccount = false;
            try
            {
                IsValidateAccount = se.Result;
            }
            catch (System.Exception)
            {
                IsValidateAccount = false;
            }

            if (IsValidateAccount)
            {
                //Get user key for accessing ACS purposes
                //GetUserKey();
                Shell.waitingForm.Close();
                GetServiceMapping();
            }
            else 
            {
                Shell.waitingForm.Close();
                ui_LoginErrorString.Visibility = Visibility.Visible;
            }
        }

        void GetServiceMapping()
        {
            //Load service configuration
            //Shell.waitingForm.SettingMessage("Loading Service Configuration");
            Shell.waitingForm.SettingMessage("Login to AMC System");
            Shell.waitingForm.Show();

            App.proxySM.GetServiceMappingCompleted += new EventHandler<GetServiceMappingCompletedEventArgs>(GetServiceMappingCompletedEventHandler);
            App.proxySM.GetServiceMappingAsync(ui_UserName.Text,ui_UserCompany.Text);            
        }

        private void GetServiceMappingCompletedEventHandler(object sender, GetServiceMappingCompletedEventArgs e)
        {
            App.proxySM.GetServiceMappingCompleted -= new EventHandler<GetServiceMappingCompletedEventArgs>(GetServiceMappingCompletedEventHandler);
            bool IsGetServiceSuccess = false;
            ObservableCollection<ServiceMappingInfo> collection = null;

          
           
                try
                {
                    collection = e.Result;
                    IsGetServiceSuccess = true;

                }
                catch (System.Exception)
                {
                   // ui_LoginErrorString.Text = "Load Service Configuration Fail!";
                    ui_LoginErrorString.Text = "載入服務組態已失效!";
                    ui_LoginErrorString.Visibility = Visibility.Visible;
                    IsGetServiceSuccess = false;
                }

                Shell.waitingForm.Close();

                if (IsGetServiceSuccess)
                {
                  //  Shell.waitingForm.SettingMessage("Setting Service Configuration");
                    Shell.waitingForm.SettingMessage("設定服務組態中");
                    Shell.waitingForm.Show();
                    if (collection == null)
                    {
                      //  ui_LoginErrorString.Text = "This account is not exist!";
                        ui_LoginErrorString.Text = "此帳號不存在!";
                        ui_LoginErrorString.Visibility = Visibility.Visible;
                        IsGetServiceSuccess = false;
                        Shell.waitingForm.Close();
                    }
                    if (collection != null)
                    {
                        foreach (ServiceMappingInfo sv in collection)
                        {
                            if (!CommonValue.FunctionKeys.ContainsKey(sv.functionName))
                                CommonValue.FunctionKeys.Add(sv.functionName, sv.svInfo.endpoint);
                            else
                                CommonValue.FunctionKeys[sv.functionName] = sv.svInfo.endpoint;
                        }
                    }

                    if (!SetServiceEndpoint(
                            CommonValue.FunctionKeys[CommonValue.DATA_ACQUISITION],
                            CommonValue.FunctionKeys[CommonValue.MODEL_CREATION],
                            CommonValue.FunctionKeys[CommonValue.MODEL_MANAGEMENT],
                            CommonValue.FunctionKeys[CommonValue.ONTOLOGY],
                            CommonValue.FunctionKeys[CommonValue.ONTOLOGYTOC])
                            )
                    {
                       // ui_LoginErrorString.Text = "Setting Service Configuration Fail!";
                        ui_LoginErrorString.Text = "設定中服務組態已失效!";
                        ui_LoginErrorString.Visibility = Visibility.Visible;
                        IsGetServiceSuccess = false;
                    }

                    Shell.waitingForm.Close();
                }

                // 返回
                if (IsGetServiceSuccess || IsOpenDebugMode)
                {
                    if (!IsGetServiceSuccess)
                    {
                    //    MessageBox.Show("Load Service Configuration Fail.", "Error", MessageBoxButton.OK);
                        MessageBox.Show("載入服務組態已失效.", "錯誤訊息", MessageBoxButton.OK);
                    }
                    this.DialogResult = true;
                }
            }


        bool SetServiceEndpoint(string DA_URL, string MC_URL, string MM_URL, string OT_URL, string OTTOC_URL)
        {
            try
            {
                //Data Acquisition
                App.proxyDA = new DataAcquisition.ServiceClient();
                App.proxyDA.Endpoint.Binding.OpenTimeout = TimeSpan.FromSeconds(CommonValue.SERVICE_TIMEOUT);
                App.proxyDA.Endpoint.Address = new System.ServiceModel.EndpointAddress(DA_URL);

                //Model Creation
                App.proxyMC = new ModelCreation.ServiceClient();
                App.proxyMC.Endpoint.Binding.OpenTimeout = TimeSpan.FromSeconds(CommonValue.SERVICE_TIMEOUT);
                App.proxyMC.Endpoint.Address = new System.ServiceModel.EndpointAddress(MC_URL);

                //Model Management
                App.proxyMM = new ModelManager.Service1Client();
                App.proxyMM.Endpoint.Binding.OpenTimeout = TimeSpan.FromSeconds(CommonValue.SERVICE_TIMEOUT);
                App.proxyMM.Endpoint.Address = new System.ServiceModel.EndpointAddress(MM_URL);

                App.proxyOT = new Ontology.Service1Client();
                App.proxyOT.Endpoint.Binding.OpenTimeout = TimeSpan.FromSeconds(CommonValue.SERVICE_TIMEOUT);
                App.proxyOT.Endpoint.Address = new System.ServiceModel.EndpointAddress(OT_URL);

                App.proxyOTTOC = new Ontology.Service1Client();
                App.proxyOTTOC.Endpoint.Binding.OpenTimeout = TimeSpan.FromSeconds(CommonValue.SERVICE_TIMEOUT);
                App.proxyOTTOC.Endpoint.Address = new System.ServiceModel.EndpointAddress(OTTOC_URL);

                return true;
            }
            catch (Exception)
            {
            	
            }
            return false;
        }

        private void imgKey_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            DebugModeOpenCount++;

            if (DebugModeOpenCount>2)
            {
                IsOpenDebugMode = true;
                this.Title = "Login To IPS(Debug Mode)";
            }

            if (DebugModeOpenCount > 4)
            {
                IsOpenDebugMode = true;
                IsOpenLocalDebugMode = true;
                this.Title = "Login To IPS(Local Debug Mode)";
            }
        }

    }
}

