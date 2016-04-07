using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ModelCreationService
{
    [DataContract]
    public class Context
    {
        public Context(int iContextID, String strPieceID)
        {
            this.ContextID = iContextID;
            this.PieceID = strPieceID;
        }

        [DataMember]
        public String PieceID { get; set; }

        [DataMember]
        public int ContextID { get; set; }

        [DataMember]
        public String ProcessStartTime { get; set; }

        [DataMember]
        public String ProcessEndTime { get; set; }

        [DataMember]
        public String MetrologyStartTime { get; set; }

        [DataMember]
        public String MetrologyEndTime { get; set; }
    }
}
