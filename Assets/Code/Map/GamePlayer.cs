public class GamePlayer
{
	public PlayerType Type { get; set; }
	public int NationNum { get; set; }
	public int CapitalProvinceNum { get; set; }

	public GamePlayer (PlayerType type, int nationNum, int capitalProvinceNum)
	{
		Type = type;
		NationNum = nationNum;
		CapitalProvinceNum = capitalProvinceNum;
	}
}