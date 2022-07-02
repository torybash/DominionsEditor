using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

namespace QuickCombat.UI.Popups
{
	
	/// <summary>
	/// Set era and nations
	/// </summary>
	public class GameSetupPopup : Popup
	{
		public TMP_Dropdown         eraDropdown;
		public List<NationSelector> nationSelectors;
		public Button startButton;
	}

}