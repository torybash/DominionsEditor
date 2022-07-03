using Data.Tables;
using UnityEngine;
using UnityEngine.UI;

namespace QuickCombat.Gizmos
{
	public class UnitGizmo : Gizmo
	{
		public const float DEFAULT_HEIGHT   = 30f;
		public const float EXTRA_ROW_HEIGHT = 30f;

		[SerializeField] Image icon;

		public Button button;

		public void SetData (int monsterId)
		{
			icon.sprite = Tbl.Get<IconsTable>().GetUnitIcon(monsterId);
		}
	}

}