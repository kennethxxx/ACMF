using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ModelCreationService
{
    public class Out_IndicatorsPopulation
    {
        public List<string> WorkPieceID_Metrology;
        public List<string> WorkPieceID_Process;
        public List<string> listContexID;
        public List<DateTime> listProcessStartTime;
        public List<DateTime> listProcessEndTime;
        public List<DateTime> listMetrologyStartTime;
        public List<DateTime> listMetrologyEndTime;
        public List<double[]> listIndicatorPopulationValue;
        public List<int[]> listActionPopulationValue;   //Huy added

        public Dictionary<String, Indicator> listAllIndicators;    //all indicators & value
        public Dictionary<String, Indicator> listAllPoints;    //all points & value
        public List<Context> listContext;   //all context id

        public List<double[]> listPointPopulationValue;
        public double[] indicatorIndexList;
        public double[] pointIndexList;
        public int Ack;
    }
}