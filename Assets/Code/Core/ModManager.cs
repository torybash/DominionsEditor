using Mod;
using UI.Menus;

namespace Core
{

	public class ModManager
	{
		public Mod.Mod Mod { get; set; }
		
		public void LoadMod ()
		{
			var modFile = DomFile.LoadPath(Paths.ModFilePath);
			Mod = ModLoader.Load(modFile);

			

		}
		
		public void SaveMod ()
		{
		
			// public string MapFolderPath        => $"{Prefs.DataFolderPath.Get()}/maps/{Constants.MapName}/";
			//  public string MapFilePath => $"{MapFolderPath}/{Constants.MapName}.map";
			// var       mapSaver = new MapSaver();
			// var       savePath = Paths.MapFilePath;
			//
			// Prefs.PreviousMapPath.Set(savePath);
			//
			// mapSaver.SaveMap(Map, Paths.MapFilePath);
			// SavedMapFileName = Path.GetFileName(Paths.MapFilePath);
		}
	}

}