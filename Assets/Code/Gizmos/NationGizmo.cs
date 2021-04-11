using System;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;
using UnityEngine.UI;

public class NationGizmo : Gizmo
{
	[SerializeField] private Image flagImage;
	[SerializeField] private TMP_Text nameLabel;
	[SerializeField] private GameObject capitalMarker;

	public int NationNum { get; private set; }

	private void Awake ()
	{
		ShowCapitalMarker(false);
	}
	
	public void SetPlayerNumber (int nationNum)
	{
		NationNum = nationNum;
		
		var entry = Man.Nations.GetNationEntry(nationNum);
		flagImage.sprite = entry.Sprite;
		nameLabel.text = entry.Name;
	}

	public void ShowCapitalMarker (bool show)
	{
		capitalMarker.SetActive(show);
	}
}