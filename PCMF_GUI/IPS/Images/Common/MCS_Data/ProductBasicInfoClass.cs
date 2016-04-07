using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace IPS.Common.MCS_Data
{
    public class ValuesCollection : ObservableCollection<ProductBasicInfoClass> { };

    public class ProductBasicInfoClass
    {
        public String Service_Broker_ID
        {
            get
            {
                return _SBID;
            }
            set
            {
                _SBID = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Service_Broker_ID"));
            }
        }

        public String vMachine_ID
        {
            get
            {
                return _VMID;
            }
            set
            {
                _VMID = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("vMachine_ID"));
            }
        }

        public String CNC_ID
        {
            get
            {
                return _CNCID;
            }
            set
            {
                _CNCID = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CNC_ID"));
            }
        }

        public String CNC_Type
        {
            get
            {
                return _CNCType;
            }
            set
            {
                _CNCType = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CNC_Type"));
            }
        }

        public String Product_ID
        {
            get
            {
                return _ProID;
            }
            set
            {
                _ProID = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Product_ID"));
            }
        }

        public String Product_Type
        {
            get
            {
                return _ProType;
            }
            set
            {
                _ProType = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Product_Type"));
            }
        }

        public String X_Table
        {
            get
            {
                return _XTable;
            }
            set
            {
                _XTable = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("X_Table"));
            }
        }

        public String Y_Table
        {
            get
            {
                return _YTable;
            }
            set
            {
                _YTable = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Y_Table"));
            }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        String _SBID;
        String _VMID;
        String _CNCID;
        String _CNCType;
        String _ProID;
        String _ProType;
        String _XTable;
        String _YTable;
    }
}