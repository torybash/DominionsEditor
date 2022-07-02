using Data.Tables;
using UnityEngine;

namespace QuickCombat
{
	public class SessionManager : MonoBehaviour
	{
		public MenuControl menu;
		public TableControl tableControl;

		// [NonSerialized] public DominionsData dom;

		void Start ()
		{
			tableControl.Init();
			menu.Init();
			
			DominionsData.Load();
			
			//TODO Start from boot scene and load appropriate scene
			//TODO Load data config menu on first launch

			//TODO Load previous session
			
			var session = new Session();
			// session.Run();
			
			var gameSetup = new GameSetupHandler(session);
			gameSetup.Run();
		}
	}

}