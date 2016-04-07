using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WcfSamples.DynamicProxy;
using System.ServiceModel.Description;
using System.Collections;
using System.Xml;

namespace WorkerRoleController.StatisticAndMonitoringLayer
{
    public class WRStatisticAndMonitoringLayer
    {
        public WRStatisticAndMonitoringLayer() { }

        public double ServiceParamsInfoJws(string uristring, List<string> wscall)
        {
            if (!uristring.Equals(null) && wscall.Count != 0)
            {
                //Hashtable pars = new Hashtable();
                Dictionary<string, string> pars = new Dictionary<string, string>();
                //pars["city"] = "上海";
                //pars["wdate"] = "2008-3-19";               
                for (int i = 1; i < wscall.Count(); i++)
                {
                    string[] param = (wscall[i]).Split(new char[] { '@' });
                    pars.Add(param[0], param[1]);
                }

                String url = uristring;
                XmlDocument doc = WebServiceCallerForJws.QuerySoapWebService(url, wscall[0], pars);
                XmlNode result = doc.SelectSingleNode("/root");
                if (result.InnerText.Equals(null) || result.InnerText.Equals("") || result.InnerText == null || result.InnerText == "")
                {
                    return 0;
                }
                else
                {
                    return double.Parse(result.InnerText);
                }
            }
            return -1;
        }

        public double ServiceParamsInfoWcf(string uristring, List<string> wscall)
        {
            if (!uristring.Equals(null) && wscall.Count != 0)
            {
                object[] tArgs = new object[wscall.Count()];
                for (int i = 1; i <= wscall.Count(); i++)
                {
                    string[] param = (wscall[i]).Split(new char[] { '@' });
                    //tArgs.Add(param[0], param[1]);
                    tArgs[i - 1] = param[1];
                }
                var result = WebServiceInvokeForWcf(uristring, wscall[0], tArgs);
                return (double)result;
            }
            return -1;
        }

        public double ServiceNoParamsInfoJws(string uristring, List<string> methodParams)
        {
            if (!uristring.Equals(null) && methodParams.Count != 0)
            {
                Dictionary<string, string> pars = new Dictionary<string, string>();
                //Hashtable pars = new Hashtable();
                String url = uristring;
                //pars["city"] = "T";
                //pars["wdate"] = "2008-3-19";
                XmlDocument doc = WebServiceCallerForJws.QuerySoapWebService(url, methodParams[0], pars);
                XmlNode result = doc.SelectSingleNode("/root");
                if (result.InnerText.Equals(null) || result.InnerText.Equals("") || result.InnerText == null || result.InnerText == "")
                {
                    return 0;
                }
                else
                {
                    return double.Parse(result.InnerText);
                }

                //XmlDocument doc = WebSvcCaller.QueryPostWebService(Url, methodParams[0], pars);

            }
            return -1;
        }

        public double ServiceNoParamsInfoWcf(string uristring, List<string> methodParams)
        {
            if (!uristring.Equals(null) && methodParams.Count != 0)
            {
                var result = WebServiceInvokeForWcf(uristring, methodParams[0], null);
                return (double)result;

            }
            return -1;
        }

        public object WebServiceInvokeForWcf(string pUrl, string pMethodName, params object[] pParams)
        {
            string serviceWsdlUri = pUrl;

            DynamicProxyFactory factory = new DynamicProxyFactory(serviceWsdlUri);
            int count = 0;
            List<object> myEndpoints = new List<object>();

            foreach (ServiceEndpoint endpoint in factory.Endpoints)
            {
                myEndpoints.Add(endpoint.Contract.Name);
            }
            foreach (string endpoint in myEndpoints)
            {
                DynamicProxy dp = factory.CreateProxy(endpoint);
                OperationDescriptionCollection operations = factory.GetEndpoint(endpoint).Contract.Operations;
                Type proxyType = dp.ProxyType;
                MethodInfo[] mi = proxyType.GetMethods(BindingFlags.Public | BindingFlags.Instance);
                for (int i = 0; i < mi.Length; i++)
                {
                    string name = mi[i].Name;
                    if (name == pMethodName)
                    {
                        DynamicProxy proxy = factory.CreateProxy(endpoint);
                        return proxy.CallMethod(pMethodName, pParams);
                    }
                }
                dp.Close();
            }
            return null;
        }

    }
}
