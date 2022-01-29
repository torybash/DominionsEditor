using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapFileGizmo : Gizmo
{
	public event Action<MapFile> LoadClicked;

	[SerializeField] private TMP_Text fileText;
	[SerializeField] private Button   loadButton;

	private MapFile _mapFile;

	private void Awake ()
	{
		loadButton.onClick.AddListener(() => LoadClicked?.Invoke(_mapFile));
	}

	public void Initialize (MapFile mapFile)
	{
		_mapFile      = mapFile;
		fileText.text = mapFile.name;
	}
}