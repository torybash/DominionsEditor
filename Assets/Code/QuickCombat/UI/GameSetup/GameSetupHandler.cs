using System.Linq;
using Dom;
using QuickCombat.UI.Popups;
using UnityEngine;
using Utility;

namespace QuickCombat
{
	public class GameSetupHandler
	{
		readonly Session _session;

		GameSetupPopup _popup;

		public GameSetupHandler (Session session)
		{
			_session = session;
		}

		public void Run ()
		{
			_popup = M.ShowPopup<GameSetupPopup>();

			_popup.eraDropdown.onValueChanged.RemoveAllListeners();
			_popup.eraDropdown.onValueChanged.AddListener(OnEraChanged);
			_popup.eraDropdown.SetValueWithoutNotify(Prefs.Era.Get() - 1);

			_popup.startButton.onClick.RemoveAllListeners();
			_popup.startButton.onClick.AddListener(OnStartClicked);

			for (int i = 0; i < _popup.nationSelectors.Count; i++)
			{
				PlayerSide side = (PlayerSide)i;

				NationSelector selector = _popup.nationSelectors[i];
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

			NationSelector selector = _popup.nationSelectors[(int)side];
			selector.SetNation(pretender.nation);

			bool hasPretendersForPlayers = _session.players.All(x => x.nation != null);
			_popup.startButton.interactable = hasPretendersForPlayers;
		}

		void OnStartClicked ()
		{
			Debug.Log("START!");
		}
	}

}