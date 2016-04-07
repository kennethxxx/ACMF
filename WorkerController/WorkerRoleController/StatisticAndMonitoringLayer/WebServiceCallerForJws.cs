using System;
using System.Xml;
using System.Collections;
using System.Net;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Web;
using System.Collections.Generic;

namespace WorkerRoleController.StatisticAndMonitoringLayer
{
    public class WebServiceCallerForJws
    {
        //<webServices>   
        //  <protocols>   
        //    <add name="HttpGet"/>   
        //    <add name="HttpPost"/>   
        //  </protocols>   
        //</webServices>   
        private static readonly Hashtable xmlNamespaces = new Hashtable();//缓存xmlNamespace，避免重复调用GetNamespace   

        /// <summary>   
        /// 需要WebService支持Post调用   
        /// </summary>   
        public static XmlDocument QueryPostWebService(String url, String methodName, Hashtable pars)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url + "/" + methodName);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            SetWebRequest(request);
            byte[] data = EncodePars(pars);
            WriteRequestData(request, data);
            return ReadXmlResponse(request.GetResponse());
        }

        /// <summary>   
        /// 需要WebService支持Get调用   
        /// </summary>   
        public static XmlDocument QueryGetWebService(String url, String methodName, Hashtable pars)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url + "/" + methodName + "?" + ParsToString(pars));
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            SetWebRequest(request);
            return ReadXmlResponse(request.GetResponse());
        }

        /// <summary>   
        /// 通用WebService调用(Soap),参数Pars为String类型的参数名、参数值   
        /// </summary>   
        public static XmlDocument QuerySoapWebService(String url, String methodName, Dictionary<string, string> pars)
        {
            if (xmlNamespaces.ContainsKey(url))
            {
                return QuerySoapWebService(url, methodName, pars, xmlNamespaces[url].ToString());
            }
            else
            {
                return QuerySoapWebService(url, methodName, pars, GetNamespace(url));
                //return QuerySoapWebService(url, methodName, pars, url);
            }
        }

        private static XmlDocument QuerySoapWebService(String url, String methodName, Dictionary<string, string> pars, string xmlNs)
        {
            xmlNamespaces[url] = xmlNs;//加入缓存，提高效率   
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "text/xml; charset=utf-8";
            request.Headers.Add("SOAPAction", "\"" + xmlNs + (xmlNs.EndsWith("/") ? "" : "/") + methodName + "\"");
            SetWebRequest(request);
            byte[] data = EncodeParsToSoap(pars, xmlNs, methodName);
            WriteRequestData(request, data);
            XmlDocument doc = new XmlDocument(), doc2 = new XmlDocument();
            doc = ReadXmlResponse(request.GetResponse());
            XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);
            mgr.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
            String retXml = doc.SelectSingleNode("//soap:Body/*/*", mgr).InnerXml;
            doc2.LoadXml("<root>" + retXml + "</root>");
            AddDelaration(doc2);
            return doc2;
        }

        private static string GetNamespace(String url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            SetWebRequest(request);
            WebResponse response = request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sr.ReadToEnd());
            sr.Close();
            return doc.SelectSingleNode("//@targetNamespace").Value;
        }

        private static byte[] EncodeParsToSoap(Dictionary<string, string> pars, String xmlNs, String methodName)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\"></soap:Envelope>");
            AddDelaration(doc);
            XmlElement soapBody = doc.CreateElement("soap", "Body", "http://schemas.xmlsoap.org/soap/envelope/");
            XmlElement soapMethod = doc.CreateElement(methodName);
            soapMethod.SetAttribute("xmlns", xmlNs);
            foreach (string k in pars.Keys)
            {
                XmlElement soapPar = doc.CreateElement(k);
                soapPar.InnerXml = ObjectToSoapXml(pars[k]);
                soapMethod.AppendChild(soapPar);
            }
            soapBody.AppendChild(soapMethod);
            doc.DocumentElement.AppendChild(soapBody);
            return Encoding.UTF8.GetBytes(doc.OuterXml);
        }

        private static string ObjectToSoapXml(object o)
        {
            XmlSerializer mySerializer = new XmlSerializer(o.GetType());
            MemoryStream ms = new MemoryStream();
            mySerializer.Serialize(ms, o);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Encoding.UTF8.GetString(ms.ToArray()));
            if (doc.DocumentElement != null)
            {
                return doc.DocumentElement.InnerXml;
            }
            else
            {
                return o.ToString();
            }
        }

        private static void SetWebRequest(HttpWebRequest request)
        {
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Timeout = 10000;
        }

        private static void WriteRequestData(HttpWebRequest request, byte[] data)
        {
            request.ContentLength = data.Length;
            Stream writer = request.GetRequestStream();
            writer.Write(data, 0, data.Length);
            writer.Close();
        }

        private static byte[] EncodePars(Hashtable pars)
        {
            return Encoding.UTF8.GetBytes(ParsToString(pars));
        }

        private static String ParsToString(Hashtable pars)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string k in pars.Keys)
            {
                if (sb.Length > 0)
                {
                    sb.Append("&");
                }
                sb.Append(HttpUtility.UrlEncode(k) + "=" + HttpUtility.UrlEncode(pars[k].ToString()));
                //sb.Append(HttpUtility.UrlEncode(k) + "=" + HttpUtility.UrlEncode(Pars[k].ToString()));   
            }
            return sb.ToString();
        }

        private static XmlDocument ReadXmlResponse(WebResponse response)
        {
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            String retXml = sr.ReadToEnd();
            sr.Close();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(retXml);
            return doc;
        }

        private static void AddDelaration(XmlDocument doc)
        {
            XmlDeclaration decl = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.InsertBefore(decl, doc.DocumentElement);
        }
    }
}