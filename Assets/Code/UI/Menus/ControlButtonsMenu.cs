using System.Linq;
using Core;
using Tools;
using UI.Gizmos;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menus
{

	public class ControlButtonsMenu : Menu
	{
		[SerializeField] Button runButton;
		[SerializeField] Button saveMapButton;
		[SerializeField] Button loadMapButton;
		[SerializeField] Button settingsButton;
		[SerializeField] Button modEditButton;

		void Awake ()
		{
			saveMapButton.onClick.AddListener(OnSaveClicked);
			loadMapButton.onClick.AddListener(OnLoadClicked);
			runButton.onClick.AddListener(OnRunClicked);
			settingsButton.onClick.AddListener(OnSettingsClicked);
			modEditButton.onClick.AddListener(OnModEditClicked);
		}

		void OnLoadClicked ()
		{
			DomEdit.I.Ui.Get<LoadMapMenu>().SelectMap(file =>
			{
				DomEdit.I.MapMan.LoadMap(file);
			});
		}
		
		public void OnModEditClicked ()
		{
			DomEdit.I.Ui.Get<ModEditMenu>().Show();
		}

		public void OnSettingsClicked ()
		{
			DomEdit.I.Ui.Get<SettingsMenu>().Show();
		}

		void OnRunClicked ()
		{
			if (DomEdit.I.MapMan.map.players.Any(x => string.IsNullOrEmpty(x.Pretender.filePath)))
			{
				var messageGizmo = DomEdit.I.Ui.Create<MessageGizmo>();
				messageGizmo.Write($"Player(s) missing pretender reference:\n{string.Join("\n", DomEdit.I.MapMan.map.players.Select(p => p.Nation))}");
				return;
			}

			var mapRunner = new DomRunner(DomEdit.I.MapMan);
			mapRunner.Run();
		}

		void OnSaveClicked ()
		{
			DomEdit.I.MapMan.SaveMap();
		}
	}

}
