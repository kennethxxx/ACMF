using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using IPS.DataAcquisition;
using IPS.Views;

namespace IPS.ViewsSub.DataAquisitionModule
{
	public partial class DataAquisitionModule_HistoryDCPView_Page : UserControl
	{
		public DataAquisitionModule_HistoryDCPView_Page()
		{
			// 必須將變數初始化
			InitializeComponent();
		}

        // 更新Hist DCP資料 會給外部使用
        public void RefreshHistDCPView()
        {
            ui_HistDCPGridView.ItemsSource = null;

            //進行更新
            //Shell.waitingForm.SettingMessage("Refresh DCP Information");
            Shell.waitingForm.SettingMessage("更新資料蒐集計畫狀態");
            Shell.waitingForm.Show();

            App.proxyDA.getDCPInfoCompleted += new EventHandler<getDCPInfoCompletedEventArgs>(getDCPInfoEventHandler);
            App.proxyDA.getDCPInfoAsync();
        }
        
        private void getDCPInfoEventHandler(object sender, getDCPInfoCompletedEventArgs e)
        {
            bool Ack = true;
            try
            {
                DCPInfoClass DCPIC = e.Result;
                if (DCPIC != null)
                {
                    ui_HistDCPGridView.ItemsSource = DCPIC.DCPinfoList;
                    //ui_LastUpdateTimeText.Content = "Last Update:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    ui_LastUpdateTimeText.Content = "最後更新:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    //MessageBox.Show("Error: No Result.");
                    MessageBox.Show("錯誤訊息: 沒有結果");
                    Ack = false;
                }
            }
            catch (Exception ex)
            {
               // MessageBox.Show("Error: " + ex.ToString());
                MessageBox.Show("錯誤訊息: " + ex.ToString());
                Ack = false;
            }

            App.proxyDA.getDCPInfoCompleted -= new EventHandler<getDCPInfoCompletedEventArgs>(getDCPInfoEventHandler);
            Shell.waitingForm.Close();

            if (!Ack)
            {
                ui_HistDCPGridView.ItemsSource = null;
            }
        }


        // 變更Row顏色
        private void ui_HistDCPGridView_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            DataGridRow row = e.Row;
            var rowdata= row.DataContext as DCPinfo;
            if (rowdata != null)
            {
                // 有錯誤的就變紅色
                if (rowdata.DCPStatus.Trim().Contains("Fail") || rowdata.DCRStatus.Trim().Contains("Fail"))
                {
                    row.Background = new System.Windows.Media.SolidColorBrush(Colors.Red);
                }
                //else if (rowdata.DCPStatus.Trim().Contains("Success") && rowdata.DCRStatus.Trim().Contains("Success"))
                //{
                //  //都完成的變綠色
                    //row.Background = new System.Windows.Media.SolidColorBrush(Colors.Green);
                //}
                //else 
                //{
                //    //剩下的是灰色
                //    row.Background = new System.Windows.Media.SolidColorBrush(Colors.LightGray);
                //}
            
            }
        }


        // 更新Hist DCP資料
        private void ui_Refresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshHistDCPView();
        }


        //給外部的事件Handler
        public event EventHandler CreateNewDCPClicked;
        private void ui_CreateNewDCP_Click(object sender, RoutedEventArgs e)
        {
            if (CreateNewDCPClicked != null)
                CreateNewDCPClicked(this, null);
        }

        public Boolean ProgressBarVisibility
        {
            set { progressBar1.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
            get { return progressBar1.Visibility == Visibility.Visible ? true : false; }
        }

        public static System.Windows.Threading.DispatcherTimer timer1s = new System.Windows.Threading.DispatcherTimer();

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            this.ProgressBarVisibility = true;
            //1s
            timer1s = new System.Windows.Threading.DispatcherTimer();
            //start to update
            timer1s.Tick += new EventHandler(timer_Tick_1s);
            timer1s.Interval = new TimeSpan(0, 0, 0, 0, 0);

            timer1s.Start();
        }

        private void Grid_Unloaded(object sender, RoutedEventArgs e)
        {
            //stop to update
            timer1s.Stop();
            timer1s = null;
        }

        private void timer_Tick_1s(object sender, EventArgs e)
        {
            this.ProgressBarVisibility = true;
            timer1s.Interval = new TimeSpan(0, 0, 0, 60, 0);

            //update
            RefreshHistDCPView();
        }

        
        
	}
}