using System;
using System.Collections.Generic;
using Core;
using Data;
using Data.Entries;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility.Extensions;

namespace UI.Menus.SearchMenu
{

	public class UnitsPanel : MonoBehaviour
	{
		[SerializeField] private TMP_Dropdown   nationDropdown;
		[SerializeField] private TMP_Dropdown   unitTypeDropdown;
		[SerializeField] private TMP_InputField searchField;
		[SerializeField] private Button         clearSearchButton;

		private readonly List<UnitSearchGizmo> searchGizmos = new List<UnitSearchGizmo>();

		private Searcher _searcher;

		private void Awake ()
		{

			searchField.onValueChanged.AddListener(OnSearchChanged);
			clearSearchButton.onClick.AddListener(() => searchField.text = "");
		}


		public void Initialize ()
		{
			AddNationOptions();
			AddUnitTypeOptions();
			
			// _searcher = new Searcher(DomEdit.I.Units.GetAll());
		}

		private void OnSearchChanged (string searchText)
		{
			var results = Search(searchText);
			
			clearSearchButton.gameObject.SetActive(!string.IsNullOrEmpty(searchText));

			searchGizmos.SafeDestroy();
			if (!string.IsNullOrEmpty(searchText)) CreateSearchGizmos(searchText, results);
		}

		private List<Unit> Search (string searchText)
		{

			var foundEntries      = new List<Unit>();
			// var entriesToSearchIn = new List<SearchableEntry>();
			// if (searchFilter.HasFlag(SearchFilter.Monsters)) entriesToSearchIn.AddRange(DomEdit.I.monsters.Entries);
			// if (searchFilter.HasFlag(SearchFilter.Items)) entriesToSearchIn.AddRange(DomEdit.I.items.Entries);
			// if (searchFilter.HasFlag(SearchFilter.Magic)) entriesToSearchIn.AddRange(DomEdit.I.magicPaths.magics);
	
			foreach (var entry in DomEdit.I.Units.GetAll())
			{
				if (entry.name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) != -1)
				{
					foundEntries.Add(entry);
					if (foundEntries.Count >= Searcher.MAX_RESULTS) break;
				}
			}
	
			return foundEntries;
		}

		private void CreateSearchGizmos (string searchText, List<Unit> results)
		{
			foreach (var searchResult in results)
			{
				var searchResultGizmo = DomEdit.I.Ui.Create<UnitSearchGizmo>();
				searchResultGizmo.Initialize($"{searchResult.name} ({searchResult.unitType})", searchResult.icon, searchText);
				searchResultGizmo.Selected += OnSelected;
				searchGizmos.Add(searchResultGizmo);
			}
		}

		private void OnSelected (string obj)
		{
			Debug.Log($"OnSelected {obj}");
		}

		private void AddNationOptions ()
		{
			var nationOptions = new List<TMP_Dropdown.OptionData>();
			nationOptions.Add(new TMP_Dropdown.OptionData("Any nation"));
			foreach (var nation in DomEdit.I.Nations.GetAll())
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

}