using System.Collections.Generic;
using Core;
using Data.Entries;
using TMPro;
using UI.Gizmos;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menus.SearchMenu
{

	public class SearchMenu : Menu
	{
		[SerializeField] private TMP_InputField searchField;

		[SerializeField] private Button           clearSearchButton;
		[SerializeField] private EntryGizmo       activeEntryGizmo;
		[SerializeField] private List<EntryGizmo> previousSelectedEntries;

		[Header("Tabs")]
		[SerializeField] private Toggle unitsToggle;
		[SerializeField] private Toggle itemsToggle;
		[SerializeField] private Toggle magicToggle;
	
		[Header("Panels")]
		[SerializeField] private UnitsPanel unitsPanel;
		[SerializeField] private ItemsPanel  itemsPanel;
		[SerializeField] private MagicsPanel magicsPanel;

		private readonly List<SearchResultGizmo> searchGizmos = new List<SearchResultGizmo>();

		private Searcher _searcher;
		private Tabs     _activeTab;

		public SearchableEntry ActiveEntry { get; private set; }

		private void Awake ()
		{
			searchField.onValueChanged.AddListener(OnSearchChanged);
		
			unitsToggle.onValueChanged.AddListener(OnToggleUnits);
			itemsToggle.onValueChanged.AddListener(OnToggleItems);
			magicToggle.onValueChanged.AddListener(OnToggleMagic);
		
			clearSearchButton.onClick.AddListener(() => searchField.text = "");
			activeEntryGizmo.gameObject.SetActive(false);
			foreach (var previousSelectedEntry in previousSelectedEntries)
			{
				previousSelectedEntry.gameObject.SetActive(false);
			}
		}

		private void Start ()
		{
			_searcher = new Searcher();
			unitsPanel.Initialize();
			itemsPanel.Initialize();
			magicsPanel.Initialize();
		}

		private void OnToggleUnits (bool enable)
		{
			if (enable) _searcher.searchFilter |= SearchFilter.Monsters;
			else _searcher.searchFilter        &= ~SearchFilter.Monsters;
		
			_activeTab = Tabs.Units;
			UpdateTabPanels();
		}

		private void OnToggleItems (bool enable)
		{
			if (enable) _searcher.searchFilter |= SearchFilter.Items;
			else _searcher.searchFilter        &= ~SearchFilter.Items;
		
			_activeTab = Tabs.Items;
			UpdateTabPanels();
		}

		private void OnToggleMagic (bool enable)
		{
			if (enable) _searcher.searchFilter |= SearchFilter.Magic;
			else _searcher.searchFilter        &= ~SearchFilter.Magic;
		
			_activeTab = Tabs.Magic;
			UpdateTabPanels();
		}

		private void OnSearchChanged (string searchText)
		{
			var results = _searcher.Search(searchText);

			foreach (var searchGizmo in searchGizmos) Destroy(searchGizmo.gameObject);
			searchGizmos.Clear();

			clearSearchButton.gameObject.SetActive(!string.IsNullOrEmpty(searchText));

			if (!string.IsNullOrEmpty(searchText)) CreateSearchGizmos(searchText, results);
		}

		private void CreateSearchGizmos (string searchText, List<SearchableEntry> results)
		{
			foreach (var searchResult in results)
			{
				var searchResultGizmo = DomEdit.I.Ui.Create<SearchResultGizmo>();
				searchResultGizmo.Initialize(searchResult, searchText);
				searchResultGizmo.Selected += OnSelected;
				searchGizmos.Add(searchResultGizmo);
			}
		}

		private void OnSelected (SearchableEntry obj)
		{
			ActiveEntry = obj;

			activeEntryGizmo.gameObject.SetActive(true);
			activeEntryGizmo.SetEntry(obj);
		}
	
		private void UpdateTabPanels ()
		{
			unitsPanel.gameObject.SetActive(_activeTab  == Tabs.Units);
			itemsPanel.gameObject.SetActive(_activeTab  == Tabs.Items);
			magicsPanel.gameObject.SetActive(_activeTab == Tabs.Magic);
		}
	
		public enum Tabs {Units, Items, Magic}
	}

}