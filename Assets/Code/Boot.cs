using System.Linq;
using UnityEngine;

public class Boot : MonoBehaviour
{
	[SerializeField] private MonstersTable monsters;
	[SerializeField] private NationsTable nations;
	private void Awake ()
	{
		var searcher = new Searcher();
		var mapMan = new MapManager(searcher);
		var uiMan = new UiManager();
		var gameSetup = new GameSetup(mapMan, uiMan);
		
		var canvas = FindObjectOfType<Canvas>();
		
		var menus = canvas.GetComponentsInChildren<Menu>(true);
		foreach (var menu in menus) menu.Initialize(mapMan, uiMan, gameSetup);
		var gizmoTemplates = canvas.GetComponentsInChildren<Gizmo>(true);
		foreach (var gizmoTemplate in gizmoTemplates)
		{
			gizmoTemplate.Init(mapMan, uiMan, gameSetup);
			gizmoTemplate.gameObject.SetActive(false);
		}

		mapMan.Monsters = monsters;
		mapMan.Nations = nations;
		uiMan.GizmoTemplates = gizmoTemplates;
		uiMan.Menus = menus;
		searcher.Monsters = monsters;
		
		menus.OfType<LoadMapMenu>().Single().Show();
		menus.OfType<ControlButtonsMenu>().Single().Show();
	}
}
