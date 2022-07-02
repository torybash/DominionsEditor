using System;
using System.IO;
using Dom;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QuickCombat.UI.Popups
{
	public class PretenderFileGizmo : MonoBehaviour
	{
		[SerializeField] TMP_Text fileText;

		public Button loadButton;

		public void Initialize (Pretender pretender)
		{
			var fileName = Path.GetFileName(pretender.filePath);
			fileText.text = fileName;
		}
	}
}