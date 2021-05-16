using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Debug = UnityEngine.Debug;

public class MapRunner
{
	private readonly MapManager _map;
	private readonly GameSetup _game;

	public MapRunner (MapManager map, GameSetup gameSetup)
	{
		_map = map;
		_game = gameSetup;
	}
	
	public void Run ()
	{
		CreateGameFolder();
		RunCreateGameProcess();
		RunPlayGameProcess();
	}

	private void CreateGameFolder ()
	{
		var folderPath = _map.SavedGamesFolderPath + Constants.GameName;
		if (Directory.Exists(folderPath))
		{
			Directory.Delete(folderPath, true);
		}
		
		Directory.CreateDirectory(folderPath);

		foreach (var player in _game.Players)
		{
			var nationEntry = _map.Nations.GetNationEntry(player.Nation);
			var newPretenderFilePath = $"{_map.SavedGamesFolderPath}{Constants.GameName}\\{nationEntry.PretenderFileName}.2h";
			
			File.Copy(player.Pretender.FilePath, newPretenderFilePath);
		}
	}
	
	private void RunCreateGameProcess ()
	{
		var args = new List<string>();

		int eraNumber = Prefs.Era.Get();
		string debugMod = $"{Constants.ModName}.dm";
		args.Add($"--newgame {Constants.GameName}");
		args.Add($"--mapfile {_map.SavedMapFileName}");
		args.Add($"--era {eraNumber}");
		args.Add($"--conqall");               // Win by eliminating all opponents only
		args.Add($"--enablemod {debugMod}"); 
		
		foreach (var player in _game.Players)
		{
			if (player.Type == PlayerType.Closed) throw new Exception("Closed player type not handled!");
			if (player.Type == PlayerType.Human) continue;

			var aiName = PlayerTypeUtil.GetAiName(player);
			args.Add($"--{aiName} {player.Nation}");
		}

		var start = new ProcessStartInfo
		{
			Arguments = string.Join(" ", args),
			// Arguments = $"--newgame {Constants.GameName} --mapfile {_man.SavedMapFileName} --era 3 {playerAiArgs}", // ", --easyai 102
			// FileName = @"G:\Games\steamapps\common\Dominions5\Dominions5.exe",
			FileName = Prefs.ExecutablePath.Get(),
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
			FileName = Prefs.ExecutablePath.Get(),
		};

		int exitCode;
		using (Process proc = Process.Start(start)) // Run the external process & wait for it to finish
		{
			// proc.WaitForExit();
			// exitCode = proc.ExitCode;
		}
		// Debug.Log("Run game process. ExitCode: " + exitCode);
	}
}