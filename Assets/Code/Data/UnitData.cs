using System;
using System.Collections.Generic;
using Core.Entities;
using UnityEngine;

namespace Data
{

	public class UnitData //: IEntityData
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

	public static class UnitTypeExtensions
	{
		public static bool IsCommander (this UnitType unitType)
		{
			return unitType switch
			{

				UnitType.Unknown                => false,
				UnitType.Commander              => true,
				UnitType.CommanderCapOnly       => true,
				UnitType.CommanderFutureCapOnly => true,
				UnitType.CommanderCave          => true,
				UnitType.CommanderCoast         => true,
				UnitType.CommanderForeign       => true,
				UnitType.CommanderForest        => true,
				UnitType.CommanderMountain      => true,
				UnitType.CommanderSwamp         => true,
				UnitType.CommanderUnderwater    => true,
				UnitType.CommanderWaste         => true,
				UnitType.CommanderPlains        => true,
				UnitType.CommanderMagicSite     => true,
				UnitType.CommanderSummon        => true,
				UnitType.CommanderLand          => true,

				UnitType.Hero       => true,
				UnitType.HeroMulti  => true,
				UnitType.HeroUnique => true,
				UnitType.Pretender  => true,

				UnitType.Unit              => false,
				UnitType.UnitCapOnly       => false,
				UnitType.UnitFutureCapOnly => false,
				UnitType.UnitCave          => false,
				UnitType.UnitCoast         => false,
				UnitType.UnitForeign       => false,
				UnitType.UnitForest        => false,
				UnitType.UnitMountain      => false,
				UnitType.UnitSwamp         => false,
				UnitType.UnitUnderwater    => false,
				UnitType.UnitWaste         => false,
				UnitType.UnitPlains        => false,
				UnitType.UnitMagicSite     => false,
				UnitType.UnitSummon        => false,
				UnitType.UnitLand          => false,

				_ => throw new ArgumentOutOfRangeException(nameof(unitType), unitType, null)
			};
		}

		public static bool IsTroop (this UnitType unitType)
		{
			return unitType switch
			{

				UnitType.Unknown                => false,
				UnitType.Commander              => false,
				UnitType.CommanderCapOnly       => false,
				UnitType.CommanderFutureCapOnly => false,
				UnitType.CommanderCave          => false,
				UnitType.CommanderCoast         => false,
				UnitType.CommanderForeign       => false,
				UnitType.CommanderForest        => false,
				UnitType.CommanderMountain      => false,
				UnitType.CommanderSwamp         => false,
				UnitType.CommanderUnderwater    => false,
				UnitType.CommanderWaste         => false,
				UnitType.CommanderPlains        => false,
				UnitType.CommanderMagicSite     => false,
				UnitType.CommanderSummon        => false,
				UnitType.CommanderLand          => false,

				UnitType.Hero       => false,
				UnitType.HeroMulti  => false,
				UnitType.HeroUnique => false,
				UnitType.Pretender  => false,

				UnitType.Unit              => true,
				UnitType.UnitCapOnly       => true,
				UnitType.UnitFutureCapOnly => true,
				UnitType.UnitCave          => true,
				UnitType.UnitCoast         => true,
				UnitType.UnitForeign       => true,
				UnitType.UnitForest        => true,
				UnitType.UnitMountain      => true,
				UnitType.UnitSwamp         => true,
				UnitType.UnitUnderwater    => true,
				UnitType.UnitWaste         => true,
				UnitType.UnitPlains        => true,
				UnitType.UnitMagicSite     => true,
				UnitType.UnitSummon        => true,
				UnitType.UnitLand          => true,

				_ => throw new ArgumentOutOfRangeException(nameof(unitType), unitType, null)
			};
		}
	}

}