using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : Menu
{
	
	[SerializeField] private TMP_InputField executablePathField;
	[SerializeField] private TMP_InputField dataPathField;
	[SerializeField] private Button closeButton;

	private void Awake ()
	{
		executablePathField.onValueChanged.AddListener(OnExecutablePathChanged); 
		dataPathField.onValueChanged.AddListener(OnDataPathChanged); 
		closeButton.onClick.AddListener(Hide); 
	}

	private void Start ()
	{
		executablePathField.text = Prefs.ExecutablePath.Get();
		dataPathField.text = Prefs.DataFolderPath.Get();
	}

	public override void Show ()
	{
		base.Show();
		
		transform.SetAsLastSibling();
	}

	private void OnExecutablePathChanged (string arg0)
	{
		Prefs.ExecutablePath.Set(arg0);
	}
	
	private void OnDataPathChanged (string arg0)
	{
		Prefs.DataFolderPath.Set(arg0);
	}

}
