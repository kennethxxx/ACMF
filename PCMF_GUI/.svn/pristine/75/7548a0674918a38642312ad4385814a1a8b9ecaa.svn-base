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
using IPS.Common;
using IPS.DataAcquisition;
using System.Collections.ObjectModel;

namespace IPS.Views
{
    public partial class DataAquisitionModule : Page
    {
        private DACreateNewDCPLocalContainer gCND_LocalContainer = null;
        private DACreateNewDCPGlobalContainer gCND_GlobalContainer = null;

        public DataAquisitionModule()
        {
            InitializeComponent();

            if (StateManager.Username == "") //避免使用者按下Refresh
            {
                return;
            }

            ui_DataAquisitionModule_HistoryDCPView_Page.CreateNewDCPClicked += new EventHandler(ui_DataAquisitionModule_HistoryDCPView_Page_CreateNewDCPClicked);
            ui_DataAquisitionModule_CreateNewDCP_Page.FanOutDCPClicked += new EventHandler(ui_DataAquisitionModule_CreateNewDCP_Page_FanOutDCPClicked);

            ui_DataAquisitionModule_HistoryDCPView_Page.RefreshHistDCPView();
        }

        
        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }


        // 提供主控介面中 子介面切換時的事件
        void ui_DataAquisitionModule_HistoryDCPView_Page_CreateNewDCPClicked(object sender, EventArgs e)
        {
            ui_CNDCP_MainTab.IsEnabled = true;
            ui_CNDCP_MainTab.IsSelected = true;
            gCND_LocalContainer = new DACreateNewDCPLocalContainer();
            gCND_GlobalContainer = new DACreateNewDCPGlobalContainer();
            ui_DataAquisitionModule_CreateNewDCP_Page.BindingContainer(gCND_LocalContainer, gCND_GlobalContainer);
            ui_DataAquisitionModule_CreateNewDCP_Page.InitChooseServiceBroker();
        }

        void ui_DataAquisitionModule_CreateNewDCP_Page_FanOutDCPClicked(object sender, EventArgs e)
        {
            ui_HistDCP_MainTab.IsSelected = true;
            ui_CNDCP_MainTab.IsEnabled = false;
            ui_DataAquisitionModule_HistoryDCPView_Page.RefreshHistDCPView();
        }


        

        

        











    }
}
