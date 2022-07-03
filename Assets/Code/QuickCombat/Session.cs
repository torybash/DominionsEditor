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
			players.Add(new Player(PlayerSide.Left));
			players.Add(new Player(PlayerSide.Right));
		}

		public void RunGameSetup ()
		{
			var gameSetup = new GameSetupHandler(this);
			gameSetup.Run();
		}

		public void RunCombatEdit ()
		{
			var combatEdit = new CombatEditHandler(this);
			combatEdit.Run();
		}

		public void SetPlayerPretender (PlayerSide side, Pretender pretender)
		{
			var player = players[(int)side];
			player.pretender = pretender;
			player.nation    = DomData.Nations.GetNationById(pretender.nation.id);

			// SavePretenders();
		}
	}



}