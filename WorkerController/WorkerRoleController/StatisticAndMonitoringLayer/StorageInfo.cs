using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkerRoleController.WrcConfigFileParser
{
    public class StorageInfo
    {
        private string name;

        private string key;

        public string Key
        {
            get
            {
                string str = this.key;
                return str;
            }
            set
            {
                this.key = value;
            }
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

        public StorageInfo()
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
                StorageInfo storageInfo = (StorageInfo)obj;
                flag2 = !this.name.Equals(storageInfo.Name);
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
            string[] strArrays = new string[] { "-------------Storage------------\nName: ", this.name, "\nKey: ", this.key, "\n" };
            string str = string.Concat(strArrays);
            return str;
        }

        public bool Validate()
        {
            bool flag;
            flag = (string.IsNullOrEmpty(this.name) ? false : !string.IsNullOrEmpty(this.key));
            bool flag1 = flag;
            return flag1;
        }
    }
}
