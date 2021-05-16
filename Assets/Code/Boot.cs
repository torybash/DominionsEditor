using UnityEngine;

public class Boot : MonoBehaviour
{
	[SerializeField] private MonstersTable monsters;
	[SerializeField] private ItemsTable items;
	[SerializeField] private NationsTable nations;
	
	private void Awake ()
	{
		var searcher = new Searcher();
		var ui = new UiManager();
		var gameSetup = new GameSetup(ui);
		var map = new MapManager(searcher, ui, gameSetup);
		
		var canvas = FindObjectOfType<Canvas>();
		
		var menus = canvas.GetComponentsInChildren<Menu>(true);
		foreach (var menu in menus)
		{
			menu.Initialize(map, ui, gameSetup);
			menu.gameObject.SetActive(false);
		}
		
		var gizmoTemplates = canvas.GetComponentsInChildren<Gizmo>(true);
		foreach (var gizmoTemplate in gizmoTemplates)
		{
			gizmoTemplate.Init(map, ui, gameSetup);
			gizmoTemplate.gameObject.SetActive(false);
		}

		map.Monsters = monsters;
		map.Items = items;
		map.Nations = nations;
		ui.GizmoTemplates = gizmoTemplates;
		ui.Menus = menus;
		searcher.Monsters = monsters;
		searcher.Items = items;

		ui.Get<ControlButtonsMenu>().Show();
		
		if (IsSetup())
		{
			map.LoadMap();
		} 
		else
		{
			ui.Get<IntroMenu>().Show();
		}
	}
	
	private bool IsSetup ()
	{
		var pretenderA = Prefs.DefaultPretenderA.Get();
		var pretenderB = Prefs.DefaultPretenderB.Get();
		if (string.IsNullOrEmpty(pretenderA)) return false;
		if (string.IsNullOrEmpty(pretenderB)) return false;

		return true;
	}
}
