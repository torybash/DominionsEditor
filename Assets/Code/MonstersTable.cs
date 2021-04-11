using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class MonstersTable : ScriptableObject
{
	[SerializeField] private List<Entry> monsterEntries;

	private Dictionary<int, Entry> idMap;
	private Dictionary<string, Entry> nameMap;
	private Dictionary<int, Sprite> _spriteMap;
	public List<Entry> Entries => monsterEntries;
	
	public Entry GetMonster (int id)
	{
		if (idMap == null)
		{
			idMap = new Dictionary<int, Entry>();
			foreach (var entry in Entries) idMap.Add(entry.Id, entry);
		}

		return idMap[id];
	}
	
	
	public Entry GetMonster (string unitName)
	{
		if (nameMap == null)
		{
			nameMap = new Dictionary<string, Entry>();
			foreach (var entry in Entries) nameMap.Add(entry.Name, entry);
		}

		return nameMap[unitName];
	}
	
	
	[System.Serializable]
	public class Entry
	{
		[SerializeField] private int id;
		[SerializeField] private string name;
		[SerializeField] private Sprite sprite;

		public int Id => id;
		public string Name => name;
		public Sprite Sprite => sprite;

		public Entry (int id, string name, Sprite sprite)
		{
			this.id = id;
			this.name = name;
			this.sprite = sprite;
		}
	}

	public Entry AddMonster (int monsterId, string monsterName)
	{
		var sprite = GetSprite(monsterId);
		
		var monsterEntry = new Entry(monsterId, monsterName, sprite );
		Entries.Add(monsterEntry);

		return monsterEntry;
	}
	
	private Sprite GetSprite (int monsterId)
	{
		const string IconsFolderPath = "Assets/Art/sprites/";

		if (_spriteMap == null) CreateSpriteMap(IconsFolderPath);
		if (!_spriteMap.TryGetValue(monsterId, out var foundSprite))
		{
			Debug.LogError($"No sprite found for monster with id {monsterId}");
		}
		return foundSprite;

	}
	private void CreateSpriteMap (string IconsFolderPath)
	{
		_spriteMap = new Dictionary<int, Sprite>();
		foreach (var iconPath in Directory.GetFiles(IconsFolderPath))
		{

			if (iconPath.EndsWith(".png"))
			{
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

}




[CustomEditor(typeof(MonstersTable))]
public class MonstersEditor : Editor
{

	private MonstersTable Target => (MonstersTable)target;
	
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Load"))
		{
			LoadMonsters();
			
			EditorUtility.SetDirty(Target);
		}
	}

	private const string DataPath = "Assets/Data/gamedata/BaseU.csv";
	private void LoadMonsters ()
	{
		Target.Entries.Clear();

		var monsterLines = File.ReadAllLines(DataPath);
		var attributeNames = monsterLines[0].Split('\t');
		
			
		for (var i = 1; i < monsterLines.Length; i++)
		{
			var monsterValues = monsterLines[i].Split('\t');


			const int ID_IDX = 0;
			const int NAME_IDX = 1;
			int id = int.Parse(monsterValues[ID_IDX]);
			var monsterName = monsterValues[NAME_IDX];

			// var attributes = new Dictionary<string, int>();
			
			Target.AddMonster(id, monsterName);
			
			// var monsterLine = monsterLines[i];
			// if (string.IsNullOrEmpty(monsterLine)) continue;
			//
			// if (monsterLine.StartsWith("\t") && currentMonster != null)
			// {
			// 	monsterLine = monsterLine.TrimStart('\t');
			// 	var split = monsterLine.Split(':');
			// 	var key = split[0];
			// 	var val = split[1];
			// 	//TODO load monster properties into currentMonster
			// } else
			// {
			// 	int startParanIdx = monsterLine.IndexOf('(');
			// 	int endParanIdx = monsterLine.IndexOf(')');
			// 	int idLen = endParanIdx - startParanIdx - 1; 
			// 	string name = monsterLine.Substring(0, startParanIdx);
			// 	string idText = monsterLine.Substring(startParanIdx + 1, idLen);
			// 	int id = int.Parse(idText);
			// 	currentMonster = Target.AddMonster(id, name);
			// }
		}
	}
	
//	private const string DataPath = "Assets/Data/monsters.txt";
	// private void LoadMonsters () 
	// {
	// 	Target.Entries.Clear();
	//
	// 	Monsters.Entry currentMonster = null; 
	// 	var monsterLines = File.ReadAllLines(DataPath);
	// 	for (var i = 0; i < monsterLines.Length; i++)
	// 	{
	// 		var monsterLine = monsterLines[i];
	// 		if (string.IsNullOrEmpty(monsterLine)) continue;
	// 		
	// 		if (monsterLine.StartsWith("\t") && currentMonster != null)
	// 		{
	// 			monsterLine = monsterLine.TrimStart('\t');
	// 			var split = monsterLine.Split(':');
	// 			var key = split[0];
	// 			var val = split[1];
	// 			//TODO load monster properties into currentMonster
	// 		} else
	// 		{
	// 			int startParanIdx = monsterLine.IndexOf('(');
	// 			int endParanIdx = monsterLine.IndexOf(')');
	// 			int idLen = endParanIdx - startParanIdx - 1; 
	// 			string name = monsterLine.Substring(0, startParanIdx);
	// 			string idText = monsterLine.Substring(startParanIdx + 1, idLen);
	// 			int id = int.Parse(idText);
	// 			currentMonster = Target.AddMonster(id, name);
	// 		}
	// 	}
	// }
}