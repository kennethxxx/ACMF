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
using IPS.Views;
using IPS.Common;
using System.Windows.Media.Imaging;
using System.Windows.Browser;
using System.Reflection;

namespace IPS
{
    public partial class Shell : UserControl
    {
        public static WaitingWindow waitingForm = new WaitingWindow();
        public static Shell ThisShell = null;
        public static LoginModule loginForm = null;
        //bool checkStatus = true;    //if user doesnt login yet

        public Shell()
        {
            InitializeComponent();
            ThisShell = this;

            //變更Size時取得即時的大小
            Application.Current.Host.Content.Resized -= new EventHandler(Content_ScreenSizeChanged); 
            Application.Current.Host.Content.Resized += new EventHandler(Content_ScreenSizeChanged);  

            //取得版本號
            Assembly CurrentAsm = Assembly.GetExecutingAssembly();
            if (Attribute.IsDefined(CurrentAsm, typeof(AssemblyDescriptionAttribute)))
            {
                //版本寫在Description
                AssemblyDescriptionAttribute adAttr = (AssemblyDescriptionAttribute)Attribute.GetCustomAttribute(CurrentAsm, typeof(AssemblyDescriptionAttribute));
                if (adAttr != null)
                {
                    //ThisShell.ui_VersionText.Text = "Cloud Version (" + adAttr.Description + ")";
                    //ThisShell.ui_VersionText.Text = "雲端版本 (" + adAttr.Description + ")";
                }
                else
                {
                    //ThisShell.ui_VersionText.Text = "Cloud Version (null)";
                    //ThisShell.ui_VersionText.Text = "雲端版本 (null)";
                }
            }

            //Link6.Visibility = System.Windows.Visibility.Collapsed;

            //            //check login
            //if ("".Equals(StateManager.Username))
            //{
            //    ContentFrame.Source = new Uri("/Home", UriKind.Relative);
            //    String url = ContentFrame.CurrentSource.AbsoluteUri;
            //    ContentFrame.Source = new Uri("/Home");
            //}

            // Get the UriMapper from the app.xaml resources, and assign it to the root frame
            //if ("".Equals(StateManager.Username))
            //{
            //    //HtmlPage.Window.Navigate(new Uri("/IPS.Web/", UriKind.Relative));

            //    //((System.Windows.Navigation.UriMapper)(ContentFrame.UriMapper)).UriMappings[0].MappedUri = new Uri("/Home", UriKind.Relative);
            //    ////ContentFrame.UriMapper = mapper;

            //    //ContentFrame.Source = new Uri("/Home", UriKind.Relative);

            //    ////// Our dummy check -- does the current time have an odd or even number of seconds?
            //    //DateTime time = DateTime.Now;
            //    //int seconds = time.Second;
            //    //bool isOdd = (seconds % 2) == 1;

            //    //// Update the mapper as appropriate
            //    //if (isOdd)
            //    //    mapper.UriMappings[0].MappedUri = new Uri("/DataAquisitionModule", UriKind.Relative);
            //    //else
            //    //    mapper.UriMappings[0].MappedUri = new Uri("/Home", UriKind.Relative);
            //}

            //if ("".Equals(StateManager.Username) && checkStatus)
            //{
            ////    ////String url = ContentFrame.CurrentSource.AbsoluteUri;
            //    checkStatus = false;
            ////    ContentFrame.Source = new Uri("/Home", UriKind.Relative);
            //    ContentFrame.Navigate(new Uri("/Views/Home.xaml", UriKind.Relative));
            //}            
        }

