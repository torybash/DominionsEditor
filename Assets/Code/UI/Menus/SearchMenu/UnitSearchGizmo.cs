using UnityEngine.EventSystems;

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