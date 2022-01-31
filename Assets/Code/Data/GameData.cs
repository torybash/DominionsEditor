namespace Data
{

	public class GameData
	{
		private const string NATIONS_PATH = "Assets/Data/gamedata/nations.csv";
		
		private const string BASE_U = "Assets/Data/gamedata/BaseU.csv";
		private const string BASE_I = "Assets/Data/gamedata/BaseI.csv";

		private const string COAST_LEADERS_BY_NATION   = "Assets/Data/gamedata/coast_leader_types_by_nation.csv";
		private const string COAST_TROOP_BY_NATION     = "Assets/Data/gamedata/coast_troop_types_by_nation.csv";
		private const string FORT_LEADERS_BY_NATION    = "Assets/Data/gamedata/fort_leader_types_by_nation.csv";
		private const string FORT_TROOP_BY_NATION      = "Assets/Data/gamedata/fort_troop_types_by_nation.csv";
		private const string NONFORT_LEADERS_BY_NATION = "Assets/Data/gamedata/nonfort_leader_types_by_nation.csv";
		private const string NONFORT_TROOP_BY_NATION   = "Assets/Data/gamedata/nonfort_troop_types_by_nation.csv";

		private const string ATTRIBUTES_BY_NATION = "Assets/Data/gamedata/attributes_by_nation.csv";

		private const string REALMS      = "Assets/Data/gamedata/realms.csv";
		private const string MAGIC_SITES = "Assets/Data/gamedata/MagicSites.csv";

		public readonly CsvTable nationsTable;
		public readonly CsvTable unitsTable;
		public readonly CsvTable itemsTable;
		public readonly CsvTable coastLeadersByNationTable;
		public readonly CsvTable coastTroopByNationTable;
		public readonly CsvTable fortLeadersByNationTable;
		public readonly CsvTable fortTroopByNationTable;
		public readonly CsvTable nonFortLeadersByNationTable;
		public readonly CsvTable nonFortTroopByNationTable;
		public readonly CsvTable attributesByNationTable;
		public readonly CsvTable realmsTable;
		public readonly CsvTable magicSitesTable;

		public GameData ()
		{
			nationsTable = CsvTable.LoadCsv(NATIONS_PATH);

			unitsTable = CsvTable.LoadCsv(BASE_U);
			itemsTable = CsvTable.LoadCsv(BASE_I);

			coastLeadersByNationTable   = CsvTable.LoadCsv(COAST_LEADERS_BY_NATION);
			coastTroopByNationTable     = CsvTable.LoadCsv(COAST_TROOP_BY_NATION);
			fortLeadersByNationTable    = CsvTable.LoadCsv(FORT_LEADERS_BY_NATION);
			fortTroopByNationTable      = CsvTable.LoadCsv(FORT_TROOP_BY_NATION);
			nonFortLeadersByNationTable = CsvTable.LoadCsv(NONFORT_LEADERS_BY_NATION);
			nonFortTroopByNationTable   = CsvTable.LoadCsv(NONFORT_TROOP_BY_NATION);

			attributesByNationTable = CsvTable.LoadCsv(ATTRIBUTES_BY_NATION);

			realmsTable     = CsvTable.LoadCsv(REALMS);
			magicSitesTable = CsvTable.LoadCsv(MAGIC_SITES);
		}
	}

}