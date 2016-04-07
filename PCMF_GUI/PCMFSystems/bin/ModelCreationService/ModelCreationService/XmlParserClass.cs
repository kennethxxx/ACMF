using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;

namespace XmlParserClassNS
{
    public class XmlParserClass
    {
        private XmlDocument TempleXML = null;

        public XmlParserClass() 
        {
            TempleXML = new XmlDocument();
        }

        public void InitialXML(String strUserName)
        {
            XmlNode xmlnode;
            XmlElement xmlelem;

            TempleXML = new XmlDocument();
            xmlnode = TempleXML.CreateNode(XmlNodeType.XmlDeclaration, "", "");
            TempleXML.AppendChild(xmlnode);

            //let's add the root element
            xmlelem = TempleXML.CreateElement("", "ParameterList", "");
            xmlelem.SetAttribute("user", strUserName);
            xmlelem.SetAttribute("time", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

            TempleXML.AppendChild(xmlelem);

            this.InitialXML(strUserName, string.Empty);
        }

        public void InitialXML(String strUserName, String strPath)
        {
            this.InitialXML(strUserName);
            this.Save(strPath);
        }

        public Int32[,] iXMLToParameter(String OutputName)
        {
            XmlNode node = TempleXML.DocumentElement.SelectSingleNode("ParameterList/Parameters[@name='" + OutputName + "']");//選擇節點

            if (node == null) return null;

            XmlElement xeOut = (XmlElement)node;

            int iColumn = Int32.Parse(xeOut.GetAttribute("columns"));
            int iRow = Int32.Parse(xeOut.GetAttribute("rows"));

            Int32[,] iArray = new Int32[iColumn, iRow];

            XmlNodeList myNodeList = TempleXML.DocumentElement.SelectNodes("ParameterList/Parameters[@name='" + OutputName + "']/parameter");

            int iRowCount = 0;
            foreach (XmlNode xn in myNodeList)
            {
                XmlElement xe = (XmlElement)xn;

                String strInner = xe.InnerXml;

                String[] s = Regex.Split(strInner, ";");

                for (int i = 0; i < s.Length - 1; i++)
                {
                    iArray[i, iRowCount] = Int32.Parse(s[i]);
                }
                iRowCount++;
            }
            return iArray;
        }

        public Int32[,] iXMLToParameter(String OutputName, String strPath)
        {
            this.Load(strPath);
            return this.iXMLToParameter(OutputName);
        }

        public double[,] dXMLToParameter(String OutputName)
        {
            XmlNode node = TempleXML.DocumentElement.SelectSingleNode("ParameterList/Parameters[@name='" + OutputName + "']");//選擇節點

            if (node == null) return null;

            XmlElement xeOut = (XmlElement)node;

            int iColumn = Int32.Parse(xeOut.GetAttribute("columns"));
            int iRow = Int32.Parse(xeOut.GetAttribute("rows"));

            double[,] dArray = new double[iColumn, iRow];

            XmlNodeList myNodeList = TempleXML.DocumentElement.SelectNodes("ParameterList/Parameters[@name='" + OutputName + "']/parameter");

            int iRowCount = 0;
            foreach (XmlNode xn in myNodeList)
            {
                XmlElement xe = (XmlElement)xn;

                String strInner = xe.InnerXml;

                String[] s = Regex.Split(strInner, ";");

                for (int i = 0; i < s.Length - 1; i++)
                {
                    dArray[i, iRowCount] = Double.Parse(s[i]);
                }
                iRowCount++;
            }
            return dArray;
        }
        
        public double[,] dXMLToParameter(String OutputName, String strPath)
        {
            this.Load(strPath);
            return this.dXMLToParameter(OutputName);
        }

        public void iParameterToXML(Int32[,] Input, String InputName, String strPath)
        {
            this.Load(strPath);
            this.iParameterToXML(Input, InputName);
            this.Save(strPath);
            return;
        }

        public void iParameterToXML(Int32[,] Input, String InputName)
        {
            XmlElement xmlelem;
            xmlelem = TempleXML.CreateElement("", "Parameters", "");
            xmlelem.SetAttribute("name", InputName);
            xmlelem.SetAttribute("type", "double");
            xmlelem.SetAttribute("rows", Input.GetLength(0).ToString());
            xmlelem.SetAttribute("columns", Input.GetLength(1).ToString());
            TempleXML.ChildNodes.Item(1).AppendChild(xmlelem);

            XmlNode node = TempleXML.SelectSingleNode("ParameterList/Parameters[@name='" + InputName + "']");//選擇節點
            if (node == null) return;

            for (int i = 0; i < Input.GetLength(0); i++)
            {
                XmlElement xe = TempleXML.CreateElement("parameter");
                xe.SetAttribute("row", (i + 1).ToString());  //設定屬性

                String strInnerContent = null;

                for (int j = 0; j < Input.GetLength(1); j++)
                {
                    strInnerContent += Input[i, j].ToString() + ";";
                }
                xe.InnerText = strInnerContent;
                node.AppendChild(xe);
            }
            return;
        }

        public void dParameterToXML(Double[,] Input, String InputName, String strPath)
        {
            this.Load(strPath);
            this.dParameterToXML(Input, InputName);
            this.Save(strPath);
            return;
        }

        public void dParameterToXML(Double[,] Input, String InputName)
        {
            XmlElement xmlelem;
            xmlelem = TempleXML.CreateElement("", "Parameters", "");
            xmlelem.SetAttribute("name", InputName);
            xmlelem.SetAttribute("type", "double");
            xmlelem.SetAttribute("rows", Input.GetLength(0).ToString());
            xmlelem.SetAttribute("columns", Input.GetLength(1).ToString());
            TempleXML.ChildNodes.Item(1).AppendChild(xmlelem);

            XmlNode node = TempleXML.SelectSingleNode("ParameterList/Parameters[@name='" + InputName + "']");//選擇節點
            if (node == null) return;

            for (int i = 0; i < Input.GetLength(0); i++)
            {
                XmlElement xe = TempleXML.CreateElement("parameter");
                xe.SetAttribute("row", (i + 1).ToString());  //設定屬性

                String strInnerContent = null;

                for (int j = 0; j < Input.GetLength(1); j++)
                {
                    strInnerContent += Input[i, j].ToString() + ";";
                }
                xe.InnerText = strInnerContent;
                node.AppendChild(xe);
            }
            return;
        }

        public void Save(String strPath) 
        {
            if (strPath.CompareTo(string.Empty) != 0)
            {
                TempleXML.Save(strPath);
            }
        }

        public void Load(String strPath)
        {
            if (strPath.CompareTo(string.Empty) != 0)
            {
                TempleXML.Load(strPath);
            }
        }

        
    }
}
