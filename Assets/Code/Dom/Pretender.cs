using System;
using System.IO;
using Core;
using Data;

namespace Dom
{

	[Serializable]
	public class Pretender
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

			var eraName = fileName.Split('_')[0];
			nationName = fileName.Split('_')[1];
			era = eraName switch 
			{
				"early" => 1,
				"mid"   => 2,
				"late"  => 3,
				_       => -1
			} ;

			nation = era != -1 ? DomEdit.I.Nations.GetNationByNameAndEra(nationName, era) : Nation.Independents;
		}
	}

}