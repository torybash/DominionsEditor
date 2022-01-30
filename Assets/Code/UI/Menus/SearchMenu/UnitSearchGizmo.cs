using UI.Gizmos;
using UnityEngine.EventSystems;

namespace UI.Menus.SearchMenu
{

	public class UnitSearchGizmo : Gizmo, IPointerDownHandler
	{
		// public event Action<SearchableEntry> Selected;
		//
		// private void OnClicked ()
		// {
		// 	Selected?.Invoke(_searchable);
		// }
		void IPointerDownHandler.OnPointerDown (PointerEventData eventData)
		{
			// Selected?.Invoke(_searchable);
		}
	}

}