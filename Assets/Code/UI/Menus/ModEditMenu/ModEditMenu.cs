using UnityEngine;
using UnityEngine.UI;
using Utility.Extensions;

namespace UI.Menus
{

	public class ModEditMenu : Menu
	{
		[SerializeField] SchoolSetting schoolSetting;
		[SerializeField] Button        closeButton;

		void Reset ()
		{
			schoolSetting = GetComponentInChildren<SchoolSetting>();
		}

		void Awake ()
		{
			schoolSetting.gameObject.SetActive(false);
			for (int i = 0; i < 8; i++)
			{
				schoolSetting.Copy().Initialize(MagicSchool.All[i]);
			}
			
			closeButton.onClick.AddListener(Hide);
		}

		public override void Show ()
		{
			base.Show();
			
			
		}
	}

}