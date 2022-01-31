using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Core;
using Core.Entities;
using Dom;
using Map.MapElements;
using Tools;
using UI.Menus;
using UnityEngine;
using Terrain = Map.MapElements.Terrain;

namespace Map
{

	public class MapLoader
	{
		public static Map Load (MapFile mapFile)
		{
			var allElements = LoadMapElements(mapFile.path);

			var map = CreateMap(allElements);
			map.MapFile = mapFile;

			map.MapTexture  = LoadMapTexture(map);
			map.ProvinceMap = LoadProvinces(map);
			map.Players     = LoadPlayers(map);

			return map;
		}

		private static Texture2D LoadMapTexture (Map map)
		{
			var    imageFile    = map.UnchangedElements.OfType<ImageFile>().First();
			string mapImagePath = $"{map.MapFile.folder}\\{imageFile.MapImageName}";
			return MapImageLoader.LoadMapTexture(mapImagePath);
		}

		public static List<MapElement> LoadMapElements (string mapPath)
		{
			var texts      = File.ReadAllLines(mapPath);
			var properties = ParseProperties(texts);
			var elements   = ParseMapElements(properties);

			return elements;
		}

		private static List<MapProperty> ParseProperties (string[] mapLineTexts)
		{
			var mapProperties = new List<MapProperty>();
			for (int i = 0; i < mapLineTexts.Length; i++)
			{
				var mapLine = mapLineTexts[i];
				if (mapLine.Length == 0) continue;
				if (mapLine[0]     != '#') continue;

				int spaceIndex  = mapLine.IndexOf(' ');
				int keyEndIndex = spaceIndex == -1 ? mapLine.Length : spaceIndex;

				var key = mapLine.Substring(1, keyEndIndex - 1);

				var mapProperty = new MapProperty(key);
				if (spaceIndex != -1)
				{
					var args = mapLine.Substring(keyEndIndex + 1, mapLine.Length - keyEndIndex - 1);

					var      separatorChar = ' ';
					Regex    regx          = new Regex(separatorChar + "(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
					string[] argsSplit     = regx.Split(args);

					foreach (var arg in argsSplit)
					{
						if (arg.Contains("--")) break;
						mapProperty.AddArg(arg);
					}
				}

				mapProperties.Add(mapProperty);
			}
			return mapProperties;
		}

		private static List<MapElement> ParseMapElements (List<MapProperty> mapLineDatas)
		{
			var mapElements = new List<MapElement>();

			Land             currentLand      = null;
			CommanderElement currentCommander = null;
			for (var i = 0; i < mapLineDatas.Count; i++)
			{
				var mapLineData = mapLineDatas[i];
				var mapElemType = GetMapElementType(mapLineData.Key);
				if (mapElemType == null) continue;

				var mapElem = (MapElement)Activator.CreateInstance(mapElemType);
				mapElem.ParseArgs(mapLineData.Args.ToArray());
				mapElements.Add(mapElem);

				if (mapElem is Land land) currentLand                       = land;
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


		public static Dictionary<int, Province> LoadProvinces (Map map)
		{
			var mapElements = map.AllElements;
		
			//Find province points (white pixels)
			var provincePoints = new List<Vector2>();
			for (int y = 0; y < map.MapTexture.height; y++)
			{
				for (int x = 0; x < map.MapTexture.width; x++)
				{
					var pixel = map.MapTexture.GetPixel(x, y);
					if (pixel == Color.white) provincePoints.Add(new Vector2(x, y));
				}
			}
		
			//Create all provinces
			var provinces     = new Dictionary<int, Province>();
			int provinceCount = mapElements.OfType<Terrain>().Max(x => x.ProvinceNum);
			for (int num = 1; num <= provinceCount; num++)
			{
				var provinceBorders = mapElements.OfType<ProvinceBorders>().Where(x => x.ProvinceNum == num).ToList();
				if (provinceBorders.Count == 0) continue;

				var provincePoint = provincePoints[num -1];
				// var centerPos = Vector2.zero;
				// foreach (var pb in provinceBorders)
				// {
				// 	centerPos.x += pb.X;
				// 	centerPos.y += pb.Y;
				// }
				// centerPos /= provinceBorders.Count();

				var province = new Province(num, provincePoint);
				province.ProvinceBorders = provinceBorders;
				provinces.Add(num, province);
			}

			//Set owner of provinces
			foreach (var provinceOwner in mapElements.OfType<ProvinceOwner>())
			{
				if (provinces.ContainsKey(provinceOwner.ProvinceNum))
				{
					var nation = DomEdit.I.Nations.GetNationById(provinceOwner.NationNum);
					provinces[provinceOwner.ProvinceNum].Owner = nation;
				}
			}

			foreach (var startLocation in mapElements.OfType<StartLocation>())
			{
				if (provinces.ContainsKey(startLocation.ProvinceNum))
				{
					var nation = DomEdit.I.Nations.GetNationById(startLocation.NationNum);
					provinces[startLocation.ProvinceNum].Owner = nation;
				}
			}

			//Create commanders
			foreach (var commanderElement in mapElements.OfType<CommanderElement>())
			{
				var province  = provinces[commanderElement.ProvinceNum];
				var commander = Commander.Create(commanderElement.MonsterId, province.Owner);
				commander.Nationality = province.Owner;
				province.Monsters.Add(commander);

				//Create owned-be-commander objects
				foreach (var ownedByCommander in mapElements.OfType<IOwnedByCommander>().Where(x => x.Commander == commanderElement))
				{
					switch (ownedByCommander)
					{
						case BodyguardsElement bodyguardsElement:
							var bodyguard = Troops.Create(bodyguardsElement.MonsterId, bodyguardsElement.Amount, commander.Nationality);
							commander.Bodyguards.Add(bodyguard);
							break;
						case ItemElement itemElement:
							var item = Item.Create(itemElement.ItemName);
							commander.Items.Add(item);
							break;
						case UnitsElement unitsElement:
							var unit = Troops.Create(unitsElement.MonsterId, unitsElement.Amount, commander.Nationality);
							commander.UnitsUnderCommand.Add(unit);
							break;
						case Experience _:
							//TODO
							break;
						case Magic magic:
							var path = magic switch
							{
								AirMagic _    => MagicPath.Air,
								AstralMagic _ => MagicPath.Astral,
								BloodMagic _  => MagicPath.Blood,
								DeathMagic _  => MagicPath.Death,
								EarthMagic _  => MagicPath.Earth,
								FireMagic _   => MagicPath.Fire,
								HolyMagic _   => MagicPath.Holy,
								NatureMagic _ => MagicPath.Nature,
								WaterMagic _  => MagicPath.Water,
								_             => throw new ArgumentOutOfRangeException(nameof(magic))
							};
							var magicOverride = new MagicOverride(path, magic.MagicLevel);
							commander.MagicOverrides.Add(magicOverride);
							break;
						case ClearMagic _:
							//TODO
							break;
						default:
							throw new ArgumentOutOfRangeException(nameof(ownedByCommander));

					}
				}
			}

			return provinces;
		}

		public static List<GamePlayer> LoadPlayers (Map map)
		{
			var mapElements = map.AllElements;

			var players = new List<GamePlayer>();

			foreach (var allowedPlayer in mapElements.OfType<AllowedPlayer>())
			{
				var startLocation      = mapElements.OfType<StartLocation>().SingleOrDefault(x => x.NationNum == allowedPlayer.NationNum);
				var capitalProvinceNum = startLocation?.ProvinceNum ?? -1;
				var player             = new GamePlayer(PlayerType.Human, DomEdit.I.Nations.GetNationById(allowedPlayer.NationNum)) { CapitalProvinceNum = capitalProvinceNum };
				players.Add(player);
			}
			return players;
		}

		public static Map CreateMap (List<MapElement> mapElements)
		{
			var map = new Map();
			map.AllElements = mapElements;
		
			foreach (var mapElement in mapElements)
			{
				if (mapElement is ProvinceDataElement) continue;
				if (mapElement is AllowedPlayer) continue;
				if (mapElement is StartLocation) continue;
				if (mapElement is IOwnedByCommander) continue;
				if (mapElement is Land) continue;

				map.UnchangedElements.Add(mapElement);
			}

			map.UnchangedElements = map.UnchangedElements.OrderBy(x => x.GetType().Name).ToList();

			return map;
		}
	}

}