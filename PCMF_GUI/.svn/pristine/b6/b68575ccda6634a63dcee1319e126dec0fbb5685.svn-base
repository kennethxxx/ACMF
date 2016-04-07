using System.Windows.Threading;
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using OMC.Comm;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Xml.Linq;

namespace IPS.ViewsSub.OntologyModule
{
    public class OntologyModuleFlowController
    {
        
        XDocument CompanyXml, ServiceBrokerXml, MachineInfoXml;

        private readonly Dispatcher dispatcher;

        public OntologyModuleFlowController(Dispatcher Owner) 
        {
            this.dispatcher = Owner;
        }

        //////////////////////////////////////////////////////////////////////////
        //呼叫DataTransfer
        public event EventHandler ExecuteDataTranferModule_Fail;
        public event EventHandler ExecuteDataTranferModule_Finish;
        public void ExecuteCompanyServiceModule()
        {
            //m_JobID = string.Empty;
            //m_Company = company;
            //m_User = user;

            //Shell.waitingForm.SettingSubMessage("Set Value");

            App.GetMachineInfoService = new IPS.MachineInfomation.Service1Client();

            App.GetMachineInfoService.GetCompanyInfoCompleted += new EventHandler<IPS.MachineInfomation.GetCompanyInfoCompletedEventArgs>(proxy_GetCompanyInfoCompleted);
            App.GetMachineInfoService.GetCompanyInfoAsync();

            //App.proxyMC.Set_DataTransferModuleCompleted += new EventHandler<Set_DataTransferModuleCompletedEventArgs>(Set_DataTransferModuleCompletedEvent);
            //App.proxyMC.Set_DataTransferModuleAsync(
            //    );
        }

        void proxy_GetCompanyInfoCompleted(object sender, IPS.MachineInfomation.GetCompanyInfoCompletedEventArgs e)
        {
            if (e.Error == null && e.Result != null)
            {
                //載入資料
                //核心尚未寫出來時用MessageBox印出結果，已寫出來把ResolveXMLFormat方法註解移除掉
                //MessageBox.Show(e.Result);
                ResolveXMLFormat(1, e.Result);
            }
            else
            {
                MessageBox.Show("未連上Service!!");
            }
        }


        private void ResolveXMLFormat(int index, string xmlStr)
        {
            switch (index)
            {
                case 1:
                    //公司資訊
                    System.IO.MemoryStream _memorystreamCompany = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xmlStr));
                    CompanyXml = XDocument.Load(_memorystreamCompany);
                    foreach (var CompanyInfo in CompanyXml.Element("Machine_Scheduling_Log").Descendants("Company_name"))
                    {
                        //CBCompany.Items.Add(CompanyInfo.Value);
                    }
                    break;
                case 2:
                    //ServiceBroker資訊
                    System.IO.MemoryStream _memorystreamServiceBroker = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xmlStr));
                    ServiceBrokerXml = XDocument.Load(_memorystreamServiceBroker);
                    foreach (var ServiceBrokerInfo in ServiceBrokerXml.Element("Machine_Scheduling_Log").Descendants("Service_Broker_ID"))
                    {
                        //CBServiceBroker.Items.Add(ServiceBrokerInfo.Value);
                    }
                    break;
                case 3:
                    //取得機台所有資訊
                    //GDGetMachineStatus.ItemsSource = null;
                    List<PE_GetStatusofMachines> CNCsource = new List<PE_GetStatusofMachines>();
                    ///取出多層中的Attribute所使用的方式
                    System.IO.MemoryStream _memorystream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xmlStr));
                    MachineInfoXml = XDocument.Load(_memorystream);
                    foreach (var MachineInfo in MachineInfoXml.Element("Machine_Scheduling_Log").Descendants("vMachine"))
                    {
                        foreach (var MachineInfo2 in MachineInfoXml.Element("Machine_Scheduling_Log").Element("vMachine").Descendants("Output_Machine"))
                        {
                            CNCsource.Add(new PE_GetStatusofMachines(MachineInfo.Attribute("Name").Value, MachineInfo2.Attribute("Name").Value, MachineInfo2.Attribute("Type").Value, MachineInfo2.Attribute("State").Value));
                        }
                    }
                    //GDGetMachineStatus.ItemsSource = CNCsource;
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
                                RuleInfo.Rule_Select = true;
                                RuleInfo.TB_Rule_ID = rules_id;
                                RuleInfo.TB_Rule_Name = rules_name;
                                RuleInfo.TB_Rule_Description = rule_Description;
                                RuleInfo.TB_Rule_SWRL = ruleswrl;
                                Rule_source.Add(RuleInfo);
                            }
                            catch (Exception ex)
                            { }
                        }
                    }
                    //DGSelectRule1.ItemsSource = Rule_source;
                    break;
                case 5:
                    ///<summery>
                    ///讀XML字串方式轉成datagrid
                    ///</summery>
                    List<PE_SelectMachineToSimulation> SelectMachineToSimulationSource = new List<PE_SelectMachineToSimulation>();
                    using (XmlReader xReader = XmlReader.Create(new StringReader(xmlStr)))
                    {
                        xReader.Read();
                        while (xReader.Read())
                        {
                            try
                            {
                                xReader.ReadToFollowing("Favorite_CNC_Select");
                                string FavSelectCNC = xReader.ReadElementContentAsString();
                                xReader.ReadToNextSibling("CNC_NameXML");
                                string CNC_NameString = xReader.ReadElementContentAsString();

                                PE_SelectMachineToSimulation SCNCInfo = new PE_SelectMachineToSimulation();
                                if (FavSelectCNC.Equals("True") == true)
                                {
                                    SCNCInfo.Machine_Select = true;
                                }
                                else
                                {
                                    SCNCInfo.Machine_Select = false;
                                }
                                SCNCInfo.Machine_Name = CNC_NameString;

                                SelectMachineToSimulationSource.Add(SCNCInfo);
                            }
                            catch (Exception ex)
                            { }

                        }
                    }
                    //DGSelectMachineToCuttingSimulation.ItemsSource = SelectMachineToSimulationSource;
                    break;
            }
        }


        private void Set_DataTransferModuleCompletedEvent(object sender, EventArgs e)
        {
            bool IsSuccess = false;
            //App.proxyMC.Set_DataTransferModuleCompleted -= new EventHandler<Set_DataTransferModuleCompletedEventArgs>(Set_DataTransferModuleCompletedEvent);
            
            try
            {
                //m_JobID = ((Set_DataTransferModuleCompletedEventArgs)e).Result;
                //if (m_JobID.CompareTo("") != 0)
                //{
                //    IsSuccess = true;
                //}
                //else
                //{
                //    IsSuccess = false;
                //    MessageBox.Show("Error: Setting Data Transfer Module Fail! Please try again.");
                //}
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                MessageBox.Show("Error: " + ex.ToString());
            }

            if (IsSuccess)
            {
                //// 等待執行完成
                //Shell.waitingForm.SettingSubMessage("Wait");
                //App.proxyMC.ChecktJobStateCompleted += new EventHandler<ChecktJobStateCompletedEventArgs>(Check_DataTransferModuleCompletedEvent);
                //App.proxyMC.ChecktJobStateAsync(
                //    m_JobID,
                //    new In_UserInfo() { Company = m_Company, User = m_User }
                //    );
            }
            else
            {
                // 不成功就呼叫失敗處理事件
                if (ExecuteDataTranferModule_Fail != null)
                {
                    ExecuteDataTranferModule_Fail(this, null);
                }
            }
        }










    }
}
