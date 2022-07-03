using Data.Tables;
using UnityEngine;
using UnityEngine.UI;

namespace QuickCombat.Gizmos
{
	public class TroopGizmo : Gizmo
	{
		[SerializeField] Image icon;
		
		public void SetData (int monsterId)
		{
			icon.sprite = Tbl.Get<IconsTable>().GetUnitIcon(monsterId);;
		}
		
		// public override void SetData (Monster data)
		// {
		// 	base.SetData(data);
		//
		// 	nameLabel.text += $" ({Data.Amount})";
		// }

	}
}