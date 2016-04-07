using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;
using System.IO;
using System.Xml.Schema;

namespace WorkerRoleController.WrcConfigFileParser
{
    public class WrcConfigXmlFileParser
    {
        private static AutoScalingConfiguration parsedConfig;

        public static AutoScalingConfiguration ParsedConfig
        {
            get
            {
                AutoScalingConfiguration configuration = WrcConfigXmlFileParser.parsedConfig;
                return configuration;
            }
        }

        static WrcConfigXmlFileParser()
        {
            WrcConfigXmlFileParser.parsedConfig = null;
        }

        public WrcConfigXmlFileParser()
        {

        }

        private static MonitoringWRPeriod GetMonitoringWRPeriod(XmlNode subNode)
        {
            MonitoringWRPeriod monitoringWRPeriod = new MonitoringWRPeriod();
            monitoringWRPeriod.SecondsPerLoop = int.Parse(WrcConfigXmlFileParser.ParseValue(subNode.Attributes["period"].Value));
            MonitoringWRPeriod monitoringWRPeriod1 = monitoringWRPeriod;
            return monitoringWRPeriod1;
        }

        private static DeploymentInfo GetDeployment(XmlNode deploymenteNode, List<StorageInfo> storages)
        {
            IDisposable disposable;
            bool flag1;
            DeploymentInfo deploymentInfo = new DeploymentInfo();
            deploymentInfo.Name = WrcConfigXmlFileParser.ParseValue(deploymenteNode.Attributes["name"].Value);
            deploymentInfo.StoragePath = WrcConfigXmlFileParser.ParseValue(deploymenteNode.Attributes["storagerelativepath"].Value);
            string str = WrcConfigXmlFileParser.ParseValue(deploymenteNode.Attributes["storage"].Value);
            deploymentInfo.Storage = storages.Where<StorageInfo>((StorageInfo stg) => {
                bool flag = stg.Name.Equals(str);
                return flag;
            }).First<StorageInfo>();
            XmlNodeList xmlNodeLists = deploymenteNode.SelectNodes("roles/role");
            flag1 = (xmlNodeLists == null ? false : xmlNodeLists.Count > 0);
            bool flag2 = flag1;
            if (!flag2)
            {
                throw new Exception();
            }
            IEnumerator enumerator = xmlNodeLists.GetEnumerator();
            try
            {
                while (true)
                {
                    flag2 = enumerator.MoveNext();
                    if (!flag2)
                    {
                        break;
                    }
                    XmlNode current = (XmlNode)enumerator.Current;
                    deploymentInfo.AddNewRole(WrcConfigXmlFileParser.GetRole(current));
                }
            }
            finally
            {
                disposable = enumerator as IDisposable;
                flag2 = disposable == null;
                if (!flag2)
                {
                    disposable.Dispose();
                }
            }
            
            DeploymentInfo deploymentInfo1 = deploymentInfo;
            return deploymentInfo1;
        }

        private static HostedServiceInfo GetHostedService(XmlNode serviceNode, List<StorageInfo> storages)
        {
            bool flag;
            IDisposable disposable;
            HostedServiceInfo hostedServiceInfo = new HostedServiceInfo();
            hostedServiceInfo.Name = WrcConfigXmlFileParser.ParseValue(serviceNode.Attributes["name"].Value);
            hostedServiceInfo.UrlPrefix = WrcConfigXmlFileParser.ParseValue(serviceNode.Attributes["urlprefix"].Value);
            IEnumerator enumerator = serviceNode.SelectNodes("deployments/deployment").GetEnumerator();
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
                    flag = hostedServiceInfo.AddNewDeployment(WrcConfigXmlFileParser.GetDeployment(xmlNodes, storages));
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
            HostedServiceInfo hostedServiceInfo1 = hostedServiceInfo;
            return hostedServiceInfo1;
        }

        private static RoleInfo GetRole(XmlNode roleNode)
        {
            RoleInfo roleInfo = new RoleInfo();
            roleInfo.Name = WrcConfigXmlFileParser.ParseValue(roleNode.Attributes["name"].Value);
            try
            {
                roleInfo.MaxInstances = int.Parse(WrcConfigXmlFileParser.ParseValue(roleNode.Attributes["maxinstances"].Value));
            }
            catch (Exception exception)
            {
                roleInfo.MaxInstances = -1;
            }
            roleInfo.SlotType = WrcConfigXmlFileParser.ParseValue(roleNode.Attributes["deploymentSlotType"].Value);
            RoleInfo roleInfo1 = roleInfo;
            return roleInfo1;
        }
      
