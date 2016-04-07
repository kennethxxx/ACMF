using System;
using System.Collections.Generic;
using System.Linq;

namespace WorkerRoleController.WrcConfigFileParser
{
    public class HostedServiceInfo
    { 
        private string name;

        private string urlPrefix;

        private readonly List<DeploymentInfo> deployments;

        public List<DeploymentInfo> Deployments
        {
            get
            {
                List<DeploymentInfo> deploymentInfos = this.deployments;
                return deploymentInfos;
            }
            set
            {
                this.deployments.Clear();
                this.deployments.AddRange(value);
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

        public string UrlPrefix
        {
            get
            {
                string str = this.urlPrefix;
                return str;
            }
            set
            {
                this.urlPrefix = value;
            }
        }

        public HostedServiceInfo()
        {
            this.deployments = new List<DeploymentInfo>();
        }

        public bool AddNewDeployment(DeploymentInfo deployment)
        {
            bool flag;
            bool flag1 = this.deployments.Contains(deployment);
            if (flag1)
            {
                flag = false;
            }
            else
            {
                this.deployments.Add(deployment);
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
                HostedServiceInfo hostedServiceInfo = (HostedServiceInfo)obj;
                flag = this.urlPrefix.Equals(hostedServiceInfo.urlPrefix);
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        public override int GetHashCode()
        {
            int hashCode = this.urlPrefix.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            bool flag;
            string str1 = "";
            List<DeploymentInfo>.Enumerator enumerator1 = this.deployments.GetEnumerator();
            try
            {
                while (true)
                {
                    flag = enumerator1.MoveNext();
                    if (!flag)
                    {
                        break;
                    }
                    DeploymentInfo deploymentInfo = enumerator1.Current;
                    str1 = string.Concat(str1, "\t", deploymentInfo.ToString(), "\n");
                }
            }
            finally
            {
                ((IDisposable)enumerator1).Dispose();
            }
            string[] strArrays = new string[] { "\n-------Hosted Service-------\nName: ", this.name, "\n" , str1, "\n_________________________________\n" };
            string str2 = string.Concat(strArrays);
            return str2;
        }

        public bool Validate()
        {
            bool flag;
            bool flag1;
            bool flag2;
            flag1 = (string.IsNullOrEmpty(this.name) ? false : !string.IsNullOrEmpty(this.urlPrefix));
            bool flag3 = flag1;
            if (flag3)
            {
                flag2 = (this.deployments == null ? false : this.deployments.Count > 0);
                flag3 = flag2;
                if (flag3)
                {
                    List<DeploymentInfo>.Enumerator enumerator = this.deployments.GetEnumerator();
                    try
                    {
                        while (true)
                        {
                            flag3 = enumerator.MoveNext();
                            if (!flag3)
                            {
                                break;
                            }
                            DeploymentInfo current = enumerator.Current;
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
