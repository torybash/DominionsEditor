using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core;
using Core.Entities;
using UnityEngine;

// ReSharper disable StringLiteralTypo

namespace Data
{

	public class Units
	{
		private List<Unit> _units;

		public List<Unit> GetAll ()
		{
			return _units;
		}

		public void ParseData ()
		{
			_units = new List<Unit>();
			foreach (var unitData in DomEdit.I.GameData.baseUTable)
			{
				var unit = new Unit();
				unit.id   = int.Parse(unitData["id"]);
				unit.name = unitData["name"];
				unit.icon = DomEdit.I.icons.GetUnitIcon(unit.id);

				_units.Add(unit);
			}
			
			foreach (var nation in DomEdit.I.Nations.GetAll())
			{
				//////////////////////////////////////////////////
				// associate units with this nation
				//  if unit is already associated with a nation it creates a duplicate (with incremented id: +.01)
				/////////////////////////////////////////////////
				var iterations = new Dictionary<string, List<int>>
				{
					{ "unit", nation.units },
					{ "commander", nation.commanders },
					{ "cmdr (foreign)", nation.foreigncommanders },
					{ "unit (foreign)", nation.foreignunits },
					{ "unit (forest)", nation.forestrec },
					{ "cmdr (forest)", nation.forestcom },
					{ "unit (mountain)", nation.mountainrec },
					{ "cmdr (mountain)", nation.mountaincom },
					{ "unit (swamp)", nation.swamprec },
					{ "cmdr (swamp)", nation.swampcom },
					{ "unit (waste)", nation.wasterec },
					{ "cmdr (waste)", nation.wastecom },
					{ "unit (cave)", nation.caverec },
					{ "cmdr (cave)", nation.cavecom },
					{ "unit (coast)", nation.coastrec },
					{ "cmdr (coast)", nation.coastcom },
					{ "unit (plains)", nation.plainsrec },
					{ "cmdr (plains)", nation.plainscom },
					{ "unit (land)", nation.landunit },
					{ "cmdr (land)", nation.landcom },
					{ "hero (unique)", nation.heroes },
					{ "hero (multi)", nation.multiheroes },
					{ "unit (u-water)", nation.uwunit },
					{ "cmdr (u-water)", nation.uwcom },
					{ "unit (cap only)", nation.capunits },
					{ "cmdr (cap only)", nation.capcommanders },
					{ "unit (future cap only)", nation.futurecapunits },
					{ "cmdr (future cap only)", nation.futurecapcommanders }
				};

				foreach (var iter in iterations)
				{
					var arr = iter.Value;
					foreach (var unitId in arr)
					{
						var unitData = DomEdit.I.GameData.baseUTable.GetData("id", unitId.ToString());
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
		}


	}

}