        // After the Frame navigates, ensure the HyperlinkButton representing the current page is selected
        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {


            if ("".Equals(StateManager.Username))
            {
                String url = ContentFrame.CurrentSource.OriginalString;
                if (url == "/Home")
                {
                    if (loginForm == null)
                    {
                        loginForm = new LoginModule();
                        loginForm.Closed += new EventHandler(LoginForm_ClosedEvent);
                        loginForm.Show();
                    }
                }
                else
                {
                    ContentFrame.Navigate(new Uri("/Home", UriKind.Relative));
                    return;
                }
            }

            if (ContentFrame.CurrentSource.OriginalString == "")
            {
                //取消連結反白
                foreach (UIElement child in LinksStackPanel.Children)
                {
                    HyperlinkButton hb = child as HyperlinkButton;
                    if (hb != null && hb.NavigateUri != null)
                    {
                        VisualStateManager.GoToState(hb, "InactiveLink", true);
                    }
                }
            }
            else 
            {
                //讓選中的連結反白
                foreach (UIElement child in LinksStackPanel.Children)
                {
                    HyperlinkButton hb = child as HyperlinkButton;
                    if (hb != null && hb.NavigateUri != null)
                    {
                        if (hb.NavigateUri.ToString().Equals(e.Uri.ToString()))
                        {
                            VisualStateManager.GoToState(hb, "ActiveLink", true);
                        }
                        else
                        {
                            VisualStateManager.GoToState(hb, "InactiveLink", true);
                        }
                    }
                }
            }
        }

