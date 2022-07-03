using Data;
using QuickCombat.Gizmos;
using UnityEngine;
using Utility.Extensions;

namespace QuickCombat
{
	public class NationPanel : MonoBehaviour
	{
		public RectTransform commanderGroup;
		
		NationGizmo _nationGizmo;

		public void SetPlayer (Player player)
		{
			_nationGizmo.SafeDestroy();
			_nationGizmo = M.CreateGizmo<NationGizmo>(transform);
			_nationGizmo.SetNation(player.nation);
		}
	}
}