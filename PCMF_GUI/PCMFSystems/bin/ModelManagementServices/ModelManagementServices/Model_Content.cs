using System;
using System.Net;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ModelManagementServices
{
    [DataContract]
    public class Model_Content
    {
        String _PK;
        String _vMachineID;
        String _CNC_ID;
        String _ProductName;
        String _ProductType;


        [DataMember]
        public String PK
        {
            get { return _PK; }
            set { _PK = value; }
        }
        [DataMember]
        public String vMachineID
        {
            get { return _vMachineID; }
            set { _vMachineID = value; }
        }
        [DataMember]
        public String CNC_ID
        {
            get { return _CNC_ID; }
            set { _CNC_ID = value; }
        }

        [DataMember]
        public String ProductType
        {
            get { return _ProductType; }
            set { _ProductType = value; }
        }

        [DataMember]
        public String ProductName
        {
            get { return _ProductName; }
            set { _ProductName = value; }
        }






    }

      
}