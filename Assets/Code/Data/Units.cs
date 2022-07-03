using System.Collections.Generic;
using System.Linq;
using Core;
using Data.Tables;
using QuickCombat;
using UnityEngine;

// ReSharper disable StringLiteralTypo

namespace Data
{

	public class Units
	{
		readonly GameData _gameData;
		List<UnitData>            _units;

		Units (GameData gameData)
		{
			_gameData = gameData;
		}

		public static Units Load (GameData gameData)
		{
			return new Units(gameData);
		}

		public List<UnitData> GetAll ()
		{
			return _units;
		}

		public void ParseData ()
		{
			_units = new List<UnitData>();
			foreach (var unitData in _gameData.unitsTable)
			{
				var unit = new UnitData();
				unit.id   = int.Parse(unitData["id"]);
				unit.name = unitData["name"];
				unit.icon = Tbl.Get<IconsTable>().GetUnitIcon(unit.id);

				_units.Add(unit);
			}
			
			foreach (var nation in DomData.Nations.GetAll())
			{
				// associate pretenders
				//  nation.pretenders = nation.pretenders.Distinct().ToList();
				foreach (int nationPretender in nation.pretenders)
				{
					var unit = _units.Single(x => x.id == nationPretender);

					unit.unitType = UnitType.Pretender;
					unit.nations.Add(nation);
				}
				
				//////////////////////////////////////////////////
				// associate units with this nation
				//  if unit is already associated with a nation it creates a duplicate (with incremented id: +.01)
				/////////////////////////////////////////////////
				var iterations = new Dictionary<UnitType, List<int>>
				{
					{ UnitType.Unit, nation.units },
					{ UnitType.Commander, nation.commanders },
					{ UnitType.CommanderForeign, nation.foreigncommanders },
					{ UnitType.UnitForeign, nation.foreignunits },
					{ UnitType.UnitForest, nation.forestrec },
					{ UnitType.CommanderForest, nation.forestcom },
					{ UnitType.UnitMountain, nation.mountainrec },
					{ UnitType.CommanderMountain, nation.mountaincom },
					{ UnitType.UnitSwamp, nation.swamprec },
					{ UnitType.CommanderSwamp, nation.swampcom },
					{ UnitType.UnitWaste, nation.wasterec },
					{ UnitType.CommanderWaste, nation.wastecom },
					{ UnitType.UnitCave, nation.caverec },
					{ UnitType.CommanderCave, nation.cavecom },
					{ UnitType.UnitCoast, nation.coastrec },
					{ UnitType.CommanderCoast, nation.coastcom },
					{ UnitType.UnitPlains, nation.plainsrec },
					{ UnitType.CommanderPlains, nation.plainscom },
					{ UnitType.UnitLand, nation.landunit },
					{ UnitType.CommanderLand, nation.landcom },
					{ UnitType.HeroUnique, nation.heroes },
					{ UnitType.HeroMulti, nation.multiheroes },
					{ UnitType.UnitUnderwater, nation.uwunit },
					{ UnitType.CommanderUnderwater, nation.uwcom },
					{ UnitType.UnitCapOnly, nation.capunits },
					{ UnitType.CommanderCapOnly, nation.capcommanders },
					{ UnitType.UnitFutureCapOnly, nation.futurecapunits },
					{ UnitType.CommanderFutureCapOnly, nation.futurecapcommanders },
					
				};

				foreach (var iter in iterations)
				{
					var arr = iter.Value;
					foreach (var unitId in arr)
					{
						var unitData = _gameData.unitsTable.GetData("id", unitId.ToString());
						if (unitData == null)
						{
							Debug.LogError($"Unit with id {unitId} not found for nation {nation}");
							continue;
						}

						var unit = _units.Single(x => x.id == unitId);
						unit.unitType = iter.Key;
						unit.nations.Add(nation);
					}
				}
			}

			var sortedUnits = _units.OrderBy(x => x.unitType);

			var unknownUnitTypes = _units.Count(x => x.unitType == UnitType.Unknown);
			Debug.Log($"Total units {_units.Count}. Unknown types: {unknownUnitTypes}");
			Debug.Log($"Units:\n\n{string.Join("\n", sortedUnits.Select(x => $"{x.unitType}\t{x.name}\t{x.id}"))}");
		}


	}

}