using System.Collections.Generic;
using Dom;

namespace QuickCombat
{
	/// <summary>
	/// Contains all data for current sessions (nations, units, province settings, mod settings, etc)
	/// </summary>
	public class Session
	{
		//TODO Save data as json. Load data on boot.
		public List<Player> players;

		public Session ()
		{
			players = new List<Player>();
			players.Add(new Player());
			players.Add(new Player());
		}

		public void Run ()
		{
			var nationPanelsControl = new NationPanelsHandler();
			

		}

		public void SetPlayerPretender (PlayerSide side, Pretender pretender)
		{
			var player = players[(int)side];
			player.pretender = pretender;
			player.nation    = D.Nations.GetNationById(pretender.nation.id);
			
			// SavePretenders();
		}
	}

}