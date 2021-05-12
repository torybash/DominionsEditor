using System;

public class Pretender
{
	public string FilePath { get; set; }
	public string FileName { get; set; }
	public string NationName { get; set; }
	public Nation Nation { get; set; }

	public Pretender (string pretenderFilePath)
	{
		FilePath = pretenderFilePath;
		FileName = pretenderFilePath.Substring(pretenderFilePath.LastIndexOf("/", StringComparison.Ordinal) + 1);

		NationName = FileName.Split('_')[1];
	}
}