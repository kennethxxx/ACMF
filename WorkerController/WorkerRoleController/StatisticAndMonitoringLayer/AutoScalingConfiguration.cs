using System;
using System.Collections.Generic;
using System.Linq;
using WorkerRoleController.WrcConfigFileParser;

namespace WorkerRoleController.WrcConfigFileParser
{
    public class AutoScalingConfiguration
    {
        private List<SubscriptionInfo> subscriptions;

        private MonitoringWRPeriod wRPeriod;

        public MonitoringWRPeriod WRPeriod
        {
            get
            {
                MonitoringWRPeriod monitoringWRPeriod = this.wRPeriod;
                return monitoringWRPeriod;
            }
            set
            {
                this.wRPeriod = value;
            }
        }

        public List<SubscriptionInfo> Subscriptions
        {
            get
            {
                List<SubscriptionInfo> subscriptionInfos = this.subscriptions;
                return subscriptionInfos;
            }
            set
            {
                this.subscriptions = value;
            }
        }

        public AutoScalingConfiguration()
        {
            this.subscriptions = new List<SubscriptionInfo>();
        }

        public override string ToString()
        {
            string str = "";
            List<SubscriptionInfo>.Enumerator enumerator = this.subscriptions.GetEnumerator();
            try
            {
                while (true)
                {
                    bool flag = enumerator.MoveNext();
                    if (!flag)
                    {
                        break;
                    }
                    SubscriptionInfo current = enumerator.Current;
                    str = string.Concat(str, current.ToString(), "\n");
                }
            }
            finally
            {
                ((IDisposable)enumerator).Dispose();
            }
            string str1 = string.Concat(this.WRPeriod.ToString(), "\n\n-----------Subscriptions-------------\n\n", str);
            return str1;
        }

        public bool Validate()
        {
            bool flag;
            bool flag1;
            flag1 = (this.subscriptions == null ? false : this.subscriptions.Count > 0);
            bool flag2 = flag1;
            if (flag2)
            {
                List<SubscriptionInfo>.Enumerator enumerator = this.subscriptions.GetEnumerator();
                try
                {
                    while (true)
                    {
                        flag2 = enumerator.MoveNext();
                        if (!flag2)
                        {
                            break;
                        }
                        SubscriptionInfo current = enumerator.Current;
                        flag2 = current.Validate();
                        if (!flag2)
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
            return flag;
        }

    }
}
