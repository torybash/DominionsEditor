using System.Collections.Generic;
using Data.Entries;
using UnityEngine;

namespace Data.Tables
{

	[CreateAssetMenu]
	public class ItemsTable : ScriptableObject
	{
		[SerializeField] private List<ItemEntry> entries;
	
		private Dictionary<string, ItemEntry> nameMap;

		public List<ItemEntry> Entries => entries;

		public ItemEntry GetItem (string itemItemName)
		{
			if (nameMap == null)
			{
				nameMap = new Dictionary<string, ItemEntry>();
				foreach (var entry in Entries)
				{
					if (nameMap.ContainsKey(entry.Name)) continue;
					nameMap.Add(entry.Name, entry);
				}
			}

			return nameMap[itemItemName];
		}
	}

}