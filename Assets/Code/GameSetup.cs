using System;
using System.Collections.Generic;
using System.Linq;

public class GameSetup
{
	private MapManager Map { get; }
	private UiManager Ui { get; }
	
	public GameSetup (MapManager mapManager, UiManager ui)
	{
		Map = mapManager;
		Ui = ui;
	}

	public List<GamePlayer> Players  => Map.Players;

	public void Initialize ()
	{
		for (int i = 0; i < Map.Players.Count; i++)
		{
			var player = Map.Players[i];
			Ui.Get<PlayersMenu>().CreateGizmo(i, player);
		}
	}

	public void RemovePlayer (int playerIdx)
	{
		var player = Players[playerIdx];
		Players.Remove(player);
		
		Ui.Get<PlayersMenu>().DestroyGizmo(player);
	}

	public void ChangeNation (int playerIdx, int nationNum)
	{
		var player = Players[playerIdx];
		player.NationNum = nationNum;
		
		Ui.Get<PlayersMenu>().UpdateGizmo(player);
	}
	
	public void IncrementType (int playerIdx, int change)
	{
		var player = Players[playerIdx];
		var newType = player.Type;
		int newTypeInt = ((int)newType + change) % Enum.GetValues(typeof(PlayerType)).Length;
		if (newTypeInt < 0) newTypeInt += Enum.GetValues(typeof(PlayerType)).Length;
		player.Type = (PlayerType) newTypeInt;
		
		Ui.Get<PlayersMenu>().UpdateGizmo(player);
	}
	

	public void RunMap ()
	{
		var mapRunner = new MapRunner(Map, this);
		mapRunner.Run();
	}
	
	public GamePlayer GetPlayer (int number)
	{
		int idx = number - 1;
		if (idx < 0 || idx >= Players.Count) return null;
		return Players[idx];
	}
}