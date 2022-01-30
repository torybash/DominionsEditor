using UnityEngine;

namespace Data.Entries
{

	public abstract class SearchableEntry
	{
		public abstract string Name   { get; }
		public abstract Sprite Sprite { get; }
	}

}