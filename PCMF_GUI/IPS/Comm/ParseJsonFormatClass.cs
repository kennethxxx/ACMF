using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace IPS.Comm
{
    public class ParseJsonFormatClass
    {
        public ParseJsonFormatClass()
        {

        }

        //Json String→Class
        public static ObservableCollection<OntologyDatabaseData> Get_KnowledgeBase(string JsonStr)
        {
            //知識庫資訊 Json反序列化
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(ObservableCollection<OntologyDatabaseData>));
            MemoryStream responseStream = new MemoryStream(Encoding.UTF8.GetBytes(JsonStr));
            ObservableCollection<OntologyDatabaseData> knowledgeBaseitemsSource = ser.ReadObject(responseStream) as ObservableCollection<OntologyDatabaseData>;
            return knowledgeBaseitemsSource;
        }

        public static ObservableCollection<PE_SelectRulesCT> Get_Rule(string JsonStr)
        {
            //規則資訊 Json反序列化
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(ObservableCollection<PE_SelectRulesCT>));
            MemoryStream responseStream = new MemoryStream(Encoding.UTF8.GetBytes(JsonStr));
            ObservableCollection<PE_SelectRulesCT> ruleitemsSource = ser.ReadObject(responseStream) as ObservableCollection<PE_SelectRulesCT>;
            return ruleitemsSource;
        }

        public static ObservableCollection<VECollisionInfo> Get_CollisionResult(string JsonStr)
        {
            //碰撞結果
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(ObservableCollection<VECollisionInfo>));
            MemoryStream responseStream = new MemoryStream(Encoding.UTF8.GetBytes(JsonStr));
            ObservableCollection<VECollisionInfo> CollisionCT = ser.ReadObject(responseStream) as ObservableCollection<VECollisionInfo>;
            return CollisionCT;
        }

        public static ObservableCollection<OntologyResultForNCFile> Get_InferenceResult(string JsonStr)
        {
            //推論結果
            DataContractJsonSerializer serinferForNC = new DataContractJsonSerializer(typeof(ObservableCollection<OntologyResultForNCFile>));
            MemoryStream responseStreamNC = new MemoryStream(Encoding.UTF8.GetBytes(JsonStr));
            ObservableCollection<OntologyResultForNCFile> InferNC = serinferForNC.ReadObject(responseStreamNC) as ObservableCollection<OntologyResultForNCFile>;
            return InferNC;
        }
        //Class→Json String
        public static string Get_OntologyResultForNCFile(ObservableCollection<OntologyResultForNCFile> NCList)
        {
            MemoryStream ms = new MemoryStream();
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ObservableCollection<OntologyResultForNCFile>));
            serializer.WriteObject(ms, NCList);
            byte[] json = ms.ToArray();
            return Encoding.UTF8.GetString(json, 0, json.Length);
        }

        public static string Get_OntologyResultForCuttingTool(ObservableCollection<OntologyResultForCuttingTool> CTList)
        {
            MemoryStream ms = new MemoryStream();
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ObservableCollection<OntologyResultForCuttingTool>));
            serializer.WriteObject(ms, CTList);
            byte[] json = ms.ToArray();
            return Encoding.UTF8.GetString(json, 0, json.Length);
        }
    }
}