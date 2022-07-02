using System;
using System.Collections.Generic;
using Core;
using Data;
using QuickCombat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility.Extensions;

namespace UI.Menus.SearchMenu
{

	public class ItemsPanel : MonoBehaviour
	{
		[SerializeField] private TMP_InputField searchField;
		[SerializeField] private Button         clearSearchButton;

		private readonly List<ItemSearchGizmo> searchGizmos = new List<ItemSearchGizmo>();

		private void Awake ()
		{
			searchField.onValueChanged.AddListener(OnSearchChanged);
			clearSearchButton.onClick.AddListener(() => searchField.text = "");
		}

		public void Initialize ()
		{

		}

		private void OnSearchChanged (string searchText)
		{
			var results = Search(searchText);

			clearSearchButton.gameObject.SetActive(!string.IsNullOrEmpty(searchText));

			searchGizmos.SafeDestroyList();
			if (!string.IsNullOrEmpty(searchText)) CreateSearchGizmos(searchText, results);
		}

		private List<ItemData> Search (string searchText)
		{
			var foundEntries = new List<ItemData>();
			foreach (var entry in D.Items.GetAll())
			{
				if (entry.name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) != -1)
				{
					foundEntries.Add(entry);
					if (foundEntries.Count >= Searcher.MAX_RESULTS) break;
				}
			}

			return foundEntries;
		}

		private void CreateSearchGizmos (string searchText, List<ItemData> results)
		{
			foreach (var itemData in results)
			{
				var searchResultGizmo = DomEdit.I.Ui.Create<ItemSearchGizmo>();
				searchResultGizmo.Initialize(itemData);
				searchResultGizmo.Selected += OnSelected;
				searchResultGizmo.GetComponent<SearchResultGizmo>().Initialize($"{itemData.name} ({itemData.unitType})", itemData.icon, searchText);
				searchGizmos.Add(searchResultGizmo);
			}
		}

		private void OnSelected (ItemData itemData)
		{
			Debug.Log($"OnSelected {itemData}");
			
			DomEdit.I.controls.SetActiveEntity(itemData);

		}
	}

}