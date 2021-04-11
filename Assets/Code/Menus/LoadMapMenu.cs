using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadMapMenu : Menu
{
	[SerializeField] private string defaultMapPath = @"C:\Users\toryb\AppData\Roaming\Dominions5\maps\Arena\Arena.map";
	
	[SerializeField] private TMP_InputField mapPathField;
	[SerializeField] private Button button;

	private void Awake ()
	{
		button.onClick.AddListener(OnClickButton);
		mapPathField.text = defaultMapPath;
	}
	
	private void OnClickButton ()
	{
		var mapFilePath = mapPathField.text;
		
		Man.ParseMap(mapFilePath);
	}
	


}