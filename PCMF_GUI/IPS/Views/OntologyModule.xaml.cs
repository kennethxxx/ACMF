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
using IPS.MachineInfomation;
using IPS.MachineInfomation2;
using System.Xml.Linq;
using OMC.Comm;
using System.Xml;
using System.IO;
using IPS.OntologyService;
using System.Collections;
using IPS.UploadFileService;
using OMC.UploadFileService;
using System.Collections.ObjectModel;
using IPS.Common;

namespace IPS.Views
{
    public partial class OntologyModule : Page
    {
        string CompanyName, ServiceBrokerID, MachineName_string, MachineXMLInfo, returnToSilmulation,
               imageStringcombine;  //記錄公司名稱，ServiceBrokerID。之後送出機台要確認是哪個公司的哪個ServiceBroker。
        IPS.OntologyService.OntologyServiceClient OntologyService;   //宣告Ontology的服務
        IPS.UploadFileService.Service1Client UploadService = null;  //呼叫上傳的服務
        //IPS.MachineInfomation.Service1Client GetMachineInfoService;
        OpenFileDialog dlg_CLorNCFiles = new OpenFileDialog();
        OpenFileDialog dlg_Workpiece = new OpenFileDialog();
        List<PE_SelectMachineToSimulation> SelectMachineToSimulationSource;
        List<PE_SelectMachineToSimulation> lstMachinetoSimulationItem;
        List<PE_SelectMachineToSimulation> SelectMachineToSimulationSource3axis;
        List<PE_SelectMachineToSimulation> SelectMachineToSimulationSource5axis;

        List<ImageEntity> ListImagesObj;
        XDocument CompanyXml, ServiceBrokerXml, MachineInfoXml;

        public OntologyModule()
        {
            InitializeComponent();
            Inni();
        }

        #region  網頁一進去立即載入的Function

        private void Inni()
        {
            //一開始顯示的提示文字
            LB_Info1.Text = "此步驟將取得特定公司裡的機台狀態。" + "\n" + "步驟一:選擇公司。" + "\n" + "步驟二:選擇Service Broker，並且得到所有的加工機台狀態。";
            LB_Info2.Text = "此步驟將由製程工程師輸入符合加工的需求。" + "\n" + "步驟零:選擇已建立完成的工具機知識庫。" + "\n" + "步驟一:上傳CL檔案或是NC檔案。" + "\n" + "步驟二:輸入製程工程師所要求的加工機台的加工精度。" + "\n" + "步驟三:選擇要推論的規則。";
            LB_Info3.Text = "此步驟由工具機知識庫所推薦的偏好機台，讓製成工程師選擇偏好機台做切削模擬。 (下表出現的工具機是經由工具機知識庫中，推薦出可加工的加工機台。)" + "\n" + "步驟一:選擇加工機台按下下一步進入執行後處理與加工時間估算模組，並將選擇的加工機台送到切削模擬模組。";
            LB_Info4.Text = "此步驟在背後執行加工時間估算模組，取得加工機台加工的時間。";
            LB_Info5.Text = "此步驟能夠看到加工機台切削過程的模擬展示圖，每張圖顯示切削過程中的NC碼行數以及加工時間點。" + "\n" + "步驟一:請按下一步進入確認偏好之實際加工機台與送出加工請求。";
            LB_Info6.Text = "此步驟將製程工程師所選擇的偏好加工機台做實際加工。" + "\n" + "步驟一:選擇要做實際加工的機台，並且送到實際加工機台做切削。";

            #region  呼叫Service Broker取得公司資訊的服務
            CallCompanyService();
            #endregion

            //new一個上傳的服務
            UploadService = new UploadFileService.Service1Client();
        }

        #endregion

        #region  呼叫Service Broker的GetCompanyInfo()取得公司資訊的服務

        private void CallCompanyService()
        {
            Shell.waitingForm.SettingMessage("連接服務中，請稍候");
            Shell.waitingForm.Show();
            OntologyService = new OntologyServiceClient();
            OntologyService.OntologySystemInterfaceAsync(StateManager.Username, "GetCompanyInfo", null);
            OntologyService.OntologySystemInterfaceCompleted += new EventHandler<IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs>(proxy_GetCompanyInfoCompleted);
        }

