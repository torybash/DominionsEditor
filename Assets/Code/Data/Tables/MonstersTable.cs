using System.Collections.Generic;
using Data.Entries;
using UnityEngine;

namespace Data.Tables
{

	[CreateAssetMenu]
	public class MonstersTable : ScriptableObject
	{
		[SerializeField] private List<MonsterEntry> monsterEntries;

		private Dictionary<int, MonsterEntry>    idMap;
		private Dictionary<string, MonsterEntry> nameMap;
		public  List<MonsterEntry>               Entries => monsterEntries;
	
		public MonsterEntry GetMonster (int id)
		{
			if (idMap == null)
			{
				idMap = new Dictionary<int, MonsterEntry>();
				foreach (var entry in Entries) idMap.Add(entry.Id, entry);
			}

			return idMap[id];
		}
	
	
		public MonsterEntry GetMonster (string unitName)
		{
			if (nameMap == null)
			{
				nameMap = new Dictionary<string, MonsterEntry>();
				foreach (var entry in Entries) nameMap.Add(entry.Name, entry);
			}

			return nameMap[unitName];
		}



	}

}