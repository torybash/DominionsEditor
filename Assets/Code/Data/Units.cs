using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Units
{
	private const string BASE_U_PATH               = "Assets/Data/gamedata/BaseU.csv";
	private const string COAST_LEADERS_BY_NATION   = "Assets/Data/gamedata/coast_leader_types_by_nation.csv";
	private const string COAST_TROOP_BY_NATION     = "Assets/Data/gamedata/coast_leader_types_by_nation.csv";
	private const string FORT_LEADERS_BY_NATION    = "Assets/Data/gamedata/fort_leader_types_by_nation.csv";
	private const string FORT_TROOP_BY_NATION      = "Assets/Data/gamedata/fort_leader_types_by_nation.csv";
	private const string NONFORT_LEADERS_BY_NATION = "Assets/Data/gamedata/nonfort_leader_types_by_nation.csv";
	private const string NONFORT_TROOP_BY_NATION   = "Assets/Data/gamedata/nonfort_leader_types_by_nation.csv";

	private const string ICONS_FOLDER_PATH = "Assets/Art/sprites/";


	private Dictionary<int, Sprite> _spriteMap;




	public Units ()
	{
		var baseU                  = CsvData.LoadCsv(BASE_U_PATH);
		var coastLeadersByNation   = CsvData.LoadCsv(COAST_LEADERS_BY_NATION);
		var coastTroopByNation     = CsvData.LoadCsv(COAST_TROOP_BY_NATION);
		var fortLeadersByNation    = CsvData.LoadCsv(FORT_LEADERS_BY_NATION);
		var fortTroopByNation      = CsvData.LoadCsv(FORT_TROOP_BY_NATION);
		var nonfortLeadersByNation = CsvData.LoadCsv(NONFORT_LEADERS_BY_NATION);
		var nonfortTroopByNation   = CsvData.LoadCsv(NONFORT_TROOP_BY_NATION);

		Debug.Log($"Loaded units {baseU.data.Count}. Attributes: {baseU.data[0].Keys.Count}");

		foreach (var unitData in baseU.data)
		{
			var id   = unitData["id"];
			var name = unitData["name"];

			var elem299 = unitData.Values.ElementAt(299);
			if (!string.IsNullOrEmpty(elem299))
			{
				var elem299Key = unitData.Keys.ElementAt(299);

				Debug.Log($"{name} ({id}) has elem299! ({elem299Key}) {elem299}");

			}

			bool isLeader = coastLeadersByNation.data.Any(x => x["monster_number"] == id)
				|| fortLeadersByNation.data.Any(x => x["monster_number"]           == id)
				|| nonfortLeadersByNation.data.Any(x => x["monster_number"]        == id);

			if (isLeader)
			{
				// Debug.Log($"{name} ({id}) is commander!");
			}
		}
	}

	private List<Monster> GetMonsters ()
	{
		var monsters = new List<Monster>();

		var monsterLines = File.ReadAllLines(BASE_U_PATH);


		return monsters;
	}

	private void LoadMonsters ()
	{
		// Target.Entries.Clear();

		var monsterLines   = File.ReadAllLines(BASE_U_PATH);
		var attributeNames = monsterLines[0].Split('\t');

		for (var i = 1; i < monsterLines.Length; i++)
		{
			var monsterValues = monsterLines[i].Split('\t');


			const int idIdx       = 0;
			const int nameIdx     = 1;
			int       id          = int.Parse(monsterValues[idIdx]);
			var       monsterName = monsterValues[nameIdx];

			// var sprite       = GetSprite(id);
			// var monsterEntry = new MonsterEntry(id, monsterName, sprite);
		}
	}

	// private Sprite GetSprite (int monsterId)
	// {
	//
	// 	if (_spriteMap == null) CreateSpriteMap();
	// 	if (!_spriteMap.TryGetValue(monsterId, out var foundSprite))
	// 	{
	// 		Debug.LogError($"No sprite found for monster with id {monsterId}");
	// 	}
	// 	return foundSprite;
	// }
	//
	// private void CreateSpriteMap ()
	// {
	// 	_spriteMap = new Dictionary<int, Sprite>();
	// 	foreach (var iconPath in Directory.GetFiles(ICONS_FOLDER_PATH))
	// 	{
	// 		if (!iconPath.EndsWith(".png")) continue;
	//
	// 		int lastSepIdx = iconPath.LastIndexOf('/');
	// 		int extIdx     = iconPath.LastIndexOf('.');
	//
	// 		var iconName = iconPath.Substring(lastSepIdx + 1, extIdx - lastSepIdx - 1);
	//
	// 		var idText   = iconName.Split('_')[0];
	// 		int id       = int.Parse(idText);
	// 		var typeText = iconName.Split('_')[1];
	// 		int type     = int.Parse(typeText);
	// 		if (type == 1)
	// 		{
	// 			// var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(iconPath);
	// 			// _spriteMap.Add(id, sprite);
	// 		}
	// 	}
	// }

}