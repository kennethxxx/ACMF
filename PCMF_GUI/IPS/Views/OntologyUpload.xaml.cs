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
using System.IO;

namespace IPS.Views
{
    public partial class OntologyUpload : Page
    {
        OpenFileDialog dlg_Class = new OpenFileDialog();
        OpenFileDialog dlg_Property = new OpenFileDialog();
        OpenFileDialog dlg_Instance = new OpenFileDialog();
        OpenFileDialog dlg_Constraint = new OpenFileDialog();
        OpenFileDialog dlg_Rule = new OpenFileDialog();
        //UploadFileService.Service1Client UploadService = null;
        int index = 1;

        public OntologyUpload()
        {
            InitializeComponent();
            Initialize();
        }

        // 使用者巡覽至這個頁面時執行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Initialize()
        {
            LB_InfoUpload.Content = "此介面是利用上傳五個知識表單的方式建立工具機本體論。" + "\n" + "Step1：為這工具機本體論命名。" + "\n" + "Step2：選擇Class、Property、Instance、Constraint、Rule的知識表單。" + "\n" + "Step3：並依Class、Property、Instance、Constraint、Rule的順序排列。" + "\n" + "Step4：按下Upload上傳建立工具機本體論。";
            //UploadService = new UploadFileService.Service1Client();
        }

        private void BT_ClassULSelect_Click(object sender, RoutedEventArgs e)
        {
            TB_UploadClass.Text = "";
            dlg_Class.Multiselect = false;
            //這邊可以設定過副檔名過濾器
            dlg_Class.Filter = "Excel活頁簿 (*.csv)|*.csv";

            bool? retval = dlg_Class.ShowDialog();
            if (retval != null && retval == true)
            {
                TB_UploadClass.Text = dlg_Class.File.FullName;
            }
        }

        private void BT_PropertyULSelect_Click(object sender, RoutedEventArgs e)
        {
            TB_UploadProperty.Text = "";
            dlg_Property.Multiselect = false;
            dlg_Property.FilterIndex = 1;
            //這邊可以設定過副檔名過濾器
            dlg_Property.Filter = "Excel活頁簿 (*.csv)|*.csv";

            bool? retval = dlg_Property.ShowDialog();
            if (retval != null && retval == true)
            {
                // Open the selected file to read.
                System.IO.Stream fileStream = dlg_Property.File.OpenRead();

                using (System.IO.StreamReader reader = new System.IO.StreamReader(fileStream))
                {
                    // Read the first line from the file and write it the textbox.
                    TB_UploadProperty.Text = reader.ReadLine();
                    fileStream.Close();
                }
            }

        }

        private void BT_InstanceULSelect_Click(object sender, RoutedEventArgs e)
        {
            TB_UploadInstance.Text = "";
            dlg_Instance.Multiselect = false;
            //這邊可以設定過副檔名過濾器
            dlg_Instance.Filter = "Excel活頁簿 (*.csv)|*.csv";

            bool? retval = dlg_Instance.ShowDialog();
            if (retval != null && retval == true)
            {
                TB_UploadInstance.Text = dlg_Instance.File.FullName;
            }
        }

        private void BT_ConstraintULSelect_Click(object sender, RoutedEventArgs e)
        {
            TB_UploadContraint.Text = "";
            dlg_Constraint.Multiselect = false;
            //這邊可以設定過副檔名過濾器
            dlg_Constraint.Filter = "Excel活頁簿 (*.csv)|*.csv";

            bool? retval = dlg_Constraint.ShowDialog();
            if (retval != null && retval == true)
            {
                TB_UploadContraint.Text = dlg_Constraint.File.FullName;

                string createdb = TBInputOntologyName.Text;
                if (createdb != "")
                {
                    //createdkdb(createdb);
                }

                index++;
            }
        }

        private void BT_RuleULSelect_Click(object sender, RoutedEventArgs e)
        {
            TB_UploadRule.Text = "";
            dlg_Rule.Multiselect = false;
            //這邊可以設定過副檔名過濾器
            dlg_Rule.Filter = "Excel活頁簿 (*.csv)|*.csv";

            bool? retval = dlg_Rule.ShowDialog();
            if (retval != null && retval == true)
            {
                TB_UploadRule.Text = dlg_Rule.File.FullName;
            }
        }

