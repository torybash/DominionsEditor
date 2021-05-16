using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class MapManager
{
	public Searcher Searcher { get; }

	public string SavedGamesFolderPath => $@"{Prefs.DataFolderPath.Get()}savedgames/";
	public string PretendersFolderPath => $@"{Prefs.DataFolderPath.Get()}savedgames/newlords/";
	public Dictionary<int, Province> ProvinceMap { get; set; } = new Dictionary<int, Province>();

	public Province this [int provinceNum] => ProvinceMap[provinceNum];
	
	public MonstersTable Monsters { get; set; }
	public ItemsTable Items { get; set; }
	public string MapFilePath => $"{MapFolderPath}/{Constants.MapName}.map";
	public string SavedMapFileName  { get; set; }
	public NationsTable Nations { get; set; }
	public MapConfig Config { get; set; }
	public UiManager Ui { get; set; }
	public string MapFolderPath => $"{Prefs.DataFolderPath.Get()}/maps/{Constants.MapName}/";
	public GameSetup Game { get; set; }

	public MapManager (Searcher searcher, UiManager uiManager, GameSetup gameSetup)
	{
		Game = gameSetup;
		Searcher = searcher;
		Ui = uiManager;
	}

	public void LoadMap ()
	{
		Debug.Log($"Loading map at {MapFilePath}");
		
		SavedMapFileName = Path.GetFileName(MapFilePath);

		var mapElements = MapLoader.LoadMapElements(MapFilePath);
		Game.Players = MapLoader.CreatePlayers(mapElements);
		Config = MapLoader.CreateConfig(mapElements);
		ProvinceMap = MapLoader.CreateMap(mapElements);

		LoadPretender(Prefs.DefaultPretenderA.Get());
		LoadPretender(Prefs.DefaultPretenderB.Get());


		

		// if (Game.Players.Count == 0)
		// {
		// 	// var pretenderA = LoadPretender(Prefs.DefaultPretenderA.Get());
		// 	// var pretenderB = LoadPretender(Prefs.DefaultPretenderB.Get());
		// 	
		// 	var pretendA = Prefs.DefaultPretenderA.Get();
		// 	if (!string.IsNullOrEmpty(pretendA))
		// 	{
		// 		var pretender = new Pretender(pretendA);
		// 		var nation = Nations.GetNationByNameAndEra(pretender.NationName, pretender.Era);
		// 		pretender.Nation = nation;
		// 		Game.Players.Add(new GamePlayer(PlayerType.Human, nation, -1));
		// 	}
		// 	
		// 	var pretendB = Prefs.DefaultPretenderB.Get();
		// 	if (!string.IsNullOrEmpty(pretendB))
		// 	{
		// 		var pretender = new Pretender(pretendB);
		// 		var nation = Nations.GetNationByNameAndEra(pretender.NationName, pretender.Era);
		// 		pretender.Nation = nation;
		// 		Game.Players.Add(new GamePlayer(PlayerType.Human, nation, -1));
		// 	}
		// }

		
		Debug.Log("MapFilePath: " + MapFilePath + ", SavedMapFileName: "+ SavedMapFileName + ", MapFolderPath: "+ MapFolderPath);
		
		//Create map texture
		var imageFile = mapElements.OfType<ImageFile>().FirstOrDefault();
		if (imageFile != null)
		{
			string mapImagePath = $"{MapFolderPath}\\{imageFile.MapImageName}";

			Ui.Get<MapPicture>().Show();
			Ui.Get<MapPicture>().SetMapImage(mapImagePath);
		}
		
		Ui.Get<LoadMapMenu>().Hide();
		Ui.Get<MapMenu>().Show();
		Ui.Get<PlayersMenu>().Show();

		foreach (var player in Game.Players)
		{
			Ui.Get<PlayersMenu>().CreateGizmo(player);
		}
	}
	
	private void LoadPretender (string path)
	{
		if (string.IsNullOrEmpty(path)) return;

		var pretender = new Pretender(path);
		pretender.Nation = Nations.GetNationByNameAndEra(pretender.NationName, pretender.Era);

		var player = Game.Players.SingleOrDefault(p => p.Nation == pretender.Nation);
		if (player == null)
		{
			player = new GamePlayer(PlayerType.Human, pretender.Nation);
			Game.Players.Add(player);
		}
		player.Pretender = pretender;
	}

	public MonsterEntry GetMonster(int monsterId)
	{
		return Monsters.GetMonster(monsterId);
	}
	
	public MonsterEntry GetMonster (string unitName)
	{
		return Monsters.GetMonster(unitName);
	}

	public void SaveMap ()
	{
		var mapSaver = new MapSaver(this);
		var savePath = MapFilePath;
		// if (MapFilePath.IndexOf(Constants.EditedMapTag, StringComparison.InvariantCultureIgnoreCase) == -1)
		// {
		// 	int extIdx = savePath.LastIndexOf('.');
		// 	// savePath = savePath.Insert(extIdx, Constants.EditedMapTag);
		//
		// 	SavedMapFileName = Path.GetFileName(savePath);
		// }

		mapSaver.SaveMap(MapFilePath);
	}
}