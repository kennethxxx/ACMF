using System;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;
using System.IO;
using System.Xml.Schema;
using WorkerRoleController.WrcConfigFileParser;

namespace WorkerRoleController.WRScalingRuleFileParser
{
    public class WrsRuleXmlFileParser
    {
        private static WRScalingRule parsedConfig;

        public static WRScalingRule ParsedConfig
        {
            get
            {
                WRScalingRule configuration = WrsRuleXmlFileParser.parsedConfig;
                return configuration;
            }
        }

        static WrsRuleXmlFileParser()
        {
            WrsRuleXmlFileParser.parsedConfig = null;
        }

        public WrsRuleXmlFileParser()
        {
        }

        private static Algorithm GetAlgorithmInfo(XmlNode subNode)
        {
            IDisposable disposable;
            bool flag = subNode != null;
            if (!flag)
            {
                throw new Exception();
            }
            
            Algorithm algorithmInfo = new Algorithm();
            algorithmInfo.Name = WrsRuleXmlFileParser.ParseValue(subNode.Attributes["name"].Value);
            algorithmInfo.Uri = WrsRuleXmlFileParser.ParseValue(subNode.Attributes["uri"].Value);
            algorithmInfo.Type = WrsRuleXmlFileParser.ParseValue(subNode.Attributes["type"].Value);
            IEnumerator enumerator = subNode.SelectNodes("Input").GetEnumerator();
            try
            {
                while (true)
                {
                    flag = enumerator.MoveNext();
                    if (!flag)
                    {
                        break;
                    }
                    XmlNode current = (XmlNode)enumerator.Current;
                    flag = algorithmInfo.AddInput(WrsRuleXmlFileParser.GetInput(current));
                    if (!flag)
                    {
                        throw new Exception();
                    }
                }
            }
            finally
            {
                disposable = enumerator as IDisposable;
                flag = disposable == null;
                if (!flag)
                {
                    disposable.Dispose();
                }
            }
            enumerator = subNode.SelectNodes("Output").GetEnumerator();
            try
            {
                while (true)
                {
                    flag = enumerator.MoveNext();
                    if (!flag)
                    {
                        break;
                    }
                    XmlNode xmlNodes = (XmlNode)enumerator.Current;
                    flag = algorithmInfo.AddOutput(WrsRuleXmlFileParser.GetOutput(xmlNodes));
                    if (!flag)
                    {
                        throw new Exception();
                    }
                }
            }
            finally
            {
                disposable = enumerator as IDisposable;
                flag = disposable == null;
                if (!flag)
                {
                    disposable.Dispose();
                }
            }
            Algorithm algorithmInfo1 = algorithmInfo;
            return algorithmInfo1;
        }

        private static Output GetOutput(XmlNode outputNode)
        {
            Output output = new Output();
            output.OutputInfo = WrsRuleXmlFileParser.ParseValue(outputNode.FirstChild.Value);
            Output output1 = output;
            return output1;
        }

        private static Input GetInput(XmlNode inputNode)
        {
            bool flag;
            IDisposable disposable;
            Input inputInfo = new Input();
            inputInfo.FunctionCount = WrsRuleXmlFileParser.ParseValue(inputNode.Attributes["function_count"].Value);
            IEnumerator enumerator = inputNode.SelectNodes("Element").GetEnumerator();
            try
            {
                while (true)
                {
                    flag = enumerator.MoveNext();
                    if (!flag)
                    {
                        break;
                    }
                    XmlNode xmlNodes = (XmlNode)enumerator.Current;
                    flag = inputInfo.AddNewElement(WrsRuleXmlFileParser.GetElement(xmlNodes));
                    if (!flag)
                    {
                        throw new Exception();
                    }
                }
            }
            finally
            {
                disposable = enumerator as IDisposable;
                flag = disposable == null;
                if (!flag)
                {
                    disposable.Dispose();
                }
            }
            Input inputInfo1 = inputInfo;
            return inputInfo1;
        }

        private static Element GetElement(XmlNode elementNode)
        {
            Element elementInfo = new Element();
            elementInfo.Name = WrsRuleXmlFileParser.ParseValue(elementNode.Attributes["name"].Value);
            elementInfo.WsCall = WrsRuleXmlFileParser.ParseValue(elementNode.Attributes["ws_call"].Value);
            Element elementInfo1 = elementInfo;
            return elementInfo1;
        }

        private static string GetSelectAlgorithm(XmlNode subNode)
        {
            string selectAlgorithm = WrsRuleXmlFileParser.ParseValue(subNode.Attributes["select"].Value);
            return selectAlgorithm;
        }

