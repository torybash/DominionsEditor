using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
public class MapLoader
{
	private MapManager _mapManager;
	public MapLoader (MapManager mapManager)
	{
		_mapManager = mapManager;
	}

	public List<MapElement> LoadMap (string mapPath)
	{
		var mapLineTexts = File.ReadAllLines(mapPath);

		var mapLineDatas = CreateLineDatas(mapLineTexts);
		var mapElements = CreateMapElements(mapLineDatas);
		return mapElements;
	}
	
	private static List<MapLineData> CreateLineDatas (string[] mapLineTexts)
	{
		var mapLineDatas = new List<MapLineData>();
		for (int i = 0; i < mapLineTexts.Length; i++)
		{
			var mapLine = mapLineTexts[i];
			if (mapLine.Length == 0) continue;
			if (mapLine[0] != '#') continue;

			int spaceIndex = mapLine.IndexOf(' ');
			int keyEndIndex = spaceIndex == -1 ? mapLine.Length : spaceIndex;

			var key = mapLine.Substring(1, keyEndIndex - 1);

			// Debug.Log($"key: {key}");
			var mapData = new MapLineData(key);

			if (spaceIndex != -1)
			{
				var args = mapLine.Substring(keyEndIndex + 1, mapLine.Length - keyEndIndex - 1);

				//var regx = new Regex(' ' + "(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))"); 
				//foreach (var arg in regx.Split(args))

				foreach (var arg in args.Split(' '))
				{
					if (arg.Contains("--")) break;
					mapData.AddArg(arg);
				}
			}

			mapLineDatas.Add(mapData);
		}
		return mapLineDatas;
	}
	
	private List<MapElement> CreateMapElements (List<MapLineData> mapLineDatas)
	{
		var mapElements = new List<MapElement>();;

		Land currentLand = null;
		Commander currentCommander = null;
		for (var i = 0; i < mapLineDatas.Count; i++)
		{
			var mapLineData = mapLineDatas[i];
			var mapElemType = GetMapElementType(mapLineData.Key);
			if (mapElemType == null) continue;

			var mapElem = (MapElement)Activator.CreateInstance(mapElemType);
			mapElem.ParseArgs(mapLineData.Args.ToArray());
			mapElements.Add(mapElem);

			if (mapElem is Land land) currentLand = land;
			if (mapElem is Commander commander) currentCommander = commander;
			if (mapElem is IOwnedByProvince landDataElement)
			{
				Debug.Assert(currentLand != null, "Found ProvinceDataElement, but currentLand not set!");
				landDataElement.ProvinceNum = currentLand.ProvinceNum;
			}
			if (mapElem is IOwnedByCommander ownedByCommander)
			{
				Debug.Assert(currentLand != null, "Found ProvinceDataElement, but currentCommander not set!");
				ownedByCommander.Commander = currentCommander;
			}
		}

		return mapElements;
	}
	
	private static Type GetMapElementType (string nameKey)
	{

		var ass = typeof(MapElement).Assembly;
		foreach (Type type in ass.GetTypes())
		{
			if (!typeof(MapElement).IsAssignableFrom(type)) continue;
			if (typeof(MapElement) == type) continue;
			if (type.IsAbstract) continue;

			var mapKeyName = ((MapKeyName[])type.GetCustomAttributes(typeof(MapKeyName), false)).Single();

			if (mapKeyName.Name == nameKey) return type;
		}
		return null;
	}

}