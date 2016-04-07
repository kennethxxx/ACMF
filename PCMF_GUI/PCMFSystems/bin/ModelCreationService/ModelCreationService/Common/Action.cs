using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace ModelCreationService
{
    [DataContract]
    public class Action
    {
        int actionNumber = 0;

        List<MetrologyPoint> indicatorList; //
        
        [DataMember]
        public int ActionNumber
        {
            get { return actionNumber; }
            set { actionNumber = value; }
        }

        [DataMember]
        public List<MetrologyPoint> IndicatorList
        {
            get { return indicatorList; }
            set { indicatorList = value; }
        }
    }
}