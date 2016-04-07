using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ModelCreationService
{
    public class Out_GSI
    {
        public List<double[]> listCOM_Ack;  //(0)
        public List<double[]> listOUTGSI_RT;//(1)
        public List<double[]> listOUTGSI_RTsize;//(2)
        public List<double[]> listOUTphaseID;//(3)
        public List<double[]> listOUTstepID;//(4)
        public List<double[]> listOUTcontextID;//(5)
        public List<double[]> listOUTGSIvalue;//(6)
        public List<double[]> listOUTGSIThreshold;//(7)
        public List<double[]> listOUTtop10_PhaseID;//(8)
        public List<double[]> listOUTtop10_ContextID;//(9)
        public List<double[]> listOUTtop10_IndicatorID;//(10)
        public List<double[]> listOUTtop10_StepTypeID;//(11)
        public List<double[]> listOUTtop10_ISIValue;//(12)
        public List<double[]> listOUTISIsize;//(13)
    }
}