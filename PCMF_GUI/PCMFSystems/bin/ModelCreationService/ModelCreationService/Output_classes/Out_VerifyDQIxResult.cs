using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ModelCreationService
{
    public class Out_VerifyDQIxResult
    {
        public List<double[]> listCOM_Ack;//(1)
        public List<double[]> listContextID;//(1)
        public List<double[]> listDQIxFlag;//(2)
        public List<double[]> listDQIx;//(3)
        public List<double[]> listDQIxThreshold;//(4)
        public List<double[]> listTypeIDt;//(5)
        public List<double[]> listStepTypeID;//(6)
        public List<double[]> listAccuracyResult;//(7)
        public List<double[]> listSizeInfo;//(8)

    }
}