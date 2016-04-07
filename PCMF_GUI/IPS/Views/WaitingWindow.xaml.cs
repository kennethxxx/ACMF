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

namespace IPS.Views
{
    public partial class WaitingWindow : ChildWindow
    {
        // A variable to count with.
        //int i = 0;
        private String strmessage = "";
        private String strsubmessage = "";
        private String strTip = "";
        private TimeSpan SetTime = new TimeSpan(DateTime.Now.Ticks);

        public void SettingMessage(string ShowMessageString)
        {
            strmessage = ShowMessageString;
            strsubmessage = "";
            strTip = "";
            SetTime = new TimeSpan(DateTime.Now.Ticks);
        }

        public void SettingTip(string ShowTipString)
        {
            strTip = ShowTipString;
        }

        public void SettingSubMessage(string ShowSubMessageString)
        {
            strsubmessage = ShowSubMessageString;
        }

        public bool isClose = false;    //false: not close - true: close

        public WaitingWindow()
        {
            InitializeComponent();
            txtMessage.Text = strmessage;
            txtTimeString.Text = "";
            ToolTipService.SetToolTip(txtMessage, "");
        }

        public void StartTimer(object o, RoutedEventArgs sender)
        {
            System.Windows.Threading.DispatcherTimer myDispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            myDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100); // 100 Milliseconds
            myDispatcherTimer.Tick += new EventHandler(Each_Tick);
            myDispatcherTimer.Start();
        }

        // Fires every 100 miliseconds while the DispatcherTimer is active.
        public void Each_Tick(object o, EventArgs sender)
        {
            if (!isClose)
            {
                TimeSpan nowTS = new TimeSpan(DateTime.Now.Ticks);
                TimeSpan ts = nowTS.Subtract(SetTime).Duration();
                txtTimeString.Text = String.Format("{0:00}:{1:00}:{2:00}.{3}", ts.Hours, ts.Minutes, ts.Seconds, (int)(ts.Milliseconds/100));
                string Waitdoc = string.Empty;
                
                for (int i = 0; i < ts.Seconds % 4; i++ )
                    Waitdoc += ".";
                // 放上訊息 
                txtMessage.Text = strmessage + strsubmessage + Waitdoc; // 設定訊息主體 + Waitdoc
                ToolTipService.SetToolTip(txtMessage, strTip); //設定訊息提示
            }
            else
            {
                strmessage = "";
                strTip = "";
                this.Close();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            //fix bug of silverlight 4 with childwindows ->> disable parent
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
    }
}

