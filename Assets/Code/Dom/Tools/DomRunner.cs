using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Debug = UnityEngine.Debug;

public class DomRunner
{
	private readonly MapManager map;

	public DomRunner (MapManager map)
	{
		this.map = map;
	}
	
	public void Run ()
	{
		CreateGameFolder();
		RunCreateGameProcess();
		RunPlayGameProcess();
	}

	private void CreateGameFolder ()
	{
		var folderPath = map.SavedGamesFolderPath + Constants.GameName;
		if (Directory.Exists(folderPath))
		{
			Directory.Delete(folderPath, true);
		}
		
		Directory.CreateDirectory(folderPath);

		foreach (var player in map.Map.Players)
		{
			var nationEntry          = DomEdit.I.nations.GetNationEntry(player.Nation);
			var newPretenderFilePath = $"{map.SavedGamesFolderPath}{Constants.GameName}\\{nationEntry.PretenderFileName}.2h";
			
			File.Copy(player.Pretender.filePath, newPretenderFilePath);
		}
	}
	
	private void RunCreateGameProcess ()
	{
		var args = new List<string>();

		int eraNumber = Prefs.Era.Get();
		string debugMod = $"{Constants.ModName}.dm";
		args.Add($"--newgame {Constants.GameName}");
		args.Add($"--mapfile {map.SavedMapFileName}");
		args.Add($"--era {eraNumber}");
		args.Add($"--conqall");               // Win by eliminating all opponents only
		// args.Add($"--thrones 0 1 0");               // Win by eliminating all opponents only
		args.Add($"--enablemod {debugMod}"); 
		
		foreach (var player in map.Map.Players)
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

		// int exitCode;
		using (Process proc = Process.Start(start)) // Run the external process & wait for it to finish
		{
			// proc.WaitForExit();
			// exitCode = proc.ExitCode;
		}
		// Debug.Log("Run game process. ExitCode: " + exitCode);
	}
}