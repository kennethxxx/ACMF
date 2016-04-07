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
using IPS.Views;
using IPS.Common;
using System.Windows.Navigation;
using System.Net.Browser;
using IPS.OntologyService;

namespace IPS
{
    public partial class App : Application
    {
        public static DataAcquisition.ServiceClient proxyDA;   //Data Acquisition
        public static ModelCreation.ServiceClient proxyMC;  //Model Creation
        public static ModelManager.Service1Client proxyMM;   //Model Management
        public static UserMgtService.Service1Client proxyUM;   //User Management
        public static ServiceManager.ServiceManagerClient proxySM;  //Service Management
        public static Ontology.Service1Client proxyOT;
        public static Ontology.Service1Client proxyOTTOC;
        public static IPS.RecommendedCuttingTool.CuttingToolInterfaceClient proxyCuttingTool;

        public static IPS.OntologyService.OntologyServiceClient OntologyService;   //宣告Ontology的服務
        public static IPS.UploadFileService.Service1Client UploadService = null;  //呼叫上傳的服務
        public static IPS.MachineInfomation.Service1Client GetMachineInfoService;


        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();

            //Initialize proxy

            //Data Acquisition
            proxyDA = new DataAcquisition.ServiceClient();
            proxyDA.Endpoint.Binding.OpenTimeout = TimeSpan.FromSeconds(CommonValue.SERVICE_TIMEOUT);

            //proxyDA.ClientCredentials.UserName.UserName = "admin";
            //proxyDA.ClientCredentials.UserName.Password = "admin";

            //Model Creation
            proxyMC = new ModelCreation.ServiceClient();
            proxyMC.Endpoint.Binding.OpenTimeout = TimeSpan.FromSeconds(CommonValue.SERVICE_TIMEOUT);

            //Model Management
            proxyMM = new ModelManager.Service1Client();
            proxyMM.Endpoint.Binding.OpenTimeout = TimeSpan.FromSeconds(CommonValue.SERVICE_TIMEOUT);

            //User Management
            proxyUM = new UserMgtService.Service1Client();
            proxyUM.Endpoint.Binding.OpenTimeout = TimeSpan.FromSeconds(CommonValue.SERVICE_TIMEOUT);

            //Service Management
            proxySM = new ServiceManager.ServiceManagerClient();
            proxySM.Endpoint.Binding.OpenTimeout = TimeSpan.FromSeconds(CommonValue.SERVICE_TIMEOUT);

            //Ontology
            proxyOT = new Ontology.Service1Client();
            proxyOT.Endpoint.Binding.OpenTimeout = TimeSpan.FromSeconds(CommonValue.SERVICE_TIMEOUT);

            //OntologyTOC
            proxyOTTOC = new Ontology.Service1Client();
            proxyOTTOC.Endpoint.Binding.OpenTimeout = TimeSpan.FromSeconds(CommonValue.SERVICE_TIMEOUT);

            OntologyService = new OntologyServiceClient();
            UploadService = new IPS.UploadFileService.Service1Client();
            GetMachineInfoService = new MachineInfomation.Service1Client();

