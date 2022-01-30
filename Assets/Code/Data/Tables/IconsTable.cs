using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Data.Tables
{

	[CreateAssetMenu]
	public class IconsTable : ScriptableObject
	{
		public List<NationIcon> nationIcons;

		public Sprite GetNationIcon (string fileNameBase)
		{
			var nationIcon = nationIcons.Single(x => x.fileNameBase == fileNameBase);
			return nationIcon.icon;
		}
	}

	[Serializable]
	public class NationIcon
	{
		public string fileNameBase;
		public Sprite icon;

		public NationIcon (string fileNameBase, Sprite icon)
		{
			this.fileNameBase = fileNameBase;
			this.icon         = icon;
		}
	}

}