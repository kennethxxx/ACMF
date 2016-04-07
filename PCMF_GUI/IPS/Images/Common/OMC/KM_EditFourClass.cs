///KM_EditFourClass
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
    public class Class_edit_Col
    {
        public string Class_id { get; set; }
        public string Class_name { get; set; }
        public string Class_Supername { get; set; }

    }

    public class Property_edit_Col
    {
        public string Property_id { get; set; }
        public string Property_name { get; set; }
        public string Property_type { get; set; }
        public string Property_Supername { get; set; }
        public string Property_Domain { get; set; }
        public string Property_Range { get; set; }
        public string Property_Inverse { get; set; }
        public string Property_Set { get; set; }

    }

    public class Instance_edit_Col
    {
        public string Instance_id { get; set; }
        public string Instance_name { get; set; }
        public string Class_name { get; set; }
        public string X_axis { get; set; }
        public string Y_axis { get; set; }
        public string Z_axis { get; set; }
        public string Table_size_width { get; set; }
        public string Table_size_height { get; set; }
        public string Max_tableLoad { get; set; }
        public string Baxis_Swibelrange_height { get; set; }
        public string Baxis_Swibelrange_low { get; set; }
        public string Aaxis_Swibelrange_height { get; set; }
        public string Aaxis_Swibelrange_low { get; set; }
        public string Max_PalletLoad { get; set; }
        public string Palletsize_width { get; set; }
        public string Palletsize_height { get; set; }
        public string Machining_accuracy { get; set; }
        public string Processing_time { get; set; }
        public string structure_type_patterns { get; set; }
        public string canworkpiece { get; set; }
        public string workstartdate { get; set; }
        public string workenddate { get; set; }
        //工件公版屬性
        public string Workpiece_Size_length { get; set; }  //工件大小
        public string Workpiece_Size_width { get; set; }
        public string Workpiece_Weight { get; set; }  //工件重量
        public string Workpiece_Material { get; set; }  //工件材質
    }

    public class Constraint_edit_Col
    {
        public string Constraint_id { get; set; }
        public string Class_name { get; set; }
        public string Constraints_Description { get; set; }
        public string Constraints_type { get; set; }
    }
}
