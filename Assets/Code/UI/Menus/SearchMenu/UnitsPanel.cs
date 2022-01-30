using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitsPanel : MonoBehaviour
{
	[SerializeField] private TMP_Dropdown   nationDropdown;
	[SerializeField] private TMP_Dropdown   unitTypeDropdown;
	[SerializeField] private TMP_InputField searchField;
	[SerializeField] private Button         clearSearchButton;

	private readonly List<UnitSearchGizmo> searchGizmos = new List<UnitSearchGizmo>();

	// private Searcher _searcher;

	private void Awake ()
	{
		// _searcher = new Searcher();

		searchField.onValueChanged.AddListener(OnSearchChanged);
		clearSearchButton.onClick.AddListener(() => searchField.text = "");
	}

	private void OnSearchChanged (string searchText)
	{
		// var results = _searcher.Search(searchText);

		searchGizmos.SafeDestroy();

		clearSearchButton.gameObject.SetActive(!string.IsNullOrEmpty(searchText));

		// if (!string.IsNullOrEmpty(searchText)) CreateSearchGizmos(searchText, results);
	}

	public void Initialize ()
	{
		AddNationOptions();
		AddUnitTypeOptions();
	}

	private void CreateSearchGizmos (string searchText, List<SearchableEntry> results)
	{
		// foreach (var searchResult in results)
		// {
		// 	var searchResultGizmo = DomEdit.I.Ui.Create<UnitSearchGizmo>();
		// 	searchResultGizmo.Initialize(searchResult, searchText);
		// 	searchResultGizmo.Selected += OnSelected;
		// 	searchGizmos.Add(searchResultGizmo);
		// }
	}
	
	private void AddNationOptions ()
	{
		var nationOptions = new List<TMP_Dropdown.OptionData>();
		nationOptions.Add(new TMP_Dropdown.OptionData("Any nation"));
		foreach (var nation in DomEdit.I.nations.GetAll())
		{
			nationOptions.Add(new TMP_Dropdown.OptionData(nation.name + " - " + nation.epithet));
		}
		nationDropdown.options = nationOptions;
	}

	private void AddUnitTypeOptions ()
	{
		var nationOptions = new List<TMP_Dropdown.OptionData>();
		foreach (var o in Enum.GetValues(typeof(UnitTypeOption)))
		{
			nationOptions.Add(new TMP_Dropdown.OptionData(o.ToString()));
		}
		unitTypeDropdown.options = nationOptions;
	}
}

public enum UnitTypeOption
{
	AllUnits,
	Available,
	Recruitable,
	Commanders,
	Units,
	Summoned,
	Pretenders,
	Heroes
}