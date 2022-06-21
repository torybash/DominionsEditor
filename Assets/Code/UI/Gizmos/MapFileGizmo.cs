using System;
using TMPro;
using UI.Menus;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gizmos
{

	public class MapFileGizmo : Gizmo
	{
		public event Action<DomFile> LoadClicked;

		[SerializeField] TMP_Text fileText;
		[SerializeField] Button   loadButton;

		DomFile _mapFile;

		void Awake ()
		{
			loadButton.onClick.AddListener(() => LoadClicked?.Invoke(_mapFile));
		}

		public void Initialize (DomFile mapFile)
		{
			_mapFile      = mapFile;
			fileText.text = mapFile.name;
		}
	}

}