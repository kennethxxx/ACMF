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
using Visifire.Charts;
//using IPS.Common;
using IPS.ModelCreation;
using System.Collections;
using System.Collections.ObjectModel;
using IPS.Common;
using Serialization;
using System.Threading;
using IPS.ViewsSub.ModelCreationModule;

namespace IPS.Views
{
    
    public partial class ModelCreationModule : Page
    {
        MCDataCollectionGlobalContainer     gDC_GlobalContainer = null;
        MCDataSelectionGlobalContainer      gDS_GlobalContainer = null;
        MCSetGroupGlobalContainer           gSG_GlobalContainer = null;
        MCClearAbnormalYGlobalContainer     gCAY_GlobalContainer = null;
        MCAbnormalIsolatedGlobalContainer   gAbIso_GlobalContainer = null;
        MCVerifyDQIxGlobalContainer         gVX_GlobalContainer = null;
        MCVerifyDQIyGlobalContainer         gVY_GlobalContainer = null;

        DCPInfo gDC_GlobalInfo = null;
        DCPInfo gDS_GlobalInfo = null;
        DCPInfo gSG_GlobalInfo = null;


        public ModelCreationModule()
        {
            InitializeComponent();
            if (StateManager.Username == "") //避免使用者按下Refresh
            {
                return;
            }

            //建立Global Container
            gDC_GlobalContainer = new MCDataCollectionGlobalContainer();
            gDS_GlobalContainer = new MCDataSelectionGlobalContainer();
            gSG_GlobalContainer = new MCSetGroupGlobalContainer();
            gCAY_GlobalContainer = new MCClearAbnormalYGlobalContainer();
            gAbIso_GlobalContainer = new MCAbnormalIsolatedGlobalContainer();
            gVX_GlobalContainer = new MCVerifyDQIxGlobalContainer();
            gVY_GlobalContainer = new MCVerifyDQIyGlobalContainer();
            
            gDC_GlobalInfo = new DCPInfo();
            gDC_GlobalInfo.X_Data = new ObservableCollection<XItem>();
            gDC_GlobalInfo.Y_Data = new ObservableCollection<YItem>();
            gDC_GlobalInfo.WP_Data = new ObservableCollection<AllPiece>();
            gDC_GlobalInfo.WP_TrainData = new ObservableCollection<TrainPiece>();
            gDC_GlobalInfo.WP_RunData = new ObservableCollection<RunPiece>();

            gDS_GlobalInfo = new DCPInfo();
            gDS_GlobalInfo.X_Data = new ObservableCollection<XItem>();
            gDS_GlobalInfo.Y_Data = new ObservableCollection<YItem>();
            gDS_GlobalInfo.WP_Data = new ObservableCollection<AllPiece>();
            gDS_GlobalInfo.WP_TrainData = new ObservableCollection<TrainPiece>();
            gDS_GlobalInfo.WP_RunData = new ObservableCollection<RunPiece>();

            gSG_GlobalInfo = new DCPInfo();
            gSG_GlobalInfo.X_Data = new ObservableCollection<XItem>();
            gSG_GlobalInfo.Y_Data = new ObservableCollection<YItem>();
            gSG_GlobalInfo.WP_Data = new ObservableCollection<AllPiece>();
            gSG_GlobalInfo.WP_TrainData = new ObservableCollection<TrainPiece>();
            gSG_GlobalInfo.WP_RunData = new ObservableCollection<RunPiece>();

            // 綁定各分頁事件
            ui_DataCollection_Page.ChangeToNextStep +=new EventHandler(ui_DataCollection_Page_ChangeToNextStep);
            ui_DataCollection_Page.DestroyNextStep += new EventHandler(ui_DataCollection_Page_DestroyNextStep);
            ui_DataSelection_Page.ChangeToNextStep += new EventHandler(ui_DataSelection_Page_ChangeToNextStep);
            ui_DataSelection_Page.DestroyNextStep += new EventHandler(ui_DataSelection_Page_DestroyNextStep);
            ui_SetGroup_Page.ChangeToNextStep += new EventHandler(ui_SetGroup_Page_ChangeToNextStep);
            ui_SetGroup_Page.DestroyNextStep += new EventHandler(ui_SetGroup_Page_DestroyNextStep);
            ui_CleanProcessData_Page.ChangeToNextStep += new EventHandler(ui_CleanProcessData_Page_ChangeToNextStep);
            ui_CleanProcessData_Page.DestroyNextStep += new EventHandler(ui_CleanProcessData_Page_DestroyNextStep);
            ui_CleanMetrologyData_Page.ChangeToNextStep += new EventHandler(ui_CleanMetrologyData_Page_ChangeToNextStep);
            ui_CleanMetrologyData_Page.DestroyNextStep += new EventHandler(ui_CleanMetrologyData_Page_DestroyNextStep);
            ui_VerifyDQIx_Page.ChangeToNextStep += new EventHandler(ui_VerifyDQIx_Page_ChangeToNextStep);
            ui_VerifyDQIx_Page.DestroyNextStep += new EventHandler(ui_VerifyDQIx_Page_DestroyNextStep);
            ui_VerifyDQIy_Page.ChangeToNextStep += new EventHandler(ui_VerifyDQIy_Page_ChangeToNextStep);
            ui_VerifyDQIy_Page.DestroyNextStep += new EventHandler(ui_VerifyDQIy_Page_DestroyNextStep);
            ui_BuildConjectureModel_Page.FinishBuildModel += new EventHandler(ui_BuildConjectureModel_Page_FinishBuildModel);
            ui_BuildConjectureModel_Page.FailBuildModel += new EventHandler(ui_BuildConjectureModel_Page_FailBuildModel);


            // 綁定Container
            ui_DataCollection_Page.BindingContainer(gDC_GlobalContainer, gDC_GlobalInfo);

            ui_DataSelection_Page.BindingContainer(
                gDC_GlobalContainer,
                gDS_GlobalContainer,
                gDC_GlobalInfo,
                gDS_GlobalInfo
                );
            ui_SetGroup_Page.BindingContainer(
                gDC_GlobalContainer,
                gDS_GlobalContainer,
                gSG_GlobalContainer,
                gAbIso_GlobalContainer,
                gDC_GlobalInfo,
                gDS_GlobalInfo,
                gSG_GlobalInfo
                );
            ui_CleanProcessData_Page.BindingContainer(
                gDC_GlobalContainer,
                gDS_GlobalContainer,
                gSG_GlobalContainer,
                gCAY_GlobalContainer,
                gAbIso_GlobalContainer
                );
            ui_CleanMetrologyData_Page.BindingContainer(
                gSG_GlobalContainer,
                gCAY_GlobalContainer,
                gAbIso_GlobalContainer
                );
            ui_VerifyDQIx_Page.BindingContainer(
                gDC_GlobalContainer,
                gDS_GlobalContainer,
                gSG_GlobalContainer,
                gVX_GlobalContainer,
                gAbIso_GlobalContainer
                );
            ui_VerifyDQIy_Page.BindingContainer(
                gDC_GlobalContainer,
                gDS_GlobalContainer,
                gSG_GlobalContainer,
                gVX_GlobalContainer,
                gVY_GlobalContainer,
                gAbIso_GlobalContainer
                );
            ui_BuildConjectureModel_Page.BindingContainer(
                gDC_GlobalContainer,
                gSG_GlobalContainer,
                gAbIso_GlobalContainer
                );

            ui_DataCollection_Page.InitionPage();
            ui_DC_MainTab.IsSelected = true;
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        #region Data Collection Tab
        void ui_DataCollection_Page_ChangeToNextStep(object sender, EventArgs e)
        {
            // Destroy Next Step - Data Selection
            ui_DataSelection_Page.DestroyPage();
            // Init Next Step - Data Selection
            ui_DataSelection_Page.InitionPage();

            // Change to Next Step
            ui_DS_MainTab.IsEnabled = true;
            ui_DS_MainTab.IsSelected = true;
        }
        void ui_DataCollection_Page_DestroyNextStep(object sender, EventArgs e)
        {
            // Destroy Next Step
            if (ui_DS_MainTab.IsEnabled)
            {
                ui_DataSelection_Page.DestroyPage();
            }

            ui_DS_MainTab.IsEnabled = false;
        }
        #endregion

        #region Data Selection Tab
        void ui_DataSelection_Page_ChangeToNextStep(object sender, EventArgs e)
        {
            // Destroy Next Step - SetGroup
            ui_SetGroup_Page.DestroyPage();
            // Init Next Step - SetGroup
            ui_SetGroup_Page.InitionPage();

            // Change to Next Step
            ui_SG_MainTab.IsEnabled = true;
            ui_SG_MainTab.IsSelected = true;
        }
        void ui_DataSelection_Page_DestroyNextStep(object sender, EventArgs e)
        {
            // Destroy Next Step
            if (ui_SG_MainTab.IsEnabled)
            {
                ui_SetGroup_Page.DestroyPage();
            }

            ui_SG_MainTab.IsEnabled = false;
        }
        #endregion

        #region Set Group Tab
        void ui_SetGroup_Page_ChangeToNextStep(object sender, EventArgs e)
        {
            // Destroy Next Step - CleanProcess
            ui_CleanProcessData_Page.DestroyPage();
            // Init Next Step - CleanProcess
            ui_CleanProcessData_Page.InitionPage();

            // Change to Next Step
            ui_CP_MainTab.IsEnabled = true;
            ui_CP_MainTab.IsSelected = true;
        }
        void ui_SetGroup_Page_DestroyNextStep(object sender, EventArgs e)
        {
            // Destroy Next Step
            if (ui_CP_MainTab.IsEnabled)
            {
                ui_CleanProcessData_Page.DestroyPage();
            }

            ui_CP_MainTab.IsEnabled = false;
        }
        #endregion

        #region Clean Process Data Tab
        void ui_CleanProcessData_Page_ChangeToNextStep(object sender, EventArgs e)
        {
            // Destroy Next Step - CleanMetrology
            ui_CleanMetrologyData_Page.DestroyPage();
            // Init Next Step - CleanMetrology
            ui_CleanMetrologyData_Page.InitionPage();

            // Change to Next Step
            ui_CM_MainTab.IsEnabled = true;
            ui_CM_MainTab.IsSelected = true;
        }
        void ui_CleanProcessData_Page_DestroyNextStep(object sender, EventArgs e)
        {
            // Destroy this Step
            if (ui_CM_MainTab.IsEnabled)
            {
                ui_CleanMetrologyData_Page.DestroyPage();
            }

            ui_CM_MainTab.IsEnabled = false;
        }
        #endregion

        #region Clear Metrology Data
        void ui_CleanMetrologyData_Page_ChangeToNextStep(object sender, EventArgs e)
        {
            // Destroy Next Step - VerifyDQIx
            ui_VerifyDQIx_Page.DestroyPage();
            // Init Next Step - VerifyDQIx
            ui_VerifyDQIx_Page.InitionPage();

            // Change to Next Step
            ui_VX_MainTab.IsEnabled = true;
            ui_VX_MainTab.IsSelected = true;
        }
        void ui_CleanMetrologyData_Page_DestroyNextStep(object sender, EventArgs e)
        {
            // Destroy this Step
            if (ui_VX_MainTab.IsEnabled)
            {
                ui_VerifyDQIx_Page.DestroyPage();
            }
            ui_VX_MainTab.IsEnabled = false;
        }
        #endregion

        #region Verify DQIx Tab
        void ui_VerifyDQIx_Page_ChangeToNextStep(object sender, EventArgs e)
        {
            // Destroy Next Step - VerifyDQIy
            ui_VerifyDQIy_Page.DestroyPage();
            // Init Next Step - VerifyDQIy
            ui_VerifyDQIy_Page.InitionPage();

            // Change to Next Step
            ui_VY_MainTab.IsEnabled = true;
            ui_VY_MainTab.IsSelected = true;
        }
        void ui_VerifyDQIx_Page_DestroyNextStep(object sender, EventArgs e)
        {
            // Destroy this Step
            if (ui_VY_MainTab.IsEnabled)
            {
                ui_VerifyDQIy_Page.DestroyPage();
            }
            ui_VY_MainTab.IsEnabled = false;
        }
        #endregion
        
        #region Verify DQIy
        void ui_VerifyDQIy_Page_ChangeToNextStep(object sender, EventArgs e)
        {
            // Destroy Next Step - BuildModel
            ui_BuildConjectureModel_Page.DestroyPage();
            // Init Next Step - BuildModel
            ui_BuildConjectureModel_Page.InitionPage();

            // Change to Next Step
            ui_BM_MainTab.IsEnabled = true;
            ui_BM_MainTab.IsSelected = true;
        }
        void ui_VerifyDQIy_Page_DestroyNextStep(object sender, EventArgs e)
        {
            // Destroy this Step
            if (ui_BM_MainTab.IsEnabled)
            {
                ui_BuildConjectureModel_Page.DestroyPage();
            }
            ui_BM_MainTab.IsEnabled = false;
        }
        #endregion

        #region Build Model Page
        void ui_BuildConjectureModel_Page_FinishBuildModel(object sender, EventArgs e)
        {
            ui_DataCollection_Page.DestroyPage();
            ui_DataCollection_Page.InitionPage();
            ui_DC_MainTab.IsSelected = true;
        }
        void ui_BuildConjectureModel_Page_FailBuildModel(object sender, EventArgs e)
        {
            ui_DataCollection_Page.DestroyPage();
            ui_DataCollection_Page.InitionPage();
            ui_DC_MainTab.IsSelected = true;
        }
        #endregion

      
    }
}
