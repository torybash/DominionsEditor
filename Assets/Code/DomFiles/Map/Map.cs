using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Entities;
using Data;
using Dom;
using Map.MapElements;
using QuickCombat;
using UI.Menus;
using UnityEngine;
using Utility;

namespace Map
{

	public class Map
	{
		public List<MapElement>          allElements       = new List<MapElement>();
		public List<MapElement>          unchangedElements = new List<MapElement>();
		public Dictionary<int, Province> provinceMap       = new Dictionary<int, Province>();

		public Texture2D        mapTexture;
		public List<GamePlayer> players;
		public DomFile          mapFile;

		public void LoadPlayers ()
		{
			players = new List<GamePlayer>();

			foreach (var allowedPlayer in allElements.OfType<AllowedPlayer>())
			{
				Nation        nation        = D.Nations.GetNationById(allowedPlayer.NationNum);
				StartLocation startLocation = allElements.OfType<StartLocation>().SingleOrDefault(x => x.NationNum == allowedPlayer.NationNum);
				GamePlayer    player        = new GamePlayer(PlayerType.Human, nation) { CapitalProvinceNum = startLocation?.ProvinceNum ?? -1 };

				Pretender pretender = LoadLastPretender(nation);
				if (pretender != null)
				{
					player.Pretender = pretender;
				}
				
				players.Add(player);
			}
			// LoadPretender(Prefs.DefaultPretenderA.Get());
			// LoadPretender(Prefs.DefaultPretenderB.Get());
		}
		
		public void AddPlayer ()
		{
			var player = new GamePlayer(PlayerType.Human);
			players.Add(player);

			DomEdit.I.Ui.Get<PlayersMenu>().CreateGizmo(player);

			SavePretenders();
		}

		public void RemovePlayer (GamePlayer player)
		{
			players.Remove(player);

			DomEdit.I.Ui.Get<PlayersMenu>().DestroyGizmo(player);
			
			SavePretenders();
		}
		
		public void SetPlayerPretender (GamePlayer player, Pretender pretender)
		{
			player.Pretender = pretender;
			player.Nation    = D.Nations.GetNationById(pretender.nation.id);

			DomEdit.I.Ui.Get<PlayersMenu>().UpdateGizmo(player);
			
			SavePretenders();
		}
		
		static Pretender LoadLastPretender (Nation nation)
		{
			LoadedPretenders lastPretenders = JsonUtility.FromJson<LoadedPretenders>(Prefs.LastPretenders);
			if (lastPretenders != null && lastPretenders.nationToPretender.TryGetElement(nation, out var pretender)) return pretender;
			return default;
		}

		// void LoadPretender (string path)
		// {
		// 	if (string.IsNullOrEmpty(path)) return;
		//
		// 	var pretender = new Pretender(path);
		// 	pretender.nation = DomEdit.I.Nations.GetNationByNameAndEra(pretender.nationName, pretender.era);
		//
		// 	var player = Map.players.SingleOrDefault(p => p.Nation == pretender.nation);
		// 	if (player == null)
		// 	{
		// 		player = new GamePlayer(PlayerType.Human, pretender.nation);
		// 		players.Add(player);
		// 	}
		// 	player.Pretender = pretender;
		// }

		void SavePretenders ()
		{
			var lastPretenders = JsonUtility.FromJson<LoadedPretenders>(Prefs.LastPretenders) ?? new LoadedPretenders();
			foreach (var p in players)
			{
				if (lastPretenders.nationToPretender.ContainsKey(p.Nation))
				{
					lastPretenders.nationToPretender.SetElement(p.Nation, p.Pretender);
				} else
				{
					lastPretenders.nationToPretender.Add(p.Nation, p.Pretender);
				}
			}
			
			Prefs.LastPretenders.Set(JsonUtility.ToJson(lastPretenders));
		}
		
		[Serializable]
		public class LoadedPretenders
		{
			public DictionarySerializable<Nation, Pretender> nationToPretender = new DictionarySerializable<Nation, Pretender>();
		}
	}

}