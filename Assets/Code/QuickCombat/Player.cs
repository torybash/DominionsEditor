using System.Collections.Generic;
using Data;
using Dom;

namespace QuickCombat
{
	public class Player
	{
		public readonly PlayerSide side;
		
		public Nation     nation;
		public Pretender  pretender;
		
		public List<UnitData> commanders = new();

		public Player (PlayerSide side)
		{
			this.side = side;
		}

		public void AddCommander (UnitData commander)
		{
			commanders.Add(commander);
		}
	}
}