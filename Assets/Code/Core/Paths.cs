using Utility;

namespace Core
{

	public static class Paths
	{

		public static string SavedGamesFolderPath => $"{Prefs.DataFolderPath.Get()}savedgames/";
		public static string PretendersFolderPath => $"{Prefs.DataFolderPath.Get()}savedgames/newlords/";
		public static string MapsFolderPath       => $"{Prefs.DataFolderPath.Get()}maps/";
		public static string ModsFolderPath       => $"{Prefs.DataFolderPath.Get()}mods/";
		public static string MapFilePath          => $"{Prefs.DataFolderPath.Get()}maps/{Constants.MapName}/{Constants.MapName}.map";
		public static string ModFilePath          => $"{Prefs.DataFolderPath.Get()}mods/{Constants.ModName}.map";
		public static string MapFolderPath        => $"{Prefs.DataFolderPath.Get()}maps/{Constants.MapName}/";
	}

}