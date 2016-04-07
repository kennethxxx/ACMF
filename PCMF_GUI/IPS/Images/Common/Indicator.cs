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
    public class Indicator
    {
        int number;
        string contextID;
        DateTime processStartTime;
        DateTime processEndTime;
        DateTime metrologyStartTime;
        DateTime metrologyEndTime;

        public int Number
        {
            get { return number; }
            set { number = value; }
        }

        public string ContextID
        {
            get { return contextID; }
            set { contextID = value; }
        }

        public DateTime MetrologyStartTime
        {
            get { return metrologyStartTime; }
            set { metrologyStartTime = value; }
        }

        public DateTime MetrologyEndTime
        {
            get { return metrologyEndTime; }
            set { metrologyEndTime = value; }
        }

        public DateTime ProcessEndTime
        {
            get { return processEndTime; }
            set { processEndTime = value; }
        }

        public DateTime ProcessStartTime
        {
            get { return processStartTime; }
            set { processStartTime = value; }
        }
    }
}
