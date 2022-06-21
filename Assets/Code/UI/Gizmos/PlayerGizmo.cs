using Core;
using TMPro;
using UI.Menus;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gizmos
{

	public class PlayerGizmo : Gizmo
	{
		[SerializeField] Button   removeButton;
		[SerializeField] Button   playerTypeButton;
		[SerializeField] Button   nationButton;
		[SerializeField] TMP_Text numberText;
		[SerializeField] TMP_Text typeText;

		NationGizmo _nationGizmo;
		public GamePlayer  Player { get; set; }
	
		public int Number => DomEdit.I.MapMan.map.players.IndexOf(Player) + 1;

		void Awake ()
		{
			removeButton.onClick.AddListener(OnRemove);
			playerTypeButton.onClick.AddListener(OnPlayerTypeButton);
			nationButton.onClick.AddListener(OnNationButton);
		}

		public void Initialize (GamePlayer gamePlayer)
		{
			Debug.Log($"Initialize: {gamePlayer.Nation}");
			Player = gamePlayer;
		
			_nationGizmo = DomEdit.I.Ui.Create<NationGizmo>(transform);
			nationButton.transform.SetAsLastSibling();
			UpdateGizmo();
		}

		void OnRemove ()
		{
			DomEdit.I.MapMan.map.RemovePlayer(Player);
		}

		void OnPlayerTypeButton ()
		{
			// int change = eventData.button switch
			// {
			// 	PointerEventData.InputButton.Left => 1,
			// 	PointerEventData.InputButton.Right => -1,
			// 	_ => 0
			// };

			int change = Input.GetMouseButton(0) ? 1 : -1;
			DomEdit.I.MapMan.IncrementType(Player, change);
		}

		void OnNationButton ()
		{
			DomEdit.I.Ui.Get<PretenderLoadMenu>().SelectPretender(pretender =>
			{
				DomEdit.I.MapMan.map.SetPlayerPretender(Player, pretender);
			});
		}

		public void UpdateGizmo ()
		{
			numberText.text = Number.ToString();

			_nationGizmo.SetNation(Player.Nation);
			typeText.text = Player.Type.ToString();
		}
	}

}