using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
/// <summary>
/// Equipment 的摘要描述
/// </summary>
//v2. 2011/04/25更改設備內容詳見於database 之table cncmachineinfo

public class Equipment
{
	public Equipment()
	{
		//
		// TODO: 在此加入建構函式的程式碼
		//
	}

    public string equipmentLocation { get; set; }

    public string equipmentIp { get; set; }

    public string equipmenNo { get; set; }

    public string equipmenType { get; set; }

    public string equimentControllerType { get; set; }

    public string IPv4 { get; set; }

    public string IPv4SubnetMask { get; set; }
    
    public byte[] equipmentPicture { get; set; }

    public string equipmenPictureName { get; set; }

    public byte[] imageToByteArray(System.Drawing.Image imageIn)
    {
        MemoryStream ms = new MemoryStream();
        imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
        return ms.ToArray();
    }


    public Image byteArrayToImage(byte[] byteArrayIn)
    {
        MemoryStream ms = new MemoryStream(byteArrayIn);
        Image returnImage = Image.FromStream(ms);
        return returnImage;
    }
}