        private static StorageInfo GetStorage(XmlNode storageNode)
        {
            StorageInfo storageInfo = new StorageInfo();
            storageInfo.Name = WrcConfigXmlFileParser.ParseValue(storageNode.Attributes["name"].Value);
            storageInfo.Key = WrcConfigXmlFileParser.ParseValue(storageNode.Attributes["key"].Value);
            StorageInfo storageInfo1 = storageInfo;
            return storageInfo1;
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
            subscriptionInfo.Name = WrcConfigXmlFileParser.ParseValue(subNode.Attributes["name"].Value);
            subscriptionInfo.Id = WrcConfigXmlFileParser.ParseValue(subNode.Attributes["id"].Value);
            IEnumerator enumerator = subNode.SelectNodes("storages/storage").GetEnumerator();
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
                    flag = subscriptionInfo.AddNewStorage(WrcConfigXmlFileParser.GetStorage(current));
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
            enumerator = subNode.SelectNodes("services/hostedservice").GetEnumerator();
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
                    flag = subscriptionInfo.AddNewService(WrcConfigXmlFileParser.GetHostedService(xmlNodes, subscriptionInfo.Storages));
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
            string errors = WrcConfigXmlFileParser.XsdValidator.GetErrors();
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
                bool flag1 = !WrcConfigXmlFileParser.XsdValidator.Validate(new MemoryStream(Encoding.UTF8.GetBytes(xml)));
                if (flag1)
                {
                    flag = false;
                }
                else
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(new StringReader(xml));
                    flag = WrcConfigXmlFileParser.ParseXml(xmlDocument.DocumentElement);
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception.ToString());
                WrcConfigXmlFileParser.parsedConfig = null;
                flag = false;
            }
            return flag;
        }

        private static bool ParseXml(XmlElement root)
        {
            bool flag;
            try
            {
                WrcConfigXmlFileParser.parsedConfig = new AutoScalingConfiguration();
                XmlNode xmlNodes = root.SelectSingleNode("/root/MonitoringWRPeriod");
                bool flag1 = xmlNodes != null;
                if (!flag1)
                {
                    throw new Exception();
                }
                WrcConfigXmlFileParser.parsedConfig.WRPeriod = WrcConfigXmlFileParser.GetMonitoringWRPeriod(xmlNodes);
                IEnumerator enumerator = root.SelectNodes("/root/subscriptions/subscription").GetEnumerator();
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
                        WrcConfigXmlFileParser.parsedConfig.Subscriptions.Add(WrcConfigXmlFileParser.GetSubscription(current));
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
                flag = WrcConfigXmlFileParser.parsedConfig.Validate();
                
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception.ToString());
                WrcConfigXmlFileParser.parsedConfig = null;
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
                WrcConfigXmlFileParser.XsdValidator.isValid = true;
            }

            public XsdValidator()
            {
            }

            public static string GetErrors()
            {
                string str;
                str = (WrcConfigXmlFileParser.XsdValidator.errors != null ? WrcConfigXmlFileParser.XsdValidator.errors.ToString() : "");
                string str1 = str;
                return str1;
            }

            public static bool Validate(Stream input)
            {
                XmlReaderSettings xmlReaderSetting = new XmlReaderSettings();
                xmlReaderSetting.Schemas.Add(null, XmlReader.Create(XsdGenerator.GetWorkerRoleControllerConfigXsd()));
                xmlReaderSetting.ValidationType = ValidationType.Schema;
                xmlReaderSetting.ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings;
                xmlReaderSetting.ValidationEventHandler += new ValidationEventHandler(WrcConfigXmlFileParser.XsdValidator.ValidationEventHandler);
                xmlReaderSetting.IgnoreWhitespace = true;
                xmlReaderSetting.IgnoreComments = true;
                WrcConfigXmlFileParser.XsdValidator.errors = new StringBuilder();
                XmlReader xmlReader = XmlReader.Create(input, xmlReaderSetting);
                WrcConfigXmlFileParser.XsdValidator.isValid = true;
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
                    WrcConfigXmlFileParser.XsdValidator.errors.AppendLine(string.Concat(exception, " - ", exception.Message));
                    WrcConfigXmlFileParser.XsdValidator.isValid = false;
                }
                xmlReader.Close();
                bool flag1 = WrcConfigXmlFileParser.XsdValidator.isValid;
                return flag1;
            }

            public static void ValidationEventHandler(object sender, ValidationEventArgs args)
            {
                WrcConfigXmlFileParser.XsdValidator.errors.AppendLine(string.Concat(args.Exception, " - ", args.Message));
                WrcConfigXmlFileParser.XsdValidator.isValid = false;
            }
        }
    }
}
