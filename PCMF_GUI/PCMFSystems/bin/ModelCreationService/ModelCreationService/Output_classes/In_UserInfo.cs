using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ModelCreationService
{
    public class In_UserInfo
    {
        public String Company;
        public String User;
        public String FullName
        {
            get { return Company + "_" + User; }
        }
    }
}