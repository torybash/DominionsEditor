using System.Collections.Generic;
using System.Linq;
using Core;
using TMPro;
using UI.Gizmos;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI.Menus
{

	public class PlayersMenu : Menu
	{
		[SerializeField] RectTransform playersGroup;
		[SerializeField] Button        addButton;
		[SerializeField] RectTransform addGizmo;	
		[SerializeField] TMP_Dropdown  eraDropdown;

		readonly List<PlayerGizmo> _gizmos = new List<PlayerGizmo>();

		void Awake ()
		{
			addButton.onClick.AddListener(OnAddClicked);
			eraDropdown.onValueChanged.AddListener(OnEraChanged);
		}

		void Start ()
		{
			eraDropdown.SetValueWithoutNotify(Prefs.Era.Get() - 1);
		}

		void OnEraChanged (int eraIdx)
		{
			Prefs.Era.Set(eraIdx + 1);
		}

		void OnAddClicked ()
		{
			DomEdit.I.MapMan.map.AddPlayer();
		}

		public override void Show ()
		{
			base.Show();
		
			foreach (var gizmo in _gizmos) Destroy(gizmo.gameObject);
			_gizmos.Clear();
		
			foreach (var player in DomEdit.I.MapMan.map.players)
			{
				DomEdit.I.Ui.Get<PlayersMenu>().CreateGizmo(player);
			}
		}
	
		public void CreateGizmo (GamePlayer player)
		{
			var playerGizmo = DomEdit.I.Ui.Create<PlayerGizmo>(playersGroup);
			playerGizmo.Initialize(player);

			_gizmos.Add(playerGizmo);
		
			addGizmo.transform.SetAsLastSibling();

			UpdateAllGizmos();
		}

		public void DestroyGizmo (GamePlayer player)
		{
			var gizmo = _gizmos.Single(x => x.Player == player);
			Destroy(gizmo.gameObject);
			_gizmos.Remove(gizmo);

			UpdateAllGizmos();
		}
	
		public void UpdateAllGizmos ()
		{
			foreach (var gizmo in _gizmos)
			{
				gizmo.UpdateGizmo();
			}
		}
	
		public void UpdateGizmo (GamePlayer player)
		{
			var gizmo = _gizmos.Single(x => x.Player == player);
			gizmo.UpdateGizmo();
		}
	}

}