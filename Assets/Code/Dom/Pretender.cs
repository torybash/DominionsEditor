using System;
using System.IO;
using Core;
using Data;
using QuickCombat;

namespace Dom
{

	[Serializable]
	public class Pretender
	{
		public string filePath;
		public string nationName;
		public Nation nation;
		public int    era;


		public Pretender (string pretenderFilePath)
		{
			filePath = pretenderFilePath;

			string fileName = Path.GetFileName(filePath);
			string eraName  = fileName.Split('_')[0];
			nationName = fileName.Split('_')[1];
			era = eraName switch
			{
				"early" => 1,
				"mid"   => 2,
				"late"  => 3,
				_       => -1
			};

			nation = era != -1 ? DomData.Nations.GetNationByNameAndEra(nationName, era) : Nation.Independents;
		}
	}

}