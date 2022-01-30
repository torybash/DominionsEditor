using System;
using Data;

namespace Map.MapData
{

	[Serializable]
	public class Unit : Monster
	{
		public int Amount { get; set; }

		public static Unit Create (int monsterId, int unitsElementAmount, Nation nationality)
		{
			return new Unit { MonsterId = monsterId, Amount = unitsElementAmount, Nationality = nationality};
		}
	}

}