using System;
using System.Collections.Generic;
using System.Linq;
public class Searcher
{
	private const int MaxResults = 10;
	
	private SearchFilter _searchFilter = SearchFilter.All;
	
	public MonstersTable Monsters { get; set; }
	public ItemsTable Items { get; set; }

	public List<SearchableEntry> Search (string searchText)
	{
		var foundEntries = new List<SearchableEntry>();
		var entriesToSearchIn = new List<SearchableEntry>();
		if (_searchFilter.HasFlag(SearchFilter.Monsters)) entriesToSearchIn.AddRange(Monsters.Entries);
		if (_searchFilter.HasFlag(SearchFilter.Items)) entriesToSearchIn.AddRange(Items.Entries);
		
		foreach (var entry in entriesToSearchIn)
		{
			if (entry.Name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) != -1)
			{
				foundEntries.Add(entry);
				if (foundEntries.Count >= MaxResults) break;
			}
		}
		
		return foundEntries;
	} 
}

[Flags]
public enum SearchFilter
{
	Nothing  = 0,
	Monsters = 1 << 0,
	Items    = 1 << 1,

	All = Monsters | Items,
}