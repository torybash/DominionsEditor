using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
public class MapSaver
{
	private MapManager _mapManager;
	
	public MapSaver (MapManager mapManager)
	{
		_mapManager = mapManager;
	}
	
	public void SaveMap (string path)
	{
		Debug.Log("path: "+ path);

		var mapLines = new List<string>();
		MapElement lastMapElem = null;
		foreach (var mapElement in _mapManager.MapElements)
		{
			var key = GetMapElementKey(mapElement.GetType());
			var mapLine = $"#{key} {string.Join(" ", mapElement.SaveArgs())}";
			
			if (!(mapElement is IOwnedByProvince) && lastMapElem is IOwnedByProvince) mapLines.Add("");
			else if (!(mapElement is IOwnedByCommander) && lastMapElem is IOwnedByCommander) mapLines.Add("");
			else if (mapElement is Terrain && !(lastMapElem is Terrain)) mapLines.Add("");
			else if (mapElement is Neighbour && !(lastMapElem is Neighbour)) mapLines.Add("");
			else if (mapElement is ProvinceBorders && !(lastMapElem is ProvinceBorders)) mapLines.Add("");
			mapLines.Add(mapLine);

			lastMapElem = mapElement;
		}
		
		File.WriteAllLines(path, mapLines);
	}
	
	private static string GetMapElementKey (Type type)
	{
		var mapKeyName = ((MapKeyName[])type.GetCustomAttributes(typeof(MapKeyName), false)).Single();
		return mapKeyName.Name;
	}
}