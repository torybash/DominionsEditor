using System.Collections.Generic;
using Core;
using Core.Entities;
using Data.Entries;
using UI.Gizmos;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menus.SearchMenu
{

	public class SearchMenu : Menu
	{
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

		// private EntityType _activeTab;

		private void Awake ()
		{

			unitsToggle.onValueChanged.AddListener(OnToggleUnits);
			itemsToggle.onValueChanged.AddListener(OnToggleItems);
			magicToggle.onValueChanged.AddListener(OnToggleMagic);

			activeEntryGizmo.gameObject.SetActive(false);
			foreach (var previousSelectedEntry in previousSelectedEntries)
			{
				previousSelectedEntry.gameObject.SetActive(false);
			}
		}

		private void Start ()
		{
			unitsPanel.Initialize();
			itemsPanel.Initialize();
			magicsPanel.Initialize();
		}

		private void OnToggleUnits (bool enable)
		{
			// _activeTab = EntityType.Units;
			UpdateTabPanels();
		}

		private void OnToggleItems (bool enable)
		{
			// _activeTab = EntityType.Items;
			UpdateTabPanels();
		}

		private void OnToggleMagic (bool enable)
		{
			// _activeTab = EntityType.Magic;
			UpdateTabPanels();
		}

		private void UpdateTabPanels ()
		{
			// unitsPanel.gameObject.SetActive(_activeTab  == EntityType.Units);
			// itemsPanel.gameObject.SetActive(_activeTab  == EntityType.Items);
			// magicsPanel.gameObject.SetActive(_activeTab == EntityType.Magic);
		}

	}

}