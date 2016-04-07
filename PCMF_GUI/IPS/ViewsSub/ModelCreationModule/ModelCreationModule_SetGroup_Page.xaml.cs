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
	public partial class ModelCreationModule_SetGroup_Page : UserControl
	{
        // Define Local Container
        MCSetGroupLocalContainer        pSG_LocalContainer = null;

        // Define Global Container 
        MCDataCollectionGlobalContainer     pDC_GlobalContainer = null;
        MCDataSelectionGlobalContainer      pDS_GlobalContainer = null;
        MCSetGroupGlobalContainer           pSG_GlobalContainer = null;
        MCAbnormalIsolatedGlobalContainer   pAbIso_GlobalContainer = null;

        // Define Global Parameter 
        bool IsAddGroupClicked = false; //To determine whether user click on add button to bind combox or auto bindding combox
        bool IsSelectGroupClicked = false; //To determine whether user click on dropdownlist or not to binding data to grid

        //  [8/8/2012 autolab]
        DCPInfo pDC_GlobalDI = new DCPInfo();
        DCPInfo pDS_GlobalDI = new DCPInfo();
        DCPInfo pSG_GlobalDI = new DCPInfo();

        // Define Global Event
        public event EventHandler ChangeToNextStep;
        public event EventHandler DestroyNextStep;

		public ModelCreationModule_SetGroup_Page()
		{
			// 必須將變數初始化
			InitializeComponent();
            pSG_LocalContainer = new MCSetGroupLocalContainer();
		}

        #region Init
        public void BindingContainer(
            MCDataCollectionGlobalContainer pDC, 
            MCDataSelectionGlobalContainer pDS,
            MCSetGroupGlobalContainer pSG,
            MCAbnormalIsolatedGlobalContainer pAbIso,
            DCPInfo qDC, DCPInfo qDS, DCPInfo qSG)
        {
            pDC_GlobalContainer = pDC;
            pDS_GlobalContainer = pDS;
            pSG_GlobalContainer = pSG;
            pAbIso_GlobalContainer = pAbIso;
            pDC_GlobalDI = qDC;
            pDS_GlobalDI = qDS;
            pSG_GlobalDI = qSG;
        }

        public void InitionPage()
        {
            pSG_LocalContainer.Clear();
            pSG_GlobalContainer.Clear();
                
            IsAddGroupClicked = false;
            IsSelectGroupClicked = false;

            //ui_SG_setUSLPoint.Value = pSG_LocalContainer.InitPoint_USL;
            //ui_SG_setUCLPoint.Value = pSG_LocalContainer.InitPoint_UCL;
            //ui_SG_setLSLPoint.Value = pSG_LocalContainer.InitPoint_LSL;
            //ui_SG_setLCLPoint.Value = pSG_LocalContainer.InitPoint_LCL;
            //ui_SG_setTargetPoint.Value = 0.0;


            // 定義SPEC & TARGET [10/29/2012 autolab]
            ui_SG_setUSLPoint.Value = 50;
            ui_SG_setUCLPoint.Value = 50;
            ui_SG_setLSLPoint.Value = -40;
            ui_SG_setLCLPoint.Value = -40;
            ui_SG_setTargetPoint.Value = 25.0;


            ui_SG_PointListSetting.ItemsSource = null;
            ui_SG_GroupList.ItemsSource = null;

            ui_SG_UngroupedMetrologyList.ItemsSource = null;
            ui_SG_IndicatorActionList1.ItemsSource = null;
            ui_SG_IndicatorActionList2.ItemsSource = null;
            ui_SG_IndicatorActionHeader1.Visibility = Visibility.Collapsed;
            ui_SG_IndicatorActionHeader2.Visibility = Visibility.Collapsed;

            pSG_LocalContainer.SelectedIndicatorTypeList = CommonValue.DataContractClone(pDC_GlobalContainer.SelectedIndicatorTypeList);
            pSG_LocalContainer.SelectedMetrologyTypeList = CommonValue.DataContractClone(pDC_GlobalContainer.SelectedMetrologyTypeList);
            ui_SG_PointListSetting.ItemsSource = pSG_LocalContainer.SelectedMetrologyTypeList;

            //////////////////////////////////////////////////////////////////////////
            //Set Group整個清空
            ui_SG_IndicatorActionHeader1.Visibility = Visibility.Collapsed;
            ui_SG_IndicatorActionHeader2.Visibility = Visibility.Collapsed;
            pSG_LocalContainer.groupList.Clear();
            pSG_LocalContainer.groupCount = 0;
            ui_SG_GroupList.ItemsSource = null;
            ui_SG_UngroupedMetrologyList.ItemsSource = null;
            ui_SG_IndicatorActionList1.ItemsSource = null;
            ui_SG_IndicatorActionList2.ItemsSource = null;
            ui_SG_AddAllGroup.IsEnabled = true;
            ui_SG_AddNewGroup.IsEnabled = true;





            ResetAllPointInformation();
        }
        public void DestroyPage()
        {
            // 呼叫清空NextStep Tab動作
            if (DestroyNextStep != null)
            {
                DestroyNextStep(null, new EventArgs());
            }

            pSG_LocalContainer.Clear();
            pSG_GlobalContainer.Clear();
            pAbIso_GlobalContainer.Clear();

            IsAddGroupClicked = false;
            IsSelectGroupClicked = false;

            ui_SG_setUSLPoint.Value = 0.0;
            ui_SG_setLSLPoint.Value = 0.0;
            ui_SG_setUCLPoint.Value = 0.0;
            ui_SG_setLCLPoint.Value = 0.0;
            ui_SG_setTargetPoint.Value = 0.0;

            ui_SG_PointListSetting.ItemsSource = null;
            ui_SG_GroupList.ItemsSource = null;

            ui_SG_UngroupedMetrologyList.ItemsSource = null;
            ui_SG_IndicatorActionList1.ItemsSource = null;
            ui_SG_IndicatorActionList2.ItemsSource = null;
            ui_SG_IndicatorActionHeader1.Visibility = Visibility.Collapsed;
            ui_SG_IndicatorActionHeader2.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Set Point Spc.
        // 重製所有點位界線
        void ResetAllPointInformation()
        {
            foreach (MetrologyPoint MPitem in ui_SG_PointListSetting.ItemsSource)
            {
                MPitem.USL = ui_SG_setUSLPoint.Value;
                MPitem.LSL = ui_SG_setLSLPoint.Value;
                MPitem.UCL = ui_SG_setUCLPoint.Value;
                MPitem.LCL = ui_SG_setLCLPoint.Value;
                MPitem.TARGET = ui_SG_setTargetPoint.Value;
            }
        }

        // 選取點位並讀取界線
        private void ui_SG_PointListSetting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ui_SG_PointListSetting.SelectedItems.Count == 1)
            {
                IList SelectedPointItems = ui_SG_PointListSetting.SelectedItems;
                if (SelectedPointItems.Count == 1)
                {
                    MetrologyPoint MPitem = (MetrologyPoint)SelectedPointItems[0];
                    ui_SG_setUSLPoint.Value = MPitem.USL;
                    ui_SG_setLSLPoint.Value = MPitem.LSL;
                    ui_SG_setUCLPoint.Value = MPitem.UCL;
                    ui_SG_setLCLPoint.Value = MPitem.LCL;
                    ui_SG_setTargetPoint.Value = MPitem.TARGET;
                }
            }
        }

        // 設定選取點位的界線
        private void ui_SG_SetPointSpcApply_Click(object sender, RoutedEventArgs e)
        {
            IList SelectedPointItems = ui_SG_PointListSetting.SelectedItems;
            if (SelectedPointItems.Count != 0)
            {
                if (ui_SG_setUSLPoint.Value >= ui_SG_setUCLPoint.Value
                 && ui_SG_setUCLPoint.Value > ui_SG_setLCLPoint.Value
                 && ui_SG_setLCLPoint.Value >= ui_SG_setLSLPoint.Value)
                {
                    foreach (MetrologyPoint MPitem in SelectedPointItems)
                    {
                        if (pSG_LocalContainer.SelectedMetrologyTypeList.Contains(MPitem))
                        {
                            MPitem.USL = ui_SG_setUSLPoint.Value;
                            MPitem.LSL = ui_SG_setLSLPoint.Value;
                            MPitem.UCL = ui_SG_setUCLPoint.Value;
                            MPitem.LCL = ui_SG_setLCLPoint.Value;
                            MPitem.TARGET = ui_SG_setTargetPoint.Value;
                        }
                    }
                }
                else
                {
                   // MessageBox.Show("You Must Follow the rule: [USL] >= [UCL] > [LCL] >= [LSL]");
                    MessageBox.Show("你必須遵循這個規則: [USL] >= [UCL] > [LCL] >= [LSL]");
                }
            }
        }

        // 設定所有點位的界線
        private void ui_SG_SetPointSpcApplyAll_Click(object sender, RoutedEventArgs e)
        {
            if (ui_SG_setUSLPoint.Value >= ui_SG_setUCLPoint.Value
             && ui_SG_setUCLPoint.Value > ui_SG_setLCLPoint.Value
             && ui_SG_setLCLPoint.Value >= ui_SG_setLSLPoint.Value)
            {
                foreach (MetrologyPoint MPitem in pSG_LocalContainer.SelectedMetrologyTypeList)
                {
                    MPitem.USL = ui_SG_setUSLPoint.Value;
                    MPitem.LSL = ui_SG_setLSLPoint.Value;
                    MPitem.UCL = ui_SG_setUCLPoint.Value;
                    MPitem.LCL = ui_SG_setLCLPoint.Value;
                    MPitem.TARGET = ui_SG_setTargetPoint.Value;
                }
            }
            else
            {
                //MessageBox.Show("You Must Follow the rule: [USL] >= [UCL] > [LCL] >= [LSL]");
                MessageBox.Show("你必須遵循這個規則: [USL] >= [UCL] > [LCL] >= [LSL]");
            }
        }

        // 設定USL點位值時移除小數點用
        private void ui_SG_setUSLPoint_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (ui_SG_setUSLPoint != null)
            {
                if (ui_SG_setUSLPoint.Value != Math.Round(ui_SG_setUSLPoint.Value, 4))
                {
                    ui_SG_setUSLPoint.Value = Math.Round(ui_SG_setUSLPoint.Value, 4);
                }
            }

        }
        // 設定LSL點位值時移除小數點用
        private void ui_SG_setLSLPoint_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (ui_SG_setLSLPoint != null)
            {
                if (ui_SG_setLSLPoint.Value != Math.Round(ui_SG_setLSLPoint.Value, 4))
                {
                    ui_SG_setLSLPoint.Value = Math.Round(ui_SG_setLSLPoint.Value, 4);
                }
            }
        }
        // 設定UCL點位值時移除小數點用
        private void ui_SG_setUCLPoint_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (ui_SG_setUCLPoint != null)
            {
                if (ui_SG_setUCLPoint.Value != Math.Round(ui_SG_setUCLPoint.Value, 4))
                {
                    ui_SG_setUCLPoint.Value = Math.Round(ui_SG_setUCLPoint.Value, 4);
                }
            }
        }
        // 設定LCL點位值時移除小數點用
        private void ui_SG_setLCLPoint_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (ui_SG_setLCLPoint != null)
            {
                if (ui_SG_setLCLPoint.Value != Math.Round(ui_SG_setLCLPoint.Value, 4))
                {
                    ui_SG_setLCLPoint.Value = Math.Round(ui_SG_setLCLPoint.Value, 4);
                }
            }
        }
        #endregion

        #region Set Group

        // 新增Group
        private void ui_SG_AddNewGroup_Click(object sender, RoutedEventArgs e)
        {
            IsAddGroupClicked = true;       //確認為增加Group 使cmbSetGroupGroupName_SelectionChanged不動作
            IsSelectGroupClicked = false;

            //hide chosen action block.
            ui_SG_IndicatorActionHeader1.Visibility = Visibility.Collapsed;
            ui_SG_IndicatorActionHeader2.Visibility = Visibility.Collapsed;

            if (!CheckAndRemovedInviledGroup(pSG_LocalContainer.groupList, pSG_LocalContainer.groupList.Count - 1))
                pSG_LocalContainer.groupCount++;

            Group GroupTemp = new Group();
            GroupTemp.GroupId = pSG_LocalContainer.groupCount;
            GroupTemp.GroupName = "Group" + pSG_LocalContainer.groupCount.ToString();
            GroupTemp.IndicatorList = new ObservableCollection<MetrologyPoint>();
            GroupTemp.ActionList = new ObservableCollection<ModelCreation.Action>();
            GroupTemp.PointList = new ObservableCollection<MetrologyPoint>();
            pSG_LocalContainer.groupList.Add(GroupTemp);

            ui_SG_GroupList.ItemsSource = null;
            ui_SG_GroupList.ItemsSource = pSG_LocalContainer.groupList;
            ui_SG_GroupList.SelectedValue = GroupTemp.GroupId;

            //create a binding for group point
            pSG_LocalContainer.UngroupedMetrologyTypeList = FindUngroupedPointList(pSG_LocalContainer.groupList, pSG_LocalContainer.SelectedMetrologyTypeList);
            pSG_LocalContainer.checkboxesPoints.Clear();
            ui_SG_UngroupedMetrologyList.ItemsSource = pSG_LocalContainer.UngroupedMetrologyTypeList;
            ui_SG_UngroupedMetrologyList.IsEnabled = true;

            ui_SG_IndicatorActionList1.ItemsSource = null;
            ui_SG_IndicatorActionList1.ItemsSource = pSG_LocalContainer.SelectedIndicatorTypeList;
            ui_SG_IndicatorActionList1.IsEnabled = true;

            ui_SG_IndicatorActionList2.ItemsSource = null;
            ui_SG_IndicatorActionList2.ItemsSource = pSG_LocalContainer.SelectedIndicatorTypeList;
            ui_SG_IndicatorActionList2.IsEnabled = true;

            CheckBox cbAllPoint = null;
            cbAllPoint = GetCheckBoxWithParent(this.ui_SG_IndicatorActionList1, typeof(CheckBox), "ui_SG_CheckAllIndicatorActionList1");
            if (cbAllPoint != null)
                cbAllPoint.IsChecked = false;
            cbAllPoint = GetCheckBoxWithParent(this.ui_SG_IndicatorActionList2, typeof(CheckBox), "ui_SG_CheckAllIndicatorActionList2");
            if (cbAllPoint != null)
                cbAllPoint.IsChecked = false;

            IsAddGroupClicked = false;
        }

        // 找尋尚未加入Group的點
        List<MetrologyPoint> FindUngroupedPointList(ObservableCollection<Group> ExistGroupList, ObservableCollection<MetrologyPoint> SelectedMetrologyPointList)
        {
            List<MetrologyPoint> OutputListYPoint = new List<MetrologyPoint>();
            bool IsExistInGroup = false;
            foreach (MetrologyPoint point in SelectedMetrologyPointList)
            {
                IsExistInGroup = false;
                foreach (Group gr in ExistGroupList)
                {
                    foreach (MetrologyPoint p in gr.PointList)
                    {
                        if (point.Name == p.Name)
                        {
                            IsExistInGroup = true;
                            break;
                        }
                    }
                    //check another group
                    if (IsExistInGroup)   //if the point exists
                        break;
                }

                if (!IsExistInGroup)  //if the point doesn't exist in any group
                {
                    OutputListYPoint.Add(point);
                }
            }
            return OutputListYPoint;
        }

        //移除單項Group
        private void ui_SG_DelSelectedGroup_Click(object sender, RoutedEventArgs e)
        {
            if (pSG_LocalContainer.groupList.Count > 0)
            {
                Group SelectedGroup = null;
                if (pSG_LocalContainer.groupList.Count == 1) //初始化
                {
                    SelectedGroup = pSG_LocalContainer.groupList[0];
                    //if (MessageBox.Show("Delete Group [" + SelectedGroup.GroupName + "] ?", "Attention Please!", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    //{
                    //    pSG_LocalContainer.groupList.Clear();
                    //    pSG_LocalContainer.groupCount = 0;
                    //    ui_SG_GroupList.ItemsSource = null;
                    //    ui_SG_IndicatorActionHeader1.Visibility = System.Windows.Visibility.Collapsed;
                    //    ui_SG_IndicatorActionHeader2.Visibility = System.Windows.Visibility.Collapsed;
                    //    ui_SG_UngroupedMetrologyList.ItemsSource = null;
                    //    ui_SG_IndicatorActionList1.ItemsSource = null;
                    //    ui_SG_IndicatorActionList2.ItemsSource = null;
                    //}
                    if (MessageBox.Show("確定刪除此群組[" + SelectedGroup.GroupName + "] ?", "注意!", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        pSG_LocalContainer.groupList.Clear();
                        pSG_LocalContainer.groupCount = 0;
                        ui_SG_GroupList.ItemsSource = null;
                        ui_SG_IndicatorActionHeader1.Visibility = System.Windows.Visibility.Collapsed;
                        ui_SG_IndicatorActionHeader2.Visibility = System.Windows.Visibility.Collapsed;
                        ui_SG_UngroupedMetrologyList.ItemsSource = null;
                        ui_SG_IndicatorActionList1.ItemsSource = null;
                        ui_SG_IndicatorActionList2.ItemsSource = null;
                    }
                }
                else
                {
                    //取回欲刪除的Group
                    if (ui_SG_GroupList.SelectedValue != null)
                    {
                        foreach (Group GPTemp in pSG_LocalContainer.groupList)
                        {
                            if ((int)ui_SG_GroupList.SelectedValue == GPTemp.GroupId)
                            {
                                SelectedGroup = GPTemp;
                                break;
                            }
                        }
                    }

                    if (SelectedGroup == null)
                    {
                        //MessageBox.Show("There are errors on the client. can not find group in groupList.");
                        MessageBox.Show("使用者端發生一些錯誤. 群組列表找不到此群組.");
                        return;
                    }

                    //if (MessageBox.Show("Delete Group [" + SelectedGroup.GroupName + "] ?", "Attention Please!", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    //{
                    //    pSG_LocalContainer.groupList.Remove(SelectedGroup);
                    //    ui_SG_IndicatorActionHeader1.Visibility = System.Windows.Visibility.Collapsed;
                    //    ui_SG_IndicatorActionHeader2.Visibility = System.Windows.Visibility.Collapsed;
                    //    ui_SG_GroupList.SelectedIndex = 0;
                    //}
                    if (MessageBox.Show("確定刪除此群組 [" + SelectedGroup.GroupName + "] ?", "注意!", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        pSG_LocalContainer.groupList.Remove(SelectedGroup);
                        ui_SG_IndicatorActionHeader1.Visibility = System.Windows.Visibility.Collapsed;
                        ui_SG_IndicatorActionHeader2.Visibility = System.Windows.Visibility.Collapsed;
                        ui_SG_GroupList.SelectedIndex = 0;
                    }
                }
            }
        }

        //移除All Group
        private void ui_SG_DelAllGroup_Click(object sender, RoutedEventArgs e)
        {
            //if (MessageBox.Show("Delete All Groups?", "Attention Please!", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            //{
            //    ui_SG_IndicatorActionHeader1.Visibility = Visibility.Collapsed;
            //    ui_SG_IndicatorActionHeader2.Visibility = Visibility.Collapsed;
            //    pSG_LocalContainer.groupList.Clear();
            //    pSG_LocalContainer.groupCount = 0;
            //    ui_SG_GroupList.ItemsSource = null;
            //    ui_SG_UngroupedMetrologyList.ItemsSource = null;
            //    ui_SG_IndicatorActionList1.ItemsSource = null;
            //    ui_SG_IndicatorActionList2.ItemsSource = null;
            //    ui_SG_AddAllGroup.IsEnabled = true;
            //    ui_SG_AddNewGroup.IsEnabled = true;
            //}
            if (MessageBox.Show("確定刪除所有群組?", "注意!", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                ui_SG_IndicatorActionHeader1.Visibility = Visibility.Collapsed;
                ui_SG_IndicatorActionHeader2.Visibility = Visibility.Collapsed;
                pSG_LocalContainer.groupList.Clear();
                pSG_LocalContainer.groupCount = 0;
                ui_SG_GroupList.ItemsSource = null;
                ui_SG_UngroupedMetrologyList.ItemsSource = null;
                ui_SG_IndicatorActionList1.ItemsSource = null;
                ui_SG_IndicatorActionList2.ItemsSource = null;
                ui_SG_AddAllGroup.IsEnabled = true;
                ui_SG_AddNewGroup.IsEnabled = true;
            }
        }

        // 自動新增所有點位
        private void ui_SG_AddAllGroup_Click(object sender, RoutedEventArgs e)
        {
            ui_SG_AddAllGroup.IsEnabled = false;
            ui_SG_AddNewGroup.IsEnabled = false;

            if (pSG_LocalContainer.groupList.Count > 0)
            {
                //if (MessageBox.Show("You need \"Clear All\" Exist Groups!", "Warning", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                if (MessageBox.Show("你需要 \"Clear All\" 存在的群組!", "警告", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    ui_SG_IndicatorActionHeader1.Visibility = Visibility.Collapsed;
                    ui_SG_IndicatorActionHeader2.Visibility = Visibility.Collapsed;
                    pSG_LocalContainer.groupList.Clear();
                    pSG_LocalContainer.groupCount = 0;
                    ui_SG_GroupList.ItemsSource = null;
                    ui_SG_UngroupedMetrologyList.ItemsSource = null;
                    ui_SG_IndicatorActionList1.ItemsSource = null;
                    ui_SG_IndicatorActionList2.ItemsSource = null;
                }
                else
                {
                    return;
                }
            }
            //////////////////////////////////////////////////////////////////////////
            pSG_LocalContainer.UngroupedMetrologyTypeList = FindUngroupedPointList(pSG_LocalContainer.groupList, pSG_LocalContainer.SelectedMetrologyTypeList);
            while (pSG_LocalContainer.UngroupedMetrologyTypeList.Count > 0)
            {
                ui_SG_AddNewGroup_Click(this, new RoutedEventArgs());
                ui_SG_UngroupedMetrologyList.SelectedIndex = 0;
                CheckBox chk = new CheckBox();
                chk.IsChecked = true;
                ui_SG_CheckUngroupedMetrologyList_Click(chk, new RoutedEventArgs());
                if (pSG_LocalContainer.groupList[pSG_LocalContainer.groupList.Count - 1].ActionList.Count >= 1)
                {
                    chk = new CheckBox();
                    chk.IsChecked = true;
                    ui_SG_CheckAllIndicatorActionList1_Click(chk, new RoutedEventArgs());
                }

                if (pSG_LocalContainer.groupList[pSG_LocalContainer.groupList.Count - 1].ActionList.Count >= 2)
                {
                    chk = new CheckBox();
                    chk.IsChecked = true;
                    ui_SG_CheckAllIndicatorActionList2_Click(chk, new RoutedEventArgs());
                }
                pSG_LocalContainer.UngroupedMetrologyTypeList = FindUngroupedPointList(pSG_LocalContainer.groupList, pSG_LocalContainer.SelectedMetrologyTypeList);
            }
            ui_SG_GroupList.SelectedIndex = 0;
        }

        // 移除沒有選取Point或Action的Group 有移除 回傳true
        bool CheckAndRemovedInviledGroup(ObservableCollection<Group> beCheckGroupList, int ListIndex)
        {
           // Shell.waitingForm.SettingMessage("Create and Initial User Processing Storage");
            Shell.waitingForm.SettingMessage("建立和初始化使用者的運行的儲存體");
            Shell.waitingForm.Show();

            if (beCheckGroupList.Count > 0)
            {
                if (beCheckGroupList[ListIndex].PointList.Count == 0)
                {
                    beCheckGroupList.RemoveAt(ListIndex);  // 沒有選擇Point 直接刪除
                    return true;
                }
                else
                {
                    if (beCheckGroupList[ListIndex].ActionList.Count == 1)
                    {
                        if (beCheckGroupList[ListIndex].ActionList[0].IndicatorList.Count == 0)
                        {
                            beCheckGroupList.RemoveAt(ListIndex);  // 沒有選擇Action 直接刪除
                            return true;
                        }
                    }
                    else
                    {
                        if (beCheckGroupList[ListIndex].ActionList[0].IndicatorList.Count == 0 && beCheckGroupList[ListIndex].ActionList[1].IndicatorList.Count == 0)
                        {
                            beCheckGroupList.RemoveAt(ListIndex);  // 沒有選擇Action1 & Action2 直接刪除
                            return true;
                        }
                    }
                }
            }

            Shell.waitingForm.Close();
            return false;
        }

        // Metrology Binding
        private void ui_SG_UngroupedMetrologyList_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            CheckBox chk = ui_SG_UngroupedMetrologyList.Columns[0].GetCellContent(e.Row) as CheckBox;
            if (IsSelectGroupClicked)
                chk.IsChecked = true;
            else
                chk.IsChecked = false;
            pSG_LocalContainer.checkboxesPoints.Add(chk);
        }

        //選擇 Metrology
        private void ui_SG_CheckUngroupedMetrologyList_Click(object sender, RoutedEventArgs e)
        {
            ui_SG_IndicatorActionHeader1.Visibility = Visibility.Collapsed;
            ui_SG_IndicatorActionHeader2.Visibility = Visibility.Collapsed;

            CheckBox cb = sender as CheckBox;
            MetrologyPoint CurrentSelectedItem = (MetrologyPoint)ui_SG_UngroupedMetrologyList.SelectedItem;
            string[] ActionsArray = CurrentSelectedItem.Actions.Split(',');
            pSG_LocalContainer.checkboxesAction1.Clear();
            pSG_LocalContainer.checkboxesAction2.Clear();

            int CurrentGroupIndex = pSG_LocalContainer.groupList.Count - 1;
            if (cb.IsChecked.Value)
            {
                // 取消之前選取項目
                if (pSG_LocalContainer.groupList[CurrentGroupIndex].PointList.Count != 0)
                {
                    MetrologyPoint MPTemp = pSG_LocalContainer.groupList[CurrentGroupIndex].PointList[0];
                    CheckBox OldMPChk = (CheckBox)ui_SG_UngroupedMetrologyList.Columns[0].GetCellContent(MPTemp);
                    OldMPChk.IsChecked = false;
                    pSG_LocalContainer.groupList[CurrentGroupIndex].PointList.Clear();
                    pSG_LocalContainer.groupList[CurrentGroupIndex].ActionList.Clear();
                    pSG_LocalContainer.groupList[CurrentGroupIndex].IndicatorList.Clear();

                    ui_SG_IndicatorActionList1.ItemsSource = null;
                    ui_SG_IndicatorActionList1.ItemsSource = pSG_LocalContainer.SelectedIndicatorTypeList;
                    ui_SG_IndicatorActionList2.ItemsSource = null;
                    ui_SG_IndicatorActionList2.ItemsSource = pSG_LocalContainer.SelectedIndicatorTypeList;
                    CheckBox CBTemp = null;
                    CBTemp = GetCheckBoxWithParent(this.ui_SG_IndicatorActionList1, typeof(CheckBox), "ui_SG_CheckAllIndicatorActionList1");
                    if (CBTemp != null) CBTemp.IsChecked = false;
                    CBTemp = GetCheckBoxWithParent(this.ui_SG_IndicatorActionList2, typeof(CheckBox), "ui_SG_CheckAllIndicatorActionList2");
                    if (CBTemp != null) CBTemp.IsChecked = false;
                }

                //新增新選取的項目
                if (CheckExistMeasureType(CurrentSelectedItem.MeasureType.Trim(), pSG_LocalContainer.groupList))
                {
                    pSG_LocalContainer.groupList[CurrentGroupIndex].PointList.Add(CurrentSelectedItem);

                    if (ActionsArray.Length == 1)
                    {
                        ui_SG_IndicatorActionHeader1.Header = "Block " + ActionsArray[0];
                        ui_SG_IndicatorActionHeader1.Visibility = Visibility.Visible;
                        ModelCreation.Action action = new ModelCreation.Action();
                        action.IndicatorList = new ObservableCollection<MetrologyPoint>();
                        action.ActionNumber = Int32.Parse(ActionsArray[0]);
                        pSG_LocalContainer.groupList[CurrentGroupIndex].ActionList.Add(action);
                        // 增加上Metrology項目的名稱
                        pSG_LocalContainer.groupList[CurrentGroupIndex].GroupName = "Group" + pSG_LocalContainer.groupList[CurrentGroupIndex].GroupId.ToString() + " - " + CurrentSelectedItem.Name;
                    }
                    else if (ActionsArray.Length == 2)
                    {
                        ui_SG_IndicatorActionHeader1.Header = "Block " + ActionsArray[0];
                        ui_SG_IndicatorActionHeader2.Header = "Block " + ActionsArray[1];
                        ui_SG_IndicatorActionHeader1.Visibility = Visibility.Visible;
                        ui_SG_IndicatorActionHeader2.Visibility = Visibility.Visible;
                        ModelCreation.Action action1 = new ModelCreation.Action();
                        ModelCreation.Action action2 = new ModelCreation.Action();
                        action1.IndicatorList = new ObservableCollection<MetrologyPoint>();
                        action2.IndicatorList = new ObservableCollection<MetrologyPoint>();
                        action1.ActionNumber = Int32.Parse(ActionsArray[0]);
                        action2.ActionNumber = Int32.Parse(ActionsArray[1]);
                        pSG_LocalContainer.groupList[CurrentGroupIndex].ActionList.Add(action1);
                        pSG_LocalContainer.groupList[CurrentGroupIndex].ActionList.Add(action2);
                        // 增加上Metrology項目的名稱
                        pSG_LocalContainer.groupList[CurrentGroupIndex].GroupName = "Group" + pSG_LocalContainer.groupList[CurrentGroupIndex].GroupId.ToString() + " - " + CurrentSelectedItem.Name;
                    }

                    //cbAll.IsChecked = false;
                }
                else
                {
                    cb.IsChecked = false;
                    ui_SG_IndicatorActionHeader1.Visibility = Visibility.Visible;
                    ui_SG_IndicatorActionHeader2.Visibility = Visibility.Visible;
                    //MessageBox.Show("You can Select Only \"One\" Metrology Type in a Group.");
                    MessageBox.Show("在這個群組你只能選擇 \"One\" 量測型態");
                }
            }
            else
            {
                int PointIndex = pSG_LocalContainer.groupList[CurrentGroupIndex].PointList.IndexOf(CurrentSelectedItem);
                int CountPoint = 0;
                for (int i = 0; i < PointIndex; i++)
                {
                    string[] ExistActions = pSG_LocalContainer.groupList[CurrentGroupIndex].PointList[i].Actions.Split(',');
                    CountPoint += ExistActions.Length;
                }
                for (int i = 0; i < ActionsArray.Length; i++)
                {
                    pSG_LocalContainer.groupList[CurrentGroupIndex].ActionList.RemoveAt(CountPoint);
                }
                pSG_LocalContainer.groupList[CurrentGroupIndex].PointList.Remove(CurrentSelectedItem);
                ui_SG_IndicatorActionList1.ItemsSource = null;
                ui_SG_IndicatorActionList1.ItemsSource = pSG_LocalContainer.SelectedIndicatorTypeList;
                GetObCollectionByCheckList(pDC_GlobalContainer.IndicatorTypeList, pDC_GlobalContainer.IndicatorTypeCheckStateList);
                ui_SG_IndicatorActionList2.ItemsSource = null;
                ui_SG_IndicatorActionList2.ItemsSource = pSG_LocalContainer.SelectedIndicatorTypeList;
                GetObCollectionByCheckList(pDC_GlobalContainer.IndicatorTypeList, pDC_GlobalContainer.IndicatorTypeCheckStateList);
            }
        }

        // check whether there are a typre measure is duplicated or not    - Set group function
        bool CheckExistMeasureType(string typeName, ObservableCollection<Group> inGroupList)
        {
            int CurrentSelectedIndex = inGroupList.Count - 1;
            foreach (MetrologyPoint point in inGroupList[CurrentSelectedIndex].PointList)
            {
                if (!typeName.Equals(point.MeasureType.Trim()))
                    return false;
            }
            return true;
        }

        //Action 1 Binding
        private void ui_SG_IndicatorActionList1_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            CheckBox chk = ui_SG_IndicatorActionList1.Columns[0].GetCellContent(e.Row) as CheckBox;
            if (IsSelectGroupClicked)
                chk.IsChecked = true;
            else
                chk.IsChecked = false;
            pSG_LocalContainer.checkboxesAction1.Add(chk);
        }

        //Action 2 Binding
        private void ui_SG_IndicatorActionList2_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            CheckBox chk = ui_SG_IndicatorActionList2.Columns[0].GetCellContent(e.Row) as CheckBox;
            if (IsSelectGroupClicked)
                chk.IsChecked = true;
            else
                chk.IsChecked = false;
            pSG_LocalContainer.checkboxesAction2.Add(chk);
        }

        // Action1 單選
        private void ui_SG_CheckIndicatorActionList1_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cbAll = GetCheckBoxWithParent(this.ui_SG_IndicatorActionList1, typeof(CheckBox), "ui_SG_CheckAllIndicatorActionList1");
            if (cbAll != null)
            {
                bool isAllSelected = true; //用來確認是否有全部勾選

                CheckBox cb = sender as CheckBox;
                int CurrentSelectedIndex = pSG_LocalContainer.groupList.Count - 1;
                MetrologyPoint selectedGroupPointItem = ui_SG_UngroupedMetrologyList.SelectedItem as MetrologyPoint;

                int PointIndex = pSG_LocalContainer.groupList[CurrentSelectedIndex].PointList.IndexOf(selectedGroupPointItem);


                int CountPoint = 0;
                for (int i = 0; i < PointIndex; i++)
                {
                    string[] ExistActions = pSG_LocalContainer.groupList[CurrentSelectedIndex].PointList[i].Actions.Split(',');
                    CountPoint += ExistActions.Length;
                }

                pSG_LocalContainer.groupList[CurrentSelectedIndex].ActionList[CountPoint].IndicatorList.Clear(); //將列表清空

                for (int i = 0; i < pSG_LocalContainer.checkboxesAction1.Count; i++)
                {
                    if (pSG_LocalContainer.checkboxesAction1[i].IsChecked == true)
                    {
                        pSG_LocalContainer.groupList[CurrentSelectedIndex].ActionList[CountPoint].IndicatorList.Add(pSG_LocalContainer.SelectedIndicatorTypeList[i]); //將有勾選的加入列表
                    }
                    else
                    {
                        isAllSelected = false;
                    }
                }
                cbAll.IsChecked = isAllSelected;
            }
        }

        // Action2 單選
        private void ui_SG_CheckIndicatorActionList2_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cbAll = GetCheckBoxWithParent(this.ui_SG_IndicatorActionList2, typeof(CheckBox), "ui_SG_CheckAllIndicatorActionList2");
            if (cbAll != null)
            {
                bool isAllSelected = true; //用來確認是否有全部勾選

                CheckBox cb = sender as CheckBox;
                int CurrentSelectedIndex = pSG_LocalContainer.groupList.Count - 1;
                MetrologyPoint selectedGroupPointItem = ui_SG_UngroupedMetrologyList.SelectedItem as MetrologyPoint;
                int PointIndex = pSG_LocalContainer.groupList[CurrentSelectedIndex].PointList.IndexOf(selectedGroupPointItem);

                int CountPoint = 0;
                for (int i = 0; i < PointIndex; i++)
                {
                    string[] ExistActions = pSG_LocalContainer.groupList[CurrentSelectedIndex].PointList[i].Actions.Split(',');
                    CountPoint += ExistActions.Length;
                }

                pSG_LocalContainer.groupList[CurrentSelectedIndex].ActionList[CountPoint + 1].IndicatorList.Clear(); //將列表清空
                for (int i = 0; i < pSG_LocalContainer.checkboxesAction2.Count; i++)
                {
                    if (pSG_LocalContainer.checkboxesAction2[i].IsChecked == true)
                    {
                        pSG_LocalContainer.groupList[CurrentSelectedIndex].ActionList[CountPoint + 1].IndicatorList.Add(pSG_LocalContainer.SelectedIndicatorTypeList[i]); //將有勾選的加入列表
                    }
                    else
                    {
                        isAllSelected = false;
                    }
                }
                cbAll.IsChecked = isAllSelected;
            }
        }

        // Action1 全選
        private void ui_SG_CheckAllIndicatorActionList1_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            bool check = chk.IsChecked.Value;
            foreach (CheckBox chkbox in pSG_LocalContainer.checkboxesAction1)
                chkbox.IsChecked = check;

            int CurrentSelectedIndex = pSG_LocalContainer.groupList.Count - 1;

            pSG_LocalContainer.groupList[CurrentSelectedIndex].ActionList[0].IndicatorList.Clear();
            if (check)
            {
                foreach (MetrologyPoint item in pSG_LocalContainer.SelectedIndicatorTypeList)
                {
                    pSG_LocalContainer.groupList[CurrentSelectedIndex].ActionList[0].IndicatorList.Add(item);
                }
            }
        }

        // Action2 全選
        private void ui_SG_CheckAllIndicatorActionList2_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            bool check = chk.IsChecked.Value;
            foreach (CheckBox chkbox in pSG_LocalContainer.checkboxesAction2)
                chkbox.IsChecked = check;

            int CurrentSelectedIndex = pSG_LocalContainer.groupList.Count - 1;

            pSG_LocalContainer.groupList[CurrentSelectedIndex].ActionList[1].IndicatorList.Clear();
            if (check)
            {
                foreach (MetrologyPoint item in pSG_LocalContainer.SelectedIndicatorTypeList)
                {
                    pSG_LocalContainer.groupList[CurrentSelectedIndex].ActionList[1].IndicatorList.Add(item);
                }
            }
        }

        // 切換Group
        private void ui_SG_GroupList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsAddGroupClicked) // 確定此次事件不是由AddGroup Event引起
            {
                //移除沒有新增成功的Group
                if (CheckAndRemovedInviledGroup(pSG_LocalContainer.groupList, pSG_LocalContainer.groupList.Count - 1))
                    pSG_LocalContainer.groupCount--;


                if (ui_SG_GroupList.SelectedValue != null)
                {
                    ui_SG_IndicatorActionHeader1.Visibility = System.Windows.Visibility.Collapsed;
                    ui_SG_IndicatorActionHeader2.Visibility = System.Windows.Visibility.Collapsed;

                    //取回設定好的Group
                    Group SelectedGroup = null;
                    foreach (Group GPTemp in pSG_LocalContainer.groupList)
                    {
                        if ((int)ui_SG_GroupList.SelectedValue == GPTemp.GroupId)
                        {
                            SelectedGroup = GPTemp;
                            break;
                        }
                    }

                    if (SelectedGroup == null)
                    {
                        //MessageBox.Show("There are some errors on the client. Couldn't find the group in groupList.");
                        MessageBox.Show("使用者端發生一些錯誤. 群組列表找不到此群組.");
                        return;
                    }

                    ui_SG_UngroupedMetrologyList.ItemsSource = null;
                    ui_SG_UngroupedMetrologyList.ItemsSource = SelectedGroup.PointList;
                    ui_SG_UngroupedMetrologyList.IsEnabled = false;

                    switch (SelectedGroup.ActionList.Count)
                    {
                        case 1:
                            ui_SG_IndicatorActionList1.ItemsSource = null;
                            ui_SG_IndicatorActionList1.ItemsSource = SelectedGroup.ActionList[0].IndicatorList;
                            ui_SG_IndicatorActionList1.IsEnabled = false;

                            ui_SG_IndicatorActionHeader1.Header = "Block " + SelectedGroup.ActionList[0].ActionNumber;
                            ui_SG_IndicatorActionHeader1.Visibility = System.Windows.Visibility.Visible;
                            break;

                        case 2:
                            ui_SG_IndicatorActionList1.ItemsSource = null;
                            ui_SG_IndicatorActionList1.ItemsSource = SelectedGroup.ActionList[0].IndicatorList;
                            ui_SG_IndicatorActionList1.IsEnabled = false;

                            ui_SG_IndicatorActionList2.ItemsSource = null;
                            ui_SG_IndicatorActionList2.ItemsSource = SelectedGroup.ActionList[1].IndicatorList;
                            ui_SG_IndicatorActionList2.IsEnabled = false;

                            ui_SG_IndicatorActionHeader1.Header = "Block " + SelectedGroup.ActionList[0].ActionNumber;
                            ui_SG_IndicatorActionHeader2.Header = "Block " + SelectedGroup.ActionList[1].ActionNumber;
                            ui_SG_IndicatorActionHeader1.Visibility = System.Windows.Visibility.Visible;
                            ui_SG_IndicatorActionHeader2.Visibility = System.Windows.Visibility.Visible;
                            break;

                    }
                    IsSelectGroupClicked = true;
                }
            }
        }

        #endregion

        #region Next Step
        private void ui_SG_NextStep_Click(object sender, RoutedEventArgs e)
        {
            ui_SG_NextStep.IsEnabled = false;
            if (CheckAndRemovedInviledGroup(pSG_LocalContainer.groupList, pSG_LocalContainer.groupList.Count - 1))
            {
                pSG_LocalContainer.groupCount--;
                if (pSG_LocalContainer.groupList.Count <= 0)
                {
                    ui_SG_UngroupedMetrologyList.ItemsSource = null;
                    ui_SG_UngroupedMetrologyList.IsEnabled = false;
                    ui_SG_IndicatorActionHeader1.Visibility = Visibility.Collapsed;
                    ui_SG_IndicatorActionHeader2.Visibility = Visibility.Collapsed;
                }
            }

            if (pSG_LocalContainer.groupList.Count <= 0)
            {
                MessageBox.Show("You should Select at least one Group!");
                MessageBox.Show("你必須選擇至少一個群組!");
                ui_SG_NextStep.IsEnabled = true;
                return;
            }
            else
            {
                ui_SG_GroupList.SelectedIndex = 0;
            }

            // 檢查Train數與Indicator數的限制，並取回最大的Inditacor限制數
            List<string> OutSpecGroupNameList = CheckIndicatorLimit(out pSG_LocalContainer.MaximumIndicatorCount);
            if (OutSpecGroupNameList != null)
            {
                string strMessage = "";
                foreach (string OutSpecGroupName in OutSpecGroupNameList)
                {
                    strMessage += "\n  " + OutSpecGroupName;
                }
                //MessageBox.Show("These Groups are over the Rule ( Train Count > Indicator Count +5 )!" + strMessage);
                MessageBox.Show("這些群組超過限制 ( Train Count > Indicator Count +5 )!" + strMessage);
                ui_SG_NextStep.IsEnabled = true;
                return;
            }

            // 在進入這一步之後 所有的失敗都要移除NextStep的資料
            InitMCSetGroupGlobalContainer(); // 產生gSG_GlobalContainer
            InitAbnormalIsolatedContainer(); // 產生Isolated和Abnormal的資料


            pSG_GlobalDI.X_Data = new ObservableCollection<XItem>();
            pSG_GlobalDI.Y_Data = new ObservableCollection<YItem>();
            pSG_GlobalDI.WP_Data = new ObservableCollection<AllPiece>();
            pSG_GlobalDI.WP_TrainData = new ObservableCollection<TrainPiece>();
            pSG_GlobalDI.WP_RunData = new ObservableCollection<RunPiece>();

            pSG_GlobalDI.CurrentInfo = pDS_GlobalDI.CurrentInfo;
            pSG_GlobalDI.WP_Data = pDS_GlobalDI.WP_Data;
            pSG_GlobalDI.X_Data = pDS_GlobalDI.X_Data;
            pSG_GlobalDI.Y_Data = pDS_GlobalDI.Y_Data;
            pSG_GlobalDI.WP_TrainData = pDS_GlobalDI.WP_TrainData;
            pSG_GlobalDI.WP_RunData = pDS_GlobalDI.WP_RunData;
            pSG_GlobalDI.StartTime = pDS_GlobalDI.StartTime;
            pSG_GlobalDI.EndTime = pDS_GlobalDI.EndTime;



            //////////////////////////////////////////////////////////////////////////
            //GSToBlob
            //  [8/8/2012 autolab]
            //App.proxyMC.SGToBlobCompleted += new EventHandler<SGToBlobCompletedEventArgs>(SGCtoBlobCompletedEvent);
            //App.proxyMC.SGToBlobAsync(pSG_GlobalDI, StateManager.Username,
            //            pDC_GlobalContainer.XTableName, pDC_GlobalContainer.YTableName);


            try
            {                
                //Shell.waitingForm.SettingMessage("Create and Initial User Processing Storage");
                Shell.waitingForm.SettingMessage("建立和初始化使用者執行儲存體");
                Shell.waitingForm.Show();
                App.proxyMC.InitTempleFolderCompleted += new EventHandler<InitTempleFolderCompletedEventArgs>(InitTempleFolderCompletedEvent);
                App.proxyMC.InitTempleFolderAsync(new In_UserInfo() { Company = pDC_GlobalContainer.Company, User = pDC_GlobalContainer.LoginUsername });
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error: " + ex.ToString());
                MessageBox.Show("錯誤訊息: " + ex.ToString());
                ui_SG_NextStep.IsEnabled = true;
            }
        }

        private void SGCtoBlobCompletedEvent(object sender, SGToBlobCompletedEventArgs e)
        {
            bool IsSuccess = false;

            App.proxyMC.SGToBlobCompleted -= new EventHandler<SGToBlobCompletedEventArgs>(SGCtoBlobCompletedEvent);

            try
            {
                IsSuccess = e.Result;
                if (IsSuccess)
                {
                    //Shell.waitingForm.SettingMessage("Create and Initial User Processing Storage");
                    Shell.waitingForm.SettingMessage("建立和初始化使用者執行儲存體");
                    Shell.waitingForm.Show();
                    App.proxyMC.InitTempleFolderCompleted += new EventHandler<InitTempleFolderCompletedEventArgs>(InitTempleFolderCompletedEvent);
                    App.proxyMC.InitTempleFolderAsync(new In_UserInfo() { Company = pDC_GlobalContainer.Company, User = pDC_GlobalContainer.LoginUsername });
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error: " + ex.ToString());
                MessageBox.Show("錯誤訊息: " + ex.ToString());
                IsSuccess = false;
            }

            //Shell.waitingForm.Close();


        }

        private void InitTempleFolderCompletedEvent(object sender, EventArgs e)
        {
            App.proxyMC.InitTempleFolderCompleted -= new EventHandler<InitTempleFolderCompletedEventArgs>(InitTempleFolderCompletedEvent);
            bool Ack = false;
            try
            {
                int result = ((InitTempleFolderCompletedEventArgs)e).Result;
                if (result == 0)
                {
                    Ack = true;
                }
                else
                {
                    MessageBox.Show("Error: Initial Temple Model Folder are fail! Please try it again.");
                    MessageBox.Show("錯誤訊息: 初始化暫存模型資料夾失敗!再試一次");
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error: " + ex.ToString());
                MessageBox.Show("錯誤訊息: " + ex.ToString());
            }

            Shell.waitingForm.Close();

            if (Ack)
            {
                //////////////////////////////////////////////////////////////////////////
                //非同步呼叫建立ModelInfo.xml檔案

                App.proxyMC.CreateXMLCompleted += new EventHandler<CreateXMLCompletedEventArgs>(CreateXMLCompletedEvent);
                App.proxyMC.CreateXMLAsync(new In_UserInfo() { Company = pDC_GlobalContainer.Company, User = pDC_GlobalContainer.LoginUsername });

                //////////////////////////////////////////////////////////////////////////
                                
            }
            else 
            {
                // 呼叫清空NextStep Tab動作 然後停在原地
                if (DestroyNextStep != null)
                {
                    DestroyNextStep(null, new EventArgs());
                }
            }
        }

        private void CreateXMLCompletedEvent(object sender, EventArgs e)
        {
            bool IsSuccess = false;
            App.proxyMC.CreateXMLCompleted -= new EventHandler<CreateXMLCompletedEventArgs>(CreateXMLCompletedEvent);
            try
            {
                IsSuccess = ((CreateXMLCompletedEventArgs)e).Result;

                if (IsSuccess)
                {
                    ExecuteDataTranferModule(pDC_GlobalContainer.XTableName, pDC_GlobalContainer.YTableName);
                }

            }
            catch (Exception ex)
            {
                IsSuccess = false;
                MessageBox.Show("錯誤訊息: " + ex.ToString());
                //MessageBox.Show("Error: " + ex.ToString());
            }
        }

        void ExecuteDataTranferModule(String XTable, String YTable)
        {
            ModelCreationModuleAlgorithmFlowControler MCAlgrControler = new ModelCreationModuleAlgorithmFlowControler(this.Dispatcher);
            MCAlgrControler.ExecuteDataTranferModule_Fail += new EventHandler(ExecuteDataTranferModule_Fail);
            MCAlgrControler.ExecuteDataTranferModule_Finish += new EventHandler(ExecuteDataTranferModule_Finish);

            //Shell.waitingForm.SettingMessage("Execute Module[Data Transfer]: ");
            Shell.waitingForm.SettingMessage("執行模組[Data Transfer]: ");
            Shell.waitingForm.SettingTip("");
            Shell.waitingForm.Show();

            MCAlgrControler.ExecuteDataTranferModule(
                pDC_GlobalContainer.TotalcontextListCount,
                pDS_GlobalContainer.TrainingCount,
                pDS_GlobalContainer.RunningCount,
                pSG_GlobalContainer.SelectedIndicatorTypeList,
                pSG_GlobalContainer.SelectedMetrologyTypeList,
                pDC_GlobalContainer.SearchStartTime,
                pDC_GlobalContainer.SearchEndTime,
                pSG_GlobalContainer.combinedIndicator,
                pSG_GlobalContainer.combinedPoint,
                pSG_GlobalContainer.groupList,
                pDC_GlobalContainer.vMachine,
                pDC_GlobalContainer.CNCType,
                pDC_GlobalContainer.CNCnumber,
                pDC_GlobalContainer.NCprogram,
                pDC_GlobalContainer.model_Id,
                pDC_GlobalContainer.version,
                pDC_GlobalContainer.allAction,
                pAbIso_GlobalContainer.checkboxesAbnormalStatusOC,
                pAbIso_GlobalContainer.checkboxesIsolatedStatusOC,
                pDC_GlobalContainer.XTableName,
                pDC_GlobalContainer.YTableName,
                pDC_GlobalContainer.strSelectedContextID,
                pDC_GlobalContainer.ProductID,
                pDC_GlobalContainer.Company, pDC_GlobalContainer.LoginUsername
                );
        }
        void ExecuteDataTranferModule_Fail(object sender, EventArgs e)
        {
            ui_SG_NextStep.IsEnabled = true;
            Shell.waitingForm.Close();
            // 呼叫清空NextStep Tab動作
            if (DestroyNextStep != null)
            {
                DestroyNextStep(null, new EventArgs());
            }
        }
        void ExecuteDataTranferModule_Finish(object sender, EventArgs e)
        {
            ui_SG_NextStep.IsEnabled = true;
            bool IsSuccess = false;
            try
            {
                int result = ((Get_DataTransferResultCompletedEventArgs)e).Result;

                if (result == 0)
                {
                    IsSuccess = true;
                }
                else
                {
                    //MessageBox.Show("Error: Data Transfer Settings are fail! Please try it again.");
                    MessageBox.Show("錯誤訊息: 資料轉換設定失敗! 請再試一次.");
                    IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error: " + ex.ToString());
                MessageBox.Show("錯誤訊息: " + ex.ToString());
                IsSuccess = false;
            }

            

            Shell.waitingForm.Close();
            
            if (IsSuccess)
            {
                pAbIso_GlobalContainer.IsChangeAbIsoValue = false; //取消掉Abnormal 和 Isolate的變更旗標

                // 呼叫ChangeToNextStep Tab動作
                if (ChangeToNextStep != null)
                {
                    ChangeToNextStep(null, new EventArgs());
                }
            }
            else
            {
                // 呼叫清空NextStep Tab動作
                if (DestroyNextStep != null)
                {
                    DestroyNextStep(null, new EventArgs());
                }
            }
        }

        // 更新gSG_GlobalContainer
        void InitMCSetGroupGlobalContainer()
        {
            //Shell.waitingForm.SettingMessage("Clean Container");
            Shell.waitingForm.SettingMessage("清除 Container");
            Shell.waitingForm.Show();

            pSG_GlobalContainer.Clear();
            pSG_GlobalContainer.SelectedMetrologyTypeList = CommonValue.DataContractClone(pSG_LocalContainer.SelectedMetrologyTypeList);
            pSG_GlobalContainer.SelectedIndicatorTypeList = CommonValue.DataContractClone(pSG_LocalContainer.SelectedIndicatorTypeList);
            pSG_GlobalContainer.groupList = CommonValue.DataContractClone(pSG_LocalContainer.groupList);
            pSG_GlobalContainer.groupCount = pSG_LocalContainer.groupCount;
            pSG_GlobalContainer.combinedIndicator = new ObservableCollection<MetrologyPoint>();
            pSG_GlobalContainer.combinedPoint = new ObservableCollection<MetrologyPoint>();
            pSG_GlobalContainer.MaximumIndicatorCount = pSG_LocalContainer.MaximumIndicatorCount;

            

            foreach (Group group in pSG_GlobalContainer.groupList)
            {
                //process for points
                foreach (MetrologyPoint point in group.PointList)
                {
                    if (!CheckExistIndicatorOrPoint(point.Name, pSG_GlobalContainer.combinedPoint))
                    {
                        pSG_GlobalContainer.combinedPoint.Add(point);
                    }
                }

                group.IndicatorList.Clear();

                //process for indicators
                foreach (ModelCreation.Action action in group.ActionList)
                {
                    
                    foreach (MetrologyPoint indicator in action.IndicatorList)
                    {
                        MetrologyPoint temp = CommonValue.DataContractClone(indicator);
                        temp.Name = action.ActionNumber + "_" + temp.Name;
                        

                        if (!CheckExistIndicatorOrPoint(temp.Name, pSG_GlobalContainer.combinedIndicator))
                        {
                            pSG_GlobalContainer.combinedIndicator.Add(temp);
                        }
                        group.IndicatorList.Add(temp);
                    }
                }
            }
            pSG_GlobalContainer.listPointCombo = GetPointListForCombobox(pSG_GlobalContainer.combinedPoint);

            Shell.waitingForm.Close();
        }

        // 產生Isolated和Abnormal的資料
        void InitAbnormalIsolatedContainer()
        {
            Shell.waitingForm.SettingMessage("Check Abnormal and Isolated Data");
            Shell.waitingForm.Show();

            pAbIso_GlobalContainer.Clear();
            pAbIso_GlobalContainer.contextList = CommonValue.DataContractClone(pDC_GlobalContainer.contextList);
            pAbIso_GlobalContainer.TotalcontextListCount = pAbIso_GlobalContainer.contextList.Count;
            pAbIso_GlobalContainer.checkboxesCleanProcessIsolated = new List<CheckBox>();
            pAbIso_GlobalContainer.checkboxesCleanProcessAbnormal = new List<CheckBox>();

            CheckBox CBTemp;
            for (int i = 0; i < pAbIso_GlobalContainer.TotalcontextListCount; i++)
            {
                CBTemp = new CheckBox();
                CBTemp.IsChecked = false;
                pAbIso_GlobalContainer.checkboxesCleanProcessIsolated.Add(CBTemp);

                CBTemp = new CheckBox();
                CBTemp.IsChecked = false;
                pAbIso_GlobalContainer.checkboxesCleanProcessAbnormal.Add(CBTemp);
            }

            pAbIso_GlobalContainer.TrainingCount = pDS_GlobalContainer.TrainingCount;
            pAbIso_GlobalContainer.RunningCount = pDS_GlobalContainer.RunningCount;
            pAbIso_GlobalContainer.iCurrentIUnIsolatedTrainCount = pAbIso_GlobalContainer.TrainingCount;
            pAbIso_GlobalContainer.iCurrentUnIsolatedRunCount = pAbIso_GlobalContainer.RunningCount;

            pAbIso_GlobalContainer.checkboxesAbnormalStatus = new List<String>();
            pAbIso_GlobalContainer.checkboxesIsolatedStatus = new List<String>();
            for (int i = 0; i < pAbIso_GlobalContainer.TotalcontextListCount; i++)
            {
                pAbIso_GlobalContainer.checkboxesIsolatedStatus.Add("0");
            }

            pAbIso_GlobalContainer.checkboxesAbnormalStatusOC = new ObservableCollection<string>(pAbIso_GlobalContainer.checkboxesAbnormalStatus);
            pAbIso_GlobalContainer.checkboxesIsolatedStatusOC = new ObservableCollection<string>(pAbIso_GlobalContainer.checkboxesIsolatedStatus);
            pAbIso_GlobalContainer.IsChangeAbIsoValue = false;

            Shell.waitingForm.Close();
        }

        #endregion

        #region Common Method
        //檢查所有的Group中的Action中的Indicator最大總數是否有符合 Train & Indicator的關係
        List<string> CheckIndicatorLimit(out int MaximumIndicatorCount)
        {
            int MaxCount = 0;
            int TotalMaxCount = 0;
            List<string> OutSpecGroupNameList = new List<string>();
            bool returnval = true;
            foreach (Group SelectedGroup in pSG_LocalContainer.groupList)
            {
                MaxCount = 0;
                switch (SelectedGroup.ActionList.Count)
                {
                    case 1:
                        MaxCount = SelectedGroup.ActionList[0].IndicatorList.Count;
                        break;

                    case 2:
                        MaxCount = (SelectedGroup.ActionList[0].IndicatorList.Count + SelectedGroup.ActionList[1].IndicatorList.Count);
                        break;
                }
                if (pDS_GlobalContainer.TrainingCount < (MaxCount + 5))
                {
                    returnval = false;
                    OutSpecGroupNameList.Add(SelectedGroup.GroupName + " (Indicator Count: " + MaxCount.ToString() + ")");
                }
                if (TotalMaxCount < MaxCount)
                {
                    TotalMaxCount = MaxCount;
                }
            }

            MaximumIndicatorCount = TotalMaxCount;

            if (returnval == false) // 檢查出有超過限制時 回傳超過限制的Group列表 沒有的話 回傳null
            {
                return OutSpecGroupNameList;
            }
            else
            {
                OutSpecGroupNameList = null;
                return OutSpecGroupNameList;
            }
        }

        bool CheckExistIndicatorOrPoint(String pValue, ObservableCollection<MetrologyPoint> list)
        {
            foreach (MetrologyPoint point in list)
            {
                if (point.Name.Equals(pValue))
                {
                    return true;    //exist
                }
            }
            return false;   //not exist
        }

        List<ComboxDataObj> GetPointListForCombobox(ObservableCollection<MetrologyPoint> list)
        {
            List<ComboxDataObj> listPoint = new List<ComboxDataObj>();
            for (int i = 0; i < list.Count; i++)
            {
                ComboxDataObj obj = new ComboxDataObj();
                obj.Name = list[i].Name;
                obj.Value = i;
                listPoint.Add(obj);
            }
            return listPoint;
        }

        private CheckBox GetCheckBoxWithParent(UIElement parent, Type targetType, string CheckBoxName)
        {
            if (parent.GetType() == targetType && ((CheckBox)parent).Name == CheckBoxName)
            {
                return (CheckBox)parent;
            }
            CheckBox result = null;
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                UIElement child = (UIElement)VisualTreeHelper.GetChild(parent, i);
                if (GetCheckBoxWithParent(child, targetType, CheckBoxName) != null)
                {
                    result = GetCheckBoxWithParent(child, targetType, CheckBoxName);
                    break;
                }
            }
            return result;
        }

        ObservableCollection<MetrologyPoint> GetObCollectionByCheckList(List<MetrologyPoint> ListMetrologyPoint, List<CheckBox> ListCheckBox)
        {
            MetrologyPoint[] ListMetrologyPointTemp = ListMetrologyPoint.ToArray();
            ObservableCollection<MetrologyPoint> Output = new ObservableCollection<MetrologyPoint>();
            int i = 0;
            foreach (CheckBox chkbox in ListCheckBox)
            {
                if (chkbox.IsChecked.Value)
                {
                    Output.Add(ListMetrologyPointTemp[i]);
                }
                i++;
            }
            return Output;
        }
        
        #endregion
    }
}