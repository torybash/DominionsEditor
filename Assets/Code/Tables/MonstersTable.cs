using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class MonstersTable : ScriptableObject
{
	[SerializeField] private List<MonsterEntry> monsterEntries;

	private Dictionary<int, MonsterEntry> idMap;
	private Dictionary<string, MonsterEntry> nameMap;
	public List<MonsterEntry> Entries => monsterEntries;
	
	public MonsterEntry GetMonster (int id)
	{
		if (idMap == null)
		{
			idMap = new Dictionary<int, MonsterEntry>();
			foreach (var entry in Entries) idMap.Add(entry.Id, entry);
		}

		return idMap[id];
	}
	
	
	public MonsterEntry GetMonster (string unitName)
	{
		if (nameMap == null)
		{
			nameMap = new Dictionary<string, MonsterEntry>();
			foreach (var entry in Entries) nameMap.Add(entry.Name, entry);
		}

		return nameMap[unitName];
	}



}



[CustomEditor(typeof(MonstersTable))]
public class MonstersTableEditor : Editor
{
	private const string DATA_PATH = "Assets/Data/gamedata/BaseU.csv";
	private const string ICONS_FOLDER_PATH = "Assets/Art/sprites/";

	private MonstersTable Target => (MonstersTable)target;
	
	private Dictionary<int, Sprite> _spriteMap;

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Load"))
		{
			LoadMonsters();
			
			EditorUtility.SetDirty(Target);
		}
	}

	private void LoadMonsters ()
	{
		Target.Entries.Clear();

		var monsterLines = File.ReadAllLines(DATA_PATH);
		var attributeNames = monsterLines[0].Split('\t');

		for (var i = 1; i < monsterLines.Length; i++)
		{
			var monsterValues = monsterLines[i].Split('\t');


			const int ID_IDX = 0;
			const int NAME_IDX = 1;
			int id = int.Parse(monsterValues[ID_IDX]);
			var monsterName = monsterValues[NAME_IDX];
			
			var sprite = GetSprite(id);
			var monsterEntry = new MonsterEntry(id, monsterName, sprite );
			Target.Entries.Add(monsterEntry);
		}
	}

	private Sprite GetSprite (int monsterId)
	{

		if (_spriteMap == null) CreateSpriteMap();
		if (!_spriteMap.TryGetValue(monsterId, out var foundSprite))
		{
			Debug.LogError($"No sprite found for monster with id {monsterId}");
		}
		return foundSprite;
	}
	
	private void CreateSpriteMap ()
	{
		_spriteMap = new Dictionary<int, Sprite>();
		foreach (var iconPath in Directory.GetFiles(ICONS_FOLDER_PATH))
		{
			if (!iconPath.EndsWith(".png")) continue;
			
			int lastSepIdx = iconPath.LastIndexOf('/');
			int extIdx = iconPath.LastIndexOf('.');

			var iconName = iconPath.Substring(lastSepIdx + 1, extIdx - lastSepIdx - 1);

			var idText = iconName.Split('_')[0];
			int id = int.Parse(idText);
			var typeText = iconName.Split('_')[1];
			int type = int.Parse(typeText);
			if (type == 1)
			{
				var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(iconPath);
				_spriteMap.Add(id, sprite);
			}
		}
	}

}