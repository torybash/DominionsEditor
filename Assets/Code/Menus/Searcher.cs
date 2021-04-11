using System;
using System.Collections.Generic;
using System.Linq;
public class Searcher
{
	private const int MaxResults = 10;
	
	public MonstersTable Monsters { get; set; }

	public List<MonstersTable.Entry> Search (string searchText)
	{
		var foundEntries = new List<MonstersTable.Entry>();
		foreach (var monster in Monsters.Entries)
		{
			if (monster.Name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) != -1)
			{
				foundEntries.Add(monster);
				if (foundEntries.Count >= MaxResults) break;
			}
		}
		
		return foundEntries;
	} 
}