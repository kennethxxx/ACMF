using System;
using System.Collections.Generic;
using System.Linq;
using WorkerRoleController.StatisticAndMonitoringLayer;
using WorkerRoleController.WRScalingRuleFileParser;
using System.Data;

namespace WorkerRoleController.WRInstanceEvaluator
{
    public class WREvaluator
    {
        private WRScalingRule wRScalingXmlRule;
        private string xmlConfigurationSource;
        private List<Element> inputElement;
        private double vmcount;


        public WREvaluator()
        {
            this.inputElement = new List<Element>();
        }

        public int GetVMCount()
        {
            try
            {
                wRScalingXmlRule = GetWRScalingRuleXmlFile();
                this.inputElement = GetWRScalingRuleInput(wRScalingXmlRule);
                if (vmcount != -1 && !vmcount.Equals(null))
                {
                    vmcount = GetWRScalingRuleOutput(wRScalingXmlRule, inputElement);
                    return (int)vmcount;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.Message);
            }
            return -1;
        }

        private WRScalingRule GetWRScalingRuleXmlFile()
        {
            xmlConfigurationSource = Utils.GetConfigurations(2);
            WrsRuleXmlFileParser.ParseXml(xmlConfigurationSource);
            //System.Diagnostics.Debug.WriteLine(WrsRuleXmlFileParser.ParseXml(xmlConfigurationSource));
            this.wRScalingXmlRule = WrsRuleXmlFileParser.ParsedConfig;
            return wRScalingXmlRule;
        }

        private List<Element> GetWRScalingRuleInput(WRScalingRule wRScalingXmlRule)
        {
            foreach (Algorithm algorithmAux in wRScalingXmlRule.Algorithms)
            {
                if ((wRScalingXmlRule.SelectAlgorithm).Equals(algorithmAux.Name))
                {
                    foreach (Input inputsAux in algorithmAux.Inputs)
                    {
                        foreach (Element elementsAux in inputsAux.Elements)
                        {
                            string uristring;
                            Element wsElement = new Element();
                            if (algorithmAux.Type == "jws")
                            {
                                //http://140.116.86.249:8080/Z_TestService/services/TestService?wsdl
                                uristring = string.Format("{0}", algorithmAux.Uri);
                                wsElement.Name = elementsAux.Name;
                                wsElement.WsCall = GetWRStatisticAndMonitoringLayer(uristring, elementsAux.WsCall, algorithmAux.Type).ToString();
                                inputElement.Add(wsElement);
                            }
                            else
                            {
                                //http://140.116.86.249/OntologyServiceWCFInterface/IontologyClassMain.svc?wsdl
                                uristring = string.Format("{0}/{1}.svc?wsdl", algorithmAux.Uri, algorithmAux.Name);
                                wsElement.Name = elementsAux.Name;
                                wsElement.WsCall = GetWRStatisticAndMonitoringLayer(uristring, elementsAux.WsCall, algorithmAux.Type).ToString();
                                inputElement.Add(wsElement);
                            }
                        }
                    }
                }
            }
            if (inputElement.Count != 0)
            {
                return inputElement;
            }
            else
            {
                return null;
            }
        }

        private double GetWRStatisticAndMonitoringLayer(string uristring, string wscall, string wstype)
        {
            List<string> methodParams = SearchParams(wscall);
            WRStatisticAndMonitoringLayer callService = new WRStatisticAndMonitoringLayer();
            if (methodParams.Count == 1)
            {
                string Str = methodParams[0];
                double Num;
                bool isNum = double.TryParse(Str, out Num);

                if (isNum)
                {
                    vmcount = Convert.ToDouble(methodParams[0]);
                    return vmcount;
                }
                else
                {
                    if (wstype == "wcf")
                    {
                        this.vmcount = callService.ServiceNoParamsInfoWcf(uristring, methodParams);
                        return vmcount;
                    }
                    else
                    {
                        this.vmcount = callService.ServiceNoParamsInfoJws(uristring, methodParams);
                        return vmcount;
                    }
                }
            }
            else
            {
                for (int i = 1; i < methodParams.Count(); i++)
                {
                    var result = inputElement.Find(x => x.Name == methodParams[i]);
                    methodParams[i] = methodParams[i] + "@" + result.WsCall;
                }

                if (wstype == "wcf")
                {
                    this.vmcount = callService.ServiceParamsInfoWcf(uristring, methodParams);
                    return vmcount;
                }
                else
                {
                    this.vmcount = callService.ServiceParamsInfoJws(uristring, methodParams);
                    return vmcount;
                }
            }
        }

        private List<string> SearchParams(string wscall)
        {
            List<string> methodAndParams = new List<string>();
            int str1 = wscall.IndexOf("(");
            int str2 = wscall.IndexOf(")");

            if (str1 == -1 && str2 == -1)
            {
                methodAndParams.Add(wscall.ToString().Trim());
            }
            else
            {
                methodAndParams.Add(wscall.Substring(0, str1));

                if (str2 - str1 != 1)
                {
                    string paramss = wscall.Substring(str1 + 1, str2 - str1 - 1);
                    string[] words = paramss.Split(new char[] { ',' });
                    foreach (string i in words)
                    {
                        methodAndParams.Add(i.ToString().Trim());
                    }
                }
            }
            return methodAndParams;
        }

        private double GetWRScalingRuleOutput(WRScalingRule wRScalingXmlRule, List<Element> inputElement)
        {
            string outputelement = default(string);
            foreach (Algorithm algorithmAux in wRScalingXmlRule.Algorithms)
            {
                if ((wRScalingXmlRule.SelectAlgorithm).Equals(algorithmAux.Name))
                {
                    foreach (Output outputsAux in algorithmAux.Outputs)
                    {
                        foreach (Element i in inputElement)
                        {
                            outputelement = outputsAux.OutputInfo.Replace(i.Name, i.WsCall);
                        }

                    }
                }
            }
            vmcount = int.Parse(new DataTable().Compute(outputelement, null).ToString());
            if (vmcount != -1 && !vmcount.Equals(null))
            {
                return vmcount;
            }
            else
            {
                return 0;
            }
        }

    }
}
