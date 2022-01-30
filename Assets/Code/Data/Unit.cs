using System.Collections.Generic;
using UnityEngine;

namespace Data
{
	public class Unit
	{
		public int          id;
		public string       name;
		public string       unitType;
		public List<Nation> nations = new List<Nation>();
		public Sprite       icon;
	}

	public enum UnitType
	{
		Commander,
		CommanderCapOnly,
		CommanderFutureCapOnly,
		CommanderCave,
		CommanderCoast,
		CommanderForeign,
		CommanderMountain,
		CommanderSwamp,
		CommanderUnderwater,
		CommanderWaste,
		CommanderPlains,
		CommanderMagicSite,
		CommanderSummon,
		Unit,
		UnitCapOnly,
		UnitFutureCapOnly,
		UnitCave,
		UnitCoast,
		UnitForeign,
		UnitMountain,
		UnitSwamp,
		UnitUnderwater,
		UnitWaste,
		UnitPlains,
		UnitMagicSite,
		UnitSummon,
		Hero,
		HeroMulti,
		HeroUnique,
		Pretender,
		Unknown,
	}
}