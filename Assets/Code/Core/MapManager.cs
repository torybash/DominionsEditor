using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Debug = UnityEngine.Debug;

public class MapManager
{
	public string SavedGamesFolderPath => $@"{Prefs.DataFolderPath.Get()}savedgames/";
	public string PretendersFolderPath => $@"{Prefs.DataFolderPath.Get()}savedgames/newlords/";
	public string MapFilePath          => $"{MapFolderPath}/{Constants.MapName}.map";
	public string MapFolderPath        => $"{Prefs.DataFolderPath.Get()}/maps/{Constants.MapName}/";

	public string                    SavedMapFileName { get; set; }
	public Map                       Map              { get; set; }
	public List<GamePlayer>          Players          { get; set; } = new List<GamePlayer>();

	public void LoadMap ()
	{
		Debug.Log($"Loading map at {MapFilePath}");

		SavedMapFileName = Path.GetFileName(MapFilePath);

		MapLoader.Load(MapFilePath, out var map, out var players);
		Map     = map;
		Players = players;

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


		Debug.Log("MapFilePath: " + MapFilePath + ", SavedMapFileName: " + SavedMapFileName + ", MapFolderPath: " + MapFolderPath);

		//Create map texture
		var imageFile = Map.Elements.OfType<ImageFile>().FirstOrDefault();
		if (imageFile != null)
		{
			string mapImagePath = $"{MapFolderPath}\\{imageFile.MapImageName}";

			DomEdit.I.Ui.Get<MapPicture>().Show();
			DomEdit.I.Ui.Get<MapPicture>().SetMapImage(mapImagePath);
		}

		DomEdit.I.Ui.Get<LoadMapMenu>().Hide();
		DomEdit.I.Ui.Get<MapMenu>().Show();
		DomEdit.I.Ui.Get<PlayersMenu>().Show();

		foreach (var player in Players)
		{
			DomEdit.I.Ui.Get<PlayersMenu>().CreateGizmo(player);
		}
	}

	private void LoadPretender (string path)
	{
		if (string.IsNullOrEmpty(path)) return;

		var pretender = new Pretender(path);
		pretender.nation = DomEdit.I.nations.GetNationByNameAndEra(pretender.nationName, pretender.era);

		var player = Players.SingleOrDefault(p => p.Nation == pretender.nation);
		if (player == null)
		{
			player = new GamePlayer(PlayerType.Human, pretender.nation);
			Players.Add(player);
		}
		player.Pretender = pretender;
	}

	public void SaveMap ()
	{
		var mapSaver = new MapSaver();
		var savePath = MapFilePath;
		// if (MapFilePath.IndexOf(Constants.EditedMapTag, StringComparison.InvariantCultureIgnoreCase) == -1)
		// {
		// 	int extIdx = savePath.LastIndexOf('.');
		// 	// savePath = savePath.Insert(extIdx, Constants.EditedMapTag);
		//
		// 	SavedMapFileName = Path.GetFileName(savePath);
		// }

		mapSaver.SaveMap(Map, MapFilePath);
	}

	public void AddNation ()
	{
		var player = new GamePlayer(PlayerType.Human);
		Players.Add(player);

		DomEdit.I.Ui.Get<PlayersMenu>().CreateGizmo(player);
	}

	public void RemovePlayer (GamePlayer player)
	{
		Players.Remove(player);

		DomEdit.I.Ui.Get<PlayersMenu>().DestroyGizmo(player);
	}

	public void ChangeNation (GamePlayer player, int nationNum)
	{
		player.Nation = nationNum;

		DomEdit.I.Ui.Get<PlayersMenu>().UpdateGizmo(player);
	}

	public void IncrementType (GamePlayer player, int change)
	{
		var newType                    = player.Type;
		int newTypeInt                 = ((int)newType + change)%Enum.GetValues(typeof(PlayerType)).Length;
		if (newTypeInt < 0) newTypeInt += Enum.GetValues(typeof(PlayerType)).Length;
		player.Type = (PlayerType)newTypeInt;

		DomEdit.I.Ui.Get<PlayersMenu>().UpdateGizmo(player);
	}

	public GamePlayer GetPlayer (int number)
	{
		int idx = number - 1;
		if (idx < 0 || idx >= Players.Count) return null;
		return Players[idx];
	}
}