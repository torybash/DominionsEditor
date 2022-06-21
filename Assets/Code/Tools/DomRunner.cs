using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Core;
using Dom;
using Utility;
using Debug = UnityEngine.Debug;

namespace Tools
{

	public class DomRunner
	{
		readonly MapManager map;

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

		void CreateGameFolder ()
		{
			var folderPath = Paths.SavedGamesFolderPath + Constants.GameName;
			if (Directory.Exists(folderPath))
			{
				Directory.Delete(folderPath, true);
			}
		
			Directory.CreateDirectory(folderPath);

			foreach (var player in map.map.players)
			{
				var newPretenderFilePath = $"{Paths.SavedGamesFolderPath}{Constants.GameName}\\{player.Nation.fileNameBase}.2h";
			
				File.Copy(player.Pretender.filePath, newPretenderFilePath);
			}
		}

		void RunCreateGameProcess ()
		{
			Debug.Log($"Running game process using game {Constants.GameName} and map {Constants.MapName} ");
			var args = new List<string>();

			int    eraNumber = Prefs.Era.Get();
			args.Add($"--newgame {Constants.GameName}");
			args.Add($"--mapfile {Constants.MapName}.map");
			args.Add($"--era {eraNumber}");
			args.Add($"--conqall"); // Win by eliminating all opponents only
			// args.Add($"--thrones 0 2 0");               // Win by eliminating all opponents only
			// args.Add($"--requiredap 4");               // Win by eliminating all opponents only
			// args.Add($"--enablemod {Constants.ModName}.dm"); 
		
			foreach (var player in map.map.players)
			{
				if (player.Type == PlayerType.Closed) throw new Exception("Closed player type not handled!");
				if (player.Type == PlayerType.Human) continue;

				var aiName = PlayerTypeUtil.GetAiName(player.Type);
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
		}


		void RunPlayGameProcess ()
		{
			var start = new ProcessStartInfo
			{
				Arguments = $"{Constants.GameName}",
				FileName  = Prefs.ExecutablePath.Get(),
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

}