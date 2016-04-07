using System;
using System.Net;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ModelCreationService
{
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
        public List<MetrologyPoint> IndicatorList { get; set; }

        [DataMember]
        public List<MetrologyPoint> PointList { get; set; }
    }
}
