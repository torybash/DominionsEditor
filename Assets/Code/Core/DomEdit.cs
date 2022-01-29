using UnityEngine;

public class DomEdit : MonoBehaviour
{
	public MonstersTable  monsters;
	public ItemsTable     items;
	public NationsTable   nations;
	public MagicPathTable magicPaths;

	public UiManager  Ui     { get; private set; }
	public MapManager MapMan { get; private set; }

	public static DomEdit I;

	private void Awake ()
	{
		I = this;

		Ui     = new UiManager();
		MapMan = new MapManager();

		Ui.Get<MainMenu>().Show();

		// if (HasDefaultPretenders())
		// {
		// 	MapMan.LoadMap();
		// } else
		// {
		// 	Ui.Get<IntroMenu>().Show();
		// }
		//
		// var maMonsters = new Monsters();
	}

	public static bool HasDefaultPretenders ()
	{
		if (string.IsNullOrEmpty(Prefs.DefaultPretenderA.Get())) return false;
		if (string.IsNullOrEmpty(Prefs.DefaultPretenderB.Get())) return false;

		return true;
	}
}