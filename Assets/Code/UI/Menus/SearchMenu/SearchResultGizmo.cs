using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menus.SearchMenu
{

	public class SearchResultGizmo : MonoBehaviour
	{
		[SerializeField] private TMP_Text nameLabel;
		[SerializeField] private Image    spritePicture;
		
		public void Initialize (string entryName, Sprite sprite, string searchText)
		{
			var nameText = entryName;
			int startIdx = nameText.IndexOf(searchText, StringComparison.OrdinalIgnoreCase);
			int endIdx   = startIdx + searchText.Length;
			nameText = nameText.Insert(endIdx,   "</b>");
			nameText = nameText.Insert(startIdx, "<b>");

			nameLabel.text       = nameText;
			spritePicture.sprite = sprite;
		}
	}

}