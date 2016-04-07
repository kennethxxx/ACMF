using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using System.Linq;
using System.Net;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using IPS.Common;
using IPS.ModelCreation;
using Visifire.Charts;
using Serialization;
using IPS.Common.MCS_Data;
using IPS.Views;

namespace IPS.ViewsSub.ModelCreationModule
{
	public partial class ModelCreationModule_DataCollection_Page : UserControl
	{
        // Define Local Container 
        MCDataCollectionLocalContainer pDC_LocalContainer = null;

        // Define Global Container 
        MCDataCollectionGlobalContainer pDC_GlobalContainer = null;

        // Define Global Parameter 
        // 全域XML CLASS [8/8/2012 autolab]
        DCPInfo pDC_GlobalDI = new DCPInfo();

        Common.VisifireChart utilityChart = new Common.VisifireChart();
        int iActionCount = 12;//之後要從DB上抓下來

        // Define Global Event
        public event EventHandler ChangeToNextStep;
        public event EventHandler DestroyNextStep;

		public ModelCreationModule_DataCollection_Page()
		{
			// 必須將變數初始化
			InitializeComponent();
            pDC_LocalContainer = new MCDataCollectionLocalContainer();
		}

        #region Init
        public void BindingContainer(MCDataCollectionGlobalContainer p, DCPInfo q)
        {
            pDC_GlobalContainer = p;
            pDC_GlobalDI = q;
        }

        
        public void InitionPage()
        {
            //  [8/8/2012 autolab]
            pDC_GlobalDI.CurrentInfo = new CurrentInfos();
            pDC_GlobalDI.X_Data = new ObservableCollection<XItem>();
            pDC_GlobalDI.Y_Data = new ObservableCollection<YItem>();
            pDC_GlobalDI.WP_Data = new ObservableCollection<AllPiece>();
            pDC_GlobalDI.WP_TrainData = new ObservableCollection<TrainPiece>();
            pDC_GlobalDI.WP_RunData = new ObservableCollection<RunPiece>();
            
            //////////////////////////////////////////////////////////////////////////
            ui_DC_vMachine.ItemsSource = null;
            ui_DC_vMachine.IsEnabled = false;

            ui_DC_CNCnumber.ItemsSource = null;
            ui_DC_CNCnumber.IsEnabled = false;

            ui_DC_NCprogram.ItemsSource = null;
            ui_DC_NCprogram.IsEnabled = false;

            ui_DC_Search.IsEnabled = false;
            ui_DC_NextStep.IsEnabled = false;

            ui_DC_StartDate.SelectedDate = DateTime.Now.AddYears(-3);
            ui_DC_EndDate.SelectedDate = DateTime.Now;
            ui_DC_StartTime.Value = ui_DC_StartDate.SelectedDate;
            ui_DC_EndTime.Value = ui_DC_EndDate.SelectedDate;

            ui_DC_MetrologyType.ItemsSource = null;
            ui_DC_MetrologyType.IsEnabled = false;
            ui_DC_IndicatorType.ItemsSource = null;
            ui_DC_IndicatorType.IsEnabled = false;

            CheckBox cbAll = GetCheckBoxWithParent(ui_DC_IndicatorType, typeof(CheckBox), "ui_DC_CheckIndicatorAll");
            if (cbAll != null) { cbAll.IsChecked = false; }

            cbAll = GetCheckBoxWithParent(ui_DC_MetrologyType, typeof(CheckBox), "ui_DC_CheckMetrologyTypeAll");
            if (cbAll != null) { cbAll.IsChecked = false; }


            //////////////////////////////////////////////////////////////////////////
            //登入後
            //getVmachine_ID();

            getProductBasicInfo();

            ui_DC_Search.Visibility = Visibility.Collapsed;
            
        }

        void getProductBasicInfo()
        {
            //Shell.waitingForm.SettingMessage("Loading Product Basic Information");
            Shell.waitingForm.SettingMessage("載入產品基本資訊");
            Shell.waitingForm.Show();

            App.proxyMC.Get_ProductBasicInfoCompleted += new EventHandler<Get_ProductBasicInfoCompletedEventArgs>(Get_ProductBasicInfoCompletedEventHandler);
            try
            {
                App.proxyMC.Get_ProductBasicInfoAsync();
            }
            catch (Exception ex)
            {
              //  MessageBox.Show("There are errors on the server. " + ex.ToString());
                MessageBox.Show("伺服器發生錯誤. " + ex.ToString());
            }
        }
        // 完成取得ProductBasicInfo
        private void Get_ProductBasicInfoCompletedEventHandler(object sender, Get_ProductBasicInfoCompletedEventArgs e)
        {
            bool IsSuccess = false;
            try
            {
                IsSuccess = true;
            }
            catch (Exception ex)
            {
               // MessageBox.Show("Error: " + ex.ToString());
                MessageBox.Show("錯誤訊息: " + ex.ToString());
                IsSuccess = false;
            }

            App.proxyMC.Get_ProductBasicInfoCompleted += new EventHandler<Get_ProductBasicInfoCompletedEventArgs>(Get_ProductBasicInfoCompletedEventHandler);
            Shell.waitingForm.Close();

            if (IsSuccess)
            {

                Dictionary<string, ObservableCollection<string>> dicProduct = e.Result;

                ObservableCollection<ProductBasicInfoClass> values = new ObservableCollection<ProductBasicInfoClass>();


                foreach (KeyValuePair<string, ObservableCollection<string>> pair in dicProduct)
                {
                    values.Add(new ProductBasicInfoClass()
                    {
                        Service_Broker_ID = pair.Value[0].ToString(),
                        
                        //CNC_ID = pair.Value[1].ToString(),
                        //CNC_Type = pair.Value[2].ToString(),
                        //Product_ID = pair.Value[3].ToString(),
                        //Product_Type = pair.Value[4].ToString(),
                        //X_Table = pair.Value[5].ToString(),
                        //Y_Table = pair.Value[6].ToString()



                        vMachine_ID = pair.Value[1].ToString(),
                        CNC_ID = pair.Value[2].ToString(),
                        CNC_Type = pair.Value[3].ToString(),
                        Product_ID = pair.Value[4].ToString(),
                        Product_Type = pair.Value[5].ToString(),
                        X_Table = pair.Value[6].ToString(),
                        Y_Table = pair.Value[7].ToString()
                    });
//                     foreach (List<string> ss in pair)
//                     {        
//                         
//                     }
// 
//                     values.Add(new ProductBasicInfoClass()
//                     {
//                         ContextID = strContextID,
//                         Type = strType,
//                         SubType = strSubType,
//                         MachineTool = strMachineTool,
//                         Time = strTime
//                     });
                }
                ui_DG_ProductBasicInfoList.ItemsSource = values;

                //ui_DG_ProductBasicInfoList.ItemsSource = e.Result.ToList();
                ui_DG_ProductBasicInfoList.IsEnabled = true;

                // 選取預設值
                if (ui_DG_ProductBasicInfoList.ItemsSource != null && 
                    ui_DG_ProductBasicInfoList.IsEnabled == true)
                {
                    ui_DG_ProductBasicInfoList.SelectedIndex = 0;
                }
            }
            else
            {
                ui_DG_ProductBasicInfoList.ItemsSource = null;
                ui_DG_ProductBasicInfoList.IsEnabled = false;
                ui_DC_vMachine.ItemsSource = null;
                ui_DC_vMachine.IsEnabled = false;
                ui_DC_CNCnumber.ItemsSource = null;
                ui_DC_CNCnumber.IsEnabled = false;
                ui_DC_NCprogram.ItemsSource = null;
                ui_DC_NCprogram.IsEnabled = false;
                ui_DC_MetrologyType.ItemsSource = null;
                ui_DC_MetrologyType.IsEnabled = false;
                ui_DC_IndicatorType.ItemsSource = null;
                ui_DC_IndicatorType.IsEnabled = false;
                ui_DC_Search.IsEnabled = false;
                ui_DC_NextStep.IsEnabled = false;
            }
        }

