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
using IPS.DataAcquisition;
using System.Collections;
using System.Collections.ObjectModel;
using IPS.Common;
using Serialization;

namespace IPS.Views
{

    public class DACreateNewDCPLocalContainer //放置該頁面的暫時資料
    {

        public List<String> ServiceBrokerNameList = new List<String>();
        public List<String> ServiceBrokerURIList = new List<String>();

        public Dictionary<String,List<String>> ProductInfoList = new Dictionary<String,List<String>>();
        public List<String> ProductTypeList = new List<String>();

        public Dictionary<String, List<String>> vMachineInfoList = new Dictionary<String, List<String>>();
        public List<String> vMachineIDList = new List<String>();

        public Dictionary<String, List<String>> vMachineTypeInfoList = new Dictionary<String, List<String>>();
        public List<String> vMachineTypeList = new List<String>();

        public List<Xinfo> ProductBasicXinfoList = new List<Xinfo>();
        public List<bool> ProductBasicXinfoCheckList = new List<bool>();
        public List<Yinfo> ProductBasicYinfoList = new List<Yinfo>();
        public List<bool> ProductBasicYinfoCheckList = new List<bool>();

    }

    public class DACreateNewDCPGlobalContainer //放置該頁面的永久暫時資料
    {
        public String LoginUsername;//   = StateManager.Username;
        public String Company;// = StateManager.UserCompany;

        public string ChooseServiceBrokerName = "";
        public string ChooseServiceBrokerURI = "";

        public string ChooseProductInfoType = "";
        public string ChooseProductInfoName = "";

        public string ChoosevMachineID = "";
        public string ChooseCNCID = "";
        public string ChooseCNCType = "";

        public List<Xinfo> AllProductBasicXinfoList = new List<Xinfo>();
        public List<Yinfo> AllProductBasicYinfoList = new List<Yinfo>();

        public List<Xinfo> ChooseProductBasicXinfoList = new List<Xinfo>();
        public List<Yinfo> ChooseProductBasicYinfoList = new List<Yinfo>();

        public String DAStartTime;
        public String DAEndTime;
        public String DAConjectureType;
        public String DACollectionMethod;

    }
}
