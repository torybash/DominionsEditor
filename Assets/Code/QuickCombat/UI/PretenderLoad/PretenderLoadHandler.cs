using System;
using System.Collections.Generic;
using System.IO;
using Core;
using Dom;
using QuickCombat.UI.Popups;
using Tools;
using Utility.Extensions;

namespace QuickCombat
{
	public class PretenderLoadHandler
	{
		Action<Pretender> _selected;

		public void Run (Action<Pretender> selected)
		{
			_selected = selected;
			
			var popup = M.ShowPopup<PretenderLoadPopup>();
			foreach (Pretender pretender in LoadPretenders())
			{
				var gizmo = popup.gizmoTemplate.Copy();
				gizmo.Initialize(pretender);
				gizmo.loadButton.onClick.AddListener(() => OnSelectedPretender(pretender));
				popup.gizmos.Add(gizmo);				
			}
		}

		void OnSelectedPretender (Pretender pretender)
		{
			M.ClosePopup<PretenderLoadPopup>();
			_selected?.Invoke(pretender);
		}

		IEnumerable<Pretender> LoadPretenders ()
		{
			foreach (var pretenderFile in Directory.GetFiles(Paths.PretendersFolderPath))
			{
				if (!pretenderFile.EndsWith("2h")) continue;

				Pretender pretender = PretenderLoad.LoadFile(pretenderFile);
				if (pretender.era == -1) continue;

				yield return pretender;
				// var gizmo = M.CreateGizmo<PretenderFileGizmo>(_popup.pretendersContainer);
				// gizmo.Initialize(pretender);
				// gizmo.LoadClicked += () =>
				// {
				// 	onSelect?.Invoke(pretender);
				// 	Hide();
				// };

				// _gizmos.Add(gizmo);
			}
		}
	}
}