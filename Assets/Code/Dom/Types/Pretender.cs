using System;
using System.IO;

public struct Pretender
{
	public string filePath;
	public string fileName;
	public string nationName;
	public Nation nation;
	public int    era;

	public Pretender (string pretenderFilePath)
	{
		filePath = pretenderFilePath;
		fileName = Path.GetFileName(pretenderFilePath);

		nationName = fileName.Split('_')[1];
		var eraName = fileName.Split('_')[0];
		era = eraName switch 
		{
			"early" => 1,
			"mid" => 2,
			"late" => 3,
			_ => throw new ArgumentOutOfRangeException()
		} ;
		
		nation = DomEdit.I.nations.GetNationByNameAndEra(nationName, era);
	}
}