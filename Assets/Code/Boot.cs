using UnityEngine;

public class Boot : MonoBehaviour
{
	[SerializeField] private MonstersTable monsters;
	[SerializeField] private NationsTable nations;
	private void Awake ()
	{
		var searcher = new Searcher();
		var mapMan = new MapManager(searcher);

		var menus = FindObjectsOfType<Menu>();
		foreach (var menu in menus) menu.Initialize(mapMan);
		var gizmoTemplates = FindObjectsOfType<Gizmo>();
		foreach (var gizmoTemplate in gizmoTemplates) gizmoTemplate.InitializeTemplate(mapMan);

		mapMan.Monsters = monsters;
		mapMan.Nations = nations;
		mapMan.GizmoTemplates = gizmoTemplates;
		mapMan.Menus = menus;
		searcher.Monsters = monsters;
	}
}
