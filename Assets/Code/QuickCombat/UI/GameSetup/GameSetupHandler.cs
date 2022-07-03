using System.Linq;
using Dom;
using QuickCombat.UI;
using QuickCombat.UI.Popups;
using UnityEngine;
using Utility;

namespace QuickCombat
{
	public class GameSetupHandler
	{
		readonly Session _session;

		GameSetupMenu _menu;

		public GameSetupHandler (Session session)
		{
			_session = session;
		}

		public void Run ()
		{
			_menu = M.ShowMenu<GameSetupMenu>();

			_menu.eraDropdown.onValueChanged.RemoveAllListeners();
			_menu.eraDropdown.onValueChanged.AddListener(OnEraChanged);
			_menu.eraDropdown.SetValueWithoutNotify(Prefs.Era.Get() - 1);

			_menu.startButton.onClick.RemoveAllListeners();
			_menu.startButton.onClick.AddListener(OnStartClicked);

			for (int i = 0; i < _menu.nationSelectors.Count; i++)
			{
				PlayerSide side = (PlayerSide)i;

				NationSelector selector = _menu.nationSelectors[i];
				selector.loadButton.onClick.AddListener(() => OnLoadPretenderClicked(side));
			}
		}



		void OnEraChanged (int eraIdx)
		{
			Prefs.Era.Set(eraIdx + 1);
		}

		void OnLoadPretenderClicked (PlayerSide side)
		{
			var pretenderLoad = new PretenderLoadHandler();
			pretenderLoad.Run(p => OnPretenderSelected(side, p));
			// _popup.nationSelectors
			// DomEdit.I.Ui.Get<PretenderLoadMenu>().SelectPretender(pretender =>
			// {
			// 	DomEdit.I.MapMan.map.SetPlayerPretender(Player, pretender);
			// });
		}

		void OnPretenderSelected (PlayerSide side, Pretender pretender)
		{
			_session.SetPlayerPretender(side, pretender);

			NationSelector selector = _menu.nationSelectors[(int)side];
			selector.SetNation(pretender.nation);

			bool hasPretendersForPlayers = _session.players.All(x => x.nation != null);
			_menu.startButton.interactable = hasPretendersForPlayers;
		}

		void OnStartClicked ()
		{
			Debug.Log("START!");
			
			M.CloseMenu<GameSetupMenu>();
			_session.RunCombatEdit();
		}
	}

}