using Controls;
using Data;
using Data.Tables;
using QuickCombat;
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
		// public MagicPathTable magicPaths;
		// public IconsTable     icons;
		public MapControls    controls;

		public UiManager  Ui       { get; private set; }
		public MapManager MapMan   { get; private set; }
		public ModManager ModMan   { get; private set; }

		public static DomEdit I;

		void Awake ()
		{
			I = this;

			Ui     = new UiManager();
			MapMan = new MapManager();
			ModMan = new ModManager();
			
			DominionsData.Load();

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