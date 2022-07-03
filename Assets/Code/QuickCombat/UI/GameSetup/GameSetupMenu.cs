using System.Collections.Generic;
using QuickCombat.UI.Popups;
using TMPro;
using UnityEngine.UI;

namespace QuickCombat.UI
{
	
	/// <summary>
	/// Set era and nations
	/// </summary>
	public class GameSetupMenu : Menu
	{
		public TMP_Dropdown         eraDropdown;
		public List<NationSelector> nationSelectors;
		public Button startButton;
	}

}