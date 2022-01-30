using System;
using Data;

namespace Core.Entities
{
	[Serializable]
	public class Troops : Monster
	{
		public int Amount { get; set; }

		public static Troops Create (int monsterId, int unitsElementAmount, Nation nationality)
		{
			return new Troops { MonsterId = monsterId, Amount = unitsElementAmount, Nationality = nationality};
		}
	}

}