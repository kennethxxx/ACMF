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

namespace IPS.Comm
{
    public class CBworkpieceProvider
    {
        public List<Combo> WorkpieceList
        {
            get
            {
                return new List<Combo> { new Combo() { Name="輪框", Value="WheelRim"},  
                                    new Combo() {  Name="手機背蓋", Value="CellphoneBack"},
                                    new Combo() {  Name="手機前蓋", Value="CellphoneFace"}};
            }
        }
    }

    /// <summary>
    /// ComboBox需要绑定的类
    /// </summary>
    public class Combo
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public override bool Equals(object obj)
        {
            if (null == obj)
            {
                return false;
            }
            return this.Value == ((Combo)obj).Value;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
