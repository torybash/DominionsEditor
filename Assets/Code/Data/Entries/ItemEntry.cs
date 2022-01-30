using Dom;
using UnityEngine;

namespace Data.Entries
{

	[System.Serializable]
	public class ItemEntry : SearchableEntry
	{
		[SerializeField] private string   name;
		[SerializeField] private int      id;
		[SerializeField] private ItemType type;
		[SerializeField] private Sprite   sprite;

		public override string Name   => name;
		public override Sprite Sprite => sprite;

		public ItemType Type => type;
		public int      Id   => id;

		public ItemEntry (int id, string name, ItemType type, Sprite sprite)
		{
			this.id     = id;
			this.name   = name;
			this.type   = type;
			this.sprite = sprite;
		}
	}

}