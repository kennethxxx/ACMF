using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using IPS.Common;
using IPS.DataAcquisition;
using IPS.Views;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace IPS.ViewsSub.DataAquisitionModule
{
    public partial class DataAquisitionModule_CreateNewDCP_Page : UserControl
    {
        private DACreateNewDCPLocalContainer gCND_LocalContainer = null;
        private DACreateNewDCPGlobalContainer gCND_GlobalContainer = null;

        public DataAquisitionModule_CreateNewDCP_Page()
        {
            // 必須將變數初始化
            InitializeComponent();
        }

        // 給上層傳遞Container用
        public void BindingContainer(DACreateNewDCPLocalContainer m_LocalContainer, DACreateNewDCPGlobalContainer m_GlobalContainer)
        {
            gCND_LocalContainer = m_LocalContainer;
            gCND_GlobalContainer = m_GlobalContainer;
        }

        #region Choose ServiceBroker Info
        public void InitChooseServiceBroker()
        {
            //移除下一層
            DestroyChooseProduct();

            // 初始化此層
            gCND_LocalContainer.ServiceBrokerURIList = new List<string>();
            gCND_LocalContainer.ServiceBrokerNameList = new List<string>();
            gCND_GlobalContainer.LoginUsername = StateManager.Username;
            gCND_GlobalContainer.Company = StateManager.UserCompany;
            gCND_GlobalContainer.ChooseServiceBrokerURI = "";
            gCND_GlobalContainer.ChooseServiceBrokerName = "";
            ui_OptLabelSB.Content = "---";
            ui_ServiceBroker.ItemsSource = null;
            ui_ConformServiceBroker.IsEnabled = false;
            ui_HeaderChooseSB.Visibility = Visibility.Visible;


            ui_CNCType.Visibility = Visibility.Collapsed;
            ui_LB_CNCType.Visibility = Visibility.Collapsed;

            // 讀取Service Broker資訊
            //Shell.waitingForm.SettingMessage("Loading Service Broker Information");
            Shell.waitingForm.SettingMessage("載入Service Broker資訊");
            Shell.waitingForm.Show();
            App.proxyDA.getServiceBrokerInfoCompleted += new EventHandler<getServiceBrokerInfoCompletedEventArgs>(getServiceBrokerInfoEventHandler);
            App.proxyDA.getServiceBrokerInfoAsync();
        }
        private void getServiceBrokerInfoEventHandler(object sender, getServiceBrokerInfoCompletedEventArgs e)
        {
            bool Ack = true;
            try
            {
                string[][] stringlist = e.Result;

                if (stringlist != null)
                {
                    foreach (string[] SBSet in stringlist)
                    {
                        if (SBSet.Length == 2)
                        {
                            gCND_LocalContainer.ServiceBrokerNameList.Add(SBSet[0]);    // SB Name
                            gCND_LocalContainer.ServiceBrokerURIList.Add(SBSet[1]);     // SB IP
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Error: No Result.");
                    Ack = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString());
                Ack = false;
            }
            App.proxyDA.getServiceBrokerInfoCompleted -= new EventHandler<getServiceBrokerInfoCompletedEventArgs>(getServiceBrokerInfoEventHandler);
            Shell.waitingForm.Close();

            if (Ack)// 如果成功，綁定SB List，並將索引只到第一個，啟動ui_ConformServiceBroker按鈕
            {
                ui_ServiceBroker.ItemsSource = gCND_LocalContainer.ServiceBrokerNameList;
                if (gCND_LocalContainer.ServiceBrokerNameList.Count > 0)
                {
                    ui_ServiceBroker.SelectedIndex = 0;
                    ui_ConformServiceBroker.IsEnabled = true;
                }
            }
            else
            {
                DestroyChooseServiceBroker();
            }
        }
        private void ui_ServiceBroker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ui_HeaderChooseProduct.Visibility = Visibility.Collapsed;
            ui_HeaderChoosevMachine.Visibility = Visibility.Collapsed;
            ui_HeaderChooseProductBasicInfo.Visibility = Visibility.Collapsed;
            ui_HeaderChooseDAOptionInfo.Visibility = Visibility.Collapsed;

            ui_OptLabelSB.Content = "---";
            ui_OptLabelPT.Content = "---";
            ui_OptLabelPN.Content = "---";
            ui_OptLabelvM.Content = "---";
            ui_OptLabelCNC.Content = "---";

            // 變更任何選擇 會再次啟動ui_ConformServiceBroker按鈕
            if (ui_ServiceBroker.ItemsSource != null)
            {
                if (ui_ServiceBroker.SelectedIndex != -1)
                {
                    ui_ConformServiceBroker.IsEnabled = true;
                }
                else
                {
                    ui_ConformServiceBroker.IsEnabled = false;
                }
            }
            else
            {
                ui_ConformServiceBroker.IsEnabled = false;
            }
        }
        private void ui_ConformServiceBroker_Click(object sender, RoutedEventArgs e)
        {
            InitChooseProductInfo();
        }
        private void DestroyChooseServiceBroker()
        {
            //移除下一層
            DestroyChooseProduct();

            //清除此層
            gCND_LocalContainer.ServiceBrokerURIList = null;
            gCND_LocalContainer.ServiceBrokerNameList = null;
            gCND_GlobalContainer.LoginUsername = "";
            gCND_GlobalContainer.Company = "";
            gCND_GlobalContainer.ChooseServiceBrokerURI = "";
            gCND_GlobalContainer.ChooseServiceBrokerName = "";

            ui_OptLabelSB.Content = "---";
            ui_ServiceBroker.ItemsSource = null;
            ui_ConformServiceBroker.IsEnabled = false;
            ui_HeaderChooseSB.Visibility = Visibility.Collapsed;
        }
        #endregion


        #region Choose Product Info
        private void InitChooseProductInfo()
        {
            DestroyChoosevMachineInfo();//移除下一層

            // 初始化此層
            gCND_LocalContainer.ProductInfoList = new Dictionary<String, List<String>>();
            gCND_LocalContainer.ProductTypeList = new List<String>();
            gCND_GlobalContainer.ChooseProductInfoType = "";
            gCND_GlobalContainer.ChooseProductInfoName = "";

            ui_OptLabelPT.Content = "---";
            ui_OptLabelPN.Content = "---";
            ui_ProductType.ItemsSource = null;
            ui_ProductName.ItemsSource = null;
            ui_ConformProduct.IsEnabled = false;
            ui_HeaderChooseProduct.Visibility = Visibility.Visible;

            // 讀取Product資訊
            //Shell.waitingForm.SettingMessage("Loading Product Information");
            Shell.waitingForm.SettingMessage("載入產品資訊");
            Shell.waitingForm.Show();

            //App.proxyDA.getProductInfoCompleted += new EventHandler<getProductInfoCompletedEventArgs>(getProductInfoEventHandler);
            //App.proxyDA.getProductInfoAsync(gCND_LocalContainer.ServiceBrokerURIList[ui_ServiceBroker.SelectedIndex]);


            // XML [11/6/2012 autolab]
            App.proxyDA.getProductInfo1Completed += new EventHandler<getProductInfo1CompletedEventArgs>(getProductInfo1EventHandler);
            App.proxyDA.getProductInfo1Async(gCND_LocalContainer.ServiceBrokerURIList[ui_ServiceBroker.SelectedIndex]);



            //GetMachineInfoService = new IPS.MachineInfomation.Service1Client();
            //GetMachineInfoService.getProductInfoAsync();
            //GetMachineInfoService.GetCompanyInfoCompleted += new EventHandler<IPS.MachineInfomation.GetCompanyInfoCompletedEventArgs>(proxy_GetCompanyInfoCompleted);
        }

        void proxy_GetCompanyInfoCompleted(object sender, IPS.MachineInfomation.GetCompanyInfoCompletedEventArgs e)
        {
            GetMachineInfoService.GetCompanyInfoCompleted -= new EventHandler<IPS.MachineInfomation.GetCompanyInfoCompletedEventArgs>(proxy_GetCompanyInfoCompleted);
            if (e.Error == null && e.Result != null)
            {
                //載入資料
                //核心尚未寫出來時用MessageBox印出結果，已寫出來把ResolveXMLFormat方法註解移除掉
                //MessageBox.Show(e.Result);
                ObservableCollection<string> companyitemsSource = new ObservableCollection<string>();
                System.IO.MemoryStream _memorystreamCompany = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(e.Result));
                CompanyXml = XDocument.Load(_memorystreamCompany);
                foreach (var CompanyInfo in CompanyXml.Elements("Machine_Scheduling_Log").Descendants("Company_name"))
                {
                    companyitemsSource.Add(CompanyInfo.Value);
                }
                //CBCompany.ItemsSource = companyitemsSource;
                //if (CBCompany.ItemsSource != null)
                    //{
                    //    CBCompany.SelectedIndex = 0;
                    //}
                    Shell.waitingForm.Close();
            }
            else
            {
                MessageBox.Show("雲端服務忙錄中，請稍候再試!");
                Shell.waitingForm.Close();
            }
        }


        IPS.MachineInfomation.Service1Client GetMachineInfoService;
        XDocument CompanyXml, CompanyXml1, ServiceBrokerXml, MachineInfoXml;

        private void getProductInfo1EventHandler(object sender, getProductInfo1CompletedEventArgs e)
        //void proxy_GetCompanyInfoCompleted(object sender, IPS.MachineInfomation.GetCompanyInfoCompletedEventArgs e)
        {
           App.proxyDA.getProductInfo1Completed -= new EventHandler<getProductInfo1CompletedEventArgs>(getProductInfo1EventHandler);

           //GetMachineInfoService.GetCompanyInfoCompleted -= new EventHandler<IPS.MachineInfomation.GetCompanyInfoCompletedEventArgs>(proxy_GetCompanyInfoCompleted);

           if (e.Error == null && e.Result != null)
            {
                //載入資料
                //核心尚未寫出來時用MessageBox印出結果，已寫出來把ResolveXMLFormat方法註解移除掉
                //MessageBox.Show(e.Result);
                //ResolveXMLFormat(1, e.Result);

                ObservableCollection<string> companyitemsSource = new ObservableCollection<string>();

                

               System.IO.MemoryStream _memorystreamCompany = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(e.Result.ToString()));
                CompanyXml = XDocument.Load(_memorystreamCompany);

                //foreach (var CompanyInfo in CompanyXml.Elements("Machine_Scheduling_Log").Descendants("Company_name"))
                
               foreach (var CompanyInfo in CompanyXml.Elements("Product_Info").Descendants("Product"))
               {
                    System.IO.MemoryStream _memorystreamCompany1 = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(CompanyInfo.ToString()));
                    CompanyXml1 = XDocument.Load(_memorystreamCompany1);


                    String strProductType = "";

                    foreach (var CompanyInfos in CompanyXml1.Elements("Product").Descendants("Type"))
                    {
                        gCND_LocalContainer.ProductTypeList.Add(CompanyInfos.Value);
                        gCND_LocalContainer.ProductInfoList.Add(CompanyInfos.Value, new List<string>());
                        strProductType = CompanyInfos.Value;
                    }
                    
                    foreach (var CompanyInfos in CompanyXml1.Elements("Product").Descendants("Name"))
                    {
                        gCND_LocalContainer.ProductInfoList[strProductType].Add(CompanyInfos.Value);
                        //strProductName = CompanyInfos.Value;
                    }
                    

                    //gCND_LocalContainer.ProductInfoList[strProductName].Add(ProductSet[1]);
                }


               Shell.waitingForm.Close();

               
                ui_ProductType.ItemsSource = gCND_LocalContainer.ProductTypeList;
                if (gCND_LocalContainer.ProductTypeList.Count > 0)
                {
                    ui_ProductType.SelectedIndex = 0;
                }

                // 紀錄Service Broker Name and URL
                gCND_GlobalContainer.ChooseServiceBrokerName = gCND_LocalContainer.ServiceBrokerNameList[ui_ServiceBroker.SelectedIndex];
                gCND_GlobalContainer.ChooseServiceBrokerURI = gCND_LocalContainer.ServiceBrokerURIList[ui_ServiceBroker.SelectedIndex];

                ui_OptLabelSB.Content = gCND_GlobalContainer.ChooseServiceBrokerName;

                // 鎖定ui_ConformServiceBroker按鈕
                ui_ConformServiceBroker.IsEnabled = false;
            }
            else
            {
                DestroyChooseProduct();
                MessageBox.Show("雲端服務忙錄中，請稍候再試!");
                
                Shell.waitingForm.Close();
            }


            /*
            bool Ack = true;
            try
            {
                string[][] stringlist = e.Result;

                if (stringlist != null)
                {
                    foreach (string[] ProductSet in stringlist)
                    {
                        if (ProductSet.Length == 2)
                        {
                            if (gCND_LocalContainer.ProductInfoList.ContainsKey(ProductSet[0]) != true)
                            {
                                gCND_LocalContainer.ProductInfoList.Add(ProductSet[0], new List<string>());
                                gCND_LocalContainer.ProductTypeList.Add(ProductSet[0]);
                            }
                            gCND_LocalContainer.ProductInfoList[ProductSet[0]].Add(ProductSet[1]);
                        }
                    }
                }
                else
                {
                    // MessageBox.Show("Error: No Result.");
                    //MessageBox.Show("錯誤訊息: 沒有結果");
                    App.proxyDA.getProductInfoCompleted += new EventHandler<getProductInfoCompletedEventArgs>(getProductInfoEventHandler);
                    App.proxyDA.getProductInfoAsync(gCND_LocalContainer.ServiceBrokerURIList[ui_ServiceBroker.SelectedIndex]);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error: " + ex.ToString());
                //MessageBox.Show("錯誤訊息: " + ex.ToString());
                App.proxyDA.getProductInfoCompleted += new EventHandler<getProductInfoCompletedEventArgs>(getProductInfoEventHandler);
                App.proxyDA.getProductInfoAsync(gCND_LocalContainer.ServiceBrokerURIList[ui_ServiceBroker.SelectedIndex]);
                Ack = false;
            }

            
            Shell.waitingForm.Close();

            if (Ack)// 綁定ui_ProductType List 並將索引指到第一個  然後將ServiceBroker的資訊記錄下來
            {
                ui_ProductType.ItemsSource = gCND_LocalContainer.ProductTypeList;
                if (gCND_LocalContainer.ProductTypeList.Count > 0)
                {
                    ui_ProductType.SelectedIndex = 0;
                }

                // 紀錄Service Broker Name and URL
                gCND_GlobalContainer.ChooseServiceBrokerName = gCND_LocalContainer.ServiceBrokerNameList[ui_ServiceBroker.SelectedIndex];
                gCND_GlobalContainer.ChooseServiceBrokerURI = gCND_LocalContainer.ServiceBrokerURIList[ui_ServiceBroker.SelectedIndex];

                ui_OptLabelSB.Content = gCND_GlobalContainer.ChooseServiceBrokerName;

                // 鎖定ui_ConformServiceBroker按鈕
                ui_ConformServiceBroker.IsEnabled = false;
            }
            else
            {
                DestroyChooseProduct();
            }
            */ 
        }
        private void ui_ProductType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ui_HeaderChoosevMachine.Visibility = Visibility.Collapsed;
            ui_HeaderChooseProductBasicInfo.Visibility = Visibility.Collapsed;
            ui_HeaderChooseDAOptionInfo.Visibility = Visibility.Collapsed;

            ui_OptLabelPT.Content = "---";
            ui_OptLabelPN.Content = "---";
            ui_OptLabelvM.Content = "---";
            ui_OptLabelCNC.Content = "---";

            // 暫關 [11/6/2012 autolab]
            // 變更任何選擇 會再重新綁定 ui_ProductName 並將索引指到第一個
            if (ui_ProductType.ItemsSource != null)
            {
                if (ui_ProductType.SelectedIndex != -1)
                {
                    List<String> ProductInfoList = gCND_LocalContainer.ProductInfoList[gCND_LocalContainer.ProductTypeList[ui_ProductType.SelectedIndex]];
                    ui_ProductName.ItemsSource = ProductInfoList;
                    if (ProductInfoList.Count > 0)
                    {
                        ui_ProductName.SelectedIndex = 0;
                    }
                }
                else
                {
                    ui_ProductName.ItemsSource = null;
                    ui_ConformProduct.IsEnabled = false;
                }
            }
            else
            {
                ui_ProductName.ItemsSource = null;
                ui_ConformProduct.IsEnabled = false;
            }
        }
        private void ui_ProductName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 變更任何選擇 會再次啟動ui_ConformServiceBroker按鈕
            if (ui_ProductName.ItemsSource != null)
            {
                if (ui_ProductName.SelectedIndex != -1)
                {
                    ui_ConformProduct.IsEnabled = true;
                }
                else
                {
                    ui_ConformProduct.IsEnabled = false;
                }
            }
            else
            {
                ui_ConformProduct.IsEnabled = false;
            }
        }
        private void ui_ConformProduct_Click(object sender, RoutedEventArgs e)
        {
            InitChoosevMachineInfo();
        }
        void DestroyChooseProduct()
        {
            //移除下一層
            DestroyChoosevMachineInfo();

            //清除此層
            gCND_LocalContainer.ProductInfoList = null;
            gCND_LocalContainer.ProductTypeList = null;
            gCND_GlobalContainer.ChooseProductInfoType = "";
            gCND_GlobalContainer.ChooseProductInfoName = "";

            ui_OptLabelPT.Content = "---";
            ui_OptLabelPN.Content = "---";
            ui_ProductType.ItemsSource = null;
            ui_ProductName.ItemsSource = null;
            ui_ConformProduct.IsEnabled = false;

            ui_HeaderChooseProduct.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region Choose vMachine Info
        private void InitChoosevMachineInfo()
        {
            DestroyChooseProductBasicInfo();//移除下一層

            // 初始化此層
            gCND_LocalContainer.vMachineInfoList = new Dictionary<String, List<String>>();
            gCND_LocalContainer.vMachineTypeInfoList = new Dictionary<String, List<String>>();
            gCND_LocalContainer.vMachineIDList = new List<String>();
            gCND_GlobalContainer.ChoosevMachineID = "";
            gCND_GlobalContainer.ChooseCNCID = "";
            gCND_GlobalContainer.ChooseCNCType = "";

            ui_OptLabelvM.Content = "---";
            ui_OptLabelCNC.Content = "---";
            ui_OptLabelCNC.Content = "---";
            ui_vMachineID.ItemsSource = null;
            ui_CNCID.ItemsSource = null;
            ui_CNCType.ItemsSource = null;
            ui_ConformvMachine.IsEnabled = false;
            ui_HeaderChoosevMachine.Visibility = Visibility.Visible;
            
            ui_CNCType.Visibility = Visibility.Visible;
            ui_LB_CNCType.Visibility = Visibility.Visible;

            // 讀取Product資訊
            //Shell.waitingForm.SettingMessage("Loading v-Machine Information");
            Shell.waitingForm.SettingMessage("載入v-Machine資訊");
            Shell.waitingForm.Show();

            App.proxyDA.getProductionHistoInfoCompleted += new EventHandler<getProductionHistoInfoCompletedEventArgs>(getProductionHistoInfoEventHandler);
            App.proxyDA.getProductionHistoInfoAsync(
                gCND_GlobalContainer.ChooseServiceBrokerURI,
                gCND_LocalContainer.ProductTypeList[ui_ProductType.SelectedIndex],
                gCND_LocalContainer.ProductInfoList[gCND_LocalContainer.ProductTypeList[ui_ProductType.SelectedIndex]][ui_ProductName.SelectedIndex]
                );
        }
        private void getProductionHistoInfoEventHandler(object sender, getProductionHistoInfoCompletedEventArgs e)
        {
            bool Ack = true;
            try
            {
                string[][] stringlist = e.Result;

                if (stringlist != null)
                {
                    foreach (string[] vMachineSet in stringlist)
                    {
                        if (vMachineSet.Length == 3)
                        {
                            if (gCND_LocalContainer.vMachineInfoList.ContainsKey(vMachineSet[0]) != true)
                            {
                                gCND_LocalContainer.vMachineInfoList.Add(vMachineSet[0], new List<string>());
                                gCND_LocalContainer.vMachineIDList.Add(vMachineSet[0]);

                                gCND_LocalContainer.vMachineTypeInfoList.Add(vMachineSet[0], new List<string>());
                                gCND_LocalContainer.vMachineTypeList.Add(vMachineSet[0]);
                            }
                            gCND_LocalContainer.vMachineInfoList[vMachineSet[0]].Add(vMachineSet[1]);

                            gCND_LocalContainer.vMachineTypeInfoList[vMachineSet[0]].Add(vMachineSet[2]);

                        }
                    }
                }
                else
                {
                    //MessageBox.Show("Error: No Result.");
                    MessageBox.Show("錯誤訊息: 沒有結果.");
                    Ack = false;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error: " + ex.ToString());
                MessageBox.Show("錯誤訊息: " + ex.ToString());
                DestroyChoosevMachineInfo();
                Ack = false;
            }

            App.proxyDA.getProductionHistoInfoCompleted -= new EventHandler<getProductionHistoInfoCompletedEventArgs>(getProductionHistoInfoEventHandler);

            Shell.waitingForm.Close();

            if (Ack)// 綁定ui_vMachineID List 並將索引指到第一個  然後將Product Type & Name的資訊記錄下來
            {
                ui_vMachineID.ItemsSource = gCND_LocalContainer.vMachineIDList;
                
                if (gCND_LocalContainer.vMachineIDList.Count > 0)
                {
                    ui_vMachineID.SelectedIndex = 0;
                }

                // 紀錄Product Type & Product Name
                gCND_GlobalContainer.ChooseProductInfoType = gCND_LocalContainer.ProductTypeList[ui_ProductType.SelectedIndex];
                gCND_GlobalContainer.ChooseProductInfoName = gCND_LocalContainer.ProductInfoList[gCND_GlobalContainer.ChooseProductInfoType][ui_ProductName.SelectedIndex];

                ui_OptLabelPT.Content = gCND_GlobalContainer.ChooseProductInfoType;
                ui_OptLabelPN.Content = gCND_GlobalContainer.ChooseProductInfoName;

                // 鎖定ui_ChooseProduct按鈕
                ui_ConformProduct.IsEnabled = false;
            }
            else
            {
                DestroyChoosevMachineInfo();
            }
        }
        private void ui_vMachineID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 變更任何選擇 會再重新綁定 ui_CNCID 並將索引指到第一個
            if (ui_vMachineID.ItemsSource != null)
            {
                if (ui_vMachineID.SelectedIndex != -1)
                {
                    List<String> CNCIDList = gCND_LocalContainer.vMachineInfoList[gCND_LocalContainer.vMachineIDList[ui_vMachineID.SelectedIndex]];

                    List<String> CNCTypeList = gCND_LocalContainer.vMachineTypeInfoList[gCND_LocalContainer.vMachineTypeList[ui_vMachineID.SelectedIndex]];

                    ui_CNCID.ItemsSource = CNCIDList;
                    ui_CNCType.ItemsSource = CNCTypeList;

                    if (CNCIDList.Count > 0)
                    {
                        ui_CNCID.SelectedIndex = 0;
                    }

                    if (CNCTypeList.Count > 0)
                    {
                        ui_CNCType.SelectedIndex = 0;
                    }
                }
                else
                {
                    ui_CNCID.ItemsSource = null;
                    ui_CNCType.ItemsSource = null;
                    ui_ConformvMachine.IsEnabled = false;
                }
            }
            else
            {
                ui_CNCID.ItemsSource = null;
                ui_CNCType.ItemsSource = null;
                ui_ConformvMachine.IsEnabled = false;
            }
        }
        private void ui_CNCID_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 變更任何選擇 會再次啟動ui_ConformServiceBroker按鈕
            if (ui_CNCID.ItemsSource != null)
            {
                if (ui_CNCID.SelectedIndex != -1)
                {
                    ui_ConformvMachine.IsEnabled = true;
                }
                else
                {
                    ui_ConformvMachine.IsEnabled = false;
                }
            }
            else
            {
                ui_ConformvMachine.IsEnabled = false;
            }
        }
        private void ui_ConformvMachine_Click(object sender, RoutedEventArgs e)
        {
            InitChooseProductBasicInfo();

            CheckBox cbAllX = GetCheckBoxWithParent(ui_XinfoDataGrid, typeof(CheckBox), "ui_CheckAllXInfoList");
            if (cbAllX != null) { cbAllX.IsChecked = false; }

            CheckBox cbAllY = GetCheckBoxWithParent(ui_YinfoDataGrid, typeof(CheckBox), "ui_CheckAllYInfoList");
            if (cbAllY != null) { cbAllY.IsChecked = false; }

        }
        void DestroyChoosevMachineInfo()
        {
            //移除下一層
            DestroyChooseProductBasicInfo();

            //清除此層
            gCND_LocalContainer.vMachineInfoList = null;
            gCND_LocalContainer.vMachineTypeInfoList = null;
            gCND_LocalContainer.vMachineIDList = null;
            gCND_GlobalContainer.ChoosevMachineID = "";
            gCND_GlobalContainer.ChooseCNCID = "";

            ui_OptLabelvM.Content = "---";
            ui_OptLabelCNC.Content = "---";

            ui_vMachineID.ItemsSource = null;
            ui_CNCID.ItemsSource = null;
            ui_CNCType.ItemsSource = null;

            ui_ConformvMachine.IsEnabled = false;
            ui_HeaderChoosevMachine.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region Choose Product Basic Info
        private void InitChooseProductBasicInfo()
        {
            DestroyDAOptionInfo();//移除下一層

            
            //////////////////////////////////////////////////////////////////////////
            // 初始化此層
            gCND_LocalContainer.ProductBasicXinfoList = new List<Xinfo>();
            gCND_LocalContainer.ProductBasicXinfoCheckList = new List<bool>();
            gCND_LocalContainer.ProductBasicYinfoList = new List<Yinfo>();
            gCND_LocalContainer.ProductBasicYinfoCheckList = new List<bool>();

            gCND_GlobalContainer.AllProductBasicXinfoList = new List<Xinfo>();
            gCND_GlobalContainer.AllProductBasicYinfoList = new List<Yinfo>();
            gCND_GlobalContainer.ChooseProductBasicXinfoList = new List<Xinfo>();
            gCND_GlobalContainer.ChooseProductBasicYinfoList = new List<Yinfo>();

            //ui_XinfoLabel.Content = "Indicator Information:";
            //ui_YinfoLabel.Content = "Metrology Information:";
            ui_XinfoLabel.Content = "特徵值資訊:";
            ui_YinfoLabel.Content = "量測項目資訊:";
            ui_XinfoDataGrid.ItemsSource = null;
            ui_YinfoDataGrid.ItemsSource = null;
            ui_ConformProductionBasic.IsEnabled = true;
            ui_HeaderChooseProductBasicInfo.Visibility = Visibility.Visible;

            

            

            // 讀取Product Basic資訊
            //Shell.waitingForm.SettingMessage("Loading Indicator & Metrology Information");
            Shell.waitingForm.SettingMessage("載入產品特徵值與量測值資訊");
            Shell.waitingForm.Show();

            App.proxyDA.getProductBasicInfoCompleted += new EventHandler<getProductBasicInfoCompletedEventArgs>(getProductBasicInfoEventHandler);
            App.proxyDA.getProductBasicInfoAsync(
                gCND_GlobalContainer.ChooseServiceBrokerURI,
                gCND_LocalContainer.vMachineInfoList[gCND_LocalContainer.vMachineIDList[ui_vMachineID.SelectedIndex]][ui_CNCID.SelectedIndex],
                gCND_LocalContainer.ProductTypeList[ui_ProductType.SelectedIndex],
                gCND_LocalContainer.ProductInfoList[gCND_LocalContainer.ProductTypeList[ui_ProductType.SelectedIndex]][ui_ProductName.SelectedIndex],
                gCND_LocalContainer.vMachineIDList[ui_vMachineID.SelectedIndex]);
        }
        private void getProductBasicInfoEventHandler(object sender, getProductBasicInfoCompletedEventArgs e)
        {
            bool Ack = true;
            try
            {
                ProductBasicInfo PBI = e.Result;
                if (PBI != null)
                {
                    Xinfo XI = null;
                    // 取得 並複製一份 X 資訊 
                    if (PBI.XinfoList != null)
                    {
                        foreach (Xinfo XinfoSet in PBI.XinfoList)
                        {
                            XI = new Xinfo();
                            XI.Name = XinfoSet.Name;
                            XI.Type = XinfoSet.Type;
                            XI.Position = XinfoSet.Position;
                            gCND_LocalContainer.ProductBasicXinfoList.Add(XI);
                            gCND_LocalContainer.ProductBasicXinfoCheckList.Add(false);
                        }
                    }
                    Yinfo YI = null;
                    // 取得 並複製一份 Y 資訊
                    if (PBI.YinfoList != null)
                    {
                        foreach (Yinfo YinfoSet in PBI.YinfoList)
                        {
                            YI = new Yinfo();
                            YI.Name = YinfoSet.Name;
                            YI.Type = YinfoSet.Type;
                            YI.Action = YinfoSet.Action;
                            gCND_LocalContainer.ProductBasicYinfoList.Add(YI);
                            gCND_LocalContainer.ProductBasicYinfoCheckList.Add(false);
                        }
                    }
                }
                else
                {
                    //MessageBox.Show("Error: No Result.");
                    MessageBox.Show("錯誤訊息:沒有結果 ");
                    Ack = false;
                }
            }
            catch (Exception ex)
            {
               // MessageBox.Show("Error: " + ex.ToString());
                MessageBox.Show("錯誤訊息: " + ex.ToString());
                Ack = false;
            }

            App.proxyDA.getProductBasicInfoCompleted -= new EventHandler<getProductBasicInfoCompletedEventArgs>(getProductBasicInfoEventHandler);
            
            //////////////////////////////////////////////////////////////////////////

            
            
            
            //////////////////////////////////////////////////////////////////////////
            Shell.waitingForm.Close();

            if (Ack)// 綁定ui_XinfoDataGrid 和 ui_YinfoDataGrid 然後將ChoosevMachineID & ChooseCNCID的資訊記錄下來
            {
                ui_XinfoDataGrid.ItemsSource = gCND_LocalContainer.ProductBasicXinfoList;
                ui_YinfoDataGrid.ItemsSource = gCND_LocalContainer.ProductBasicYinfoList;

                // 紀錄ChoosevMachineID & ChooseCNCID
                gCND_GlobalContainer.ChoosevMachineID = gCND_LocalContainer.vMachineIDList[ui_vMachineID.SelectedIndex];
                gCND_GlobalContainer.ChooseCNCID = gCND_LocalContainer.vMachineInfoList[gCND_GlobalContainer.ChoosevMachineID][ui_CNCID.SelectedIndex];

                ui_OptLabelvM.Content = gCND_GlobalContainer.ChoosevMachineID;
                ui_OptLabelCNC.Content = gCND_GlobalContainer.ChooseCNCID;

                // 鎖定ui_ConformvMachine按鈕
                ui_ConformvMachine.IsEnabled = false;                
            }
            else
            {
                DestroyChooseProductBasicInfo();
            }
        }

        // XY Info的DataGrid事件 資料室綁訂在gCND_LocalContainer的ProductBasicXinfoList和ProductBasicYinfoList
        private void ui_XinfoDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            CheckBox chk = ui_XinfoDataGrid.Columns[0].GetCellContent(e.Row) as CheckBox;
            chk.IsChecked = gCND_LocalContainer.ProductBasicXinfoCheckList[e.Row.GetIndex()];
        }
        private void ui_YinfoDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            CheckBox chk = ui_YinfoDataGrid.Columns[0].GetCellContent(e.Row) as CheckBox;
            chk.IsChecked = gCND_LocalContainer.ProductBasicYinfoCheckList[e.Row.GetIndex()];
        }
        private void ui_CheckAllXInfoList_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            bool check = chk.IsChecked.Value;
            int iCheckCount = 0;

            // 將Local的資料全部設定成跟CheckBox的狀態一樣
            for (int i = 0; i < gCND_LocalContainer.ProductBasicXinfoCheckList.Count; i++)
            {
                gCND_LocalContainer.ProductBasicXinfoCheckList[i] = check;
            }
            iCheckCount = gCND_LocalContainer.ProductBasicXinfoCheckList.Count;
            
            if (check)
            {
                //ui_XinfoLabel.Content = "Indicator Information(" + iCheckCount.ToString() + "/" + iCheckCount.ToString() + ") :";
                ui_XinfoLabel.Content = "特徵值資訊(" + iCheckCount.ToString() + "/" + iCheckCount.ToString() + ") :";
            }
            else
            {
                //ui_XinfoLabel.Content = "Indicator Information(0/" + iCheckCount.ToString() + ") :";
                ui_XinfoLabel.Content = "特徵值資訊(0/" + iCheckCount.ToString() + ") :";
            }

            // 再從新綁定
            ui_XinfoDataGrid.ItemsSource = null;
            ui_XinfoDataGrid.ItemsSource = gCND_LocalContainer.ProductBasicXinfoList;
        }
        private void ui_CheckAllYInfoList_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            bool check = chk.IsChecked.Value;
            int iCheckCount = 0;

            // 將Local的資料全部設定成跟CheckBox的狀態一樣
            for (int i = 0; i < gCND_LocalContainer.ProductBasicYinfoCheckList.Count; i++)
            {
                gCND_LocalContainer.ProductBasicYinfoCheckList[i] = check;
            }
            iCheckCount = gCND_LocalContainer.ProductBasicYinfoCheckList.Count;


            if (check)
            {
                //ui_YinfoLabel.Content = "Metrology Information(" + iCheckCount.ToString() + "/" + iCheckCount.ToString() + ") :";
                ui_YinfoLabel.Content = "量測項目資訊(" + iCheckCount.ToString() + "/" + iCheckCount.ToString() + ") :";
            }
            else
            {
                //ui_YinfoLabel.Content = "Metrology Information(0/" + iCheckCount.ToString() + ") :";
                ui_YinfoLabel.Content = "量測項目資訊(0/" + iCheckCount.ToString() + ") :";
            }

            // 再從新綁定
            ui_YinfoDataGrid.ItemsSource = null;
            ui_YinfoDataGrid.ItemsSource = gCND_LocalContainer.ProductBasicYinfoList;
        }
        private void ui_CheckSingleXInfoList_Click(object sender, RoutedEventArgs e)
        {
            CheckBox OriginalCheckBox = (CheckBox)e.OriginalSource; //取得Checkbox目標的狀態
            if (ui_XinfoDataGrid.SelectedIndex != -1)
            {
                int Index = ui_XinfoDataGrid.SelectedIndex;
                gCND_LocalContainer.ProductBasicXinfoCheckList[Index] = (OriginalCheckBox.IsChecked == true);

                CheckBox CBParent = GetCheckBoxWithParent(ui_XinfoDataGrid, typeof(CheckBox), "ui_CheckAllXInfoList");
                if (CBParent != null)
                {
                    bool AllCheck = true;
                    int iCheckCount = 0;
                    int iTotalCount = 0;
                    foreach (bool CheckedItem in gCND_LocalContainer.ProductBasicXinfoCheckList)
                    {
                        iTotalCount++;
                        if (CheckedItem)
                        {
                            iCheckCount++;
                        }
                        AllCheck = AllCheck && CheckedItem;
                    }
                    CBParent.IsChecked = AllCheck;
                    //ui_XinfoLabel.Content = "Indicator Information(" + iCheckCount.ToString() + "/" + iTotalCount.ToString() + ") :";
                    ui_XinfoLabel.Content = "特徵值資訊(" + iCheckCount.ToString() + "/" + iTotalCount.ToString() + ") :";
                }
            }
        }
        private void ui_CheckSingleYInfoList_Click(object sender, RoutedEventArgs e)
        {
            CheckBox OriginalCheckBox = (CheckBox)e.OriginalSource; //取得Checkbox目標的狀態
            if (ui_YinfoDataGrid.SelectedIndex != -1)
            {
                int Index = ui_YinfoDataGrid.SelectedIndex;
                gCND_LocalContainer.ProductBasicYinfoCheckList[Index] = (OriginalCheckBox.IsChecked == true);

                CheckBox CBParent = GetCheckBoxWithParent(ui_YinfoDataGrid, typeof(CheckBox), "ui_CheckAllYInfoList");
                if (CBParent != null)
                {
                    bool AllCheck = true;
                    int iCheckCount = 0;
                    int iTotalCount = 0;
                    foreach (bool CheckedItem in gCND_LocalContainer.ProductBasicYinfoCheckList)
                    {
                        iTotalCount++;
                        if (CheckedItem)
                        {
                            iCheckCount++;
                        }
                        AllCheck = AllCheck && CheckedItem;
                    }
                    CBParent.IsChecked = AllCheck;
                    //ui_YinfoLabel.Content = "Metrology Information(" + iCheckCount.ToString() + "/" + iTotalCount.ToString() + ") :";
                    ui_YinfoLabel.Content = "量測項目資訊(" + iCheckCount.ToString() + "/" + iTotalCount.ToString() + ") :";
                }
            }
        }
        private void ui_ConformProductionBasic_Click(object sender, RoutedEventArgs e)
        {
            // 判定選取XY數量皆不為0時才能到下一項
            bool CheckSelXNotZero = false;
            bool CheckSelYNotZero = false;
            foreach (bool CheckedItem in gCND_LocalContainer.ProductBasicXinfoCheckList)
            {
                CheckSelXNotZero = CheckSelXNotZero || CheckedItem;
            }
            foreach (bool CheckedItem in gCND_LocalContainer.ProductBasicYinfoCheckList)
            {
                CheckSelYNotZero = CheckSelYNotZero || CheckedItem;
            }
            if (!CheckSelXNotZero)
            {
               // MessageBox.Show("You must Select Indicator Information item at least one!");
                MessageBox.Show("必須選擇至少一個特徵值!");
                return;
            }
            if (!CheckSelYNotZero)
            {
               // MessageBox.Show("You must Select Metrology Information item at least one!");
                MessageBox.Show("必須選擇至少一個量測項目!");
                return;
            }

            InitDAOptionInfo();
        }
        void DestroyChooseProductBasicInfo()
        {
            //移除下一層
            DestroyDAOptionInfo();

            //清除此層
            gCND_LocalContainer.ProductBasicXinfoList = null;
            gCND_LocalContainer.ProductBasicYinfoList = null;
            gCND_LocalContainer.ProductBasicXinfoCheckList = null;
            gCND_LocalContainer.ProductBasicYinfoCheckList = null;

            gCND_GlobalContainer.AllProductBasicXinfoList = null;
            gCND_GlobalContainer.AllProductBasicYinfoList = null;
            gCND_GlobalContainer.ChooseProductBasicXinfoList = null;
            gCND_GlobalContainer.ChooseProductBasicYinfoList = null;

            //ui_XinfoLabel.Content = "Indicator Information:";
            //ui_YinfoLabel.Content = "Metrology Information:";
            ui_XinfoLabel.Content = "特徵值資訊:";
            ui_YinfoLabel.Content = "量測項目資訊:";
            ui_XinfoDataGrid.ItemsSource = null;
            ui_YinfoDataGrid.ItemsSource = null;
            ui_ConformProductionBasic.IsEnabled = false;
            ui_HeaderChooseProductBasicInfo.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region DC Option
        private void InitDAOptionInfo()
        {
            // 初始化此層
            gCND_GlobalContainer.DAStartTime = "";
            gCND_GlobalContainer.DAEndTime = "";
            gCND_GlobalContainer.DAConjectureType = "";
            gCND_GlobalContainer.DACollectionMethod = "";

            ui_DAStartTime.DisplayData = DateTime.Now.AddYears(-1); //往後移動一年
            ui_DAEndTime.DisplayData = DateTime.Now;
            ui_CollectMethod_Immediate.IsChecked = true;
            ui_ConjectureType_PPP.IsChecked = true;
            ui_FanOutDCP.IsEnabled = true;                          // 到這個地步不用再確認
            ui_HeaderChooseDAOptionInfo.Visibility = Visibility.Visible;


            // 整理所有XY放到Global.AllProductBasicXinfoList 將選取的XY放到Global.ChooseProductBasicXinfoList

            // 讀取Product Basic資訊
            // 複製X到Global中
            Xinfo XI, XITemp;
            for (int Xindex = 0; Xindex < gCND_LocalContainer.ProductBasicXinfoList.Count; Xindex++)
            {
                XITemp = gCND_LocalContainer.ProductBasicXinfoList[Xindex];
                XI = new Xinfo();
                XI.Name = XITemp.Name;
                XI.Type = XITemp.Type;
                XI.Position = XITemp.Position;

                gCND_GlobalContainer.AllProductBasicXinfoList.Add(XI);
                if (gCND_LocalContainer.ProductBasicXinfoCheckList[Xindex])
                {
                    gCND_GlobalContainer.ChooseProductBasicXinfoList.Add(XI);
                }
            }

            // 複製Y到Global中
            Yinfo YI, YITemp;
            for (int Yindex = 0; Yindex < gCND_LocalContainer.ProductBasicYinfoList.Count; Yindex++)
            {
                YITemp = gCND_LocalContainer.ProductBasicYinfoList[Yindex];
                YI = new Yinfo();
                YI.Name = YITemp.Name;
                YI.Type = YITemp.Type;
                YI.Action = YITemp.Action;
                gCND_GlobalContainer.AllProductBasicYinfoList.Add(YI);
                if (gCND_LocalContainer.ProductBasicYinfoCheckList[Yindex])
                {
                    gCND_GlobalContainer.ChooseProductBasicYinfoList.Add(YI);
                }
            }
        }
        public event EventHandler FanOutDCPClicked;
        private void ui_FanOutDCP_Click(object sender, RoutedEventArgs e)
        {
            //檢查時間 Start < End
            if (ui_DAStartTime.DisplayData.CompareTo(ui_DAEndTime.DisplayData) < 0)
            {
                gCND_GlobalContainer.DAStartTime = ui_DAStartTime.DisplayData.ToString("yyyy-MM-dd HH:mm:ss");
                gCND_GlobalContainer.DAEndTime = ui_DAEndTime.DisplayData.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                //MessageBox.Show("The Start Time must be less than the End Time.");
                MessageBox.Show("開始日期必須小於結束日期");
                return;
            }

            //檢查收值方法
            //不用檢查 一定會有一項是選擇的
            if (ui_CollectMethod_Immediate.IsChecked == true)
            {
                gCND_GlobalContainer.DACollectionMethod = "Immediate";
                //gCND_GlobalContainer.DACollectionMethod = "立即";
            }
            else if (ui_CollectMethod_Continued.IsChecked == true)
            {
                gCND_GlobalContainer.DACollectionMethod = "Continued";
                //gCND_GlobalContainer.DACollectionMethod = "繼續";
            }
            else
            {
                //MessageBox.Show("Select a Collected Data Type");
                MessageBox.Show("選擇一個資料蒐集型態");
                return;
            }

            //檢查收值類型形態
            //不用檢查 一定會有一項是選擇的
            if (ui_ConjectureType_PPP.IsChecked == true)
            {
                gCND_GlobalContainer.DAConjectureType = "PPP";
            }
            else if (ui_ConjectureType_KDP.IsChecked == true)
            {
                gCND_GlobalContainer.DAConjectureType = "KDP";
            }
            else
            {
               // MessageBox.Show("Select a Conjecture Type.");
                MessageBox.Show("選擇一個預測模型");
                return;
            }

         //   MessageBoxResult MBResult = MessageBox.Show("Fan out this DCP?", "Fan out DCP", MessageBoxButton.OKCancel);
            MessageBoxResult MBResult = MessageBox.Show("確定下載這個DCP", "下載DCP", MessageBoxButton.OKCancel);

            if (MBResult == MessageBoxResult.OK)
            {
                DADCPInfo DDI = PrepareDADCPInfo(gCND_GlobalContainer);
                ProductBasicInfo PBI = PrepareProductBasicInfo(gCND_GlobalContainer);

                FanOutDCP(DDI, PBI);
            }

            
        }
        private DADCPInfo PrepareDADCPInfo(DACreateNewDCPGlobalContainer m_GlobalContainer)
        {
            DADCPInfo DDI = new DADCPInfo();

            DDI.FactoryName = m_GlobalContainer.Company;

            DDI.StartTime = m_GlobalContainer.DAStartTime;
            DDI.EndTime = m_GlobalContainer.DAEndTime;
            DDI.CollectionMethod = m_GlobalContainer.DACollectionMethod;
            DDI.ConjectureType = m_GlobalContainer.DAConjectureType;

            DDI.WorkPieceName = m_GlobalContainer.ChooseProductInfoName;
            DDI.WorkPieceType = m_GlobalContainer.ChooseProductInfoType;

            DDI.ServiceBrokerInformation = new ServiceBrokerInfo();
            DDI.ServiceBrokerInformation.Name = m_GlobalContainer.ChooseServiceBrokerName;
            DDI.ServiceBrokerInformation.URL = m_GlobalContainer.ChooseServiceBrokerURI;

            // 增加vMachine-CNC
            DDI.ServiceBrokerInformation.vMachineList = new vMachineInfo[1];
            DDI.ServiceBrokerInformation.vMachineList[0] = new vMachineInfo();
            DDI.ServiceBrokerInformation.vMachineList[0].Name = m_GlobalContainer.ChoosevMachineID;
            DDI.ServiceBrokerInformation.vMachineList[0].CNCList = new CNCInfo[1];
            DDI.ServiceBrokerInformation.vMachineList[0].CNCList[0] = new CNCInfo();
            DDI.ServiceBrokerInformation.vMachineList[0].CNCList[0].Name = m_GlobalContainer.ChooseCNCID;
            DDI.ServiceBrokerInformation.vMachineList[0].CNCList[0].Type = m_GlobalContainer.ChooseCNCID;
            // 增加X_Data
            DDI.X_Data = new XItem[m_GlobalContainer.ChooseProductBasicXinfoList.Count];
            Xinfo XI;
            XItem XIT;
            for (int XIindex = 0; XIindex < m_GlobalContainer.ChooseProductBasicXinfoList.Count; XIindex++)
            {
                XI = m_GlobalContainer.ChooseProductBasicXinfoList[XIindex];
                XIT = new XItem();
                XIT.Name = XI.Name;
                XIT.Type = XI.Type;
                XIT.Position = XI.Position;
                DDI.X_Data[XIindex] = XIT;
            }

            // 增加Y_Data
            DDI.Y_Data = new YItem[m_GlobalContainer.ChooseProductBasicYinfoList.Count];
            Yinfo YI;
            YItem YIT;
            for (int YIindex = 0; YIindex < m_GlobalContainer.ChooseProductBasicYinfoList.Count; YIindex++)
            {
                YI = m_GlobalContainer.ChooseProductBasicYinfoList[YIindex];
                YIT = new YItem();
                YIT.Name = YI.Name;
                YIT.Type = YI.Type;
                DDI.Y_Data[YIindex] = YIT;
            }

            return DDI;
        }
        private ProductBasicInfo PrepareProductBasicInfo(DACreateNewDCPGlobalContainer m_GlobalContainer)
        {
            ProductBasicInfo PBI = new ProductBasicInfo();

            PBI.XinfoList = new Xinfo[m_GlobalContainer.AllProductBasicXinfoList.Count];
            for (int XIindex = 0; XIindex < m_GlobalContainer.AllProductBasicXinfoList.Count; XIindex++)
            {
                PBI.XinfoList[XIindex] = m_GlobalContainer.AllProductBasicXinfoList[XIindex];
            }

            PBI.YinfoList = new Yinfo[m_GlobalContainer.AllProductBasicYinfoList.Count];
            for (int YIindex = 0; YIindex < m_GlobalContainer.AllProductBasicYinfoList.Count; YIindex++)
            {
                PBI.YinfoList[YIindex] = m_GlobalContainer.AllProductBasicYinfoList[YIindex];
            }

            return PBI;
        }
        void FanOutDCP(DADCPInfo DDI, ProductBasicInfo PBI)
        {
            //if (MessageBox.Show("Fan out this DCP?", "", MessageBoxButton.OKCancel) == MessageBoxResult.No)
            //{
            //    return;
            //}

            // Fan out DCP資訊
            //Shell.waitingForm.SettingMessage("Fan out DCP. Please wait...");
            Shell.waitingForm.SettingMessage("正在下載DCP...");
            Shell.waitingForm.Show();

            App.proxyDA.FanoutDCPCompleted += new EventHandler<FanoutDCPCompletedEventArgs>(FanoutDCPEventHandler);
            App.proxyDA.FanoutDCPAsync(DDI, PBI);
        }
        private void FanoutDCPEventHandler(object sender, FanoutDCPCompletedEventArgs e)
        {
            bool Ack = true;
            try
            {
                Ack = e.Result;

                if (!Ack)
                {
                    //MessageBox.Show("Fan Out DCP Fail. Please try again.");
                    MessageBox.Show("下載DCP失敗,請再試一次");
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error: " + ex.ToString());
                MessageBox.Show("錯誤訊息: " + ex.ToString());
                Ack = false;
            }

            App.proxyDA.FanoutDCPCompleted -= new EventHandler<FanoutDCPCompletedEventArgs>(FanoutDCPEventHandler);
            Shell.waitingForm.Close();

            if (Ack) //成功後執行清除Create NEW DCP 頁面，並返回DCPHistory頁
            {
                DestroyChooseServiceBroker();

                if (FanOutDCPClicked != null)
                {
                    FanOutDCPClicked(this, null);
                }
            }
        }
        void DestroyDAOptionInfo()
        {
            //清除此層
            gCND_GlobalContainer.DAStartTime = "";
            gCND_GlobalContainer.DAEndTime = "";
            gCND_GlobalContainer.DAConjectureType = "";
            gCND_GlobalContainer.DACollectionMethod = "";

            ui_DAStartTime.DisplayData = DateTime.Now;
            ui_DAEndTime.DisplayData = DateTime.Now;
            ui_CollectMethod_Immediate.IsChecked = true;
            ui_ConjectureType_PPP.IsChecked = true;
            ui_FanOutDCP.IsEnabled = false;
            ui_HeaderChooseDAOptionInfo.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region Common Method
        private CheckBox GetCheckBoxWithParent(UIElement parent, Type targetType, string CheckBoxName)
        {
            if (parent.GetType() == targetType && ((CheckBox)parent).Name == CheckBoxName)
            {
                return (CheckBox)parent;
            }
            CheckBox result = null;
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                UIElement child = (UIElement)VisualTreeHelper.GetChild(parent, i);
                if (GetCheckBoxWithParent(child, targetType, CheckBoxName) != null)
                {
                    result = GetCheckBoxWithParent(child, targetType, CheckBoxName);
                    break;
                }
            }
            return result;
        }
        #endregion

    }
}