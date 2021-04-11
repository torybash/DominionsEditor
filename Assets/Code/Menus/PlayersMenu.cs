using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayersMenu : Menu
{
	[SerializeField] private RectTransform playersGroup;
	
	private List<PlayerGizmo> _gizmos = new List<PlayerGizmo>();

	public void CreateGizmo (int idx, GamePlayer player)
	{
		var playerGizmo = Ui.Create<PlayerGizmo>(playersGroup);
		playerGizmo.Initialize(idx, player);

		_gizmos.Add(playerGizmo);
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