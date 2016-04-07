using System;
using System.Net;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ModelCreationService
{
    [DataContract]
    public class Indicator
    {
        List<double> listItemValue;

        public Indicator()
        {
            listItemValue = new List<double>();
        }

        [DataMember]
        public List<double> ListItemValue
        {
            get { return listItemValue; }
            set { listItemValue = value; }
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Number { get; set; }

        [DataMember]
        public string ContextID { get; set; }

        [DataMember]
        public DateTime MetrologyStartTime { get; set; }

        [DataMember]
        public DateTime MetrologyEndTime {get; set;}

        [DataMember]
        public DateTime ProcessEndTime {get; set;}

        [DataMember]
        public DateTime ProcessStartTime { get; set; }

        //public static Indicator Copy(Indicator obj)
        //{
        //    return (Indicator)obj.MemberwiseClone();
        //}
    }
}
