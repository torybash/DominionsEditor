using Data;
using QuickCombat.Gizmos;
using UnityEngine;
using UnityEngine.UI;
using Utility.Extensions;

namespace QuickCombat.UI.Popups
{
	public class NationSelector : MonoBehaviour
	{
		public Button loadButton;

		NationGizmo _nationGizmo;

		public void SetNation (Nation nation)
		{
			_nationGizmo.SafeDestroy();
			_nationGizmo = M.CreateGizmo<NationGizmo>(transform);
			_nationGizmo.SetNation(nation);
		}
	}
}