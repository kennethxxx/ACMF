using System;
using System.Collections.Generic;
using System.Linq;

namespace WorkerRoleController.WrcConfigFileParser
{
    public class SubscriptionInfo
    {
        private string name;
        private string id;

        private readonly List<HostedServiceInfo> services;

        private readonly List<StorageInfo> storages;

        public List<HostedServiceInfo> HostedServices
        {
            get
            {
                List<HostedServiceInfo> hostedServiceInfos = this.services;
                return hostedServiceInfos;
            }
            set
            {
                this.services.Clear();
                this.services.AddRange(value);
            }
        }

        public string Id
        {
            get
            {
                string str = this.id;
                return str;
            }
            set
            {
                this.id = value;
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

        public List<StorageInfo> Storages
        {
            get
            {
                List<StorageInfo> storageInfos = this.storages;
                return storageInfos;
            }
            set
            {
                this.storages.Clear();
                this.storages.AddRange(value);
            }
        }

        public SubscriptionInfo()
        {
            this.services = new List<HostedServiceInfo>();
            this.storages = new List<StorageInfo>();
        }

        public bool AddNewService(HostedServiceInfo service)
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

        public bool AddNewStorage(StorageInfo storage)
        {
            bool flag;
            bool flag1 = this.storages.Contains(storage);
            if (flag1)
            {
                flag = false;
            }
            else
            {
                this.storages.Add(storage);
                flag = true;
            }
            return flag;
        }

        public override string ToString()
        {
            bool flag;
            string str = "";
            List<HostedServiceInfo>.Enumerator enumerator = this.services.GetEnumerator();
            try
            {
                while (true)
                {
                    flag = enumerator.MoveNext();
                    if (!flag)
                    {
                        break;
                    }
                    HostedServiceInfo current = enumerator.Current;
                    str = string.Concat(str, "\t", current.ToString(), "\n");
                }
            }
            finally
            {
                ((IDisposable)enumerator).Dispose();
            }
            string str1 = "";
            List<StorageInfo>.Enumerator enumerator1 = this.storages.GetEnumerator();
            try
            {
                while (true)
                {
                    flag = enumerator1.MoveNext();
                    if (!flag)
                    {
                        break;
                    }
                    StorageInfo storageInfo = enumerator1.Current;
                    str1 = string.Concat(str1, "\t", storageInfo.ToString(), "\n");
                }
            }
            finally
            {
                ((IDisposable)enumerator1).Dispose();
            }
            string[] strArrays = new string[] { "------Subscription------\nName: ", this.name, "\nId: ", this.id, "\n", str1, "\n", str, "\n" };
            string str2 = string.Concat(strArrays);
            return str2;
        }

        public bool Validate()
        {
            bool flag;
            bool flag1;
            bool flag2;
            bool flag3 = !string.IsNullOrEmpty(this.id);
            if (flag3)
            {
                flag1 = (this.services == null ? false : this.services.Count > 0);
                flag3 = flag1;
                if (flag3)
                {
                    List<HostedServiceInfo>.Enumerator enumerator = this.services.GetEnumerator();
                    try
                    {
                        while (true)
                        {
                            flag3 = enumerator.MoveNext();
                            if (!flag3)
                            {
                                break;
                            }
                            HostedServiceInfo current = enumerator.Current;
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
                    flag2 = (this.services == null ? false : this.storages.Count > 0);
                    flag3 = flag2;
                    if (flag3)
                    {
                        List<StorageInfo>.Enumerator enumerator1 = this.storages.GetEnumerator();
                        try
                        {
                            while (true)
                            {
                                flag3 = enumerator1.MoveNext();
                                if (!flag3)
                                {
                                    break;
                                }
                                StorageInfo storageInfo = enumerator1.Current;
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
            }
            else
            {
                flag = false;
            }
            return flag;
        }

    }
}
