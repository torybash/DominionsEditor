using System;
using Data;
using UI.Gizmos;
using UnityEngine.EventSystems;

namespace UI.Menus.SearchMenu
{

	public class UnitSearchGizmo : Gizmo, IPointerDownHandler
	{
		public event Action<UnitData> Selected;

		private UnitData _unitData;

		void IPointerDownHandler.OnPointerDown (PointerEventData eventData)
		{
			Selected?.Invoke(_unitData);
		}

		public void Initialize (UnitData unitData)
		{
			_unitData = unitData;
		}
	}


}