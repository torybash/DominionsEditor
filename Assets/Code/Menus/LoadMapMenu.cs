using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadMapMenu : Menu
{
	
	private const string DefaultMapPath = @"maps\Arena\Arena.map";
	
	[SerializeField] private TMP_InputField mapPathField;
	[SerializeField] private Button button;

	private void Awake ()
	{
		button.onClick.AddListener(OnClickButton);

		var mapPath = PrefManager.DataFolderPath.Get() + DefaultMapPath;
		mapPathField.text = mapPath;
	}
	
	private void OnClickButton ()
	{
		var mapFilePath = mapPathField.text;
		
		Map.ParseMap(mapFilePath);
		
		Ui.Get<LoadMapMenu>().Hide();
		Ui.Get<MapMenu>().Show();
		Ui.Get<PlayersMenu>().Show();

		Game.Initialize();
	}

}