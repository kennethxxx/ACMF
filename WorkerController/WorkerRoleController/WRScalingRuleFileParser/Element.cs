using System;
using System.Linq;

namespace WorkerRoleController.WRScalingRuleFileParser
{
    public class Element
    {
        private string name;
        private string wsCall;

        public Element()
        {
       
        }

        public string Name
        {
            get
            {
                string str = this.name;
                return str;
            }
            set
            {
                this.name = value;
            }
        }

        public string WsCall
        {
            get
            {
                string str = this.wsCall;
                return str;
            }
            set
            {
                this.wsCall = value;
            }
        }

        public override bool Equals(object obj)
        {
            bool flag;
            bool flag1;
            flag1 = (obj == null ? false : !(this.GetType() != obj.GetType()));
            bool flag2 = flag1;
            if (flag2)
            {
                Element element = (Element)obj;
                flag2 = !this.name.Equals(element.Name);
                flag = (flag2 ? false : true);
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        public override int GetHashCode()
        {
            int hashCode = this.GetHashCode();
            return hashCode;
        }


        public override string ToString()
        {
            string[] strArrays = new string[] { "-------------Element------------\nName: ", this.name, "\nWsCall: ", this.wsCall, "\n" };
            string str1 = string.Concat(strArrays);
            return str1;
        }

        public bool Validate()
        {
            bool flag;
            flag = (string.IsNullOrEmpty(this.name) ? false : !string.IsNullOrEmpty(this.wsCall));
            bool flag1 = flag;
            return flag1;
        }
    }
}
