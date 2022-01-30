using System;
using System.Collections.Generic;
using Data;
using Data.Entries;

namespace Core
{

	public class Searcher
	{
		public const int MAX_RESULTS = 25;
	
		public SearchFilter searchFilter = SearchFilter.All;

		// public Searcher (List<Unit> getAll)
		// {
		// 	throw new NotImplementedException();
		// }

		public List<SearchableEntry> Search (string searchText)
		{
			var foundEntries      = new List<SearchableEntry>();
			var entriesToSearchIn = new List<SearchableEntry>();
			if (searchFilter.HasFlag(SearchFilter.Monsters)) entriesToSearchIn.AddRange(DomEdit.I.monsters.Entries);
			if (searchFilter.HasFlag(SearchFilter.Items)) entriesToSearchIn.AddRange(DomEdit.I.items.Entries);
			if (searchFilter.HasFlag(SearchFilter.Magic)) entriesToSearchIn.AddRange(DomEdit.I.magicPaths.magics);
		
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
		Magic    = 1 << 2,

		All = Monsters | Items | Magic,
	}

}