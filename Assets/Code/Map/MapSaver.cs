using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class MapSaver
{

	public void SaveMap (Map map, string path)
	{
		Debug.Log("Saving map at: "+ path);

		var mapLines = new List<string>();
		MapElement lastMapElem = null;
		
		var elems = new List<MapElement>();
		
		//Config
		elems.AddRange(map.Elements);

		//Players
		foreach (var player in DomEdit.I.PlayerSetup.Players)
		{
			elems.Add(new AllowedPlayer{NationNum = player.Nation.Number});
			elems.Add(new StartLocation{NationNum = player.Nation.Number, ProvinceNum = player.CapitalProvinceNum});
		}

		//Provinces
		foreach (var province in map.ProvinceMap.Values)
		{
			bool isIndieProvince = province.Owner == Nation.Independents;
			bool hasIndieMonsters = isIndieProvince && province.Monsters.Any();
			if (!isIndieProvince || hasIndieMonsters)
			{
				elems.Add(new Land{ProvinceNum = province.ProvinceNumber});

				if (!isIndieProvince)
				{
					elems.Add(new ProvinceOwner{NationNum = province.Owner.Number});
					if (province.HasLab) elems.Add(new Laboratory());
					if (province.HasTemple) elems.Add(new Temple());
					if (province.HasFort) elems.Add(new Fort{FortId = 1});
					
					if (DomEdit.I.PlayerSetup.Players.Any(x => x.CapitalProvinceNum == province.ProvinceNumber))
					{
						elems.Add(new KnownMagicSite{ProvinceNum = province.ProvinceNumber, SiteId = 1500});
					}
				}
			}
		}
		
		//Monsters
		foreach (var province in map.ProvinceMap.Values.Where(x => x.Monsters.Any()))
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
							
							foreach (var item in commander.Items)
							{
								elems.Add(new ItemElement{ItemName = item.ItemName});
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
		var mapKeyName = ((MapKeyNameAttribute[])type.GetCustomAttributes(typeof(MapKeyNameAttribute), false)).Single();
		return mapKeyName.Name;
	}
}