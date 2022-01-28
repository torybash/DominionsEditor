using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapMenu : Menu
{
	[SerializeField] private TMP_InputField   searchField;
	[SerializeField] private Toggle           unitsToggle;
	[SerializeField] private Toggle           itemsToggle;
	[SerializeField] private Toggle           magicToggle;
	[SerializeField] private Button           clearSearchButton;
	[SerializeField] private EntryGizmo       activeEntryGizmo;
	[SerializeField] private List<EntryGizmo> previousSelectedEntries;

	private readonly List<SearchResultGizmo> searchGizmos = new List<SearchResultGizmo>();

	private Searcher _searcher;

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

		_searcher = new Searcher();
	}


	private void OnToggleUnits (bool enable)
	{
		if (enable) _searcher.searchFilter |= SearchFilter.Monsters;
		else _searcher.searchFilter        &= ~SearchFilter.Monsters;
	}

	private void OnToggleItems (bool enable)
	{
		if (enable) _searcher.searchFilter |= SearchFilter.Items;
		else _searcher.searchFilter        &= ~SearchFilter.Items;
	}

	private void OnToggleMagic (bool enable)
	{
		if (enable) _searcher.searchFilter |= SearchFilter.Magic;
		else _searcher.searchFilter        &= ~SearchFilter.Magic;
	}

	public override void Show ()
	{
		base.Show();

		// var mapTex = Man.MapElements.OfType<ImageFile>().Single().GetTexture(); //TODO Fix TGA loading
		// mapImage.texture = mapTex;

		CreateProvinceGizmos();
	}

	private void CreateProvinceGizmos ()
	{
		var mapPicture = DomEdit.I.Ui.Get<MapPicture>();
		foreach (var province in DomEdit.I.MapMan.Map.ProvinceMap.Values)
		{
			var gizmo = DomEdit.I.Ui.Create<ProvinceGizmo>(mapPicture.MapImage.transform);
			gizmo.Initialize(province);
		}
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
}