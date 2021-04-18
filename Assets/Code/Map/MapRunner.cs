using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Debug = UnityEngine.Debug;
public class MapRunner
{
	private readonly MapManager _man;
	private readonly GameSetup _game;

	public MapRunner (MapManager man, GameSetup gameSetup)
	{
		_man = man;
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
		var folderPath = _man.SavedGamesFolderPath + Constants.GameName;
		Directory.Delete(folderPath, true);
		Directory.CreateDirectory(folderPath);

		foreach (var player in _man.Players)
		{
			var nationEntry = _man.Nations.GetNationEntry(player.NationNum);
			var nationPath = $"{_man.SavedGamesFolderPath}{Constants.GameName}\\{nationEntry.PretenderFileName}.2h";

			File.Copy(nationEntry.SamplePretenderFilePath, nationPath);
		}
	}
	
	private void RunCreateGameProcess ()
	{
		var args = new List<string>();

		int HARDCODED_ERA_NUMBER_TODO = 3;
		args.Add($"--newgame {Constants.GameName}");
		args.Add($"--mapfile {_man.SavedMapFileName}");
		args.Add($"--era {HARDCODED_ERA_NUMBER_TODO}");
		args.Add($"--conqall"); // Win by eliminating all opponents only
		
		foreach (var player in _game.Players)
		{
			if (player.Type == PlayerType.Closed) throw new Exception("Closed player type not handled!");
			if (player.Type == PlayerType.Human) continue;

			var aiName = PlayerTypeUtil.GetAiName(player);
			args.Add($"--{aiName} {player.NationNum}");
		}

		var start = new ProcessStartInfo
		{
			Arguments = string.Join(" ", args),
			// Arguments = $"--newgame {Constants.GameName} --mapfile {_man.SavedMapFileName} --era 3 {playerAiArgs}", // ", --easyai 102
			// FileName = @"G:\Games\steamapps\common\Dominions5\Dominions5.exe",
			FileName = PrefManager.ExecutablePath.Get(),
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
			FileName = PrefManager.ExecutablePath.Get(),
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