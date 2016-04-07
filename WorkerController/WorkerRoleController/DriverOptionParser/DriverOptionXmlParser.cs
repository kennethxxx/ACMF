using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;

namespace WorkerRoleController.DriverOptionParser
{
    class DriverOptionXmlParser
    {
        private string xmlDriverOptionSource;

        private string DriverOption;

        public string GetDriverOption()
        {            
            try
            {
                xmlDriverOptionSource = Utils.GetConfigurations(3);
                System.IO.MemoryStream memoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(xmlDriverOptionSource));
                XDocument DriverXml = XDocument.Load(memoryStream);
                foreach(var DriverInfo in DriverXml.Elements("root").Descendants("Driver"))
                {
                    DriverOption = DriverInfo.Attribute("Option").Value;
                }
           }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.Message);
            }
            return DriverOption;
        }

    }
}
