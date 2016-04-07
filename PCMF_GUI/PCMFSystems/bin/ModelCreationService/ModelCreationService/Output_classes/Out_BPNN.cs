using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ModelCreationService
{
    public class Out_BPNN
    {
        public List<double[]> listCOM_Ack;
        public List<double[]> listAll_ConjectureID;
        public List<double[]> listOutAll_ContextID;
        public List<double[]> listOutAll_PhaseID;
        public List<double[]> listOutAll_PointID;
        public List<double[]> listOutAll_PredictValue;
        public List<double[]> listOutAll_ErrorValue;
        public List<double[]> listOutAll_MaxError;
        public List<double[]> listOutAll_MAPE;
        public List<double[]> listOutAll_RT;
        public List<double[]> listOutAll_ConjectureTime;
        public bool AlgoValue;
        public List<double[]> listY;
    }
}