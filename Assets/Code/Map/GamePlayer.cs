public class GamePlayer
{
	public PlayerType Type { get; set; }
	public int NationNum { get; set; } = -1;
	public int CapitalProvinceNum { get; set; } = -1;
	public Pretender Pretender { get; set; }

	public GamePlayer (PlayerType type)
	{
		Type = type;
	}
	
	public GamePlayer (PlayerType type, int nationNum, int capitalProvinceNum)
	{
		Type = type;
		NationNum = nationNum;
		CapitalProvinceNum = capitalProvinceNum;
	}
}