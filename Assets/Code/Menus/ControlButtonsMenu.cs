using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlButtonsMenu : Menu
{
	public void ClickedSettings ()
	{
		Ui.Get<SettingsMenu>().Show();
	}
}
