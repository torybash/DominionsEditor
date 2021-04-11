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

	public List<GamePlayer> Players { get; } = new List<GamePlayer>();

	public void Initialize ()
	{
		foreach (var allowedPlayer in Map.MapElements.OfType<AllowedPlayer>())
		{
			AddPlayer(allowedPlayer.NationNum);		
		}
	}
	
	public void AddPlayer(int nationNum)
	{
		var player = new GamePlayer(PlayerType.Human, nationNum);
		Players.Add(player);

		var playerIdx = Players.IndexOf(player);
		Ui.GetMenu<PlayersMenu>().CreateGizmo(playerIdx, player);
	}

	public void RemovePlayer (int playerIdx)
	{
		var player = Players[playerIdx];
		Players.Remove(player);
		
		Ui.GetMenu<PlayersMenu>().DestroyGizmo(player);
	}

	public void ChangeNation (int playerIdx, int nationNum)
	{
		var player = Players[playerIdx];
		player.NationNum = nationNum;
		
		Ui.GetMenu<PlayersMenu>().UpdateGizmo(player);
	}
	
	public void IncrementType (int playerIdx, int change)
	{
		var player = Players[playerIdx];
		var newType = player.Type;
		int newTypeInt = ((int)newType + change) % Enum.GetValues(typeof(PlayerType)).Length;
		player.Type = (PlayerType) newTypeInt;
		
		Ui.GetMenu<PlayersMenu>().UpdateGizmo(player);
	}
	

	public void RunMap ()
	{
		var mapRunner = new MapRunner(Map, this);
		mapRunner.Run();
	}
}