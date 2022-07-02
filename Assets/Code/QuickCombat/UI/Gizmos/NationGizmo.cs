using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QuickCombat.Gizmos
{
	public class NationGizmo : Gizmo
	{
		[Header("Components")]
		[SerializeField] Image flagImage;
		[SerializeField] Image    blankImage;
		[SerializeField] TMP_Text nameText;
		
		public void SetNation (Nation nation, bool showName = true)
		{
			flagImage.gameObject.SetActive(nation != null);
			blankImage.gameObject.SetActive(nation == null);

			if (nation == null)
			{
				nameText.text = "";
			} else
			{
				flagImage.sprite = nation.icon;
				nameText.text    = showName ? nation.name : "";
			}
		}
	}
}