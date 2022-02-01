using System.IO;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI.Menus
{

	public class IntroMenu : Menu
	{
		[Header("Step 1")]
		[SerializeField] private TMP_InputField executablePathField;
		[SerializeField] private TMP_InputField dataPathField;

		[Header("Step 2")]
		[SerializeField] private Button mapButton; 
		[SerializeField] private TMP_Text mapResultText; 
		[SerializeField] private Button   modButton; 
		[SerializeField] private TMP_Text modResultText; 
		
		[Header("Step 3")]
		[SerializeField] private Button addPretenderButton;
		[SerializeField] private Button   clearPretendersButton;
		[SerializeField] private TMP_Text addedPretendersText;

		[Header("Step 4")]
		[SerializeField] private Button startButton;
	
		[Header("Other")]
		[SerializeField] private Button quitButton;
		
		private void Awake ()
		{
			executablePathField.onValueChanged.AddListener(OnExecutablePathChanged); 
			dataPathField.onValueChanged.AddListener(OnDataPathChanged); 
			quitButton.onClick.AddListener(Application.Quit); 
			mapButton.onClick.AddListener(OnCreateMap); 
			modButton.onClick.AddListener(OnCreateMod); 
			addPretenderButton.onClick.AddListener(OnAddPretender); 
			clearPretendersButton.onClick.AddListener(OnClearPretenders); 
			startButton.onClick.AddListener(OnStart); 
		}

		private void OnCreateMap ()
		{
			var inputFolderPath  = Application.streamingAssetsPath + "/Map";
			var outputFolderPath = $"{Prefs.DataFolderPath.Get()}/maps/";
			CopyFilesRecursively(inputFolderPath, outputFolderPath);

			mapResultText.gameObject.SetActive(true);
		}
	
		private void OnCreateMod ()
		{
			var inputFolderPath  = Application.streamingAssetsPath + "/Mod";
			var outputFolderPath = $"{Prefs.DataFolderPath.Get()}/mods/";
			CopyFilesRecursively(inputFolderPath, outputFolderPath);
		
			modResultText.gameObject.SetActive(true);
		}

		private void Start ()
		{
			executablePathField.text = Prefs.ExecutablePath.Get();
			dataPathField.text       = Prefs.DataFolderPath.Get();
		}

		public override void Show ()
		{
			base.Show();
		
			transform.SetAsLastSibling();
			UpdatePretendersText();
		}

		private void OnExecutablePathChanged (string arg0)
		{
			Prefs.ExecutablePath.Set(arg0);
		}
	
		private void OnDataPathChanged (string arg0)
		{
			Prefs.DataFolderPath.Set(arg0);
		}
	
		private void OnStart ()
		{
			Hide();

			DomEdit.I.MapMan.LoadMap();
		}
	
		private void OnAddPretender ()
		{
			DomEdit.I.Ui.Get<PretenderLoadMenu>().SelectPretender((pretender) =>
			{
				if (string.IsNullOrEmpty(Prefs.DefaultPretenderA.Get()))
				{
					Prefs.DefaultPretenderA.Set(pretender.filePath);
				}
				else if (string.IsNullOrEmpty(Prefs.DefaultPretenderB.Get()))
				{
					Prefs.DefaultPretenderB.Set(pretender.filePath);
				}

				UpdatePretendersText();
			});
		}

		private void OnClearPretenders ()
		{
			Prefs.DefaultPretenderA.Clear();
			Prefs.DefaultPretenderB.Clear();
		
			UpdatePretendersText();
		}
	
		private void UpdatePretendersText ()
		{
			var pretendersText = "Pretenders Added:";
			if (!string.IsNullOrEmpty(Prefs.DefaultPretenderA.Get()))
			{
				pretendersText += $"\n{Prefs.DefaultPretenderA.Get()}";
			}
			if (!string.IsNullOrEmpty(Prefs.DefaultPretenderB.Get()))
			{
				pretendersText += $"\n{Prefs.DefaultPretenderB.Get()}";
			}
			addedPretendersText.text = pretendersText;
		}

		//https://stackoverflow.com/a/3822913
		private static void CopyFilesRecursively(string sourcePath, string targetPath)
		{
			//Now Create all of the directories
			foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
			{
				Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
			}

			//Copy all the files & Replaces any files with the same name
			foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
			{
				if (Path.GetExtension(newPath) == ".meta") continue;
			
				File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
			}
		}
	}

}
