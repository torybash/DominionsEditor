using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayersMenu : Menu
{
	[SerializeField] private RectTransform playersGroup;
	[SerializeField] private Button addButton;
	
	private readonly List<PlayerGizmo> _gizmos = new List<PlayerGizmo>();

	private void Awake ()
	{
		addButton.onClick.AddListener(OnAddClicked);
	}
	
	private void OnAddClicked ()
	{
		Game.AddNation();
	}

	public void CreateGizmo (int idx, GamePlayer player)
	{
		var playerGizmo = Ui.Create<PlayerGizmo>(playersGroup);
		playerGizmo.Initialize(idx, player);

		_gizmos.Add(playerGizmo);
		
		addButton.transform.SetAsLastSibling();
	}
	
	public void UpdateGizmo (GamePlayer player)
	{
		var gizmo = _gizmos.Single(x => x.Player == player);
		gizmo.UpdateGizmo();
	}
	
	public void DestroyGizmo (GamePlayer player)
	{
		var gizmo = _gizmos.Single(x => x.Player == player);
		Destroy(gizmo.gameObject);
		_gizmos.Remove(gizmo);
	}
}