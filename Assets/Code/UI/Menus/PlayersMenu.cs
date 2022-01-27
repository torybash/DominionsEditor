using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayersMenu : Menu
{
	[SerializeField] private RectTransform playersGroup;
	[SerializeField] private Button addButton;
	[SerializeField] private RectTransform addGizmo;	
	[SerializeField] private TMP_Dropdown eraDropdown;	
	private readonly List<PlayerGizmo> _gizmos = new List<PlayerGizmo>();

	private void Awake ()
	{
		addButton.onClick.AddListener(OnAddClicked);
		eraDropdown.onValueChanged.AddListener(OnEraChanged);
	}

	private void Start ()
	{
		eraDropdown.SetValueWithoutNotify(Prefs.Era.Get() - 1);
	}
	
	private void OnEraChanged (int eraIdx)
	{
		Prefs.Era.Set(eraIdx + 1);
	}

	private void OnAddClicked ()
	{
		DomEdit.I.PlayerSetup.AddNation();
	}

	public override void Show ()
	{
		base.Show();
		
		foreach (var gizmo in _gizmos) Destroy(gizmo.gameObject);
		_gizmos.Clear();
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