        void proxy_GetCompanyInfoCompleted(object sender, IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs e)
        {
            OntologyService.OntologySystemInterfaceCompleted -= new EventHandler<IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs>(proxy_GetCompanyInfoCompleted);
            if (e.Error == null && e.Result != null)
            {
                //載入資料
                //核心尚未寫出來時用MessageBox印出結果，已寫出來把ResolveXMLFormat方法註解移除掉
                //MessageBox.Show(e.Result);
                ResolveXMLFormat(1, e.Result);
                if (CBCompany.ItemsSource != null)
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

        #region TabControl切換的狀態管控

        private void Control_TabItemChange(int Step)
        {
            switch (Step)
            {
                case 2:
                    //TBGetMachineStatus.IsEnabled = false;
                    TBPEInputParameters.IsEnabled = true;
                    TBPEInputParameters.IsSelected = true;
                    break;
                case 3:
                    //TBPEInputParameters.IsEnabled = false;
                    TBSelectFavoriteMachineTools.IsEnabled = true;
                    TBSelectFavoriteMachineTools.IsSelected = true;
                    break;
                case 4:
                    //TBSelectFavoriteMachineTools.IsEnabled = false;
                    TBPicofCutSimulationProcess.IsEnabled = true;
                    TBPicofCutSimulationProcess.IsSelected = true;
                    break;
                case 5:
                    //TBPicofCutSimulationProcess.IsEnabled = false;
                    R_PT_APTEM.IsEnabled = true;
                    R_PT_APTEM.IsSelected = true;
                    break;
                case 6:
                    //R_PT_APTEM.IsEnabled = false;
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
                    foreach (var CompanyInfo in CompanyXml.Elements("Machine_Scheduling_Log").Descendants("Company_name"))
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
                    GDGetMachineStatus.ItemsSource = null;
                    List<PE_GetStatusofMachines> CNCsource = new List<PE_GetStatusofMachines>();
                    ///取出多層中的Attribute所使用的方式
                    System.IO.MemoryStream _memorystream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xmlStr));
                    MachineInfoXml = XDocument.Load(_memorystream);

                    /**
                     * MachineInfoXml.Elements("Machine_Scheduling_Log").Elements("vMachine").Descendants("Output_Machine")的
                     * Elements與Element差很多，若使用Element XML第二層有兩個以上會解析不到
                     * 若使用Elements就可以讀到
                     */

                    foreach (var MachineInfo2 in MachineInfoXml.Elements("Machine_Scheduling_Log").Elements("vMachine").Descendants("Output_Machine"))
                    {
                        CNCsource.Add(new PE_GetStatusofMachines(MachineInfo2.Parent.Attribute("Name").Value, MachineInfo2.Attribute("Name").Value, MachineInfo2.Attribute("Type").Value, MachineInfo2.Attribute("State").Value));
                    }
                    GDGetMachineStatus.ItemsSource = CNCsource;
                    break;
                case 4:
                    List<PE_SelectRules> Rule_source = new List<PE_SelectRules>();
                    using (XmlReader xReader = XmlReader.Create(new StringReader(xmlStr)))
                    {
                        while (xReader.Read())
                        {
                            try
                            {
                                xReader.ReadToFollowing("idrules");
                                string rules_id = xReader.ReadElementContentAsString();
                                xReader.ReadToNextSibling("rules_name");
                                string rules_name = xReader.ReadElementContentAsString();
                                xReader.ReadToNextSibling("rules_description");
                                string rule_Description = xReader.ReadElementContentAsString();
                                xReader.ReadToNextSibling("rules");
                                string ruleswrl = xReader.ReadElementContentAsString();

                                //解析一行資料庫XML的資料
                                PE_SelectRules RuleInfo = new PE_SelectRules();
                                if (rules_id.Equals("4"))
                                {
                                    RuleInfo.Rule_Select = false;
                                }
                                else
                                {
                                    RuleInfo.Rule_Select = true;
                                }
                                RuleInfo.TB_Rule_ID = rules_id;
                                RuleInfo.TB_Rule_Name = rules_name;
                                RuleInfo.TB_Rule_Description = rule_Description;
                                RuleInfo.TB_Rule_SWRL = ruleswrl;
                                Rule_source.Add(RuleInfo);
                            }
                            catch (System.Exception ex)
                            { }
                        }
                    }
                    DGSelectRule1.ItemsSource = Rule_source;
                    Shell.waitingForm.Close();
                    break;
            }
        }

        #endregion

        #region   Step:1 Select Company,Machine information (Read XML Format)

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
            Shell.waitingForm.SettingMessage("連結服務中，請稍候");
            Shell.waitingForm.Show();
            OntologyService = new OntologyServiceClient();
            OntologyService.OntologySystemInterfaceAsync(StateManager.Username, "GetServiceBrokerInfo", XMLstring_CompanyName);
            OntologyService.OntologySystemInterfaceCompleted += new EventHandler<IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs>(proxy_GetServiceBrokerInfoCompleted);
        }

