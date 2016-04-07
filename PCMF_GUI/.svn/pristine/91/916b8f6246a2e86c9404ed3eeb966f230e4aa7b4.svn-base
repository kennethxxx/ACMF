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
using System.Collections.ObjectModel;
using IPS.ModelCreation;
using IPS.Common;
using System.Threading;
using System.Windows.Navigation;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Threading;


namespace IPS.ViewsSub.ModelCreationModule
{
    public class ModelCreationModuleAlgorithmFlowControler
    {
        string m_JobID = string.Empty;
        String m_Company = string.Empty;
        String m_User = string.Empty;
        private readonly Dispatcher dispatcher;

        public ModelCreationModuleAlgorithmFlowControler(Dispatcher Owner) 
        {
            this.dispatcher = Owner;
        }

        //呼叫DataTransfer
        public event EventHandler ExecuteDataTranferModule_Fail;
        public event EventHandler ExecuteDataTranferModule_Finish;
        public void ExecuteDataTranferModule(
            int iContextIDCount, int iTrainContextIDCount, int iRunContextIDCount,
            ObservableCollection<MetrologyPoint> ProcessKeyList,
            ObservableCollection<MetrologyPoint> MetrologyKeyList,
            DateTime SearchStartTime, DateTime SearchEndTime,
            ObservableCollection<MetrologyPoint> CombinedIndicator,
            ObservableCollection<MetrologyPoint> CombinedPoint,
            ObservableCollection<Group> GroupValue,
            String vMachineID, String CNCType, String CNCNumber,
            String NCProgram, String ModelID, String Version,
            ObservableCollection<int> AllAction,
            ObservableCollection<String> AbnormationList,
            ObservableCollection<String> IsolatedList,
            String XTable, String YTable,
            String strSelectID, String strProductID,
            String company, String user
            )
        {
            m_JobID = string.Empty;
            m_Company = company;
            m_User = user;

            //Shell.waitingForm.SettingSubMessage("Set Value");
            Shell.waitingForm.SettingSubMessage("設定變數");
            

            App.proxyMC.Set_DataTransferModuleCompleted += new EventHandler<Set_DataTransferModuleCompletedEventArgs>(Set_DataTransferModuleCompletedEvent);
            App.proxyMC.Set_DataTransferModuleAsync(
                iContextIDCount,
                iTrainContextIDCount,
                iRunContextIDCount,
                ProcessKeyList,
                MetrologyKeyList,
                SearchStartTime,
                SearchEndTime,
                CombinedIndicator,
                CombinedPoint,
                GroupValue,
                new In_UserInfo() { Company = m_Company, User = m_User },
                vMachineID,
                CNCType,
                CNCNumber,
                NCProgram,
                ModelID,
                Version,
                AllAction,
                AbnormationList,                
                IsolatedList,
                XTable,
                YTable,
                strSelectID,
                strProductID);
        }
        private void Set_DataTransferModuleCompletedEvent(object sender, EventArgs e)
        {
            bool IsSuccess = false;
            App.proxyMC.Set_DataTransferModuleCompleted -= new EventHandler<Set_DataTransferModuleCompletedEventArgs>(Set_DataTransferModuleCompletedEvent);
            try
            {
                m_JobID = ((Set_DataTransferModuleCompletedEventArgs)e).Result;
                if (m_JobID.CompareTo("") != 0)
                {
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                    MessageBox.Show("Error: Setting Data Transfer Module Fail! Please try again.");
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                MessageBox.Show("Error: " + ex.ToString());
            }

            if (IsSuccess)
            {
                // 等待執行完成
                //Shell.waitingForm.SettingSubMessage("Wait");
                Shell.waitingForm.SettingSubMessage("等待中");
                App.proxyMC.ChecktJobStateCompleted += new EventHandler<ChecktJobStateCompletedEventArgs>(Check_DataTransferModuleCompletedEvent);
                App.proxyMC.ChecktJobStateAsync(
                    m_JobID,
                    new In_UserInfo() { Company = m_Company, User = m_User }
                    );
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
        private void Check_DataTransferModuleCompletedEvent(object sender, EventArgs e)
        {
            App.proxyMC.ChecktJobStateCompleted -= new EventHandler<ChecktJobStateCompletedEventArgs>(Check_DataTransferModuleCompletedEvent);

            int iStep = 0; // 0: fail 1:success other: message
            string ResultMessage = "";

            try
            {
                ResultMessage = ((ChecktJobStateCompletedEventArgs)e).Result;
                if (ResultMessage.Contains("success"))
                {
                    //已經完成 可以進行取回
                    iStep = 1;
                }
                else if (ResultMessage.Contains("fail"))
                {
                    //已經失敗
                    iStep = 0;
                }
                else
                {
                    //未完成
                    iStep = 2;
                }
            }
            catch (Exception ex)
            {
                ResultMessage = ("Error: " + ex.ToString());
                iStep = 0;
            }

            if (iStep == 0)
            {
                MessageBox.Show(ResultMessage);
                // 不成功就呼叫失敗處理事件
                if (ExecuteDataTranferModule_Fail != null)
                {
                    ExecuteDataTranferModule_Fail(this, null);
                }            
            }
            else if (iStep == 1)
            {
              //  Shell.waitingForm.SettingSubMessage("Get Result");
                Shell.waitingForm.SettingSubMessage("取回結果");
                App.proxyMC.Get_DataTransferResultCompleted += new EventHandler<Get_DataTransferResultCompletedEventArgs>(Get_DataTransferResultCompletedEvent);
                App.proxyMC.Get_DataTransferResultAsync(
                    m_JobID,
                    new In_UserInfo() { Company = m_Company, User = m_User }
                    );
            }
            else
            {
                Shell.waitingForm.SettingSubMessage(ResultMessage);

                Thread SleepThread = new Thread(() =>
                {
                    Thread.Sleep(4000);
                    // 需要使用UI帶進來的Dispatcher執行體來執行
                    dispatcher.BeginInvoke(delegate()
                    {
                        App.proxyMC.ChecktJobStateCompleted += new EventHandler<ChecktJobStateCompletedEventArgs>(Check_DataTransferModuleCompletedEvent);
                        App.proxyMC.ChecktJobStateAsync(
                            m_JobID,
                            new In_UserInfo() { Company = m_Company, User = m_User }
                        );
                    });                    
                });

                SleepThread.Start();
            }
        }
        private void Get_DataTransferResultCompletedEvent(object sender, EventArgs e)
        {
            App.proxyMC.Get_DataTransferResultCompleted -= new EventHandler<Get_DataTransferResultCompletedEventArgs>(Get_DataTransferResultCompletedEvent);

            if (ExecuteDataTranferModule_Finish != null)
            {
                ExecuteDataTranferModule_Finish(this, e);
            }
        }

        //呼叫MDFRModule
        public event EventHandler ExecuteMDFRModule_Fail;
        public event EventHandler ExecuteMDFRModule_Finish;
        public void ExecuteMDFRModule(String company, String user)
        {
            m_JobID = string.Empty;
            m_Company = company;
            m_User = user;

            //double ewmaLamda = 0.5;
            //double ewmaTolerance = 0.33;
            //double ewmaWindow = 30;
            //double varConfidence = 0.7;
            //double baseSampleNum = 30;
            //double rangeMultipleValue = 1.5;

            ////////20121026-Kenny建議

            double ewmaLamda = 0.5;
            double ewmaTolerance = 2;
            double ewmaWindow = 30;
            double varConfidence = 0.9999;
            double baseSampleNum = 30;
            double rangeMultipleValue = 5;

            //Shell.waitingForm.SettingSubMessage("Set Value");
            Shell.waitingForm.SettingSubMessage("設置變數");
            App.proxyMC.Set_MDFRModuleCompleted += new EventHandler<Set_MDFRModuleCompletedEventArgs>(Set_MDFRModuleCompletedEvent);
            App.proxyMC.Set_MDFRModuleAsync(
                ewmaLamda,
                ewmaTolerance,
                ewmaWindow,
                varConfidence,
                baseSampleNum,
                rangeMultipleValue,
                new In_UserInfo() { Company = m_Company, User = m_User }
                );
        }
        private void Set_MDFRModuleCompletedEvent(object sender, Set_MDFRModuleCompletedEventArgs e)
        {
            bool IsSuccess = false;
            App.proxyMC.Set_MDFRModuleCompleted -= new EventHandler<Set_MDFRModuleCompletedEventArgs>(Set_MDFRModuleCompletedEvent);

            try
            {
                m_JobID = e.Result;
                if (m_JobID.CompareTo("") != 0)
                {
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                    MessageBox.Show("Error: Setting MDFR Module fail! Please try it again.");
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                MessageBox.Show("Error: " + ex.ToString());
            }

            if (IsSuccess)
            {
                // 等待執行完成
               // Shell.waitingForm.SettingSubMessage("Wait");
                Shell.waitingForm.SettingSubMessage("等待中");
                App.proxyMC.ChecktJobStateCompleted += new EventHandler<ChecktJobStateCompletedEventArgs>(Check_MDFRModuleCompletedEvent);
                App.proxyMC.ChecktJobStateAsync(
                    m_JobID,
                    new In_UserInfo() { Company = m_Company, User = m_User }
                    );
            }
            else
            {
                // 不成功就呼叫失敗處理事件
                if (ExecuteMDFRModule_Fail != null)
                {
                    ExecuteMDFRModule_Fail(this, null);
                }
            }
        }
        private void Check_MDFRModuleCompletedEvent(object sender, ChecktJobStateCompletedEventArgs e)
        {
            App.proxyMC.ChecktJobStateCompleted -= new EventHandler<ChecktJobStateCompletedEventArgs>(Check_MDFRModuleCompletedEvent);

            int iStep = 0; // 0: fail 1:success other: message
            string ResultMessage = "";

            try
            {
                ResultMessage = ((ChecktJobStateCompletedEventArgs)e).Result;
                if (ResultMessage.Contains("success"))
                {
                    //已經完成 可以進行取回
                    iStep = 1;
                }
                else if (ResultMessage.Contains("fail"))
                {
                    //已經失敗
                    iStep = 0;
                }
                else
                {
                    //未完成
                    iStep = 2;
                }
            }
            catch (Exception ex)
            {
                ResultMessage = ("Error: " + ex.ToString());
                iStep = 0;
            }

            if (iStep == 0)
            {
                MessageBox.Show(ResultMessage);
                // 不成功就呼叫失敗處理事件
                if (ExecuteMDFRModule_Fail != null)
                {
                    ExecuteMDFRModule_Fail(this, null);
                }
            }
            else if (iStep == 1)
            {
                //Shell.waitingForm.SettingSubMessage("Get Result");
                Shell.waitingForm.SettingSubMessage("取回結果");
                App.proxyMC.Get_MDFRResultCompleted += new EventHandler<Get_MDFRResultCompletedEventArgs>(Get_MDFRResultCompletedEvent);
                App.proxyMC.Get_MDFRResultAsync(
                    m_JobID,
                    new In_UserInfo() { Company = m_Company, User = m_User }
                    );
            }
            else
            {
                Shell.waitingForm.SettingSubMessage(ResultMessage);

                Thread SleepThread = new Thread(() =>
                {
                    Thread.Sleep(4000);
                    // 需要使用UI帶進來的Dispatcher執行體來執行
                    dispatcher.BeginInvoke(delegate()
                    {
                        App.proxyMC.ChecktJobStateCompleted += new EventHandler<ChecktJobStateCompletedEventArgs>(Check_MDFRModuleCompletedEvent);
                        App.proxyMC.ChecktJobStateAsync(
                            m_JobID,
                            new In_UserInfo() { Company = m_Company, User = m_User }
                        );
                    });
                });
                SleepThread.Start();
            }
        }
        private void Get_MDFRResultCompletedEvent(object sender, Get_MDFRResultCompletedEventArgs e)
        {
            App.proxyMC.Get_MDFRResultCompleted -= new EventHandler<Get_MDFRResultCompletedEventArgs>(Get_MDFRResultCompletedEvent);
            if (ExecuteMDFRModule_Finish != null)
            {
                ExecuteMDFRModule_Finish(this, e);
            }
        }

        //呼叫DQIyModule_GetDQIyPattern
        public event EventHandler ExecuteDQIyModule_GetDQIyPattern_Fail;
        public event EventHandler ExecuteDQIyModule_GetDQIyPattern_Finish;
        public void ExecuteDQIyModule_GetDQIyPattern(String company, String user)
        {
            m_JobID = string.Empty;
            m_Company = company;
            m_User = user;

          //  Shell.waitingForm.SettingSubMessage("Set Value");
            Shell.waitingForm.SettingSubMessage("設置變數");
            App.proxyMC.Set_DQIyModule_GetDQIyPatternCompleted += new EventHandler<Set_DQIyModule_GetDQIyPatternCompletedEventArgs>(Set_DQIyModule_GetDQIyPatternCompletedEvent);
            App.proxyMC.Set_DQIyModule_GetDQIyPatternAsync(
                new In_UserInfo() { Company = m_Company, User = m_User }
                );
        }
        private void Set_DQIyModule_GetDQIyPatternCompletedEvent(object sender, Set_DQIyModule_GetDQIyPatternCompletedEventArgs e)
        {
            bool IsSuccess = false;
            App.proxyMC.Set_DQIyModule_GetDQIyPatternCompleted -= new EventHandler<Set_DQIyModule_GetDQIyPatternCompletedEventArgs>(Set_DQIyModule_GetDQIyPatternCompletedEvent);

            try
            {
                m_JobID = ((Set_DQIyModule_GetDQIyPatternCompletedEventArgs)e).Result;
                if (m_JobID.CompareTo("") != 0)
                {
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                    MessageBox.Show("Error: Setting DQIy Module - GetDQIyPattern fail! Please try it again.");
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                MessageBox.Show("Error: " + ex.ToString());
            }

            if (IsSuccess)
            {
                // 等待執行完成
                //Shell.waitingForm.SettingSubMessage("Wait");
                Shell.waitingForm.SettingSubMessage("等待中");
                App.proxyMC.ChecktJobStateCompleted += new EventHandler<ChecktJobStateCompletedEventArgs>(Check_DQIyModule_GetDQIyPatternCompletedEvent);
                App.proxyMC.ChecktJobStateAsync(
                    m_JobID,
                    new In_UserInfo() { Company = m_Company, User = m_User }
                    );
            }
            else
            {
                // 不成功就呼叫失敗處理事件
                if (ExecuteDQIyModule_GetDQIyPattern_Fail != null)
                {
                    ExecuteDQIyModule_GetDQIyPattern_Fail(this, null);
                }
            }
        }
        private void Check_DQIyModule_GetDQIyPatternCompletedEvent(object sender, ChecktJobStateCompletedEventArgs e)
        {
            App.proxyMC.ChecktJobStateCompleted -= new EventHandler<ChecktJobStateCompletedEventArgs>(Check_DQIyModule_GetDQIyPatternCompletedEvent);

            int iStep = 0; // 0: fail 1:success other: message
            string ResultMessage = "";

            try
            {
                ResultMessage = ((ChecktJobStateCompletedEventArgs)e).Result;
                if (ResultMessage.Contains("success"))
                {
                    //已經完成 可以進行取回
                    iStep = 1;
                }
                else if (ResultMessage.Contains("fail"))
                {
                    //已經失敗
                    iStep = 0;
                }
                else
                {
                    //未完成
                    iStep = 2;
                }
            }
            catch (Exception ex)
            {
                ResultMessage = ("Error: " + ex.ToString());
                iStep = 0;
            }

            if (iStep == 0)
            {
                MessageBox.Show(ResultMessage);
                // 不成功就呼叫失敗處理事件
                if (ExecuteDQIyModule_GetDQIyPattern_Fail != null)
                {
                    ExecuteDQIyModule_GetDQIyPattern_Fail(this, null);
                }
            }
            else if (iStep == 1)
            {
              //  Shell.waitingForm.SettingSubMessage("Get Result");
                Shell.waitingForm.SettingSubMessage("取回結果");
                App.proxyMC.Get_DQIyResult_GetDQIyPatternCompleted += new EventHandler<Get_DQIyResult_GetDQIyPatternCompletedEventArgs>(Get_DQIyResult_GetDQIyPatternCompletedEvent);
                App.proxyMC.Get_DQIyResult_GetDQIyPatternAsync(
                    m_JobID,
                    new In_UserInfo() { Company = m_Company, User = m_User }
                    );
            }
            else
            {
                Shell.waitingForm.SettingSubMessage(ResultMessage);

                Thread SleepThread = new Thread(() =>
                {
                    Thread.Sleep(4000);
                    // 需要使用UI帶進來的Dispatcher執行體來執行
                    dispatcher.BeginInvoke(delegate()
                    {
                        App.proxyMC.ChecktJobStateCompleted += new EventHandler<ChecktJobStateCompletedEventArgs>(Check_DQIyModule_GetDQIyPatternCompletedEvent);
                        App.proxyMC.ChecktJobStateAsync(
                            m_JobID,
                            new In_UserInfo() { Company = m_Company, User = m_User }
                        );
                    });
                });
                SleepThread.Start();
            }
        }
        private void Get_DQIyResult_GetDQIyPatternCompletedEvent(object sender, Get_DQIyResult_GetDQIyPatternCompletedEventArgs e)
        {
            App.proxyMC.Get_DQIyResult_GetDQIyPatternCompleted -= new EventHandler<Get_DQIyResult_GetDQIyPatternCompletedEventArgs>(Get_DQIyResult_GetDQIyPatternCompletedEvent);

            if (ExecuteDQIyModule_GetDQIyPattern_Finish != null)
            {
                ExecuteDQIyModule_GetDQIyPattern_Finish(this, e);
            }
        }

        //呼叫KSS
        public event EventHandler ExecuteKSSModule_Fail;
        public event EventHandler ExecuteKSSModule_Finish;
        public void ExecuteKSSModule(String AlgorithmName, Double ClusterNumber, String company, String user)
        {
            m_JobID = string.Empty;
            m_Company = company;
            m_User = user;

            //Shell.waitingForm.SettingSubMessage("Set Value");
            Shell.waitingForm.SettingSubMessage("設置變數");
            App.proxyMC.Set_KSSModuleCompleted += new EventHandler<Set_KSSModuleCompletedEventArgs>(Set_KSSModuleCompletedEvent);
            App.proxyMC.Set_KSSModuleAsync(
                AlgorithmName,
                ClusterNumber,
                new In_UserInfo() { Company = m_Company, User = m_User }
                );
        }
        private void Set_KSSModuleCompletedEvent(object sender, Set_KSSModuleCompletedEventArgs e)
        {
            bool IsSuccess = false;
            App.proxyMC.Set_KSSModuleCompleted -= new EventHandler<Set_KSSModuleCompletedEventArgs>(Set_KSSModuleCompletedEvent);

            try
            {
                m_JobID = ((Set_KSSModuleCompletedEventArgs)e).Result;
                if (m_JobID.CompareTo("") != 0)
                {
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                    MessageBox.Show("Error: Setting KSS Module fail! Please try it again.");
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                MessageBox.Show("Error: " + ex.ToString());
            }

            if (IsSuccess)
            {
                // 等待執行完成
                //Shell.waitingForm.SettingSubMessage("Wait");
                Shell.waitingForm.SettingSubMessage("等待中");
                App.proxyMC.ChecktJobStateCompleted += new EventHandler<ChecktJobStateCompletedEventArgs>(Check_KSSModuleCompletedEvent);
                App.proxyMC.ChecktJobStateAsync(
                    m_JobID,
                    new In_UserInfo() { Company = m_Company, User = m_User }
                    );
            }
            else
            {
                // 不成功就呼叫失敗處理事件
                if (ExecuteKSSModule_Fail != null)
                {
                    ExecuteKSSModule_Fail(this, null);
                }
            }
        }
        private void Check_KSSModuleCompletedEvent(object sender, ChecktJobStateCompletedEventArgs e)
        {
            App.proxyMC.ChecktJobStateCompleted -= new EventHandler<ChecktJobStateCompletedEventArgs>(Check_KSSModuleCompletedEvent);

            int iStep = 0; // 0: fail 1:success other: message
            string ResultMessage = "";

            try
            {
                ResultMessage = ((ChecktJobStateCompletedEventArgs)e).Result;
                if (ResultMessage.Contains("success"))
                {
                    //已經完成 可以進行取回
                    iStep = 1;
                }
                else if (ResultMessage.Contains("fail"))
                {
                    //已經失敗
                    iStep = 0;
                }
                else
                {
                    //未完成
                    iStep = 2;
                }
            }
            catch (Exception ex)
            {
                ResultMessage = ("Error: " + ex.ToString());
                iStep = 0;
            }

            if (iStep == 0)
            {
                MessageBox.Show(ResultMessage);
                // 不成功就呼叫失敗處理事件
                if (ExecuteKSSModule_Fail != null)
                {
                    ExecuteKSSModule_Fail(this, null);
                }
            }
            else if (iStep == 1)
            {
                //Shell.waitingForm.SettingSubMessage("Get Result");
                Shell.waitingForm.SettingSubMessage("取回結果");
                App.proxyMC.Get_KSSResultCompleted += new EventHandler<Get_KSSResultCompletedEventArgs>(Get_KSSResultCompletedEvent);
                App.proxyMC.Get_KSSResultAsync(
                    m_JobID,
                    new In_UserInfo() { Company = m_Company, User = m_User }
                    );
            }
            else
            {
                Shell.waitingForm.SettingSubMessage(ResultMessage);

                Thread SleepThread = new Thread(() =>
                {
                    Thread.Sleep(4000);
                    // 需要使用UI帶進來的Dispatcher執行體來執行
                    dispatcher.BeginInvoke(delegate()
                    {
                        App.proxyMC.ChecktJobStateCompleted += new EventHandler<ChecktJobStateCompletedEventArgs>(Check_KSSModuleCompletedEvent);
                        App.proxyMC.ChecktJobStateAsync(
                            m_JobID,
                            new In_UserInfo() { Company = m_Company, User = m_User }
                        );
                    }); 
                });
                SleepThread.Start();
            }
        }
        private void Get_KSSResultCompletedEvent(object sender, Get_KSSResultCompletedEventArgs e)
        {
            App.proxyMC.Get_KSSResultCompleted -= new EventHandler<Get_KSSResultCompletedEventArgs>(Get_KSSResultCompletedEvent);

            if (ExecuteKSSModule_Finish != null)
            {
                ExecuteKSSModule_Finish(this, e);
            }
        }

        //呼叫KVS
        public event EventHandler ExecuteKVSModule_Fail;
        public event EventHandler ExecuteKVSModule_Finish;
        public void ExecuteKVSModule(String Algorithm, Double FIn_Alpha, Double Fout_Alpha, String VerifiedMode, String company, String user)
        {
            m_JobID = string.Empty;
            m_Company = company;
            m_User = user;

          //  Shell.waitingForm.SettingSubMessage("Set Value");
            Shell.waitingForm.SettingSubMessage("設置變數");
            App.proxyMC.Set_KVSModuleCompleted += new EventHandler<Set_KVSModuleCompletedEventArgs>(Set_KVSModuleCompletedEvent);
            App.proxyMC.Set_KVSModuleAsync(
                FIn_Alpha,
                Fout_Alpha,
                VerifiedMode,
                Algorithm,
                new In_UserInfo() { Company = m_Company, User = m_User }
                );
        }
        private void Set_KVSModuleCompletedEvent(object sender, Set_KVSModuleCompletedEventArgs e)
        {
            bool IsSuccess = false;
            App.proxyMC.Set_KVSModuleCompleted -= new EventHandler<Set_KVSModuleCompletedEventArgs>(Set_KVSModuleCompletedEvent);

            try
            {
                m_JobID = ((Set_KVSModuleCompletedEventArgs)e).Result;
                if (m_JobID.CompareTo("") != 0)
                {
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                    MessageBox.Show("Error: Setting KVS Module fail! Please try it again.");
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                MessageBox.Show("Error: " + ex.ToString());
            }

            if (IsSuccess)
            {
                // 等待執行完成
              //  Shell.waitingForm.SettingSubMessage("Wait");
                Shell.waitingForm.SettingSubMessage("等待中");
                App.proxyMC.ChecktJobStateCompleted += new EventHandler<ChecktJobStateCompletedEventArgs>(Check_KVSModuleCompletedEvent);
                App.proxyMC.ChecktJobStateAsync(
                    m_JobID,
                    new In_UserInfo() { Company = m_Company, User = m_User }
                    );
            }
            else
            {
                // 不成功就呼叫失敗處理事件
                if (ExecuteKVSModule_Fail != null)
                {
                    ExecuteKVSModule_Fail(this, null);
                }
            }
        }
        private void Check_KVSModuleCompletedEvent(object sender, ChecktJobStateCompletedEventArgs e)
        {
            App.proxyMC.ChecktJobStateCompleted -= new EventHandler<ChecktJobStateCompletedEventArgs>(Check_KVSModuleCompletedEvent);

            int iStep = 0; // 0: fail 1:success other: message
            string ResultMessage = "";

            try
            {
                ResultMessage = ((ChecktJobStateCompletedEventArgs)e).Result;
                if (ResultMessage.Contains("success"))
                {
                    //已經完成 可以進行取回
                    iStep = 1;
                }
                else if (ResultMessage.Contains("fail"))
                {
                    //已經失敗
                    iStep = 0;
                }
                else
                {
                    //未完成
                    iStep = 2;
                }
            }
            catch (Exception ex)
            {
                ResultMessage = ("Error: " + ex.ToString());
                iStep = 0;
            }

            if (iStep == 0)
            {
                MessageBox.Show(ResultMessage);
                // 不成功就呼叫失敗處理事件
                if (ExecuteKVSModule_Fail != null)
                {
                    ExecuteKVSModule_Fail(this, null);
                }    
            }
            else if (iStep == 1)
            {
                //Shell.waitingForm.SettingSubMessage("Get Result");
                Shell.waitingForm.SettingSubMessage("取回結果");
                App.proxyMC.Get_KVSResultCompleted += new EventHandler<Get_KVSResultCompletedEventArgs>(Get_KVSResultCompletedEvent);
                App.proxyMC.Get_KVSResultAsync(
                    m_JobID,
                    new In_UserInfo() { Company = m_Company, User = m_User }
                    );
            }
            else
            {
                Shell.waitingForm.SettingSubMessage(ResultMessage);

                Thread SleepThread = new Thread(() =>
                {
                    Thread.Sleep(4000);
                    // 需要使用UI帶進來的Dispatcher執行體來執行
                    dispatcher.BeginInvoke(delegate()
                    {
                        App.proxyMC.ChecktJobStateCompleted += new EventHandler<ChecktJobStateCompletedEventArgs>(Check_KVSModuleCompletedEvent);
                        App.proxyMC.ChecktJobStateAsync(
                            m_JobID,
                            new In_UserInfo() { Company = m_Company, User = m_User }
                        );
                    }); 
                });
                SleepThread.Start();
            }
        }
        private void Get_KVSResultCompletedEvent(object sender, Get_KVSResultCompletedEventArgs e)
        {
            App.proxyMC.Get_KVSResultCompleted -= new EventHandler<Get_KVSResultCompletedEventArgs>(Get_KVSResultCompletedEvent);
            if (ExecuteKVSModule_Finish != null)
            {
                ExecuteKVSModule_Finish(this, e);
            }
        }

        //呼叫DQIxModule_VerifyDQIx
        public event EventHandler ExecuteDQIxModule_VerifyDQIx_Fail;
        public event EventHandler ExecuteDQIxModule_VerifyDQIx_Finish;
        public void ExecuteDQIxModule_VerifyDQIx(Double Lambda, Double Threshold, Double FilterPercentage, Double RefreshCounter, String company, String user)
        {
            m_JobID = string.Empty;
            m_Company = company;
            m_User = user;

           // Shell.waitingForm.SettingSubMessage("Set Value");
            Shell.waitingForm.SettingSubMessage("設置變數");
            App.proxyMC.Set_DQIxModule_VerifyDQIxCompleted += new EventHandler<Set_DQIxModule_VerifyDQIxCompletedEventArgs>(Set_DQIxModule_VerifyDQIxCompletedEvent);
            App.proxyMC.Set_DQIxModule_VerifyDQIxAsync(
                Lambda,
                Threshold,
                FilterPercentage,
                RefreshCounter,
                new In_UserInfo() { Company = m_Company, User = m_User }
                );
        }
        private void Set_DQIxModule_VerifyDQIxCompletedEvent(object sender, Set_DQIxModule_VerifyDQIxCompletedEventArgs e)
        {
            bool IsSuccess = false;
            App.proxyMC.Set_DQIxModule_VerifyDQIxCompleted -= new EventHandler<Set_DQIxModule_VerifyDQIxCompletedEventArgs>(Set_DQIxModule_VerifyDQIxCompletedEvent);

            try
            {
                m_JobID = ((Set_DQIxModule_VerifyDQIxCompletedEventArgs)e).Result;
                if (m_JobID.CompareTo("") != 0)
                {
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                    MessageBox.Show("Error: Setting DQIx Module - Verifying DQIx fail! Please try it again.");
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                MessageBox.Show("Error: " + ex.ToString());
            }

            if (IsSuccess)
            {
                // 等待執行完成
               // Shell.waitingForm.SettingSubMessage("Wait");
                Shell.waitingForm.SettingSubMessage("等待中");
                App.proxyMC.ChecktJobStateCompleted += new EventHandler<ChecktJobStateCompletedEventArgs>(Check_DQIxModule_VerifyDQIxCompletedEvent);
                App.proxyMC.ChecktJobStateAsync(
                    m_JobID,
                    new In_UserInfo() { Company = m_Company, User = m_User }
                    );
            }
            else
            {
                // 不成功就呼叫失敗處理事件
                if (ExecuteDQIxModule_VerifyDQIx_Fail != null)
                {
                    ExecuteDQIxModule_VerifyDQIx_Fail(this, null);
                }
            }
        }
        private void Check_DQIxModule_VerifyDQIxCompletedEvent(object sender, ChecktJobStateCompletedEventArgs e)
        {
            App.proxyMC.ChecktJobStateCompleted -= new EventHandler<ChecktJobStateCompletedEventArgs>(Check_DQIxModule_VerifyDQIxCompletedEvent);

            int iStep = 0; // 0: fail 1:success other: message
            string ResultMessage = "";

            try
            {
                ResultMessage = ((ChecktJobStateCompletedEventArgs)e).Result;
                if (ResultMessage.Contains("success"))
                {
                    //已經完成 可以進行取回
                    iStep = 1;
                }
                else if (ResultMessage.Contains("fail"))
                {
                    //已經失敗
                    iStep = 0;
                }
                else
                {
                    //未完成
                    iStep = 2;
                }
            }
            catch (Exception ex)
            {
                ResultMessage = ("Error: " + ex.ToString());
                iStep = 0;
            }

            if (iStep == 0)
            {
                MessageBox.Show(ResultMessage);
                // 不成功就呼叫失敗處理事件
                if (ExecuteDQIxModule_VerifyDQIx_Fail != null)
                {
                    ExecuteDQIxModule_VerifyDQIx_Fail(this, null);
                }
            }
            else if (iStep == 1)
            {
               // Shell.waitingForm.SettingSubMessage("Get Result");
                Shell.waitingForm.SettingSubMessage("取回結果");
                App.proxyMC.Get_DQIxResult_VerifyDQIxCompleted += new EventHandler<Get_DQIxResult_VerifyDQIxCompletedEventArgs>(Get_DQIxResult_VerifyDQIxCompletedEvent);
                App.proxyMC.Get_DQIxResult_VerifyDQIxAsync(
                    m_JobID,
                    new In_UserInfo() { Company = m_Company, User = m_User }
                    );
            }
            else
            {
                Shell.waitingForm.SettingSubMessage(ResultMessage);

                Thread SleepThread = new Thread(() =>
                {
                    Thread.Sleep(4000);
                    // 需要使用UI帶進來的Dispatcher執行體來執行
                    dispatcher.BeginInvoke(delegate()
                    {
                        App.proxyMC.ChecktJobStateCompleted += new EventHandler<ChecktJobStateCompletedEventArgs>(Check_DQIxModule_VerifyDQIxCompletedEvent);
                        App.proxyMC.ChecktJobStateAsync(
                            m_JobID,
                            new In_UserInfo() { Company = m_Company, User = m_User }
                        );
                    });
                });
                SleepThread.Start();
            }
        }
        private void Get_DQIxResult_VerifyDQIxCompletedEvent(object sender, Get_DQIxResult_VerifyDQIxCompletedEventArgs e)
        {
            App.proxyMC.Get_DQIxResult_VerifyDQIxCompleted -= new EventHandler<Get_DQIxResult_VerifyDQIxCompletedEventArgs>(Get_DQIxResult_VerifyDQIxCompletedEvent);

            if (ExecuteDQIxModule_VerifyDQIx_Finish != null)
            {
                ExecuteDQIxModule_VerifyDQIx_Finish(this, e);
            }
        }


        //呼叫DQIyModule_VerifyDQIy
        public event EventHandler ExecuteDQIyModule_VerifyDQIy_Fail;
        public event EventHandler ExecuteDQIyModule_VerifyDQIy_Finish;
        public void ExecuteDQIyModule_VerifyDQIy(String company, String user)
        {
            m_JobID = string.Empty;
            m_Company = company;
            m_User = user;

            //need to update GUI to set parameters
            double CorrAlpha = 0.0001;
            double MixedModel = 1;
           // Shell.waitingForm.SettingSubMessage("Set Value");
            Shell.waitingForm.SettingSubMessage("設定變數");
            App.proxyMC.Set_DQIyModule_VerifyDQIyCompleted += new EventHandler<Set_DQIyModule_VerifyDQIyCompletedEventArgs>(Set_DQIyModule_VerifyDQIyCompletedEvent);
            App.proxyMC.Set_DQIyModule_VerifyDQIyAsync(
                CorrAlpha,
                MixedModel,
                new In_UserInfo() { Company = m_Company, User = m_User }
                );
        }
        private void Set_DQIyModule_VerifyDQIyCompletedEvent(object sender, Set_DQIyModule_VerifyDQIyCompletedEventArgs e)
        {
            bool IsSuccess = false;
            App.proxyMC.Set_DQIyModule_VerifyDQIyCompleted -= new EventHandler<Set_DQIyModule_VerifyDQIyCompletedEventArgs>(Set_DQIyModule_VerifyDQIyCompletedEvent);

            try
            {
                m_JobID = ((Set_DQIyModule_VerifyDQIyCompletedEventArgs)e).Result;
                if (m_JobID.CompareTo("") != 0)
                {
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                    MessageBox.Show("Error: Setting DQIy Module - Verifying DQIy fail! Please try it again.");
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                MessageBox.Show("Error: " + ex.ToString());
            }

            if (IsSuccess)
            {
                // 等待執行完成
                //Shell.waitingForm.SettingSubMessage("Wait");
                Shell.waitingForm.SettingSubMessage("等待中");
                App.proxyMC.ChecktJobStateCompleted += new EventHandler<ChecktJobStateCompletedEventArgs>(Check_DQIyModule_VerifyDQIyCompletedEvent);
                App.proxyMC.ChecktJobStateAsync(
                    m_JobID,
                    new In_UserInfo() { Company = m_Company, User = m_User }
                    );
            }
            else
            {
                // 不成功就呼叫失敗處理事件
                if (ExecuteDQIyModule_VerifyDQIy_Fail != null)
                {
                    ExecuteDQIyModule_VerifyDQIy_Fail(this, null);
                }
            }
        }
        private void Check_DQIyModule_VerifyDQIyCompletedEvent(object sender, ChecktJobStateCompletedEventArgs e)
        {
            App.proxyMC.ChecktJobStateCompleted -= new EventHandler<ChecktJobStateCompletedEventArgs>(Check_DQIyModule_VerifyDQIyCompletedEvent);

            int iStep = 0; // 0: fail 1:success other: message
            string ResultMessage = "";

            try
            {
                ResultMessage = ((ChecktJobStateCompletedEventArgs)e).Result;
                if (ResultMessage.Contains("success"))
                {
                    //已經完成 可以進行取回
                    iStep = 1;
                }
                else if (ResultMessage.Contains("fail"))
                {
                    //已經失敗
                    iStep = 0;
                }
                else
                {
                    //未完成
                    iStep = 2;
                }
            }
            catch (Exception ex)
            {
                ResultMessage = ("Error: " + ex.ToString());
                iStep = 0;
            }

            if (iStep == 0)
            {              
                MessageBox.Show(ResultMessage);
                // 不成功就呼叫失敗處理事件
                if (ExecuteDQIyModule_VerifyDQIy_Fail != null)
                {
                    ExecuteDQIyModule_VerifyDQIy_Fail(this, null);
                }
            }
            else if (iStep == 1)
            {
                //Shell.waitingForm.SettingSubMessage("Get Result");
                Shell.waitingForm.SettingSubMessage("取回結果");
                App.proxyMC.Get_DQIyResult_VerifyDQIyCompleted += new EventHandler<Get_DQIyResult_VerifyDQIyCompletedEventArgs>(Get_DQIyResult_VerifyDQIyCompletedEvent);
                App.proxyMC.Get_DQIyResult_VerifyDQIyAsync(
                    m_JobID,
                    new In_UserInfo() { Company = m_Company, User = m_User }
                    );
            }
            else
            {
                Shell.waitingForm.SettingSubMessage(ResultMessage);

                Thread SleepThread = new Thread(() =>
                {
                    Thread.Sleep(4000);
                    // 需要使用UI帶進來的Dispatcher執行體來執行
                    dispatcher.BeginInvoke(delegate()
                    {
                        App.proxyMC.ChecktJobStateCompleted += new EventHandler<ChecktJobStateCompletedEventArgs>(Check_DQIyModule_VerifyDQIyCompletedEvent);
                        App.proxyMC.ChecktJobStateAsync(
                            m_JobID,
                            new In_UserInfo() { Company = m_Company, User = m_User }
                        );
                    }); 
                });
                SleepThread.Start();
            }
        }
        private void Get_DQIyResult_VerifyDQIyCompletedEvent(object sender, Get_DQIyResult_VerifyDQIyCompletedEventArgs e)
        {
            App.proxyMC.Get_DQIyResult_VerifyDQIyCompleted -= new EventHandler<Get_DQIyResult_VerifyDQIyCompletedEventArgs>(Get_DQIyResult_VerifyDQIyCompletedEvent);
            if (ExecuteDQIyModule_VerifyDQIy_Finish != null)
            {
                ExecuteDQIyModule_VerifyDQIy_Finish(this, e);
            }
        }



        //呼叫BPNNModule
        public event EventHandler ExecuteBPNNModule_Fail;
        public event EventHandler ExecuteBPNNModule_Finish;
        public void ExecuteBPNNModule(String company, String user)
        {
            m_JobID = string.Empty;
            m_Company = company;
            m_User = user;

            double MomTermRange_Min = 0.5;
            double MomTermRange_Int = 0.2;
            double MomTermRange_Max = 0.9;
            double AlphaRange_Min = 0.15;
            double AlphaRange_Int = 0.10;
            double AlphaRange_Max = 0.15;
            double EpochsRange_1 = 60;
            double EpochsRange_2 = 80;
            double EpochsRange_3 = 100;
            ObservableCollection<double> InNodesRange = new ObservableCollection<double> { 5, 6, 7, 8, 9, 10, 11 };
            string InOneByOneChoose = "Tune";
            double BPNNRefreshCounter = 3;

          //  Shell.waitingForm.SettingSubMessage("Set Value");
            Shell.waitingForm.SettingSubMessage("設置變數");
            App.proxyMC.Set_BPNNModuleCompleted += new EventHandler<Set_BPNNModuleCompletedEventArgs>(Set_BPNNModuleCompletedEvent);
            App.proxyMC.Set_BPNNModuleAsync(
                MomTermRange_Min, MomTermRange_Int, MomTermRange_Max,
                AlphaRange_Min, AlphaRange_Int, AlphaRange_Max,
                EpochsRange_1, EpochsRange_2, EpochsRange_3,
                InNodesRange, InOneByOneChoose, BPNNRefreshCounter,
                new In_UserInfo() { Company = m_Company, User = m_User }
                );
        }
        private void Set_BPNNModuleCompletedEvent(object sender, Set_BPNNModuleCompletedEventArgs e)
        {
            bool IsSuccess = false;
            App.proxyMC.Set_BPNNModuleCompleted -= new EventHandler<Set_BPNNModuleCompletedEventArgs>(Set_BPNNModuleCompletedEvent);
            try
            {
                m_JobID = ((Set_BPNNModuleCompletedEventArgs)e).Result;
                if (m_JobID.CompareTo("") != 0)
                {
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                    MessageBox.Show("Error: Building Conjecture Model: Setting BPNN Module fail! Please try it again.");
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                MessageBox.Show("Error: " + ex.ToString());
            }

            if (IsSuccess)
            {
                // 等待執行完成
              //  Shell.waitingForm.SettingSubMessage("Wait");
                Shell.waitingForm.SettingSubMessage("等待中");
                App.proxyMC.ChecktJobStateCompleted += new EventHandler<ChecktJobStateCompletedEventArgs>(Check_BPNNModuleCompletedEvent);
                App.proxyMC.ChecktJobStateAsync(
                    m_JobID,
                    new In_UserInfo() { Company = m_Company, User = m_User }
                    );
            }
            else
            {
                // 不成功就呼叫失敗處理事件
                if (ExecuteBPNNModule_Fail != null)
                {
                    ExecuteBPNNModule_Fail(this, null);
                }
            }
        }
        private void Check_BPNNModuleCompletedEvent(object sender, ChecktJobStateCompletedEventArgs e)
        {
            App.proxyMC.ChecktJobStateCompleted -= new EventHandler<ChecktJobStateCompletedEventArgs>(Check_BPNNModuleCompletedEvent);

            int iStep = 0; // 0: fail 1:success other: message
            string ResultMessage = "";

            try
            {
                ResultMessage = ((ChecktJobStateCompletedEventArgs)e).Result;
                if (ResultMessage.Contains("success"))
                {
                    //已經完成 可以進行取回
                    iStep = 1;
                }
                else if (ResultMessage.Contains("fail"))
                {
                    //已經失敗
                    iStep = 0;
                }
                else
                {
                    //未完成
                    iStep = 2;
                }
            }
            catch (Exception ex)
            {
                ResultMessage = ("Error: " + ex.ToString());
                iStep = 0;
            }

            if (iStep == 0)
            {
                MessageBox.Show(ResultMessage);
                // 不成功就呼叫失敗處理事件
                if (ExecuteBPNNModule_Fail != null)
                {
                    ExecuteBPNNModule_Fail(this, null);
                }               
            }
            else if (iStep == 1)
            {
           //     Shell.waitingForm.SettingSubMessage("Get Result");
                Shell.waitingForm.SettingSubMessage("取回結果");
                App.proxyMC.Get_BPNNResultCompleted += new EventHandler<Get_BPNNResultCompletedEventArgs>(Get_BPNNModuleCompletedEvent);
                App.proxyMC.Get_BPNNResultAsync(
                    m_JobID,
                    new In_UserInfo() { Company = m_Company, User = m_User }
                    );
            }
            else
            {
                Shell.waitingForm.SettingSubMessage(ResultMessage);
                Thread SleepThread = new Thread(() =>
                {
                    Thread.Sleep(4000);
                    // 需要使用UI帶進來的Dispatcher執行體來執行
                    dispatcher.BeginInvoke(delegate()
                    {
                        App.proxyMC.ChecktJobStateCompleted += new EventHandler<ChecktJobStateCompletedEventArgs>(Check_BPNNModuleCompletedEvent);
                        App.proxyMC.ChecktJobStateAsync(
                            m_JobID,
                            new In_UserInfo() { Company = m_Company, User = m_User }
                        );
                    }); 
                });
                SleepThread.Start();
            }
        }
        private void Get_BPNNModuleCompletedEvent(object sender, Get_BPNNResultCompletedEventArgs e)
        {
            App.proxyMC.Get_BPNNResultCompleted -= new EventHandler<Get_BPNNResultCompletedEventArgs>(Get_BPNNModuleCompletedEvent);

            if (ExecuteBPNNModule_Finish != null)
            {
                ExecuteBPNNModule_Finish(this, e);
            }
        }
 
        //呼叫MRModule
        public event EventHandler ExecuteMRModule_Fail;
        public event EventHandler ExecuteMRModule_Finish;
        public void ExecuteMRModule(String company, String user)
        {
            m_JobID = string.Empty;
            m_Company = company;
            m_User = user;

            double InMR_TSVD_Condition_Number_Criteria = 50;
            double InMR_TSVD_Energy_Ratio_Criteria = 99.5;
            double MRRefreshCounter = 3;

         //   Shell.waitingForm.SettingSubMessage("Set Value");
            Shell.waitingForm.SettingSubMessage("設置變數");
            App.proxyMC.Set_MRModuleCompleted += new EventHandler<Set_MRModuleCompletedEventArgs>(Set_MRModuleCompletedEvent);
            App.proxyMC.Set_MRModuleAsync(
                BuildModelOption.MR_ALGORITHM,
                InMR_TSVD_Condition_Number_Criteria,
                InMR_TSVD_Energy_Ratio_Criteria,
                MRRefreshCounter,
                new In_UserInfo() { Company = m_Company, User = m_User }
                );
        }
        private void Set_MRModuleCompletedEvent(object sender, Set_MRModuleCompletedEventArgs e)
        {
            bool IsSuccess = false;
            App.proxyMC.Set_MRModuleCompleted -= new EventHandler<Set_MRModuleCompletedEventArgs>(Set_MRModuleCompletedEvent);

            try
            {
                m_JobID = ((Set_MRModuleCompletedEventArgs)e).Result;
                if (m_JobID.CompareTo("") != 0)
                {
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                    MessageBox.Show("Error: Building Conjecture Model: Setting MR Module fail! Please try it again.");
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                MessageBox.Show("Error: " + ex.ToString());
            }

            if (IsSuccess)
            {
                // 等待執行完成
               // Shell.waitingForm.SettingSubMessage("Wait");
                Shell.waitingForm.SettingSubMessage("等待中");
                App.proxyMC.ChecktJobStateCompleted += new EventHandler<ChecktJobStateCompletedEventArgs>(Check_MRModuleCompletedEvent);
                App.proxyMC.ChecktJobStateAsync(
                    m_JobID,
                    new In_UserInfo() { Company = m_Company, User = m_User }
                    );
            }
            else
            {
                // 不成功就呼叫失敗處理事件
                if (ExecuteMRModule_Fail != null)
                {
                    ExecuteMRModule_Fail(this, null);
                }
            }
        }
        private void Check_MRModuleCompletedEvent(object sender, ChecktJobStateCompletedEventArgs e)
        {
            App.proxyMC.ChecktJobStateCompleted -= new EventHandler<ChecktJobStateCompletedEventArgs>(Check_MRModuleCompletedEvent);

            int iStep = 0; // 0: fail 1:success other: message
            string ResultMessage = "";

            try
            {
                ResultMessage = ((ChecktJobStateCompletedEventArgs)e).Result;
                if (ResultMessage.Contains("success"))
                {
                    //已經完成 可以進行取回
                    iStep = 1;
                }
                else if (ResultMessage.Contains("fail"))
                {
                    //已經失敗
                    iStep = 0;
                }
                else
                {
                    //未完成
                    iStep = 2;
                }
            }
            catch (Exception ex)
            {
                ResultMessage = ("Error: " + ex.ToString());
                iStep = 0;
            }

            if (iStep == 0)
            {
                MessageBox.Show(ResultMessage);
                // 不成功就呼叫失敗處理事件
                if (ExecuteMRModule_Fail != null)
                {
                    ExecuteMRModule_Fail(this, null);
                } 
            }
            else if (iStep == 1)
            {
               // Shell.waitingForm.SettingSubMessage("Get Result");
                Shell.waitingForm.SettingSubMessage("取回結果");
                App.proxyMC.Get_MRResultCompleted += new EventHandler<Get_MRResultCompletedEventArgs>(Get_MRModuleCompletedEvent);
                App.proxyMC.Get_MRResultAsync(
                    m_JobID,
                    new In_UserInfo() { Company = m_Company, User = m_User }
                    );
            }
            else
            {
                Shell.waitingForm.SettingSubMessage(ResultMessage);
                Thread SleepThread = new Thread(() =>
                {
                    Thread.Sleep(4000);
                    // 需要使用UI帶進來的Dispatcher執行體來執行
                    dispatcher.BeginInvoke(delegate()
                    {
                        App.proxyMC.ChecktJobStateCompleted += new EventHandler<ChecktJobStateCompletedEventArgs>(Check_MRModuleCompletedEvent);
                        App.proxyMC.ChecktJobStateAsync(
                            m_JobID,
                            new In_UserInfo() { Company = m_Company, User = m_User }
                        );
                    }); 
                });
                SleepThread.Start();
            }
        }
        private void Get_MRModuleCompletedEvent(object sender, Get_MRResultCompletedEventArgs e)
        {
            App.proxyMC.Get_MRResultCompleted -= new EventHandler<Get_MRResultCompletedEventArgs>(Get_MRModuleCompletedEvent);

            if (ExecuteMRModule_Finish != null)
            {
                ExecuteMRModule_Finish(this, e);
            }
        }

        //呼叫RIModule
        public event EventHandler ExecuteRIModule_Fail;
        public event EventHandler ExecuteRIModule_Finish;
        public void ExecuteRIModule(
            bool FirstAlgoValue, String FirstAlgoName, ObservableCollection<ObservableCollection<double>> FirstPredictValue, 
            bool SecondAlgoValue, String SecondAlgoName, ObservableCollection<ObservableCollection<double>> SecondPredictValue,
            String company, String user)
        {
            m_JobID = string.Empty;
            m_Company = company;
            m_User = user;

            double Tolerant_MaxError = 7;
            string SelectCalculator = "ThresholdAutoSetting";

          //  Shell.waitingForm.SettingSubMessage("Set Value");
            Shell.waitingForm.SettingSubMessage("設置變數");
            App.proxyMC.Set_RIModuleCompleted += new EventHandler<Set_RIModuleCompletedEventArgs>(Set_RIModuleCompletedEvent);
            App.proxyMC.Set_RIModuleAsync(
                FirstAlgoValue,
                FirstAlgoName,
                FirstPredictValue,
                SecondAlgoValue,
                SecondAlgoName,
                SecondPredictValue,
                Tolerant_MaxError,
                SelectCalculator,
                new In_UserInfo() { Company = m_Company, User = m_User }
                );
        }
        private void Set_RIModuleCompletedEvent(object sender, Set_RIModuleCompletedEventArgs e)
        {
            bool IsSuccess = false;
            App.proxyMC.Set_RIModuleCompleted -= new EventHandler<Set_RIModuleCompletedEventArgs>(Set_RIModuleCompletedEvent);

            try
            {
                m_JobID = ((Set_RIModuleCompletedEventArgs)e).Result;
                if (m_JobID.CompareTo("") != 0)
                {
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                    MessageBox.Show("Error: Building Conjecture Model: Setting RI Module fail! Please try it again.");
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                MessageBox.Show("Error: " + ex.ToString());
            }

            if (IsSuccess)
            {
                // 等待執行完成
               // Shell.waitingForm.SettingSubMessage("Wait");
                Shell.waitingForm.SettingSubMessage("等待中");
                App.proxyMC.ChecktJobStateCompleted += new EventHandler<ChecktJobStateCompletedEventArgs>(Check_RIModuleCompletedEvent);
                App.proxyMC.ChecktJobStateAsync(
                    m_JobID,
                    new In_UserInfo() { Company = m_Company, User = m_User }
                    );
            }
            else
            {
                // 不成功就呼叫失敗處理事件
                if (ExecuteRIModule_Fail != null)
                {
                    ExecuteRIModule_Fail(this, null);
                }
            }
        }
        private void Check_RIModuleCompletedEvent(object sender, ChecktJobStateCompletedEventArgs e)
        {
            App.proxyMC.ChecktJobStateCompleted -= new EventHandler<ChecktJobStateCompletedEventArgs>(Check_RIModuleCompletedEvent);

            int iStep = 0; // 0: fail 1:success other: message
            string ResultMessage = "";

            try
            {
                ResultMessage = ((ChecktJobStateCompletedEventArgs)e).Result;
                if (ResultMessage.Contains("success"))
                {
                    //已經完成 可以進行取回
                    iStep = 1;
                }
                else if (ResultMessage.Contains("fail"))
                {
                    //已經失敗
                    iStep = 0;
                }
                else
                {
                    //未完成
                    iStep = 2;
                }
            }
            catch (Exception ex)
            {
                ResultMessage = ("Error: " + ex.ToString());
                iStep = 0;
            }

            if (iStep == 0)
            {
                MessageBox.Show(ResultMessage);
                // 不成功就呼叫失敗處理事件
                if (ExecuteRIModule_Fail != null)
                {
                    ExecuteRIModule_Fail(this, null);
                }  
            }
            else if (iStep == 1)
            {
              //  Shell.waitingForm.SettingSubMessage("Get Result");
                Shell.waitingForm.SettingSubMessage("等待結果");
                App.proxyMC.Get_RIResultCompleted += new EventHandler<Get_RIResultCompletedEventArgs>(Get_RIModuleCompletedEvent);
                App.proxyMC.Get_RIResultAsync(
                    m_JobID,
                    new In_UserInfo() { Company = m_Company, User = m_User }
                    );
            }
            else
            {
                Shell.waitingForm.SettingSubMessage(ResultMessage);

                Thread SleepThread = new Thread(() =>
                {
                    Thread.Sleep(4000);
                    // 需要使用UI帶進來的Dispatcher執行體來執行
                    dispatcher.BeginInvoke(delegate()
                    {
                        App.proxyMC.ChecktJobStateCompleted += new EventHandler<ChecktJobStateCompletedEventArgs>(Check_RIModuleCompletedEvent);
                        App.proxyMC.ChecktJobStateAsync(
                            m_JobID,
                            new In_UserInfo() { Company = m_Company, User = m_User }
                        );
                    }); 
                });
                SleepThread.Start();
            }
        }
        private void Get_RIModuleCompletedEvent(object sender, Get_RIResultCompletedEventArgs e)
        {
            App.proxyMC.Get_RIResultCompleted -= new EventHandler<Get_RIResultCompletedEventArgs>(Get_RIModuleCompletedEvent);

            if (ExecuteRIModule_Finish != null)
            {
                ExecuteRIModule_Finish(this, e);
            }
        }

        //呼叫GSIModule
        public event EventHandler ExecuteGSIModule_Fail;
        public event EventHandler ExecuteGSIModule_Finish;
        public void ExecuteGSIModule(int numberOfGroup, String iInSelectAlgorithm, String company, String user)
        {
            m_JobID = string.Empty;
            m_Company = company;
            m_User = user;

            double GSI_RT = 0.7;
            double GSI_Threshold = 0.7;
            double GSI_RefreshCounter = 3;
            double InGSI_TSVD_Condition_Number_Criteria = 50;
            double InGSI_TSVD_Energy_Ratio_Criteria = 99.5;

            //Shell.waitingForm.SettingSubMessage("Set Value");
            Shell.waitingForm.SettingSubMessage("設置變數");
            App.proxyMC.Set_GSIModuleCompleted += new EventHandler<Set_GSIModuleCompletedEventArgs>(Set_GSIModuleCompletedEvent);
            App.proxyMC.Set_GSIModuleAsync(
                numberOfGroup,
                iInSelectAlgorithm,
                GSI_RT,
                GSI_Threshold,
                GSI_RefreshCounter,
                InGSI_TSVD_Condition_Number_Criteria,
                InGSI_TSVD_Energy_Ratio_Criteria,
                new In_UserInfo() { Company = m_Company, User = m_User }
                );
        }
        private void Set_GSIModuleCompletedEvent(object sender, Set_GSIModuleCompletedEventArgs e)
        {
            bool IsSuccess = false;
            App.proxyMC.Set_GSIModuleCompleted -= new EventHandler<Set_GSIModuleCompletedEventArgs>(Set_GSIModuleCompletedEvent);

            try
            {
                m_JobID = ((Set_GSIModuleCompletedEventArgs)e).Result;
                if (m_JobID.CompareTo("") != 0)
                {
                    IsSuccess = true;
                }
                else
                {
                    IsSuccess = false;
                    MessageBox.Show("Error: Building Conjecture Model: Setting GSI Module fail! Please try it again.");
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;
                MessageBox.Show("Error: " + ex.ToString());
            }

            if (IsSuccess)
            {
                // 等待執行完成
               // Shell.waitingForm.SettingSubMessage("Wait");
                Shell.waitingForm.SettingSubMessage("等待中");
                App.proxyMC.ChecktJobStateCompleted += new EventHandler<ChecktJobStateCompletedEventArgs>(Check_GSIModuleCompletedEvent);
                App.proxyMC.ChecktJobStateAsync(
                    m_JobID,
                    new In_UserInfo() { Company = m_Company, User = m_User }
                    );
            }
            else
            {
                // 不成功就呼叫失敗處理事件
                if (ExecuteGSIModule_Fail != null)
                {
                    ExecuteGSIModule_Fail(this, null);
                }
            }
        }
        private void Check_GSIModuleCompletedEvent(object sender, ChecktJobStateCompletedEventArgs e)
        {
            App.proxyMC.ChecktJobStateCompleted -= new EventHandler<ChecktJobStateCompletedEventArgs>(Check_GSIModuleCompletedEvent);

            int iStep = 0; // 0: fail 1:success other: message
            string ResultMessage = "";

            try
            {
                ResultMessage = ((ChecktJobStateCompletedEventArgs)e).Result;
                if (ResultMessage.Contains("success"))
                {
                    //已經完成 可以進行取回
                    iStep = 1;
                }
                else if (ResultMessage.Contains("fail"))
                {
                    //已經失敗
                    iStep = 0;
                }
                else
                {
                    //未完成
                    iStep = 2;
                }
            }
            catch (Exception ex)
            {
                ResultMessage = ("Error: " + ex.ToString());
                iStep = 0;
            }

            if (iStep == 0)
            {
                MessageBox.Show(ResultMessage);
                // 不成功就呼叫失敗處理事件
                if (ExecuteGSIModule_Fail != null)
                {
                    ExecuteGSIModule_Fail(this, null);
                }    
            }
            else if (iStep == 1)
            {
               // Shell.waitingForm.SettingSubMessage("Get Result");
                Shell.waitingForm.SettingSubMessage("取回結果");
                App.proxyMC.Get_GSIResultCompleted += new EventHandler<Get_GSIResultCompletedEventArgs>(Get_GSIModuleCompletedEvent);
                App.proxyMC.Get_GSIResultAsync(
                    m_JobID,
                    new In_UserInfo() { Company = m_Company, User = m_User }
                    );
            }
            else
            {
                Shell.waitingForm.SettingSubMessage(ResultMessage);
                Thread SleepThread = new Thread(() =>
                {
                    Thread.Sleep(4000);
                    // 需要使用UI帶進來的Dispatcher執行體來執行
                    dispatcher.BeginInvoke(delegate()
                    {
                        App.proxyMC.ChecktJobStateCompleted += new EventHandler<ChecktJobStateCompletedEventArgs>(Check_GSIModuleCompletedEvent);
                        App.proxyMC.ChecktJobStateAsync(
                            m_JobID,
                            new In_UserInfo() { Company = m_Company, User = m_User }
                        );
                    }); 
                });
                SleepThread.Start();
            }
        }
        private void Get_GSIModuleCompletedEvent(object sender, Get_GSIResultCompletedEventArgs e)
        {
            App.proxyMC.Get_GSIResultCompleted -= new EventHandler<Get_GSIResultCompletedEventArgs>(Get_GSIModuleCompletedEvent);
            if (ExecuteGSIModule_Finish != null)
            {
                ExecuteGSIModule_Finish(this, e);
            }
        }
    }
}
