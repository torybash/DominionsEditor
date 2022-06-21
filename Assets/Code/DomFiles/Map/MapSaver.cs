using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core;
using Core.Entities;
using Data;
using Dom;
using Map.MapElements;
using UnityEngine;

namespace Map
{

	public class MapSaver
	{

		public void SaveMap (Map map, string path)
		{
			Debug.Log("Saving map at: " + path);

			var        mapLines    = new List<string>();
			MapElement lastMapElem = null;
		
			var elems = new List<MapElement>();
		
			//Config
			elems.AddRange(map.unchangedElements);

			//Players
			foreach (var player in DomEdit.I.MapMan.map.players)
			{
				elems.Add(new AllowedPlayer{NationNum = player.Nation.id});
				elems.Add(new StartLocation{NationNum = player.Nation.id, ProvinceNum = player.CapitalProvinceNum});
			}

			//Provinces
			foreach (var province in map.provinceMap.Values)
			{
				bool hasIndieMonsters = province.Monsters.Any();
				if (province.Owner != null || hasIndieMonsters)
				{
					elems.Add(new Land{ProvinceNum = province.ProvinceNumber});

					if (!hasIndieMonsters)
					{
						elems.Add(new ProvinceOwner{NationNum = province.Owner.id});
						if (province.HasLab) elems.Add(new Laboratory());
						if (province.HasTemple) elems.Add(new Temple());
						if (province.HasFort) elems.Add(new Fort{FortId = 1});
					
						if (DomEdit.I.MapMan.map.players.Any(x => x.CapitalProvinceNum == province.ProvinceNumber))
						{
							elems.Add(new KnownMagicSite{ProvinceNum = province.ProvinceNumber, SiteId = 1500});
						}
					}
				}
			}
		
			//Monsters
			foreach (var province in map.provinceMap.Values.Where(x => x.Monsters.Any()))
			{
				elems.Add(new SetLand{ProvinceNum = province.ProvinceNumber});

				var nationalityMonsterGroups = province.Monsters.GroupBy(x => x.Nationality);

				foreach (var group in nationalityMonsterGroups)
				{
					var nation = group.Key;

					// if (nation != Nation.Independents)
					// {
					elems.Add(new ProvinceOwner{NationNum = nation.id});
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
								foreach (var magicOverride in commander.MagicOverrides)
								{
									Magic magic = magicOverride.Path switch
									{
										MagicPath.Fire   => new FireMagic(),
										MagicPath.Air    => new AirMagic(),
										MagicPath.Water  => new WaterMagic(),
										MagicPath.Earth  => new EarthMagic(),
										MagicPath.Astral => new AstralMagic(),
										MagicPath.Death  => new DeathMagic(),
										MagicPath.Nature => new NatureMagic(),
										MagicPath.Blood  => new BloodMagic(),
										MagicPath.Holy   => new HolyMagic(),
										_                => throw new ArgumentOutOfRangeException()
									};
									magic.MagicLevel = magicOverride.MagicValue;
									elems.Add(magic);
								}

								if (commander.Xp > 0)
								{
									elems.Add(new Experience{Amount = commander.Xp});
								}

								break;
							case Troops troops:
								elems.Add(new UnitsElement{MonsterId = troops.MonsterId, Amount = troops.Amount, ProvinceNum = province.ProvinceNumber});
								break;
							default:
								throw new ArgumentOutOfRangeException(nameof(monster));

						}
					}
				}
			
				if (province.Owner != Nation.Independents)
				{
					elems.Add(new ProvinceOwner{NationNum = province.Owner.id});
				}
			}


			foreach (var mapElement in elems)
			{
				var key     = GetMapElementKey(mapElement.GetType());
				var mapLine = $"#{key} {string.Join(" ", mapElement.SaveArgs())}";

				bool isDifferentTypeFromPrevious                                                                   = lastMapElem != null && lastMapElem.GetType() != mapElement.GetType();
				if (mapElement is IOwnedByProvince && lastMapElem is IOwnedByProvince) isDifferentTypeFromPrevious = false;
				if (isDifferentTypeFromPrevious) mapLines.Add("");
			
				mapLines.Add(mapLine);
		
				lastMapElem = mapElement;
			}
		
			File.WriteAllLines(path, mapLines);
		}

		static string GetMapElementKey (Type type)
		{
			var mapKeyName = ((MapKeyNameAttribute[])type.GetCustomAttributes(typeof(MapKeyNameAttribute), false)).Single();
			return mapKeyName.Name;
		}
	}

}