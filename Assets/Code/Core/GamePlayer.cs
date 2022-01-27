public class GamePlayer
{
	public PlayerType Type { get; set; }
	public Nation Nation { get; set; } = -1;
	public int CapitalProvinceNum { get; set; } = -1;
	public Pretender Pretender { get; set; }

	public GamePlayer (PlayerType type)
	{
		Type = type;
	}
	
	public GamePlayer (PlayerType type, Nation nation)
	{
		Type = type;
		Nation = nation;
	}
}