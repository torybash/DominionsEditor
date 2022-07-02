using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using QuickCombat.Gizmos;
using QuickCombat.UI.Popups;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QuickCombat
{
	/// <summary>
	/// References to all UI panels and gizmos
	/// Only has logic for spawning gizmos, toggling panels
	/// </summary>
	public class MenuControl : MonoBehaviour
	{
		public NationPanel[] nationPanels;
		public UnitsPanel    unitPanel;
		public ItemsPanel    itemPanel;
		
		public Gizmo[]    gizmoPrefabs;

		[NonSerialized] public List<Popup> popups;

		void Awake ()
		{
			popups = GetComponentsInChildren<Popup>(true).ToList();
		}

		public void Init ()
		{
			M.SetInstance(this);
		}
	}

	public static class M
	{
		static MenuControl menu;

		public static NationPanel[] NationPanels => menu.nationPanels;
		public static UnitsPanel    UnitPanel    => menu.unitPanel;
		public static ItemsPanel    ItemPanel    => menu.itemPanel;

		public static NationPanel GetNationPanel (PlayerSide side)
		{
			return menu.nationPanels[(int)side];
		}

		public static void SetInstance (MenuControl menuControl)
		{
			Debug.Assert(menu == null);
			menu = menuControl;
		}

		public static T ShowPopup<T> () where T : Popup
		{
			var popup = GetPopup<T>();
			popup.gameObject.SetActive(true);
			popup.transform.SetAsLastSibling();
			return popup;
		}

		public static void ClosePopup<T> () where T : Popup
		{
			var popup = GetPopup<T>();
			popup.gameObject.SetActive(false);
		}

		static T GetPopup<T> () where T : Popup
		{
			foreach (var popup in menu.popups)
			{
				if (popup is T p) return p;
			}
			throw new Exception($"Popup {typeof(T)} not found");
		}
		
		public static T CreateGizmo<T> (Transform parent) where T : Gizmo
		{
			var gizmoPrefab = GetGizmoPrefab<T>();
			var gizmo       = Object.Instantiate(gizmoPrefab, parent);
			return gizmo;
		}

		static T GetGizmoPrefab<T> () where T : Gizmo
		{
			foreach (var gizmoPrefab in menu.gizmoPrefabs)
			{
				if (gizmoPrefab is T p) return p;
			}
			return null;
		}
	}




}