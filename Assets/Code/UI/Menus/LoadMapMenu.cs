using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadMapMenu : Menu
{
	[SerializeField] private RectTransform gizmoContainer;
	[SerializeField] private Button        closeButton;

	private List<MapFileGizmo> _gizmos = new List<MapFileGizmo>();

	private void Awake ()
	{
		closeButton.onClick.AddListener(Hide);
	}

	public override void Show ()
	{
		base.Show();
	
		transform.SetAsLastSibling();
	}

	public override void Hide ()
	{
		base.Hide();

		foreach (var g in _gizmos) Destroy(g.gameObject);
		_gizmos.Clear();
	}

	public void SelectMap (Action<MapFile> onSelect)
	{
		Show();
		// Directory.GetFiles()

		foreach (var mapPath in Directory.GetFiles(DomEdit.I.MapMan.MapsFolderPath, "*.*", SearchOption.AllDirectories))
		{
			if (!mapPath.EndsWith("map")) continue;

			var mapFile = MapFile.LoadPath(mapPath);
			// if (mapFile.era == -1) continue;
		
			var gizmo = DomEdit.I.Ui.Create<MapFileGizmo>(gizmoContainer);
			gizmo.Initialize(mapFile);
			gizmo.LoadClicked += (m) =>
			{
				onSelect?.Invoke(m);
				Hide();
			};

			_gizmos.Add(gizmo);
		}
	}

}