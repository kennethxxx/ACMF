using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;
using System.IO;
using System.Xml.Schema;
using WorkerRoleController.PrivateWRConfigFileParser;

namespace WorkerRoleController.PrivateWRConfigFileParser
{
    public class PrivateWrcConfigXmlFileParser
    {
        private static PrivateAutoScalingConfiguration parsedConfig;

        public static PrivateAutoScalingConfiguration ParsedConfig
        {
            get
            {
                PrivateAutoScalingConfiguration configuration = PrivateWrcConfigXmlFileParser.parsedConfig;
                return configuration;
            }
        }

        static PrivateWrcConfigXmlFileParser()
        {
            PrivateWrcConfigXmlFileParser.parsedConfig = null;
        }

        public PrivateWrcConfigXmlFileParser()
        {

        }

        private static MonitoringWRPeriod GetMonitoringWRPeriod(XmlNode subNode)
        {
            MonitoringWRPeriod monitoringWRPeriod = new MonitoringWRPeriod();
            monitoringWRPeriod.SecondsPerLoop = int.Parse(PrivateWrcConfigXmlFileParser.ParseValue(subNode.Attributes["period"].Value));
            MonitoringWRPeriod monitoringWRPeriod1 = monitoringWRPeriod;
            return monitoringWRPeriod1;
        }

        private static VcenterInfo GetVcenter(XmlNode serviceNode)
        {
            bool flag;
            IDisposable disposable;
            VcenterInfo VcenterInfos = new VcenterInfo();
            VcenterInfos.vIP = PrivateWrcConfigXmlFileParser.ParseValue(serviceNode.Attributes["VIP"].Value);
            VcenterInfos.vaccount = PrivateWrcConfigXmlFileParser.ParseValue(serviceNode.Attributes["Vaccount"].Value);
            VcenterInfos.vpassword = PrivateWrcConfigXmlFileParser.ParseValue(serviceNode.Attributes["Vpassword"].Value);
            IEnumerator enumerator = serviceNode.SelectNodes("roles/role").GetEnumerator();
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
                    flag = VcenterInfos.AddNewDeployment(PrivateWrcConfigXmlFileParser.GetRole(xmlNodes));
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
            VcenterInfo VcenterInfos1 = VcenterInfos;
            return VcenterInfos1;
        }

        private static RoleInfo GetRole(XmlNode roleNode)
        {
            RoleInfo roleInfo = new RoleInfo();
            try
            {
                roleInfo.MaxInstances = int.Parse(PrivateWrcConfigXmlFileParser.ParseValue(roleNode.Attributes["maxinstances"].Value));
            }
            catch (Exception exception)
            {
                roleInfo.MaxInstances = -1;
            }
            roleInfo.SlotType = PrivateWrcConfigXmlFileParser.ParseValue(roleNode.Attributes["deploymentSlotType"].Value);
            RoleInfo roleInfo1 = roleInfo;
            return roleInfo1;
        }

        private static MySQLInfo GetDatabase(XmlNode databaseNode)
        {
            MySQLInfo databaseInfo = new MySQLInfo();
            databaseInfo.myIP = PrivateWrcConfigXmlFileParser.ParseValue(databaseNode.Attributes["MyIP"].Value);
            databaseInfo.myUser = PrivateWrcConfigXmlFileParser.ParseValue(databaseNode.Attributes["MyUser"].Value);
            databaseInfo.myPassword = PrivateWrcConfigXmlFileParser.ParseValue(databaseNode.Attributes["MyPassword"].Value);
            databaseInfo.myPort = PrivateWrcConfigXmlFileParser.ParseValue(databaseNode.Attributes["MyPort"].Value);
            MySQLInfo databaseInfo1 = databaseInfo;
            return databaseInfo1;
        }

        private static SubscriptionInfo GetSubscription(XmlNode subNode)
        {
            IDisposable disposable;
            bool flag = subNode != null;
            if (!flag)
            {
                throw new Exception();
            }
            SubscriptionInfo subscriptionInfo = new SubscriptionInfo();
            IEnumerator enumerator = subNode.SelectNodes("databae/MySQL").GetEnumerator();
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
                    flag = subscriptionInfo.AddNewDatabase(PrivateWrcConfigXmlFileParser.GetDatabase(current));
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
            enumerator = subNode.SelectNodes("services/Vcenter").GetEnumerator();
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
                    flag = subscriptionInfo.AddNewService(PrivateWrcConfigXmlFileParser.GetVcenter(xmlNodes));
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
            SubscriptionInfo subscriptionInfo1 = subscriptionInfo;
            return subscriptionInfo1;
        }
     
