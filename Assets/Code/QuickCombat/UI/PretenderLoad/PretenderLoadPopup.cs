using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility.Extensions;

namespace QuickCombat.UI.Popups
{
	public class PretenderLoadPopup : Popup
	{
		public RectTransform      pretendersContainer;
		public Button             closeButton;
		public PretenderFileGizmo gizmoTemplate;

		[NonSerialized]  public List<PretenderFileGizmo> gizmos = new();

		void Awake ()
		{
			// closeButton.onClick.AddListener(Hide);
		}

		void OnDisable ()
		{
			gizmos.SafeDestroyList();
		}
	}

}