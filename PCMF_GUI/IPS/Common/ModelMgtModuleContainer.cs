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
using IPS.ModelManager;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Collections.ObjectModel;

namespace IPS.Views
{
    public class MMModelSelectionLocalContainer //放置該頁面的暫時資料
    {
        public ObservableCollection<string> vMachineList = null;
        public ObservableCollection<string> CNCnumberList = null;
        public ObservableCollection<string> NCprogramList = null;
        public ObservableCollection<string> ProductIDList = null;
        public ObservableCollection<string> ServiceBrokerList = null;
        public List<ModelInformation> modelList = null;
        public ModelInformation SelectedModelInformation = null;
        
    }

    public class MMModelFanOutLocalContainer //放置該頁面的暫時資料
    {
        public List<EquipmentInformation> vmList = null;
        public List<EquipmentInformation> selectedVMList = null;
    }
}