        public static string GetXmlErrors()
        {
            string errors = PrivateWrcConfigXmlFileParser.XsdValidator.GetErrors();
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
                bool flag1 = !PrivateWrcConfigXmlFileParser.XsdValidator.Validate(new MemoryStream(Encoding.UTF8.GetBytes(xml)));
                if (flag1)
                {
                    flag = false;
                }
                else
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(new StringReader(xml));
                    flag = PrivateWrcConfigXmlFileParser.ParseXml(xmlDocument.DocumentElement);
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception.ToString());
                PrivateWrcConfigXmlFileParser.parsedConfig = null;
                flag = false;
            }
            return flag;
        }

        private static bool ParseXml(XmlElement root)
        {
            bool flag;
            try
            {
                PrivateWrcConfigXmlFileParser.parsedConfig = new PrivateAutoScalingConfiguration();
                XmlNode xmlNodes = root.SelectSingleNode("/root/MonitoringWRPeriod");
                bool flag1 = xmlNodes != null;
                if (!flag1)
                {
                    throw new Exception();
                }
                PrivateWrcConfigXmlFileParser.parsedConfig.WRPeriod = PrivateWrcConfigXmlFileParser.GetMonitoringWRPeriod(xmlNodes);
                IEnumerator enumerator = root.SelectNodes("/root/subscriptions").GetEnumerator();
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
                        PrivateWrcConfigXmlFileParser.parsedConfig.Subscriptions.Add(PrivateWrcConfigXmlFileParser.GetSubscription(current));
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
                flag = PrivateWrcConfigXmlFileParser.parsedConfig.Validate();
                
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception.ToString());
                PrivateWrcConfigXmlFileParser.parsedConfig = null;
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
                PrivateWrcConfigXmlFileParser.XsdValidator.isValid = true;
            }

            public XsdValidator()
            {
            }

            public static string GetErrors()
            {
                string str;
                str = (PrivateWrcConfigXmlFileParser.XsdValidator.errors != null ? PrivateWrcConfigXmlFileParser.XsdValidator.errors.ToString() : "");
                string str1 = str;
                return str1;
            }

            public static bool Validate(Stream input)
            {
                XmlReaderSettings xmlReaderSetting = new XmlReaderSettings();
                xmlReaderSetting.Schemas.Add(null, XmlReader.Create(XsdGenerator.GetPrivateWorkerRoleControllerConfigXsd()));
                xmlReaderSetting.ValidationType = ValidationType.Schema;
                xmlReaderSetting.ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings;
                xmlReaderSetting.ValidationEventHandler += new ValidationEventHandler(PrivateWrcConfigXmlFileParser.XsdValidator.ValidationEventHandler);
                xmlReaderSetting.IgnoreWhitespace = true;
                xmlReaderSetting.IgnoreComments = true;
                PrivateWrcConfigXmlFileParser.XsdValidator.errors = new StringBuilder();
                XmlReader xmlReader = XmlReader.Create(input, xmlReaderSetting);
                PrivateWrcConfigXmlFileParser.XsdValidator.isValid = true;
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
                    PrivateWrcConfigXmlFileParser.XsdValidator.errors.AppendLine(string.Concat(exception, " - ", exception.Message));
                    PrivateWrcConfigXmlFileParser.XsdValidator.isValid = false;
                }
                xmlReader.Close();
                bool flag1 = PrivateWrcConfigXmlFileParser.XsdValidator.isValid;
                return flag1;
            }

            public static void ValidationEventHandler(object sender, ValidationEventArgs args)
            {
                PrivateWrcConfigXmlFileParser.XsdValidator.errors.AppendLine(string.Concat(args.Exception, " - ", args.Message));
                PrivateWrcConfigXmlFileParser.XsdValidator.isValid = false;
            }
        }
    }
}
