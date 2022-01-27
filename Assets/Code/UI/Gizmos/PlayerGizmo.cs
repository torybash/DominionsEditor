using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerGizmo : Gizmo
{
	[SerializeField] private Button removeButton;
	[SerializeField] private Button playerTypeButton;
	[SerializeField] private Button nationButton;
	[SerializeField] private TMP_Text numberText;
	[SerializeField] private TMP_Text typeText;
	
	private NationGizmo _nationGizmo;
	public  GamePlayer  Player { get; set; }
	
	public int Number => DomEdit.I.MapMan.Players.IndexOf(Player) + 1;

	private void Awake ()
	{
		removeButton.onClick.AddListener(OnRemove);
		playerTypeButton.onClick.AddListener(OnPlayerTypeButton);
		nationButton.onClick.AddListener(OnNationButton);
	}

	public void Initialize (GamePlayer gamePlayer)
	{
		Player = gamePlayer;
		
		_nationGizmo = DomEdit.I.Ui.Create<NationGizmo>(transform);
		nationButton.transform.SetAsLastSibling();
		UpdateGizmo();
	}
	
	private void OnRemove ()
	{
		DomEdit.I.MapMan.RemovePlayer(Player);
	}
	
	private void OnPlayerTypeButton ()
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
	
	private void OnNationButton ()
	{
		DomEdit.I.Ui.Get<PretenderLoadMenu>().SelectPretender((pretender) =>
		{
			Player.Pretender = pretender;
			
			DomEdit.I.MapMan.ChangeNation(Player, pretender.nation.Number);
		});
	}

	public void UpdateGizmo ()
	{
		numberText.text = Number.ToString();

		_nationGizmo.SetNation(Player.Nation);
		typeText.text = Player.Type.ToString();
	}
}