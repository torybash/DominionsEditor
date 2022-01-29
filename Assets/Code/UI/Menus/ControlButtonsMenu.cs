using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ControlButtonsMenu : Menu
{
	[SerializeField] private Button runButton;
	[SerializeField] private Button saveMapButton;
	[SerializeField] private Button loadMapButton;
	[SerializeField] private Button settingsButton;

	private void Awake ()
	{
		saveMapButton.onClick.AddListener(OnSaveClicked);
		loadMapButton.onClick.AddListener(OnLoadClicked);
		runButton.onClick.AddListener(OnRunClicked);
		settingsButton.onClick.AddListener(OnSettingsClicked);
	}
	private void OnLoadClicked ()
	{
		DomEdit.I.Ui.Get<LoadMapMenu>().SelectMap(file =>
		{
			DomEdit.I.MapMan.LoadMap(file);
		});
	}

	public void OnSettingsClicked ()
	{
		DomEdit.I.Ui.Get<IntroMenu>().Show();
	}
	
	private void OnRunClicked ()
	{
		if (DomEdit.I.MapMan.Map.Players.Any(x => string.IsNullOrEmpty(x.Pretender.filePath)))
		{
			DomEdit.I.Ui.Create<MessageGizmo>().Write("Player missing pretender reference");
			return;
		}

		var mapRunner = new DomRunner(DomEdit.I.MapMan);
		mapRunner.Run();
	}

	private void OnSaveClicked ()
	{
		DomEdit.I.MapMan.SaveMap();
	}
}
