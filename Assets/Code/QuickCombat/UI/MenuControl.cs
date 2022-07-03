using System;
using System.Collections.Generic;
using System.Linq;
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
		// public NationPanel[] nationPanels;
		// public UnitsPanel    unitPanel;
		// public ItemsPanel    itemPanel;

		public Gizmo[] gizmoPrefabs;

		[NonSerialized] public List<Menu> menus;

		void Awake ()
		{
			menus = GetComponentsInChildren<Menu>(true).ToList();
		}

		public void Init ()
		{
			M.SetInstance(this);
		}
	}

	public static class M
	{
		static MenuControl menu;

		public static void SetInstance (MenuControl menuControl)
		{
			Debug.Assert(menu == null);
			menu = menuControl;
		}

		public static T ShowMenu<T> () where T : Menu
		{
			var popup = GetMenu<T>();
			popup.gameObject.SetActive(true);
			popup.transform.SetAsLastSibling();
			return popup;
		}

		public static void CloseMenu<T> () where T : Menu
		{
			var popup = GetMenu<T>();
			popup.gameObject.SetActive(false);
		}

		static T GetMenu<T> () where T : Menu
		{
			foreach (var popup in menu.menus)
			{
				if (popup is T p) return p;
			}
			throw new Exception($"Menu {typeof(T)} not found");
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
			throw new Exception($"Gizmo {typeof(T)} not found");
		}
	}




}