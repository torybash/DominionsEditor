using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class MapLoader
{
	public static void Load (string mapPath, out Map map, out List<GamePlayer> players)
	{
		var allElements = LoadMapElements(mapPath);
		map         = CreateConfig(allElements);
		map.ProvinceMap = CreateMap(allElements);
		
		players = CreatePlayers(allElements);
	}
	
	public static List<MapElement> LoadMapElements (string mapPath)
	{
		var mapLineTexts = File.ReadAllLines(mapPath);

		var mapLine = ParseMapLines(mapLineTexts);
		var mapElements = ParseMapElements(mapLine);
		
		return mapElements;
	}

	private static List<MapLine> ParseMapLines (string[] mapLineTexts)
	{
		var mapLineDatas = new List<MapLine>();
		for (int i = 0; i < mapLineTexts.Length; i++)
		{
			var mapLine = mapLineTexts[i];
			if (mapLine.Length == 0) continue;
			if (mapLine[0] != '#') continue;

			int spaceIndex = mapLine.IndexOf(' ');
			int keyEndIndex = spaceIndex == -1 ? mapLine.Length : spaceIndex;

			var key = mapLine.Substring(1, keyEndIndex - 1);

			var mapData = new MapLine(key);
			if (spaceIndex != -1)
			{
				var args = mapLine.Substring(keyEndIndex + 1, mapLine.Length - keyEndIndex - 1);

				var separatorChar = ' ';
				Regex regx = new Regex(separatorChar + "(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))"); 
				string[] argsSplit = regx.Split(args);

				foreach (var arg in argsSplit)
				{
					if (arg.Contains("--")) break;
					mapData.AddArg(arg);
				}
			}

			mapLineDatas.Add(mapData);
		}
		return mapLineDatas;
	}
	
	private static List<MapElement> ParseMapElements (List<MapLine> mapLineDatas)
	{
		var mapElements = new List<MapElement>();;

		Land currentLand = null;
		CommanderElement currentCommander = null;
		for (var i = 0; i < mapLineDatas.Count; i++)
		{
			var mapLineData = mapLineDatas[i];
			var mapElemType = GetMapElementType(mapLineData.Key);
			if (mapElemType == null) continue;

			var mapElem = (MapElement)Activator.CreateInstance(mapElemType);
			mapElem.ParseArgs(mapLineData.Args.ToArray());
			mapElements.Add(mapElem);

			if (mapElem is Land land) currentLand = land;
			if (mapElem is CommanderElement commander) currentCommander = commander;
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

			var mapKeyName = ((MapKeyNameAttribute[])type.GetCustomAttributes(typeof(MapKeyNameAttribute), false)).Single();

			if (mapKeyName.Name == nameKey) return type;
		}
		return null;
	}

	
	public static Dictionary<int, Province> CreateMap (List<MapElement> mapElements)
	{
		//Create all provinces
		var provinces = new Dictionary<int, Province>();
		int provinceCount = mapElements.OfType<Terrain>().Max(x => x.ProvinceNum);
		for (int num = 1; num <= provinceCount; num++)
		{
			
			var pbs = mapElements.OfType<ProvinceBorders>().Where(x => x.ProvinceNum == num).ToList();
			if (pbs.Count == 0) continue;

			var centerPos = Vector2.zero;
			foreach (var pb in pbs)
			{
				centerPos.x += pb.X;
				centerPos.y += pb.Y;
			}
			centerPos /= pbs.Count();
			
			var province = new Province(num, centerPos);
			provinces.Add(num, province);
		}

		//Set owner of provinces
		foreach (var provinceOwner in mapElements.OfType<ProvinceOwner>())
		{
			if (provinces.ContainsKey(provinceOwner.ProvinceNum))
			{
				provinces[provinceOwner.ProvinceNum].Owner = provinceOwner.NationNum;
			}
		}
		
		foreach (var startLocation in mapElements.OfType<StartLocation>())
		{
			if (provinces.ContainsKey(startLocation.ProvinceNum))
			{
				provinces[startLocation.ProvinceNum].Owner = startLocation.NationNum;
			}
		}
		
		//Create commanders
		foreach (var commanderElement in mapElements.OfType<CommanderElement>())
		{
			var province = provinces[commanderElement.ProvinceNum];
			var commander = Commander.Create(commanderElement.MonsterId, province.Owner);
			commander.Nationality = province.Owner;
			province.Monsters.Add(commander);
			
			//Create owned-be-commander objects
			foreach (var ownedByCommander in mapElements.OfType<IOwnedByCommander>().Where(x => x.Commander == commanderElement))
			{
				switch (ownedByCommander)
				{
					case BodyguardsElement bodyguardsElement:
						var bodyguard = Unit.Create(bodyguardsElement.MonsterId, bodyguardsElement.Amount, commander.Nationality);
						commander.Bodyguards.Add(bodyguard);
						break;
					case ItemElement itemElement:
						var item = Item.Create(itemElement.ItemName);
						commander.Items.Add(item);
						break;
					case UnitsElement unitsElement:
						var unit = Unit.Create(unitsElement.MonsterId, unitsElement.Amount, commander.Nationality);
						commander.UnitsUnderCommand.Add(unit);
						break;
					case Experience experience:
						//TODO
						break;
					case Magic magic:
						//TODO
						break;
					case ClearMagic clearMagic:
						//TODO
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(ownedByCommander));

				}
			}
		}
		
		return provinces;
	}
	
	public static List<GamePlayer> CreatePlayers (List<MapElement> mapElements)
	{
		var players = new List<GamePlayer>();

		foreach (var allowedPlayer in mapElements.OfType<AllowedPlayer>())
		{
			var startLocation = mapElements.OfType<StartLocation>().SingleOrDefault(x => x.NationNum == allowedPlayer.NationNum);
			var capitalProvinceNum = startLocation?.ProvinceNum ?? -1;
			var player = new GamePlayer(PlayerType.Human, allowedPlayer.NationNum){CapitalProvinceNum = capitalProvinceNum};
			players.Add(player);
		}
		return players;
	}
	
	public static Map CreateConfig (List<MapElement> mapElements)
	{
		var map = new Map();
		foreach (var mapElement in mapElements)
		{
			if (mapElement is ProvinceDataElement) continue;
			if (mapElement is AllowedPlayer) continue;
			if (mapElement is StartLocation) continue;
			if (mapElement is IOwnedByCommander) continue;
			if (mapElement is Land) continue;
			
			map.Elements.Add(mapElement);
		}
		
		map.Elements = map.Elements.OrderBy(x => x.GetType().Name).ToList();

		return map;
	}
}