        public void DestroyPage()
        {
            // 呼叫清空NextStep Tab動作
            if (DestroyNextStep != null)
            {
                DestroyNextStep(null, new EventArgs());
            }
            ui_DC_vMachine.ItemsSource = null;
            ui_DC_vMachine.IsEnabled = false;

            ui_DC_CNCnumber.ItemsSource = null;
            ui_DC_CNCnumber.IsEnabled = false;

            ui_DC_NCprogram.ItemsSource = null;
            ui_DC_NCprogram.IsEnabled = false;

            ui_DC_Search.IsEnabled = false;
            ui_DC_NextStep.IsEnabled = false;

            ui_DC_StartDate.SelectedDate = DateTime.Now.AddYears(-3);
            ui_DC_EndDate.SelectedDate = DateTime.Now;
            ui_DC_StartTime.Value = ui_DC_StartDate.SelectedDate;
            ui_DC_EndTime.Value = ui_DC_EndDate.SelectedDate;

            ui_DC_MetrologyType.ItemsSource = null;
            ui_DC_MetrologyType.IsEnabled = false;
            ui_DC_IndicatorType.ItemsSource = null;
            ui_DC_IndicatorType.IsEnabled = false;

            CheckBox cbAll = GetCheckBoxWithParent(ui_DC_IndicatorType, typeof(CheckBox), "ui_DC_CheckIndicatorAll");
            if (cbAll != null) { cbAll.IsChecked = false; }

            cbAll = GetCheckBoxWithParent(ui_DC_MetrologyType, typeof(CheckBox), "ui_DC_CheckMetrologyTypeAll");
            if (cbAll != null) { cbAll.IsChecked = false; }
        }
        #endregion

