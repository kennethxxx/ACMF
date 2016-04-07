using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ModelManagementServices
{
    public class NamesByte
    {
        public List<string> modelNameList { get; set; }
        public List<double> matSize { get; set; }
        public byte[] modelByte { get; set; }
    }
}