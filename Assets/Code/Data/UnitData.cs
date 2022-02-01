using System.Collections.Generic;
using Core.Entities;
using UnityEngine;

namespace Data
{

	public class UnitData : IEntityData
	{
		public int          id;
		public string       name;
		public UnitType     unitType;
		public List<Nation> nations = new List<Nation>();
		public Sprite       icon;

		public override string ToString ()
		{
			return $"{nameof(id)}: {id}, {nameof(name)}: {name}";
		}
	}

	public enum UnitType
	{
		Unknown,
		Commander,
		CommanderCapOnly,
		CommanderFutureCapOnly,
		CommanderCave,
		CommanderCoast,
		CommanderForeign,
		CommanderForest,
		CommanderMountain,
		CommanderSwamp,
		CommanderUnderwater,
		CommanderWaste,
		CommanderPlains,
		CommanderMagicSite,
		CommanderSummon,
		CommanderLand,
		Unit,
		UnitCapOnly,
		UnitFutureCapOnly,
		UnitCave,
		UnitCoast,
		UnitForeign,
		UnitForest,
		UnitMountain,
		UnitSwamp,
		UnitUnderwater,
		UnitWaste,
		UnitPlains,
		UnitMagicSite,
		UnitSummon,
		UnitLand,
		Hero,
		HeroMulti,
		HeroUnique,
		Pretender,
	}
}