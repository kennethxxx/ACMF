using System;
using System.Collections.Generic;
using System.Linq;

namespace WorkerRoleController.WRScalingRuleFileParser
{
    public class Algorithm
    {
        private string name;
        private string uri;
        private string type;

        private readonly List<Input> input;
        private readonly List<Output> output;

        public string Uri
        {
            get
            {
                string str = this.uri;
                return str;
            }
            set
            {
                this.uri = value;
            }
        }

        public string Type
        {
            get
            {
                string str = this.type;
                return str;
            }
            set
            {
                this.type = value;
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

        public Algorithm()
        {
            this.input = new List<Input>();
            this.output = new List<Output>();
        }

        public override bool Equals(object obj)
        {
            bool flag;
            bool flag1;
            flag1 = (obj == null ? false : !(this.GetType() != obj.GetType()));
            bool flag2 = flag1;
            if (flag2)
            {
                Algorithm algorithmInfo = (Algorithm)obj;
                flag = this.name.Equals(algorithmInfo.Name);
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        public override int GetHashCode()
        {
            int hashCode = this.name.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            bool flag;
            string str = "";
            List<Input>.Enumerator enumerator = this.input.GetEnumerator();
            try
            {
                while (true)
                {
                    flag = enumerator.MoveNext();
                    if (!flag)
                    {
                        break;
                    }
                    Input current = enumerator.Current;
                    str = string.Concat(str, "\t", current.ToString(), "\n");
                }
            }
            finally
            {
                ((IDisposable)enumerator).Dispose();
            }
            string str1 = "";
            List<Output>.Enumerator enumerator1 = this.output.GetEnumerator();
            try
            {
                while (true)
                {
                    flag = enumerator1.MoveNext();
                    if (!flag)
                    {
                        break;
                    }
                    Output outputInfo = enumerator1.Current;
                    str1 = string.Concat(str1, "\t", outputInfo.ToString(), "\n");
                }
            }
            finally
            {
                ((IDisposable)enumerator1).Dispose();
            }
            string[] strArrays = new string[] { "------Algorithm------\nName: ", this.name, "\nUri: ", this.uri, "\nType: ", this.type, "\n", str1, "\n", str, "\n" };
            string str2 = string.Concat(strArrays);
            return str2;
        }

        public bool Validate()
        {            
            bool flag;
            bool flag1;
            bool flag2;
            bool flag3 = (string.IsNullOrEmpty(this.name) ? false : string.IsNullOrEmpty(this.uri) ? false : this.Type.Equals("JWS".ToLower()) ? true : this.Type.Equals("WCF".ToLower()));
            if (flag3)
            {
                flag1 = (this.input == null ? false : this.input.Count > 0);
                flag3 = flag1;
                if (flag3)
                {
                    List<Input>.Enumerator enumerator = this.input.GetEnumerator();
                    try
                    {
                        while (true)
                        {
                            flag3 = enumerator.MoveNext();
                            if (!flag3)
                            {
                                break;
                            }
                            Input current = enumerator.Current;
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
                    flag2 = (this.output == null ? false : this.output.Count > 0);
                    flag3 = flag2;
                    if (flag3)
                    {
                        List<Output>.Enumerator enumerator1 = this.output.GetEnumerator();
                        try
                        {
                            while (true)
                            {
                                flag3 = enumerator1.MoveNext();
                                if (!flag3)
                                {
                                    break;
                                }
                                Output outputInfo = enumerator1.Current;
                                flag3 = outputInfo.Validate();
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

        public bool AddInput(Input input)
        {
            bool flag;
            bool flag1 = this.input.Contains(input);
            if (flag1)
            {
                flag = false;
            }
            else
            {
                this.input.Add(input);
                flag = true;
            }
            return flag;
        }

        public List<Input> Inputs
        {
            get
            {
                List<Input> inputInfos = this.input;
                return inputInfos;
            }
            set
            {
                this.input.Clear();
                this.input.AddRange(value);
            }
        }

        public bool AddOutput(Output output)
        {
            bool flag;
            bool flag1 = this.output.Contains(output);
            if (flag1)
            {
                flag = false;
            }
            else
            {
                this.output.Add(output);
                flag = true;
            }
            return flag;
        }

        public List<Output> Outputs
        {
            get
            {
                List<Output> outputInfos = this.output;
                return outputInfos;
            }
            set
            {
                this.output.Clear();
                this.output.AddRange(value);
            }
        }
    }
}
