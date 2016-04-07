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
using System.Xml.Linq;
using System.Linq;
using OMC.Comm;

namespace OMC.Comm
{
    public static class ImagesView
    {
        public static List<ImageEntity> GetAllImagesData(string xmlStr, string scenarioIndex)
        {
            try
            {
                ///"<sim_feature>
                ///  <graphNum>2</graphNum>
                ///  <overcutting>true</overcutting>   [是否過切]
                ///  <graph line=""50"" time=""1000"">gif1.jpg</graph>
                ///  <graph line=""150"" time=""2000"">gif2.jpg</graph>
                ///</sim_feature>"

                System.IO.MemoryStream _memorystreamImage = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xmlStr));
                XDocument ImageXml = XDocument.Load(_memorystreamImage);

                if (scenarioIndex.Equals("1"))
                {
                    List<ImageEntity> images=new List<ImageEntity>();
                    foreach(var ImageInfo in ImageXml.Elements("sim_feature").Descendants("graph"))
                    {
                        ImageEntity temp=new ImageEntity();
                        temp.ImageNum = ImageInfo.Parent.Value;
                        temp.ImageName ="http://boyili.blob.core.windows.net/image/" + ImageInfo.Value;   //"../Images/Example/" + 
                                         //ImageCNCName = ImageInfo.Attribute("vmtType").Value,
                        temp.isOvercutting = "";
                        temp.ImageLine = "NC碼行數: " + ImageInfo.Attribute("line").Value;
                        temp.ImageTime = "時間點: " + ImageInfo.Attribute("time").Value;
                        images.Add(temp);                     
                    }
                    return images;
                }
                else if (scenarioIndex.Equals("2"))
                {
                    var ImageQuery = from ImageInfo in ImageXml.Elements("sim_feature").Descendants("graph")
                                     select new ImageEntity()
                                     {
                                         isOvercutting = ImageInfo.Parent.Element("overcutting").Value,
                                         ImageNum = ImageInfo.Parent.Value,
                                         ImageName = "http://boyili.blob.core.windows.net/image/" + ImageInfo.Value,   //"../Images/Example/" + 
                                         ImageLine = "NC碼行數: " + ImageInfo.Attribute("line").Value,
                                         ImageTime = "時間點: " + ImageInfo.Attribute("time").Value

                                     };
                    return ImageQuery.ToList<ImageEntity>();
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


            ///原先範例是呼叫XML檔解析
            /*
            try
            {
                // Load Xml Document
                XDocument XDoc = XDocument.Load("ImageData.xml");

                // Query for retriving all Images data from XML
                var Query = from Q in XDoc.Descendants("Image")
                            select new ImageEntity
                            {
                                ImageName = Q.Element("ImageName").Value,
                                ImagePath = Q.Element("ImagePath").Value
                            };

                // return images data
                return Query.ToList<ImageEntity>();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
          * */
        }
    }
}