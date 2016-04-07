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
    public class ComboxDataObj
    {
        int value;

        String name;

        public ComboxDataObj()
        {

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
    }
}
