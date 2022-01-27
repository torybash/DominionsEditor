using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UiManager
{
	private readonly Menu[]  menus;
	private readonly Gizmo[] gizmoTemplates;
	
	public GraphicRaycaster Raycaster      { get; }

	public UiManager ()
	{
		var canvas = Object.FindObjectOfType<Canvas>();
		Raycaster = canvas.GetComponent<GraphicRaycaster>();

		menus = canvas.GetComponentsInChildren<Menu>(true);
		foreach (var menu in menus)
		{
			menu.gameObject.SetActive(false);
		}

		gizmoTemplates = canvas.GetComponentsInChildren<Gizmo>(true);
		foreach (var gizmoTemplate in gizmoTemplates)
		{
			gizmoTemplate.gameObject.SetActive(false);
		}
	}

	public T Get<T> () where T : Menu => menus.OfType<T>().SingleOrDefault();

	public T Create<T> (Transform parent = null) where T : Gizmo
	{
		var template = (Component)gizmoTemplates.OfType<T>().First();

		var instance = Object.Instantiate(template, parent != null ? parent : template.transform.parent);
		instance.gameObject.SetActive(true);

		return (T)instance;
	}
}