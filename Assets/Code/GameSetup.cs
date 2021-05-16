using System;
using System.Collections.Generic;

public class GameSetup
{
	private UiManager Ui { get; }
	
	public GameSetup (UiManager ui)
	{
		Ui = ui;
	}

	public List<GamePlayer> Players { get; set;  } = new List<GamePlayer>();

	public void AddNation ()
	{
		var player = new GamePlayer(PlayerType.Human);
		Players.Add(player);
		
		Ui.Get<PlayersMenu>().CreateGizmo(player);
	}
	
	public void RemovePlayer (GamePlayer player)
	{
		Players.Remove(player);
		
		Ui.Get<PlayersMenu>().DestroyGizmo(player);
	}

	public void ChangeNation (GamePlayer player, int nationNum)
	{
		player.Nation = nationNum;
		
		Ui.Get<PlayersMenu>().UpdateGizmo(player);
	}
	
	public void IncrementType (GamePlayer player, int change)
	{
		var newType = player.Type;
		int newTypeInt = ((int)newType + change) % Enum.GetValues(typeof(PlayerType)).Length;
		if (newTypeInt < 0) newTypeInt += Enum.GetValues(typeof(PlayerType)).Length;
		player.Type = (PlayerType) newTypeInt;
		
		Ui.Get<PlayersMenu>().UpdateGizmo(player);
	}

	public GamePlayer GetPlayer (int number)
	{
		int idx = number - 1;
		if (idx < 0 || idx >= Players.Count) return null;
		return Players[idx];
	}
}