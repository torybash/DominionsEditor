using System;
using System.IO;

public class Pretender
{
	public string FilePath { get; set; }
	public string FileName { get; set; }
	public string NationName { get; set; }
	public Nation Nation { get; set; }
	public int Era { get; set; }

	public Pretender (string pretenderFilePath)
	{
		FilePath = pretenderFilePath;
		FileName = Path.GetFileName(pretenderFilePath);

		NationName = FileName.Split('_')[1];
		var eraName = FileName.Split('_')[0];
		Era = eraName switch 
		{
			"early" => 1,
			"mid" => 2,
			"late" => 3,
			_ => throw new ArgumentOutOfRangeException()
		} ;
	}
}