        void CheckServiceExist(bool useDebug)
        {
            // 重設服務連結名稱
            ThisShell.LinkOntologyTOC.Content = "讀取中...";
            ThisShell.LinkOntologyCreation.Content = "讀取中...";
            ThisShell.LinkDataAquisition.Content = "讀取中...";
            ThisShell.LinkModelCreation.Content = "讀取中...";
            ThisShell.LinkModelMgt.Content = "讀取中...";
            ThisShell.LinkHistoricalData.Content = "讀取中...";
           //ThisShell.LinkServiceMgt.Content = "Loading...";
           // ThisShell.LinkUserMgt.Content = "Loading...";

            ThisShell.LinkUserMgt.Visibility = Visibility.Collapsed;

            // 重設服務連結位址
            ThisShell.LinkOntologyTOC.NavigateUri = new Uri("", UriKind.Relative);
            ThisShell.LinkOntologyCreation.NavigateUri = new Uri("", UriKind.Relative);
            ThisShell.LinkDataAquisition.NavigateUri = new Uri("", UriKind.Relative);
            ThisShell.LinkModelCreation.NavigateUri = new Uri("", UriKind.Relative);
            ThisShell.LinkModelMgt.NavigateUri = new Uri("", UriKind.Relative);
            ThisShell.LinkHistoricalData.NavigateUri = new Uri("", UriKind.Relative);
            //ThisShell.LinkServiceMgt.NavigateUri = new Uri("", UriKind.Relative);
            //ThisShell.LinkUserMgt.NavigateUri = new Uri("", UriKind.Relative);

            try
            {

                //檢查Service是否存在

                HttpWebRequest OTTOCHttpWebReq = (HttpWebRequest)WebRequest.Create(CommonValue.FunctionKeys[CommonValue.ONTOLOGYTOC]);
                OTTOCHttpWebReq.BeginGetResponse(r =>
                {
                    try
                    {
                        HttpWebResponse reponse = (HttpWebResponse)OTTOCHttpWebReq.EndGetResponse(r);
                        if (useDebug)
                        {
                            ThisShell.SettingLinkConfig(ThisShell.LinkOntologyTOC, "刀具尺寸評估", "/OntologyTOC", CommonValue.FunctionKeys[CommonValue.ONTOLOGYTOC]);

                        }
                        else
                        {
                            ThisShell.SettingLinkConfig(ThisShell.LinkOntologyTOC, "刀具尺寸評估", "/OntologyTOC", "");

                        }
                    }
                    catch (Exception)
                    {
                        if (useDebug)
                        {
                          //  ThisShell.SettingLinkConfig(ThisShell.LinkOntologyTOC, "Invalid", "", CommonValue.FunctionKeys[CommonValue.ONTOLOGYTOC]);
                            ThisShell.SettingLinkConfig(ThisShell.LinkOntologyTOC, "關閉服務", "", CommonValue.FunctionKeys[CommonValue.ONTOLOGYTOC]);

                        }
                        else
                        {
                            //ThisShell.SettingLinkConfig(ThisShell.LinkOntologyTOC, "Invalid", "", "");
                            ThisShell.SettingLinkConfig(ThisShell.LinkOntologyTOC, "關閉服務", "", "");

                        }
                    }
                }, null);


                //////////////////////////////////////////////////////////////////////////
                //Ontology
                HttpWebRequest OTHttpWebReq = (HttpWebRequest)WebRequest.Create(CommonValue.FunctionKeys[CommonValue.ONTOLOGY]);
                OTHttpWebReq.BeginGetResponse(r =>
                {
                    try
                    {
                        HttpWebResponse reponse = (HttpWebResponse)OTHttpWebReq.EndGetResponse(r);
                        if (useDebug)
                        {
                            ThisShell.SettingLinkConfig(ThisShell.LinkOntologyCreation, "OntologyUpload", "/OntologyUpload", CommonValue.FunctionKeys[CommonValue.ONTOLOGY]);

                        }
                        else
                        {
                            ThisShell.SettingLinkConfig(ThisShell.LinkOntologyCreation, "OntologyUpload", "/OntologyUpload", "");

                        }
                    }
                    catch (Exception)
                    {
                        if (useDebug)
                        {
                            //ThisShell.SettingLinkConfig(ThisShell.LinkOntologyCreation, "Invalid", "", CommonValue.FunctionKeys[CommonValue.ONTOLOGY]);
                            ThisShell.SettingLinkConfig(ThisShell.LinkOntologyCreation, "關閉服務", "", CommonValue.FunctionKeys[CommonValue.ONTOLOGY]);

                        }
                        else
                        {
                            //ThisShell.SettingLinkConfig(ThisShell.LinkOntologyCreation, "Invalid", "", "");
                            ThisShell.SettingLinkConfig(ThisShell.LinkOntologyCreation, "關閉服務", "", "");

                        }
                    }
                }, null);



                HttpWebRequest DAHttpWebReq = (HttpWebRequest)WebRequest.Create(CommonValue.FunctionKeys[CommonValue.DATA_ACQUISITION]);
                DAHttpWebReq.BeginGetResponse(r =>
                {
                    try
                    {
                        HttpWebResponse reponse = (HttpWebResponse)DAHttpWebReq.EndGetResponse(r);
                        if (useDebug)
                        {
                            ThisShell.SettingLinkConfig(ThisShell.LinkDataAquisition, "資料蒐集", "/DataAquisitionModule", CommonValue.FunctionKeys[CommonValue.DATA_ACQUISITION]);
                            ThisShell.SettingLinkConfig(ThisShell.LinkHistoricalData, "Historical Data", "/HistoricalDataModule", CommonValue.FunctionKeys[CommonValue.DATA_ACQUISITION]);
                        }
                        else
                        {
                            ThisShell.SettingLinkConfig(ThisShell.LinkDataAquisition, "資料蒐集", "/DataAquisitionModule", "");
                            ThisShell.SettingLinkConfig(ThisShell.LinkHistoricalData, "Historical Data", "/HistoricalDataModule", "");
                        }
                    }
                    catch (Exception)
                    {
                        if (useDebug)
                        {
                            //ThisShell.SettingLinkConfig(ThisShell.LinkDataAquisition, "Invalid", "", CommonValue.FunctionKeys[CommonValue.DATA_ACQUISITION]);
                            //ThisShell.SettingLinkConfig(ThisShell.LinkHistoricalData, "Invalid", "", CommonValue.FunctionKeys[CommonValue.DATA_ACQUISITION]);

                            ThisShell.SettingLinkConfig(ThisShell.LinkDataAquisition, "關閉服務", "", CommonValue.FunctionKeys[CommonValue.DATA_ACQUISITION]);
                            ThisShell.SettingLinkConfig(ThisShell.LinkHistoricalData, "關閉服務", "", CommonValue.FunctionKeys[CommonValue.DATA_ACQUISITION]);
                        }
                        else
                        {
                            //ThisShell.SettingLinkConfig(ThisShell.LinkDataAquisition, "Invalid", "", "");
                            //ThisShell.SettingLinkConfig(ThisShell.LinkHistoricalData, "Invalid", "", "");

                            ThisShell.SettingLinkConfig(ThisShell.LinkDataAquisition, "關閉服務", "", "");
                            ThisShell.SettingLinkConfig(ThisShell.LinkHistoricalData, "關閉服務", "", "");
                        }
                    }
                }, null);

                HttpWebRequest MCHttpWebReq = (HttpWebRequest)WebRequest.Create(CommonValue.FunctionKeys[CommonValue.MODEL_CREATION]);
                MCHttpWebReq.BeginGetResponse(r =>
                {
                    try
                    {
                        HttpWebResponse reponse = (HttpWebResponse)MCHttpWebReq.EndGetResponse(r);
                        if (useDebug)
                            ThisShell.SettingLinkConfig(ThisShell.LinkModelCreation, "模型建立", "/ModelCreationModule", CommonValue.FunctionKeys[CommonValue.MODEL_CREATION]);
                        else
                            ThisShell.SettingLinkConfig(ThisShell.LinkModelCreation, "模型建立", "/ModelCreationModule", "");
                    }
                    catch (Exception)
                    {
                        if (useDebug)
                        {
                           // ThisShell.SettingLinkConfig(ThisShell.LinkModelCreation, "Invalid", "", CommonValue.FunctionKeys[CommonValue.MODEL_CREATION]);
                            ThisShell.SettingLinkConfig(ThisShell.LinkModelCreation, "關閉服務", "", CommonValue.FunctionKeys[CommonValue.MODEL_CREATION]);
                        }
                        else
                        {
                            //ThisShell.SettingLinkConfig(ThisShell.LinkModelCreation, "Invalid", "", "");
                            ThisShell.SettingLinkConfig(ThisShell.LinkModelCreation, "關閉服務", "", "");
                        }
                    }
                }, null);

                HttpWebRequest MMHttpWebReq = (HttpWebRequest)WebRequest.Create(CommonValue.FunctionKeys[CommonValue.MODEL_MANAGEMENT]);
                MMHttpWebReq.BeginGetResponse(r =>
                {
                    try
                    {
                        HttpWebResponse reponse = (HttpWebResponse)MMHttpWebReq.EndGetResponse(r);
                        if (useDebug)
                            ThisShell.SettingLinkConfig(ThisShell.LinkModelMgt, "模型管理", "/ModelMgtModule", CommonValue.FunctionKeys[CommonValue.MODEL_MANAGEMENT]);
                        else
                            ThisShell.SettingLinkConfig(ThisShell.LinkModelMgt, "模型管理", "/ModelMgtModule", "");
                    }
                    catch (Exception)
                    {
                        if (useDebug)
                        {
                           // ThisShell.SettingLinkConfig(ThisShell.LinkModelMgt, "Invalid", "", CommonValue.FunctionKeys[CommonValue.MODEL_MANAGEMENT]);
                            ThisShell.SettingLinkConfig(ThisShell.LinkModelMgt, "關閉服務", "", CommonValue.FunctionKeys[CommonValue.MODEL_MANAGEMENT]);
                        }
                        else
                        {
                            //ThisShell.SettingLinkConfig(ThisShell.LinkModelMgt, "Invalid", "", "");
                            ThisShell.SettingLinkConfig(ThisShell.LinkModelMgt, "關閉服務", "", "請檢查相關設定");
                        }
                    }
                }, null);

                //HttpWebRequest USHttpWebReq = (HttpWebRequest)WebRequest.Create(CommonValue.FunctionKeys[CommonValue.USER_MANAGEMENT]);
                //USHttpWebReq.BeginGetResponse(r =>
                //{
                //    try
                //    {
                //        HttpWebResponse reponse = (HttpWebResponse)USHttpWebReq.EndGetResponse(r);
                //        if (useDebug)
                //            ThisShell.SettingLinkConfig(ThisShell.LinkUserMgt, "User Management", "/UserMgtModule", CommonValue.FunctionKeys[CommonValue.USER_MANAGEMENT]);
                //        else
                //            ThisShell.SettingLinkConfig(ThisShell.LinkUserMgt, "User Management", "/UserMgtModule", "");

                //        HttpWebRequest SMHttpWebReq = (HttpWebRequest)WebRequest.Create(CommonValue.FunctionKeys[CommonValue.SERVICE_MANAGEMENT]);
                //        SMHttpWebReq.BeginGetResponse(s =>
                //        {
                //            try
                //            {
                //                HttpWebResponse sreponse = (HttpWebResponse)SMHttpWebReq.EndGetResponse(s);
                //                if (useDebug)
                //                    ThisShell.SettingLinkConfig(ThisShell.LinkServiceMgt, "Service Management", "/ServiceMgtModule", CommonValue.FunctionKeys[CommonValue.SERVICE_MANAGEMENT]);
                //                else
                //                    ThisShell.SettingLinkConfig(ThisShell.LinkServiceMgt, "Service Management", "/ServiceMgtModule", "");
                //            }
                //            catch (Exception)
                //            {
                //                if (useDebug)
                //                {
                //                    ThisShell.SettingLinkConfig(ThisShell.LinkServiceMgt, "Invalid", "", CommonValue.FunctionKeys[CommonValue.SERVICE_MANAGEMENT]);
                //                }
                //                else
                //                {
                //                    ThisShell.SettingLinkConfig(ThisShell.LinkServiceMgt, "Invalid", "", "");
                //                }
                //            }
                //        }, null);

                //    }
                //    catch (Exception)
                //    {
                //        if (useDebug)
                //        {
                //            ThisShell.SettingLinkConfig(ThisShell.LinkUserMgt, "Invalid", "", CommonValue.FunctionKeys[CommonValue.USER_MANAGEMENT]);
                //            ThisShell.SettingLinkConfig(ThisShell.LinkServiceMgt, "Invalid", "", CommonValue.FunctionKeys[CommonValue.SERVICE_MANAGEMENT]);
                //        }
                //        else
                //        {
                //            ThisShell.SettingLinkConfig(ThisShell.LinkUserMgt, "Invalid", "", "");
                //            ThisShell.SettingLinkConfig(ThisShell.LinkServiceMgt, "Invalid", "", "");
                //        }
                //    }
                //}, null);
            }
            catch (System.Exception ex)
            {
                return;
            }

            
        }
        void LoginForm_ClosedEvent(object sender, EventArgs e)
        {
            //fix bug of silverlight 4 with childwindows ->> diable parent
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
            StateManager.UserCompany = loginForm.ui_UserCompany.Text;
            StateManager.Username    = loginForm.ui_UserName.Text.Trim().ToLower();
            StateManager.Password    = loginForm.ui_Password.Password.Trim();
            txtUsername.Text = StateManager.Username;
            CheckServiceExist(loginForm.IsOpenDebugMode);
            loginForm = null;
        }

