using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerGizmo : Gizmo, IPointerDownHandler
{
	[SerializeField] private Button typeButton;
	[SerializeField] private TMP_Text numberText;
	[SerializeField] private TMP_Text typeText;
	
	private NationGizmo _nationGizmo;
	public GamePlayer Player { get; set; }
	public int Index { get; set; }

	private void Awake ()
	{
		// typeButton.onClick.AddListener(OnClicked);
	}

	public void Initialize (int idx, GamePlayer gamePlayer)
	{
		Index = idx;
		Player = gamePlayer;
		
		numberText.text = idx.ToString();

		_nationGizmo = Ui.Create<NationGizmo>(transform);

		UpdateGizmo();
	}

	private void OnClicked ()
	{
		// Man.GameSetup.IncrementType(Index, 1);
	}

	public void UpdateGizmo ()
	{
		numberText.text = (Index + 1).ToString();

		_nationGizmo.SetNation(Player.NationNum);
		typeText.text = Player.Type.ToString();
	}
	
	public void OnPointerDown (PointerEventData eventData)
	{
		int change = eventData.button switch
		{
			PointerEventData.InputButton.Left => 1,
			PointerEventData.InputButton.Right => -1,
			_ => 0
		};
		
		Game.IncrementType(Index, change);
	}
}