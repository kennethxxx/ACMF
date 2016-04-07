using System;
using System.Net;
using System.Runtime.Serialization;

namespace ModelCreationService
{
    [DataContract]
    public class MetrologyPoint
    {
        double uSL;
        double uCL;
        double lCL;
        double lSL;
        double target;

        public MetrologyPoint()
        {
            uSL = 0;
            uCL = 0;
            lCL = 0;
            lSL = 0;
            target = 0;
        }
        
        [DataMember]
        public String DataField { get; set; } //to determine this data com from which field.

        [DataMember]
        public String Actions { get; set; }//format: 1,5 -> this point has 2 action 1 and 5    - ONLY FOR METROLOGY ITEMS
        
        [DataMember]
        public int Value { get; set; }

        [DataMember]
        public String Name { get; set; }

        [DataMember]
        public String MeasureType { get; set; }

        [DataMember]
        public double USL
        {
            get { return uSL; }
            set { uSL = value; }
        }

        [DataMember]
        public double LSL
        {
            get { return lSL; }
            set { lSL = value; }
        }

        [DataMember]
        public double UCL
        {
            get { return uCL; }
            set { uCL = value; }
        }

        [DataMember]
        public double LCL
        {
            get { return lCL; }
            set { lCL = value; }
        }

        [DataMember]
        public double TARGET
        {
            get { return target; }
            set { target = value; }
        }
        
        //public static MetrologyPoint Copy(MetrologyPoint obj)
        //{
        //    return (MetrologyPoint)obj.MemberwiseClone();
        //}
    }
}
