public class ControlButtonsMenu : Menu
{
	public void ClickedSettings ()
	{
		DomEdit.I.Ui.Get<IntroMenu>().Show();
	}
}
