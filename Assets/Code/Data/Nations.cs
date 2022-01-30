using System;
using System.Collections.Generic;
using System.Linq;
using Core;

namespace Data
{

	public class Nations
	{
		private List<Nation> _nations;

		public List<Nation> GetAll ()
		{
			return _nations;
		}

		public Nation GetNationByNameAndEra (string nationName, int era)
		{
			var entry = _nations.SingleOrDefault(x => x.name.Equals(nationName, StringComparison.OrdinalIgnoreCase) && x.era == era);
			return entry;
		}

		public void LoadData ()
		{
			// bool disableold = modctx.disableoldnations;
			// if (!disableold)
			// {
			// 	for (FIXME_VAR_TYPE oi = 0, o; o = modctx.nationdata[oi]; oi++)
			// 	{
			// 		if (nation.disableoldnations)
			// 		{
			// 			disableold = true;
			// 			continue;
			// 		}
			// 	}
			// }

			// if (disableold)
			// {
			// 	for (FIXME_VAR_TYPE oi = modctx.nationdata.length - 1, o; o = modctx.nationdata[oi]; oi--)
			// 	{
			// 		nation.id = int.Parse(nation.id);
			//
			// 		if (disableold && nation.id < 100)
			// 		{
			// 			modctx.nationdata.splice(oi, 1);
			// 		}
			// 	}
			// }
			_nations = new List<Nation>();

			foreach (var nationData in DomEdit.I.GameData.nationsTable)
			{
				var nation = new Nation();
				_nations.Add(nation);

				nation.id           = int.Parse(nationData["id"]);
				nation.name         = nationData["name"];
				nation.epithet      = nationData["epithet"];
				nation.fileNameBase = nationData["file_name_base"];
				nation.era          = int.Parse(nationData["era"]);

				nation.icon = DomEdit.I.icons.GetNationIcon(nation.fileNameBase);

				// nation.id = int.Parse(nation.id);
				//
				// nation.renderOverlay = MNation.renderOverlay;
				//
				// nation.eracode   = modconstants.eracodes[nation.era];
				// nation.shortname = nation.eracode + '  ' + nation.name;
				// nation.fullname  = nation.eracode + '  ' + nation.name + '  -  ' + nation.epithet;

				// Get realms of nation
				var realms = new List<int>();
				foreach (var data in DomEdit.I.GameData.attributesByNationTable)
				{
					if (int.Parse(data["nation_number"]) == nation.id)
					{
						if (data["attribute"] == "289")
						{
							realms.Add(int.Parse(data["raw_value"]));
						}
					}
				}
				// if (nation.homerealm)
				// {
				// 	realms = realms.concat(nation.homerealm);
				// }

				// get monsters in realm
				foreach (var data in DomEdit.I.GameData.realmsTable)
				{
					foreach (int realm in realms)
					{
						if (int.Parse(data["realm"]) == realm)
						{
							nation.pretenders.Add(int.Parse(data["monster_number"]));
						}
					}
				}
				nation.homerealm = realms;

				// look for added pretenders
				// if (!nation.cleargods)
				// {
				// 	for (FIXME_VAR_TYPE oj = 0, attr; attr = modctx.pretender_types_by_nation[oj]; oj++)
				// 	{
				// 		if (int.Parse(attr.nation_number) == nation.id)
				// 		{
				// 			if (attr.monster_number != 134)
				// 			{ // Why is royal guard marked as pretender?
				// 				nation.pretenders.push(attr.monster_number);
				// 			}
				// 		}
				// 	}
				// }
				// if (nation.addgod)
				// {
				// 	nation.pretenders = nation.pretenders.concat(nation.addgod);
				// }

				// look for deleted pretenders
				// for (FIXME_VAR_TYPE oj = 0, attr; attr = modctx.unpretender_types_by_nation[oj]; oj++)
				// {
				// 	if (int.Parse(attr.nation_number) == nation.id)
				// 	{
				// 		for (FIXME_VAR_TYPE ok = 0, pret; pret = nation.pretenders[ok]; ok++)
				// 		{
				// 			if (int.Parse(pret) == int.Parse(attr.monster_number))
				// 			{
				// 				nation.pretenders.splice(ok, 1);
				// 				ok--;
				// 			}
				// 		}
				// 	}
				// }
				// if (nation.delgod)
				// {
				// 	nation.pretenders = nation.pretenders.filter(function(item) {
				// 		return nation.delgod.indexOf(item) == = -1;
				// 	});
				// }

				// look for commanders
				foreach (var data in DomEdit.I.GameData.fortLeadersByNationTable)
				{
					if (int.Parse(data["nation_number"]) == nation.id)
					{
						nation.commanders.Add(int.Parse(data["monster_number"]));
					}
				}

				// look for foreign commanders
				foreach (var data in DomEdit.I.GameData.nonFortLeadersByNationTable)
				{
					if (int.Parse(data["nation_number"]) == nation.id)
					{
						nation.foreigncommanders.Add(int.Parse(data["monster_number"]));
					}
				}

				// look for units
				foreach (var data in DomEdit.I.GameData.fortTroopByNationTable)
				{
					if (int.Parse(data["nation_number"]) == nation.id)
					{
						nation.units.Add(int.Parse(data["monster_number"]));
					}
				}

				// look for foreign units
				foreach (var data in DomEdit.I.GameData.nonFortTroopByNationTable)
				{
					if (int.Parse(data["nation_number"]) == nation.id)
					{
						nation.foreignunits.Add(int.Parse(data["monster_number"]));
					}
				}

				// look for coast commanders
				foreach (var data in DomEdit.I.GameData.coastLeadersByNationTable)
				{
					if (int.Parse(data["nation_number"]) == nation.id)
					{
						var unitData = DomEdit.I.GameData.baseUTable.GetData("id", data["monster_number"]);
						if (unitData.ContainsKey("landshape"))
						{
							nation.coastcom.Add(int.Parse(unitData["landshape"]));
						} else
						{
							nation.coastcom.Add(int.Parse(data["monster_number"]));
						}
					}
				}

				// look for coast units
				foreach (var data in DomEdit.I.GameData.coastTroopByNationTable)
				{
					if (int.Parse(data["nation_number"]) == nation.id)
					{
						var unitData = DomEdit.I.GameData.baseUTable.GetData("id", data["monster_number"]);
						if (unitData.ContainsKey("landshape"))
						{
							nation.coastrec.Add(int.Parse(unitData["landshape"]));
						} else
						{
							nation.coastrec.Add(int.Parse(data["monster_number"]));
						}
					}
				}

				// look for special stuff
				foreach (var data in DomEdit.I.GameData.attributesByNationTable)
				{
					if (int.Parse(data["nation_number"]) != nation.id) continue;

					// if ()

					if (data["attribute"] == "52" || data["attribute"] == "100")
					{
						nation.sites.Add(int.Parse(data["raw_value"]));
					}
					if (data["attribute"] == "631")
					{
						nation.futuresites.Add(int.Parse(data["raw_value"]));
					}
					if (data["attribute"] == "158" || data["attribute"] == "159")
					{
						var unitData = DomEdit.I.GameData.baseUTable.GetData("id", data["raw_value"]);
						if (unitData.ContainsKey("landshape"))
						{
							nation.coastcom.Add(int.Parse(unitData["landshape"]));
						} else
						{
							nation.coastcom.Add(int.Parse(data["raw_value"]));
						}
					}
					if (data["attribute"] == "160" || data["attribute"] == "161" || data["attribute"] == "162")
					{
						var unitData = DomEdit.I.GameData.baseUTable.GetData("id", data["raw_value"]);
						if (unitData.ContainsKey("landshape"))
						{
							nation.coastrec.Add(int.Parse(unitData["landshape"]));
						} else
						{
							nation.coastrec.Add(int.Parse(data["raw_value"]));
						}
					}
					if (data["attribute"] == "163")
					{
						nation.landcom.Add(int.Parse(data["raw_value"]));
					}
					if (data["attribute"] == "186")
					{
						var unitData = DomEdit.I.GameData.baseUTable.GetData("id", data["raw_value"]);
						if (unitData.ContainsKey("watershape"))
						{
							nation.uwcom.Add(int.Parse(unitData["watershape"]));
						} else
						{
							nation.uwcom.Add(int.Parse(data["raw_value"]));
						}
					}
					if (data["attribute"] == "187" ||
					    data["attribute"] == "189" ||
					    data["attribute"] == "190" ||
					    data["attribute"] == "191" ||
					    data["attribute"] == "213")
					{
						var unitData = DomEdit.I.GameData.baseUTable.GetData("id", data["raw_value"]);
						if (unitData.ContainsKey("watershape"))
						{
							nation.uwunit.Add(int.Parse(unitData["watershape"]));
						} else
						{
							nation.uwunit.Add(int.Parse(data["raw_value"]));
						}
					}
					if (data["attribute"] == "294" || data["attribute"] == "412")
					{
						nation.forestrec.Add(int.Parse(data["raw_value"]));
					}
					if (data["attribute"] == "295" || data["attribute"] == "413")
					{
						nation.forestcom.Add(int.Parse(data["raw_value"]));
					}
					if (data["attribute"] == "296")
					{
						nation.swamprec.Add(int.Parse(data["raw_value"]));
					}
					if (data["attribute"] == "297")
					{
						nation.swampcom.Add(int.Parse(data["raw_value"]));
					}
					if (data["attribute"] == "298" || data["attribute"] == "408")
					{
						nation.mountainrec.Add(int.Parse(data["raw_value"]));
					}
					if (data["attribute"] == "299" || data["attribute"] == "409")
					{
						nation.mountaincom.Add(int.Parse(data["raw_value"]));
					}
					if (data["attribute"] == "300" || data["attribute"] == "416")
					{
						nation.wasterec.Add(int.Parse(data["raw_value"]));
					}
					if (data["attribute"] == "301" || data["attribute"] == "417")
					{
						nation.wastecom.Add(int.Parse(data["raw_value"]));
					}
					if (data["attribute"] == "302")
					{
						nation.caverec.Add(int.Parse(data["raw_value"]));
					}
					if (data["attribute"] == "303")
					{
						nation.cavecom.Add(int.Parse(data["raw_value"]));
					}
					if (data["attribute"] == "404" || data["attribute"] == "406")
					{
						nation.plainsrec.Add(int.Parse(data["raw_value"]));
					}
					if (data["attribute"] == "405" || data["attribute"] == "407")
					{
						nation.plainscom.Add(int.Parse(data["raw_value"]));
					}
					if (data["attribute"] == "139" ||
					    data["attribute"] == "140" ||
					    data["attribute"] == "141" ||
					    data["attribute"] == "142" ||
					    data["attribute"] == "143" ||
					    data["attribute"] == "144")
					{
						nation.heroes.Add(int.Parse(data["raw_value"]));
					}
					if (data["attribute"] == "145" ||
					    data["attribute"] == "146" ||
					    data["attribute"] == "149")
					{
						nation.multiheroes.Add(int.Parse(data["raw_value"]));
					}
				}

				//associate spells
				//national spells already listed themselves in nation.spells
				//now we need to set nation details on the spells
// 				for (FIXME_VAR_TYPE si = 0, s; s = nation.spells[si]; si++)
// 				{
// 					s.nations = s.nations || {
// 					}
// 					;
// 					s.nations[nation.id] = o;
// 					s.eracodes      = s.eracodes || {
// 					}
// 					;
// 					s.eracodes[nation.eracode] = true;
//
// 					//nationname
// 					FIXME_VAR_TYPE ncount = 0;
// 					foreach (var k in s.nations) ncount++;
// 					if (ncount == 1)
// 						s.nationname = nation.shortname;
// 					else
// 						s.nationname = 'various (' + ncount + ')';
//
// 					//set nation value on summoned units
// 					FIXME_VAR_TYPE spell = s;
// 					do
// 					{
// 						FIXME_VAR_TYPE arr = spell.summonsunits || []
// 						;
// 						for (FIXME_VAR_TYPE i = 0, u; u = arr[i]; i++)
// 						{
// //					FIXME_VAR_TYPE basekey;
// //					if (u.typeclass == 'Unit') {
// //						basekey = 'unit (Summon)';
// //					} else {
// //						basekey = 'cmdr (Summon)';
// //					}
// //					if (u.typechar && u.typechar!=basekey) {
// //						//find pretender version of this unit
// //						u = modctx.getUnitOfType(u, basekey) || modctx.cloneUnit(u);
// //					}
// //					u.typechar = basekey;
//
// 							u.nations = u.nations || {
// 							}
// 							;
// 							u.nations[nation.id] = o;
// 							u.eracodes      = u.eracodes || {
// 							}
// 							;
// 							u.eracodes[nation.eracode] = true;
//
// 							//nationname
// 							FIXME_VAR_TYPE ncount = 0;
// 							foreach (var k in u.nations) ncount++;
// 							if (ncount == 1)
// 								u.nationname = nation.shortname;
// 							else
// 								u.nationname = 'various (' + ncount + ')';
//
// 							FIXME_VAR_TYPE otherList =  []
// 							;
// 							FIXME_VAR_TYPE other = u;
// 							otherList.push(other);
// 							while (other = modctx.unitlookup[other.secondshape || other.shapechange || other.forestshape])
// 							{
// 								if (otherList.indexOf(other) != -1)
// 								{
// 									break;
// 								}
// 								otherList.push(other);
// 								other.typechar = u.typechar;
//
// 								other.nations = other.nations || {
// 								}
// 								;
// 								other.nations[nation.id] = o;
// 								other.eracodes      = other.eracodes || {
// 								}
// 								;
// 								other.eracodes[nation.eracode] = true;
//
// 								//nationname
// 								FIXME_VAR_TYPE ncount = 0;
// 								foreach (var k in other.nations) ncount++;
// 								if (ncount == 1)
// 									other.nationname = nation.shortname;
// 								else
// 									other.nationname = 'various (' + ncount + ')';
// 							}
// 						}
// 						if (spell == spell.nextspell) break; // avoid infinite loop
// 					} while (spell = spell.nextspell);
// 				}

				//associate pretenders
				// nation.pretenders = nation.pretenders.Distinct().ToList();
				// foreach (int nationPretender in nation.pretenders)
				// {
				// 	var unitData = baseUTable.GetData("id", nationPretender.ToString());
				// 	if (unitData == null) continue;
				// }

				// for (FIXME_VAR_TYPE i = 0; i < arr.length; i++)
				// {
				// 	if (!arr[i]) continue;
				// 	FIXME_VAR_TYPE u = modctx.unitlookup[arr[i]];
				// 	if (!u)
				// 	{
				// 		console.log(basekey + ' ' + arr[i] + ' not found (nation ' + nation.id + ')');
				// 		continue;
				// 	}
				// 	if (u.typechar && u.typechar != basekey)
				// 	{
				// 		//find pretender version of this unit
				// 		u = modctx.getUnitOfType(u, basekey) || modctx.cloneUnit(u);
				// 	}
				// 	u.typechar = basekey;
				// 	u.nations  = u.nations || {
				// 	}
				// 	;
				// 	u.nations[nation.id] = o;
				// 	u.eracodes      = u.eracodes || {
				// 	}
				// 	;
				// 	u.eracodes[nation.eracode] = true;
				//
				// 	for (FIXME_VAR_TYPE oj = 0, attr; attr = modctx.attributes_by_nation[oj]; oj++)
				// 	{
				// 		if (int.Parse(attr.nation_number) == nation.id)
				// 		{
				// 			//FIXME_VAR_TYPE attribute= modctx.attributes_lookup[int.Parse(attr.attribute_record_id)];
				// 			if (attr.attribute == "314")
				// 			{
				// 				if (u.id == attr.raw_value)
				// 				{
				// 					u.cheapgod20 = u.cheapgod20 || []
				// 					;
				// 					u.cheapgod20.push(o);
				// 				}
				// 			}
				// 			if (attr.attribute == "315")
				// 			{
				// 				if (u.id == attr.raw_value)
				// 				{
				// 					u.cheapgod40 = u.cheapgod40 || []
				// 					;
				// 					u.cheapgod40.push(o);
				// 				}
				// 			}
				// 		}
				// 	}
				// }

				//units from sites
				// FIXME_VAR_TYPE basekey = 'site';
				// FIXME_VAR_TYPE arr     = nation.sites;
				// var gemkeys = new Dictionary<string, int>{ {"F", 0}, {"A", 0}, {"W", 0}, {"E",0}, {"S",0}, {"D",0}, {"N",0}, {"B",0 }};
				// foreach (var site in nation.sites)
				// {
				// 	var siteData = magicSitesTable.GetData("id", site.ToString());
				// 	if (siteData == null)
				// 	{
				// 		// Debug.Log("Site " + siteData + " not found (nation ' + nation.id + ')');
				// 		continue;
				// 	}
				// 	
				// 	nation.capunits = nation.capunits.Union(site)
				// 	// nation.capunits      = nation.capunits.concat(s.units, s.hmon, s.mon);
				// 	// nation.capcommanders = nation.capcommanders.concat(s.commanders, s.hcom);
				// 	// foreach (k in gemkeys) {
				// 	// 	if (s[k])
				// 	// 		gemkeys[k] += int.Parse(s[k]);
				// 	// }
				// }

				// FIXME_VAR_TYPE basekey = 'futuresite';
				// FIXME_VAR_TYPE arr     = nation.futuresites;
				// for (FIXME_VAR_TYPE i = 0; i < arr.length; i++)
				// {
				// 	if (!arr[i]) continue;
				// 	FIXME_VAR_TYPE s = modctx.sitelookup[arr[i]];
				// 	if (!s)
				// 	{
				// 		console.log(basekey + ' ' + arr[i] + ' not found (nation ' + nation.id + ')');
				// 		continue;
				// 	}
				// 	nation.futurecapunits      = nation.futurecapunits.concat(s.units, s.hmon, s.mon);
				// 	nation.futurecapcommanders = nation.futurecapcommanders.concat(s.commanders, s.hcom);
				// }

				//remove capunits duplicated in units (etc)
				// Utils.arrayDisect(nation.capunits,      nation.units)
				// Utils.arrayDisect(nation.capcommanders, nation.commanders)
				//
				// Utils.arrayUnique(nation.units);
				// Utils.arrayUnique(nation.commanders);
				// Utils.arrayUnique(nation.capunits);
				// Utils.arrayUnique(nation.capcommanders);
				//should do it..?

				// nation.gems = '';
				// foreach (k in gemkeys) {
				// 	if (gemkeys[k])
				// 		nation.gems += '+' + string(gemkeys[k]) + k;
				// }
			}

			// foreach (var nation in _nations)
			// {
			// 	//////////////////////////////////////////////////
			// 	// associate units with this nation
			// 	//  if unit is already associated with a nation it creates a duplicate (with incremented id: +.01)
			// 	/////////////////////////////////////////////////
			// 	var iterations = new Dictionary<string, List<int>>
			// 	{
			// 		{ "unit", nation.units },
			// 		{ "commander", nation.commanders },
			// 		{ "cmdr (foreign)", nation.foreigncommanders },
			// 		{ "unit (foreign)", nation.foreignunits },
			// 		{ "unit (forest)", nation.forestrec },
			// 		{ "cmdr (forest)", nation.forestcom },
			// 		{ "unit (mountain)", nation.mountainrec },
			// 		{ "cmdr (mountain)", nation.mountaincom },
			// 		{ "unit (swamp)", nation.swamprec },
			// 		{ "cmdr (swamp)", nation.swampcom },
			// 		{ "unit (waste)", nation.wasterec },
			// 		{ "cmdr (waste)", nation.wastecom },
			// 		{ "unit (cave)", nation.caverec },
			// 		{ "cmdr (cave)", nation.cavecom },
			// 		{ "unit (coast)", nation.coastrec },
			// 		{ "cmdr (coast)", nation.coastcom },
			// 		{ "unit (plains)", nation.plainsrec },
			// 		{ "cmdr (plains)", nation.plainscom },
			// 		{ "unit (land)", nation.landunit },
			// 		{ "cmdr (land)", nation.landcom },
			// 		{ "hero (unique)", nation.heroes },
			// 		{ "hero (multi)", nation.multiheroes },
			// 		{ "unit (u-water)", nation.uwunit },
			// 		{ "cmdr (u-water)", nation.uwcom },
			// 		{ "unit (cap only)", nation.capunits },
			// 		{ "cmdr (cap only)", nation.capcommanders },
			// 		{ "unit (future cap only)", nation.futurecapunits },
			// 		{ "cmdr (future cap only)", nation.futurecapcommanders }
			// 	};
			// 	
			// 	foreach (var iter in iterations) {
			// 		var arr = iter.Value;
			// 		foreach (var unitId in arr)
			// 		{
			// 			var u = DomEdit.I.GameData.baseUTable.GetData("id", unitId.ToString());
			// 			//~ if (!u) {
			// 				//~ console.log(basekey+' '+arr[i]+' not found (nation '+nation.id+')');
			// 				//~ continue;
			// 			//~ }
			// 			//~ if ((u.nation && u.nation!=o) || (u.type && u.type!=basekey))
			// 				//~ u = modctx.cloneUnit(u);
			// 			//~
			// 			//~ u.type = basekey;
			// 			//~ u.nation = o;
			// 			//~ u.nationname = nation.shortname;
			// 			//~ delete u.nations;
			//
			// 		///////////////////////////////
			// 			if (u == null) {
			// 				// console.log(basekey+' '+arr[i]+' not found (nation '+nation.id+')');
			// 				continue;
			// 			}
			// 			
			// 			if (u.typechar && u.typechar!=basekey) {
			// 				//find right version of this unit
			// 				var newu = modctx.getUnitOfType(u, basekey);
			// 				if (newu) u = newu;
			// 				else {
			// 					u = modctx.cloneUnit(u);
			// 					u.nations = {};
			// 					u.eracodes = {};
			// 				}
			// 			}
			// 			u.typechar = basekey;
			// 			if (u.typechar.indexOf('unit') != -1) {
			// 				if (!u.type || u.type == 'c') {
			// 					u.type = 'u';
			// 					MUnit.autocalc(u);
			// 				}
			// 			} else {
			// 				if (!u.type || u.type == 'u') {
			// 					u.type = 'c';
			// 					MUnit.autocalc(u);
			// 				}
			// 			}
			// 			u.nations = u.nations || {};
			// 			if (u.nations[nation.id]) continue;
			//
			// 			u.nations[nation.id] = o;
			// 			u.eracodes = u.eracodes || {};
			// 			u.eracodes[nation.eracode] = true;
			//
			// 			//nationname
			// 			var ncount=0; for (var k in u.nations) ncount++;
			// 			if (ncount == 1)
			// 				u.nationname = nation.shortname;
			// 			else
			// 				u.nationname = 'various ('+ncount+')';
			//
			// 			// Marverni gets Iron Boars
			// 			if (parseInt(nation.id) == 8) {
			// 				var ironBoar = modctx.unitlookup[1808];
			// 				ironBoar.nationname = nation.shortname;
			// 			}
			// 		}
			// 	}
			// }
		}


	}

}