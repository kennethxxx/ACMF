using System;
using System.Collections.Generic;
using System.Linq;

namespace WorkerRoleController.PrivateWRConfigFileParser
{
    public class VcenterInfo
    { 
        private string VIP;

        private string Vaccount;

        private string Vpassword;

        private readonly List<RoleInfo> roles;

        public List<RoleInfo> Roles
        {
            get
            {
                List<RoleInfo> deploymentInfos = this.roles;
                return deploymentInfos;
            }
            set
            {
                this.roles.Clear();
                this.roles.AddRange(value);
            }
        }

        public string vIP
        {
            get
            {
                string str = this.VIP;
                return str;
            }
            set
            {
                this.VIP = value;
            }
        }

        public string vaccount
        {
            get
            {
                string str = this.Vaccount;
                return str;
            }
            set
            {
                this.Vaccount = value;
            }
        }

        public string vpassword
        {
            get
            {
                string str = this.Vpassword;
                return str;
            }
            set
            {
                this.Vpassword = value;
            }
        }

        public VcenterInfo()
        {
            this.roles = new List<RoleInfo>();
        }

        public bool AddNewDeployment(RoleInfo role)
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
            flag1 = (obj == null ? false : !(this.GetType() != obj.GetType()));
            bool flag2 = flag1;
            if (flag2)
            {
                VcenterInfo VcenterInfos = (VcenterInfo)obj;
                flag = this.VIP.Equals(VcenterInfos.VIP);
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        public override int GetHashCode()
        {
            int hashCode = this.VIP.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            bool flag;
            string str1 = "";
            List<RoleInfo>.Enumerator enumerator1 = this.roles.GetEnumerator();
            try
            {
                while (true)
                {
                    flag = enumerator1.MoveNext();
                    if (!flag)
                    {
                        break;
                    }
                    RoleInfo deploymentInfo = enumerator1.Current;
                    str1 = string.Concat(str1, "\t", deploymentInfo.ToString(), "\n");
                }
            }
            finally
            {
                ((IDisposable)enumerator1).Dispose();
            }
            string[] strArrays = new string[] { "\n-------Vcenter-------\nVaccount: ", this.Vaccount, "\n", str1, "\n_________________________________\n" };
            string str2 = string.Concat(strArrays);
            return str2;
        }

        public bool Validate()
        {
            bool flag;
            bool flag1;
            bool flag2;
            flag1 = (string.IsNullOrEmpty(this.Vaccount) ? false : !string.IsNullOrEmpty(this.VIP));
            bool flag3 = flag1;
            if (flag3)
            {
                flag2 = (this.roles == null ? false : this.roles.Count > 0);
                flag3 = flag2;
                if (flag3)
                {
                    List<RoleInfo>.Enumerator enumerator = this.roles.GetEnumerator();
                    try
                    {
                        while (true)
                        {
                            flag3 = enumerator.MoveNext();
                            if (!flag3)
                            {
                                break;
                            }
                            RoleInfo current = enumerator.Current;
                            flag3 = current.Validate();
                            if (!flag3)
                            {
                                flag = false;
                                return flag;
                            }
                        }
                    }
                    finally
                    {
                        ((IDisposable)enumerator).Dispose();
                    }
                    flag = true;
                }
                else
                {
                    flag = false;
                }
            }
            else
            {
                flag = false;
            }
            return flag;
        }
    }
}