        public static string GetXmlErrors()
        {
            string errors = WrsRuleXmlFileParser.XsdValidator.GetErrors();
            return errors;
        }

        private static string ParseValue(string s)
        {
            string str = s.Trim();
            return str;
        }

        public static bool ParseXml(string xml)
        {
            bool flag;
            try
            {
                bool flag1 = !WrsRuleXmlFileParser.XsdValidator.Validate(new MemoryStream(Encoding.UTF8.GetBytes(xml)));
                if (flag1)
                {
                    flag = false;
                }
                else
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(new StringReader(xml));
                    flag = WrsRuleXmlFileParser.ParseXml(xmlDocument.DocumentElement);
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception.ToString());
                WrsRuleXmlFileParser.parsedConfig = null;
                flag = false;
            }
            return flag;
        }

        private static bool ParseXml(XmlElement root)
        {
            bool flag;
            try
            {
                WrsRuleXmlFileParser.parsedConfig = new WRScalingRule();
                XmlNode xmlNodes = root.SelectSingleNode("/root/Algorithms");
                bool flag1 = xmlNodes != null;
                if (!flag1)
                {
                    throw new Exception();
                }
                parsedConfig.SelectAlgorithm = WrsRuleXmlFileParser.GetSelectAlgorithm(xmlNodes);
                IEnumerator enumerator = root.SelectNodes("/root/Algorithms/Algorithm").GetEnumerator();
                try
                {
                    while (true)
                    {
                        flag1 = enumerator.MoveNext();
                        if (!flag1)
                        {
                            break;
                        }
                        XmlNode current = (XmlNode)enumerator.Current;
                        WrsRuleXmlFileParser.parsedConfig.Algorithms.Add(WrsRuleXmlFileParser.GetAlgorithmInfo(current));
                    }
                }
                finally
                {
                    IDisposable disposable = enumerator as IDisposable;
                    flag1 = disposable == null;
                    if (!flag1)
                    {
                        disposable.Dispose();
                    }
                }
                flag = WrsRuleXmlFileParser.parsedConfig.Validate();
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception.ToString());
                WrsRuleXmlFileParser.parsedConfig = null;
                flag = false;
            }
            return flag;
        }

        private class XsdValidator
        {
            private static bool isValid;

            private static StringBuilder errors;

            static XsdValidator()
            {
                WrsRuleXmlFileParser.XsdValidator.isValid = true;
            }

            public XsdValidator()
            {
            }

            public static string GetErrors()
            {
                string str;
                str = (WrsRuleXmlFileParser.XsdValidator.errors != null ? WrsRuleXmlFileParser.XsdValidator.errors.ToString() : "");
                string str1 = str;
                return str1;
            }

            public static bool Validate(Stream input)
            {
                XmlReaderSettings xmlReaderSetting = new XmlReaderSettings();
                xmlReaderSetting.Schemas.Add(null, XmlReader.Create(XsdGenerator.GetWRScalingRuleXsd()));
                xmlReaderSetting.ValidationType = ValidationType.Schema;
                xmlReaderSetting.ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings;
                xmlReaderSetting.ValidationEventHandler += new ValidationEventHandler(WrsRuleXmlFileParser.XsdValidator.ValidationEventHandler);
                xmlReaderSetting.IgnoreWhitespace = true;
                xmlReaderSetting.IgnoreComments = true;
                WrsRuleXmlFileParser.XsdValidator.errors = new StringBuilder();
                XmlReader xmlReader = XmlReader.Create(input, xmlReaderSetting);
                WrsRuleXmlFileParser.XsdValidator.isValid = true;
                try
                {
                    while (true)
                    {
                        bool flag = xmlReader.Read();
                        if (!flag)
                        {
                            break;
                        }
                    }
                }
                catch (Exception exception1)
                {
                    Exception exception = exception1;
                    WrsRuleXmlFileParser.XsdValidator.errors.AppendLine(string.Concat(exception, " - ", exception.Message));
                    WrsRuleXmlFileParser.XsdValidator.isValid = false;
                }
                xmlReader.Close();
                bool flag1 = WrsRuleXmlFileParser.XsdValidator.isValid;
                return flag1;
            }

            public static void ValidationEventHandler(object sender, ValidationEventArgs args)
            {
                WrsRuleXmlFileParser.XsdValidator.errors.AppendLine(string.Concat(args.Exception, " - ", args.Message));
                WrsRuleXmlFileParser.XsdValidator.isValid = false;
            }
        }
    }
}