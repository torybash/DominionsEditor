using System;
using System.Collections.Generic;
using Data;

namespace Map.MapData
{

	[Serializable]
	public class Commander : Monster
	{
		public List<Unit>          UnitsUnderCommand { get; set; } = new List<Unit>();
		public List<Unit>          Bodyguards        { get; set; } = new List<Unit>();
		public List<Item>          Items             { get; set; } = new List<Item>();
		public List<MagicOverride> MagicOverrides    { get; set; } = new List<MagicOverride>();
		public int                 Xp                { get; set; }

		public static Commander Create (int monsterId, Nation nationality)
		{
			return new Commander{MonsterId = monsterId, Nationality = nationality};
		}
	}

}