using System.Collections.Generic;
using Core;
using Core.Entities;
using UnityEngine;

namespace QuickCombat
{
	public class UnitsPanel : MonoBehaviour
	{
		//TODO Add toggle: Show in list
		//TODO Add toggle: Show unknown types (switches off commander/units, and forces player to manually select what is commander)
		//TODO Add toggle(s): Show name, nation 
		
		public List<UnitFilterButton> filterButtons;

		public RectTransform commanderGroup;
		public RectTransform troopsGroup;

	}

}