        private void btnUpload_OntologyAllTable_Click(object sender, RoutedEventArgs e)
        {
            if (TBInputOntologyName.Text != "" & dlg_Class.File.Name != null & dlg_Property.File.Name != null & dlg_Instance.File.Name != null & dlg_Constraint.File.Name != null & dlg_Rule.File.Name != null)
            {
                //Progress Bar Start

                //UploadFile1(dlg_Class.File.Name, dlg_Class.File.OpenRead(), dlg_Class.File.FullName, dlg_Class.Filter);
                //UploadFile1(dlg_Property.File.Name, dlg_Property.File.OpenRead(), dlg_Property.File.FullName, dlg_Property.Filter);
                //UploadFile1(dlg_Instance.File.Name, dlg_Instance.File.OpenRead(), dlg_Instance.File.FullName, dlg_Instance.Filter);
                //UploadFile1(dlg_Constraint.File.Name, dlg_Constraint.File.OpenRead(), dlg_Constraint.File.FullName, dlg_Constraint.Filter);
                //UploadFile1(dlg_Rule.File.Name, dlg_Rule.File.OpenRead(), dlg_Rule.File.FullName, dlg_Rule.Filter);

                //Progress Bar End
            }
        }



        //建立DomainKnowledge
        private void createdk(String selectFilePath, String selectFile)
        {
            //OntologyService.Receive_ModulePortTypeClient dkproxy = new OntologyService.Receive_ModulePortTypeClient();
            //dkproxy.createdkAsync(selectFilePath, selectFile);
            //dkproxy.createdkCompleted += new EventHandler<OMC.OntologyService.createdkCompletedEventArgs>(dkproxy_createdkCompleted);
            //dkproxy.CloseAsync();

        }
        //void dkproxy_createdkCompleted(object sender, OMC.OntologyService.createdkCompletedEventArgs e)
        //{

        //    MessageBox.Show(e.Error.ToString());

        //}

        ////建立DomainKnowledge db
        //private void createdkdb(string createdb)
        //{
        //    OntologyService.Receive_ModulePortTypeClient dkproxy = new OntologyService.Receive_ModulePortTypeClient();
        //    dkproxy.createdkdbAsync(createdb);
        //    dkproxy.createdkdbCompleted += new EventHandler<OMC.OntologyService.createdkdbCompletedEventArgs>(dkproxy_createdkdbCompleted);
        //    dkproxy.CloseAsync();



        //    String[] selectFile = new String[] { "class", "Property", "Istance", "Constraints" };
        //    String[] selectdb = new String[] { "FamilyOntology", "FamilyOntology", "FamilyOntology", "FamilyOntology" };

        //    string createdbname = TBInputOntologyName.Text;
        //    if (createdbname != "")
        //    {
        //        string selectFilePath = "D:\\FileUpload\\" + createdbname + "\\" + index + dlg_Class.File.Name + "@@@";
        //        selectFilePath += "D:\\FileUpload\\" + createdbname + "\\" + index + dlg_Property.File.Name + "@@@";
        //        selectFilePath += "D:\\FileUpload\\" + createdbname + "\\" + index + dlg_Instance.File.Name + "@@@";
        //        selectFilePath += "D:\\FileUpload\\" + createdbname + "\\" + index + dlg_Constraint.File.Name;
        //        createdk(selectFilePath, createdbname);
        //    }

        //}
        //void dkproxy_createdkdbCompleted(object sender, OMC.OntologyService.createdkdbCompletedEventArgs e)
        //{
        //    MessageBox.Show(e.Result);
        //    //if (e.Error == null)
        //    //    txtFileDisplayName.Text = e.Result;
        //}

        ////上傳檔案
        //private void UploadFile1(string fileName, Stream strm, string filepath, string filetype)
        //{
        //    byte[] Buffer = new byte[strm.Length];
        //    strm.Read(Buffer, 0, (int)strm.Length);
        //    strm.Dispose();
        //    strm.Close();

        //    UploadFile file = new UploadFile();
        //    file.FileName = fileName;

        //    file.File = Buffer;
        //    string newfilename = filepath;
        //    if (newfilename != "")
        //    {
        //        //UploadService.SaveFileAsync(file, filepath);
        //        UploadService.SaveFileAsync(file.File);
        //        UploadService.SaveFileCompleted += new EventHandler<OMC.UploadFileService.SaveFileCompletedEventArgs>(proxy_SaveFileCompleted);
        //    }
        //    else
        //    {
        //        MessageBox.Show("請輸入輸入檔名");
        //    }
        //}

        //void proxy_SaveFileCompleted(object sender, OMC.UploadFileService.SaveFileCompletedEventArgs e)
        //{
        //    if (e.Error == null)
        //        MessageBox.Show("Successfully Saved at ServerLocation");
        //}

        private void PushData(Stream input, Stream output)
        {
            byte[] buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) != 0)
            {
                output.Write(buffer, 0, bytesRead);
            }
        }

    }
}
