using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapMenu : Menu
{
	[SerializeField] private TMP_InputField   searchField;
	[SerializeField] private Button           clearSearchButton;
	[SerializeField] private Button           saveMapButton;
	[SerializeField] private Button           runButton;
	[SerializeField] private EntryGizmo       activeEntryGizmo;
	[SerializeField] private List<EntryGizmo> previousSelectedEntries;

	private readonly List<SearchResultGizmo> searchGizmos = new List<SearchResultGizmo>();

	private Searcher _searcher;

	public SearchableEntry SelectedEntry { get; private set; }

	private void Awake ()
	{
		searchField.onValueChanged.AddListener(OnSearchChanged);
		clearSearchButton.onClick.AddListener(() => searchField.text = "");
		saveMapButton.onClick.AddListener(OnSaveClicked);
		runButton.onClick.AddListener(OnRunClicked);
		activeEntryGizmo.gameObject.SetActive(false);
		foreach (var previousSelectedEntry in previousSelectedEntries)
		{
			previousSelectedEntry.gameObject.SetActive(false);
		}

		_searcher = new Searcher();
	}

	private void OnRunClicked ()
	{
		if (DomEdit.I.PlayerSetup.Players.Any(x => string.IsNullOrEmpty(x.Pretender.filePath)))
		{
			DomEdit.I.Ui.Create<MessageGizmo>().Write("Player missing pretender reference");
			return;
		}

		var mapRunner = new DomRunner(DomEdit.I.MapMan, DomEdit.I.PlayerSetup);
		mapRunner.Run();
	}

	private void OnSaveClicked ()
	{
		DomEdit.I.MapMan.SaveMap();
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
		SelectedEntry = obj;

		activeEntryGizmo.gameObject.SetActive(true);
		activeEntryGizmo.SetEntry(obj);
	}
}