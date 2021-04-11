using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SearchResultGizmo : Gizmo
{
	[SerializeField] private TMP_Text nameLabel;
	[SerializeField] private Image spritePicture;
	[SerializeField] private Button selectButton;
	private MonstersTable.Entry _monster;

	private void Awake ()
	{
		selectButton.onClick.AddListener(OnClicked);
	}
	
	private void OnClicked ()
	{
		Ui.GetMenu<MapMenu>().SetSelected(_monster);
	}

	public void Initialize (MonstersTable.Entry monster, string searchText)
	{
		_monster = monster;
		var nameText = monster.Name;
		int startIdx = nameText.IndexOf(searchText, StringComparison.OrdinalIgnoreCase);
		int endIdx = startIdx + searchText.Length;
		nameText = nameText.Insert(endIdx, "</b>");
		nameText = nameText.Insert(startIdx, "<b>");
		nameLabel.text = nameText;
		spritePicture.sprite = monster.Sprite;
	}
}