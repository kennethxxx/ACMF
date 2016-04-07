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
using OMC.Comm;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.ServiceModel;
using System.ServiceModel.Channels;
using IPS.Common;

namespace IPS.Views
{
    public partial class OntologyTOC : UserControl
    {
        //記錄公司名稱，ServiceBrokerID，刀具種類，刀具直徑大小，CNCFileName。

        string CompanyName, ServiceBrokerID, MachineName_string, KindofTool_string, ToolofDiameter_string, NCFileName_string, Blob_string, MachineXMLInfo;
        string showNCFile = "";
        string showCNC = "";

        //宣告使用的服務

        IPS.OntologyService.OntologyServiceClient OntologyService;
        //IPS.MachineInfomation.Service1Client GetMachineInfoService;
        List<PE2_ToolSelection> lstMachinetoSimulationItem;
       
        XDocument CompanyXml, ServiceBrokerXml, MachineInfoXml;

        public OntologyTOC()
        {
            InitializeComponent();
            Initialize();
        }

        #region  網頁一啟動的前置作業

        private void Initialize()
        {//一開始顯示的提示文字
            LB_Info1.Text = "此步驟，系統將會顯示特定公司機台的刀具的資料。" + "\n" + "步驟一:選擇特定公司機台的位置資訊" + "\n" + "步驟二:選擇刀具進行過切比對。" + "\n" + "步驟三:請按下一步進入到刀具是否過切的比對。";
            LB_Info2.Text = "此步驟，能夠看到替換的刀具刀具過切展示圖，每張圖顯示刀具比對過切過程中的NC碼行數以及加工時間點。" + "\n" + "步驟一:請按下一步進入確替換的刀具不會過切與送出加工請求。";
            LB_Info3.Text = "此步驟，現場製程工程師選擇NC檔之新刀具。" + "\n" + "步驟一:顯示可替換NC檔之刀具資料。";

            ig_Undercutting.Visibility = Visibility.Collapsed;
            lb_Undercutting.Visibility = Visibility.Collapsed;
            btnLastToChangeTool.Visibility = Visibility.Collapsed;

            #region  呼叫Service Broker取得公司資訊的服務
            CallCompanyService();
            #endregion
        }

        #endregion

        #region TabControl切換的狀態管控

        private void Control_TabItemChange(int Step)
        {
            switch (Step)
            {
                case 1:
                    TBGetMachineStatus.IsEnabled = true;
                    TBGetMachineStatus.IsSelected = true;
                    break;
                case 2:
                    TIShowPic.IsEnabled = true;
                    TIShowPic.IsSelected = true;
                    break;
                case 3:
                    TBRealMachining.IsEnabled = true;
                    TBRealMachining.IsSelected = true;
                    break;
            }
        }

        #endregion

        #region 打包成XML格式字串Function

