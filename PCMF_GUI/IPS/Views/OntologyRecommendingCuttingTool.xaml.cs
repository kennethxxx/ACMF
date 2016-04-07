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
using System.IO;
using IPS.Comm;
using IPS.Common;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Json;
using OMC.Comm;
using System.Collections;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Windows.Data;
using System.Globalization;
using System.Reflection;
using System.Threading;
using IPS.BPELInvoker;


namespace IPS.Views
{
    public partial class OntologyRecommendingCuttingTool : UserControl
    {
        OpenFileDialog dlgleft, dlgright;
        FileInfo left_fileInfo, right_fileInfo;
        FileStream left_fs, right_fs;
        OntologyResultForNCFile objDept;
        int left_offset, right_offset;
        long left_newOffset, right_newOffset;
        int BUFFER_SIZE = 1024 * 16;

        string inferJson = "";
        string inferColl = "";
        string jsonStr = "";
        string leftturretchoose = "";
        string rightturretchoose = "";
        static string DriverOption = "";
        ObservableCollection<OntologyResultForNCFile> InferNC2 = new ObservableCollection<OntologyResultForNCFile>();
        ObservableCollection<OntologyResultForNCFile> AllList = new ObservableCollection<OntologyResultForNCFile>();
        ObservableCollection<OntologyResultForNCFile> InferNC;
        ObservableCollection<OntologyResultForNCFile> LinferJsonString_After;
        ObservableCollection<OntologyResultForNCFile> RinferJsonString_After;
        OntologyResultForNCFile objNCInfoL;
        OntologyResultForNCFile objNCInfoR;
        public OntologyRecommendingCuttingTool()
        {

            InitializeComponent();           
            LB_Info1.Text = "Input Related Parameters";
            LB_Info2.Text = "Select Proper Cutting Tools";
            LB_Info3.Text = "Collision Detection of VMT Service";
            LB_Info4.Text = "Confirm NC Files before Sending to the Machine Tool";
            InferNC2.Clear();
            AllList.Clear();
            GetDriverOption();                    
        }

        #region TabControl切換的狀態管控

        private void Control_TabItemChange(int Step)
        {
            switch (Step)
            {
                case 2:
                    TBSelectFavoriteCT.IsEnabled = true;
                    TBSelectFavoriteCT.IsSelected = true;
                    break;
                case 3:
                    TBGetVMTModule.IsEnabled = true;
                    TBGetVMTModule.IsSelected = true;
                    break;
                case 4:
                    TBRealMachining.IsEnabled = true;
                    TBRealMachining.IsSelected = true;
                    break;
            }
        }

        #endregion