        // If an error occurs during navigation, show an error window
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            NavigationService NS = (NavigationService)sender;
            e.Handled = true;
            ChildWindow ErrorWins = null;

            if (NS.CurrentSource == null)
            {
                ErrorWins = new ErrorWindow("/Home", e.Uri);
            }else
            {
                ErrorWins = new ErrorWindow(NS.CurrentSource.OriginalString, e.Uri);
            }
            ErrorWins.Closed += new EventHandler(ErrorWins_Closed);
            ErrorWins.Show();
        }

        void ErrorWins_Closed(object sender, EventArgs e)
        {
            ErrorWindow EW = (ErrorWindow)sender;
            if (loginForm == null) //預防在首頁就有人亂打網址造成登入視窗鎖住
            {
                ContentFrame.Navigate(new Uri(EW.UriOriginalString, UriKind.Relative));
            }
        }

        public void SettingLinkConfig(DependencyObject SettingObj, string ContentString, string URIString, string TipString)
        {
            this.Dispatcher.BeginInvoke(delegate()
            {
                ((HyperlinkButton)SettingObj).Content = ContentString;
                ((HyperlinkButton)SettingObj).NavigateUri = new Uri(URIString, UriKind.Relative);
                ToolTipService.SetToolTip(SettingObj, TipString);
            });
        }