        private void ResolveXMLFormat(int index, string xmlStr)
        {
            switch (index)
            {
                case 1:
                    //公司資訊
                    ObservableCollection<string> companyitemsSource = new ObservableCollection<string>();
                    System.IO.MemoryStream _memorystreamCompany = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xmlStr));
                    CompanyXml = XDocument.Load(_memorystreamCompany);
                    foreach (var CompanyInfo in CompanyXml.Element("Machine_Scheduling_Log").Descendants("Company_name"))
                    {
                        companyitemsSource.Add(CompanyInfo.Value);
                    }
                    CBCompany.ItemsSource = companyitemsSource;
                    break;
                case 2:
                    //ServiceBroker資訊
                    ObservableCollection<string> servicebrokeritemsSource = new ObservableCollection<string>();
                    System.IO.MemoryStream _memorystreamServiceBroker = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xmlStr));
                    ServiceBrokerXml = XDocument.Load(_memorystreamServiceBroker);
                    foreach (var ServiceBrokerInfo in ServiceBrokerXml.Element("Machine_Scheduling_Log").Descendants("Service_Broker_ID"))
                    {
                        servicebrokeritemsSource.Add(ServiceBrokerInfo.Value);
                    }
                    CBServiceBroker.ItemsSource = servicebrokeritemsSource;
                    break;
                case 3:
                    //取得機台所有資訊
                    ObservableCollection<string> vMachineitemsSource = new ObservableCollection<string>();
                    GDGetMachineStatus.ItemsSource = null;
                    List<PE_GetStatusofMachines> CNCsource = new List<PE_GetStatusofMachines>();
                    ///取出多層中的Attribute所使用的方式
                    System.IO.MemoryStream _memorystream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xmlStr));
                    MachineInfoXml = XDocument.Load(_memorystream);
                    foreach (var MachineInfo in MachineInfoXml.Element("Machine_Scheduling_Log").Descendants("vMachine"))
                    {
                        vMachineitemsSource.Add(MachineInfo.Attribute("Name").Value);
                    }
                    CBvMachine.ItemsSource = vMachineitemsSource;
                    GDGetMachineStatus.ItemsSource = CNCsource;
                    break;
            }
        }

        #endregion

        #region Release memery

        private void release_memery(string stringTag)
        {
            switch (stringTag)
            {
                case "lstMachinetoSimulationItem":
                    lstMachinetoSimulationItem.Clear();
                    break;
            }
        }

        #endregion

        #region  呼叫Service Broker的GetCompanyInfo()取得公司資訊的服務

        private void CallCompanyService()
        {
            Shell.waitingForm.SettingMessage("連接服務中，請稍候");
            Shell.waitingForm.Show();
            OntologyService = new IPS.OntologyService.OntologyServiceClient();
            OntologyService.OntologySystemInterfaceAsync(StateManager.Username, "GetCompanyInfo2", null);
            OntologyService.OntologySystemInterfaceCompleted += new EventHandler<IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs>(proxy_GetCompanyInfoCompleted);
        }

        void proxy_GetCompanyInfoCompleted(object sender, IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs e)
        {
            OntologyService.OntologySystemInterfaceCompleted -= new EventHandler<IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs>(proxy_GetCompanyInfoCompleted);
            if (e.Error == null && e.Result != null)
            {
                //載入資料
                ResolveXMLFormat(1, e.Result);
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

        #endregion

        #region Step 1 to Step 2

        private void CBCompany_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CompanyName = CBCompany.SelectedItem.ToString().Trim();
            XDocument companyNameXML;
            companyNameXML = new XDocument(
              new XElement("Machine_Scheduling_Log",
                  new XElement("Company_name", CompanyName))
            );
            string XMLstring_CompanyName = companyNameXML.ToString();
            CallMachineInfoService(XMLstring_CompanyName);
        }

        #region   呼叫Service Broker的GetServiceBrokerInfo(XMLFile)取得ServiceBrokerID資訊的服務

        private void CallMachineInfoService(string XMLstring_CompanyName)
        {
            Shell.waitingForm.SettingMessage("連接服務中，請稍候");
            Shell.waitingForm.Show();
            OntologyService = new IPS.OntologyService.OntologyServiceClient();
            OntologyService.OntologySystemInterfaceAsync(StateManager.Username, "GetServiceBrokerInfo2", XMLstring_CompanyName);
            OntologyService.OntologySystemInterfaceCompleted += new EventHandler<IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs>(proxy_GetServiceBrokerInfoCompleted);
        }

        void proxy_GetServiceBrokerInfoCompleted(object sender, IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs e)
        {
            OntologyService.OntologySystemInterfaceCompleted -= new EventHandler<IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs>(proxy_GetServiceBrokerInfoCompleted);
            if (e.Error == null && e.Result != null)
            {
                MachineXMLInfo = e.Result;
                //載入資料
                //MessageBox.Show(e.Result);
                ResolveXMLFormat(2, e.Result);
                //if (CBServiceBroker.ItemsSource != null)
                //{
                //    CBServiceBroker.SelectedIndex = 0;
                //}
                Shell.waitingForm.Close();
            }
            else
            {
                MessageBox.Show("雲端服務忙錄中，請稍候再試!");
                Shell.waitingForm.Close();
            }
        }
        #endregion

        private void CBServiceBroker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CBCompany.SelectedIndex > -1)
            {
                ServiceBrokerID = CBServiceBroker.SelectedItem.ToString();
                XDocument ServiceBrokerXML;
                ServiceBrokerXML = new XDocument(
                  new XElement("Machine_Scheduling_Log",
                      new XElement("Service_Broker_ID", ServiceBrokerID,
                        new XAttribute("Company_Name", CompanyName)))
                );
                string XMLstring_ServiceBroker = ServiceBrokerXML.ToString();
                CallMachineCNCInfoService(XMLstring_ServiceBroker);
            }
            else
            {
                MessageBox.Show("請選擇公司!");
            }
        }

        #region   呼叫Service Broker的Get_vMachineInfo_Ontology(XMLFile)取得機台資訊的服務

        private void CallMachineCNCInfoService(string XMLstring_ServiceBroker)
        {
            Shell.waitingForm.SettingMessage("連接服務中，請稍候");
            Shell.waitingForm.Show();
            OntologyService = new IPS.OntologyService.OntologyServiceClient();
            OntologyService.OntologySystemInterfaceAsync(StateManager.Username, "Get_vMachineInfo_Ontology", XMLstring_ServiceBroker);
            OntologyService.OntologySystemInterfaceCompleted += new EventHandler<IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs>(proxy_Get_vMachineInfo_OntologyCompleted);
        }

        void proxy_Get_vMachineInfo_OntologyCompleted(object sender, IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs e)
        {
            OntologyService.OntologySystemInterfaceCompleted -= new EventHandler<IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs>(proxy_Get_vMachineInfo_OntologyCompleted);
            if (e.Error == null && e.Result != null)
            {
                ObservableCollection<string> vMachineitemsSource = new ObservableCollection<string>();
                //切割字串，使用逗號分割
                string[] arrvMachine = e.Result.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string vMachineString in arrvMachine)
                {
                    vMachineitemsSource.Add(vMachineString.ToString());
                }
                CBvMachine.ItemsSource = vMachineitemsSource;
                //if (CBvMachine.ItemsSource != null)
                //{
                //    CBvMachine.SelectedIndex = 0;
                //}
                Shell.waitingForm.Close();
            }
            else
            {
                MessageBox.Show("雲端服務忙錄中，請稍候再試!");
                Shell.waitingForm.Close();
            }
        }

        #endregion

        #region   呼叫Service Broker的Get_CNCInfo_Ontology(選擇的vmachine)取得機台資訊的服務

        private void CBvMachine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Shell.waitingForm.SettingMessage("連接服務中，請稍候");
            Shell.waitingForm.Show();
            OntologyService = new IPS.OntologyService.OntologyServiceClient();
            OntologyService.OntologySystemInterfaceAsync(StateManager.Username, "Get_CNCInfo_Ontology", CBvMachine.SelectedItem.ToString());
            OntologyService.OntologySystemInterfaceCompleted += new EventHandler<IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs>(proxy_Get_CNCInfo_OntologyCompleted);
        }

        void proxy_Get_CNCInfo_OntologyCompleted(object sender, IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs e)
        {
            OntologyService.OntologySystemInterfaceCompleted -= new EventHandler<IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs>(proxy_Get_CNCInfo_OntologyCompleted);
            if (e.Error == null && e.Result != null)
            {
                ObservableCollection<string> cncitemsSource = new ObservableCollection<string>();
                //載入資料
                //MessageBox.Show(e.Result);
                //切割字串，使用逗號分割

                string[] arrCNC = e.Result.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string CNCString in arrCNC)
                {
                    cncitemsSource.Add(CNCString.ToString());
                }
                CBCNCname.ItemsSource = cncitemsSource;
                //if (CBCNCname.ItemsSource != null)
                //{
                //    CBCNCname.SelectedIndex = 0;
                //}
                Shell.waitingForm.Close();
            }
            else
            {
                MessageBox.Show("雲端服務忙錄中，請稍候再試!");
                Shell.waitingForm.Close();
            }
        }

        #endregion

        private void CBCNCname_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /**要查詢kinfe的資訊需要給IT Team
             * <Knife_SearchInfo xsi:noNamespaceSchemaLocation="" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
             * <vMachine_Name>vMachine_44</vMachine_Name>
             * <CNC_Name>CNC_001</CNC_Name>
             * </Knife_SearchInfo>
             */
            if (CBvMachine.SelectedIndex > -1 && CBCNCname.SelectedIndex > -1)
            {
                XDocument knifeXML;
                knifeXML = new XDocument(
                  new XElement("Knife_SearchInfo",
                      new XElement("vMachine_Name", CBvMachine.SelectedItem.ToString()),
                      new XElement("CNC_Name", CBCNCname.SelectedItem.ToString()))
                );
                CallgetKinfeFunction(knifeXML.ToString());
            }
        }

        #region   呼叫Service Broker的Get_CNCInfo_Ontology(XMLFile)取得機台資訊的服務

        private void CallgetKinfeFunction(string knifeXML)
        {
            Shell.waitingForm.SettingMessage("連接服務中，請稍候");
            Shell.waitingForm.Show();
            OntologyService = new IPS.OntologyService.OntologyServiceClient();
            OntologyService.OntologySystemInterfaceAsync(StateManager.Username, "kinfe_Info", knifeXML);
            OntologyService.OntologySystemInterfaceCompleted += new EventHandler<IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs>(proxy_kinfe_InfoCompleted);


        }

        void proxy_kinfe_InfoCompleted(object sender, IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs e)
        {
            OntologyService.OntologySystemInterfaceCompleted -= new EventHandler<IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs>(proxy_kinfe_InfoCompleted);
            if (e.Error == null && e.Result != null)
            {
                //string[][] knifeArray = e.Result;
                List<PE2_ToolSelection> ToolSelectionList = new List<PE2_ToolSelection>();

                //切割字串，使用逗號分割
                string[] arreResult1 = e.Result.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string MState1String in arreResult1)
                {
                    string[] arreResult2 = MState1String.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    //MessageBox.Show(arreResult2[1] + "," + arreResult2[2] + "," + arreResult2[3] + "," + arreResult2[4]);
                    PE2_ToolSelection toolInstanceInsertValues = new PE2_ToolSelection();
                    toolInstanceInsertValues.Tool_Select = true;
                    toolInstanceInsertValues.A_kind_of_Tool = arreResult2[1].ToString();
                    toolInstanceInsertValues.Tool_of_Diameter = arreResult2[2].ToString();
                    toolInstanceInsertValues.NC_File_Name = arreResult2[3].ToString();
                    toolInstanceInsertValues.BLOB_Address = arreResult2[4].ToString();
                    toolInstanceInsertValues.NC_Descriptions = arreResult2[5].ToString();
                    ToolSelectionList.Add(toolInstanceInsertValues);
                }
                GDGetMachineStatus.ItemsSource = ToolSelectionList;

                /*
                for (int i = 0; i <= knifeArray.Length - 1; i++)
                {
                    PE2_ToolSelection toolInstanceInsertValues = new PE2_ToolSelection();
                    toolInstanceInsertValues.Tool_Select = true;
                    toolInstanceInsertValues.A_kind_of_Tool = knifeArray[i][0].ToString();
                    toolInstanceInsertValues.Tool_of_Diameter = knifeArray[i][1].ToString();
                    toolInstanceInsertValues.NC_File_Name = knifeArray[i][2].ToString();
                    toolInstanceInsertValues.BLOB_Address = knifeArray[i][3].ToString();
                    toolInstanceInsertValues.NC_Descriptions = knifeArray[i][4].ToString();
                    ToolSelectionList.Add(toolInstanceInsertValues);
                }
                GDGetMachineStatus.ItemsSource = ToolSelectionList;
                */

                OntologyService = new IPS.OntologyService.OntologyServiceClient();
                OntologyService.OntologySystemInterfaceAsync(StateManager.Username, "getScenario1Contents", null);
                OntologyService.OntologySystemInterfaceCompleted += new EventHandler<IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs>(proxy_getScenario1ContentsCompleted);

                //Shell.waitingForm.Close();
            }
            else
            {
                MessageBox.Show("雲端服務忙錄中，請稍候再試!");
                Shell.waitingForm.Close();
            }
        }

        void proxy_getScenario1ContentsCompleted(object sender, IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs e)
        {
            OntologyService.OntologySystemInterfaceCompleted += new EventHandler<IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs>(proxy_getScenario1ContentsCompleted);
            if (e.Error == null && e.Result != null)
            {
                string[] arrNCfileAndCNC = e.Result.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                showNCFile = arrNCfileAndCNC[0].ToString();
                showCNC = arrNCfileAndCNC[1].ToString();
                tb_NCfile.Text = showNCFile;
                tb_tool.Text = "一號刀";
                tb_machine.Text = showCNC;

                lbNCfile.Visibility = Visibility.Visible;
                lbtool.Visibility = Visibility.Visible;
                lbmachine.Visibility = Visibility.Visible;
                tb_NCfile.Visibility = Visibility.Visible;
                tb_tool.Visibility = Visibility.Visible;
                tb_machine.Visibility = Visibility.Visible;

                Shell.waitingForm.Close();
            }
            else
            {
                MessageBox.Show("雲端服務忙錄中，請稍候再試!");
                Shell.waitingForm.Close();
            }
        }
        #endregion

        private void btnNextToPEIP_Click(object sender, RoutedEventArgs e)
        {
            if (this.GDGetMachineStatus.SelectedIndex > -1)
            {

                MachineName_string = CBCNCname.SelectedItem.ToString();
                lstMachinetoSimulationItem = new List<PE2_ToolSelection>();
                if (this.GDGetMachineStatus.SelectedItem is PE2_ToolSelection)
                {
                    lstMachinetoSimulationItem.Add(((PE2_ToolSelection)this.GDGetMachineStatus.SelectedItem));
                }
                foreach (var gettoolSelectionList in lstMachinetoSimulationItem)
                {
                    gettoolSelectionList.Tool_Select = true;
                    KindofTool_string = gettoolSelectionList.A_kind_of_Tool;
                    ToolofDiameter_string = gettoolSelectionList.Tool_of_Diameter;
                    NCFileName_string = gettoolSelectionList.NC_File_Name;
                    Blob_string = gettoolSelectionList.BLOB_Address;
                }
                TB_NCFileName.Text = NCFileName_string;

                /**
                 * 呼叫VE圑隊的過切比對模組進行過切比對
                 * input:  NC Name,BloB位置
                 * output:  過切模擬展示圖
                 */

                overCuttingChecking(Blob_string, NCFileName_string,1);

                Control_TabItemChange(2);
            }
            else
            {
                MessageBox.Show("請選擇刀具!");
            }
        }

        private void btnNextToPEIP2_Click(object sender, RoutedEventArgs e)
        {
            if (this.GDGetMachineStatus.SelectedIndex > -1)
            {

                MachineName_string = CBCNCname.SelectedItem.ToString();
                lstMachinetoSimulationItem = new List<PE2_ToolSelection>();
                if (this.GDGetMachineStatus.SelectedItem is PE2_ToolSelection)
                {
                    lstMachinetoSimulationItem.Add(((PE2_ToolSelection)this.GDGetMachineStatus.SelectedItem));
                }
                foreach (var gettoolSelectionList in lstMachinetoSimulationItem)
                {
                    gettoolSelectionList.Tool_Select = true;
                    KindofTool_string = gettoolSelectionList.A_kind_of_Tool;
                    ToolofDiameter_string = gettoolSelectionList.Tool_of_Diameter;
                    NCFileName_string = gettoolSelectionList.NC_File_Name;
                    Blob_string = gettoolSelectionList.BLOB_Address;
                }
                TB_NCFileName.Text = NCFileName_string;

                /**
                 * 呼叫VE圑隊的過切比對模組進行過切比對
                 * input:  NC Name,BloB位置
                 * output:  過切模擬展示圖
                 */

                overCuttingChecking(Blob_string, NCFileName_string, 2);

                Control_TabItemChange(2);
            }
            else
            {
                MessageBox.Show("請選擇刀具!");
            }
        }


        private void overCuttingChecking(string postblob, string postNCFileName, int cuttingstep)
        {
            Shell.waitingForm.SettingMessage("執行過切比對模組中");
            Shell.waitingForm.Show();

            if (cuttingstep.Equals(1))
            {
                btnNextToPEIP.Visibility = System.Windows.Visibility.Collapsed;
                btnNextToPEIP2.Visibility = System.Windows.Visibility.Visible;
                Shell.waitingForm.SettingMessage("執行實體切削中");
                Shell.waitingForm.Show();
                OntologyService = new IPS.OntologyService.OntologyServiceClient();
                OntologyService.OntologySystemInterfaceAsync(StateManager.Username, "PE_Sim_Overcutting_Module", postblob + "@" + postNCFileName);
                OntologyService.OntologySystemInterfaceCompleted += new EventHandler<IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs>(proxy_PE_Sim_Overcutting_ModuleCompleted);
            }
            else if (cuttingstep.Equals(2))
            {
                Shell.waitingForm.SettingMessage("執行實體切削中");
                Shell.waitingForm.Show();
                OntologyService = new IPS.OntologyService.OntologyServiceClient();
                OntologyService.OntologySystemInterfaceAsync(StateManager.Username, "PE_Sim_Overcutting_Module2", postblob + "@" + postNCFileName);
                OntologyService.OntologySystemInterfaceCompleted += new EventHandler<IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs>(proxy_PE_Sim_Overcutting_ModuleCompleted);
            }
        }

        void proxy_PE_Sim_Overcutting_ModuleCompleted(object sender, IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs e)
        {
            OntologyService.OntologySystemInterfaceCompleted -= new EventHandler<IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs>(proxy_PE_Sim_Overcutting_ModuleCompleted);
            if (e.Error == null && e.Result != null)
            {
                //顯示切削過程展示圖的Function
                CallphotoShow(e.Result);
                //release_memery("lstMachinetoSimulationItem");

                Shell.waitingForm.Close();
            }
        }

        #endregion

        #region Step 2 to Step 3

        private void CallphotoShow(string imageXml)
        {
            try
            {
                string XMLStr = @"<sim_feature><graphNum>2</graphNum><graph line='50' time='1000'>http://140.116.86.249/image/cut_1.jpg</graph><graph line='150' time='2000'>http://140.116.86.249/image/cut_color_1.jpg</graph><graph line='160' time='2200'>http://140.116.86.249/image/cuttingWithNormal.jpg</graph><graph line='160' time='2200'>http://140.116.86.249/image/cuttingWithNormal.jpg</graph></sim_feature>";
                string XMLStr1 = @"<sim_feature><graphNum>2</graphNum><overcutting>true</overcutting><graph vmtType='DMU125PdB' line='50' time='1000'>http://140.116.86.249/image/cut_color_1.jpg</graph><graph vmtType='DMU125PdB' line='150' time='2000'>http://140.116.86.249/image/cut_color_1.jpg</graph><graph vmtType='DMU125PdB' line='160' time='2200'>http://140.116.86.249/image/cuttingWithNormal.jpg</graph><graph vmtType='DMU125PdB' line='160' time='2200'>http://140.116.86.249/image/cuttingWithNormal.jpg</graph><graphNum>2</graphNum><graph vmtType='DMU100PdB' line='50' time='1000'>http://140.116.86.249/image/cut_color_1.jpg</graph><graph vmtType='DMU160PdB' line='150' time='2000'>http://140.116.86.249/image/cut_color_1.jpg</graph><graph vmtType='DMU160PdB' line='160' time='2200'>http://140.116.86.249/image/cuttingWithNormal.jpg</graph><graph vmtType='DMU160PdB' line='160' time='2200'>http://140.116.86.249/image/cuttingWithNormal.jpg</graph></sim_feature>";

                /**
                 * GetAllImagesData(ve圑隊給的XML格式,腳本代號)
                 */
                //MessageBox.Show(imageXml);
                // Store Data in List Box.
                List<ImageEntity>  ListImagesObj = ImagesView.GetAllImagesData(imageXml, "2");

                if (ListImagesObj.Count > 0)
                {
                    foreach (ImageEntity ListImages in ListImagesObj)
                    {
                        if (ListImages.isOvercutting.Equals("false"))
                        {
                            //顯示上一步的按扭，否則上一步的按扭隱藏
                            ig_Undercutting.Visibility = Visibility.Visible;
                            lb_Undercutting.Visibility = Visibility.Visible;
                            btnLastToChangeTool.Visibility = Visibility.Visible;
                            lb_Undercutting.Content = "會過切!";
                            btnNextToRealMachining.Visibility = Visibility.Collapsed;
                        }
                        else if (ListImages.isOvercutting.Equals("true"))
                        {
                            lb_Undercutting.Content = "不會過切";
                            lb_Undercutting.Visibility = Visibility.Visible;
                            ig_Undercutting.Visibility = Visibility.Collapsed;
                            btnLastToChangeTool.Visibility = Visibility.Collapsed;
                            btnNextToRealMachining.Visibility = Visibility.Visible;
                        }
                    }

                    // Bind data in List Box
                    LsImageGallery.DataContext = ListImagesObj;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void btnNextToRealMachining_Click(object sender, RoutedEventArgs e)
        {
            CallShowCuttingDatagrid(NCFileName_string);
            Control_TabItemChange(3);
        }

        private void CallShowCuttingDatagrid(string ncfileName_string)
        {
            List<PE2_ToolSelectionToCutting> LsttoolselectionTocutting = new List<PE2_ToolSelectionToCutting>();
            PE2_ToolSelectionToCutting toolselectionTocutting = new PE2_ToolSelectionToCutting();
            toolselectionTocutting.CNC_Name = showCNC;
            toolselectionTocutting.ToolNo = "三號刀";
            toolselectionTocutting.ToolNC_Name = ncfileName_string;
            LsttoolselectionTocutting.Add(toolselectionTocutting);
            DGRealMachining.ItemsSource = LsttoolselectionTocutting;
        }

        #endregion

        #region Step 2 to Step 1  目的：過切比對模組顯示此刀具切削會過切時，可以讓現場製程工程師按上一步選擇另一把刀具進行過切比對

        private void btnLastToChangeTool_Click(object sender, RoutedEventArgs e)
        {
            Control_TabItemChange(1);
        }

        #endregion

        #region Step 3 to cutting

        private void btnStartCutting_Click(object sender, RoutedEventArgs e)
        {
            /**
             * 送出
             * <Machine_Scheduling_Log>
             *   <Company_name Name='Chevalier'>
             *    <vMachine Name='vMachine_235'>
             *      <Output_Machine  Name='CNC_001'  Type='QP-2440'>
             *        <NCFile_Blob_URL>https://portalvhdslbfp14hklh2x.blob.core.windows.net/ontology/PHONEBACK%20OUTSIDE.NC</NCFile_Blob_URL>
             *      </Output_Machine>
             *    </vMachine>
             *   </Company_name>
             * </Machine_Scheduling_Log>
             */

            XDocument realCuttingXML;
            realCuttingXML = new XDocument(
              new XElement("Machine_Scheduling_Log",
                  new XElement("Company_name", new XAttribute("Name", CBCompany.SelectedItem.ToString()),
                  new XElement("vMachine", new XAttribute("Name", CBvMachine.SelectedItem.ToString()),
                  new XElement("Output_Machine", new XAttribute("Name", CBCNCname.SelectedItem.ToString()), new XAttribute("Type", "QP-2440"),
                  new XElement("NCFile_Blob_URL", Blob_string))))
              )
            );
            realCutting(realCuttingXML.ToString());
        }

        private void realCutting(string postxml)
        {
            Shell.waitingForm.SettingMessage("連接服務中，請稍候");
            Shell.waitingForm.Show();
            OntologyService = new IPS.OntologyService.OntologyServiceClient();
            OntologyService.OntologySystemInterfaceAsync(StateManager.Username, "PE_SimOvercutting_ActualProcessingCNC", postxml);
            OntologyService.OntologySystemInterfaceCompleted += new EventHandler<IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs>(proxy_PE_SimOvercutting_ActualProcessingCNCeCompleted);
        }

        void proxy_PE_SimOvercutting_ActualProcessingCNCeCompleted(object sender, IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs e)
        {
            OntologyService.OntologySystemInterfaceCompleted -= new EventHandler<IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs>(proxy_PE_SimOvercutting_ActualProcessingCNCeCompleted);
            if (e.Error == null && e.Result != null)
            {
                //顯示切削過程展示圖的Function
                if (e.Result == "true")
                {
                    MessageBox.Show("已將選擇的機台機台之NC檔傳送到雲端資料庫中!");
                }
                else
                {
                    MessageBox.Show("上傳失敗，請重新再上傳一次");

                }
                Shell.waitingForm.Close();
            }
        }

        #endregion

    }
}
