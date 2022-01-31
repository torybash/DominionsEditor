using System;
using Data;
using UI.Gizmos;
using UnityEngine.EventSystems;

namespace UI.Menus.SearchMenu
{

	public class ItemSearchGizmo : Gizmo, IPointerDownHandler
	{
		public event Action<ItemData> Selected;

		private ItemData _itemData;

		void IPointerDownHandler.OnPointerDown (PointerEventData eventData)
		{
			Selected?.Invoke(_itemData);
		}

		public void Initialize (ItemData itemData)
		{
			_itemData = itemData;
		}
	}

}