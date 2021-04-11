public enum PlayerType
{
	Closed,
	Human,
	AiEasy,
	AiStandard,
	AiDifficult,
	AiMighty,
	AiMaster,
	AiImpossible,
}

public static class PlayerTypeUtil
{
	public static string GetAiName (GamePlayer player)
	{

		var aiName = player.Type switch
		{
			PlayerType.AiEasy => "easyai",
			PlayerType.AiStandard => "normai",
			PlayerType.AiDifficult => "diffai",
			PlayerType.AiMighty => "mightyai",
			PlayerType.AiMaster => "masterai",
			PlayerType.AiImpossible => "impai",
			_ => "ERROR"
		};
		return aiName;
	}
}