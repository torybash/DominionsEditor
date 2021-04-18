using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
public class MapSaver
{
	private MapManager _map;
	
	public MapSaver (MapManager map)
	{
		_map = map;
	}
	
	public void SaveMap (string path)
	{
		Debug.Log("Saving map at: "+ path);

		var mapLines = new List<string>();
		MapElement lastMapElem = null;
		
		// throw new Exception("TODO Reimplement!");

		var elems = new List<MapElement>();
		
		//Config
		elems.AddRange(_map.Config.MapElements);

		//Players
		foreach (var player in _map.Players)
		{
			elems.Add(new AllowedPlayer{NationNum = player.NationNum});
			elems.Add(new StartLocation{NationNum = player.NationNum, ProvinceNum = player.CapitalProvinceNum});
		}

		//Provinces
		foreach (var province in _map.ProvinceMap.Values)
		{
			bool isIndieProvince = province.Owner == Nation.Independents;
			bool hasIndieMonsters = isIndieProvince && province.Monsters.Any();
			if (!isIndieProvince || hasIndieMonsters)
			{
				elems.Add(new Land{ProvinceNum = province.ProvinceNumber});

				if (!isIndieProvince)
				{
					elems.Add(new ProvinceOwner{NationNum = province.Owner.Number});
				}
			}
		}
		
		//Monsters
		foreach (var province in _map.ProvinceMap.Values.Where(x => x.Monsters.Any()))
		{
			elems.Add(new SetLand{ProvinceNum = province.ProvinceNumber});

			var nationalityMonsterGroups = province.Monsters.GroupBy(x => x.Nationality);

			foreach (var group in nationalityMonsterGroups)
			{
				var nation = group.Key;

				// if (nation != Nation.Independents)
				// {
					elems.Add(new ProvinceOwner{NationNum = nation.Number});
				// }

				foreach (var monster in group)
				{
					switch (monster)
					{
						case Commander commander:
							elems.Add(new CommanderElement{MonsterId = commander.MonsterId, ProvinceNum = province.ProvinceNumber});

							foreach (var unit in commander.UnitsUnderCommand)
							{
								elems.Add(new UnitsElement{MonsterId = unit.MonsterId, Amount = unit.Amount, ProvinceNum = province.ProvinceNumber});
							}
						
							break;
						case Unit unit:
							elems.Add(new UnitsElement{MonsterId = unit.MonsterId, Amount = unit.Amount, ProvinceNum = province.ProvinceNumber});
							break;
						default:
							throw new ArgumentOutOfRangeException(nameof(monster));

					}
				}
			}
			
			if (province.Owner != Nation.Independents)
			{
				elems.Add(new ProvinceOwner{NationNum = province.Owner.Number});
			}
		}


		foreach (var mapElement in elems)
		{
			var key = GetMapElementKey(mapElement.GetType());
			var mapLine = $"#{key} {string.Join(" ", mapElement.SaveArgs())}";

			bool isDifferentTypeFromPrevious = lastMapElem != null && lastMapElem.GetType() != mapElement.GetType();
			if (mapElement is IOwnedByProvince && lastMapElem is IOwnedByProvince) isDifferentTypeFromPrevious = false;
			if (isDifferentTypeFromPrevious) mapLines.Add("");
			
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