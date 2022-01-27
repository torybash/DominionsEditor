using System;
using System.Collections.Generic;

public class Searcher
{
	private const int MAX_RESULTS = 10;
	
	public SearchFilter searchFilter = SearchFilter.All;
	
	public List<SearchableEntry> Search (string searchText)
	{
		var foundEntries = new List<SearchableEntry>();
		var entriesToSearchIn = new List<SearchableEntry>();
		if (searchFilter.HasFlag(SearchFilter.Monsters)) entriesToSearchIn.AddRange(DomEdit.I.monsters.Entries);
		if (searchFilter.HasFlag(SearchFilter.Items)) entriesToSearchIn.AddRange(DomEdit.I.items.Entries);
		
		foreach (var entry in entriesToSearchIn)
		{
			if (entry.Name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) != -1)
			{
				foundEntries.Add(entry);
				if (foundEntries.Count >= MAX_RESULTS) break;
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