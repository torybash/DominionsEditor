using System;
using System.Collections.Generic;
using Core;
using Data;
using Data.Tables;
using Dom;
using QuickCombat;
using UnityEngine;
using Utility.Extensions;

namespace UI.Menus.SearchMenu
{

	public class MagicsPanel : MonoBehaviour
	{
		[SerializeField] private SimpleButton       magicButtonTemplate;
		[SerializeField] private List<SimpleButton> xpButtons;
		
		private void Awake ()
		{
			magicButtonTemplate.gameObject.SetActive(false);
		}

		public void Initialize ()
		{
			// foreach (MagicPath magicPath in Enum.GetValues(typeof(MagicPath)))
			// {
			// 	var btn       = magicButtonTemplate.Copy();
			// 	var itemEntry = Tbl.Get<MagicPathTable>().GetEntry(magicPath);
			// 	btn.icon.sprite    = itemEntry.Sprite;
			// 	btn.button.onClick.AddListener(() => DomEdit.I.controls.SetActiveEntity(new MagicData(magicPath, itemEntry.Sprite)));
			// }
			//
			// xpButtons[0].button.onClick.AddListener(() => DomEdit.I.controls.SetActiveEntity(new ExperienceData(10)));
			// xpButtons[1].button.onClick.AddListener(() => DomEdit.I.controls.SetActiveEntity(new ExperienceData(100)));
			// xpButtons[2].button.onClick.AddListener(() => DomEdit.I.controls.SetActiveEntity(new ExperienceData(-100)));
			// xpButtons[3].button.onClick.AddListener(() => DomEdit.I.controls.SetActiveEntity(new ExperienceData(-10)));
		}
	}

}