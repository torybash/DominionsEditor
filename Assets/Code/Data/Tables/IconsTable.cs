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
		public List<UnitIcon> unitIcons;

		public Sprite GetNationIcon (string fileNameBase)
		{
			var nationIcon = nationIcons.SingleOrDefault(x => x.fileNameBase == fileNameBase);
			return nationIcon?.icon;
		}
		public Sprite GetUnitIcon (int unitId)
		{
			var unitIcon = unitIcons.SingleOrDefault(x => x.id == unitId);
			return unitIcon?.icon;
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
	
	[Serializable]
	public class UnitIcon
	{
		public int id;
		public Sprite icon;

		public UnitIcon (int id, Sprite icon)
		{
			this.id   = id;
			this.icon = icon;
		}
	}

}