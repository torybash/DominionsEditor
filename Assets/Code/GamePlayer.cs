public class GamePlayer
{
	public PlayerType Type { get; set; }
	public int NationNum { get; set; }

	public GamePlayer (PlayerType type, int nationNum)
	{
		Type = type;
		NationNum = nationNum;
	}
}