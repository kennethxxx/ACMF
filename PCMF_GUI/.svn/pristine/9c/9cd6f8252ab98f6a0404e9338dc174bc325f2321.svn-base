using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using IPS.ServiceManager;
using System.Runtime.Serialization;
using System.IO;

namespace IPS.Common
{
    public class CommonValue
    {
        //public const int SERVICE_TIMEOUT = 240000;
        public const int SERVICE_TIMEOUT = 840000;

        public const string SERVICE_MANAGEMENT = "ServiceManager";
        public const string DATA_ACQUISITION = "DataAcquisition";
        public const string MODEL_CREATION = "ModelCreation";
        public const string MODEL_MANAGEMENT = "ModelManagement";
        public const string USER_MANAGEMENT = "UserManagement";
        public const string ONTOLOGY = "Ontology";

        //Function Key
        public static Dictionary<string, string> FunctionKeys = new Dictionary<string, string>();

        //Function Usuage
        public static Dictionary<string, bool> FunctionState = new Dictionary<string, bool>();

        public const int NUMBER_OF_PHASE = 5;  //total number of phase: Stage1, Stage2, FreeRun, Phase1, Phase2

        //phase in detail & value: Stage1, Stage2, FreeRun, Phase1, Phase2
        public static Dictionary<int, string> phaseDic = new Dictionary<int, string>()
	    {
	        {1, "Stage1"},
	        {2, "Stage2"},
	        {3, "FreeRun"},
	        {4, "PhaseI"},
	        {5, "PhaseII"}
	    };

        public static Color NN_COLOR = Colors.Black;
        public static Color MR_COLOR = Color.FromArgb(255,0,255,255);

        public static Color THRESHOLD_COLOR = Colors.Red;
        public static Color RI_GSI_COLOR = Colors.Black;

        public static Color DQIY_COLOR = Colors.Purple;
        public static Color DQIX_COLOR = Colors.Purple;
        public static Color YVALUE_COLOR = Color.FromArgb(255,255,0,255);

        public static Color TREND_USL_COLOR = Colors.Red;
        public static Color TREND_UCL_COLOR = Colors.Green;

        public static T DataContractClone<T>(T source)
        {
            DataContractSerializer serializer;
            serializer = new DataContractSerializer(typeof(T));

            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, source);
                ms.Seek(0, SeekOrigin.Begin);

                return (T)serializer.ReadObject(ms);
            }
        }
    }
}
