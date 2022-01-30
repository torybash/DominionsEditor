using System;
using System.Collections.Generic;
using System.Linq;
using Core;

namespace Data
{

	public class Nations
	{
		private const int ID_IDX             = 0;
		private const int NAME_IDX           = 1;
		private const int EPITHET_IDX        = 2;
		private const int FILE_NAME_BASE_IDX = 4;
		private const int ERA_IDX            = 5;

		private readonly CsvData      _nationsData;
		private readonly List<Nation> _nations;

		private const string NATIONS_PATH = "Assets/Data/gamedata/nations.csv";

		public Nations ()
		{
			_nationsData = CsvData.LoadCsv(NATIONS_PATH);
			_nations     = LoadNations();
		}

		private List<Nation> LoadNations ()
		{
			var nations = new List<Nation>();
			foreach (var nationData in _nationsData.data)
			{
				int    id           = int.Parse(nationData[nameof(Nation.id)]);
				string name         = nationData[nameof(Nation.name)];
				string epithet      = nationData[nameof(Nation.epithet)];
				string fileNameBase = nationData[nameof(Nation.file_name_base)];
				int    era          = int.Parse(nationData[nameof(Nation.era)]);

				var nation = new Nation { id = id, name = name, epithet = epithet, file_name_base = fileNameBase, era = era };

				nation.icon = DomEdit.I.icons.GetNationIcon(nation.file_name_base);

				nations.Add(nation);
			}
			return nations;
		}

		public List<Nation> GetAll ()
		{
			return _nations;
		}

		public Nation GetNationByNameAndEra (string nationName, int era)
		{
			var entry = _nations.SingleOrDefault(x => x.name.Equals(nationName, StringComparison.OrdinalIgnoreCase) && x.era == era);
			return entry;
		}
	}

}