using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

public class MapManager
{
	public Searcher Searcher { get; }
	public MapManager (Searcher searcher)
	{
		Searcher = searcher;
	}
	public string MapFolderPath { get; private set; }
	public List<MapElement> MapElements { get; private set; } = new List<MapElement>();
	public Menu[] Menus { get; set; }
	public Gizmo[] GizmoTemplates { get; set; }
	public MonstersTable Monsters { get; set; }
	public string MapFilePath { get; set; }
	public NationsTable Nations { get; set; }

	public T GetMenu<T> () => Menus.OfType<T>().SingleOrDefault();

	public void ParseMap (string mapFilePath)
	{
		Debug.Log($"Loading map at {mapFilePath}");

		var mapLoader = new MapLoader(this);

		MapFilePath = mapFilePath;
		int lastSepIdx = mapFilePath.LastIndexOf('\\');
		MapFolderPath = mapFilePath.Substring(0, lastSepIdx + 1);
		
		MapElements = mapLoader.LoadMap(mapFilePath);

		GetMenu<LoadMapMenu>().Hide();
		GetMenu<MapMenu>().Show();
	}

	public T Create<T> (Transform parent = null) where T : Component
	{
		var template = (Component) GizmoTemplates.OfType<T>().Single();
		
		var instance = Object.Instantiate(template, parent != null ? parent : template.transform.parent);
		if (instance is Gizmo gizmo)
		{
			gizmo.Man = this;
		}
		instance.gameObject.SetActive(true);

		return (T)instance;
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
				MapElements.Insert(landIdx + 1, elem);
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
		var savePath = MapFilePath.Insert(extIdx, "_edited");
		mapSaver.SaveMap(savePath);
	}
}