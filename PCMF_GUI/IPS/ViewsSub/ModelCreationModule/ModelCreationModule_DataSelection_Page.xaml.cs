using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using System.Linq;
using System.Net;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using IPS.Common;
using IPS.ModelCreation;
using Visifire.Charts;
using Serialization;

namespace IPS.ViewsSub.ModelCreationModule
{
	public partial class ModelCreationModule_DataSelection_Page : UserControl
	{
        // Define Local Container 
        MCDataSelectionLocalContainer   pDS_LocalContainer = null;

        // Define Global Container 
        MCDataCollectionGlobalContainer pDC_GlobalContainer = null;
        MCDataSelectionGlobalContainer  pDS_GlobalContainer = null;

        // Define Global Parameter 
        //  [8/8/2012 autolab]
        DCPInfo pDC_GlobalDI = new DCPInfo();
        DCPInfo pDS_GlobalDI = new DCPInfo();
        

        // Define Global Event
        public event EventHandler ChangeToNextStep;
        public event EventHandler DestroyNextStep;

		public ModelCreationModule_DataSelection_Page()
		{
			// 必須將變數初始化
			InitializeComponent();
            pDS_LocalContainer = new MCDataSelectionLocalContainer();
		}

        #region Init
        public void BindingContainer(MCDataCollectionGlobalContainer pDC, MCDataSelectionGlobalContainer pDS, 
            DCPInfo qDC, DCPInfo qDS)
        {
            pDC_GlobalContainer = pDC;
            pDS_GlobalContainer = pDS;
            pDC_GlobalDI = qDC;
            pDS_GlobalDI = qDS;

        }

        public void InitionPage()
        {
            //pDS_GlobalDI.X_Data = new ObservableCollection<XItem>();
            //pDS_GlobalDI.Y_Data = new ObservableCollection<YItem>();
            //pDS_GlobalDI.WP_Data = new ObservableCollection<AllPiece>();
            //pDS_GlobalDI.WP_TrainData = new ObservableCollection<AllPiece>();
            //pDS_GlobalDI.WP_RunData = new ObservableCollection<AllPiece>();


            ui_DS_AllPopulationDataSet.IsSelected = true;
            pDS_LocalContainer.Clear();
            pDS_GlobalContainer.Clear();

            //set total population
           // ui_DS_AllPopulationDataSet.Header = "All Population [" + pDC_GlobalContainer.contextList.Count.ToString() + "]";
            ui_DS_AllPopulationDataSet.Header = "所有建模資料筆數 [" + pDC_GlobalContainer.contextList.Count.ToString() + "]";
            ui_DS_AllPopulationList.ItemsSource = pDC_GlobalContainer.contextList;
            pDS_LocalContainer.TotalContextCount = pDC_GlobalContainer.TotalcontextListCount;

            //ui_DS_ApplyDataSetTraining.Header = "Training [0]";
            ui_DS_ApplyDataSetTraining.Header = "訓練資料 [0]";
            ui_DS_TrainingList.ItemsSource = null;

            //ui_DS_ApplyDataSetRunning.Header = "Running [0]";
            ui_DS_ApplyDataSetRunning.Header = "測試資料 [0]";
            ui_DS_RunningList.ItemsSource = null;

            ui_DS_TrainingPercentage.Text = pDS_LocalContainer.InitTrainingPercentage.ToString();

            SetComputeDataByPercentage(ui_DS_TrainingPercentage.Text, pDS_LocalContainer.TotalContextCount);

            ui_DS_SelectionApply.IsEnabled = true;
            ui_DS_NextStep.IsEnabled = false;
        }

