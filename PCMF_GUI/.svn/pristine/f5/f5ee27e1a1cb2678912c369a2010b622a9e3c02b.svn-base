using System;
using System.Windows;
using System.Windows.Controls;

namespace IPS.Views
{
    public partial class ErrorWindow : ChildWindow
    {
        public string UriOriginalString = "";
        public ErrorWindow(Exception e)
        {
            InitializeComponent();
            if (e != null)
            {
                ErrorTextBox.Text = (e.Message + e.StackTrace).Replace('"', '\'').Replace("\r\n", @"\n");
            }
        }

        public ErrorWindow(string OriginalString, Uri CurrentUri)
        {
            InitializeComponent();
            if (CurrentUri != null)
            {
                ErrorTextBox.Text = "Page not found: \"" + CurrentUri.ToString() + "\"";
            }
            UriOriginalString = OriginalString;
        }

        public ErrorWindow(string message, string details)
        {
            InitializeComponent();
            ErrorTextBox.Text = message + Environment.NewLine + Environment.NewLine + details;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            
            Shell.waitingForm.Close();
            //Shell.waitingForm.isClose = true;
            this.DialogResult = true;
        }
    }
}