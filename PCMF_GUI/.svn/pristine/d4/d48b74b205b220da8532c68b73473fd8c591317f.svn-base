///KM_CreationFourClass
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
    public class Instance_Insert_Col
    {
        public int Instance_Property_id;
        public string Instance_Name;
        public string Property_Name;
        public string Property_Values;

        public Instance_Insert_Col(int instance_property_id, string instance_name, string property_name, string property_Values)
        {
            this.Instance_Property_id = instance_property_id;
            this.Instance_Name = instance_name;
            this.Property_Name = property_name;
            this.Property_Values = property_Values;
        }
    }

    public class ClassNode
    {
        public string Class_Name { get; set; }
        public string Class_SuperName { get; set; }

        public ClassNode(string class_name, string class_supername)
        {
            this.Class_Name = class_name;
            this.Class_SuperName = class_supername;
        }
    }

    public class PropertyNode
    {
        public int PropertyId { get; set; }
        public string Property_Name { get; set; }
        public string Property_Type { get; set; }
        public string Property_SuperName { get; set; }
        public string Property_Domain { get; set; }
        public string Property_Range { get; set; }
        public string Property_Inverse { get; set; }
        public string Property_Set { get; set; }

        public PropertyNode(int propertyid, string property_name, string property_type, string property_supername, string property_domain, string property_range, string property_inverse, string property_set)
        {
            this.PropertyId = propertyid;
            this.Property_Name = property_name;
            this.Property_Type = property_type;
            this.Property_SuperName = property_supername;
            this.Property_Domain = property_domain;
            this.Property_Range = property_range;
            this.Property_Inverse = property_inverse;
            this.Property_Set = property_set;
        }

        public PropertyNode()
        {
            // TODO: Complete member initialization
        }
    }

    public class PropertyDomainNode
    {
        public string Property_Name { get; set; }
        public string Property_Domain { get; set; }

        public PropertyDomainNode(string property_name, string property_domain)
        {
            Property_Name = property_name;
            Property_Domain = property_domain;
        }

    }

    public class PropertyRangeNode
    {
        public string Property_Name { get; set; }
        public string Property_Range { get; set; }

        public PropertyRangeNode(string property_name, string property_range)
        {
            Property_Name = property_name;
            Property_Range = property_range;
        }
    }

    public class PropertyDTDomainNode
    {
        public string Property_Name { get; set; }
        public string Property_Domain { get; set; }

        public PropertyDTDomainNode(string property_name, string property_domain)
        {
            Property_Name = property_name;
            Property_Domain = property_domain;
        }

    }

    public class PropertyDTRangeNode
    {
        public string Property_Name { get; set; }
        public string Property_Range_Type { get; set; }
        public string Property_Allowed_Values { get; set; }

        public PropertyDTRangeNode(string property_name, string property_range_type, string property_allowed_values)
        {
            Property_Name = property_name;
            Property_Range_Type = property_range_type;
            Property_Allowed_Values = property_allowed_values;
        }

    }

    public class InstanceNode
    {
        public string Select_Class { get; set; }
        public string Instance_Name { get; set; }

        public InstanceNode(string select_class, string instance_name)
        {
            Select_Class = select_class;
            Instance_Name = instance_name;
        }
    }

    public class DB_InstanceNode
    {
        public string Select_Class { get; set; }
        public string Instance_Name { get; set; }

        public DB_InstanceNode(string select_class, string instance_name)
        {
            Select_Class = select_class;
            Instance_Name = instance_name;
        }
    }

    public class Instance_Col
    {
        public string Instance_Att_Name { get; set; }
    }
    public class ConstraintNode
    {
        public int Constraint_Number { get; set; }
        public string Con_choose_Class { get; set; }
        public string Constraint3 { get; set; }
        public string Constraint_NorNS { get; set; }


        public ConstraintNode(int constraint_number, string con_choose_class, string constraint3, string constraint_NorNS)
        {
            Constraint_Number = constraint_number;
            Con_choose_Class = con_choose_class;
            Constraint3 = constraint3;
            Constraint_NorNS = constraint_NorNS;
        }
    }  
}
