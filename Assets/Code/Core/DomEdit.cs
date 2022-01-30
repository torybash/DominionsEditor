using Data;
using Data.Tables;
using UI.Menus;
using UnityEngine;
using Utility;

namespace Core
{

	public class DomEdit : MonoBehaviour
	{
		public MonstersTable monsters;
		public ItemsTable    items;
		// public NationsTable   nations;
		public MagicPathTable magicPaths;
		public IconsTable     icons;

		public UiManager  Ui       { get; private set; }
		public MapManager MapMan   { get; private set; }
		public GameData   GameData { get; set; }
		public Nations    Nations  { get; private set; }
		public Units      Units    { get; private set; }

		public static DomEdit I;

		private void Awake ()
		{
			I = this;

			Ui       = new UiManager();
			MapMan   = new MapManager();
			GameData = new GameData();
			Nations  = new Nations();
			Units    = new Units();

			Nations.LoadData();
			Units.ParseData();

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

}