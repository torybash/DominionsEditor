using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Debug = UnityEngine.Debug;

public class MapManager
{
	public Searcher Searcher { get; }

	public string MapFolderPath { get; private set; }
	public string SavedGamesFolderPath => @"C:\Users\toryb\AppData\Roaming\Dominions5\savedgames\"; //TODO Set paths in dynamic way. Store in prefs.
	public List<MapElement> MapElements { get; private set; } = new List<MapElement>();

	public MonstersTable Monsters { get; set; }
	public string MapFilePath { get; set; }
	public string SavedMapFileName  { get; set; }
	public NationsTable Nations { get; set; }


	public MapManager (Searcher searcher)
	{
		Searcher = searcher;
	}
	
	public void ParseMap (string mapFilePath)
	{
		Debug.Log($"Loading map at {mapFilePath}");
		
		var mapLoader = new MapLoader(this);

		MapFilePath = mapFilePath;
		int lastSepIdx = mapFilePath.LastIndexOf('\\');

		MapFolderPath = mapFilePath.Substring(0, lastSepIdx + 1);
		Debug.Log("MapFolderPath: "+ MapFolderPath);

		SavedMapFileName = mapFilePath.Substring(lastSepIdx + 1);
		int extIdx = SavedMapFileName.LastIndexOf('.');
		SavedMapFileName = SavedMapFileName.Insert(extIdx, Constants.EditedMapTag);
		Debug.Log("SavedMapFileName: " + SavedMapFileName);
		
		MapElements = mapLoader.LoadMap(mapFilePath);
	}
	
	public MonstersTable.Entry GetMonster(int monsterId)
	{
		return Monsters.GetMonster(monsterId);
	}
	
	public MonstersTable.Entry GetMonster (string unitName)
	{
		return Monsters.GetMonster(unitName);
	}

	public void AddMapElement (MapElement elem)
	{
		MapElements.Add(elem);
		
		switch (elem)
		{
			case IOwnedByCommander ownedByCommander:
			{
				int commanderIdx = MapElements.IndexOf(ownedByCommander.Commander);
				MapElements.Remove(elem);
				MapElements.Insert(commanderIdx + 1, elem);
				break;
			}
			case IOwnedByProvince ownedByProvince:
			{
				var landOwner = MapElements.OfType<Land>().SingleOrDefault(x => x.ProvinceNum == ownedByProvince.ProvinceNum);
				if (landOwner == null)
				{
					landOwner = new Land
					{
						Man = this,
						ProvinceNum = ownedByProvince.ProvinceNum
					};
					MapElements.Add(landOwner);
				}
				int landIdx = MapElements.IndexOf(landOwner);
				MapElements.Remove(elem);
				if (landIdx + 1 >= MapElements.Count)
				{
					MapElements.Add(elem);
				} else
				{
					MapElements.Insert(landIdx + 1, elem);
				}
				break;
			}
		}
	}
	
	public void RemoveMapElement (MapElement elem)
	{
		MapElements.Remove(elem);
	}
	
	public void SaveMap ()
	{
		var mapSaver = new MapSaver(this);

		int extIdx = MapFilePath.LastIndexOf('.');
		var savePath = MapFilePath.Insert(extIdx, Constants.EditedMapTag);
		mapSaver.SaveMap(savePath);
	}
}