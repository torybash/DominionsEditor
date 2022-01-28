using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Province
{
	public Province (int provinceNum, Vector2 centerPos)
	{
		ProvinceNumber = provinceNum;
		CenterPos = centerPos;
	}
	public Vector2 CenterPos { get; set; }
	public int ProvinceNumber { get; set; }
	public Nation Owner { get; set; }
	public List<Monster> Monsters { get; set; } = new List<Monster>();
	public bool HasLab { get; set; }
	public bool HasTemple { get; set; }
	public bool HasFort { get; set; }
	// public bool HasThrone { get; set; }
}