namespace Dom
{
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
		public static string GetAiName (PlayerType playerType)
		{

			var aiName = playerType switch
			{
				PlayerType.AiEasy       => "easyai",
				PlayerType.AiStandard   => "normai",
				PlayerType.AiDifficult  => "diffai",
				PlayerType.AiMighty     => "mightyai",
				PlayerType.AiMaster     => "masterai",
				PlayerType.AiImpossible => "impai",
				_                       => "ERROR"
			};
			return aiName;
		}
	}

}