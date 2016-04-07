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

namespace OMC.Comm
{
    public class RuleDB_Select
    {
        public bool Rule_Select { get; set; }
        public string Rule_Name { get; set; }
        public string Rule_Description { get; set; }
        public string Rule_SWRL { get; set; }
    }

    public class FavoriteCNC
    {
        public bool FavCNC_Select { get; set; }
        public string CNC_Name { get; set; }
    }
}
