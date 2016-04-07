using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Vmachine 的摘要描述
/// </summary>
public class Vmachine
{
	public Vmachine()
	{
		//
		// TODO: 在此加入建構函式的程式碼
		//
	}

    public string vMachineId { get; set; }

    public string vMachineIp { get; set; }

    public List<Equipment> equipmentList { get; set; }

}