        private void ui_ChangeTheme_Blue_Click(object sender, RoutedEventArgs e)
        {
            ChangeTheme("BlueBannerBorderStyle", "BlueLinkStyle"); 
        }

        private void ui_ChangeTheme_Green_Click(object sender, RoutedEventArgs e)
        {
            ChangeTheme("GreenBannerBorderStyle", "GreenLinkStyle"); 
        }

        private void ui_ChangeTheme_Orange_Click(object sender, RoutedEventArgs e)
        {
            ChangeTheme("OrangeBannerBorderStyle", "OrangeLinkStyle"); 
        }

        private void ChangeTheme(string BorderStyleName, string LinkStyleName)
        {
            //borderStyle
            Style newStyle = new Style
            {
                TargetType = typeof(Border),
                BasedOn = App.Current.Resources.MergedDictionaries[0][BorderStyleName] as Style
            };
            borderBanner.Style = newStyle;

            //LinkStyle
            Style newLinkStyle = new Style
            {
                TargetType = typeof(HyperlinkButton),
                BasedOn = App.Current.Resources.MergedDictionaries[0][LinkStyleName] as Style
            };

            LinkHome.Style = newLinkStyle;
            LinkOntology.Style = newLinkStyle;
            LinkDataAquisition.Style = newLinkStyle;
            LinkModelCreation.Style = newLinkStyle;
            LinkModelMgt.Style = newLinkStyle;
            LinkHistoricalData.Style = newLinkStyle;
            LinkServiceMgt.Style = newLinkStyle;
            LinkUserMgt.Style = newLinkStyle;
        }

