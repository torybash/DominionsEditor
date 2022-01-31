using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gizmos
{

	public class NationGizmo : Gizmo
	{
		[Header("Components")]
		[SerializeField] private Image flagImage;
		[SerializeField] private TMP_Text   nameLabel;
		[SerializeField] private GameObject capitalMarker;

		[Header("Assets")]
		[SerializeField] private Sprite blankSprite;
	
		public Nation Nation { get; private set; }

		private void Awake ()
		{
			ShowCapitalMarker(false);
		}
	
		public void SetNation (Nation nation, bool showName = true)
		{
			Nation = nation;
		
			if (nation == null)
			{
				flagImage.sprite = blankSprite;
				nameLabel.text   = "";
			} 
			else
			{
				flagImage.sprite = nation.icon;
				nameLabel.text   = showName ? nation.name : "";
			}
		}

		public void ShowCapitalMarker (bool show)
		{
			capitalMarker.SetActive(show);
		}
	}

}