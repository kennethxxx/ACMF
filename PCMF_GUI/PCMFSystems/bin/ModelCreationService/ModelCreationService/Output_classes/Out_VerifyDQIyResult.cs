using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ModelCreationService
{
    public class Out_VerifyDQIyResult
    {
        public List<double[]> listCOM_Ack;//(0)
        public List<double[]> listContextID;//(1)
        public List<double[]> listPointList;//(2)
        public List<double[]> listPatternListIndex4PatternID;//(3)
        public List<double[]> listContextIDOfStepIndex;//(4)
        public List<double[]> listArtUList;//(5)
        public List<double[]> listIndicatorIDIndex4ContextID;//(6)
        public List<double[]> listContextIDIndex4PointID;//(7)
        public List<double[]> listDQIyResult;//(8)
        public List<double[]> listDQIyResultIndex;//(9)
        public List<double[]> listAccuracyResult;//(10)
        public List<double[]> listRunArtUList;//(11)
        public List<double[]> listRunDQIyData;//(12)
        public List<double[]> listRunContextInfo;//(13)
        public List<double[]> listIndicatorIDIndex4RunContextID;//(14)
        public List<double[]> listRunContextIDIndex4PointID;//(15)
        public List<double[]> listSizeInfo;//(16)

        //////////////////////////////////////////////////////////////
        public List<double[]> listDQIyResultChart;

        public TimeSpan timeSpan;

    }
}