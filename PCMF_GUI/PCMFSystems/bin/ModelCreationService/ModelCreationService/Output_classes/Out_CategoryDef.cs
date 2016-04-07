using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ModelCreationService
{
    public class Out_CategoryDef
    {
        //public List<string[]> Categorydef;
        public List<string> position;
        public List<string> seat_number;
        public List<string> producttype;
        public List<string> cnc_number;
        public List<string> G_Code;
        public List<string> PostYType_1;
        public List<string> PostYType_2;
        public int Ack;//Ack = 1 表示error
    }
}