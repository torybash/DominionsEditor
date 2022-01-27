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

		var mapPath = Prefs.DataFolderPath.Get() + DefaultMapPath;
		mapPathField.text = mapPath;
	}
	
	private void OnClickButton ()
	{
		var mapFilePath = mapPathField.text;
		DomEdit.I.MapMan.LoadMap();
	}

}