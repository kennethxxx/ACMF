using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ModelCreationService
{
    public class Out_ConjectureModel
    {
        //BPNN output
        public List<double[]> nnListCOM_Ack;
        public List<double[]> nnListAll_ConjectureID;
        public List<double[]> nnListOutAll_ContextID;
        public List<double[]> nnListOutAll_PhaseID;
        public List<double[]> nnListOutAll_PointID;
        public List<double[]> nnListOutAll_PredictValue;
        public List<double[]> nnListOutAll_ErrorValue;
        public List<double[]> nnListOutAll_MaxError;
        public List<double[]> nnListOutAll_MAPE;
        public List<double[]> nnListOutAll_RT;
        public List<double[]> nnListOutAll_ConjectureTime;

        //MR output
        public List<double[]> mrListCOM_Ack;
        public List<double[]> mrListAll_ConjectureID;
        public List<double[]> mrListOutAll_ContextID;
        public List<double[]> mrListOutAll_PhaseID;
        public List<double[]> mrListOutAll_PointID;
        public List<double[]> mrListOutAll_PredictValue;
        public List<double[]> mrListOutAll_ErrorValue;
        public List<double[]> mrListOutAll_MaxError;
        public List<double[]> mrListOutAll_MAPE;
        public List<double[]> mrListOutAll_RT;
        public List<double[]> mrListOutAll_ConjectureTime;

        //RI output
        public List<double[]> riListOutRI_Value;
        public List<double[]> riListOutRI_Threshold;
        public List<double[]> riListOutTolerant_MaxError;
        public List<double[]> riListCOM_Ack;

        //GSI output
        public List<double[]> gsiListCOM_Ack;
        public List<double[]> gsiListOUTGSI_RT;
        public List<double[]> gsiListOUTGSI_RTsize;
        public List<double[]> gsiListOUTphaseID;
        public List<double[]> gsiListOUTstepID;
        public List<double[]> gsiListOUTcontextID;
        public List<double[]> gsiListOUTGSIvalue;
        public List<double[]> gsiListOUTGSIThreshold;
        public List<double[]> gsiListOUTtop10_PhaseID;
        public List<double[]> gsiListOUTtop10_ContextID;
        public List<double[]> gsiListOUTtop10_IndicatorID;
        public List<double[]> gsiListOUTtop10_StepTypeID;
        public List<double[]> gsiListOUTtop10_ISIValue;
        public List<double[]> gsiListOUTISIsize;

        //Y(Real) Value
        public List<double[]> listY;
    }
}