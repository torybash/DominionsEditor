using Core;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI.Menus
{

	public class MainMenu : Menu
	{
		[SerializeField] Button loadPreviousMap;
		[SerializeField] Button loadMap;
		[SerializeField] Button settings;
		[SerializeField] Button quit;

		void Awake ()
		{
			loadPreviousMap.onClick.AddListener(() =>
			{
				var mapFile = DomFile.LoadPath(Paths.MapFilePath);
				DomEdit.I.MapMan.LoadMap(mapFile);
				Hide();
			});
			loadMap.onClick.AddListener(() =>
			{
				DomEdit.I.Ui.Get<LoadMapMenu>().SelectMap(file =>
				{
					DomEdit.I.MapMan.LoadMap(file);
					Hide();
				});
			});
			settings.onClick.AddListener(() =>
			{
				DomEdit.I.Ui.Get<SettingsMenu>().Show();
			});
			quit.onClick.AddListener(() =>
			{
				Application.Quit();
			});
		}

		public override void Show ()
		{
			base.Show();

			bool hasPreviousMap = !string.IsNullOrEmpty(Prefs.PreviousMapPath.Get());
			loadPreviousMap.gameObject.SetActive(hasPreviousMap);
		}
	}

}