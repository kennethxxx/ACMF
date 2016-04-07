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
using System.Collections.Generic;

namespace IPS.Common
{
    public class Group
    {
        int groupId;
        List<MetrologyPoint> pointList;
        List<MetrologyPoint> indicatorList;
        List<Action> actionList;

        public Group()
        {
            groupId = 0;
        }

        public int GroupId
        {
            get { return groupId; }
            set { groupId = value; }
        }

        public String GroupName
        {
            get { 
                return ("Group" + groupId); 
            }
        }

        public List<Action> ActionList
        {
            get { return actionList; }
            set { actionList = value; }
        }
        
        public List<MetrologyPoint> IndicatorList
        {
            get { return indicatorList; }
            set { indicatorList = value; }
        }

        public List<MetrologyPoint> PointList
        {
            get { return pointList; }
            set { pointList = value; }
        }
    }
}
