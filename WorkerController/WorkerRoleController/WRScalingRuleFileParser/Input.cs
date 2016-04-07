using System;
using System.Collections.Generic;
using System.Linq;

namespace WorkerRoleController.WRScalingRuleFileParser
{
    public class Input
    {
        private string functionCount;
        private readonly List<Element> elements;

        public string FunctionCount
        {
            get
            {
                string str = this.functionCount;
                return str;
            }
            set
            {
                this.functionCount = value;
            }
        }

        public Input()
        {
            this.elements = new List<Element>();
        }

        public List<Element> Elements
        {
            get
            {
                List<Element> element = this.elements;
                return element;
            }
            set
            {
                this.elements.Clear();
                this.elements.AddRange(value);
            }
        }

        public bool AddNewElement(Element element)
        {
            bool flag;
            bool flag1 = this.elements.Contains(element);
            if (flag1)
            {
                flag = false;
            }
            else
            {
                this.elements.Add(element);
                flag = true;
            }
            return flag;
        }

        public override string ToString()
        {
            bool flag;
            string str1 = "";
            List<Element>.Enumerator enumerator1 = this.elements.GetEnumerator();
            try
            {
                while (true)
                {
                    flag = enumerator1.MoveNext();
                    if (!flag)
                    {
                        break;
                    }
                    Element element = enumerator1.Current;
                    str1 = string.Concat(str1, "\t", element.ToString(), "\n");
                }
            }
            finally
            {
                ((IDisposable)enumerator1).Dispose();
            }
            string[] strArrays = new string[] { "\n-------Input-------\nFunctionCount: ", this.functionCount, "\n", str1, "\n_________________________________\n" };
            string str2 = string.Concat(strArrays);
            return str2;
        }

        public override bool Equals(object obj)
        {
            bool flag;
            bool flag1;
            flag1 = (obj == null ? false : !(this.GetType() != obj.GetType()));
            bool flag2 = flag1;
            if (flag2)
            {
                Input inputInfo = (Input)obj;
                flag = this.functionCount.Equals(inputInfo.functionCount);
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        public override int GetHashCode()
        {
            int hashCode = this.functionCount.GetHashCode();
            return hashCode;
        }

        public bool Validate()
        {
            bool flag;
            bool flag1;
            bool flag2;
            flag1 = (this.functionCount == null ? false : int.Parse(this.functionCount) > 0);
            bool flag3 = flag1;
            if (flag3)
            {
                flag2 = (this.elements == null ? false : this.elements.Count > 0);
                flag3 = flag2;
                if (flag3)
                {
                    List<Element>.Enumerator enumerator = this.elements.GetEnumerator();
                    try
                    {
                        while (true)
                        {
                            flag3 = enumerator.MoveNext();
                            if (!flag3)
                            {
                                break;
                            }
                            Element current = enumerator.Current;
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
