using UnityEngine;
using UnityEngine.UI;

namespace QuickCombat
{
	public class UnitFilterButton : MonoBehaviour
	{
		public Filter     filter;
		public Button     button;
		public GameObject selectedHighlight;

		public enum Filter { Nation, Favorites, All }
	}
}