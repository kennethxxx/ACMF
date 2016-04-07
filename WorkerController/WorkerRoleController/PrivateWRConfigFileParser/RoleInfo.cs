using System;
using System.Linq;

namespace WorkerRoleController.PrivateWRConfigFileParser
{
    public class RoleInfo
    {
        private int maxInstances;
        private string deploymentSlotType;

        public int MaxInstances
        {
            get
            {
                int num = this.maxInstances;
                return num;
            }
            set
            {
                this.maxInstances = value;
            }
        }

        public string SlotType
        {
            get
            {
                string str = this.deploymentSlotType;
                return str;
            }
            set
            {
                this.deploymentSlotType = value;
            }
        }

        public RoleInfo()
        {
       
        }

        public override bool Equals(object obj)
        {
            bool flag;
            bool flag1;
            flag1 = (obj == null ? false : !(this.GetType() != obj.GetType()));
            bool flag2 = flag1;
            if (flag2)
            {
                RoleInfo roleInfo = (RoleInfo)obj;
                flag = this.deploymentSlotType.Equals(roleInfo.deploymentSlotType);
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        public override int GetHashCode()
        {
            int hashCode = this.deploymentSlotType.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            string[] strArrays = new string[] { "\n-------Worker Role-------\nWorkingInfo: ", this.deploymentSlotType, "\n" };
            string str1 = string.Concat(strArrays);
            return str1;
        }

        public bool Validate()
        {
            bool flag;
            flag = (string.IsNullOrEmpty(this.deploymentSlotType) ? false : this.maxInstances > 0 ? true : this.SlotType.ToLower().Equals("Production".ToLower()) ? true : this.SlotType.Equals("Staging".ToLower()));
            bool flag1 = flag;
            return flag1;
        }
    }
}