        #region 打包成Json格式字串Function
        /// <summary>
        ///Class Object type to json format Ref.:http://msdn.microsoft.com/zh-tw/library/cc197957(v=vs.95).aspx
        ///http://www.dotblogs.com.tw/junegoat/archive/2012/04/30/asp-net-json-javascriptserializer-serialization.aspx
        /// </summary>
        /// <param name="index"></param>
        /// <param name="JsonStr"></param>
        private void ResolveXMLFormat(int index, string JsonStr)
        {
            switch (index)
            {
                case 0:
                    //知識庫資訊 Json反序列化
                    foreach (OntologyDatabaseData _CBOntolgoy in ParseJsonFormatClass.Get_KnowledgeBase(JsonStr))
                    {
                       CBOntolgoy.Items.Add(_CBOntolgoy.OntologyList_name);
                    }
                    break;
                case 1:
                    //規則資訊 Json反序列化
                    ObservableCollection<PE_SelectRulesCT> ruleitemsListSource = new ObservableCollection<PE_SelectRulesCT>();
                    foreach (PE_SelectRulesCT _Rule in ParseJsonFormatClass.Get_Rule(JsonStr))
                    {
                        PE_SelectRulesCT _PESelectListParse =  new PE_SelectRulesCT();
                        _PESelectListParse.Rule_Select = true;
                        _PESelectListParse.idrules= _Rule.idrules;
                        _PESelectListParse.rules_name = _Rule.rules_name;
                        _PESelectListParse.rules_description = _Rule.rules_description;
                        _PESelectListParse.rules = _Rule.rules;
                        ruleitemsListSource.Add(_PESelectListParse);
                    }
                    DGSelectRule.ItemsSource = ruleitemsListSource;
                    break;
                case 2:                    
                    //碰撞結果
                    foreach (VECollisionInfo _result in ParseJsonFormatClass.Get_CollisionResult(JsonStr))
                    {
                        if (_result.FileName.Equals("O0512"))
                        {
                            DG_leftCTNumber.ItemsSource = new ObservableCollection<SelectCuttingToolCollision>(
                                                          from chooseL in _result.CuttingToolList
                                                          select chooseL);
                        }
                        else
                        {
                            DG_rightCTNumber.ItemsSource = new ObservableCollection<SelectCuttingToolCollision>(
                                                          from chooseR in _result.CuttingToolList
                                                          select chooseR);
                        }
                        if (_result.CollisionDetection == "true")
                        {
                            TB_CollisionResult.Foreground = new SolidColorBrush(Colors.Red);
                            TB_CollisionResult.Text = "collision";
                            btnLastToChangeTool.Visibility = System.Windows.Visibility.Visible;
                            btnNextToRealMachining.Visibility = System.Windows.Visibility.Collapsed;
                            this.ImgCollisionResult.Source = new BitmapImage(new Uri("/IPS;component/Images/not-ok-icon.jpg", UriKind.Relative));
                        }
                        else
                        {
                            TB_CollisionResult.Foreground = new SolidColorBrush(Colors.Blue);
                            TB_CollisionResult.Text = "finished";
                            btnLastToChangeTool.Visibility = System.Windows.Visibility.Collapsed;
                            btnNextToRealMachining.Visibility = System.Windows.Visibility.Visible;
                            this.ImgCollisionResult.Source = new BitmapImage(new Uri("/IPS;component/Images/ok-icon.jpg", UriKind.Relative));
                        }
                        TB_TimeResult.Text = _result.MachiningTime;
                        TB_TimeResult.Foreground = new SolidColorBrush(Colors.Blue);                       
                    }
                    break;
                case 3:
                    //推論結果
                    InferNC = ParseJsonFormatClass.Get_InferenceResult(JsonStr);
                    foreach (OntologyResultForNCFile _result in InferNC)
                    {
                        if (_result.FileName.Equals("O0512"))
                        {
                            ObservableCollection<OntologyResultForNCFile> LinferJsonString = new ObservableCollection<OntologyResultForNCFile>(
                                                          from chooseL in InferNC
                                                          where chooseL.FileName.Equals("O0512")
                                                          orderby chooseL.StageNo
                                                          select chooseL);
                            PagedCollectionView LList = new PagedCollectionView(LinferJsonString);
                            LList.GroupDescriptions.Add(new PropertyGroupDescription("StageNo"));
                            DBInferenceCuttingToolResult_left.ItemsSource = LList;
                            DBInferenceCuttingToolResult_left.LoadingRow += new EventHandler<DataGridRowEventArgs>(DBCahngeBackgroud_LoadingRow);
                        }
                        else if (_result.FileName.Equals("O0511"))
                        {
                            ObservableCollection<OntologyResultForNCFile> RinferJsonString = new ObservableCollection<OntologyResultForNCFile>(
                                                          from chooseR in InferNC
                                                          where chooseR.FileName.Equals("O0511")
                                                          orderby chooseR.StageNo
                                                          select chooseR);
                            PagedCollectionView RList = new PagedCollectionView(RinferJsonString);
                            RList.GroupDescriptions.Add(new PropertyGroupDescription("StageNo"));
                            DBInferenceCuttingToolResult_right.ItemsSource = RList;
                            DBInferenceCuttingToolResult_right.LoadingRow += new EventHandler<DataGridRowEventArgs>(DBCahngeBackgroud_LoadingRow);
                        } 
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Step1: 製程工程師選擇客製化參數

        #region UPload File
        private void btnUpload_LeftNCFile_Click(object sender, RoutedEventArgs e)
        {
            dlgleft = new OpenFileDialog();
            dlgleft.Multiselect = false;
            //這邊可以設定過副檔名過濾器
            //dlgleft.Filter = "NC檔 (*.nc)|*.nc";

            if (dlgleft.ShowDialog() ?? false)
            {
                left_offset = 0;
                left_newOffset = 0;
                left_fileInfo = dlgleft.File;
                TBUpload_NCFilenameLT.Text = dlgleft.File.Name;                
            }
        }

        private void btnUpload_RightNCFile_Click(object sender, RoutedEventArgs e)
        {
            dlgright = new OpenFileDialog();
            dlgright.Multiselect = false;
            //這邊可以設定過副檔名過濾器
            //dlgright.Filter = "NC檔 (*.nc)|*.nc";

            if (dlgright.ShowDialog() ?? false)
            {
                right_offset = 0;
                right_newOffset = 0;
                right_fileInfo = dlgright.File;
                TBUpload_NCFilenameRT.Text = dlgright.File.Name;
            }
        }

        #region DriverOption
        private void GetDriverOption()
        {
            Shell.waitingForm.SettingMessage("Connecting to Service");
            Shell.waitingForm.Show();
            IPS.RecommendedCuttingTool.CuttingToolInterfaceClient proxyCuttingTool = new IPS.RecommendedCuttingTool.CuttingToolInterfaceClient();
            proxyCuttingTool.GetDriverOptionCompleted += (s, e) =>
            {
                if (e.Error == null)
                {
                    try
                    {
                        if (e.Error == null && e.Result != null)
                        {                          
                            DriverOption = e.Result;
                            call_GetknowledgeBaseService();                         
                        }
                        else
                        {
                            MessageBox.Show("Service is busying now!");
                        }
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
                else
                {
                    MessageBox.Show(e.Error.ToString());
                }
            };
            proxyCuttingTool.GetDriverOptionAsync();
            proxyCuttingTool.CloseAsync();
        }
        #endregion

        #region PrivateUplaodfile
        private void PrivateUploadFile(string UploadPath,string TargetPath)
        {
            IPS.RecommendedCuttingTool.CuttingToolInterfaceClient proxyCuttingTool = new IPS.RecommendedCuttingTool.CuttingToolInterfaceClient();
            proxyCuttingTool.PriavateUploadCompleted += (s, e) =>
            {
                if (e.Error == null)
                {
                    //MessageBox.Show(e.Result);
                }
                else
                {
                    MessageBox.Show(e.Error.ToString());
                }

                //Shell.waitingForm.DialogResult = false;
            };
            proxyCuttingTool.PriavateUploadAsync(UploadPath, TargetPath);
            proxyCuttingTool.CloseAsync();
        }

        #endregion

        #region file1
        private void UploadFile(string filename)
        {
            IPS.RecommendedCuttingTool.CuttingToolInterfaceClient proxyCuttingTool = new IPS.RecommendedCuttingTool.CuttingToolInterfaceClient();
            proxyCuttingTool.uploadBlob2Completed += (s, e) =>
            {
                if (e.Error == null)
                {
                    UploadFile2(TBUpload_NCFilenameRT.Text);
                    //MessageBox.Show(e.Result);
                }
                else
                {
                    MessageBox.Show(e.Error.ToString());
                }

                //Shell.waitingForm.DialogResult = false;
            };

            proxyCuttingTool.uploadBlob2Async(filename);
            proxyCuttingTool.CloseAsync();
        }

        #endregion

        #region file2
        private void UploadFile2(string filename)
        {
            IPS.RecommendedCuttingTool.CuttingToolInterfaceClient proxyCuttingTool = new IPS.RecommendedCuttingTool.CuttingToolInterfaceClient();
            proxyCuttingTool.uploadBlob2Completed += (s, e) =>
            {
                if (e.Error == null)
                {
                    switch (DriverOption)
                    {
                        case "Public":
                            UploadFile(TBUpload_NCFilenameLT.Text);
                            UploadFile2(TBUpload_NCFilenameRT.Text);
                            break;
                        case "Private":
                            PrivateUploadFile(@"D://FileUpload/"+TBUpload_NCFilenameLT.Text,@"D://ontology/"+TBUpload_NCFilenameLT.Text);
                            PrivateUploadFile(@"D://FileUpload/" + TBUpload_NCFilenameRT.Text, @"D://ontology/" + TBUpload_NCFilenameRT.Text);
                            break;
                        default: break;
                    }

                    List<PE_SelectRulesCT> _selectruleList = new List<PE_SelectRulesCT>();

                    IEnumerator ppEnum = DGSelectRule.ItemsSource.GetEnumerator();

                    while (ppEnum.MoveNext())
                    {
                        _selectruleList.Add((PE_SelectRulesCT)ppEnum.Current);
                    }

                    //JSON 打包製程工程師輸入的所有參數
                    PEParameters _Parameter = new PEParameters();
                    if (DriverOption == "Public")
                    {
                        _Parameter.LeftTurretNCName = "https://portalvhdsd32zr3v7k67p3.blob.core.windows.net/ontology/" + TBUpload_NCFilenameLT.Text;
                        _Parameter.RighttTurretNCName = "https://portalvhdsd32zr3v7k67p3.blob.core.windows.net/ontology/" + TBUpload_NCFilenameRT.Text;
                    }
                    else if (DriverOption == "Private")
                    {
                        _Parameter.LeftTurretNCName = "https://portalvhdsd32zr3v7k67p3.blob.core.windows.net/ontology/" + TBUpload_NCFilenameLT.Text;
                        _Parameter.RighttTurretNCName = "https://portalvhdsd32zr3v7k67p3.blob.core.windows.net/ontology/" + TBUpload_NCFilenameRT.Text;
                    }
                    _Parameter.WorkPieceRadiusDiffer = TB_WPRadiusReduce.Text;
                    _Parameter.WheelInnerRadiu = TB_WheelsInnerRadiu.Text;
                    List<OntologyDatabaseData> newFactory = new List<OntologyDatabaseData>{
                        new OntologyDatabaseData() { OntologyList_id = null, OntologyList_name = CBOntolgoy.SelectedItem.ToString(), Ontology_time = null }
                    };
                    _Parameter.KnowledgeBaseData = newFactory;
                    _Parameter.SelectRuleList = _selectruleList;

                    MemoryStream ms = new MemoryStream();
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(PEParameters));
                    serializer.WriteObject(ms, _Parameter);
                    byte[] json = ms.ToArray();
                    ms.Close();

                    string toJson = Encoding.UTF8.GetString(json, 0, json.Length);
                    textBox1.Text = Encoding.UTF8.GetString(json, 0, json.Length);
                    //連接服務並進行刀具選用推論
                    if (CBOntolgoy.SelectedItem.ToString() != null)
                    {
                        Service1Client bpelclient = new Service1Client();
                        bpelclient.InvokeBPELCompleted += (sr, er) =>
                        {
                            try
                            {
                                if (er.Error == null & er.Result != null)
                                {
                                    //textBox1.Text = er.Result.result;
                                    IPS.RecommendedCuttingTool.CuttingToolInterfaceClient proxyCuttingTool2 = new RecommendedCuttingTool.CuttingToolInterfaceClient();
                                    proxyCuttingTool2.ConvertJsonFunctionCompleted += (sedb, erdb) =>
                                    {
                                        if (erdb.Error == null & erdb.Result != null)
                                        {
                                            textBox1.Text = erdb.Result;
                                            inferJson = er.Result.ToString();
                                            ResolveXMLFormat(3, inferJson);
                                            Shell.waitingForm.Close();
                                        }
                                        else
                                        {
                                            MessageBox.Show("Service is busying now!");
                                            Shell.waitingForm.Close();
                                        }
                                    };
                                    proxyCuttingTool2.ConvertJsonFunctionAsync("Step3_CallRecommandedUseCuttingToolService",er.Result.ToString());
                                    proxyCuttingTool2.CloseAsync();

                                }
                                else
                                {
                                    MessageBox.Show("Service is busying now!");
                                    Shell.waitingForm.Close();
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }

                        };
                        bpelclient.InvokeBPELAsync(StateManager.Username, "InferenceForCuttingTool", toJson, DriverOption);
                        bpelclient.CloseAsync();
                    }
                }
                else
                {
                    MessageBox.Show(e.Error.ToString());
                }

                //Shell.waitingForm.DialogResult = false;
            };

            proxyCuttingTool.uploadBlob2Async(filename);
            proxyCuttingTool.CloseAsync();
        }
        #endregion
        #endregion
        /**
         * 取得刀具規則
         */ 
        private void CBOntolgoy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string step2parameterjson = @"[{'selectdb':'swrl_rule','selecttable':'rule_cuttingtool_table'}]";

            //讀取規則資料
            call_GetknowledgeRuleService(step2parameterjson);
        }

        private void call_GetknowledgeBaseService()
        {
            Shell.waitingForm.SettingMessage("Connecting to Service");
            Shell.waitingForm.Show();
            //將資料庫的OntologyDB取出
            
            //BPEL服務
            Service1Client bpelclient = new Service1Client();
            bpelclient.InvokeBPELCompleted += (s, e) =>
            {
                if (e.Error == null)
                {
                    try
                    {
                        if (e.Error == null && e.Result != null)
                        {
                            ResolveXMLFormat(0, e.Result.ToString());
                            Shell.waitingForm.Close();
                        }
                        else
                        {
                            MessageBox.Show("Service is busying now!");
                            Shell.waitingForm.Close();
                        }
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
                else
                {
                    MessageBox.Show(e.Error.ToString());
                }

                Shell.waitingForm.DialogResult = false;
            };
            bpelclient.InvokeBPELAsync(StateManager.Username, "CallOntologyDataBase", "data", DriverOption);
            bpelclient.CloseAsync();
        }

        private void call_GetknowledgeRuleService(string  step2parameterjson)
        {
            if (CBOntolgoy.SelectedItem.ToString() != null)
            {
                Shell.waitingForm.SettingMessage("Connecting to Service");
                Shell.waitingForm.Show();

                Service1Client bpelclient = new Service1Client();
                bpelclient.InvokeBPELCompleted += (s, e) =>
                {
                    try
                    {
                        if (e.Error == null & e.Result != null)
                        {
                            //須解析e.result格式之後，才能變String
                            textBox1.Text = e.Result.ToString();
                            ResolveXMLFormat(1, e.Result.ToString());
                            Shell.waitingForm.Close();
                        }
                        else
                        {
                            MessageBox.Show("Service is busying now!");
                            Shell.waitingForm.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                };
                bpelclient.InvokeBPELAsync(StateManager.Username, "CallOntologyRuleDataBase", step2parameterjson, DriverOption);
                bpelclient.CloseAsync();
            }
        }

        private void btnNextToInferenceResult_Click(object sender, RoutedEventArgs e)
        {
            Shell.waitingForm.SettingMessage("Ontology Inferring");
            Shell.waitingForm.Show();

            TB_leftNCName.Text = TBUpload_NCFilenameLT.Text;
            TB_rightNCName.Text = TBUpload_NCFilenameRT.Text;
            Control_TabItemChange(2);

            UploadFile(TBUpload_NCFilenameLT.Text);
        }
        #endregion

        #region Step2: 推論後適用的替換刀具
       
        private void btnNextToCollision_Click(object sender, RoutedEventArgs e)
        {
            TB_ShowuploadLeftNCName.Text = TBUpload_NCFilenameLT.Text;
            TB_ShowuploadRightNCName.Text = TBUpload_NCFilenameRT.Text;

            Control_TabItemChange(3);

            #region 進行碰撞檢測與加工時間估算
            Shell.waitingForm.SettingMessage("Collision Detecting");
            Shell.waitingForm.Show();

            //先呼叫WCF取得資料庫選取的資料再進行VMT BPEL呼叫
            IPS.RecommendedCuttingTool.CuttingToolInterfaceClient proxyCuttingTool = new IPS.RecommendedCuttingTool.CuttingToolInterfaceClient();
            proxyCuttingTool.ConvertJsonFunctionCompleted += (srdb, erdb) =>
            {
                if (erdb.Error == null && erdb.Result != null)
                {
                    textBox1.Text = erdb.Result;

                    //BPEL服務
                    Service1Client bpelclient = new Service1Client();
                    bpelclient.InvokeBPELCompleted += (s, er) =>
                    {
                        try
                        {
                            if (er.Error == null && er.Result != null)
                            {
                                inferColl = er.Result.ToString();
                                ResolveXMLFormat(2, inferColl);
                                Shell.waitingForm.Close();
                            }
                            else
                            {
                                MessageBox.Show("Service is busying now!");
                                Shell.waitingForm.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    };
                    bpelclient.InvokeBPELAsync(StateManager.Username, "CallVMTService", erdb.Result, DriverOption);
                    bpelclient.CloseAsync();
                }
                else
                {
                    MessageBox.Show("Service is busying now!");
                    Shell.waitingForm.Close();
                }
            };
            proxyCuttingTool.ConvertJsonFunctionAsync("Step4_CallVMTService","");
            proxyCuttingTool.CloseAsync();
            #endregion
        }

        #region 推論後的刀具點選的button
        private void btnShowLeftDetails_Click(object sender, RoutedEventArgs e)
        {
            objNCInfoL = (OntologyResultForNCFile)((Button)sender).DataContext;
            //取得資料庫最新資料，並且顯示到datagrid中
            Shell.waitingForm.SettingMessage("Connecting to service");
            Shell.waitingForm.Show();

            IPS.RecommendedCuttingTool.CuttingToolInterfaceClient proxyCuttingTool = new IPS.RecommendedCuttingTool.CuttingToolInterfaceClient();
            proxyCuttingTool.getInferSelectStatusCompleted += (s, er) =>
            {
                if (er.Error == null & er.Result != null)
                {
                    ObservableCollection<OntologyResultForNCFile> AllList2 = new ObservableCollection<OntologyResultForNCFile>();
                    jsonStr = er.Result;
                    textBox1.Text = jsonStr;
                    //取得目前的刀具，推論有哪些刀
                    AllList = OntologyResultForNCFile.get_String(objNCInfoL, jsonStr);
                    AllList2 = OrderByClass.GetOrderby(AllList);
                    foreach (var i in AllList2)
                    {
                        DG_ReplaceToolNReasion.ItemsSource = new ObservableCollection<OntologyResultForCuttingTool>(
                               from xlist in i.ReplaceableRuleSet
                               where i.FileName.Equals("O0512")
                               orderby xlist.sortindex
                               select xlist
                           );
                        DG_ReplaceToolNReasion.LoadingRow += new EventHandler<DataGridRowEventArgs>(DBCahngeBackgroudAfter_LoadingRow);
                    }

                    Shell.waitingForm.Close();
                }
                else
                {
                    MessageBox.Show("Service is busying now!");
                    Shell.waitingForm.Close();
                }
            };
            proxyCuttingTool.getInferSelectStatusAsync();
            proxyCuttingTool.CloseAsync();
        }

        private void btnShowRightDetails_Click(object sender, RoutedEventArgs e)
        {
            objNCInfoR = (OntologyResultForNCFile)((Button)sender).DataContext;

            //取得資料庫最新資料，並且顯示到datagrid中
            Shell.waitingForm.SettingMessage("Connecting to service");
            Shell.waitingForm.Show();
            IPS.RecommendedCuttingTool.CuttingToolInterfaceClient proxyCuttingTool = new IPS.RecommendedCuttingTool.CuttingToolInterfaceClient();
            proxyCuttingTool.getInferSelectStatusCompleted += (s, er) =>
            {
                if (er.Error == null & er.Result != null)
                {
                    ObservableCollection<OntologyResultForNCFile> AllList2 = new ObservableCollection<OntologyResultForNCFile>();
                    jsonStr = er.Result;
                    textBox1.Text = jsonStr;
                    //取得目前的刀具，推論有哪些刀
                    AllList = OntologyResultForNCFile.get_String(objNCInfoR, jsonStr);
                    AllList2 = OrderByClass.GetOrderby(AllList);
                    foreach (var i in AllList2)
                    {
                        DG_ReplaceToolNReasion.ItemsSource = new ObservableCollection<OntologyResultForCuttingTool>(
                               from xlist in i.ReplaceableRuleSet
                               where i.FileName.Equals("O0511")
                               orderby xlist.sortindex
                               select xlist
                           );
                        DG_ReplaceToolNReasion.LoadingRow += new EventHandler<DataGridRowEventArgs>(DBCahngeBackgroudAfter_LoadingRow);
                    }

                    Shell.waitingForm.Close();
                }
                else
                {
                    MessageBox.Show("Service is busying now!");
                    Shell.waitingForm.Close();
                }
            };
            proxyCuttingTool.getInferSelectStatusAsync();
            proxyCuttingTool.CloseAsync();
        }

        #endregion

        #region radio button change
        private void radioSelect_Checked(object sender, RoutedEventArgs e)
        {
            ObservableCollection<OntologyResultForCuttingTool> selectedList = new ObservableCollection<OntologyResultForCuttingTool>();
            selectedList.Clear();
            if (this.DG_ReplaceToolNReasion.SelectedItem is OntologyResultForCuttingTool)
            {
                selectedList.Add(((OntologyResultForCuttingTool)this.DG_ReplaceToolNReasion.SelectedItem));
            }

            foreach (var c in selectedList)
            {

                if (objNCInfoL.CuttingToolNo.Equals(c.CuttingToolNo))
                {
                    //更新資料庫
                    Shell.waitingForm.SettingMessage("Connecting to service");
                    Shell.waitingForm.Show();
                    IPS.RecommendedCuttingTool.CuttingToolInterfaceClient proxyCuttingTool = new IPS.RecommendedCuttingTool.CuttingToolInterfaceClient();
                    proxyCuttingTool.updateInferSelectStatusCompleted += (s, er) =>
                    {
                        if (er.Error == null & er.Result != null)
                        {
                            //將使用者選擇的狀態記錄資料庫取出來更新到資料庫
                            DataContractJsonSerializer serinferForNC = new DataContractJsonSerializer(typeof(ObservableCollection<OntologyResultForNCFile>));
                            MemoryStream responseStreamNC = new MemoryStream(Encoding.UTF8.GetBytes(er.Result));
                            InferNC2 = serinferForNC.ReadObject(responseStreamNC) as ObservableCollection<OntologyResultForNCFile>;
                            LinferJsonString_After = new ObservableCollection<OntologyResultForNCFile>(
                                                            from chooseL in InferNC2
                                                            where chooseL.FileName.Equals("O0512")
                                                            orderby chooseL.StageNo
                                                            select chooseL);
                            PagedCollectionView LList = new PagedCollectionView(LinferJsonString_After);
                            LList.GroupDescriptions.Add(new PropertyGroupDescription("StageNo"));
                            DBInferenceCuttingToolResult_left.ItemsSource = LList;
                            DBInferenceCuttingToolResult_left.LoadingRow += new EventHandler<DataGridRowEventArgs>(DBCahngeBackgroud_LoadingRow);
                            Shell.waitingForm.Close();
                        }
                        else
                        {
                            MessageBox.Show("Service is busying now!");
                            Shell.waitingForm.Close();
                        }
                    };
                    proxyCuttingTool.updateInferSelectStatusAsync(objNCInfoL.FileName, objNCInfoL.StageNo, c.CuttingToolNo, c.ReplaceableCuttingToolNo, c.ReplaceableCuttingToolNo, "true");
                    proxyCuttingTool.CloseAsync();

                }
                else if (objNCInfoR.CuttingToolNo.Equals(c.CuttingToolNo))
                {
                    //更新資料庫
                    Shell.waitingForm.SettingMessage("Connecting to service");
                    Shell.waitingForm.Show();
                    IPS.RecommendedCuttingTool.CuttingToolInterfaceClient proxyCuttingTool = new IPS.RecommendedCuttingTool.CuttingToolInterfaceClient();
                    proxyCuttingTool.updateInferSelectStatusCompleted += (s, er) =>
                    {
                        if (er.Error == null & er.Result != null)
                        {
                            //將使用者選擇的狀態記錄資料庫取出來更新到資料庫
                            DataContractJsonSerializer serinferForNC2 = new DataContractJsonSerializer(typeof(ObservableCollection<OntologyResultForNCFile>));
                            MemoryStream responseStreamNC2 = new MemoryStream(Encoding.UTF8.GetBytes(er.Result));
                            InferNC2 = serinferForNC2.ReadObject(responseStreamNC2) as ObservableCollection<OntologyResultForNCFile>;
                            RinferJsonString_After = new ObservableCollection<OntologyResultForNCFile>(
                                                          from chooseR in InferNC2
                                                          where chooseR.FileName.Equals("O0511")
                                                          orderby chooseR.StageNo
                                                          select chooseR);
                            PagedCollectionView RList = new PagedCollectionView(RinferJsonString_After);
                            RList.GroupDescriptions.Add(new PropertyGroupDescription("StageNo"));
                            DBInferenceCuttingToolResult_right.ItemsSource = RList;
                            DBInferenceCuttingToolResult_right.LoadingRow += new EventHandler<DataGridRowEventArgs>(DBCahngeBackgroud_LoadingRow);
                            Shell.waitingForm.Close();
                        }
                        else
                        {
                            MessageBox.Show("Service is busying now!");
                            Shell.waitingForm.Close();
                        }
                    };
                    proxyCuttingTool.updateInferSelectStatusAsync(objNCInfoR.FileName, objNCInfoR.StageNo, c.CuttingToolNo, c.ReplaceableCuttingToolNo, c.ReplaceableCuttingToolNo, "true");
                    proxyCuttingTool.CloseAsync();
                }             
            }
        }
        #endregion

        #endregion

        #region Step3: 虛擬工具機團隊之碰撞檢測模組與加工時間估算

        private void btnNextToRealMachining_Click(object sender, RoutedEventArgs e)
        {
            Control_TabItemChange(4);
            List<VECollisionInfo> Json_PostToService = new List<VECollisionInfo>();
            List<SelectCuttingToolCollision> L_selectCTList = new List<SelectCuttingToolCollision>();
            List<SelectCuttingToolCollision> R_selectCTList = new List<SelectCuttingToolCollision>();
            IEnumerator ppEnum_DG_leftCTNumber = DG_leftCTNumber.ItemsSource.GetEnumerator();
            IEnumerator ppEnum_DG_rightCTNumber = DG_rightCTNumber.ItemsSource.GetEnumerator();

            while (ppEnum_DG_leftCTNumber.MoveNext())
            {
                L_selectCTList.Add((SelectCuttingToolCollision)ppEnum_DG_leftCTNumber.Current);
            }

            while (ppEnum_DG_rightCTNumber.MoveNext())
            {
                R_selectCTList.Add((SelectCuttingToolCollision)ppEnum_DG_rightCTNumber.Current);
            }


            //JSON 打包製程工程師輸入的所有參數
            VECollisionInfo L_VECollisionInfo = new VECollisionInfo();
            L_VECollisionInfo.FileName = TB_ShowuploadLeftNCName.Text;
            L_VECollisionInfo.MachiningTime = TB_TimeResult.Text;
            L_VECollisionInfo.CollisionDetection = TB_CollisionResult.Text;
            L_VECollisionInfo.CuttingToolList = L_selectCTList;

            VECollisionInfo R_VECollisionInfo = new VECollisionInfo();
            R_VECollisionInfo.FileName = TB_ShowuploadRightNCName.Text;
            R_VECollisionInfo.MachiningTime = TB_TimeResult.Text;
            R_VECollisionInfo.CollisionDetection = TB_CollisionResult.Text;
            R_VECollisionInfo.CuttingToolList = R_selectCTList;

            Json_PostToService.Add(L_VECollisionInfo);
            Json_PostToService.Add(R_VECollisionInfo);

            MemoryStream ms_VECollision = new MemoryStream();
            DataContractJsonSerializer serializer_VECollision = new DataContractJsonSerializer(typeof(List<VECollisionInfo>));
            serializer_VECollision.WriteObject(ms_VECollision, Json_PostToService);
            byte[] json_VECollision = ms_VECollision.ToArray();
            ms_VECollision.Close();
            
            //確認刀具資訊
            List<ConfirmCTNCInfo> _list = new List<ConfirmCTNCInfo>() { new ConfirmCTNCInfo() { L_CuttingToolNCInfo = TB_ShowuploadLeftNCName.Text, R_CuttingToolNCInfo = TB_ShowuploadRightNCName.Text} };
            DGRealMachining.ItemsSource = _list;
            DG_AfterCuttingToolInfo_Left.ItemsSource = L_selectCTList;
            DG_AfterCuttingToolInfo_Right.ItemsSource = R_selectCTList;
        }

        private void btnLastToChangeTool_Click(object sender, RoutedEventArgs e)
        {
            TBSelectFavoriteCT.IsSelected = true;
            TBGetVMTModule.IsSelected = false;
        }

        #endregion

        #region Step4: 確認選用刀具並送到實際機台

        private void btnStartCutting_Click(object sender, RoutedEventArgs e)
        {
            //確認、送往實際機台進行加工
            Shell.waitingForm.SettingMessage("Connecting to Service");
            Shell.waitingForm.Show();
            string NCAddr = @"[{'FileName':'https://portalvhdsd32zr3v7k67p3.blob.core.windows.net/ontology/O0511'},{'FileName':'https://portalvhdsd32zr3v7k67p3.blob.core.windows.net/ontology/O0512'}]";

            Service1Client bpelclient = new Service1Client();
            bpelclient.InvokeBPELCompleted += (s, er) =>
            {
                try
                {
                    if (er.Error == null & er.Result != null)
                    {
                        if (er.Result.Equals("true"))
                        {
                            MessageBox.Show("Already Send To Machine Tool!");
                        }
                        else
                        {
                            MessageBox.Show("IT Service is closing!");
                        }
                        Shell.waitingForm.Close();
                    }
                    else
                    {
                        MessageBox.Show("Service is busying now!");
                        Shell.waitingForm.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                Shell.waitingForm.DialogResult = false;
            };
            bpelclient.InvokeBPELAsync(StateManager.Username, "Step5_CallITService", NCAddr, DriverOption);
            bpelclient.CloseAsync();
        }

        #endregion

        #region 更換datagrid的樣板呈現

        private void DBCahngeBackgroud_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            DataGridRow row = e.Row;
            var c = row.DataContext as OntologyResultForNCFile;
            foreach (var d in c.ReplaceableRuleSet)
            {
                if (d.ReplaceableCuttingToolNo.Equals(c.CuttingToolNo))
                {
                    if (c != null & d.RoughingRule.Equals("Failed") | d.FinishingRule.Equals("Failed") | d.ExternalDiameterProcessingRule.Equals("Failed") | d.InternalDiameterProcessingCuboidRule.Equals("Failed") | d.InternalDiameterProcessingCylinderRule.Equals("Failed") | d.ToolNWPHardnessRule.Equals("Failed"))
                    {
                        e.Row.Foreground = new SolidColorBrush(Colors.Red);
                    }
                }
            }
        }

        void DBCahngeBackgroudAfter_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            SolidColorBrush brushfore = new SolidColorBrush(Colors.White);
            SolidColorBrush brushFailed = new SolidColorBrush(Colors.Red);
            SolidColorBrush brushPass = new SolidColorBrush(Colors.Green);

            OntologyResultForCuttingTool clist = e.Row.DataContext as OntologyResultForCuttingTool;
            FrameworkElement el2 = this.DG_ReplaceToolNReasion.Columns[2].GetCellContent(e.Row);
            DataGridCell changeCell2 = GetParent(el2, typeof(DataGridCell)) as DataGridCell;
            if (changeCell2 != null)
            {
                if (clist.ExternalDiameterProcessingRule.Equals("Failed"))
                {
                    changeCell2.Background = brushFailed;
                    changeCell2.Foreground = brushfore;
                }
                else if (clist.ExternalDiameterProcessingRule.Equals("Passed"))
                {
                    changeCell2.Foreground = brushfore;
                    changeCell2.Background = brushPass;
                }
            }
            FrameworkElement el3 = this.DG_ReplaceToolNReasion.Columns[3].GetCellContent(e.Row);
            DataGridCell changeCell3 = GetParent(el3, typeof(DataGridCell)) as DataGridCell;
            if (changeCell3 != null)
            {
                if (clist.InternalDiameterProcessingCylinderRule.Equals("Failed"))
                {
                    changeCell3.Background = brushFailed;
                    changeCell3.Foreground = brushfore;
                }
                else if (clist.InternalDiameterProcessingCylinderRule.Equals("Passed"))
                {
                    changeCell3.Foreground = brushfore;
                    changeCell3.Background = brushPass;
                }
            }
            FrameworkElement el4 = this.DG_ReplaceToolNReasion.Columns[4].GetCellContent(e.Row);
            DataGridCell changeCell4 = GetParent(el4, typeof(DataGridCell)) as DataGridCell;
            if (changeCell4 != null)
            {
                if (clist.InternalDiameterProcessingCuboidRule.Equals("Failed"))
                {
                    changeCell4.Background = brushFailed;
                    changeCell4.Foreground = brushfore;
                }
                else if (clist.InternalDiameterProcessingCuboidRule.Equals("Passed"))
                {
                    changeCell4.Foreground = brushfore;
                    changeCell4.Background = brushPass;
                }
            }
            FrameworkElement el5 = this.DG_ReplaceToolNReasion.Columns[5].GetCellContent(e.Row);
            DataGridCell changeCell5 = GetParent(el5, typeof(DataGridCell)) as DataGridCell;
            if (changeCell5 != null)
            {
                if (clist.FinishingRule.Equals("Failed"))
                {
                    changeCell5.Background = brushFailed;
                    changeCell5.Foreground = brushfore;
                }
                else if (clist.FinishingRule.Equals("Passed"))
                {
                    changeCell5.Foreground = brushfore;
                    changeCell5.Background = brushPass;
                }
            }
            FrameworkElement el6 = this.DG_ReplaceToolNReasion.Columns[6].GetCellContent(e.Row);
            DataGridCell changeCell6 = GetParent(el6, typeof(DataGridCell)) as DataGridCell;
            if (changeCell6 != null)
            {
                if (clist.RoughingRule.Equals("Failed"))
                {
                    changeCell6.Background = brushFailed;
                    changeCell6.Foreground = brushfore;
                }
                else if (clist.RoughingRule.Equals("Passed"))
                {
                    changeCell6.Foreground = brushfore;
                    changeCell6.Background = brushPass;
                }
            }
        }

        private FrameworkElement GetParent(FrameworkElement child, Type targetType)
        {
            object parent = child.Parent;
            if (parent != null)
            {
                if (parent.GetType() == targetType)
                {
                    return (FrameworkElement)parent;
                }
                else
                {
                    return GetParent((FrameworkElement)parent, targetType);
                }
            }
            return null;
        }
        #endregion
    }
}
