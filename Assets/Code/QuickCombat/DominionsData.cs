using Data;
using UnityEngine;

namespace QuickCombat
{
	public class DominionsData
	{
		public Spells  spells;
		public Nations nations;
		public Units   units;
		public Items   items;


		public static void Load ()
		{
			var domData  = new DominionsData();
			DomData.SetInstance(domData);

			var gameData = GameData.Load();
			domData.spells  = Spells.Load(gameData);
			domData.nations = Nations.Load(gameData);
			domData.units   = Units.Load(gameData);
			domData.items   = Items.Load(gameData);

			domData.spells.ParseData();
			domData.nations.ParseData();
			domData.units.ParseData();
			domData.items.ParseData();

		}
	}

	public static class DomData
	{
		static DominionsData data;

		public static Spells  Spells  => data.spells;
		public static Nations Nations => data.nations;
		public static Units   Units   => data.units;
		public static Items   Items   => data.items;

		public static void SetInstance (DominionsData domData)
		{
			Debug.Assert(data == null);
			data = domData;
		}
	}
}