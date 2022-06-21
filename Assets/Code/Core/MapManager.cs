using System;
using System.IO;
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

		public Map.Map map;

		public void LoadMap (DomFile mapFile)
		{
			Debug.Log($"Load mapFile: {mapFile}");

			map = MapLoader.Load(mapFile);
			map.LoadPlayers();

			// LoadPretender(Prefs.DefaultPretenderA.Get());
			// LoadPretender(Prefs.DefaultPretenderB.Get());

			//Create map texture
			DomEdit.I.Ui.Get<MapPicture>().LoadMap(map.provinceMap, map.mapTexture);

			//Open menus
			DomEdit.I.Ui.Get<SearchMenu>().Show();
			DomEdit.I.Ui.Get<PlayersMenu>().Show();
			DomEdit.I.Ui.Get<ControlButtonsMenu>().Show();
		}

		public void SaveMap ()
		{
			var    mapSaver = new MapSaver();
			mapSaver.SaveMap(map, Paths.MapFilePath);
			Prefs.PreviousMapPath.Set(Paths.MapFilePath);
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
			if (idx < 0 || idx >= map.players.Count) return null;
			return map.players[idx];
		}
	}

}