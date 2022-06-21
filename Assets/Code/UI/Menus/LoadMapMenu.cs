using System;
using System.Collections.Generic;
using System.IO;
using Core;
using UI.Gizmos;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menus
{

	public class LoadMapMenu : Menu
	{
		[SerializeField] RectTransform gizmoContainer;
		[SerializeField] Button        closeButton;

		List<MapFileGizmo> _gizmos = new List<MapFileGizmo>();

		void Awake ()
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

		public void SelectMap (Action<DomFile> onSelect)
		{
			Show();
			// Directory.GetFiles()

			foreach (var mapPath in Directory.GetFiles(Paths.MapsFolderPath, "*.*", SearchOption.AllDirectories))
			{
				if (!mapPath.EndsWith("map")) continue;

				var mapFile = DomFile.LoadPath(mapPath);
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

}