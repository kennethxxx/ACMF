using System;
using System.Collections.Generic;
using System.Linq;


namespace WorkerRoleController.WrcConfigFileParser
{
    public class DeploymentInfo
    {
        private string name;
        private StorageInfo storage;
        private string storageRelativePath;
        private readonly List<RoleInfo> roles;

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

        public List<RoleInfo> Roles
        {
            get
            {
                List<RoleInfo> roleInfos = this.roles;
                return roleInfos;
            }
            set
            {
                this.roles.Clear();
                this.roles.AddRange(value);
            }
        }

        public StorageInfo Storage
        {
            get
            {
                StorageInfo storageInfo = this.storage;
                return storageInfo;
            }
            set
            {
                this.storage = value;
            }
        }

        public string StoragePath
        {
            get
            {
                string str = this.storageRelativePath;
                return str;
            }
            set
            {
                this.storageRelativePath = value;
            }
        }

        public DeploymentInfo()
        {
            this.roles = new List<RoleInfo>();
        }

        public bool AddNewRole(RoleInfo role)
        {
            bool flag;
            bool flag1 = this.roles.Contains(role);
            if (flag1)
            {
                flag = false;
            }
            else
            {
                this.roles.Add(role);
                flag = true;
            }
            return flag;
        }

        public override bool Equals(object obj)
        {
            bool flag;
            bool flag1;
            bool flag2;
            flag1 = (obj == null ? false : !(this.GetType() != obj.GetType()));
            bool flag3 = flag1;
            if (flag3)
            {
                DeploymentInfo deploymentInfo = (DeploymentInfo)obj;
                flag2 = (!this.name.Equals(deploymentInfo.Name) ? true : !this.StoragePath.Equals(deploymentInfo.StoragePath));
                flag3 = flag2;
                flag = (flag3 ? false : true);
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
            bool flag;
            string str = "\n\n-------Roles-------\n";
            List<RoleInfo>.Enumerator enumerator = this.roles.GetEnumerator();
            try
            {
                while (true)
                {
                    flag = enumerator.MoveNext();
                    if (!flag)
                    {
                        break;
                    }
                    RoleInfo current = enumerator.Current;
                    str = string.Concat(str, "\t", current.ToString(), "\n");
                }
            }
            finally
            {
                ((IDisposable)enumerator).Dispose();
            }
            string[] name = new string[] { "-------------DEPLOYMENT------------\nName: ", this.name, "\nStorage: ", this.Storage.Name, "\nStorage Path: ", this.storageRelativePath, "\n", str, "\n"};
            string str2 = string.Concat(name);
            return str2;
        }

        public bool Validate()
        {
            bool flag;
            bool flag1;
            bool flag2;
            List<RoleInfo>.Enumerator enumerator1 = this.Roles.GetEnumerator();
            try
            {
                do
                {
                    flag1 = enumerator1.MoveNext();
                    if (flag1)
                    {
                        RoleInfo roleInfo = enumerator1.Current;
                        flag1 = roleInfo.Validate();
                    }
                    else
                    {
                        flag2 = (string.IsNullOrEmpty(this.name) || this.storage == null ? false : !string.IsNullOrEmpty(this.storageRelativePath));
                        flag = flag2;
                        return flag;
                    }
                }
                while (flag1);
                flag = false;
                return flag;
            }
            finally
            {
                ((IDisposable)enumerator1).Dispose();
            }
            //return flag;
        }
    }
}
