using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Data.Tables
{
	public abstract class Table : ScriptableObject
	{
		
	}
	[CreateAssetMenu]
	public class IconsTable : Table
	{
		public List<NationIcon> nationIcons;
		public List<UnitIcon>   unitIcons;
		public List<ItemIcon>   itemIcons;

		public Sprite GetNationIcon (string fileNameBase)
		{
			var nationIcon = nationIcons.SingleOrDefault(x => x.fileNameBase == fileNameBase);
			return nationIcon?.icon;
		}
		public Sprite GetUnitIcon (int id)
		{
			var unitIcon = unitIcons.SingleOrDefault(x => x.id == id);
			return unitIcon?.icon;
		}
		
		public Sprite GetItemIcon (int id)
		{
			var itemIcon = itemIcons.SingleOrDefault(x => x.id == id);
			return itemIcon?.icon;
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

		
	[Serializable]
	public class ItemIcon
	{
		public int    id;
		public Sprite icon;

		public ItemIcon (int id, Sprite icon)
		{
			this.id   = id;
			this.icon = icon;
		}
	}
}