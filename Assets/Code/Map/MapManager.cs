using System;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;

public class MapManager
{
	public Searcher Searcher { get; }

	public string SavedGamesFolderPath => $@"{PrefManager.DataFolderPath.Get()}savedgames/";
	public Dictionary<int, Province> ProvinceMap { get; set; } = new Dictionary<int, Province>();
	public List<GamePlayer> Players { get; set;  } = new List<GamePlayer>();

	public Province this [int provinceNum] => ProvinceMap[provinceNum];
	
	public MonstersTable Monsters { get; set; }
	public string MapFilePath { get; set; }
	public string SavedMapFileName  { get; set; }
	public NationsTable Nations { get; set; }
	public MapConfig Config { get; set; }

	public MapManager (Searcher searcher)
	{
		Searcher = searcher;
	}
	
	public void ParseMap (string mapFilePath)
	{
		Debug.Log($"Loading map at {mapFilePath}");
		
		MapFilePath = mapFilePath;
		
		int lastSepIdx = mapFilePath.LastIndexOf('\\');
		SavedMapFileName = mapFilePath.Substring(lastSepIdx + 1);

		var mapElements = MapLoader.LoadMapElements(mapFilePath);
		Players = MapLoader.CreatePlayers(mapElements);
		Config = MapLoader.CreateConfig(mapElements);
		ProvinceMap = MapLoader.CreateMapProvinces(mapElements);
	}
	
	public MonstersTable.Entry GetMonster(int monsterId)
	{
		return Monsters.GetMonster(monsterId);
	}
	
	public MonstersTable.Entry GetMonster (string unitName)
	{
		return Monsters.GetMonster(unitName);
	}

	// public void AddMapElement (MapElement elem)
	// {
	// 	switch (elem)
	// 	{
	// 		case IOwnedByCommander ownedByCommander:
	// 			int commanderIdx = MapElements.IndexOf(ownedByCommander.Commander);
	// 			MapElements.Insert(commanderIdx + 1, elem);
	// 			break;
	// 		case IOwnedByProvince ownedByProvince:
	// 			var land = GetLand(ownedByProvince.ProvinceNum);
	// 			int insertIdx = MapElements.IndexOf(land) + 1;
	//
	// 			var provinceOwner = MapElements.OfType<ProvinceOwner>().SingleOrDefault(x => x.ProvinceNum == ownedByProvince.ProvinceNum);
	// 			if (provinceOwner != null)
	// 			{
	// 				insertIdx = MapElements.IndexOf(provinceOwner) + 1;
	// 			}
	// 			
	// 			// if (landIdx + 1 >= MapElements.Count)
	// 			// {
	// 			// 	MapElements.Add(elem);
	// 			// } else
	// 			// {
	// 				MapElements.Insert(insertIdx, elem);
	// 			// }
	// 			break;
	// 		default:
	// 			MapElements.Add(elem);
	// 			break;
	// 	}
	// }
	// public Land GetLand (int provinceNum)
	// {
	//
	// 	var landOwner = MapElements.OfType<Land>().SingleOrDefault(x => x.ProvinceNum == provinceNum);
	// 	if (landOwner == null)
	// 	{
	// 		landOwner = new Land
	// 		{
	// 			ProvinceNum = provinceNum
	// 		};
	// 		MapElements.Add(landOwner);
	// 	}
	// 	return landOwner;
	// }

	public void SaveMap ()
	{
		var mapSaver = new MapSaver(this);

		var savePath = MapFilePath;
		if (MapFilePath.IndexOf(Constants.EditedMapTag, StringComparison.InvariantCultureIgnoreCase) == -1)
		{
			int extIdx = savePath.LastIndexOf('.');
			savePath = savePath.Insert(extIdx, Constants.EditedMapTag);
			
			int lastSepIdx = savePath.LastIndexOf('\\');
			SavedMapFileName = SavedMapFileName.Substring(lastSepIdx + 1);
		}

		mapSaver.SaveMap(savePath);
	}
}