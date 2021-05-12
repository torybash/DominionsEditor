using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayersMenu : Menu
{
	[SerializeField] private RectTransform playersGroup;
	[SerializeField] private Button addButton;
	[SerializeField] private RectTransform addGizmo;	
	private readonly List<PlayerGizmo> _gizmos = new List<PlayerGizmo>();

	private void Awake ()
	{
		addButton.onClick.AddListener(OnAddClicked);
	}
	
	private void OnAddClicked ()
	{
		Game.AddNation();
	}

	public void CreateGizmo (GamePlayer player)
	{
		var playerGizmo = Ui.Create<PlayerGizmo>(playersGroup);
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