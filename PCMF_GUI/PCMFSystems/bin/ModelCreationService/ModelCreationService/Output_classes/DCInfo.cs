using System;
using System.Net;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DataCollection
{
    [DataContract]
    public class CNCInfo
    {
        String _Name;
        String _Type;

        [DataMember]
        public String Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        [DataMember]
        public String Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
    }

    [DataContract]
    public class vMachineInfo
    {
        String _Name;
        List<CNCInfo> _CNCList;

        [DataMember]
        public String Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        [DataMember]
        public List<CNCInfo> CNCList
        {
            get
            {
                if (_CNCList == null)
                {
                    _CNCList = new List<CNCInfo>();
                }
                return _CNCList;
            }
            set { _CNCList = value; }
        }
    }

    [DataContract]
    public class ServiceBrokerInfo
    {
        String _Name;
        String _URL;
        List<vMachineInfo> _vMachineList;

        [DataMember]
        public String Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        [DataMember]
        public String URL
        {
            get { return _URL; }
            set { _URL = value; }
        }
        [DataMember]
        public List<vMachineInfo> vMachineList
        {
            get
            {
                if (_vMachineList == null)
                {
                    _vMachineList = new List<vMachineInfo>();
                }
                return _vMachineList;
            }
            set { _vMachineList = value; }
        }
    }

    [DataContract]
    public class CurrentInfos
    {
        String _CNCName;
        String _CNCType;
        String _vMachineID;
        String _ServiceBrokerID;
        String _ProductName;
        String _ProductType;
        String _XTableName;
        String _YTableName;

        [DataMember]
        public String CNCName
        {
            get { return _CNCName; }
            set { _CNCName = value; }
        }
        [DataMember]
        public String CNCType
        {
            get { return _CNCType; }
            set { _CNCType = value; }
        }
        [DataMember]
        public String vMachineID
        {
            get { return _vMachineID; }
            set { _vMachineID = value; }
        }
        [DataMember]
        public String ServiceBrokerID
        {
            get { return _ServiceBrokerID; }
            set { _ServiceBrokerID = value; }
        }
        [DataMember]
        public String ProductName
        {
            get { return _ProductName; }
            set { _ProductName = value; }
        }
        [DataMember]
        public String ProductType
        {
            get { return _ProductType; }
            set { _ProductType = value; }
        }
        [DataMember]
        public String XTableName
        {
            get { return _XTableName; }
            set { _XTableName = value; }
        }
        [DataMember]
        public String YTableName
        {
            get { return _YTableName; }
            set { _YTableName = value; }
        }
    }

    [DataContract]
    public class Group
    {
        int groupId;

        public Group()
        {
            groupId = 0;
        }

        [DataMember]
        public int GroupId
        {
            get { return groupId; }
            set { groupId = value; }
        }

        [DataMember]
        public String GroupName { get; set; }

        [DataMember]
        public List<Action> ActionList { get; set; }

        [DataMember]
        public List<XItem> IndicatorList { get; set; }

        [DataMember]
        public List<YItem> PointList { get; set; }
    }

    [DataContract]
    public class XItem
    {
        String _Name;
        String _Type;
        String _Position;

        [DataMember]
        public String Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        [DataMember]
        public String Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        [DataMember]
        public String Position
        {
            get { return _Position; }
            set { _Position = value; }
        }
        [DataMember]
        public String Description
        {
            get { return _Name + "_" + _Type + "_" + _Position; }
            set { ; }
        }
    }

    [DataContract]
    public class YItem
    {
        String _Name;
        String _Type;
        String _Block;
        String _USL;
        String _LSL;
        String _UCL;
        String _LCL;
        String _TargetValue;
        
        [DataMember]
        public String Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        [DataMember]
        public String Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        [DataMember]
        public String Block
        {
            get { return _Block; }
            set { _Block = value; }
        }
        
        [DataMember]
        public String USL
        {
            get { return _USL; }
            set { _USL = value; }
        }
        [DataMember]
        public String LSL
        {
            get { return _LSL; }
            set { _LSL = value; }
        }
        [DataMember]
        public String UCL
        {
            get { return _UCL; }
            set { _UCL = value; }
        }
        [DataMember]
        public String LCL
        {
            get { return _LCL; }
            set { _LCL = value; }
        }
        [DataMember]
        public String TargetValue
        {
            get { return _TargetValue; }
            set { _TargetValue = value; }
        }
        
        [DataMember]
        public String Description
        {
            get { return _Type + "_" + _Name; }
            set { ; }
        }
    }

    [DataContract]
    public class AllPiece
    {
        String _ID;
        String _Metrology_StartTime;
        String _Metrology_EndTime;
        String _Process_StartTime;
        String _Process_EndTime;

        [DataMember]
        public String ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        [DataMember]
        public String Metrology_StartTime
        {
            get { return _Metrology_StartTime; }
            set { _Metrology_StartTime = value; }
        }
        [DataMember]
        public String Metrology_EndTime
        {
            get { return _Metrology_EndTime; }
            set { _Metrology_EndTime = value; }
        }
        [DataMember]
        public String Process_StartTime
        {
            get { return _Process_StartTime; }
            set { _Process_StartTime = value; }
        }
        [DataMember]
        public String Process_EndTime
        {
            get { return _Process_EndTime; }
            set { _Process_EndTime = value; }
        }
    }

    [DataContract]
    public class TrainPiece
    {
        String _ID;
        String _Metrology_StartTime;
        String _Metrology_EndTime;
        String _Process_StartTime;
        String _Process_EndTime;

        [DataMember]
        public String ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        [DataMember]
        public String Metrology_StartTime
        {
            get { return _Metrology_StartTime; }
            set { _Metrology_StartTime = value; }
        }
        [DataMember]
        public String Metrology_EndTime
        {
            get { return _Metrology_EndTime; }
            set { _Metrology_EndTime = value; }
        }
        [DataMember]
        public String Process_StartTime
        {
            get { return _Process_StartTime; }
            set { _Process_StartTime = value; }
        }
        [DataMember]
        public String Process_EndTime
        {
            get { return _Process_EndTime; }
            set { _Process_EndTime = value; }
        }
    }

    [DataContract]
    public class RunPiece
    {
        String _ID;
        String _Metrology_StartTime;
        String _Metrology_EndTime;
        String _Process_StartTime;
        String _Process_EndTime;

        [DataMember]
        public String ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        [DataMember]
        public String Metrology_StartTime
        {
            get { return _Metrology_StartTime; }
            set { _Metrology_StartTime = value; }
        }
        [DataMember]
        public String Metrology_EndTime
        {
            get { return _Metrology_EndTime; }
            set { _Metrology_EndTime = value; }
        }
        [DataMember]
        public String Process_StartTime
        {
            get { return _Process_StartTime; }
            set { _Process_StartTime = value; }
        }
        [DataMember]
        public String Process_EndTime
        {
            get { return _Process_EndTime; }
            set { _Process_EndTime = value; }
        }
    }
    
    [DataContract]
    public class DCPInfo
    {
        CurrentInfos _CurrentInfo;
        String _FactoryName;
        String _CollectionMethod;
        String _ConjectureType;
        //DateTime _StartTime;
        //DateTime _EndTime;
        String _StartTime;
        String _EndTime;
        String _XTable;
        String _YTable;

        String _WorkPieceName;
        String _WorkPieceType;

        ServiceBrokerInfo _ServiceBrokerInformation;
        List<XItem> _X_Data;
        List<YItem> _Y_Data;
        List<AllPiece> _AllPiece;
        List<TrainPiece> _TrainPiece;
        List<RunPiece> _RunPiece;

        


        [DataMember]
        public String FactoryName
        {
            get { return _FactoryName; }
            set { _FactoryName = value; }
        }

        [DataMember]
        public String CollectionMethod
        {
            get { return _CollectionMethod; }
            set { _CollectionMethod = value; }
        }

        [DataMember]
        public String ConjectureType
        {
            get { return _ConjectureType; }
            set { _ConjectureType = value; }
        }

        [DataMember]
        public String StartTime
        {
            get { return _StartTime; }
            set { _StartTime = value; }
        }

        [DataMember]
        public String EndTime
        {
            get { return _EndTime; }
            set { _EndTime = value; }
        }

        [DataMember]
        public ServiceBrokerInfo ServiceBrokerInformation
        {
            get
            {
                if (_ServiceBrokerInformation == null)
                {
                    _ServiceBrokerInformation = new ServiceBrokerInfo();
                }
                return _ServiceBrokerInformation;
            }
            set { _ServiceBrokerInformation = value; }
        }

        [DataMember]
        public CurrentInfos CurrentInfo
        {
            get
            {
                if (_CurrentInfo == null)
                {
                    _CurrentInfo = new CurrentInfos();
                }
                return _CurrentInfo;
            }
            set { _CurrentInfo = value; }
        }

        [DataMember]
        public String XTable
        {
            get { return _XTable; }
            set { _XTable = value; }
        }
        [DataMember]
        public String YTable
        {
            get { return _YTable; }
            set { _YTable = value; }
        }


        [DataMember]
        public String WorkPieceName
        {
            get { return _WorkPieceName; }
            set { _WorkPieceName = value; }
        }

        [DataMember]
        public String WorkPieceType
        {
            get { return _WorkPieceType; }
            set { _WorkPieceType = value; }
        }

        [DataMember]
        public List<XItem> X_Data
        {
            get
            {
                if (_X_Data == null)
                {
                    _X_Data = new List<XItem>();
                }
                return _X_Data;
            }
            set { _X_Data = value; }
        }        

        [DataMember]
        public List<YItem> Y_Data
        {
            get
            {
                if (_Y_Data == null)
                {
                    _Y_Data = new List<YItem>();
                }
                return _Y_Data;
            }
            set { _Y_Data = value; }
        }

        [DataMember]
        public List<AllPiece> WP_Data
        {
            get
            {
                if (_AllPiece == null)
                {
                    _AllPiece = new List<AllPiece>();
                }
                return _AllPiece;
            }
            set { _AllPiece = value; }
        }


        [DataMember]
        public List<TrainPiece> WP_TrainData
        {
            get
            {
                if (_TrainPiece == null)
                {
                    _TrainPiece = new List<TrainPiece>();
                }
                return _TrainPiece;
            }
            set { _TrainPiece = value; }
        }


        [DataMember]
        public List<RunPiece> WP_RunData
        {
            get
            {
                if (_RunPiece == null)
                {
                    _RunPiece = new List<RunPiece>();
                }
                return _RunPiece;
            }
            set { _RunPiece = value; }
        }

    }
}