            //OntologyTO2
            proxyCuttingTool = new RecommendedCuttingTool.CuttingToolInterfaceClient();
            proxyCuttingTool.Endpoint.Binding.OpenTimeout = TimeSpan.FromSeconds(CommonValue.SERVICE_TIMEOUT);

        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //Get values passing from web.config
            if (e.InitParams != null)
            {
                if (e.InitParams.ContainsKey("Authentication"))
                {
                    StateManager.SERVICE_AUTHENTICATION_URL = e.InitParams["Authentication"];
                }

                if (e.InitParams.ContainsKey(CommonValue.SERVICE_MANAGEMENT))
                {
                    StateManager.SERVICE_MANAGER_URL = e.InitParams[CommonValue.SERVICE_MANAGEMENT];
                    proxySM.Endpoint.Address = new System.ServiceModel.EndpointAddress(e.InitParams[CommonValue.SERVICE_MANAGEMENT]);
                    
                    if (CommonValue.FunctionKeys.ContainsKey(CommonValue.SERVICE_MANAGEMENT))
                        CommonValue.FunctionKeys[CommonValue.SERVICE_MANAGEMENT] = e.InitParams[CommonValue.SERVICE_MANAGEMENT];
                    else
                        CommonValue.FunctionKeys.Add(CommonValue.SERVICE_MANAGEMENT, e.InitParams[CommonValue.SERVICE_MANAGEMENT]);

                    if (CommonValue.FunctionState.ContainsKey(CommonValue.SERVICE_MANAGEMENT))
                        CommonValue.FunctionState[CommonValue.SERVICE_MANAGEMENT] = false;
                    else
                        CommonValue.FunctionState.Add(CommonValue.SERVICE_MANAGEMENT, false);
                }

                if (e.InitParams.ContainsKey(CommonValue.ONTOLOGY))
                {
                    proxyOT.Endpoint.Address = new System.ServiceModel.EndpointAddress(e.InitParams[CommonValue.ONTOLOGY]);
                    if (CommonValue.FunctionKeys.ContainsKey(CommonValue.ONTOLOGY))
                        CommonValue.FunctionKeys[CommonValue.ONTOLOGY] = e.InitParams[CommonValue.ONTOLOGY];
                    else
                        CommonValue.FunctionKeys.Add(CommonValue.ONTOLOGY, e.InitParams[CommonValue.ONTOLOGY]);

                    if (CommonValue.FunctionState.ContainsKey(CommonValue.ONTOLOGY))
                        CommonValue.FunctionState[CommonValue.ONTOLOGY] = false;
                    else
                        CommonValue.FunctionState.Add(CommonValue.ONTOLOGY, false);
                }

                if (e.InitParams.ContainsKey(CommonValue.ONTOLOGYTOC))
                {
                    proxyOT.Endpoint.Address = new System.ServiceModel.EndpointAddress(e.InitParams[CommonValue.ONTOLOGYTOC]);
                    if (CommonValue.FunctionKeys.ContainsKey(CommonValue.ONTOLOGYTOC))
                        CommonValue.FunctionKeys[CommonValue.ONTOLOGYTOC] = e.InitParams[CommonValue.ONTOLOGYTOC];
                    else
                        CommonValue.FunctionKeys.Add(CommonValue.ONTOLOGYTOC, e.InitParams[CommonValue.ONTOLOGYTOC]);

                    if (CommonValue.FunctionState.ContainsKey(CommonValue.ONTOLOGYTOC))
                        CommonValue.FunctionState[CommonValue.ONTOLOGYTOC] = false;
                    else
                        CommonValue.FunctionState.Add(CommonValue.ONTOLOGYTOC, false);
                }



                if (e.InitParams.ContainsKey(CommonValue.DATA_ACQUISITION))
                {
                    proxyDA.Endpoint.Address = new System.ServiceModel.EndpointAddress(e.InitParams[CommonValue.DATA_ACQUISITION]);
                    if (CommonValue.FunctionKeys.ContainsKey(CommonValue.DATA_ACQUISITION))
                        CommonValue.FunctionKeys[CommonValue.DATA_ACQUISITION] = e.InitParams[CommonValue.DATA_ACQUISITION];
                    else
                        CommonValue.FunctionKeys.Add(CommonValue.DATA_ACQUISITION, e.InitParams[CommonValue.DATA_ACQUISITION]);

                    if (CommonValue.FunctionState.ContainsKey(CommonValue.DATA_ACQUISITION))
                        CommonValue.FunctionState[CommonValue.DATA_ACQUISITION] = false;
                    else
                        CommonValue.FunctionState.Add(CommonValue.DATA_ACQUISITION, false);
                }

                if (e.InitParams.ContainsKey(CommonValue.MODEL_CREATION))
                {
                    proxyMC.Endpoint.Address = new System.ServiceModel.EndpointAddress(e.InitParams[CommonValue.MODEL_CREATION]);
                    if (CommonValue.FunctionKeys.ContainsKey(CommonValue.MODEL_CREATION))
                        CommonValue.FunctionKeys[CommonValue.MODEL_CREATION] = e.InitParams[CommonValue.MODEL_CREATION];
                    else
                        CommonValue.FunctionKeys.Add(CommonValue.MODEL_CREATION, e.InitParams[CommonValue.MODEL_CREATION]);

                    if (CommonValue.FunctionState.ContainsKey(CommonValue.MODEL_CREATION))
                        CommonValue.FunctionState[CommonValue.MODEL_CREATION] = false;
                    else
                        CommonValue.FunctionState.Add(CommonValue.MODEL_CREATION, false);
                }


                if (e.InitParams.ContainsKey(CommonValue.MODEL_MANAGEMENT))
                {
                    proxyMM.Endpoint.Address = new System.ServiceModel.EndpointAddress(e.InitParams[CommonValue.MODEL_MANAGEMENT]);
                    if (CommonValue.FunctionKeys.ContainsKey(CommonValue.MODEL_MANAGEMENT))
                        CommonValue.FunctionKeys[CommonValue.MODEL_MANAGEMENT] = e.InitParams[CommonValue.MODEL_MANAGEMENT];
                    else
                        CommonValue.FunctionKeys.Add(CommonValue.MODEL_MANAGEMENT, e.InitParams[CommonValue.MODEL_MANAGEMENT]);

                    if (CommonValue.FunctionState.ContainsKey(CommonValue.MODEL_MANAGEMENT))
                        CommonValue.FunctionState[CommonValue.MODEL_MANAGEMENT] = false;
                    else
                        CommonValue.FunctionState.Add(CommonValue.MODEL_MANAGEMENT, false);
                }

                if (e.InitParams.ContainsKey(CommonValue.USER_MANAGEMENT))
                {
                    proxyUM.Endpoint.Address = new System.ServiceModel.EndpointAddress(e.InitParams[CommonValue.USER_MANAGEMENT]);
                    if (CommonValue.FunctionKeys.ContainsKey(CommonValue.USER_MANAGEMENT))
                        CommonValue.FunctionKeys[CommonValue.USER_MANAGEMENT] = e.InitParams[CommonValue.USER_MANAGEMENT];
                    else
                        CommonValue.FunctionKeys.Add(CommonValue.USER_MANAGEMENT, e.InitParams[CommonValue.USER_MANAGEMENT]);

                    if (CommonValue.FunctionState.ContainsKey(CommonValue.USER_MANAGEMENT))
                        CommonValue.FunctionState[CommonValue.USER_MANAGEMENT] = false;
                    else
                        CommonValue.FunctionState.Add(CommonValue.USER_MANAGEMENT, false);
                }
                
            }

            this.RootVisual = new Shell();
        }

        private void Application_Exit(object sender, EventArgs e)
        {

        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if (!System.Diagnostics.Debugger.IsAttached)
            {

                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            }

            //Show error
            Shell.waitingForm.SettingMessage("An unknown error was found.");
            IPS.Views.ErrorWindow error = new IPS.Views.ErrorWindow(e.ExceptionObject);
            //fix bug of silverlight 4 with childwindows ->> disable parent
            error.Closed += (s, eargs) => { Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true); };
            error.Show();
        }

        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (System.Exception)
            {
            }
        }
    }
}
