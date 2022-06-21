using System;
using System.Collections.Generic;
using System.IO;
using Core;
using Dom;
using Tools;
using UI.Gizmos;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menus
{

	public class PretenderLoadMenu : Menu
	{
		[SerializeField] RectTransform pretendersContainer;
		[SerializeField] Button        closeButton;

		List<PretenderFileGizmo> _gizmos = new List<PretenderFileGizmo>();

		void Awake ()
		{
			closeButton.onClick.AddListener(Hide);
		}

		public override void Show ()
		{
			base.Show();
		
			transform.SetAsLastSibling();
		}

		public override void Hide ()
		{
			base.Hide();

			foreach (var g in _gizmos) Destroy(g.gameObject);
			_gizmos.Clear();
		}

		public void SelectPretender (Action<Pretender> onSelect)
		{
			Show();

			foreach (var pretenderFile in Directory.GetFiles(Paths.PretendersFolderPath))
			{
				if (!pretenderFile.EndsWith("2h")) continue;

				var pretender = PretenderLoad.LoadFile(pretenderFile);
				if (pretender.era == -1) continue;
			
				var gizmo = DomEdit.I.Ui.Create<PretenderFileGizmo>(pretendersContainer);
				gizmo.Initialize(pretender);
				gizmo.LoadClicked += () =>
				{
					onSelect?.Invoke(pretender);
					Hide();
				};

				_gizmos.Add(gizmo);
			}
		}
	}

}