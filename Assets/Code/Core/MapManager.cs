using System;
using System.IO;
using System.Linq;
using Dom;
using Map;
using UI.Menus;
using UI.Menus.SearchMenu;
using Utility;
using Debug = UnityEngine.Debug;

namespace Core
{

	public class MapManager
	{
		public string SavedGamesFolderPath => $"{Prefs.DataFolderPath.Get()}savedgames/";
		public string PretendersFolderPath => $"{Prefs.DataFolderPath.Get()}savedgames/newlords/";
		public string MapsFolderPath       => $"{Prefs.DataFolderPath.Get()}maps/";
		public string MapFilePath          => $"{MapFolderPath}/{Constants.MapName}.map";
		public string MapFolderPath        => $"{Prefs.DataFolderPath.Get()}/maps/{Constants.MapName}/";

		public string  SavedMapFileName { get; set; }
		public Map.Map Map              { get; set; }

		public void LoadMap (MapFile mapFile)
		{
			Debug.Log($"Load mapFile: {mapFile}");

			Map = MapLoader.Load(mapFile);

			LoadPretender(Prefs.DefaultPretenderA.Get());
			LoadPretender(Prefs.DefaultPretenderB.Get());
			
			//Create map texture
			DomEdit.I.Ui.Get<MapPicture>().LoadMap(Map);

			//Open menus
			DomEdit.I.Ui.Get<SearchMenu>().Show();
			DomEdit.I.Ui.Get<PlayersMenu>().Show();
			DomEdit.I.Ui.Get<ControlButtonsMenu>().Show();
		}

		public void LoadMap ()
		{
			// Debug.Log($"Loading map at {MapFilePath}");
			//
			// SavedMapFileName = Path.GetFileName(MapFilePath);
			//
			// Map = MapLoader.Load(MapFilePath);
			//

			//
			//
			// // if (Game.Players.Count == 0)
			// // {
			// // 	// var pretenderA = LoadPretender(Prefs.DefaultPretenderA.Get());
			// // 	// var pretenderB = LoadPretender(Prefs.DefaultPretenderB.Get());
			// // 	
			// // 	var pretendA = Prefs.DefaultPretenderA.Get();
			// // 	if (!string.IsNullOrEmpty(pretendA))
			// // 	{
			// // 		var pretender = new Pretender(pretendA);
			// // 		var nation = Nations.GetNationByNameAndEra(pretender.NationName, pretender.Era);
			// // 		pretender.Nation = nation;
			// // 		Game.Players.Add(new GamePlayer(PlayerType.Human, nation, -1));
			// // 	}
			// // 	
			// // 	var pretendB = Prefs.DefaultPretenderB.Get();
			// // 	if (!string.IsNullOrEmpty(pretendB))
			// // 	{
			// // 		var pretender = new Pretender(pretendB);
			// // 		var nation = Nations.GetNationByNameAndEra(pretender.NationName, pretender.Era);
			// // 		pretender.Nation = nation;
			// // 		Game.Players.Add(new GamePlayer(PlayerType.Human, nation, -1));
			// // 	}
			// // }
			//
			//
			// Debug.Log("MapFilePath: " + MapFilePath + ", SavedMapFileName: " + SavedMapFileName + ", MapFolderPath: " + MapFolderPath);
			//
			// //Create map texture
			// var imageFile = Map.Elements.OfType<ImageFile>().FirstOrDefault();
			// if (imageFile != null)
			// {
			// 	string mapImagePath = $"{MapFolderPath}\\{imageFile.MapImageName}";
			// 	DomEdit.I.Ui.Get<MapPicture>().LoadMapImage(mapImagePath);
			// }
			//
			// //Open menus
			// DomEdit.I.Ui.Get<MapMenu>().LoadMap(Map);
			// DomEdit.I.Ui.Get<PlayersMenu>().Show();
		}

		private void LoadPretender (string path)
		{
			if (string.IsNullOrEmpty(path)) return;

			var pretender = new Pretender(path);
			pretender.nation = DomEdit.I.Nations.GetNationByNameAndEra(pretender.nationName, pretender.era);

			var player = Map.Players.SingleOrDefault(p => p.Nation == pretender.nation);
			if (player == null)
			{
				player = new GamePlayer(PlayerType.Human, pretender.nation);
				Map.Players.Add(player);
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

			Prefs.PreviousMapPath.Set(savePath);

			mapSaver.SaveMap(Map, MapFilePath);
			SavedMapFileName = Path.GetFileName(MapFilePath);
		}

		public void AddNation ()
		{
			var player = new GamePlayer(PlayerType.Human);
			Map.Players.Add(player);

			DomEdit.I.Ui.Get<PlayersMenu>().CreateGizmo(player);
		}

		public void RemovePlayer (GamePlayer player)
		{
			Map.Players.Remove(player);

			DomEdit.I.Ui.Get<PlayersMenu>().DestroyGizmo(player);
		}

		public void ChangeNation (GamePlayer player, int nationNum)
		{
			player.Nation = DomEdit.I.Nations.GetNationById(nationNum);

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
			if (idx < 0 || idx >= Map.Players.Count) return null;
			return Map.Players[idx];
		}
	}

}