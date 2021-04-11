using System.Diagnostics;
using System.IO;
using System.Linq;
using Debug = UnityEngine.Debug;
public class MapRunner
{
	private MapManager _man;
	
	public MapRunner (MapManager man)
	{
		_man = man;
	}
	
	public void Run ()
	{
		CreateGameFolder();
		RunCreateGameProcess();
		RunPlayGameProcess();
	}

	private void CreateGameFolder ()
	{
		var allowedPlayers = _man.MapElements.OfType<AllowedPlayer>();
		var folderPath = _man.SavedGamesFolderPath + Constants.GameName;
		Directory.Delete(folderPath, true);
		Directory.CreateDirectory(folderPath);

		foreach (var allowedPlayer in allowedPlayers)
		{
			var nationEntry = _man.Nations.GetNationEntry(allowedPlayer.NationNum);
			var nationPath = $"{_man.SavedGamesFolderPath}{Constants.GameName}\\{nationEntry.PretenderFileName}.2h";

			File.Copy(nationEntry.SamplePretenderFilePath, nationPath);
		}
	}
	
	private void RunCreateGameProcess ()
	{
		var start = new ProcessStartInfo
		{
			Arguments = $"--newgame {Constants.GameName} --mapfile {_man.SavedMapFileName} --era 3", // ", --easyai 102
			FileName = @"G:\Games\steamapps\common\Dominions5\Dominions5.exe",
		};

		int exitCode;
		
		using (Process proc = Process.Start(start)) // Run the external process & wait for it to finish
		{
			proc.WaitForExit();
			exitCode = proc.ExitCode;
		}
		Debug.Log("Create game process. ExitCode: "+ exitCode);
	}
	
	private void RunPlayGameProcess ()
	{
		var start = new ProcessStartInfo
		{
			Arguments = $"{Constants.GameName}",
			FileName = @"G:\Games\steamapps\common\Dominions5\Dominions5.exe",
		};

		int exitCode;
		using (Process proc = Process.Start(start)) // Run the external process & wait for it to finish
		{
			proc.WaitForExit();
			exitCode = proc.ExitCode;
		}
		Debug.Log("Run game process. ExitCode: " + exitCode);
	}
}