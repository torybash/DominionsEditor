using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(NationsTable))]
public class NationsTableEditor : Editor
{
	private const string DATA_PATH = "Assets/Data/gamedata/nations.csv";
	private const string ICONS_FOLDER_PATH = "Assets/Art/nations/";

	private NationsTable Target => (NationsTable)target;
	
	private Dictionary<string, Sprite> _spriteMap;

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Load"))
		{
			LoadNations();
			
			EditorUtility.SetDirty(Target);
		}
	}
	
	private void LoadNations ()
	{
		Target.Nations.Clear();
	
		var nationLines = File.ReadAllLines(DATA_PATH);
		var attributeNames = nationLines[0].Split('\t');
	
		for (var i = 1; i < nationLines.Length; i++)
		{
			var line = nationLines[i].Split('\t');
			
			const int ID_IDX = 0;
			const int NAME_IDX = 1;
			const int EPITHET_IDX = 2;
			const int FILE_NAME_BASE_IDX = 4;
			const int ERA_IDX = 5;
			int nationNum = int.Parse(line[ID_IDX]);
			var nationName = line[NAME_IDX];
			var epithet = line[EPITHET_IDX];
			var nationFileName = line[FILE_NAME_BASE_IDX];
			int era = int.Parse(line[ERA_IDX]);

			var sprite = GetSprite(nationFileName);


			
			Color color;
			if (sprite != null)
			{
				color = sprite.texture.GetPixel(sprite.texture.width/2, sprite.texture.height/2);
			} else
			{
				Debug.LogError($"No sprite found for nation {nationName} with filename {nationFileName}");

				color = Color.clear;
			}
			var monsterEntry = new NationEntry(nationName, epithet, nationNum, nationFileName, era, sprite, color );
			
			if (Target.Nations.Any(x => x.NationNum == nationNum)) continue;
			
			Target.Nations.Add(monsterEntry);
		}
	}
	
	private Sprite GetSprite (string nationFileName)
	{
		if (_spriteMap == null) CreateSpriteMap();
		if (!_spriteMap.TryGetValue(nationFileName, out var foundSprite))
		{
			Debug.LogError($"No sprite found for nation with name {nationFileName}");
		}
		return foundSprite;
	}
	
	private void CreateSpriteMap ()
	{
		_spriteMap = new Dictionary<string, Sprite>();
		foreach (var iconPath in Directory.GetFiles(ICONS_FOLDER_PATH))
		{
			if (!iconPath.EndsWith(".png")) continue;
			
			int lastSepIdx = iconPath.LastIndexOf('/');
			int extIdx = iconPath.LastIndexOf('.');
	
			var iconName = iconPath.Substring(lastSepIdx + 1, extIdx - lastSepIdx - 1);

			
			var eraShort = iconName.Split('_')[0];
			var nationName = iconName.Split('_')[1];

			var eraLong = eraShort switch
			{
				"ea" => "early",
				"ma" => "mid",
				"la" => "late",
				_ => throw new ArgumentOutOfRangeException()
			};
			
			var nationFileName = eraLong + "_" + nationName;
			
			var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(iconPath);
			_spriteMap.Add(nationFileName, sprite);
		}
	}
}