        private void ui_ChangeScreenSizeButton_Click(object sender, RoutedEventArgs e)
        {
            // C#  
            Application.Current.Host.Content.IsFullScreen = !Application.Current.Host.Content.IsFullScreen;
            if (Application.Current.Host.Content.IsFullScreen)
            {
                ui_ChangeScreenSizeButtonImg.Source = new BitmapImage(new Uri("Images/fullscreen_exit.png", UriKind.Relative));
            }
            else
            {
                ui_ChangeScreenSizeButtonImg.Source = new BitmapImage(new Uri("Images/fullscreen.png", UriKind.Relative));
            }
        }

        private void ui_ChangeScreenSizeButtonImg_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            //不知道怎麼處理....
        }

        private void ui_ExitButton_Click(object sender, RoutedEventArgs e)
        {
            // Logout & Clear session
            StateManager.UserCompany = "";
            StateManager.Username = "";
            StateManager.UserKey = "";
            CommonValue.FunctionKeys.Clear();
            this.ContentFrame.Navigate(new Uri("/Home", UriKind.Relative));
        }

        void Content_ScreenSizeChanged(object sender, EventArgs e)
        {
            // get the full screen dimension  
            StateManager.screenWidth = Application.Current.Host.Content.ActualWidth;
            StateManager.screenHeight = Application.Current.Host.Content.ActualHeight;
            StateManager.ContentFrameHeight = this.ContentFrame.ActualHeight;
            StateManager.ContentFrameWidth = this.ContentFrame.ActualWidth;
        }


    }
}
