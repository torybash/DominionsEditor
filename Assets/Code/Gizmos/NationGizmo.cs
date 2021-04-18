using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NationGizmo : Gizmo
{
	[SerializeField] private Image flagImage;
	[SerializeField] private TMP_Text nameLabel;
	[SerializeField] private GameObject capitalMarker;

	public Nation Nation { get; private set; }

	private void Awake ()
	{
		ShowCapitalMarker(false);
	}
	
	public void SetNation (Nation nation)
	{
		Nation = nation;
		
		var entry = Map.Nations.GetNationEntry(nation);
		flagImage.sprite = entry.Sprite;
		nameLabel.text = entry.Name;
	}

	public void ShowCapitalMarker (bool show)
	{
		capitalMarker.SetActive(show);
	}
}