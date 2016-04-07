using System;
using System.Linq;

namespace WorkerRoleController.WRScalingRuleFileParser
{
    public class Output
    {
        private string output;

        public Output()
        {

        }

        public string OutputInfo
        {
            get
            {
                string str = this.output;
                return str;
            }
            set
            {
                this.output = value;
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
                flag2 = !this.output.Equals(element.Name);
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
            string[] strArrays = new string[] { "-------------Output------------\nOutput: ", this.output };
            string str1 = string.Concat(strArrays);
            return str1;
        }

        public bool Validate()
        {
            bool flag;
            flag = (!string.IsNullOrEmpty(this.output));
            bool flag1 = flag;
            return flag1;
        }
    }
}
