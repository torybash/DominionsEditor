using System.Linq;
using UnityEngine;

public class UiManager
{
	public Menu[] Menus { get; set; }
	public Gizmo[] GizmoTemplates { get; set; }

	public T GetMenu<T> () => Menus.OfType<T>().SingleOrDefault();

	public T Create<T> (Transform parent = null) where T : Component
	{
		var template = (Component) GizmoTemplates.OfType<T>().First();
		
		var instance = Object.Instantiate(template, parent != null ? parent : template.transform.parent);
		if (instance is Gizmo gizmo && template is Gizmo templateGizmo)
		{
			gizmo.Init(templateGizmo.Man, templateGizmo.Ui, templateGizmo.Game);
		}
		instance.gameObject.SetActive(true);

		return (T)instance;
	}
}