        #region For vMachine CNCNumber and NCProgram
        // 變更Vmachine_ID 同時執行 取得CNCNumber
        private void ui_DC_vMachine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ui_DC_vMachine.ItemsSource != null)
            {
                if (ui_DC_vMachine.SelectedIndex != -1)
                {
                    if (ui_DC_vMachine.SelectedValue != null)
                    {
                        LoadCNCNumber(ui_DC_vMachine.SelectedValue.ToString());
                    }
                }
            }
        }
        // 變更CNCNumber 同時執行 取得NC Number
        private void ui_DC_CNCnumber_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ui_DC_vMachine.SelectedIndex != -1 && ui_DC_CNCnumber.SelectedIndex != -1)
            {
                if (ui_DC_CNCnumber.SelectedValue != null)
                {
                    LoadNCProgram(ui_DC_vMachine.SelectedValue.ToString(), ui_DC_CNCnumber.SelectedValue.ToString());
                }
            }
        }
        // 變更NC Number 同時執行 取得量測種類與Indicator種類
        private void ui_DC_NCprogram_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ui_DC_vMachine.SelectedIndex != -1 && ui_DC_CNCnumber.SelectedIndex != -1 && ui_DC_NCprogram.SelectedIndex != -1)
            {
                if (ui_DC_NCprogram.SelectedValue != null)
                {
                    ui_DC_Search.IsEnabled = true;
                    ui_DC_NextStep.IsEnabled = false;
                    //ui_DC_Search_Click(this, new RoutedEventArgs());

                    if (ui_DG_ProductBasicInfoList.SelectedItem != null)
                    {
                        ProductBasicInfoClass currentRow = (ProductBasicInfoClass)ui_DG_ProductBasicInfoList.SelectedItem;

                        BuildVariableData(currentRow.X_Table, currentRow.Y_Table);
                    }
                }
                else 
                {
                    ui_DC_Search.IsEnabled = false;
                    ui_DC_NextStep.IsEnabled = false;
                }
            }
        }

        void getVmachine_ID()
        {
          //  Shell.waitingForm.SettingMessage("Loading v-Machine ID");
            Shell.waitingForm.SettingMessage("v-Machine ID 載入中");
            Shell.waitingForm.Show();

            App.proxyMC.getVmachine_IDCompleted += new EventHandler<getVmachine_IDCompletedEventArgs>(getVmachine_IDCompletedEventHandler);
            try
            {
                App.proxyMC.getVmachine_IDAsync();
            }
            catch (Exception ex)
            {
               // MessageBox.Show("There are errors on the server. " + ex.ToString());
                MessageBox.Show("伺服器發生錯誤. " + ex.ToString());
            }
        }
        // 完成取得Vmachine_ID
        private void getVmachine_IDCompletedEventHandler(object sender, getVmachine_IDCompletedEventArgs e)
        {
            bool IsSuccess = false;
            try
            {
                IsSuccess = true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error: " + ex.ToString());
                MessageBox.Show("錯誤訊息: " + ex.ToString());
                IsSuccess = false;
            }

            App.proxyMC.getVmachine_IDCompleted -= new EventHandler<getVmachine_IDCompletedEventArgs>(getVmachine_IDCompletedEventHandler);
            Shell.waitingForm.Close();

            if (IsSuccess)
            {
                ui_DC_vMachine.ItemsSource = e.Result.ToList();
                ui_DC_vMachine.IsEnabled = true;

                // 選取預設值
                if (ui_DC_vMachine.ItemsSource != null && ui_DC_vMachine.IsEnabled == true && ui_DC_vMachine.Items.Count > 0)
                {
                    ui_DC_vMachine.SelectedIndex = 0;
                }
            }
            else 
            {
                ui_DC_vMachine.ItemsSource = null;
                ui_DC_vMachine.IsEnabled = false;
                ui_DC_CNCnumber.ItemsSource = null;
                ui_DC_CNCnumber.IsEnabled = false;
                ui_DC_NCprogram.ItemsSource = null;
                ui_DC_NCprogram.IsEnabled = false;
                ui_DC_MetrologyType.ItemsSource = null;
                ui_DC_MetrologyType.IsEnabled = false;
                ui_DC_IndicatorType.ItemsSource = null;
                ui_DC_IndicatorType.IsEnabled = false;
                ui_DC_Search.IsEnabled = false;
                ui_DC_NextStep.IsEnabled = false;
            }
        }

        // 取得CNCNumber
        void LoadCNCNumber(string VmachineID)
        {
          //  Shell.waitingForm.SettingMessage("Loading CNC Number");
            Shell.waitingForm.SettingMessage("載入 CNC Number 中");
            Shell.waitingForm.Show();

            App.proxyMC.getCNC_NumberCompleted += new EventHandler<getCNC_NumberCompletedEventArgs>(getCNC_NumberEventHandler);
            try
            {
                App.proxyMC.getCNC_NumberAsync(VmachineID);

            }
            catch (Exception ex)
            {
               // MessageBox.Show("There are errors on the server. " + ex.ToString());
                MessageBox.Show("伺服器發生錯誤. " + ex.ToString());
            }
        }

        // 完成取得CNCNumber
        private void getCNC_NumberEventHandler(object sender, getCNC_NumberCompletedEventArgs e)
        {
            bool IsSuccess = false;
            try
            {
                IsSuccess = true;
            }
            catch (Exception ex)
            {
               // MessageBox.Show("Error: " + ex.ToString());
                MessageBox.Show("錯誤訊息: " + ex.ToString());
                IsSuccess = false;
            }

            App.proxyMC.getCNC_NumberCompleted -= new EventHandler<getCNC_NumberCompletedEventArgs>(getCNC_NumberEventHandler);
            Shell.waitingForm.Close();

            if (IsSuccess)
            {
                ui_DC_CNCnumber.ItemsSource = e.Result.ToList();
                ui_DC_CNCnumber.IsEnabled = true;

                // 選取預設值
                if (ui_DC_CNCnumber.ItemsSource != null && ui_DC_CNCnumber.IsEnabled == true && ui_DC_CNCnumber.Items.Count > 0)
                {
                    ui_DC_CNCnumber.SelectedIndex = 0;
                }
            }
            else
            {
                ui_DC_CNCnumber.ItemsSource = null;
                ui_DC_CNCnumber.IsEnabled = false;
                ui_DC_NCprogram.ItemsSource = null;
                ui_DC_NCprogram.IsEnabled = false;
                ui_DC_MetrologyType.ItemsSource = null;
                ui_DC_MetrologyType.IsEnabled = false;
                ui_DC_IndicatorType.ItemsSource = null;
                ui_DC_IndicatorType.IsEnabled = false;
                ui_DC_Search.IsEnabled = false;
                ui_DC_NextStep.IsEnabled = false;
            }
        }

        // 取得NC Number
        void LoadNCProgram(string VmachineID, string CNCNumber)
        {
           // Shell.waitingForm.SettingMessage("Loading NC Programs");
            Shell.waitingForm.SettingMessage("載入 NC Programs 中");
            Shell.waitingForm.Show();

            App.proxyMC.getNCProgram_IDCompleted += new EventHandler<getNCProgram_IDCompletedEventArgs>(getNCProgram_IDEventHandler);
            try
            {
                App.proxyMC.getNCProgram_IDAsync(VmachineID, CNCNumber);
            }
            catch (Exception ex)
            {
              //  MessageBox.Show("There are errors on the server. " + ex.ToString());
                MessageBox.Show("伺服器發生錯誤. " + ex.ToString());
            }
        }
        // 完成取得NC Number
        private void getNCProgram_IDEventHandler(object sender, getNCProgram_IDCompletedEventArgs e)
        {
            bool IsSuccess = false;
            try
            {
                IsSuccess = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString());
                IsSuccess = false;
            }

            App.proxyMC.getNCProgram_IDCompleted -= new EventHandler<getNCProgram_IDCompletedEventArgs>(getNCProgram_IDEventHandler);
            Shell.waitingForm.Close();

            if (IsSuccess)
            {
                ui_DC_NCprogram.ItemsSource = e.Result.ToList();
                ui_DC_NCprogram.IsEnabled = true;

                // 選取預設值
                if (ui_DC_NCprogram.ItemsSource != null && ui_DC_NCprogram.IsEnabled == true && ui_DC_NCprogram.Items.Count > 0)
                {
                    ui_DC_NCprogram.SelectedIndex = 0;
                }
            }
            else
            {
                ui_DC_NCprogram.ItemsSource = null;
                ui_DC_NCprogram.IsEnabled = false;
                ui_DC_MetrologyType.ItemsSource = null;
                ui_DC_MetrologyType.IsEnabled = false;
                ui_DC_IndicatorType.ItemsSource = null;
                ui_DC_IndicatorType.IsEnabled = false;
                ui_DC_Search.IsEnabled = false;
                ui_DC_NextStep.IsEnabled = false;
            }
        }
        #endregion

        #region For Indicator and Metrology Item
        
        //取得量測種類與Indicator種類
        private void ui_DC_Search_Click(object sender, RoutedEventArgs e)
        {
            if (ui_DG_ProductBasicInfoList.SelectedItem != null)
            {
                ProductBasicInfoClass currentRow = (ProductBasicInfoClass)ui_DG_ProductBasicInfoList.SelectedItem;

                BuildVariableData(currentRow.X_Table, currentRow.Y_Table);
            }
            
        }

        private void ui_DG_ProductBasicInfoList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ui_DG_ProductBasicInfoList.SelectedItem != null)
            {
                ProductBasicInfoClass currentRow = (ProductBasicInfoClass)ui_DG_ProductBasicInfoList.SelectedItem;

                BuildVariableData(currentRow.X_Table, currentRow.Y_Table);
            }
        }

        //抓取量測種類與Indicator種類
        void BuildVariableData(String strXTableName, String strYTableName)
        {
            //ui_DC_MetrologyType.IsEnabled = false;
            //ui_DC_IndicatorType.IsEnabled = false;

           // Shell.waitingForm.SettingMessage("Loading Variable Data");
            Shell.waitingForm.SettingMessage("變數資料載入中");
            Shell.waitingForm.Show();

            ui_DC_MetrologyType.ItemsSource = null;
            ui_DC_IndicatorType.ItemsSource = null;
            ui_DC_MetrologyType.IsEnabled = false;
            ui_DC_IndicatorType.IsEnabled = false;
            ui_DC_NextStep.IsEnabled = false;

            pDC_LocalContainer.Clear();

            CheckBox CBTemp = GetCheckBoxWithParent(ui_DC_IndicatorType, typeof(CheckBox), 
                "ui_DC_CheckIndicatorAll");
            if (CBTemp != null)
            { CBTemp.IsChecked = false; }

            CBTemp = GetCheckBoxWithParent(ui_DC_MetrologyType, typeof(CheckBox), 
                "ui_DC_CheckMetrologyTypeAll");
            if (CBTemp != null)
            { CBTemp.IsChecked = false; }

            //getListOfMetrology
            //App.proxyMC.getListOfMetrologyCompleted += new EventHandler<getListOfMetrologyCompletedEventArgs>(getListOfMetrologyEventHandler);
            //App.proxyMC.getListOfIndicatorCompleted += new EventHandler<getListOfIndicatorCompletedEventArgs>(getListOfIndicatorEventHandler);
            //App.proxyMC.getListOfMetrologyAsync();
            //App.proxyMC.getListOfIndicatorAsync();

            //////////////////////////////////////////////////////////////////////////
            // 新版XY List [7/27/2012 pili7545]
            App.proxyMC.GetXTableDEFCompleted += new EventHandler<GetXTableDEFCompletedEventArgs>(GetXTableDEFEventHandler);
            App.proxyMC.GetXTableDEFAsync(strXTableName);
            App.proxyMC.GetYTableDEFCompleted += new EventHandler<GetYTableDEFCompletedEventArgs>(GetYTableDEFEventHandler);
            App.proxyMC.GetYTableDEFAsync(strYTableName);
        }

        //完成取得Metrology種類
        private void GetYTableDEFEventHandler(object sender, GetYTableDEFCompletedEventArgs e)
        {
            bool IsSuccess = false;

            try
            {
                ObservableCollection<ModelCreation.MetrologyPoint> list = e.Result;
                pDC_LocalContainer.MetrologyTypeList = CloneObservableCollectionToList(list);
                IsSuccess = true;
            }
            catch (Exception ex)
            {
               // MessageBox.Show("Error: " + ex.ToString());
                MessageBox.Show("錯誤訊息: " + ex.ToString());
                IsSuccess = false;

            }

            App.proxyMC.getListOfMetrologyCompleted -= new EventHandler<getListOfMetrologyCompletedEventArgs>(getListOfMetrologyEventHandler);

            if (IsSuccess)
            {
               // ui_DC_MetrologyTypeTitle.Text = "Metrology Items (0/" + pDC_LocalContainer.MetrologyTypeList.Count.ToString() + ")";
                ui_DC_MetrologyTypeTitle.Text = "量測精度 (0/" + pDC_LocalContainer.MetrologyTypeList.Count.ToString() + ")";
                pDC_LocalContainer.MetrologyTypeCheckStateList.Clear();
                for (int i = 0; i < pDC_LocalContainer.MetrologyTypeList.Count; i++)
                {
                    CheckBox chk = new CheckBox();
                    chk.IsChecked = false;
                    pDC_LocalContainer.MetrologyTypeCheckStateList.Add(chk);
                }
                ui_DC_MetrologyType.ItemsSource = pDC_LocalContainer.MetrologyTypeList;
            }
            else
            {
                ui_DC_MetrologyType.ItemsSource = null;
            }
            
            ui_DC_MetrologyType.IsEnabled = true;
            
            //兩種抓完後才關閉等待視窗
            //if (ui_DC_MetrologyType.IsEnabled && ui_DC_IndicatorType.IsEnabled)
            {
                Shell.waitingForm.Close();
            }
        }
        //完成取得Indicator種類
        private void GetXTableDEFEventHandler(object sender, GetXTableDEFCompletedEventArgs e)
        {
            bool IsSuccess = false;
            try
            {
                ObservableCollection<ModelCreation.MetrologyPoint> list = e.Result;
                pDC_LocalContainer.IndicatorTypeList = CloneObservableCollectionToList(list);
                IsSuccess = true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error: " + ex.ToString());
                MessageBox.Show("錯誤訊息: " + ex.ToString());
                IsSuccess = false;
            }

            App.proxyMC.GetXTableDEFCompleted -= new EventHandler<GetXTableDEFCompletedEventArgs>(GetXTableDEFEventHandler);
            
            if (IsSuccess)
            {
                //ui_DC_IndicatorTypeTitle.Text = "Indicators (0/" + pDC_LocalContainer.IndicatorTypeList.Count.ToString() + ")";
                ui_DC_IndicatorTypeTitle.Text = "特徵值 (0/" + pDC_LocalContainer.IndicatorTypeList.Count.ToString() + ")";
                pDC_LocalContainer.IndicatorTypeCheckStateList.Clear();
                for (int i = 0; i < pDC_LocalContainer.IndicatorTypeList.Count; i++)
                {
                    CheckBox chk = new CheckBox();
                    chk.IsChecked = false;
                    pDC_LocalContainer.IndicatorTypeCheckStateList.Add(chk);
                }
                ui_DC_IndicatorType.ItemsSource = pDC_LocalContainer.IndicatorTypeList;
            }
            else
            {
                ui_DC_IndicatorType.ItemsSource = null;
            }

            ui_DC_IndicatorType.IsEnabled = true;

            //兩種抓完後才關閉等待視窗
            //if (ui_DC_MetrologyType.IsEnabled && ui_DC_IndicatorType.IsEnabled)
            {
                Shell.waitingForm.Close();
            }
        }
        //完成取得Metrology種類
        private void getListOfMetrologyEventHandler(object sender, getListOfMetrologyCompletedEventArgs e)
        {
            bool IsSuccess = false;

            try
            {
                ObservableCollection<ModelCreation.MetrologyPoint> list = e.Result;
                pDC_LocalContainer.MetrologyTypeList = CloneObservableCollectionToList(list);
                IsSuccess = true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error: " + ex.ToString());
                MessageBox.Show("錯誤訊息: " + ex.ToString());
                IsSuccess = false;
                
            }

            App.proxyMC.getListOfMetrologyCompleted -= new EventHandler<getListOfMetrologyCompletedEventArgs>(getListOfMetrologyEventHandler);
            
            if (IsSuccess)
            {
                //ui_DC_MetrologyTypeTitle.Text = "Metrology Items (0/" + pDC_LocalContainer.MetrologyTypeList.Count.ToString() + ")";
                ui_DC_MetrologyTypeTitle.Text = "量測精度 (0/" + pDC_LocalContainer.MetrologyTypeList.Count.ToString() + ")";
                pDC_LocalContainer.MetrologyTypeCheckStateList.Clear();
                for (int i = 0; i < pDC_LocalContainer.MetrologyTypeList.Count; i++)
                {
                    CheckBox chk = new CheckBox();
                    chk.IsChecked = false;
                    pDC_LocalContainer.MetrologyTypeCheckStateList.Add(chk);
                }
                ui_DC_MetrologyType.ItemsSource = pDC_LocalContainer.MetrologyTypeList;
            }
            else 
            {
                ui_DC_MetrologyType.ItemsSource = null;
            }

            ui_DC_MetrologyType.IsEnabled = true;
            ui_DC_MetrologyType.IsEnabled = true;
            ui_DC_MetrologyType.IsEnabled = true;
            ui_DC_MetrologyType.IsEnabled = true;
            ui_DC_MetrologyType.IsEnabled = true;
            ui_DC_MetrologyType.IsEnabled = true;

            //this.Dispatcher.BeginInvoke(delegate()
            //{
            //    ui_DC_MetrologyType.IsEnabled = true;
            //});
            //兩種抓完後才關閉等待視窗
            //if (ui_DC_MetrologyType.IsEnabled && ui_DC_IndicatorType.IsEnabled)
            {
                Shell.waitingForm.Close();
            }
        }
        //完成取得Indicator種類
        private void getListOfIndicatorEventHandler(object sender, getListOfIndicatorCompletedEventArgs e)
        {
            bool IsSuccess = false;
            try
            {
                ObservableCollection<ModelCreation.MetrologyPoint> list = e.Result;
                pDC_LocalContainer.IndicatorTypeList = CloneObservableCollectionToList(list);
                IsSuccess = true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error: " + ex.ToString());
                MessageBox.Show("錯誤訊息: " + ex.ToString());
                IsSuccess = false;
            }

            App.proxyMC.getListOfIndicatorCompleted -= new EventHandler<getListOfIndicatorCompletedEventArgs>(getListOfIndicatorEventHandler);
            
            if (IsSuccess)
            {
                //ui_DC_IndicatorTypeTitle.Text = "Indicators (0/" + pDC_LocalContainer.IndicatorTypeList.Count.ToString() + ")";
                ui_DC_IndicatorTypeTitle.Text = "特徵值 (0/" + pDC_LocalContainer.IndicatorTypeList.Count.ToString() + ")";
                pDC_LocalContainer.IndicatorTypeCheckStateList.Clear();
                for (int i = 0; i < pDC_LocalContainer.IndicatorTypeList.Count; i++)
                {
                    CheckBox chk = new CheckBox();
                    chk.IsChecked = false;
                    pDC_LocalContainer.IndicatorTypeCheckStateList.Add(chk);
                }
                ui_DC_IndicatorType.ItemsSource = pDC_LocalContainer.IndicatorTypeList;
            }
            else
            {
                ui_DC_IndicatorType.ItemsSource = null;
            }
            ui_DC_IndicatorType.IsEnabled = true;
            ui_DC_IndicatorType.IsEnabled = true;
            ui_DC_IndicatorType.IsEnabled = true;
            ui_DC_IndicatorType.IsEnabled = true;
            ui_DC_IndicatorType.IsEnabled = true;

            //this.Dispatcher.BeginInvoke(delegate() {  
            //    ui_DC_IndicatorType.IsEnabled = true;
            //});
            //兩種抓完後才關閉等待視窗
            //if (ui_DC_MetrologyType.IsEnabled && ui_DC_IndicatorType.IsEnabled)
            {
                Shell.waitingForm.Close();
            }
        }

        private void ui_DC_MetrologyType_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            CheckBox chk = ui_DC_MetrologyType.Columns[1].GetCellContent(e.Row) as CheckBox;
            chk.IsChecked = pDC_LocalContainer.MetrologyTypeCheckStateList[e.Row.GetIndex()].IsChecked;
        }

        private void ui_DC_IndicatorType_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            CheckBox chk = ui_DC_IndicatorType.Columns[1].GetCellContent(e.Row) as CheckBox;
            chk.IsChecked = pDC_LocalContainer.IndicatorTypeCheckStateList[e.Row.GetIndex()].IsChecked;
        }

        void ui_DC_CheckMetrologyTypeAll_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            bool check = chk.IsChecked.Value;
            int iMetrologyCount = 0;
            foreach (CheckBox chkbox in pDC_LocalContainer.MetrologyTypeCheckStateList)
            {
                chkbox.IsChecked = check;
                iMetrologyCount++;
            }
            if (check)
            {
                //ui_DC_MetrologyTypeTitle.Text = "Metrology Items (" + iMetrologyCount.ToString() + "/" + iMetrologyCount.ToString() + ")";
                ui_DC_MetrologyTypeTitle.Text = "量測精度 (" + iMetrologyCount.ToString() + "/" + iMetrologyCount.ToString() + ")";
                pDC_LocalContainer.SelectedMetrologyItemsCount = iMetrologyCount;
            }
            else
            {
                //ui_DC_MetrologyTypeTitle.Text = "Metrology Items (0/" + iMetrologyCount.ToString() + ")";
                ui_DC_MetrologyTypeTitle.Text = "量測精度 (0/" + iMetrologyCount.ToString() + ")";
                pDC_LocalContainer.SelectedMetrologyItemsCount = 0;
            }

            ui_DC_MetrologyType.ItemsSource = null;
            ui_DC_MetrologyType.ItemsSource = pDC_LocalContainer.MetrologyTypeList;

            if (pDC_LocalContainer.SelectedIndicatorItemsCount > 0 && pDC_LocalContainer.SelectedMetrologyItemsCount > 0)
                ui_DC_NextStep.IsEnabled = true;
            else
                ui_DC_NextStep.IsEnabled = false;
        }

        void ui_DC_CheckIndicatorTypeAll_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            bool check = chk.IsChecked.Value;
            int iIndicatorCount = 0;
            foreach (CheckBox chkbox in pDC_LocalContainer.IndicatorTypeCheckStateList)
            {
                chkbox.IsChecked = check;
                iIndicatorCount++;
            }
            if (check)
            {
               // ui_DC_IndicatorTypeTitle.Text = "Indicators (" + iIndicatorCount.ToString() + "/" + iIndicatorCount.ToString() + ")";
                ui_DC_IndicatorTypeTitle.Text = "特徵值 (" + iIndicatorCount.ToString() + "/" + iIndicatorCount.ToString() + ")";
                pDC_LocalContainer.SelectedIndicatorItemsCount = iIndicatorCount;
            }
            else
            {
                //ui_DC_IndicatorTypeTitle.Text = "Indicators (0/" + iIndicatorCount.ToString() + ")";
                ui_DC_IndicatorTypeTitle.Text = "特徵值 (0/" + iIndicatorCount.ToString() + ")";
                pDC_LocalContainer.SelectedIndicatorItemsCount = 0;
            }

            ui_DC_IndicatorType.ItemsSource = null;
            ui_DC_IndicatorType.ItemsSource = pDC_LocalContainer.IndicatorTypeList;

            if (pDC_LocalContainer.SelectedIndicatorItemsCount > 0 && pDC_LocalContainer.SelectedMetrologyItemsCount > 0)
                ui_DC_NextStep.IsEnabled = true;
            else
                ui_DC_NextStep.IsEnabled = false;
        }

        private void ui_DC_CheckMetrologyType_Click(object sender, RoutedEventArgs e)
        {
            CheckBox OriginalCheckBox = (CheckBox)e.OriginalSource; //取得Checkbox目標的狀態
            int Index = ui_DC_MetrologyType.SelectedIndex;
            pDC_LocalContainer.MetrologyTypeCheckStateList[Index].IsChecked = OriginalCheckBox.IsChecked;

            CheckBox CBParent = GetCheckBoxWithParent(ui_DC_MetrologyType, typeof(CheckBox), "ui_DC_CheckMetrologyTypeAll");
            if (CBParent != null)
            {
                bool AllCheck = true;
                int iSelectedMetrologyCount = 0;
                int iMetrologyCount = 0;
                foreach (CheckBox chkbox in pDC_LocalContainer.MetrologyTypeCheckStateList)
                {
                    AllCheck = AllCheck && (chkbox.IsChecked == true);
                    iMetrologyCount++;
                    if (chkbox.IsChecked == true)
                    {
                        iSelectedMetrologyCount++;
                    }
                }
                CBParent.IsChecked = AllCheck;
               // ui_DC_MetrologyTypeTitle.Text = "Metrology Items (" + iSelectedMetrologyCount.ToString() + "/" + iMetrologyCount.ToString() + ")";
                ui_DC_MetrologyTypeTitle.Text = "量測精度 (" + iSelectedMetrologyCount.ToString() + "/" + iMetrologyCount.ToString() + ")";
                pDC_LocalContainer.SelectedMetrologyItemsCount = iSelectedMetrologyCount;
            }

            if (pDC_LocalContainer.SelectedIndicatorItemsCount > 0 && pDC_LocalContainer.SelectedMetrologyItemsCount > 0)
                ui_DC_NextStep.IsEnabled = true;
            else
                ui_DC_NextStep.IsEnabled = false;
        }

        private void ui_DC_CheckIndicatorType_Click(object sender, RoutedEventArgs e)
        {
            CheckBox OriginalCheckBox = (CheckBox)e.OriginalSource; //取得Checkbox目標的狀態
            int Index = ui_DC_IndicatorType.SelectedIndex;
            pDC_LocalContainer.IndicatorTypeCheckStateList[Index].IsChecked = OriginalCheckBox.IsChecked;

            CheckBox CBParent = GetCheckBoxWithParent(ui_DC_IndicatorType, typeof(CheckBox), "ui_DC_CheckIndicatorAll");
            if (CBParent != null)
            {
                bool AllCheck = true;
                int iSelectedIndicatorCount = 0;
                int iIndicatorCount = 0;
                foreach (CheckBox chkbox in pDC_LocalContainer.IndicatorTypeCheckStateList)
                {
                    AllCheck = AllCheck && (chkbox.IsChecked == true);
                    iIndicatorCount++;
                    if (chkbox.IsChecked == true)
                    {
                        iSelectedIndicatorCount++;
                    }
                }
                CBParent.IsChecked = AllCheck;
                //ui_DC_IndicatorTypeTitle.Text = "Indicators (" + iSelectedIndicatorCount.ToString() + "/" + iIndicatorCount.ToString() + ")";
                ui_DC_IndicatorTypeTitle.Text = "特徵值 (" + iSelectedIndicatorCount.ToString() + "/" + iIndicatorCount.ToString() + ")";
                pDC_LocalContainer.SelectedIndicatorItemsCount = iSelectedIndicatorCount;
            }

            if (pDC_LocalContainer.SelectedIndicatorItemsCount > 0 && pDC_LocalContainer.SelectedMetrologyItemsCount > 0)
                ui_DC_NextStep.IsEnabled = true;
            else
                ui_DC_NextStep.IsEnabled = false;
        }

        #endregion

        #region Next Step
        
        private void ui_DC_NextStep_Click(object sender, RoutedEventArgs e)
        {
            ProductBasicInfoClass currentRow = (ProductBasicInfoClass)ui_DG_ProductBasicInfoList.SelectedItem;

            //currentRow.

            

            GetNewPopulationData();
        }
        
        void GetNewPopulationData()
        {
            ObservableCollection<MetrologyPoint> listMetrology = GetObCollectionByCheckList(pDC_LocalContainer.MetrologyTypeList, pDC_LocalContainer.MetrologyTypeCheckStateList);
            ObservableCollection<MetrologyPoint> listIndicator = GetObCollectionByCheckList(pDC_LocalContainer.IndicatorTypeList, pDC_LocalContainer.IndicatorTypeCheckStateList);

            if (listMetrology.Count <= 0 || listIndicator.Count <= 0)
            {
               // MessageBox.Show("You Must Select Last One of Indicator And Metrology Item");
                MessageBox.Show("必須選擇至少一個特徵值和量測項目");
                return;
            }

            ProductBasicInfoClass currentRow =
                    (ProductBasicInfoClass)ui_DG_ProductBasicInfoList.SelectedItem;

            pDC_GlobalContainer.ServiceBrokerID = currentRow.Service_Broker_ID;
            pDC_GlobalContainer.ProductID = currentRow.Product_ID;
            pDC_GlobalContainer.CNCnumber = currentRow.CNC_ID;
            pDC_GlobalContainer.CNCType = currentRow.CNC_Type;
            pDC_GlobalContainer.vMachine = currentRow.vMachine_ID;
            

            pDC_LocalContainer.XTableName = currentRow.X_Table;
            pDC_LocalContainer.YTableName = currentRow.Y_Table;

            pDC_LocalContainer.SearchStartTime = DateTime.ParseExact(ui_DC_StartDate.SelectedDate.Value.ToString("yyyy/MM/dd") + " " + ui_DC_StartTime.Value.Value.ToString("HH:mm:ss"), "yyyy/MM/dd HH:mm:ss", null);
            pDC_LocalContainer.SearchEndTime = DateTime.ParseExact(ui_DC_EndDate.SelectedDate.Value.ToString("yyyy/MM/dd") + " " + ui_DC_EndTime.Value.Value.ToString("HH:mm:ss"), "yyyy/MM/dd HH:mm:ss", null);

            DateTime startTime = pDC_LocalContainer.SearchStartTime;
            DateTime endTime = pDC_LocalContainer.SearchEndTime;

            //Shell.waitingForm.SettingMessage("Loading Population Data");
            Shell.waitingForm.SettingMessage("載入所有建模資料中");
            Shell.waitingForm.Show();

            

            App.proxyMC.getNewPopulationCompleted += new EventHandler<getNewPopulationCompletedEventArgs>(getNewPopulationCompletedEvent);
            App.proxyMC.getNewPopulationAsync(listIndicator, listMetrology, startTime, endTime, currentRow.X_Table, currentRow.Y_Table);
        }

        private void getNewPopulationCompletedEvent(object sender, getNewPopulationCompletedEventArgs e)
        {
            bool IsSuccess = false;

            App.proxyMC.getNewPopulationCompleted -= new EventHandler<getNewPopulationCompletedEventArgs>(getNewPopulationCompletedEvent);
                        
            MCDataCollectionGlobalContainer pDC_GlobalContainerTemp = new MCDataCollectionGlobalContainer();
            try
            {
                pDC_GlobalContainerTemp.Clear();
                Out_IndicatorsPopulation list = e.Result;
                if (list.Ack == 0)
                {
                    if (list.listContexID.Count >= 3)
                    {

                        //pDC_GlobalContainerTemp.vMachine = ui_DC_vMachine.SelectedValue.ToString();
                        //pDC_GlobalContainerTemp.CNCnumber = ui_DC_CNCnumber.SelectedValue.ToString();
                        //pDC_GlobalContainerTemp.NCprogram = ui_DC_NCprogram.SelectedValue.ToString();

                        pDC_GlobalContainer.XTableName = pDC_LocalContainer.XTableName;
                        pDC_GlobalContainer.YTableName = pDC_LocalContainer.YTableName;
                        
                        //pDC_GlobalContainerTemp.CNCType = "CNCType1";
                        //pDC_GlobalContainerTemp.model_Id = "001";
                        //pDC_GlobalContainerTemp.version = "1.0";

                        ProductBasicInfoClass currentRow =
                    (ProductBasicInfoClass)ui_DG_ProductBasicInfoList.SelectedItem;

                        pDC_GlobalContainerTemp.ServiceBrokerID = currentRow.Service_Broker_ID;
                        pDC_GlobalContainerTemp.ProductID = currentRow.Product_ID;
                        pDC_GlobalContainerTemp.CNCnumber = currentRow.CNC_ID;
                        pDC_GlobalContainerTemp.CNCType = currentRow.CNC_Type;
                        pDC_GlobalContainerTemp.vMachine = currentRow.vMachine_ID;


                        pDC_GlobalContainerTemp.LoginUsername = StateManager.Username;
                        pDC_GlobalContainerTemp.Company = StateManager.UserCompany;

                        pDC_GlobalContainerTemp.allAction = new ObservableCollection<int>();
                        for (int i = 0; i < iActionCount; i++) pDC_GlobalContainerTemp.allAction.Add(i + 1);//之後要從DB上抓下來

                        pDC_GlobalContainerTemp.SearchStartTime = pDC_LocalContainer.SearchStartTime;
                        pDC_GlobalContainerTemp.SearchEndTime = pDC_LocalContainer.SearchEndTime;

                        //Get Indicator List
                        pDC_GlobalContainerTemp.listContextID = list.listContexID.ToArray();
                        pDC_GlobalContainerTemp.listIndicatorPopulationValue = utilityChart.ConvertToRealPredictData(list.listIndicatorPopulationValue.ToArray());
                        pDC_GlobalContainerTemp.listActionPopulationValue = utilityChart.ConvertToRealPredictDataInt(list.listActionPopulationValue.ToArray());
                        pDC_GlobalContainerTemp.listPointPopulationValue = utilityChart.ConvertToRealPredictData(list.listPointPopulationValue.ToArray());

                        pDC_GlobalContainerTemp.contextList = list.listContext;
                        pDC_GlobalContainerTemp.TotalcontextListCount = list.listContext.Count;
                        pDC_GlobalContainerTemp.listAllIndicators = list.listAllIndicators;
                        pDC_GlobalContainerTemp.listAllPoints = list.listAllPoints;

                        int iContextCount = 0;
                        
                        //foreach (Context item in list.listContext)
                        //{
                        //    if (iContextCount == 0)
                        //    {
                        //        pDC_GlobalContainerTemp.strSelectedContextID += item.ContextID;
                        //    }
                        //    else
                        //    {
                        //        pDC_GlobalContainerTemp.strSelectedContextID += ", " + item.ContextID;
                        //    }
                            
                        //    iContextCount++;
                        //}

                        foreach (String strContextID in pDC_GlobalContainerTemp.listContextID)
                        {
                            if (iContextCount == 0)
                            {
                                pDC_GlobalContainerTemp.strSelectedContextID += strContextID;
                            }
                            else
                            {
                                pDC_GlobalContainerTemp.strSelectedContextID += ", " + strContextID;
                            }

                            iContextCount++;
                        }



                        pDC_GlobalContainerTemp.IndicatorTypeCheckStateList = CloneList(pDC_LocalContainer.IndicatorTypeCheckStateList);
                        pDC_GlobalContainerTemp.MetrologyTypeCheckStateList = CloneList(pDC_LocalContainer.MetrologyTypeCheckStateList);
                        pDC_GlobalContainerTemp.MetrologyTypeList = CloneList(pDC_LocalContainer.MetrologyTypeList);
                        pDC_GlobalContainerTemp.IndicatorTypeList = CloneList(pDC_LocalContainer.IndicatorTypeList);
                        pDC_GlobalContainerTemp.SelectedMetrologyTypeList = GetObCollectionByCheckList(pDC_GlobalContainerTemp.MetrologyTypeList, pDC_GlobalContainerTemp.MetrologyTypeCheckStateList);
                        pDC_GlobalContainerTemp.SelectedIndicatorTypeList = GetObCollectionByCheckList(pDC_GlobalContainerTemp.IndicatorTypeList, pDC_GlobalContainerTemp.IndicatorTypeCheckStateList);

                        IsSuccess = true;
                    }
                    else
                    {
                        //MessageBox.Show("The Population Result is less than 3 record!");
                        MessageBox.Show("所有測試項目結果少於3筆!");
                        IsSuccess = false;
                    }
                }
                else
                {
                    //MessageBox.Show("Error: the Ack of ModelCreation.Out_IndicatorsPopulation is zero.");
                    MessageBox.Show("錯誤訊息: ModelCreation.Out_IndicatorsPopulation 沒有 Ack 訊號");
                    IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
               // MessageBox.Show("Error: " + ex.ToString());
                MessageBox.Show("錯誤訊息: " + ex.ToString());
                IsSuccess = false;
            }
            
            Shell.waitingForm.Close();

            if (IsSuccess)
            {
                //////////////////////////////////////////////////////////////////////////
                //將GUI上選擇的資料傳到MCS之後，

                pDC_GlobalDI.CurrentInfo = new CurrentInfos();
                pDC_GlobalDI.X_Data = new ObservableCollection<XItem>();
                pDC_GlobalDI.Y_Data = new ObservableCollection<YItem>();
                pDC_GlobalDI.WP_Data = new ObservableCollection<AllPiece>();
                pDC_GlobalDI.WP_TrainData = new ObservableCollection<TrainPiece>();
                pDC_GlobalDI.WP_RunData = new ObservableCollection<RunPiece>();

                if (ui_DG_ProductBasicInfoList.SelectedItem != null)
                {
                    ProductBasicInfoClass currentRow = 
                    (ProductBasicInfoClass)ui_DG_ProductBasicInfoList.SelectedItem;

                    pDC_GlobalDI.ServiceBrokerInformation = new ServiceBrokerInfo();
                    pDC_GlobalDI.ServiceBrokerInformation.Name = currentRow.Service_Broker_ID;

                    //vMachineInfo vMI = new vMachineInfo();
                    //vMI.Name = "XXX";
                    //CNCInfo CNCI = new CNCInfo();
                    //CNCI.Name = currentRow.CNC_ID;
                    //CNCI.Type = currentRow.CNC_Type;
                    //vMI.CNCList = new ObservableCollection<CNCInfo>();
                    //vMI.CNCList.Add(CNCI);
                    //pDC_GlobalDI.ServiceBrokerInformation.vMachineList = new ObservableCollection<vMachineInfo>();
                    //pDC_GlobalDI.ServiceBrokerInformation.vMachineList.Add(vMI);

                    pDC_GlobalDI.CurrentInfo.CNCName = currentRow.CNC_ID;
                    pDC_GlobalDI.CurrentInfo.CNCType = currentRow.CNC_Type;
                    pDC_GlobalDI.CurrentInfo.ProductName = currentRow.Product_ID;
                    pDC_GlobalDI.CurrentInfo.ProductType = currentRow.Product_Type;
                    pDC_GlobalDI.CurrentInfo.ServiceBrokerID = currentRow.Service_Broker_ID;
                    //pDC_GlobalDI.CurrentInfo.vMachineID = currentRow.;
                    pDC_GlobalDI.CurrentInfo.XTableName = currentRow.X_Table;
                    pDC_GlobalDI.CurrentInfo.YTableName = currentRow.Y_Table;
                    
                    pDC_GlobalDI.StartTime = pDC_GlobalContainer.SearchStartTime.ToString("yyyy-MM-dd HH:mm:ss");
                    pDC_GlobalDI.EndTime = pDC_GlobalContainer.SearchEndTime.ToString("yyyy-MM-dd HH:mm:ss");

                    foreach (MetrologyPoint MP in pDC_GlobalContainer.SelectedIndicatorTypeList)
                    {
                        XItem xitem = new XItem();
                        xitem.Name = MP.Name;
                        xitem.Type = MP.MeasureType;
                        xitem.Position = "XXX";

                        pDC_GlobalDI.X_Data.Add(xitem);
                    }
                    foreach (MetrologyPoint MP in pDC_GlobalContainer.SelectedMetrologyTypeList)
                    {
                        YItem yitem = new YItem();
                        yitem.Name = MP.Name;
                        yitem.Type = MP.MeasureType;
                        yitem.Block = MP.Actions;
                        pDC_GlobalDI.Y_Data.Add(yitem);
                    }
                    foreach (ModelCreation.Context ctx in pDC_GlobalContainer.contextList)
                    {
                        AllPiece WP = new AllPiece();
                        WP.ID = ctx.ContextID.ToString();
                        WP.Process_StartTime = ctx.ProcessStartTime;
                        WP.Process_EndTime = ctx.ProcessEndTime;
                        WP.Metrology_StartTime = ctx.MetrologyStartTime;
                        WP.Metrology_EndTime = ctx.MetrologyEndTime;

                        pDC_GlobalDI.WP_Data.Add(WP);


                    }


                    
                    //App.proxyMC.DCtoBlobCompleted += new EventHandler<DCtoBlobCompletedEventArgs>(DCtoBlobCompletedEvent);
                    //App.proxyMC.DCtoBlobAsync(pDC_GlobalDI, StateManager.Username,  currentRow.X_Table, currentRow.Y_Table);
                }



                // 把pDC_GlobalContainerTemp複製到pDC_GlobalContainer
                pDC_GlobalContainer.Clear();
                pDC_GlobalContainer.Copy(pDC_GlobalContainerTemp);

                // 呼叫切換到DataSelection Tab動作
                if (ChangeToNextStep != null)
                {
                    ChangeToNextStep(null, new EventArgs());
                }


                
                
            }
            else 
            {
                // 停在本頁中
            }
        }


        private void DCtoBlobCompletedEvent(object sender, DCtoBlobCompletedEventArgs e)
        {
            bool IsSuccess = false;

            App.proxyMC.DCtoBlobCompleted -= new EventHandler<DCtoBlobCompletedEventArgs>(DCtoBlobCompletedEvent);
            
            try
            {
                IsSuccess = e.Result;                
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error: " + ex.ToString());
                MessageBox.Show("錯誤訊息: " + ex.ToString());
                IsSuccess = false;
            }

            Shell.waitingForm.Close();

            
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

        List<MetrologyPoint> CloneObservableCollectionToList(ObservableCollection<ModelCreation.MetrologyPoint> input)
        {
            ObservableCollection<ModelCreation.MetrologyPoint> TempOut = CommonValue.DataContractClone(input);
            List<MetrologyPoint> output = new List<MetrologyPoint>();
            foreach (MetrologyPoint TempIn in TempOut)
            {
                output.Add(TempIn);
            }
            return output;
        }

        ObservableCollection<MetrologyPoint> GetObCollectionByCheckList(List<MetrologyPoint> ListMetrologyPoint, List<CheckBox> ListCheckBox)
        {
            MetrologyPoint[] ListMetrologyPointTemp = ListMetrologyPoint.ToArray();
            ObservableCollection<MetrologyPoint> Output = new ObservableCollection<MetrologyPoint>();
            int i = 0;
            foreach (CheckBox chkbox in ListCheckBox)
            {
                if (chkbox.IsChecked.Value)
                {
                    Output.Add(ListMetrologyPointTemp[i]);                    
                }
                i++;
            }
            return Output;
        }

        // 複製資料
        List<CheckBox> CloneList(List<CheckBox> inList)
        {
            List<CheckBox> output = new List<CheckBox>();
            CheckBox TempOut = null;
            foreach (CheckBox TempIn in inList)
            {
                TempOut = new CheckBox();
                TempOut.IsChecked = TempIn.IsChecked;
                output.Add(TempOut);
            }
            return output;
        }

        // 複製資料
        List<MetrologyPoint> CloneList(List<MetrologyPoint> inList)
        {
            List<MetrologyPoint> output = new List<MetrologyPoint>();
            MetrologyPoint TempOut = null;
            foreach (MetrologyPoint TempIn in inList)
            {
                TempOut = new MetrologyPoint();
                TempOut.Actions = TempIn.Actions;
                TempOut.DataField = TempIn.DataField;
                TempOut.Name = TempIn.Name;
                TempOut.Value = TempIn.Value;
                TempOut.MeasureType = TempIn.MeasureType;
                TempOut.LCL = TempIn.LCL;
                TempOut.LSL = TempIn.LSL;
                TempOut.UCL = TempIn.UCL;
                TempOut.USL = TempIn.USL;
                output.Add(TempOut);
            }
            return output;
        }

        #endregion

        private void ui_DC_IndicatorType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ui_Button_RetrieveVariable_Click(object sender, RoutedEventArgs e)
        {
            if (ui_DG_ProductBasicInfoList.SelectedItem != null)
            {
                ProductBasicInfoClass currentRow = (ProductBasicInfoClass)ui_DG_ProductBasicInfoList.SelectedItem;

                BuildVariableData(currentRow.X_Table, currentRow.Y_Table);
            }
        }

        private void ui_DC_Search_Click_1(object sender, RoutedEventArgs e)
        {

        }



        
    }
}