        void proxy_GetServiceBrokerInfoCompleted(object sender, IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs e)
        {
            OntologyService.OntologySystemInterfaceCompleted -= new EventHandler<IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs>(proxy_GetServiceBrokerInfoCompleted);
            if (e.Error == null && e.Result != null)
            {
                //載入資料
                //MessageBox.Show(e.Result);
                ResolveXMLFormat(2, e.Result);
                //if (CBServiceBroker.ItemsSource != null )
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
                ServiceBrokerID = CBServiceBroker.SelectedItem.ToString().Trim();
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

        #region   呼叫Service Broker的GetvMachineCNCInfo(XMLFile)取得機台資訊的服務

        private void CallMachineCNCInfoService(string XMLstring_ServiceBroker)
        {
            Shell.waitingForm.SettingMessage("連接服務中，請稍候");
            Shell.waitingForm.Show();
            OntologyService = new OntologyServiceClient();
            OntologyService.OntologySystemInterfaceAsync(StateManager.Username, "GetvMachineCNCInfo_Ontology", XMLstring_ServiceBroker);
            OntologyService.OntologySystemInterfaceCompleted += new EventHandler<IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs>(proxy_GetvMachineCNCInfo_OntologyCompleted);
        }

        void proxy_GetvMachineCNCInfo_OntologyCompleted(object sender, IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs e)
        {
            OntologyService.OntologySystemInterfaceCompleted -= new EventHandler<IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs>(proxy_GetvMachineCNCInfo_OntologyCompleted);
            if (e.Error == null && e.Result != null)
            {
                MachineXMLInfo = e.Result;
                //載入資料
                //MessageBox.Show(e.Result);
                ResolveXMLFormat(3, e.Result);
                Shell.waitingForm.Close();
            }
            else
            {
                MessageBox.Show("雲端服務忙錄中，請稍候再試!");
                Shell.waitingForm.Close();
            }
        }
        #endregion

        #endregion

        #region  Step:1 to Step:2

        private void btnNextToPEIP_Click(object sender, RoutedEventArgs e)
        {

            ///
            /// 顯示Progress Bar as 呼叫it team服務 Start
            /// 
            Shell.waitingForm.SettingMessage("連接服務中，請稍候");
            Shell.waitingForm.Show();
            Control_TabItemChange(2);
            CallLoadOntologyDB();
            ///
            /// 顯示Progress Bar as 呼叫it team服務 End
            /// 
        }

        private void CallLoadOntologyDB()
        {
            //new OntologyService
            //將資料庫的OntologyDB取出
            OntologyService = new OntologyServiceClient();
            OntologyService.OntologySystemInterfaceAsync(StateManager.Username, "ShowAllOntology", null);
            OntologyService.OntologySystemInterfaceCompleted += new EventHandler<IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs>(proxy_ShowAllOntologyCompleted);
        }

        void proxy_ShowAllOntologyCompleted(object sender, IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs e)
        {
            OntologyService.OntologySystemInterfaceCompleted -= new EventHandler<IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs>(proxy_ShowAllOntologyCompleted);
            try
            {
                if (e.Error == null && e.Result != null)
                {

                    ObservableCollection<string> ontologyDBitemsSource = new ObservableCollection<string>();
                    //切割字串，使用逗號分割
                    string[] arrOntology = e.Result.Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string OntString in arrOntology)
                    {
                        ontologyDBitemsSource.Add(OntString.ToString());
                    }
                    CBOntolgoy.ItemsSource = ontologyDBitemsSource;
                    Shell.waitingForm.Close();
                }
                else
                {
                    MessageBox.Show("雲端服務忙錄中，請稍候再試!");
                    Shell.waitingForm.Close();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        #region  Step:2 to Step:3

        private void btnUpload_CLorNCFile_Click(object sender, RoutedEventArgs e)
        {

            dlg_CLorNCFiles.Multiselect = false;
            //這邊可以設定過副檔名過濾器
            dlg_CLorNCFiles.Filter = "NC File (*.nc) | *.nc| CL File (*.cl)|*.cl |All (*.*) | *.*";

            bool? retval = dlg_CLorNCFiles.ShowDialog();
            if (retval != null && retval == true && CBOntolgoy != null)
            {
                TBUpload_CLorNCFile.Text = dlg_CLorNCFiles.File.Name;
            }
        }
        #region  上傳CL檔與NC檔

        private string UploadFile1(string fileName, Stream strm, string filename)
        {

            byte[] Buffer = new byte[strm.Length];
            strm.Read(Buffer, 0, (int)strm.Length);
            strm.Dispose();
            strm.Close();

            UploadFile file = new UploadFile();
            file.FileName = fileName;
            file.File = Buffer;
            string newfilename = filename;
            if (newfilename != "")
            {
                UploadService.SaveFile2Async(file, "CNC");
                UploadService.SaveFile2Completed += new EventHandler<IPS.UploadFileService.SaveFile2CompletedEventArgs>(proxy_SaveFile2Completed);

                return "上傳成功!";

            }
            else
            {
                MessageBox.Show("請輸入輸入檔名");
            }
            return "";
        }

        void proxy_SaveFile2Completed(object sender, IPS.UploadFileService.SaveFile2CompletedEventArgs e)
        {
            UploadService.SaveFile2Completed -= new EventHandler<IPS.UploadFileService.SaveFile2CompletedEventArgs>(proxy_SaveFile2Completed);
                if (e.Error == null & e.Result != null) { }
                    // MessageBox.Show(e.Result);
        }

        #endregion

        private void CBOntolgoy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CallSelectRuleService();
        }

        private void CallSelectRuleService()
        {
            if (CBOntolgoy.SelectedItem.ToString() != null)
            {
                Shell.waitingForm.SettingMessage("連接服務中，請稍候");
                Shell.waitingForm.Show();
                //new OntologyService
                //將資料庫的Rule取出
                OntologyService = new OntologyServiceClient();
                OntologyService.OntologySystemInterfaceAsync(StateManager.Username, "ShowDBTable", "swrl_rule@rule_table");
                OntologyService.OntologySystemInterfaceCompleted += new EventHandler<IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs>(proxy_ShowDBTableCompleted);
            }
        }

        void proxy_ShowDBTableCompleted(object sender, IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs e)
        {
            OntologyService.OntologySystemInterfaceCompleted -= new EventHandler<IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs>(proxy_ShowDBTableCompleted);
            if (e.Error == null & e.Result != null)
            {
                string rule_alldata = e.Result;
                ResolveXMLFormat(4, rule_alldata);
            }
            else
            {
                MessageBox.Show("雲端服務忙錄中，請稍候再試!");
                Shell.waitingForm.Close();
            }
        }

        private void btnNextToSFavoriteMachineTools_Click(object sender, RoutedEventArgs e)
        {
            if (CBOntolgoy.SelectedIndex > -1 & TBUpload_CLorNCFile.Text != "" & TBProcessingAccuracy.Text != "")
            {
                Control_TabItemChange(3);
                //上傳CL檔或NC檔
                UploadFile1(dlg_CLorNCFiles.File.Name, dlg_CLorNCFiles.File.OpenRead(), TBUpload_CLorNCFile.Text);
                /*
                 * 上傳加工件初胚檔
                 * 可人學長說可以不需要加工初胚檔了
                 * 註解掉 UploadFile1(dlg_Workpiece.File.Name, dlg_Workpiece.File.OpenRead(), TBUpload_Workpiece.Text);
                 */
                //UploadFile1(dlg_Workpiece.File.Name, dlg_Workpiece.File.OpenRead(), TBUpload_Workpiece.Text);
                ///抓取製程選擇的Rule轉換成XML資料
                string RuleXMLString = CatchSelectRuleToXMLFunction();
                callSPARQLFunction(RuleXMLString);
            }
            else
            {
                MessageBox.Show("Null Parameter!");
            }
        }

        private void callSPARQLFunction(string RuleXMLString)
        {
            Shell.waitingForm.SettingMessage("推論中，請稍候");
            Shell.waitingForm.Show();
           
            //new OntologyService
            //將精度條件傳到本體核心做SPARQL篩選
            OntologyService = new OntologyServiceClient();
            OntologyService.OntologySystemInterfaceAsync(StateManager.Username, "setUserParameters", CBOntolgoy.SelectedItem.ToString() + "@" + TBProcessingAccuracy.Text + "@" + RuleXMLString + "@" + MachineXMLInfo + "@" + TBUpload_CLorNCFile.Text);
            OntologyService.OntologySystemInterfaceCompleted += new EventHandler<IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs>(proxy_setUserParametersCompleted);
        }

        void proxy_setUserParametersCompleted(object sender, IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs e)
        {
            if (e.Error == null & e.Result != null)
            {
                OntologyService.OntologySystemInterfaceCompleted -= new EventHandler<IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs>(proxy_setUserParametersCompleted);
            
                SelectMachineToSimulationSource = new List<PE_SelectMachineToSimulation>();   //尚未分軸數的陣列，儲存所有推薦的機台ListArray
                SelectMachineToSimulationSource3axis = new List<PE_SelectMachineToSimulation>();   //3軸數的陣列，儲存所有3軸推薦的機台ListArray
                SelectMachineToSimulationSource5axis = new List<PE_SelectMachineToSimulation>();   //3軸數的陣列，儲存所有3軸推薦的機台ListArray

                //切割字串，使用逗號分割
                string[] arreResult1 = e.Result.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string MState1String in arreResult1)
                {

                    string[] arreResult2 = MState1String.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    //MessageBox.Show(arreResult2[1] + "," + arreResult2[2] + "," + arreResult2[3] + "," + arreResult2[4] + "," + arreResult2[5] + "," + arreResult2[6].ToString());
                    if (arreResult2[2].Equals("axis3"))
                    {
                        if (arreResult2[6].ToString() == "START")
                        {
                            PE_SelectMachineToSimulation SCNCInfo3axis = new PE_SelectMachineToSimulation();
                            SCNCInfo3axis.Machine_Select = true;
                            SCNCInfo3axis.Machine_Type = arreResult2[1].ToString();
                            SCNCInfo3axis.Machine_Name = arreResult2[1].ToString();
                            SCNCInfo3axis.vMachine = arreResult2[3].ToString();
                            SCNCInfo3axis.Machine_CNC = arreResult2[5].ToString();
                            SCNCInfo3axis.Machine_Number_of_axes = "三軸";
                            SCNCInfo3axis.Machine_Status = arreResult2[6].ToString();
                            SelectMachineToSimulationSource3axis.Add(SCNCInfo3axis);
                        }
                        else
                        {
                            PE_SelectMachineToSimulation SCNCInfo3axis = new PE_SelectMachineToSimulation();
                            SCNCInfo3axis.Machine_Select = false;
                            SCNCInfo3axis.Machine_Type = arreResult2[1].ToString();
                            SCNCInfo3axis.Machine_Name = arreResult2[1].ToString();
                            SCNCInfo3axis.vMachine = arreResult2[3].ToString();
                            SCNCInfo3axis.Machine_CNC = arreResult2[5].ToString();
                            SCNCInfo3axis.Machine_Number_of_axes = "三軸";
                            SCNCInfo3axis.Machine_Status = arreResult2[6].ToString();
                            SelectMachineToSimulationSource3axis.Add(SCNCInfo3axis);
                        }
                    }
                    else
                    {
                        PE_SelectMachineToSimulation SCNCInfo5axis = new PE_SelectMachineToSimulation();
                        SCNCInfo5axis.Machine_Select = false;
                        SCNCInfo5axis.Machine_Type = arreResult2[1].ToString();
                        SCNCInfo5axis.Machine_Name = arreResult2[1].ToString();
                        SCNCInfo5axis.vMachine = arreResult2[3].ToString();
                        SCNCInfo5axis.Machine_CNC = arreResult2[5].ToString();
                        SCNCInfo5axis.Machine_Number_of_axes = "五軸";
                        SCNCInfo5axis.Machine_Status = arreResult2[6].ToString();
                        SelectMachineToSimulationSource5axis.Add(SCNCInfo5axis);
                    }
                }
                /*
                for (int i = 0; i < e.Result.Length; i++)
                {
                    /// =============================================================================================================
                    /// array[0]= MachineType, array[1]= vMachineName, array[2]= CNCID, array[3]= 機台狀態, array[4]= 工具機的軸數
                    /// =============================================================================================================
                    string[] bbb = e.Result[i].array.ToArray();
                    //MessageBox.Show(bbb[0].ToString() + "," + bbb[1].ToString() + "," + bbb[2].ToString() + "," + bbb[3].ToString() + "," + bbb[4].ToString());
                    PE_SelectMachineToSimulation SCNCInfo = new PE_SelectMachineToSimulation();
                    SCNCInfo.Machine_Select = true;
                    SCNCInfo.Machine_Type = bbb[0].ToString();
                    SCNCInfo.Machine_Name = bbb[0].ToString();
                    SCNCInfo.vMachine = bbb[1].ToString();
                    SCNCInfo.Machine_CNC = bbb[2].ToString();
                    SCNCInfo.Machine_Status = bbb[3].ToString();
                    SCNCInfo.Machine_Number_of_axes = bbb[4].ToString();
                    SelectMachineToSimulationSource.Add(SCNCInfo);
                }
                */
                ///*************************20130125 comment**********************************************************
                ///              在這裡寫判斷五軸還是三軸，分別顯示在不同的grid上
                ///              三軸的datagrid name is DGSelectMachineToCuttingSimulation
                ///              五軸的datagrid name is DGSelectMachineToCuttingSimulation5axis
                ///              _Selmachine.Machine_Number_of_axes.Equals("3")  ← 需跟芝吟討論，傳回來的值是什麼
                ///*************************20130125 comment**********************************************************
                ///               Finish 
                /// ↓   ↓   ↓   ↓


                /*
                foreach (PE_SelectMachineToSimulation _Selmachine in SelectMachineToSimulationSource)
                {

                    if (_Selmachine.Machine_Number_of_axes.Equals("axis3"))
                    {
                        if (_Selmachine.Machine_Status == "START")
                        {
                            PE_SelectMachineToSimulation SCNCInfo3axis = new PE_SelectMachineToSimulation();
                            SCNCInfo3axis.Machine_Select = true;
                            SCNCInfo3axis.Machine_Type = _Selmachine.Machine_Type;
                            SCNCInfo3axis.Machine_Name = _Selmachine.Machine_Name;
                            SCNCInfo3axis.vMachine = _Selmachine.vMachine;
                            SCNCInfo3axis.Machine_CNC = _Selmachine.Machine_CNC;
                            SCNCInfo3axis.Machine_Number_of_axes = "三軸";
                            SCNCInfo3axis.Machine_Status = _Selmachine.Machine_Status;
                            SelectMachineToSimulationSource3axis.Add(SCNCInfo3axis);
                        }
                        else
                        {
                            PE_SelectMachineToSimulation SCNCInfo3axis = new PE_SelectMachineToSimulation();
                            SCNCInfo3axis.Machine_Select = false;
                            SCNCInfo3axis.Machine_Type = _Selmachine.Machine_Type;
                            SCNCInfo3axis.Machine_Name = _Selmachine.Machine_Name;
                            SCNCInfo3axis.vMachine = _Selmachine.vMachine;
                            SCNCInfo3axis.Machine_CNC = _Selmachine.Machine_CNC;
                            SCNCInfo3axis.Machine_Number_of_axes = "三軸";
                            SCNCInfo3axis.Machine_Status = _Selmachine.Machine_Status;
                            SelectMachineToSimulationSource3axis.Add(SCNCInfo3axis);
                        }
                    }
                    else
                    {
                        PE_SelectMachineToSimulation SCNCInfo5axis = new PE_SelectMachineToSimulation();
                        SCNCInfo5axis.Machine_Select = false;
                        SCNCInfo5axis.Machine_Type = _Selmachine.Machine_Type;
                        SCNCInfo5axis.Machine_Name = _Selmachine.Machine_Name;
                        SCNCInfo5axis.vMachine = _Selmachine.vMachine;
                        SCNCInfo5axis.Machine_CNC = _Selmachine.Machine_CNC;
                        SCNCInfo5axis.Machine_Number_of_axes = "五軸";
                        SCNCInfo5axis.Machine_Status = _Selmachine.Machine_Status;
                        SelectMachineToSimulationSource5axis.Add(SCNCInfo5axis);
                    }
                }
                 * */
                DGSelectMachineToCuttingSimulation3axis.ItemsSource = SelectMachineToSimulationSource3axis;   //如果是軸數是3軸在DGSelectMachineToCuttingSimulation3axis 的datagrid顯示
                DGSelectMachineToCuttingSimulation5axis.ItemsSource = SelectMachineToSimulationSource5axis;   //如果是軸數是5軸在DGSelectMachineToCuttingSimulation5axis 的datagrid顯示
                Shell.waitingForm.Close();
            }
            else
            {
                MessageBox.Show("雲端服務忙錄中，請稍候再試!");
                Shell.waitingForm.Close();
            }
        }

        private string CatchSelectRuleToXMLFunction()
        {
            string All_String = "";
            XDocument srcRuleC;
            List<string> RuleNameLists = new List<string>();
            List<string> RuleDescriptionLists = new List<string>();
            List<string> RuleSWRLLists = new List<string>();

            List<PE_SelectRules> RuleList = new List<PE_SelectRules>();
            IEnumerator ppEnum = DGSelectRule1.ItemsSource.GetEnumerator();
            while (ppEnum.MoveNext())
            {
                RuleList.Add((PE_SelectRules)ppEnum.Current);
            }

            foreach (PE_SelectRules a in RuleList)
            {
                if (a.Rule_Select.Equals(true))
                {
                    RuleNameLists.Add(a.TB_Rule_Name);
                    RuleDescriptionLists.Add(a.TB_Rule_Description);
                    RuleSWRLLists.Add(a.TB_Rule_SWRL);
                    srcRuleC = new XDocument(
                      new XElement(
                          new XElement("PERule", a.TB_Rule_ID)
                       )
                    );
                    All_String += srcRuleC.ToString();
                }
            }
            return All_String;
        }

        #endregion

        #region  Step:3 to Step:4

        private void btnNextToPicoCutSimulationProcess_Click(object sender, RoutedEventArgs e)
        {
            Control_TabItemChange(4);
            //callVE圑隊提供的服務(後處理模組)
            CallVEAfterTreatment1();

            /* 因為目前ve的後處理 CallVEAfterTreatment1()沒有用所以直接跳過執行時間估算模組 */
            //CallVEAfterTreatment2();
        }

        private void CallVEAfterTreatment1()
        {
            Shell.waitingForm.SettingMessage("執行加工模擬中");
            Shell.waitingForm.Show();
            OntologyService = new OntologyServiceClient();
            OntologyService.OntologySystemInterfaceAsync(StateManager.Username, "PE_setPostprocessor", "aaa@bbb");
            OntologyService.OntologySystemInterfaceCompleted += new EventHandler<IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs>(proxy_PE_setPostprocessorCompleted);
        }

        private void CallVEAfterTreatment2()
        {
            Shell.waitingForm.SettingMessage("執行碰撞檢測中");
            Shell.waitingForm.Show();
            OntologyService = new OntologyServiceClient();
            OntologyService.OntologySystemInterfaceAsync(StateManager.Username, "PE_setPe_machiningTime", "aaa@bbb");
            OntologyService.OntologySystemInterfaceCompleted += new EventHandler<IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs>(proxy_PE_setPe_machiningTimeCompleted);
        }

        private void CallVEAfterTreatment3(string SelectMachineName)
        {
            Shell.waitingForm.SettingMessage("執行實體切削中");
            Shell.waitingForm.Show();
            //new OntologyService
            //將精度條件傳到本體核心做SPARQL篩選
            OntologyService = new OntologyServiceClient();
            OntologyService.OntologySystemInterfaceAsync(StateManager.Username, "PE_setPreferencesCNC", SelectMachineName + "@" + TBUpload_CLorNCFile.Text);
            OntologyService.OntologySystemInterfaceCompleted += new EventHandler<IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs>(proxy_PE_setPreferencesCNCCompleted);
            Control_TabItemChange(5);
        }

        void proxy_PE_setPostprocessorCompleted(object sender, IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs e)
        {
            OntologyService.OntologySystemInterfaceCompleted -= new EventHandler<IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs>(proxy_PE_setPostprocessorCompleted);
            if (e.Error == null & e.Result == "True")
            {
                Shell.waitingForm.SettingMessage("執行碰撞檢測中");
                Shell.waitingForm.Show();
                //callVE圑隊提供的服務
                CallVEAfterTreatment2();
            }
            else
            {
                MessageBox.Show("雲端服務忙錄中，請稍候再試!");
                Shell.waitingForm.Close();
            }
        }

        void proxy_PE_setPe_machiningTimeCompleted(object sender, IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs e)
        {
            OntologyService.OntologySystemInterfaceCompleted -= new EventHandler<IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs>(proxy_PE_setPe_machiningTimeCompleted);
            ///**********************20130125******Finish***not compile*****
            ///          這裡要修改，抓取DGSelectMachineToCuttingSimulation3axis datagrid的資料和 DGSelectMachineToCuttingSimulation5axis datagrid的資料
            ///          因為這裡只有抓一個grid送出，目前還要想怎麼抓出所有選擇的3軸或5軸的機台送出
            ///          還有可能會因為要enlabel忙碌狀態的機台不能給選。
            ///**********************20130125****Finish***not compile*******



            if (e.Error == null & e.Result == "True")
            {
                MachineName_string = "";
                lstMachinetoSimulationItem = new List<PE_SelectMachineToSimulation>();
                IEnumerator ppEnum3axis = DGSelectMachineToCuttingSimulation3axis.ItemsSource.GetEnumerator();
                IEnumerator ppEnum5axis = DGSelectMachineToCuttingSimulation5axis.ItemsSource.GetEnumerator();

                while (ppEnum3axis.MoveNext())
                {
                    lstMachinetoSimulationItem.Add((PE_SelectMachineToSimulation)ppEnum3axis.Current);
                }

                while (ppEnum5axis.MoveNext())
                {
                    lstMachinetoSimulationItem.Add((PE_SelectMachineToSimulation)ppEnum5axis.Current);
                }

                foreach (PE_SelectMachineToSimulation i in lstMachinetoSimulationItem)
                {
                    if (i.Machine_Select.Equals(true))
                    {
                        
                        //foreach (PE_SelectMachineToSimulation ii in SelectMachineToSimulationSource)
                        //{
                        //    MessageBox.Show(ii.Machine_Name+"會有值嗎");
                         //   if (i.Machine_Name.Equals(ii.Machine_Name))
                         //   {
                         //       MessageBox.Show(i.Machine_Name + "為什麼沒進來…");
                                MachineName_string += i.Machine_Name + ",";
                                
                         //   }
                        //}
                    }
                }

                //callVE圑隊提供的服務
                CallVEAfterTreatment3(MachineName_string);
            }
            else
            {
                MessageBox.Show("雲端服務忙錄中，請稍候再試!");
                Shell.waitingForm.Close();
            }
        }

        void proxy_PE_setPreferencesCNCCompleted(object sender, IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs e)
        {
            OntologyService.OntologySystemInterfaceCompleted -= new EventHandler<IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs>(proxy_PE_setPreferencesCNCCompleted);
            if (e.Error == null && e.Result != null)
            {
                Shell.waitingForm.Close();
                //顯示切削過程展示圖
                CallStep4toStep5Function(e.Result);
            }
            else
            {
                MessageBox.Show("雲端服務忙錄中，請稍候再試!");
                Shell.waitingForm.Close();
            }
        }

        #endregion

        #region  Step:4 to Step:5

        private void CallStep4toStep5Function(string ImageString)
        {
            try
            {
                ObservableCollection<string> imgMachineitemsSource = new ObservableCollection<string>();

                //切割字串，使用逗號分割
                string[] arrMachineName = MachineName_string.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string MCString in arrMachineName)
                {
                    imgMachineitemsSource.Add(MCString);
                }
                CB_SelectDisplayMachinePhoto.ItemsSource = imgMachineitemsSource;

                BindImages(ImageString); // Call Bind Image Function
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Bind Images in List Box
        /// </summary>
        private void BindImages(string ImageString)
        {
            try
            {
                string XMLStr = @"<sim_feature><graphNum>2</graphNum><graph line='50' time='1000'>http://140.116.86.249/image/cut_1.jpg</graph><graph line='150' time='2000'>http://140.116.86.249/image/cut_color_1.jpg</graph><graph line='160' time='2200'>http://140.116.86.249/image/cuttingWithNormal.jpg</graph><graph line='160' time='2200'>http://140.116.86.249/image/cuttingWithNormal.jpg</graph></sim_feature>";
                string XMLStr1 = @"<sim_feature><graphNum>2</graphNum><graph CNCName='DMU125PdB' line='50' time='1000'>http://140.116.86.249/image/cut_color_1.jpg</graph><graph CNCName='DMU125PdB' line='150' time='2000'>http://140.116.86.249/image/cut_color_1.jpg</graph><graph CNCName='DMU125PdB' line='160' time='2200'>http://140.116.86.249/image/cuttingWithNormal.jpg</graph><graph CNCName='DMU125PdB' line='160' time='2200'>http://140.116.86.249/image/cuttingWithNormal.jpg</graph><graphNum>2</graphNum><graph CNCName='DMU100PdB' line='50' time='1000'>http://140.116.86.249/image/cut_color_1.jpg</graph><graph CNCName='DMU160PdB' line='150' time='2000'>http://140.116.86.249/image/cut_color_1.jpg</graph><graph CNCName='DMU160PdB' line='160' time='2200'>http://140.116.86.249/image/cuttingWithNormal.jpg</graph><graph CNCName='DMU160PdB' line='160' time='2200'>http://140.116.86.249/image/cuttingWithNormal.jpg</graph></sim_feature>";

                /**
                 * GetAllImagesData(ve圑隊給的XML格式,腳本代號)
                 */

                // Store Data in List Box.
                ListImagesObj = ImagesView.GetAllImagesData(ImageString, "1");
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }


        private void CB_SelectDisplayMachinePhoto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<ImageEntity> ListImagesObj2 = new List<ImageEntity>();
            // Check the List Object Count
            if (ListImagesObj.Count > 0)
            {
                foreach (ImageEntity ListImages in ListImagesObj)
                {
                    if (ListImages.ImageCNCName == CB_SelectDisplayMachinePhoto.SelectedItem.ToString())
                    {
                        ListImagesObj2.Add(new ImageEntity()
                        {
                            ImageCNCName = ListImages.ImageCNCName,
                            ImageName = ListImages.ImageName,
                            ImageLine = ListImages.ImageLine,
                            ImageNum = ListImages.ImageNum,
                            ImageTime = ListImages.ImageTime
                        });
                     }
                }
                // Bind data in List Box
                LsImageGallery.DataContext = ListImagesObj2;
            }
        }

        #endregion

        #region  Step:6 to cutting

        private void btnStartCutting_Click(object sender, RoutedEventArgs e)
        {
            ///<Upload>
            ///將NC檔透過請求實際加工機台切削子模組傳送到雲端上的BLOB，取得NC檔在雲端上的URL
            ///Function:UploadNCFile()
            ///<UploadtoBlob>

            XDocument srcFavCNC;
            string all_string = "";
            List<PE_SelectMachineToCutting> lstItem = new List<PE_SelectMachineToCutting>();
            IEnumerator ppEnum = DGRealMachining.ItemsSource.GetEnumerator();
            while (ppEnum.MoveNext())
            {
                lstItem.Add((PE_SelectMachineToCutting)ppEnum.Current);
            }

            foreach (PE_SelectMachineToCutting a in lstItem)
            {
                if (a.Machine_Select.Equals(true))
                {
                    foreach (PE_SelectMachineToSimulation b in SelectMachineToSimulationSource3axis)
                    {
                        if (a.Machine_Type.Equals(b.Machine_Type))
                        {
                            all_string = CBCompany.SelectedItem.ToString() + "," + b.vMachine + "," + b.Machine_CNC + "," + b.Machine_Type;
                        }
                    }

                    foreach (PE_SelectMachineToSimulation b in SelectMachineToSimulationSource5axis)
                    {
                        if (a.Machine_Type.Equals(b.Machine_Type))
                        {
                            all_string = CBCompany.SelectedItem.ToString() + "," + b.vMachine + "," + b.Machine_CNC + "," + b.Machine_Type;
                        }
                    }
                }
            }

            if (all_string != null)
            {
                Shell.waitingForm.SettingMessage("傳送選擇的機台機台之NC檔到雲端資料庫中");
                Shell.waitingForm.Show();
                ///呼叫PE_setActualProcessingCNC("公司名稱，vMachine，CNC ID，TYPE",NC檔名稱)
                OntologyService = new OntologyServiceClient();
                OntologyService.OntologySystemInterfaceAsync(StateManager.Username, "PE_setActualProcessingCNC", all_string + "@" + TBUpload_CLorNCFile.Text);
                OntologyService.OntologySystemInterfaceCompleted += new EventHandler<IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs>(proxy_PE_setActualProcessingCNCCompleted);
            }
        }

        void proxy_PE_setActualProcessingCNCCompleted(object sender, IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs e)
        {
            OntologyService.OntologySystemInterfaceCompleted -= new EventHandler<IPS.OntologyService.OntologySystemInterfaceCompletedEventArgs>(proxy_PE_setActualProcessingCNCCompleted);
            if (e.Error == null && e.Result != null)
            {
                if (e.Result.Equals("true"))
                {
                    MessageBox.Show("已將選擇的機台機台之NC檔傳送到雲端資料庫中!");
                }
                else
                {
                    MessageBox.Show("上傳失敗，請重新再上傳一次");
                }
                Shell.waitingForm.Close();
            }
            else
            {
                MessageBox.Show("雲端服務忙錄中，請稍候再試!");
                Shell.waitingForm.Close();
            }
        }


        private void btnNextToRealMachining_Click(object sender, RoutedEventArgs e)
        {

            Shell.waitingForm.SettingMessage("請稍候");
            Shell.waitingForm.Show();
            RealMachiningCut();
            Control_TabItemChange(6);
            Shell.waitingForm.Close();
        }

        private void RealMachiningCut()
        {
            ///存儲目前選擇的機台名稱 StorageMachineName
            List<PE_SelectMachineToCutting> lstCuttingItem = new List<PE_SelectMachineToCutting>();


            foreach (PE_SelectMachineToSimulation CuttingSource in lstMachinetoSimulationItem)
            {
                if (CuttingSource.Machine_Select.Equals(true))
                {
                    //foreach (PE_SelectMachineToSimulation CuttingSource in SelectMachineToSimulationSource)
                    //{
                    //    if (i.Machine_Name.Equals(CuttingSource.Machine_Name))
                    //    {
                            PE_SelectMachineToCutting CuttingInfo = new PE_SelectMachineToCutting();
                            CuttingInfo.Machine_Select = false;
                            CuttingInfo.Machine_Name = CuttingSource.Machine_Name;
                            CuttingInfo.Machine_Type = CuttingSource.Machine_Name;
                            CuttingInfo.Company_Name = CuttingSource.Company_Name;
                            CuttingInfo.Service_Broker = CuttingSource.Service_Broker;
                            CuttingInfo.vMachine = CuttingSource.vMachine;
                            lstCuttingItem.Add(CuttingInfo);
                   //   }
                   // }
                }
            }
            DGRealMachining.ItemsSource = lstCuttingItem;
        }

        #endregion
    }
}