using System;
using Data.Entries;
using TMPro;
using UI.Gizmos;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Menus.SearchMenu
{

	public class SearchResultGizmo : Gizmo, IPointerDownHandler
	{
		public event Action<string> Selected;

		[SerializeField] private TMP_Text nameLabel;
		[SerializeField] private Image    spritePicture;

		private string _entryName;
		

		void IPointerDownHandler.OnPointerDown (PointerEventData eventData)
		{
			Selected?.Invoke(_entryName);
		}

		public void Initialize (string entryName, Sprite sprite, string searchText)
		{
			_entryName = entryName;

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