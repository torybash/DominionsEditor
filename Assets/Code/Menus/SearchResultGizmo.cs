using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SearchResultGizmo : Gizmo
{
	[SerializeField] private TMP_Text nameLabel;
	[SerializeField] private Image spritePicture;
	[SerializeField] private Button selectButton;
	
	private SearchableEntry _searchable;

	private void Awake ()
	{
		selectButton.onClick.AddListener(OnClicked);
	}
	
	private void OnClicked ()
	{
		Ui.Get<MapMenu>().SetSelected(_searchable);
	}

	public void Initialize (SearchableEntry searchable, string searchText)
	{
		_searchable = searchable;
		var nameText = searchable.Name;
		int startIdx = nameText.IndexOf(searchText, StringComparison.OrdinalIgnoreCase);
		int endIdx = startIdx + searchText.Length;
		nameText = nameText.Insert(endIdx, "</b>");
		nameText = nameText.Insert(startIdx, "<b>");
		nameLabel.text = nameText;
		spritePicture.sprite = searchable.Sprite;
	}
}