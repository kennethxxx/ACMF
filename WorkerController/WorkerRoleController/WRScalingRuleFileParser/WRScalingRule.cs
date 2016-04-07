using System;
using System.Collections.Generic;


namespace WorkerRoleController.WRScalingRuleFileParser
{
    public class WRScalingRule
    {
        private List<Algorithm> algorithms;
        private string selectAlgorithm;

        public string SelectAlgorithm
        {
            get
            {
                string str = this.selectAlgorithm;
                return str;
            }
            set
            {
                this.selectAlgorithm = value;
            }
        }

        public List<Algorithm> Algorithms
        {
            get
            {
                List<Algorithm> algorithmInfo = this.algorithms;
                return algorithmInfo;
            }
            set
            {
                this.algorithms = value;
            }
        }

        public WRScalingRule()
        {
            this.algorithms = new List<Algorithm>();
        }

        public override string ToString()
        {
            string str = "";
            List<Algorithm>.Enumerator enumerator = this.algorithms.GetEnumerator();
            try
            {
                while (true)
                {
                    bool flag = enumerator.MoveNext();
                    if (!flag)
                    {
                        break;
                    }
                    Algorithm current = enumerator.Current;
                    str = string.Concat(str, current.ToString(), "\n");
                }
            }
            finally
            {
                ((IDisposable)enumerator).Dispose();
            }
            return str;
        }

        public bool Validate()
        {
            bool flag;
            bool flag1;
            flag1 = (this.algorithms == null ? false : this.algorithms.Count > 0);
            bool flag2 = flag1;
            if (flag2)
            {
                List<Algorithm>.Enumerator enumerator = this.algorithms.GetEnumerator();
                try
                {
                    while (true)
                    {
                        flag2 = enumerator.MoveNext();
                        if (!flag2)
                        {
                            break;
                        }
                        Algorithm current = enumerator.Current;
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
