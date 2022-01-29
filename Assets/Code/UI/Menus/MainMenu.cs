using UnityEngine;
using UnityEngine.UI;

public class MainMenu : Menu
{
	[SerializeField] private Button loadPreviousMap;
	[SerializeField] private Button loadMap;
	[SerializeField] private Button settings;
	[SerializeField] private Button quit;

	private void Awake ()
	{
		loadPreviousMap.onClick.AddListener(() =>
		{
			var mapFile = MapFile.LoadPath(Prefs.PreviousMapPath.Get());
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