using Data;
using QuickCombat.Gizmos;

namespace QuickCombat
{
	public class NationPanelHandler
	{
		readonly Player      _player;
		readonly NationPanel _panel;
		
		public NationPanelHandler (Player player, NationPanel panel)
		{
			_player = player;
			_panel  = panel;
		}

		public void Run ()
		{
			_panel.SetPlayer(_player);
		}

		public void AddCommander (UnitData commander)
		{
			var commanderGizmo = M.CreateGizmo<CommanderGizmo>(_panel.commanderGroup);
			commanderGizmo.SetData(commander.id);
		}
	}
}