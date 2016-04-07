using System;
using System.Collections.Generic;
using System.Linq;

namespace WorkerRoleController.PrivateWRConfigFileParser
{
    public class SubscriptionInfo
    {

        private readonly List<VcenterInfo> services;

        private readonly List<MySQLInfo> databases;

        public List<VcenterInfo> Vcenter
        {
            get
            {
                List<VcenterInfo> VcenterInfos = this.services;
                return VcenterInfos;
            }
            set
            {
                this.services.Clear();
                this.services.AddRange(value);
            }
        }

        public List<MySQLInfo> Databases
        {
            get
            {
                List<MySQLInfo> MySQLInfos = this.databases;
                return MySQLInfos;
            }
            set
            {
                this.databases.Clear();
                this.databases.AddRange(value);
            }
        }

        public SubscriptionInfo()
        {
            this.services = new List<VcenterInfo>();
            this.databases = new List<MySQLInfo>();
        }

        public bool AddNewService(VcenterInfo service)
        {
            bool flag;
            bool flag1 = this.services.Contains(service);
            if (flag1)
            {
                flag = false;
            }
            else
            {
                this.services.Add(service);
                flag = true;
            }
            return flag;
        }

        public bool AddNewDatabase(MySQLInfo database)
        {
            bool flag;
            bool flag1 = this.databases.Contains(database);
            if (flag1)
            {
                flag = false;
            }
            else
            {
                this.databases.Add(database);
                flag = true;
            }
            return flag;
        }

        public override string ToString()
        {
            bool flag;
            string str = "";
            List<VcenterInfo>.Enumerator enumerator = this.services.GetEnumerator();
            try
            {
                while (true)
                {
                    flag = enumerator.MoveNext();
                    if (!flag)
                    {
                        break;
                    }
                    VcenterInfo current = enumerator.Current;
                    str = string.Concat(str, "\t", current.ToString(), "\n");
                }
            }
            finally
            {
                ((IDisposable)enumerator).Dispose();
            }
            string str1 = "";
            List<MySQLInfo>.Enumerator enumerator1 = this.databases.GetEnumerator();
            try
            {
                while (true)
                {
                    flag = enumerator1.MoveNext();
                    if (!flag)
                    {
                        break;
                    }
                    MySQLInfo storageInfo = enumerator1.Current;
                    str1 = string.Concat(str1, "\t", storageInfo.ToString(), "\n");
                }
            }
            finally
            {
                ((IDisposable)enumerator1).Dispose();
            }
            string[] strArrays = new string[] { "------Subscription------\n", str1, "\n", str, "\n" };
            string str2 = string.Concat(strArrays);
            return str2;
        }

        public bool Validate()
        {
            bool flag;
            bool flag1;
            bool flag2;
            bool flag3;
            flag1 = (this.services == null ? false : this.services.Count > 0);
            flag3 = flag1;
            if (flag3)
            {
                List<VcenterInfo>.Enumerator enumerator = this.services.GetEnumerator();
                try
                {
                    while (true)
                    {
                        flag3 = enumerator.MoveNext();
                        if (!flag3)
                        {
                            break;
                        }
                        VcenterInfo current = enumerator.Current;
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
                flag2 = (this.services == null ? false : this.databases.Count > 0);
                flag3 = flag2;
                if (flag3)
                {
                    List<MySQLInfo>.Enumerator enumerator1 = this.databases.GetEnumerator();
                    try
                    {
                        while (true)
                        {
                            flag3 = enumerator1.MoveNext();
                            if (!flag3)
                            {
                                break;
                            }
                            MySQLInfo storageInfo = enumerator1.Current;
                            flag3 = storageInfo.Validate();
                            if (!flag3)
                            {
                                flag = false;
                                return flag;
                            }
                        }
                    }
                    finally
                    {
                        ((IDisposable)enumerator1).Dispose();
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
