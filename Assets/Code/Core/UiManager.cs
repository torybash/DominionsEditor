using System.Linq;
using UI.Gizmos;
using UI.Menus;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{

	public class UiManager
	{
		private readonly Menu[]  menus;
		private readonly Gizmo[] gizmoTemplates;
	
		public Canvas           Canvas    { get; }
		public GraphicRaycaster Raycaster { get; }

		public UiManager ()
		{
			Canvas    = Object.FindObjectOfType<Canvas>();
			Raycaster = Canvas.GetComponent<GraphicRaycaster>();

			menus = Canvas.GetComponentsInChildren<Menu>(true);
			foreach (var menu in menus)
			{
				menu.gameObject.SetActive(false);
			}

			gizmoTemplates = Canvas.GetComponentsInChildren<Gizmo>(true);
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

}