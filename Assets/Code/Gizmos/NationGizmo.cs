using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NationGizmo : Gizmo
{
	[Header("Components")]
	[SerializeField] private Image flagImage;
	[SerializeField] private TMP_Text nameLabel;
	[SerializeField] private GameObject capitalMarker;

	[Header("Assets")]
	[SerializeField] private Sprite blankSprite;
	
	public Nation Nation { get; private set; }

	private void Awake ()
	{
		ShowCapitalMarker(false);
	}
	
	public void SetNation (Nation nation)
	{
		Nation = nation;
		
		var nationEntry = Map.Nations.GetNationEntry(nation);
		if (nationEntry == null)
		{
			flagImage.sprite = blankSprite;
			nameLabel.text = "";
		} 
		else
		{
			flagImage.sprite = nationEntry.Sprite;
			nameLabel.text = nationEntry.Name;
		}
	}

	public void ShowCapitalMarker (bool show)
	{
		capitalMarker.SetActive(show);
	}
}