        public void DestroyPage()
        {
            // 呼叫清空NextStep Tab動作
            if (DestroyNextStep != null)
            {
                DestroyNextStep(null, new EventArgs());
            }

            pDS_LocalContainer.Clear();
            pDS_GlobalContainer.Clear();
            
            ui_DS_AllPopulationDataSet.Header = "";
            ui_DS_AllPopulationList.ItemsSource = null;
            pDS_LocalContainer.TotalContextCount = 0;

            ui_DS_ApplyDataSetTraining.Header = "";
            ui_DS_TrainingList.ItemsSource = null;

            ui_DS_ApplyDataSetRunning.Header = "";
            ui_DS_RunningList.ItemsSource = null;

            ui_DS_TrainingPercentage.Text = "";

            ui_DS_SelectionApply.IsEnabled = false;
            ui_DS_NextStep.IsEnabled = false;
        }
        #endregion

        #region Compute Train and Run Percentage

        private void ui_DS_TrainingPercentage_LostFocus(object sender, RoutedEventArgs e)
        {
            int trainingPercentage = 101; //預設大於100
            try
            {
                trainingPercentage = Int16.Parse(ui_DS_TrainingPercentage.Text);
                ui_DS_TrainingPercentage.Text = trainingPercentage.ToString();
            }
            catch (System.Exception)
            {
                trainingPercentage = 101;
            }

            if (trainingPercentage >= 100 || trainingPercentage <= 0)
            {
                ui_DS_TrainingPercentage.Text = "76";
            }

            SetComputeDataByPercentage(ui_DS_TrainingPercentage.Text, pDS_LocalContainer.TotalContextCount); // set size data of data selection
        }

        private void ui_DS_TrainingCount_LostFocus(object sender, RoutedEventArgs e)
        {
            int TrainingCount = 0;
            int RunningCount = 0;
            int TrainingPercentage = 0;
            int RunningPercentage = 0;
            try
            {
                TrainingCount = Int16.Parse(ui_DS_TrainingCount.Text);
                if (TrainingCount >= pDC_GlobalContainer.TotalcontextListCount || TrainingCount <= 0)
                {
                    SetComputeDataByPercentage(ui_DS_TrainingPercentage.Text, pDS_LocalContainer.TotalContextCount);
                }
                else
                {
                    RunningCount = pDS_LocalContainer.TotalContextCount - TrainingCount;
                    TrainingPercentage = (int)((TrainingCount * 100) / pDS_LocalContainer.TotalContextCount);
                    RunningPercentage = 100 - TrainingPercentage;

                    ui_DS_TrainingPercentage.Text = TrainingPercentage.ToString();
                    ui_DS_RunningPercentage.Text = RunningPercentage.ToString();
                    ui_DS_TrainingCount.Text = TrainingCount.ToString();
                    ui_DS_RunningCount.Text = RunningCount.ToString();

                    ui_DS_SelectionApply.IsEnabled = true;
                    ui_DS_NextStep.IsEnabled = false;
                }
            }
            catch (System.Exception)
            {
                SetComputeDataByPercentage(ui_DS_TrainingPercentage.Text, pDC_GlobalContainer.TotalcontextListCount);
            }
        }

        void SetComputeDataByPercentage(string strTrainingPercentage, int TotalComputeDataSize)
        {
            int TrainingPercentage = Convert.ToInt32(strTrainingPercentage);
            int RunningPercentage = 100 - TrainingPercentage;

            double dbTP = (double)TrainingPercentage / 100;
            int TrainingCount = (int)(TotalComputeDataSize * dbTP);
            int RunningCount = TotalComputeDataSize - TrainingCount;

            ui_DS_TrainingPercentage.Text = TrainingPercentage.ToString();
            ui_DS_RunningPercentage.Text = RunningPercentage.ToString();
            ui_DS_TrainingCount.Text = TrainingCount.ToString();
            ui_DS_RunningCount.Text = RunningCount.ToString();

            ui_DS_SelectionApply.IsEnabled = true;
            ui_DS_NextStep.IsEnabled = false;
        }

