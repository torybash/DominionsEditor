using UnityEngine;

namespace Data.Entries
{

	[System.Serializable]
	public class MonsterEntry : SearchableEntry
	{
		[SerializeField] private int    id;
		[SerializeField] private string name;
		[SerializeField] private Sprite sprite;

		public          int    Id     => id;
		public override string Name   => name;
		public override Sprite Sprite => sprite;

		public MonsterEntry (int id, string name, Sprite sprite)
		{
			this.id     = id;
			this.name   = name;
			this.sprite = sprite;
		}
	}

}