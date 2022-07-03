using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using QuickCombat.Gizmos;
using Utility.Extensions;

namespace QuickCombat
{
	public class UnitsPanelHandler
	{
		readonly CombatEditHandler _combatEdit;
		readonly Session           _session;
		readonly UnitsPanel        _unitsPanel;

		readonly List<UnitGizmo> _unitGizmos = new();
		// readonly List<CommanderGizmo> _commanderGizmos = new();
		// readonly List<TroopGizmo>     _troopGizmos     = new();

		public UnitsPanelHandler (CombatEditHandler combatEdit, Session session, UnitsPanel unitsPanel)
		{
			_combatEdit = combatEdit;
			_session           = session;
			_unitsPanel        = unitsPanel;
		}

		public void Run ()
		{
			foreach (var filterButton in _unitsPanel.filterButtons)
			{
				filterButton.button.onClick.RemoveAllListeners();
				filterButton.button.onClick.AddListener(() => CreateUnitsForFilter(filterButton.filter));
			}

			CreateUnitsForFilter(UnitFilterButton.Filter.Nation);
		}


		void CreateUnitsForFilter (UnitFilterButton.Filter filter)
		{
			List<UnitData> allUnits = new List<UnitData>();
			switch (filter)
			{
				case UnitFilterButton.Filter.Nation:

					if (_combatEdit.selectedPlayer != null)
					{
						Nation selectedNation = _combatEdit.selectedPlayer.nation;
						allUnits = DomData.Units.GetAll()
							.Where(x => x.nations.Any(x => x.id == selectedNation.id))
							.ToList();
					}

					break;
				// case UnitFilterButton.Filter.Favorites:
				// 	break;
				// case UnitFilterButton.Filter.All:
				// 	break;
				default:
					throw new ArgumentOutOfRangeException(nameof(filter), filter, null);
			}
			
			_unitGizmos.SafeDestroyList();

			var commanders = allUnits.Where(x => x.unitType.IsCommander());
			foreach (UnitData commander in commanders)
			{
				var commanderGizmo = M.CreateGizmo<UnitGizmo>(_unitsPanel.commanderGroup);
				commanderGizmo.SetData(commander.id);
				
				commanderGizmo.button.onClick.AddListener(() => OnClickedCommander(commander));
				
				_unitGizmos.Add(commanderGizmo);
				// _unitsPanel.commanderGroup
			}

			var troops     = allUnits.Where(x => x.unitType.IsTroop());
			foreach (UnitData troop in troops)
			{
				var troopGizmo = M.CreateGizmo<UnitGizmo>(_unitsPanel.troopsGroup);
				troopGizmo.SetData(troop.id);
				
				troopGizmo.button.onClick.AddListener(() => OnClickedTroop(troop));

				_unitGizmos.Add(troopGizmo);
				// _unitsPanel.commanderGroup
			}
			// _combatEditHandler.selectedPlayer
			// DomData.Units.GetAll()

			// _unitsPanel.commanderGroup
		}

		void OnClickedTroop (UnitData troop)
		{
			
		}

		void OnClickedCommander (UnitData commander)
		{
			_combatEdit.AddCommanderToPlayer(commander, _combatEdit.selectedPlayer);
		}
	}
}