        private void ui_DS_SelectionApply_Click(object sender, RoutedEventArgs e)
        {
            pDS_LocalContainer.TrainingPercentage = Convert.ToInt32(ui_DS_TrainingPercentage.Text);
            pDS_LocalContainer.RunningPercentage = Convert.ToInt32(ui_DS_RunningPercentage.Text);
            pDS_LocalContainer.TrainingCount = Convert.ToInt32(ui_DS_TrainingCount.Text);
            pDS_LocalContainer.RunningCount = Convert.ToInt32(ui_DS_RunningCount.Text);

            pDS_LocalContainer.TrainingContextList.Clear();
            pDS_LocalContainer.RunningContextList.Clear();

            for (int i = 0; i < pDS_LocalContainer.TotalContextCount; i++)
            {
                ModelCreation.Context ContextTemp = pDC_GlobalContainer.contextList[i];
                if (i < pDS_LocalContainer.TrainingCount)
                {
                    pDS_LocalContainer.TrainingContextList.Add(ContextTemp);  //加到Training
                }
                else
                {
                    pDS_LocalContainer.RunningContextList.Add(ContextTemp);   //加到Running
                }
            }

            ui_DS_TrainingList.ItemsSource = pDS_LocalContainer.TrainingContextList;
            ui_DS_RunningList.ItemsSource = pDS_LocalContainer.RunningContextList;

            //ui_DS_ApplyDataSetTraining.Header = "Training [" + pDS_LocalContainer.TrainingContextList.Count.ToString() + "]";
            //ui_DS_ApplyDataSetRunning.Header = "Running [" + pDS_LocalContainer.RunningContextList.Count.ToString() + "]";

            ui_DS_ApplyDataSetTraining.Header = "訓練資料 [" + pDS_LocalContainer.TrainingContextList.Count.ToString() + "]";
            ui_DS_ApplyDataSetRunning.Header = "測試資料 [" + pDS_LocalContainer.RunningContextList.Count.ToString() + "]";

            ui_DS_ApplyDataSetTraining.IsSelected = true;

            //////////////////////////////////////////////////////////////////////////
            ui_DS_NextStep.IsEnabled = true;
        }

        #endregion

