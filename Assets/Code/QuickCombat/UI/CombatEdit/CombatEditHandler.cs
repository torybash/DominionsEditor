using System.Collections.Generic;
using Data;

namespace QuickCombat
{
	public class CombatEditHandler
	{
		readonly Session _session;

		List<NationPanelHandler> _nationPanels = new();

		public Player selectedPlayer;

		public CombatEditHandler (Session session)
		{
			_session = session;
		}

		public void Run ()
		{
			selectedPlayer = _session.players[0];
			
			var menu = M.ShowMenu<CombatEditMenu>();
			
			//Nation panels
			for (int i = 0; i < menu.nationPanels.Length; i++)
			{
				Player      player = _session.players[i];
				NationPanel panel  = menu.nationPanels[i];

				var nationPanelHandler = new NationPanelHandler(player, panel);
				nationPanelHandler.Run();
				_nationPanels.Add(nationPanelHandler);
			}
			
			//Units panel
			var unitPanelHandler = new UnitsPanelHandler(this, _session, menu.unitPanel);
			unitPanelHandler.Run();

			//Items panel
			var itemPanelHandler = new ItemPanelHandler(_session, menu.itemPanel);

			
		}


		public void AddCommanderToPlayer (UnitData commander, Player player)
		{
			player.AddCommander(commander);

			var nationPanel = _nationPanels[(int)player.side];
			nationPanel.AddCommander(commander);
		}
	}

}