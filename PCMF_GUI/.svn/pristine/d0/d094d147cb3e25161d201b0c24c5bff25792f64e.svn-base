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
using IPS.ModelManager;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace IPS.Views
{
    public partial class ModelMgtModule : Page
    {
        
        public ModelMgtModule()
        {
            InitializeComponent();
            if (StateManager.Username == "") //避免使用者按下Refresh
            {
                return;
            }
            InitModelSelectionTab();
        }
        
        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

        MMModelSelectionLocalContainer gMS_LocalContainer = null;
        
        

        
        #region Model Selection
        
        // 初始化Model Selection Tab
        void InitModelSelectionTab()
        {
            DestroyModelFanOutTab();
            ShowOutModelInfo(null);

            ui_MS_SearchModel.IsEnabled = false;
            ui_MS_NextStep.IsEnabled = false;

            ui_MS_vMachineID.ItemsSource = null;
            ui_MS_CNCID.ItemsSource = null;
            ui_MS_ProductID.ItemsSource = null;
            ui_MS_ServiveBrokerID.ItemsSource = null;

            

            //set default value [11/24/2011 pili7545]
            ui_MS_SelectedDateStart.SelectedDate = DateTime.Now;
            ui_MS_SelectedDateEnd.SelectedDate = DateTime.Now;

            ui_MS_SearchMoreModel.Visibility = Visibility.Collapsed;

            //ui_MS_ModelSearchResultHeader.Header = "Model Searching Result";
            ui_MS_ModelSearchResultHeader.Header = "模型搜尋結果";

            ui_MS_ModelList.ItemsSource = null;
          

            gMS_LocalContainer = new MMModelSelectionLocalContainer();

            

            LoadFilterList();

          
        }

         
        

        
        // 讀取模型參數
        void LoadFilterList()
        {
            //Shell.waitingForm.SettingMessage("Loading Filter Parameters");
            Shell.waitingForm.SettingMessage("正在搜尋模型");
            Shell.waitingForm.Show();

            App.proxyMM.getModelFilterParameterCompleted += new EventHandler<getModelFilterParameterCompletedEventArgs>(GetModelParameterCompletedEvent);
            App.proxyMM.getModelFilterParameterAsync(StateManager.UserCompany);
        }
        
        // 完成讀取模型參數
        private void GetModelParameterCompletedEvent(object sender, getModelFilterParameterCompletedEventArgs e)
        {
            bool IsSuccess = true;
            Shell.waitingForm.Close(); 
            App.proxyMM.getModelFilterParameterCompleted -= new EventHandler<getModelFilterParameterCompletedEventArgs>(GetModelParameterCompletedEvent);
            ModelManager.ModelFilterParameters Result = null;

            try
            {
                Result = e.Result;

                if (Result.vMachineID == null || Result.cnc_number == null ||  Result.ProductID == null)
                {
                    IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Some Error Happen:" + ex.ToString());
                MessageBox.Show("錯誤訊息:" + ex.ToString());
                IsSuccess = false;
            }

            if (IsSuccess)
            {
                gMS_LocalContainer.vMachineList = Result.vMachineID;
                gMS_LocalContainer.CNCnumberList = Result.cnc_number;
                gMS_LocalContainer.ProductIDList = Result.ProductID;
                gMS_LocalContainer.ServiceBrokerList = Result.ServiceBrokerID;

                ui_MS_vMachineID.ItemsSource = Result.vMachineID;
                ui_MS_CNCID.ItemsSource = Result.cnc_number;
                ui_MS_ProductID.ItemsSource = Result.ProductID;
                ui_MS_ServiveBrokerID.ItemsSource = Result.ServiceBrokerID;

                ui_MS_SelectedDateStart.SelectedDate = Result.modelStartDate;
                ui_MS_SelectedDateEnd.SelectedDate = DateTime.Now.Date;
                ui_MS_SelectedTimeStart.Value = Result.modelStartDate;
                ui_MS_SelectedTimeEnd.Value = DateTime.Now; 

                ui_MS_ServiveBrokerID.SelectedIndex = 0;
                ui_MS_vMachineID.SelectedIndex = 0;
                ui_MS_CNCID.SelectedIndex = 0;
                ui_MS_ProductID.SelectedIndex = 0;


             

                ui_MS_SearchModel.IsEnabled = true;
            }
            else 
            {
                ui_MS_SearchModel.IsEnabled = false;
            }
        }

        // 找尋模型
        private void ui_MS_SearchModel_Click(object sender, RoutedEventArgs e)
        {
            //Shell.waitingForm.SettingMessage("Searching Models");
            Shell.waitingForm.SettingMessage("搜尋模型");
            Shell.waitingForm.Show();
            DateTime SearchStart = DateTime.ParseExact(ui_MS_SelectedDateStart.SelectedDate.Value.ToString("yyyy-MM-dd") + " " + ui_MS_SelectedTimeStart.Value.Value.ToString("HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", null);
            DateTime SearchEnd   = DateTime.ParseExact(ui_MS_SelectedDateEnd.SelectedDate.Value.ToString("yyyy-MM-dd") + " " + ui_MS_SelectedTimeEnd.Value.Value.ToString("HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", null);

            App.proxyMM.getModelInformationListCompleted += new EventHandler<getModelInformationListCompletedEventArgs>(GetModelInformationListCompletedEvent);

           
            App.proxyMM.getModelInformationListAsync(ui_MS_ServiveBrokerID.SelectedValue.ToString(),
                ui_MS_vMachineID.SelectedValue.ToString(),
                ui_MS_CNCID.SelectedValue.ToString() ,
                ui_MS_ProductID.SelectedValue.ToString(),
                SearchStart, SearchEnd, 
                StateManager.Username, StateManager.UserCompany);
              //App.proxyMM.getModelInformationListAsync( gMS_LocalContainer.SelectedProductBasicInformation.vMachineID, 
              //                                            gMS_LocalContainer.SelectedProductBasicInformation.CNCID,
              //                                            gMS_LocalContainer.SelectedProductBasicInformation.CNCType,
              //                                            SearchStart, SearchEnd, StateManager.
              //                                            Username, StateManager.UserCompany);
         
        }

        private void ui_MS_SearchMoreModel_Click(object sender, RoutedEventArgs e)
        {
            ui_MS_SearchMoreModel.Visibility = Visibility.Collapsed;
            ui_MS_SelectedDateStart.SelectedDate = gMS_LocalContainer.modelList[99].createTime;
            ui_MS_SelectedTimeStart.Value = gMS_LocalContainer.modelList[99].createTime;
            ui_MS_SearchModel_Click(sender, new RoutedEventArgs());
        }

        // 完成找尋模型
        private void GetModelInformationListCompletedEvent(object sender, getModelInformationListCompletedEventArgs e)
        {
            bool IsSuccess = false;
            Shell.waitingForm.Close();
            App.proxyMM.getModelInformationListCompleted -= new EventHandler<getModelInformationListCompletedEventArgs>(GetModelInformationListCompletedEvent);
            try
            {
                if (e.Result != null)
                {
                    gMS_LocalContainer.modelList = e.Result.ToList();
                    IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Some Error Happen:" + ex.ToString());
                MessageBox.Show("錯誤訊息:" + ex.ToString());
                IsSuccess = false;
            }

            if (IsSuccess)
            {
                if (gMS_LocalContainer.modelList.Count > 100)   //還有更多的資料
                {
                    //移除100以後的資料，因為是不正確的
                    while (gMS_LocalContainer.modelList.Count > 100)
                    {
                        gMS_LocalContainer.modelList.RemoveAt(100);
                    }
                    ui_MS_SearchMoreModel.Visibility = Visibility.Visible; // 讓使用者可以抓取更多的資料的按鈕
                    //ui_MS_ModelSearchResultHeader.Header = "Model Searching Result(" + gMS_LocalContainer.modelList.Count.ToString() + " more)"; //變更顯示數量
                    ui_MS_ModelSearchResultHeader.Header = "模型搜尋結果(" + gMS_LocalContainer.modelList.Count.ToString() + " more)"; //變更顯示數量
                }
                else 
                {
                    //ui_MS_ModelSearchResultHeader.Header = "Model Searching Result(" + gMS_LocalContainer.modelList.Count.ToString() + ")"; //變更顯示數量
                    ui_MS_ModelSearchResultHeader.Header = "模型搜尋結果(" + gMS_LocalContainer.modelList.Count.ToString() + ")"; //變更顯示數量
                }

                ui_MS_ModelList.ItemsSource = gMS_LocalContainer.modelList;
               
            }
            else
            {
                gMS_LocalContainer.modelList = null;
                ui_MS_ModelList.ItemsSource = null;
                //ui_MS_ModelSearchResultHeader.Header = "Model Searching Result(Error)";
                ui_MS_ModelSearchResultHeader.Header = "模型搜尋結果(錯誤)";
                ui_MS_NextStep.IsEnabled = false;
            }
        }

        

        // 選取模型事件
        private void ui_MS_ModelList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ModelInformation MI = (ModelInformation)ui_MS_ModelList.SelectedItem;
            if (MI != null)
            {
                ui_MS_NextStep.IsEnabled = true;
                gMS_LocalContainer.SelectedModelInformation = MI;
                ShowOutModelInfo(MI);
                
            }
            else 
            {
                ui_MS_NextStep.IsEnabled = false;
                gMS_LocalContainer.SelectedModelInformation = null;
                ShowOutModelInfo(null);
            }
        }

        void ShowOutModelInfo(ModelInformation SelectedModel)
        {
            if (SelectedModel == null)
            {
                ui_MS_MIModelID.Text = "---";
                ui_MS_MICreateTime.Text = "---";
                ui_MS_MIModelSize.Text = "---";
                ui_MS_MIvMachineID.Text = "---";
                ui_MS_MICNCID.Text = "---";
                ui_MS_MICNCtype.Text = "---";
                ui_MS_MIProductID.Text = "---";
                ui_MS_MICreator.Text = "---";
                ui_MS_MICompany.Text = "---";
                ui_MS_MIServiveBrokerID.Text = "---";

                ToolTipService.SetToolTip(ui_MS_MIModelID, "");
                ToolTipService.SetToolTip(ui_MS_MICreateTime, "");
                ToolTipService.SetToolTip(ui_MS_MIModelSize, "");
                ToolTipService.SetToolTip(ui_MS_MIvMachineID, "");
                ToolTipService.SetToolTip(ui_MS_MICNCID, "");
                ToolTipService.SetToolTip(ui_MS_MICNCtype, "");
                ToolTipService.SetToolTip(ui_MS_MIProductID, "");
                ToolTipService.SetToolTip(ui_MS_MIServiveBrokerID, "");
                ToolTipService.SetToolTip(ui_MS_MICreator, "");
                ToolTipService.SetToolTip(ui_MS_MICompany, "");
            }
            else 
            {
                ui_MS_MIModelID.Text = SelectedModel.PK;
                ui_MS_MICreateTime.Text = SelectedModel.createTime.ToString("yyyy/MM/dd HH:mm:ss");
                double ModelSizeKB = SelectedModel.modelSize ;
                ui_MS_MIModelSize.Text = Math.Round(ModelSizeKB, 2).ToString() + "MB";// (" + (ModelSizeKB / 1024).ToString() + " KB)";
                ui_MS_MIvMachineID.Text = SelectedModel.vMachineID;
                ui_MS_MICNCID.Text = SelectedModel.cnc_number;
                ui_MS_MICNCtype.Text = SelectedModel.CNCType;
                ui_MS_MIProductID.Text = SelectedModel.ProductID;
                ui_MS_MICreator.Text = SelectedModel.createUser;
                ui_MS_MICompany.Text = SelectedModel.COMPANY;
                ui_MS_MIServiveBrokerID.Text = SelectedModel.ServiceBrokerID;

                ToolTipService.SetToolTip(ui_MS_MIModelID, ui_MS_MIModelID.Text);
                ToolTipService.SetToolTip(ui_MS_MICreateTime, ui_MS_MICreateTime.Text);
                ToolTipService.SetToolTip(ui_MS_MIModelSize, ui_MS_MIModelSize.Text);
                ToolTipService.SetToolTip(ui_MS_MIvMachineID, ui_MS_MIvMachineID.Text);
                ToolTipService.SetToolTip(ui_MS_MICNCID, ui_MS_MICNCID.Text);
                ToolTipService.SetToolTip(ui_MS_MICNCtype, ui_MS_MICNCtype.Text);
                ToolTipService.SetToolTip(ui_MS_MIProductID, ui_MS_MIProductID.Text);
                ToolTipService.SetToolTip(ui_MS_MICreator, ui_MS_MICreator.Text);
                ToolTipService.SetToolTip(ui_MS_MICompany, ui_MS_MICompany.Text);
                ToolTipService.SetToolTip(ui_MS_MIServiveBrokerID,  ui_MS_MIServiveBrokerID.Text);
            }

        }

       

        // Move to Fanout Model Tab
        private void ui_MS_NextStep_Click(object sender, RoutedEventArgs e)
        {
            if (gMS_LocalContainer.SelectedModelInformation != null)
            {
                InitModelFanOutTab();
                FillSelectedModelInfo(gMS_LocalContainer.SelectedModelInformation);
                LoadVMachineList();
            }
            else 
            {
                //MessageBox.Show("Please Choose a Model!");
                MessageBox.Show("請選擇一個模型!");
            }
        }
        
        #endregion


        MMModelFanOutLocalContainer gMF_LocalContainer = null;

        #region Model FanOut

        // 初始化Model Fanout Tab
        void InitModelFanOutTab()
        {
            DestroyModelFanOutTab();
            gMF_LocalContainer = new MMModelFanOutLocalContainer();
            ui_MF_MainTab.IsEnabled = true;
        }

        // 清除Model Fanout Tab
        void DestroyModelFanOutTab()
        {
            gMF_LocalContainer = null;

            ui_MF_vMachineListTotally.ItemsSource = null;
            ui_MF_vMachineListSelected.ItemsSource = null;
            ui_MF_MainTab.IsEnabled = false;
            
            FillSelectedModelInfo(null);
        }

        // 填寫Model資訊
        void FillSelectedModelInfo(ModelInformation SelectedModel)
        {
            
            if (SelectedModel == null)
            {
                ui_MF_SelectedModelID.Text = "---";
                ui_MF_SelectedModelFileSize.Text = "---";
                ui_MF_SelectedModelCreatedTime.Text = "---";
                ui_MF_SelectedModelvMachineID.Text = "---";
                ui_MF_SelectedModelCNCnumber.Text = "---";
             
                ui_MF_SelectedModelCNCType.Text = "---";
                ui_MF_SelectedModelCreatedUser.Text = "---";
                ui_MF_SelectedModelCompany.Text = "---";
                ui_MF_SelectedModelServiceBrokerID.Text = "---";

                ToolTipService.SetToolTip(ui_MF_SelectedModelID, "");
                ToolTipService.SetToolTip(ui_MF_SelectedModelFileSize, "");
                ToolTipService.SetToolTip(ui_MF_SelectedModelCreatedTime, "");
                ToolTipService.SetToolTip(ui_MF_SelectedModelvMachineID, "");
                ToolTipService.SetToolTip(ui_MF_SelectedModelCNCnumber, "");
                
                ToolTipService.SetToolTip(ui_MF_SelectedModelCNCType, "");
                ToolTipService.SetToolTip(ui_MF_SelectedModelCreatedUser, "");
                ToolTipService.SetToolTip(ui_MF_SelectedModelCompany, "");
                ToolTipService.SetToolTip(ui_MF_SelectedModelServiceBrokerID, "");
            }
            else
            {
                ui_MF_SelectedModelID.Text = SelectedModel.PK;
                double ModelSizeMB = SelectedModel.modelSize;
                ui_MF_SelectedModelFileSize.Text = Math.Round(ModelSizeMB, 2).ToString() + "MB"; //(" + SelectedModel.modelSize.ToString() + " Bytes)";
                ui_MF_SelectedModelCreatedTime.Text = SelectedModel.createTime.ToString("yyyy/MM/dd HH:mm:ss");
                ui_MF_SelectedModelvMachineID.Text = SelectedModel.vMachineID;
                ui_MF_SelectedModelCNCnumber.Text = SelectedModel.cnc_number;
               
                ui_MF_SelectedModelCNCType.Text = SelectedModel.CNCType;
                ui_MF_SelectedModelCreatedUser.Text = SelectedModel.createUser;
                ui_MF_SelectedModelCompany.Text = SelectedModel.COMPANY;
                ui_MF_SelectedModelServiceBrokerID.Text = SelectedModel.ServiceBrokerID;

                ToolTipService.SetToolTip(ui_MF_SelectedModelID, ui_MF_SelectedModelID.Text);
                ToolTipService.SetToolTip(ui_MF_SelectedModelFileSize, ui_MF_SelectedModelFileSize.Text);
                ToolTipService.SetToolTip(ui_MF_SelectedModelCreatedTime, ui_MF_SelectedModelCreatedTime.Text);
                ToolTipService.SetToolTip(ui_MF_SelectedModelvMachineID, ui_MF_SelectedModelvMachineID.Text);
                ToolTipService.SetToolTip(ui_MF_SelectedModelCNCnumber, ui_MF_SelectedModelCNCnumber.Text);
               
                ToolTipService.SetToolTip(ui_MF_SelectedModelCNCType, ui_MF_SelectedModelCNCType.Text);
                ToolTipService.SetToolTip(ui_MF_SelectedModelCreatedUser, ui_MF_SelectedModelCreatedUser.Text);
                ToolTipService.SetToolTip(ui_MF_SelectedModelCompany, ui_MF_SelectedModelCompany.Text);
                ToolTipService.SetToolTip(ui_MF_SelectedModelServiceBrokerID, ui_MF_SelectedModelServiceBrokerID.Text);
            }

        }

        // 讀取VMachineList
        void LoadVMachineList()
        {
            //Shell.waitingForm.SettingMessage("Loading V-Machine List");
            Shell.waitingForm.SettingMessage("正在載入v-Machine列表");
            Shell.waitingForm.Show();

            //原版取得方式
            //App.proxyMM.getEquipmentInformationListCompleted += new EventHandler<getEquipmentInformationListCompletedEventArgs>(getEquipmentInformationListCompletedEvent);
            //App.proxyMM.getEquipmentInformationListAsync(ui_MS_ServiveBrokerID.SelectedValue.ToString(), ui_MF_SelectedModelCompany.Text);
            
            //新版取得方式XML
            App.proxyMM.getEquipmentInformationListsCompleted += new EventHandler<getEquipmentInformationListsCompletedEventArgs>(getEquipmentInformationListsCompletedEvent);
            App.proxyMM.getEquipmentInformationListsAsync(ui_MS_ServiveBrokerID.SelectedValue.ToString(), ui_MF_SelectedModelCompany.Text);
        }

        XDocument MachineInfoXml = null;

        // 完成讀取VMachineList
        private void getEquipmentInformationListsCompletedEvent(object sender, getEquipmentInformationListsCompletedEventArgs e)
        {
            App.proxyMM.getEquipmentInformationListsCompleted -= new EventHandler<getEquipmentInformationListsCompletedEventArgs>(getEquipmentInformationListsCompletedEvent);

            bool IsSuccess = false;
            Shell.waitingForm.Close();

            try
            {

                if (e.Result != null)
                {
                    //gMF_LocalContainer.vmList = e.Result.ToList();




                    //GDGetMachineStatus.ItemsSource = null;
                    //List<PE_GetStatusofMachines> CNCsource = new List<PE_GetStatusofMachines>();
                    /////取出多層中的Attribute所使用的方式
                    

                    ///**
                    // * MachineInfoXml.Elements("Machine_Scheduling_Log").Elements("vMachine").Descendants("Output_Machine")的
                    // * Elements與Element差很多，若使用Element XML第二層有兩個以上會解析不到
                    // * 若使用Elements就可以讀到
                    // */

                    //foreach (var MachineInfo2 in MachineInfoXml.Elements("Machine_Scheduling_Log").Elements("vMachine").Descendants("Output_Machine"))
                    //{
                    //    CNCsource.Add(new PE_GetStatusofMachines(MachineInfo2.Parent.Attribute("Name").Value, MachineInfo2.Attribute("Name").Value, MachineInfo2.Attribute("Type").Value, MachineInfo2.Attribute("State").Value));
                    //}
                    //GDGetMachineStatus.ItemsSource = CNCsource;

                    System.IO.MemoryStream _memorystream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(e.Result));
                    MachineInfoXml = XDocument.Load(_memorystream);



                    List<EquipmentInformation> ltVM  = new List<EquipmentInformation>();

                    

                    foreach (var MachineInfo2 in MachineInfoXml.Elements("Equipment_Info").Descendants("v_Machine"))
                    {
                        EquipmentInformation EI = new EquipmentInformation();

                        EI.CNCID = MachineInfo2.Attribute("CNCID").Value;
                        EI.State = MachineInfo2.Attribute("State").Value;
                        EI.v_Machine_ID = MachineInfo2.Attribute("ID").Value;
                        EI.v_Machine_IP = MachineInfo2.Attribute("v_Machine_IP").Value;
                        
                        ltVM.Add(EI);
                        //CNCsource.Add(new PE_GetStatusofMachines(MachineInfo2.Parent.Attribute("Name").Value, MachineInfo2.Attribute("Name").Value, MachineInfo2.Attribute("Type").Value, MachineInfo2.Attribute("State").Value));
                    }

                    gMF_LocalContainer.vmList = ltVM;
                    /*gMF_LocalContainer.vmList = e.Result.ToList();*/
                    IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show("Some Error Happen:" + ex.ToString());
                //MessageBox.Show("錯誤訊息:" + ex.ToString());
                //App.proxyMM.getEquipmentInformationListCompleted += new EventHandler<getEquipmentInformationListCompletedEventArgs>(getEquipmentInformationListCompletedEvent);
                //App.proxyMM.getEquipmentInformationListAsync(ui_MS_ServiveBrokerID.SelectedValue.ToString(), ui_MF_SelectedModelCompany.Text);

                IsSuccess = false;
            }

            if (IsSuccess)
            {
                ui_MF_vMachineListTotally.ItemsSource = gMF_LocalContainer.vmList;
                gMF_LocalContainer.selectedVMList = new List<EquipmentInformation>();
                ui_MF_vMachineListSelected.ItemsSource = null;
                ui_MF_MainTab.IsSelected = true;
            }
            else
            {
                ui_MS_MainTab.IsSelected = true;
                DestroyModelFanOutTab();
            }
        }


        // 完成讀取VMachineList
        private void getEquipmentInformationListCompletedEvent(object sender, getEquipmentInformationListCompletedEventArgs e)
        {
            App.proxyMM.getEquipmentInformationListCompleted -= new EventHandler<getEquipmentInformationListCompletedEventArgs>(getEquipmentInformationListCompletedEvent);
            
            bool IsSuccess = false;
            Shell.waitingForm.Close();
            
            try
            {
                
                if (e.Result != null)
                {
                    gMF_LocalContainer.vmList = e.Result.ToList();
                    IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
               // MessageBox.Show("Some Error Happen:" + ex.ToString());
                //MessageBox.Show("錯誤訊息:" + ex.ToString());
                App.proxyMM.getEquipmentInformationListCompleted += new EventHandler<getEquipmentInformationListCompletedEventArgs>(getEquipmentInformationListCompletedEvent);
                App.proxyMM.getEquipmentInformationListAsync(ui_MS_ServiveBrokerID.SelectedValue.ToString(), ui_MF_SelectedModelCompany.Text);

                IsSuccess = false;
            }

            if (IsSuccess)
            {
                ui_MF_vMachineListTotally.ItemsSource = gMF_LocalContainer.vmList;
                gMF_LocalContainer.selectedVMList = new List<EquipmentInformation>();
                ui_MF_vMachineListSelected.ItemsSource = null;
                ui_MF_MainTab.IsSelected = true;
            }
            else
            {
                ui_MS_MainTab.IsSelected = true;
                DestroyModelFanOutTab();
            }
        }

        // 選取 vMachine
        private void ui_MF_SelectDown_Click(object sender, RoutedEventArgs e)
        {
            System.Collections.IList SelectedVMs = ui_MF_vMachineListTotally.SelectedItems;
            List<EquipmentInformation> TempList = new List<EquipmentInformation>();
            if (SelectedVMs != null)
            {
                foreach (EquipmentInformation SelectedVM in SelectedVMs)
                {
                    TempList.Add(SelectedVM);
                }
                ui_MF_vMachineListTotally.ItemsSource = null;
                ui_MF_vMachineListSelected.ItemsSource = null;
                foreach (EquipmentInformation SelectedVM in TempList)
                {
                    gMF_LocalContainer.selectedVMList.Add(SelectedVM);
                    gMF_LocalContainer.vmList.Remove(SelectedVM);
                }
                ui_MF_vMachineListTotally.ItemsSource = gMF_LocalContainer.vmList;
                ui_MF_vMachineListSelected.ItemsSource = gMF_LocalContainer.selectedVMList;
                ui_MF_Fanout.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("請在列表選擇一個 v-Machine!");
                // MessageBox.Show("Please choose a v-Machine From The List!");
            }

            if (gMF_LocalContainer.vmList.Count > 0)
            {
                ui_MF_vMachineListTotally.SelectedIndex = 0;
            }
            else 
            {
                ui_MF_vMachineListTotally.SelectedIndex = -1;
            }

            if (gMF_LocalContainer.selectedVMList.Count > 0)
            {
                ui_MF_Fanout.IsEnabled = true;
            }
            else
            {
                ui_MF_Fanout.IsEnabled = false;
            }
            TempList = null;
        }

        // 移除 vMachine
        private void ui_MF_SelectUp_Click(object sender, RoutedEventArgs e)
        {
            System.Collections.IList SelectedVMs = ui_MF_vMachineListSelected.SelectedItems;
            List<EquipmentInformation> TempList = new List<EquipmentInformation>();
            if (SelectedVMs != null)
            {
                foreach (EquipmentInformation SelectedVM in SelectedVMs)
                {
                    TempList.Add(SelectedVM);
                }
                ui_MF_vMachineListTotally.ItemsSource = null;
                ui_MF_vMachineListSelected.ItemsSource = null;
                foreach (EquipmentInformation SelectedVM in TempList)
                {
                    gMF_LocalContainer.vmList.Add(SelectedVM);
                    gMF_LocalContainer.selectedVMList.Remove(SelectedVM);
                }
                ui_MF_vMachineListTotally.ItemsSource = gMF_LocalContainer.vmList;
                ui_MF_vMachineListSelected.ItemsSource = gMF_LocalContainer.selectedVMList;
                ui_MF_Fanout.IsEnabled = true;
            }
            else
            {
                //MessageBox.Show("Please choose a v-Machine From The List!");
                MessageBox.Show("請在列表選擇一個 v-Machine!");
            }

            if (gMF_LocalContainer.selectedVMList.Count > 0)
            {
                ui_MF_Fanout.IsEnabled = true;
                ui_MF_vMachineListSelected.SelectedIndex = 0;
            }
            else
            {
                ui_MF_Fanout.IsEnabled = false;
            }
            TempList = null;
        }

        EquipmentInformation getEquipmentObject(List<EquipmentInformation> list, string vMachineID)
        {
            EquipmentInformation result = null;
            foreach (EquipmentInformation obj in list)
            {
                if (obj.v_Machine_ID== vMachineID)
                {
                    result = obj;
                    break;
                }
            }
            return result;
        }

        // Fanout Model
        private void btnFanOut_Click(object sender, RoutedEventArgs e)
        {
            ModelInformation selectedModel = (ModelInformation)ui_MS_ModelList.SelectedItem;
            EquipmentInformation selectedVM = (EquipmentInformation)ui_MF_vMachineListSelected.SelectedItem;

            //ObservableCollection<FanOutEquipmentInformation> fanoutModel = new FanOutEquipmentInformation();

            //Shell.waitingForm.SettingMessage("Fanning-Out Model");
            Shell.waitingForm.SettingMessage("模型下載中");
            Shell.waitingForm.Show();


            //=========================================================================
            ModelManager.Model_SendContent mc = new ModelManager.Model_SendContent();
            List<ModelManager.Model_SendContent> lmc = new List<ModelManager.Model_SendContent>();
            //ObservableCollection<ModelManager.Model_Content> lmc = new ObservableCollection<ModelManager.Model_Content>();
           // ObservableCollection<ModelManager.Model_Content> lmc_ob = GetObCollectionByCheckList(lmc);

            //this.ui_MS_ModelList.ItemsSource.OfType<object>().Count()
            //gMS_LocalContainer.modelList.Count


            for (int i = 0; i < this.ui_MF_vMachineListSelected.ItemsSource.OfType<object>().Count(); i++)
            {

                mc = new  ModelManager.Model_SendContent();

                    mc.PK = gMS_LocalContainer.SelectedModelInformation.PK;
                   // mc.vMachineID = ListToObjectCollection(gMF_LocalContainer.selectedVMList).ElementAt(i).vMachineName;
                    mc.vMachineID = gMF_LocalContainer.selectedVMList[i].v_Machine_ID;
                    mc.CNCID = gMF_LocalContainer.selectedVMList[i].CNCID;
                    mc.Company = StateManager.UserCompany;
                    mc.ServiceBrokerID = ui_MF_SelectedModelServiceBrokerID.Text;
              
                    lmc.Add(mc);

                

            }

            ObservableCollection<ModelManager.Model_SendContent> lmc_ob = GetObCollectionByCheckList(lmc);

            //var lmc_ob = new ObservableCollection<ModelManager.Model_Content>();
            //foreach (var item in lmc)
            //{
            //    lmc_ob.Add(item);
            //}


            App.proxyMM.fanOutModelCompleted += new EventHandler<fanOutModelCompletedEventArgs>(fanOutModelCompletedEvent);
            //App.proxyMM.fanOutModelAsync(gMS_LocalContainer.SelectedModelInformation.PK, ListToObjectCollection(gMF_LocalContainer.selectedVMList), StateManager.Username);
             App.proxyMM.fanOutModelAsync(lmc_ob);
          
            
        }
        ObservableCollection<ModelManager.Model_SendContent> GetObCollectionByCheckList(List<ModelManager.Model_SendContent> ListModel_Content)
        {
            ModelManager.Model_SendContent[] ListModel_ContentTemp = ListModel_Content.ToArray();
            ObservableCollection<ModelManager.Model_SendContent> Output = new ObservableCollection<ModelManager.Model_SendContent>();
            int i = 0;
            foreach (var item in ListModel_Content)
            {

                Output.Add(ListModel_ContentTemp[i]);
                
                i++;
            }
            return Output;
        }

        private void fanOutModelCompletedEvent(object sender, fanOutModelCompletedEventArgs e)
        {
            bool IsSuccess = false;
            Shell.waitingForm.Close();
            App.proxyMM.fanOutModelCompleted -= new EventHandler<fanOutModelCompletedEventArgs>(fanOutModelCompletedEvent);
            try
            {
                IsSuccess = e.Result;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Some Error Happen:" + ex.ToString());
                MessageBox.Show("錯誤訊息:" + ex.ToString());
                IsSuccess = false;
            }

            if (IsSuccess)
            {
                //MessageBox.Show("FanOut successful.");
                MessageBox.Show("下載成功");
            }
            else
            {
                //MessageBox.Show("Fanning-Out error. Please try it again!");
                MessageBox.Show("下載發生錯誤. 請再試一次!");
            }
        }

        System.Collections.ObjectModel.ObservableCollection<FanOutEquipmentInformation> ListToObjectCollection(List<EquipmentInformation> list)
        {
            System.Collections.ObjectModel.ObservableCollection<FanOutEquipmentInformation> result = new System.Collections.ObjectModel.ObservableCollection<FanOutEquipmentInformation>();
            foreach (EquipmentInformation vm in list)
            {
                FanOutEquipmentInformation foModel = new FanOutEquipmentInformation();
               // foModel.equipmentName = vm.v_equipmentName;
               // foModel.vMachineName = vm.vMachineName;
                result.Add(foModel);
            }
            return result;
        }

        #endregion





        
    }
}