        #region Next Step
        private void ui_DS_NextStep_Click(object sender, RoutedEventArgs e)
        {
            // 檢查資料是否正確可以到下一Step
            int MinimalTotalCount = (int)(pDC_GlobalContainer.SelectedIndicatorTypeList.Count + 5);

            if (pDS_LocalContainer.TrainingCount < 3 || pDS_LocalContainer.RunningCount < 3) // 每筆最小個數要大於3
            {
              //  MessageBox.Show("The Training or Running Records Could not less 3!");
                MessageBox.Show("測試資料和訓練資料不能少於3筆!");
                return;
            }

            if (pDS_LocalContainer.TrainingCount <= MinimalTotalCount)  //訓練總數比數不能少於X個數+5
            {
                //MessageBox.Show("The Training Record Count " + pDS_LocalContainer.TrainingCount + " must large than " + MinimalTotalCount.ToString() + "\n(Training Count >= Indicator Count + 5)");
                MessageBox.Show("訓練資料數量 " + pDS_LocalContainer.TrainingCount + "不能少於" + MinimalTotalCount.ToString() + "\n(Training Count >= Indicator Count + 5)");
                return;
            }

            int BestTotalCount = (int)(pDC_GlobalContainer.SelectedIndicatorTypeList.Count * 2.5);

            if (pDS_LocalContainer.TrainingCount <= BestTotalCount)  //訓練總數比數最好多於X個數的2.5
            {
                //MessageBox.Show("The Training Record Count is " + pDS_LocalContainer.TrainingCount + "\nand The Best Count is large than " + BestTotalCount.ToString() + " (IndicatorCount * 2.5)");
                MessageBox.Show("訓練資料數量" + pDS_LocalContainer.TrainingCount + "\nand 最好多於  " + BestTotalCount.ToString() + " (IndicatorCount * 2.5)");
            }

            // 複製到 DS Global Container
            pDS_GlobalContainer.Clear();
            pDS_GlobalContainer.TrainingPercentage = pDS_LocalContainer.TrainingPercentage;
            pDS_GlobalContainer.RunningPercentage = pDS_LocalContainer.RunningPercentage;
            pDS_GlobalContainer.TrainingCount = pDS_LocalContainer.TrainingCount;
            pDS_GlobalContainer.RunningCount = pDS_LocalContainer.RunningCount;
            pDS_GlobalContainer.TrainingContextList = CommonValue.DataContractClone(pDS_LocalContainer.TrainingContextList);
            pDS_GlobalContainer.RunningContextList = CommonValue.DataContractClone(pDS_LocalContainer.RunningContextList);

            pDS_GlobalDI.X_Data = new ObservableCollection<XItem>();
            pDS_GlobalDI.Y_Data = new ObservableCollection<YItem>();
            pDS_GlobalDI.WP_Data = new ObservableCollection<AllPiece>();
            pDS_GlobalDI.WP_TrainData = new ObservableCollection<TrainPiece>();
            pDS_GlobalDI.WP_RunData = new ObservableCollection<RunPiece>();

            pDS_GlobalDI.CurrentInfo = pDC_GlobalDI.CurrentInfo;
            pDS_GlobalDI.WP_Data = pDC_GlobalDI.WP_Data;
            pDS_GlobalDI.X_Data = pDC_GlobalDI.X_Data;
            pDS_GlobalDI.Y_Data = pDC_GlobalDI.Y_Data;
            pDS_GlobalDI.StartTime = pDC_GlobalDI.StartTime;
            pDS_GlobalDI.EndTime = pDC_GlobalDI.EndTime;
            
            //////////////////////////////////////////////////////////////////////////
            //Train In
            foreach (ModelCreation.Context ctx in pDS_GlobalContainer.TrainingContextList)
            {
                TrainPiece TrainWP = new TrainPiece();
                TrainWP.ID = ctx.ContextID.ToString();
                TrainWP.Process_StartTime = ctx.ProcessStartTime;
                TrainWP.Process_EndTime = ctx.ProcessEndTime;
                TrainWP.Metrology_StartTime = ctx.MetrologyStartTime;
                TrainWP.Metrology_EndTime = ctx.MetrologyEndTime;

                pDS_GlobalDI.WP_TrainData.Add(TrainWP);
            }
            //////////////////////////////////////////////////////////////////////////
            //Run In
            foreach (ModelCreation.Context ctx in pDS_GlobalContainer.RunningContextList)
            {
                RunPiece RunWP = new RunPiece();
                RunWP.ID = ctx.ContextID.ToString();
                RunWP.Process_StartTime = ctx.ProcessStartTime;
                RunWP.Process_EndTime = ctx.ProcessEndTime;
                RunWP.Metrology_StartTime = ctx.MetrologyStartTime;
                RunWP.Metrology_EndTime = ctx.MetrologyEndTime;

                pDS_GlobalDI.WP_RunData.Add(RunWP);
            }


            // 呼叫切換到Set Group Tab動作
            if (ChangeToNextStep != null)
            {
                ChangeToNextStep(null, new EventArgs());
            }     

            //////////////////////////////////////////////////////////////////////////
            //DSToBlob
            //  [8/8/2012 autolab]
            //App.proxyMC.DSToBlobCompleted += new EventHandler<DSToBlobCompletedEventArgs>(DSCtoBlobCompletedEvent);
            //App.proxyMC.DSToBlobAsync(pDS_GlobalDI, StateManager.Username,
            //            pDC_GlobalContainer.XTableName, pDC_GlobalContainer.YTableName);


        }

        private void DSCtoBlobCompletedEvent(object sender, DSToBlobCompletedEventArgs e)
        {
            bool IsSuccess = false;

            App.proxyMC.DSToBlobCompleted += new EventHandler<DSToBlobCompletedEventArgs>(DSCtoBlobCompletedEvent);

            try
            {
                IsSuccess = e.Result;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error: " + ex.ToString());
                MessageBox.Show("錯誤訊息: " + ex.ToString());
                IsSuccess = false;
            }

            //Shell.waitingForm.Close();


        }

        #endregion

        #region Common Method
        #endregion
    }
}