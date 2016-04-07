using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkerRoleController.PrivateWRConfigFileParser
{
    public class MySQLInfo
    {
        private string MyIP;

        private string MyUser;

        private string MyPassword;

        private string MyPort;

        public string myIP
        {
            get
            {
                string str = this.MyIP;
                return str;
            }
            set
            {
                this.MyIP = value;
            }
        }

        public string myUser
        {
            get
            {
                string str = this.MyUser;
                return str;
            }
            set
            {
                this.MyUser = value;
            }
        }

        public string myPassword
        {
            get
            {
                string str = this.MyPassword;
                return str;
            }
            set
            {
                this.MyPassword = value;
            }
        }

        public string myPort
        {
            get
            {
                string str = this.MyPort;
                return str;
            }
            set
            {
                this.MyPort = value;
            }
        }

        public MySQLInfo()
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
                MySQLInfo MySQLInfos = (MySQLInfo)obj;
                flag2 = !this.MyIP.Equals(MySQLInfos.MyIP);
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
            string[] strArrays = new string[] { "-------------Database------------\nMyIP: ", this.MyIP ,"\nMyPort: ", this.MyPort,"\nMyUser: ", this.MyUser,"\nMyPassword: ", this.MyPassword, "\n" };
            string str = string.Concat(strArrays);
            return str;
        }

        public bool Validate()
        {
            bool flag;
            flag = (string.IsNullOrEmpty(this.MyIP) ? false : !string.IsNullOrEmpty(this.MyUser));
            bool flag1 = flag;
            return flag1;
        }
    }
}
