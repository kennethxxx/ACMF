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

namespace IPS.Common
{
    public class MetrologyPoint
    {
        int value;

        String name;

        String measureType;

        String actions; //format: 1,5 -> this point has 2 action 1 and 5    - ONLY FOR METROLOGY ITEMS

        String dataField;   //to determine this data com from which field.

        public String DataField
        {
            get { return dataField; }
            set { dataField = value; }
        }

        public String Actions
        {
            get { return actions; }
            set { actions = value; }
        }

        double uSL;
        double uCL;
        double lCL;
        double lSL;

        public MetrologyPoint()
        {
            uSL = 0;
            uCL = 0;
            lCL = 0;
            lSL = 0;
        }

        public int Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public String MeasureType
        {
            get { return measureType; }
            set { measureType = value; }
        }

        public double USL
        {
            get { return uSL; }
            set { uSL = value; }
        }

        public double LSL
        {
            get { return lSL; }
            set { lSL = value; }
        }

        public double UCL
        {
            get { return uCL; }
            set { uCL = value; }
        }

        public double LCL
        {
            get { return lCL; }
            set { lCL = value; }
        }
    }
}
